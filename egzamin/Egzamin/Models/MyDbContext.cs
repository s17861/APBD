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

        //public DbSet<TestItem> TestItems { get; set; }

        public MyDbContext() : base(GetOptions()) { }

        private static DbContextOptions GetOptions()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }
    }
}
