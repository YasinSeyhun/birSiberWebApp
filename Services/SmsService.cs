using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BirSiberDanismanlik.Services
{
    public class SmsService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromNumber;
        public SmsService(IConfiguration configuration)
        {
            _accountSid = configuration["Twilio:AccountSid"]!;
            _authToken = configuration["Twilio:AuthToken"]!;
            _fromNumber = configuration["Twilio:FromNumber"]!;
        }

        public async Task SendSmsAsync(string to, string message)
        {
            TwilioClient.Init(_accountSid, _authToken);
            await MessageResource.CreateAsync(
                body: message,
                from: new Twilio.Types.PhoneNumber(_fromNumber),
                to: new Twilio.Types.PhoneNumber(to)
            );
        }
    }
} 