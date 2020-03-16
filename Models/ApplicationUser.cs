using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace JeromeCore.Models
{
    public enum Month
    {
        JAN=1, FEB, MAR, APR, MAY, JUN, JUL, AUG, SEP,
        OCT, NEV, DEC
    }
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public Month Month { get; set; }
        public string NameOnCard { get; set; }
        public string CreditCard { get; set; }
        public string CardConfirm { get; set; }
        public string Year { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsClosed { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
