using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.Model
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int Semester { get; set; }
        public int StudyId { get; set; }
        public DateTime StartDate { get; set; }
    }
}
