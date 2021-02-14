using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Egzamin.Models
{
    public class MyDbContext : DbContext
    {
        private const string connectionString = "Data Source=192.168.42.74,1433;Initial Catalog=apbd_egzamin;User ID=sa;Password=H@rdPassw0rd";

        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }

        public MyDbContext() : base(GetOptions()) { }

        private static DbContextOptions GetOptions()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Prescription>().HasMany<PrescriptionMedicament>(p => p.Medicaments).WithOne("MedicamentId").IsRequired();
            //modelBuilder.Entity<Medicament>().HasMany<PrescriptionMedicament>(m => m.Prescriptions).WithOne("PrescriptionId").IsRequired();
            modelBuilder.Entity<PrescriptionMedicament>().HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });
        }
    }
}
