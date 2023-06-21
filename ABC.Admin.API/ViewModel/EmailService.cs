using System;
using System.Net.Mail;

namespace ABC.Admin.API.ViewModel
{
    public class EmailService
    {


        public bool SendEmail(string type,string toEmail, string user,string invoiceno,string reason)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.ab-sol.net");
                // toEmail = "usmanamjad5529@outlook.com";
                mail.From = new MailAddress("eService@ab-sol.net");
                mail.To.Add(toEmail);
                if (type == "approveorder")
                {
                    mail.Subject = "Order Approved";
                    mail.Body = OrderApproveEmail(user, invoiceno);
                }
                else if(type == "rejectorder")
                {
                    mail.Subject = "Order Rejected";
                    mail.Body = OrderRejectEmail(user, invoiceno, reason);
                }
                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("eService@ab-sol.net", "P@kistan1947");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string OrderRejectEmail(string user,string invoiceno, string reason)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{user} Sorry againts Invoice # {invoiceno}, your Order has been rejected  </td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "The Reason is: " + reason;
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>ABC Discounts</b><p>Administration</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; ABC Discounts";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        private string OrderApproveEmail(string user, string invoiceno)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{user} Your order against invoice # {invoiceno} has been Approved, the delivery date will be send to you in next email.</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Thank you for shopping with us.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'><a href=";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>ABC Discounts</b><p>Administration</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; ABC Discounts";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }

        public bool SendNewCustomerEmail(string type, string toEmail, string user)
        {
            try
            {
               // toEmail = "usmanamjad5529@outlook.com";
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("lydiacms00@gmail.com");
                mail.To.Add(toEmail);
                if (type == "NewCustomer")
                {
                    mail.Subject = "Thank You For Registration";
                    mail.Body = RegisteredNewCustomer(toEmail, user);
                }
                else if (type == "ApproveCustomer")
                {
                    mail.Subject = "Welcome, Your account has approved";
                    mail.Body = ApprovedNewCustomer(toEmail, user);
                }
                else if (type == "RejectCustomer")
                {
                    mail.Subject = "Sorry, Your account has been rejected";
                    mail.Body = RejectedNewCustomer(toEmail, user);
                }

                mail.IsBodyHtml = true;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new System.Net.NetworkCredential("lydiacms00@gmail.com", "Cms@firm00");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private string RegisteredNewCustomer(string email, string fullname)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{fullname} Thank you for registration on ABC Disc</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your registered account is under review by ABC Discounts administration.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'>";
            html += "<p>You will receive an email on approval/rejection.</p></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>ABC Discounts</b><p>Administration</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; ABC Disc";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        private string ApprovedNewCustomer(string email, string fullname)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{fullname} Thank you for registration on ABC Disc</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your registered account has been Approved by ABC Discounts Administration.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'>";
            html += "<p>You can use your Registered Email & Password for Login.</p></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>ABC Discounts</b><p>Administration</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; ABC Disc";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
        private string RejectedNewCustomer(string email, string fullname)
        {
            string html = string.Empty;
            html += "<style>body { margin-top: 20px;} </style>";
            html += "<table class='body-wrap' style='width:100%;'><tbody><tr><td></td><td>";
            html += "<div style='box-sizing:border-box;max-width:600px;margin:0 auto;padding:20px;'><table><tbody><tr>";
            html += "<td style='padding:30px;border:3px solid #67a8e4;border-radius:7px;'><meta><table><tbody><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += $"{fullname} Thank you for registration on ABC Disc</td></tr><tr>";
            html += "<td style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "Your account has been Rejected by ABC Discounts Administration.";
            html += "</td></tr><tr><td style ='padding: 0 0 20px;'>";
            html += "<p>Please Contact Administration through Email at ABCDiscounts@gmail.com</p></td></tr><tr>";
            html += "<td class='content-block' style='font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;padding: 0 0 20px;'>";
            html += "<b>ABC Discounts</b><p>Administration</p></td></tr><tr>";
            html += "<td style='text-align:center;font-family:Helvetica Neue,Helvetica,Arial,sans-serif;font-size:14px;'>&copy; ABC Disc";
            html += "</td></tr></tbody></table></td></tr></tbody></table></div></td></tr></tbody></table>";
            return html;
        }
    }
}
