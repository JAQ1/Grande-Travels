using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrandeTravels.Models;
using GrandeTravels.ViewModels;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel vm)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                user.UserName = vm.Username;
                user.Email = vm.Email;

                try
                {
                    var res = await _userManager.CreateAsync(user, vm.Password);

                    if (res.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Customer");
                        await _signInManager.SignInAsync(user, false);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in res.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, vm.RememberMe, lockoutOnFailure: false);

                if (res.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(vm);
                }
            }

            return View(vm);
            
        }

        
    }
}
