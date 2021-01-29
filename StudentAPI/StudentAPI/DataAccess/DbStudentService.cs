using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using StudentAPI.DTO;
using StudentAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAPI.DataAccess
{
    public class DbStudentService : IStudentService
    {
        private const string connString = "Server=192.168.42.74;Database=apbd;User Id=sa;Password=<YourStrong@Passw0rd>;";

        public IEnumerable<Student> GetAll()
        {
            var students = new List<Student>();
            using(var conn = new SqlConnection(connString))
            using(var command = new SqlCommand("SELECT * FROM Student", conn))
            {
                conn.Open();
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    var student = new Student
                    {
                        Id = reader["IndexNumber"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        BirthDate = DateTime.Parse(reader["BirthDate"].ToString())
                    };
                    students.Add(student);
                }
            }
            return students;
        }
        
        public Student GetById(string id)
        {
            var students = new List<Student>();
            using(var conn = GetConnection())
            using(var command = new SqlCommand("SELECT * FROM Student where IndexNumber = @id", conn))
            {
                conn.Open();
                command.Parameters.AddWithValue("id", id);
                var reader = command.ExecuteReader();
                while(reader.Read())
                {
                    return new Student
                    {
                        Id = reader["IndexNumber"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        BirthDate = DateTime.Parse(reader["BirthDate"].ToString()),
                        EnrollmentId = int.Parse(reader["IdEnrollment"].ToString())
                    };
                }
            }
            return null;
        }

        public EnrollmentResponse EnrollStudent(EnrollmentRequest request)
        {
            var response = new EnrollmentResponse
            {
                IndexNumber = request.IndexNumber,
                Semester = 1,
                Message = "OK"
            };
            using(var conn = GetConnection())
            using(var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    // 1.
                    command.CommandText = "SELECT IdStudy FROM Studies WHERE Name=@name";
                    command.Parameters.AddWithValue("name", request.Studies);
                    var reader = command.ExecuteReader();
                    if(!reader.Read())
                    {
                        response.Message = "No such studies exist.";
                        return response;
                    }
                    var studyId = int.Parse(reader["IdStudy"].ToString());
                    reader.Close();

                    // 2.
                    command.Parameters.Clear();
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE IdStudy=@id AND Semester=@sem";
                    command.Parameters.AddWithValue("id", studyId);
                    command.Parameters.AddWithValue("sem", 1);
                    int enrollmentId;
                    reader = command.ExecuteReader();
                    if(!reader.Read())
                    {
                        command.Parameters.Clear();
                        command.CommandText = "SELECT MAX(IdEnrollment) FROM Enrollment";
                        enrollmentId = int.Parse(command.ExecuteScalar().ToString()) + 1;
                        command.CommandText = "INSERT INTO Enrollment(IdEnrollment, Semester, IdStudy, StartDate) VALUES (@id, @sem, @study, @start)";
                        command.Parameters.AddWithValue("id", enrollmentId);
                        command.Parameters.AddWithValue("sem", 1);
                        command.Parameters.AddWithValue("study", studyId);
                        command.Parameters.AddWithValue("start", DateTime.Today);
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        enrollmentId = int.Parse(reader["IdEnrollment"].ToString());
                    }
                    reader.Close();

                    // 3.
                    command.Parameters.Clear();
                    command.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@id, @fname, @lname, @bday, @enrollId)";
                    command.Parameters.AddWithValue("id", request.IndexNumber);
                    command.Parameters.AddWithValue("fname", request.FirstName);
                    command.Parameters.AddWithValue("lname", request.LastName);
                    command.Parameters.AddWithValue("bday", request.BirthDate);
                    command.Parameters.AddWithValue("enrollId", enrollmentId);
                    command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    response.Semester = -1;
                    response.Message = e.Message;
                    return response;
                }
                return response;
            }
        }

        public PromotionResponse PromoteStudents(PromotionRequest request)
        {
            var response = new PromotionResponse
            {
                IdEnrollment = -1,
                Message = "OK"
            };
            using(var conn = GetConnection())
            using(var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    // 1.
                    command.CommandText = "SELECT IdEnrollment FROM Enrollment e JOIN Studies s ON e.IdStudy = s.IdStudy WHERE s.Name=@name AND e.Semester=@sem";
                    command.Parameters.AddWithValue("name", request.Studies);
                    command.Parameters.AddWithValue("sem", request.Semester);
                    var id = command.ExecuteScalar();
                    if(id == null)
                    {
                        response.Message = "No such studies.";
                        return response;
                    }
                    
                    // 2.
                    command.Parameters.Clear();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PromoteStudents";
                    command.Parameters.AddWithValue("Studies", request.Studies);
                    command.Parameters.AddWithValue("Semester", request.Semester);

                    var ret = command.Parameters.Add("@ret", SqlDbType.Int);
                    ret.Direction = ParameterDirection.ReturnValue;
                    command.ExecuteNonQuery();
                    var value = int.Parse(ret.Value.ToString());
                    if(value == 0)
                    {
                        response.Message = "No such semester.";
                        return response;
                    }
                    response.IdEnrollment = value;
                }
                catch(SqlException e)
                {
                    response.Message = e.Message;
                    return response;
                }
                return response;
            }
        }

        public bool CheckIndexExists(string id)
        {
            using(var conn = GetConnection())
            using(var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("id", id);
                    var temp = (string)command.ExecuteScalar();
                    return temp != null;
                    //return (string)command.ExecuteScalar() == null;
                }
                catch(SqlException e)
                {
                    // Can't validate, let's be cautious;
                    return false;
                }
            }
        }

        public bool LoginAs(string id, string password)
        {
            using(var conn = GetConnection())
            using(var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "SELECT Password, Salt FROM Student WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("id", id);
                    var reader = command.ExecuteReader();
                    if(!reader.Read())
                    {
                        return false;
                    }
                    var hash = reader["Password"].ToString();
                    var salt = reader["Salt"].ToString();
                    return CreateHash(password, salt) == hash;
                }
                catch(SqlException e)
                {
                    return false;
                }
            }
        }

        public string GetRefreshToken(string id)
        {
            using (var conn = GetConnection())
            using (var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "SELECT RefreshToken FROM Student WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("id", id);
                    return (string)command.ExecuteScalar();
                }
                catch (SqlException e)
                {
                    return null;
                }
            }
        }

        public void SaveRefreshToken(string id, string token)
        {
            using(var conn = GetConnection())
            using(var command = new SqlCommand())
            {
                try
                {
                    conn.Open();
                    command.Connection = conn;
                    command.CommandText = "UPDATE Student SET RefreshToken=@token WHERE IndexNumber=@id";
                    command.Parameters.AddWithValue("token", token);
                    command.Parameters.AddWithValue("id", id);
                    var reader = command.ExecuteNonQuery();
                }
                finally
                {
                    ;
                }
            }
        }

        private string CreateHash(string password, string salt)
        {
            var bytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 10000,
                numBytesRequested: 32
            );
            return Convert.ToBase64String(bytes);
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(connString);
        }
    }
}
