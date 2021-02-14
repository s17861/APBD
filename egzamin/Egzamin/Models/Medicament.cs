using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Egzamin.Models
{
    public class Medicament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [ForeignKey("IdMedicament")]
        public ICollection<PrescriptionMedicament> Prescriptions { get; set; }
    }
}
