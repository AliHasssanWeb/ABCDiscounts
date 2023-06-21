
using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    public class FaqsController : Controller
    {
        [ServiceFilter(typeof(ConfigureSession))]
        public IActionResult Index()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "CompanySetup/FaqsGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Faq>> response =
                            JsonConvert.DeserializeObject<ResponseBack<List<Faq>>>(ress.Resp);
                if (response.Message == "Success.")
                {
                    List<Faq> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult FaqsGet()
        {

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "CompanySetup/FaqsGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Faq>> response =
                            JsonConvert.DeserializeObject<ResponseBack<List<Faq>>>(ress.Resp);
                if (response.Message == "Success.")
                {
                    List<Faq> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult Faqs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Faqs(Faq faq)
        {
            try
            {
                var body = JsonConvert.SerializeObject(faq);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "CompanySetup/AddFaqs", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("Faqs");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add.";
                    return RedirectToAction("FaqsGet");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("FaqsGet");
            }
        }


        [HttpGet]
        public IActionResult FaqDelete(int? id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "CompanySetup/DeleteFaq" + "/" + id, "Get");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Faq>>(ress.Resp);
                    TempData["response"] = "Delete Successfully";
                    return RedirectToAction("FaqsGet");
                }
                return Json(null);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IActionResult FaqUpate()
        {
            return View();
        }

        [HttpGet]
        public IActionResult FaqUpate(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "CompanySetup/FaqUpdate" + "/" + id, "Get");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<Faq>(ress.Resp);
                    return View(response);
                }
                return Json(null);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public IActionResult Faqsdataupdate(Faq faq)
        {
            try
            {
                var body = JsonConvert.SerializeObject(faq);
                SResponse resp = RequestSender.Instance.CallAPI("api", "CompanySetup/Faqsdataupadte", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("FaqsGet");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add.";
                    return RedirectToAction("FaqsGet");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("FaqsGet");
            }
        }

    }
}
