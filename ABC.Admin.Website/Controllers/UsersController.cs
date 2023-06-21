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
    //[Route("Administration/User")]
    public class UsersController : Controller
    {
        public  HostingEnvironment _hostingEnvironment = new HostingEnvironment();


    
        //public UsersController(HostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        // GET: Administration/Users
        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult AddUser()
        {
            StatesofAmerica ob = new StatesofAmerica();
            var allstates = ob.Get();
            ViewBag.States = new SelectList(allstates, "StateName", "StateName");

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "UserManagement/RoleGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<AspNetRole>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<AspNetRole>>>(ress.Resp);
                if (response.Data.Count() > 0 )
                {
                    List<AspNetRole> responseObject = response.Data;
                   
                    ViewBag.Roles = new SelectList(responseObject, "ID", "Name");
                    
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddUser(AspNetUser users, IFormFile file)
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
                //if (file != null && file.Length > 0)
                //{
                //    var uploadDir = @"images/Admin";
                //    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                //    var extension = Path.GetExtension(file.FileName);
                //    string webRootPath = @"wwwroot";
                //    fileName = DateTime.UtcNow.ToString("yymmssff") + fileName + extension;
                //    var path = Path.Combine(webRootPath, uploadDir, fileName);
                //    file.CopyTo(new FileStream(path, FileMode.Create));
                //    users.Imageupload = "/" + uploadDir + "/" + fileName;
                //}
                if (file != null)
                {
                    var input = file.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    users.UserPic = byteData;
                }
                
                var body = JsonConvert.SerializeObject(users);
                SResponse res = RequestSender.Instance.CallAPI("api", "UserManagement/UserCreate", "Post",body);
                if (res.Status && (res.Resp != null) && (res.Resp != "")) 
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessToken.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Add";
                    activity.NewDetails = "New Admin Added on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    TempData["Msg"] = "Add Successfully";
                    return RedirectToAction("ManageUser");
                }
                else
                {
                    TempData["Msg"] = res.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddUser");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult UpdateUser()
        {
            return View();
        }
        public IActionResult ManageUser()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "UserManagement/GetAdminUsers", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<AspNetUser>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<AspNetUser>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<AspNetUser> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult BlockUser()
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "UserManagement/BlockedUsersGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<AspNetUser>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<AspNetUser>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<AspNetUser> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult UserProfile()
        {
            AspNetUser model = new AspNetUser();
            return View(model);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }

        public IActionResult PendingEmployees()
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
                "UserManagement/EmployeeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public IActionResult DetailEmployee(int id)
        {
            try
            {
                SResponse ressrole = RequestSender.Instance.CallAPI("api",
                    "UserManagement/RoleGet", "GET");
                if (ressrole.Status && (ressrole.Resp != null) && (ressrole.Resp != ""))
                {
                    ResponseBack<List<AspNetRole>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<AspNetRole>>> (ressrole.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<AspNetRole> responseObject = response.Data.ToList().Where(x => x.Name != "Admin" && x.Name != "Administration").ToList();
                        ViewBag.Roles = new SelectList(responseObject, "Id", "Name");
                    }
                    else
                    {
                        TempData["response"] = "Request to get roles not completed.";
                        return RedirectToAction("ManageEmployees", "HR");
                    }
                }

                SResponse resss = RequestSender.Instance.CallAPI("api",
             "UserManagement/EmployeeDocumentGet", "GET");
                if (resss.Status && (resss.Resp != null) && (resss.Resp != ""))
                {
                   ResponseBack<List<EmployeeDocument>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmployeeDocument>>>(resss.Resp);
                    if (response.Data.Count > 0)
                    {
                        List<EmployeeDocument> responseObject = response.Data.ToList().Where(x => x.EmployeeId == id).ToList();

                        if (responseObject.Count > 0)
                        {
                            ViewBag.Doclist = responseObject;
                        }
                    }
                }

                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "UserManagement/EmployeeByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<Employee> response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                        return RedirectToAction("ManageEmployees", "HR");
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("ManageEmployees", "HR");
            }

        }

        

        //  ---------------------- EMPLOYEE DETAILS BUTTON -----------------

        public IActionResult Approve(int id)
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
             "UserManagement/ApprovedEmployee" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    if (response.Data != null)
                    {
                        TempData["response"] = "Employee request is approved.";
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Decision";
                        activity.NewDetails = "Employee Approved on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                        return Json(true);
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                        return RedirectToAction("DetailEmployee", "Users", new {  id = id });

                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("DetailEmployee", "Employees", new {  id = id });

            }


        }

        [HttpPost]
        public IActionResult ApproveAndRegister(EmployeeRegister data)
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
                var body = JsonConvert.SerializeObject(data);
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "UserManagement/ApprovedRegisterEmployee", "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    TempData["response"] = "Employee request is approved & registered.";

                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessToken.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Decision";
                    activity.NewDetails = "Employee Approved & Registerd on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    return Json(true);


                }
                else {
                    TempData["response"] = "Request not completed.";
                    return RedirectToAction("DetailEmployee", "Employees", new {  id = data.EmployeeID });
                }
                

                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("DetailEmployee", "Employees", new { id = data.EmployeeID });
            }
        }

        public IActionResult Reject(int id)
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
                "HR/RejectEmployee" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    if (response.Data != null)
                    {
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Decision";
                        activity.NewDetails = "Employee Rejected on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                        TempData["response"] = "Employee request is successfully rejected.";
                        return RedirectToAction("DetailEmployee", "Employees", new { id = id });
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                        return RedirectToAction("DetailEmployee", "Employees", new {  id = id });

                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("DetailEmployee", "Employees", new {  id = id });
            }
        }


        public ActionResult ShowEmployeeContract(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeContractEmployeeID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmployeeContract>>(ress.Resp);
                    if (response.Data != null)
                    {
                        TempData["response"] = "Employee request is successfully rejected.";
                        string PDFpath  =  response.Data.ContractDocumentByPath;
                        var pth = "wwwroot"+PDFpath;
                        byte[] abc = System.IO.File.ReadAllBytes(pth);
                        System.IO.File.WriteAllBytes(pth, abc);
                        MemoryStream ms = new MemoryStream(abc);
                        return new FileStreamResult(ms, "application/pdf");
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                       
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("DetailEmployee", "Employees", new { id = id });
            }

        }
        public JsonResult OpenPDFPath()
        {
            string PDFpath = "wwwroot/images/Employee Contract/21270296Aashir's Resume (1).pdf";
            return Json(PDFpath);
        }
        public FileResult OpenPDF()
        {
            string PDFpath = "wwwroot/images/Employee Contract/21270296Aashir's Resume (1).pdf";
            byte[] abc = System.IO.File.ReadAllBytes(PDFpath);
            System.IO.File.WriteAllBytes(PDFpath, abc);
            MemoryStream ms = new MemoryStream(abc);
            return new FileStreamResult(ms, "application/pdf");
        }



        public IActionResult ShowPdf(string path, int id)
        {
            try
            {
                if (path != null)
                {
                    string webRootPath = @"wwwroot";
                    path = webRootPath + path;

                    byte[] abc = System.IO.File.ReadAllBytes(path);
                    System.IO.File.WriteAllBytes(path, abc);
                    MemoryStream ms = new MemoryStream(abc);
                    return new FileStreamResult(ms, "application/pdf");

                }
                TempData["Msg"] = "Unable to Show File";
                return RedirectToAction("DetailEmployee", "Users", new {  id = id });
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to download" + ex.Message;
                return RedirectToAction("DetailEmployee", "Employees", new {  id = id });
            }
        }
        public FileResult DownloadFile(string path)
        {
            try
            {
                if (path != null)
                {
                    string webRootPath = @"wwwroot";
                    string name = Path.GetFileName(path);
                    path = webRootPath + path;
                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    return File(bytes, "application/pdf", name);
                }
                TempData["Msg"] = "Unable to download";
                return null;
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to download" + ex.Message;
                return null;
            }

        }

        public IActionResult SupervisorsAndManagers()
        {
            try
            {
                SResponse ressrole = RequestSender.Instance.CallAPI("api",
                    "UserManagement/SupervisorManagersGet", "GET");
                if (ressrole.Status && (ressrole.Resp != null) && (ressrole.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressrole.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data.ToList().ToList();
                        ViewBag.Roles = new SelectList(responseObject, "EmployeeId", "FullName");
                    }
                    else
                    {
                        TempData["response"] = "Request to get roles not completed.";
                        return RedirectToAction("SupervisorsAndManagers", "Users");
                    }
                }
                SResponse resCustomers = RequestSender.Instance.CallAPI("api",
                    "UserManagement/GetCustomers", "GET");
                if (resCustomers.Status && (resCustomers.Resp != null) && (resCustomers.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(resCustomers.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<CustomerInformation> responseObject = response.Data.ToList().ToList();
                        ViewBag.Customer = new SelectList(responseObject, "Id", "Company");
                    }
                    else
                    {
                        TempData["response"] = "Request to get customerd not completed.";
                        return RedirectToAction("SupervisorsAndManagers", "Users");
                    }
                }


                SResponse ress = RequestSender.Instance.CallAPI("api",
                "UserManagement/SupervisorAndManagerGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Supervisor>> response = JsonConvert.DeserializeObject<ResponseBack<List<Supervisor>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Supervisor> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public IActionResult EditSupervisorDetails(int id, string pin, string credit)
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
                Supervisor supervisor = new Supervisor();
                supervisor.SupervisorId = id;
                if (credit != null)
                {
                    supervisor.CreditLimit = credit;
                }
                else
                {
                    supervisor.CreditLimit = null;
                }
                if (pin != null)
                {
                    supervisor.AccessPin = pin;
                }
                else
                {
                    supervisor.AccessPin = null;
                }
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse resp = RequestSender.Instance.CallAPI("api", "UserManagement/UpdateSupervisor", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(resp.Resp);
                    if (response != null)
                    {
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Add";
                        activity.NewDetails = "Supervision Pin Added on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                        return Json("true");
                    }
                    else
                    {
                        return Json("false");
                    }
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult SupervisorGet(int id)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "UserManagement/SuperVisorGet" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Supervisor>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<Supervisor>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Supervisor> responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    List<Supervisor> responseObject = response.Data;
                    return Json(responseObject);
                }
            }
            return View();
        }


        [HttpPost]
        public IActionResult AddNewCreditAllower(Supervisor data)
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
                //string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                //if (!string.IsNullOrEmpty(userData))
                //{
                //    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                //    data.UserId = userDto.Id;
                //}
                var body = JsonConvert.SerializeObject(data);
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "UserManagement/AddCreditSupervisor", "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Message == "Success.")
                    {
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Add";
                        activity.NewDetails = "Supervision Credit Added on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);

                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                        TempData["response"] = "User is added sucessfully.";
                        return Json(true);
                    }
                    else if (response.Message == "Already Exists.")
                    {
                        TempData["response"] = "User already Exists.";
                        return Json(false);
                    }
                    return Json(true);
                }
                else
                {
                    TempData["response"] = "Request not completed.";
                    return RedirectToAction("SupervisorsAndManagers", "Users");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("SupervisorsAndManagers", "Users");
            }
        }


        public IActionResult AdminPin()
        {
            try
            {
               

                SResponse resAdmin = RequestSender.Instance.CallAPI("api",
                    "UserManagement/GetAdminUsers", "GET");
                if (resAdmin.Status && (resAdmin.Resp != null) && (resAdmin.Resp != ""))
                {
                    ResponseBack<List<AspNetUser>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<AspNetUser>>>(resAdmin.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<AspNetUser> responseObject = response.Data.ToList().ToList();
                        ViewBag.Admins = new SelectList(responseObject, "Id", "UserName");
                    }
                    else
                    {
                        TempData["response"] = "Request to get customerd not completed.";
                        return RedirectToAction("AdminPin", "Users");
                    }
                }

                SResponse ressrole = RequestSender.Instance.CallAPI("api",
                   "UserManagement/AdminPinGet", "GET");
                if (ressrole.Status && (ressrole.Resp != null) && (ressrole.Resp != ""))
                {
                    ResponseBack<List<SecurityKey>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<SecurityKey>>>(ressrole.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<SecurityKey> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }

                
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult ChangeAdminStatus(int keyid, int status)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "UserManagement/AdminPinChangeStatus" + "?keyid=" + keyid + "&status=" + status, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<SecurityKey>>(ress.Resp);
                    if (response.Message=="Success.")
                    {
                        return Json("Updated Sucessfully");
                    }
                    
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("AdminPin", "Users");
            }
        }

        [HttpPost]
        public IActionResult AddNewAdminPinUser(SecurityKey data)
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
                var body = JsonConvert.SerializeObject(data);
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "UserManagement/AddAdminUser", "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<SecurityKey>>(ress.Resp);
                    if (response.Message == "Success.")
                    {

                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessToken.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Add";
                        activity.NewDetails = "Supervision Pin Added on " + DateTime.Now.Date + " " + "by " + AccessToken.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);

                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                        TempData["response"] = "User is added sucessfully.";
                        return Json(true);
                    }
                    else if (response.Message == "Already Exists.")
                    {
                        TempData["response"] = "User already Exists.";
                        return Json(false);
                    }
                    return Json(true);
                }
                else
                {
                    TempData["response"] = "Request not completed.";
                    return RedirectToAction("AdminPin", "Users");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("AdminPin", "Users");
            }
        }

        public IActionResult EmployeeContract(int id)
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
               "UserManagement/EmployeeGet", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        ViewBag.Employee = new SelectList(responseObject, "EmployeeId", "FullName");
                    }
                    else
                    {
                        List<Employee> listLeaveTypes = new List<Employee>();
                        ViewBag.Employee = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                    }
                }
                else
                {
                    List<Employee> listLeaveTypes = new List<Employee>();
                    ViewBag.Employee = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                }

                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "UserManagement/EmployeeContractEmployeeID" + "/" + id, "GET");

                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmployeeContract>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Contract Exists";
                        return RedirectToAction("DetailEmployee", "Users", new { id = id });
                    }
                }
                return RedirectToAction("ManageEmployeeContract");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Get." + ex.Message;
                return RedirectToAction("DetailEmployee", "Users", new { id = id });
            }
        }

    }
}
