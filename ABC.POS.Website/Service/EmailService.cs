﻿using ABC.POS.Website.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;

namespace ABC.POS.Website.Service
{
    public class EmailService : IEmailService
    {
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;
        public EmailService(IRazorViewToStringRenderer razorViewToStringRenderer, SMTPConfigModel smtpConfig)
        {
            _razorViewToStringRenderer = razorViewToStringRenderer;
            _smtpConfig = smtpConfig;
    }
        

        public async Task SendTestEmail(UserEmailOptions userEmailOptions)
        {
            //var listInvmodel = new List<InvoiceTotal>() {
            //    new InvoiceTotal { ItemCode = "234",ItemName = "abc" },
            //    new InvoiceTotal { ItemCode = "345",ItemName = "cdf" },
            //    new InvoiceTotal { ItemCode = "567",ItemName = "bgt" }
            //};
            //var confirmAccountModel = new ConfirmAccountEmailViewModel($"index/{Guid.NewGuid()}");

            //userEmailOptions.Subject = "domey data subject test "; ///UpdatePlaceHolders("Hello {{UserName}}, This is test email subject from book store web app", userEmailOptions.PlaceHolders);

            //userEmailOptions.Body = emailBody; //await _razorViewToStringRenderer.RenderViewToStringAsync(templatePath, listinvTotal);

            await SendEmail(userEmailOptions);
        }

        //public async Task SendEmailForEmailConfirmation(UserEmailOptions userEmailOptions)
        //{
        //    userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, Confirm your email id.", userEmailOptions.PlaceHolders);

        //    userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), userEmailOptions.PlaceHolders);

        //    await SendEmail(userEmailOptions);
        //}

        //public async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        //{
        //    userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", userEmailOptions.PlaceHolders);

        //    userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

        //    await SendEmail(userEmailOptions);
        //}

        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new MailMessage
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            NetworkCredential networkCredential = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpConfig.Host,
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                UseDefaultCredentials = _smtpConfig.UseDefaultCredentials,
                Credentials = networkCredential
            };

            mail.BodyEncoding = Encoding.Default;

            await smtpClient.SendMailAsync(mail);
        }

        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }

        //public Task SendEmailForEmailConfirmation(UserEmailOptions invoiceModel)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task SendEmailForForgotPassword(UserEmailOptions invoiceModel)
        //{
        //    throw new System.NotImplementedException();
        //}

        ////public Task SendTestEmail(UserEmailOptions userEmailOptions)
        ////{
        ////    throw new System.NotImplementedException();
        ////}

        //public Task SendEmailForEmailConfirmation(InvoiceModel invoiceModel)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public Task SendEmailForForgotPassword(InvoiceModel invoiceModel)
        //{
        //    throw new System.NotImplementedException();
        //}
    }
}
