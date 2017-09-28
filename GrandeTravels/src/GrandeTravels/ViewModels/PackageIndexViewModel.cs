using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrandeTravels.Models;

namespace GrandeTravels.ViewModels
{
    public class PackageIndexViewModel
    {
        public string SearchQuery { get; set; }
        public int PackageCount { get; set; }
        public IEnumerable<Package> Packages { get; set; }
    }
}
