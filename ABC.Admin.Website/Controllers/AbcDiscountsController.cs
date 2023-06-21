using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;

namespace ABC.Admin.Website.Controllers
{
    public class AbcDiscountsController : Controller
    {
        private readonly ISession session;
        public AbcDiscountsController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddEmployer()
        {
            try
            {
                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "CompanySetup/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employer>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employer>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employer> responseObject = response.Data;
                        return RedirectToAction("CompanySetupUpdate");
                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult AddEmployer(Employer obj)
        {
            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Employer/EmployerCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = User.Identity.Name.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Company";
                    activity.NewDetails = "Company Profile Added on " + DateTime.Now.Date + " " + "by " + User.Identity.Name.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    TempData["Msg"] = "Add Successfully";
                    return RedirectToAction("AddEmployer");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployer");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult GetEmployer()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Employer/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employer>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employer>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employer> responseObject = response.Data;
                        return View(responseObject.ElementAt(0));
                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult GetEmployerForAjax()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Employer/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employer>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employer>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employer> responseObject = response.Data;
                        return Json(responseObject.ElementAt(0));
                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                        return Json("false.");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult UpdateEmployer()
        {
            try
            {
                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Employer/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employer>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employer>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employer> responseObject = response.Data;
                        return View(responseObject.ElementAt(0));
                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                        return View("AddEmployer");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public IActionResult UpdateEmployer(int id, Employer obj)
        {

            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Employer/EmployerUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Update";
                    activity.NewDetails = "Company Profile Updated on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("CompanySetup");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("CompanySetup");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult CompanySetup()
        {
            try
            {
                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "CompanySetup/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<Employer> response =
                                  JsonConvert.DeserializeObject<ResponseBack<Employer>>(ress.Resp);
                    if (response.Data != null)
                    {
                        Employer responseObject = response.Data;
                        return RedirectToAction("CompanySetupUpdate");

                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public IActionResult CompanySetup(Employer obj)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "CompanySetup/EmployerCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Company";
                    activity.NewDetails = "Company Setup Added on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    TempData["Msg"] = "Add Successfully";
                    return RedirectToAction("CompanySetup");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("CompanySetup");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult CompanySetupUpdate()
        {
            try
            {
                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "CompanySetup/EmployerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<Employer> response =
                                 JsonConvert.DeserializeObject<ResponseBack<Employer>>(ress.Resp);
                    if (response.Data != null)
                    {
                        Employer responseObject = response.Data;
                        return View(responseObject);

                    }
                    else
                    {
                        TempData["response"] = "Please Add ABCDiscounts as Employer Profile.";
                        return RedirectToAction("CompanySetup");

                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("CompanySetup", "CompanySetup");
                throw;
            }
        }
        [HttpPost]
        public IActionResult CompanySetupUpdate(Employer obj)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "CompanySetup/EmployerUpdate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Update";
                    activity.NewDetails = "Company Setup Updated on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("CompanySetupUpdate");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployer");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult StTaxes()
        {
            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "CompanySetup/StTaxGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Sttax>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<Sttax>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Sttax> responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        [HttpPost]
        public IActionResult StTax([FromBody] Sttax obj)
        {
            var Msg = "";
            var body = JsonConvert.SerializeObject(obj);
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "CompanySetup/StTaxCreate", "POST",body);
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<Sttax> response =
                              JsonConvert.DeserializeObject<ResponseBack<Sttax>>(ress.Resp);
                if (response.Data!=null)
                {
                    Sttax responseObject = response.Data;
                    return Json(responseObject);
                }

                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        [HttpPost]
        public IActionResult StTaxUpdate([FromBody] Sttax obj)
        {
            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse ress = RequestSender.Instance.CallAPI("api", "CompanySetup/StTaxUpdate" + "/" + obj.TaxId, "PUT", body);

                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("CompanySetupUpdate");
                }
                else
                {
                    TempData["response"] = ress.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageGroup");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
            //var Msg = "";
            //var body = JsonConvert.SerializeObject(obj);
            //SResponse ress = RequestSender.Instance.CallAPI("api", "CompanySetup/StTaxUpdate" + "/" + obj.TaxId, "PUT", body);

            //if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            //{
            //    ResponseBack<Sttax> response =
            //                  JsonConvert.DeserializeObject<ResponseBack<Sttax>>(ress.Resp);
            //    if (response.Data!=null)
            //    {
            //        Sttax responseObject = response.Data;
            //        return Json(responseObject);
            //    }

            //    else
            //    {
            //        Msg = "Server is down.";
            //    }
            //}
            //return Json(Msg);
        }

        public IActionResult Visitors()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "CompanySetup/GetVisitors", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Contact>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Contact>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Contact> responseObject = response.Data.ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Record Exists";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get Visitors." + ex.Message;
                return View();
            }
        }








    }
}
