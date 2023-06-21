using ABC.EFCore.Repository.Edmx;
using ABC.HR.Domain.DataConfig;
using ABC.HR.Domain.Entities;
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
using static ABC.HR.Domain.DataConfig.RequestSender;

namespace ABC.HR.Website.Controllers
{
    public class EmployeesController : Controller
    {
        private static IHttpContextAccessor httpContextAccessor;
        public EmployeesController(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        //private static IHttpContextAccessor httpContextAccessor;
        //public static void SetHttpContextAccessor(IHttpContextAccessor accessor)
        //{
        //    httpContextAccessor = accessor;
        //}
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult AddEmployee(Employee obj, IFormFile file)
        {

            try
            {
                var img = "";
                //Profile Image
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
                    obj.ProfileImage = byteData;
                }

                var number = "";
                if (obj.EmployeeCode != null)
                {
                    number = obj.EmployeeCode;
                }
                obj.ProfileImagePath = img;
                var body = JsonConvert.SerializeObject(obj);
                //var body = "{\"EmployeeId\":0,\"FullName\":\"testing\",\"Email\":\"usmsjkdk1950@outlook.com\",\"Mobile\":\"03134331845\",\"Phone\":null,\"Address\":\"Hally Tower DHA Lahore\",\"DrivingLisence\":\"343\",\"State\":null,\"EmployeeCode\":null,\"AdminApproval\":null,\"AccessAccount\":null,\"MartialStatus\":null,\"SpouseName\":null,\"NoofChildren\":null,\"ProfileImage\":null,\"ProfileImagePath\":\"\",\"AdminStatus\":null,\"IsActive\":null,\"EmployeeCity\":\"343\",\"EmployeeZipCode\":\"54500\",\"FederalEmployeeId\":\"32434\",\"PayrolAddress\":\"Dha,Lahore\",\"Extention\":\"33232\",\"Fax\":null,\"Ssn\":\"343\",\"YesContractor\":false,\"EmployerZipCode\":\"22222\",\"DateofHire\":\"2021-11-05T00:00:00\",\"Dob\":\"2021-11-05T00:00:00\",\"EmployerName\":\"4343\",\"EmployerState\":\"4234\",\"EmployerEmail\":null,\"City\":\"Lahore\",\"EmployeeWithHoldingTax\":{\"EAC_ID\":0,\"FirstName\":\"Huxley\",\"MiddleName\":\"Benjamin\",\"LastName\":\"Lucas\",\"SSN\":\"1212\",\"Address\":\"214 W Franklin Street. Chapel Hill. NC 27514. Arboretum. 8016 Providence Road\",\"MarriedStatus\":null,\"City\":\"Cary\",\"State\":\"North Carolina\",\"ZipCode\":\"25231\",\"NameDiffSSN\":null,\"Date\":\"2021-11-05T18:36:00\",\"EmployeeSignature\":\"red\",\"Country\":\"Pakistan\",\"NoofAllowances\":null,\"AdditionalAmount\":\"221\",\"NoTaxLaibility\":false,\"MilatrySpouseExempt\":false,\"Domiciled\":false,\"NoExemptions\":false,\"EmployerWithHold\":false,\"EmployeeID\":0,\"EmployeeOfficeCode\":\"3434\",\"Ein\":\"54115818\",\"FullName\":\"Huxley Benjamin Lucas\"},\"employeeAuthorizedRepresentative\":{\"EmpAuthRepId\":0,\"FirstName\":null,\"MiddleName\":null,\"LastName\":null,\"DocumentTitle\":null,\"IssuingAuthority\":null,\"DocumentNumber\":null,\"Signature\":null,\"EmpTitle\":null,\"OrgName\":null,\"OrgAddress\":null,\"City\":null,\"State\":null,\"ZipCode\":null,\"ExpirationDate\":null,\"Date\":null,\"EmployeeId\":null,\"FirstDate\":null,\"EmpAuthDocumentTitle\":null,\"EmpAuthIssuingAuthority\":null,\"EmpAuthDocumentNumber\":null,\"EmpAuthExpirationDate\":null},\"employeeReverificationAndRehire\":{\"EmpReverificationId\":0,\"NewName\":null,\"FirstName\":null,\"MiddleName\":null,\"LastName\":null,\"DocumentTitle\":null,\"DocumentNumber\":null,\"Signature\":null,\"EmpName\":null,\"DateofRehire\":null,\"Date\":null,\"ExpirationDate\":null,\"EmployeeId\":null},\"EmploymentEligibilityVerification\":{\"EEVID\":0,\"EmployeeId\":0,\"LastName\":\"Lucas\",\"FirstName\":null,\"MiddleName\":null,\"OtherNames\":null,\"Address\":\"214 W Franklin Street. Chapel Hill. NC 27514. Arboretum. 8016 Providence Road\",\"AptNumber\":null,\"City\":\"Cary\",\"State\":\"North Carolina\",\"ZipCode\":\"25231\",\"DOB\":\"2021-11-05T18:37:00\",\"SSN\":null,\"Email\":\"usama@gmail.com\",\"Phone\":null,\"Citizen\":false,\"NoCitizen\":false,\"Lawful\":false,\"LawfulNumber\":\"4343\",\"AllienAuthorized\":false,\"AllienAuthorizedNumber\":null,\"AllienRegNumber\":\"345435\",\"AdmissionNumber\":\"45435\",\"ForiegnPAssportNumber\":\"4543\",\"CountryIssuance\":\"4534\",\"EmployeeSignature\":\"red\",\"Date\":\"2021-11-05T18:38:00\",\"SignatureOfTransferer\":\"us\",\"PrepareDate\":\"2021-11-05T18:38:00\",\"PrepareLastName\":\"dfd\",\"PrepareFirstName\":\"fdfd\",\"PrepareAddress\":\"dfd\",\"PrepareCity\":\"Lahore\",\"PrepareState\":\"Dha,Lahore\",\"PrepareZipCode\":\"22222\",\"Expirationdate\":null},\"EmployeeDDAuthorization\":{\"EmpDdaid\":0,\"Authorize\":true,\"Revise\":true,\"Cancel\":true,\"Signature\":\"usd\",\"BankNameOne\":\"faysal\",\"BankNameTwo\":\"ubl\",\"BankNameThree\":null,\"AddressOne\":\"fddhghjhgbf\",\"AddressTwo\":\"cbcbvc\",\"AddressThree\":null,\"PhoneOne\":\"16503858068\",\"PhoneTwo\":\"16503858068\",\"PhoneThree\":null,\"AccTypeOne\":\"current\",\"AccTypeTwo\":\"currnet\",\"AccTypeThree\":null,\"BankRoutingOne\":\"efrythh\",\"BankRoutingTwo\":\"re\",\"BankRoutingThree\":null,\"BankAccNumberOne\":\"ertegrth\",\"BankAccNumberTwo\":\"reg\",\"BankAccNumberThree\":null,\"BankAmountOne\":\"34334\",\"BankAmountTwo\":null,\"BankAmountThree\":null,\"PctOne\":\"43\",\"PctTwo\":null,\"PctThree\":null,\"Total\":null,\"BankName\":null,\"Bank\":null,\"PayTo\":null,\"Memo\":null,\"Date\":\"2021-11-05T18:37:00\",\"EmployeeId\":null,\"CheckDate\":null,\"DepositName\":\"ali\",\"EmployerName\":\"bgfggd54\",\"RemainingBalance\":true,\"UsePercentage\":true,\"CheckAmount\":null},\"EmployerPhone\":null,\"NoContractor\":false,\"EmployeeOfficeCode\":null,\"Ein\":null,\"Expirationdate\":null}";
                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<Employee> responseobj = JsonConvert.DeserializeObject<ResponseBack<Employee>>(resp.Resp);
                    if(responseobj.Status == 6)
                    {

                        TempData["response"] =  "Email Already Exist";
                        //return RedirectToAction("RegisterEmployee",new { employee = obj });
                        return Redirect(Url.Action("RegisterEmployee", "Employees", obj));
                    }
                    // HttpContext.Session.SetString("CurrentEmployeeID",responseobj.Data.EmployeeId.ToString());
                    // JsonConvert.SerializeObject(ressuser.Data)
                    var srt = JsonConvert.SerializeObject(responseobj.Data);
                    httpContextAccessor.HttpContext.Session.SetString("CurrentEmployeeID", srt);
                    if (resp.Resp == "Employee Added Successfully Could't Send Email Due to Slow Internet")
                    {
                        TempData["Msg"] = "Employee approval request has been send for approval, but Could't Send Email because your internet provider does allow SMTP service.";
                        return RedirectToAction("ManageEmployee");
                    }
                    TempData["Msg"] = "Employee Added Successfully, Email Send";
                    return RedirectToAction("ManageEmployee");

                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("RegisterEmployee");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("RegisterEmployee");
            }
        }
      
