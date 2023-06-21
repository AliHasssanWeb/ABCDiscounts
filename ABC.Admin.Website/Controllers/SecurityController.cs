using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using static ABC.Admin.Domain.DataConfig.RequestSender;

namespace ABC.Admin.Website.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginValidate logindata)
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
            SResponse ress = RequestSender.Instance.CallAPI("api", "Security/UserLogin", "POST", body);
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
                        "UserManagement/RoleGet", "GET");
                        if (ressRole.Status && (ressRole.Resp != null) && (ressRole.Resp != ""))
                        {
                            ResponseBack<List<AspNetRole>> responseRole =
                                         JsonConvert.DeserializeObject<ResponseBack<List<AspNetRole>>>(ressRole.Resp);
                            if (responseRole.Data != null && responseRole.Data.Count() > 0)
                            {
                                ActivityLog activity = new ActivityLog();
                                activity.CreatedDate = DateTime.Now;
                                activity.CreatedBy = ressuser.Data.UserName.ToString();
                                activity.Deleted = false;
                                activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                                activity.OperationName = "Login";
                                activity.NewDetails = "Login on " + DateTime.Now.Date + " " + "by " + ressuser.Data.UserName.ToString();
                                activity.Extraone = "Admin";
                                var Activitybody = JsonConvert.SerializeObject(activity);

                                SResponse Activityress = RequestSender.Instance.CallAPI("api",
                                 "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                                if (ressuser.Data.RoleName != "Supervisor" && ressuser.Data.RoleName != "Sales Manager" && ressuser.Data.RoleName != "Admin")
                                {
                                    TempData["response"] = "Invalid User Account To Access This Portal.";
                                    return View();
                                }
                                if (ressuser.Data.IsCancelled == true)
                                {
                                    TempData["response"] = "User Blocked.";
                                    return View();
                                }
                                var responseObject = responseRole.Data.ToList().Where(x=>x.Id == ressuser.Data.RoleId).FirstOrDefault();
                                if(responseObject != null)
                                {
                                    if(responseObject.Name == "Supervisor" || responseObject.Name == "Sales Manager")
                                    {
                                        return RedirectToAction("Managers", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                            }
                            else
                            {
                                TempData["response"] = "Invalid Role Access.";
                                return RedirectToAction("Login", "Security");
                            }
                        }
                    }
                    else
                    {
                        TempData["response"] = "Invalid User Access";
                        return RedirectToAction("Login", "Security");
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

        public IActionResult Signoff()
        {
            string AccessToken = string.Empty;
            string AccessName = string.Empty;
            string userData =  HttpContext.Session.GetString("userobj");
            if (!string.IsNullOrEmpty(userData))
            {
                AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                AccessToken = userDto.RefreshToken;
                AccessName = userDto.UserName;
            }
            ActivityLog activity = new ActivityLog();
            activity.CreatedDate = DateTime.Now;
            activity.CreatedBy = AccessName.ToString();
            activity.Deleted = false;
            activity.LogTime = DateTime.Now.TimeOfDay.ToString();
            activity.OperationName = "Sign Out";
            activity.NewDetails = "Sign Out on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
            activity.Extraone = "Admin";
            var Activitybody = JsonConvert.SerializeObject(activity);

            SResponse Activityress = RequestSender.Instance.CallAPI("api",
             "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Security");
        }

        public ActionResult BlockAndUnblock(int id)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api", "UserManagement/BlocknUnblock" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var ressuser = JsonConvert.DeserializeObject<ResponseBack<AspNetUser>>(ress.Resp);
                if (ressuser.Data != null)
                {
                    return Json("true");
                }
                return Json("true");
            }
            else
            {
                return Json("false");
            }
            return View();
        }
    }
}
