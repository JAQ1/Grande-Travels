using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Customer : User
    {
        public int CustomerID { get; set; }
        public int CustomerProfileID { get; set; }
    }
}
