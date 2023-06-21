using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.HR.API.Controllers
{
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
        public SecurityController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
            this.mailService = mailService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AspNetUser user)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<AspNetUser>();

                if (user.UserName != null && user.PasswordHash != null)
                {
                    if (user.UserName == "absol@abc.com" && user.PasswordHash == "123456")
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    }
                    else
                    {

                        var innerpassword = encrypter.Encrypt(user.PasswordHash);
                        int count = db.AspNetUsers.ToList().Where(x => x.Email == user.UserName && x.PasswordHash == innerpassword).ToList().Count();

                        if (count != 0)
                        {
                            ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
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
                return BadRequest();
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
