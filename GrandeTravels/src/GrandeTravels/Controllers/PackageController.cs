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
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    [Authorize(Roles = "TravelProvider")]
    public class PackageController : Controller
    {
        private IRepository<Package> _packageRepo;
        private IRepository<Feedback> _feedbackRepo;
        private UserManager<User> _userManager;
        private IHostingEnvironment _hostingEnviro;

        public PackageController(IRepository<Package> packageRepo,
                                IRepository<Feedback> feedbackRepo,
                                UserManager<User> userManager,
                                IHostingEnvironment hostingEnviro)
        {
            _packageRepo = packageRepo;
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
            _hostingEnviro = hostingEnviro;
        }

        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Index()
        {
            IEnumerable<Package> allPackages = _packageRepo.GetAll();

            PackageIndexViewModel vm = new PackageIndexViewModel();
            if (allPackages != null)
            {
                List<Package> activePackages = new List<Package>();

                foreach (var item in allPackages)
                {
                    if (item.ActiveStatus != "Inactive")
                    {
                        activePackages.Add(item);
                    }
                }

                vm.Packages = activePackages;
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult AddPackage()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPackage(AddPackageViewModel vm, IFormFile PhotoLocation)
        {
            if (ModelState.IsValid)
            {
                User loggedUser = await _userManager.FindByNameAsync(User.Identity.Name);
                Package newPackage = new Package()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Location = vm.Location,
                    Price = vm.Price,
                    UserId = loggedUser.Id,
                    TravelProviderName = User.Identity.Name
                };

                if (PhotoLocation != null)
                {
                    string uploadPath = Path.Combine(_hostingEnviro.WebRootPath, "Media\\TravelPackage");
                    //uploadPath = Path.Combine(uploadPath, User.Identity.Name);
                    //Directory.CreateDirectory(Path.Combine(uploadPath, tp.PackageName));
                    string filename = User.Identity.Name + "-" + newPackage.Name + "-1" + Path.GetExtension(PhotoLocation.FileName);
                    uploadPath = Path.Combine(uploadPath, filename);


                    using (FileStream fs = new FileStream(uploadPath, FileMode.Create))
                    {
                        PhotoLocation.CopyTo(fs);
                    }
                    string SaveFilename = Path.Combine("Media\\TravelPackage", filename);
                    newPackage.PhotoLocation = SaveFilename;
                }

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

                vm.PackageID = selectedPackage.ID;
                vm.Name = selectedPackage.Name;
                vm.Description = selectedPackage.Description;
                vm.Location = selectedPackage.Location;
                vm.Price = selectedPackage.Price;

                return View(vm);
            }

            return RedirectToAction("Index", "Package");
        }

        [HttpPost]
        public IActionResult UpdatePackage(UpdatePackageViewModel vm, int id)
        {
            if (ModelState.IsValid)
            {
                Package pack = _packageRepo.GetSingle(p => p.ID == id);

                pack.Name = vm.Name;
                pack.Description = vm.Description;
                pack.Location = vm.Location;
                pack.Price = vm.Price;
                
                _packageRepo.Update(pack);

                return RedirectToAction("Index", "Package");
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult DiscontinuePackage(int id)
        {
            Package pack = _packageRepo.GetSingle(p => p.ID == id);

            try
            {
                pack.ActiveStatus = "Inactive";
                _packageRepo.Update(pack);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View();
                throw;
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PackageDetails(int id)
        {

            Package pack = _packageRepo.GetSingle(p => p.ID == id);
            IEnumerable<Feedback> feedback = _feedbackRepo.Query(f => f.PackageID == id);
            IEnumerable<Package> otherPackages = _packageRepo.Query(p => p.ID != id && p.ActiveStatus != "Inactive");

            if (pack != null)
            {
                PackageDetailsViewModel vm = new PackageDetailsViewModel();
                vm.Package = pack;
                vm.PackageFeedback = feedback;
                vm.OtherPackages = otherPackages;

                return View(vm);
            }

            return RedirectToAction("Index");
        }
    }
}
