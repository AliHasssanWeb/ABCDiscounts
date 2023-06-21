using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using ABC.Shared.Interface;
using ABC.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StatusCodes = ABC.Shared.DataConfig.StatusCodes;

namespace ABC.HR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;
        private readonly IMailService mailService;
        public ConfigurationController(ABCDiscountsContext _db, IMailService mailService)
        {
            db = _db;
            this.mailService = mailService;
            GenericPrograms emailsend = new GenericPrograms(mailService);

        }


        [HttpGet("EmployeeDocumentTypeGet")]
        public IActionResult EmployeeDocumentTypeGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmployeeDocumentType>>();
                var record = db.EmployeeDocumentTypes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EmployeeDocumentTypeCreate")]
        public async Task<IActionResult> EmployeeDocumentTypeCreate(EmployeeDocumentType obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.EmployeeDocumentTypes.ToList().Exists(p => p.TypeName.Equals(obj.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.EmployeeDocumentTypes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EmployeeDocumentTypeUpdate/{id}")]
        public async Task<IActionResult> EmployeeDocumentTypeUpdate(int id, EmployeeDocumentType data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                bool isValid = db.EmployeeDocumentTypes.ToList().Exists(x => x.TypeName.Equals(data.TypeName, StringComparison.CurrentCultureIgnoreCase) && x.DocTypeId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }
                if (id != data.DocTypeId)
                {
                    return BadRequest();
                }
                var record = await db.EmployeeDocumentTypes.FindAsync(id);
                if (data.TypeName != null && data.TypeName != "undefined")
                {
                    record.TypeName = data.TypeName;
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("EmployeeDocumentTypeByID/{id}")]
        public IActionResult EmployeeDocumentTypeByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                var record = db.EmployeeDocumentTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("DeleteEmployeeDocumentType/{id}")]
        public async Task<IActionResult> DeleteEmployeeDocumentType(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                EmployeeDocumentType data = await db.EmployeeDocumentTypes.FindAsync(id);
                if (data == null)
                {
                    return NotFound();
                }
                db.EmployeeDocumentTypes.Remove(data);
                await db.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmployeeDocumentType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        //EmployeeDocumentType End

        // Start LeaveTypes
        [HttpGet("LeaveTypesGet")]
        public IActionResult LeaveTypesGet()
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<LeaveType>>();
                var record = db.LeaveTypes.ToList();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("LeaveTypesCreate")]
        public async Task<IActionResult> LeaveTypesCreate(LeaveType obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.LeaveTypes.ToList().Exists(p => p.TypeName.Equals(obj.TypeName, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.LeaveTypes.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("LeaveTypesUpdate/{id}")]
        public async Task<IActionResult> LeaveTypesUpdate(int id, LeaveType data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != data.LeaveTypeId)
                {
                    return BadRequest();
                }
                bool isValid = db.LeaveTypes.ToList().Exists(x => x.TypeName.Equals(data.TypeName, StringComparison.CurrentCultureIgnoreCase) && x.LeaveTypeId != Convert.ToInt32(id));
                if (isValid)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                var record = await db.LeaveTypes.FindAsync(id);
                if (data.TypeName != null && data.TypeName != "undefined")
                {
                    record.TypeName = data.TypeName;
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
                    var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("LeaveTypesByID/{id}")]
        public IActionResult LeaveTypesByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                var record = db.LeaveTypes.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("DeleteLeaveTypes/{id}")]
        public async Task<IActionResult> DeleteLeaveTypes(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                LeaveType data = await db.LeaveTypes.FindAsync(id);
                if (data == null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.RECORD_NOTFOUND, null, null);
                }
                db.LeaveTypes.Remove(data);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<LeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        // End LeaveTypes



        
    }
}
