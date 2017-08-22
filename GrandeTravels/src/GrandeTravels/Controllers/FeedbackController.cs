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
    [Authorize(Roles = "Customer")]
    public class FeedbackController : Controller
    {
        private UserManager<User> _userManager;
        private IRepository<Package> _packageRepo;
        private IRepository<Feedback> _feedbackRepo;


        public FeedbackController(UserManager<User> userManager, 
                                IRepository<Package> packageRepo, 
                                IRepository<Feedback> feedbackRepo)
        {
            _userManager = userManager;
            _packageRepo = packageRepo;
            _feedbackRepo = feedbackRepo;
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
        public async Task<ActionResult> LeaveFeedback(LeaveFeedbackViewModel vm, int id)
        {
            Package package = _packageRepo.GetSingle(p => p.ID == id);

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);
                Feedback feedback = new Feedback()
                {
                    Comment = vm.Comment,
                    Rating = vm.Rating,
                    PackageID = package.ID,
                    UserId = user.Id,
                    UserName = user.UserName,
                    Date = DateTime.Today
                };

                _feedbackRepo.Create(feedback);

                return RedirectToAction("MyBookings", "Booking");
            }

            vm.Package = package;
            return View(vm);

        }
    }
}
