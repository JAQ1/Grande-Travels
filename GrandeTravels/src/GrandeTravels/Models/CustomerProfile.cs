using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace GrandeTravels.Models
{
    public class CustomerProfile
    {
        public int CustomerProfileID { get; set; }
        public string UserID { get; set; }
    
        [StringLength(25, MinimumLength = 3)]
        [Required]
        public string DisplayName { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [RegularExpression(@"/^\({0, 1}((0|\+61)(2|4|3|7|8)){0,1}\){0,1}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{2}(\ |-){0,1}[0-9]{1}(\ |-){0,1}[0-9]{3}$/")]
        public int Phone { get; set; }
        public string DisplayPhotoPath { get; set; }
    }
}
