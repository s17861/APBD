using StudentAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DataAccess
{
    public interface IStudentService
    {
        public IEnumerable<Student> GetAll();
    }
}
