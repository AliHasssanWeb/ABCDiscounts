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
    public class EmpLoanTypesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpLoanTypesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpLoanTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpLoanType>>> GetEmpLoanTypes()
        {
            // return await _context.EmpLoanTypes.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpLoanType>>();
                var record = await _context.EmpLoanTypes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpLoanTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpLoanType>> GetEmpLoanType(int id)
        {
            //var empLoanType = await _context.EmpLoanTypes.FindAsync(id);

            //if (empLoanType == null)
            //{
            //    return NotFound();
            //}

            //return empLoanType;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                var record = await _context.EmpLoanTypes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpLoanTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpLoanType(int id, EmpLoanType empLoanType)
        {
            //if (id != empLoanType.EmpLoanTypeId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empLoanType).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpLoanTypeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empLoanType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empLoanType.EmpLoanTypeId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpLoanTypes.FindAsync(id);
                if (record.AmountLimit != empLoanType.AmountLimit)
                {
                    record.AmountLimit = empLoanType.AmountLimit;
                }
                if (record.Description != empLoanType.Description)
                {
                    record.Description = empLoanType.Description;
                }
                if (record.EmpLoanTypeName != empLoanType.EmpLoanTypeName)
                {
                    record.EmpLoanTypeName = empLoanType.EmpLoanTypeName;
                }
                if (record.IsActive != empLoanType.IsActive)
                {
                    record.IsActive = empLoanType.IsActive;
                }

                empLoanType = record;
                _context.Entry(empLoanType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpLoanTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpLoanType>> PostEmpLoanType(EmpLoanType empLoanType)
        {
            //_context.EmpLoanTypes.Add(empLoanType);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpLoanType", new { id = empLoanType.EmpLoanTypeId }, empLoanType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpLoanTypes.Add(empLoanType);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpLoanTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpLoanType(int id)
        {
            //var empLoanType = await _context.EmpLoanTypes.FindAsync(id);
            //if (empLoanType == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpLoanTypes.Remove(empLoanType);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();

                EmpLoanType data = await _context.EmpLoanTypes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpLoanTypes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        private bool EmpLoanTypeExists(int id)
        {
            return _context.EmpLoanTypes.Any(e => e.EmpLoanTypeId == id);
        }
    }
}
