using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using JeromeCore.Data;

namespace JeromeCore.Models
{
    public static class SeedData
    {



        public static void EnsurePopulated(IApplicationBuilder app)
        {

            ApplicationDbContext context = app.ApplicationServices

                .GetRequiredService<ApplicationDbContext>();

            if (!context.Products.Any())
            {

                context.Products.AddRange(

                    new Product
                    {

                        Name = "Bee Sure",
                        Description = "15.8oz Bag",
                        Price = 24.95M
                    },

                    new Product
                    {

                        Name = "Super Cereal",
                        Description = "16oz Bag",
                        Price = 5.95M
                    }

                );

                context.SaveChanges();

            }

        }
    }
}
