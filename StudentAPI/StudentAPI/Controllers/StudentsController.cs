using System;
using System.Collections.Generic;
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
            return Ok(_studentService.GetAll());
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "Get")]
        public IActionResult Get(int id)
        {
            var student = new Student
            {
                Id = id,
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
            student.Id = id;
            return Ok(student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
