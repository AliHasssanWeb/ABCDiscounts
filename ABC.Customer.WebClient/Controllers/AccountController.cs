using ABC.Customer.Domain.Configuration;
using ABC.Customer.Domain.DataConfig;
using ABC.Customer.WebClient.Models;
using ABC.EFCore.Entities.Admin;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABCDiscountsWebsite.Areas.Security.Controllers
{
    public class AccountController : Controller
    {
        private readonly ISession session;
        EncryptDecrypt encdec = new EncryptDecrypt();
        public AccountController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;

        }
        [HttpPost]
        public JsonResult fetchSignature(string signature)
        {
            HttpContext.Session.SetString("signature", signature);
            return Json("");
        }
        public IActionResult Register()
        {
            try
            {
                List<SelectListItem> CustomerType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="WholeSaler",Value="WholeSaler"},
                    new SelectListItem(){Text="Retailer",Value="Retailer"}
                };
                ViewData["CustomerType"] = CustomerType;

                List<SelectListItem> CustomerState = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="North Carolina",Value="North Carolina"}
                };
                ViewData["CustomerState"] = CustomerState;

                List<SelectListItem> RegistrationType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Customer",Value="Customer"}
                };
                ViewData["RegistrationType"] = RegistrationType;

                StatesofAmerica ob = new StatesofAmerica();
                var allstates = ob.Get();
                ViewBag.States = new SelectList(allstates, "StateName", "StateName");

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Login(Login users)
        //{



        //    try
        //    {
        //        AspNetUser objuser = new AspNetUser();
        //        objuser.Email = users.Email;
        //        objuser.PasswordHash = users.PasswordHash;
        //        var body = JsonConvert.SerializeObject(objuser);
        //        SResponse resp = RequestSender.Instance.CallAPI("api", "Security/Login", "POST", body);
        //        if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
        //        {
        //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            var userName = User.FindFirstValue(ClaimTypes.Name);
        //            return RedirectToAction("Index", "CustomerHomePage", new { @area = "Customer" });
        //        }

        //        else
        //        {
        //            objuser.UserPic = null;
        //            objuser.CreatedDate = null;
        //            objuser.ExpiryDate = null;
        //            objuser.LastChangePwdDate = null;

        //            objuser.ModifiedDate = null;
        //            objuser.LastLogin = null;
        //          //  objuser.CreatedDate = DateTime.Now;
        //            objuser.Id = 0;
        //            var bodyy = JsonConvert.SerializeObject(users);
        //            SResponse ress = RequestSender.Instance.CallAPI("api", "Security/AuthenticateLogin", "POST", bodyy);
        //            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
        //            {
        //                if (ress.Resp == "Invalid Email or Password")
        //                {
        //                    TempData["msg"] = "Invalid Email or Password.";
        //                    return View();
        //                }

        //                else
        //                {
        //                    //var ressuser = JsonConvert.DeserializeObject<AspNetUser>(ress.Resp);
        //                    var ressuser = JsonConvert.DeserializeObject<ResponseBack<AspNetUser>>(ress.Resp);

        //                    SResponse ressRole = RequestSender.Instance.CallAPI("api", "UserManagement/RoleGetByID/" + ressuser.Data.RoleId, "GET");
        //                    if (ressRole.Status && (ressRole.Resp != null) && (ressRole.Resp != ""))
        //                    {
        //                        var responseRole = JsonConvert.DeserializeObject<ResponseBack<AspNetRole>>(ressRole.Resp);
        //                        if (responseRole.Data.Name == "Customer" || responseRole.Data.Name == "customer" || responseRole.Data.Name == "CUSTOMER")
        //                        {
        //                            return RedirectToAction("Index", "CustomerHomePage", new { @area = "Customer" });
        //                        }
        //                        else if (responseRole.Data.Name == "ADMIN" || responseRole.Data.Name == "admin" || responseRole.Data.Name == "Admin")
        //                        {
        //                            return RedirectToAction("Index", "Home", new { @area = "Home" });
        //                        }
        //                        else
        //                        {
        //                            TempData["Msg"] = "Invalid Email and Password ";
        //                            return RedirectToAction("Login");
        //                        }

        //                    }
        //                    else
        //                    {
        //                        TempData["Msg"] = "Invalid Email and Password ";
        //                        return RedirectToAction("Login");
        //                    }
        //                }
        //            }

        //        }

        //        return View();

        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
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
            var checkpass = encdec.Decrypt("RDWc6W7v6/gFzmwEwWVr3Jqb6+EZ2GDT");
            SResponse ress = RequestSender.Instance.CallAPI("api", "Security/Login", "POST", body);
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
                        if (ressuser.Data.RoleName != "Customer")
                        {
                            TempData["response"] = "Invalid Customer Account.";
                            return View();
                        }
                        if (ressuser.Data.IsCancelled == true)
                        {
                            TempData["response"] = "Your Account Approval Is Rejected.";
                            return View();
                        }
                        if (ressuser.Data.AdminApproval == false && ressuser.Data.IsCancelled == false)
                        {
                            TempData["response"] = "Your Account Approval Is In Process.";
                            return View();
                        }
                        else if (ressuser.Data.IsActive == false)
                        {
                            TempData["response"] = "Your Account Is Blocked.";
                            return View();
                        }
                        return RedirectToAction("Index", "CustomerHomePage");
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

        [HttpGet]
        public IActionResult Forget()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Forget(ForgetPassword forget)
        {
            try
            {
                var email = forget.Email;
                var OTP = forget.Otp;
                var body = JsonConvert.SerializeObject(forget);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Security/GenerateNewPassword", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "";
                    return RedirectToAction("Forget", "Account", new { Area = "Security" });
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Reset";
                    return RedirectToAction("Forget", "Account", new { Area = "Security" });
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + " " + "Error Occured";
                return RedirectToAction("Forget", "Account", new { Area = "Security" });
            }

        }

        public IActionResult PasswordChange()
        {
            return View();
        }
        [HttpPost]
        public IActionResult PasswordChange(ForgetPassword forget)
        {
            try
            {

                var email = forget.Email;
                var OTP = forget.Otp;
                var body = JsonConvert.SerializeObject(forget);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Security/ForgetPasswordOTP", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "";
                    return RedirectToAction("#");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("#");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public JsonResult SendOtp(string email)
        {
            try
            {
                var msg = "";

                SResponse resp = RequestSender.Instance.CallAPI("api", "Security/ForgetPasswordOTP/" + email, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var responseRole = JsonConvert.DeserializeObject<ResponseBack<ForgetPassword>>(resp.Resp);
                    msg = "OTP Sent Successfully";
                    return Json(msg);

                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Invail Email";
                    return Json(JsonConvert.DeserializeObject("false."));
                }

            }
            catch (Exception ex)
            {

                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }

        [HttpPost]
        public IActionResult Register(CustomerInformation customers, IFormFile profileImage, IFormFile Ftextform, IFormFile Saletaxid, IFormFile Idcopy, IFormFile SignaturPadHidden)
        {
            try
            {
                customers.RegistrationType = "Customer";
                customers.FromScreen = "E-Commerce";
                if (customers.CertificateBusinessTypes != null)
                {
                    List<CertificateBusinessType> businesslist = null;
                    businesslist = new List<CertificateBusinessType>();


                    if (customers.EmpBusinesss.IsSelectedAccommodationAndFoodServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Accommodation And Food Services";
                        businesslist.Add(business);
                    }

                    if (customers.EmpBusinesss.IsSelectedAgriculturalforestryFishingAndHunting == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Agricultural forestry Fishing And Hunting";
                        businesslist.Add(business);
                    }

                    if (customers.EmpBusinesss.IsSelectedConstruction == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Construction";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedFinanceAndInsurance == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Finance And Insurance";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedInformationPublishingAndCommunications == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Information Publishing And Communications";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedManufacturing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Manufacturing";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedMining == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Mining";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedRealEstate == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "RealEstate";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedRentalAndLeasing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Rental And Leasing";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedRetailTrade == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Retail Trade";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedTransportationAndWarehousing == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Transportation And Warehousing";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedUtilities == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Utilities";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedWholesaleTrade == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Whole sale Trade";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedBusinessServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Business Services";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedProfessionalServices == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Professional Services";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedEducationAndHealth == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Education And Health";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedNonprofitOrganization == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Non Profit Organization";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedGovernment == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Government";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedNotABusiness == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        business.Type = "Not A Business";
                        businesslist.Add(business);
                    }
                    if (customers.EmpBusinesss.IsSelectedothers == true)
                    {
                        CertificateBusinessType business = null;
                        business = new CertificateBusinessType();
                        //business.Type = customers.EmpBusinesss.others ;
                        business.Type = "Others";
                        businesslist.Add(business);
                    }

                    customers.CertificateBusinessTypes = businesslist;
                }
                if (customers.EmpIdentification != null)
                {
                    List<CertificateIdentification> Identificationlist = null;
                    Identificationlist = new List<CertificateIdentification>();
                    if (customers.EmpIdentification.IdentificationNumberAR != null && customers.EmpIdentification.IdentificationNumberAR != "" && customers.EmpIdentification.ReasonExamptionAR != null && customers.EmpIdentification.ReasonExamptionAR != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "AR";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionAR;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberAR;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberIA != null && customers.EmpIdentification.IdentificationNumberIA != "" && customers.EmpIdentification.ReasonExamptionIA != null && customers.EmpIdentification.ReasonExamptionIA != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "IA";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionIA;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberIA;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberIN != null && customers.EmpIdentification.IdentificationNumberIN != "" && customers.EmpIdentification.ReasonExamptionIN != null && customers.EmpIdentification.ReasonExamptionIN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "IN";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionIN;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberIN;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberKS != null && customers.EmpIdentification.IdentificationNumberKS != "" && customers.EmpIdentification.ReasonExamptionKS != null && customers.EmpIdentification.ReasonExamptionKS != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "KS";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionKS;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberKS;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.CertificateIdentifications != null && customers.EmpIdentification.IdentificationNumberKY != "" && customers.EmpIdentification.ReasonExamptionKY != null && customers.EmpIdentification.ReasonExamptionKY != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "KY";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionKY;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberKY;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberMI != null && customers.EmpIdentification.IdentificationNumberMI != "" && customers.EmpIdentification.ReasonExamptionMI != null && customers.EmpIdentification.ReasonExamptionMI != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "MI";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionKY;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberKY;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberMN != null && customers.EmpIdentification.IdentificationNumberMN != "" && customers.EmpIdentification.ReasonExamptionMN != null && customers.EmpIdentification.ReasonExamptionMN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "MN";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionMN;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberMN;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberNC != null && customers.EmpIdentification.IdentificationNumberNC != "" && customers.EmpIdentification.ReasonExamptionNC != null && customers.EmpIdentification.ReasonExamptionNC != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NC";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionNC;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberNC;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberND != null && customers.EmpIdentification.IdentificationNumberND != "" && customers.EmpIdentification.ReasonExamptionND != null && customers.EmpIdentification.ReasonExamptionND != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "ND";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionND;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberND;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberNE != null && customers.EmpIdentification.IdentificationNumberNE != "" && customers.EmpIdentification.ReasonExamptionNE != null && customers.EmpIdentification.ReasonExamptionNE != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NE";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionNE;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberNE;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberNJ != null && customers.EmpIdentification.IdentificationNumberNJ != "" && customers.EmpIdentification.ReasonExamptionNJ != null && customers.EmpIdentification.ReasonExamptionNJ != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NJ";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionNJ;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberNJ;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberNV != null && customers.EmpIdentification.IdentificationNumberNV != "" && customers.EmpIdentification.ReasonExamptionNV != null && customers.EmpIdentification.ReasonExamptionNV != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "NV";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionNV;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberNV;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberOH != null && customers.EmpIdentification.IdentificationNumberOH != "" && customers.EmpIdentification.ReasonExamptionOH != null && customers.EmpIdentification.ReasonExamptionOH != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "OH";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionOH;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberOH;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberRI != null && customers.EmpIdentification.IdentificationNumberRI != "" && customers.EmpIdentification.ReasonExamptionRI != null && customers.EmpIdentification.ReasonExamptionRI != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "RI";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionRI;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberRI;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberOK != null && customers.EmpIdentification.IdentificationNumberOK != "" && customers.EmpIdentification.ReasonExamptionOK != null && customers.EmpIdentification.ReasonExamptionOK != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "OK";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionOK;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberOK;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberSD != null && customers.EmpIdentification.IdentificationNumberSD != "" && customers.EmpIdentification.ReasonExamptionSD != null && customers.EmpIdentification.ReasonExamptionSD != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "SD";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionSD;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberSD;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberTN != null && customers.EmpIdentification.IdentificationNumberTN != "" && customers.EmpIdentification.ReasonExamptionTN != null && customers.EmpIdentification.ReasonExamptionTN != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "TN";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionTN;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberTN;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberUT != null && customers.EmpIdentification.IdentificationNumberUT != "" && customers.EmpIdentification.ReasonExamptionUT != null && customers.EmpIdentification.ReasonExamptionUT != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "UT";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionUT;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberUT;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberVT != null && customers.EmpIdentification.IdentificationNumberVT != "" && customers.EmpIdentification.ReasonExamptionVT != null && customers.EmpIdentification.ReasonExamptionVT != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "VT";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionVT;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberVT;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberWV != null && customers.EmpIdentification.IdentificationNumberWV != "" && customers.EmpIdentification.ReasonExamptionWV != null && customers.EmpIdentification.ReasonExamptionWV != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "WV";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionWV;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberWV;
                        Identificationlist.Add(Identification);
                    }
                    if (customers.EmpIdentification.IdentificationNumberWY != null && customers.EmpIdentification.IdentificationNumberWY != "" && customers.EmpIdentification.ReasonExamptionWY != null && customers.EmpIdentification.ReasonExamptionWY != "")
                    {
                        CertificateIdentification Identification = null;
                        Identification = new CertificateIdentification();
                        Identification.State = "WY";
                        Identification.ReasonExamption = customers.EmpIdentification.ReasonExamptionWY;
                        Identification.IdentificationNumber = customers.EmpIdentification.IdentificationNumberWY;
                        Identificationlist.Add(Identification);
                    }


                    customers.CertificateIdentifications = Identificationlist;
                }
                if (customers.EmpReasonExemption != null)
                {
                    List<CertificateReasonExemption> ExemptionList = null;
                    ExemptionList = new List<CertificateReasonExemption>();

                    if (customers.EmpReasonExemption.IsSelectedFederalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.FederalGovernmentText;
                        Exemption.Reason = customers.EmpReasonExemption.FederalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedStateOrLocalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.StateOrLocalGovernmentText;
                        Exemption.Reason = customers.EmpReasonExemption.StateOrLocalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedTribalGovernment == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.TribalGovernmentText;
                        Exemption.Reason = customers.EmpReasonExemption.TribalGovernment;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedForeignDiplomat == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.ForeignDiplomatText;
                        Exemption.Reason = customers.EmpReasonExemption.ForeignDiplomat;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedAgriculturalProduction == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.AgriculturalProductionText;
                        Exemption.Reason = customers.EmpReasonExemption.AgriculturalProduction;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedIndustrialProductionManufacturing == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.IndustrialProductionManufacturingText;
                        Exemption.Reason = customers.EmpReasonExemption.IndustrialProductionManufacturing;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedDirectPayPermit == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.DirectPayPermitText;
                        Exemption.Reason = customers.EmpReasonExemption.DirectPayPermit;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedDirectMail == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.DirectMailText;
                        Exemption.Reason = customers.EmpReasonExemption.DirectMail;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedResale == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.ResaleText;
                        Exemption.Reason = customers.EmpReasonExemption.Resale;
                        ExemptionList.Add(Exemption);
                    }
                    if (customers.EmpReasonExemption.IsSelectedothers == true)
                    {
                        CertificateReasonExemption Exemption = null;
                        Exemption = new CertificateReasonExemption();
                        Exemption.Text = customers.EmpReasonExemption.othersText;
                        Exemption.Reason = customers.EmpReasonExemption.others;
                        ExemptionList.Add(Exemption);
                    }

                    customers.CertificateReasonExemptions = ExemptionList;
                }
                customers.paperWork = new ABC.EFCore.Repository.Edmx.CustomerPaperWork();
                if (Ftextform != null)
                {
                    var input = Ftextform.OpenReadStream();
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
                    customers.paperWork.FederalForm = byteData;
                }
                if (Saletaxid != null)
                {
                    var input = Saletaxid.OpenReadStream();
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
                    customers.paperWork.SalesTaxId = byteData;
                }
                if (Idcopy != null)
                {
                    var input = Idcopy.OpenReadStream();
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
                    customers.paperWork.DrivingLicenseId = byteData;
                }
                if (profileImage != null)
                {
                    var input = profileImage.OpenReadStream();
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

                }


                string signature = HttpContext.Session.GetString("signature");
                if (signature != null)
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(signature);
                    customers.CertificateExemptionInstructions.Signature = bytes;
                }
                var body = JsonConvert.SerializeObject(customers);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Security/CustomerInformationCreate", "POST", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var ressuser = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(resp.Resp);
                    if (ressuser.Message == "Email Already Exist.")
                    {
                        TempData["response"] = "Email Already Exist.";
                        List<SelectListItem> CustomerType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="WholeSaler",Value="WholeSaler"},
                    new SelectListItem(){Text="Retailer",Value="Retailer"}
                };
                        ViewData["CustomerType"] = CustomerType;

                        List<SelectListItem> CustomerState = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="North Carolina",Value="North Carolina"}
                };
                        ViewData["CustomerState"] = CustomerState;

                        List<SelectListItem> RegistrationType = new List<SelectListItem>()
                {
                    new SelectListItem(){Text="Customer",Value="Customer"}
                };
                        ViewData["RegistrationType"] = RegistrationType;

                        StatesofAmerica ob = new StatesofAmerica();
                        var allstates = ob.Get();
                        ViewBag.States = new SelectList(allstates, "StateName", "StateName");
                        return View(customers);
                    }
                    else if (ressuser.Message == "Success But Email Not Send.")
                    {
                        TempData["response"] = "Thank you for registration, your request has send to administration for approval, TCP Blocked Email";
                        return RedirectToAction("Register");
                    }
                    TempData["response"] = "Thank you for registration, your request has send to administration for approval";
                    return RedirectToAction("Register");
                }
                else
                {
                    //  TempData["response"] = "Request not completed right now, Try Again";
                    TempData["response"] = "Thank you for registration, your request has send to administration for approval";
                    return RedirectToAction("Register");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed right now due to " + ex.Message;
                return RedirectToAction("Register");
            }
            // return RedirectToAction("Login", "Account", new { @area = "Security" });
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPassword forgetPassword)
        {
            try
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Security/ResetPasswordEmail" + "/" + forgetPassword.Email, "GET");
                if (resp.Status)
                {
                    ViewBag.Message = "Password reset link has been sent to your email Address, Please click on the button to reset your password.";
                    return View();
                }
                else
                {
                    TempData["response"] = "Email Not Found!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message;
                return View();
            }
        }


        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ForgetPassword forgetPassword)
        {
            var body = JsonConvert.SerializeObject(forgetPassword);
            SResponse resp = RequestSender.Instance.CallAPI("api", "Security/ResetPassword", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                TempData["response"] = "Password has been changed successfully";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["response"] = "Something went wrong, please try again";
                return RedirectToAction("Login");
            }
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ForgetPassword forgetPassword)
        {
            var userData = HttpContext.Session.GetString("userobj").ToString();
            if (!string.IsNullOrEmpty(userData))
            {
                AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                forgetPassword.Email = userDto.Email;
            }
            //forgetPassword.Email = userName;
            var body = JsonConvert.SerializeObject(forgetPassword);
            SResponse resp = RequestSender.Instance.CallAPI("api", "Security/changepassword", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                TempData["response"] = "Password has been changed successfully";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["response"] = "Your Current Password is not correct";
            }
            return View();
        }

    }
}
