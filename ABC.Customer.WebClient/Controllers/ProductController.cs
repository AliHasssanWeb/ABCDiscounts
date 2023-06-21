using ABC.Customer.Domain.Configuration;
using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABCDiscountsWebsite.Controllers
{

    [ServiceFilter(typeof(ConfigureSession))]
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Cigrattes(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
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
                return View();
            }
            catch (Exception)
            {
                throw ;
            }
        }


    }
}
