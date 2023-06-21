using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Domain.DataConfig.Configurations;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StockInventory()
        {
            try
            {

                if (GlobalPOS.listcart.Count() > 0)
                {
                    List<Product> responseObject = GlobalPOS.listcart;
                    ViewBag.Items = new SelectList(responseObject.ToList(), "Id", "Name");
                }
                else
                {
                    SResponse ItemGet = RequestSender.Instance.CallAPI("api",
                                         "Inventory/ItemGet", "GET");
                    if (ItemGet.Status && (ItemGet.Resp != null) && (ItemGet.Resp != ""))
                    {
                        ResponseBack<List<Product>> response =
                                     JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ItemGet.Resp);
                        if (response.Data.Count() > 0)
                        {
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
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult InventoryStock()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "Inventory/StockGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Stock>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Stock>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Stock> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Stock Inventory Is Empty, Please Add Purchase.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public IActionResult StockEvaluation()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "Inventory/STockEvaluationGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<StockEvaluation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<StockEvaluation>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<StockEvaluation> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult GeneratePdfStock()
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/StockGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Stock>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Stock>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Stock> responseObject = response.Data;
                        PdfDocument doc = new PdfDocument();
                        PdfPage page = doc.Pages.Add();
                        PdfGrid pdfGridObj = new PdfGrid();
                        ReportStockInventory insertdata = null;
                        List<ReportStockInventory> dataTable = new List<ReportStockInventory>();
                        for (int i = 0; i < responseObject.Count; i++)
                        {
                            insertdata = new ReportStockInventory();
                            insertdata.ItemName = responseObject[i].ItemName;
                            insertdata.ItemCode = responseObject[i].ItemCode;
                            insertdata.Quantity = responseObject[i].Quantity;
                            insertdata.ItemBarCode = responseObject[i].ItemBarCode;
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
                        string fileName = "StockList.pdf";
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


        public IActionResult InventoryStockAlertCount()
        {
            try
            {
                //var Count = 0;
                //var Msg = "StockLevelAlert";
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/GetStockAlertCount", "GET");
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
        public IActionResult InventoryStockAlertList()
        {
            try
            {
                List<InventoryStock> responseObject = new List<InventoryStock>();
                var FoundSession = HttpContext.Session.GetString("loadedStocks");
                List<InventoryStock> FoundSession_Result = new List<InventoryStock>();
                if (FoundSession != null && FoundSession.Count() > 0)
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<InventoryStock>>(FoundSession);
                }
                if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/GetStockAlertList", "GET");
                    var Msg = "";
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<InventoryStock>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            responseObject = response.Data;
                            HttpContext.Session.SetString("loadedStocks", JsonConvert.SerializeObject(responseObject));
                            return Json(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "List Is Empty.";
                        }
                    }
                    return Json(responseObject);
                }
                else
                {
                    return Json(FoundSession_Result);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
