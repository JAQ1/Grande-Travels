﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrandeTravels.Services;
using GrandeTravels.Models;
using GrandeTravels.ViewModels;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class BookingController : Controller
    {
        private UserManager<User> _userManager;
        private IRepository<Booking> _bookingRepo;
        private IRepository<Package> _packageRepo;

        public BookingController(UserManager<User> userManager,
                                IRepository<Booking> bookingRepo,
                                IRepository<Package> packageRepo)
        {
            _userManager = userManager;
            _bookingRepo = bookingRepo;
            _packageRepo = packageRepo;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BookPackage(int id)
        {
            BookPackageViewModel vm = new BookPackageViewModel();
            vm.Package = _packageRepo.GetSingle(p => p.ID == id);

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> BookPackage(BookPackageViewModel vm, int id)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity.Name);

                Booking newBooking = new Booking();

                newBooking.ArrivalDate = vm.ArrivalDate;
                newBooking.DepartureDate = vm.DepartureDate;
                newBooking.People = vm.People;
                newBooking.TotalCost = vm.TotalCost;
                newBooking.PackageID = id;
                newBooking.UserID = user.Id;

                _bookingRepo.Create(newBooking);
                return RedirectToAction("Index", "Home");
            }
            return View(vm);
        }
    }
}