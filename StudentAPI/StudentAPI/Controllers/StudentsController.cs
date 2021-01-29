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

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        // GET: api/Students
        [HttpGet]
        public IActionResult Get()
        {
            var students = _studentService.GetAll();
            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(string id)
        {
            var student = _studentService.GetById(id);
            if(student == null)
            {
                return NotFound();
            }
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
        public IActionResult Put(string id, [FromBody] Student student)
        {
            return Ok(student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return Ok();
        }

        // GET: api/Students/5/Enrollments
        [HttpGet("{id}/enrollments")]
        public IActionResult GetEnrollments(string id)
        {
            return Ok();
        }
    }
}
