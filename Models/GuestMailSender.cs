using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace JeromeCore.Models
{
    public class GuestMailSender
    {
        public void ProcessOrder(Cart cart, ShippingDetails shippingInfo)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddDataProtection();
            var services = serviceCollection.BuildServiceProvider();

            // create an instance of MyClass using the service provider
            var instance = ActivatorUtilities.CreateInstance<Encryptor>(services);
            string creditCardNumber = instance.Encrypt(shippingInfo.CreditCard);

            var client = new SmtpClient();
            client.Connect("firstsuperfoods.com", 587, SecureSocketOptions.None);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate("jeromeklugh@firstsuperfoods.com", "goDman0o712!");

           

                StringBuilder body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("Items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2:c}  ", line.Quantity,
                                      line.Product.Name,
                                      subtotal);
                }

            body.AppendFormat("  Total order value: {0:c}", cart.ComputeTotalValue())
                .AppendLine("---")
                .AppendLine()
                .AppendLine("Ship to:")
                .AppendLine(shippingInfo.Name)
                .AppendLine(shippingInfo.Phone)
                .AppendLine(shippingInfo.ShippingLine1)
                .AppendLine(shippingInfo.ShippingLine2 ?? "")
                .AppendLine(shippingInfo.ShippingCity)
                .AppendLine(shippingInfo.ShippingState)
                .AppendLine(shippingInfo.ShippingCountry)
                .AppendLine(shippingInfo.ShippingZip)
                .AppendLine()
                .AppendLine("Billing Info:")
                .AppendLine(shippingInfo.NameOnCard)
                .AppendLine("CreditCard:")
                .AppendLine(creditCardNumber)
                .AppendFormat("Expired Month: {0}", shippingInfo.Month)
                .AppendLine()
                .AppendFormat("Expired Year: {0}", shippingInfo.Year)
                .AppendLine()
                .AppendLine(shippingInfo.Line1)
                .AppendLine(shippingInfo.Line2 ?? "")
                .AppendLine(shippingInfo.City)
                .AppendLine(shippingInfo.State)
                .AppendLine(shippingInfo.Country)
                .AppendLine(shippingInfo.Zip);
                   
        

            var mail = new MimeMessage();

            mail.From.Add(new MailboxAddress("FirstSuperFoods", "jeromeklugh@firstsuperfoods.com"));
            mail.To.Add(new MailboxAddress("", "jeromeklugh@gmail.com"));
            mail.Subject = "New order submitted!";
            //BodyBuilder bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = msg.MessageBody;
            //mail.Body = bodyBuilder.ToMessageBody();
            mail.Body = new TextPart("plain") { Text = body.ToString() };

            client.Send(mail);
            client.Disconnect(true);
        }
        }
    }

