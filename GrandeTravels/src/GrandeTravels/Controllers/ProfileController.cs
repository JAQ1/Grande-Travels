using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using GrandeTravels.Models;
using GrandeTravels.Services;
using GrandeTravels.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IRepository<CustomerProfile> _customerProfileRepo;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<CustomerProfile> customerProfileRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _customerProfileRepo = customerProfileRepo;
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomerProfile()
        {
            UpdateCustomerProfileViewModel vm = new UpdateCustomerProfileViewModel();
            try
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                
                if (user != null)
                {
                    CustomerProfile custProfile = _customerProfileRepo.GetSingle(p => p.UserID == user.Id);

                    if (custProfile != null)
                    {
                        vm.DisplayName = custProfile.DisplayName;
                        vm.FirstName = custProfile.FirstName;
                        vm.LastName = custProfile.LastName;
                        vm.Email = custProfile.Email;
                        vm.Phone = custProfile.Phone;
                        vm.Title = "Update Profile";
                    }
                    else
                    {
                        vm.Title = "Welcome to your new Profile!";
                        
                    }

                    return View(vm);
                }
                else
                {
                    RedirectToAction("SignUp", "Account");
                }
            }
            catch (Exception)
            {

                throw;
            }
            
            return View();
        }
    }
}
