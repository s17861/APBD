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
                new Student{Id="s0", FirstName="Steve", LastName="Bobs"},
                new Student{Id="s1", FirstName="John", LastName="Doe"},
                new Student{Id="s2", FirstName="Stephen", LastName="Hunt"},
                new Student{Id="s3", FirstName="Bob", LastName="Bobson"},
                new Student{Id="s4", FirstName="Richard", LastName="Dicks"}
            };
        }
        public IEnumerable<Student> GetAll()
        {
            return _students;
        }
    }
}
