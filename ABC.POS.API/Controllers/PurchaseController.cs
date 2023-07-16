using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
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
    public class PurchaseController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public PurchaseController(ABCDiscountsContext _db)
        {
            db = _db;
        }

        [HttpGet("PurchaseGet")]
        public IActionResult PurchaseGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Purchase>>();
                var record = db.Purchases.ToList();
                var allsuppliers = db.Vendors.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].VendorId).ToList())
                    {
                        record[i].VendorName = item.FullName;
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Purchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PurchaseCreate")]
        public async Task<IActionResult> PurchaseCreate(List<Purchase> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Purchase>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                InventoryStock stock = null;
                double grossamount = 0;
                for (int a = 0; a < obj.Count(); a++)
                {
                    grossamount += Convert.ToDouble(obj[a].TotalAmount);
                }
                for (int i = 0; i < obj.Count(); i++)
                {
                    stock = new InventoryStock();
                    obj[i].GrossAmount = grossamount.ToString();
                    obj[i].PurchaseDate = DateTime.Now;

                    var getstores = db.Stores.ToList().Where(x => x.Id == obj[i].StoreId).FirstOrDefault();
                    if (getstores != null)
                    {

                        obj[i].StoreName = getstores.StoreName;
                    }
                    var getwarehouse = db.WareHouses.ToList().Where(x => x.WareHouseId == obj[i].WareHouseId).FirstOrDefault();
                    if (getwarehouse != null)
                    {
                        obj[i].WareHouseName = getwarehouse.WareHouseName;
                    }
                    db.Purchases.Add(obj[i]);
                    db.SaveChanges();
                    var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                    if (getstock != null)
                    {
                        getstock.Quantity = (Convert.ToDouble(getstock.Quantity) + Convert.ToDouble(obj[i].Quantity)).ToString();
                        db.Entry(getstock).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        stock.ProductId = obj[i].ItemId;
                        stock.ItemCode = obj[i].ProductCode;
                        stock.ItemName = obj[i].ItemName;
                        stock.ItemBarCode = obj[i].ProductBarCode;
                        stock.Quantity = obj[i].Quantity;
                        stock.Sku = obj[i].Sku;
                        db.InventoryStocks.Add(stock);
                        db.SaveChanges();
                    }



                }

                var getvendor = db.Vendors.Find(obj[0].VendorId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();

                    var getFGaccount = db.Accounts.ToList().Where(a => a.Title == "Finish Goods").FirstOrDefault();
                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                    for (int i = 0; i < 2; i++)
                    {
                        Transaction transaction = null;
                        transaction = new Transaction();
                        if (i == 0)
                        {
                            transaction.AccountName = getaccount.Title;
                            transaction.AccountNumber = getaccount.AccountId;
                            transaction.DetailAccountId = getaccount.AccountId;
                            transaction.Credit = grossamount.ToString();
                            transaction.Debit = "0.00";
                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                            transaction.Date = DateTime.Now;
                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                            db.Transactions.Add(transaction);
                            db.SaveChanges();

                        }
                        else
                        {
                            transaction.AccountName = getFGaccount.Title;
                            transaction.AccountNumber = getFGaccount.AccountId;
                            transaction.DetailAccountId = getFGaccount.AccountId;
                            transaction.Credit = "0.00";
                            transaction.Debit = grossamount.ToString();
                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                            transaction.Date = DateTime.Now;
                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                            db.Transactions.Add(transaction);
                            db.SaveChanges();
                        }
                    }



                    Payable pay = null;
                    if (obj[0].Cash == false)
                    {
                        pay = new Payable();
                        if (getaccount != null)
                        {
                            var getpay = db.Payables.ToList().Where(x => x.AccountId == getaccount.AccountId).FirstOrDefault();
                            if (getpay != null)
                            {
                                getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(grossamount)).ToString();
                                db.Entry(getpay).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                            else
                            {
                                pay.AccountId = getaccount.AccountId;
                                pay.AccountNumber = getaccount.AccountId;
                                pay.Amount = grossamount.ToString();
                                pay.AccountName = getaccount.Title;
                                db.Payables.Add(pay);
                                db.SaveChanges();
                            }
                        }

                    }
                    else
                    {

                        if (getFGaccount != null)
                        {

                            var fullcode = "";
                            Paying newitems = new Paying();
                            var recordemp = db.Payings.ToList();
                            if (recordemp.Count() > 0)
                            {
                                if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
                                {
                                    int large, small;
                                    int salesID = 0;
                                    large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                    small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                    for (int i = 0; i < recordemp.Count; i++)
                                    {
                                        if (recordemp[i].InvoiceNumber != null)
                                        {
                                            var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                            if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
                                            {
                                                salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

                                            }
                                            else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
                                            {
                                                small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                            }
                                            else
                                            {
                                                if (large < 2)
                                                {
                                                    salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                }
                                            }
                                        }
                                    }
                                    newitems = recordemp.ToList().Where(x => x.PayingId == salesID).FirstOrDefault();
                                    if (newitems != null)
                                    {
                                        if (newitems.InvoiceNumber != null)
                                        {
                                            var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                            int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                            fullcode = "CP00" + "-" + Convert.ToString(code);
                                        }
                                        else
                                        {
                                            fullcode = "CP00" + "-" + "1";
                                        }
                                    }
                                    else
                                    {
                                        fullcode = "CP00" + "-" + "1";
                                    }
                                }
                                else
                                {
                                    fullcode = "CP00" + "-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "CP00" + "-" + "1";
                            }

                            Paying paying = null;
                            paying = new Paying();

                            //  paying.AccountId = getFGaccount.AccountId;
                            paying.AccountName = getFGaccount.Title;
                            paying.AccountNumber = getFGaccount.AccountId;
                            paying.CashBalance = grossamount.ToString();
                            paying.StoreId = obj[0].StoreId;
                            var getstore = db.Stores.ToList().Where(x => x.Id == obj[0].StoreId).FirstOrDefault();
                            if (getstore != null)
                            {

                                obj[0].StoreName = getstore.StoreName;
                            }
                            var getwarehouse = db.WareHouses.ToList().Where(x => x.WareHouseId == obj[0].WareHouseId).FirstOrDefault();
                            if (getwarehouse != null)
                            {
                                obj[0].WareHouseName = getwarehouse.WareHouseName;
                            }
                            // paying.StoreName =    ;
                            paying.Tax = obj[0].VatTax;
                            paying.TaxAmount = obj[0].VatTax;
                            paying.PaymentType = "Cash";
                            paying.PayFromAccountNumber = getCHaccount.AccountId;
                            paying.PayFromAccountId = getCHaccount.AccountId;
                            paying.PayFromAccount = getCHaccount.Title;
                            paying.Note = "";
                            paying.NetAmount = grossamount.ToString();
                            paying.InvoiceNumber = fullcode;
                            paying.Debit = grossamount.ToString();
                            paying.Credit = "0.00";
                            //  paying.Date = null;
                            db.Payings.Add(paying);
                            await db.SaveChangesAsync();
                            for (int i = 0; i < 2; i++)
                            {
                                Transaction transaction = null;
                                transaction = new Transaction();
                                if (i == 0)
                                {
                                    transaction.AccountName = getaccount.Title;
                                    transaction.AccountNumber = getaccount.AccountId;
                                    transaction.DetailAccountId = getaccount.AccountId;
                                    transaction.Credit = "0.00";
                                    transaction.Debit = grossamount.ToString();
                                    transaction.InvoiceNumber = fullcode;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    transaction.AccountName = getCHaccount.Title;
                                    transaction.AccountNumber = getCHaccount.AccountId;
                                    transaction.DetailAccountId = getCHaccount.AccountId;
                                    transaction.Credit = grossamount.ToString();
                                    transaction.Debit = "0.00";
                                    transaction.InvoiceNumber = fullcode;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();
                                }
                            }

                        }
                    }
                }


                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Purchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("PurchaseByID/{invoicenumber}")]
        public IActionResult PurchaseByID(string invoicenumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Purchase>>();
                var record = db.Purchases.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                if (record != null)
                {
                    var allsuppliers = db.Vendors.ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].VendorId).ToList())
                        {
                            record[i].VendorName = item.FullName;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Purchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PurchaseByproductID/{id}")]
        public IActionResult PurchaseByproductID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.Where(x => x.ItemId == id).ToList();
                if (record != null)
                {
                    var allsuppliers = db.Vendors.ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].SupplierId).ToList())
                        {
                            record[i].SupplierName = item.FullName;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        //Purchase Order New Create
        [HttpGet("PurchaseOrderGet")]
        public IActionResult PurchaseOrderGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.ToList();
                var allsuppliers = db.Vendors.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].SupplierId).ToList())
                    {
                        record[i].SupplierName = item.FullName;
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("PurchaseOrderCreate")]
        public async Task<IActionResult> PurchaseOrderCreate(List<PurchaseOrder> obj)        
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var CurrentInvoice = db.PurchaseOrders.Any(x => x.InvoiceNumber == obj[0].InvoiceNumber);
                if (CurrentInvoice)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                InventoryStock stock = null;
                double grossamount = 0;

                for (int a = 0; a < obj.Count(); a++)
                {
                    grossamount += Convert.ToDouble(obj[a].Amount);
                }
                for (int i = 0; i < obj.Count(); i++)
                {
                    bool firstTime = false;
                    stock = new InventoryStock();
                    obj[i].GrossAmount = grossamount.ToString();
                    obj[i].Podate = DateTime.Now;
                    obj[i].InvoiceDate = DateTime.Now;
                    obj[i].DateReceived = DateTime.Now;
                    if (obj[i].ItemId != null)
                    {
                        var getproductsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                        if (getproductsdetails != null)
                        {
                            stock.ItemBarCode = getproductsdetails.BarCode;
                            stock.ItemCode = getproductsdetails.ItemNumber;
                            stock.Sku = getproductsdetails.Sku;
                        }
                        stock.ProductId = obj[i].ItemId;
                        stock.ItemName = obj[i].Description;
                        stock.Quantity = obj[i].Qty;
                    }


                    if (obj[0].Received == true)
                    {
                        obj[i].IsPostStatus = "true";
                        obj[i].Received = obj[0].Received;
                    }
                    else
                    {
                        obj[i].IsPostStatus = "false";
                        obj[i].Received = obj[0].Received;
                    }
                    if (obj[0].IsPaid == true)
                    {
                        obj[i].PaidDate = DateTime.Now;
                        obj[i].IsPaid = obj[0].IsPaid;
                    }
                    //var getuser= User.Identity.Name.ToString();
                    //obj[i].Currrentuser = getuser;
                    var SimpleInvoiceNumber = obj[i].InvoiceNumber.Replace("-", "");
                    obj[i].SimpleInvoiceNumber = SimpleInvoiceNumber;
                    var newPOrder = db.PurchaseOrders.Add(obj[i]);

                    var productdetail = db.Products.Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                    if (productdetail.Cost != obj[i].Price)
                    {
                        obj[i].ItemCode = productdetail.ProductCode;
                        var itemcostchange = new ItemCostChange();
                        itemcostchange.ItemId = obj[i].ItemId;
                        itemcostchange.ItemName = obj[i].Description;
                        itemcostchange.PoInvoiceNumber = obj[i].InvoiceNumber;
                        itemcostchange.OldPrice = productdetail.Cost;
                        itemcostchange.NewPrice = obj[i].Price;
                        itemcostchange.PriceDiff = (float.Parse(productdetail.Cost) - float.Parse(obj[i].Price)).ToString();
                        itemcostchange.PricePercentage = ((float.Parse(itemcostchange.PriceDiff) / float.Parse(productdetail.Cost)) * 100).ToString();
                        itemcostchange.CreatedDate = DateTime.Now;
                        itemcostchange.NewPrice = obj[i].Price;
                        itemcostchange.CretedBy = obj[i].Currrentuser;
                        db.ItemCostChanges.Add(itemcostchange);

                        productdetail.OldPrice = productdetail.Cost;
                        productdetail.Cost = obj[i].Price;
                        productdetail.PriceDiff = itemcostchange.PriceDiff;
                        productdetail.PriceDiffPercentage = itemcostchange.PricePercentage;
                        db.Entry(productdetail).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    if (obj[0].Received == true)
                    {
                        var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                        if (getstock != null)
                        {
                            getstock.Quantity = (Convert.ToDouble(getstock.Quantity) + Convert.ToDouble(obj[i].Qty)).ToString();
                            db.Entry(getstock).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            InventoryStock stocknum = new InventoryStock();
                            var productsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                            if (productsdetails != null)
                            {
                                stock.ItemBarCode = productsdetails.BarCode;
                                stock.ItemCode = productsdetails.ItemNumber;
                                stock.Sku = productsdetails.Sku;
                            }
                            stock.ProductId = obj[i].ItemId;
                            stock.ItemName = obj[i].Description;
                            stock.Quantity = obj[i].Qty;
                            //stock.StockItemNumber = fullcode;
                            stock.StockItemNumber = obj[i].StockItemNumber;

                            db.InventoryStocks.Add(stock);
                            db.SaveChanges();
                            firstTime = true;
                            var updateitem = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                            if (updateitem != null)
                            {

                                //updateitem.StockItemNumber = fullcode;
                                updateitem.StockItemNumber = obj[i].ItemCode;
                                db.Entry(updateitem).State = EntityState.Modified;
                            }
                            if (firstTime == true)
                            {
                                var foundPOrder = db.PurchaseOrders.Find(Convert.ToInt32(newPOrder.Entity.PurchaseorderId));
                                if (foundPOrder != null)
                                {
                                    // foundPOrder.StockItemNumber = fullcode;
                                    foundPOrder.StockItemNumber = obj[i].ItemCode;
                                    db.Entry(foundPOrder).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                        }
                    }

                    if (obj[i].SupplierItemNumber != "")
                    {
                        var suplieritem = db.SupplierItemNumbers.ToList().Where(x => x.VendorId == obj[i].SupplierId && x.ProductId == obj[i].ItemId && x.SupplierItemNum.Equals(obj[i].SupplierItemNumber, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (suplieritem == null)
                        {
                            var supplieritemnum = new SupplierItemNumber();
                            supplieritemnum.ProductId = obj[i].ItemId;
                            supplieritemnum.VendorId = obj[i].SupplierId;
                            supplieritemnum.SupplierItemNum = obj[i].SupplierItemNumber;
                            supplieritemnum.CreatedDate = DateTime.Now;
                            db.SupplierItemNumbers.Add(supplieritemnum);
                            db.SaveChanges();

                        }
                    }

                }
                if (obj[0].PaymentTerms == "Cheque")
                {
                    obj[0].IsPaid = true;
                }
                var getvendor = db.Vendors.Find(obj[0].SupplierId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();

                    if (getaccount != null)
                    {
                        if (obj[0].IsPaid == false || obj[0].IsPaid == null)
                        {
                            var getFGaccount = db.Accounts.ToList().Where(a => a.Title == "Finish Goods").FirstOrDefault();
                            //Payables 
                            Payable pay = null;
                            pay = new Payable();
                            if (getaccount != null)
                            {
                                var getpay = db.Payables.ToList().Where(x => x.AccountId == getaccount.AccountId).FirstOrDefault();
                                if (getpay != null)
                                {
                                    getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(obj[0].GrossAmount)).ToString();
                                    db.Entry(getpay).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                else
                                {
                                    pay.AccountId = getaccount.AccountId;
                                    pay.AccountNumber = getaccount.AccountId;
                                    pay.Amount = obj[0].GrossAmount.ToString();
                                    pay.AccountName = getaccount.Title;
                                    db.Payables.Add(pay);
                                    db.SaveChanges();
                                }
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                Transaction transaction = null;
                                transaction = new Transaction();
                                if (i == 0)
                                {
                                    if (getaccount != null)
                                    {
                                        if (getaccount.Title != null)
                                        {
                                            transaction.AccountName = getaccount.Title;
                                        }
                                        if (getaccount.AccountId != null)
                                        {
                                            transaction.AccountNumber = getaccount.AccountId;
                                        }
                                        if (getaccount.AccountId != null)
                                        {
                                            transaction.DetailAccountId = getaccount.AccountId;
                                        }
                                    }

                                    transaction.Credit = obj[0].GrossAmount.ToString();
                                    transaction.Debit = "0.00";
                                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    transaction.AccountName = getFGaccount.Title;
                                    transaction.AccountNumber = getFGaccount.AccountId;
                                    transaction.DetailAccountId = getFGaccount.AccountId;
                                    transaction.Credit = "0.00";
                                    transaction.Debit = obj[0].GrossAmount.ToString();
                                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();
                                }
                            }

                        }
                        else
                        {
                            var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                            if (getCHaccount != null)
                            {
                                var fullcode = "";
                                Paying newitems = new Paying();
                                var recordemp = db.Payings.ToList();
                                if (recordemp.Count() > 0)
                                {
                                    if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
                                    {
                                        int large, small;
                                        int salesID = 0;
                                        large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                        small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                        for (int i = 0; i < recordemp.Count; i++)
                                        {
                                            if (recordemp[i].InvoiceNumber != null)
                                            {
                                                var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
                                                {
                                                    salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                    large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

                                                }
                                                else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
                                                {
                                                    small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                }
                                                else
                                                {
                                                    if (large < 2)
                                                    {
                                                        salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                    }
                                                }
                                            }
                                        }
                                        newitems = recordemp.ToList().Where(x => x.PayingId == salesID).FirstOrDefault();
                                        if (newitems != null)
                                        {
                                            if (newitems.InvoiceNumber != null)
                                            {
                                                var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                                fullcode = "CP00" + "-" + Convert.ToString(code);
                                            }
                                            else
                                            {
                                                fullcode = "CP00" + "-" + "1";
                                            }
                                        }
                                        else
                                        {
                                            fullcode = "CP00" + "-" + "1";
                                        }
                                    }
                                    else
                                    {
                                        fullcode = "CP00" + "-" + "1";
                                    }
                                }
                                else
                                {
                                    fullcode = "CP00" + "-" + "1";
                                }

                                Paying paying = null;
                                paying = new Paying();
                                paying.AccountName = getaccount.Title;
                                paying.AccountNumber = getaccount.AccountId;
                                paying.AccountId = getaccount.AccountId;

                                if (obj[0].PaymentTerms == "Cheque")
                                {
                                    paying.CheckDate = obj[0].CheckDate;
                                    paying.CheckNumber = obj[0].CheckNumber;
                                    paying.CheckTitle = obj[0].CheckTitle;
                                    paying.PaymentType = "Wire";

                                }
                                else
                                {
                                    paying.PaymentType = "Cash";
                                }
                                paying.Note = "";
                                paying.InvoiceNumber = fullcode;
                                paying.Debit = obj[0].GrossAmount.ToString();
                                paying.Credit = "0.00";
                                paying.Date = DateTime.Now;
                                db.Payings.Add(paying);
                                await db.SaveChangesAsync();


                                for (int i = 0; i < 2; i++)
                                {
                                    Transaction transaction = null;
                                    transaction = new Transaction();
                                    if (i == 0)
                                    {
                                        if (getaccount != null)
                                        {
                                            transaction.AccountName = getaccount.Title;
                                            transaction.AccountNumber = getaccount.AccountId;
                                            transaction.DetailAccountId = getaccount.AccountId;
                                            transaction.Credit = "0.00";
                                            transaction.Debit = obj[0].GrossAmount.ToString();
                                            transaction.InvoiceNumber = fullcode;
                                            transaction.Date = DateTime.Now;
                                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                            db.Transactions.Add(transaction);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        transaction.AccountName = getCHaccount.Title;
                                        transaction.AccountNumber = getCHaccount.AccountId;
                                        transaction.DetailAccountId = getCHaccount.AccountId;
                                        transaction.Credit = obj[0].GrossAmount.ToString();
                                        transaction.Debit = "0.00";
                                        transaction.InvoiceNumber = fullcode;
                                        transaction.Date = DateTime.Now;
                                        transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                        db.Transactions.Add(transaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Account objacount = null;
                        objacount = new Account();

                        Account customeracc = null;
                        customeracc = new Account();
                        var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Customers").FirstOrDefault();
                        if (subaccrecord != null)
                        {
                            var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == subaccrecord.AccountSubGroupId).LastOrDefault();
                            if (getAccount != null)
                            {
                                var code = getAccount.AccountId.Split("-")[3];
                                int getcode = 0;
                                if (code != null)
                                {

                                    getcode = Convert.ToInt32(code) + 1;
                                }
                                if (getcode > 9)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);

                                }
                                else if (getcode > 99)
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);
                                }
                                else
                                {
                                    objacount.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                                }
                                objacount.Title = obj[0].SupplierName;
                                objacount.Status = 1;
                                objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objacount).Entity;
                                db.SaveChanges();
                            }
                            else
                            {
                                objacount.AccountId = subaccrecord.AccountSubGroupId + "-0001";
                                objacount.Title = obj[0].SupplierName;
                                objacount.Status = 1;
                                objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                                customeracc = db.Accounts.Add(objacount).Entity;
                                db.SaveChanges();
                            }
                            if (customeracc != null)
                            {
                                var foundPOrder = db.Vendors.Find(obj[0].SupplierId);
                                if (foundPOrder != null)
                                {
                                    // foundPOrder.StockItemNumber = fullcode;
                                    foundPOrder.AccountId = customeracc.AccountId;
                                    foundPOrder.AccountNumber = customeracc.AccountId;
                                    foundPOrder.AccountTitle = customeracc.Title;
                                    db.Entry(foundPOrder).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                if (obj[0].IsPaid == false || obj[0].IsPaid == null)
                                {
                                    var getFGaccount = db.Accounts.ToList().Where(a => a.Title == "Finish Goods").FirstOrDefault();
                                    //Payables 
                                    Payable pay = null;
                                    pay = new Payable();
                                    if (customeracc != null)
                                    {
                                        var getpay = db.Payables.ToList().Where(x => x.AccountId == customeracc.AccountId).FirstOrDefault();
                                        if (getpay != null)
                                        {
                                            getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(obj[0].GrossAmount)).ToString();
                                            db.Entry(getpay).State = EntityState.Modified;
                                            db.SaveChanges();

                                        }
                                        else
                                        {
                                            pay.AccountId = customeracc.AccountId;
                                            pay.AccountNumber = customeracc.AccountId;
                                            pay.Amount = obj[0].GrossAmount.ToString();
                                            pay.AccountName = customeracc.Title;
                                            db.Payables.Add(pay);
                                            db.SaveChanges();
                                        }
                                    }

                                    for (int i = 0; i < 2; i++)
                                    {
                                        Transaction transaction = null;
                                        transaction = new Transaction();
                                        if (i == 0)
                                        {
                                            if (customeracc != null)
                                            {
                                                if (customeracc.Title != null)
                                                {
                                                    transaction.AccountName = customeracc.Title;
                                                }
                                                if (customeracc.AccountId != null)
                                                {
                                                    transaction.AccountNumber = customeracc.AccountId;
                                                }
                                                if (customeracc.AccountId != null)
                                                {
                                                    transaction.DetailAccountId = customeracc.AccountId;
                                                }
                                            }

                                            transaction.Credit = obj[0].GrossAmount.ToString();
                                            transaction.Debit = "0.00";
                                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                            transaction.Date = DateTime.Now;
                                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                            db.Transactions.Add(transaction);
                                            db.SaveChanges();

                                        }
                                        else
                                        {
                                            transaction.AccountName = getFGaccount.Title;
                                            transaction.AccountNumber = getFGaccount.AccountId;
                                            transaction.DetailAccountId = getFGaccount.AccountId;
                                            transaction.Credit = "0.00";
                                            transaction.Debit = obj[0].GrossAmount.ToString();
                                            transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                            transaction.Date = DateTime.Now;
                                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                            db.Transactions.Add(transaction);
                                            db.SaveChanges();
                                        }
                                    }

                                }
                                else
                                {
                                    var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                                    if (getCHaccount != null)
                                    {
                                        var fullcode = "";
                                        Paying newitems = new Paying();
                                        var recordemp = db.Payings.ToList();
                                        if (recordemp.Count() > 0)
                                        {
                                            if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
                                            {
                                                int large, small;
                                                int salesID = 0;
                                                large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                                small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                                for (int i = 0; i < recordemp.Count; i++)
                                                {
                                                    if (recordemp[i].InvoiceNumber != null)
                                                    {
                                                        var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                        if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
                                                        {
                                                            salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                            large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

                                                        }
                                                        else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
                                                        {
                                                            small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                        }
                                                        else
                                                        {
                                                            if (large < 2)
                                                            {
                                                                salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                            }
                                                        }
                                                    }
                                                }
                                                newitems = recordemp.ToList().Where(x => x.PayingId == salesID).FirstOrDefault();
                                                if (newitems != null)
                                                {
                                                    if (newitems.InvoiceNumber != null)
                                                    {
                                                        var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                                        int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                                        fullcode = "CP00" + "-" + Convert.ToString(code);
                                                    }
                                                    else
                                                    {
                                                        fullcode = "CP00" + "-" + "1";
                                                    }
                                                }
                                                else
                                                {
                                                    fullcode = "CP00" + "-" + "1";
                                                }
                                            }
                                            else
                                            {
                                                fullcode = "CP00" + "-" + "1";
                                            }
                                        }
                                        else
                                        {
                                            fullcode = "CP00" + "-" + "1";
                                        }

                                        Paying paying = null;
                                        paying = new Paying();
                                        paying.AccountName = customeracc.Title;
                                        paying.AccountNumber = customeracc.AccountId;
                                        paying.AccountId = customeracc.AccountId;

                                        if (obj[0].PaymentTerms == "Cheque")
                                        {
                                            paying.CheckDate = obj[0].CheckDate;
                                            paying.CheckNumber = obj[0].CheckNumber;
                                            paying.CheckTitle = obj[0].CheckTitle;
                                            paying.PaymentType = "Wire";

                                        }
                                        else
                                        {
                                            paying.PaymentType = "Cash";
                                        }
                                        paying.Note = "";
                                        paying.InvoiceNumber = fullcode;
                                        paying.Debit = obj[0].GrossAmount.ToString();
                                        paying.Credit = "0.00";
                                        paying.Date = DateTime.Now;
                                        db.Payings.Add(paying);
                                        await db.SaveChangesAsync();


                                        for (int i = 0; i < 2; i++)
                                        {
                                            Transaction transaction = null;
                                            transaction = new Transaction();
                                            if (i == 0)
                                            {
                                                if (customeracc != null)
                                                {
                                                    transaction.AccountName = customeracc.Title;
                                                    transaction.AccountNumber = customeracc.AccountId;
                                                    transaction.DetailAccountId = customeracc.AccountId;
                                                    transaction.Credit = "0.00";
                                                    transaction.Debit = obj[0].GrossAmount.ToString();
                                                    transaction.InvoiceNumber = fullcode;
                                                    transaction.Date = DateTime.Now;
                                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                                    db.Transactions.Add(transaction);
                                                    db.SaveChanges();
                                                }
                                            }
                                            else
                                            {
                                                transaction.AccountName = getCHaccount.Title;
                                                transaction.AccountNumber = getCHaccount.AccountId;
                                                transaction.DetailAccountId = getCHaccount.AccountId;
                                                transaction.Credit = obj[0].GrossAmount.ToString();
                                                transaction.Debit = "0.00";
                                                transaction.InvoiceNumber = fullcode;
                                                transaction.Date = DateTime.Now;
                                                transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                                db.Transactions.Add(transaction);
                                                db.SaveChanges();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, obj);

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("PurchaseOrderByID/{invoicenumber}")]
        public IActionResult PurchaseOrderByID(string invoicenumber)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                if (record != null)
                {
                    var allsuppliers = db.Vendors.ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].SupplierId).ToList())
                        {
                            record[i].SupplierName = item.FullName;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //[HttpGet("PurchaseOrderByproductID/{id}")]
        public IActionResult PurchaseOrderByproductID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.Where(x => x.ItemId == id).ToList();
                if (record != null)
                {
                    var allsuppliers = db.Vendors.ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        foreach (var item in allsuppliers.ToList().Where(x => x.VendorId == record[i].SupplierId).ToList())
                        {
                            record[i].SupplierName = item.FullName;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //Supplier Item Number
        [HttpGet("SupplierItemNumberGet")]
        public IActionResult SupplierItemNumberGet()
        {
            try
            {
                var record = db.SupplierItemNumbers.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<SupplierItemNumber>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SupplierItemNumberCreate")]
        public async Task<IActionResult> SupplierItemNumberCreate(SupplierItemNumber obj)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }


                bool isValid = db.SupplierItemNumbers.ToList().Exists(p => p.SupplierItemNum.Equals(obj.SupplierItemNum, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.SupplierItemNumbers.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("SupplierItemNumberUpdate/{id}")]
        public async Task<IActionResult> SupplierItemNumberUpdate(int id, SupplierItemNumber data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.SupplierItemNumberId)
                {
                    return BadRequest();
                }
                bool isValid = db.SupplierItemNumbers.ToList().Exists(x => x.SupplierItemNum.Equals(data.SupplierItemNum, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)
                {
                    return BadRequest("Store Already Exists");

                }

                else
                {
                    var record = await db.SupplierItemNumbers.FindAsync(id);
                    if (data.SupplierItemNum != null && data.SupplierItemNum != "undefined")
                    {
                        record.SupplierItemNum = data.SupplierItemNum;
                    }
                    data = record;
                    db.Entry(data).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok(Response);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteSupplierItemNumber/{id}")]
        public async Task<IActionResult> DeleteSupplierItemNumber(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                SupplierItemNumber data = await db.SupplierItemNumbers.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.SupplierItemNumbers.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SupplierItemNumberGetByID/{id}")]
        public IActionResult SupplierItemNumberGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();

                var record = db.SupplierItemNumbers.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("POByInvoiceID/{invoicenumber}/{method}")]
        public IActionResult POByInvoiceID(string invoicenumber, string method)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = new List<PurchaseOrder>();

                if (method == "next")
                {
                    var lastvalue = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).OrderBy(y => y.PurchaseorderId).LastOrDefault();
                    if (lastvalue != null)
                    {
                        var nextvalue = db.PurchaseOrders.Where(x => Convert.ToDouble(x.SimpleInvoiceNumber) > Convert.ToDouble(lastvalue.SimpleInvoiceNumber)).OrderBy(y => y.SimpleInvoiceNumber).FirstOrDefault();
                        if (nextvalue != null)
                        {
                            record = db.PurchaseOrders.Where(x => x.InvoiceNumber == nextvalue.InvoiceNumber).ToList();
                        }
                        else
                        {
                            record = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                        }
                    }
                    else
                    {

                    }

                }

                else if (method == "previous")
                {
                    var firstvalue = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).OrderBy(y => y.PurchaseorderId).FirstOrDefault();
                    if (firstvalue != null)
                    {
                        // var previousvalue = db.PurchaseOrders.Where(x => x.PurchaseorderId < firstvalue.PurchaseorderId).OrderBy(y => y.PurchaseorderId).LastOrDefault();
                        var previousvalue = db.PurchaseOrders.Where(x => Convert.ToDouble(x.SimpleInvoiceNumber) < Convert.ToDouble(firstvalue.SimpleInvoiceNumber)).OrderBy(y => y.SimpleInvoiceNumber).LastOrDefault();

                        if (previousvalue != null)
                        {
                            record = db.PurchaseOrders.Where(x => x.InvoiceNumber == previousvalue.InvoiceNumber).ToList();
                        }
                        else
                        {
                            record = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                        }
                    }
                    else
                    {
                        var previousvalue = db.PurchaseOrders.ToList().OrderBy(y => y.SimpleInvoiceNumber).LastOrDefault();
                        if (previousvalue != null)
                        {
                            record = db.PurchaseOrders.Where(x => x.InvoiceNumber == previousvalue.InvoiceNumber).ToList();
                        }
                    }

                }


                else if (method == "first")
                {
                    var firstvalue = db.PurchaseOrders.OrderBy(y => y.SimpleInvoiceNumber).FirstOrDefault();
                    if (firstvalue != null)
                    {

                        record = db.PurchaseOrders.Where(x => x.InvoiceNumber == firstvalue.InvoiceNumber).ToList();

                    }

                }
                else if (method == "last")
                {
                    var lastvalue = db.PurchaseOrders.OrderBy(y => y.SimpleInvoiceNumber).LastOrDefault();
                    if (lastvalue != null)
                    {

                        record = db.PurchaseOrders.Where(x => x.InvoiceNumber == lastvalue.InvoiceNumber).ToList();

                    }

                }
                else if (method == "null")
                {
                    record = db.PurchaseOrders.Where(x => x.InvoiceNumber == invoicenumber).ToList();
                }

                if (record != null && record.Count != 0)
                {

                    var GetPayments = db.Payings.Where(x => x.InvoiceNumber == invoicenumber).FirstOrDefault();
                    if (GetPayments != null)
                    {
                        record[0].InvoicePayments = GetPayments;
                    }
                    else
                    {
                        record[0].InvoicePayments = null;
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("PurchaseOrderUpdate")]
        public async Task<IActionResult> PurchaseOrderUpdate(List<PurchaseOrder> obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var oldPOList = db.PurchaseOrders.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();
                foreach (var oldpo in oldPOList)
                {
                    var existpo = obj.Where(x => x.ItemId == oldpo.ItemId).FirstOrDefault();
                    if (existpo == null)
                    {
                        db.PurchaseOrders.Remove(oldpo);
                    }
                }

                for (int j = 0; j < obj.Count; j++)
                {
                    var existpo = oldPOList.Where(x => x.ItemId == obj[j].ItemId).FirstOrDefault();
                    if (existpo != null)
                    {
                        if (obj[j].PurchaseorderId == 0)
                        {
                            obj[j].PurchaseorderId = existpo.PurchaseorderId;
                        }
                    }
                }
                db.SaveChanges();
                InventoryStock stock = null;
                double grossamount = 0;

                for (int a = 0; a < obj.Count(); a++)
                {
                    grossamount += Convert.ToDouble(obj[a].Amount);
                }

                for (int i = 0; i < obj.Count(); i++)
                {
                    var purchaseOrder = db.PurchaseOrders.Where(x => x.PurchaseorderId == obj[i].PurchaseorderId).SingleOrDefault();
                    if (purchaseOrder != null)
                    {

                        purchaseOrder.SupplierId = obj[i].SupplierId;
                        purchaseOrder.SupplierName = obj[i].SupplierName;
                        purchaseOrder.SupplierNumber = obj[i].SupplierNumber;
                        purchaseOrder.ShowAllItems = obj[i].ShowAllItems;
                        purchaseOrder.Supplieritems = obj[i].Supplieritems;
                        purchaseOrder.TotalItems = obj[i].TotalItems;
                        purchaseOrder.UpdateCost = obj[i].UpdateCost;
                        purchaseOrder.UpdateOscost = obj[i].UpdateOscost;
                        purchaseOrder.Received = obj[i].Received;


                        if (obj[0].Received == true)
                        {
                            obj[i].IsPostStatus = "true";
                            purchaseOrder.ReceivedDate = obj[i].ReceivedDate;
                        }
                        else
                        {
                            obj[i].IsPostStatus = "false";
                            purchaseOrder.PaidDate = DateTime.Now;
                        }
                        if (obj[0].IsPaid == true)
                        {
                            obj[i].PaidDate = DateTime.Now;
                            purchaseOrder.IsPaid = obj[i].IsPaid;
                            purchaseOrder.PaidDate = DateTime.Now;
                        }
                        purchaseOrder.Currrentuser = obj[i].Currrentuser;
                        purchaseOrder.PaymentTerms = obj[i].PaymentTerms;
                        purchaseOrder.TotalTobacco = obj[i].TotalTobacco;
                        purchaseOrder.TotalCigar = obj[i].TotalCigar;
                        purchaseOrder.TotalCigarette = obj[i].TotalCigarette;
                        purchaseOrder.CigaretteStick = obj[i].CigaretteStick;
                        purchaseOrder.Notes = obj[i].Notes;
                        purchaseOrder.IsReport = obj[i].IsReport;
                        //purchaseOrder.IsPaid = obj[i].IsPaid;
                        purchaseOrder.IsPostStatus = obj[i].IsPostStatus;
                        purchaseOrder.PaidAmount = obj[i].PaidAmount;
                        purchaseOrder.SubTotal = obj[i].SubTotal;
                        purchaseOrder.Freight = obj[i].Freight;
                        purchaseOrder.IsTax = obj[i].IsTax;
                        purchaseOrder.Other = obj[i].Other;
                        purchaseOrder.Total = obj[i].Total;
                        purchaseOrder.SupplierItemNumber = obj[i].SupplierItemNumber;
                        purchaseOrder.StockItemNumber = obj[i].StockItemNumber;
                        purchaseOrder.Description = obj[i].Description;
                        purchaseOrder.Retail = obj[i].Retail;
                        purchaseOrder.IsPrice = obj[i].IsPrice;
                        purchaseOrder.Price = obj[i].Price;
                        purchaseOrder.IsQty = obj[i].IsQty;
                        purchaseOrder.Qty = obj[i].Qty;
                        purchaseOrder.IsCaseQty = obj[i].IsCaseQty;
                        purchaseOrder.CaseQty = obj[i].CaseQty;
                        purchaseOrder.IsDiscount = obj[i].IsDiscount;
                        purchaseOrder.Discount = obj[i].Discount;
                        purchaseOrder.Amount = obj[i].Amount;
                        purchaseOrder.ItemId = obj[i].ItemId;
                        purchaseOrder.ItemCode = obj[i].ItemCode;
                        purchaseOrder.ProductBarCode = obj[i].ProductBarCode;
                        purchaseOrder.Sku = obj[i].Sku;
                        purchaseOrder.GrossAmount = grossamount.ToString();
                        purchaseOrder.Podate = DateTime.Now;
                        purchaseOrder.InvoiceDate = DateTime.Now;
                        purchaseOrder.DateReceived = DateTime.Now;
                        //purchaseOrder.PaidDate = DateTime.Now;
                        bool firstTime = false;
                        stock = new InventoryStock();

                        if (obj[i].ItemId != null)
                        {
                            var getproductsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                            if (getproductsdetails != null)
                            {
                                stock.ItemBarCode = getproductsdetails.BarCode;
                                stock.ItemCode = getproductsdetails.ItemNumber;
                                stock.Sku = getproductsdetails.Sku;
                            }
                            stock.ProductId = obj[i].ItemId;
                            stock.ItemName = obj[i].Description;
                            stock.Quantity = obj[i].Qty;
                        }
                        // Purchase Update
                        db.Entry(purchaseOrder).State = EntityState.Modified;

                        var productdetail = db.Products.Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                        if (productdetail.Cost != obj[i].Price)
                        {
                            var itemcostchange = new ItemCostChange();
                            itemcostchange.ItemId = obj[i].ItemId;
                            itemcostchange.ItemName = obj[i].Description;
                            itemcostchange.PoInvoiceNumber = obj[i].InvoiceNumber;
                            itemcostchange.OldPrice = productdetail.Cost;
                            itemcostchange.NewPrice = obj[i].Price;
                            itemcostchange.PriceDiff = (float.Parse(productdetail.Cost) - float.Parse(obj[i].Price)).ToString();
                            itemcostchange.PricePercentage = ((float.Parse(itemcostchange.PriceDiff) / float.Parse(productdetail.Cost)) * 100).ToString();
                            itemcostchange.CreatedDate = DateTime.Now;
                            itemcostchange.NewPrice = obj[i].Price;
                            itemcostchange.CretedBy = obj[i].Currrentuser;
                            db.ItemCostChanges.Add(itemcostchange);
                            productdetail.OldPrice = productdetail.Cost;
                            productdetail.Cost = obj[i].Price;
                            productdetail.PriceDiff = itemcostchange.PriceDiff;
                            productdetail.PriceDiffPercentage = itemcostchange.PricePercentage;
                            db.Entry(productdetail).State = EntityState.Modified;
                        }
                        db.SaveChanges();
                        //Stock Update
                        if (obj[0].Received == true)
                        {
                            var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                            if (getstock != null)
                            {
                                getstock.Quantity = (Convert.ToDouble(getstock.Quantity) + Convert.ToDouble(obj[i].Qty)).ToString();
                                db.Entry(getstock).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                InventoryStock stocknum = new InventoryStock();
                                var productsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                                if (productsdetails != null)
                                {
                                    stock.ItemBarCode = productsdetails.BarCode;
                                    stock.ItemCode = productsdetails.ItemNumber;
                                    stock.Sku = productsdetails.Sku;
                                }
                                stock.ProductId = obj[i].ItemId;
                                stock.ItemName = obj[i].Description;
                                stock.Quantity = obj[i].Qty;
                                //stock.StockItemNumber = fullcode;
                                stock.StockItemNumber = obj[i].StockItemNumber;

                                db.InventoryStocks.Add(stock);
                                db.SaveChanges();
                                firstTime = true;
                                var updateitem = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                                if (updateitem != null)
                                {

                                    //updateitem.StockItemNumber = fullcode;
                                    updateitem.StockItemNumber = obj[i].ItemCode;
                                    db.Entry(updateitem).State = EntityState.Modified;
                                }
                                if (firstTime == true)
                                {
                                    var foundPOrder = db.PurchaseOrders.Find(Convert.ToInt32(purchaseOrder.PurchaseorderId));
                                    if (foundPOrder != null)
                                    {
                                        // foundPOrder.StockItemNumber = fullcode;
                                        foundPOrder.StockItemNumber = obj[i].ItemCode;
                                        db.Entry(foundPOrder).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var purchaseOrderNew = new PurchaseOrder();

                        purchaseOrderNew.SupplierId = obj[i].SupplierId;
                        purchaseOrderNew.SupplierName = obj[i].SupplierName;
                        purchaseOrderNew.SupplierNumber = obj[i].SupplierNumber;
                        purchaseOrderNew.ShowAllItems = obj[i].ShowAllItems;
                        purchaseOrderNew.Supplieritems = obj[i].Supplieritems;
                        purchaseOrderNew.TotalItems = obj[i].TotalItems;
                        purchaseOrderNew.UpdateCost = obj[i].UpdateCost;
                        purchaseOrderNew.UpdateOscost = obj[i].UpdateOscost;
                        purchaseOrderNew.Received = obj[i].Received;
                        if (obj[0].Received == true)
                        {
                            obj[i].IsPostStatus = "true";
                            purchaseOrderNew.ReceivedDate = obj[i].ReceivedDate;
                        }
                        else
                        {
                            obj[i].IsPostStatus = "false";
                        }
                        if (obj[0].IsPaid == true)
                        {
                            obj[i].PaidDate = DateTime.Now;
                            purchaseOrderNew.IsPaid = obj[i].IsPaid;
                            purchaseOrderNew.PaidDate = DateTime.Now;
                        }
                        purchaseOrderNew.Currrentuser = obj[i].Currrentuser;
                        purchaseOrderNew.PaymentTerms = obj[i].PaymentTerms;
                        purchaseOrderNew.TotalTobacco = obj[i].TotalTobacco;
                        purchaseOrderNew.TotalCigar = obj[i].TotalCigar;
                        purchaseOrderNew.TotalCigarette = obj[i].TotalCigarette;
                        purchaseOrderNew.CigaretteStick = obj[i].CigaretteStick;
                        purchaseOrderNew.Notes = obj[i].Notes;
                        purchaseOrderNew.IsReport = obj[i].IsReport;
                       // purchaseOrderNew.IsPaid = obj[i].IsPaid;
                        purchaseOrderNew.IsPostStatus = obj[i].IsPostStatus;
                        purchaseOrderNew.PaidAmount = obj[i].PaidAmount;
                        purchaseOrderNew.SubTotal = obj[i].SubTotal;
                        purchaseOrderNew.Freight = obj[i].Freight;
                        purchaseOrderNew.IsTax = obj[i].IsTax;
                        purchaseOrderNew.Other = obj[i].Other;
                        purchaseOrderNew.Total = obj[i].Total;
                        purchaseOrderNew.SupplierItemNumber = obj[i].SupplierItemNumber;
                        purchaseOrderNew.StockItemNumber = obj[i].StockItemNumber;
                        purchaseOrderNew.Description = obj[i].Description;
                        purchaseOrderNew.Retail = obj[i].Retail;
                        purchaseOrderNew.IsPrice = obj[i].IsPrice;
                        purchaseOrderNew.Price = obj[i].Price;
                        purchaseOrderNew.IsQty = obj[i].IsQty;
                        purchaseOrderNew.Qty = obj[i].Qty;
                        purchaseOrderNew.IsCaseQty = obj[i].IsCaseQty;
                        purchaseOrderNew.CaseQty = obj[i].CaseQty;
                        purchaseOrderNew.IsDiscount = obj[i].IsDiscount;
                        purchaseOrderNew.Discount = obj[i].Discount;
                        purchaseOrderNew.Amount = obj[i].Amount;
                        purchaseOrderNew.ItemId = obj[i].ItemId;
                        purchaseOrderNew.ItemCode = obj[i].ItemCode;
                        purchaseOrderNew.ProductBarCode = obj[i].ProductBarCode;
                        purchaseOrderNew.Sku = obj[i].Sku;
                        purchaseOrderNew.GrossAmount = grossamount.ToString();
                        purchaseOrderNew.Podate = DateTime.Now;
                        purchaseOrderNew.InvoiceDate = DateTime.Now;
                        purchaseOrderNew.DateReceived = DateTime.Now;
                      //  purchaseOrderNew.PaidDate = DateTime.Now;
                        purchaseOrderNew.InvoiceNumber = obj[i].InvoiceNumber;
                        var SimpleInvoiceNumber = obj[i].InvoiceNumber.Replace("-", "");
                        purchaseOrderNew.SimpleInvoiceNumber = SimpleInvoiceNumber;
                        bool firstTime = false;
                        stock = new InventoryStock();

                        if (obj[i].ItemId != null)
                        {
                            var getproductsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                            if (getproductsdetails != null)
                            {
                                stock.ItemBarCode = getproductsdetails.BarCode;
                                stock.ItemCode = getproductsdetails.ItemNumber;
                                stock.Sku = getproductsdetails.Sku;
                            }
                            stock.ProductId = obj[i].ItemId;
                            stock.ItemName = obj[i].Description;
                            stock.Quantity = obj[i].Qty;
                        }
                        // Purchase Update
                        var neworder = db.PurchaseOrders.Add(purchaseOrderNew);
                        //db.Entry(purchaseOrderNew).State = EntityState.Modified;
                        var productdetail = db.Products.Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                        if (productdetail.Cost != obj[i].Price)
                        {
                            var itemcostchange = new ItemCostChange();
                            itemcostchange.ItemId = obj[i].ItemId;
                            itemcostchange.ItemName = obj[i].Description;
                            itemcostchange.PoInvoiceNumber = obj[i].InvoiceNumber;
                            itemcostchange.OldPrice = productdetail.Cost;
                            itemcostchange.NewPrice = obj[i].Price;
                            itemcostchange.PriceDiff = (float.Parse(productdetail.Cost) - float.Parse(obj[i].Price)).ToString();
                            itemcostchange.PricePercentage = ((float.Parse(itemcostchange.PriceDiff) / float.Parse(productdetail.Cost)) * 100).ToString();
                            itemcostchange.CreatedDate = DateTime.Now;
                            itemcostchange.NewPrice = obj[i].Price;
                            itemcostchange.CretedBy = obj[i].Currrentuser;
                            db.ItemCostChanges.Add(itemcostchange);
                            productdetail.OldPrice = productdetail.Cost;
                            productdetail.Cost = obj[i].Price;
                            productdetail.PriceDiff = itemcostchange.PriceDiff;
                            productdetail.PriceDiffPercentage = itemcostchange.PricePercentage;
                            db.Entry(productdetail).State = EntityState.Modified;
                        }
                        db.SaveChanges();

                        if (obj[0].Received == true)
                        {
                            var getstock = db.InventoryStocks.ToList().Where(x => x.ProductId == obj[i].ItemId).FirstOrDefault();
                            if (getstock != null)
                            {
                                getstock.Quantity = (Convert.ToDouble(getstock.Quantity) + Convert.ToDouble(obj[i].Qty)).ToString();
                                db.Entry(getstock).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            else
                            {
                                InventoryStock stocknum = new InventoryStock();
                                var productsdetails = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                                if (productsdetails != null)
                                {
                                    stock.ItemBarCode = productsdetails.BarCode;
                                    stock.ItemCode = productsdetails.ItemNumber;
                                    stock.Sku = productsdetails.Sku;
                                }
                                stock.ProductId = obj[i].ItemId;
                                stock.ItemName = obj[i].Description;
                                stock.Quantity = obj[i].Qty;
                                //stock.StockItemNumber = fullcode;
                                stock.StockItemNumber = obj[i].StockItemNumber;

                                db.InventoryStocks.Add(stock);
                                db.SaveChanges();
                                firstTime = true;
                                var updateitem = db.Products.ToList().Where(x => x.Id == obj[i].ItemId).FirstOrDefault();
                                if (updateitem != null)
                                {

                                    //updateitem.StockItemNumber = fullcode;
                                    updateitem.StockItemNumber = obj[i].ItemCode;
                                    db.Entry(updateitem).State = EntityState.Modified;
                                }
                                if (firstTime == true)
                                {
                                    var foundPOrder = db.PurchaseOrders.Find(Convert.ToInt32(neworder.Entity.PurchaseorderId));
                                    if (foundPOrder != null)
                                    {
                                        // foundPOrder.StockItemNumber = fullcode;
                                        foundPOrder.StockItemNumber = obj[i].ItemCode;
                                        db.Entry(foundPOrder).State = EntityState.Modified;
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }

                    }

                    if (obj[i].SupplierItemNumber != "")
                    {
                        var suplieritem = db.SupplierItemNumbers.ToList().Where(x => x.VendorId == obj[i].SupplierId && x.ProductId == obj[i].ItemId && x.SupplierItemNum.Equals(obj[i].SupplierItemNumber, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                        if (suplieritem == null)
                        {
                            var supplieritemnum = new SupplierItemNumber();
                            supplieritemnum.ProductId = obj[i].ItemId;
                            supplieritemnum.VendorId = obj[i].SupplierId;
                            supplieritemnum.SupplierItemNum = obj[i].SupplierItemNumber;
                            supplieritemnum.CreatedDate = DateTime.Now;
                            db.SupplierItemNumbers.Add(supplieritemnum);
                            db.SaveChanges();

                        }
                    }
                }

                var getvendor = db.Vendors.Find(obj[0].SupplierId);
                if (getvendor != null)
                {
                    var getaccount = db.Accounts.ToList().Where(a => a.Title == getvendor.AccountTitle && a.AccountId == getvendor.AccountId).FirstOrDefault();

                    if (getaccount != null)
                    {
                        if (obj[0].IsPaid == false || obj[0].IsPaid == null)
                        {
                            var getFGaccount = db.Accounts.ToList().Where(a => a.Title == "Finish Goods").FirstOrDefault();
                            //Payables 
                            Payable pay = null;
                            pay = new Payable();
                            if (getaccount != null)
                            {
                                var getpay = db.Payables.ToList().Where(x => x.AccountId == getaccount.AccountId).FirstOrDefault();
                                if (getpay != null)
                                {
                                    getpay.Amount = (Convert.ToDouble(getpay.Amount) + Convert.ToDouble(obj[0].GrossAmount)).ToString();
                                    db.Entry(getpay).State = EntityState.Modified;
                                    db.SaveChanges();

                                }
                                else
                                {
                                    pay.AccountId = getaccount.AccountId;
                                    pay.AccountNumber = getaccount.AccountId;
                                    pay.Amount = obj[0].GrossAmount.ToString();
                                    pay.AccountName = getaccount.Title;
                                    db.Payables.Add(pay);
                                    db.SaveChanges();
                                }
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                Transaction transaction = null;
                                transaction = new Transaction();
                                if (i == 0)
                                {
                                    if (getaccount != null)
                                    {
                                        if (getaccount.Title != null)
                                        {
                                            transaction.AccountName = getaccount.Title;
                                        }
                                        if (getaccount.AccountId != null)
                                        {
                                            transaction.AccountNumber = getaccount.AccountId;
                                        }
                                        if (getaccount.AccountId != null)
                                        {
                                            transaction.DetailAccountId = getaccount.AccountId;
                                        }
                                    }

                                    if (obj[0].GrossAmount != null)
                                    {
                                        transaction.Credit = obj[0].GrossAmount.ToString();
                                    }
                                    transaction.Debit = "0.00";
                                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();

                                }
                                else
                                {
                                    transaction.AccountName = getFGaccount.Title;
                                    transaction.AccountNumber = getFGaccount.AccountId;
                                    transaction.DetailAccountId = getFGaccount.AccountId;
                                    transaction.Credit = "0.00";
                                    if (obj[0].GrossAmount != null)
                                    {
                                        transaction.Debit = obj[0].GrossAmount.ToString();
                                    }
                                    transaction.InvoiceNumber = obj[0].InvoiceNumber;
                                    transaction.Date = DateTime.Now;
                                    transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                    db.Transactions.Add(transaction);
                                    db.SaveChanges();
                                }
                            }

                        }
                        else
                        {
                            var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                            if (getCHaccount != null)
                            {
                                var fullcode = "";
                                Paying newitems = new Paying();
                                var recordemp = db.Payings.ToList();
                                if (recordemp.Count() > 0)
                                {
                                    if (recordemp[0].InvoiceNumber != null && recordemp[0].InvoiceNumber != "string" && recordemp[0].InvoiceNumber != "")
                                    {
                                        int large, small;
                                        int salesID = 0;
                                        large = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                        small = Convert.ToInt32(recordemp[0].InvoiceNumber.Split('-')[1]);
                                        for (int i = 0; i < recordemp.Count; i++)
                                        {
                                            if (recordemp[i].InvoiceNumber != null)
                                            {
                                                var t = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) > large)
                                                {
                                                    salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                    large = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);

                                                }
                                                else if (Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]) < small)
                                                {
                                                    small = Convert.ToInt32(recordemp[i].InvoiceNumber.Split('-')[1]);
                                                }
                                                else
                                                {
                                                    if (large < 2)
                                                    {
                                                        salesID = Convert.ToInt32(recordemp[i].PayingId);
                                                    }
                                                }
                                            }
                                        }
                                        newitems = recordemp.ToList().Where(x => x.PayingId == salesID).FirstOrDefault();
                                        if (newitems != null)
                                        {
                                            if (newitems.InvoiceNumber != null)
                                            {
                                                var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                                int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                                fullcode = "CP00" + "-" + Convert.ToString(code);
                                            }
                                            else
                                            {
                                                fullcode = "CP00" + "-" + "1";
                                            }
                                        }
                                        else
                                        {
                                            fullcode = "CP00" + "-" + "1";
                                        }
                                    }
                                    else
                                    {
                                        fullcode = "CP00" + "-" + "1";
                                    }
                                }
                                else
                                {
                                    fullcode = "CP00" + "-" + "1";
                                }

                                Paying paying = null;
                                paying = new Paying();
                                paying.AccountName = getaccount.Title;
                                paying.AccountNumber = getaccount.AccountId;
                                paying.AccountId = getaccount.AccountId;
                                // paying.CashBalance = grossamount.ToString();
                                paying.PaymentType = "Cash";
                                if (paying.PaymentType == "Check")
                                {
                                    paying.CheckDate = obj[0].CheckDate;
                                    paying.CheckNumber = obj[0].CheckNumber;
                                    paying.CheckTitle = obj[0].CheckTitle;

                                }
                                //paying.PayFromAccountNumber = getCHaccount.AccountId;
                                //paying.PayFromAccountId = getCHaccount.AccountId;
                                //paying.PayFromAccount = getCHaccount.Title;
                                paying.Note = "";
                                // paying.NetAmount = grossamount.ToString();
                                paying.InvoiceNumber = fullcode;
                                if (obj[0].GrossAmount != null)
                                {
                                    paying.Debit = obj[0].GrossAmount.ToString();
                                }    
                                paying.Credit = "0.00";
                                paying.Date = DateTime.Now;
                                db.Payings.Add(paying);
                                await db.SaveChangesAsync();
                                for (int i = 0; i < 2; i++)
                                {
                                    Transaction transaction = null;
                                    transaction = new Transaction();
                                    if (i == 0)
                                    {
                                        if (getaccount != null)
                                        {
                                            transaction.AccountName = getaccount.Title;
                                            transaction.AccountNumber = getaccount.AccountId;
                                            transaction.DetailAccountId = getaccount.AccountId;
                                            transaction.Credit = "0.00";
                                            transaction.Debit = obj[0].GrossAmount.ToString();
                                            transaction.InvoiceNumber = fullcode;
                                            transaction.Date = DateTime.Now;
                                            transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                            db.Transactions.Add(transaction);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        transaction.AccountName = getCHaccount.Title;
                                        transaction.AccountNumber = getCHaccount.AccountId;
                                        transaction.DetailAccountId = getCHaccount.AccountId;
                                        transaction.Credit = obj[0].GrossAmount.ToString();
                                        transaction.Debit = "0.00";
                                        transaction.InvoiceNumber = fullcode;
                                        transaction.Date = DateTime.Now;
                                        transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                                        db.Transactions.Add(transaction);
                                        db.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                var obj1 = db.PurchaseOrders.Where(x => x.InvoiceNumber == obj[0].InvoiceNumber).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, obj1);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetInvoicesByVendorId/{id}")]
        public IActionResult GetInvoicesByVendorId(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.Where(x => x.SupplierId == id).ToList().GroupBy(x => x.InvoiceNumber).Select(x => x.First()).OrderByDescending(x => x.InvoiceNumber).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetpayablesByAccountId/{id}")]
        public IActionResult GetpayablesByAccountId(string id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Payable>();
                var record = db.Payables.Where(x => x.AccountNumber == id).FirstOrDefault();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("InvoicePayment")]
        public async Task<IActionResult> InvoicePayment(SuppliersPayment obj)
        {
            try
            {
                obj.PaidDate = DateTime.Now;
                var record = db.Vendors.Find(obj.SupplierId);
                var Response = ResponseBuilder.BuildWSResponse<SuppliersPayment>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                bool isValid = db.SuppliersPayments.ToList().Exists(p => p.Ponumber == obj.Ponumber);
                if (isValid)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (record != null)
                {
                    var account = db.Payables.Where(x => x.AccountNumber == record.AccountNumber)?.FirstOrDefault();
                    if (account != null)
                    {
                        account.Amount = obj.Pobalance;
                        db.Entry(account).State = EntityState.Modified;
                    }

                }
                var purchaseOrder = db.PurchaseOrders.Where(x => x.InvoiceNumber == obj.Ponumber).ToList();
                if (purchaseOrder.Count > 0)
                {
                    for (int i = 0; i < purchaseOrder.Count; i++)
                    {
                      //  purchaseOrder[i].IsPaid = true;
                        purchaseOrder[i].PaidAmount = obj.TotalPaid;
                        purchaseOrder[i].RemaningPayment = obj.RemaningPayment;
                        db.Entry(purchaseOrder[i]).State = EntityState.Modified;
                    }
                }
                db.SuppliersPayments.Add(obj);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetItemByUcpOrItemNo/{itemno}")]
        public IActionResult GetItemByUcpOrItemNo(string itemno)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.ToList().Where(x => x.ItemNumber == itemno || x.Sku == itemno).FirstOrDefault();
                if (record != null)
                {
                    var CheckQty = db.InventoryStocks.Where(x => x.ItemCode == itemno || x.ItemName == itemno).FirstOrDefault();
                    if (CheckQty != null)
                    {
                        record.ItemQuantity = CheckQty.Quantity;
                    }
                    else
                    {
                        record.ItemQuantity = "Nill";
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }


            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetOpenedPurchase")]
        public IActionResult GetOpenedPurchase()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.ToList().GroupBy(x => x.InvoiceNumber).Select(i => i.FirstOrDefault()).Where(x => x.Received == false).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Purchase>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetPostedPurchase")]
        public IActionResult GetPostedPurchase()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<PurchaseOrder>>();
                var record = db.PurchaseOrders.ToList().GroupBy(x => x.InvoiceNumber).Select(i => i.FirstOrDefault()).Where(x => x.Received == true).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<PurchaseOrder>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveOtherPayment")]
        public async Task<IActionResult> SaveOtherPayment(Paying obj)
        {
            try
            {
                if (obj.NetAmount != null && obj.NetAmount != "undefined")
                {
                    string trimmed = (obj.NetAmount as string).Trim('$');
                    obj.NetAmount = trimmed;
                }
                var Response = ResponseBuilder.BuildWSResponse<Paying>();
                var PurchaseOrder = db.PurchaseOrders.ToList().Where(x => x.InvoiceNumber == obj.InvoiceNumber).GroupBy(x => x.InvoiceNumber).Select(x => x.FirstOrDefault()).FirstOrDefault();
                //if (PurchaseOrder != null)
                //{
                //    if(PurchaseOrder.Received == null || PurchaseOrder.Received == false && PurchaseOrder.IsPaid == null || PurchaseOrder.IsPaid == false)
                //    {
                //        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                //    }
                //}
                obj.Date = DateTime.Now;
                obj.PaymentType = "Cash";
                var id = Convert.ToInt32(obj.AccountId);
                var payingfound = db.Payings.ToList().Where(x => x.InvoiceNumber == obj.InvoiceNumber).FirstOrDefault();
                var record = db.Vendors.Find(id);
                if (record != null)
                {
                    obj.AccountId = record.AccountId;
                    obj.AccountNumber = record.AccountNumber;
                    obj.AccountName = record.AccountTitle;
                }
                if (PurchaseOrder != null)
                {

                    if (Convert.ToDouble(PurchaseOrder.Total) <= Convert.ToDouble(obj.Debit))
                    {
                        PurchaseOrder.IsPaid = true;
                        PurchaseOrder.IsPartialPaid = false;
                        db.Payings.Add(obj);
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        PurchaseOrder.IsPaid = false;
                        PurchaseOrder.IsPartialPaid = true;

                        if (payingfound != null)
                        {
                            var TodayWePay = Convert.ToDouble(obj.Debit) - Convert.ToDouble(payingfound.Debit);
                            payingfound.Debit = (Convert.ToDouble(TodayWePay) + Convert.ToDouble(payingfound.Debit)).ToString();
                            payingfound.Comments = obj.Comments;
                            payingfound.Comments = obj.Note;
                            if (Convert.ToDouble(payingfound.Debit) >= Convert.ToDouble(obj.NetAmount))
                            {
                                payingfound.TotalPaid = true;
                            }
                            else
                            {
                                payingfound.TotalPaid = false;
                            }
                            payingfound.NetAmount = obj.NetAmount;
                            payingfound.CashBalance = (Convert.ToDouble(obj.NetAmount) - Convert.ToDouble(payingfound.Debit)).ToString();
                            db.Entry(payingfound).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            db.Payings.Add(obj);
                            await db.SaveChangesAsync();
                        }
                    }
                    PurchaseOrder.PaymentComments = obj.Comments;
                    db.Entry(PurchaseOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    obj.PaymentType = "Cash";
                    db.Payings.Add(obj);
                    await db.SaveChangesAsync();
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //bool isValid = db.Payings.ToList().Exists(p => p.InvoiceNumber == obj.InvoiceNumber);
                //if (isValid)
                //{
                //    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                //}



                var payable = db.Payables.Where(x => x.AccountNumber == obj.AccountNumber).FirstOrDefault();
                if (payable != null)
                {
                    double payableAmount = 0.00;
                    var payingCashBalances = db.Payings.Where(x => x.AccountId == obj.AccountId).ToList();
                    if(payingCashBalances != null)
                    {
                        foreach(var payingCashBalance in payingCashBalances)
                        {
                            payableAmount += Convert.ToDouble(payingCashBalance.CashBalance);
                        }
                    }
                    //double num1 = Convert.ToDouble(payable.Amount);
                    //double num2 = 0;
                    //if (payingfound != null)
                    //{
                    //    num2 = Convert.ToDouble(payingfound.Debit);
                    //    double TodayWePay = 0;
                    //    if (payingfound.TotalPaid != true)
                    //    {
                    //        //TodayWePay = Convert.ToDouble(num1) - Convert.ToDouble(obj.CashBalance);
                    //        //payingfound.Debit = (Convert.ToDouble(payingfound.Debit) + TodayWePay).ToString();
                    //        TodayWePay = num2;
                    //    }
                    //    else
                    //    {
                    //        TodayWePay = num2;
                    //    }
                    //    if (Convert.ToDouble(PurchaseOrder.Total) <= Convert.ToDouble(payingfound.Debit))
                    //    {
                    //        //var PayableAMount = Convert.ToDouble(payingfound.Debit) - Convert.ToDouble(PurchaseOrder.Total);
                    //        //payable.Amount = Convert.ToString(PayableAMount);

                    //    }
                    //    else
                    //    {
                    //        var num3 = num1 - TodayWePay;
                    //        payable.Amount = Convert.ToString(num3);
                    //    }
                    //}
                    //else
                    //{
                    //    num2 = Convert.ToDouble(obj.Debit);
                    //    var num3 = num1 - num2;
                    //    if (Convert.ToDouble(PurchaseOrder.Total) <= Convert.ToDouble(obj.Debit))
                    //    {
                    //        //var PayableAMount = Convert.ToDouble(PurchaseOrder.Total) - Convert.ToDouble(obj.Debit);
                    //        //payable.Amount = Convert.ToString(PayableAMount);

                    //    }
                    //    else
                    //    {
                    //        payable.Amount = Convert.ToString(num3);
                    //    }
                    //}
                    payable.Amount = Convert.ToString(payableAmount);
                    db.Entry(payable).State = EntityState.Modified;
                    db.SaveChanges();

                    //Convert.ToDouble(payable.Amount) - Convert.ToDouble(obj.NetAmount);
                }

                var getaccount = db.Accounts.ToList().Where(a => a.AccountId == obj.AccountNumber).FirstOrDefault();

                var getCHaccount = db.Accounts.ToList().Where(a => a.Title == "Cash in hand").FirstOrDefault();
                for (int i = 0; i < 2; i++)
                {
                    Transaction transaction = null;
                    transaction = new Transaction();
                    if (i == 0)
                    {
                        transaction.AccountName = getaccount.Title;
                        transaction.AccountNumber = getaccount.AccountId;
                        transaction.DetailAccountId = getaccount.AccountId;
                        transaction.Credit = "0.00";
                        transaction.Debit = obj.Debit;
                        transaction.InvoiceNumber = obj.InvoiceNumber;
                        transaction.Date = DateTime.Now;
                        transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                        db.Transactions.Add(transaction);
                        db.SaveChanges();

                    }
                    else
                    {
                        transaction.AccountName = getCHaccount.Title;
                        transaction.AccountNumber = getCHaccount.AccountId;
                        transaction.DetailAccountId = getCHaccount.AccountId;
                        transaction.Credit = obj.Debit;
                        transaction.Debit = "0.00";
                        transaction.InvoiceNumber = obj.InvoiceNumber; ;
                        transaction.Date = DateTime.Now;
                        transaction.ClosingBalance = (Convert.ToDouble(transaction.Debit) - Convert.ToDouble(transaction.Credit)).ToString();
                        db.Transactions.Add(transaction);
                        db.SaveChanges();
                    }
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SupplierItemNumber>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetTodayAllSalesCounter")]
        public IActionResult GetTodayAllSalesCounter()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<int>();
                int counter = 0;
                DateTime date = DateTime.Now;
                var shortDateValue = date.ToShortDateString();
                var Getorders = db.PosSales.ToList();
                if (Getorders.Count() > 0)
                {
                    for (int i = 0; i < Getorders.Count(); i++)
                    {
                        var getdate = Getorders[i].InvoiceDate.Value.ToShortDateString();
                        if (getdate == shortDateValue)
                        {
                            counter++;
                        }

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, counter);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<int>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllPayables")]
        public IActionResult GetAllPayables()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<double>();
                double AllPay = 0;
                var counter = db.Payables.ToList();
                if (counter.Count() > 0)
                {
                    for (int i = 0; i < counter.Count(); i++)
                    {
                        var aa = counter[i].Amount;
                        AllPay += Convert.ToDouble(aa);
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, AllPay);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, AllPay);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<int>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllReceivables")]
        public IActionResult GetAllReceivables()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<double>();
                double AllRec = 0;
                var counter = db.Receivables.ToList();
                if (counter.Count() > 0)
                {
                    for (int i = 0; i < counter.Count(); i++)
                    {
                        var aa = counter[i].Amount;
                        AllRec += Convert.ToDouble(aa);
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, AllRec);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, AllRec);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<int>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllOnlineOrders")]
        public IActionResult GetAllOnlineOrders()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<double>();
                double counter = db.CartDetails.ToList().Where(x => x.PendingForApproval == false).GroupBy(x => x.TicketId).Select(x => x.First()).Count();
                if (counter != null)
                {

                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, counter);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<double>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetCashierOrdersByName/{username}")]
        public IActionResult GetCashierOrdersByName(string username)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<double>();
                int counter = 0;
                DateTime date = DateTime.Now;
                var shortDateValue = date.ToShortDateString();
                var Getorders = db.Sales.ToList().Where(x => x.CashierName == username).ToList();
                if (Getorders.Count() > 0)
                {
                    for (int i = 0; i < Getorders.Count(); i++)
                    {
                        var getdate = Getorders[i].SaleDate.Value.ToShortDateString();
                        if (getdate == shortDateValue)
                        {
                            counter++;
                        }

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, counter);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, counter);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<double>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, 0);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}


