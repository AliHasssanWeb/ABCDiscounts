using ABC.POS.Website.Models;
using ABC.POS.Website.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public class SmsController : ControllerBase
{
    private readonly ISmsService _smsService;

    public SmsController(ISmsService smsService)
    {
        _smsService = smsService;
    }

    public async Task<IActionResult> SendSms()
    {
        var messageBody = "Hello, Every Body......!";
        var toPhoneNumbers = new List<string>
        {
            "+923084711290",
            //"+923087951610",
            //"+923032630309",
        };

        var sentSids = await _smsService.SendSmsAsync(toPhoneNumbers, messageBody);

        return Ok(new { SentSids = sentSids });
    }

}


