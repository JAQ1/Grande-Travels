using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Package> Packages { get; set; }
        public double TotalPrice { get; set; }
    }
}
