using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class ShippingDetails
    {
        [BindNever]

        public ICollection<CartLine> Lines { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter name of the credit card")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Please enter credit card number")]
        [CreditCard]
        public string CreditCard { get; set; }

        [Required(ErrorMessage = "Please enter credit card number to comfirm it")]
        [Compare("CreditCard", ErrorMessage = "Credit Card is not matching!")]
        public string CardConfirm { get; set; }

        [Required(ErrorMessage = "Please select a month")]
        public Month Month { get; set; }

        [Required(ErrorMessage = "Please select a year")]
        public string Year { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        public string ShippingLine1 { get; set; }
        public string ShippingLine2 { get; set; }
        
        [Required(ErrorMessage = "Please enter a city name")]
        public string ShippingCity { get; set; }

        [Required(ErrorMessage = "Please enter a state name")]
        public string ShippingState { get; set; }

        [Required(ErrorMessage = "Please enter the Zip Code")]
        public string ShippingZip { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public string ShippingCountry { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a state name")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter the Zip Code")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }

    }
}

