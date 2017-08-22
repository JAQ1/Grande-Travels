using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Package
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        //public IFormFile PackageImage { get; set; }
        public string TravelProviderName { get; set; }
        public string ActiveStatus { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public IEnumerable<Booking> Bookings { get; set; }
    }
}
