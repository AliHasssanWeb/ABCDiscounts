
using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Entities.Admin;
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
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();

        }
        [HttpGet]
        public IActionResult getcustomerinfo(string accountid)
        {

            SResponse ress = RequestSender.Instance.CallAPI("api",
       "Customers/getcustomerdata" + "/" + accountid, "GET");
          
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<costomerformodel>> response =
                                    JsonConvert.DeserializeObject<ResponseBack<List<costomerformodel>>>(ress.Resp);

                var results = response.Data;
                return Json (results);
            }
            return Ok();



        }
        public IActionResult Pending_for_Approvel()
        {
            try
            {
          
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customer/CustomersinfoGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Customerdata>> response =
                                    JsonConvert.DeserializeObject<ResponseBack<List<Customerdata>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Customerdata> responseObject = response.Data;


                        var dataapprove = responseObject.Where(o => o.Status == false).ToList();
                        return View(dataapprove);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                        return View();
                    }
                }
                else
                {
                    TempData["response"] = "Server is down.";
                    return View();
                }


            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult Rejected()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customer/CustomersinfoGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Customerdata>> response =
                                    JsonConvert.DeserializeObject<ResponseBack<List<Customerdata>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Customerdata> responseObject = response.Data;


                        var dataapprove = responseObject.Where(o => o.Status == false).ToList();
                        return View(dataapprove);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                        return View();
                    }
                }
                else
                {
                    TempData["response"] = "Server is down.";
                    return View();
                }


            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpGet]
        public IActionResult approve(int accountid)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customers/CustomersapproveByID" + "/" + accountid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    return Ok();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
                

                return View();
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IActionResult blacklist(int accountid)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customers/CustomersblacklistByID" + "/" + accountid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    return Ok();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }


                return View();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public IActionResult customer_approval()
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Customer/CustomersinfoGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Customerdata>> response =
                                    JsonConvert.DeserializeObject<ResponseBack<List<Customerdata>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Customerdata> responseObject = response.Data;


                        var dataapprove = responseObject.Where(o => o.Status == true).ToList();
                        return View(dataapprove);
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
                throw;
            }
        }
    }
}
