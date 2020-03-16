using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models
{
    public class GuestInfo
    {
        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter name of the credit card")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "Please enter credit card number")]
        public string CreditCard { get; set; }

        [Required(ErrorMessage = "Please enter credit card number to comfirm it")]
        [Compare("CreditCard", ErrorMessage = "Credit Card is not matching!")]
        public string CardConfirm { get; set; }

        [Required(ErrorMessage = "Please select a month")]
        public Month Month { get; set; }

        [Required(ErrorMessage = "Please select a year")]
        public string Year { get; set; }

        public int OrderId { get; set; }

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
