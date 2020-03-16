using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class FakeProductRepository : IProductRepository
    {



        public IEnumerable<Product> Products => new List<Product> {

            new Product { ProductID = 1, Name = "Bee Sure", Description = "15.8oz Bag", Price = 24.95M },

            new Product { ProductID = 2, Name = "Super Cereal", Description = "16oz Bag", Price = 5.95M }

        };

    }
}
