using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JeromeCore.Models;

namespace JeromeCore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }
        public IActionResult Products()
        {
            return View();
        }

        public IActionResult beeSure()
        {
            var beesure = repository.Products.Where(p => p.ProductID == 1).FirstOrDefault(); 
           
            return View(beesure);
        }

        public IActionResult superCereal()
        {
            var supercereal = repository.Products.Where(p => p.ProductID == 2).FirstOrDefault();
            
            return View(supercereal);
        }
    }
}