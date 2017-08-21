using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class MyBookingsViewModel
    {
        public IEnumerable<Booking> MyBookings { get; set; }
    }
}
