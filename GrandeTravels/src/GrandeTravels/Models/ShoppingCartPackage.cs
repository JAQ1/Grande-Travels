using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class ShoppingCartPackage
    {
        public int ID { get; set; }
        public int ShoppingCartID { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int PackageID { get; set; }
        public Package Package { get; set; }
    }
}
