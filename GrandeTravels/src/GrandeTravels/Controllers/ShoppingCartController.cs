using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GrandeTravels.Models;
using GrandeTravels.ViewModels;
using GrandeTravels.Services;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace GrandeTravels.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Package> _packageRepo;
        private readonly IRepository<ShoppingCart> _shoppingCartRepo;
        private readonly IRepository<ShoppingCartPackage> _shoppingCartPackageRepo;
        private readonly IRepository<Booking> _bookingRepo;

        public ShoppingCartController(
            UserManager<User> userManager, 
            IRepository<Package> packageRepo,
            IRepository<ShoppingCart> shoppingCartRepo,
            IRepository<ShoppingCartPackage> shoppingCartPackageRepo,
            IRepository<Booking> bookingRepo
            )
        {
            _userManager = userManager;
            _packageRepo = packageRepo;
            _shoppingCartRepo = shoppingCartRepo;
            _shoppingCartPackageRepo = shoppingCartPackageRepo;
            _bookingRepo = bookingRepo;
        }
        
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            ShoppingCart shoppingCart = _shoppingCartRepo.GetSingle(cart => cart.UserID == user.Id);
            int cartID = shoppingCart.ID;

            //get all PackageID's that belong to user's ShoppingCart
            IEnumerable<ShoppingCartPackage> cartPackages = 
                _shoppingCartPackageRepo.Query(cartPack => cartPack.ShoppingCart.ID == cartID);

            List<Package> packages = new List<Package>();
            double totalPrice = 0;
            foreach (var item in cartPackages)
            {
                Package package = new Package();
                package = _packageRepo.GetSingle(p => p.ID == item.PackageID);

                totalPrice += package.Price;
                packages.Add(package);
            }

            ShoppingCartViewModel vm = new ShoppingCartViewModel();
            vm.TotalPrice = totalPrice;
            vm.Packages = packages;

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ShoppingCartViewModel vm)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            ShoppingCart shoppingCart = _shoppingCartRepo.GetSingle(cart => cart.UserID == user.Id);
            int cartID = shoppingCart.ID;

            //get all PackageID's that belong to user's ShoppingCart
            IEnumerable<ShoppingCartPackage> cartPackages =
                _shoppingCartPackageRepo.Query(cartPack => cartPack.ShoppingCart.ID == cartID);

            List<Package> packages = new List<Package>();
            foreach (var item in cartPackages)
            {
                Booking booking = new Booking();

                booking.PackageID = item.PackageID;
                booking.UserID = user.Id;

                _bookingRepo.Create(booking);
            }

            return RedirectToAction("MyBookings", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            Package package = _packageRepo.GetSingle(p => p.ID == id);
            ShoppingCart shoppingCart = _shoppingCartRepo.GetSingle(cart => cart.UserID == user.Id);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = user.Id;
                _shoppingCartRepo.Create(shoppingCart);
            }

            ShoppingCartPackage cartPackage = new ShoppingCartPackage();
            cartPackage.PackageID = package.ID;
            cartPackage.ShoppingCartID = shoppingCart.ID;

            _shoppingCartPackageRepo.Create(cartPackage);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCartPackage(int id)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ShoppingCart shoppingCart = _shoppingCartRepo.GetSingle(cart => cart.UserID == user.Id);

            ShoppingCartPackage cartPackage = _shoppingCartPackageRepo.GetSingle(p => p.ShoppingCartID == shoppingCart.ID && p.PackageID == id);

            _shoppingCartPackageRepo.Delete(cartPackage);

            return RedirectToAction("Index");
        }

        public double GetTotalPrice(IEnumerable<Package> packages)
        {
            if (packages != null)
            {
                double totalPrice = 0;

                foreach (var package in packages)
                {
                    totalPrice += package.Price;
                }

                return totalPrice;
            }
            else
            {
                return 0;
            }
        }

        private ShoppingCart GetUserShoppingCart(string id)
        {
            ShoppingCart shoppingCart = _shoppingCartRepo.GetSingle(cart => cart.UserID == id);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart();
                shoppingCart.UserID = id;

                _shoppingCartRepo.Create(shoppingCart);
            }

            return shoppingCart;
        }
    }
}
