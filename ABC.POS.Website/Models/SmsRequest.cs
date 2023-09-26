namespace ABC.POS.Website.Models
{
    public class SmsRequest
    {
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string PhoneNumber { get; set; }

    }
}
