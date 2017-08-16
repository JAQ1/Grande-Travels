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
            vm.SavedStatus = null;

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
                        vm.DisplayName = user.UserName;
                        vm.Title = "Welcome to your new Profile!";
                        
                    }

                    return View(vm);
                }
                else
                {
                    RedirectToAction("SignUp", "Account");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UpdateCustomerProfile(UpdateCustomerProfileViewModel vm)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            CustomerProfile tempProfile = new CustomerProfile();
            tempProfile.DisplayName = vm.DisplayName;
            tempProfile.FirstName = vm.FirstName;
            tempProfile.LastName = vm.LastName;
            tempProfile.Email = vm.Email;
            tempProfile.Phone = vm.Phone;
            tempProfile.UserID = user.Id;

            CustomerProfile custProfile = _customerProfileRepo.GetSingle(p => p.UserID == user.Id);

            try
            {
                if (custProfile != null)
                {
                    //keep the existing profile ID
                    custProfile.DisplayName = tempProfile.DisplayName;
                    custProfile.FirstName = tempProfile.FirstName;
                    custProfile.LastName = tempProfile.LastName;
                    custProfile.Email = tempProfile.Email;
                    custProfile.Phone = tempProfile.Phone;

                    _customerProfileRepo.Update(custProfile);
                }
                else
                {
                    _customerProfileRepo.Create(tempProfile);
                }

                vm.SavedStatus = "Your detials were successfully saved!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                vm.SavedStatus = "Something went wrong with saving your details :/";
                throw;
            }
            

            return View(vm);
        }
    }
}
