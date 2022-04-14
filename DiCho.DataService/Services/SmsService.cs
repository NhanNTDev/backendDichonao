using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DiCho.DataService.Services
{
    public partial interface ISmsService
    {
        Task<bool> SendSmsAsync(string toPhone, string body);
    }
    public partial class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;
        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> SendSmsAsync(string toPhone, string body)
        {
            var accountSid = _configuration["Twolio:AccountSid"];
            var authToken = _configuration["Twolio:authToken"];
            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: "+84" + toPhone,
                from: "+17655387693",
                body: body);
            return null;
        }
    }
}
