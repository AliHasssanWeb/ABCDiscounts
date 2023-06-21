using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class ContactUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contactus(Contact Contact)
        {
            try
            {
                var body = JsonConvert.SerializeObject(Contact);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/ContactCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
