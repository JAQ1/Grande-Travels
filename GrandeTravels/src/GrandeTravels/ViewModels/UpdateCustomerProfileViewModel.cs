using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace GrandeTravels.ViewModels
{
    public class UpdateCustomerProfileViewModel
    {
        public string Title { get; set; }

        [StringLength(25, MinimumLength = 3, ErrorMessage = "Display name must be between 2 and 50 Characters long")]
        public string DisplayName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 LETTERS long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name must only contain letters")]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 LETTERS long")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name must only contain letters")]
        [Required]
        public string LastName { get; set; }


        //[DataType(DataType.EmailAddress, ErrorMessage = "Invalid email address")]
        [RegularExpression(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
            + "@"
            + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$", ErrorMessage = "Invalid Email")]
        [Required]
        public string Email { get; set; }

        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
            ErrorMessage = "Invalid Australian phone number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Display(Name = "Display Photo")]
        public string DisplayPhotoPath { get; set; }
        public string SavedStatus { get; set; }
    }
}
