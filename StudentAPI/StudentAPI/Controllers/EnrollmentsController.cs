using Microsoft.AspNetCore.Mvc;
using StudentAPI.DataAccess;
using StudentAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {
        private IStudentService _service;

        public EnrollmentsController(IStudentService service)
        {
            _service = service;
        }

        // POST api/enrollment
        [HttpPost]
        public IActionResult EnrollStudent(EnrollmentRequest request)
        {
            var res = _service.EnrollStudent(request);
            if(res.Semester == -1)
            {
                return BadRequest(res.Message);
            }
            return CreatedAtAction(nameof(EnrollStudent), res);
        }

        // POST api/enrollment
        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromotionRequest request)
        {
            var res = _service.PromoteStudents(request);
            if(res.IdEnrollment == -1)
            {
                return NotFound(res.Message);
            }
            return CreatedAtAction(nameof(PromoteStudent), res);
        }
    }
}
