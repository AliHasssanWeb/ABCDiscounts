using ABC.Shared.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync2(MailRequest mailRequest)
        {
            //_mailSettings.Mail = "noreply@abc-discounts.com";
            //_mailSettings.Password = "Yousid@1234";
            ////_mailSettings.Port = 465;
            ////_mailSettings.DisplayName = "ABC Discounts";
            ////_mailSettings.Host = "smtpout.secureserver.net";    

            //_mailSettings.Port = 993;
            //_mailSettings.DisplayName = "ABC Discounts";
            //_mailSettings.Host = "imap.secureserver.net";

            //var email = new MimeMessage();
            //email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            //email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            //email.Subject = mailRequest.Subject;
            //var builder = new BodyBuilder();
            ////if (mailRequest.Attachments != null)
            ////{
            ////    byte[] fileBytes;
            ////    foreach (var file in mailRequest.Attachments)
            ////    {
            ////        if (file.Length > 0)
            ////        {
            ////            using (var ms = new MemoryStream())
            ////            {
            ////                file.CopyTo(ms);
            ////                fileBytes = ms.ToArray();
            ////            }
            ////            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
            ////        }
            ////    }
            ////}
            //builder.HtmlBody = mailRequest.Body;
            //email.Body = builder.ToMessageBody();
            //using var smtp = new MailKit.Net.Smtp.SmtpClient();
            //smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            //smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            //await smtp.SendAsync(email);
            //smtp.Disconnect(true);
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            //_mailSettings.Mail = "noreply@abc-discounts.com";
            //_mailSettings.Password = "Yousid@1234";
            //_mailSettings.Port = 993;
            //_mailSettings.DisplayName = "ABC Discounts";
            //_mailSettings.Host = "imap.secureserver.net";

            MailMessage mailMessage = new MailMessage();
            SmtpClient client = new SmtpClient("smtpout.secureserver.net");

            mailMessage.From = new MailAddress("noreply@abc-discounts.com");
            mailMessage.To.Add(new MailAddress(mailRequest.ToEmail));
            mailMessage.Subject = mailRequest.Subject;
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = mailRequest.Body;
            //client.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
            client.Credentials = new System.Net.NetworkCredential("noreply@abc-discounts.com", "Yousid@1234");
            client.Port = 465;
            //client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // log exception
            }
        }
    }
}
