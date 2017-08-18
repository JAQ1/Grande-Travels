using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.Models
{
    public class Package
    {
        public int ID { get; set; }

        [Required]
        [MaxLength(50), MinLength(5)]
        public string Name { get; set; }

        [Required]
        [MaxLength(225, ErrorMessage = "Description must be less than 225 characters")]
        [MinLength(25, ErrorMessage = "Needs more detail! Must be over 25 characters")]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        
        //public IFormFile PackageImage { get; set; }
        public string UserId { get; set; }
        public string TravelProviderName { get; set; }
    }
}
