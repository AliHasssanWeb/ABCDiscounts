using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Domain.DataConfig.Configurations;
using ABC.POS.Website.Models;
using ABC.Shared;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SaleOrder()
        {
            Sale Model = new Sale();
            try
            {

                SResponse respEmp = RequestSender.Instance.CallAPI("api", "Sale/SaleGet", "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<Sale>> record = JsonConvert.DeserializeObject<ResponseBack<List<Sale>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        Sale newitems = new Sale();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int SaleID = 0;
                            large = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count; i++)
                            {
                                if (record.Data[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) > large)
                                    {
                                        SaleID = Convert.ToInt32(record.Data[i].SaleId);
                                        large = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            SaleID = Convert.ToInt32(record.Data[i].SaleId);
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.SaleId == SaleID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.InvoiceNumber != null)
                                {
                                    var VcodeSplit = newitems.InvoiceNumber.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "SAL-00" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "SAL-" + "001";
                                }
                            }
                            else
                            {
                                fullcode = "SAL-" + "001";
                            }
                        }
                        else
                        {
                            fullcode = "SAL-" + "001";
                        }

                        ViewBag.InvoiceNumber = fullcode;
                    }
                    else
                    {
                        ViewBag.InvoiceNumber = "SAL-" + "001";
                    }
                    Model.InvoiceNumber = ViewBag.InvoiceNumber;
                }
                else
                {
                    ViewBag.InvoiceNumber = "SAL-" + "001";

                }

                List<CustomerInformation> responseObjectCustomer = new List<CustomerInformation>();
                var FoundSession = HttpContext.Session.GetString("loadedCustomers");
                List<CustomerInformation> FoundSession_Result = new List<CustomerInformation>();
                if (FoundSession != null)
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<CustomerInformation>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    SResponse CustomerGet = RequestSender.Instance.CallAPI("api",
                        "Customer/CustomersGet", "GET");
                    if (CustomerGet.Status && (CustomerGet.Resp != null) && (CustomerGet.Resp != ""))
                    {
                        ResponseBack<List<CustomerInformation>> response =
                                     JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(CustomerGet.Resp);
                        if (response.Data.Count() > 0)
                        {
                            responseObjectCustomer = response.Data;
                            ViewBag.Customer = new SelectList(responseObjectCustomer.ToList(), "CustomerId", "FullName");
                        }
                        else
                        {
                            List<Customer> responseObject = new List<Customer>();
                            ViewBag.Customer = new SelectList(responseObject, "CustomerId", "FullName");
                        }
                    }
                    else
                    {
                        List<Customer> responseObject = new List<Customer>();
                        ViewBag.Customer = new SelectList(responseObject, "CustomerId", "FullName");
                    }
                }
                else
                {
                    ViewBag.Customer = new SelectList(FoundSession_Result.ToList(), "CustomerId", "FullName");
                }


                var getterminalname = HttpContext.Session.GetString("TerminalName");
                if (getterminalname != null)
                {
                    ViewBag.TerminalName = getterminalname;
                }
                else
                {
                    ViewBag.TerminalName = "NA";
                }

                var getcashier = HttpContext.Session.GetString("CashierName");
                if (getcashier != null)
                {
                    ViewBag.CashierName = getcashier;
                }
                else
                {
                    ViewBag.CashierName = "NA";
                }

                List<Product> responseObjectItemGet = new List<Product>();
                var FoundSessionItemGet = HttpContext.Session.GetString("loadedItemGet");
                List<Product> FoundSession_Result_ItemGet = new List<Product>();
                if (FoundSessionItemGet != null)
                {
                    FoundSession_Result_ItemGet = JsonConvert.DeserializeObject<List<Product>>(FoundSessionItemGet);
                }
                if (FoundSession_Result_ItemGet != null && FoundSession_Result_ItemGet.Count() < 1)
                {
                    SResponse ItemGet = RequestSender.Instance.CallAPI("api",
                   "Inventory/ItemGet", "GET");
                    if (ItemGet.Status && (ItemGet.Resp != null) && (ItemGet.Resp != ""))
                    {
                        ResponseBack<List<Product>> response =
                                     JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ItemGet.Resp);
                        if (response.Data.Count() > 0)
                        {
                            HttpContext.Session.SetString("loadedItemGet", JsonConvert.SerializeObject(response.Data));
                            List<Product> responseObject = response.Data;
                            ViewBag.Items = new SelectList(responseObject.ToList(), "Id", "Name");
                        }
                        else
                        {

                            List<Product> responseObject = new List<Product>();
                            ViewBag.Items = new SelectList(responseObject, "Id", "Name");
                        }
                    }
                }
                else
                {
                    ViewBag.Items = new SelectList(FoundSession_Result_ItemGet, "Id", "Name");
                }

                List<Sale> QuantityUnit = new List<Sale>
                    {
                        new Sale{QuantityId=1,QuantityUnit="Loose"},
                        new Sale{QuantityId=2,QuantityUnit="Pack"}
                    };
                ViewBag.QuantityUnit = QuantityUnit;

                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        [HttpPost]
        public IActionResult AddSaleOrderList([FromBody] List<JsonSale> SaleDetails)
        {
            Sale model = null;
            List<Sale> modellist = new List<Sale>();

            foreach (JsonSale sale in SaleDetails)
            {

                model = new Sale();
                model.ItemId = Convert.ToInt32(sale.ItemId);
                model.CustomerId = Convert.ToInt32(sale.CustomerId);
                model.QuantityUnit = sale.QuantityUnit;
                model.ItemCode = sale.ItemCode;
                model.BarCode = sale.BarCode;
                model.QuantityUnit = sale.QuantityUnit;
                model.UnitPrice = sale.UnitPrice;
                model.TotalAmount = sale.TotalAmount;
                model.ItemName = sale.ItemName;
                model.Quantity = sale.Quantity;
                model.OnCash = Convert.ToBoolean(sale.OnCash);
                model.OnCredit = Convert.ToBoolean(sale.OnCredit);
                model.InvoiceNumber = sale.InvoiceNumber;

                if (sale.CashierName == "NA")
                {
                    model.CashierName = "Admin";
                }
                else
                {
                    model.CashierName = sale.CashierName;
                }
                model.TerminalNumber = sale.TerminalNumber;

                modellist.Add(model);
                //await addPurchase(model);
            }
            var body = JsonConvert.SerializeObject(modellist);


            SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SaleCreate", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                TempData["Msg"] = "Add Successfully";
                return Content("true");
            }
            else
            {
                TempData["Msg"] = resp.Resp + " " + "Unable To Update";
                return Content("false");
            }


        }
        public JsonResult ItemGetByIDWithStock(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult CheckCreditSalesAvailableForCustomerID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/CheckCreditSalesAvailableForCustomerID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<string>>(ress.Resp);
                if (response.Data == "CreditNotAllow")
                {
                    Msg = "CreditNotAllow";
                    return Json(Msg);

                }
                if (response.Data == "AskSalesManager")
                {
                    Msg = "AskSalesManager";
                    return Json(Msg);
                }
                if (response.Data == "AskSupervisor")
                {
                    Msg = "AskSupervisor";
                    return Json(Msg);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult AuthenticateLoginSupervisor(int AccessPin)
        {

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/AuthenticateLoginSupervisor" + "/" + AccessPin, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<string>>(ress.Resp);
                if (response.Data == "RecordFound")
                {
                    Msg = "RecordFound";
                    return Json(Msg);

                }
                if (response.Data == "RecordNotFound")
                {
                    Msg = "RecordNotFound";
                    return Json(Msg);
                }

                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult AuthenticateLoginSalesMan(int AccessPin)
        {


            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/AuthenticateLoginSupervisor" + "/" + AccessPin, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<string>>(ress.Resp);
                if (response.Data == "RecordFound")
                {
                    Msg = "RecordFound";
                    return Json(Msg);

                }
                if (response.Data == "RecordNotFound")
                {
                    Msg = "RecordNotFound";
                    return Json(Msg);
                }

                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public IActionResult AllSaleInvoices()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/SaleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Sale>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Sale>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {

                        List<Sale> responseObject = response.Data;

                        var query = responseObject.GroupBy(x => x.InvoiceNumber)
                          .Select(g => g.FirstOrDefault())
                          .ToList();

                        return View(query);
                    }
                    else
                    {
                        TempData["response"] = "No SaleInvoices List Found. Please Enter Sale First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        public JsonResult GetSaleOrderDetail(string InvoiceNumber = "")
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/SaleGetByInvoiceNumber" + "/" + InvoiceNumber, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Sale>> response = JsonConvert.DeserializeObject<ResponseBack<List<Sale>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        //new merge on sunday 21nov 
        public IActionResult SalesInvoice()
        {

            try
            {
                PosSale model = new PosSale();
                SResponse respcustomer = RequestSender.Instance.CallAPI("api", "Inventory/SaleGet", "GET");
                if (respcustomer.Status && (respcustomer.Resp != null) && (respcustomer.Resp != ""))
                {
                    ResponseBack<List<PosSale>> record = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(respcustomer.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        PosSale newcustomer = new PosSale();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {

                            int large, small;
                            int POSSALEID = 0;
                            large = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count(); i++)
                            {
                                if (record.Data[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) > large)
                                    {
                                        POSSALEID = Convert.ToInt32(record.Data[i].PossaleId);
                                        large = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        //if (large < 2)
                                        //{
                                        POSSALEID = Convert.ToInt32(record.Data[i].PossaleId);
                                        //}
                                    }
                                }
                            }
                            newcustomer = record.Data.ToList().Where(x => x.PossaleId == POSSALEID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.InvoiceNumber != null)
                                {

                                    int code = 0;
                                    var VcodeSplit = newcustomer.InvoiceNumber.Split('-');

                                    code = Convert.ToInt32(VcodeSplit[1]) + 1;

                                    long checknumber = Convert.ToInt64(VcodeSplit[1]);
                                    if (checknumber > 9)
                                    {
                                        fullcode = "0000000-00" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        fullcode = "0000000-0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 999)
                                    {
                                        fullcode = "0000000-" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit[0]) + 1;

                                        fullcode = "000000" + Convert.ToString(ndcode) + "-" + "9999";
                                    }
                                    else
                                    {
                                        fullcode = "0000000-000" + Convert.ToString(code);
                                    }
                                    //fullcode = "0000000" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "0000000" + "-" + "0001";
                                }
                                ViewBag.InvoiceNumber = fullcode;
                            }
                            else
                            {
                                fullcode = "0000000" + "-" + "0001";
                                ViewBag.InvoiceNumber = fullcode;
                            }
                            model.InvoiceNumber = ViewBag.InvoiceNumber;

                            //int large, small;
                            //int CustomerInfoID = 0;
                            //large = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            //small = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            //for (int i = 0; i < record.Data.Count; i++)
                            //{
                            //    if (record.Data[i].InvoiceNumber != null)
                            //    {
                            //        var t = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                            //        if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) > large)
                            //        {
                            //            CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                            //            large = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);

                            //        }
                            //        else if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) < small)
                            //        {
                            //            small = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                            //        }
                            //        else
                            //        {
                            //            if (large < 2)
                            //            {
                            //                CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                            //            }
                            //        }
                            //    }
                            //}
                            //newcustomer = record.Data.ToList().Where(x => x.PossaleId == CustomerInfoID).FirstOrDefault();
                            //if (newcustomer != null)
                            //{
                            //    if (newcustomer.InvoiceNumber != null)
                            //    {
                            //        var VcodeSplit = newcustomer.InvoiceNumber.Split('-');
                            //        int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                            //        fullcode = "00-" + Convert.ToString(code);
                            //    }
                            //    else
                            //    {
                            //        fullcode = "00-" + "1";
                            //    }
                            //}
                            //else
                            //{
                            //    fullcode = "00-" + "1";
                            //}
                        }
                        else
                        {
                            //fullcode = "00-" + "1";
                            fullcode = "0000000" + "-" + "0001";
                        }

                        ViewBag.InvoiceNumber = fullcode;
                    }
                    else
                    {
                        ViewBag.InvoiceNumber = "0000000" + "-" + "0001";
                    }
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }
                else
                {
                    ViewBag.InvoiceNumber = "0000000" + "-" + "0001";
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }


        }

        public JsonResult GetItemStockFinancialByItemNumber(string itemnumber)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetWithStockAndFinancialByItemnumber/" + itemnumber, "GET");
            var Msg = "";

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult SearchDataByCompanyName(string name)
        {
            if (name != null)
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/CustomerInformationByCompany" + "/" + name, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<CustomerInformation> response =
                                 JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(resp.Resp);
                    CustomerInformation cusinfo = response.Data;
                    return Json(cusinfo);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }
        public JsonResult AutoCompleteSearchCustomerInfo()
        {
            string Msg = "";

            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/CustomerInformationGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {

                ResponseBack<List<CustomerInformation>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<CustomerInformation> responseObject = response.Data;

                    return Json(responseObject);

                }
                else
                {
                    return Json(Msg);
                }

            }

            return Json(Msg);
        }
        public JsonResult GetCompanyByID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/CustomerInformationByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "NotFound.";
                }
            }
            return Json(Msg);
        }




        [HttpPost]
        public IActionResult OngoingSale([FromBody] List<JsonOngoingSaleInvoice> SaleDetails)
        {
            try
            {
                OngoingSaleInvoice model = null;
                List<OngoingSaleInvoice> modellist = new List<OngoingSaleInvoice>();

                foreach (JsonOngoingSaleInvoice sale in SaleDetails)
                {


                    model = new OngoingSaleInvoice();
                    if (sale.ItemId != null && sale.ItemId != "undefined")
                    {
                        model.ItemId = Convert.ToInt32(sale.ItemId);
                    }
                    if (sale.CustomerId != null && sale.CustomerId != "undefined")
                    {
                        string trimmed = (sale.CustomerId as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.OnCredit != null && sale.OnCredit != "undefined")
                    {
                        model.OnCredit = Convert.ToBoolean(sale.OnCredit);
                    }
                    else
                    {
                        model.OnCredit = false;
                    }

                    if (sale.OnCash != null && sale.OnCash != "undefined")
                    {
                        model.OnCash = Convert.ToBoolean(sale.OnCash);
                    }
                    else
                    {
                        model.OnCash = false;
                    }
                    if (sale.Quantity != null && sale.Quantity != "undefined")
                    {
                        model.Quantity = sale.Quantity;
                    }
                    if (sale.ItemNumber != null && sale.ItemNumber != "undefined")
                    {
                        model.ItemNumber = sale.ItemNumber;
                    }
                    if (sale.ItemName != null && sale.ItemName != "undefined")
                    {
                        model.ItemName = sale.ItemName;
                    }
                    if (sale.Total != null && sale.Total != "undefined")
                    {
                        string trimmed = (sale.Total as string).Trim('$');
                        model.Total = trimmed;
                    }
                    if (sale.Price != null && sale.Price != "undefined")
                    {
                        string trimmed = (sale.Price as string).Trim('$');
                        model.Price = trimmed;
                    }
                    if (sale.AmountRetail != null && sale.AmountRetail != "undefined")
                    {
                        string trimmed = (sale.AmountRetail as string).Trim('$');
                        model.AmountRetail = trimmed;
                    }
                    if (sale.CurrentInvoiceNumber != null && sale.CurrentInvoiceNumber != "undefined")
                    {
                        string trimmed = (sale.CurrentInvoiceNumber as string).Trim('$');
                        model.CurrentInvoiceNumber = trimmed;
                    }
                    if (sale.InvoiceTotal != null && sale.InvoiceTotal != "undefined")
                    {
                        string trimmed = (sale.InvoiceTotal as string).Trim('$');
                        model.InvoiceTotal = trimmed;
                    }
                    if (sale.CustomerNumber != null && sale.CustomerNumber != "undefined")
                    {
                        string trimmed = (sale.CustomerNumber as string).Trim('$');
                        model.CustomerNumber = trimmed;
                    }

                    if (sale.Discount != null && sale.Discount != "undefined")
                    {
                        string trimmed = (sale.Discount as string).Trim('$');
                        model.Discount = trimmed;
                    }
                    if (sale.Other != null && sale.Other != "undefined")
                    {
                        string trimmed = (sale.Other as string).Trim('$');
                        model.Other = trimmed;
                    }
                    if (sale.Tax != null && sale.Tax != "undefined")
                    {
                        string trimmed = (sale.Tax as string).Trim('$');
                        model.Tax = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Count != null && sale.Count != "undefined")
                    {
                        string trimmed = (sale.Count as string).Trim('$');
                        model.Count = trimmed;
                    }
                    modellist.Add(model);
                }
                var body = JsonConvert.SerializeObject(modellist);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/OngoingSaleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Add Successfully";
                    return Json(resp.Status);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To add";
                    return Json(resp.Status);
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }

        }
        public JsonResult GetOnGoingSale()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/OnGoingSaleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<OngoingSaleInvoice>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<OngoingSaleInvoice>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {

                        List<OngoingSaleInvoice> responseObject = response.Data;

                        var query = responseObject.GroupBy(x => x.CurrentInvoiceNumber)
                          .Select(g => g.FirstOrDefault())
                          .ToList();

                        return Json(query);
                    }
                    else
                    {
                        TempData["response"] = "No OnGoingSaleGet List Found. Please Enter Sale First.";
                    }
                }
                return Json("NotFound");
            }
            catch (Exception ex)
            {
                return Json("NotFound");
            }
        }
        public JsonResult PostedSales()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/SaleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<PosSale>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {

                        List<PosSale> responseObject = response.Data;

                        var query = responseObject.GroupBy(x => x.InvoiceNumber)
                          .Select(g => g.FirstOrDefault())
                          .ToList();

                        return Json(query);
                    }
                    else
                    {
                        TempData["response"] = "No Sale List Found. Please Enter Sale First.";
                    }
                }
                return Json("NotFound");
            }
            catch (Exception ex)
            {
                return Json("NotFound");
            }
        }

        [HttpPost]
        public IActionResult AddSalePayment([FromBody] List<JsonPayment> salepayment)
        {
            try
            {
                Payment model = null;
                List<Payment> modellist = new List<Payment>();

                foreach (JsonPayment sale in salepayment)
                {
                    model = new Payment();
                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        model.InvoiceNumber = sale.InvoiceNumber;
                    }
                    if (sale.TotalBill != null && sale.TotalBill != "undefined")
                    {
                        string trimmed = (sale.TotalBill as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.CustomerId != null && sale.CustomerId > 0)
                    {
                        string trimmed = (sale.TotalBill as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.CustomerName != null && sale.CustomerName != "undefined")
                    {
                        string trimmed = (sale.TotalBill as string).Trim('$');
                        model.CustomerName = trimmed;

                    }

                    if (sale.CustomerAccountNumber != null && sale.CustomerAccountNumber != "undefined")
                    {
                        string trimmed = (sale.CustomerAccountNumber as string).Trim('$');
                        model.CustomerAccountNumber = trimmed;
                    }
                    if (sale.TotalPaid != null && sale.TotalPaid != "undefined")
                    {
                        string trimmed = (sale.TotalPaid as string).Trim('$');
                        model.TotalPaid = trimmed;
                    }
                    if (sale.TotalAlloc != null && sale.TotalAlloc != "undefined")
                    {
                        string trimmed = (sale.TotalAlloc as string).Trim('$');
                        model.TotalAlloc = trimmed;
                    }

                    if (sale.Change != null && sale.Change != "undefined")
                    {
                        string trimmed = (sale.Change as string).Trim('$');
                        model.Change = trimmed;
                    }
                    if (sale.IsBalanceToChange != null)
                    {
                        model.IsBalanceToChange = Convert.ToBoolean(sale.IsBalanceToChange);
                    }
                    else
                    {
                        model.IsBalanceToChange = false;
                    }
                    if (sale.Balance != null && sale.Balance != "undefined")
                    {
                        string trimmed = (sale.Balance as string).Trim('$');
                        model.Balance = trimmed;
                    }
                    if (sale.IsEmailCopy != null)
                    {
                        model.IsEmailCopy = Convert.ToBoolean(sale.IsEmailCopy);
                    }
                    else
                    {
                        model.IsEmailCopy = false;
                    }
                    if (sale.IsTextCopy != null)
                    {
                        model.IsTextCopy = Convert.ToBoolean(sale.IsTextCopy);
                    }
                    else
                    {
                        model.IsTextCopy = false;
                    }
                    if (sale.IsStandardReceipt != null)
                    {
                        model.IsStandardReceipt = Convert.ToBoolean(sale.IsStandardReceipt);
                    }
                    else
                    {
                        model.IsStandardReceipt = false;
                    }
                    if (sale.OrderBy != null && sale.OrderBy != "undefined")
                    {
                        string trimmed = (sale.OrderBy as string).Trim('$');
                        model.OrderBy = trimmed;
                    }
                    if (sale.ByUser != null && sale.ByUser != "undefined")
                    {

                        model.ByUser = sale.ByUser;
                    }
                    if (sale.OrderBy != null && sale.OrderBy != "undefined")
                    {

                        model.OrderBy = sale.OrderBy;
                    }
                    if (sale.ByUserId != null && sale.OrderBy != "undefined")
                    {

                        model.ByUserId = sale.ByUserId;
                    }

                    if (sale.JsonpaymentDetail.AmountAlloc != null && sale.JsonpaymentDetail.AmountAlloc != "undefined")
                    {

                        model.paymentDetail.AmountAlloc = sale.JsonpaymentDetail.AmountAlloc;
                    }
                    if (sale.JsonpaymentDetail.AmountPaid != null && sale.JsonpaymentDetail.AmountPaid != "undefined")
                    {

                        model.paymentDetail.AmountPaid = sale.JsonpaymentDetail.AmountPaid;
                    }
                    if (sale.JsonpaymentDetail.PaymentType != null && sale.JsonpaymentDetail.PaymentType != "undefined")
                    {

                        model.paymentDetail.PaymentType = sale.JsonpaymentDetail.PaymentType;
                    }
                    if (sale.JsonpaymentDetail.CkcardNumber != null && sale.JsonpaymentDetail.CkcardNumber != "undefined")
                    {

                        model.paymentDetail.CkcardNumber = sale.JsonpaymentDetail.CkcardNumber;
                    }

                    modellist.Add(model);
                }
                var body = JsonConvert.SerializeObject(modellist);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Cash/AddPayment", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Add Successfully";
                    return Json(resp.Status);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To add";
                    return Json(resp.Status);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public IActionResult GetItemListByName(string itemName)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/ItemGetWithItemName/" + itemName, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        var Productlist = new SelectList(record.Data, "ID", "Value");
                        return Json(Productlist);
                    }
                    else
                    {
                        TempData["Msg"] = respEmp.Resp + " " + "No Product Found";
                        return Json(respEmp.Status);
                    }
                }
                else
                {
                    TempData["Msg"] = respEmp.Resp + " " + "Unable To search";
                    return Json(respEmp.Status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult GetAllItem()
        {
            try
            {

                //if (GlobalPOS.listcart.Count() > 0)
                //{
                //    var productDropDown = GlobalPOS.listcart.Select(x => new { ID = x.ItemNumber, Value = x.Name });
                //    var Productlist = new SelectList(productDropDown, "ID", "Value");
                //    return Json(Productlist);
                //}

                List<Product> responseObject = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {

                    var productDropDown = FoundSession_Result.Select(x => new { ID = x.ItemNumber, Value = x.Name });
                    var Productlist = new SelectList(productDropDown, "ID", "Value");
                    return Json(Productlist);
                }
                else
                {
                    SResponse respEmp = RequestSender.Instance.CallAPI("api",
                                   "Inventory/GetAllItem/", "GET");
                    if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                    {
                        ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(respEmp.Resp);
                        if (record != null && record.Data.Count() > 0)
                        {
                            var productDropDown = record.Data.Select(x => new { ID = x.ItemNumber, Value = x.Name });
                            var Productlist = new SelectList(productDropDown, "ID", "Value");
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(record.Data));

                            return Json(Productlist);
                        }
                        else
                        {
                            TempData["Msg"] = respEmp.Resp + " " + "No Product Found";
                            return Json(respEmp.Status);
                        }
                    }
                    else
                    {
                        TempData["Msg"] = respEmp.Resp + " " + "Unable To search";
                        return Json(respEmp.Status);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult GetAllItemReturnWithId()
        {
            try
            {
                //if (GlobalPOS.listcart.Count() > 0)
                //{
                //    var productDropDown = GlobalPOS.listcart.Select(x => new { ID = x.Id, Value = x.Name });
                //    var Productlist = new SelectList(productDropDown, "ID", "Value");
                //    return Json(Productlist);
                //}

                List<Product> responseObject = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    var productDropDown = FoundSession_Result.Select(x => new { ID = x.Id, Value = x.Name });
                    var Productlist = new SelectList(productDropDown, "ID", "Value");
                    return Json(Productlist);
                }
                else
                {
                    SResponse respEmp = RequestSender.Instance.CallAPI("api",
                                    "Inventory/GetAllItem/", "GET");
                    if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                    {
                        ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(respEmp.Resp);
                        if (record != null && record.Data.Count() > 0)
                        {
                            var productDropDown = record.Data.Select(x => new { ID = x.Id, Value = x.Name });
                            var Productlist = new SelectList(productDropDown, "ID", "Value");
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(record.Data));
                            return Json(Productlist);
                        }
                        else
                        {
                            TempData["Msg"] = respEmp.Resp + " " + "No Product Found";
                            return Json(respEmp.Status);
                        }
                    }
                    else
                    {
                        TempData["Msg"] = respEmp.Resp + " " + "Unable To search";
                        return Json(respEmp.Status);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public IActionResult GetSttax()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Sale/GetAllSttax", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Sttax>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Sttax>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        return Json(response.Data);
                    }
                    else
                    {
                        TempData["response"] = "No SaleInvoices List Found. Please Enter Sale First.";
                    }
                }
                return Json("Failed");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult CheckSupervisorCreditAmount(string customerId, string accessPin, string creditAmount)
        {
            try
            {
                var Msg = "";
                Supervisor supervisor = new Supervisor();
                supervisor.AccessPin = accessPin;
                supervisor.CreditLimit = creditAmount;
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse ress = RequestSender.Instance.CallAPI("api", "Sale/CheckSupervisorCreditAmount/" + int.Parse(customerId), "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Status == 1)
                    {
                        Msg = "Success.";
                        return Json(response);

                    }
                    if (response.Status == 13)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 5)
                    {
                        Msg = "InvlaidPin";
                        return Json(response);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                        return Json("Error");
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public IActionResult PosCheckSupervisorCreditAmount(string customerId, string accessPin, string creditAmount)
        {
            try
            {
                var Msg = "";
                Supervisor supervisor = new Supervisor();
                supervisor.AccessPin = accessPin;
                supervisor.CreditLimit = creditAmount;
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse ress = RequestSender.Instance.CallAPI("api", "Sale/posCheckSupervisorCreditAmount/" + int.Parse(customerId), "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Status == 1)
                    {
                        Msg = "Success.";
                        return Json(response);

                    }
                    if (response.Status == 13)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 14)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 15)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 16)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 5)
                    {
                        Msg = "InvlaidPin";
                        return Json(response);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                        return Json("Error");
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public IActionResult CheckSupervisorCreditAmountUpdate(string customerId, string accessPin, string creditAmount, string possaleinvoive)
        {
            try
            {
                var Msg = "";
                Supervisor supervisor = new Supervisor();
                supervisor.AccessPin = accessPin;
                supervisor.CreditLimit = creditAmount;
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse ress = RequestSender.Instance.CallAPI("api", "Sale/CheckSupervisorCreditAmountforUpdate/" + int.Parse(customerId) + "/" + possaleinvoive, "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Status == 1)
                    {
                        Msg = "Success";
                        return Json(response);

                    }
                    if (response.Status == 13)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 5)
                    {
                        Msg = "InvlaidPin";
                        return Json(response);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                        return Json("Error");
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult PosCheckSupervisorCreditAmountUpdate(string customerId, string accessPin, string creditAmount, string possaleinvoive)
        {
            try
            {
                var Msg = "";
                Supervisor supervisor = new Supervisor();
                supervisor.AccessPin = accessPin;
                supervisor.CreditLimit = creditAmount;
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse ress = RequestSender.Instance.CallAPI("api", "Sale/PosCheckSupervisorCreditforUpdate/" + int.Parse(customerId) + "/" + possaleinvoive, "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Status == 1)
                    {
                        Msg = "Success";
                        return Json(response);

                    }
                    if (response.Status == 13)
                    {
                        Msg = "ExceedLimit";
                        return Json(response);
                    }
                    if (response.Status == 5)
                    {
                        Msg = "InvlaidPin";
                        return Json(response);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                        return Json("Error");
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult CashierSalesInvoice()
        {
            try
            {
                PosSale model = new PosSale();
                SResponse respcustomer = RequestSender.Instance.CallAPI("api", "Inventory/SaleGet", "GET");
                if (respcustomer.Status && (respcustomer.Resp != null) && (respcustomer.Resp != ""))
                {
                    ResponseBack<List<PosSale>> record = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(respcustomer.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        PosSale newcustomer = new PosSale();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int CustomerInfoID = 0;
                            large = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count; i++)
                            {
                                if (record.Data[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) > large)
                                    {
                                        CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                                        large = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                                        }
                                    }
                                }
                            }
                            newcustomer = record.Data.ToList().Where(x => x.PossaleId == CustomerInfoID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.InvoiceNumber != null)
                                {
                                    var VcodeSplit = newcustomer.InvoiceNumber.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "00-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "00-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "00-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "00-" + "1";
                        }
                        ViewBag.InvoiceNumber = fullcode;
                    }
                    else
                    {
                        ViewBag.InvoiceNumber = "00-" + "1";
                    }
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }
                else
                {
                    ViewBag.InvoiceNumber = "00-" + "1";
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IActionResult AddSalePos([FromBody] List<JsonPosSale> SaleDetails)
        {
            try
            {
                PosSale model = null;
                List<PosSale> modellist = new List<PosSale>();
                double TotalDiscount = 0;
                foreach (JsonPosSale sale in SaleDetails)
                {
                    model = new PosSale();
                    if (sale.ItemId != null && sale.ItemId != "undefined")
                    {
                        if (sale.ItemId.Contains("-"))
                        {
                            sale.ItemId = sale.ItemId.Split('-')[1];
                        }
                        model.ItemId = Convert.ToInt32(sale.ItemId);
                    }
                    if (sale.CustomerId != null && sale.CustomerId != "undefined")
                    {
                        string trimmed = (sale.CustomerId as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.OnCredit != null && sale.OnCredit != "undefined")
                    {
                        model.OnCredit = Convert.ToBoolean(sale.OnCredit);
                    }
                    else
                    {
                        model.OnCredit = false;
                    }
                    if (sale.OnCash != null && sale.OnCash != "undefined")
                    {
                        model.OnCash = Convert.ToBoolean(sale.OnCash);
                    }
                    else
                    {
                        model.OnCash = false;
                    }
                    if (sale.GetSaleDiscount != null && sale.GetSaleDiscount != "undefined")
                    {
                        model.GetSaleDiscount = Convert.ToBoolean(sale.GetSaleDiscount);
                    }
                    else
                    {
                        model.GetSaleDiscount = false;
                    }
                    if (sale.Quantity != null && sale.Quantity != "undefined")
                    {
                        model.Quantity = sale.Quantity;
                    }
                    if (sale.ItemNumber != null && sale.ItemNumber != "undefined")
                    {
                        model.ItemNumber = sale.ItemNumber;
                    }
                    if (sale.ItemName != null && sale.ItemName != "undefined")
                    {
                        model.ItemName = sale.ItemName;
                    }
                    if (sale.ItemDescription != null && sale.ItemDescription != "undefined")
                    {
                        model.ItemDescription = sale.ItemDescription;
                    }

                    if (sale.Total != null && sale.Total != "undefined")
                    {
                        string trimmed = (sale.Total as string).Trim('$');
                        model.Total = trimmed;
                    }
                    if (sale.Price != null && sale.Price != "undefined")
                    {
                        string trimmed = (sale.Price as string).Trim('$');
                        model.Price = trimmed;
                    }
                    if (sale.AmountRetail != null && sale.AmountRetail != "undefined")
                    {
                        string trimmed = (sale.AmountRetail as string).Trim('$');
                        model.AmountRetail = trimmed;
                    }
                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        string trimmed = (sale.InvoiceNumber as string).Trim('$');
                        model.InvoiceNumber = trimmed;
                    }
                    if (sale.InvoiceTotal != null && sale.InvoiceTotal != "undefined")
                    {
                        string trimmed = (sale.InvoiceTotal as string).Trim('$');
                        model.InvoiceTotal = trimmed;
                    }
                    if (sale.CustomerNumber != null && sale.CustomerNumber != "undefined")
                    {
                        string trimmed = (sale.CustomerNumber as string).Trim('$');
                        model.CustomerNumber = trimmed;
                    }

                    if (sale.Other != null && sale.Other != "undefined")
                    {
                        string trimmed = (sale.Other as string).Trim('$');
                        model.Other = trimmed;
                    }
                    if (sale.Tax != null && sale.Tax != "undefined")
                    {
                        string trimmed = (sale.Tax as string).Trim('$');
                        model.Tax = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Count != null && sale.Count != "undefined")
                    {
                        string trimmed = (sale.Count as string).Trim('$');
                        model.Count = trimmed;
                    }
                    if (sale.RingerQuantity != null && sale.RingerQuantity != "undefined" && sale.RingerQuantity != "")
                    {
                        model.RingerQuantity = sale.RingerQuantity;

                    }
                    if (sale.OutDiscount != null && sale.OutDiscount != "undefined")
                    {
                        if (sale.OutDiscount == "" || sale.OutDiscount == "0.00%")
                        {
                            model.OutDiscount = "0.00";
                        }
                        else
                        {
                            if(Convert.ToDouble(model.Price) < 0)
                            {
                                model.Price = "0";
                            }
                            if (Convert.ToDouble(model.RingerQuantity) < 0)
                            {
                                model.RingerQuantity = "1";
                            }
                            string trimmed = (sale.OutDiscount as string).Trim('%');
                            model.OutDiscount = trimmed;
                            var ValueItem = Convert.ToDouble(model.Price) * Convert.ToDouble(model.RingerQuantity);
                            var CalValue = Convert.ToDouble((Convert.ToDouble(model.OutDiscount) / 100) * ValueItem);
                            TotalDiscount += Math.Round(CalValue,2);
                        }
                    }
                    if (sale.InDiscount != null && sale.InDiscount != "undefined")
                    {
                        if(sale.InDiscount == "" || sale.InDiscount == "$0.00")
                        {
                            model.InDiscount = "0.00";
                        }
                        else
                        {
                            string trimmed = (sale.InDiscount as string).Trim('$');
                            model.InDiscount = trimmed;
                            TotalDiscount += Math.Round(Convert.ToDouble(model.InDiscount), 2);
                        }
                    }
                    if (sale.CustomerName != null && sale.CustomerName != "undefined")
                    {
                        model.CustomerName = sale.CustomerName;

                    }
                    if (sale.IsOpen != null)
                    {
                        model.IsOpen = sale.IsOpen;

                    }
                    if (sale.IsClose != null)
                    {
                        model.IsClose = sale.IsClose;

                    }
                    if (sale.SupervisorId != null && sale.SupervisorId != "undefined" && sale.SupervisorId != "")
                    {
                        model.SupervisorId = int.Parse(sale.SupervisorId);

                    }
                  
                    if (sale.ShipmentLimit != null && sale.ShipmentLimit != "undefined" && sale.ShipmentLimit != "")
                    {
                        model.ShipmentLimit = sale.ShipmentLimit;

                    }
                    if (sale.plateNo != null && sale.plateNo != "undefined" && sale.plateNo != "")
                    {
                        model.plateNo = sale.plateNo;

                    }
                    if (sale.drivingLicense != null && sale.drivingLicense != "undefined" && sale.drivingLicense != "")
                    {
                        model.drivingLicense = sale.drivingLicense;

                    }
                    if (sale.InUnits != null && sale.InUnits != "undefined" && sale.InUnits != "")
                    {
                        model.InUnits = sale.InUnits;

                    }
                    if (sale.OutUnits != null && sale.OutUnits != "undefined" && sale.OutUnits != "")
                    {
                        model.OutUnits = sale.OutUnits;

                    }

                    if (sale.Charges != null && sale.Charges != "undefined" && sale.Charges != "")
                    {
                        model.Charges = sale.Charges;

                    } 
                    if (sale.SubTotal != null && sale.SubTotal != "undefined" && sale.SubTotal != "")
                    {
                        if (sale.SubTotal == "" || sale.SubTotal == "$0.00")
                        {
                            model.SubTotal = "0.00";
                        }
                        else
                        {
                            string trimmed = (sale.SubTotal as string).Trim('$');
                            model.SubTotal = trimmed;
                        }
                    }

                    modellist.Add(model);
                }
                if (TotalDiscount != 0 && TotalDiscount > 0)
                {
                    modellist[0].Discount = TotalDiscount.ToString();
                }
                else
                {
                    modellist[0].Discount = "0";
                }
                SaleInfoModel obj = new SaleInfoModel();
                obj.BusinessAddress = SaleDetails[0].BusinessAddress;
                obj.CustomerEmail = SaleDetails[0].CustomerEmail;
                obj.AddToMailList = SaleDetails[0].AddToMailList;
                var saleInfo = JsonConvert.SerializeObject(obj);

                var body = JsonConvert.SerializeObject(modellist);
                //TempData["saleorder"] = body;
                //TempData["saleorderInfo"] = saleInfo;
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SaleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {   //Send Mail If Add to Mail list is true
                    if (obj.AddToMailList)
                    {
                        MailRequest mail = new MailRequest();
                        mail.ToEmail = obj.CustomerEmail;
                        mail.Subject = "Invoice Generated";
                        mail.Body = "Check mail";
                        var mailbody = JsonConvert.SerializeObject(mail);
                        //SResponse mailresp = RequestSender.Instance.CallAPI("api", "Sale/MailToCustomer", "POST", mailbody);
                        //if (mailresp.Status && (mailresp.Resp != null) && (mailresp.Resp != ""))
                        //{
                        //    TempData["Msg"] = "Mail Sent Successfully";
                        //}
                        //else
                        //{
                        //    TempData["Msg"] = resp.Resp + " " + "Unable To Sent Mail";
                        //}
                    }


                    SResponse resp1 = RequestSender.Instance.CallAPI("api", "Inventory/SaleHistoryCreate", "POST", body);
                    if (resp1.Status && (resp1.Resp != null) && (resp1.Resp != ""))
                    {   //Send Mail If Add to Mail list is true

                        if (SaleDetails[0].IsPrintinvoice)
                        {
                            return Json(true);
                        }
                        TempData["Msg"] = "Add Successfully";
                        return Json(resp.Status);
                    }
                    else
                    {
                        TempData["Msg"] = resp.Resp + " " + "Unable To add";
                        return Json(resp.Status);
                    }

                    //if (SaleDetails[0].IsPrintinvoice)
                    //{
                    //    return Json(true);
                    //}
                    //TempData["Msg"] = "Add Successfully";
                    //return Json(resp.Status);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To add";
                    return Json(resp.Status);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }





        public IActionResult UpdateSalePos([FromBody] List<JsonPosSale> SaleDetails)
        {
            try
            {
                PosSale model = null;
                List<PosSale> modellist = new List<PosSale>();

                foreach (JsonPosSale sale in SaleDetails)
                {
                    model = new PosSale();
                    if (sale.ItemId != null && sale.ItemId != "undefined")
                    {
                        if (sale.ItemId.Contains("-"))
                        {
                            sale.ItemId = sale.ItemId.Split('-')[1];
                        }
                        model.ItemId = Convert.ToInt32(sale.ItemId);
                    }
                    if (sale.CustomerId != null && sale.CustomerId != "undefined")
                    {
                        string trimmed = (sale.CustomerId as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.OnCredit != null && sale.OnCredit != "undefined")
                    {
                        model.OnCredit = Convert.ToBoolean(sale.OnCredit);
                    }
                    else
                    {
                        model.OnCredit = false;
                    }
                    if (sale.OnCash != null && sale.OnCash != "undefined")
                    {
                        model.OnCash = Convert.ToBoolean(sale.OnCash);
                    }
                    else
                    {
                        model.OnCash = false;
                    }
                    if (sale.GetSaleDiscount != null && sale.GetSaleDiscount != "undefined")
                    {
                        model.GetSaleDiscount = Convert.ToBoolean(sale.GetSaleDiscount);
                    }
                    else
                    {
                        model.GetSaleDiscount = false;
                    }
                    if (sale.Quantity != null && sale.Quantity != "undefined")
                    {
                        model.Quantity = sale.Quantity;
                    }
                    if (sale.ItemNumber != null && sale.ItemNumber != "undefined")
                    {
                        model.ItemNumber = sale.ItemNumber;
                    }
                    if (sale.ItemName != null && sale.ItemName != "undefined")
                    {
                        model.ItemName = sale.ItemName;
                    }
                    if (sale.ItemDescription != null && sale.ItemDescription != "undefined")
                    {
                        model.ItemDescription = sale.ItemDescription;
                    }

                    if (sale.Total != null && sale.Total != "undefined")
                    {
                        string trimmed = (sale.Total as string).Trim('$');
                        model.Total = trimmed;
                    }
                    if (sale.Price != null && sale.Price != "undefined")
                    {
                        string trimmed = (sale.Price as string).Trim('$');
                        model.Price = trimmed;
                    }
                    if (sale.AmountRetail != null && sale.AmountRetail != "undefined")
                    {
                        string trimmed = (sale.AmountRetail as string).Trim('$');
                        model.AmountRetail = trimmed;
                    }
                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        string trimmed = (sale.InvoiceNumber as string).Trim('$');
                        model.InvoiceNumber = trimmed;
                    }
                    if (sale.InvoiceTotal != null && sale.InvoiceTotal != "undefined")
                    {
                        string trimmed = (sale.InvoiceTotal as string).Trim('$');
                        model.InvoiceTotal = trimmed;
                    }
                    if (sale.CustomerNumber != null && sale.CustomerNumber != "undefined")
                    {
                        string trimmed = (sale.CustomerNumber as string).Trim('$');
                        model.CustomerNumber = trimmed;
                    }
                    if (sale.Discount != null && sale.Discount != "undefined")
                    {
                        string trimmed = (sale.Discount as string).Trim('$');
                        model.Discount = trimmed;
                    }
                    if (sale.Other != null && sale.Other != "undefined")
                    {
                        string trimmed = (sale.Other as string).Trim('$');
                        model.Other = trimmed;
                    }
                    if (sale.Tax != null && sale.Tax != "undefined")
                    {
                        string trimmed = (sale.Tax as string).Trim('$');
                        model.Tax = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Count != null && sale.Count != "undefined")
                    {
                        string trimmed = (sale.Count as string).Trim('$');
                        model.Count = trimmed;
                    }
                    if (sale.OutDiscount != null && sale.OutDiscount != "undefined")
                    {
                        string trimmed = (sale.OutDiscount as string).Trim('$');
                        model.OutDiscount = trimmed;

                    }
                    if (sale.CustomerName != null && sale.CustomerName != "undefined")
                    {
                        model.CustomerName = sale.CustomerName;

                    }
                    if (sale.IsOpen != null)
                    {
                        model.IsOpen = sale.IsOpen;

                    }
                    if (sale.IsClose != null)
                    {
                        model.IsClose = sale.IsClose;

                    }
                    if (sale.SupervisorId != null && sale.SupervisorId != "undefined" && sale.SupervisorId != "")
                    {
                        model.SupervisorId = int.Parse(sale.SupervisorId);

                    }
                    if (sale.RingerQuantity != null && sale.RingerQuantity != "undefined" && sale.RingerQuantity != "")
                    {
                        model.RingerQuantity = sale.RingerQuantity;

                    }
                    if (sale.ShipmentLimit != null && sale.ShipmentLimit != "undefined" && sale.ShipmentLimit != "")
                    {
                        model.ShipmentLimit = sale.ShipmentLimit;

                    }
                    if (sale.plateNo != null && sale.plateNo != "undefined" && sale.plateNo != "")
                    {
                        model.plateNo = sale.plateNo;

                    }
                    if (sale.drivingLicense != null && sale.drivingLicense != "undefined" && sale.drivingLicense != "")
                    {
                        model.drivingLicense = sale.drivingLicense;

                    }
                    if (sale.InUnits != null && sale.InUnits != "undefined" && sale.InUnits != "")
                    {
                        model.InUnits = sale.InUnits;

                    }
                    if (sale.OutUnits != null && sale.OutUnits != "undefined" && sale.OutUnits != "")
                    {
                        model.OutUnits = sale.OutUnits;

                    }

                    if (sale.PossaleId != null && sale.PossaleId != "undefined" && sale.PossaleId != "")
                    {
                        model.PossaleId = int.Parse(sale.PossaleId);

                    }
                    modellist.Add(model);
                }
                SaleInfoModel obj = new SaleInfoModel();
                obj.BusinessAddress = SaleDetails[0].BusinessAddress;
                obj.CustomerEmail = SaleDetails[0].CustomerEmail;
                obj.AddToMailList = SaleDetails[0].AddToMailList;
                var saleInfo = JsonConvert.SerializeObject(obj);

                var body = JsonConvert.SerializeObject(modellist);
                TempData["saleorder"] = body;
                TempData["saleorderInfo"] = saleInfo;
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SaleUpdate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {   //Send Mail If Add to Mail list is true
                    if (obj.AddToMailList)
                    {
                        MailRequest mail = new MailRequest();
                        mail.ToEmail = obj.CustomerEmail;
                        mail.Subject = "Invoice Generated";
                        mail.Body = "Check mail";
                        var mailbody = JsonConvert.SerializeObject(mail);
                        SResponse mailresp = RequestSender.Instance.CallAPI("api", "Sale/MailToCustomer", "POST", mailbody);
                        if (mailresp.Status && (mailresp.Resp != null) && (mailresp.Resp != ""))
                        {
                            TempData["Msg"] = "Mail Sent Successfully";
                        }
                        else
                        {
                            TempData["Msg"] = resp.Resp + " " + "Unable To Sent Mail";
                        }
                    }


                    SResponse resp1 = RequestSender.Instance.CallAPI("api", "Inventory/SaleHistoryCreate", "POST", body);
                    if (resp1.Status && (resp1.Resp != null) && (resp1.Resp != ""))
                    {   //Send Mail If Add to Mail list is true

                        if (SaleDetails[0].IsPrintinvoice)
                        {
                            return Json(true);
                        }
                        TempData["Msg"] = "Add Successfully";
                        return Json(resp.Status);
                    }
                    else
                    {
                        TempData["Msg"] = resp.Resp + " " + "Unable To add";
                        return Json(resp.Status);
                    }

                    //if (SaleDetails[0].IsPrintinvoice)
                    //{
                    //    return Json(true);
                    //}
                    //TempData["Msg"] = "Add Successfully";
                    //return Json(resp.Status);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To add";
                    return Json(resp.Status);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public IActionResult AddSalePosOFI([FromBody] List<JsonPosSale> SaleDetails)
        {
            try
            {
                PosSale model = null;
                List<PosSale> modellist = new List<PosSale>();

                foreach (JsonPosSale sale in SaleDetails)
                {
                    model = new PosSale();
                    sale.FromScreen = "Online";
                    model.FromScreen = "Online";
                    if (sale.ItemId != null && sale.ItemId != "undefined")
                    {
                        if (sale.ItemId.Contains("-"))
                        {
                            sale.ItemId = sale.ItemId.Split('-')[1];
                        }
                        model.ItemId = Convert.ToInt32(sale.ItemId);
                    }
                    if (sale.CustomerId != null && sale.CustomerId != "undefined")
                    {
                        string trimmed = (sale.CustomerId as string).Trim('$');
                        model.CustomerId = Convert.ToInt32(trimmed);

                    }
                    if (sale.OnCredit != null && sale.OnCredit != "undefined")
                    {
                        model.OnCredit = Convert.ToBoolean(sale.OnCredit);
                    }
                    else
                    {
                        model.OnCredit = false;
                    }
                    if (sale.OnCash != null && sale.OnCash != "undefined")
                    {
                        model.OnCash = Convert.ToBoolean(sale.OnCash);
                    }
                    else
                    {
                        model.OnCash = false;
                    }
                    if (sale.GetSaleDiscount != null && sale.GetSaleDiscount != "undefined")
                    {
                        model.GetSaleDiscount = Convert.ToBoolean(sale.GetSaleDiscount);
                    }
                    else
                    {
                        model.GetSaleDiscount = false;
                    }
                    if (sale.Quantity != null && sale.Quantity != "undefined")
                    {
                        model.Quantity = sale.Quantity;
                    }
                    if (sale.ItemNumber != null && sale.ItemNumber != "undefined")
                    {
                        model.ItemNumber = sale.ItemNumber;
                    }
                    if (sale.ItemName != null && sale.ItemName != "undefined")
                    {
                        model.ItemName = sale.ItemName;
                    }
                    if (sale.ItemDescription != null && sale.ItemDescription != "undefined")
                    {
                        model.ItemDescription = sale.ItemDescription;
                    }

                    if (sale.Total != null && sale.Total != "undefined")
                    {
                        string trimmed = (sale.Total as string).Trim('$');
                        model.Total = trimmed;
                    }
                    if (sale.Price != null && sale.Price != "undefined")
                    {
                        string trimmed = (sale.Price as string).Trim('$');
                        model.Price = trimmed;
                    }
                    if (sale.AmountRetail != null && sale.AmountRetail != "undefined")
                    {
                        string trimmed = (sale.AmountRetail as string).Trim('$');
                        model.AmountRetail = trimmed;
                    }
                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        string trimmed = (sale.InvoiceNumber as string).Trim('$');
                        model.InvoiceNumber = trimmed;
                    }
                    if (sale.InvoiceTotal != null && sale.InvoiceTotal != "undefined")
                    {
                        string trimmed = (sale.InvoiceTotal as string).Trim('$');
                        model.InvoiceTotal = trimmed;
                    }
                    if (sale.CustomerNumber != null && sale.CustomerNumber != "undefined")
                    {
                        string trimmed = (sale.CustomerNumber as string).Trim('$');
                        model.CustomerNumber = trimmed;
                    }
                    if (sale.Discount != null && sale.Discount != "undefined")
                    {
                        string trimmed = (sale.Discount as string).Trim('$');
                        model.Discount = trimmed;
                    }
                    if (sale.Other != null && sale.Other != "undefined")
                    {
                        string trimmed = (sale.Other as string).Trim('$');
                        model.Other = trimmed;
                    }
                    if (sale.Tax != null && sale.Tax != "undefined")
                    {
                        string trimmed = (sale.Tax as string).Trim('$');
                        model.Tax = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Freight != null && sale.Freight != "undefined")
                    {
                        string trimmed = (sale.Freight as string).Trim('$');
                        model.Freight = trimmed;
                    }
                    if (sale.Count != null && sale.Count != "undefined")
                    {
                        string trimmed = (sale.Count as string).Trim('$');
                        model.Count = trimmed;
                    }
                    if (sale.OutDiscount != null && sale.OutDiscount != "undefined")
                    {
                        string trimmed = (sale.OutDiscount as string).Trim('$');
                        model.OutDiscount = trimmed;

                    }
                    if (sale.CustomerName != null && sale.CustomerName != "undefined")
                    {
                        model.CustomerName = sale.CustomerName;

                    }
                    if (sale.IsOpen != null)
                    {
                        model.IsOpen = sale.IsOpen;

                    }
                    if (sale.IsClose != null)
                    {
                        model.IsClose = sale.IsClose;

                    }
                    if (sale.SupervisorId != null && sale.SupervisorId != "undefined" && sale.SupervisorId != "")
                    {
                        model.SupervisorId = int.Parse(sale.SupervisorId);

                    }
                    if (sale.RingerQuantity != null && sale.RingerQuantity != "undefined" && sale.RingerQuantity != "")
                    {
                        model.RingerQuantity = sale.RingerQuantity;

                    }
                    if (sale.ShipmentLimit != null && sale.ShipmentLimit != "undefined" && sale.ShipmentLimit != "")
                    {
                        model.ShipmentLimit = sale.ShipmentLimit;

                    }
                    modellist.Add(model);
                }
                SaleInfoModel obj = new SaleInfoModel();
                obj.BusinessAddress = SaleDetails[0].BusinessAddress;
                obj.CustomerEmail = SaleDetails[0].CustomerEmail;
                obj.AddToMailList = SaleDetails[0].AddToMailList;
                var saleInfo = JsonConvert.SerializeObject(obj);

                var body = JsonConvert.SerializeObject(modellist);
                //TempData["saleorder"] = body;
                //TempData["saleorderInfo"] = saleInfo;
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SaleCreateOFI", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {   //Send Mail If Add to Mail list is true
                    //if (obj.AddToMailList)
                    //{
                    //    MailRequest mail = new MailRequest();
                    //    mail.ToEmail = obj.CustomerEmail;
                    //    mail.Subject = "Invoice Generated";
                    //    mail.Body = "Check mail";
                    //    var mailbody = JsonConvert.SerializeObject(mail);
                    //    SResponse mailresp = RequestSender.Instance.CallAPI("api", "Sale/MailToCustomer", "POST", mailbody);
                    //    if (mailresp.Status && (mailresp.Resp != null) && (mailresp.Resp != ""))
                    //    {
                    //        TempData["Msg"] = "Mail Sent Successfully";
                    //    }
                    //    else
                    //    {
                    //        TempData["Msg"] = resp.Resp + " " + "Unable To Sent Mail";
                    //    }
                    //}


                    SResponse resp1 = RequestSender.Instance.CallAPI("api", "Inventory/SaleHistoryCreate", "POST", body);
                    if (resp1.Status && (resp1.Resp != null) && (resp1.Resp != ""))
                    {   //Send Mail If Add to Mail list is true

                        if (SaleDetails[0].IsPrintinvoice)
                        {
                           return Json(true);
                            //return RedirectToAction("OrdersForInvoice", "Customer");
                        }
                        // TempData["Msg"] = "Add Successfully";
                        return Json(true);
                        //return RedirectToAction("OrdersForInvoice", "Customer");

                    }
                    else
                    {
                       // TempData["Msg"] = resp1.Resp + " " + "Unable To add";
                        //return RedirectToAction("OrdersForInvoice", "Customer");
                        return Json(false);


                    }

                    //if (SaleDetails[0].IsPrintinvoice)
                    //{
                    //    return Json(true);
                    //}
                    //TempData["Msg"] = "Add Successfully";
                    //return Json(resp.Status);
                }
                else
                {
                   // TempData["Msg"] = resp.Resp + " " + "Unable To add";
                    //return RedirectToAction("OrdersForInvoice", "Customer");
                    return Json(false);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        public IActionResult SaleInvoicePdf(InvoiceModel model)
        {
            try
            {
                var list1 = TempData["saleorder"];
                var list2 = JsonConvert.DeserializeObject<List<PosSale>>((string)list1);
                var infolist = TempData["saleorderInfo"];
                var info_list = JsonConvert.DeserializeObject<SaleInfoModel>((string)infolist);
                List<PosSale> obj = new List<PosSale>();
                InvoiceSaleModel invoiceModel = new InvoiceSaleModel();
                var test = list2[0].InvoiceNumber;
                InvoiceTotal total = new InvoiceTotal();
                total.InvoiceNumber = list2[0].InvoiceNumber;
                total.SupplierNumber = list2[0].CustomerName;
                total.ItemCode = list2[0].ItemNumber;
                total.ItemName = list2[0].ItemName;
                total.TotalItems = list2[0].RingerQuantity;
                total.Other = list2[0].Other;
                total.Charges = list2[0].Charges;
                total.BusinessAddress = info_list.BusinessAddress;
                total.SubTotal = list2[0].SubTotal;
                total.Discount = list2[0].Discount;
                total.Tax = list2[0].Tax;
                total.Freight = list2[0].Freight;
                for (int i = 0; i < list2.Count(); i++)
                {
                    if (list2[i].Discount != null)
                    {
                        list2[i].Discount = list2[i].Discount.Replace("%", "");

                    }
                    if(list2[i].RingerQuantity != null && list2[i].Price != null)
                    {
                        decimal current = (decimal.Parse(list2[i].RingerQuantity)) * (decimal.Parse(list2[i].Price));
                        list2[i].AmountRetail = (Math.Round((current), 2)).ToString();
                    }
                }

                //foreach (var item in list2)
                //{
                //    item.Discount = item.OutDiscount.Replace("%", "");
                //    var newSubtotal = (decimal.Parse(item.Price) * decimal.Parse(item.RingerQuantity));
                //    total.SubTotal = (decimal.Parse(total.SubTotal) + newSubtotal).ToString();
                //    total.Discount = (decimal.Parse(total.Discount) + (decimal.Parse(item.RingerQuantity) * ((decimal.Parse(item.Price) / 100) * decimal.Parse(item.Discount)))).ToString();
                //}
                invoiceModel.SaleOrders = list2;
                invoiceModel.InvoiceTotal = total;
                //total.SubTotal = (Math.Round(decimal.Parse(total.SubTotal), 2)).ToString();
                //total.Discount = (Math.Round(decimal.Parse(total.Discount), 2)).ToString();
                //var total1 = (Math.Round((decimal.Parse(total.SubTotal) - decimal.Parse(total.Discount)), 2)).ToString();
                total.Total = list2[0].InvoiceTotal;
                    //(Math.Round((decimal.Parse(total1) + (decimal.Parse(total.Other))), 2)).ToString();
                var pdfResult = new ViewAsPdf(invoiceModel);
                return pdfResult;


            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


       

        public IActionResult SendApproval([FromBody] List<JsonPosSale> SaleDetails)
        {
            try
            {
                PosSale model = null;
                List<PosSale> modellist = new List<PosSale>();

                foreach (JsonPosSale sale in SaleDetails)
                {
                    model = new PosSale();

                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        string trimmed = (sale.InvoiceNumber as string).Trim('$');
                        model.InvoiceNumber = trimmed;
                    }

                    if (sale.ItemName != null && sale.ItemName != "undefined")
                    {
                        model.ItemName = sale.ItemName;
                    }

                    if (sale.RingerQuantity != null && sale.RingerQuantity != "undefined")
                    {
                        model.RingerQuantity = sale.RingerQuantity;
                    }
                    if (sale.ShipmentLimit != null && sale.ShipmentLimit != "undefined")
                    {
                        model.ShipmentLimit = sale.ShipmentLimit;
                    }
                    if (sale.Quantity != null && sale.Quantity != "undefined")
                    {
                        model.Quantity = sale.Quantity;
                    }

                    modellist.Add(model);
                }


                var body = JsonConvert.SerializeObject(modellist);
                //TempData["saleorder"] = body;
                //TempData["saleorderInfo"] = saleInfo;
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SendRequestforApproval", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {   //Send Mail If Add to Mail list is true

                    TempData["Msg"] = "Send Request Successfully";
                    return Json(resp.Status);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Send Request";
                    return Json(resp.Status);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }


        [HttpPost]
        public IActionResult ApproveItemCustomQty(int? id, int? productId, string ticketid, string plateNum, string licenseNo, IFormFile scannedDocument)
        {
            string userData = HttpContext.Session.GetString("userobj");
            if (!string.IsNullOrEmpty(userData))
            {
                AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                id = userDto.Id;
                try
                {
                    AutherizeOrderLimit autherizeOrderLimit = new AutherizeOrderLimit();
                    if (plateNum != null)
                    {
                        autherizeOrderLimit.PlateNumber = plateNum;
                    }
                    else
                    {
                        autherizeOrderLimit.PlateNumber = "NA";
                    }
                    if (licenseNo != null)
                    {
                        autherizeOrderLimit.DrivingLicenseNumber = licenseNo;
                    }
                    else
                    {
                        autherizeOrderLimit.DrivingLicenseNumber = "NA";
                    }
                    if (ticketid != null)
                    {
                        autherizeOrderLimit.TicketId = ticketid;
                    }
                    else
                    {
                        autherizeOrderLimit.TicketId = "NA";
                    }

                    autherizeOrderLimit.UserId = id;
                    autherizeOrderLimit.ProductId = productId;

                    if (scannedDocument != null)
                    {
                        var input = scannedDocument.OpenReadStream();
                        byte[] byteData = null, buffer = new byte[input.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            byteData = ms.ToArray();
                            autherizeOrderLimit.UploadFile = byteData;
                        }

                    }

                    var body = JsonConvert.SerializeObject(autherizeOrderLimit);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/AuthorizeQuantityInCart", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<AutherizeOrderLimit>>(resp.Resp);
                        if (response != null)
                        {
                            return Json("true");
                        }
                        else
                        {
                            return Json("false");
                        }
                    }
                    else
                    {
                        TempData["response"] = resp.Resp + " " + "Unable To generate";
                        return Json("false");
                    }
                }
                catch (Exception ex)
                {
                    TempData["response"] = ex.Message + "Error Occured.";
                    return Json("false");
                }
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult SalesInvoiceOFI(string ticketId)
        {
            try
            {

                if (ticketId != null)
                {
                    var CartDetailList = new List<CartDetail>();
                    var productlist = new List<Product>();
                    var CustomerInformation = new CustomerInformation();
                    var customerorders = new List<CustomerOrder>();
                    try
                    {
                        SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartByAdminApproval/" + ticketId, "GET");
                        if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                        {
                            var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                            //return Json(response.Data);
                            CartDetailList = response.Data;
                            //ViewBag.CartDetailList = CartDetailList;
                            var cardetaillist = new List<CartDetail>();
                            foreach (var item in CartDetailList)
                            {
                                SResponse ress = RequestSender.Instance.CallAPI("api", "Inventory/ItemGetByIDWithStockAndFinancial" + "/" + item.Id, "Get");

                                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                                {
                                    var response1 = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                                    if (response1.Data != null)
                                    {
                                        item.NeedHighAuthorization = response1.Data.NeedHighAuthorization;
                                        item.HighlimitOn = response1.Data.HighlimitOn;
                                        productlist.Add(response1.Data);
                                        cardetaillist.Add(item);
                                        //return Json(responseObject);
                                    }

                                }


                            }
                            ViewBag.CartDetailList = cardetaillist;
                            ViewBag.productlist = productlist;


                            SResponse resp1 = RequestSender.Instance.CallAPI("api", "Customer/CustomerInformationByUserID" + "/" + CartDetailList[0].UserId, "GET");
                            if (resp1.Status && (resp1.Resp != null) && (resp1.Resp != ""))
                            {
                                var response1 = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(resp1.Resp);
                                //return Json(response.Data);
                                CustomerInformation = response1.Data;
                                ViewBag.CustomerInformation = CustomerInformation;
                            }

                            SResponse resp5 = RequestSender.Instance.CallAPI("api", "Customer/GetcustomerOrders" + "/" + ticketId, "GET");
                            if (resp5.Status && (resp5.Resp != null) && (resp5.Resp != ""))
                            {
                                var response5 = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(resp5.Resp);
                                //return Json(response.Data);
                                customerorders = response5.Data;
                                ViewBag.customerorders = customerorders;
                            }
                        }
                        //return Json("null");
                        ViewBag.InvoiceNumberOFI = ticketId;

                    }
                    catch (Exception ex)
                    {
                        ViewBag.CartDetailList = null;
                    }
                }
                PosSale model = new PosSale();
                SResponse respcustomer = RequestSender.Instance.CallAPI("api", "Inventory/SaleGet", "GET");
                if (respcustomer.Status && (respcustomer.Resp != null) && (respcustomer.Resp != ""))
                {
                    ResponseBack<List<PosSale>> record = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(respcustomer.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        PosSale newcustomer = new PosSale();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int CustomerInfoID = 0;
                            large = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].InvoiceNumber.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count; i++)
                            {
                                if (record.Data[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) > large)
                                    {
                                        CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                                        large = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].InvoiceNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            CustomerInfoID = Convert.ToInt32(record.Data[i].PossaleId);
                                        }
                                    }
                                }
                            }
                            newcustomer = record.Data.ToList().Where(x => x.PossaleId == CustomerInfoID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.InvoiceNumber != null)
                                {
                                    var VcodeSplit = newcustomer.InvoiceNumber.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "00-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "00-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "00-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "00-" + "1";
                        }

                        ViewBag.InvoiceNumber = fullcode;
                    }
                    else
                    {
                        ViewBag.InvoiceNumber = "00-" + "1";
                    }
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }
                else
                {
                    ViewBag.InvoiceNumber = "00-" + "1";
                    model.InvoiceNumber = ViewBag.InvoiceNumber;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        public JsonResult GetOpensales()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/GetOpenSales", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    ResponseBack<List<PosSale>> response =
                                JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(ress.Resp);

                    if (response.Data.Count() > 0)
                    {

                        //List<PosSale> responseObject = response.Data;
                        List<string> responseObject = response.Data.Select(x => x.InvoiceNumber).Distinct().ToList();


                        //var query = responseObject.GroupBy(x => x.CurrentInvoiceNumber).Select(g => g.FirstOrDefault()).ToList();

                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Sale List Found. Please Enter Sale First.";
                    }
                }
                return Json("NotFound");
            }
            catch (Exception ex)
            {
                return Json("NotFound");
            }
        }



        public JsonResult GetPostedsales()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/GetPostedsales", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    ResponseBack<List<PosSale>> response =
                                JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(ress.Resp);

                    if (response.Data.Count() > 0)
                    {
                        List<string> responseObject = response.Data.Select(x => x.InvoiceNumber).Distinct().ToList(); ;

                        //var query = responseObject.GroupBy(x => x.CurrentInvoiceNumber).Select(g => g.FirstOrDefault()).ToList();

                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Sale List Found. Please Enter Sale First.";
                    }
                }
                return Json("NotFound");
            }
            catch (Exception ex)
            {
                return Json("NotFound");
            }
        }



        [HttpGet]
        public JsonResult GetSaleInvoiceByInvoiceNumber(string InvoiceNumber = "", string Method = "")
        {
            var Msg = "";

            SResponse res = RequestSender.Instance.CallAPI("api", "Sale/PosSaleByInvoiceNumber/" + InvoiceNumber + "/" + Method, "GET");
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                ResponseBack<List<PosSale>> response = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(res.Resp);
                if (response.Data != null)
                {
                    List<PosSale> responseobj = response.Data;
                    return Json(responseobj);

                }
                else
                {
                    return Json("false");
                }

            }
            else
            {
                return Json("false");
            }

        }

        [HttpPost]
        public IActionResult ChangePayment([FromBody] List<SalesInvoicesJson> SaleDetails)
        {
            try
            {
                SalesInvTransaction model = null;
                SalesInvTransaction model1 = null;
                List<SalesInvTransaction> modellist = new List<SalesInvTransaction>();
                SalesInvoice obj = new SalesInvoice();


                obj.InvoiceNumber = SaleDetails[0].InvoiceNumber;
                //obj.Date = new DateTime();
                DateTime now = DateTime.Now;
                obj.Date = now;
                obj.CustomerId = SaleDetails[0].CustomerId;
                obj.CustomerName = SaleDetails[0].CustomerName;
                obj.CustomerCode = SaleDetails[0].CustomerCode;
                string trimmed = (SaleDetails[0].TotalPaid as string).Trim('$');
                obj.TotalPaid = trimmed;

                string str = SaleDetails[0].TotalAmount;
                str = str.Replace(" ", String.Empty).Replace("\n", "").Replace("$", "");
                obj.TotalAmount = str;

                string str1 = SaleDetails[0].Balance;
                str = str.Replace(" ", String.Empty).Replace("\n", "").Replace("$", "");
                obj.Balance = str1;

                string trimmed2 = (SaleDetails[0].Change as string).Trim('$');
                obj.Change = trimmed2;
                string trimmed8 = (SaleDetails[0].PreviousBalance as string).Trim('$');
                obj.PreviousBalance = trimmed8;
                string trimmed9 = (SaleDetails[0].InvoiceBalance as string).Trim('$');
                obj.InvoiceBalance = trimmed9;
                obj.Buyer = SaleDetails[0].CustomerName;

                var saleInfo = JsonConvert.SerializeObject(obj);

                foreach (SalesInvoicesJson sale in SaleDetails)
                {
                    model1 = new SalesInvTransaction();
                    if (sale.AmountPaid != null && sale.AmountPaid != "undefined")
                    {
                        string trimmed4 = (sale.AmountPaid as string).Trim('$');
                        model1.AmountPaid = trimmed4;

                    }
                    if (sale.AmountAllocate != null && sale.AmountAllocate != "undefined")
                    {
                        string trimmed5 = (sale.AmountAllocate as string).Trim('$');
                        model1.AmountAllocate = trimmed5;

                    }
                    if (sale.PaymentType != null && sale.PaymentType != "undefined")
                    {
                        model1.PaymentType = sale.PaymentType;
                    }
                    if (sale.ChequeNumber != null && sale.ChequeNumber != "undefined")
                    {
                        model1.ChequeNumber = sale.ChequeNumber;
                    }
                    if (sale.HoldDate != null)
                    {
                        model1.HoldDate = sale.HoldDate;
                    }
                    if (sale.InvoiceNumber != null && sale.InvoiceNumber != "undefined")
                    {
                        model1.InvoiceNumber = sale.InvoiceNumber;
                    }

                    modellist.Add(model1);
                }


                var body = JsonConvert.SerializeObject(modellist);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ChangePayment", "POST", saleInfo);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    SResponse ressp = RequestSender.Instance.CallAPI("api", "Inventory/SalesInvoiceTransactions", "POST", body);
                    if (ressp.Status && (ressp.Resp != null) && (ressp.Resp != ""))
                    {
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }

}