using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;
namespace ABC.POS.Website.Controllers
{
    public class AccountController : Controller
    {
        private static IHttpContextAccessor httpContextAccessor;
        public AccountController(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        public IActionResult Payables()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Cash/PayableGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Payable>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Payable>>>(ress.Resp);
                    List<Payable> responseObject = response.Data;
                    return View(responseObject);

                }
                else
                {
                    TempData["response"] = "Unable to get Payables.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Payables." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }


        public IActionResult Receivables()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Cash/ReceivablesGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Receivable>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Receivable>>>(ress.Resp);

                    List<Receivable> responseObject = response.Data;
                    return View(responseObject);

                }
                else
                {
                    TempData["response"] = "Unable to get Receivables.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Receivables." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public IActionResult Paying()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Cash/PayingGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Paying>> response =
                            JsonConvert.DeserializeObject<ResponseBack<List<Paying>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Paying> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }

                }
                else
                {
                    TempData["response"] = "Unable to get Paying.";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Paying." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }


        public IActionResult Receiving()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Cash/ReceivingGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Receiving>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Receiving>>>(ress.Resp);

                    List<Receiving> responseObject = response.Data;
                    return View(responseObject);

                }
                else
                {
                    TempData["response"] = "Unable to get Receiving.";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Receiving." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }
        public IActionResult GetEmployeeInfo()
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customer/GetEmployeeInfoo" + "/" + userDto.EmployeeId, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                        if (response.Data != null)
                        {

                            var responseObject = response.Data;
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Customer Detail Exists.";
                            List<Employee> obj2 = new List<Employee>();
                            return View(obj2);
                        }
                    }
                    else
                    {
                        TempData["response"] = "Session Expired";
                        return RedirectToAction("Login", "Account");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }

        }

    }
}
