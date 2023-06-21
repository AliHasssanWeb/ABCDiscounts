using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.HR.Domain.Entities;
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
using System.Threading.Tasks;

namespace ABC.HR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployerController : ControllerBase
    {
        protected readonly ABCDiscountsContext db;

        public EmployerController(ABCDiscountsContext _db)
        {
            db = _db;
        }

        [HttpGet("EmployerGet")]
        public IActionResult EmployerGet()
        {
            try
            {
                var record = db.Employers.ToList();
                var Response = ResponseBuilder.BuildWSResponse<List<Employer>>();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EmployerCreate")]
        public async Task<IActionResult> EmployerCreate(Employer obj)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                bool checkname = db.Employers.ToList().Exists(p => p.FederalEmployeeId.Equals(obj.FederalEmployeeId, StringComparison.CurrentCultureIgnoreCase));
                if (checkname)

                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                }

                db.Employers.Add(obj);
                await db.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EmployerByID/{id}")]
        public IActionResult EmployerByID(int id)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                var record = db.Employers.Find(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("EmployerUpdate/{id}")]
        public async Task<IActionResult> EmployerUpdate(int id, Employer data)
        {
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<Employer>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == 0)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                }
                if (id != data.EmployerId)
                {
                    return BadRequest();
                }
                var record = await db.Employers.FindAsync(id);
                if (data.City != null && data.City != "undefined")
                {
                    record.City = data.City;
                }
                if (data.EmployerEmail != null && data.EmployerEmail != "undefined")
                {
                    record.EmployerEmail = data.EmployerEmail;
                }
                if (data.EmployerName != null && data.EmployerName != "undefined")
                {
                    record.EmployerName = data.EmployerName;
                }
                if (data.EmployerPhone != null && data.EmployerPhone != "undefined")
                {
                    record.EmployerPhone = data.EmployerPhone;
                }
                if (data.EmployerState != null && data.EmployerState != "undefined")
                {
                    record.EmployerState = data.EmployerState;
                }
                if (data.EmployerZipCode != null && data.EmployerZipCode != "undefined")
                {
                    record.EmployerZipCode = data.EmployerZipCode;
                }
                if (data.Extention != null && data.Extention != "undefined")
                {
                    record.Extention = data.Extention;
                }
                if (data.Fax != null && data.Fax != "undefined")
                {
                    record.Fax = data.Fax;
                }
                if (data.FederalEmployeeId != null && data.FederalEmployeeId != "undefined")
                {
                    record.FederalEmployeeId = data.FederalEmployeeId;
                }
                if (data.PayrolAddress != null && data.PayrolAddress != "undefined")
                {
                    record.PayrolAddress = data.PayrolAddress;
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
                    var Response = ResponseBuilder.BuildWSResponse<Employer>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
    }
}
