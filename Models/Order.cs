using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class Order
    {
        [BindNever]

        public int OrderID { get; set; }

        [BindNever]

        public ICollection<CartLine> Lines { get; set; }

        [BindNever]

        public bool Shipped { get; set; }

        [Required(ErrorMessage = "Please enter a name")]

        public string Name { get; set; }
        [Required(ErrorMessage = "Please enter a email")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter a phone")]

        public string Phone { get; set; }
        
        public DateTime AddedDate { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]

        public string ShippingLine1 { get; set; }

        public string ShippingLine2 { get; set; }

       [Required(ErrorMessage = "Please enter a city name")]

        public string ShippingCity { get; set; }



        [Required(ErrorMessage = "Please enter a state name")]

        public string ShippingState { get; set; }


        [Required(ErrorMessage = "Please enter a zip")]
        public string ShippingZip { get; set; }


        [Required(ErrorMessage = "Please enter a country name")]

        public string ShippingCountry { get; set; }

        public decimal Total { get; set; }

        public ApplicationUser applicationuser { get; set; }
    }
}
