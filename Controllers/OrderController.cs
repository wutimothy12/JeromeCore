using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JeromeCore.Models;
using JeromeCore.Models.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using JeromeCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System;
using JeromeCore.Services;

namespace JeromeCore.Controllers
{
    public class OrderController : Controller
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IOrderRepository repository;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private Cart cart;
        private readonly ApplicationDbContext _context;
        public int PageSize = 10;

        public OrderController(IOrderRepository repoService, Cart cartService, UserManager<ApplicationUser> userManager, IEmailSender emailSender, ApplicationDbContext context, IRazorViewToStringRenderer razorViewToStringRenderer)
        {

            repository = repoService;
            _emailSender = emailSender;
            cart = cartService;
            _userManager = userManager;
            _context = context;
            _razorViewToStringRenderer = razorViewToStringRenderer;

        }
        [Authorize(Roles = "Admins")]
        public ViewResult List() =>

           View(repository.Orders.Where(o => !o.Shipped));

        [Authorize(Roles = "Admins")]
        public ViewResult ListAll(int page = 1) => View(new ListAllViewModel
        {
            Orders = repository.Orders
                    .OrderBy(p => p.OrderID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
            PagingInfo = new PagingInfo(@repository.Orders.Count(), page, PageSize, 10)
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = repository.Orders.Count()

            }
        });

        [Authorize(Roles = "Admins")]
        [HttpPost]
        public IActionResult MarkShipped(int orderID)
        {

            Order order = repository.Orders

                .FirstOrDefault(o => o.OrderID == orderID);

            if (order != null)
            {

                order.Shipped = true;

                repository.SaveOrder(order);

            }

            return RedirectToAction(nameof(List));

        }

        [Authorize(Roles = "Admins")]
        [HttpPost]
        public IActionResult OrderDetail(int orderID)
        {

            Order order = repository.Orders

                .FirstOrDefault(o => o.OrderID == orderID);
            if(order.applicationuser != null)
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDataProtection();
                var services = serviceCollection.BuildServiceProvider();

                // create an instance of MyClass using the service provider
                var instance = ActivatorUtilities.CreateInstance<Encryptor>(services);
                var card = instance.Decrypt(order.applicationuser.CreditCard);
                order.applicationuser.CreditCard = card;
            }

           

