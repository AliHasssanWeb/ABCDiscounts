using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ABC.HR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HRController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        EncryptDecrypt encdec = new EncryptDecrypt();
        private readonly IMailService mailService;

        public HRController(ABCDiscountsContext db, IWebHostEnvironment webHostEnvironment, IMailService mailService)
        {
            this.db = db;
            _webHostEnvironment = webHostEnvironment;
            this.mailService = mailService;
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

        [HttpGet("EmployeeGet")]
        public IActionResult EmployeeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                var record = db.Employees.ToList();

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
        [HttpGet("EmployeeGetDropDown")]
        public IActionResult EmployeeGetDropDown()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                var employeeWithContract = db.EmployeeContracts.Where(x=>x.ContractName== "Permanent").ToList();
                var record = new List<Employee>();
                if (employeeWithContract.Count() > 0)
                {
                    foreach (var item in employeeWithContract)
                    {
                        var employee = db.Employees.Where(x => x.EmployeeId == item.EmployeeId)?.FirstOrDefault();
                        if (employee != null)
                        {
                            record.Add(employee);
                        }
                    }


                }


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
        [HttpGet("EmployeeContractDropDown")]
        public IActionResult EmployeeContractDropDown()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                var employeeWithContract = db.EmployeeContracts.Where(x=>x.ContractName== "Contract").ToList();
                var record = new List<Employee>();
                if (employeeWithContract.Count() > 0)
                {
                    foreach (var item in employeeWithContract)
                    {
                        var employee = db.Employees.Where(x => x.EmployeeId == item.EmployeeId)?.FirstOrDefault();
                        if (employee != null)
                        {
                            record.Add(employee);
                        }
                    }


                }


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
        [HttpGet("EmployeeActiveDropDown")]
        public IActionResult EmployeeActiveDropDown()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();

                var record = new List<Employee>();

                record = db.Employees.Where(x => x.AdminApproval == true).ToList();
                



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

        [HttpGet("GetAllEmployeeData")]
        public IActionResult GetAllEmployeeData()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Employee>>();
                var record = db.Employees.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    var EmpWithHoldTex = db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == record[i].EmployeeId).FirstOrDefault();
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
                        record[i].EmployeeWithHoldingTax = EmpWithHoldTex;
                    }

                    var EmpAuthRep = db.EmployeeAuthorizedRepresentatives.ToList().Where(x => x.EmployeeId == record[i].EmployeeId).FirstOrDefault();
                    if (EmpAuthRep != null)
                    {
                        record[i].employeeAuthorizedRepresentative = EmpAuthRep;
                    }

                    var EmpRevf = db.EmployeeReverificationAndRehires.ToList().Where(x => x.EmployeeId == record[i].EmployeeId).FirstOrDefault();
                    if (EmpRevf != null)
                    {
                        record[i].EmployeeReverificationAndRehire = EmpRevf;
                    }

                    var EmpEllig = db.EmploymentEligibilityVerifications.ToList().Where(x => x.EmployeeId == record[i].EmployeeId).FirstOrDefault();
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
                        record[i].EmploymentEligibilityVerification = EmpEllig;
                    }

                    var EmpDDAuth = db.EmployeeDdauthorizations.ToList().Where(x => x.EmployeeId == record[i].EmployeeId).FirstOrDefault();
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
                        record[i].EmployeeDdauthorization = EmpDDAuth;
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



        [HttpPost("ApprovedEmployee/{id}")]
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
                var record = await db.Employees.FindAsync(id);
                record.AdminApproval = true;
                record.AccessAccount = false;
                record.IsActive = true;
                record.AdminStatus = false;
                db.Entry(record).State = EntityState.Modified;
                await db.SaveChangesAsync();
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

        [HttpGet("EmployeeByID/{id}")]
        public IActionResult EmployeeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                var record = db.Employees.Find(id);
                var getWithHold = db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
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

                    var EmpWithHoldTex = db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
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

                    var EmpAuthRep = db.EmployeeAuthorizedRepresentatives.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpAuthRep != null)
                    {
                        record.employeeAuthorizedRepresentative = EmpAuthRep;
                    }

                    var EmpRevf = db.EmployeeReverificationAndRehires.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
                    if (EmpRevf != null)
                    {
                        record.EmployeeReverificationAndRehire = EmpRevf;
                    }

                    var EmpEllig = db.EmploymentEligibilityVerifications.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
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

                    var EmpDDAuth = db.EmployeeDdauthorizations.ToList().Where(x => x.EmployeeId == record.EmployeeId).FirstOrDefault();
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

        [HttpPost("ApprovedRegisterEmployee")]
        public async Task<IActionResult> ApprovedRegisterEmployee(EmployeeRegister data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeRegister>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (data.EmployeeID == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                var record = await db.Employees.FindAsync(data.EmployeeID);
                if (record == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                record.AdminApproval = true;
                record.AccessAccount = true;
                record.IsActive = true;
                record.AdminStatus = false;
                db.Entry(record).State = EntityState.Modified;
                await db.SaveChangesAsync();

                AspNetUser objuser = new AspNetUser();
                objuser.Email = record.Email;
                objuser.EmployeeId = record.EmployeeId;
              //  objuser.CreatedDate = DateTime.Now;
                objuser.Deleted = false;
                objuser.DrivingLicense = record.DrivingLisence;
                objuser.Firstname = record.FullName;
                objuser.FromScreen = "From Employees";
                objuser.IsActive = true;
                objuser.Mobile = record.Mobile;
                objuser.PasswordHash = encdec.Encrypt(record.Email);
                objuser.RoleId = data.RoleId;
                objuser.UserName = record.FullName;

                db.AspNetUsers.Add(objuser);
                db.SaveChanges();

                var currentuser = db.AspNetUsers.AsQueryable().ToList().Where(x => x.EmployeeId == data.EmployeeID && x.Email == record.Email).FirstOrDefault();
                if (currentuser != null)
                {
                    AspNetUserRole objuserrole = new AspNetUserRole();
                    objuserrole.UserId = Convert.ToInt32(currentuser.Id);
                    objuserrole.RolesId = Convert.ToInt32(currentuser.RoleId);
                    if (currentuser.RoleId != null)
                    {
                        var rolename = await db.AspNetRoles.FindAsync(currentuser.RoleId);
                       
                    }
                    db.AspNetUserRoles.Add(objuserrole);
                    db.SaveChanges();
                }




                //Add Supervisor
                //Add SalesManager
                //var newuser = db.AspNetUser.AsQueryable().ToList().Where(x => x.EmployeeId == data.EmployeeID && x.Email == record.Email).FirstOrDefault();
                //if (currentuser != null)
                //{
                //    if (currentuser.RoleId != null)
                //    {
                //        var rolename = await db.AspNetRole.FindAsync(currentuser.RoleId);
                //        if (rolename.Name == "Supervisor")
                //        {
                //            Supervisor objsuper = new Supervisor();
                //            if (data.AccessPin == null || data.AccessPin == "undefined")
                //            {
                //                ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                //            }
                //            else-

                //            {
                //                objsuper.AccessPin = data.AccessPin;
                //                objsuper.UserId = currentuser.Id;
                //                objsuper.RoleId = currentuser.RoleId;
                //                objsuper.EmployeeId = currentuser.EmployeeId;
                //                db.Supervisors.Add(objsuper);
                //                db.SaveChanges();
                //            }
                //        }
                //        if(rolename.Name == "Sales Manager") {
                //            SalesManager objsuper = new SalesManager();
                //            if (data.AccessPin == null || data.AccessPin == "undefined")
                //            {
                //                ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                //            }
                //            else
                //            {
                //                objsuper.AccessPin = data.AccessPin;
                //                objsuper.UserId = currentuser.Id;
                //                objsuper.RoleId = currentuser.RoleId;
                //                objsuper.CreditLimit = data.CreditLimit;
                //                objsuper.EmployeeId = currentuser.EmployeeId;
                //                db.SalesManagers.Add(objsuper);
                //                db.SaveChanges();
                //            }
                //        }
                //    }

                //}

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
                var record = await db.Employees.FindAsync(id);
                record.AdminApproval = false;
                record.AccessAccount = false;
                record.IsActive = false;
                record.AdminStatus = false;
                db.Entry(record).State = EntityState.Modified;
                await db.SaveChangesAsync();
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

                Employee data = await db.Employees.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                var isemployeewithholdtax = db.EmployeeWithHoldingTaxes.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeewithholdtax)
                {
                    var employeewithholdtaxid = db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault().Eacid;
                    var employeewithholdtax = await db.EmployeeWithHoldingTaxes.FindAsync(employeewithholdtaxid);
                    if (employeewithholdtax != null)
                    {
                        db.EmployeeWithHoldingTaxes.Remove(employeewithholdtax);
                    }
                }
                var isemploymenteligibilityverification = db.EmploymentEligibilityVerifications.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemploymenteligibilityverification)
                {
                    var employmenteligibilityverificationid = db.EmploymentEligibilityVerifications.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault().Eevid;
                    var employmenteligibilityverification = await db.EmploymentEligibilityVerifications.FindAsync(employmenteligibilityverificationid);
                    if (employmenteligibilityverification != null)
                    {
                        db.EmploymentEligibilityVerifications.Remove(employmenteligibilityverification);
                    }
                }
                var isemployeeauthorizedrepresentative = db.EmployeeAuthorizedRepresentatives.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeeauthorizedrepresentative)
                {
                    var employeeauthorizedrepresentativeid = db.EmployeeAuthorizedRepresentatives.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault().EmpAuthRepId;
                    var employeeauthorizedrepresentative = await db.EmployeeAuthorizedRepresentatives.FindAsync(employeeauthorizedrepresentativeid);
                    if (employeeauthorizedrepresentative != null)
                    {
                        db.EmployeeAuthorizedRepresentatives.Remove(employeeauthorizedrepresentative);
                    }
                }
                var isemployeereverificationandrehires = db.EmployeeReverificationAndRehires.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeereverificationandrehires)
                {
                    var employeereverificationandrehiresid = db.EmployeeReverificationAndRehires.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault().EmpReverificationId;
                    var employeereverificationandrehires = await db.EmployeeReverificationAndRehires.FindAsync(employeereverificationandrehiresid);
                    if (employeereverificationandrehires != null)
                    {
                        db.EmployeeReverificationAndRehires.Remove(employeereverificationandrehires);
                    }
                }
                var isemployeeddauthorization = db.EmployeeDdauthorizations.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeeddauthorization)
                {
                    var employeeddauthorizationid = db.EmployeeDdauthorizations.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault().EmpDdaid;
                    var employeeddauthorization = await db.EmployeeDdauthorizations.FindAsync(employeeddauthorizationid);
                    if (employeeddauthorization != null)
                    {
                        db.EmployeeDdauthorizations.Remove(employeeddauthorization);
                    }
                }
                db.Employees.Remove(data);
                await db.SaveChangesAsync();
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

        [HttpPut("EmployeeUpdate/{id}")]
        public async Task<IActionResult> EmployeeUpdate(int id, Employee data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != data.EmployeeId)
                {
                    return BadRequest();
                }
                var employeerecord = await db.Employees.FindAsync(id);
                if (employeerecord != null)
                {
                    if (data.YesContractor == null)
                    {
                        employeerecord.YesContractor = false;
                    }
                    if (data.NoContractor == null)
                    {
                        employeerecord.NoContractor = false;
                    }
                    if (data.ProfileImage != null)
                    {
                        var imgPath = SaveImage(data.ProfileImage, Guid.NewGuid().ToString());
                        data.ProfileImagePath = imgPath;
                        data.ProfileImage = null;
                    }
                    if (data.EmployeeWithHoldingTax.Domiciled == null)
                    {
                        data.EmployeeWithHoldingTax.Domiciled = false;
                    }
                    if (data.EmployeeWithHoldingTax.EmployerWithHold == null)
                    {
                        data.EmployeeWithHoldingTax.EmployerWithHold = false;
                    }
                    if (data.EmployeeWithHoldingTax.MilatrySpouseExempt == null)
                    {
                        data.EmployeeWithHoldingTax.MilatrySpouseExempt = false;
                    }
                    if (data.EmployeeWithHoldingTax.NoExemptions == null)
                    {
                        data.EmployeeWithHoldingTax.NoExemptions = false;
                    }
                    if (data.EmployeeWithHoldingTax.NoTaxLaibility == null)
                    {
                        data.EmployeeWithHoldingTax.NoTaxLaibility = false;
                    }
                    if (data.EmploymentEligibilityVerification.Lawful == null)
                    {
                        data.EmploymentEligibilityVerification.Lawful = false;
                    }
                    if (data.EmploymentEligibilityVerification.NoCitizen == null)
                    {
                        data.EmploymentEligibilityVerification.NoCitizen = false;
                    }
                    if (data.EmploymentEligibilityVerification.Citizen == null)
                    {
                        data.EmploymentEligibilityVerification.Citizen = false;
                    }
                    if (data.EmploymentEligibilityVerification.AllienAuthorized == null)
                    {
                        data.EmploymentEligibilityVerification.AllienAuthorized = false;
                    }
                    if (data.EmployeeDdauthorization.Authorize == null)
                    {
                        data.EmployeeDdauthorization.Authorize = false;
                    }
                    if (data.EmployeeDdauthorization.Cancel == null)
                    {
                        data.EmployeeDdauthorization.Cancel = false;
                    }
                    if (data.EmployeeDdauthorization.Revise == null)
                    {
                        data.EmployeeDdauthorization.Revise = false;
                    }
                    if (data.EmployeeDdauthorization.RemainingBalance == null)
                    {
                        data.EmployeeDdauthorization.RemainingBalance = false;
                    }
                    if (data.EmployeeDdauthorization.UsePercentage == null)
                    {
                        data.EmployeeDdauthorization.UsePercentage = false;
                    }

                    if (data.FullName != null && data.FullName != "undefined")
                    {
                        employeerecord.FullName = data.FullName;
                    }
                    if (data.Email != null && data.Email != "undefined")
                    {
                        employeerecord.Email = data.Email;
                    }
                    if (data.Mobile != null && data.Mobile != "undefined")
                    {
                        employeerecord.Mobile = data.Mobile;
                    }
                    if (data.Phone != null && data.Phone != "undefined")
                    {
                        employeerecord.Mobile = data.Mobile;
                    }
                    if (data.Address != null && data.Address != "undefined")
                    {
                        employeerecord.Address = data.Address;
                    }
                    if (data.DrivingLisence != null && data.DrivingLisence != "undefined")
                    {
                        employeerecord.DrivingLisence = data.DrivingLisence;
                    }
                    if (data.State != null && data.State != "undefined")
                    {
                        employeerecord.State = data.State;
                    }
                    if (data.MartialStatus != null && data.MartialStatus != "undefined")
                    {
                        employeerecord.MartialStatus = data.MartialStatus;
                    }
                    if (data.SpouseName != null && data.SpouseName != "undefined")
                    {
                        employeerecord.SpouseName = data.SpouseName;
                    }
                    if (data.NoofChildren != null && data.NoofChildren != "undefined")
                    {
                        employeerecord.NoofChildren = data.NoofChildren;
                    }
                    if (data.EmployeeCity != null && data.EmployeeCity != "undefined")
                    {
                        employeerecord.EmployeeCity = data.EmployeeCity;
                    }
                    if (data.EmployeeZipCode != null && data.EmployeeZipCode != "undefined")
                    {
                        employeerecord.EmployeeZipCode = data.EmployeeZipCode;
                    }
                    if (data.FederalEmployeeId != null && data.FederalEmployeeId != "undefined")
                    {
                        employeerecord.FederalEmployeeId = data.FederalEmployeeId;
                    }
                    if (data.PayrolAddress != null && data.PayrolAddress != "undefined")
                    {
                        employeerecord.PayrolAddress = data.PayrolAddress;
                    }
                    if (data.Extention != null && data.Extention != "undefined")
                    {
                        employeerecord.Extention = data.Extention;
                    }
                    if (data.Ssn != null && data.Ssn != "undefined")
                    {
                        employeerecord.Ssn = data.Ssn;
                    }
                    if (data.YesContractor != null)
                    {
                        employeerecord.YesContractor = data.YesContractor;
                    }
                    if (data.EmployerZipCode != null && data.EmployerZipCode != "undefined")
                    {
                        employeerecord.EmployerZipCode = data.EmployerZipCode;
                    }
                    if (data.DateofHire != null)
                    {
                        employeerecord.DateofHire = data.DateofHire;
                    }
                    if (data.Dob != null)
                    {
                        employeerecord.Dob = data.Dob;
                    }
                    if (data.EmployerName != null && data.EmployerName != "undefined")
                    {
                        employeerecord.EmployerName = data.EmployerName;
                    }
                    if (data.EmployerEmail != null && data.EmployerEmail != "undefined")
                    {
                        employeerecord.EmployerEmail = data.EmployerEmail;
                    }
                    if (data.EmployerState != null && data.EmployerState != "undefined")
                    {
                        employeerecord.EmployerState = data.EmployerState;
                    }
                    if (data.EmployerPhone != null && data.EmployerPhone != "undefined")
                    {
                        employeerecord.EmployerPhone = data.EmployerPhone;
                    }
                    if (data.City != null && data.City != "undefined")
                    {
                        employeerecord.City = data.City;
                    }
                    if (data.NoContractor != null)
                    {
                        employeerecord.NoContractor = data.NoContractor;
                    }
                    if (data.EmployeeOfficeCode != null && data.EmployeeOfficeCode != "undefined")
                    {
                        employeerecord.EmployeeOfficeCode = data.EmployeeOfficeCode;
                    }
                    if (data.Ein != null && data.Ein != "undefined")
                    {
                        employeerecord.Ein = data.Ein;
                    }
                    if (data.Ein != null && data.Ein != "undefined")
                    {
                        employeerecord.Ein = data.Ein;
                    }
                    if (data.Expirationdate != null)
                    {
                        employeerecord.Expirationdate = data.Expirationdate;
                    }
                    db.Entry(employeerecord).State = EntityState.Modified;
                }
                var isemployeewithholdtax = db.EmployeeWithHoldingTaxes.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeewithholdtax)
                {
                    var employeewithholdtax = db.EmployeeWithHoldingTaxes.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                    if (employeewithholdtax != null)
                    {
                        if (data.EmployeeWithHoldingTax.FirstName != null && data.EmployeeWithHoldingTax.FirstName != "undefined")
                        {
                            employeewithholdtax.FirstName = data.EmployeeWithHoldingTax.FirstName;
                        }
                        if (data.EmployeeWithHoldingTax.MiddleName != null && data.EmployeeWithHoldingTax.MiddleName != "undefined")
                        {
                            employeewithholdtax.MiddleName = data.EmployeeWithHoldingTax.MiddleName;
                        }
                        if (data.EmployeeWithHoldingTax.LastName != null && data.EmployeeWithHoldingTax.LastName != "undefined")
                        {
                            employeewithholdtax.LastName = data.EmployeeWithHoldingTax.LastName;
                        }
                        if (data.EmployeeWithHoldingTax.Ssn != null && data.EmployeeWithHoldingTax.Ssn != "undefined")
                        {
                            employeewithholdtax.Ssn = data.EmployeeWithHoldingTax.Ssn;
                        }
                        if (data.EmployeeWithHoldingTax.Address != null && data.EmployeeWithHoldingTax.Address != "undefined")
                        {
                            employeewithholdtax.Address = data.EmployeeWithHoldingTax.Address;
                        }
                        if (data.EmployeeWithHoldingTax.MarriedStatus != null && data.EmployeeWithHoldingTax.MarriedStatus != "undefined")
                        {
                            employeewithholdtax.MarriedStatus = data.EmployeeWithHoldingTax.MarriedStatus;
                        }
                        if (data.EmployeeWithHoldingTax.City != null && data.EmployeeWithHoldingTax.City != "undefined")
                        {
                            employeewithholdtax.City = data.EmployeeWithHoldingTax.City;
                        }
                        if (data.EmployeeWithHoldingTax.State != null && data.EmployeeWithHoldingTax.State != "undefined")
                        {
                            employeewithholdtax.State = data.EmployeeWithHoldingTax.State;
                        }
                        if (data.EmployeeWithHoldingTax.ZipCode != null && data.EmployeeWithHoldingTax.ZipCode != "undefined")
                        {
                            employeewithholdtax.ZipCode = data.EmployeeWithHoldingTax.ZipCode;
                        }
                        if (data.EmployeeWithHoldingTax.NameDiffSsn != null && data.EmployeeWithHoldingTax.NameDiffSsn != "undefined")
                        {
                            employeewithholdtax.NameDiffSsn = data.EmployeeWithHoldingTax.NameDiffSsn;
                        }
                        if (data.EmployeeWithHoldingTax.Date != null)
                        {
                            employeewithholdtax.Date = data.EmployeeWithHoldingTax.Date;
                        }
                        if (data.EmployeeWithHoldingTax.EmployeeSignature != null && data.EmployeeWithHoldingTax.EmployeeSignature != "undefined")
                        {
                            employeewithholdtax.EmployeeSignature = data.EmployeeWithHoldingTax.EmployeeSignature;
                        }
                        if (data.EmployeeWithHoldingTax.Country != null && data.EmployeeWithHoldingTax.Country != "undefined")
                        {
                            employeewithholdtax.Country = data.EmployeeWithHoldingTax.Country;
                        }
                        if (data.EmployeeWithHoldingTax.NoofAllowances != null && data.EmployeeWithHoldingTax.NoofAllowances != "undefined")
                        {
                            employeewithholdtax.NoofAllowances = data.EmployeeWithHoldingTax.NoofAllowances;
                        }
                        if (data.EmployeeWithHoldingTax.AdditionalAmount != null && data.EmployeeWithHoldingTax.AdditionalAmount != "undefined")
                        {
                            employeewithholdtax.AdditionalAmount = data.EmployeeWithHoldingTax.AdditionalAmount;
                        }
                        if (data.EmployeeWithHoldingTax.NoTaxLaibility != null)
                        {
                            employeewithholdtax.NoTaxLaibility = data.EmployeeWithHoldingTax.NoTaxLaibility;
                        }
                        if (data.EmployeeWithHoldingTax.MilatrySpouseExempt != null)
                        {
                            employeewithholdtax.MilatrySpouseExempt = data.EmployeeWithHoldingTax.MilatrySpouseExempt;
                        }
                        if (data.EmployeeWithHoldingTax.Domiciled != null)
                        {
                            employeewithholdtax.Domiciled = data.EmployeeWithHoldingTax.Domiciled;
                        }
                        if (data.EmployeeWithHoldingTax.NoExemptions != null)
                        {
                            employeewithholdtax.NoExemptions = data.EmployeeWithHoldingTax.NoExemptions;
                        }
                        if (data.EmployeeWithHoldingTax.EmployerWithHold != null)
                        {
                            employeewithholdtax.EmployerWithHold = data.EmployeeWithHoldingTax.EmployerWithHold;
                        }

                        if (data.EmployeeWithHoldingTax.AdditionalAmount != null && data.EmployeeWithHoldingTax.AdditionalAmount != "undefined")
                        {
                            employeewithholdtax.AdditionalAmount = data.EmployeeWithHoldingTax.AdditionalAmount;
                        }
                        db.Entry(employeerecord).State = EntityState.Modified;
                    }
                }
                else
                {
                    EFCore.Repository.Edmx.EmployeeWithHoldingTax employeewithholdtax = new EFCore.Repository.Edmx.EmployeeWithHoldingTax();
                    if (data.EmployeeWithHoldingTax != null)
                    {
                        employeewithholdtax.EmployeeId = data.EmployeeId;
                        if (data.EmployeeWithHoldingTax.FirstName != null && data.EmployeeWithHoldingTax.FirstName != "undefined")
                        {
                            employeewithholdtax.FirstName = data.EmployeeWithHoldingTax.FirstName;
                        }
                        if (data.EmployeeWithHoldingTax.MiddleName != null && data.EmployeeWithHoldingTax.MiddleName != "undefined")
                        {
                            employeewithholdtax.MiddleName = data.EmployeeWithHoldingTax.MiddleName;
                        }
                        if (data.EmployeeWithHoldingTax.LastName != null && data.EmployeeWithHoldingTax.LastName != "undefined")
                        {
                            employeewithholdtax.LastName = data.EmployeeWithHoldingTax.LastName;
                        }
                        if (data.EmployeeWithHoldingTax.Ssn != null && data.EmployeeWithHoldingTax.Ssn != "undefined")
                        {
                            employeewithholdtax.Ssn = data.EmployeeWithHoldingTax.Ssn;
                        }
                        if (data.EmployeeWithHoldingTax.Address != null && data.EmployeeWithHoldingTax.Address != "undefined")
                        {
                            employeewithholdtax.Address = data.EmployeeWithHoldingTax.Address;
                        }
                        if (data.EmployeeWithHoldingTax.MarriedStatus != null && data.EmployeeWithHoldingTax.MarriedStatus != "undefined")
                        {
                            employeewithholdtax.MarriedStatus = data.EmployeeWithHoldingTax.MarriedStatus;
                        }
                        if (data.EmployeeWithHoldingTax.City != null && data.EmployeeWithHoldingTax.City != "undefined")
                        {
                            employeewithholdtax.City = data.EmployeeWithHoldingTax.City;
                        }
                        if (data.EmployeeWithHoldingTax.State != null && data.EmployeeWithHoldingTax.State != "undefined")
                        {
                            employeewithholdtax.State = data.EmployeeWithHoldingTax.State;
                        }
                        if (data.EmployeeWithHoldingTax.ZipCode != null && data.EmployeeWithHoldingTax.ZipCode != "undefined")
                        {
                            employeewithholdtax.ZipCode = data.EmployeeWithHoldingTax.ZipCode;
                        }
                        if (data.EmployeeWithHoldingTax.NameDiffSsn != null && data.EmployeeWithHoldingTax.NameDiffSsn != "undefined")
                        {
                            employeewithholdtax.NameDiffSsn = data.EmployeeWithHoldingTax.NameDiffSsn;
                        }
                        if (data.EmployeeWithHoldingTax.Date != null)
                        {
                            employeewithholdtax.Date = data.EmployeeWithHoldingTax.Date;
                        }
                        if (data.EmployeeWithHoldingTax.EmployeeSignature != null && data.EmployeeWithHoldingTax.EmployeeSignature != "undefined")
                        {
                            employeewithholdtax.EmployeeSignature = data.EmployeeWithHoldingTax.EmployeeSignature;
                        }
                        if (data.EmployeeWithHoldingTax.Country != null && data.EmployeeWithHoldingTax.Country != "undefined")
                        {
                            employeewithholdtax.Country = data.EmployeeWithHoldingTax.Country;
                        }
                        if (data.EmployeeWithHoldingTax.NoofAllowances != null && data.EmployeeWithHoldingTax.NoofAllowances != "undefined")
                        {
                            employeewithholdtax.NoofAllowances = data.EmployeeWithHoldingTax.NoofAllowances;
                        }
                        if (data.EmployeeWithHoldingTax.AdditionalAmount != null && data.EmployeeWithHoldingTax.AdditionalAmount != "undefined")
                        {
                            employeewithholdtax.AdditionalAmount = data.EmployeeWithHoldingTax.AdditionalAmount;
                        }
                        if (data.EmployeeWithHoldingTax.NoTaxLaibility != null)
                        {
                            employeewithholdtax.NoTaxLaibility = data.EmployeeWithHoldingTax.NoTaxLaibility;
                        }
                        if (data.EmployeeWithHoldingTax.MilatrySpouseExempt != null)
                        {
                            employeewithholdtax.MilatrySpouseExempt = data.EmployeeWithHoldingTax.MilatrySpouseExempt;
                        }
                        if (data.EmployeeWithHoldingTax.Domiciled != null)
                        {
                            employeewithholdtax.Domiciled = data.EmployeeWithHoldingTax.Domiciled;
                        }
                        if (data.EmployeeWithHoldingTax.NoExemptions != null)
                        {
                            employeewithholdtax.NoExemptions = data.EmployeeWithHoldingTax.NoExemptions;
                        }
                        if (data.EmployeeWithHoldingTax.EmployerWithHold != null)
                        {
                            employeewithholdtax.EmployerWithHold = data.EmployeeWithHoldingTax.EmployerWithHold;
                        }

                        if (data.EmployeeWithHoldingTax.AdditionalAmount != null && data.EmployeeWithHoldingTax.AdditionalAmount != "undefined")
                        {
                            employeewithholdtax.AdditionalAmount = data.EmployeeWithHoldingTax.AdditionalAmount;
                        }
                        await db.EmployeeWithHoldingTaxes.AddAsync(employeewithholdtax);
                        await db.SaveChangesAsync();
                    }
                }
                var isemploymenteligibilityverification = db.EmploymentEligibilityVerifications.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemploymenteligibilityverification)
                {
                    var employmenteligibilityverification = db.EmploymentEligibilityVerifications.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                    if (employmenteligibilityverification != null)
                    {
                        if (data.EmploymentEligibilityVerification.FirstName != null && data.EmploymentEligibilityVerification.FirstName != "undefined")
                        {
                            employmenteligibilityverification.FirstName = data.EmploymentEligibilityVerification.FirstName;
                        }
                        if (data.EmploymentEligibilityVerification.MiddleName != null && data.EmploymentEligibilityVerification.MiddleName != "undefined")
                        {
                            employmenteligibilityverification.MiddleName = data.EmploymentEligibilityVerification.MiddleName;
                        }
                        if (data.EmploymentEligibilityVerification.LastName != null && data.EmploymentEligibilityVerification.LastName != "undefined")
                        {
                            employmenteligibilityverification.LastName = data.EmploymentEligibilityVerification.LastName;
                        }
                        if (data.EmploymentEligibilityVerification.OtherNames != null && data.EmploymentEligibilityVerification.OtherNames != "undefined")
                        {
                            employmenteligibilityverification.OtherNames = data.EmploymentEligibilityVerification.OtherNames;
                        }
                        if (data.EmploymentEligibilityVerification.Address != null && data.EmploymentEligibilityVerification.Address != "undefined")
                        {
                            employmenteligibilityverification.Address = data.EmploymentEligibilityVerification.Address;
                        }
                        if (data.EmploymentEligibilityVerification.AptNumber != null && data.EmploymentEligibilityVerification.AptNumber != "undefined")
                        {
                            employmenteligibilityverification.AptNumber = data.EmploymentEligibilityVerification.AptNumber;
                        }
                        if (data.EmploymentEligibilityVerification.City != null && data.EmploymentEligibilityVerification.City != "undefined")
                        {
                            employmenteligibilityverification.City = data.EmploymentEligibilityVerification.City;
                        }
                        if (data.EmploymentEligibilityVerification.State != null && data.EmploymentEligibilityVerification.State != "undefined")
                        {
                            employmenteligibilityverification.State = data.EmploymentEligibilityVerification.State;
                        }
                        if (data.EmploymentEligibilityVerification.ZipCode != null && data.EmploymentEligibilityVerification.ZipCode != "undefined")
                        {
                            employmenteligibilityverification.ZipCode = data.EmploymentEligibilityVerification.ZipCode;
                        }
                        if (data.EmploymentEligibilityVerification.Dob != null)
                        {
                            employmenteligibilityverification.Dob = data.EmploymentEligibilityVerification.Dob;
                        }
                        if (data.EmploymentEligibilityVerification.Ssn != null && data.EmploymentEligibilityVerification.Ssn != "undefined")
                        {
                            employmenteligibilityverification.Ssn = data.EmploymentEligibilityVerification.Ssn;
                        }
                        if (data.EmploymentEligibilityVerification.Email != null && data.EmploymentEligibilityVerification.Email != "undefined")
                        {
                            employmenteligibilityverification.Email = data.EmploymentEligibilityVerification.Email;
                        }
                        if (data.EmploymentEligibilityVerification.Phone != null && data.EmploymentEligibilityVerification.Phone != "undefined")
                        {
                            employmenteligibilityverification.Phone = data.EmploymentEligibilityVerification.Phone;
                        }
                        if (data.EmploymentEligibilityVerification.NoCitizen != null)
                        {
                            employmenteligibilityverification.NoCitizen = data.EmploymentEligibilityVerification.NoCitizen;
                        }
                        if (data.EmploymentEligibilityVerification.Lawful != null)
                        {
                            employmenteligibilityverification.Lawful = data.EmploymentEligibilityVerification.Lawful;
                        }
                        if (data.EmploymentEligibilityVerification.LawfulNumber != null && data.EmploymentEligibilityVerification.LawfulNumber != "undefined")
                        {
                            employmenteligibilityverification.LawfulNumber = data.EmploymentEligibilityVerification.LawfulNumber;
                        }
                        if (data.EmploymentEligibilityVerification.AllienAuthorized != null)
                        {
                            employmenteligibilityverification.AllienAuthorized = data.EmploymentEligibilityVerification.AllienAuthorized;
                        }
                        if (data.EmploymentEligibilityVerification.AllienAuthorizedNumber != null && data.EmploymentEligibilityVerification.AllienAuthorizedNumber != "undefined")
                        {
                            employmenteligibilityverification.AllienAuthorizedNumber = data.EmploymentEligibilityVerification.AllienAuthorizedNumber;
                        }
                        if (data.EmploymentEligibilityVerification.AllienRegNumber != null && data.EmploymentEligibilityVerification.AllienRegNumber != "undefined")
                        {
                            employmenteligibilityverification.AllienRegNumber = data.EmploymentEligibilityVerification.AllienRegNumber;
                        }
                        if (data.EmploymentEligibilityVerification.ForeignPassportNumber != null && data.EmploymentEligibilityVerification.ForeignPassportNumber != "undefined")
                        {
                            employmenteligibilityverification.ForeignPassportNumber = data.EmploymentEligibilityVerification.ForeignPassportNumber;
                        }
                        if (data.EmploymentEligibilityVerification.EmployeeSignature != null && data.EmploymentEligibilityVerification.EmployeeSignature != "undefined")
                        {
                            employmenteligibilityverification.EmployeeSignature = data.EmploymentEligibilityVerification.EmployeeSignature;
                        }
                        if (data.EmploymentEligibilityVerification.Date != null)
                        {
                            employmenteligibilityverification.Date = data.EmploymentEligibilityVerification.Date;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareDate != null)
                        {
                            employmenteligibilityverification.PrepareDate = data.EmploymentEligibilityVerification.PrepareDate;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareFirstName != null && data.EmploymentEligibilityVerification.PrepareFirstName != "undefined")
                        {
                            employmenteligibilityverification.PrepareFirstName = data.EmploymentEligibilityVerification.PrepareFirstName;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareLastName != null && data.EmploymentEligibilityVerification.PrepareLastName != "undefined")
                        {
                            employmenteligibilityverification.PrepareLastName = data.EmploymentEligibilityVerification.PrepareLastName;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareAddress != null && data.EmploymentEligibilityVerification.PrepareAddress != "undefined")
                        {
                            employmenteligibilityverification.PrepareAddress = data.EmploymentEligibilityVerification.PrepareAddress;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareCity != null && data.EmploymentEligibilityVerification.PrepareCity != "undefined")
                        {
                            employmenteligibilityverification.PrepareCity = data.EmploymentEligibilityVerification.PrepareCity;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareState != null && data.EmploymentEligibilityVerification.PrepareState != "undefined")
                        {
                            employmenteligibilityverification.PrepareState = data.EmploymentEligibilityVerification.PrepareState;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareZipCode != null && data.EmploymentEligibilityVerification.PrepareZipCode != "undefined")
                        {
                            employmenteligibilityverification.PrepareZipCode = data.EmploymentEligibilityVerification.PrepareZipCode;
                        }
                        if (data.EmploymentEligibilityVerification.Citizen != null)
                        {
                            employmenteligibilityverification.Citizen = data.EmploymentEligibilityVerification.Citizen;
                        }
                        if (data.EmploymentEligibilityVerification.Expirationdate != null)
                        {
                            employmenteligibilityverification.Expirationdate = data.EmploymentEligibilityVerification.Expirationdate;
                        }

                        db.Entry(employmenteligibilityverification).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (data.EmploymentEligibilityVerification != null)
                    {
                        EFCore.Repository.Edmx.EmploymentEligibilityVerification employmenteligibilityverification = new EFCore.Repository.Edmx.EmploymentEligibilityVerification();
                        employmenteligibilityverification.EmployeeId = data.EmployeeId;
                        if (data.EmploymentEligibilityVerification.FirstName != null && data.EmploymentEligibilityVerification.FirstName != "undefined")
                        {
                            employmenteligibilityverification.FirstName = data.EmploymentEligibilityVerification.FirstName;
                        }
                        if (data.EmploymentEligibilityVerification.MiddleName != null && data.EmploymentEligibilityVerification.MiddleName != "undefined")
                        {
                            employmenteligibilityverification.MiddleName = data.EmploymentEligibilityVerification.MiddleName;
                        }
                        if (data.EmploymentEligibilityVerification.LastName != null && data.EmploymentEligibilityVerification.LastName != "undefined")
                        {
                            employmenteligibilityverification.LastName = data.EmploymentEligibilityVerification.LastName;
                        }
                        if (data.EmploymentEligibilityVerification.OtherNames != null && data.EmploymentEligibilityVerification.OtherNames != "undefined")
                        {
                            employmenteligibilityverification.OtherNames = data.EmploymentEligibilityVerification.OtherNames;
                        }
                        if (data.EmploymentEligibilityVerification.Address != null && data.EmploymentEligibilityVerification.Address != "undefined")
                        {
                            employmenteligibilityverification.Address = data.EmploymentEligibilityVerification.Address;
                        }
                        if (data.EmploymentEligibilityVerification.AptNumber != null && data.EmploymentEligibilityVerification.AptNumber != "undefined")
                        {
                            employmenteligibilityverification.AptNumber = data.EmploymentEligibilityVerification.AptNumber;
                        }
                        if (data.EmploymentEligibilityVerification.City != null && data.EmploymentEligibilityVerification.City != "undefined")
                        {
                            employmenteligibilityverification.City = data.EmploymentEligibilityVerification.City;
                        }
                        if (data.EmploymentEligibilityVerification.State != null && data.EmploymentEligibilityVerification.State != "undefined")
                        {
                            employmenteligibilityverification.State = data.EmploymentEligibilityVerification.State;
                        }
                        if (data.EmploymentEligibilityVerification.ZipCode != null && data.EmploymentEligibilityVerification.ZipCode != "undefined")
                        {
                            employmenteligibilityverification.ZipCode = data.EmploymentEligibilityVerification.ZipCode;
                        }
                        if (data.EmploymentEligibilityVerification.Dob != null)
                        {
                            employmenteligibilityverification.Dob = data.EmploymentEligibilityVerification.Dob;
                        }
                        if (data.EmploymentEligibilityVerification.Ssn != null && data.EmploymentEligibilityVerification.Ssn != "undefined")
                        {
                            employmenteligibilityverification.Ssn = data.EmploymentEligibilityVerification.Ssn;
                        }
                        if (data.EmploymentEligibilityVerification.Email != null && data.EmploymentEligibilityVerification.Email != "undefined")
                        {
                            employmenteligibilityverification.Email = data.EmploymentEligibilityVerification.Email;
                        }
                        if (data.EmploymentEligibilityVerification.Phone != null && data.EmploymentEligibilityVerification.Phone != "undefined")
                        {
                            employmenteligibilityverification.Phone = data.EmploymentEligibilityVerification.Phone;
                        }
                        if (data.EmploymentEligibilityVerification.NoCitizen != null)
                        {
                            employmenteligibilityverification.NoCitizen = data.EmploymentEligibilityVerification.NoCitizen;
                        }
                        if (data.EmploymentEligibilityVerification.Lawful != null)
                        {
                            employmenteligibilityverification.Lawful = data.EmploymentEligibilityVerification.Lawful;
                        }
                        if (data.EmploymentEligibilityVerification.LawfulNumber != null && data.EmploymentEligibilityVerification.LawfulNumber != "undefined")
                        {
                            employmenteligibilityverification.LawfulNumber = data.EmploymentEligibilityVerification.LawfulNumber;
                        }
                        if (data.EmploymentEligibilityVerification.AllienAuthorized != null)
                        {
                            employmenteligibilityverification.AllienAuthorized = data.EmploymentEligibilityVerification.AllienAuthorized;
                        }
                        if (data.EmploymentEligibilityVerification.AllienAuthorizedNumber != null && data.EmploymentEligibilityVerification.AllienAuthorizedNumber != "undefined")
                        {
                            employmenteligibilityverification.AllienAuthorizedNumber = data.EmploymentEligibilityVerification.AllienAuthorizedNumber;
                        }
                        if (data.EmploymentEligibilityVerification.AllienRegNumber != null && data.EmploymentEligibilityVerification.AllienRegNumber != "undefined")
                        {
                            employmenteligibilityverification.AllienRegNumber = data.EmploymentEligibilityVerification.AllienRegNumber;
                        }
                        if (data.EmploymentEligibilityVerification.ForeignPassportNumber != null && data.EmploymentEligibilityVerification.ForeignPassportNumber != "undefined")
                        {
                            employmenteligibilityverification.ForeignPassportNumber = data.EmploymentEligibilityVerification.ForeignPassportNumber;
                        }
                        if (data.EmploymentEligibilityVerification.EmployeeSignature != null && data.EmploymentEligibilityVerification.EmployeeSignature != "undefined")
                        {
                            employmenteligibilityverification.EmployeeSignature = data.EmploymentEligibilityVerification.EmployeeSignature;
                        }
                        if (data.EmploymentEligibilityVerification.Date != null)
                        {
                            employmenteligibilityverification.Date = data.EmploymentEligibilityVerification.Date;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareDate != null)
                        {
                            employmenteligibilityverification.PrepareDate = data.EmploymentEligibilityVerification.PrepareDate;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareFirstName != null && data.EmploymentEligibilityVerification.PrepareFirstName != "undefined")
                        {
                            employmenteligibilityverification.PrepareFirstName = data.EmploymentEligibilityVerification.PrepareFirstName;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareLastName != null && data.EmploymentEligibilityVerification.PrepareLastName != "undefined")
                        {
                            employmenteligibilityverification.PrepareLastName = data.EmploymentEligibilityVerification.PrepareLastName;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareAddress != null && data.EmploymentEligibilityVerification.PrepareAddress != "undefined")
                        {
                            employmenteligibilityverification.PrepareAddress = data.EmploymentEligibilityVerification.PrepareAddress;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareCity != null && data.EmploymentEligibilityVerification.PrepareCity != "undefined")
                        {
                            employmenteligibilityverification.PrepareCity = data.EmploymentEligibilityVerification.PrepareCity;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareState != null && data.EmploymentEligibilityVerification.PrepareState != "undefined")
                        {
                            employmenteligibilityverification.PrepareState = data.EmploymentEligibilityVerification.PrepareState;
                        }
                        if (data.EmploymentEligibilityVerification.PrepareZipCode != null && data.EmploymentEligibilityVerification.PrepareZipCode != "undefined")
                        {
                            employmenteligibilityverification.PrepareZipCode = data.EmploymentEligibilityVerification.PrepareZipCode;
                        }
                        if (data.EmploymentEligibilityVerification.Citizen != null)
                        {
                            employmenteligibilityverification.Citizen = data.EmploymentEligibilityVerification.Citizen;
                        }
                        if (data.EmploymentEligibilityVerification.Expirationdate != null)
                        {
                            employmenteligibilityverification.Expirationdate = data.EmploymentEligibilityVerification.Expirationdate;
                        }
                        await db.EmploymentEligibilityVerifications.AddAsync(employmenteligibilityverification);
                        await db.SaveChangesAsync();
                    }
                }
                var isemployeeauthorizedrepresentative = db.EmployeeAuthorizedRepresentatives.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeeauthorizedrepresentative)
                {
                    var employeeauthorizedrepresentative = db.EmployeeAuthorizedRepresentatives.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                    if (employeeauthorizedrepresentative != null)
                    {
                        if (data.employeeAuthorizedRepresentative.FirstName != null && data.employeeAuthorizedRepresentative.FirstName != "undefined")
                        {
                            employeeauthorizedrepresentative.FirstName = data.employeeAuthorizedRepresentative.FirstName;
                        }
                        if (data.employeeAuthorizedRepresentative.MiddleName != null && data.employeeAuthorizedRepresentative.MiddleName != "undefined")
                        {
                            employeeauthorizedrepresentative.MiddleName = data.employeeAuthorizedRepresentative.MiddleName;
                        }
                        if (data.employeeAuthorizedRepresentative.LastName != null && data.employeeAuthorizedRepresentative.LastName != "undefined")
                        {
                            employeeauthorizedrepresentative.LastName = data.employeeAuthorizedRepresentative.LastName;
                        }
                        if (data.employeeAuthorizedRepresentative.DocumentTitle != null && data.employeeAuthorizedRepresentative.DocumentTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.DocumentTitle = data.employeeAuthorizedRepresentative.DocumentTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.IssuingAuthority != null && data.employeeAuthorizedRepresentative.IssuingAuthority != "undefined")
                        {
                            employeeauthorizedrepresentative.IssuingAuthority = data.employeeAuthorizedRepresentative.IssuingAuthority;
                        }
                        if (data.employeeAuthorizedRepresentative.DocumentNumber != null && data.employeeAuthorizedRepresentative.DocumentNumber != "undefined")
                        {
                            employeeauthorizedrepresentative.DocumentNumber = data.employeeAuthorizedRepresentative.DocumentNumber;
                        }
                        if (data.employeeAuthorizedRepresentative.Signature != null && data.employeeAuthorizedRepresentative.Signature != "undefined")
                        {
                            employeeauthorizedrepresentative.Signature = data.employeeAuthorizedRepresentative.Signature;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpTitle != null && data.employeeAuthorizedRepresentative.EmpTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpTitle = data.employeeAuthorizedRepresentative.EmpTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.OrgName != null && data.employeeAuthorizedRepresentative.OrgName != "undefined")
                        {
                            employeeauthorizedrepresentative.OrgName = data.employeeAuthorizedRepresentative.OrgName;
                        }
                        if (data.employeeAuthorizedRepresentative.OrgAddress != null && data.employeeAuthorizedRepresentative.OrgAddress != "undefined")
                        {
                            employeeauthorizedrepresentative.OrgAddress = data.employeeAuthorizedRepresentative.OrgAddress;
                        }
                        if (data.employeeAuthorizedRepresentative.City != null && data.employeeAuthorizedRepresentative.City != "undefined")
                        {
                            employeeauthorizedrepresentative.City = data.employeeAuthorizedRepresentative.City;
                        }
                        if (data.employeeAuthorizedRepresentative.State != null && data.employeeAuthorizedRepresentative.State != "undefined")
                        {
                            employeeauthorizedrepresentative.State = data.employeeAuthorizedRepresentative.State;
                        }
                        if (data.employeeAuthorizedRepresentative.ZipCode != null && data.employeeAuthorizedRepresentative.ZipCode != "undefined")
                        {
                            employeeauthorizedrepresentative.ZipCode = data.employeeAuthorizedRepresentative.ZipCode;
                        }
                        if (data.employeeAuthorizedRepresentative.ExpirationDate != null)
                        {
                            employeeauthorizedrepresentative.ExpirationDate = data.employeeAuthorizedRepresentative.ExpirationDate;
                        }
                        if (data.employeeAuthorizedRepresentative.Date != null)
                        {
                            employeeauthorizedrepresentative.Date = data.employeeAuthorizedRepresentative.Date;
                        }
                        if (data.employeeAuthorizedRepresentative.FirstDate != null)
                        {
                            employeeauthorizedrepresentative.FirstDate = data.employeeAuthorizedRepresentative.FirstDate;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle != null && data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthDocumentTitle = data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority != null && data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthIssuingAuthority = data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber != null && data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthDocumentNumber = data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthExpirationDate != null)
                        {
                            employeeauthorizedrepresentative.EmpAuthExpirationDate = data.employeeAuthorizedRepresentative.EmpAuthExpirationDate;
                        }

                        db.Entry(employeeauthorizedrepresentative).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (data.employeeAuthorizedRepresentative != null)
                    {
                        EFCore.Repository.Edmx.EmployeeAuthorizedRepresentative employeeauthorizedrepresentative = new EFCore.Repository.Edmx.EmployeeAuthorizedRepresentative();
                        employeeauthorizedrepresentative.EmployeeId = data.EmployeeId;
                        if (data.employeeAuthorizedRepresentative.FirstName != null && data.employeeAuthorizedRepresentative.FirstName != "undefined")
                        {
                            employeeauthorizedrepresentative.FirstName = data.employeeAuthorizedRepresentative.FirstName;
                        }
                        if (data.employeeAuthorizedRepresentative.MiddleName != null && data.employeeAuthorizedRepresentative.MiddleName != "undefined")
                        {
                            employeeauthorizedrepresentative.MiddleName = data.employeeAuthorizedRepresentative.MiddleName;
                        }
                        if (data.employeeAuthorizedRepresentative.LastName != null && data.employeeAuthorizedRepresentative.LastName != "undefined")
                        {
                            employeeauthorizedrepresentative.LastName = data.employeeAuthorizedRepresentative.LastName;
                        }
                        if (data.employeeAuthorizedRepresentative.DocumentTitle != null && data.employeeAuthorizedRepresentative.DocumentTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.DocumentTitle = data.employeeAuthorizedRepresentative.DocumentTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.IssuingAuthority != null && data.employeeAuthorizedRepresentative.IssuingAuthority != "undefined")
                        {
                            employeeauthorizedrepresentative.IssuingAuthority = data.employeeAuthorizedRepresentative.IssuingAuthority;
                        }
                        if (data.employeeAuthorizedRepresentative.DocumentNumber != null && data.employeeAuthorizedRepresentative.DocumentNumber != "undefined")
                        {
                            employeeauthorizedrepresentative.DocumentNumber = data.employeeAuthorizedRepresentative.DocumentNumber;
                        }
                        if (data.employeeAuthorizedRepresentative.Signature != null && data.employeeAuthorizedRepresentative.Signature != "undefined")
                        {
                            employeeauthorizedrepresentative.Signature = data.employeeAuthorizedRepresentative.Signature;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpTitle != null && data.employeeAuthorizedRepresentative.EmpTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpTitle = data.employeeAuthorizedRepresentative.EmpTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.OrgName != null && data.employeeAuthorizedRepresentative.OrgName != "undefined")
                        {
                            employeeauthorizedrepresentative.OrgName = data.employeeAuthorizedRepresentative.OrgName;
                        }
                        if (data.employeeAuthorizedRepresentative.OrgAddress != null && data.employeeAuthorizedRepresentative.OrgAddress != "undefined")
                        {
                            employeeauthorizedrepresentative.OrgAddress = data.employeeAuthorizedRepresentative.OrgAddress;
                        }
                        if (data.employeeAuthorizedRepresentative.City != null && data.employeeAuthorizedRepresentative.City != "undefined")
                        {
                            employeeauthorizedrepresentative.City = data.employeeAuthorizedRepresentative.City;
                        }
                        if (data.employeeAuthorizedRepresentative.State != null && data.employeeAuthorizedRepresentative.State != "undefined")
                        {
                            employeeauthorizedrepresentative.State = data.employeeAuthorizedRepresentative.State;
                        }
                        if (data.employeeAuthorizedRepresentative.ZipCode != null && data.employeeAuthorizedRepresentative.ZipCode != "undefined")
                        {
                            employeeauthorizedrepresentative.ZipCode = data.employeeAuthorizedRepresentative.ZipCode;
                        }
                        if (data.employeeAuthorizedRepresentative.ExpirationDate != null)
                        {
                            employeeauthorizedrepresentative.ExpirationDate = data.employeeAuthorizedRepresentative.ExpirationDate;
                        }
                        if (data.employeeAuthorizedRepresentative.Date != null)
                        {
                            employeeauthorizedrepresentative.Date = data.employeeAuthorizedRepresentative.Date;
                        }
                        if (data.employeeAuthorizedRepresentative.FirstDate != null)
                        {
                            employeeauthorizedrepresentative.FirstDate = data.employeeAuthorizedRepresentative.FirstDate;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle != null && data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthDocumentTitle = data.employeeAuthorizedRepresentative.EmpAuthDocumentTitle;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority != null && data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthIssuingAuthority = data.employeeAuthorizedRepresentative.EmpAuthIssuingAuthority;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber != null && data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber != "undefined")
                        {
                            employeeauthorizedrepresentative.EmpAuthDocumentNumber = data.employeeAuthorizedRepresentative.EmpAuthDocumentNumber;
                        }
                        if (data.employeeAuthorizedRepresentative.EmpAuthExpirationDate != null)
                        {
                            employeeauthorizedrepresentative.EmpAuthExpirationDate = data.employeeAuthorizedRepresentative.EmpAuthExpirationDate;
                        }

                        await db.EmployeeAuthorizedRepresentatives.AddAsync(employeeauthorizedrepresentative);
                        await db.SaveChangesAsync();
                    }
                }
                var isemployeereverificationandrehires = db.EmployeeReverificationAndRehires.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeereverificationandrehires)
                {
                    var employeereverificationandrehires = db.EmployeeReverificationAndRehires.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                    if (employeereverificationandrehires != null)
                    {
                        if (data.EmployeeReverificationAndRehire.NewName != null && data.EmployeeReverificationAndRehire.NewName != "undefined")
                        {
                            employeereverificationandrehires.NewName = data.EmployeeReverificationAndRehire.NewName;
                        }
                        if (data.EmployeeReverificationAndRehire.FirstName != null && data.EmployeeReverificationAndRehire.FirstName != "undefined")
                        {
                            employeereverificationandrehires.FirstName = data.EmployeeReverificationAndRehire.FirstName;
                        }
                        if (data.EmployeeReverificationAndRehire.MiddleName != null && data.EmployeeReverificationAndRehire.MiddleName != "undefined")
                        {
                            employeereverificationandrehires.MiddleName = data.EmployeeReverificationAndRehire.MiddleName;
                        }
                        if (data.EmployeeReverificationAndRehire.LastName != null && data.EmployeeReverificationAndRehire.LastName != "undefined")
                        {
                            employeereverificationandrehires.LastName = data.EmployeeReverificationAndRehire.LastName;
                        }
                        if (data.EmployeeReverificationAndRehire.DocumentTitle != null && data.EmployeeReverificationAndRehire.DocumentTitle != "undefined")
                        {
                            employeereverificationandrehires.DocumentTitle = data.EmployeeReverificationAndRehire.DocumentTitle;
                        }
                        if (data.EmployeeReverificationAndRehire.DocumentNumber != null && data.EmployeeReverificationAndRehire.DocumentNumber != "undefined")
                        {
                            employeereverificationandrehires.DocumentNumber = data.EmployeeReverificationAndRehire.DocumentNumber;
                        }
                        if (data.EmployeeReverificationAndRehire.Signature != null && data.EmployeeReverificationAndRehire.Signature != "undefined")
                        {
                            employeereverificationandrehires.Signature = data.EmployeeReverificationAndRehire.Signature;
                        }
                        if (data.EmployeeReverificationAndRehire.EmpName != null && data.EmployeeReverificationAndRehire.EmpName != "undefined")
                        {
                            employeereverificationandrehires.EmpName = data.EmployeeReverificationAndRehire.EmpName;
                        }
                        if (data.EmployeeReverificationAndRehire.DateofRehire != null)
                        {
                            employeereverificationandrehires.DateofRehire = data.EmployeeReverificationAndRehire.DateofRehire;
                        }
                        if (data.EmployeeReverificationAndRehire.Date != null)
                        {
                            employeereverificationandrehires.Date = data.EmployeeReverificationAndRehire.Date;
                        }
                        if (data.EmployeeReverificationAndRehire.ExpirationDate != null)
                        {
                            employeereverificationandrehires.ExpirationDate = data.EmployeeReverificationAndRehire.ExpirationDate;
                        }

                        db.Entry(employeereverificationandrehires).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (data.EmployeeReverificationAndRehire != null)
                    {
                        EFCore.Repository.Edmx.EmployeeReverificationAndRehire employeereverificationandrehires = new EFCore.Repository.Edmx.EmployeeReverificationAndRehire();
                        employeereverificationandrehires.EmployeeId = data.EmployeeId;
                        if (data.EmployeeReverificationAndRehire.NewName != null && data.EmployeeReverificationAndRehire.NewName != "undefined")
                        {
                            employeereverificationandrehires.NewName = data.EmployeeReverificationAndRehire.NewName;
                        }
                        if (data.EmployeeReverificationAndRehire.FirstName != null && data.EmployeeReverificationAndRehire.FirstName != "undefined")
                        {
                            employeereverificationandrehires.FirstName = data.EmployeeReverificationAndRehire.FirstName;
                        }
                        if (data.EmployeeReverificationAndRehire.MiddleName != null && data.EmployeeReverificationAndRehire.MiddleName != "undefined")
                        {
                            employeereverificationandrehires.MiddleName = data.EmployeeReverificationAndRehire.MiddleName;
                        }
                        if (data.EmployeeReverificationAndRehire.LastName != null && data.EmployeeReverificationAndRehire.LastName != "undefined")
                        {
                            employeereverificationandrehires.LastName = data.EmployeeReverificationAndRehire.LastName;
                        }
                        if (data.EmployeeReverificationAndRehire.DocumentTitle != null && data.EmployeeReverificationAndRehire.DocumentTitle != "undefined")
                        {
                            employeereverificationandrehires.DocumentTitle = data.EmployeeReverificationAndRehire.DocumentTitle;
                        }
                        if (data.EmployeeReverificationAndRehire.DocumentNumber != null && data.EmployeeReverificationAndRehire.DocumentNumber != "undefined")
                        {
                            employeereverificationandrehires.DocumentNumber = data.EmployeeReverificationAndRehire.DocumentNumber;
                        }
                        if (data.EmployeeReverificationAndRehire.Signature != null && data.EmployeeReverificationAndRehire.Signature != "undefined")
                        {
                            employeereverificationandrehires.Signature = data.EmployeeReverificationAndRehire.Signature;
                        }
                        if (data.EmployeeReverificationAndRehire.EmpName != null && data.EmployeeReverificationAndRehire.EmpName != "undefined")
                        {
                            employeereverificationandrehires.EmpName = data.EmployeeReverificationAndRehire.EmpName;
                        }
                        if (data.EmployeeReverificationAndRehire.DateofRehire != null)
                        {
                            employeereverificationandrehires.DateofRehire = data.EmployeeReverificationAndRehire.DateofRehire;
                        }
                        if (data.EmployeeReverificationAndRehire.Date != null)
                        {
                            employeereverificationandrehires.Date = data.EmployeeReverificationAndRehire.Date;
                        }
                        if (data.EmployeeReverificationAndRehire.ExpirationDate != null)
                        {
                            employeereverificationandrehires.ExpirationDate = data.EmployeeReverificationAndRehire.ExpirationDate;
                        }

                        await db.EmployeeReverificationAndRehires.AddAsync(employeereverificationandrehires);
                        await db.SaveChangesAsync();
                    }
                }
                var isemployeeddauthorization = db.EmployeeDdauthorizations.ToList().Exists(x => x.EmployeeId == data.EmployeeId);
                if (isemployeeddauthorization)
                {
                    var employeeddauthorization = db.EmployeeDdauthorizations.ToList().Where(x => x.EmployeeId == data.EmployeeId).FirstOrDefault();
                    if (employeeddauthorization != null)
                    {
                        if (data.EmployeeDdauthorization.Authorize != null)
                        {
                            employeeddauthorization.Authorize = data.EmployeeDdauthorization.Authorize;
                        }
                        if (data.EmployeeDdauthorization.Revise != null)
                        {
                            employeeddauthorization.Revise = data.EmployeeDdauthorization.Revise;
                        }
                        if (data.EmployeeDdauthorization.Cancel != null)
                        {
                            employeeddauthorization.Cancel = data.EmployeeDdauthorization.Cancel;
                        }
                        if (data.EmployeeDdauthorization.Signature != null && data.EmployeeDdauthorization.Signature != "undefined")
                        {
                            employeeddauthorization.Signature = data.EmployeeDdauthorization.Signature;
                        }
                        if (data.EmployeeDdauthorization.BankNameOne != null && data.EmployeeDdauthorization.BankNameOne != "undefined")
                        {
                            employeeddauthorization.BankNameOne = data.EmployeeDdauthorization.BankNameOne;
                        }
                        if (data.EmployeeDdauthorization.BankNameTwo != null && data.EmployeeDdauthorization.BankNameTwo != "undefined")
                        {
                            employeeddauthorization.BankNameTwo = data.EmployeeDdauthorization.BankNameTwo;
                        }
                        if (data.EmployeeDdauthorization.BankNameThree != null && data.EmployeeDdauthorization.BankNameThree != "undefined")
                        {
                            employeeddauthorization.BankNameThree = data.EmployeeDdauthorization.BankNameThree;
                        }
                        if (data.EmployeeDdauthorization.AddressOne != null && data.EmployeeDdauthorization.AddressOne != "undefined")
                        {
                            employeeddauthorization.AddressOne = data.EmployeeDdauthorization.AddressOne;
                        }
                        if (data.EmployeeDdauthorization.AddressTwo != null && data.EmployeeDdauthorization.AddressTwo != "undefined")
                        {
                            employeeddauthorization.AddressTwo = data.EmployeeDdauthorization.AddressTwo;
                        }
                        if (data.EmployeeDdauthorization.AddressThree != null && data.EmployeeDdauthorization.AddressThree != "undefined")
                        {
                            employeeddauthorization.AddressThree = data.EmployeeDdauthorization.AddressThree;
                        }
                        if (data.EmployeeDdauthorization.PhoneOne != null && data.EmployeeDdauthorization.PhoneOne != "undefined")
                        {
                            employeeddauthorization.PhoneOne = data.EmployeeDdauthorization.PhoneOne;
                        }
                        if (data.EmployeeDdauthorization.PhoneTwo != null && data.EmployeeDdauthorization.PhoneTwo != "undefined")
                        {
                            employeeddauthorization.PhoneTwo = data.EmployeeDdauthorization.PhoneTwo;
                        }
                        if (data.EmployeeDdauthorization.PhoneThree != null && data.EmployeeDdauthorization.PhoneThree != "undefined")
                        {
                            employeeddauthorization.PhoneThree = data.EmployeeDdauthorization.PhoneThree;
                        }
                        if (data.EmployeeDdauthorization.AccTypeOne != null && data.EmployeeDdauthorization.AccTypeOne != "undefined")
                        {
                            employeeddauthorization.AccTypeOne = data.EmployeeDdauthorization.AccTypeOne;
                        }
                        if (data.EmployeeDdauthorization.AccTypeTwo != null && data.EmployeeDdauthorization.AccTypeTwo != "undefined")
                        {
                            employeeddauthorization.AccTypeTwo = data.EmployeeDdauthorization.AccTypeTwo;
                        }
                        if (data.EmployeeDdauthorization.AccTypeThree != null && data.EmployeeDdauthorization.AccTypeThree != "undefined")
                        {
                            employeeddauthorization.AccTypeThree = data.EmployeeDdauthorization.AccTypeThree;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingOne != null && data.EmployeeDdauthorization.BankRoutingOne != "undefined")
                        {
                            employeeddauthorization.BankRoutingOne = data.EmployeeDdauthorization.BankRoutingOne;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingTwo != null && data.EmployeeDdauthorization.BankRoutingTwo != "undefined")
                        {
                            employeeddauthorization.BankRoutingTwo = data.EmployeeDdauthorization.BankRoutingTwo;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingThree != null && data.EmployeeDdauthorization.BankRoutingThree != "undefined")
                        {
                            employeeddauthorization.BankRoutingThree = data.EmployeeDdauthorization.BankRoutingThree;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberOne != null && data.EmployeeDdauthorization.BankAccNumberOne != "undefined")
                        {
                            employeeddauthorization.BankAccNumberOne = data.EmployeeDdauthorization.BankAccNumberOne;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberTwo != null && data.EmployeeDdauthorization.BankAccNumberTwo != "undefined")
                        {
                            employeeddauthorization.BankAccNumberTwo = data.EmployeeDdauthorization.BankAccNumberTwo;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberThree != null && data.EmployeeDdauthorization.BankAccNumberThree != "undefined")
                        {
                            employeeddauthorization.BankAccNumberThree = data.EmployeeDdauthorization.BankAccNumberThree;
                        }
                        if (data.EmployeeDdauthorization.BankAmountOne != null && data.EmployeeDdauthorization.BankAmountOne != "undefined")
                        {
                            employeeddauthorization.BankAmountOne = data.EmployeeDdauthorization.BankAmountOne;
                        }
                        if (data.EmployeeDdauthorization.BankAmountTwo != null && data.EmployeeDdauthorization.BankAmountTwo != "undefined")
                        {
                            employeeddauthorization.BankAmountTwo = data.EmployeeDdauthorization.BankAmountTwo;
                        }
                        if (data.EmployeeDdauthorization.BankAmountThree != null && data.EmployeeDdauthorization.BankAmountThree != "undefined")
                        {
                            employeeddauthorization.BankAmountThree = data.EmployeeDdauthorization.BankAmountThree;
                        }
                        if (data.EmployeeDdauthorization.PctOne != null && data.EmployeeDdauthorization.PctOne != "undefined")
                        {
                            employeeddauthorization.PctOne = data.EmployeeDdauthorization.PctOne;
                        }
                        if (data.EmployeeDdauthorization.PctTwo != null && data.EmployeeDdauthorization.PctTwo != "undefined")
                        {
                            employeeddauthorization.PctTwo = data.EmployeeDdauthorization.PctTwo;
                        }
                        if (data.EmployeeDdauthorization.PctThree != null && data.EmployeeDdauthorization.PctThree != "undefined")
                        {
                            employeeddauthorization.PctThree = data.EmployeeDdauthorization.PctThree;
                        }
                        if (data.EmployeeDdauthorization.Total != null && data.EmployeeDdauthorization.Total != "undefined")
                        {
                            employeeddauthorization.Total = data.EmployeeDdauthorization.Total;
                        }
                        if (data.EmployeeDdauthorization.Bank != null && data.EmployeeDdauthorization.Bank != "undefined")
                        {
                            employeeddauthorization.Bank = data.EmployeeDdauthorization.Bank;
                        }
                        if (data.EmployeeDdauthorization.BankName != null && data.EmployeeDdauthorization.BankName != "undefined")
                        {
                            employeeddauthorization.BankName = data.EmployeeDdauthorization.BankName;
                        }
                        if (data.EmployeeDdauthorization.PayTo != null && data.EmployeeDdauthorization.PayTo != "undefined")
                        {
                            employeeddauthorization.PayTo = data.EmployeeDdauthorization.PayTo;
                        }
                        if (data.EmployeeDdauthorization.Memo != null && data.EmployeeDdauthorization.Memo != "undefined")
                        {
                            employeeddauthorization.Memo = data.EmployeeDdauthorization.Memo;
                        }
                        if (data.EmployeeDdauthorization.Date != null)
                        {
                            employeeddauthorization.Date = data.EmployeeDdauthorization.Date;
                        }
                        if (data.EmployeeDdauthorization.CheckDate != null)
                        {
                            employeeddauthorization.CheckDate = data.EmployeeDdauthorization.CheckDate;
                        }
                        if (data.EmployeeDdauthorization.DepositName != null && data.EmployeeDdauthorization.DepositName != "undefined")
                        {
                            employeeddauthorization.DepositName = data.EmployeeDdauthorization.DepositName;
                        }
                        if (data.EmployeeDdauthorization.EmployerName != null && data.EmployeeDdauthorization.EmployerName != "undefined")
                        {
                            employeeddauthorization.EmployerName = data.EmployeeDdauthorization.EmployerName;
                        }
                        if (data.EmployeeDdauthorization.RemainingBalance != null)
                        {
                            employeeddauthorization.RemainingBalance = data.EmployeeDdauthorization.RemainingBalance;
                        }
                        if (data.EmployeeDdauthorization.UsePercentage != null)
                        {
                            employeeddauthorization.UsePercentage = data.EmployeeDdauthorization.UsePercentage;
                        }
                        if (data.EmployeeDdauthorization.CheckAmount != null && data.EmployeeDdauthorization.CheckAmount != "undefined")
                        {
                            employeeddauthorization.CheckAmount = data.EmployeeDdauthorization.CheckAmount;
                        }

                        db.Entry(employeeddauthorization).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (data.EmployeeDdauthorization != null)
                    {
                        EFCore.Repository.Edmx.EmployeeDdauthorization employeeddauthorization = new EFCore.Repository.Edmx.EmployeeDdauthorization();
                        employeeddauthorization.EmployeeId = data.EmployeeId;
                        if (data.EmployeeDdauthorization.Authorize != null)
                        {
                            employeeddauthorization.Authorize = data.EmployeeDdauthorization.Authorize;
                        }
                        if (data.EmployeeDdauthorization.Revise != null)
                        {
                            employeeddauthorization.Revise = data.EmployeeDdauthorization.Revise;
                        }
                        if (data.EmployeeDdauthorization.Cancel != null)
                        {
                            employeeddauthorization.Cancel = data.EmployeeDdauthorization.Cancel;
                        }
                        if (data.EmployeeDdauthorization.Signature != null && data.EmployeeDdauthorization.Signature != "undefined")
                        {
                            employeeddauthorization.Signature = data.EmployeeDdauthorization.Signature;
                        }
                        if (data.EmployeeDdauthorization.BankNameOne != null && data.EmployeeDdauthorization.BankNameOne != "undefined")
                        {
                            employeeddauthorization.BankNameOne = data.EmployeeDdauthorization.BankNameOne;
                        }
                        if (data.EmployeeDdauthorization.BankNameTwo != null && data.EmployeeDdauthorization.BankNameTwo != "undefined")
                        {
                            employeeddauthorization.BankNameTwo = data.EmployeeDdauthorization.BankNameTwo;
                        }
                        if (data.EmployeeDdauthorization.BankNameThree != null && data.EmployeeDdauthorization.BankNameThree != "undefined")
                        {
                            employeeddauthorization.BankNameThree = data.EmployeeDdauthorization.BankNameThree;
                        }
                        if (data.EmployeeDdauthorization.AddressOne != null && data.EmployeeDdauthorization.AddressOne != "undefined")
                        {
                            employeeddauthorization.AddressOne = data.EmployeeDdauthorization.AddressOne;
                        }
                        if (data.EmployeeDdauthorization.AddressTwo != null && data.EmployeeDdauthorization.AddressTwo != "undefined")
                        {
                            employeeddauthorization.AddressTwo = data.EmployeeDdauthorization.AddressTwo;
                        }
                        if (data.EmployeeDdauthorization.AddressThree != null && data.EmployeeDdauthorization.AddressThree != "undefined")
                        {
                            employeeddauthorization.AddressThree = data.EmployeeDdauthorization.AddressThree;
                        }
                        if (data.EmployeeDdauthorization.PhoneOne != null && data.EmployeeDdauthorization.PhoneOne != "undefined")
                        {
                            employeeddauthorization.PhoneOne = data.EmployeeDdauthorization.PhoneOne;
                        }
                        if (data.EmployeeDdauthorization.PhoneTwo != null && data.EmployeeDdauthorization.PhoneTwo != "undefined")
                        {
                            employeeddauthorization.PhoneTwo = data.EmployeeDdauthorization.PhoneTwo;
                        }
                        if (data.EmployeeDdauthorization.PhoneThree != null && data.EmployeeDdauthorization.PhoneThree != "undefined")
                        {
                            employeeddauthorization.PhoneThree = data.EmployeeDdauthorization.PhoneThree;
                        }
                        if (data.EmployeeDdauthorization.AccTypeOne != null && data.EmployeeDdauthorization.AccTypeOne != "undefined")
                        {
                            employeeddauthorization.AccTypeOne = data.EmployeeDdauthorization.AccTypeOne;
                        }
                        if (data.EmployeeDdauthorization.AccTypeTwo != null && data.EmployeeDdauthorization.AccTypeTwo != "undefined")
                        {
                            employeeddauthorization.AccTypeTwo = data.EmployeeDdauthorization.AccTypeTwo;
                        }
                        if (data.EmployeeDdauthorization.AccTypeThree != null && data.EmployeeDdauthorization.AccTypeThree != "undefined")
                        {
                            employeeddauthorization.AccTypeThree = data.EmployeeDdauthorization.AccTypeThree;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingOne != null && data.EmployeeDdauthorization.BankRoutingOne != "undefined")
                        {
                            employeeddauthorization.BankRoutingOne = data.EmployeeDdauthorization.BankRoutingOne;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingTwo != null && data.EmployeeDdauthorization.BankRoutingTwo != "undefined")
                        {
                            employeeddauthorization.BankRoutingTwo = data.EmployeeDdauthorization.BankRoutingTwo;
                        }
                        if (data.EmployeeDdauthorization.BankRoutingThree != null && data.EmployeeDdauthorization.BankRoutingThree != "undefined")
                        {
                            employeeddauthorization.BankRoutingThree = data.EmployeeDdauthorization.BankRoutingThree;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberOne != null && data.EmployeeDdauthorization.BankAccNumberOne != "undefined")
                        {
                            employeeddauthorization.BankAccNumberOne = data.EmployeeDdauthorization.BankAccNumberOne;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberTwo != null && data.EmployeeDdauthorization.BankAccNumberTwo != "undefined")
                        {
                            employeeddauthorization.BankAccNumberTwo = data.EmployeeDdauthorization.BankAccNumberTwo;
                        }
                        if (data.EmployeeDdauthorization.BankAccNumberThree != null && data.EmployeeDdauthorization.BankAccNumberThree != "undefined")
                        {
                            employeeddauthorization.BankAccNumberThree = data.EmployeeDdauthorization.BankAccNumberThree;
                        }
                        if (data.EmployeeDdauthorization.BankAmountOne != null && data.EmployeeDdauthorization.BankAmountOne != "undefined")
                        {
                            employeeddauthorization.BankAmountOne = data.EmployeeDdauthorization.BankAmountOne;
                        }
                        if (data.EmployeeDdauthorization.BankAmountTwo != null && data.EmployeeDdauthorization.BankAmountTwo != "undefined")
                        {
                            employeeddauthorization.BankAmountTwo = data.EmployeeDdauthorization.BankAmountTwo;
                        }
                        if (data.EmployeeDdauthorization.BankAmountThree != null && data.EmployeeDdauthorization.BankAmountThree != "undefined")
                        {
                            employeeddauthorization.BankAmountThree = data.EmployeeDdauthorization.BankAmountThree;
                        }
                        if (data.EmployeeDdauthorization.PctOne != null && data.EmployeeDdauthorization.PctOne != "undefined")
                        {
                            employeeddauthorization.PctOne = data.EmployeeDdauthorization.PctOne;
                        }
                        if (data.EmployeeDdauthorization.PctTwo != null && data.EmployeeDdauthorization.PctTwo != "undefined")
                        {
                            employeeddauthorization.PctTwo = data.EmployeeDdauthorization.PctTwo;
                        }
                        if (data.EmployeeDdauthorization.PctThree != null && data.EmployeeDdauthorization.PctThree != "undefined")
                        {
                            employeeddauthorization.PctThree = data.EmployeeDdauthorization.PctThree;
                        }
                        if (data.EmployeeDdauthorization.Total != null && data.EmployeeDdauthorization.Total != "undefined")
                        {
                            employeeddauthorization.Total = data.EmployeeDdauthorization.Total;
                        }
                        if (data.EmployeeDdauthorization.Bank != null && data.EmployeeDdauthorization.Bank != "undefined")
                        {
                            employeeddauthorization.Bank = data.EmployeeDdauthorization.Bank;
                        }
                        if (data.EmployeeDdauthorization.BankName != null && data.EmployeeDdauthorization.BankName != "undefined")
                        {
                            employeeddauthorization.BankName = data.EmployeeDdauthorization.BankName;
                        }
                        if (data.EmployeeDdauthorization.PayTo != null && data.EmployeeDdauthorization.PayTo != "undefined")
                        {
                            employeeddauthorization.PayTo = data.EmployeeDdauthorization.PayTo;
                        }
                        if (data.EmployeeDdauthorization.Memo != null && data.EmployeeDdauthorization.Memo != "undefined")
                        {
                            employeeddauthorization.Memo = data.EmployeeDdauthorization.Memo;
                        }
                        if (data.EmployeeDdauthorization.Date != null)
                        {
                            employeeddauthorization.Date = data.EmployeeDdauthorization.Date;
                        }
                        if (data.EmployeeDdauthorization.CheckDate != null)
                        {
                            employeeddauthorization.CheckDate = data.EmployeeDdauthorization.CheckDate;
                        }
                        if (data.EmployeeDdauthorization.DepositName != null && data.EmployeeDdauthorization.DepositName != "undefined")
                        {
                            employeeddauthorization.DepositName = data.EmployeeDdauthorization.DepositName;
                        }
                        if (data.EmployeeDdauthorization.EmployerName != null && data.EmployeeDdauthorization.EmployerName != "undefined")
                        {
                            employeeddauthorization.EmployerName = data.EmployeeDdauthorization.EmployerName;
                        }
                        if (data.EmployeeDdauthorization.RemainingBalance != null)
                        {
                            employeeddauthorization.RemainingBalance = data.EmployeeDdauthorization.RemainingBalance;
                        }
                        if (data.EmployeeDdauthorization.UsePercentage != null)
                        {
                            employeeddauthorization.UsePercentage = data.EmployeeDdauthorization.UsePercentage;
                        }
                        if (data.EmployeeDdauthorization.CheckAmount != null && data.EmployeeDdauthorization.CheckAmount != "undefined")
                        {
                            employeeddauthorization.CheckAmount = data.EmployeeDdauthorization.CheckAmount;
                        }

                        await db.EmployeeDdauthorizations.AddAsync(employeeddauthorization);
                        await db.SaveChangesAsync();
                    }
                }
                await db.SaveChangesAsync();
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

        [HttpPost("EmployeeCreate")]
        public async Task<IActionResult> EmployeeCreate(Employee data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                EFCore.Repository.Edmx.EmploymentEligibilityVerification eligibility = null;
                EFCore.Repository.Edmx.EmployeeAuthorizedRepresentative EmpAuth = null;
                EFCore.Repository.Edmx.EmployeeDdauthorization EmpDD = null;
                EFCore.Repository.Edmx.EmployeeReverificationAndRehire empReV = null;
                EFCore.Repository.Edmx.EmployeeWithHoldingTax employeetax = null;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                Employee Innerdata = new Employee();
                bool isValid = db.Employees.ToList().Exists(x => x.Email == data.Email);
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.EMAIL_ALREADY, null, null);
                    return Ok(Response);
                }
                var fullcode = "";
                Employee newitems = new Employee();
                var recordemp = db.Employees.ToList();
                if (recordemp.Count() > 0)
                {
                    if (recordemp[0].EmployeeCode != null && recordemp[0].EmployeeCode != "string" && recordemp[0].EmployeeCode != "")
                    {
                        int large, small;
                        int salesID = 0;
                        large = Convert.ToInt32(recordemp[0].EmployeeCode.Split('-')[1]);
                        small = Convert.ToInt32(recordemp[0].EmployeeCode.Split('-')[1]);
                        for (int i = 0; i < recordemp.Count(); i++)
                        {
                            if (recordemp[i].EmployeeCode != null)
                            {
                                var t = Convert.ToInt32(recordemp[i].EmployeeCode.Split('-')[1]);
                                if (Convert.ToInt32(recordemp[i].EmployeeCode.Split('-')[1]) > large)
                                {
                                    salesID = Convert.ToInt32(recordemp[i].EmployeeId);
                                    large = Convert.ToInt32(recordemp[i].EmployeeCode.Split('-')[1]);

                                }
                                else if (Convert.ToInt32(recordemp[i].EmployeeCode.Split('-')[1]) < small)
                                {
                                    small = Convert.ToInt32(recordemp[i].EmployeeCode.Split('-')[1]);
                                }
                                else
                                {
                                    if (large < 2)
                                    {
                                        salesID = Convert.ToInt32(recordemp[i].EmployeeId);
                                    }
                                }
                            }
                        }
                        newitems = recordemp.ToList().Where(x => x.EmployeeId == salesID).FirstOrDefault();
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
                }
                else
                {
                    fullcode = "EMP00" + "-" + "1";
                }

                if (data.YesContractor == null)
                {
                    Innerdata.YesContractor = false;
                }
                if (data.NoContractor == null)
                {
                    Innerdata.NoContractor = false;
                }
                if (data.ProfileImage != null)
                {
                    //  return BadRequest("Image Path Required Instead of Byte");
                    var imgPath = SaveImage(data.ProfileImage, Guid.NewGuid().ToString());
                    data.ProfileImagePath = imgPath;
                    data.ProfileImage = null;
                }

                if (data.EmployeeWithHoldingTax != null)
                {
                    if (data.EmployeeWithHoldingTax.Domiciled == null)
                    {
                        data.EmployeeWithHoldingTax.Domiciled = false;
                    }
                    if (data.EmployeeWithHoldingTax.EmployerWithHold == null)
                    {
                        data.EmployeeWithHoldingTax.EmployerWithHold = false;
                    }
                    if (data.EmployeeWithHoldingTax.MilatrySpouseExempt == null)
                    {
                        data.EmployeeWithHoldingTax.MilatrySpouseExempt = false;
                    }
                    if (data.EmployeeWithHoldingTax.NoExemptions == null)
                    {
                        data.EmployeeWithHoldingTax.NoExemptions = false;
                    }
                    if (data.EmployeeWithHoldingTax.NoTaxLaibility == null)
                    {
                        data.EmployeeWithHoldingTax.NoTaxLaibility = false;
                    }
                    employeetax = new EFCore.Repository.Edmx.EmployeeWithHoldingTax();
                    employeetax.Address = data.EmployeeWithHoldingTax.Address;
                    employeetax.AdditionalAmount = data.EmployeeWithHoldingTax.AdditionalAmount;
                    employeetax.City = data.EmployeeWithHoldingTax.City;
                    employeetax.Country = data.EmployeeWithHoldingTax.Country;
                    employeetax.Date = data.EmployeeWithHoldingTax.Date;
                    employeetax.Domiciled = data.EmployeeWithHoldingTax.Domiciled;
                    employeetax.EmployeeSignature = data.EmployeeWithHoldingTax.EmployeeSignature;
                    employeetax.EmployerWithHold = data.EmployeeWithHoldingTax.EmployerWithHold;
                    employeetax.FirstName = data.EmployeeWithHoldingTax.FirstName;
                    employeetax.LastName = data.EmployeeWithHoldingTax.LastName;
                    employeetax.MarriedStatus = data.EmployeeWithHoldingTax.MarriedStatus;
                    employeetax.MiddleName = data.EmployeeWithHoldingTax.MiddleName;
                    employeetax.MilatrySpouseExempt = data.EmployeeWithHoldingTax.MilatrySpouseExempt;
                    employeetax.NameDiffSsn = data.EmployeeWithHoldingTax.NameDiffSsn;
                    employeetax.NoExemptions = data.EmployeeWithHoldingTax.NoExemptions;
                    employeetax.NoofAllowances = data.EmployeeWithHoldingTax.NoofAllowances;
                    employeetax.NoTaxLaibility = data.EmployeeWithHoldingTax.NoTaxLaibility;
                    employeetax.Ssn = data.EmployeeWithHoldingTax.Ssn;
                    employeetax.State = data.EmployeeWithHoldingTax.State;
                    employeetax.ZipCode = data.EmployeeWithHoldingTax.ZipCode;
                }

                Innerdata = data;
                Innerdata.EmployeeWithHoldingTax = null;
                Innerdata.IsActive = false;
                Innerdata.AdminApproval = null;
                Innerdata.AccessAccount = null;
                Innerdata.AdminStatus = null;

                Innerdata.EmployeeCode = fullcode;
                await db.Employees.AddAsync(Innerdata);
                await db.SaveChangesAsync();
                var getEmployee = db.Employees.ToList().Where(x => x.Email == data.Email && x.EmployeeCode == fullcode).FirstOrDefault();
                if (getEmployee != null && employeetax != null)
                {
                    employeetax.EmployeeId = getEmployee.EmployeeId;
                    db.EmployeeWithHoldingTaxes.Add(employeetax);
                    db.SaveChanges();
                }
                if (data.EmploymentEligibilityVerification != null)
                {
                    if (data.EmploymentEligibilityVerification.Lawful == null)
                    {
                        data.EmploymentEligibilityVerification.Lawful = false;
                    }
                    if (data.EmploymentEligibilityVerification.NoCitizen == null)
                    {
                        data.EmploymentEligibilityVerification.NoCitizen = false;
                    }
                    if (data.EmploymentEligibilityVerification.Citizen == null)
                    {
                        data.EmploymentEligibilityVerification.Citizen = false;
                    }
                    if (data.EmploymentEligibilityVerification.AllienAuthorized == null)
                    {
                        data.EmploymentEligibilityVerification.AllienAuthorized = false;
                    }
                    eligibility = new EFCore.Repository.Edmx.EmploymentEligibilityVerification();
                    eligibility.Address = data.EmploymentEligibilityVerification.Address;
                    eligibility.AdmissionNumber = data.EmploymentEligibilityVerification.AdmissionNumber;
                    eligibility.AllienAuthorized = data.EmploymentEligibilityVerification.AllienAuthorized;
                    eligibility.AllienAuthorizedNumber = data.EmploymentEligibilityVerification.AllienAuthorizedNumber;
                    eligibility.AptNumber = data.EmploymentEligibilityVerification.AptNumber;
                    eligibility.Citizen = data.EmploymentEligibilityVerification.Citizen;
                    eligibility.City = data.EmploymentEligibilityVerification.City;
                    eligibility.CountryIssuance = data.EmploymentEligibilityVerification.CountryIssuance;
                    eligibility.Date = data.EmploymentEligibilityVerification.Date;
                    eligibility.Dob = data.EmploymentEligibilityVerification.Dob;
                    eligibility.Email = data.EmploymentEligibilityVerification.Email;
                    eligibility.FirstName = data.EmploymentEligibilityVerification.FirstName;
                    eligibility.ForeignPassportNumber = data.EmploymentEligibilityVerification.ForeignPassportNumber;
                    eligibility.LastName = data.EmploymentEligibilityVerification.LastName;
                    eligibility.Lawful = data.EmploymentEligibilityVerification.Lawful;
                    eligibility.LawfulNumber = data.EmploymentEligibilityVerification.LawfulNumber;
                    eligibility.LawfulNumber = data.EmploymentEligibilityVerification.LawfulNumber;
                    eligibility.OtherNames = data.EmploymentEligibilityVerification.OtherNames;
                    eligibility.PrepareAddress = data.EmploymentEligibilityVerification.PrepareAddress;
                    eligibility.PrepareDate = data.EmploymentEligibilityVerification.PrepareDate;
                    eligibility.PrepareFirstName = data.EmploymentEligibilityVerification.PrepareFirstName;
                    eligibility.PrepareLastName = data.EmploymentEligibilityVerification.PrepareLastName;
                    eligibility.PrepareState = data.EmploymentEligibilityVerification.PrepareState;
                    eligibility.SignatureOfTransferer = data.EmploymentEligibilityVerification.SignatureOfTransferer;
                    eligibility.Ssn = data.EmploymentEligibilityVerification.Ssn;
                    eligibility.State = data.EmploymentEligibilityVerification.State;
                    eligibility.NoCitizen = data.EmploymentEligibilityVerification.NoCitizen;

                }
                if (data.EmployeeReverificationAndRehire != null)
                {
                    empReV = new EFCore.Repository.Edmx.EmployeeReverificationAndRehire();
                    empReV.Date = data.EmployeeReverificationAndRehire.Date;
                    empReV.DateofRehire = data.EmployeeReverificationAndRehire.DateofRehire;
                    empReV.DocumentNumber = data.EmployeeReverificationAndRehire.DocumentNumber;
                    empReV.DocumentTitle = data.EmployeeReverificationAndRehire.DocumentTitle;
                    empReV.EmpName = data.EmployeeReverificationAndRehire.EmpName;
                    empReV.EmpReverificationId = data.EmployeeReverificationAndRehire.EmpReverificationId;
                    empReV.ExpirationDate = data.EmployeeReverificationAndRehire.ExpirationDate;
                    empReV.FirstName = data.EmployeeReverificationAndRehire.FirstName;
                    empReV.LastName = data.EmployeeReverificationAndRehire.LastName;
                    empReV.MiddleName = data.EmployeeReverificationAndRehire.MiddleName;
                    empReV.NewName = data.EmployeeReverificationAndRehire.NewName;
                    empReV.Signature = data.EmployeeReverificationAndRehire.Signature;
                }

                if (data.employeeAuthorizedRepresentative != null)
                {
                    EmpAuth = new EFCore.Repository.Edmx.EmployeeAuthorizedRepresentative();
                    EmpAuth.Date = data.employeeAuthorizedRepresentative.Date;
                    EmpAuth.DocumentNumber = data.employeeAuthorizedRepresentative.DocumentNumber;
                    EmpAuth.DocumentTitle = data.employeeAuthorizedRepresentative.DocumentTitle;
                    EmpAuth.EmpAuthRepId = data.employeeAuthorizedRepresentative.EmpAuthRepId;
                    EmpAuth.EmpTitle = data.employeeAuthorizedRepresentative.EmpTitle;
                    EmpAuth.ExpirationDate = data.employeeAuthorizedRepresentative.ExpirationDate;
                    EmpAuth.FirstDate = data.employeeAuthorizedRepresentative.FirstDate;
                    EmpAuth.FirstName = data.employeeAuthorizedRepresentative.FirstName;
                    EmpAuth.IssuingAuthority = data.employeeAuthorizedRepresentative.IssuingAuthority;
                    EmpAuth.LastName = data.employeeAuthorizedRepresentative.LastName;
                    EmpAuth.MiddleName = data.employeeAuthorizedRepresentative.MiddleName;
                    EmpAuth.OrgAddress = data.employeeAuthorizedRepresentative.OrgAddress;
                    EmpAuth.OrgName = data.employeeAuthorizedRepresentative.OrgName;
                    EmpAuth.Signature = data.employeeAuthorizedRepresentative.Signature;
                    EmpAuth.State = data.employeeAuthorizedRepresentative.State;
                    EmpAuth.ZipCode = data.employeeAuthorizedRepresentative.ZipCode;
                }

                if (data.EmployeeDdauthorization != null)
                {
                    if (data.EmployeeDdauthorization.Authorize == null)
                    {
                        data.EmployeeDdauthorization.Authorize = false;
                    }
                    if (data.EmployeeDdauthorization.Cancel == null)
                    {
                        data.EmployeeDdauthorization.Cancel = false;
                    }
                    if (data.EmployeeDdauthorization.Revise == null)
                    {
                        data.EmployeeDdauthorization.Revise = false;
                    }
                    if (data.EmployeeDdauthorization.RemainingBalance == null)
                    {
                        data.EmployeeDdauthorization.RemainingBalance = false;
                    }
                    if (data.EmployeeDdauthorization.UsePercentage == null)
                    {
                        data.EmployeeDdauthorization.UsePercentage = false;
                    }
                    EmpDD = new EFCore.Repository.Edmx.EmployeeDdauthorization();
                    EmpDD.AccTypeOne = data.EmployeeDdauthorization.AccTypeOne;
                    EmpDD.AccTypeThree = data.EmployeeDdauthorization.AccTypeThree;
                    EmpDD.AccTypeTwo = data.EmployeeDdauthorization.AccTypeTwo;
                    EmpDD.AddressOne = data.EmployeeDdauthorization.AddressOne;
                    EmpDD.AddressThree = data.EmployeeDdauthorization.AddressThree;
                    EmpDD.AddressTwo = data.EmployeeDdauthorization.AddressTwo;
                    EmpDD.Authorize = data.EmployeeDdauthorization.Authorize;
                    EmpDD.Bank = data.EmployeeDdauthorization.Bank;
                    EmpDD.BankAccNumberOne = data.EmployeeDdauthorization.BankAccNumberOne;
                    EmpDD.BankAccNumberThree = data.EmployeeDdauthorization.BankAccNumberThree;
                    EmpDD.BankAccNumberTwo = data.EmployeeDdauthorization.BankAccNumberTwo;
                    EmpDD.BankAmountOne = data.EmployeeDdauthorization.BankAmountOne;
                    EmpDD.BankAmountThree = data.EmployeeDdauthorization.BankAmountThree;
                    EmpDD.BankAmountTwo = data.EmployeeDdauthorization.BankAmountTwo;
                    EmpDD.BankName = data.EmployeeDdauthorization.BankName;
                    EmpDD.BankNameOne = data.EmployeeDdauthorization.BankNameOne;
                    EmpDD.BankNameThree = data.EmployeeDdauthorization.BankNameThree;
                    EmpDD.BankNameTwo = data.EmployeeDdauthorization.BankNameTwo;
                    EmpDD.BankRoutingOne = data.EmployeeDdauthorization.BankRoutingOne;
                    EmpDD.BankRoutingThree = data.EmployeeDdauthorization.BankRoutingThree;
                    EmpDD.BankRoutingTwo = data.EmployeeDdauthorization.BankRoutingTwo;
                    EmpDD.Cancel = data.EmployeeDdauthorization.Cancel;
                    EmpDD.Date = data.EmployeeDdauthorization.Date;
                    EmpDD.EmpDdaid = data.EmployeeDdauthorization.EmpDdaid;
                    EmpDD.Memo = data.EmployeeDdauthorization.Memo;
                    EmpDD.PayTo = data.EmployeeDdauthorization.PayTo;
                    EmpDD.PctOne = data.EmployeeDdauthorization.PctOne;
                    EmpDD.PctThree = data.EmployeeDdauthorization.PctThree;
                    EmpDD.PctTwo = data.EmployeeDdauthorization.PctTwo;
                    EmpDD.PhoneOne = data.EmployeeDdauthorization.PhoneOne;
                    EmpDD.PhoneThree = data.EmployeeDdauthorization.PhoneThree;
                    EmpDD.PhoneTwo = data.EmployeeDdauthorization.PhoneTwo;
                    EmpDD.Revise = data.EmployeeDdauthorization.Revise;
                    EmpDD.Signature = data.EmployeeDdauthorization.Signature;
                    EmpDD.Total = data.EmployeeDdauthorization.Total;

                }

                var getEmployeenew = db.Employees.ToList().Where(x => x.Email == data.Email && x.EmployeeCode == fullcode).FirstOrDefault();
                if (getEmployeenew != null && eligibility != null)
                {
                    eligibility.EmployeeId = getEmployeenew.EmployeeId;
                    db.EmploymentEligibilityVerifications.Add(eligibility);
                    await db.SaveChangesAsync();

                    empReV.EmployeeId = getEmployeenew.EmployeeId;
                    db.EmployeeReverificationAndRehires.Add(empReV);
                    await db.SaveChangesAsync();

                    EmpAuth.EmployeeId = getEmployeenew.EmployeeId;
                    db.EmployeeAuthorizedRepresentatives.Add(EmpAuth);
                    await db.SaveChangesAsync();
                    if (data.EmployeeDdauthorization != null)
                    {
                        EmpDD.EmployeeId = getEmployeenew.EmployeeId;
                        db.EmployeeDdauthorizations.Add(EmpDD);
                        await db.SaveChangesAsync();
                    }
                }



                MailRequest request = new MailRequest();
                request.ToEmail = "usman.amjad@ab-sol.net";
                request.Subject = "OTP ABCDiscounts";
                var usermsg = "<div></h3>Dear Admin, Request for approval of new empoyee has received.</h3><div>";
                request.Body = usermsg;
                var emailresponse = await Send(request);
                if (emailresponse == "Email Sent Successfully")
                {
                    //return CreatedAtAction("EmployeeGet", new { id = data.EmployeeId }, "Employee approval Email request has been send for approval");
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getEmployeenew);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getEmployeenew);
                    return Ok(Response);
                    //return CreatedAtAction("EmployeeGet", new { id = data.EmployeeId }, "Employee approval request has been send for approval, but Could't Send Email Due to Slow Internet");
                }
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


        //Employee End
        // Start EmployeeLeaveEntitle
        [HttpGet("EmployeeLeaveEntitleGet")]
        public IActionResult EmployeeLeaveEntitleGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmployeeLeaveEntitle>>();
                var record = db.EmployeeLeaveEntitles.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EmployeeLeaveEntitleCreate")]
        public async Task<IActionResult> EmployeeLeaveEntitleCreate(EmployeeLeaveEntitle data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (data.EmployeeId != null && data.EmployeeNo != "")
                {
                    var employee = db.Employees.Find(data.EmployeeId);
                    if (employee.EmployeeCode != null && employee.EmployeeCode != "undefined")
                    {
                        data.EmployeeNo = employee.EmployeeCode;
                        data.EmployeeName = employee.FullName;
                    }
                }
                if (data.LeaveTypeId != null && data.LeaveTypeId > 0)
                {
                    var leavetyp = db.LeaveTypes.Find(data.LeaveTypeId);
                    if (leavetyp.LeaveTypeId > 0)
                    {
                        data.LeaveTypeName = leavetyp.TypeName;
                    }
                }
                db.EmployeeLeaveEntitles.Add(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EmployeeLeaveEntitleUpdate/{id}")]
        public async Task<IActionResult> EmployeeLeaveEntitleUpdate(int id, EmployeeLeaveEntitle data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }

                if (id != data.LeaveTypeId)
                {
                    return BadRequest();
                }
                var record = await db.EmployeeLeaveEntitles.FindAsync(id);
                if (data.NoofLeaves != null && data.NoofLeaves != "undefined")
                {
                    record.NoofLeaves = data.NoofLeaves;
                }
                if (data.ApprovedLeave != null && data.ApprovedLeave != "undefined")
                {
                    record.ApprovedLeave = data.ApprovedLeave;
                }
                if (data.AvailableLeave != null && data.AvailableLeave != "undefined")
                {
                    record.AvailableLeave = data.AvailableLeave;
                }
                if (data.RejectedLeave != null && data.RejectedLeave != "undefined")
                {
                    record.RejectedLeave = data.RejectedLeave;
                }
                if (data.EmployeeId != null && data.EmployeeNo != "")
                {
                    var employee = db.Employees.Find(data.EmployeeId);
                    if (employee.EmployeeCode != null && employee.EmployeeCode != "undefined")
                    {
                        record.EmployeeNo = employee.EmployeeCode;
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                if (data.LeaveTypeId != null && data.LeaveTypeId > 0)
                {
                    var leavetyp = db.LeaveTypes.Find(data.LeaveTypeId);
                    if (leavetyp.LeaveTypeId > 0)
                    {
                        record.EmployeeNo = leavetyp.TypeName;
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("EmployeeLeaveEntitleByID/{id}")]
        public IActionResult EmployeeLeaveEntitleByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                var record = db.EmployeeLeaveEntitles.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteEmployeeLeaveEntitle/{id}")]
        public async Task<IActionResult> DeleteEmployeeLeaveEntitle(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                EmployeeLeaveEntitle data = await db.EmployeeLeaveEntitles.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeLeaveEntitles.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeLeaveEntitle>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        // End EmployeeLeaveEntitle
        [HttpGet("EmployeeDocumentGet")]
        public IActionResult EmployeeDocumentGet()
        {
            try
            {
                var record = db.EmployeeDocuments.ToList();
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
        [HttpPost("EmployeeDocumentCreate")]
        public async Task<IActionResult> EmployeeDocumentCreate(EmployeeDocument data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (data.EmployeeId != null)
                {
                    var employee = db.Employees.Find(data.EmployeeId);
                    if (employee.EmployeeCode != null && employee.EmployeeCode != "undefined")
                    {
                        data.EmployeeNumber = employee.EmployeeCode;
                        data.EmployeeName = employee.FullName;
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                db.EmployeeDocuments.Add(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
        [HttpPut("EmployeeDocumentUpdate/{id}")]
        public async Task<IActionResult> EmployeeDocumentUpdate(int id, EmployeeDocument data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }

                if (id != data.DocId)
                {
                    return BadRequest();
                }
                if (data.DocTypeId != null && data.DocTypeId > 0)
                {
                    var leavetyp = db.LeaveTypes.Find(data.DocTypeId);
                    if (leavetyp != null)
                    {
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.FAILURE_CODE, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }

                if (data.EmployeeId != null && data.EmployeeId > 0)
                {
                    var leavetyp = db.Employees.Find(data.EmployeeId);
                    if (leavetyp != null)
                    {
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.FAILURE_CODE, null, null);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                var record = await db.EmployeeDocuments.FindAsync(id);

                if (data.DocTypeId != null)
                {
                    record.DocTypeId = data.DocTypeId;
                    var gettype = await db.EmployeeDocumentTypes.FindAsync(data.DocTypeId);
                    if (gettype != null)
                    {
                        if (gettype.TypeName != null && gettype.TypeName != "undefined")
                        {
                            record.DocumentTypeName = gettype.TypeName;
                        }
                    }
                }
                if (data.DocumentByPath != null && data.DocumentByPath != "")
                {
                    record.DocumentByPath = data.DocumentByPath;
                }

                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
        [HttpGet("EmployeeDocumentByID/{id}")]
        public IActionResult EmployeeDocumentByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                var record = db.EmployeeDocuments.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeDetailsByID/{id}")]
        public IActionResult EmployeeDetailsByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employee>();
                var record = db.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeContractDetailsByCode/{id}")]
        public IActionResult EmployeeContractDetailsByCode(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                var record = db.EmployeeContracts.Where(x => x.EmployeeId == id)?.FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeTaxList/{id}")]
        public IActionResult EmployeeTaxesByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpTaxType>>();
                List<EmpTaxType> taxlist = new List<EmpTaxType>();
                var record = db.EmpTaxes.Where(x => x.EmployeeId == id).ToList();
                foreach (var item in record)
                {
                    var tax = db.EmpTaxTypes.Where(x => x.EmpTaxTypeId == item.EmpTaxTypeId).FirstOrDefault();
                    taxlist.Add(tax);
                }

                if (taxlist.Count() > 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, taxlist);

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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeDocumentByEmployeeID/{id}")]
        public IActionResult EmployeeDocumentByEmployeeID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                var record = db.EmployeeDocuments.Where(x => x.EmployeeId == id).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteEmployeeDocument/{id}")]
        public async Task<IActionResult> DeleteEmployeeDocument(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocument>();
                EmployeeDocument data = await db.EmployeeDocuments.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeDocuments.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
        //  Employee Contract Start
        [HttpGet("EmployeeContractGet")]
        public IActionResult EmployeeContractGet()
        {
            try
            {
                var record = db.EmployeeContracts.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<EmployeeContract>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
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
        [HttpPost("EmployeeContractCreate")]
        public async Task<IActionResult> EmployeeContractCreate(EmployeeContract data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool isValid = db.EmployeeContracts.ToList().Exists(x => x.EmployeeNumber == data.EmployeeNumber);
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }

                data.JoiningDate = DateTime.Now;
                data.ContractDate = DateTime.Now;
                db.EmployeeContracts.Add(data);
                await db.SaveChangesAsync();
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
        [HttpPost("EmployeeContractUpdate/{id}")]
        public async Task<IActionResult> EmployeeContractUpdate(int id, EmployeeContract data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }

                if (id != data.ContractId)
                {
                    return BadRequest();
                }
                bool isValid = db.EmployeeContracts.ToList().Exists(x => x.EmployeeNumber == data.EmployeeNumber && x.ContractId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }

                var record = await db.EmployeeContracts.FindAsync(id);

                if (data.IsProbation != null)
                {
                    record.IsProbation = data.IsProbation;
                }
                if (data.Permanent != null)
                {
                    record.Permanent = data.Permanent;
                }
                if (data.DailyWages != null)
                {
                    record.DailyWages = data.DailyWages;
                }
                if (data.ProbationSalary != null)
                {
                    record.ProbationSalary = data.ProbationSalary;
                }
                if (data.Salary != null)
                {
                    record.Salary = data.Salary;
                }
                if (data.Salary != null)
                {
                    record.Salary = data.Salary;
                }
                if (data.ContractDocumentByPath != null)
                {
                    record.ContractDocumentByPath = data.ContractDocumentByPath;
                }
                if (data.ContractDocumentByPath != null)
                {
                    record.ContractDocumentByPath = data.ContractDocumentByPath;
                }
                if (data.OnContract != null)
                {
                    record.OnContract = data.OnContract;
                }
                if(data.DailyWagesChargesAmount != null)
                {
                    record.DailyWagesChargesAmount = data.DailyWagesChargesAmount;
                }
                if(data.JoiningDate != null)
                {
                    record.JoiningDate = data.JoiningDate;
                } if(data.ContractDate != null)
                {
                    record.ContractDate = data.ContractDate;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
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
        [HttpGet("EmployeeContractByID/{id}")]
        public IActionResult EmployeeContractByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                var record = db.EmployeeContracts.Find(id);
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
                var record = db.EmployeeContracts.Where(x => x.EmployeeId == id).FirstOrDefault();
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
        [HttpDelete("DeleteEmployeeContract/{id}")]
        public async Task<IActionResult> DeleteEmployeeContract(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeContract>();
                EmployeeContract data = await db.EmployeeContracts.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeContracts.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
        //  Employee Contract End

        // Employee Settlement Start

        [HttpGet("EmployeeSettlementGet")]
        public IActionResult EmployeeSettlementGet()
        {
            try
            {
                var record = db.EmployeeSettlements.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<EmployeeSettlement>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EmployeeSettlementCreate")]
        public async Task<IActionResult> EmployeeSettlementCreate(EmployeeSettlement data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (data.EmployeeId == null || data.EmployeeId == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                bool isValid = db.EmployeeSettlements.ToList().Exists(x => x.SettlementId == data.SettlementId);
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);

                }

                data.CreatedDate = DateTime.Now;

                db.EmployeeSettlements.Add(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("EmployeeSettlementUpdate/{id}")]
        public async Task<IActionResult> EmployeeSettlementUpdate(int id, EmployeeSettlement data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                if (data.EmployeeId == 0 || data.EmployeeId == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }

                bool isValid = db.EmployeeSettlements.ToList().Exists(x => x.EmployeeEmail == data.EmployeeEmail && x.SettlementId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                var record = await db.EmployeeSettlements.FindAsync(id);

                if (data.EmployeeName != null && data.EmployeeName != "")
                {
                    record.EmployeeName = data.EmployeeName;
                }
                if (data.EmployeeEmail != null && data.EmployeeEmail != "")
                {
                    record.EmployeeEmail = data.EmployeeEmail;
                }
                if (data.PendingSalary != null && data.PendingSalary != "")
                {
                    record.PendingSalary = data.PendingSalary;
                }
                if (data.PendingLeave != null && data.PendingLeave != "")
                {
                    record.PendingLeave = data.PendingLeave;
                }
                if (data.EmployeeEmail != null && data.EmployeeEmail != "")
                {
                    record.EmployeeEmail = data.EmployeeEmail;
                }
                if (data.TerminationNode != null && data.TerminationNode != "")
                {
                    record.TerminationNode = data.TerminationNode;
                }
                if (data.TerminationDate != null)
                {
                    record.TerminationDate = data.TerminationDate;
                }
                if (data.IsLoan != null)
                {
                    record.IsLoan = data.IsLoan;
                }
                if (data.ConfidentialClear != null)
                {
                    record.ConfidentialClear = data.ConfidentialClear;
                }
                if (data.AssetsReturn != null)
                {
                    record.AssetsReturn = data.AssetsReturn;
                }
                data = record;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeSettlementByID/{id}")]
        public IActionResult EmployeeSettlementByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                var record = db.EmployeeSettlements.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeSettlementEmployeeID/{id}")]
        public IActionResult EmployeeSettlementEmployeeID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                var record = db.EmployeeSettlements.Where(x => x.SettlementId == id).FirstOrDefault();
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteEmployeeSettlement/{id}")]
        public async Task<IActionResult> DeleteEmployeeSettlement(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                EmployeeSettlement data = await db.EmployeeSettlements.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeSettlements.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeSettlement>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        // Employee Settlement End


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
                imgPath = "http://45.35.97.246:5595/abcdhrapi/images/" + imageName;
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
                imgPath = "http://45.35.97.246:5595/abcdhrapi/images/" + imageName;
                return imgPath;
            }
            imgPath = imgPath.Replace(" ", "");
            return imgPath;
        }
        [HttpGet("EmployeeDetail/{id}")]
        public IActionResult EmployeeGet(int id)
        {
            try
            {
                var record = db.Employees.FirstOrDefault(x => x.EmployeeId == id);
                var record2 = new List<EmpTaxType>();
                var Response = ResponseBuilder.BuildWSResponse<List<EmpTaxType>>();
                if (record.MartialStatus != null)
                {
                    var employeeContract = db.EmployeeContracts.Where(x => x.EmployeeId == id)?.FirstOrDefault();
                    if (employeeContract != null)
                    {   //For Monthly Salary
                        if (employeeContract.DailyWages == null || employeeContract.DailyWages == false)
                        {
                            var salary = decimal.Parse(employeeContract.Salary);
                            var taxes = db.EmpTaxTypes.Where(x => x.TaxType == record.MartialStatus).ToList();
                            foreach (var item in taxes)
                            {
                                var salaryRange = item.SalaryRange;
                                var amounts = salaryRange.Split(" -> ");
                                var lowestAmount = int.Parse((amounts[0].Split("$")[1]).Replace(",", ""));
                                var highestAmount = int.Parse((amounts[1].Split("$")[1]).Replace(",", ""));
                                if (salary >= lowestAmount && salary <= highestAmount)
                                {
                                    record2.Add(item);
                                }
                            }

                        }
                    }

                }
                if (record2.Count() > 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record2);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record2);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<List<EmpTaxType>>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("TaxDetail/{id}")]
        public IActionResult TaxDetail(int id)
        {
            try
            {
                var record = db.EmpTaxTypes.FirstOrDefault(x => x.EmpTaxTypeId == id);
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                if (record != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

    }



}
