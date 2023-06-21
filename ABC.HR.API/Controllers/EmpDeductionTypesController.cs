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
    public class EmpDeductionTypesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpDeductionTypesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpDeductionTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpDeductionType>>> GetEmpDeductionTypes()
        {
            // return await _context.EmpDeductionTypes.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpDeductionType>>();
                var record = await _context.EmpDeductionTypes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpDeductionTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpDeductionType>> GetEmpDeductionType(int id)
        {
            //var empDeductionType = await _context.EmpDeductionTypes.FindAsync(id);

            //if (empDeductionType == null)
            //{
            //    return NotFound();
            //}

            //return empDeductionType;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                var record = await _context.EmpDeductionTypes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpDeductionTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpDeductionType(int id, EmpDeductionType empDeductionType)
        {
            //if (id != empDeductionType.EmpDeductionTypeId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empDeductionType).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpDeductionTypeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empDeductionType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empDeductionType.EmpDeductionTypeId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpDeductionTypes.FindAsync(id);
                if (record.AmountLimit != empDeductionType.AmountLimit)
                {
                    record.AmountLimit = empDeductionType.AmountLimit;
                }
                if (record.Description != empDeductionType.Description)
                {
                    record.Description = empDeductionType.Description;
                }
                if (record.EmpDeductionTypeName != empDeductionType.EmpDeductionTypeName)
                {
                    record.EmpDeductionTypeName = empDeductionType.EmpDeductionTypeName;
                }
                if (record.IsActive != empDeductionType.IsActive)
                {
                    record.IsActive = empDeductionType.IsActive;
                }



                empDeductionType = record;
                _context.Entry(empDeductionType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpDeductionTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpDeductionType>> PostEmpDeductionType(EmpDeductionType empDeductionType)
        {
            //_context.EmpDeductionTypes.Add(empDeductionType);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpDeductionType", new { id = empDeductionType.EmpDeductionTypeId }, empDeductionType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpDeductionTypes.Add(empDeductionType);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpDeductionTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpDeductionType(int id)
        {
            //var empDeductionType = await _context.EmpDeductionTypes.FindAsync(id);
            //if (empDeductionType == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpDeductionTypes.Remove(empDeductionType);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();

                EmpDeductionType data = await _context.EmpDeductionTypes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpDeductionTypes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        private bool EmpDeductionTypeExists(int id)
        {
            return _context.EmpDeductionTypes.Any(e => e.EmpDeductionTypeId == id);
        }
    }
}
