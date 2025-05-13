using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BirSiberDanismanlik.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var emailConfig = _config.GetSection("Email");
            var smtpClient = new SmtpClient(emailConfig["Host"])
            {
                Port = int.Parse(emailConfig["Port"]),
                Credentials = new NetworkCredential(emailConfig["User"], emailConfig["Pass"]),
                EnableSsl = bool.Parse(emailConfig["EnableSsl"])
            };

            // No-reply gönderen
            var fromAddress = new MailAddress(emailConfig["User"], "birSiber Danışmanlık | No-Reply");

            var mailMessage = new MailMessage
            {
                From = fromAddress,
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
} 