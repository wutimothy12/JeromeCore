using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JeromeCore.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [BindNever]
        public DateTime AddedDate { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        public string PhoneNumber { get; set; }

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
