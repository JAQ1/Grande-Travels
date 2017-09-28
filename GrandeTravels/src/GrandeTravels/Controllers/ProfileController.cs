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
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IRepository<Profile> _profileRepo;
        private IHostingEnvironment _hostingEnviro;


        public ProfileController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager, 
            IRepository<Profile> profileRepo, 
            IHostingEnvironment hostingEnviro)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
            _hostingEnviro = hostingEnviro;
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

                    //shouldnt be null
                    if (profile == null)
                    {
                        profile = new Profile()
                        {
                            DisplayPhotoPath = "images/user.png",
                            DisplayName = "No Display Name",
                            FirstName = "No Firstname",
                            LastName = "No Lastname",
                            Email = user.Email,
                            Phone = "No Phone",
                            UserID = user.Id
                        };

                        _profileRepo.Create(profile);
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

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Profile profile = _profileRepo.GetSingle(p => p.UserID == user.Id);

            UpdateProfileViewModel vm = new UpdateProfileViewModel();

            vm.DisplayName = profile.DisplayName;
            vm.FirstName = profile.FirstName;
            vm.LastName = profile.LastName;
            vm.Email = profile.Email;
            vm.Phone = profile.Phone;
            vm.DisplayPhotoPath = profile.DisplayPhotoPath;
            

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel vm, IFormFile DisplayPhotoPath)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                Profile profile = _profileRepo.GetSingle(p => p.UserID == user.Id);

                profile.DisplayName = vm.DisplayName;
                profile.FirstName = vm.FirstName;
                profile.LastName = vm.LastName;
                profile.Email = vm.Email;
                profile.Phone = vm.Phone;

                if (DisplayPhotoPath != null)
                {
                    string uploadPath = Path.Combine(_hostingEnviro.WebRootPath, "Media\\Profile");
                    string filename = User.Identity.Name + "-" + profile.ID + Path.GetExtension(DisplayPhotoPath.FileName);

                    uploadPath = Path.Combine(uploadPath, filename);


                    using (FileStream fs = new FileStream(uploadPath, FileMode.Create))
                    {
                        DisplayPhotoPath.CopyTo(fs);
                    }

                    string SaveFilename = Path.Combine("Media\\Profile", filename);
                    profile.DisplayPhotoPath = SaveFilename;
                }
                else
                {
                    profile.DisplayPhotoPath = "images/user.png";
                }

                _profileRepo.Update(profile);

                return RedirectToAction("Index");
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
