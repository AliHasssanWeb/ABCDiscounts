using ABC.Admin.API.ViewModel;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        EncryptDecrypt encdec = new EncryptDecrypt();
        private readonly IMailService mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private EmailService emailService = new EmailService();
        public CustomersController(ABCDiscountsContext _db, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            db = _db;
            this.mailService = mailService;
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet("CustomersGet")]
        public IActionResult CustomersGet()
        {
            try
            {
                var record = db.CustomerInformations.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomersGetByID/{id}")]
        public IActionResult CustomersGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var record = db.CustomerInformations.Find(id);

                if (record != null)
                {
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        record.CertificateExemptionInstructions = getCertificate;

                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            record.CertificateBusinessTypes = getBusiness;
                        }

                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            record.CertificateIdentifications = getIdentifications;
                        }

                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            record.CertificateReasonExemptions = getReasons;
                        }

                        var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        if (getuser != null)
                        {
                            record.AspNetUser = getuser;
                        }
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomerInformationByID/{id}")]
        public IActionResult CustomerInformationByID(int id)
        {
            try
            {

                var hceck = db.CertificateBusinessTypes.ToList();
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();

                var record = db.CustomerInformations.Find(id);
                if (record != null)
                {
                    var getpay = db.Receivables.ToList().Where(x => x.AccountId == record.AccountId).FirstOrDefault();
                    if (getpay != null)
                    {

                        record.Balance = getpay.Amount;
                    }
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        record.CertificateExemptionInstructions = getCertificate;
                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            record.CertificateBusinessTypes = getBusiness;
                        }
                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            record.CertificateIdentifications = getIdentifications;
                        }
                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            record.CertificateReasonExemptions = getReasons;
                        }
                        var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        if (getuser != null)
                        {
                            record.AspNetUser = getuser;
                        }
                    }
                    var classificationdata = db.CustomerClassifications.Where(x => x.CustomerInfoId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (classificationdata != null)
                    {
                        record.CustomerClassification = classificationdata;
                    }
                    else
                    {
                        record.CustomerClassification = null;
                    }
                    var billinginfodata = db.CustomerBillingInfos.Where(x => x.CustomerInformationId == record.Id && x.CustomerCode == record.CustomerCode).FirstOrDefault();
                    if (billinginfodata != null)
                    {
                        record.CustomerBillingInfo = billinginfodata;
                    }
                    else
                    {
                        record.CustomerBillingInfo = null;
                    }

                    var paperworkdata = db.CustomerPaperWorks.Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if(paperworkdata != null)
                    {
                        record.paperWork = new CustomerPaperWork();
                        record.paperWork = paperworkdata;
                    }
                    else
                    {
                        record.paperWork = new CustomerPaperWork();
                    }
                   
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CustomersGetByLoginUserID/{id}")]
        public IActionResult CustomersGetByLoginUserID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var Getallrecord = db.CustomerInformations.ToList();
                if (Getallrecord.Count() > 0)
                {
                    var getallASPUsers = db.AspNetUsers.ToList();
                    for (int i = 0; i < Getallrecord.Count(); i++)
                    {
                        foreach (var item in getallASPUsers.ToList().Where(x => x.Email == Getallrecord[i].Email).ToList())
                        {
                            Getallrecord[i].AspNetUser = item;
                        }
                    }
                }
                var record = Getallrecord.ToList().Where(x => x.AspNetUser.Id == Convert.ToInt32(id)).FirstOrDefault();
                //var Getallrecord = db.Customers.ToList().Where(x=>x.user)
                if (record != null)
                {
                    if(record.AspNetUser == null)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Customer_Profile_Not_Found, null, null);
                        return Ok(Response);
                    }
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == record.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        record.CertificateExemptionInstructions = getCertificate;

                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            record.CertificateBusinessTypes = getBusiness;
                        }

                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            record.CertificateIdentifications = getIdentifications;
                        }

                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            record.CertificateReasonExemptions = getReasons;
                        }

                        //var getuser = db.AspNetUsers.ToList().Where(x => x.Email == record.Email).FirstOrDefault();
                        //if (getuser != null)
                        //{
                        //    record.AspNetUser = getuser;
                        //}
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RegisterCustomerECommerce")]
        public async Task<IActionResult> RegisterCustomerECommerce(CustomerInformation model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                bool isvalid = db.Customers.ToList().Exists(x => x.Email.Equals(model.Email, StringComparison.CurrentCultureIgnoreCase));
                if (isvalid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                else
                {
                    //if (model.ProfileImage != null)
                    //{
                    //    //  return BadRequest("Image Path Required Instead of Byte");
                    //    var imgPath = SaveImage(model.ProfileImage, Guid.NewGuid().ToString());
                    //    model.ImageByPath = imgPath;
                    //    model.ProfileImage = null;
                    //}
                    db.CustomerInformations.Add(model);
                    db.SaveChanges();

                    var getcustomer = db.Customers.AsQueryable().ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                    if (getcustomer != null)
                    {
                        CertificateExemptionInstruction certificate = null;
                        certificate = new CertificateExemptionInstruction();
                        certificate.BusinessAddress = model.CertificateExemptionInstructions.BusinessAddress;
                        certificate.CertificateSinglePurchase = model.CertificateExemptionInstructions.CertificateSinglePurchase;
                        certificate.CountryIssue = model.CertificateExemptionInstructions.CountryIssue;
                        certificate.DrivingLicenseNo = model.CertificateExemptionInstructions.DrivingLicenseNo;
                        certificate.CreatedDate = DateTime.Now;
                        certificate.Fein = model.CertificateExemptionInstructions.Fein;
                        certificate.InvoicePurchaseOrderNo = model.CertificateExemptionInstructions.InvoicePurchaseOrderNo;
                        certificate.MultistateSupplementForm = model.CertificateExemptionInstructions.MultistateSupplementForm;
                        certificate.PostalAbbreviation = model.CertificateExemptionInstructions.PostalAbbreviation;
                        certificate.PurchaserCity = model.CertificateExemptionInstructions.PurchaserCity;
                        certificate.PurchaserName = model.CertificateExemptionInstructions.PurchaserName;
                        certificate.PurchaserState = model.CertificateExemptionInstructions.PurchaserState;
                        certificate.PurchaserZipCode = model.CertificateExemptionInstructions.PurchaserZipCode;
                        certificate.PurchaseTaxId = model.CertificateExemptionInstructions.PurchaseTaxId;
                        certificate.SellerAdress = model.CertificateExemptionInstructions.SellerAdress;
                        certificate.SellerCity = model.CertificateExemptionInstructions.SellerCity;
                        certificate.SellerName = model.CertificateExemptionInstructions.SellerName;
                        certificate.SellerState = model.CertificateExemptionInstructions.SellerState;
                        certificate.SellerZipCode = model.CertificateExemptionInstructions.SellerZipCode;
                        certificate.Signature = model.CertificateExemptionInstructions.Signature;
                        certificate.StateIssue = model.CertificateExemptionInstructions.StateIssue;
                        certificate.TermsCondition = model.CertificateExemptionInstructions.TermsCondition;
                        certificate.CustomerId = getcustomer.CustomerId;
                        certificate.FeinCountry = model.CertificateExemptionInstructions.FeinCountry;
                        db.CertificateExemptionInstructions.Add(certificate);
                        await db.SaveChangesAsync();

                        var getCertificate = db.CertificateExemptionInstructions.AsQueryable().ToList().Where(x => x.CustomerId == getcustomer.CustomerId).FirstOrDefault();
                        if (getCertificate != null)
                        {
                            if (model.FromScreen == "IOS" || model.FromScreen == "IoS" || model.FromScreen == "Ios")
                            {
                                CertificateBusinessType cbt = null;
                                for (int i = 0; i < model.CertificateBusinessTypes[0].Type.ToString().Split(',').Count(); i++)
                                {
                                    cbt = new CertificateBusinessType();
                                    cbt.CustomerId = getcustomer.CustomerId;
                                    cbt.CertificateId = getCertificate.Ceiid;
                                    cbt.Type = model.CertificateBusinessTypes[0].Type.ToString().Split(',')[i];
                                    db.CertificateBusinessTypes.Add(cbt);
                                    await db.SaveChangesAsync();
                                }

                                CertificateReasonExemption reason = null;
                                for (int x = 0; x < model.CertificateReasonExemptions[0].Reason.ToString().Split(',').Count(); x++)
                                {
                                    reason = new CertificateReasonExemption();
                                    reason.CustomerId = getcustomer.CustomerId;
                                    reason.CertificateId = getCertificate.Ceiid;
                                    reason.Reason = model.CertificateReasonExemptions[0].Reason.ToString().Split(',')[x];
                                    db.CertificateReasonExemptions.Add(reason);
                                    await db.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                CertificateBusinessType cbt = null;
                                for (int i = 0; i < model.CertificateBusinessTypes.Count(); i++)
                                {
                                    cbt = new CertificateBusinessType();
                                    cbt.CustomerId = getcustomer.CustomerId;
                                    cbt.CertificateId = getCertificate.Ceiid;
                                    cbt.Type = model.CertificateBusinessTypes[i].Type;
                                    db.CertificateBusinessTypes.Add(cbt);
                                    await db.SaveChangesAsync();
                                }

                                CertificateReasonExemption reason = null;
                                for (int x = 0; x < model.CertificateReasonExemptions.Count(); x++)
                                {
                                    reason = new CertificateReasonExemption();
                                    reason.CustomerId = getcustomer.CustomerId;
                                    reason.CertificateId = getCertificate.Ceiid;
                                    reason.Reason = model.CertificateReasonExemptions[x].Reason;
                                    db.CertificateReasonExemptions.Add(reason);
                                    await db.SaveChangesAsync();
                                }
                            }

                            CertificateIdentification identification = null;
                            for (int a = 0; a < model.CertificateIdentifications.Count(); a++)
                            {
                                identification = new CertificateIdentification();
                                identification.CertificateId = getCertificate.Ceiid;
                                identification.CustomerId = getcustomer.CustomerId;
                                identification.IdentificationNumber = model.CertificateIdentifications[a].IdentificationNumber;
                                identification.ReasonExamption = model.CertificateIdentifications[a].ReasonExamption;
                                db.CertificateIdentifications.Add(identification);
                                await db.SaveChangesAsync();
                            }


                        }


                        var getrole = db.AspNetRoles.ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                        if (getrole != null)
                        {
                            AspNetUser makeuser = new AspNetUser();
                            makeuser.Email = getcustomer.Email;
                            makeuser.PasswordHash = encdec.Encrypt(getcustomer.Email);
                            makeuser.Deleted = false;
                            makeuser.AdminApproval = false;
                            makeuser.Address = getcustomer.Address;
                            makeuser.City = getcustomer.City;
                           // makeuser.CreatedDate = DateTime.Now;
                            makeuser.DrivingLicense = getcustomer.DrivingLicense;
                            makeuser.EmployeeId = null;
                            makeuser.Firstname = getcustomer.FullName;
                            makeuser.FromScreen = "E-Commerce";
                            makeuser.IsActive = false;
                            makeuser.IsCancelled = false;
                            makeuser.Firstname = getcustomer.FullName;
                            makeuser.Lastname = "";
                            makeuser.Mobile = getcustomer.Mobile;
                            makeuser.PhoneNumber = getcustomer.Phone;
                            makeuser.RoleId = Convert.ToInt32(getrole.Id);
                            //makeuser.State = getcustomer.CutomerState;
                            makeuser.PhoneNumber = getcustomer.Phone;
                            makeuser.UserName = getcustomer.Email;
                            makeuser.ZipCode = getcustomer.PostalCode;

                            db.AspNetUsers.Add(makeuser);
                            await db.SaveChangesAsync();



                        }
                        else
                        {
                            AspNetRole makerole = new AspNetRole();
                            makerole.Name = "Customer";
                            db.AspNetRoles.Add(makerole);
                            await db.SaveChangesAsync();

                            var getnewrole = db.AspNetRoles.AsQueryable().ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                            if (getnewrole != null)
                            {
                                AspNetUser makeuser = new AspNetUser();
                                makeuser.Email = getcustomer.Email;
                                makeuser.PasswordHash = encdec.Encrypt(getcustomer.Email);
                                makeuser.Deleted = false;
                                makeuser.AdminApproval = false;
                                makeuser.Address = getcustomer.Address;
                                makeuser.City = getcustomer.City;
                               // makeuser.CreatedDate = DateTime.Now;
                                makeuser.DrivingLicense = getcustomer.DrivingLicense;
                                makeuser.EmployeeId = null;
                                makeuser.Firstname = getcustomer.FullName;
                                makeuser.FromScreen = "E-Commerce";
                                makeuser.IsActive = false;
                                makeuser.IsCancelled = false;
                                makeuser.Firstname = getcustomer.FullName;
                                makeuser.Lastname = "";
                                makeuser.Mobile = getcustomer.Mobile;
                                makeuser.PhoneNumber = getcustomer.Phone;
                                makeuser.RoleId = Convert.ToInt32(getnewrole.Id);
                                //  makeuser.State = getcustomer.CutomerState;
                                makeuser.PhoneNumber = getcustomer.Phone;
                                makeuser.UserName = getcustomer.Email;
                                makeuser.ZipCode = getcustomer.PostalCode;

                                db.AspNetUsers.Add(makeuser);
                                await db.SaveChangesAsync();

                            }
                        }
                    }


                    MailRequest request = new MailRequest();
                    request.ToEmail = getcustomer.Email;
                    request.Subject = "Welcome to ABCDiscounts";
                    var usermsg = "<div></h3>Dear Customer, Request for approval of your registration has received by admin. You will receive an confirmation email soon. For Login to system please use your email as username & Password.</h3><div>";
                    request.Body = usermsg;
                    var emailresponse = await Send(request);
                    if (emailresponse == "Email Sent Successfully")
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                        // return CreatedAtAction("CustomersGetByID", new { id = getcustomer.CustomerId }, "Customer Profile created thank you, please check your email");
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                        return Ok(Response);
                        //  return CreatedAtAction("CustomersGetByID", new { id = getcustomer.CustomerId }, "Customer Profile created thank you,For Login to system please use your email as username & Password.");
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Send")]
        public async Task<string> Send(MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return ("Email Sent Successfully");
            }
            catch (Exception ex)
            {

                return (ex.Message.ToString());
            }

        }


        public string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = _webHostEnvironment.WebRootPath;
            string webRootPath = _webHostEnvironment.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = "http://10.10.10.98:5595/abcdadminapi/images/" + imageName;
                return imgPath;
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                //imgPath = path + imageName;
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = "http://10.10.10.98:5595/abcdadminapi/images/" + imageName;
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
        }


        [HttpDelete("DeleteCustomer/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var data = db.Customers.Find(id);
                if (data == null)
                {
                    return NotFound();
                }
                var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CustomerId == data.CustomerId).ToList();
                for (int i = 0; i < getBusiness.Count(); i++)
                {
                    db.CertificateBusinessTypes.Remove(getBusiness[i]);
                    db.SaveChanges();
                }
                var getExemption = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == data.CustomerId).ToList();
                for (int i = 0; i < getExemption.Count(); i++)
                {
                    db.CertificateExemptionInstructions.Remove(getExemption[i]);
                    db.SaveChanges();
                }
                var getReason = db.CertificateReasonExemptions.ToList().Where(x => x.CustomerId == data.CustomerId).ToList();
                for (int i = 0; i < getReason.Count(); i++)
                {
                    db.CertificateReasonExemptions.Remove(getReason[i]);
                    db.SaveChanges();
                }
                var getPaper = db.CustomerPaperWorks.ToList().Where(x => x.CustomerId == data.CustomerId).ToList();
                for (int i = 0; i < getPaper.Count(); i++)
                {
                    db.CustomerPaperWorks.Remove(getPaper[i]);
                    db.SaveChanges();
                }
                var getIdentity = db.CertificateIdentifications.ToList().Where(x => x.CustomerId == data.CustomerId).ToList();
                for (int i = 0; i < getIdentity.Count(); i++)
                {
                    db.CertificateIdentifications.Remove(getIdentity[i]);
                    db.SaveChanges();
                }
                var getUser = db.AspNetUsers.ToList().Where(x => x.Email == data.Email).FirstOrDefault();
                if (getUser != null)
                {
                    db.AspNetUsers.Remove(getUser);
                    db.SaveChanges();
                }
                db.Customers.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ApproveCustomerByID")]
        public IActionResult ApproveCustomerByID(int id,string CreditLimit)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var CurrentCustomer = db.CustomerInformations.Find(id);
                if(CurrentCustomer != null)
                {
                    var CurrentUser = db.CustomerBillingInfos.ToList().Where(x => x.CustomerInformationId == Convert.ToInt32(id)).FirstOrDefault();
                    if (CurrentUser != null)
                    {
                        CurrentUser.CreditLimit = CreditLimit;
                        db.Entry(CurrentUser).State = EntityState.Modified;
                    }
                    CurrentCustomer.Approved= true;
                    CurrentCustomer.Rejected = false;
                    CurrentCustomer.Pending = false;
                    CurrentCustomer.AdminApproved = true;
                    db.Entry(CurrentCustomer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    var GetUser = db.AspNetUsers.ToList().Where(x=>x.Email == CurrentCustomer.Email || x.Id == CurrentCustomer.UserId).FirstOrDefault();
                    if(GetUser != null)
                    {
                        GetUser.AdminApproval = true;
                        GetUser.IsCancelled = false;
                        GetUser.IsActive = true;
                        db.Entry(GetUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                    }

                    bool isEmailSent = emailService.SendNewCustomerEmail("ApproveCustomer", GetUser.Email, GetUser.UserName);
                    if (isEmailSent)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);

                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("RejectCustomerByID/{id}")]
        public IActionResult RejectCustomerByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                var CurrentCustomer = db.CustomerInformations.Find(id);
                if (CurrentCustomer != null)
                {
                    CurrentCustomer.Approved = false;
                    CurrentCustomer.Rejected = true;
                    CurrentCustomer.Pending = false;
                    CurrentCustomer.AdminApproved = false;
                    db.Entry(CurrentCustomer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    var GetUser = db.AspNetUsers.Find(CurrentCustomer.UserId);
                    if (GetUser != null)
                    {
                        GetUser.AdminApproval = false;
                        GetUser.IsCancelled = true;
                        GetUser.IsActive = false;
                        db.Entry(GetUser).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                    }
                    bool isEmailSent = emailService.SendNewCustomerEmail("RejectCustomer", GetUser.Email, GetUser.UserName);
                    if (isEmailSent)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);

                }
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
