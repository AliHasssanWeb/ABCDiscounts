using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    public class FaqsController : Controller
    {
        public IActionResult Index()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Customer/GetAllFaqs", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<Faq>>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        public IActionResult Faqs()
        {
            return View();
        }
    }
}
