using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrandeTravels.ViewModels
{
    public class UpdateCustomerProfileViewModel
    {
        public string Title { get; set; }
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string DisplayPhotoPath { get; set; }
        public string SavedStatus { get; set; }
    }
}
