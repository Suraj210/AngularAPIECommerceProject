using ECommerceAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerceAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate,  string userSurname)
        {
            string mail = $"Hello Dear  {userSurname}, <br>" +
                $"{orderCode} order at {orderDate} date has been completed and sent to the third party cargo company.<br>" +
                $"Best regards!";

            await SendMailAsync(to, $"{orderCode}-Order Completed", mail);
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);

        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
            {
                mail.To.Add(to);
            }
            mail.Subject = subject;
            mail.Body = body;

            mail.From = new(_configuration["Mail:Username"], "SI E-Commerce", System.Text.Encoding.UTF8);

            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.Append("Hello<br>If you want to reste your password you can do it through link:<strong> <a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">");
            //var data = mail.ToString().Length;
            var restLink = mail.ToString().IndexOf("href=") + 6;
            string hyperLink = mail.ToString().Substring(restLink, mail.ToString().Length - restLink - 8);
            mail.Append(hyperLink);
            mail.Append("</a></strong><br><br><span style=\"font-size:12px;\">If you do not know about this request please skip this mail.</span><br><br><br>SI-Mini|E-commerce");
            await SendMailAsync(to, "Reset Password", mail.ToString());
        }
    }
}
