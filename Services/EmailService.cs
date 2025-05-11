using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BirSiberDanismanlik.Services
{
    public class EmailService
    {
        private readonly string _apiKey;
        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"]!;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress("no-reply@birSiberDanismanlik.com", "Bir Siber Danışmanlık");
            var toEmail = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(from, toEmail, subject, body, body);
            await client.SendEmailAsync(msg);
        }
    }
} 