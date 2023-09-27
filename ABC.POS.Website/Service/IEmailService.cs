using ABC.POS.Website.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABC.POS.Website.Service
{
    public interface IEmailService
    {
        //Task SendEmailForEmailConfirmation(InvoiceModel invoiceModel);
        //Task SendEmailForForgotPassword(InvoiceModel invoiceModel);
        Task SendTestEmail(UserEmailOptions userEmailOptions);
    }
}