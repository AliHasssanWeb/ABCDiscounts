using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
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
    public class EmpTaxTypesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpTaxTypesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpTaxTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpTaxType>>> GetEmpTaxTypes()
        {
            // return await _context.EmpTaxTypes.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpTaxType>>();
                var record = await _context.EmpTaxTypes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpTaxTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpTaxType>> GetEmpTaxType(int id)
        {
            //var empTaxType = await _context.EmpTaxTypes.FindAsync(id);

            //if (empTaxType == null)
            //{
            //    return NotFound();
            //}

            //return empTaxType;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                var record = await _context.EmpTaxTypes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpTaxTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpTaxType(int id, EmpTaxType empTaxType)
        {
            //if (id != empTaxType.EmpTaxTypeId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empTaxType).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpTaxTypeExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empTaxType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empTaxType.EmpTaxTypeId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpTaxTypes.FindAsync(id);
                var dataExists = _context.EmpTaxTypes.Where(x => x.EmpTaxTypeName == empTaxType.EmpTaxTypeName && x.TaxType == empTaxType.TaxType && x.SalaryRange == empTaxType.SalaryRange).FirstOrDefault();
                if (dataExists != null)
                {
                    if(dataExists.Description == empTaxType.Description && dataExists.AmountLimit ==empTaxType.AmountLimit)
                    {
                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                        return Ok(Response);
                    }
                   
                }
                if (record.AmountLimit != empTaxType.AmountLimit)
                {
                    record.AmountLimit = empTaxType.AmountLimit;
                }
                if (record.Description != empTaxType.Description)
                {
                    record.Description = empTaxType.Description;
                }
                
                if (record.IsActive != empTaxType.IsActive)
                {
                    record.IsActive = empTaxType.IsActive;
                }
                if (record.IsTax != empTaxType.IsTax)
                {
                    record.IsTax = empTaxType.IsTax;
                }
                if (record.IsErTax != empTaxType.IsErTax)
                {
                    record.IsErTax = empTaxType.IsErTax;
                }
                if(dataExists == null)
                {

               
                if (record.EmpTaxTypeName != empTaxType.EmpTaxTypeName)
                {
                    record.EmpTaxTypeName = empTaxType.EmpTaxTypeName;
                }
                if (record.TaxType != empTaxType.TaxType)
                {
                    record.TaxType = empTaxType.TaxType;
                }
                if (record.SalaryRange != empTaxType.SalaryRange)
                {
                    record.SalaryRange = empTaxType.SalaryRange;
                }
                }
                empTaxType = record;
                _context.Entry(empTaxType).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpTaxTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
      //  [HttpPost("PostEmpTaxType")]
        public async Task<ActionResult<EmpTaxType>> PostEmpTaxType(EmpTaxType empTaxType)
        {
            //_context.EmpTaxTypes.Add(empTaxType);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpTaxType", new { id = empTaxType.EmpTaxTypeId }, empTaxType);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var dataExists = _context.EmpTaxTypes.Where(x => x.EmpTaxTypeName == empTaxType.EmpTaxTypeName && x.TaxType == empTaxType.TaxType && x.SalaryRange == empTaxType.SalaryRange).FirstOrDefault();
                if (dataExists != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }

                _context.EmpTaxTypes.Add(empTaxType);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpTaxTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpTaxType(int id)
        {
            //var empTaxType = await _context.EmpTaxTypes.FindAsync(id);
            //if (empTaxType == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpTaxTypes.Remove(empTaxType);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();

                EmpTaxType data = await _context.EmpTaxTypes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpTaxTypes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxType>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        private bool EmpTaxTypeExists(int id)
        {
            return _context.EmpTaxTypes.Any(e => e.EmpTaxTypeId == id);
        }
    }
}
