using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using GrandeTravels.ViewModels;
using GrandeTravels.Models;
using GrandeTravels.Services;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    [Authorize(Roles ="TravelProvider")]
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
        [AllowAnonymous]
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

        [HttpGet]
        public IActionResult UpdatePackage(int id)
        {
            Package selectedPackage = _packageRepo.GetSingle(p => p.ID == id);

            if (selectedPackage != null)
            {
                UpdatePackageViewModel vm = new UpdatePackageViewModel();

                vm.Package = selectedPackage;
                vm.Name = selectedPackage.Name;
                vm.Description = selectedPackage.Description;
                vm.Location = selectedPackage.Location;
                vm.Price = selectedPackage.Price;
                vm.Package = selectedPackage;

                return View(vm);
            }

            return RedirectToAction("Index", "Package");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePackage(UpdatePackageViewModel vm, int id)
        {
            User loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
            
            Package pack = _packageRepo.GetSingle(p => p.ID == id);

            if (ModelState.IsValid)
            {

                pack.Name = vm.Name;
                pack.Description = vm.Description;
                pack.Location = vm.Location;
                pack.Price = vm.Price;

                _packageRepo.Update(pack);

                return RedirectToAction("Index", "Package");

            }

            return View(vm);
        }
    }
}
