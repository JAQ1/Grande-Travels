using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace GrandeTravels.ViewModels
{
    public class UpdateProfileViewModel
    {
        [Required]
        [MaxLength(25, ErrorMessage = "Your DisplayName can only have 25 characters")]
        [MinLength(3, ErrorMessage = "Your DisplayName must have at least 3 characters")]
        public string DisplayName { get; set; }

        [MaxLength(25, ErrorMessage = "Your Firstname can only have 25 characters")]
        public string FirstName { get; set; }

        [MaxLength(25, ErrorMessage = "Your Lastname can only have 25 characters")]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }


        public string Phone { get; set; }
        public string DisplayPhotoPath { get; set; }
    }
}
