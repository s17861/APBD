using Egzamin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Egzamin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicationsController : ControllerBase
    {
        private MyDbContext db;
        public MedicationsController(MyDbContext db)
        {
            this.db = db;
        }

        // GET api/<ValuesController1>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var med = db.Medicaments.Find(id);
            //var med = db.Medicaments.Include(m => m.Prescriptions).ThenInclude(pm => pm.Prescription).Where(m => m.Id == id).FirstOrDefault();
            //var med = db.Medicaments.Where(m => m.Id == id).Include(m => m.Prescriptions).FirstOrDefault();
            return Ok(med);
        }
    }
}
