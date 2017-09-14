using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

using System.Threading.Tasks;

namespace GrandeTravels.ViewModels
{
    public class AddPackageViewModel
    {
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
        public string PhotoLocation { get; set; }
    }
}
