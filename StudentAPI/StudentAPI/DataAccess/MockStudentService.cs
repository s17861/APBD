using StudentAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DataAccess
{
    public class MockStudentService : IStudentService
    {
        private static IEnumerable<Student> _students;

        static MockStudentService()
        {
            _students = new List<Student>
            {
                new Student{Id=0, FirstName="Steve", LastName="Bobs", IndexNumber="s1"},
                new Student{Id=1, FirstName="John", LastName="Doe", IndexNumber="s3"},
                new Student{Id=2, FirstName="Stephen", LastName="Hunt", IndexNumber="s151"},
                new Student{Id=3, FirstName="Bob", LastName="Bobson", IndexNumber="s122"},
                new Student{Id=4, FirstName="Richard", LastName="Dicks", IndexNumber="s132"}
            };
        }
        public IEnumerable<Student> GetAll()
        {
            return _students;
        }
    }
}
