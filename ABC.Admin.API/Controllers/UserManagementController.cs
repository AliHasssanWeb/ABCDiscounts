using ABC.EFCore.Entities.HR;
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
using SecurityKey = ABC.EFCore.Repository.Edmx.SecurityKey;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;


namespace ABC.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {


        protected readonly ABCDiscountsContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMailService mailService;
        private IWebHostEnvironment env;
        EncryptDecrypt encdec = new EncryptDecrypt();

        public UserManagementController(ABCDiscountsContext db, IWebHostEnvironment webHostEnvironment, IMailService mailService, IWebHostEnvironment env)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            this.mailService = mailService;
            this.env = env;
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

        [HttpGet("UserGet")]
        public IActionResult UserGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<AspNetUser>>();
                var record = _db.AspNetUsers.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UserCreate")]
        public async Task<IActionResult> UserCreate(AspNetUser obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                obj.UserName = obj.Firstname + " " + obj.Lastname;
                if (obj.UserName == null || obj.UserName == "")
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);

                }
                if (obj.UserPic != null)
                {
                    //  return BadRequest("Image Path Required Instead of Byte");
                    var imgPath = SaveImage(obj.UserPic, Guid.NewGuid().ToString());
                    obj.Imageupload = imgPath;
                    obj.UserPic = null;

                }

                if (obj.PasswordHash == null && obj.PasswordHash == "")
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);

                }
                else
                {
                    obj.PasswordHash = encdec.Encrypt(obj.PasswordHash);
                }


                bool isValid = _db.AspNetUsers.ToList().Exists(p => p.Email.Equals(obj.Email, StringComparison.CurrentCultureIgnoreCase));
                if (isValid)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.EMAIL_ALREADY, null, null);
                }
                else
                {

                   // obj.CreatedDate = DateTime.Now;
                    var getrole = _db.AspNetRoles.ToList().Where(x => x.Name == "Admin").FirstOrDefault();
                    if (getrole == null)
                    {
                        AspNetRole newrole = new AspNetRole();
                        newrole.Name = "Admin";
                        _db.AspNetRoles.Add(newrole);
                        await _db.SaveChangesAsync();
                        var getroleagain = _db.AspNetRoles.AsQueryable().ToList().Where(x => x.Name == "Admin").FirstOrDefault();
                        obj.RoleId = Convert.ToInt32(getroleagain.Id);

                    }
                    else
                    {
                        obj.RoleId = Convert.ToInt32(getrole.Id); 
                    }
                    obj.ExpiryDate = null;
                    obj.ModifiedDate = null;
                    obj.LastChangePwdDate = null;
                    obj.LastLogin = null;
                    _db.AspNetUsers.Add(obj);
                    await _db.SaveChangesAsync();

                    var newUser = _db.AspNetUsers.AsQueryable().ToList().Where(x => x.Email == obj.Email).FirstOrDefault();
                    if (newUser != null)
                    {
                        AspNetUserRole userroles = new AspNetUserRole();
                        userroles.RolesId = getrole.Id;
                        userroles.UserId = Convert.ToInt32(newUser.Id);
                        _db.AspNetUserRoles.Add(userroles);
                    }
                    MailRequest request = new MailRequest();
                    request.ToEmail = newUser.Email;
                    request.Subject = "ABCDiscounts";
                    var usermsg = "<div><h1>Welcome To ABC Discounts We are delighted to have you among us. On behalf of all the members and the management, we would like to extend our warmest welcome and good wishes!</h3><h4> Your new account has generated, you can login from the following link </h4><a href='https://localhost:44372/' target='_blank'> ABCDiscounts Application </a><h3>Your login Security PIN is.0000 </h3><div>";
                    request.Body = usermsg;
                    var emailresponse = await Send(request);
                    if (emailresponse == "Email Sent Successfully")
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                        // return CreatedAtAction("UserGet", new { id = obj.Id }, "Admin Account Registered Successfully Email Sent Successfully");
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);
                        return Ok(Response);
                        // return CreatedAtAction("UserGet", new { id = obj.Id }, "Admin Account Registered Successfully Could't Send Email Due to Slow Internet");
                    }
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UserUpdate/{id}")]
        public async Task<IActionResult> UserUpdate(int id, AspNetUser data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != Convert.ToInt32(data.Id))
                {
                    return BadRequest();
                }
                bool isValid = _db.AspNetUsers.ToList().Exists(x => x.UserName.Equals(data.UserName, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                else
                {
                    var record = await _db.AspNetUsers.FindAsync(id);
                    if (data.UserName != null && data.UserName != "undefined")
                    {
                        record.UserName = data.UserName;
                    }
                    if (data.RoleId != null)
                    {
                        record.RoleId = data.RoleId;
                    }
                    data = record;
                    _db.Entry(data).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                }
                return Ok(Response);


            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                AspNetUser data = await _db.AspNetUsers.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                _db.AspNetUsers.Remove(data);
                await _db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }




        [HttpGet("RoleGet")]
        public IActionResult RoleGet()
        {
            try
            {
                var record = _db.AspNetRoles.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<AspNetRole>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("RoleGetByID/{id}")]
        public IActionResult RoleGetByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();

                var record = _db.AspNetRoles.Find(id);
                if (record != null)
                {
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
                    var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("RoleCreate")]
        public async Task<IActionResult> RoleCreate(AspNetRole obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                bool checkname = _db.AspNetRoles.ToList().Exists(p => p.Name.Equals(obj.Name, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                _db.AspNetRoles.Add(obj);
                await _db.SaveChangesAsync();
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("RoleUpdate/{id}")]
        public async Task<IActionResult> RoleUpdate(int id, AspNetRole data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != Convert.ToInt32(data.Id))
                {
                    return BadRequest();
                }
                bool isValid = _db.AspNetRoles.ToList().Exists(x => x.Name.Equals(data.Name, StringComparison.CurrentCultureIgnoreCase) && x.Id != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                var record = await _db.AspNetRoles.FindAsync(id);
                if (data.Name != null && data.Name != "undefined")
                {
                    record.Name = data.Name;
                }
                data = record;
                _db.Entry(data).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                AspNetRole data = await _db.AspNetRoles.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _db.AspNetRoles.Remove(data);
                await _db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetRole>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        private string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = env.WebRootPath;
            string webRootPath = env.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                return imgPath;
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
        }


        [HttpGet("EmployeeGet")]
        public IActionResult EmployeeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                var record = _db.Employees.ToList();


                for (int i = 0; i < record.Count(); i++)
                {
                    if (record[i].YesContractor == null)
                    {
                        record[i].YesContractor = false;
                    }
                    if (record[i].NoContractor == null)
                    {
                        record[i].NoContractor = false;
                    }

                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("PullerEmployeeGet")]
        public IActionResult PullerEmployeeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();

                var GetRolePuller = _db.AspNetRoles.ToList().Where(x=>x.Name == "Puller").FirstOrDefault();
                if(GetRolePuller == null)
                {
                    AspNetRole newrole = new AspNetRole();
                    newrole.Name = "Puller";
                   var foundRole = _db.AspNetRoles.Add(newrole);   
                    _db.SaveChanges();

                    var record = _db.Employees.ToList().Where(x=>x.RoleId == foundRole.Entity.Id).ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].YesContractor == null)
                        {
                            record[i].YesContractor = false;
                        }
                        if (record[i].NoContractor == null)
                        {
                            record[i].NoContractor = false;
                        }

                    }
                    if (record.Count > 0)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                    return Ok(Response);
                }
                else
                {

                    var record = _db.Employees.ToList().Where(x => x.RoleId == GetRolePuller.Id).ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].YesContractor == null)
                        {
                            record[i].YesContractor = false;
                        }
                        if (record[i].NoContractor == null)
                        {
                            record[i].NoContractor = false;
                        }

                    }
                    if(record.Count > 0)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EmployeeDocumentGet")]
        public IActionResult EmployeeDocumentGet()
        {
            try
            {
                var record = _db.EmployeeDocuments.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<EmployeeDocument>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EmployeeByID/{id}")]
        public IActionResult EmployeeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                var record = _db.Employees.Find(id);
                var getWithHold = _db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                if (record != null)
                {
                    if (record.YesContractor == null)
                    {
                        record.YesContractor = false;
                    }
                    if (record.NoContractor == null)
                    {
                        record.NoContractor = false;
                    }
                    if (getWithHold != null)
                    {
                        record.EmployeeWithHoldingTax = getWithHold;
                    }
                    else
                    {
                        record.EmployeeWithHoldingTax = null;
                    }

                    var EmpWithHoldTex = _db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpWithHoldTex != null)
                    {
                        if (EmpWithHoldTex.Domiciled == null)
                        {
                            EmpWithHoldTex.Domiciled = false;
                        }
                        if (EmpWithHoldTex.MilatrySpouseExempt == null)
                        {
                            EmpWithHoldTex.MilatrySpouseExempt = false;
                        }
                        if (EmpWithHoldTex.NoExemptions == null)
                        {
                            EmpWithHoldTex.NoExemptions = false;
                        }
                        if (EmpWithHoldTex.NoTaxLaibility == null)
                        {
                            EmpWithHoldTex.NoTaxLaibility = false;
                        }
                        record.EmployeeWithHoldingTax = EmpWithHoldTex;
                    }

                    var EmpAuthRep = _db.EmployeeAuthorizedRepresentatives.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpAuthRep != null)
                    {
                        record.employeeAuthorizedRepresentative = EmpAuthRep;
                    }

                    var EmpRevf = _db.EmployeeReverificationAndRehires.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpRevf != null)
                    {
                        record.EmployeeReverificationAndRehire = EmpRevf;
                    }

                    var EmpEllig = _db.EmploymentEligibilityVerifications.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpEllig != null)
                    {
                        if (EmpEllig.Lawful == null)
                        {
                            EmpEllig.Lawful = false;
                        }
                        if (EmpEllig.NoCitizen == null)
                        {
                            EmpEllig.NoCitizen = false;
                        }
                        if (EmpEllig.Citizen == null)
                        {
                            EmpEllig.Citizen = false;
                        }
                        if (EmpEllig.AllienAuthorized == null)
                        {
                            EmpEllig.AllienAuthorized = false;
                        }
                        record.EmploymentEligibilityVerification = EmpEllig;
                    }

                    var EmpDDAuth = _db.EmployeeDdauthorizations.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpDDAuth != null)
                    {
                        if (EmpDDAuth.Authorize == null)
                        {
                            EmpDDAuth.Authorize = false;
                        }
                        if (EmpDDAuth.RemainingBalance == null)
                        {
                            EmpDDAuth.RemainingBalance = false;
                        }
                        if (EmpDDAuth.UsePercentage == null)
                        {
                            EmpDDAuth.UsePercentage = false;
                        }
                        if (EmpDDAuth.Authorize == null)
                        {
                            EmpDDAuth.Authorize = false;
                        }
                        if (EmpDDAuth.Revise == null)
                        {
                            EmpDDAuth.Revise = false;
                        }
                        if (EmpDDAuth.Cancel == null)
                        {
                            EmpDDAuth.Cancel = false;
                        }
                        record.EmployeeDdauthorization = EmpDDAuth;
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);

                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ApprovedEmployee/{id}")]
        public async Task<IActionResult> ApprovedEmployee(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                var record = await _db.Employees.FindAsync(id);
                record.AdminApproval = true;
                record.AccessAccount = false;
                record.IsActive = true;
                record.AdminStatus = false;
                record.AdminStatus = false;
                record.RoleId = 0;
                _db.Entry(record).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("ApprovedRegisterEmployee")]
        public async Task<IActionResult> ApprovedRegisterEmployee(EmployeeRegister data)
        {
            try
            {
                
                var Response = ResponseBuilder.BuildWSResponse<EmployeeRegister>();
                var accessPinExist = _db.Supervisors.FirstOrDefault(x => x.AccessPin == data.AccessPin);
                if (accessPinExist != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (data.EmployeeID == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                var record = await _db.Employees.FindAsync(data.EmployeeID);
                if (record == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                record.AdminApproval = true;
                record.AccessAccount = true;
                record.IsActive = true;
                record.AdminStatus = false;
                record.RoleId = data.RoleId;
                _db.Entry(record).State = EntityState.Modified;
                await _db.SaveChangesAsync();

                AspNetUser objuser = new AspNetUser();
                objuser.Email = record.Email;
                objuser.EmployeeId = record.EmployeeId;
               // objuser.CreatedDate = DateTime.Now;
                objuser.Deleted = false;
                objuser.DrivingLicense = record.DrivingLisence;
                objuser.Firstname = record.FullName;
                objuser.FromScreen = "From Employees";
                objuser.IsActive = true;
                objuser.Mobile = record.Mobile;
                objuser.PasswordHash = encdec.Encrypt(record.Email);
                objuser.RoleId = data.RoleId;
                objuser.UserName = record.FullName;

                _db.AspNetUsers.Add(objuser);
                _db.SaveChanges();

                var currentuser = _db.AspNetUsers.AsQueryable().ToList().Where(x => x.EmployeeId == data.EmployeeID && x.Email == record.Email).FirstOrDefault();
                if (currentuser != null)
                {
                    AspNetUserRole objuserrole = new AspNetUserRole();
                    objuserrole.UserId = Convert.ToInt32(currentuser.Id);
                    objuserrole.RolesId = Convert.ToInt32(currentuser.RoleId);
                   
                    if (currentuser.RoleId != null)
                    {
                        var rolename = await _db.AspNetRoles.FindAsync(currentuser.RoleId);
                       
                    }
                    _db.AspNetUserRoles.Add(objuserrole);
                    _db.SaveChanges();
                }




                //Add Supervisor
                //Add SalesManager
                var newuser = _db.AspNetUsers.AsQueryable().ToList().Where(x => x.EmployeeId == data.EmployeeID && x.Email == record.Email).FirstOrDefault();
                if (currentuser != null)
                {
                    if (currentuser.RoleId != null)
                    {
                        var rolename = await _db.AspNetRoles.FindAsync(currentuser.RoleId);
                        if (rolename.Name == "Supervisor")
                        {
                            Supervisor objsuper = new Supervisor();


                            if (data.AccessPin == null || data.AccessPin == "undefined")
                            {
                                ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                            }
                            else
                            {
                                objsuper.AccessPin = data.AccessPin;
                                objsuper.UserId = Convert.ToInt32(currentuser.Id);
                                objsuper.RoleId = currentuser.RoleId;
                                objsuper.EmployeeId = currentuser.EmployeeId;
                                objsuper.CreditLimit = data.CreditLimit;
                                objsuper.RoleName = rolename.Name;
                                _db.Supervisors.Add(objsuper);
                                _db.SaveChanges();
                            }
                        }
                        if (rolename.Name == "Sales Manager")
                        {
                            SalesManager objsuper = new SalesManager();
                            if (data.AccessPin == null || data.AccessPin == "undefined")
                            {
                                ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                            }
                            else
                            {
                                objsuper.AccessPin = data.AccessPin;
                                objsuper.UserId = Convert.ToInt32(currentuser.Id);
                                objsuper.RoleId = currentuser.RoleId;
                                objsuper.CreditLimit = data.CreditLimit;
                                objsuper.EmployeeId = currentuser.EmployeeId;
                                _db.SalesManagers.Add(objsuper);
                                _db.SaveChanges();
                            }
                        }
                    }

                }

                MailRequest request = new MailRequest();
                request.ToEmail = record.Email;
                request.Subject = "OTP ABCDiscounts";
                var usermsg = "<div></h3>Dear Employee, Welcome on board, Please use your email as username and password to login on system.</h3><div>";
                request.Body = usermsg;
                var emailresponse = await Send(request);
                if (emailresponse == "Email Sent Successfully")
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                    //return CreatedAtAction("EmployeeGet", new { id = record.EmployeeId }, "Employee Approved & Registered Successfully & Email Sent Successfully ");
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                    //return CreatedAtAction("EmployeeGet", new { id = record.EmployeeId }, "Employee Approved & Registered Successfully Could't Send Email Due to Slow Internet");
                }

                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeRegister>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RejectEmployee/{id}")]
        public async Task<IActionResult> RejectEmployee(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                var record = await _db.Employees.FindAsync(id);
                record.AdminApproval = false;
                record.AccessAccount = false;
                record.IsActive = false;
                record.AdminStatus = false;
                _db.Entry(record).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();

                Employee data = await _db.Employees.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                _db.Employees.Remove(data);
                await _db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("SupervisorAndManagerGet")]
        public IActionResult SupervisorAndManagerGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Supervisor>>();
                var record = _db.Supervisors.ToList();
                var recordCredit = _db.SupervisorCredits.ToList();

                DateTime dt = DateTime.Now.Date.AddDays(-13);
                for (int i = 0; i < record.Count(); i++)
                {
                    double totalavail = 0;
                    var GetUser = _db.AspNetUsers.Find(record[i].UserId);
                    if (GetUser != null)
                    {
                        record[i].FullName = GetUser.UserName;
                    }
                    var Getcustomer = _db.CustomerInformations.Find(record[i].CustomerId);
                    if (Getcustomer != null)
                    {
                        record[i].CustomerName = Getcustomer.Company;
                    }
                    foreach (var item in recordCredit.ToList().Where(x => x.SupervisorId == record[i].SupervisorId && x.CreditDate >= dt && x.CustomerId == record[i].CustomerId).ToList())
                    {
                        totalavail += Convert.ToDouble(item.CreditAmount);
                    }

                    record[i].AvailedCredit = totalavail.ToString();
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("PullerCashierGet")]
        public IActionResult PullerCashierGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();

                var GetRolePuller = _db.AspNetRoles.ToList().Where(x => x.Name == "Cashier").FirstOrDefault();
                if (GetRolePuller == null)
                {
                    AspNetRole newrole = new AspNetRole();
                    newrole.Name = "Cashier";
                    var foundRole = _db.AspNetRoles.Add(newrole);
                    _db.SaveChanges();

                    var record = _db.Employees.ToList().Where(x => x.RoleId == foundRole.Entity.Id).ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].YesContractor == null)
                        {
                            record[i].YesContractor = false;
                        }
                        if (record[i].NoContractor == null)
                        {
                            record[i].NoContractor = false;
                        }

                    }
                    if (record.Count > 0)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                    return Ok(Response);
                }
                else
                {

                    var record = _db.Employees.ToList().Where(x => x.RoleId == GetRolePuller.Id).ToList();
                    for (int i = 0; i < record.Count(); i++)
                    {
                        if (record[i].YesContractor == null)
                        {
                            record[i].YesContractor = false;
                        }
                        if (record[i].NoContractor == null)
                        {
                            record[i].NoContractor = false;
                        }

                    }
                    if (record.Count > 0)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);

                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    }
                    return Ok(Response);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateSupervisor")]
        public async Task<IActionResult> UpdateSupervisor(Supervisor model)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Supervisor>>();
                var GetData = _db.Supervisors.ToList().Where(x => x.SupervisorId == model.SupervisorId).FirstOrDefault();
                if (GetData != null)
                {
                    if (model.AccessPin != null)
                    {
                        GetData.AccessPin = model.AccessPin;
                    }
                    if (model.CreditLimit != null)
                    {
                        GetData.CreditLimit = model.CreditLimit;
                    }
                    model = GetData;
                    _db.Entry(model).State = EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return Ok(Response);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            try
            {
                var record = _db.CustomerInformations.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<CustomerInformation>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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
        [HttpGet("SupervisorManagersGet")]
        public IActionResult SupervisorManagersGet()
        {
            try
            {
                var roledata = _db.AspNetRoles.Where(x => x.Name == "Supervisor" || x.Name == "Sales Manager").ToList();
                var list = new List<Employee>();
                var lastlist = new List<Employee>();
                foreach (var item in roledata)
                {
                    list = _db.Employees.Where(x => x.RoleId == item.Id).ToList();
                    lastlist.AddRange(list);
                }
                // var record = _db.AspNetUsers.Where(x=>x.RoleName).ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, lastlist);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employee>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("SuperVisorGet/{id}")]
        public IActionResult SuperVisorGet(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Supervisor>>();
                var record = _db.Supervisors.Where(x => x.RoleId == id).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddCreditSupervisor")]
        public async Task<IActionResult> AddCreditSupervisor(Supervisor data)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                var accessPinExist = _db.Supervisors.FirstOrDefault(x => x.CustomerId == data.CustomerId && x.EmployeeId==data.EmployeeId);/* User id match*/
                if (accessPinExist != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                Supervisor supervisor = new Supervisor();


                var userdata = _db.AspNetUsers.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                if (userdata != null)
                {
                    var rolename = await _db.AspNetRoles.FindAsync(userdata.RoleId);
                    if (rolename != null)
                    {
                        supervisor.RoleName = rolename.Name;
                        supervisor.RoleId = data.RoleId;

                    }
                    if (userdata.EmployeeId != null)
                    {
                        supervisor.EmployeeId = userdata.EmployeeId;
                    }
                    supervisor.UserId = userdata.Id;
                }
                supervisor.CustomerId = data.CustomerId;
                supervisor.CreditLimit = data.CreditLimit;
                supervisor.AccessPin = data.AccessPin;
                _db.Supervisors.Add(supervisor);
                _db.SaveChanges();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Supervisor>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AdminPinGet")]
        public IActionResult AdminPinGet()
        {
            try
            {
                var roledata = _db.AspNetRoles.Where(x => x.Name == "Admin").FirstOrDefault();
                var adminUsers=_db.AspNetUsers.ToList().Where(x=>x.RoleId==roledata.Id).ToList();
                var list = new List<SecurityKey>();
                var lastlist = new List<SecurityKey>();
                foreach (var item in adminUsers)
                {
                    list = _db.SecurityKeys.ToList().Where(x=>x.UserEmail==item.Email).ToList();
                    lastlist.AddRange(list);

                }
                var Response = ResponseBuilder.BuildWSResponse<List<SecurityKey>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, lastlist);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SecurityKey>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AdminPinChangeStatus")]
        public async Task<IActionResult> AdminPinChangeStatus(int keyid,int status)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<SecurityKey>();
                Boolean activebit = true;
                var getdata = _db.SecurityKeys.ToList().Where(x => x.SecurityKeyId == keyid).FirstOrDefault();
                if (getdata!=null)
                {
                    if (status == 1)
                    {
                        activebit = true;
                    }
                    else
                    {
                        activebit = false;
                    }
                    if (getdata != null)
                    {
                        getdata.Active = activebit;
                    }
                    getdata.UpdateDate = DateTime.Now;
                    _db.Entry(getdata).State = EntityState.Modified;
                    await _db.SaveChangesAsync();

                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }
                else
                {
                    var Responsee = ResponseBuilder.BuildWSResponse<SecurityKey>();
                    ResponseBuilder.SetWSResponse(Responsee, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Responsee);
                }
                
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SecurityKey>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAdminUsers")]
        public IActionResult GetAdminUsers()
        {
            try
            {
                var roledata = _db.AspNetRoles.Where(x => x.Name == "Admin").FirstOrDefault();
                var adminUsers = _db.AspNetUsers.ToList().Where(x => x.RoleId == roledata.Id).ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<AspNetUser>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, adminUsers);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddAdminUser")]
        public async Task<IActionResult> AddAdminUser(SecurityKey data)
        {
            try
            {

                var Response = ResponseBuilder.BuildWSResponse<SecurityKey>();
                var accessuserExists = _db.SecurityKeys.FirstOrDefault(x => x.UserId == data.UserId);/* User id match*/
                if (accessuserExists != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                SecurityKey securityKey = new SecurityKey();


                var userdata = _db.AspNetUsers.ToList().Where(x => x.Id == data.UserId).FirstOrDefault();
                if (userdata != null)
                {
                    securityKey.UserName=userdata.UserName;
                    securityKey.UserEmail = userdata.Email;
                    securityKey.UserId = userdata.Id;
                }

                securityKey.CreadtedDate = DateTime.Now;
                securityKey.KeyPin = data.KeyPin;
                securityKey.Type = data.Type;
                securityKey.Active = true;
                _db.SecurityKeys.Add(securityKey);
                _db.SaveChanges();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<SecurityKey>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EmployeeContractByID/{id}")]
        public IActionResult EmployeeContractByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                var record = _db.EmployeeContracts.Find(id);
                if (record != null)
                {
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EmployeeContractEmployeeID/{id}")]
        public IActionResult EmployeeContractEmployeeID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                var record = _db.EmployeeContracts.Where(x => x.EmployeeId == id).FirstOrDefault();
                if (record != null)
                {
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("BlocknUnblock/{id}")]
        public IActionResult BlocknUnblock(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                var record = _db.AspNetUsers.Find(id);
                if (record != null)
                {
                    if(record.IsCancelled == true)
                    {
                        record.IsCancelled = false;
                        _db.Entry(record).State = EntityState.Modified;
                         _db.SaveChangesAsync();
                    }
                    else if (record.IsCancelled == false)
                    {
                        record.IsCancelled = true;
                        _db.Entry(record).State = EntityState.Modified;
                        _db.SaveChangesAsync();
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
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("BlockedUsersGet")]
        public IActionResult BlockedUsersGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<AspNetUser>>();
                var roledata = _db.AspNetRoles.Where(x => x.Name == "Admin").FirstOrDefault();
                var adminUsers = _db.AspNetUsers.ToList().Where(x => x.RoleId == roledata.Id).ToList();
                var getusers = _db.AspNetUsers.ToList().Where(x => x.RoleId == roledata.Id && x.IsCancelled == true).ToList();

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getusers);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
