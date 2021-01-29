using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.DataAccess;
using StudentAPI.Model;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly string connString = "Server=192.168.42.74;Database=apbd;User Id=sa;Password=<YourStrong@Passw0rd>;";

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        public IActionResult Get()
        {
            using(var conn = new SqlConnection(connString))
            using(var command = new SqlCommand("SELECT * FROM Student", conn))
            {
                var students = new List<Student>();
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
                return Ok(students);
            }
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var student = new Student
            {
                Id = "" + id,
                FirstName = "Steve",
                LastName = "Bobs"
            };
            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            return Ok(student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Student student)
        {
            student.Id = "" + id;
            return Ok(student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }

        // GET: api/Students/5/Enrollments
        [HttpGet("{id}/enrollments")]
        public IActionResult GetEnrollments(int id)
        {
            using (var conn = new SqlConnection(connString))
            using (var command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Enrollment e JOIN Student s ON s.IdEnrollment = e.IdEnrollment WHERE s.IndexNumber = @id";
                command.Parameters.AddWithValue("id", id);
                var enrollments = new List<Enrollment>();
                conn.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var enrollment = new Enrollment
                    {
                        Id = int.Parse(reader["IdEnrollment"].ToString()),
                        Semester = int.Parse(reader["Semester"].ToString()),
                        StartDate = DateTime.Parse(reader["StartDate"].ToString())
                    };
                    enrollments.Add(enrollment);
                }
                return Ok(enrollments);
            }
        }
    }
}
