using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using GrandeTravels.Services;
using GrandeTravels.ViewModels;
using GrandeTravels.Models;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    [Authorize]
    public class FeedbackController : Controller
    {
        private UserManager<User> _userManager;
        private IRepository<Package> _packageRepo;
        private IRepository<Feedback> _feedbackRepo;
        private IRepository<Profile> _profileRepo;



        public FeedbackController(UserManager<User> userManager, 
                                IRepository<Package> packageRepo, 
                                IRepository<Feedback> feedbackRepo,
                                IRepository<Profile> profileRepo)
        {
            _userManager = userManager;
            _packageRepo = packageRepo;
            _feedbackRepo = feedbackRepo;
            _profileRepo = profileRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult LeaveFeedback(int id)
        {
            Package package = _packageRepo.GetSingle(p => p.ID == id);

            LeaveFeedbackViewModel vm = new LeaveFeedbackViewModel();
            vm.Package = package;

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> LeaveFeedback(PackageDetailsViewModel vm, int id)
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
                    feedback.Profile = profile;
                    feedback.Date = DateTime.Today;


                    _feedbackRepo.Create(feedback);

                }

            }
            else
            {
                return RedirectToAction("Login", "Account");

            }

            return RedirectToAction("PackageDetails", "Package");

        }
    }
}
