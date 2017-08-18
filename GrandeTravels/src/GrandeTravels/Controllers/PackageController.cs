using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using GrandeTravels.ViewModels;
using GrandeTravels.Models;
using GrandeTravels.Services;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class PackageController : Controller
    {
        private IRepository<Package> _packageRepo;
        private UserManager<User> _userManager;

        public PackageController(IRepository<Package> packageRepo, UserManager<User> userManager)
        {
            _packageRepo = packageRepo;
            _userManager = userManager;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<Package> allPackages = _packageRepo.GetAll();

            PackageIndexViewModel vm = new PackageIndexViewModel();
            if (allPackages != null)
            {
                vm.Packages = allPackages;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult AddPackage()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPackage(AddPackageViewModel vm)
        {
            User loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (ModelState.IsValid)
            {
                Package newPackage = new Package()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Location = vm.Location,
                    Price = vm.Price,
                    UserId = loggedUser.Id

                };

                _packageRepo.Create(newPackage);

                return RedirectToAction("Index", "Package");

            }

            return View(vm);
        }
    }
}
