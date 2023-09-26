using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;

namespace ABC.POS.Website.Service
{
    public class SmsService : ISmsService
    {
        private readonly string TwilioAccountSid;
        private readonly string TwilioAuthToken;
        private readonly string TwilioPhoneNumber;

        public SmsService(string twilioAccountSid, string twilioAuthToken, string twilioPhoneNumber)
        {
            TwilioAccountSid = twilioAccountSid;
            TwilioAuthToken = twilioAuthToken;
            TwilioPhoneNumber = twilioPhoneNumber;

            TwilioClient.Init(twilioAccountSid, twilioAuthToken);
        }

        public async Task<List<string>> SendSmsAsync(List<string> toPhoneNumbers, string messageBody)
        {
            var sentSids = new List<string>();

            foreach (var toPhoneNumber in toPhoneNumbers)
            {
                var message = await MessageResource.CreateAsync(
                    to: new PhoneNumber(toPhoneNumber),
                    from: new PhoneNumber(TwilioPhoneNumber),
                    body: messageBody
                );

                sentSids.Add(message.Sid);
            }

            return sentSids;
        }
    }
}
