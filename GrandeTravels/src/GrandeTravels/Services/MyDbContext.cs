using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GrandeTravels.Services
{
    public class MyDbContext : DbContext
    {
        public DbSet<Package> TblPackage { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TravelDB;Trusted_Connection=True;");
        }
    }
}
