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
        public string Details { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
    }
}
