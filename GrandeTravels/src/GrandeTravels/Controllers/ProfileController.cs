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
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IRepository<Profile> _profileRepo;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, IRepository<Profile> profileRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ProfileIndexViewModel vm = new ProfileIndexViewModel();

            try
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (user != null)
                {
                    Profile profile = _profileRepo.GetSingle(p => p.UserID == user.Id);

                    if (profile == null)
                    {
                        if (User.IsInRole("Customer"))
                        {
                            CustomerProfile custProf = new CustomerProfile()
                            {
                                DisplayPhotoPath = "images/user.png",
                                DisplayName = "No Display Name",
                                FirstName = "No Firstname",
                                LastName = "No Lastname",
                                Email = user.Email,
                                Phone = "No Phone"
                            };

                            profile = custProf;
                        }
                    }

                    vm.DisplayPhotoPath = profile.DisplayPhotoPath;
                    vm.DisplayName = profile.DisplayName;
                    vm.FirstName = profile.FirstName;
                    vm.LastName = profile.LastName;
                    vm.Email = profile.Email;
                    vm.Phone = profile.Phone;
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

            return View(vm);

        }

        //[HttpGet]
        //public async Task<IActionResult> UpdateCustomerProfile()
        //{
        //    UpdateCustomerProfileViewModel vm = new UpdateCustomerProfileViewModel();
        //    vm.SavedStatus = null;

        //}

        //[HttpPost]
        //public async Task<ActionResult> UpdateCustomerProfile(UpdateCustomerProfileViewModel vm, IFormFile photoLocation)
        //{
        //    vm.SavedStatus = null;

        //    if (ModelState.IsValid)
        //    {
        //        User user = await _userManager.FindByNameAsync(User.Identity.Name);

        //        CustomerProfile tempProfile = new CustomerProfile();
        //        tempProfile.DisplayName = vm.DisplayName;
        //        tempProfile.FirstName = vm.FirstName;
        //        tempProfile.LastName = vm.LastName;
        //        tempProfile.Email = vm.Email;
        //        tempProfile.Phone = vm.Phone;
        //        tempProfile.UserID = user.Id;

        //        CustomerProfile custProfile = _customerProfileRepo.GetSingle(p => p.UserID == user.Id);

        //        try
        //        {
        //            if (custProfile != null)
        //            {
        //                //keep the existing profile ID
        //                custProfile.DisplayName = tempProfile.DisplayName;
        //                custProfile.FirstName = tempProfile.FirstName;
        //                custProfile.LastName = tempProfile.LastName;
        //                custProfile.Email = tempProfile.Email;
        //                custProfile.Phone = tempProfile.Phone;

        //                _customerProfileRepo.Update(custProfile);
        //            }
        //            else
        //            {
        //                _customerProfileRepo.Create(tempProfile);
        //            }

        //            if (vm.DisplayPhotoPath != null)
        //            {
        //                // full path to file in temp location
        //                var filePath = Path.GetTempFileName();
        //            }

        //            vm.SavedStatus = "Your detials were successfully saved!";
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            vm.SavedStatus = "Something went wrong with saving your details :/";
        //            throw;
        //        }


        //    }

        //    return View(vm);
        //}
    }
}
