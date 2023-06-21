using ABC.Customer.Domain.Configuration;
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
    [ServiceFilter(typeof(ConfigureSession))]
    public class CustomerController : Controller
    {
       public IActionResult CustomerCertification()
        {
            return View();
        }

        public JsonResult GetJsonDataByID(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customers/CustomersGetByID/" + "/" + id, "GET");
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
                        TempData["response"] = "Unable to get details of selected Customer.";
                        return Json(JsonConvert.DeserializeObject("false."));
                    }
                }
                return Json(JsonConvert.DeserializeObject("false."));
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        
        
    }
}
