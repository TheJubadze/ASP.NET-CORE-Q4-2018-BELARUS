using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WebApp.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com")
            {
                UseDefaultCredentials = false, 
                Credentials = new NetworkCredential("testu8888@gmail.com", ""),
                Port = 587,
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress("testu8888@gmail.com")
            };
            mailMessage.To.Add(email);
            mailMessage.Body = htmlMessage;
            mailMessage.Subject = subject;
            return client.SendMailAsync(mailMessage);
        }
    }
}