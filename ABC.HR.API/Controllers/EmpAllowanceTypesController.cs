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
using EmpAllowanceType = ABC.EFCore.Repository.Edmx.EmpAllowanceType;

namespace ABC.HR.API.Controllers


{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpAllowanceTypesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpAllowanceTypesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpAllowanceTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpAllowanceType>>> GetEmpAllowanceTypes()
        {
            //return await _context.EmpAllowanceTypes.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpAllowanceType>>();
                var record = await _context.EmpAllowanceTypes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }


        }

        // GET: api/EmpAllowanceTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpAllowanceType>> GetEmpAllowanceType(int id)
        {
            //var empAllowanceType = await _context.EmpAllowanceTypes.FindAsync(id);

            //if (empAllowanceType == null)
            //{
            //    return NotFound();
            //}

            //return empAllowanceType;


            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                var record = await _context.EmpAllowanceTypes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        // PUT: api/EmpAllowanceTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpAllowanceType(int id, EmpAllowanceType empAllowanceType)
            {
            //if (id != empAllowanceType.EmpAllowanceTypeId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empAllowanceType).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpAllowanceTypeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empAllowanceType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id == empAllowanceType.EmpAllowanceTypeId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpAllowanceTypes.FindAsync(id);
                if (record.AllowanceTypeName != empAllowanceType.AllowanceTypeName)
                {
                    record.AllowanceTypeName = empAllowanceType.AllowanceTypeName;
                }
                if (record.AmountLimit != empAllowanceType.AmountLimit)
                {
                    record.AmountLimit = empAllowanceType.AmountLimit;
                }
                if (record.Description != empAllowanceType.Description)
                {
                    record.Description = empAllowanceType.Description;
                }
                if (record.IsActive != empAllowanceType.IsActive)
                {
                    record.IsActive = empAllowanceType.IsActive;
                }


                empAllowanceType = record;
                _context.Entry(empAllowanceType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpAllowanceTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpAllowanceType>> PostEmpAllowanceType(EmpAllowanceType empAllowanceType)
        {
            //_context.EmpAllowanceTypes.Add(empAllowanceType);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpAllowanceType", new { id = empAllowanceType.EmpAllowanceTypeId }, empAllowanceType);


            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpAllowanceTypes.Add(empAllowanceType);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        // DELETE: api/EmpAllowanceTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpAllowanceType(int id)
        {
            //var empAllowanceType = await _context.EmpAllowanceTypes.FindAsync(id);
            //if (empAllowanceType == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpAllowanceTypes.Remove(empAllowanceType);
            //await _context.SaveChangesAsync();

            //return NoContent();


            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();

                EmpAllowanceType data = await _context.EmpAllowanceTypes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpAllowanceTypes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        private bool EmpAllowanceTypeExists(int id)
        {
            return _context.EmpAllowanceTypes.Any(e => e.EmpAllowanceTypeId == id);
        }
    }
}
