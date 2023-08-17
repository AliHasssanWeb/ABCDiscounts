using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Website.Models;
using ABC.Shared.DataConfig;
using iTextSharp.text;
using iTextSharp.tool.xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Rotativa;
using Rotativa.AspNetCore;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    public class PurchaseController : Controller
    {
        private static IHttpContextAccessor httpContextAccessor;

        public string controllername;
        public PurchaseController(IHttpContextAccessor accessor)
        {

            httpContextAccessor = accessor;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PurchaseOrder()
        {
            Purchase model = new Purchase();
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api", "Purchase/PurchaseGet", "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<Purchase>> record = JsonConvert.DeserializeObject<ResponseBack<List<Purchase>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        Purchase newitems = new Purchase();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int PurchaseID = 0;
                            large = Convert.ToInt32(record.Data[0].InvoiceNumber);
                            small = Convert.ToInt32(record.Data[0].InvoiceNumber);
                            for (int i = 0; i < record.Data.Count(); i++)
                            {
                                if (record.Data[i].InvoiceNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].InvoiceNumber);
                                    if (Convert.ToInt32(record.Data[i].InvoiceNumber) > large)
                                    {
                                        PurchaseID = Convert.ToInt32(record.Data[i].Id);
                                        large = Convert.ToInt32(record.Data[i].InvoiceNumber);
                                    }
                                    else if (Convert.ToInt32(record.Data[i].InvoiceNumber) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].InvoiceNumber);
                                    }
                                    else
                                    {
                                        if (large <= 2)
                                        {
                                            PurchaseID = Convert.ToInt32(record.Data[i].Id);
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.Id == PurchaseID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.InvoiceNumber != null)
                                {
                                    var VcodeSplit = newitems.InvoiceNumber;
                                    int code = Convert.ToInt32(VcodeSplit) + 1;
                                    long checknumber = Convert.ToInt64(VcodeSplit);
                                    if (checknumber > 9)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        fullcode = Convert.ToString(code);
                                    }
                                    else if (checknumber > 999)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                    }
                                    else if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit) + 1;
                                        fullcode = Convert.ToString(ndcode) + "9999";
                                    }
                                    else
                                    {
                                        fullcode = "000" + Convert.ToString(code);
                                    }
                                }
                                else
                                {
                                    fullcode = "0001";
                                }
                            }
                            else
                            {
                                fullcode = "0001";
                            }
                        }
                        else
                        {
                            fullcode = "0001";
                        }
                        ViewBag.Invoicenumber = fullcode;
                    }
                    else
                    {
                        ViewBag.Invoicenumber = "0001";
                    }
                    model.InvoiceNumber = ViewBag.Invoicenumber;
                }
                else
                {
                    ViewBag.Invoicenumber = "0001";
                }
               
                List<Product> responseObjectPro = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    SResponse ItemGet = RequestSender.Instance.CallAPI("api",
                        "Inventory/OptimizeItems", "GET");
                    if (ItemGet.Status && (ItemGet.Resp != null) && (ItemGet.Resp != ""))
                    {
                        ResponseBack<List<Product>> response =
                                      JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ItemGet.Resp);
                        if (response.Data.Count() > 0)
                        {
                            responseObjectPro = response.Data;
                            ViewBag.Items = new SelectList(responseObjectPro.ToList(), "Id", "Name");

                        }
                        else
                        {
                            responseObjectPro = new List<Product>();
                            ViewBag.Items = new SelectList(responseObjectPro, "Id", "Name");
                        }
                    }
                    else
                    {
                        responseObjectPro = new List<Product>();
                        ViewBag.Items = new SelectList(responseObjectPro, "Id", "Name");
                    }
                }
                else
                {
                    ViewBag.Items = new SelectList(FoundSession_Result.ToList(), "Id", "Name");
                }
                   

                SResponse storeGet = RequestSender.Instance.CallAPI("api",
                 "Inventory/StoreGet", "GET");
                if (storeGet.Status && (storeGet.Resp != null) && (storeGet.Resp != ""))
                {
                    ResponseBack<List<Store>> responsestore =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Store>>>(storeGet.Resp);
                    if (responsestore.Data.Count() > 0)
                    {
                        List<Store> responseObject = responsestore.Data;
                        ViewBag.store = new SelectList(responseObject.ToList(), "Id", "StoreName");
                    }
                    else
                    {
                        List<Store> responseObject = new List<Store>();
                        ViewBag.store = new SelectList(responseObject, "Id", "StoreName");
                    }
                }
                else
                {
                    List<Store> responseObject = new List<Store>();
                    ViewBag.store = new SelectList(responseObject, "Id", "StoreName");
                }

                List<Vendor> responseObjectVendor = new List<Vendor>();
                var FoundSessionVendor = HttpContext.Session.GetString("loadedVendors");
                List<Vendor> FoundSession_Result_Vendor = new List<Vendor>();
                if (FoundSessionVendor != null)
                {
                    FoundSession_Result_Vendor = JsonConvert.DeserializeObject<List<Vendor>>(FoundSessionVendor);
                }
                if (FoundSession_Result_Vendor != null && FoundSession_Result_Vendor.Count() < 1)
                {
                    SResponse VendorGet = RequestSender.Instance.CallAPI("api",
                     "Inventory/VendorGet", "GET");
                    if (VendorGet.Status && (VendorGet.Resp != null) && (VendorGet.Resp != ""))
                    {
                        ResponseBack<List<Vendor>> response =
                                     JsonConvert.DeserializeObject<ResponseBack<List<Vendor>>>(VendorGet.Resp);
                        if (response.Data.Count() > 0)
                        {
                            responseObjectVendor = response.Data;
                            for (int i = 0; i < responseObjectVendor.Count(); i++)
                            {
                                responseObjectVendor[i].FullName = responseObjectVendor[i].FullName + "(" + responseObjectVendor[i].VendorCode + ")";
                            }
                            ViewBag.Vendors = new SelectList(responseObjectVendor.ToList(), "VendorId", "FullName");
                        }
                        else
                        {
                            responseObjectVendor = new List<Vendor>();
                            ViewBag.Vendors = new SelectList(responseObjectVendor.ToList(), "VendorId", "FullName");
                        }
                    }
                    else
                    {
                        responseObjectVendor = new List<Vendor>();
                        ViewBag.warehouse = new SelectList(responseObjectVendor.ToList(), "VendorId", "FullName");
                    }
                }
                else
                {
                    ViewBag.warehouse = new SelectList(FoundSession_Result_Vendor.ToList(), "VendorId", "FullName");
                }
                   

                SResponse WareHouseGet = RequestSender.Instance.CallAPI("api",
                  "Configuration/WareHouseGet", "GET");
                if (WareHouseGet.Status && (WareHouseGet.Resp != null) && (WareHouseGet.Resp != ""))
                {
                    ResponseBack<List<WareHouse>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<WareHouse>>>(WareHouseGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<WareHouse> responseObject = response.Data;
                        ViewBag.warehouse = new SelectList(responseObject.ToList(), "WareHouseId", "WareHouseName");
                    }
                    else
                    {
                        List<WareHouse> responseObject = new List<WareHouse>();
                        ViewBag.warehouse = new SelectList(responseObject, "WareHouseId", "WareHouseName");
                    }
                }
                else
                {
                    List<WareHouse> responseObject = new List<WareHouse>();
                    ViewBag.Items = new SelectList(responseObject, "WareHouseId", "WareHouseName");
                }

                List<Purchase> QuantityUnit = new List<Purchase>
                    {
                        new Purchase{QuantityId=1,QuantityUnit="Loose"},
                        new Purchase{QuantityId=2,QuantityUnit="Pack"}
                    };
                ViewBag.QuantityUnit = QuantityUnit;
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        public IActionResult AddPurchaseOrderList([FromBody] List<JsonPurchase> PurchaseDetails)
        {
            Purchase model = null;
            List<Purchase> modellist = new List<Purchase>();

            foreach (JsonPurchase purchase in PurchaseDetails)
            {

                model = new Purchase();
                if (purchase.ItemId != null)
                {
                    model.ItemId = Convert.ToInt32(purchase.ItemId);
                }
                if (purchase.VendorId != null)
                {
                    model.VendorId = Convert.ToInt32(purchase.VendorId);
                }
                if (purchase.StoreId != null && purchase.StoreId != "")
                {
                    model.StoreId = Convert.ToInt32(purchase.StoreId);
                }
                if (purchase.WareHouseId != null && purchase.WareHouseId != "")
                {
                    model.WareHouseId = Convert.ToInt32(purchase.WareHouseId);
                }
                else
                {
                    model.WareHouseId = null;
                }
                model.QuantityUnit = purchase.QuantityUnit;
                model.ProductCode = purchase.ProductCode;
                model.ProductBarCode = purchase.ProductBarCode;
                model.QuantityUnit = purchase.QuantityUnit;
                model.UnitPrice = purchase.UnitPrice;
                model.TotalAmount = purchase.TotalAmount;
                model.ItemName = purchase.ItemName;
                model.Quantity = purchase.Quantity;
                model.Cash = Convert.ToBoolean(purchase.Cash);
                model.Credit = Convert.ToBoolean(purchase.Credit);
                model.InvoiceNumber = purchase.InvoiceNumber;

                modellist.Add(model);

            }
            var body = JsonConvert.SerializeObject(modellist);

            SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/PurchaseCreate", "POST", body);
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


        public IActionResult GeneratePdfpurchase(string invoicenumber = "")
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Purchase/PurchaseByID/" + invoicenumber, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Purchase>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Purchase>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Purchase> responseObject = response.Data;
                        PdfDocument doc = new PdfDocument();
                        PdfPage page = doc.Pages.Add();
                        PdfGrid pdfGridObj = new PdfGrid();
                        PurchaseReport insertdata = null;
                        List<PurchaseReport> dataTable = new List<PurchaseReport>();
                        for (int i = 0; i < responseObject.Count; i++)
                        {
                            insertdata = new PurchaseReport();
                            insertdata.InvoiceNumber = responseObject[i].InvoiceNumber;
                            insertdata.ItemName = responseObject[i].ItemName;
                            insertdata.Quantity = responseObject[i].Quantity;
                            insertdata.ProductCode = responseObject[i].ProductCode;
                            insertdata.Sku = responseObject[i].Sku;
                            dataTable.Add(insertdata);
                        }

                        pdfGridObj.DataSource = dataTable;
                        pdfGridObj.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
                        MemoryStream stream = new MemoryStream();
                        doc.Save(stream);
                        stream.Position = 0;
                        doc.Close(true);
                        string contentType = "application/pdf";
                        string fileName = "PurchasePrint.pdf";
                        return File(stream, contentType, fileName);
                    }
                    else
                    {
                        TempData["response"] = "Invalid Request.";
                        return null;
                    }
                }
                else
                {
                    TempData["response"] = "Server not responding.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult GetItem()
        {
            return View();
        }

        public JsonResult GetItemByID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetByID" + "/" + id, "GET");
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

        public JsonResult GetItemList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemCategoryGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<ItemCategory>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<ItemCategory> responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }

        public JsonResult GetItemNameByID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemCategoryUpdateByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<ItemCategory>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }

        public JsonResult GetBrandList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/BrandGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Brand>> response =
                                JsonConvert.DeserializeObject<ResponseBack<List<Brand>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Brand> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);

        }
        public JsonResult GetBrandNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/BrandGetByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetSubItemList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                      "Inventory/ItemSubCategoryGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<ItemSubCategory>> response =
                               JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<ItemSubCategory> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetSubItemNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemSubCategoryUpdateByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetModelList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/ModelGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Model>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<Model>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Model> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetModelNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
              "Inventory/ModelByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetGroupList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/GroupGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Group>> response =
                               JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Group> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetGroupNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GroupByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetColorList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/ColorGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Color>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<Color>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Color> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetColorNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ColorByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetArticleList()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/ArticleGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<ArticleType>> response =
                               JsonConvert.DeserializeObject<ResponseBack<List<ArticleType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<ArticleType> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetArticleNameById(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ArticleGetByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<ArticleType>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetItemNumber()
        {
            var Msg = "";

             List<Product> responseObject = new List<Product>();
            var FoundSession = HttpContext.Session.GetString("loadedProducts");
            List<Product> FoundSession_Result = new List<Product>();
            if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
            {
                FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
            }
            if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
            {
                Product Model = new Product();
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/OptimizeItems", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(resp.Resp);
                    if (record.Data != null && record.Data.Count() > 0)
                    {
                        Product newitems = new Product();
                        //        var fullcode = "";
                        //        if (record.Data[0].ItemNumber != null && record.Data[0].ItemNumber != "string" && record.Data[0].ItemNumber != "")
                        //        {
                        //            int large, small;
                        //            int salesID = 0;
                        //            large = Convert.ToInt32(record.Data[0].ItemNumber.Split('-')[1]);
                        //            small = Convert.ToInt32(record.Data[0].ItemNumber.Split('-')[1]);
                        //            for (int i = 0; i < record.Data.Count; i++)
                        //            {
                        //                if (record.Data[i].ItemNumber != null)
                        //                {
                        //                    var t = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);
                        //                    if (Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]) > large)
                        //                    {
                        //                        salesID = record.Data[i].Id;
                        //                        large = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);

                        //                    }
                        //                    else if (Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]) < small)
                        //                    {
                        //                        small = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);
                        //                    }
                        //                    else
                        //                    {
                        //                        if (large < 2)
                        //                        {
                        //                            salesID = record.Data[i].Id;
                        //                        }
                        //                    }
                        //                }
                        //            }
                        //            newitems = record.Data.ToList().Where(x => x.Id == Convert.ToInt32(salesID)).FirstOrDefault();
                        //            if (newitems != null)
                        //            {
                        //                if (newitems.ItemNumber != null)
                        //                {
                        //                    var VcodeSplit = newitems.ItemNumber.Split('-');
                        //                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                        //                    fullcode = "00" + "-" + Convert.ToString(code);
                        //                    return Json(fullcode);
                        //                }
                        //                else
                        //                {
                        //                    fullcode = "00" + "-" + "1";
                        //                    return Json(fullcode);
                        //                }
                        //            }
                        //            else
                        //            {
                        //                fullcode = "00" + "-" + "1";
                        //                return Json(fullcode);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            fullcode = "00" + "-" + "1";
                        //            return Json(fullcode);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        var fullcode = "00" + "-" + "1";
                        //        return Json(fullcode);
                        //    }
                        //}
                        //else if (resp.Status && resp.Resp == "")
                        //{
                        //    var fullcode = "00" + "-" + "1";
                        //    return Json(fullcode);
                        //}
                        //else
                        //{
                        //    Msg = resp.Resp + " " + "Unable To Get Item Number";
                        //    return Json(Msg);
                        //}

                        var fullcode = "";
                        if (record.Data[0].ItemNumber != null && record.Data[0].ItemNumber != "string" && record.Data[0].ItemNumber != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(record.Data[0].ItemNumber);
                            small = Convert.ToInt32(record.Data[0].ItemNumber);
                            for (int i = 0; i < record.Data.Count(); i++)
                            {
                                if (record.Data[i].ItemNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].ItemNumber);
                                    if (Convert.ToInt32(record.Data[i].ItemNumber) > large)
                                    {
                                        salesID = Convert.ToInt32(record.Data[i].Id);
                                        large = Convert.ToInt32(record.Data[i].ItemNumber);
                                    }
                                    else if (Convert.ToInt32(record.Data[i].ItemNumber) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].ItemNumber);
                                    }
                                    else
                                    {
                                        if (large <= 2)
                                        {
                                            salesID = Convert.ToInt32(record.Data[i].Id);
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.Id == salesID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.ItemNumber != null)
                                {
                                    var VcodeSplit = newitems.ItemNumber;
                                    int code = Convert.ToInt32(VcodeSplit) + 1;
                                    long checknumber = Convert.ToInt64(VcodeSplit);

                                    if (checknumber > 9)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                        return Json(fullcode);
                                    }
                                    else if (checknumber > 99)
                                    {
                                        fullcode = Convert.ToString(code);
                                        return Json(fullcode);
                                    }
                                    else if (checknumber > 999)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                        return Json(fullcode);
                                    }
                                    else if (checknumber > 9999)
                                    {
                                        //10000
                                        long ndcode = Convert.ToInt64(VcodeSplit) + 1;
                                        fullcode = Convert.ToString(ndcode) + "9999";
                                        return Json(fullcode);
                                    }
                                    else
                                    {
                                        fullcode = "000" + Convert.ToString(code);
                                        return Json(fullcode);
                                    }
                                }
                                else
                                {
                                    fullcode = "0001";
                                    return Json(fullcode);
                                }
                            }
                            else
                            {
                                fullcode = "0001";
                                return Json(fullcode);
                            }
                        }
                        else
                        {
                            fullcode = "0001";
                            return Json(fullcode);
                        }

                    }
                    else
                    {
                        var fullcode = "0001";
                        return Json(fullcode);
                    }

                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Get Item Number";
                    return Json(Msg);
                }
            }
            else{
                Product newitems = new Product();
                var fullcode = "";
                if (FoundSession_Result[0].ItemNumber != null && FoundSession_Result[0].ItemNumber != "string" && FoundSession_Result[0].ItemNumber != "")
                {
                    int large, small;
                    int salesID = 0;
                    large = Convert.ToInt32(FoundSession_Result[0].ItemNumber);
                    small = Convert.ToInt32(FoundSession_Result[0].ItemNumber);
                    for (int i = 0; i < FoundSession_Result.Count(); i++)
                    {
                        if (FoundSession_Result[i].ItemNumber != null)
                        {
                            var t = Convert.ToInt32(FoundSession_Result[i].ItemNumber);
                            if (Convert.ToInt32(FoundSession_Result[i].ItemNumber) > large)
                            {
                                salesID = Convert.ToInt32(FoundSession_Result[i].Id);
                                large = Convert.ToInt32(FoundSession_Result[i].ItemNumber);
                            }
                            else if (Convert.ToInt32(FoundSession_Result[i].ItemNumber) < small)
                            {
                                small = Convert.ToInt32(FoundSession_Result[i].ItemNumber);
                            }
                            else
                            {
                                if (large <= 2)
                                {
                                    salesID = Convert.ToInt32(FoundSession_Result[i].Id);
                                }
                            }
                        }
                    }
                    newitems = FoundSession_Result.ToList().Where(x => x.Id == salesID).FirstOrDefault();
                    if (newitems != null)
                    {
                        if (newitems.ItemNumber != null)
                        {
                            var VcodeSplit = newitems.ItemNumber;
                            int code = Convert.ToInt32(VcodeSplit) + 1;
                            long checknumber = Convert.ToInt64(VcodeSplit);

                            if (checknumber > 9)
                            {
                                fullcode = "0" + Convert.ToString(code);
                                return Json(fullcode);
                            }
                            else if (checknumber > 99)
                            {
                                fullcode = Convert.ToString(code);
                                return Json(fullcode);
                            }
                            else if (checknumber > 999)
                            {
                                fullcode = "0" + Convert.ToString(code);
                                return Json(fullcode);
                            }
                            else if (checknumber > 9999)
                            {
                                //10000
                                long ndcode = Convert.ToInt64(VcodeSplit) + 1;
                                fullcode = Convert.ToString(ndcode) + "9999";
                                return Json(fullcode);
                            }
                            else
                            {
                                fullcode = "000" + Convert.ToString(code);
                                return Json(fullcode);
                            }
                        }
                        else
                        {
                            fullcode = "0001";
                            return Json(fullcode);
                        }
                    }
                    else
                    {
                        fullcode = "0001";
                        return Json(fullcode);
                    }
                }
                else
                {
                    fullcode = "0001";
                    return Json(fullcode);
                }
            }
              
        }
        public JsonResult AddItem(Product obj)
        {
            try
            {
                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(Msg);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        public IActionResult GeneratePdfpurchaselist()
        {
            try
            {


                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Purchase/PurchaseGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Purchase>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Purchase>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Purchase> responseObject = response.Data;
                        PdfDocument doc = new PdfDocument();
                        PdfPage page = doc.Pages.Add();
                        PdfGrid pdfGridObj = new PdfGrid();
                        PurchaseReport insertdata = null;
                        List<PurchaseReport> dataTable = new List<PurchaseReport>();
                        for (int i = 0; i < responseObject.Count; i++)
                        {
                            insertdata = new PurchaseReport();
                            insertdata.InvoiceNumber = responseObject[i].InvoiceNumber;
                            insertdata.ItemName = responseObject[i].ItemName;
                            insertdata.Quantity = responseObject[i].Quantity;
                            insertdata.ProductCode = responseObject[i].ProductCode;
                            insertdata.Sku = responseObject[i].Sku;
                            dataTable.Add(insertdata);
                        }

                        pdfGridObj.DataSource = dataTable;
                        pdfGridObj.Draw(page, new Syncfusion.Drawing.PointF(10, 10));
                        MemoryStream stream = new MemoryStream();
                        doc.Save(stream);
                        stream.Position = 0;
                        doc.Close(true);
                        string contentType = "application/pdf";
                        string fileName = "PurchasePrint.pdf";
                        return File(stream, contentType, fileName);
                    }
                    else
                    {
                        TempData["response"] = "Invalid Request.";
                        return null;
                    }
                }
                else
                {
                    TempData["response"] = "Server not responding.";
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public IActionResult AddVendorShortcut(Vendor vendor)
        {
            try
            {
                var number = "";
                if (vendor.VendorCode != null)
                {
                    number = vendor.VendorCode;
                }
                if (number != "" && number != null)
                {
                    string path1 = @"wwwroot";
                    //if (vendor.ProfileImage != null)
                    //{
                    //    string fullPath;
                    //    fullPath = Path.GetFullPath(path1);
                    //    string uploadsFolder = fullPath + "/Images/suppliers";
                    //    var uniqueFileName = Guid.NewGuid().ToString() + "_" + vendor.ProfileImage.FileName;
                    //    vendor.ImageByPath = Path.Combine(uploadsFolder, uniqueFileName);
                    //    using (var fileStream = new FileStream(vendor.ImageByPath, FileMode.Create))
                    //    {
                    //        vendor.ProfileImage.CopyTo(fileStream);
                    //    }
                    //    vendor.ProfileImage = null;
                    //}
                    var body = JsonConvert.SerializeObject(vendor);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/VendorCreate", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        TempData["Msg"] = "Add Successfully";
                        return RedirectToAction("PurchaseOrder", "Purchase");
                    }
                    else
                    {
                        TempData["Msg"] = resp.Resp + " " + "Unable To Updates";
                        return RedirectToAction("PurchaseOrder", "Purchase");
                    }
                }
                TempData["Msg"] = "Session Expired.";
                return RedirectToAction("SessionExpire", "Home");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public JsonResult GetPurchaseOrderByInvoiceNumber(string InvoiceNumber = "", string Method = "")
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Purchase/POByInvoiceID/" + InvoiceNumber + "/" + Method, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<PurchaseOrder>> response = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ress.Resp);
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

        [HttpPost]
        public IActionResult AddStoreShortcut(Store store)
        {

            try
            {
                //var body = JsonConvert.SerializeObject(store.StoreName);
                var body = JsonConvert.SerializeObject(store);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/StoreCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
            }

        }

        [HttpPost]
        public IActionResult AddWareHouseshortcut(WareHouse Warehousedata)
        {

            try
            {
                var body = JsonConvert.SerializeObject(Warehousedata);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/WareHouseCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
            }

        }


        public IActionResult AllPurchaseInvoices()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Purchase/PurchaseGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Purchase>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Purchase>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {

                        List<Purchase> responseObject = response.Data;

                        var query = responseObject.GroupBy(x => x.InvoiceNumber)
                          .Select(g => g.FirstOrDefault())
                          .ToList();

                        return View(query);
                    }
                    else
                    {
                        TempData["response"] = "No PurchaseInvoices List Found. Please Enter Purchase First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public JsonResult GetPurchaseOrderDetail(string InvoiceNumber = "")
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Purchase/PurchaseByID" + "/" + InvoiceNumber, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Purchase>> response = JsonConvert.DeserializeObject<ResponseBack<List<Purchase>>>(ress.Resp);
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




        //NewPurchase Order
        public IActionResult NewPurchaseOrderGet()
        {
            PurchaseOrder model = new PurchaseOrder();
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api", "Purchase/PurchaseOrderGet", "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<PurchaseOrder>> record = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        PurchaseOrder newitems = new PurchaseOrder();
                        var fullcode = "";
                        if (record.Data[0].InvoiceNumber != null && record.Data[0].InvoiceNumber != "string" && record.Data[0].InvoiceNumber != "")
                        {
                            int large, small;
                            int PurchaseID = 0;
                            var invoicenumber = record.Data[0].InvoiceNumber.Replace(@"-", string.Empty);
                            large = Convert.ToInt32(invoicenumber);
                            small = Convert.ToInt32(invoicenumber);
                            for (int i = 0; i < record.Data.Count(); i++)
                            {
                                var invoicenumberloop = record.Data[i].InvoiceNumber.Replace(@"-", string.Empty);
                                if (invoicenumberloop != null)
                                {
                                    var t = Convert.ToInt32(invoicenumberloop);
                                    if (Convert.ToInt32(invoicenumberloop) > large)
                                    {
                                        PurchaseID = Convert.ToInt32(record.Data[i].PurchaseorderId);
                                        large = Convert.ToInt32(invoicenumberloop);
                                    }
                                    else if (Convert.ToInt32(invoicenumberloop) < small)
                                    {
                                        small = Convert.ToInt32(invoicenumberloop);
                                    }
                                    else
                                    {
                                        if (large <= 2)
                                        {
                                            PurchaseID = Convert.ToInt32(record.Data[i].PurchaseorderId);
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.PurchaseorderId == PurchaseID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.InvoiceNumber != null)
                                {
                                    var VcodeSplit = newitems.InvoiceNumber.Replace(@"-", string.Empty);
                                    int code = Convert.ToInt32(VcodeSplit) + 1;
                                    long checknumber = Convert.ToInt64(VcodeSplit);
                                    if (code <= 9)
                                    {
                                        fullcode = "0000000-" + Convert.ToString(code);
                                    }
                                    else
                                    {
                                        if (code > 9999999)
                                        {
                                            fullcode = Convert.ToString(code).Insert(7, "-");
                                        }
                                        else if (code > 999999)
                                        {
                                            fullcode = "0" + Convert.ToString(code).Insert(6, "-");
                                        }
                                        else if (code > 99999)
                                        {
                                            fullcode = "00" + Convert.ToString(code).Insert(5, "-");
                                        }
                                        else if (code > 9999)
                                        {
                                            fullcode = "000" + Convert.ToString(code).Insert(4, "-");
                                        }
                                        else if (code > 999)
                                        {
                                            fullcode = "0000" + Convert.ToString(code).Insert(3, "-");
                                        }
                                        else if (code > 99)
                                        {
                                            fullcode = "00000" + Convert.ToString(code).Insert(2, "-");
                                        }
                                        else if (code > 9)
                                        {
                                            fullcode = "000000" + Convert.ToString(code).Insert(1, "-");
                                        }
                                    }
                                }
                                else
                                {
                                    fullcode = "0000000-1";
                                }
                            }
                            else
                            {
                                fullcode = "0000000-1";
                            }
                        }
                        else
                        {
                            fullcode = "0000000-1";
                        }
                        ViewBag.Invoicenumber = fullcode;
                    }
                    else
                    {
                        ViewBag.Invoicenumber = "0000000-1";
                    }
                    model.InvoiceNumber = ViewBag.Invoicenumber;
                }
                else
                {
                    ViewBag.Invoicenumber = "0000000-1";
                }

                List<SelectListItem> PaymentTerm = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Cash",Value="Cash"},
                    new SelectListItem(){Text="Bank", Value="Bank"},
                    new SelectListItem(){Text="Cash & Bank",Value="Cash & Bank"},
                    new SelectListItem(){Text="Cash & Credit",Value="Cash & Credit"},
                };
                ViewBag.PaymentTerm = new SelectList(PaymentTerm, "Value", "Text");

                SResponse ItemCategoryGet = RequestSender.Instance.CallAPI("api",
                     "Inventory/ItemCategoryGet", "GET");
                if (ItemCategoryGet.Status && (ItemCategoryGet.Resp != null) && (ItemCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemCategory> responseObject = response.Data;
                        ViewBag.ItemCategory = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<ItemCategory> responseObject = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject, "Id", "Name");
                    }
                }
                else
                {
                    List<ItemCategory> listItemCategory = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(listItemCategory, "Id", "Name");
                }




                List<SelectListItem> Title = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Mr.",Value="Mr"},
                    new SelectListItem(){Text="Mrs.",Value="Mrs"}
                };
                ViewData["Gender"] = Title;

                List<SelectListItem> VendorType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="AT-Unclassified Acquirer",Value="AT Unclassified Acquirer"},
                    new SelectListItem(){Text="Inter-Company", Value="InterCompany"},
                    new SelectListItem(){Text="PM-Participate",Value="PMParticipate"},
                    new SelectListItem(){Text="ST-Secondary Wholesale",Value="STSecondaryWholesale"},
                    new SelectListItem(){Text="ST-Secondary Wholesale",Value="STSecondaryWholesale"},
                    new SelectListItem(){Text="WT-Wholesale",Value="WTWholesale"},
                    new SelectListItem(){Text="WT-Wholesale",Value="WTWholesale"},
                };
                ViewData["VendorType"] = VendorType;

                //List<SelectListItem> PaymentTerm = new List<SelectListItem>()
                //{
                //    new SelectListItem(){Text="Cash",Value="Cash"},
                //    new SelectListItem(){Text="Bank", Value="Bank"},
                //    new SelectListItem(){Text="Cash & Bank",Value="Cash & Bank"},
                //    new SelectListItem(){Text="Cash & Credit",Value="Cash & Credit"},
                //};
                //ViewBag.PaymentTerm = new SelectList(PaymentTerm, "Value", "Text");

                //  AccountSubGroup GET
                SResponse resss = RequestSender.Instance.CallAPI("api",
                "Inventory/AccountSubGroupsGet", "GET");
                if (resss.Status && (resss.Resp != null) && (resss.Resp != ""))
                {
                    ResponseBack<List<AccountSubGroup>> response = JsonConvert.DeserializeObject<ResponseBack<List<AccountSubGroup>>>(resss.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<AccountSubGroup> responseObject = response.Data;
                        ViewBag.AccSubGroup = new SelectList(responseObject, "AccountSubGroupId", "Title");
                    }
                    else
                    {
                        List<AccountSubGroup> listEmpDocType = new List<AccountSubGroup>();
                        ViewBag.AccSubGroup = new SelectList(listEmpDocType, "AccountSubGroupId", "Title");
                    }
                }
                else
                {
                    List<AccountSubGroup> listEmpNo = new List<AccountSubGroup>();
                    ViewBag.AccSubGroup = new SelectList(listEmpNo, "AccountSubGroupId", "Title");
                }
                //  DOCUMENT TYPE GET
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/SupplierDocumentTypeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<SupplierDocumentType>> response = JsonConvert.DeserializeObject<ResponseBack<List<SupplierDocumentType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<SupplierDocumentType> responseObject = response.Data;
                        ViewBag.SupDocType = new SelectList(responseObject, "DocumentTypeId", "DocumentType");
                    }
                    else
                    {
                        List<SupplierDocumentType> listEmpDocType = new List<SupplierDocumentType>();
                        ViewBag.SupDocType = new SelectList(listEmpDocType, "DocumentTypeId", "DocumentType");
                    }
                }
                else
                {
                    List<SupplierDocumentType> listEmpNo = new List<SupplierDocumentType>();
                    ViewBag.SupDocType = new SelectList(listEmpNo, "DocumentTypeId", "DocumentType");
                }

                //SUPPLIER TYPE GET
                SResponse Supress = RequestSender.Instance.CallAPI("api",
                "Inventory/SupplierTypeGet", "GET");
                if (Supress.Status && (Supress.Resp != null) && (Supress.Resp != ""))
                {
                    ResponseBack<List<SupplierType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<SupplierType>>>(Supress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<SupplierType> responseObject = response.Data;
                        ViewBag.SupType = new SelectList(responseObject, "VendorId", "VendorType");
                    }
                    else
                    {
                        List<SupplierType> listEmpDocType = new List<SupplierType>();
                        ViewBag.SupType = new SelectList(listEmpDocType, "VendorId", "VendorType");
                    }
                }
                else
                {
                    List<SupplierDocumentType> listEmpNo = new List<SupplierDocumentType>();
                    ViewBag.SupType = new SelectList(listEmpNo, "VendorId", "VendorType");
                }

                return View();

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpPost]
        public IActionResult CreatePurchaseOrder([FromBody] List<JsonPurchaseOrder> PurchaseDetails)
        {
            try
            {

                PurchaseOrder model = null;
                List<PurchaseOrder> modellist = new List<PurchaseOrder>();

                foreach (JsonPurchaseOrder purchase in PurchaseDetails)
                {
                    model = new PurchaseOrder();
                    if (purchase.CheckNumber != null && purchase.CheckNumber != "undefined")
                    {
                        var removechar = purchase.CheckNumber.Trim(new Char[] { ' ', '|', ':' });
                        model.CheckNumber = removechar;
                    }
                    if (purchase.CheckDate != null && Convert.ToString(purchase.CheckDate) != "undefined")
                    {

                        DateTime dt = Convert.ToDateTime(purchase.CheckDate);
                        dt = dt.AddDays(1);
                        //purchase.CheckDate =  purchase.CheckDate.AddDays(1);
                        model.CheckDate = dt;
                        
                    }
                    if (purchase.CheckTitle != null && purchase.CheckTitle != "undefined")
                    {
                        model.CheckTitle = purchase.CheckTitle;
                    }
                    if (purchase.Supplieritems != null && purchase.Supplieritems != "undefined")
                    {
                        model.Supplieritems = Convert.ToBoolean(purchase.Supplieritems);
                    }
                    if (purchase.SupplierId != null && purchase.SupplierId != "undefined")
                    {
                        model.SupplierId = Convert.ToInt32(purchase.SupplierId);
                    }
                    if (purchase.SupplierName != null && purchase.SupplierName != "undefined")
                    {
                        model.SupplierName = purchase.SupplierName;
                    }
                    if (purchase.SupplierNumber != null && purchase.SupplierNumber != "undefined")
                    {
                        model.SupplierNumber = purchase.SupplierNumber;
                    }
                    if (purchase.ShowAllItems != null && purchase.ShowAllItems != "undefined")
                    {
                        model.ShowAllItems = Convert.ToBoolean(purchase.ShowAllItems);
                    }
                    if (purchase.TotalItems != null && purchase.TotalItems != "undefined")
                    {
                        model.TotalItems = purchase.TotalItems;
                    }
                    if (purchase.UpdateCost != null && purchase.UpdateCost != "undefined")
                    {
                        model.UpdateCost = Convert.ToBoolean(purchase.UpdateCost);
                    }
                    if (purchase.UpdateOscost != null && purchase.UpdateOscost != "undefined")
                    {
                        model.UpdateOscost = Convert.ToBoolean(purchase.UpdateOscost);
                    }
                    if (purchase.Received != null && purchase.Received != "undefined")
                    {
                        model.Received = Convert.ToBoolean(purchase.Received);
                    }
                    //if (purchase.ReceivedDate != null && purchase.ReceivedDate != "undefined")
                    //{
                    //    model.ReceivedDate = Convert.ToDateTime(purchase.ReceivedDate);
                    //}
                    if (purchase.Currrentuser != null && purchase.Currrentuser != "undefined")
                    {
                        model.Currrentuser = purchase.Currrentuser;
                    }
                    if (purchase.ManualInvoiceNumber != null && purchase.ManualInvoiceNumber != "undefined")
                    {
                        model.ManualInvoiceNumber = purchase.ManualInvoiceNumber;
                    }
                    //if (purchase.Podate != null && purchase.Podate != "undefined")
                    //{
                    //    model.Podate = Convert.ToDateTime(purchase.Podate);
                    //}
                    if (purchase.PaymentTerms != null && purchase.PaymentTerms != "undefined")
                    {
                        model.PaymentTerms = purchase.PaymentTerms;
                    }
                    if (purchase.TotalTobacco != null && purchase.TotalTobacco != "undefined")
                    {
                        model.TotalTobacco = purchase.TotalTobacco;
                    }
                    if (purchase.TotalCigar != null && purchase.TotalCigar != "undefined")
                    {
                        model.TotalCigar = purchase.TotalCigar;
                    }
                    if (purchase.TotalCigarette != null && purchase.TotalCigarette != "undefined")
                    {
                        model.TotalCigarette = purchase.TotalCigarette;
                    }
                    if (purchase.CigaretteStick != null && purchase.CigaretteStick != "undefined")
                    {
                        model.CigaretteStick = purchase.CigaretteStick;
                    }
                    //if (purchase.DateReceived != null && purchase.DateReceived != "undefined")
                    //{
                    //    model.DateReceived = Convert.ToDateTime(purchase.DateReceived);
                    //}
                    if (purchase.Notes != null && purchase.Notes != "undefined")
                    {
                        model.Notes = purchase.Notes;
                    }
                    if (purchase.IsReport != null && purchase.IsReport != "undefined")
                    {
                        model.IsReport = Convert.ToBoolean(purchase.IsReport);
                    }
                    if (purchase.IsPaid != null && purchase.IsPaid != "undefined")
                    {
                        model.IsPaid = Convert.ToBoolean(purchase.IsPaid);
                    }
                    //if (purchase.PaidDate != null && purchase.PaidDate != "undefined")
                    //{
                    //    model.PaidDate = Convert.ToDateTime(purchase.PaidDate);
                    //}
                    if (purchase.IsPostStatus != null && purchase.IsPostStatus != "undefined")
                    {
                        model.IsPostStatus = purchase.IsPostStatus;
                    }
                    if (purchase.PaidAmount != null && purchase.PaidAmount != "undefined")
                    {
                        model.PaidAmount = purchase.PaidAmount;
                    }
                    if (purchase.InvoiceNumber != null && purchase.InvoiceNumber != "undefined")
                    {
                        model.InvoiceNumber = purchase.InvoiceNumber;
                    }
                    if (purchase.SubTotal != null && purchase.SubTotal != "undefined")
                    {
                        string trimmed = (purchase.SubTotal as string).Trim('$');
                        model.SubTotal = trimmed;

                    }
                    if (purchase.Freight != null && purchase.Freight != "undefined")
                    {
                        model.Freight = purchase.Freight;
                    }
                    if (purchase.IsTax != null && purchase.IsTax != "undefined")
                    {
                        model.IsTax = Convert.ToBoolean(purchase.IsTax);
                    }
                    if (purchase.Tax != null && purchase.Tax != "undefined")
                    {
                        model.Tax = purchase.Tax;
                    }
                    if (purchase.Other != null && purchase.Other != "undefined")
                    {
                        model.Other = purchase.Other;
                    }
                    if (purchase.Total != null && purchase.Total != "undefined")
                    {
                        string trimmed = (purchase.Total as string).Trim('$');
                        model.Total = trimmed;

                    }
                    if (purchase.SupplierItemNumber != null && purchase.SupplierItemNumber != "undefined")
                    {
                        model.SupplierItemNumber = purchase.SupplierItemNumber;
                    }
                    if (purchase.StockItemNumber != null && purchase.StockItemNumber != "undefined")
                    {
                        model.StockItemNumber = purchase.StockItemNumber;
                    }
                    if (purchase.Description != null && purchase.Description != "undefined")
                    {
                        model.Description = purchase.Description;
                    }
                    if (purchase.Retail != null && purchase.Retail != "undefined")
                    {
                        string trimmed = (purchase.Retail as string).Trim('$');
                        model.Retail = trimmed;

                    }
                    if (purchase.IsPrice != null && purchase.IsPrice != "undefined")
                    {
                        model.IsPrice = Convert.ToBoolean(purchase.IsPrice);
                    }
                    if (purchase.Price != null && purchase.Price != "undefined")
                    {
                        string trimmed = (purchase.Price as string).Trim('$');
                        model.Price = trimmed;

                    }
                    if (purchase.IsQty != null && purchase.IsQty != "undefined")
                    {
                        model.IsQty = Convert.ToBoolean(purchase.IsQty);
                    }
                    if (purchase.Qty != null && purchase.Qty != "undefined")
                    {
                        model.Qty = purchase.Qty;
                    }
                    if (purchase.IsCaseQty != null && purchase.IsCaseQty != "undefined")
                    {
                        model.IsCaseQty = Convert.ToBoolean(purchase.IsCaseQty);
                    }
                    if (purchase.CaseQty != null && purchase.CaseQty != "undefined")
                    {
                        model.CaseQty = purchase.CaseQty;
                    }
                    if (purchase.IsDiscount != null && purchase.IsDiscount != "undefined")
                    {
                        model.IsDiscount = Convert.ToBoolean(purchase.IsDiscount);
                    }
                    if (purchase.Discount != null && purchase.Discount != "undefined")
                    {
                        string trimmed = (purchase.Discount as string).Trim('$');
                        model.Discount = trimmed;

                    }
                    //if (purchase.InvoiceDate != null && purchase.InvoiceDate != "undefined")
                    //{
                    //    model.InvoiceDate = Convert.ToDateTime(purchase.InvoiceDate);
                    //}
                    if (purchase.GrossAmount != null && purchase.GrossAmount != "undefined")
                    {
                        string trimmed = (purchase.GrossAmount as string).Trim('$');
                        model.GrossAmount = trimmed;

                    }
                    if (purchase.ItemId != null && purchase.ItemId != "undefined")
                    {
                        model.ItemId = Convert.ToInt32(purchase.ItemId);
                    }
                    if (purchase.Qty != null && purchase.Qty != "undefined")
                    {
                        model.Qty = purchase.Qty;
                    }
                    if (purchase.Amount != null && purchase.Amount != "undefined")
                    {
                        string trimmed = (purchase.Amount as string).Trim('$');
                        model.Amount = trimmed;

                    }
                    if (purchase.ItemNumber != null && purchase.ItemNumber != "undefined")
                    {
                        model.ItemCode = purchase.ItemNumber;
                    }

                    if (purchase.InvoiceNumber != null && purchase.InvoiceNumber != "undefined")
                    {
                        model.InvoiceNumber = purchase.InvoiceNumber;
                    }
                    modellist.Add(model);
                }
                var body = JsonConvert.SerializeObject(modellist);
                // TempData["products1"] = body;
                HttpContext.Session.SetString("printinvoicePurchase", body);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/PurchaseOrderCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(resp.Resp);
                    if(response.Status == 8)
                    {
                        List<PurchaseOrder> objreturn = new List<PurchaseOrder>();
                        return Json(new { success = "Justpay", Data = objreturn });
                    }
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        if (PurchaseDetails[0].IsPrintinvoice)
                        {

                            TempData["Msg"] = "Add Successfully";
                            return Json(new { success = "print", Data = responseObject });

                        }
                        else
                        {
                            TempData["Msg"] = "Add Successfully";
                            return Json(new { success = "notprint", Data = responseObject });
                        }
                    }
                    else
                    {
                        return Json(resp.Status);
                    }
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
        // [HttpGet]
        public IActionResult PurchaseInvoice(InvoiceModel model)
        {
            try
            {
                var FoundSession = HttpContext.Session.GetString("printinvoicePurchase");
               // var FoundSession = "\"[{\\\"PurchaseorderId\\\":0,\\\"SupplierId\\\":1,\\\"SupplierName\\\":\\\"HIGH POINT CASH & CARY\\\",\\\"SupplierNumber\\\":\\\"101\\\",\\\"ShowAllItems\\\":true,\\\"Supplieritems\\\":false,\\\"TotalItems\\\":\\\"10\\\",\\\"UpdateCost\\\":false,\\\"UpdateOscost\\\":false,\\\"Received\\\":false,\\\"ReceivedDate\\\":null,\\\"Currrentuser\\\":null,\\\"ManualInvoiceNumber\\\":\\\"888\\\",\\\"Podate\\\":null,\\\"PaymentTerms\\\":\\\"\\\",\\\"TotalTobacco\\\":\\\"\\\",\\\"TotalCigar\\\":\\\"\\\",\\\"TotalCigarette\\\":\\\"\\\",\\\"CigaretteStick\\\":\\\"\\\",\\\"DateReceived\\\":null,\\\"Notes\\\":\\\"\\\",\\\"IsReport\\\":false,\\\"IsPaid\\\":null,\\\"PaidDate\\\":null,\\\"IsPostStatus\\\":null,\\\"PaidAmount\\\":null,\\\"InvoiceNumber\\\":\\\"0000000-8\\\",\\\"SubTotal\\\":\\\"90.20\\\",\\\"Freight\\\":\\\"\\\",\\\"IsTax\\\":false,\\\"Tax\\\":\\\"\\\",\\\"Other\\\":\\\"\\\",\\\"Total\\\":\\\"90.20\\\",\\\"SupplierItemNumber\\\":\\\"\\\",\\\"StockItemNumber\\\":\\\"11114\\\",\\\"Description\\\":\\\"Hype Blue Mystiq 4/.99 15ct\\\",\\\"Retail\\\":\\\"7.99\\\",\\\"IsPrice\\\":false,\\\"Price\\\":\\\"9.02\\\",\\\"IsQty\\\":false,\\\"Qty\\\":\\\"10\\\",\\\"IsCaseQty\\\":false,\\\"CaseQty\\\":\\\"1\\\",\\\"IsDiscount\\\":false,\\\"Discount\\\":\\\"0.00%\\\",\\\"Amount\\\":\\\"90.20\\\",\\\"InvoiceDate\\\":null,\\\"GrossAmount\\\":null,\\\"ItemId\\\":1116,\\\"ItemCode\\\":\\\"11114\\\",\\\"ProductBarCode\\\":null,\\\"Sku\\\":null,\\\"RemaningPayment\\\":null,\\\"SimpleInvoiceNumber\\\":null,\\\"IsPartialPaid\\\":null,\\\"PaymentComments\\\":null,\\\"POrderDate\\\":null,\\\"RecievedOrderDate\\\":null,\\\"InvoicestringDate\\\":null,\\\"CheckDate\\\":null,\\\"CheckNumber\\\":null,\\\"CheckTitle\\\":null,\\\"ItemPrice\\\":null,\\\"InvoicePayments\\\":null}]\"";
                var list1 = FoundSession;
                    //TempData["products1"];
                    var test2 = JsonConvert.DeserializeObject<List<PurchaseOrder>>((string)FoundSession);
                var list2 = JsonConvert.DeserializeObject<List<PurchaseOrder>>(list1);
                var list = ViewBag.Products;
                List<PurchaseOrder> obj = new List<PurchaseOrder>();
                InvoiceModel invoiceModel = new InvoiceModel();
               // invoiceModel.PurchaseOrders = list2;
                var test = list2[0].InvoiceNumber;
                InvoiceTotal total = new InvoiceTotal();
                total.InvoiceNumber = list2[0].InvoiceNumber;
                total.SupplierNumber = list2[0].SupplierNumber;
                total.ItemCode = list2[0].ItemCode;
                total.ItemName = list2[0].Description;
                total.TotalItems = list2[0].Qty;
                total.SubTotal = list2[0].Amount;
                total.Total = list2[0].Total;
                total.Other = list2[0].Other;
                total.Freight = list2[0].Freight;
                total.Tax = list2[0].Tax;
                total.GrossTotal = list2[0].GrossAmount;

                //total.Discount = "0";
                double discounttotal = 0;
                for (int i = 0; i < list2.Count(); i++)
                {
                    list2[i].Discount = list2[i].Discount.Replace("%", "");
                    //var newSubtotal = (decimal.Parse(item.Price) * decimal.Parse(item.Qty));
                    //total.SubTotal = (decimal.Parse(total.SubTotal) + newSubtotal).ToString();
                    // var Dis = (decimal.Parse(list2[i].Discount) + (decimal.Parse(list2[i].Qty) * ((decimal.Parse(list2[i].Price) / 100) * decimal.Parse(list2[i].Discount)))).ToString();
                    decimal current = (decimal.Parse(list2[i].Qty)) * (decimal.Parse(list2[i].Price));
                    list2[i].Amount = (Math.Round((current), 2)).ToString();
                    var Dis = ((decimal.Parse(list2[i].Discount) / 100) * (current)).ToString();
                    discounttotal += Convert.ToDouble(Dis);
                    var amount = (Convert.ToDouble(list2[i].Qty) * Convert.ToDouble(list2[i].Price));
                    var FinalPriceItem = amount - Convert.ToDouble(Dis);
                    list2[i].ItemPrice = (Math.Round((FinalPriceItem), 2)).ToString();
                }
                //foreach (var item in list2)
                //{


                //}
                invoiceModel.PurchaseOrders = list2;
                invoiceModel.InvoiceTotal = total;
                total.SubTotal = (Math.Round(decimal.Parse(total.SubTotal), 2)).ToString();
                total.Discount = (Math.Round(discounttotal, 2)).ToString();
                //total.Total = (Math.Round((decimal.Parse(total.SubTotal) - decimal.Parse(total.Discount)), 2)).ToString();
                total.Total = (Math.Round(decimal.Parse(total.Total), 2)).ToString();
                string path1 = @"wwwroot";
                string path2 = @"test";
                string fullPath;
                Random rnd1 = new Random();
                fullPath = Path.GetFullPath(path1);
                fullPath = fullPath + "\\test";

                string fileName = rnd1.Next().ToString() + ".pdf";
                var pdfResult = new ViewAsPdf(invoiceModel);
                //var pdfResult = new ViewAsPdf { Model = invoiceModel, FileName = "IN-001", SaveOnServerPath = fullPath }; ;
                return pdfResult;


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult ExistingPurchaseInvoice(InvoiceModel model)
        {
            try
            {
                var list1 = TempData["productsExisting"];
                var list2 = JsonConvert.DeserializeObject<List<PurchaseOrder>>((string)list1);
                var list = ViewBag.Products;
                List<PurchaseOrder> obj = new List<PurchaseOrder>();
                InvoiceModel invoiceModel = new InvoiceModel();
                var test = list2[0].InvoiceNumber;
                InvoiceTotal total = new InvoiceTotal();
                total.InvoiceNumber = list2[0].InvoiceNumber;
                total.SupplierNumber = list2[0].SupplierNumber;
                total.ItemCode = list2[0].ItemCode;
                total.ItemName = list2[0].Description;
                total.TotalItems = list2[0].Qty;
                total.SubTotal = list2[0].Amount;
                total.Total = list2[0].Total;
                total.Other = list2[0].Other;
                total.Freight = list2[0].Freight;
                total.Tax = list2[0].Tax;
                total.GrossTotal = list2[0].GrossAmount;
                //total.Discount = "0";
                double discounttotal = 0;
                for (int i = 0; i < list2.Count(); i++)
                {
                    list2[i].Discount = list2[i].Discount.Replace("%", "");
                    //var newSubtotal = (decimal.Parse(item.Price) * decimal.Parse(item.Qty));
                    //total.SubTotal = (decimal.Parse(total.SubTotal) + newSubtotal).ToString();
                    // var Dis = (decimal.Parse(list2[i].Discount) + (decimal.Parse(list2[i].Qty) * ((decimal.Parse(list2[i].Price) / 100) * decimal.Parse(list2[i].Discount)))).ToString();
                    decimal current = (decimal.Parse(list2[i].Qty)) * (decimal.Parse(list2[i].Price));
                    list2[i].Amount = (Math.Round((current), 2)).ToString();
                    var Dis = ((decimal.Parse(list2[i].Discount) / 100) * (current)).ToString();
                    discounttotal += Convert.ToDouble(Dis);
                    var amount = (Convert.ToDouble(list2[i].Qty) * Convert.ToDouble(list2[i].Price));
                    var FinalPriceItem = amount - Convert.ToDouble(Dis);
                    list2[i].ItemPrice = (Math.Round((FinalPriceItem), 2)).ToString();
                }
                //foreach (var item in list2)
                //{
                    

                //}
                invoiceModel.PurchaseOrders = list2;
                invoiceModel.InvoiceTotal = total;
                
                total.SubTotal = (Math.Round(decimal.Parse(total.SubTotal), 2)).ToString();
                total.Discount = (Math.Round(discounttotal, 2)).ToString();
                //total.Total = (Math.Round((decimal.Parse(total.SubTotal) - decimal.Parse(total.Discount)), 2)).ToString();
                total.Total = (Math.Round(decimal.Parse(total.Total), 2)).ToString();
                string path1 = @"wwwroot";
                string path2 = @"test";
                string fullPath;
                Random rnd1 = new Random();
                fullPath = Path.GetFullPath(path1);
                fullPath = fullPath + "\\test";

                string fileName = rnd1.Next().ToString() + ".pdf";
                var pdfResult = new ViewAsPdf(invoiceModel);
                //var pdfResult = new ViewAsPdf { Model = invoiceModel, FileName = "IN-001", SaveOnServerPath = fullPath }; ;
                return pdfResult;


            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task Export(string GridHtml)
        {
            //using (MemoryStream stream = new System.IO.MemoryStream())
            //{
            //    StringReader sr = new StringReader(GridHtml);
            //    iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 100f, 0f);
            //    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, stream);
            //    pdfDoc.Open();
            //    var enc1252 = CodePagesEncodingProvider.Instance.GetEncoding(1252);
            //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
            //    pdfDoc.Close();
            //    string path1 = @"wwwroot";
            //    string fullPath;
            //    Random rnd1 = new Random();
            //    fullPath = Path.GetFullPath(path1);
            //    fullPath = fullPath + "/images/Admin/";
            //    string fileName = rnd1.Next().ToString() + ".pdf";
            //    iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, new FileStream(fullPath, FileMode.Create));

            //}

            //var list1 = TempData["products2"];
            //var list2 = JsonConvert.DeserializeObject<List<PurchaseOrder>>((string)list1);
            //ViewAsPdf pdf = new ViewAsPdf("PRVRequestPdf", list2)
            //{
            //    FileName = "Test.pdf",
            //    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            //};
            //byte[] pdfData = await pdf.BuildFile(ControllerContext);
            //string fullPath = @"\\server\network\path\pdfs\" + pdf.FileName;
            //using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            //{
            //    fileStream.Write(pdfData, 0, pdfData.Length);
            //}
        }
        [HttpPost]
        public IActionResult UpdatePurchaseOrder([FromBody] List<JsonPurchaseOrder> PurchaseDetails)
        {
            try
            {

                PurchaseOrder model = null;
                List<PurchaseOrder> modellist = new List<PurchaseOrder>();

                foreach (JsonPurchaseOrder purchase in PurchaseDetails)
                {
                    model = new PurchaseOrder();

                    if (purchase.Supplieritems != null && purchase.Supplieritems != "undefined")
                    {
                        model.Supplieritems = Convert.ToBoolean(purchase.Supplieritems);
                    }
                    if (purchase.SupplierId != null && purchase.SupplierId != "undefined")
                    {
                        model.SupplierId = Convert.ToInt32(purchase.SupplierId);
                    }
                    if (purchase.SupplierName != null && purchase.SupplierName != "undefined")
                    {
                        model.SupplierName = purchase.SupplierName;
                    }
                    if (purchase.SupplierNumber != null && purchase.SupplierNumber != "undefined")
                    {
                        model.SupplierNumber = purchase.SupplierNumber;
                    }
                    if (purchase.ShowAllItems != null && purchase.ShowAllItems != "undefined")
                    {
                        model.ShowAllItems = Convert.ToBoolean(purchase.ShowAllItems);
                    }
                    if (purchase.TotalItems != null && purchase.TotalItems != "undefined")
                    {
                        model.TotalItems = purchase.TotalItems;
                    }
                    if (purchase.UpdateCost != null && purchase.UpdateCost != "undefined")
                    {
                        model.UpdateCost = Convert.ToBoolean(purchase.UpdateCost);
                    }
                    if (purchase.UpdateOscost != null && purchase.UpdateOscost != "undefined")
                    {
                        model.UpdateOscost = Convert.ToBoolean(purchase.UpdateOscost);
                    }
                    if (purchase.Received != null && purchase.Received != "undefined")
                    {
                        model.Received = Convert.ToBoolean(purchase.Received);
                    }
                    //if (purchase.ReceivedDate != null && purchase.ReceivedDate != "undefined")
                    //{
                    //    model.ReceivedDate = Convert.ToDateTime(purchase.ReceivedDate);
                    //}
                    if (purchase.Currrentuser != null && purchase.Currrentuser != "undefined")
                    {
                        model.Currrentuser = purchase.Currrentuser;
                    }
                    if (purchase.ManualInvoiceNumber != null && purchase.ManualInvoiceNumber != "undefined")
                    {
                        model.ManualInvoiceNumber = purchase.ManualInvoiceNumber;
                    }
                    //if (purchase.Podate != null && purchase.Podate != "undefined")
                    //{
                    //    model.Podate = Convert.ToDateTime(purchase.Podate);
                    //}
                    if (purchase.PaymentTerms != null && purchase.PaymentTerms != "undefined")
                    {
                        model.PaymentTerms = purchase.PaymentTerms;
                    }
                    if (purchase.TotalTobacco != null && purchase.TotalTobacco != "undefined")
                    {
                        model.TotalTobacco = purchase.TotalTobacco;
                    }
                    if (purchase.TotalCigar != null && purchase.TotalCigar != "undefined")
                    {
                        model.TotalCigar = purchase.TotalCigar;
                    }
                    if (purchase.TotalCigarette != null && purchase.TotalCigarette != "undefined")
                    {
                        model.TotalCigarette = purchase.TotalCigarette;
                    }
                    if (purchase.CigaretteStick != null && purchase.CigaretteStick != "undefined")
                    {
                        model.CigaretteStick = purchase.CigaretteStick;
                    }
                    //if (purchase.DateReceived != null && purchase.DateReceived != "undefined")
                    //{
                    //    model.DateReceived = Convert.ToDateTime(purchase.DateReceived);
                    //}
                    if (purchase.Notes != null && purchase.Notes != "undefined")
                    {
                        model.Notes = purchase.Notes;
                    }
                    if (purchase.IsReport != null && purchase.IsReport != "undefined")
                    {
                        model.IsReport = Convert.ToBoolean(purchase.IsReport);
                    }
                    if (purchase.IsPaid != null && purchase.IsPaid != "undefined")
                    {
                        model.IsPaid = Convert.ToBoolean(purchase.IsPaid);
                    }
                    //if (purchase.PaidDate != null && purchase.PaidDate != "undefined")
                    //{
                    //    model.PaidDate = Convert.ToDateTime(purchase.PaidDate);
                    //}
                    if (purchase.IsPostStatus != null && purchase.IsPostStatus != "undefined")
                    {
                        model.IsPostStatus = purchase.IsPostStatus;
                    }
                    if (purchase.PaidAmount != null && purchase.PaidAmount != "undefined")
                    {
                        model.PaidAmount = purchase.PaidAmount;
                    }
                    if (purchase.InvoiceNumber != null && purchase.InvoiceNumber != "undefined")
                    {
                        model.InvoiceNumber = purchase.InvoiceNumber;
                    }
                    if (purchase.SubTotal != null && purchase.SubTotal != "undefined")
                    {
                        string trimmed = (purchase.SubTotal as string).Trim('$');
                        model.SubTotal = trimmed;

                    }
                    if (purchase.Freight != null && purchase.Freight != "undefined")
                    {
                        model.Freight = purchase.Freight;
                    }
                    if (purchase.IsTax != null && purchase.IsTax != "undefined")
                    {
                        model.IsTax = Convert.ToBoolean(purchase.IsTax);
                    }
                    if (purchase.Tax != null && purchase.Tax != "undefined")
                    {
                        model.Tax = purchase.Tax;
                    }
                    if (purchase.Other != null && purchase.Other != "undefined")
                    {
                        model.Other = purchase.Other;
                    }
                    if (purchase.Total != null && purchase.Total != "undefined")
                    {
                        string trimmed = (purchase.Total as string).Trim('$');
                        model.Total = trimmed;

                    }
                    if (purchase.SupplierItemNumber != null && purchase.SupplierItemNumber != "undefined")
                    {
                        model.SupplierItemNumber = purchase.SupplierItemNumber;
                    }
                    if (purchase.StockItemNumber != null && purchase.StockItemNumber != "undefined")
                    {
                        model.StockItemNumber = purchase.StockItemNumber;
                    }
                    if (purchase.Description != null && purchase.Description != "undefined")
                    {
                        model.Description = purchase.Description;
                    }
                    if (purchase.Retail != null && purchase.Retail != "undefined")
                    {
                        string trimmed = (purchase.Retail as string).Trim('$');
                        model.Retail = trimmed;

                    }
                    if (purchase.IsPrice != null && purchase.IsPrice != "undefined")
                    {
                        model.IsPrice = Convert.ToBoolean(purchase.IsPrice);
                    }
                    if (purchase.Price != null && purchase.Price != "undefined")
                    {
                        string trimmed = (purchase.Price as string).Trim('$');
                        model.Price = trimmed;

                    }
                    if (purchase.IsQty != null && purchase.IsQty != "undefined")
                    {
                        model.IsQty = Convert.ToBoolean(purchase.IsQty);
                    }
                    if (purchase.Qty != null && purchase.Qty != "undefined")
                    {
                        model.Qty = purchase.Qty;
                    }
                    if (purchase.IsCaseQty != null && purchase.IsCaseQty != "undefined")
                    {
                        model.IsCaseQty = Convert.ToBoolean(purchase.IsCaseQty);
                    }
                    if (purchase.CaseQty != null && purchase.CaseQty != "undefined")
                    {
                        model.CaseQty = purchase.CaseQty;
                    }
                    if (purchase.IsDiscount != null && purchase.IsDiscount != "undefined")
                    {
                        model.IsDiscount = Convert.ToBoolean(purchase.IsDiscount);
                    }
                    if (purchase.Discount != null && purchase.Discount != "undefined")
                    {
                        string trimmed = (purchase.Discount as string).Trim('$');
                        model.Discount = trimmed;

                    }
                    //if (purchase.InvoiceDate != null && purchase.InvoiceDate != "undefined")
                    //{
                    //    model.InvoiceDate = Convert.ToDateTime(purchase.InvoiceDate);
                    //}
                    if (purchase.GrossAmount != null && purchase.GrossAmount != "undefined")
                    {
                        string trimmed = (purchase.GrossAmount as string).Trim('$');
                        model.GrossAmount = trimmed;

                    }
                    if (purchase.ItemId != null && purchase.ItemId != "undefined")
                    {
                        model.ItemId = Convert.ToInt32(purchase.ItemId);
                    }
                    if (purchase.Qty != null && purchase.Qty != "undefined")
                    {
                        model.Qty = purchase.Qty;
                    }
                    if (purchase.Amount != null && purchase.Amount != "undefined")
                    {
                        string trimmed = (purchase.Amount as string).Trim('$');
                        model.Amount = trimmed;

                    }
                    if (purchase.ItemNumber != null && purchase.ItemNumber != "undefined")
                    {
                        model.ItemCode = purchase.ItemNumber;
                    }
                    if (purchase.PurchaseorderId != null && purchase.PurchaseorderId != "undefined" && purchase.PurchaseorderId != "")
                    {
                        model.PurchaseorderId = Convert.ToInt32(purchase.PurchaseorderId);
                    }
                    if (purchase.SimpleInvoiceNumber != null && purchase.SimpleInvoiceNumber != "undefined" && purchase.SimpleInvoiceNumber != "")
                    {
                        model.SimpleInvoiceNumber = purchase.SimpleInvoiceNumber;
                    }
                    modellist.Add(model);
                }
                var body = JsonConvert.SerializeObject(modellist);
                //TempData["products1"] = body;
                HttpContext.Session.SetString("printinvoicePurchase", body);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/PurchaseOrderUpdate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(resp.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        if (PurchaseDetails[0].IsPrintinvoice)
                        {

                            ///return RedirectToAction("PurchaseInvoice");
                            TempData["Msg"] = "Update Successfully";
                            return Json(new { success = "print", Data = responseObject });

                        }
                        else
                        {
                            TempData["Msg"] = "Update Successfully";
                            return Json(new { success = "notprint", Data = responseObject });

                            // return Json(resp.Status);
                        }
                    }
                    else
                    {
                        return Json(resp.Status);
                    }

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
        public JsonResult SaveSupplierItemNumber(SupplierItemNumber obj)
        {
            try
            {
                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/SupplierItemNumberCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }

        //Json Supplier No Search
        public IActionResult GetSupplierJson()
        {
            try
            {

                List<Vendor> responseObject = new List<Vendor>();
                var FoundSession = HttpContext.Session.GetString("loadedVendors");
                List<Vendor> FoundSession_Result = new List<Vendor>();
                if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Vendor>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/OptimizedVendors", "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<Vendor>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            responseObject = response.Data;
                            HttpContext.Session.SetString("loadedVendors", JsonConvert.SerializeObject(responseObject));

                            return Json(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "Unable to get details of Items.";
                            return Json(JsonConvert.DeserializeObject("false."));
                        }
                    }
                }
                else
                {
                    return Json(FoundSession_Result);

                }

                return Json(JsonConvert.DeserializeObject("false."));
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public IActionResult GetJsonVendorDetail(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/NewVendorGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Vendor>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get details of Items.";
                        return Json(false);
                    }
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        public IActionResult GetItemJson()
        {
            try
            {

                List<Product> responseObject = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/OptimizeItems" + "/", "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            responseObject = response.Data;
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responseObject));
                            return Json(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "Unable to get details of Items.";
                            return Json(JsonConvert.DeserializeObject("false."));
                        }
                    }
                }
                else
                {
                    return Json(FoundSession_Result);
                }

                return Json(JsonConvert.DeserializeObject("false."));
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }



        public JsonResult GetItemByItemName(string itemname)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetWithStockAndFinancialByItemName/" + itemname, "GET");
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
                    TempData["response"] = "List Is Empty.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetItemByStockItemNumber(string itemnumber)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetStockByItemNumber/" + itemnumber, "GET");
            var Msg = "";

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "List Is Empty.";
                }
            }
            return Json(Msg);
        }


        public JsonResult GetItemByItemNumber(string itemnumber)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetItemByItemNumber/" + itemnumber, "GET");
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
                    TempData["response"] = "List Is Empty.";
                }
            }
            return Json(Msg);
        }

        [HttpPost]
        public JsonResult GetItemBySupplierItemNumber(SupplierItemNumber obj)
        {
            var body = JsonConvert.SerializeObject(obj);
            SResponse res = RequestSender.Instance.CallAPI("api", "Inventory/GetItemBySupplierItemNumber", "POST", body);

            //SResponse res = RequestSender.Instance.CallAPI("api", "Inventory/GetItemBySupplierItemNumber/" + supplieritemno, "GET");
            var Msg = "";
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(res.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    TempData["response"] = "List is Empty.";

                }

            }
            return Json(Msg);
        }



        [HttpPost]
        public JsonResult SaveSupplier(SupplierItemNumber obj)
        {

            try
            {
                //var body = JsonConvert.SerializeObject(store.StoreName);
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/SaveSupplierItemNumber", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<SupplierItemNumber> responseobj = JsonConvert.DeserializeObject<ResponseBack<SupplierItemNumber>>(resp.Resp);

                    if (responseobj.Message == "Already Exists.")
                    {
                        TempData["response"] = "Already Exists.";
                        return Json("Already Exists.");

                    }

                    else
                    {
                        TempData["response"] = "Add Successfully";
                        return Json("true");
                    }

                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return Json("true");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return Json("true");

            }

        }


        public JsonResult GetCountToCheckCheapestProduct(int id, string price)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetStockLevel/" + id + "/" + price, "GET");
                var Msg = "";

                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<String>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "List Is Empty.";
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public JsonResult GetListToCheckCheapestProduct(int id, string price)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetStockLevellist/" + id + "/" + price, "GET");
                var Msg = "";

                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "List Is Empty.";
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        public JsonResult GetItemListformodal()
        {
            var Msg = "";
            List<Product> responseObject = new List<Product>();
            var FoundSession = HttpContext.Session.GetString("loadedProducts");
            List<Product> FoundSession_Result = new List<Product>();
            if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
            {
                FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
            }
            if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/OptimizeItems", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Product>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                         responseObject = response.Data;
                        HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responseObject));
                        return Json(responseObject);
                    }
                    else
                    {
                        Msg = "Server is down.";
                    }
                }
            }
            else
            {
                return Json(FoundSession_Result);
            }

            return Json(Msg);
        }
        [HttpGet]
        public IActionResult GetItemsWithVenodrId(int? vendorId)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/GetItemWithVendorId/" + vendorId, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        var productDropDown = record.Data.Select(x => new { ID = x.Id, Value = x.Name });
                        var Productlist = new SelectList(productDropDown, "ID", "Value");
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
        public IActionResult GetVendorOutStateTax(int? vendorId)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/GetVendorOutStateTax/" + vendorId, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<Vendor> record = JsonConvert.DeserializeObject<ResponseBack<Vendor>>(respEmp.Resp);
                    if (record != null)
                    {
                        var IsoutodState = record.Data.OutOfStateSupplier;
                        var tax = 0.0;
                        if (IsoutodState == true)
                        {
                            tax = 2.5;
                        }
                        return Json(tax);
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
        public IActionResult GetInvoicesByVendorId(int id)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Purchase/GetInvoicesByVendorId/" + id, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<PurchaseOrder>> record = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(respEmp.Resp);
                    if (record != null && record.Data.Count() > 0)
                    {
                        var invoices = record.Data;
                        foreach (var item in invoices)
                        {
                            if (item.Podate != null)
                            {
                                DateTime dateAndTime = (DateTime)item.Podate;
                                var getdate = dateAndTime.ToString("dd/MM/yyyy");
                                item.InvoicestringDate = getdate;
                            }


                        }
                        return Json(invoices);
                    }
                    else
                    {
                        TempData["Msg"] = respEmp.Resp + " " + "No Invoice Found";
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
        public IActionResult GetpayablesByAccountId(string id)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Purchase/GetpayablesByAccountId/" + id, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<Payable> record = JsonConvert.DeserializeObject<ResponseBack<Payable>>(respEmp.Resp);
                    if (record != null)
                    {
                        var payable = record.Data;
                        return Json(payable);
                    }
                    else
                    {
                        TempData["Msg"] = respEmp.Resp + " " + "No Invoice Found";
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
        [HttpPost]
        public IActionResult SaveInvoicePayment([FromBody] SuppliersPayment supplierPayments)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(store.StoreName);
                var body = JsonConvert.SerializeObject(supplierPayments);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/InvoicePayment", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return Json(resp.Status);
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
            }
        }

        [HttpPost]
        public IActionResult AddItemfromPO(Product obj)
        {
            try
            {
                obj.BrandId = 0;
                obj.ArticleId = 0;
                obj.Unit = "oz";
                obj.ColorId = 0;
                obj.TaxExempt = true;
                obj.ShippingEnable = true;
                obj.AllowECommerce = true;
                obj.FamilyId = 0;
                obj.SalesLimit = "100";
                obj.Adjustment = "0";
                obj.QtyUnit = "1";
                obj.TaxonSale = false;
                obj.TaxOnPurchase = false;
                obj.QtyinStock = "1000";
                obj.ModelId = 0;
                obj.ItemSubCategoryId = 0;
                obj.ModelId = 0;
                obj.Units = "oz";
                obj.MaintainStockForDays = "7-14 Days";
                obj.ShipmentLimit = "50";
                obj.NeedHighAuthorization = false;


                obj.Financial.UnitCharge = obj.Retail;
                obj.Financial.FixedCost = false;
                obj.Financial.St = "0";
                obj.Financial.Tax = "0";
                obj.Financial.Price = obj.Retail;
                obj.Financial.AutoSetSrp = false;
                obj.Financial.QuantityInStock = obj.QtyinStock;
                obj.Financial.AskForPricing = false;
                obj.Financial.AskForDescrip = false;
                obj.Financial.Serialized = false;
                obj.Financial.TaxOnSales = true;
                obj.Financial.Purchase = true;
                obj.Financial.NoSuchDiscount = false;
                obj.Financial.NoReturns = false;
                obj.Financial.SellBelowCost = false;
                obj.Financial.CodeA = false;
                obj.Financial.CodeB = false;
                obj.Financial.CodeC = false;
                obj.Financial.CodeD = false;
                obj.Financial.Retail = obj.Retail;
                obj.Financial.Quantity = "1000";

                //"{\"Stock\":null,\"Financial\":{\"FinancialId\":0,\"Cost\":\"50\",\"Profit\":\"37.50\",\"MsgPromotion\":null,\"AddToCost\":null,\"UnitCharge\":null,\"FixedCost\":null,\"CostPerQuantity\":null,\"St\":null,\"Tax\":null,\"OutOfStateCost\":null,\"OutOfStateRetail\":null,\"Price\":null,\"Quantity\":null,\"QuantityPrice\":null,\"SuggestedRetailPrice\":null,\"AutoSetSrp\":false,\"ItemNumber\":null,\"QuantityInStock\":null,\"Adjustment\":null,\"AskForPricing\":false,\"AskForDescrip\":false,\"Serialized\":false,\"TaxOnSales\":false,\"Purchase\":false,\"NoSuchDiscount\":false,\"NoReturns\":false,\"SellBelowCost\":false,\"OutOfState\":null,\"CodeA\":false,\"CodeB\":false,\"CodeC\":false,\"CodeD\":false,\"AddCustomersDiscount\":null,\"ItemName\":null,\"Retail\":\"80\",\"ItemId\":null},\"Id\":0,\"Name\":\"pepsi\",\"ItemCategoryId\":12,\"BrandId\":5,\"ArticleId\":null,\"StoreId\":null,\"Unit\":null,\"ProductCode\":null,\"BarCode\":null,\"Size\":null,\"ColorId\":null,\"Sku\":null,\"Description\":null,\"UnitRetail\":\"40.00\",\"SaleRetail\":null,\"TaxExempt\":null,\"ShippingEnable\":null,\"AllowECommerce\":null,\"CreatedDate\":null,\"CreatedBy\":null,\"ModifiedBy\":null,\"ModifiedDate\":null,\"OldPrice\":null,\"MsareportAs\":null,\"StateReportAs\":null,\"ReportingWeight\":null,\"FamilyId\":3,\"Family\":null,\"QtyUnit\":null,\"UnitsInPack\":null,\"RetailPackPrice\":null,\"SalesLimit\":null,\"Adjustment\":null,\"ProfitPercentage\":null,\"Cost\":null,\"MfgPromotion\":null,\"AddtoCostPercenatge\":null,\"UnitCharge\":null,\"OutofstateCost\":null,\"OutofstateRetail\":null,\"TaxonSale\":null,\"TaxOnPurchase\":null,\"Location\":null,\"GroupId\":2,\"ItemNumber\":null,\"QtyinStock\":null,\"ItemSubCategoryId\":8,\"ModelId\":null,\"ModelName\":null,\"CategoryName\":null,\"SubCatName\":null,\"GroupName\":null,\"BrandName\":null,\"ColorName\":null,\"ItemImage\":null,\"ItemImageByPath\":null,\"Variations\":null,\"DiscountPrice\":null,\"Rating\":null,\"MinOrderQty\":\"undefined\",\"MaxOrderQty\":null,\"Retail\":\"80\",\"QuantityCase\":null,\"QuantityPallet\":null,\"SingleUnitMsa\":null,\"MisPickId\":null,\"MisPickName\":null,\"OrderQuantity\":null,\"Units\":\"undefined\",\"WeightOz\":null,\"WeightLb\":null,\"LocationTwo\":null,\"LocationThree\":null,\"LocationFour\":null,\"MaintainStockForDays\":null,\"IsActive\":null}"
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<Product> responseobj = JsonConvert.DeserializeObject<ResponseBack<Product>>(resp.Resp);

                    //var srt = JsonConvert.SerializeObject(responseobj.Data);
                    //httpContextAccessor.HttpContext.Session.SetString("CurrentProductdata", srt);
                    if (responseobj != null)
                    {
                        if (responseobj.Message == "Already Exists.")
                        {
                            return Json("Already Exists.");
                        }
                        else
                        {
                            if (obj.SupplierItemNumber.SupplierItemNum != "")
                            {
                                responseobj.Data.SupplierItemNumber.ProductId = responseobj.Data.Id;

                                var Msg = "";
                                var body1 = JsonConvert.SerializeObject(responseobj.Data.SupplierItemNumber);

                                SResponse resp1 = RequestSender.Instance.CallAPI("api", "Inventory/SaveSupplierItemNumber", "POST", body1);
                                if (resp1.Status && (resp1.Resp != null) && (resp1.Resp != ""))
                                {
                                    ResponseBack<SupplierItemNumber> responseobj1 = JsonConvert.DeserializeObject<ResponseBack<SupplierItemNumber>>(resp1.Resp);

                                    if (responseobj1.Message == "Already Exists.")
                                    {
                                        TempData["response"] = "Already Exists.";
                                        //return Json("Already Exists.");
                                        return Json(responseobj.Data);

                                    }

                                    else
                                    {
                                        TempData["response"] = "Add Successfully";
                                        //return Json("true");
                                        return Json(responseobj.Data);
                                    }

                                }
                                else
                                {
                                    TempData["response"] = resp1.Resp + " " + "Unable To Add";
                                    // return Json("true");
                                    return Json(responseobj.Data);
                                }

                                // TempData["Msg"] = "Add Successfully";
                            }
                            else
                            {
                                return Json(responseobj.Data);
                            }
                        }


                    }
                    TempData["Msg"] = "Add Successfully";




                    return Content("true");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";


                    return RedirectToAction("NewPurchaseOrderGet", "Purchase");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }


        [HttpPost]
        public IActionResult GetItemCostChangeList(string invoicenumber)
        {

            SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/GetItemCostChangeList/" + invoicenumber, "GET");
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                ResponseBack<List<ItemCostChange>> record = JsonConvert.DeserializeObject<ResponseBack<List<ItemCostChange>>>(resp.Resp);
                if (record != null)
                {
                    var itemcostchange = record.Data;
                    return Json(itemcostchange);
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "No Invoice Found";
                    return Json(resp.Status);
                }
            }
            else
            {
                TempData["Msg"] = resp.Resp + " " + "Unable To search";
                return Json(resp.Status);
            }
        }


        [HttpGet]
        public IActionResult GetProductDataWithStockItemNo(string id)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/GetproductByStockNo/" + id, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<Product> record = JsonConvert.DeserializeObject<ResponseBack<Product>>(respEmp.Resp);
                    if (record != null)
                    {
                        return Json(record.Data);
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
        public IActionResult GetProductDataWithDesc(string Name)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/GetproductByStockDesc/" + Name, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<Product> record = JsonConvert.DeserializeObject<ResponseBack<Product>>(respEmp.Resp);
                    if (record != null)
                    {
                        return Json(record.Data);
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

        public JsonResult GetItemsFetch(int id)
        {
            string Msg = "";

            SResponse ress = RequestSender.Instance.CallAPI("api",
                   "Inventory/ItemGetByID/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {

                ResponseBack<Product> response =
                              JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<Product> record = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                    if (record != null)
                    {
                        return Json(record.Data);
                    }
                    else
                    {
                        TempData["Msg"] = ress.Resp + " " + "No Product Found";
                        return Json(ress.Status);
                    }
                }
                else
                {
                    TempData["Msg"] = ress.Resp + " " + "Unable To search";
                    return Json(ress.Status);
                }

            }

            return Json(Msg);
        }

        public JsonResult GetCustomerauto()
        {
            var Msg = "";


            List<CustomerInformation> responseObjectPro = new List<CustomerInformation>();
            var FoundSession = HttpContext.Session.GetString("loadedCustomers");
            List<CustomerInformation> FoundSession_Result = new List<CustomerInformation>();
            if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
            {
                FoundSession_Result = JsonConvert.DeserializeObject<List<CustomerInformation>>(FoundSession);
            }
            if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/CustomerInformationfetch", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        HttpContext.Session.SetString("loadedCustomers", JsonConvert.SerializeObject(responseObject));
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Record Found.";
                    }
                }
            }
            else
            {
                return Json(FoundSession_Result);
            }
               
                
            return Json(Msg);
        }
        public JsonResult GetCustomerByID(int id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/CustomerDataByID" + "/" + id, "GET");
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
                    TempData["response"] = "No Record Found.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetCustomerBycustomerCode(string id)
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/CustomerDataByCode" + "/" + id, "GET");
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
                    TempData["response"] = "No Record Found.";
                }
            }
            return Json(Msg);
        }


        public JsonResult ItemWithUcpAndItemNo(string itemno)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Purchase/GetItemByUcpOrItemNo" + "/" + itemno, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<Product> response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
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






        public JsonResult GetOpenPurchase()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Purchase/GetOpenedPurchase", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    ResponseBack<List<PurchaseOrder>> response =
                                JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ress.Resp);

                    if (response.Data.Count() > 0)
                    {

                        //List<PurchaseOrder> responseObject = response.Data;
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



        public JsonResult GetPostPurchase()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Purchase/GetPostedPurchase", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    ResponseBack<List<PurchaseOrder>> response =
                                JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ress.Resp);

                    if (response.Data.Count() > 0)
                    {

                        //List<PurchaseOrder> responseObject = response.Data;

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
        public IActionResult GetJsonVendorDetailByCode(string id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/NewVendorGetByCode" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Vendor>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get details of Items.";
                        return Json(false);
                    }
                }
                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }


        [HttpGet]
        public IActionResult GenerateAutoCodeForCheque()
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/CreateCodeForCheque/", "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<int> record = JsonConvert.DeserializeObject<ResponseBack<int>>(respEmp.Resp);
                    if (record != null)
                    {
                        return Json(record.Data);
                    }
                    else
                    {
                        return Json(respEmp.Status);
                    }
                }
                else
                {
                    return Json(respEmp.Status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult AddCreditSupplier(CreditAdjustment obj)
        {
            var body = JsonConvert.SerializeObject(obj);
            SResponse res = RequestSender.Instance.CallAPI("api", "Inventory/CreateCreditSupplier", "POST", body);
            var Msg = "";
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<int>>(res.Resp);
                if (response.Message == "Success.")
                {
                    var responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    TempData["response"] = "Something Went Wrong";
                    return Json("false");

                }

            }
            return Json(Msg);
        }

        [HttpGet]
        public IActionResult GetCreditSupplierById(int id,string invNo)
        {
            try
            {
                SResponse respEmp = RequestSender.Instance.CallAPI("api",
                 "Inventory/GetCreditSupplierById" + "/" + id + "/" + invNo, "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    var record = JsonConvert.DeserializeObject<ResponseBack<List<CreditAdjustment>>>(respEmp.Resp);
                    if (record.Data!= null)
                    {
                        return Json(record.Data);
                    }
                    else
                    {
                        return Json(respEmp.Status);
                    }
                }
                else
                {
                    return Json(respEmp.Status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public IActionResult SavePaymentOther([FromBody] Paying paying)
        {
            try
            {
                //var body = JsonConvert.SerializeObject(store.StoreName);
                var body = JsonConvert.SerializeObject(paying);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Purchase/SaveOtherPayment", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var ResponseResult = JsonConvert.DeserializeObject<ResponseBack<Paying>>(resp.Resp);
                    //TempData["response"] = "Add Successfully";
                    return Json(true);
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
            }
        }


        public IActionResult GetTotalPayables()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Purchase/GetAllPayables", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<double>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        //TempData["response"] = "No Document Uploaded";
                        return Json("false");
                    }
                }
                return Json("true");
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }

        public IActionResult GetTotalReceivables()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Purchase/GetAllReceivables", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<double>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        //TempData["response"] = "No Document Uploaded";
                        return Json("false");
                    }
                }
                return Json("true");
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
    }
}

