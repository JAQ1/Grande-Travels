using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class ShoppingCart
    {
        public int ID { get; set; }
        public double TotalPrice { get; set; }
        public int PackageCount { get; set; }
        public bool CheckedOut { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public IEnumerable<ShoppingCartPackage> ShoppingCartPackages { get; set; }
    }
}
