using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public Package Package { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
