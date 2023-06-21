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
    public class EmpLeaveTypesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpLeaveTypesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpLeaveTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpLeaveType>>> GetEmpLeaveTypes()
        {
            // return await _context.EmpLeaveTypes.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpLeaveType>>();
                var record = await _context.EmpLeaveTypes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpLeaveTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpLeaveType>> GetEmpLeaveType(int id)
        {
            //var empLeaveType = await _context.EmpLeaveTypes.FindAsync(id);

            //if (empLeaveType == null)
            //{
            //    return NotFound();
            //}

            //return empLeaveType;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                var record = await _context.EmpLeaveTypes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpLeaveTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpLeaveType(int id, EmpLeaveType empLeaveType)
        {
            //if (id != empLeaveType.EmpLeaveTypeId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empLeaveType).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpLeaveTypeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empLeaveType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empLeaveType.EmpLeaveTypeId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpLeaveTypes.FindAsync(id);
                if (record.Description != empLeaveType.Description)
                {
                    record.Description = empLeaveType.Description;
                }
                if (record.IsActive != empLeaveType.IsActive)
                {
                    record.IsActive = empLeaveType.IsActive;
                }
                if (record.LeaveLimit != empLeaveType.LeaveLimit)
                {
                    record.LeaveLimit = empLeaveType.LeaveLimit;
                }
                if (record.LeaveTypeName != empLeaveType.LeaveTypeName)
                {
                    record.LeaveTypeName = empLeaveType.LeaveTypeName;
                }


                empLeaveType = record;
                _context.Entry(empLeaveType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpLeaveTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpLeaveType>> PostEmpLeaveType(EmpLeaveType empLeaveType)
        {
            //_context.EmpLeaveTypes.Add(empLeaveType);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpLeaveType", new { id = empLeaveType.EmpLeaveTypeId }, empLeaveType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpLeaveTypes.Add(empLeaveType);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpLeaveTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpLeaveType(int id)
        {
            //var empLeaveType = await _context.EmpLeaveTypes.FindAsync(id);
            //if (empLeaveType == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpLeaveTypes.Remove(empLeaveType);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();

                EmpLeaveType data = await _context.EmpLeaveTypes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpLeaveTypes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        private bool EmpLeaveTypeExists(int id)
        {
            return _context.EmpLeaveTypes.Any(e => e.EmpLeaveTypeId == id);
        }
    }
}
