using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class TerminalController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IMailService mailService;
       // GenericPrograms emailsend = new GenericPrograms();
        public TerminalController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
            this.mailService = mailService;

        }
        //Terminal  Start
        [HttpGet("TerminalGet")]
        public IActionResult TerminalGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<Terminal>>();
                var record = db.Terminals.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);

             
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TerminalCreate")]
        public async Task<IActionResult> TerminalCreate(Terminal obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Terminals.ToList().Exists(p => p.TerminalName.Equals(obj.TerminalName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Name_AlreadyExists, null, null);

                }

                bool checkpin = db.Terminals.ToList().Exists(p => p.SecurityPin.Equals(obj.SecurityPin, StringComparison.CurrentCultureIgnoreCase));
                if (checkpin)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                obj.CreatedDate = DateTime.Now;
                db.Terminals.Add(obj);
                await db.SaveChangesAsync();
               // return CreatedAtAction("TerminalGet", new { id = obj.TerminalId }, obj);

                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("TerminalUpdate/{id}")]
        public async Task<IActionResult> TerminalUpdate(int id, Terminal data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.Terminals.ToList().Exists(x => x.TerminalName.Equals(data.TerminalName, StringComparison.CurrentCultureIgnoreCase) && x.TerminalId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Name_AlreadyExists, null, null);
                }
                if (id != data.TerminalId)
                {
                    return BadRequest();
                }
                var record = await db.Terminals.FindAsync(id);
                if (data.TerminalName != null && data.TerminalName != "undefined")
                {
                    record.TerminalName = data.TerminalName;
                }
                if (data.TerminalNumber != null && data.TerminalNumber != "undefined")
                {
                    record.TerminalNumber = data.TerminalNumber;
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
                    var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
          
        }
        [HttpGet("TerminalByID/{id}")]
        public IActionResult TerminalByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                var record = db.Terminals.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteTerminal/{id}")]
        public async Task<IActionResult> DeleteTerminal(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();

                Terminal data = await db.Terminals.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                db.Terminals.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("MatchTerminal/{id}")]
        public async Task<IActionResult> MatchTerminal(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                Terminal record = db.Terminals.ToList().Where(x=>x.SecurityPin ==  Convert.ToString(id)).FirstOrDefault();
                if (record == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, record);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Terminal End


        //Terminal Access Start

        [HttpGet("TerminalAcccessGet")]
        public IActionResult TerminalAcccessGet()
        {
            try
            {
               
                var Response = ResponseBuilder.BuildWSResponse<List<TerminalAccess>>();
                var record = db.TerminalAccesses.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<TerminalAccess>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("TerminalAcccessCreate")]
        public async Task<IActionResult> TerminalAcccessCreate(TerminalAccess obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<TerminalAccess>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                obj.AccessDate = DateTime.Now;
                obj.StartTime = (DateTime.Now.Hour + ":" + DateTime.Now.Minute).ToString();
                var getterminal = db.Terminals.ToList().Where(x => x.SecurityPin == obj.TerminalNumber).FirstOrDefault();
                if(getterminal != null)
                {
                    obj.TerminalId = getterminal.TerminalId;
                    obj.TerminalNumber = getterminal.TerminalName + "-" +getterminal.TerminalNumber;
                }
                db.TerminalAccesses.Add(obj);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<TerminalAccess>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("TerminalAccessUpdate/{id}")]
        public async Task<IActionResult> TerminalAccessUpdate(int id, TerminalAccess data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Terminal>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var terminalaccessobj = await db.TerminalAccesses.FindAsync(id);
               
                if (id != data.TerminalAccessId)
                {
                    return BadRequest();
                }
                terminalaccessobj.CloseTime=(DateTime.Now.Hour + ":" + DateTime.Now.Minute).ToString();
                data = terminalaccessobj;
                db.Entry(data).State = EntityState.Modified;
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<TerminalAccess>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
           
        }

        //Terminal Access End
    }
}
