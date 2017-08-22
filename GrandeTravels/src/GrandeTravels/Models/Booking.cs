using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Booking
    {
        public int BookingID { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public int People { get; set; }
        public double TotalCost { get; set; }
        public DateTime Date { get; set; }

        public int PackageID { get; set; }
        public Package Package { get; set; }
        public string PackageName { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
