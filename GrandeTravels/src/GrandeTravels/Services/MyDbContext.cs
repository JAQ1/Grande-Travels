using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using GrandeTravels.Models;

namespace GrandeTravels.Services
{
    public class MyDbContext : IdentityDbContext<User>
    {
        public DbSet<Package> TblPackage { get; set; }
        public DbSet<Profile> TblProfile { get; set; }
        public DbSet<Booking> TblBooking { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TravelDB;Trusted_Connection=True;");
        }
    }
}
