using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrandeTravels.Models;
using GrandeTravels.ViewModels;
using GrandeTravels.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private IRepository<Profile> _profileRepo;
        private readonly ILogger _logger;


        public AccountController(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IRepository<Profile> profileRepo,
            ILoggerFactory loggerFactory
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _profileRepo = profileRepo;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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
                        if (vm.UserRole == "customer") 
                        {
                            await _userManager.AddToRoleAsync(user, "Customer");
                        }
                        else if (vm.UserRole == "travel-provider")
                        {
                            await _userManager.AddToRoleAsync(user, "TravelProvider");
                        }

                        //give user profile on SignUp
                        Profile profile = new Profile
                        {
                            DisplayPhotoPath = "images/user.png",
                            DisplayName = user.UserName,
                            FirstName = "No Firstname",
                            LastName = "No Lastname",
                            Email = user.Email,
                            Phone = "No Phone",
                            UserID = user.Id
                        };

                        _profileRepo.Create(profile);

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
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        #endregion

    }
}
