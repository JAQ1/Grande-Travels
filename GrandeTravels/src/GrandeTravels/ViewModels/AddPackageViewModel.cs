using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.ViewModels
{
    public class AddPackageViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public IFormFile PackageImage { get; set; }
    }
}
