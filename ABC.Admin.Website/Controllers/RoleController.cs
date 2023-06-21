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

    //[Area("Administration")]
    //[Route("Administration/Role")]
    public class RoleController : Controller
    {
        // GET: Administration/Role
        public IActionResult Index()
        {
            return View();
        }
   
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddRole(AspNetRole obj) 
        {

            try
            {
                var AccessToken = "";
                string userData = HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDetails = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDetails.UserName;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "UserManagement/RoleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessToken.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Add";
                    activity.NewDetails = "Role Added on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageRoles");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddRole");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
          
        }
        [HttpPost]
        public IActionResult UpdateRole(int id, AspNetRole obj)
        {

            try
            {
                var AccessToken = "";
                string userData = HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDetails = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDetails.UserName;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "UserManagement/RoleUpdate" + "/" + id, "PUT", body);
               
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessToken.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Update";
                    activity.NewDetails = "Role Update on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageRoles");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddRole");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        
        public IActionResult UpdateRole(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "UserManagement/RoleGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<AspNetRole>>(ress.Resp);
                    if (response.Data!=null)
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

                throw;
            }
         
        }
        
        [HttpGet]
        public IActionResult ManageRoles()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "UserManagement/RoleGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<AspNetRole>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<AspNetRole>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<AspNetRole> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult DeleteRoles(string id = "")
        {

            try
            {
                var AccessToken = "";
                string userData = HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDetails = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDetails.UserName;
                }
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "UserManagement/DeleteRole" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Delete";
                        activity.NewDetails = "Role Deleted on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageRoles");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageRoles");
                    }
                }
                return RedirectToAction("ManageRoles");
            }
            catch (Exception ex)
            {

                throw ex;
            }

    

        }

    }
}
