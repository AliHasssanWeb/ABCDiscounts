using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Domain.DataConfig.Configurations;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    [AllowAnonymous]
    public class POSSecurityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult POSLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult POSLogin(LoginValidate logindata)
        {
            AspNetUser data = new AspNetUser();
            // data.Id = Convert.ToInt32(userID);
            data.Email = logindata.Email;
            data.PasswordHash = logindata.PasswordHash;
            data.UserPic = null;
            data.CreatedDate = null;
            data.ExpiryDate = null;
            data.LastChangePwdDate = null;
            data.ModifiedDate = null;
            data.LastLogin = null;
            data.CreatedDate = DateTime.Now;
            data.Id = 0;
            var body = JsonConvert.SerializeObject(data);
            SResponse ress = RequestSender.Instance.CallAPI("api", "Security/PosLogin", "POST", body);
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var ressuser = JsonConvert.DeserializeObject<ResponseBack<AspNetUser>>(ress.Resp);
                if (ressuser.Message == "Invalid Email or Password.")
                {
                    TempData["response"] = "Invalid Email or Password.";
                    return View();
                }
                else
                {
                    HttpContext.Session.SetString("userobj", JsonConvert.SerializeObject(ressuser.Data));
                    if (ressuser != null)
                    {
                        SResponse ressRole = RequestSender.Instance.CallAPI("api",
                        "Security/RoleGet", "GET");
                        if (ressRole.Status && (ressRole.Resp != null) && (ressRole.Resp != ""))
                        {
                            ResponseBack<List<AspNetRole>> responseRole =
                                         JsonConvert.DeserializeObject<ResponseBack<List<AspNetRole>>>(ressRole.Resp);
                            if (responseRole.Data != null && responseRole.Data.Count() > 0)
                            {
                                var responseObject = responseRole.Data.ToList().Where(x => x.Id == ressuser.Data.RoleId).FirstOrDefault();
                                if (responseObject != null)
                                {
                                    ViewBag.CurrentEmail = ressuser.Data.Email;

                                    var FoundSession = HttpContext.Session.GetString("loadedProducts");
                                    List<Product> FoundSession_Result = new List<Product>();
                                    if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
                                    {
                                        FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                                    }
                                    if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
                                    {
                                        SResponse ressPro = RequestSender.Instance.CallAPI("api",
                                        "Security/OpenOptimizeItems", "GET");
                                        if (ressPro.Status && (ressPro.Resp != null) && (ressPro.Resp != ""))
                                        {
                                            ResponseBack<List<Product>> responsePro =
                                            JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ressPro.Resp);
                                            if (responsePro.Data != null && responsePro.Data.Count() > 0)
                                            {
                                                HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responsePro.Data));
                                            }
                                            else
                                            {
                                                HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(null));
                                            }
                                        }
                                    }
                                    
                                    if (responseObject.Name == "Cashier")
                                    {
                                        return RedirectToAction("CashierIndex", "Home");
                                    }
                                    else if (responseObject.Name == "Admin")
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("POSLogin", "POSSecurity");
                                    }
                                }
                            }
                            else
                            {
                                TempData["response"] = "Invalid Role Access.";
                                return RedirectToAction("POSLogin", "Security");
                            }
                        }
                    }
                    else
                    {
                        TempData["response"] = "Invalid User Access";
                        return RedirectToAction("POSLogin", "Security");
                    }
                }
            }
            else
            {
                TempData["response"] = "Invalid Email or Password.";
                return View();
            }
            return View();
        }

        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("POSLogin", "POSSecurity");
        }
       
    }
}
