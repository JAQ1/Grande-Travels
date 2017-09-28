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

    public class PackageController : Controller
    {
        private IRepository<Package> _packageRepo;
        private IRepository<Feedback> _feedbackRepo;
        private IRepository<Profile> _profileRepo;
        private UserManager<User> _userManager;
        private IHostingEnvironment _hostingEnviro;

        public PackageController(
            IRepository<Package> packageRepo,
                                IRepository<Feedback> feedbackRepo,
                                UserManager<User> userManager,
                                IRepository<Profile> profileRepo,
                                IHostingEnvironment hostingEnviro)
        {
            _packageRepo = packageRepo;
            _feedbackRepo = feedbackRepo;
            _userManager = userManager;
            _profileRepo = profileRepo;
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
                vm.MaxPrice = activePackages.OrderByDescending(p => p.Price).ElementAt(0).Price;
                vm.MinPrice = activePackages.OrderBy(p => p.Price).ElementAt(0).Price;
            }
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(PackageIndexViewModel vm)
        {
            string query = vm.SearchQuery;
            double maxPrice = vm.MaxPrice;
            double minPrice = vm.MinPrice;

            if (maxPrice == 0)
            {
                maxPrice = vm.Packages.OrderByDescending(p => p.Price).ElementAt(0).Price;
            }

            if (query != null)
            {
                IEnumerable<Package> searchResult = _packageRepo.Query(i => i.Name.Contains(query) || i.Location.Contains(query));
                vm.Packages = searchResult;
            }
            else
            {
                vm.Packages = _packageRepo.GetAll();
            }

            List<Package> activePackages = new List<Package>();

            foreach (var item in vm.Packages)
            {
                if (item.ActiveStatus != "Inactive")
                {
                    if (item.Price > minPrice && item.Price < maxPrice)
                    {
                        activePackages.Add(item);
                    }
                }
            }

            vm.Packages = activePackages;

            switch (vm.SortBy)
            {
                case "Name(A - Z)":
                    vm.Packages = vm.Packages.OrderBy(p => p.Name);
                    break;
                case "Location(A - Z)":
                    vm.Packages = vm.Packages.OrderBy(p => p.Location);
                    break;
                case "Price(High - Low)":
                    vm.Packages = vm.Packages.OrderByDescending(p => p.Price);
                    break;
                case "Price(Low - High)":
                    vm.Packages = vm.Packages.OrderBy(p => p.Price);
                    break;
                default:
                    break;
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
        public async Task<IActionResult> PackageDetails(int id)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            Package pack = _packageRepo.GetSingle(p => p.ID == id);
            IEnumerable<Feedback> feedbacks = _feedbackRepo.Query(f => f.PackageID == id);
            IEnumerable<Package> otherPackages = _packageRepo.Query(p => p.ID != id && p.ActiveStatus != "Inactive");

            if (pack != null)
            {
                PackageDetailsViewModel vm = new PackageDetailsViewModel();
                vm.Package = pack;
                vm.PackageFeedback = feedbacks;
                vm.CommentCount = feedbacks.Count();
                vm.OtherPackages = otherPackages;

                for (int i = 0; i < feedbacks.Count(); i++)
                {


                    Feedback feedback = feedbacks.ElementAt(i);
                    feedback.Profile = new Profile();

                    Profile profile = _profileRepo.GetSingle(p => p.ID == feedback.ProfileID);

                    feedback.Profile.DisplayPhotoPath = profile.DisplayPhotoPath;
                    feedback.Profile.DisplayName = profile.DisplayName;
                }

                return View(vm);
            }

            return RedirectToAction("Index");
        }

        //POST commnent
        [HttpPost]
        public async Task<ActionResult> PackageDetails(PackageDetailsViewModel vm, int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    User user = await _userManager.FindByNameAsync(User.Identity.Name);
                    Profile profile = _profileRepo.GetSingle(p => p.UserID == user.Id);
                    Package package = _packageRepo.GetSingle(p => p.ID == id);

                    Feedback feedback = new Feedback();

                    feedback.Comment = vm.Comment;
                    feedback.PackageID = package.ID;
                    feedback.ProfileID = profile.ID;
                    feedback.Date = DateTime.Today;


                    _feedbackRepo.Create(feedback);

                    vm.Package = package;
                }

            }
            else
            {
                return RedirectToAction("Login", "Account");

            }

            return RedirectToAction("PackageDetails");
        }
    }
}
