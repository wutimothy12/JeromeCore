using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using JeromeCore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using JeromeCore.Data;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace JeromeCore.Controllers
{
    [Authorize(Roles = "Admins")]
    public class AdminController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;
        public int PageSize = 10;

        public AdminController(UserManager<ApplicationUser> usrMgr, ApplicationDbContext context)
        {
            userManager = usrMgr;
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public ViewResult Users(int page = 1) => View(new UserListViewModel
             {
            ApplicationUsers = userManager.Users
                    .OrderBy(p => p.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
            PagingInfo = new PagingInfo(@userManager.Users.Count(), page, PageSize, 10)
        {
            CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = userManager.Users.Count()

            }
    });

            
        [HttpPost]

        public async Task<IActionResult> Delete(string id)
        {

            ApplicationUser user = await userManager.FindByIdAsync(id);
            //var appuser = await _context.Orders.SingleOrDefaultAsync(m => m.applicationuser.Id == id);
            //if(appuser != null)
            //{
            //    _context.Orders.Remove(appuser);
            //    await _context.SaveChangesAsync();
            //}
           
            if (user != null)
            {
                user.IsClosed = true;
                _context.Users.Attach(user).Property(x => x.IsClosed).IsModified = true;
                await _context.SaveChangesAsync();
                //IdentityResult result = await userManager.DeleteAsync(user);

                //if (result.Succeeded)
                //{

                //    return RedirectToAction("Index");

                //}
                //else
                //{

                //    AddErrorsFromResult(result);

                //}

            }
            else
            {

                ModelState.AddModelError("", "User Not Found");

            }

            return View("Index", userManager.Users);

        }

        [HttpPost]

        public async Task<IActionResult> UnDelete(string id)
        {

            ApplicationUser user = await userManager.FindByIdAsync(id);
            
            if (user != null)
            {
                user.IsClosed = false;
                _context.Users.Attach(user).Property(x => x.IsClosed).IsModified = true;
                await _context.SaveChangesAsync();
               
            }
            else
            {

                ModelState.AddModelError("", "User Not Found");

            }

            return View("Index", userManager.Users);

        }

        public IActionResult Decrytor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Decrytor(string cardNumber)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            // create an instance of MyClass using the service provider
            var instance = ActivatorUtilities.CreateInstance<Encryptor>(services);

            string pwcode = Request.Form["cardNumber"];
            string realcard = instance.Decrypt(pwcode);
            ViewBag.review = realcard;

            return View();
            
        }

        private void AddErrorsFromResult(IdentityResult result)
        {

            foreach (IdentityError error in result.Errors)
            {

                ModelState.AddModelError("", error.Description);

            }

        }
    }
}
