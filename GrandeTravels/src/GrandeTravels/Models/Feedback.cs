using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Feedback
    {
        public int ID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PackageID { get; set; }
        public Package Package { get; set; }

    }
}
