using ABC.EFCore.Repository.Edmx;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IMailService mailService;
        public SaleController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
            this.mailService = mailService;
        }
        [HttpPost("Send")]
        public async Task<string> Send(MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return ("Email Sent Successfully");
            }
            catch (Exception ex)
            {

                return (ex.Message.ToString());
            }

        }
        [HttpGet("SaleGet")]
        public IActionResult SaleGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].SupervisorId != null)
                    {
                        var GetSupervisors = db.Supervisors.Find(record[i].SupervisorId);
                        if (GetSupervisors != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSupervisors.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSupervisors.AspNetUser = getCurrentUser;
                            }
                            record[i].Supervisor = GetSupervisors;
                        }
                    }

                    if (record[i].SalesManagerId != null)
                    {
                        var GetSalesManager = db.SalesManagers.Find(record[i].SalesManagerId);
                        if (GetSalesManager != null)
                        {
                            var getCurrentUser = db.AspNetUsers.Find(GetSalesManager.UserId);
                            if (getCurrentUser != null)
                            {
                                GetSalesManager.AspNetUser = getCurrentUser;
                            }
                            record[i].SalesManager = GetSalesManager;

                        }
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Sale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("MailToCustomer")]
        public async Task<IActionResult> MailtoCustomer(MailRequest obj)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<MailRequest>();
                var emailresponse = await Send(obj);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<MailRequest>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAllSttax")]
        public IActionResult GetAllSttax()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Sttax>>();
                var record = db.Sttaxes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<List<Sttax>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //[HttpPost("CheckSupervisorCreditAmount/{customerId}")]
        //public IActionResult CheckSupervisorCredit(int? customerId, Supervisor supervisor)
        //{
        //    try
        //    {
        //        var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
        //        var record = db.Supervisors.Where(x => x.AccessPin == supervisor.AccessPin)?.FirstOrDefault();
        //        if (record != null)
        //        {
        //            if (double.Parse(supervisor.CreditLimit) <= double.Parse(record.CreditLimit))
        //            {
        //                var checkCredit = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == customerId && x.PaymentStatus == false).ToList();
        //                //DateTime dt = DateTime.Now.Date.AddDays(-7);
        //                if (checkCredit.Count() >= 1)
        //                {
        //                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, record);
        //                    return Ok(Response);
        //                }
        //                DateTime dt = DateTime.Now.Date.AddDays(-13);
        //                var creditAmount = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId).ToList();
        //                var totalCreditAmount = 0.00;
        //                if (creditAmount != null)
        //                {
        //                    for (int i = 0; i < creditAmount.Count(); i++)
        //                    {
        //                        var creditDate = creditAmount[i].CreditDate;
        //                        if(creditDate >= dt)
        //                        {
        //                             totalCreditAmount = totalCreditAmount + double.Parse(creditAmount[i].CreditAmount);
        //                        }
        //                    }
        //                }
        //                if(totalCreditAmount > double.Parse(record.CreditLimit))
        //                {
        //                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, record);
        //                    return Ok(Response);
        //                }
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
        //            }
        //            else
        //            {
        //                ResponseBuilder.SetWSResponse(Response, StatusCodes.Exceed_Credit_Limit, null, record);
        //            }
        //        }
        //        else
        //        {
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, record);
        //        }
        //        return Ok(Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
        //        {
        //            var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
        //            ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
        //            return Ok(Response);
        //        }
        //        return BadRequest(ex.Message);
        //    }
        //}



        [HttpPost("CheckSupervisorCreditAmount/{customerId}")]
        public IActionResult CheckSupervisorCredit(int? customerId, Supervisor supervisor)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var customerdata = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
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
                            var GetCustomer = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
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
                                        if (Convert.ToDateTime(getCustomerOrder.DueDate).Date >= DateTime.Now.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == false)
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
                                        else if (Convert.ToDateTime(getCustomerOrder.DueDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == false)
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





        [HttpPost("CheckSupervisorCreditAmountforUpdate/{customerId}/{possaleinvoive}")]
        public IActionResult CheckSupervisorCreditforUpdate(int? customerId, Supervisor supervisor, string possaleinvoive)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var customerdata = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
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
                            var GetCustomer = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
                            DateTime dt = DateTime.Now.Date.AddDays(-13);
                            var creditAmount = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == GetCustomer.Id && x.SaleId == possaleinvoive).ToList();
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
                                        if (Convert.ToDateTime(getCustomerOrder.DueDate).Date >= DateTime.Now.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == false)
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
                                        else if (Convert.ToDateTime(getCustomerOrder.DueDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == false)
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



        [HttpPost("posCheckSupervisorCreditAmount/{customerId}")]
        public IActionResult posCheckSupervisorCreditAmount(int? customerId, Supervisor supervisor)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var customerdata = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
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
                            var GetCustomer = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
                            DateTime dt = DateTime.Now.Date.AddDays(-13);
                            var creditAmount = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == GetCustomer.Id).ToList();
                            double totalCreditAmount = 0;
                            if (creditAmount != null)
                            {
                                for (int i = 0; i < creditAmount.Count(); i++)
                                {
                                    var getCustomerOrder = db.PosSales.ToList().GroupBy(x => x.InvoiceNumber).Select(i => i.FirstOrDefault()).Where(x => x.CustomerId == creditAmount[i].CustomerId && x.IsPaid == false || x.IsPaid == null).LastOrDefault();
                                    if (getCustomerOrder != null)
                                    {
                                        var a = Convert.ToDateTime(getCustomerOrder.InvoiceDate).Date;
                                        var aa = dtSeven.Date;
                                        var aaa = DateTime.Now.Date;
                                        if (Convert.ToDateTime(getCustomerOrder.InvoiceDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == null)
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
                                        else if (Convert.ToDateTime(getCustomerOrder.InvoiceDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == null)
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


        [HttpPost("CheckSupervisorCreditAmountforUpdate/{customerId}/{possaleinvoive}")]
        public IActionResult PosCheckSupervisorCreditforUpdate(int? customerId, Supervisor supervisor, string possaleinvoive)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var customerdata = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
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
                            var GetCustomer = db.CustomerInformations.ToList().Where(x => x.Id == customerId).FirstOrDefault();
                            DateTime dt = DateTime.Now.Date.AddDays(-13);
                            var creditAmount = db.SupervisorCredits.Where(x => x.SupervisorId == record.SupervisorId && x.CustomerId == GetCustomer.Id && x.SaleId == possaleinvoive).ToList();
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
                                        if (Convert.ToDateTime(getCustomerOrder.DueDate).Date >= DateTime.Now.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == null)
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
                                        else if (Convert.ToDateTime(getCustomerOrder.DueDate).Date <= dtSeven.Date && getCustomerOrder.IsPaid == false || getCustomerOrder.IsPaid == null)
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

        [HttpGet("PosSaleByInvoiceNumber1/{invoicenumber}/{method}")]
        public IActionResult PosSaleByInvoiceNumber1(string invoicenumber, string method)
        {
            try
            {
                var response = ResponseBuilder.BuildWSResponse<PointOfSaleModel>();
                PointOfSale record = db.PointOfSales.Where(x => x.InvoiceNumber == invoicenumber).FirstOrDefault();

                PointOfSaleModel obj = new PointOfSaleModel();
                obj.PointOfSaleId = record.PointOfSaleId;
                obj.InvoiceNumber = record.InvoiceNumber;
                obj.CustomerId = record.CustomerId;
                obj.Other = record.Other;
                obj.Tax = record.Tax;
                obj.IsPaid = record.IsPaid;
                obj.IsOpen = record.IsOpen;
                obj.IsClose = record.IsClose;
                obj.Count = Convert.ToString(record.Count);
                obj.SubTotal = record.SubTotal;

                if (record != null)
                {
                    record.PointOfSaleDetails = db.PointOfSaleDetails.Where(x => x.PointOfSaleId == record.PointOfSaleId).ToList();
                    if (record.PointOfSaleDetails.Count > 0)
                    {
                        var query = (
                            from itemsdetails in db.PointOfSaleDetails
                            join P in db.Products on itemsdetails.ItemId equals P.Id into result
                            from jrresult in result.DefaultIfEmpty()
                            join InvS in db.InventoryStocks on itemsdetails.ItemId equals InvS.ProductId into InvStockResult
                            from jrresult1 in InvStockResult.DefaultIfEmpty()
                            where itemsdetails.PointOfSaleId == record.PointOfSaleId
                            select new
                            {
                                itemsdetails.PointOfSaleId,
                                itemsdetails.PosSaleDetailId,
                                itemsdetails.ItemId,
                                itemsdetails.InDiscount,
                                itemsdetails.OutDiscount,
                                itemsdetails.Quantity,
                                itemsdetails.InUnit,
                                itemsdetails.OutUnit,
                                itemsdetails.Price,
                                itemsdetails.Total,
                                itemsdetails.RingerQty,
                                Description = jrresult.Description,
                                ItemNumber = jrresult.ItemNumber,
                                AmountRetail = jrresult.SaleRetail,
                                SalesLimit = jrresult.SalesLimit,
                                NeedHighAuthorization = jrresult.NeedHighAuthorization,
                                HighLimitOn = jrresult.HighlimitOn,
                                StockQty = jrresult1.Quantity
                            }
                            ).ToList();

                        foreach( var item in query )
                        {
                            PointOfSaleDetailModel pointOfSaleDetailModel = new PointOfSaleDetailModel();
                            pointOfSaleDetailModel.PointOfSaleId = Convert.ToString(item.PointOfSaleId);
                            pointOfSaleDetailModel.ItemId = Convert.ToString(item.ItemId);
                            pointOfSaleDetailModel.InDiscount = item.InDiscount;
                            pointOfSaleDetailModel.OutDiscount = item.OutDiscount;
                            pointOfSaleDetailModel.Quantity = item.Quantity;
                            pointOfSaleDetailModel.InUnit = item.InUnit;
                            pointOfSaleDetailModel.OutUnit = item.OutUnit;
                            pointOfSaleDetailModel.Price = item.Price;
                            pointOfSaleDetailModel.Total = item.Total;
                            pointOfSaleDetailModel.RingerQty = item.RingerQty;
                            pointOfSaleDetailModel.Description = item.Description;
                            pointOfSaleDetailModel.ItemNumber = item.ItemNumber;
                            pointOfSaleDetailModel.AmountRetail = item.AmountRetail;
                            pointOfSaleDetailModel.SalesLimit = item.SalesLimit;
                            pointOfSaleDetailModel.NeedHighAuthorization = item.NeedHighAuthorization;
                            pointOfSaleDetailModel.HighLimitOn = item.HighLimitOn;
                            pointOfSaleDetailModel.StockQty = item.StockQty;

                            obj.PointOfSaleDetails.Add( pointOfSaleDetailModel );
                        }

                        var receiving = db.Receivings.Where(f => f.InvoiceNumber == invoicenumber).FirstOrDefault();
                        if(receiving != null)
                        {
                            obj.InvoiceBalance = receiving.InvBalance;
                            obj.PreBalance = receiving.PreBalance;
                        }
                    }
                }
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(response, StatusCodes.SUCCESS_CODE, null, obj);
                    return Ok(response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var response = ResponseBuilder.BuildWSResponse<PointOfSale>();
                    ResponseBuilder.SetWSResponse(response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PosSaleByInvoiceNumber/{invoicenumber}/{method}")]
        public IActionResult PosSaleByInvoiceNumber(string invoicenumber, string method)
        {
            try
            {
                var response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                var record = db.PosSales.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                foreach (var sales in record)
                {
                    var itemdata = db.Products.Where(x => x.Id == sales.ItemId).FirstOrDefault();
                    sales.product = itemdata;
                    var itemstock = db.InventoryStocks.Where(x => x.ProductId == sales.ItemId).FirstOrDefault();
                    sales.stock = itemstock;

                }
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var response = ResponseBuilder.BuildWSResponse<List<PosSale>>();
                    ResponseBuilder.SetWSResponse(response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CashierCounters/{id}")]
        public IActionResult CashierCounters(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CashierCounters>();
                var OnlineOrdersCount = db.CustomerOrders.ToList().Where(x => x.InvoiceEmployeeId == id && x.IsInvoiced == false).GroupBy(x => x.TicketId).Count();
                CashierCounters objectResponse = new CashierCounters();
                objectResponse.OnlineOrders = OnlineOrdersCount;
                var SalesInvoices = db.PosSales.ToList().Where(x => x.SalesmanId == id && x.InvoiceDate.Value == DateTime.Now.Date).GroupBy(x => x.InvoiceNumber).Count();
                objectResponse.TodaysSale = SalesInvoices;
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, objectResponse);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CashierCounters>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
