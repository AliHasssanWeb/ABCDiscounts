using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.POS.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]

    public class SecurityController : ControllerBase
    {
        protected readonly ABCDiscountsContext db ;
        public EncryptDecrypt encrypter = new EncryptDecrypt();
        private const string secretKey = "this_is_my_case_secret-Key-for-token_generation";
        public static readonly SymmetricSecurityKey signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        public SecurityController(ABCDiscountsContext _db)
        {
           this.db = _db;
        }

        [HttpPost("PosLogin")]
        public IActionResult PosLogin(AspNetUser user)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();

                if (user.Email != null && user.PasswordHash != null)
                {
                    if (user.UserName == "absol@abc.com" && user.PasswordHash == "123456")
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {

                        var innerpassword = encrypter.Encrypt(user.PasswordHash);
                            var FoundUser = db.AspNetUsers.ToList().Where(x => x.Email == user.Email && x.PasswordHash == innerpassword).FirstOrDefault();
                        if (FoundUser != null)
                        {
                            DateTime Datee = DateTime.Now;
                            FoundUser.LastLogin = Datee;

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

        [HttpGet("RoleGet")]
        public IActionResult RoleGet()
        {
            try
            {
                var record = db.AspNetRoles.ToList();
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


        [AllowAnonymous]
        [HttpGet("OpenItemFetch")]
        public IActionResult OpenItemFetch()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                var record = db.Products.ToList();
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
        [HttpGet("OpenOptimizeItems")]
        public IActionResult OpenOptimizeItems()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Product>>();
                List<Product> dataObject = new List<Product>();
                dataObject = (from e in db.Products
                              orderby e.Description
                              select new Product { Id = e.Id, Name = e.Name, Retail = e.Retail }
                              ).ToList();
                //var record = db.Products.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, dataObject);
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
    }
}
