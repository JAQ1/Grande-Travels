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
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
        public int PackageID { get; set; }
        public Package Package { get; set; }
        public DateTime Date{ get; set; }

    }
}
