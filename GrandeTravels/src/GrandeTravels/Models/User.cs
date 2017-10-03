using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GrandeTravels.Models
{
    public class User : IdentityUser
    {
        public IEnumerable<Package> Packages { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
    }
}
