using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace JeromeCore.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public async Task SendEmail(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("FirstSuperFoods", "jeromeklugh@firstsuperfoods.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart("plain") { Text = message };
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message;
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                //client.LocalDomain = "some.domain.com";
                await client.ConnectAsync("firstsuperfoods.com", 587, SecureSocketOptions.None);
                await client.AuthenticateAsync("jeromeklugh@firstsuperfoods.com", "xxx");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
               
            }
        }
       
        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
