using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class MyBookingsViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; }
        public string DisplayPhotoPath { get; set; }
    }
}
