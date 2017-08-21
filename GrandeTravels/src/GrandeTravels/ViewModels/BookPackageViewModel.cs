using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class BookPackageViewModel
    {
        public Package Package { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public double TotalCost { get; set; }
        public int People { get; set; }
        public string Payments { get; set; }

    }
}
