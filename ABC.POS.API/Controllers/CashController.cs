using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers   
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public CashController(ABCDiscountsContext _db)
        {
            db = _db;
        }

        // Paying Start
        [HttpGet("PayingGet")]
        public IActionResult PayingGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Paying>>();
                var record = db.Payings.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Paying>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }


        }

        [HttpPost("PayingCreate")]
        public async Task<IActionResult> PayingCreate(Paying obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Paying>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                obj.Date = DateTime.Now;
                db.Payings.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Paying>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PayingUpdate/{id}")]
        public async Task<IActionResult> PayingUpdate(int id, Paying data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Paying>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Payings.ToList().Exists(x => x.InvoiceNumber.Equals(data.InvoiceNumber, StringComparison.CurrentCultureIgnoreCase) && x.PayingId != Convert.ToInt32(id));
                if (isValid)
                {
                    return BadRequest("Paying Already Exists");
                }
                if (id != data.PayingId)
                {
                    return BadRequest();
                }
                var record = await db.Payings.FindAsync(id);
                if (data.InvoiceNumber != null && data.InvoiceNumber != "undefined")
                {
                    record.InvoiceNumber = data.InvoiceNumber;
                }
                if (data.AccountId != null)
                {
                    record.AccountId = data.AccountId;
                }
                if (data.StoreId != null)
                {
                    record.StoreId = data.StoreId;
                }
                if (data.StoreName != null && data.StoreName != "undefined")
                {
                    record.StoreName = data.StoreName;
                }
                if (data.InvoiceTypeId != null)
                {
                    record.InvoiceTypeId = data.InvoiceTypeId;
                }

                if (data.AccountName != null)
                {
                    record.AccountName = data.AccountName;
                }
                if (data.AccountNumber != null && data.AccountNumber != "undefined")
                {
                    record.AccountNumber = data.AccountNumber;
                }

                if (data.Discount != null && data.Discount != "undefined")
                {
                    record.Discount = data.Discount;
                }
                if (data.Tax != null && data.Tax != "undefined")
                {
                    record.Tax = data.Tax;
                }

                if (data.TaxAmount != null && data.TaxAmount != "undefined")
                {
                    record.TaxAmount = data.TaxAmount;
                }

                if (data.PaymentType != null && data.PaymentType != "undefined")
                {
                    record.PaymentType = data.PaymentType;
                }

                if (data.Note != null && data.Note != "undefined")
                {
                    record.Note = data.Note;
                }
                if (data.PayFromAccountId != null)
                {
                    record.PayFromAccountId = data.PayFromAccountId;
                }

                if (data.PayFromAccount != null && data.PayFromAccount != "undefined")
                {
                    record.PayFromAccount = data.PayFromAccount;
                }
                if (data.PayFromAccountNumber != null && data.PayFromAccountNumber != "undefined")
                {
                    record.PayFromAccountNumber = data.PayFromAccountNumber;
                }
                if (data.CheckNumber != null && data.CheckNumber != "undefined")
                {
                    record.CheckNumber = data.CheckNumber;
                }
                if (data.CheckTitle != null && data.CheckTitle != "undefined")
                {
                    record.CheckTitle = data.CheckTitle;
                }
                if (data.CheckDate != null)
                {
                    record.CheckDate = data.CheckDate;
                }
                if (data.CashBalance != null && data.CashBalance != "undefined")
                {
                    record.CashBalance = data.CashBalance;
                }
                if (data.NetAmount != null && data.NetAmount != "undefined")
                {
                    record.NetAmount = data.NetAmount;
                }
                if (data.CashierUser != null && data.CashierUser != "undefined")
                {
                    record.CashierUser = data.CashierUser;
                }
                if (data.Debit != null && data.Debit != "undefined")
                {
                    record.Debit = data.Debit;
                }
                if (data.Credit != null && data.Credit != "undefined")
                {
                    record.Credit = data.Credit;
                }
                if (data.DueDate != null)
                {
                    record.DueDate = data.DueDate;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Paying>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("PayingByID/{id}")]
        public IActionResult PayingByID(int id)
        {
            try
            {
                var record = db.Payings.Find(id);
                if (record != null)
                    return Ok(record);
                else
                    return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePaying/{id}")]
        public async Task<IActionResult> DeletePaying(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Paying>();
                Paying data = await db.Payings.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Payings.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Paying>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Paying End


        // Receiving Start
        [HttpGet("ReceivingGet")]
        public IActionResult ReceivingGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Receiving>>();
                var record = db.Receivings.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ReceivingCreate")]
        public async Task<IActionResult> ReceivingCreate(Receiving obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                obj.Date = DateTime.Now;
                db.Receivings.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("ReceivingUpdate/{id}")]
        public async Task<IActionResult> ReceivingUpdate(int id, Receiving data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //bool isValid = db.Receivings.ToList().Exists(x => x.InvoiceNumber.Equals(data.InvoiceNumber, StringComparison.CurrentCultureIgnoreCase) && x.ReceivingId != Convert.ToInt32(id));
                //if (isValid)
                //{
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                //}
                if (id != data.ReceivingId)
                {
                    return BadRequest();
                }
                var record = await db.Receivings.FindAsync(id);
                if (data.InvoiceNumber != null && data.InvoiceNumber != "undefined")
                {
                    record.InvoiceNumber = data.InvoiceNumber;
                }
                if (data.AccountId != null)
                {
                    record.AccountId = data.AccountId;
                }
                if (data.StoreId != null)
                {
                    record.StoreId = data.StoreId;
                }
                if (data.StoreName != null && data.StoreName != "undefined")
                {
                    record.StoreName = data.StoreName;
                }

                if (data.InvoiceTypeId != null)
                {
                    record.InvoiceTypeId = data.InvoiceTypeId;
                }

                if (data.AccountName != null)
                {
                    record.AccountName = data.AccountName;
                }
                if (data.AccountNumber != null && data.AccountNumber != "undefined")
                {
                    record.AccountNumber = data.AccountNumber;
                }

                if (data.Discount != null && data.Discount != "undefined")
                {
                    record.Discount = data.Discount;
                }
                if (data.Tax != null && data.Tax != "undefined")
                {
                    record.Tax = data.Tax;
                }

                if (data.TaxAmount != null && data.TaxAmount != "undefined")
                {
                    record.TaxAmount = data.TaxAmount;
                }

                if (data.PaymentType != null && data.PaymentType != "undefined")
                {
                    record.PaymentType = data.PaymentType;
                }

                if (data.Note != null && data.Note != "undefined")
                {
                    record.Note = data.Note;
                }
                if (data.PayFromAccountId != null)
                {
                    record.PayFromAccountId = data.PayFromAccountId;
                }

                if (data.PayFromAccount != null && data.PayFromAccount != "undefined")
                {
                    record.PayFromAccount = data.PayFromAccount;
                }
                if (data.PayFromAccountNumber != null && data.PayFromAccountNumber != "undefined")
                {
                    record.PayFromAccountNumber = data.PayFromAccountNumber;
                }
                if (data.CheckNumber != null && data.CheckNumber != "undefined")
                {
                    record.CheckNumber = data.CheckNumber;
                }
                if (data.CheckTitle != null && data.CheckTitle != "undefined")
                {
                    record.CheckTitle = data.CheckTitle;
                }
                if (data.CheckDate != null)
                {
                    record.CheckDate = data.CheckDate;
                }
                if (data.CashBalance != null && data.CashBalance != "undefined")
                {
                    record.CashBalance = data.CashBalance;
                }
                if (data.NetAmount != null && data.NetAmount != "undefined")
                {
                    record.NetAmount = data.NetAmount;
                }
                if (data.CashierUser != null && data.CashierUser != "undefined")
                {
                    record.CashierUser = data.CashierUser;
                }
                if (data.Debit != null && data.Debit != "undefined")
                {
                    record.Debit = data.Debit;
                }
                if (data.Credit != null && data.Credit != "undefined")
                {
                    record.Credit = data.Credit;
                }
                if (data.DueDate != null)
                {
                    record.DueDate = data.DueDate;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }

            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("ReceivingByID/{id}")]
        public IActionResult ReceivingByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Receiving>();

                var record = db.Receivings.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteReceiving/{id}")]
        public async Task<IActionResult> DeleteReceiving(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                Receiving data = await db.Receivings.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.Receivings.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Receiving>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Receiving End


        // PayExpense Start
        [HttpGet("PayExpenseGet")]
        public IActionResult PayExpenseGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PayExpense>>();
                var record = db.PayExpenses.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PayExpenseCreate")]
        public async Task<IActionResult> PayExpenseCreate(PayExpense obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                obj.Date = DateTime.Now;
                db.PayExpenses.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PayExpenseUpdate/{id}")]
        public async Task<IActionResult> PayExpenseUpdate(int id, PayExpense data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //bool isValid = db.Receivings.ToList().Exists(x => x.InvoiceNumber.Equals(data.InvoiceNumber, StringComparison.CurrentCultureIgnoreCase) && x.ReceivingId != Convert.ToInt32(id));
                //if (isValid)
                //{
                //    return BadRequest("Receiving Already Exists");
                //}
                if (id != data.PayExpenseId)
                {
                    return BadRequest();
                }
                var record = await db.PayExpenses.FindAsync(id);
                if (data.InvoiceNumber != null && data.InvoiceNumber != "undefined")
                {
                    record.InvoiceNumber = data.InvoiceNumber;
                }
                if (data.AccountId != null)
                {
                    record.AccountId = data.AccountId;
                }
                if (data.StoreId != null)
                {
                    record.StoreId = data.StoreId;
                }
                if (data.StoreName != null && data.StoreName != "undefined")
                {
                    record.StoreName = data.StoreName;
                }

                if (data.InvoiceTypeId != null)
                {
                    record.InvoiceTypeId = data.InvoiceTypeId;
                }

                if (data.AccountName != null)
                {
                    record.AccountName = data.AccountName;
                }
                if (data.AccountNumber != null && data.AccountNumber != "undefined")
                {
                    record.AccountNumber = data.AccountNumber;
                }

                if (data.Discount != null && data.Discount != "undefined")
                {
                    record.Discount = data.Discount;
                }
                if (data.Tax != null && data.Tax != "undefined")
                {
                    record.Tax = data.Tax;
                }

                if (data.TaxAmount != null && data.TaxAmount != "undefined")
                {
                    record.TaxAmount = data.TaxAmount;
                }

                if (data.PaymentType != null && data.PaymentType != "undefined")
                {
                    record.PaymentType = data.PaymentType;
                }

                if (data.Note != null && data.Note != "undefined")
                {
                    record.Note = data.Note;
                }
                if (data.PayFromAccountId != null)
                {
                    record.PayFromAccountId = data.PayFromAccountId;
                }

                if (data.PayFromAccount != null && data.PayFromAccount != "undefined")
                {
                    record.PayFromAccount = data.PayFromAccount;
                }
                if (data.PayFromAccountNumber != null && data.PayFromAccountNumber != "undefined")
                {
                    record.PayFromAccountNumber = data.PayFromAccountNumber;
                }
                if (data.CheckNumber != null && data.CheckNumber != "undefined")
                {
                    record.CheckNumber = data.CheckNumber;
                }
                if (data.CheckTitle != null && data.CheckTitle != "undefined")
                {
                    record.CheckTitle = data.CheckTitle;
                }
                if (data.CheckDate != null)
                {
                    record.CheckDate = data.CheckDate;
                }
                if (data.CashBalance != null && data.CashBalance != "undefined")
                {
                    record.CashBalance = data.CashBalance;
                }
                if (data.NetAmount != null && data.NetAmount != "undefined")
                {
                    record.NetAmount = data.NetAmount;
                }
                if (data.CashierUser != null && data.CashierUser != "undefined")
                {
                    record.CashierUser = data.CashierUser;
                }
                if (data.Debit != null && data.Debit != "undefined")
                {
                    record.Debit = data.Debit;
                }
                if (data.Credit != null && data.Credit != "undefined")
                {
                    record.Credit = data.Credit;
                }
                if (data.DueDate != null)
                {
                    record.DueDate = data.DueDate;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("PayExpenseByID/{id}")]
        public IActionResult PayExpenseByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PayExpense>();

                var record = db.PayExpenses.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeletePayExpense/{id}")]
        public async Task<IActionResult> DeletePayExpense(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                PayExpense data = await db.PayExpenses.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.PayExpenses.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PayExpense>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //PayExpense End

        //Stock

        //Payables
        [HttpGet("PayableGet")]
        public IActionResult PayableGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Payable>>();
                var record = db.Payables.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Payable>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //payable end

        [HttpGet("ReceivablesGet")]
        public IActionResult GetReceivables()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Receivable>>();
                var record = db.Receivables.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Receivable>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        //Receivable Get
        //Stock Evaluation
        // [HttpGet("STockEvaluationGet")]

        //public IActionResult STockEvaluationGet()
        //{
        //    try
        //    {
        //        var record = db.InventoryStocks.ToList();
        //        //var items = db.Products.ToList();

        //        List<StockEvaluation> liststock = null;
        //        liststock = new List<StockEvaluation>();

        //        StockEvaluation obj = null;


        //        double TotalGross = 0;
        //        double TotalPrice = 0;
        //        double TotalQuantiy = 0;
        //        for (int i = 0; i < record.Count(); i++)
        //        {
        //            obj = new StockEvaluation();
        //            obj.ItemBarCode = record[i].ItemBarCode;
        //            obj.ItemCode = record[i].ItemCode;
        //            obj.ItemName = record[i].ItemName;
        //            obj.Quantity = Convert.ToDouble(record[i].Quantity);
        //            TotalQuantiy += obj.Quantity;
        //            obj.TotalQuantityInHand = TotalQuantiy;
        //            obj.Sku = record[i].Sku;
        //            obj.StockId = record[i].StockId;
        //            obj.ProductId = record[i].ProductId;
        //            var items = db.Products.ToList().Where(x => x.Id == record[i].ProductId).FirstOrDefault();
        //            if (items != null)
        //            {
        //                TotalPrice = Convert.ToDouble(record[i].Quantity) * Convert.ToDouble(items.UnitCharge);

        //                obj.TotalAmount = TotalPrice;
        //                TotalGross += TotalPrice;
        //                obj.GrossAmount = TotalGross;
        //            }
        //            liststock.Add(obj);
        //        }

        //        return Ok(liststock);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("FinanceTransactionGet")]
        public IActionResult FinanceTransactionGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Transaction>>();
                var record = db.Transactions.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Transaction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddPayment")]
        public async Task<IActionResult> AddPayment(List<Payment> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Payment>>();

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                Payment payobj = null;
                payobj = new Payment();
                payobj.Balance = obj[0].Balance;
                payobj.ByUser = obj[0].ByUser;
                payobj.Change = obj[0].Change;
                payobj.CustomerAccountNumber = obj[0].CustomerAccountNumber;
                payobj.CustomerId = obj[0].CustomerId;
                payobj.CustomerName = obj[0].CustomerName;
                payobj.InvoiceNumber = obj[0].InvoiceNumber;
                payobj.IsBalanceToChange = obj[0].IsBalanceToChange;
                payobj.IsEmailCopy = obj[0].IsEmailCopy;
                payobj.IsStandardReceipt = obj[0].IsStandardReceipt;
                payobj.IsTextCopy = obj[0].IsTextCopy;
                payobj.OrderBy = obj[0].OrderBy;
                payobj.PaymentDate = obj[0].PaymentDate;
                payobj.TotalAlloc = obj[0].TotalAlloc;
                payobj.TotalBill = obj[0].TotalBill;
                payobj.TotalPaid = obj[0].TotalPaid;
                var CurrentPayment = db.Payments.Add(payobj);
                await db.SaveChangesAsync();
                PaymentDetail paymentDetail = null;
                if (obj != null && obj.Count > 0)
                {
                    for (int t = 0; t < obj.Count(); t++)
                    {
                        paymentDetail = new PaymentDetail();
                        paymentDetail.AmountAlloc = obj[t].paymentDetail.AmountAlloc;
                        paymentDetail.PaymentType = obj[t].paymentDetail.PaymentType;
                        paymentDetail.AmountPaid = obj[t].paymentDetail.AmountPaid;
                        paymentDetail.CkcardNumber = obj[t].paymentDetail.CkcardNumber;
                        paymentDetail.HoldDate = obj[t].paymentDetail.HoldDate;
                        paymentDetail.PaymentDate = obj[t].paymentDetail.PaymentDate;
                        if (CurrentPayment.Entity.PaymentId > 0)
                        {
                            paymentDetail.PaymentId = CurrentPayment.Entity.PaymentId;
                        }

                        if (CurrentPayment.Entity.InvoiceNumber != null && CurrentPayment.Entity.InvoiceNumber != "undefined")
                        {
                            paymentDetail.InvoiceNumber = CurrentPayment.Entity.InvoiceNumber;
                        }

                        await db.PaymentDetails.AddAsync(paymentDetail);
                        await db.SaveChangesAsync();
                        return Ok(Response);
                    }
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PosSale>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }




        }
    }
}
