using StudentAPI.DTO;
using StudentAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DataAccess
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();
        Student GetById(string id);
        EnrollmentResponse EnrollStudent(EnrollmentRequest request);
        PromotionResponse PromoteStudents(PromotionRequest request);
        bool CheckIndexExists(string id);
    }
}
