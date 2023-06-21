
using ABC.Customer.API.ViewModel;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.Customer.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public EncryptDecrypt encrypter = new EncryptDecrypt();
        private readonly IMailService mailService;
        private const string secretKey = "this_is_my_case_secret-Key-for-token_generation";
        public static readonly SymmetricSecurityKey signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        EncryptDecrypt encdec = new EncryptDecrypt();
        private readonly IConfiguration config;
        private EmailService emailService = new EmailService();
        private IWebHostEnvironment env;

        public SecurityController(ABCDiscountsContext _db, IMailService mailService, IConfiguration config, IWebHostEnvironment env)
        {
            db = _db;
            this.mailService = mailService;
            this.config = config;
            this.env = env;
        }


        [HttpPost("Login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AspNetUser user)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();

                if (user.Email != null && user.PasswordHash != null)
                {


                    var innerpassword = encrypter.Encrypt(user.PasswordHash);
                    var FoundUser = db.AspNetUsers.ToList().Where(x => x.Email == user.Email && x.PasswordHash == innerpassword).FirstOrDefault();
                    if (FoundUser != null)
                    {
                        FoundUser.LastLogin = DateTime.Now;
                        db.Entry(FoundUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        var checkInnerRole = db.AspNetRoles.ToList().Where(x => x.Id == Convert.ToInt32(FoundUser.RoleId)).FirstOrDefault();
                        if (checkInnerRole != null)
                        {

                            FoundUser.RoleName = checkInnerRole.Name;
                            var Token = GenerateToken(FoundUser);
                            FoundUser.RefreshToken = Token;
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, FoundUser);
                        }
                        else
                        {
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.FAILURE_CODE, null, null);

                        }
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, null);
                    }

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, null);
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

        [AllowAnonymous]
        [HttpGet]
        [Route("Register-Admin")]
        public async Task<IActionResult> RegisterAdmin()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();
                AspNetUser model = new AspNetUser()
                {
                    UserName = "absol",
                    Email = "absol@ab-sol.net",
                    PhoneNumber = "123456789",
                    PasswordHash = "123",
                };


                AspNetRole role = new AspNetRole();
                var checkfound = db.AspNetRoles.ToList().Where(x => x.Name == "Admin").FirstOrDefault();
                if (checkfound != null)
                {
                }
                else
                {
                    role.Name = "Admin";
                    var newRole = db.AspNetRoles.Add(role);
                    await db.SaveChangesAsync();
                }
                // var userExists = await userManager.FindByEmailAsync(model.Email);
                var userExists = db.AspNetUsers.ToList().Where(x => x.Email == model.Email).FirstOrDefault();
                if (userExists == null)
                {
                    model.UserName = model.Email;
                    if (!string.IsNullOrEmpty(model.PasswordHash))
                    {
                        var checkInnerRole = db.AspNetRoles.ToList().Where(x => x.Name == "Admin").FirstOrDefault();
                        model.PasswordHash = encdec.Encrypt(model.PasswordHash);
                        model.TwoFactorEnabled = false;
                        model.IsCancelled = false;
                        model.IsActive = true;
                        model.RoleId = checkInnerRole.Id;
                        var NewUser = db.AspNetUsers.Add(model);
                        await db.SaveChangesAsync();

                        AspNetUserRole userrole = new AspNetUserRole();
                        userrole.UserId = NewUser.Entity.Id;
                        userrole.RolesId = checkInnerRole.Id;
                        db.AspNetUserRoles.Add(userrole);
                        await db.SaveChangesAsync();

                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                        return Ok(Response);
                    }
                    else
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.INVALID_PASSWORD_EMAIL, null, null);
                        return BadRequest(Response);
                    }
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.EMAIL_ALREADY, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateToken(AspNetUser userDto)
        {
            var token = new JwtSecurityToken(
                   claims: new Claim[]
                   {
                       new Claim(ClaimTypes.Email, userDto.Email),
                       new Claim(ClaimTypes.Role, userDto.RoleName),
                       new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString())
                   },
                   notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                   expires: new DateTimeOffset(DateTime.Now.AddDays(7)).DateTime,
                   issuer: userDto.Email,
                   signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Authorize(Roles = "Admin, Customer")]
        [HttpGet("CustomerProfileByID/{id}")]
        public IActionResult CustomerProfileByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.CustomerInformation>();
                var getCurrentCustomer = db.CustomerInformations.Find(id);
                if (getCurrentCustomer != null)
                {

                    var GetUser = db.AspNetUsers.ToList().Where(x => x.Id == getCurrentCustomer.UserId).FirstOrDefault();
                    if (GetUser != null)
                    {
                        getCurrentCustomer.AspNetUser = GetUser;
                    }
                    var getCertificate = db.CertificateExemptionInstructions.ToList().Where(x => x.CustomerId == getCurrentCustomer.Id).FirstOrDefault();
                    if (getCertificate != null)
                    {
                        getCurrentCustomer.CertificateExemptionInstructions = getCertificate;

                        var getBusiness = db.CertificateBusinessTypes.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getBusiness != null)
                        {
                            getCurrentCustomer.CertificateBusinessTypes = getBusiness;
                        }

                        var getIdentifications = db.CertificateIdentifications.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getIdentifications != null)
                        {
                            getCurrentCustomer.CertificateIdentifications = getIdentifications;
                        }

                        var getReasons = db.CertificateReasonExemptions.ToList().Where(x => x.CertificateId == getCertificate.Ceiid).ToList();
                        if (getReasons != null)
                        {
                            getCurrentCustomer.CertificateReasonExemptions = getReasons;
                        }

                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, getCurrentCustomer);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Customer_Profile_Not_Found, null, null);
                }
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ABC.EFCore.Repository.Edmx.Customer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("ResetPasswordEmail/{Email}")]
        public async Task<IActionResult> ResetPasswordEmail(string Email)
        {
            try
            {

                var currentUser = db.AspNetUsers.ToList().Where(x => x.Email == Email).FirstOrDefault();
                if (currentUser != null)
                {
                    string token = Guid.NewGuid().ToString();
                    string webUrl = config.GetValue<string>("webbaseurl");
                    string url = $"{webUrl}Account/ResetPassword?email={Email}&&token={token}";
                    bool isEmailSent = emailService.SendEmail(currentUser.Email, "", url);
                    if (isEmailSent)
                        return Ok(token);
                    return BadRequest("An error occured while sending email");
                }
                else
                    return BadRequest("User Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ForgetPassword forgetPassword)
        {
            try
            {
                var currentUser = db.AspNetUsers.ToList().Where(x => x.Email == forgetPassword.Email).FirstOrDefault();
                if (currentUser != null)
                {

                    currentUser.PasswordHash = encrypter.Encrypt(forgetPassword.NewPasswords);
                    db.Entry(currentUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok("Password changed successfully");
                }
                else
                    return BadRequest("User Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("changepassword")]
        public async Task<IActionResult> ChangePassword(ForgetPassword forgetPassword)
        {
            try
            {
                var currentPassword = encrypter.Encrypt(forgetPassword.Password);
                var currentUser = db.AspNetUsers.ToList().Where(x => x.PasswordHash == currentPassword && x.Email == forgetPassword.Email).FirstOrDefault();
                if (currentUser != null)
                {

                    currentUser.PasswordHash = encrypter.Encrypt(forgetPassword.NewPasswords);
                    DateTime now = DateTime.Now;
                    currentUser.LastChangePwdDate = now;
                    db.Entry(currentUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return Ok("Password changed successfully");
                }
                else
                    return BadRequest("User Not Found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("OpenItemCategoryGet")]
        public IActionResult OpenItemCategoryGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemCategory>>();
                var record = db.ItemCategories.ToList().OrderBy(x => x.Name).ToList();
                //   return Ok(record);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("OpenItemGet")]
        public IActionResult OpenItemGet()
        {
            try
            {
                var record = db.Products.ToList();
                for (int i = 0; i < record.Count(); i++)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == record[i].Id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record[i].Stock.ProductId = getStock.ProductId;
                        record[i].Stock.ItemCode = getStock.ItemCode;
                        record[i].Stock.Quantity = getStock.Quantity;
                        record[i].Stock.ItemName = getStock.ItemName;
                        record[i].Stock.Sku = getStock.Sku;
                        record[i].Stock.ItemBarCode = getStock.ItemBarCode;
                        record[i].Stock.StockId = getStock.StockId;
                    }
                    var getFinancial = db.Financials.AsQueryable().Where(x => x.ItemId == record[i].Id).FirstOrDefault();
                    if (getFinancial != null)
                    {
                        record[i].Financial = new Financial();
                        record[i].Financial.ItemName = getFinancial.ItemName;
                        record[i].Financial.ItemId = getFinancial.ItemId;
                        record[i].Financial.ItemNumber = getFinancial.ItemNumber;
                        record[i].Financial.Quantity = getFinancial.Quantity;
                        record[i].Financial.Cost = getFinancial.Cost;
                        record[i].Financial.Profit = getFinancial.Profit;
                        record[i].Financial.MsgPromotion = getFinancial.MsgPromotion;
                        record[i].Financial.AddToCost = getFinancial.AddToCost;
                        record[i].Financial.ItemId = getFinancial.ItemId;
                        record[i].Financial.FixedCost = getFinancial.FixedCost;
                        record[i].Financial.CostPerQuantity = getFinancial.CostPerQuantity;
                        record[i].Financial.St = getFinancial.St;
                        record[i].Financial.Tax = getFinancial.Tax;
                        record[i].Financial.OutOfStateCost = getFinancial.OutOfStateCost;
                        record[i].Financial.OutOfStateRetail = getFinancial.OutOfStateRetail;
                        record[i].Financial.Price = getFinancial.Price;
                        record[i].Financial.Quantity = getFinancial.Quantity;
                        record[i].Financial.QuantityPrice = getFinancial.QuantityPrice;
                        record[i].Financial.SuggestedRetailPrice = getFinancial.SuggestedRetailPrice;
                        record[i].Financial.AutoSetSrp = getFinancial.AutoSetSrp;
                        record[i].Financial.QuantityInStock = getFinancial.MsgPromotion;
                        record[i].Financial.Adjustment = getFinancial.Adjustment;
                        record[i].Financial.AskForPricing = getFinancial.AskForPricing;
                        record[i].Financial.AskForDescrip = getFinancial.AskForDescrip;
                        record[i].Financial.Serialized = getFinancial.Serialized;
                        record[i].Financial.TaxOnSales = getFinancial.TaxOnSales;
                        record[i].Financial.Purchase = getFinancial.Purchase;
                        record[i].Financial.NoSuchDiscount = getFinancial.NoSuchDiscount;
                        record[i].Financial.NoReturns = getFinancial.NoReturns;
                        record[i].Financial.SellBelowCost = getFinancial.SellBelowCost;
                        record[i].Financial.OutOfState = getFinancial.OutOfState;
                        record[i].Financial.CodeA = getFinancial.CodeA;
                        record[i].Financial.CodeB = getFinancial.CodeB;
                        record[i].Financial.CodeC = getFinancial.CodeC;
                        record[i].Financial.CodeD = getFinancial.CodeD;
                        record[i].Financial.AddCustomersDiscount = getFinancial.AddCustomersDiscount;
                        record[i].Financial.Retail = getFinancial.Retail;

                    }

                }
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record.ToList().OrderBy(x => x.Name).ToList());
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("OpenItemFetch")]
        public IActionResult OpenItemFetch()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.ToList().OrderBy(x => x.Name).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("OpenItemSubCategoryGet")]
        public IActionResult OpenItemSubCategoryGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemSubCategory>>();
                var record = db.ItemSubCategories.ToList().OrderBy(x => x.SubCategory).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [AllowAnonymous]
        [HttpGet("OpenItemGetByIDWithStock/{id}")]
        public IActionResult OpenItemGetByIDWithStock(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Product>();
                var record = db.Products.Find(id);
                if (record != null)
                {
                    var getStock = db.InventoryStocks.ToList().Where(x => x.ProductId == id).FirstOrDefault();
                    if (getStock != null)
                    {
                        record.Stock.ProductId = getStock.ProductId;
                        record.Stock.ItemCode = getStock.ItemCode;
                        record.Stock.Quantity = getStock.Quantity;
                        record.Stock.ItemName = getStock.ItemName;
                        record.Stock.Sku = getStock.Sku;
                        record.Stock.ItemBarCode = getStock.ItemBarCode;
                        record.Stock.StockId = getStock.StockId;
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                    return Ok(Response);
                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Product>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CustomerInformationCreate")]
        public async Task<IActionResult> CustomerInformationCreate(CustomerInformation obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<CustomerInformation>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var allusers = db.AspNetUsers.ToList().Where(x => x.Email.Equals(obj.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (allusers != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.EMAIL_ALREADY, null, null);
                    return Ok(Response);
                }

                var allcustomer = db.CustomerInformations.ToList().Where(x => x.Email != null && x.Email.Equals(obj.Email, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                if (allcustomer != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.EMAIL_ALREADY, null, null);
                    return Ok(Response);
                }
                if (obj.FirstName != null && obj.LastName != null)
                {
                    obj.FullName = obj.FirstName + obj.LastName;
                }
                //account
                Account objacount = null;
                objacount = new Account();
                var subaccrecord = db.AccountSubGroups.ToList().Where(x => x.Title == "Customers").FirstOrDefault();
                if (subaccrecord != null)
                {
                    var getAccount = db.Accounts.ToList().Where(x => x.AccountSubGroupId == subaccrecord.AccountSubGroupId).LastOrDefault();
                    if (getAccount != null)
                    {
                        var code = getAccount.AccountId.Split("-")[3];
                        int getcode = 0;
                        if (code != null)
                        {

                            getcode = Convert.ToInt32(code) + 1;
                        }
                        if (getcode > 9)
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-00" + Convert.ToString(getcode);

                        }
                        else if (getcode > 99)
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-0" + Convert.ToString(getcode);
                        }
                        else
                        {
                            objacount.AccountId = subaccrecord.AccountSubGroupId + "-000" + Convert.ToString(getcode);
                        }
                        objacount.Title = obj.Company;
                        objacount.Status = 1;
                        objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                        var customeracc = db.Accounts.Add(objacount);
                        db.SaveChanges();

                        obj.AccountId = customeracc.Entity.AccountId;
                        obj.AccountNumber = customeracc.Entity.AccountId;
                        obj.AccountTitle = customeracc.Entity.Title;
                    }
                    else
                    {
                        objacount.AccountId = subaccrecord.AccountSubGroupId + "-0001";
                        objacount.Title = obj.Company;
                        objacount.Status = 1;
                        objacount.AccountSubGroupId = subaccrecord.AccountSubGroupId;
                        var customeracc = db.Accounts.Add(objacount);
                        db.SaveChanges();
                        obj.AccountId = customeracc.Entity.AccountId;
                        obj.AccountNumber = customeracc.Entity.AccountId;
                        obj.AccountTitle = customeracc.Entity.Title;
                    }
                }
                //aspnet
                AspNetUser newuser = new AspNetUser();
                newuser.Address = obj.Address;
                newuser.Email = obj.Email;
                newuser.AdminApproval = false;
                newuser.PasswordHash = encrypter.Encrypt(obj.ConfirmPassword);
                newuser.CreatedDate = DateTime.Now;
                newuser.Deleted = false;
                newuser.DrivingLicense = obj.DrivingLicenseNumber;
                newuser.ZipCode = obj.Zip;
                newuser.PhoneNumber = obj.Phone;
                newuser.IsCancelled = false;
                newuser.IsActive = false;
                if (obj.FirstName != null && obj.LastName != null)
                {
                    newuser.Firstname = obj.FirstName;
                    newuser.Lastname = obj.LastName;
                    obj.FullName = obj.FirstName + obj.LastName;
                    newuser.UserName = obj.FirstName + obj.LastName;
                }
                var GetRole = db.AspNetRoles.ToList().Where(x => x.Name == "Customer").FirstOrDefault();
                if (GetRole == null)
                {
                    AspNetRole newrole = new AspNetRole();
                    newrole.Name = "Customer";
                    newrole.Status = true;
                    var newGenRole = db.AspNetRoles.Add(newrole);
                    await db.SaveChangesAsync();
                    newuser.RoleId = newGenRole.Entity.Id;
                    newuser.RoleName = "Customer";
                }
                else
                {
                    newuser.RoleId = GetRole.Id;
                    newuser.RoleName = "Customer";
                }
                newuser.FromScreen = "E-Commerce";
                var GenUser = db.AspNetUsers.Add(newuser);
                await db.SaveChangesAsync();

                obj.UserId = GenUser.Entity.Id;
                CustomerInformation customerinformation = new CustomerInformation();

                var respcustomer = db.CustomerInformations.ToList();
                if (respcustomer.Count() > 0)
                {
                    var customeroderecord = respcustomer;
                    if (customeroderecord != null && customeroderecord.Count() > 0)
                    {
                        CustomerInformation newcustomer = new CustomerInformation();
                        var fullcode = "";
                        if (customeroderecord[0].CustomerCode != null && customeroderecord[0].CustomerCode != "string" && customeroderecord[0].CustomerCode != "")
                        {
                            int large, small;
                            int CustomerInfoID = 0;
                            large = Convert.ToInt32(customeroderecord[0].CustomerCode);
                            small = Convert.ToInt32(customeroderecord[0].CustomerCode);
                            for (int i = 0; i < customeroderecord.Count(); i++)
                            {
                                if (customeroderecord[i].CustomerCode != null)
                                {
                                    var t = Convert.ToInt32(customeroderecord[i].CustomerCode);
                                    if (Convert.ToInt32(customeroderecord[i].CustomerCode) > large)
                                    {
                                        CustomerInfoID = Convert.ToInt32(customeroderecord[i].Id);
                                        large = Convert.ToInt32(customeroderecord[i].CustomerCode);
                                    }
                                    else if (Convert.ToInt32(customeroderecord[i].CustomerCode) < small)
                                    {
                                        small = Convert.ToInt32(customeroderecord[i].CustomerCode);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            CustomerInfoID = Convert.ToInt32(customeroderecord[i].Id);
                                        }
                                    }
                                }
                            }
                            newcustomer = customeroderecord.ToList().Where(x => x.Id == CustomerInfoID).FirstOrDefault();
                            if (newcustomer != null)
                            {
                                if (newcustomer.CustomerCode != null)
                                {
                                    var VcodeSplit = newcustomer.CustomerCode;
                                    int code = Convert.ToInt32(VcodeSplit) + 1;
                                    if (Convert.ToDouble(VcodeSplit) > 9)
                                    {
                                        fullcode = "00" + Convert.ToString(code);
                                    }
                                    else if (Convert.ToDouble(VcodeSplit) > 99)
                                    {
                                        fullcode = "0" + Convert.ToString(code);
                                    }
                                    else if (Convert.ToDouble(VcodeSplit) > 999)
                                    {
                                        fullcode = Convert.ToString(code);
                                    }
                                    else
                                    {
                                        fullcode = "000" + Convert.ToString(code);
                                    }
                                }
                                else
                                {
                                    fullcode = "000" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "000" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "000" + "1";
                        }
                        customerinformation.CustomerCode = fullcode;
                        obj.CustomerCode = customerinformation.CustomerCode;
                    }
                    else
                    {
                        customerinformation.CustomerCode = "000" + "1";
                        obj.CustomerCode = customerinformation.CustomerCode;
                    }
                }
                else
                {
                    customerinformation.CustomerCode = "000" + "1";
                    obj.CustomerCode = customerinformation.CustomerCode;
                }
                bool checkcustomercode = db.CustomerInformations.ToList().Exists(x => x.CustomerCode.Equals(obj.CustomerCode, StringComparison.CurrentCultureIgnoreCase) && x.Company == obj.Company);
                if (checkcustomercode)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (obj != null)
                {
                    customerinformation.Balance = obj.Balance;
                    customerinformation.Company = obj.Company;
                    customerinformation.Gender = obj.Gender;
                    customerinformation.FirstName = obj.FirstName;
                    customerinformation.LastName = obj.LastName;
                    customerinformation.Street = obj.Street;
                    customerinformation.City = obj.City;
                    customerinformation.StateId = obj.StateId;
                    customerinformation.Country = obj.Country;
                    customerinformation.Phone = obj.Phone;
                    customerinformation.Fax = obj.Fax;
                    customerinformation.CheckAddress = obj.CheckAddress;
                    customerinformation.Email = obj.Email;
                    customerinformation.Cell = obj.Cell;
                    customerinformation.ProviderId = obj.ProviderId;
                    customerinformation.Other = obj.Other;
                    customerinformation.Website = obj.Website;
                    customerinformation.DrivingLicense = obj.DrivingLicense;
                    customerinformation.DrivingLicenseNumber = obj.DrivingLicenseNumber;
                    customerinformation.DrivingLicenseState = obj.DrivingLicenseState;
                    customerinformation.DrivingLicenseStateId = obj.DrivingLicenseStateId;
                    customerinformation.TobaccoLicenseNumber = obj.TobaccoLicenseNumber;
                    customerinformation.CigaretteLicenseNumber = obj.CigaretteLicenseNumber;
                    customerinformation.OwnerAddress = obj.OwnerAddress;
                    customerinformation.BusinessAddress = obj.BusinessAddress;
                    customerinformation.MailingAddress = obj.MailingAddress;
                    customerinformation.TaxIdfein = obj.TaxIdfein;
                    customerinformation.StateIdnumber = obj.StateIdnumber;
                    customerinformation.Vendor = obj.Vendor;
                    customerinformation.Dea = obj.Dea;
                    customerinformation.Memo = obj.Memo;
                    customerinformation.CustomerTypeId = obj.CustomerTypeId;
                    customerinformation.Dob = obj.Dob;
                    customerinformation.Ssn = obj.Ssn;
                    customerinformation.DrivingLicenseNumber = obj.DrivingLicenseNumber;
                    customerinformation.DrivingLicenseStateId = obj.DrivingLicenseStateId;
                    customerinformation.VehicleNumber = obj.VehicleNumber;
                    customerinformation.Authorized = obj.Authorized;
                    customerinformation.FullName = obj.FullName;
                    customerinformation.FromScreen = obj.FromScreen;
                    customerinformation.BusinessName = obj.BusinessName;
                    customerinformation.Mobile = obj.Mobile;
                    customerinformation.CustomerState = obj.CustomerState;
                    customerinformation.StateResaleTaxId = obj.StateResaleTaxId;
                    customerinformation.DrivingLicense = obj.DrivingLicense;
                    customerinformation.CigratteLicenceNumber = obj.CigratteLicenceNumber;
                    customerinformation.PostalCode = obj.PostalCode;
                    customerinformation.RegistrationType = obj.RegistrationType;
                    customerinformation.Approved = false;
                    customerinformation.Rejected = false;
                    customerinformation.Pending = true;
                    customerinformation.UserId = obj.UserId;
                    customerinformation.AccountId = obj.AccountId;
                    customerinformation.AccountNumber = obj.AccountNumber;
                    customerinformation.AccountTitle = obj.AccountTitle;
                    customerinformation.CustomerCode = obj.CustomerCode;

                    if (obj.ProviderId != null && obj.ProviderId != 0)
                    {
                        var getprovider = db.Providers.Find(obj.ProviderId);
                        if (getprovider != null)
                        {
                            customerinformation.Provider = getprovider.Name;
                        }

                    }

                    if (obj.CustomerTypeId != null && obj.CustomerTypeId != 0)
                    {
                        var getcustomer = db.CustomerTypes.Find(obj.CustomerTypeId);
                        if (getcustomer != null)
                        {
                            customerinformation.CustomerType = getcustomer.TypeName;
                        }

                    }

                    if (obj.StateId != null && obj.StateId != 0)
                    {
                        //var getcustomerstate = db.CustomerStates.Find(obj.StateId);
                        //if (getcustomerstate != null)
                        //{
                        //    customerinformation.State = getcustomerstate.StateName;
                        //}

                        customerinformation.State = obj.StateId.ToString();

                    }

                    if (obj.DrivingLicenseStateId != null && obj.DrivingLicenseStateId != 0)
                    {
                        var getdrivinglicense = db.DrivingLicenseStates.Find(obj.DrivingLicenseStateId);
                        if (getdrivinglicense != null)
                        {
                            customerinformation.DrivingLicenseState = getdrivinglicense.Name;
                        }

                    }
                    obj.CustomerClassification = new CustomerClassification();
                    if (obj.CustomerClassification.GroupId != null && obj.CustomerClassification.GroupId != 0)
                    {
                        var getgroup = db.Groups.Find(obj.CustomerClassification.GroupId);
                        if (getgroup != null)
                        {
                            obj.CustomerClassification.GroupName = getgroup.Name;
                        }

                    }

                    if (obj.CustomerClassification.SubGroupId != null && obj.CustomerClassification.SubGroupId != 0)
                    {
                        var getsubgroup = db.SubGroups.Find(obj.CustomerClassification.SubGroupId);
                        if (getsubgroup != null)
                        {
                            obj.CustomerClassification.SubGroupName = getsubgroup.SubGroupName;
                        }

                    }


                    if (obj.CustomerClassification.BusinessTypeId != null && obj.CustomerClassification.BusinessTypeId != 0)
                    {
                        var getbusiness = db.BusinessTypes.Find(obj.CustomerClassification.BusinessTypeId);
                        if (getbusiness != null)
                        {
                            obj.CustomerClassification.BusinessType = getbusiness.TypeName;
                        }

                    }
                    if (obj.CustomerClassification.ZoneId != null && obj.CustomerClassification.ZoneId != 0)
                    {
                        var getzone = db.Zones.Find(obj.CustomerClassification.ZoneId);
                        if (getzone != null)
                        {
                            obj.CustomerClassification.Zone = getzone.Name;
                        }

                    }
                    if (obj.CustomerClassification.SalesmanId != null && obj.CustomerClassification.SalesmanId != 0)
                    {
                        var getsaleman = db.Salesmen.Find(obj.CustomerClassification.SalesmanId);
                        if (getsaleman != null)
                        {
                            obj.CustomerClassification.Salesman = getsaleman.Name;
                        }

                    }
                    if (obj.CustomerClassification.ShippedViaId != null && obj.CustomerClassification.ShippedViaId != 0)
                    {
                        var getshipment = db.ShipmentPurchases.Find(obj.CustomerClassification.ShippedViaId);
                        if (getshipment != null)
                        {
                            customerinformation.CustomerClassification.ShippedVia = getshipment.Type;
                        }

                    }
                    if (obj.CustomerClassification.RouteId != null && obj.CustomerClassification.RouteId != 0)
                    {
                        var getroute = db.Routes.Find(obj.CustomerClassification.RouteId);
                        if (getroute != null)
                        {
                            obj.CustomerClassification.RouteName = getroute.RouteName;
                        }

                    }
                    if (obj.CustomerClassification.DriverId != null && obj.CustomerClassification.DriverId != 0)
                    {
                        var getdriver = db.Drivers.Find(obj.CustomerClassification.DriverId);
                        if (getdriver != null)
                        {
                            obj.CustomerClassification.DriverName = getdriver.Name;
                        }

                    }
                    if (obj.CustomerClassification.ShiptoReferenceId != null && obj.CustomerClassification.ShiptoReferenceId != 0)
                    {
                        var getshipreference = db.ShiptoReferences.Find(obj.CustomerClassification.ShiptoReferenceId);
                        if (getshipreference != null)
                        {
                            obj.CustomerClassification.ShiptoReference = getshipreference.Name;
                        }
                    }
                    obj.CustomerBillingInfo = new CustomerBillingInfo();
                    if (obj.CustomerBillingInfo.PaymentTermsId != null && obj.CustomerBillingInfo.PaymentTermsId != 0)
                    {
                        var getpaymentterm = db.PaymentTerms.Find(obj.CustomerBillingInfo.PaymentTermsId);
                        if (getpaymentterm != null)
                        {
                            obj.CustomerBillingInfo.PaymentTerms = getpaymentterm.Name;
                        }
                    }
                    if (obj.CustomerBillingInfo.PricingId != null && obj.CustomerBillingInfo.PricingId != 0)
                    {
                        var getpricing = db.Pricings.Find(obj.CustomerBillingInfo.PricingId);
                        if (getpricing != null)
                        {
                            obj.CustomerBillingInfo.Pricing = getpricing.Name;
                        }
                    }
                }



                // customerinformation.pending
                var record = db.CustomerInformations.Add(customerinformation);
                await db.SaveChangesAsync();



                CustomerClassification customerclassification = new CustomerClassification();

                if (record.Entity.CustomerClassification != null)
                {
                    customerclassification.CustomerInfoId = record.Entity.Id;
                    customerclassification.CustomerCode = record.Entity.CustomerCode;
                    customerclassification.GroupId = obj.CustomerClassification.GroupId;
                    customerclassification.GroupName = obj.CustomerClassification.GroupName;
                    customerclassification.SubGroupId = obj.CustomerClassification.SubGroupId;
                    customerclassification.SubGroupName = obj.CustomerClassification.SubGroupName;
                    customerclassification.ZoneId = obj.CustomerClassification.ZoneId;
                    customerclassification.Zone = obj.CustomerClassification.Zone;
                    customerclassification.SalesmanId = obj.CustomerClassification.SalesmanId;
                    customerclassification.Salesman = obj.CustomerClassification.Salesman;
                    customerclassification.ShippedViaId = obj.CustomerClassification.ShippedViaId;
                    customerclassification.ShippedVia = obj.CustomerClassification.ShippedVia;
                    customerclassification.RouteId = obj.CustomerClassification.RouteId;
                    customerclassification.RouteName = obj.CustomerClassification.RouteName;
                    customerclassification.RouteDeliveryDay = obj.CustomerClassification.RouteDeliveryDay;
                    customerclassification.DriverId = obj.CustomerClassification.DriverId;
                    customerclassification.DriverName = obj.CustomerClassification.DriverName;
                    customerclassification.ShiptoReferenceId = obj.CustomerClassification.ShiptoReferenceId;
                    customerclassification.ShiptoReference = obj.CustomerClassification.ShiptoReference;
                    customerclassification.OutOfStateCustomer = obj.CustomerClassification.OutOfStateCustomer;
                    customerclassification.AddtoMaillingList = obj.CustomerClassification.AddtoMaillingList;
                    customerclassification.AddtoemailTextList = obj.CustomerClassification.AddtoemailTextList;
                    customerclassification.RejectPromotion = obj.CustomerClassification.RejectPromotion;
                    customerclassification.ViewInvoicePrevBalance = obj.CustomerClassification.ViewInvoicePrevBalance;
                    customerclassification.ViewRetailandDiscount = obj.CustomerClassification.ViewRetailandDiscount;
                    customerclassification.BarCodeId = obj.CustomerClassification.BarCodeId;
                    customerclassification.BarCode = obj.CustomerClassification.BarCode;
                    customerclassification.SpecialInvoiceCustom = obj.CustomerClassification.SpecialInvoiceCustom;
                    customerclassification.OtherCustomerReference = obj.CustomerClassification.OtherCustomerReference;
                    customerclassification.UseDefaultInvMemo = obj.CustomerClassification.UseDefaultInvMemo;
                    customerclassification.InvoiceMemo = obj.CustomerClassification.InvoiceMemo;
                }
                else
                {
                    customerclassification.CustomerInfoId = record.Entity.Id;
                    customerclassification.CustomerCode = record.Entity.CustomerCode;
                    customerclassification.GroupId = obj.CustomerClassification.GroupId;
                    customerclassification.GroupName = obj.CustomerClassification.GroupName;
                    customerclassification.SubGroupId = obj.CustomerClassification.SubGroupId;
                    customerclassification.SubGroupName = obj.CustomerClassification.SubGroupName;
                    customerclassification.ZoneId = obj.CustomerClassification.ZoneId;
                    customerclassification.Zone = obj.CustomerClassification.Zone;
                    customerclassification.SalesmanId = obj.CustomerClassification.SalesmanId;
                    customerclassification.Salesman = obj.CustomerClassification.Salesman;
                    customerclassification.ShippedViaId = obj.CustomerClassification.ShippedViaId;
                    customerclassification.ShippedVia = obj.CustomerClassification.ShippedVia;
                    customerclassification.RouteId = obj.CustomerClassification.RouteId;
                    customerclassification.RouteName = obj.CustomerClassification.RouteName;
                    customerclassification.RouteDeliveryDay = obj.CustomerClassification.RouteDeliveryDay;
                    customerclassification.DriverId = obj.CustomerClassification.DriverId;
                    customerclassification.DriverName = obj.CustomerClassification.DriverName;
                    customerclassification.ShiptoReferenceId = obj.CustomerClassification.ShiptoReferenceId;
                    customerclassification.ShiptoReference = obj.CustomerClassification.ShiptoReference;
                    customerclassification.OutOfStateCustomer = obj.CustomerClassification.OutOfStateCustomer;
                    customerclassification.AddtoMaillingList = obj.CustomerClassification.AddtoMaillingList;
                    customerclassification.AddtoemailTextList = obj.CustomerClassification.AddtoemailTextList;
                    customerclassification.RejectPromotion = obj.CustomerClassification.RejectPromotion;
                    customerclassification.ViewInvoicePrevBalance = obj.CustomerClassification.ViewInvoicePrevBalance;
                    customerclassification.ViewRetailandDiscount = obj.CustomerClassification.ViewRetailandDiscount;
                    customerclassification.BarCodeId = obj.CustomerClassification.BarCodeId;
                    customerclassification.BarCode = obj.CustomerClassification.BarCode;
                    customerclassification.SpecialInvoiceCustom = obj.CustomerClassification.SpecialInvoiceCustom;
                    customerclassification.OtherCustomerReference = obj.CustomerClassification.OtherCustomerReference;
                    customerclassification.UseDefaultInvMemo = obj.CustomerClassification.UseDefaultInvMemo;
                    customerclassification.InvoiceMemo = obj.CustomerClassification.InvoiceMemo;
                }
                var classificationrecord = db.CustomerClassifications.Add(customerclassification);
                await db.SaveChangesAsync();
                CustomerBillingInfo customerbillinginfo = new CustomerBillingInfo();

                if (record.Entity.CustomerBillingInfo != null)
                {
                    customerbillinginfo.CustomerInformationId = record.Entity.Id;
                    customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
                    customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;
                    customerbillinginfo.IsTaxExempt = obj.CustomerBillingInfo.IsTaxExempt;
                    customerbillinginfo.PricingId = obj.CustomerBillingInfo.PricingId;
                    customerbillinginfo.Pricing = obj.CustomerBillingInfo.Pricing;
                    customerbillinginfo.RetailPlus = obj.CustomerBillingInfo.RetailPlus;
                    customerbillinginfo.RetailPlusPercentage = obj.CustomerBillingInfo.RetailPlusPercentage;
                    customerbillinginfo.IsGetSalesDiscounts = obj.CustomerBillingInfo.IsGetSalesDiscounts;
                    customerbillinginfo.IsOutOfStateCustomer = obj.CustomerBillingInfo.IsOutOfStateCustomer;
                    customerbillinginfo.AdditionalInvoiceCharge = obj.CustomerBillingInfo.AdditionalInvoiceCharge;
                    customerbillinginfo.AdditionalInvoiceDiscount = obj.CustomerBillingInfo.AdditionalInvoiceDiscount;
                    customerbillinginfo.ScheduleMessage = obj.CustomerBillingInfo.ScheduleMessage;
                    customerbillinginfo.ScheduleMessageFromDate = obj.CustomerBillingInfo.ScheduleMessageFromDate;
                    customerbillinginfo.ScheduleMessageToDate = obj.CustomerBillingInfo.ScheduleMessageToDate;
                    customerbillinginfo.PaymentTermsId = obj.CustomerBillingInfo.PaymentTermsId;
                    customerbillinginfo.PaymentTerms = obj.CustomerBillingInfo.PaymentTerms;
                    customerbillinginfo.CreditLimit = obj.CustomerBillingInfo.CreditLimit;
                    customerbillinginfo.IsCreditHold = obj.CustomerBillingInfo.IsCreditHold;
                    customerbillinginfo.IsBillToBill = obj.CustomerBillingInfo.IsBillToBill;
                    customerbillinginfo.IsNoCheckAccepted = obj.CustomerBillingInfo.IsNoCheckAccepted;
                    customerbillinginfo.IsExclude = obj.CustomerBillingInfo.IsExclude;
                    customerbillinginfo.ThirdPartyCheckCharge = obj.CustomerBillingInfo.ThirdPartyCheckCharge;
                    customerbillinginfo.IsCashBackBalance = obj.CustomerBillingInfo.IsCashBackBalance;
                    customerbillinginfo.CashBackBalance = obj.CustomerBillingInfo.CashBackBalance;
                    customerbillinginfo.IsPopupMessage = obj.CustomerBillingInfo.IsPopupMessage;
                    customerbillinginfo.PopupMessage = obj.CustomerBillingInfo.PopupMessage;
                }
                else
                {
                    customerbillinginfo.CustomerInformationId = record.Entity.Id;
                    customerbillinginfo.CustomerCode = record.Entity.CustomerCode;
                    customerbillinginfo.CustomerClassificationId = classificationrecord.Entity.Id;
                    customerbillinginfo.IsTaxExempt = obj.CustomerBillingInfo.IsTaxExempt;
                    customerbillinginfo.PricingId = obj.CustomerBillingInfo.PricingId;
                    customerbillinginfo.Pricing = obj.CustomerBillingInfo.Pricing;
                    customerbillinginfo.RetailPlus = obj.CustomerBillingInfo.RetailPlus;
                    customerbillinginfo.RetailPlusPercentage = obj.CustomerBillingInfo.RetailPlusPercentage;
                    customerbillinginfo.IsGetSalesDiscounts = obj.CustomerBillingInfo.IsGetSalesDiscounts;
                    customerbillinginfo.IsOutOfStateCustomer = obj.CustomerBillingInfo.IsOutOfStateCustomer;
                    customerbillinginfo.AdditionalInvoiceCharge = obj.CustomerBillingInfo.AdditionalInvoiceCharge;
                    customerbillinginfo.AdditionalInvoiceDiscount = obj.CustomerBillingInfo.AdditionalInvoiceDiscount;
                    customerbillinginfo.ScheduleMessage = obj.CustomerBillingInfo.ScheduleMessage;
                    customerbillinginfo.ScheduleMessageFromDate = obj.CustomerBillingInfo.ScheduleMessageFromDate;
                    customerbillinginfo.ScheduleMessageToDate = obj.CustomerBillingInfo.ScheduleMessageToDate;
                    customerbillinginfo.PaymentTermsId = obj.CustomerBillingInfo.PaymentTermsId;
                    customerbillinginfo.PaymentTerms = obj.CustomerBillingInfo.PaymentTerms;
                    customerbillinginfo.CreditLimit = obj.CustomerBillingInfo.CreditLimit;
                    customerbillinginfo.IsCreditHold = obj.CustomerBillingInfo.IsCreditHold;
                    customerbillinginfo.IsBillToBill = obj.CustomerBillingInfo.IsBillToBill;
                    customerbillinginfo.IsNoCheckAccepted = obj.CustomerBillingInfo.IsNoCheckAccepted;
                    customerbillinginfo.IsExclude = obj.CustomerBillingInfo.IsExclude;
                    customerbillinginfo.ThirdPartyCheckCharge = obj.CustomerBillingInfo.ThirdPartyCheckCharge;
                    customerbillinginfo.IsCashBackBalance = obj.CustomerBillingInfo.IsCashBackBalance;
                    customerbillinginfo.CashBackBalance = obj.CustomerBillingInfo.CashBackBalance;
                    customerbillinginfo.IsPopupMessage = obj.CustomerBillingInfo.IsPopupMessage;
                    customerbillinginfo.PopupMessage = obj.CustomerBillingInfo.PopupMessage;
                }
                db.CustomerBillingInfos.Add(customerbillinginfo);
                await db.SaveChangesAsync();

                //paper work
                CustomerPaperWork customerpaperwork = new CustomerPaperWork();

                if (record.Entity.paperWork != null)
                {

                    customerpaperwork.CustomerId = record.Entity.Id;
                    customerpaperwork.UserId = GenUser.Entity.Id;
                    customerpaperwork.FederalForm = obj.paperWork.FederalForm;
                    if (customerpaperwork.FederalForm != null && customerpaperwork.FederalForm.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.FederalForm, "pdf");
                        customerpaperwork.FederalForm = null;
                        customerpaperwork.FederalFormPath = imgPath.ToString();
                    }
                    customerpaperwork.SalesTaxId = obj.paperWork.SalesTaxId;
                    if (customerpaperwork.SalesTaxId != null && customerpaperwork.SalesTaxId.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.SalesTaxId, "pdf");
                        customerpaperwork.SalesTaxId = null;
                        customerpaperwork.SalesTaxIdpath = imgPath.ToString();
                    }
                    customerpaperwork.DrivingLicenseId = obj.paperWork.DrivingLicenseId;
                    if (customerpaperwork.DrivingLicenseId != null && customerpaperwork.DrivingLicenseId.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.DrivingLicenseId, "pdf");
                        customerpaperwork.DrivingLicenseId = null;
                        customerpaperwork.DrivingLicenseIdpath = imgPath.ToString();
                    }
                    customerpaperwork.UploadedDate = DateTime.Now;
                    customerpaperwork.FromScreen = obj.FromScreen;
                }
                else
                {
                    customerpaperwork.CustomerId = record.Entity.Id;
                    customerpaperwork.UserId = GenUser.Entity.Id;
                    customerpaperwork.FederalForm = obj.paperWork.FederalForm;
                    if (customerpaperwork.FederalForm != null && customerpaperwork.FederalForm.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.FederalForm, "pdf");
                        customerpaperwork.FederalForm = null;
                        customerpaperwork.FederalFormPath = imgPath.Result.ToString();
                    }
                    customerpaperwork.SalesTaxId = obj.paperWork.SalesTaxId;
                    if (customerpaperwork.SalesTaxId != null && customerpaperwork.SalesTaxId.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.SalesTaxId, "pdf");
                        customerpaperwork.SalesTaxId = null;
                        customerpaperwork.SalesTaxIdpath = imgPath.Result.ToString();
                    }
                    customerpaperwork.DrivingLicenseId = obj.paperWork.DrivingLicenseId;
                    if (customerpaperwork.DrivingLicenseId != null && customerpaperwork.DrivingLicenseId.Length > 0)
                    {
                        var imgPath = SaveFileAsync(customerpaperwork.DrivingLicenseId, "pdf");
                        customerpaperwork.DrivingLicenseId = null;
                        customerpaperwork.DrivingLicenseIdpath = imgPath.Result.ToString();
                    }
                    customerpaperwork.UploadedDate = DateTime.Now;
                    customerpaperwork.FromScreen = obj.FromScreen;

                }
                db.CustomerPaperWorks.Add(customerpaperwork);
                await db.SaveChangesAsync();


                CertificateExemptionInstruction certificateexemptioninstruction = new CertificateExemptionInstruction();

                if (record.Entity.CertificateExemptionInstructions != null)
                {

                    certificateexemptioninstruction.PostalAbbreviation = obj.CertificateExemptionInstructions.PostalAbbreviation;
                    certificateexemptioninstruction.CertificateSinglePurchase = obj.CertificateExemptionInstructions.CertificateSinglePurchase;
                    certificateexemptioninstruction.InvoicePurchaseOrderNo = obj.CertificateExemptionInstructions.InvoicePurchaseOrderNo;
                    certificateexemptioninstruction.PurchaserName = obj.CertificateExemptionInstructions.PurchaserName;
                    certificateexemptioninstruction.BusinessAddress = obj.CertificateExemptionInstructions.BusinessAddress;
                    certificateexemptioninstruction.PurchaserCity = obj.CertificateExemptionInstructions.PurchaserCity;
                    certificateexemptioninstruction.PurchaserState = obj.CertificateExemptionInstructions.PurchaserState;
                    certificateexemptioninstruction.PurchaserZipCode = obj.CertificateExemptionInstructions.PurchaserZipCode;
                    certificateexemptioninstruction.PurchaseTaxId = obj.CertificateExemptionInstructions.PurchaseTaxId;
                    certificateexemptioninstruction.StateIssue = obj.CertificateExemptionInstructions.StateIssue;
                    certificateexemptioninstruction.CountryIssue = obj.CertificateExemptionInstructions.CountryIssue;
                    certificateexemptioninstruction.Fein = obj.CertificateExemptionInstructions.Fein;
                    certificateexemptioninstruction.DrivingLicenseNo = obj.CertificateExemptionInstructions.DrivingLicenseNo;
                    certificateexemptioninstruction.SellerName = obj.CertificateExemptionInstructions.SellerName;
                    certificateexemptioninstruction.SellerAdress = obj.CertificateExemptionInstructions.SellerAdress;
                    certificateexemptioninstruction.SellerCity = obj.CertificateExemptionInstructions.SellerCity;
                    certificateexemptioninstruction.SellerState = obj.CertificateExemptionInstructions.SellerState;
                    certificateexemptioninstruction.SellerZipCode = obj.CertificateExemptionInstructions.SellerZipCode;
                    certificateexemptioninstruction.TermsCondition = obj.CertificateExemptionInstructions.TermsCondition;
                    certificateexemptioninstruction.CreatedDate = DateTime.Now;
                    certificateexemptioninstruction.CustomerId = record.Entity.Id;
                    certificateexemptioninstruction.FeinCountry = obj.CertificateExemptionInstructions.FeinCountry;
                    certificateexemptioninstruction.Signature = obj.CertificateExemptionInstructions.Signature;
                    if (certificateexemptioninstruction.Signature != null && certificateexemptioninstruction.Signature.Length > 0)
                    {
                        var imgPath = SaveImage(certificateexemptioninstruction.Signature, Guid.NewGuid().ToString());
                        certificateexemptioninstruction.Signature = null;
                        certificateexemptioninstruction.SignatureByPath = imgPath.ToString();
                    }
                }
                else
                {
                    certificateexemptioninstruction.PostalAbbreviation = obj.CertificateExemptionInstructions.PostalAbbreviation;
                    certificateexemptioninstruction.CertificateSinglePurchase = obj.CertificateExemptionInstructions.CertificateSinglePurchase;
                    certificateexemptioninstruction.InvoicePurchaseOrderNo = obj.CertificateExemptionInstructions.InvoicePurchaseOrderNo;
                    certificateexemptioninstruction.PurchaserName = obj.CertificateExemptionInstructions.PurchaserName;
                    certificateexemptioninstruction.BusinessAddress = obj.CertificateExemptionInstructions.BusinessAddress;
                    certificateexemptioninstruction.PurchaserCity = obj.CertificateExemptionInstructions.PurchaserCity;
                    certificateexemptioninstruction.PurchaserState = obj.CertificateExemptionInstructions.PurchaserState;
                    certificateexemptioninstruction.PurchaserZipCode = obj.CertificateExemptionInstructions.PurchaserZipCode;
                    certificateexemptioninstruction.PurchaseTaxId = obj.CertificateExemptionInstructions.PurchaseTaxId;
                    certificateexemptioninstruction.StateIssue = obj.CertificateExemptionInstructions.StateIssue;
                    certificateexemptioninstruction.CountryIssue = obj.CertificateExemptionInstructions.CountryIssue;
                    certificateexemptioninstruction.Fein = obj.CertificateExemptionInstructions.Fein;
                    certificateexemptioninstruction.DrivingLicenseNo = obj.CertificateExemptionInstructions.DrivingLicenseNo;
                    certificateexemptioninstruction.SellerName = obj.CertificateExemptionInstructions.SellerName;
                    certificateexemptioninstruction.SellerAdress = obj.CertificateExemptionInstructions.SellerAdress;
                    certificateexemptioninstruction.SellerCity = obj.CertificateExemptionInstructions.SellerCity;
                    certificateexemptioninstruction.SellerState = obj.CertificateExemptionInstructions.SellerState;
                    certificateexemptioninstruction.SellerZipCode = obj.CertificateExemptionInstructions.SellerZipCode;
                    certificateexemptioninstruction.TermsCondition = obj.CertificateExemptionInstructions.TermsCondition;
                    certificateexemptioninstruction.CreatedDate = DateTime.Now;
                    certificateexemptioninstruction.CustomerId = record.Entity.Id;
                    certificateexemptioninstruction.FeinCountry = obj.CertificateExemptionInstructions.FeinCountry;
                    certificateexemptioninstruction.Signature = obj.CertificateExemptionInstructions.Signature;
                    if (certificateexemptioninstruction.Signature != null && certificateexemptioninstruction.Signature.Length > 0)
                    {
                        var imgPath = SaveImage(certificateexemptioninstruction.Signature, Guid.NewGuid().ToString());
                        certificateexemptioninstruction.Signature = null;
                        certificateexemptioninstruction.SignatureByPath = imgPath.ToString();
                    }

                }
                var certificate = db.CertificateExemptionInstructions.Add(certificateexemptioninstruction);
                await db.SaveChangesAsync();

                if (obj.CertificateReasonExemptions != null)
                {
                    for (int i = 0; i < obj.CertificateReasonExemptions.Count(); i++)
                    {
                        CertificateReasonExemption certificatereasonexemption = new CertificateReasonExemption();

                        if (record.Entity.CertificateReasonExemptions != null)
                        {
                            certificatereasonexemption.CertificateId = record.Entity.Id;
                            certificatereasonexemption.CustomerId = classificationrecord.Entity.Id;
                            certificatereasonexemption.Reason = obj.CertificateReasonExemptions[i].Reason;
                            certificatereasonexemption.Text = obj.CertificateReasonExemptions[i].Text;

                        }
                        else
                        {
                            certificatereasonexemption.CertificateId = record.Entity.Id;
                            certificatereasonexemption.CustomerId = classificationrecord.Entity.Id;
                            certificatereasonexemption.Reason = obj.CertificateReasonExemptions[i].Reason;
                            certificatereasonexemption.Text = obj.CertificateReasonExemptions[i].Text;


                        }
                        db.CertificateReasonExemptions.Add(certificatereasonexemption);
                        await db.SaveChangesAsync();
                    }
                }
                if (obj.CertificateBusinessTypes != null)
                {
                    for (int i = 0; i < obj.CertificateBusinessTypes.Count(); i++)
                    {
                        CertificateBusinessType certificatebusinesstype = new CertificateBusinessType();

                        if (record.Entity.CertificateBusinessTypes != null)
                        {
                            if (certificate.Entity.Ceiid != null)
                            {
                                certificatebusinesstype.CertificateId = certificate.Entity.Ceiid;
                            }
                            certificatebusinesstype.CertificateId = record.Entity.Id;
                            certificatebusinesstype.CustomerId = record.Entity.Id;
                            certificatebusinesstype.Type = obj.CertificateBusinessTypes[i].Type;

                        }
                        else
                        {
                            if (certificate.Entity.Ceiid != null)
                            {
                                certificatebusinesstype.CertificateId = certificate.Entity.Ceiid;
                            }
                            certificatebusinesstype.CustomerId = classificationrecord.Entity.Id;
                            certificatebusinesstype.Type = obj.CertificateBusinessTypes[i].Type;


                        }
                        db.CertificateBusinessTypes.Add(certificatebusinesstype);
                        await db.SaveChangesAsync();
                    }
                }

                if (obj.CertificateIdentifications != null)
                {
                    for (int i = 0; i < obj.CertificateIdentifications.Count(); i++)
                    {
                        CertificateIdentification certificateidentification = new CertificateIdentification();

                        if (obj.CertificateIdentifications != null)
                        {
                            certificateidentification.CustomerId = record.Entity.Id;
                            if (certificate.Entity.Ceiid != null)
                            {
                                certificateidentification.CertificateId = certificate.Entity.Ceiid;
                            }

                            certificateidentification.ReasonExamption = obj.CertificateIdentifications[i].ReasonExamption;
                            certificateidentification.IdentificationNumber = obj.CertificateIdentifications[i].IdentificationNumber;
                            certificateidentification.State = obj.CertificateIdentifications[i].State;
                        }
                        else
                        {
                            certificateidentification.CustomerId = record.Entity.Id;
                            if (certificate.Entity.Ceiid != null)
                            {
                                certificateidentification.CertificateId = certificate.Entity.Ceiid;
                            }
                            certificateidentification.ReasonExamption = obj.CertificateIdentifications[i].ReasonExamption;
                            certificateidentification.IdentificationNumber = obj.CertificateIdentifications[i].IdentificationNumber;
                            certificateidentification.State = obj.CertificateIdentifications[i].State;

                        }
                        db.CertificateIdentifications.Add(certificateidentification);
                        await db.SaveChangesAsync();
                    }
                }
                //
                var name = obj.FirstName + obj.LastName;
                if (name == null)
                {
                    name = obj.FullName;
                }
                bool isEmailSent = emailService.SendNewCustomerEmail("NewCustomer", obj.Email, name);
                if (isEmailSent)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);

                }
                else
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Success_WithOutEmail, null, null);

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
        //http://199.231.160.216/abc.website.api/
        //http://localhost:54440/
        //http://199.231.160.216/abcdwebapi/Images/
        //http://204.2.194.223/abcdadminapi/
        //http://10.10.10.98:5595/
        protected string SaveImage(byte[] str, string ImgName)
        {
            string hostRootPath = env.WebRootPath;
            string webRootPath = env.ContentRootPath;
            string imgPath = string.Empty;

            if (!string.IsNullOrEmpty(webRootPath))
            {
                string path = webRootPath + "\\Images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"Images/{imageName}";
            }
            else if (!string.IsNullOrEmpty(hostRootPath))
            {
                string path = hostRootPath + "\\Images\\";
                string imageName = ImgName + ".jpg";
                imgPath = Path.Combine(path, imageName);
                byte[] bytes = str;
                System.IO.File.WriteAllBytes(imgPath, bytes);
                imgPath = $"http://10.10.10.98:5595/abcdwebapi/images/{imageName}"; ;
            }
            return imgPath;
        }

        protected async Task<string> SaveFileAsync(byte[] file, string extention)
        {
            try
            {
                string hostRootPath = env.WebRootPath;
                string webRootPath = env.ContentRootPath;
                string _decumentPath = string.Empty;
                string _decuments = Guid.NewGuid().ToString();
                if (!string.IsNullOrEmpty(webRootPath))
                {

                    string path = webRootPath + "\\Images\\";
                    string _decumentName = _decuments + "." + extention;
                    _decumentPath = Path.Combine(path, _decumentName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return $"http://10.10.10.98:5595/abcdwebapi/images/{_decumentName}";
                }
                else if (!string.IsNullOrEmpty(hostRootPath))
                {
                    string path = hostRootPath + "\\Images\\";
                    string _decumentsName = _decuments + "." + extention;
                    _decumentPath = Path.Combine(path, _decumentsName);
                    byte[] bytes = file;
                    System.IO.File.WriteAllBytes(_decumentPath, bytes);
                    return _decumentPath;
                }
                _decumentPath = _decumentPath.Replace(" ", "");
                _decumentPath = _decumentPath.Substring(1);
                return _decumentPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpGet("OpenItemSubCategoryGetById")]
        public IActionResult OpenItemSubCategoryGetById(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ItemSubCategory>>();
                var record = db.ItemSubCategories.Where(x => x.CategoryId == id).ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ItemSubCategory>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