            return View(order);

        }

        [Authorize]
        [HttpPost]
        public IActionResult UserOrderDetail(int orderID)
        {

            Order order = repository.Orders

                .FirstOrDefault(o => o.OrderID == orderID);
            if (order.applicationuser != null)
            {
                var serviceCollection = new ServiceCollection();
                serviceCollection.AddDataProtection();
                var services = serviceCollection.BuildServiceProvider();

                // create an instance of MyClass using the service provider
                var instance = ActivatorUtilities.CreateInstance<Encryptor>(services);
                var card = instance.Decrypt(order.applicationuser.CreditCard);
                order.applicationuser.CreditCard = card;
            }

       
            return View(order);

        }


        public async Task<IActionResult> Completed()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            cart.Clear();

            return View();

        }

        public IActionResult GuestCompleted()
        {
            if (cart.Lines.Count() != 0)
            {
                cart.Clear();
            }

            return View();

        }

        public async Task<IActionResult> CheckOutChoice()
        {
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                return RedirectToAction(nameof(Checkout));
            }
            else
            {
                ViewData["ReturnUrl"] = "/Order/Checkout";
                return View(new LoginViewModel());
            }
        }

        public IActionResult GuestCheckOut()
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            return View(new ShippingDetails());

        }

        [HttpPost]

        public async Task<IActionResult> GuestCheckOut(ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.Lines = cart.Lines.ToArray();
                order.AddedDate = DateTime.Now;
                order.ShippingCity = shippingDetails.City;
                order.ShippingCountry = shippingDetails.Country;
                order.ShippingState = shippingDetails.State;
                order.ShippingZip = shippingDetails.Zip;
                order.Name = shippingDetails.Name;
                order.Email = shippingDetails.Email;
                order.Phone = shippingDetails.Phone;
                order.ShippingLine1 = shippingDetails.Line1;
                order.ShippingLine2 = shippingDetails.Line2;
                order.Shipped = false;
                order.Total = cart.ComputeTotalValue();

                repository.SaveOrder(order);
                ViewBag.orderid = order.OrderID;
                TempData["orderid"] = order.OrderID;

                GuestMailSender eop = new GuestMailSender();
                eop.ProcessOrder(cart, shippingDetails);

                string result = await _razorViewToStringRenderer.RenderViewToStringAsync("Email/Invoice", order);
                string email = order.Email;
                string subject = "Invoice";

                await _emailSender.SendEmail(email, subject, result);


                GuestInfo ginfo = new GuestInfo();
                ginfo.Name = shippingDetails.Name;
                ginfo.Email = shippingDetails.Email;
                ginfo.Phone = shippingDetails.Phone;
                ginfo.NameOnCard = shippingDetails.NameOnCard;
                ginfo.CreditCard = shippingDetails.CreditCard;
                ginfo.CardConfirm = shippingDetails.CardConfirm;
                ginfo.Month = shippingDetails.Month;
                ginfo.Year = shippingDetails.Year;
                ginfo.Line1 = shippingDetails.Line1;
                ginfo.Line2 = shippingDetails.Line2;
                ginfo.City = shippingDetails.City;
                ginfo.State = shippingDetails.State;
                ginfo.Zip = shippingDetails.Zip;
                ginfo.Country = shippingDetails.Country;
                ginfo.OrderId = ViewBag.orderid;

                HttpContext.Session.SetJson("Guest", ginfo);
                cart.Clear();

                return View("GuestCompleted");
            }
            else
            {
                return View(shippingDetails);
            }
        }


        public async Task<IActionResult> Checkout()
        {
            ApplicationUser user  = await GetCurrentUserAsync();

            if (user != null)
            {
                ShippingDetails currentUser = new ShippingDetails();
                currentUser.Name = user.UserName;
                currentUser.Email = user.Email;
                currentUser.Phone = user.PhoneNumber;
                currentUser.NameOnCard = user.NameOnCard;
                currentUser.CreditCard = user.CreditCard;
                currentUser.CardConfirm = user.CardConfirm;
                currentUser.Month = user.Month;
                currentUser.Year = user.Year;
                currentUser.Line1 = user.Line1;
                currentUser.Line2 = user.Line2;
                currentUser.City = user.City;
                currentUser.State = user.State;
                currentUser.Zip = user.Zip;
                currentUser.Country = user.Country;

                var ShipInfo = await _context.Users
     .Include(s => s.Orders).SingleOrDefaultAsync(m => m.Id == user.Id);
                var lastorder = ShipInfo.Orders.OrderByDescending(d => d.OrderID).FirstOrDefault();
                if (lastorder != null)
                {
                    currentUser.ShippingLine1 = lastorder.ShippingLine1;
                    currentUser.ShippingLine2 = lastorder.ShippingLine2;
                    currentUser.ShippingCity = lastorder.ShippingCity;
                    currentUser.ShippingState = lastorder.ShippingState;
                    currentUser.ShippingZip = lastorder.ShippingZip;
                    currentUser.ShippingCountry = lastorder.ShippingCountry;
                }

                    return View(currentUser);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {

                ModelState.AddModelError("", "Sorry, your cart is empty!");

            }

            if (ModelState.IsValid)
            {
                Order order = new Order();
                order.Lines = cart.Lines.ToArray();
                order.AddedDate = DateTime.Now;
                order.ShippingCity = shippingDetails.City;
                order.ShippingCountry = shippingDetails.Country;
                order.ShippingState = shippingDetails.State;
                order.ShippingZip = shippingDetails.Zip;
                order.Name = shippingDetails.Name;
                order.Email = shippingDetails.Email;
                order.Phone = shippingDetails.Phone;
                order.ShippingLine1 = shippingDetails.Line1;
                order.ShippingLine2 = shippingDetails.Line2;
                order.Shipped = false;
                order.Total = cart.ComputeTotalValue();

                ApplicationUser currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    IdentityResult result = await _userManager.UpdateAsync(currentUser);
                    if (result.Succeeded)
                    {
                        order.applicationuser = currentUser;
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                repository.SaveOrder(order);
                ViewBag.orderid = order.OrderID;
                TempData["orderid"] = order.OrderID;

                string message = await _razorViewToStringRenderer.RenderViewToStringAsync("Email/Invoice", order);
                string email = order.Email;
                string subject = "Invoice";

                await _emailSender.SendEmail(email, subject, message);


                await _emailSender.SendEmail("jeromeklugh@gmail.com", "New Member Order", $"There is new Member Order and the Order ID is {ViewBag.orderid}");

                return RedirectToAction(nameof(Completed));

            }
            else
            {
                ApplicationUser user = await GetCurrentUserAsync();

                if (user != null)
                {
                    ShippingDetails currentUser = new ShippingDetails();
                    currentUser.Name = user.UserName;
                    currentUser.Email = user.Email;
                    currentUser.Phone = user.PhoneNumber;
                    currentUser.NameOnCard = user.NameOnCard;
                    currentUser.CreditCard = user.CreditCard;
                    currentUser.CardConfirm = user.CardConfirm;
                    currentUser.Month = user.Month;
                    currentUser.Year = user.Year;
                    currentUser.Line1 = user.Line1;
                    currentUser.Line2 = user.Line2;
                    currentUser.City = user.City;
                    currentUser.State = user.State;
                    currentUser.Zip = user.Zip;
                    currentUser.Country = user.Country;

                    var ShipInfo = await _context.Users
         .Include(s => s.Orders).SingleOrDefaultAsync(m => m.Id == user.Id);
                    var lastorder = ShipInfo.Orders.OrderByDescending(d => d.OrderID).FirstOrDefault();
                    if (lastorder != null)
                    {
                        currentUser.Line1 = lastorder.ShippingLine1;
                        currentUser.Line2 = lastorder.ShippingLine2;
                        currentUser.City = lastorder.ShippingCity;
                        currentUser.State = lastorder.ShippingState;
                        currentUser.Zip = lastorder.ShippingZip;
                        currentUser.Country = lastorder.ShippingCountry;
                    }

                    return View(currentUser);
                }
                return View();
            }
             }
   
        private async Task<ApplicationUser> GetCurrentUserAsync()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            // create an instance of MyClass using the service provider
            var instance = ActivatorUtilities.CreateInstance<Encryptor>(services);

            //ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            ApplicationUser AppUser = await _userManager.GetUserAsync(User);
            var currentUser = await _context.Users
      .Include(s => s.Orders).SingleOrDefaultAsync(m => m.Id == AppUser.Id);
            if (currentUser != null)
            {
                var card = instance.Decrypt(currentUser.CreditCard);
                var cardConfirm = instance.Decrypt(currentUser.CardConfirm);
                //card = string.Format("XXXXXXX{0}", card.Substring(card.Length - 4, 4));
                //cardConfirm = string.Format("XXXXXXX{0}", card.Substring(card.Length - 4, 4));
                currentUser.CreditCard = card;
                currentUser.CardConfirm = cardConfirm;

            }

            return currentUser;
        }


    }
}