        [HttpGet]
        public IActionResult ManageEmployees()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeGet", "GET");
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


        [HttpGet]
        public IActionResult ManageEmployee()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
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
                        TempData["response"] = "Unable to get employees.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get employees." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }
        //EmployeesControllerWorking
        [HttpPost]
        public IActionResult UpdateEmployee(int id, Employee obj, IFormFile file)
        {

            try
            {
                obj.EmployeeId = id;
                var img = "";
                //Profile Image
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
                    obj.ProfileImage = byteData;
                }
                obj.ProfileImagePath = img;
                var body = JsonConvert.SerializeObject(obj);
                string url = id.ToString();
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeUpdate/" + url + "", "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("ManageEmployee");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Update";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployee");
            }

        }
        public IActionResult UpdateEmployee(int id)
        {
            try
            {
                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");
                string url = id.ToString();
                SResponse ress = RequestSender.Instance.CallAPI("api", "HR/EmployeeByID/" + url + "", "GET");
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
                        TempData["Msg"] = ress.Resp + " " + "Unable To Update";
                        return RedirectToAction("AddEmployee");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployee");
            }

        }

        public IActionResult DeleteEmployee(int id)
        {
            try
            {
                string url = id.ToString();
                SResponse ress = RequestSender.Instance.CallAPI("api", "HR/DeleteEmployee/" + url + "", "Delete");
                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployee");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployee");
                    }
                }
                return RedirectToAction("ManageEmployee");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to get employees." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }



        }

        ////EmployeeLeaveEntitle
        public IActionResult AddEmployeeLeaveEntitle()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "HR/EmployeeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        ViewBag.EmployeeNo = new SelectList(responseObject, "EmployeeId", "FullName");
                    }
                    else
                    {
                        List<Employee> listEmpNo = new List<Employee>();
                        ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                    }
                }
                else
                {
                    List<Employee> listEmpNo = new List<Employee>();
                    ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                }
                SResponse res = RequestSender.Instance.CallAPI("api",
                     "Configuration/LeaveTypesGet", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<LeaveTypes>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<LeaveTypes>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<LeaveTypes> responseObject = response.Data;
                        ViewBag.LeaveTypes = new SelectList(responseObject, "LeaveTypeID", "TypeName");
                    }
                    else
                    {
                        List<LeaveTypes> listLeaveTypes = new List<LeaveTypes>();
                        ViewBag.LeaveTypes = new SelectList(listLeaveTypes, "LeaveTypeID", "TypeName");
                    }
                }
                else
                {
                    List<LeaveTypes> listLeaveTypes = new List<LeaveTypes>();
                    ViewBag.LeaveTypes = new SelectList(listLeaveTypes, "LeaveTypeID", "TypeName");
                }
                EmployeeLeaveEntitle generate = null;
                generate = new EmployeeLeaveEntitle();


                var empdataobj = httpContextAccessor.HttpContext.Session.GetString("CurrentEmployeeID");
                if (empdataobj != null)
                {
                    var EmployeeIDFound = empdataobj;
                    if (EmployeeIDFound != null)
                    {
                        Employee userDetails = JsonConvert.DeserializeObject<Employee>(empdataobj);
                        generate.EmployeeId = Convert.ToInt32(userDetails.EmployeeId);


                        return View(generate);
                    }
                    return View();

                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployeeLeaveEntitle");
            }

        }
        [HttpPost]
        public IActionResult AddEmployeeLeaveEntitle(EmployeeLeaveEntitle employeeLeaveEntitle)
        {
            try
            {

                employeeLeaveEntitle.ApprovedLeave = employeeLeaveEntitle.AvailableLeave;
                SResponse leavetypesress = RequestSender.Instance.CallAPI("api",
              "Configuration/LeaveTypesByID/" + employeeLeaveEntitle.LeaveTypeId, "GET");
                if (leavetypesress.Status && (leavetypesress.Resp != null) && (leavetypesress.Resp != ""))
                {
                    ResponseBack<LeaveTypes> record = JsonConvert.DeserializeObject<ResponseBack<LeaveTypes>>(leavetypesress.Resp);
                    if (record.Data != null)
                    {
                        employeeLeaveEntitle.LeaveTypeName = record.Data.TypeName;
                    }
                }

                SResponse employeeress = RequestSender.Instance.CallAPI("api",
              "HR/EmployeeByID/" + employeeLeaveEntitle.EmployeeId, "GET");
                if (employeeress.Status && (employeeress.Resp != null) && (employeeress.Resp != ""))
                {
                    ResponseBack<Employee> record = JsonConvert.DeserializeObject<ResponseBack<Employee>>(employeeress.Resp);
                    if (record.Data != null)
                    {
                        employeeLeaveEntitle.EmployeeName = record.Data.FullName;
                        employeeLeaveEntitle.EmployeeNo = record.Data.EmployeeCode;
                    }
                }

                var body = JsonConvert.SerializeObject(employeeLeaveEntitle);


                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeLeaveEntitleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("AddEmployeeLeaveEntitle");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployeeLeaveEntitle");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("AddEmployeeLeaveEntitle");
            }
        }
        [HttpPost]
        public IActionResult UpdateEmployeeLeaveEntitle(int id, EmployeeLeaveEntitle obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeLeaveEntitleUpdate​" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("ManageEmployeeLeaveEntitle");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployeeLeaveEntitle");
                }
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployeeLeaveEntitle");
            }

        }
        public IActionResult UpdateEmployeeLeaveEntitle(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeLeaveEntitleByID​" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmployeeLeaveEntitle>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["Msg"] = "Unable to get record for update.";
                        return RedirectToAction("ManageEmployeeLeaveEntitle");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployeeLeaveEntitle");
            }

        }
        [HttpGet]
        public IActionResult ManageEmployeeLeaveEntitle()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeLeaveEntitleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeLeaveEntitle>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeLeaveEntitle>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmployeeLeaveEntitle> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Employee Leave Entitle List Found. Please Enter Employee Leave Entitle First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public IActionResult DeleteEmployeeLeaveEntitle(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR//DeleteEmployeeLeaveEntitle​" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLeaveEntitle");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLeaveEntitle");
                    }
                }
                return RedirectToAction("ManageEmployeeLeaveEntitle");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageEmployeeLeaveEntitle");
            }

        }


        public IActionResult AddEmployeeDocuments()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Configuration/EmployeeDocumentTypeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeDocumentType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeDocumentType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmployeeDocumentType> responseObject = response.Data;
                        ViewBag.EmpDocType = new SelectList(responseObject, "DocTypeId", "TypeName");
                    }
                    else
                    {
                        List<EmployeeDocumentType> listEmpDocType = new List<EmployeeDocumentType>();
                        ViewBag.EmpDocType = new SelectList(listEmpDocType, "DocTypeId", "TypeName");
                    }
                }
                else
                {
                    List<Employee> listEmpNo = new List<Employee>();
                    ViewBag.EmpDocType = new SelectList(listEmpNo, "DocTypeId", "TypeName");
                }

                SResponse res = RequestSender.Instance.CallAPI("api",
                     "HR/EmployeeGet", "GET");
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

                EmployeeDocument generate = null;
                generate = new EmployeeDocument();
                var empdataobj = httpContextAccessor.HttpContext.Session.GetString("CurrentEmployeeID");
                if (empdataobj != null)
                {
                    var EmployeeIDFound = empdataobj;
                    if (EmployeeIDFound != null)
                    {
                        Employee userDetails = JsonConvert.DeserializeObject<Employee>(empdataobj);
                        generate.EmployeeId = Convert.ToInt32(userDetails.EmployeeId);
                        //  generate.EmployeeName = userDetails.EmployeeCode;

                        return View(generate);
                    }
                    return View();

                }
                else
                {
                    return View();
                }


            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployeeDocuments");
            }

        }
        [HttpPost]
        public IActionResult AddEmployeeDocuments(EmployeeDocument employeeDocuments, IFormFile file)
        {
            try
            {

                SResponse employeedocumenttype = RequestSender.Instance.CallAPI("api",
              "Configuration/EmployeeDocumentTypeByID/" + employeeDocuments.DocTypeId, "GET");
                if (employeedocumenttype.Status && (employeedocumenttype.Resp != null) && (employeedocumenttype.Resp != ""))
                {
                    ResponseBack<EmployeeDocumentType> record = JsonConvert.DeserializeObject<ResponseBack<EmployeeDocumentType>>(employeedocumenttype.Resp);
                    if (record.Data != null)
                    {
                        employeeDocuments.DocumentTypeName = record.Data.TypeName;
                    }
                }

                SResponse employeeress = RequestSender.Instance.CallAPI("api",
              "HR/EmployeeByID/" + employeeDocuments.EmployeeId, "GET");
                if (employeeress.Status && (employeeress.Resp != null) && (employeeress.Resp != ""))
                {
                    ResponseBack<Employee> record = JsonConvert.DeserializeObject<ResponseBack<Employee>>(employeeress.Resp);
                    if (record != null)
                    {
                        employeeDocuments.EmployeeName = record.Data.FullName;
                        employeeDocuments.EmployeeNumber = record.Data.EmployeeCode;
                    }
                }
                var img = "";
                if (file != null && file.Length > 0)
                {
                    var uploadDir = @"images/Employee Documents";
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".PDF")
                    {
                        //var webRootPath = _hostingEnvironment.ContentRootPath;
                        string webRootPath = @"wwwroot";
                        fileName = DateTime.UtcNow.ToString("yymmssff") + fileName + extension;
                        var path = Path.Combine(webRootPath, uploadDir, fileName);
                        file.CopyTo(new FileStream(path, FileMode.Create));
                        employeeDocuments.DocumentByPath = "/" + uploadDir + "/" + fileName;
                        img = employeeDocuments.DocumentByPath;
                        employeeDocuments.Document = null;
                    }
                    else
                    {
                        TempData["Msg"] = "Please select valid file";
                        return View(employeeDocuments);
                    }

                }
                var body = JsonConvert.SerializeObject(employeeDocuments);


                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeDocumentCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("AddEmployeeDocuments");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployeeDocuments");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployeeDocuments");
            }
        }
        public IActionResult ManageEmployeeDocuments()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeDocumentGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeDocument>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeDocument>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmployeeDocument> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Employee Documents List Found. Please Enter Employee Documents First.";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public IActionResult DeleteEmployeeDocuments(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "HR/DeleteEmployeeDocument" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeDocuments");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeDocuments");
                    }
                }
                return RedirectToAction("ManageEmployeeDocuments");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageEmployeeDocuments");
            }


        }
        public IActionResult DownloadFile(string filepath)
        {
            try
            {
                if (filepath != null)
                {
                    string webRootPath = @"wwwroot";
                    string name = Path.GetFileName(filepath);
                    filepath = webRootPath + filepath;
                    byte[] bytes = System.IO.File.ReadAllBytes(filepath);

                    return File(bytes, "application/pdf", name);
                }
                TempData["Msg"] = "Unable to download";
                return RedirectToAction("ManageEmployeeDocuments");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to download" + ex.Message;
                return RedirectToAction("ManageEmployeeDocuments");
            }
        }


        public IActionResult DetailEmployee(int id)
        {
            Employee emp = new Employee();
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeByID" + "/" + id, "GET");
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
                        TempData["response"] = "Unable to get employee record.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult EmployeeById(int id)
        {
            Employee emp = new Employee();
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeDetailsByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get employee record.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult EmployeeContractByEmployeeCode(int id)
        {
            Employee emp = new Employee();
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeContractDetailsByCode" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmployeeContract>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get employee record.";
                        return Json(response.Data);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult EmployeeTaxbyId(int id)
        {
            Employee emp = new Employee();
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeTaxList" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get employee record.";
                        return Json(null);
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public ActionResult CreateEmployeeSettlement()
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeGet", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        ViewData["Employee"] = new SelectList(responseObject, "EmployeeId", "FullName");
                    }
                    else
                    {
                        List<Employee> listLeaveTypes = new List<Employee>();
                        ViewData["Employee"] = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                    }
                }
                else
                {
                    List<Employee> listLeaveTypes = new List<Employee>();
                    ViewData["Employee"] = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                }

                EmployeeSettlement generate = null;
                generate = new EmployeeSettlement();



                var getemployee = httpContextAccessor.HttpContext.Session.GetString("CurrentEmployeeID");
                if (getemployee != null)
                {
                        Employee userDetails = JsonConvert.DeserializeObject<Employee>(getemployee);
                        generate.EmployeeId = Convert.ToInt32(userDetails.EmployeeId);
                        generate.EmployeeCode = userDetails.EmployeeCode;
                        generate.EmployeeEmail = userDetails.Email;

                        return View(generate);
                }
                else
                {
                    return View();
                }
                // return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployeeSettlement");
            }
        }
        [HttpPost]
        public IActionResult CreateEmployeeSettlement(EmployeeSettlement employeeSettlement)
        {
            try
            {

                SResponse employeeress = RequestSender.Instance.CallAPI("api",
              "HR/EmployeeByID/" + employeeSettlement.EmployeeId, "GET");
                if (employeeress.Status && (employeeress.Resp != null) && (employeeress.Resp != ""))
                {
                    ResponseBack<Employee> record = JsonConvert.DeserializeObject<ResponseBack<Employee>>(employeeress.Resp);
                    if (record.Data != null)
                    {
                        employeeSettlement.EmployeeName = record.Data.FullName;
                    }
                }

                var body = JsonConvert.SerializeObject(employeeSettlement);


                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeSettlementCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("CreateEmployeeSettlement");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("CreateEmployeeSettlement");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("CreateEmployeeSettlement");
            }
        }
        [HttpPost]
        public IActionResult UpdateEmployeeSettlement(int id, EmployeeSettlement obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeSettlementUpdate​" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("ManageEmployeeSettlement");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("CreateEmployeeSettlement");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployeeSettlement");
            }

        }
        public IActionResult UpdateEmployeeSettlement(int id)
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeGet", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                               JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        ViewData["Employee"] = new SelectList(responseObject, "EmployeeId", "FullName");
                    }
                    else
                    {
                        List<Employee> listLeaveTypes = new List<Employee>();
                        ViewData["Employee"] = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                    }
                }
                else
                {
                    List<Employee> listLeaveTypes = new List<Employee>();
                    ViewData["Employee"] = new SelectList(listLeaveTypes, "EmployeeId", "FullName");
                }

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeSettlementByID​" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmployeeSettlement>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["Msg"] = "Unable to get record for update.";
                        return RedirectToAction("ManageEmployeeSettlement");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployeeSettlement");
            }

        }
        [HttpGet]
        public IActionResult ManageEmployeeSettlement()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeSettlementGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeSettlement>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeSettlement>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmployeeSettlement> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Employee Settlement List Found. Please Enter Employee Settlement First..";
                        return View();
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public IActionResult DeleteEmployeeSettlement(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/DeleteEmployeeSettlement​" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeSettlement");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeSettlement");
                    }
                }
                return RedirectToAction("ManageEmployeeSettlement");
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageEmployeeSettlement");
            }

        }

        public JsonResult GetEmpData()
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<Employee>> response =
                               JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Employee> responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get employees.";
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



        public IActionResult AddEmployeeContract()
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeActiveDropDown", "GET");
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

                EmployeeContract generate = null;
                generate = new EmployeeContract();
                var empdataobj = httpContextAccessor.HttpContext.Session.GetString("CurrentEmployeeID");
                if (empdataobj != null)
                {
                    var EmployeeIDFound = empdataobj;
                    if (EmployeeIDFound != null)
                    {
                        Employee userDetails = JsonConvert.DeserializeObject<Employee>(empdataobj);
                        generate.EmployeeId = Convert.ToInt32(userDetails.EmployeeId);
                        generate.EmployeeNumber = userDetails.EmployeeCode;
                        //  generate.EmployeeName = userDetails.EmployeeCode;

                        return View(generate);
                    }
                    return View();

                }
                else
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployeeContract");
            }

        }

        [HttpPost]
        public IActionResult AddEmployeeContract(EmployeeContract obj, IFormFile file)
        {
            try
            {
                obj.DailyWages = true;
                if (obj.DailyWages == false)
                {
                    obj.DailyWagesChargesAmount = "0";
                }
                if (obj.IsProbation == false)
                {
                    obj.ProbationSalary = "0";
                    obj.ProbationSdate = null;
                    obj.ProbationEdate = null;
                }
                 if(obj.ContractName == "Contract")
                {
                    obj.PaymentMethod = "Cash";
                    obj.Iban = null;
                    obj.AccountNo = null;
                    obj.RoutingNo = null;
                }
                var img = "";
                if (file != null && file.Length > 0)
                {
                    var uploadDir = @"images/Employee Contract";
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".PDF")
                    {
                        string webRootPath = @"wwwroot";
                        fileName = DateTime.UtcNow.ToString("yymmssff") + fileName + extension;
                        var path = Path.Combine(webRootPath, uploadDir, fileName);
                        file.CopyTo(new FileStream(path, FileMode.Create));
                        obj.ContractDocumentByPath = "/" + uploadDir + "/" + fileName;
                        img = obj.ContractDocumentByPath;
                        obj.ContractDocument = null;
                    }
                    else
                    {
                        TempData["Msg"] = "Please select valid file";
                        return View(obj);
                    }

                }
                var body = JsonConvert.SerializeObject(obj);


                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeContractCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<List<EmployeeContract>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeContract>>>(resp.Resp);
                    if(response.Message == "Already Exists.")
                    {
                        TempData["responseDanger"] = "Employee Already Exist";
                        return RedirectToAction("AddEmployeeContract");
                    }
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeContract");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployeeContract");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("AddEmployeeContract");
            }


        }

        [HttpGet]
        public IActionResult ManageEmployeeContract()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeContractGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeContract>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeContract>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmployeeContract> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Employee Contract List Found. Please Enter Employee Contract First..";
                        return View();

                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }
        public IActionResult UpdateEmployeeContract(int id)
        {
            try
            {
                SResponse res = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeGet", "GET");
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
                  "HR/EmployeeContractByID" + "/" + id, "GET");
             
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
                        TempData["Msg"] = "Unable To Update";
                        return RedirectToAction("ManageEmployeeContract");
                    }
                }
                return RedirectToAction("ManageEmployeeContract");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployeeContract");
            }

        }
        [HttpPost]
        public IActionResult UpdateEmployeeContract(int id, EmployeeContract obj, IFormFile file)
        {

            try
            {
                var img = "";
                if (file != null && file.Length > 0)
                {
                    var uploadDir = @"images/Employee Contract";
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var extension = Path.GetExtension(file.FileName);
                    if (extension == ".pdf" || extension == ".PDF")
                    {
                        string webRootPath = @"wwwroot";
                        fileName = DateTime.UtcNow.ToString("yymmssff") + fileName + extension;
                        var path = Path.Combine(webRootPath, uploadDir, fileName);
                        file.CopyTo(new FileStream(path, FileMode.Create));
                        obj.ContractDocumentByPath = "/" + uploadDir + "/" + fileName;
                        img = obj.ContractDocumentByPath;
                        obj.ContractDocument = null;
                    }
                    else
                    {
                        TempData["Msg"] = "Please select valid file";
                        return View(obj);
                    }

                }

                var body = JsonConvert.SerializeObject(obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "HR/EmployeeContractUpdate/" + id, "POST", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("ManageEmployeeContract");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddEmployee");
                }
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageEmployee");
            }

        }

        public IActionResult DeleteEmployeeContract(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "HR/DeleteEmployeeContract" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeContract");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeContract");
                    }
                }
                return RedirectToAction("ManageEmployeeContract");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageEmployeeContract");
            }

        }


        public IActionResult DownloadFileContract(string filename)
        {

            try
            {
                if (filename != null)
                {
                    string webRootPath = @"wwwroot";
                    string name = Path.GetFileName(filename);
                    filename = webRootPath + filename;
                    byte[] bytes = System.IO.File.ReadAllBytes(filename);

                    return File(bytes, "application/pdf", name);
                }
                TempData["Msg"] = "Unable to download";
                return RedirectToAction("ManageEmployeeContract");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Unable to download" + ex.Message;
                return RedirectToAction("ManageEmployeeContract");
            }

        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }
        public JsonResult GetEmpDataByID(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable to get details of selected employee.";
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


        //[HttpGet]
        //public ActionResult ShowEmployeeContract()
        //{


        //    string filePath = "~/file/test.pdf";
        //    Response.Headers.Add("Content-Disposition", "inline; filename=test.pdf");
        //    return File(filePath, "application/pdf");
        //}

        public JsonResult AddLeaveTypes(LeaveTypes obj)
        {
            try
            {
                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/LeaveTypesCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }

        public JsonResult AddEmployeeDocumentType(EmployeeDocumentType obj)
        {
            try
            {
                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/EmployeeDocumentTypeCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }


        public IActionResult EmploymentEligibilityVerification()
        {
            try
            {

                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                //          SResponse EmployeeGet = RequestSender.Instance.CallAPI("api",
                //"HR/EmployeeGet", "GET");
                //          if (EmployeeGet.Status && (EmployeeGet.Resp != null) && (EmployeeGet.Resp != ""))
                //          {
                //              List<Employee> response =
                //                           JsonConvert.DeserializeObject<List<Employee>>(EmployeeGet.Resp);
                //              if (response.Count() > 0)
                //              {
                //                  List<Employee> responseObject = response;
                //                  ViewBag.EmployeeGet = new SelectList(responseObject.ToList(), "EmployeeId", "FullName");
                //              }
                //              else
                //              {
                //                  List<Employee> responseObject = new List<Employee>();
                //                  ViewBag.EmployeeGet = new SelectList(responseObject, "EmployeeId", "FullName");
                //              }
                //          }
                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IActionResult RegisterEmployee(Employee employee)
        {
            try
            {

                Employee Model = new Employee();


                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                SResponse respEmp = RequestSender.Instance.CallAPI("api", "HR/EmployeeGet", "GET");
                if (respEmp.Status && (respEmp.Resp != null) && (respEmp.Resp != ""))
                {
                    ResponseBack<List<Employee>> record = JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(respEmp.Resp);
                    if (record.Data != null && record.Data.Count() > 0)
                    {
                        Employee newitems = new Employee();
                        var fullcode = "";
                        if (record.Data[0].EmployeeCode != null && record.Data[0].EmployeeCode != "string" && record.Data[0].EmployeeCode != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(record.Data[0].EmployeeCode.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].EmployeeCode.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count; i++)
                            {
                                if (record.Data[i].EmployeeCode != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].EmployeeCode.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].EmployeeCode.Split('-')[1]) > large)
                                    {
                                        salesID = Convert.ToInt32(record.Data[i].EmployeeId);
                                        large = Convert.ToInt32(record.Data[i].EmployeeCode.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(record.Data[i].EmployeeCode.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].EmployeeCode.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            salesID = Convert.ToInt32(record.Data[i].EmployeeId);
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.EmployeeId == salesID).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.EmployeeCode != null)
                                {
                                    var VcodeSplit = newitems.EmployeeCode.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "EMP00" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "EMP00" + "-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "EMP00" + "-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "EMP00" + "-" + "1";
                        }

                        ViewBag.EmployeeCode = fullcode;
                    }
                    else
                    {
                        ViewBag.EmployeeCode = "EMP00" + "-" + "1";
                    }
                    Model.EmployeeCode = ViewBag.EmployeeCode;
                }
                else
                {
                    ViewBag.EmployeeCode = "EMP00" + "-" + "1";

                }

                return View(employee);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageEmployee");
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
    }
}
