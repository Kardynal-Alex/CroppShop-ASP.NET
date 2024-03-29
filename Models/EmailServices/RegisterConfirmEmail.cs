﻿using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CroppShop.Models.EmailServices
{
    public class RegisterConfirmEmail
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mm = new MailMessage("email", email);
            mm.Subject = subject;
            mm.Body = message;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("email", "password");
            //smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);
        }
    }
}
