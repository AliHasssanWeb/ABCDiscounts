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


namespace ABC.Admin.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogsController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        public ActivityLogsController(ABCDiscountsContext _db)
        {
            db = _db;
           

        }
        [HttpGet("ActivityLogsGet")]
        public IActionResult ActivityLogsGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ActivityLog>>();
                var record = db.ActivityLogs.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("ActivityLogsCreate")]
        public async Task<IActionResult> ActivityLogsCreate(ActivityLog obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<ActivityLog>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                //ActivityLog log = new ActivityLog();
                //var count = db.ActivityLogs.ToList().Count();
                //var userNameis = obj.CreatedBy;
                //var completemessage = obj.NewDetails;
                //log.CreatedBy = userNameis;
                //log.CreatedDate = DateTime.Now;
                //log.NewDetails = completemessage;
                //log.OperationName = obj.OperationName;
                //log.LogTime = (count + 1).ToString();
                //db.ActivityLogs.Add(log);
                //db.SaveChanges();
                db.ActivityLogs.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<ActivityLog>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("AdminActivityLogsGet")]
        public IActionResult AdminActivityLogsGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<ActivityLog>>();
                var record = db.ActivityLogs.ToList().Where(x=>x.Extraone == "Admin").ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
