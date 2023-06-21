using ABC.Admin.API.ViewModel;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        protected readonly ABCDiscountsContext db;
        EncryptDecrypt encdec = new EncryptDecrypt();
        private readonly IMailService mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private EmailService emailService = new EmailService();
        public OrdersController(ABCDiscountsContext _db, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            this.mailService = mailService;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet("AllOrderGet")]
        public IActionResult AllOrderGet()
        {
            try
            {
                var list = db.CustomerOrders.ToList();
                var Record = db.CartDetails.ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].OrderDaysAlert = false;
                    var Counttickets = Record.ToList().Where(x => x.TicketId == list[i].TicketId).Count();
                    list[i].LineCounts = Counttickets;
                    if (CheckDays(Convert.ToDateTime(list[i].AdminActionDate)) == false)
                    {
                        list[i].OrderDaysAlert = true;
                    }
                    double value = Convert.ToDouble(list[i].OrderAmount);
                    value = (double)System.Math.Round(value, 2);
                    list[i].OrderAmount = Convert.ToString(value);
                }
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, list.OrderByDescending(x => x.TicketId).ToList());
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUserCartByAdminApproval")]
        public IActionResult GetUserCartByAdminApproval(int id, string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
               // var record = db.CartDetails.Where(x => x.UserId == id && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                var record = db.CartDetails.Where(x => x.TicketId == ticketId).ToList();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("ApproveOrderById")]
        public IActionResult ApproveOrderById(int id,int userid,string ordernum, string orderDate,string pullerId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();

                var GetEmployeePuller = db.Employees.Find(Convert.ToInt32(pullerId));
                if(GetEmployeePuller != null)
                {
                    var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == false)?.ToList();
                    if (record.Count() > 0)
                    {
                        var CurrentUser = db.AspNetUsers.Find(userid);
                        for (int i = 0; i < record.Count(); i++)
                        {
                            if (CurrentUser != null)
                            {
                                if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                                {
                                    record[i].AdminActionBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                                }
                                else
                                {
                                    if (CurrentUser.UserName != null)
                                    {
                                        record[i].AdminActionBy = CurrentUser.UserName;
                                    }
                                }
                            }
                            record[i].AdminActionDate = DateTime.Now;
                            record[i].AdminActionTime = DateTime.Now.ToString("hh:mm tt");
                            record[i].AdminStatus = true;
                            //record[i].OrderDate = DateTime.Parse(orderDate);
                            record[i].ExpectedDate = Convert.ToDateTime(orderDate);
                            record[i].PullerEmployeeId = GetEmployeePuller.EmployeeId;
                            record[i].PulledBy = GetEmployeePuller.FullName;
                            db.Entry(record[i]).State = EntityState.Modified;
                            db.SaveChanges();

                            if(record[0].PaymentMode != null && record[0].PaymentMode != "" && record[0].PaymentMode != "undefined" && record[0].PaymentMode == "Cod")
                            {
                                var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record[0].UserId).FirstOrDefault();
                                var FindRec = db.Receivables.ToList().Where(x => x.AccountId == GetCustomer.AccountId).FirstOrDefault();
                                if (FindRec != null)
                                {

                                    double orderPriceAmount = Math.Round(Convert.ToDouble(record[0].OrderAmount), 2);
                                    FindRec.Amount = (Convert.ToDouble(FindRec.Amount) + orderPriceAmount).ToString();
                                    db.Entry(FindRec).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    double orderPriceAmount = Math.Round(Convert.ToDouble(record[0].OrderAmount), 2);

                                    Receivable newRec = new Receivable();
                                    newRec.AccountId = GetCustomer.AccountId;
                                    newRec.AccountNumber = GetCustomer.AccountNumber;
                                    newRec.AccountName = GetCustomer.AccountTitle;
                                    newRec.Amount = orderPriceAmount.ToString();
                                    db.Receivables.Add(newRec);
                                    db.SaveChanges();
                                }
                            }
                        }

                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record[0]);
                        //send email to client
                        //var name = obj.FirstName + obj.LastName;
                        //bool isEmailSent = emailService.SendEmail("approveorder", CurrentUser.Email, record[0].CustomerName, record[0].TicketId, "");
                        //if (isEmailSent)
                        //{
                        //    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        //}
                        //else
                        //{
                        //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                        //}
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }

                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("DeliverOrderById")]
        public IActionResult DeliverOrderById(int userid, string ordernum, string orderDate)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == true && x.IsPulled == true && x.Delivered == false && x.IsRejected == false).ToList();
                if (record.Count() > 0)
                {
                    var CurrentUser = db.AspNetUsers.Find(userid);

                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (CurrentUser != null)
                        {
                            if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                            {
                                record[i].DeliveredBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                            }
                            else
                            {
                                if (CurrentUser.UserName != null)
                                {
                                    record[i].DeliveredBy = CurrentUser.UserName;
                                }
                            }
                        }
                        record[i].Delivered = true;
                        record[i].DeliveredDate = DateTime.Now;
                        record[i].FinalDate = Convert.ToDateTime(orderDate);
                        record[i].DeliveredTime = DateTime.Now.ToString("hh:mm tt");
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                    //send email to client
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("RejectOrderById")]
        public IActionResult RejectOrderById(int id, int userid, string ordernum, string reason, string reasondropdown)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == false)?.ToList();
                if (record.Count() > 0)
                {
                    var CurrentUser = db.AspNetUsers.Find(userid);
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (CurrentUser != null)
                        {
                            if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                            {
                                record[i].AdminActionBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                            }
                            else
                            {
                                if (CurrentUser.UserName != null)
                                {
                                    record[i].AdminActionBy = CurrentUser.UserName;
                                }
                            }
                        }
                        record[i].AdminActionDate = DateTime.Now;
                        record[i].AdminActionTime = DateTime.Now.ToString("hh:mm tt");
                        record[i].IsRejected = true;
                        record[i].RejectReason = reason;
                        record[i].RejectComments = reasondropdown;
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //send email to client
                    bool isEmailSent = emailService.SendEmail("rejectorder", CurrentUser.Email, record[0].CustomerName, record[0].TicketId, record[0].RejectReason);
                    if (isEmailSent)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("OrderDetail")]
        public IActionResult OrderDetail(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ticketId).ToList();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetOrderDetails/{ticketId}")]
        public IActionResult GetOrderDetails(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CartDetail>>();
                // var record = db.CartDetails.Where(x => x.UserId == id && x.PendingForApproval == true && x.IsDelivered == false).ToList();
                var record = db.CartDetails.Where(x => x.TicketId == ticketId).ToList();
                if (record != null)
                {
                    var customerorderData = db.CustomerOrders.Where(x => x.TicketId == record[0].TicketId).ToList();
                    for (int i = 0; i < customerorderData.Count(); i++)
                    {
                        double value = Convert.ToDouble(customerorderData[i].OrderAmount);
                        value = (double)System.Math.Round(value, 2);
                        customerorderData[i].OrderAmount = Convert.ToString(value);
                    }
                    record[0].CustomerOrders = customerorderData;
                    for (int i = 0; i < record.Count(); i++)
                    {
                        

                        record[i].Products = new List<Product>();
                        var GetProduct = db.Products.ToList().Where(x=>x.Id == record[i].Id && x.NeedHighAuthorization == true).ToList();
                        record[i].Products = GetProduct;
                        var authorizedata = db.AutherizeOrderLimits.Where(x => x.ProductId == record[i].Id).ToList();
                        if (authorizedata.Count > 0)
                        {
                            foreach (var item in authorizedata.ToList().Where(x => x.ProductId == record[i].Id).ToList())
                            {
                                record[i].AutherizeOrderLimits = item;
                            }

                        }
                    }
                    var CurrentCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record[0].UserId).FirstOrDefault();
                    if (CurrentCustomer != null)
                    {

                        var billingData = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == CurrentCustomer.Id && x.CustomerCode == CurrentCustomer.CustomerCode).FirstOrDefault();
                        var getpay = db.Receivables.ToList().Where(x => x.AccountId == CurrentCustomer.AccountId).FirstOrDefault();

                        if (billingData != null)
                        {
                            CurrentCustomer.CustomerBillingInfo = billingData;
                        }
                        if (getpay != null)
                        {
                            if (CurrentCustomer.CustomerBillingInfo != null)
                            {
                                if (Convert.ToDouble(CurrentCustomer.CustomerBillingInfo.CreditLimit) > Convert.ToDouble(getpay.Amount))
                                {
                                    record[0].Balance = "0";
                                }
                                else
                                {
                                    double balanceamount = Math.Round(Convert.ToDouble(getpay.Amount), 2);
                                    record[0].Balance = balanceamount.ToString();
                                   // record[0].Balance = getpay.Amount;
                                }
                            }
                            else
                            {
                                record[0].Balance = "0";
                            }

                        }
                        else
                        {
                            record[0].Balance = "0";
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


   


            [HttpGet("NewOrdersGet")]
        public IActionResult NewOrdersGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsInvoiced == false && x.IsRejected == false).ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    record[i].OrderDaysAlert = false;
                    var Counttickets = db.CustomerOrders.ToList().Where(x=>x.TicketId == record[i].TicketId).Count();
                    record[i].LineCounts = Counttickets;
                    if(record[i].OrderDate == null)
                    {
                        record[i].OrderDaysAlert = false;
                    }
                    else
                    {
                        if (CheckDays(Convert.ToDateTime(record[i].OrderDate)) == false)
                        {
                            record[i].OrderDaysAlert = true;
                        }
                    }
                    
                    var CurrentCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == record[i].UserId).FirstOrDefault();
                    if (CurrentCustomer != null)
                    {
                        var billingData = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == CurrentCustomer.Id && x.CustomerCode == CurrentCustomer.CustomerCode).FirstOrDefault();
                        var getpay = db.Receivables.ToList().Where(x => x.AccountId == CurrentCustomer.AccountId).FirstOrDefault();

                        if (billingData != null)
                        {
                            CurrentCustomer.CustomerBillingInfo = billingData;
                        }
                        if (getpay != null)
                        {
                            if (CurrentCustomer.CustomerBillingInfo != null)
                            {
                                if (Convert.ToDouble(CurrentCustomer.CustomerBillingInfo.CreditLimit) > Convert.ToDouble(getpay.Amount))
                                {
                                    record[i].Balance = "0";
                                }
                                else
                                {
                                    double balanceamount = Math.Round(Convert.ToDouble(getpay.Amount), 2);
                                    record[i].Balance = balanceamount.ToString();
                                }
                            }
                            else
                            {
                                record[i].Balance = "0";
                            }

                        }
                        else
                        {
                            record[i].Balance = "0";
                        }
                    }
                }
              
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.OrderByDescending(x=>x.TicketId).ToList());
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("CheckSupervisorCreditAmount/{customerId}")]
        public IActionResult CheckSupervisorCreditAmount(int? customerId, Supervisor supervisor)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var customerdata = db.CustomerInformations.ToList().Where(x => x.UserId == customerId).FirstOrDefault();
                if (customerdata != null)
                {
                    var record = db.Supervisors.Where(x => x.AccessPin == supervisor.AccessPin && x.CustomerId == customerdata.Id)?.FirstOrDefault();
                    if (record != null)
                    {
                        //if (supervisor.CreditLimit.Contains('.'))
                        //{
                        //    supervisor.CreditLimit = supervisor.CreditLimit.Split('.')[0];
                        //}
                        if (Convert.ToDouble(supervisor.CreditLimit) <= Convert.ToDouble(record.CreditLimit))
                        {
                            //var checkCredit = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == customerId && x.PaymentStatus == false).ToList();
                            DateTime dtSeven = DateTime.Now.Date.AddDays(-7);
                            //if (checkCredit.Count() >= 1)
                            //{
                            //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, record);
                            //    return Ok(Response);
                            //}
                            var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == customerId).FirstOrDefault();
                            DateTime dt = DateTime.Now.Date.AddDays(-13);
                            var creditAmount = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == GetCustomer.Id).ToList();
                            double totalCreditAmount = 0;
                            if (creditAmount != null)
                            {
                                for (int i = 0; i < creditAmount.Count(); i++)
                                {
                                    var getCustomerOrder = db.CustomerOrders.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.TicketId == creditAmount[i].SaleId && x.DueDate != null).FirstOrDefault();
                                    if (getCustomerOrder != null)
                                    {
                                        var a = Convert.ToDateTime(getCustomerOrder.DueDate).Date;
                                        var aa = dtSeven.Date;
                                        var aaa = DateTime.Now.Date;
                                        if (Convert.ToDateTime(getCustomerOrder.DueDate).Date >= DateTime.Now.Date && getCustomerOrder.IsPaid == false)
                                        {
                                            if (record.RoleName == "Supervisor")
                                            {
                                                record.AvailedSupervisor = true;
                                                record.AvailedManager = false;
                                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Availed_Supervisor, null, record);
                                                return Ok(Response);
                                            }
                                            else if (record.RoleName == "Sales Manager")
                                            {
                                                record.AvailedManager = true;
                                                record.AvailedSupervisor = false;
                                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Availed_Manager, null, record);
                                                return Ok(Response);
                                            }
                                        }
                                        else if (Convert.ToDateTime(getCustomerOrder.DueDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false)
                                        {
                                            if (record.RoleName == "Supervisor")
                                            {
                                                record.AvailedSupervisor = true;
                                                record.AvailedManager = false;
                                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Availed_Supervisor, null, record);
                                                return Ok(Response);
                                            }
                                            else if (record.RoleName == "Sales Manager")
                                            {
                                                record.AvailedManager = true;
                                                record.AvailedSupervisor = false;
                                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Availed_Manager, null, record);
                                                return Ok(Response);
                                            }
                                        }
                                    }
                                    var creditDate = creditAmount[i].CreditDate;
                                    if (creditDate >= dt)
                                    {
                                        totalCreditAmount = totalCreditAmount + Convert.ToDouble(creditAmount[i].CreditAmount);
                                    }
                                }
                            }
                            if (totalCreditAmount >= Convert.ToDouble(record.CreditLimit))
                            {
                                ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, record);
                                return Ok(Response);
                            }
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                        }
                        else
                        {
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.Less_Credit_Allowed, null, record);
                        }
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, record);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AccessApproveOrderById")]
        public IActionResult AccessApproveOrderById(int id, int userid, string ordernum = "", string orderDate = "", string pullerId = "", string comment="", string CreditTotal = "", string autherizePin = "",string creditduedate="")
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                //if (!ordernum.Contains("00-"))
                //{
                //    ordernum = "00" + ordernum;
                //}
                var GetEmployeePuller = db.Employees.Find(Convert.ToInt32(pullerId));
                if (GetEmployeePuller != null)
                {
                    var checkTicketrecord = db.CustomerOrders.Where(x => x.OrderId == Convert.ToInt32(id) && x.AdminStatus == false)?.FirstOrDefault();
                    var record = db.CustomerOrders.Where(x => x.TicketId == checkTicketrecord.TicketId && x.AdminStatus == false)?.ToList();
                    if(checkTicketrecord != null)
                    {
                        if (record.Count() > 0)
                        {
                            var CurrentUser = db.AspNetUsers.Find(userid);
                            for (int i = 0; i < record.Count(); i++)
                            {
                                if (CurrentUser != null)
                                {
                                    if (CurrentUser.Firstname != null && CurrentUser.Lastname != null)
                                    {
                                        record[i].AdminActionBy = CurrentUser.Firstname + " " + CurrentUser.Lastname;
                                    }
                                    else
                                    {
                                        if (CurrentUser.UserName != null)
                                        {
                                            record[i].AdminActionBy = CurrentUser.UserName;
                                        }
                                    }
                                }
                                record[i].AdminActionDate = DateTime.Now;
                                record[i].AdminActionTime = DateTime.Now.ToString("hh:mm tt");
                                record[i].AdminStatus = true;
                                record[i].OrderDate = DateTime.Now;
                                record[i].ExpectedDate = Convert.ToDateTime(orderDate);
                                record[i].PullerEmployeeId = GetEmployeePuller.EmployeeId;
                                record[i].PulledBy = GetEmployeePuller.FullName;
                                record[i].IsPaid = false;
                                record[i].DueDate = creditduedate;
                                db.Entry(record[i]).State = EntityState.Modified;
                                db.SaveChanges();

                                SupervisorCredit NewCreditAllow = new SupervisorCredit();

                                var GetCustomer = db.CustomerInformations.ToList().Where(x => x.UserId == userid).FirstOrDefault();
                                var GetUserData = db.Supervisors.ToList().Where(x => x.AccessPin == autherizePin && x.CustomerId == GetCustomer.Id).FirstOrDefault();
                                NewCreditAllow.CustomerId = GetCustomer.Id;
                                NewCreditAllow.Comment = comment;
                                NewCreditAllow.RoleId = GetUserData.RoleId;
                                NewCreditAllow.SupervisorId = GetUserData.SupervisorId;
                                if (GetUserData.RoleName != null)
                                {
                                    NewCreditAllow.RoleName = GetUserData.RoleName;

                                }
                                else
                                {
                                    var GetRole = db.AspNetRoles.Find(GetUserData.RoleId);
                                    if (GetRole != null)
                                    {
                                        NewCreditAllow.RoleName = GetRole.Name;
                                    }
                                }
                                NewCreditAllow.CreditDate = DateTime.Now;
                                NewCreditAllow.CustomerId = GetCustomer.Id;
                                NewCreditAllow.SaleId = record[0].TicketId;
                                NewCreditAllow.CreditAmount = record[0].OrderAmount;
                                NewCreditAllow.PaymentDate = Convert.ToDateTime(creditduedate);
                                NewCreditAllow.PaymentStatus = false;
                                if (GetUserData.UserId != null)
                                {
                                    var getUserInfo = db.AspNetUsers.Find(GetUserData.UserId);
                                    if (getUserInfo != null)
                                    {
                                        if (getUserInfo.UserName != null)
                                        {
                                            NewCreditAllow.FullName = getUserInfo.UserName;
                                        }
                                    }
                                }
                                if (GetUserData.EmployeeId != null)
                                {
                                    var getEmployeeInfo = db.Employees.Find(GetUserData.EmployeeId);
                                    if (getEmployeeInfo != null)
                                    {
                                        NewCreditAllow.EmployeeCode = getEmployeeInfo.EmployeeCode;
                                    }
                                }
                                db.SupervisorCredits.Add(NewCreditAllow);
                                db.SaveChanges();

                                var FindRec = db.Receivables.ToList().Where(x => x.AccountId == GetCustomer.AccountId).FirstOrDefault();
                                if (FindRec != null)
                                {

                                    double orderPriceAmount = Math.Round(Convert.ToDouble(record[0].OrderAmount), 2);
                                    FindRec.Amount = (Convert.ToDouble(FindRec.Amount) + orderPriceAmount).ToString();
                                    db.Entry(FindRec).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                else
                                {
                                    double orderPriceAmount = Math.Round(Convert.ToDouble(record[0].OrderAmount), 2);

                                    Receivable newRec = new Receivable();
                                    newRec.AccountId = GetCustomer.AccountId;
                                    newRec.AccountNumber = GetCustomer.AccountNumber;
                                    newRec.AccountName = GetCustomer.AccountTitle;
                                    newRec.Amount = orderPriceAmount.ToString();
                                    db.Receivables.Add(newRec);
                                    db.SaveChanges();
                                }
                            }
                            //send email to client
                            //var name = obj.FirstName + obj.LastName;
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record[0]);

                            //bool isEmailSent = emailService.SendEmail("approveorder", CurrentUser.Email, record[0].CustomerName, record[0].TicketId, "");
                            //if (isEmailSent)
                            //{
                            //    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                            //}
                            //else
                            //{
                            //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                            //}
                        }
                        else
                        {
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                        }
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }

                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        public bool CheckDays(DateTime actionDate)
        {

            try
            {
                var CurrentDate = DateTime.Now;
                var FoundedDays = (CurrentDate.Date - actionDate.Date).Days;
                if (FoundedDays >= 2)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return false;
            }
        }

        [HttpGet("PullOrderById")]
        public IActionResult PullOrderById(int id, string ordernum, string pullerId)
        {
            try
            {
                var employeeidtoint = Convert.ToInt32(pullerId);
                var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ordernum && x.AdminStatus == true && x.IsPulled == false)?.ToList();
                var employeedata = db.Employees.ToList().Where(x => x.EmployeeId == employeeidtoint).FirstOrDefault();
                if (record.Count() > 0)
                {
                    // var CurrentUser = db.AspNetUsers.Find(userid);
                    for (int i = 0; i < record.Count(); i++)
                    {
                        record[i].PulledDate = DateTime.Now;
                        record[i].PulledTime = DateTime.Now.ToString("hh:mm tt");
                        record[i].IsPulled = true;
                        record[i].InvoiceEmployeeId = employeeidtoint;
                        record[i].InvoicedBy = employeedata.Email;
                        record[i].IsInvoiced = false;
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //send email to client
                    //var name = obj.FirstName + obj.LastName;
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);

                    //bool isEmailSent = emailService.SendEmail("approveorder", CurrentUser.Email, record[0].CustomerName, record[0].TicketId, "");
                    //if (isEmailSent)
                    //{
                    //    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    //}
                    //else
                    //{
                    //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                    //}
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ReOpenRejectedOrder")]
        public IActionResult ReOpenRejectedOrder(string ticketId)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerOrder>>();
                var record = db.CustomerOrders.Where(x => x.TicketId == ticketId).ToList();
                if (record != null)
                {
                    for (int i = 0; i < record.Count(); i++)
                    {
                        record[i].IsPulled = false;
                        record[i].Delivered = false;
                        record[i].AdminStatus = false;
                        record[i].IsInvoiced = false;
                        record[i].IsRejected = false;
                        record[i].AdminActionBy = null;
                        record[i].AdminActionDate = null;
                        record[i].AdminActionTime = null;
                        record[i].RejectComments = null;
                        record[i].RejectReason = null;
                        db.Entry(record[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsInvoiced == false && x.IsRejected == false
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Pricing>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

    }
}
