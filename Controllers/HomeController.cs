using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JeromeCore.Models;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using System.Text;
using MailKit.Security;

namespace JeromeCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Policy()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
           return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var client = new SmtpClient();
                client.Connect("firstsuperfoods.com", 587, SecureSocketOptions.None);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate("jeromeklugh@firstsuperfoods.com", "goDman0o712!");



                StringBuilder body = new StringBuilder()
                    .AppendLine("Name: " + model.Name)
                    .AppendLine("Return Address: " + model.From)
                    .AppendLine("Message: " + model.Message);

               
                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress("FirstSuperFoods", "jeromeklugh@firstsuperfoods.com"));
                mail.To.Add(new MailboxAddress("", "jeromeklugh@gmail.com"));
                mail.Subject = model.Subject;
               
                mail.Body = new TextPart("plain") { Text = body.ToString() };

                client.Send(mail);
                client.Disconnect(true);
               
                return View("Thanks");
            }
            else
            {
                return View();
            }


        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
