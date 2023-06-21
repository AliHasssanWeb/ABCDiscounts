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
    public class EmpTaxTypeEmpsController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpTaxTypeEmpsController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpTaxTypeEmps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpTaxTypeEmp>>> GetEmpTaxTypeEmps()
        {
            //return await _context.EmpTaxTypeEmps.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpTaxTypeEmp>>();
                var record = await _context.EmpTaxTypeEmps.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpTaxTypeEmps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpTaxTypeEmp>> GetEmpTaxTypeEmp(int id)
        {
            //var empTaxTypeEmp = await _context.EmpTaxTypeEmps.FindAsync(id);

            //if (empTaxTypeEmp == null)
            //{
            //    return NotFound();
            //}

            //return empTaxTypeEmp;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                var record = await _context.EmpTaxTypeEmps.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpTaxTypeEmps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpTaxTypeEmp(int id, EmpTaxTypeEmp empTaxTypeEmp)
        {
            //if (id != empTaxTypeEmp.EmpTaxTypeEmpID)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empTaxTypeEmp).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpTaxTypeEmps(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empTaxTypeEmp);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empTaxTypeEmp.EmpTaxTypeEmpId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpTaxTypeEmps.FindAsync(id);
                if (record.Date != empTaxTypeEmp.Date)
                {
                    record.Date = empTaxTypeEmp.Date;
                }
                if (record.EmployeeId != empTaxTypeEmp.EmployeeId)
                {
                    record.EmployeeId = empTaxTypeEmp.EmployeeId;
                }
                if (record.EmpTaxTypeId != empTaxTypeEmp.EmpTaxTypeId)
                {
                    record.EmpTaxTypeId = empTaxTypeEmp.EmpTaxTypeId;
                }
                if (record.IsApprove != empTaxTypeEmp.IsApprove)
                {
                    record.IsApprove = empTaxTypeEmp.IsApprove;
                }
                if (record.TaxPer != empTaxTypeEmp.TaxPer)
                {
                    record.TaxPer = empTaxTypeEmp.TaxPer;
                }


                empTaxTypeEmp = record;
                _context.Entry(empTaxTypeEmp).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpTaxTypeEmps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpTaxTypeEmp>> PostEmpTaxTypeEmp(EmpTaxTypeEmp empTaxTypeEmp)
        {
            //_context.EmpTaxTypeEmps.Add(empTaxTypeEmp);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpTaxTypeEmp", new { id = empTaxTypeEmp.EmpTaxTypeEmpId }, empTaxTypeEmp);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpTaxTypeEmps.Add(empTaxTypeEmp);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpTaxTypeEmps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpTaxTypeEmp(int id)
        {
            //var empTaxTypeEmp = await _context.EmpTaxTypeEmps.FindAsync(id);
            //if (empTaxTypeEmp == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpTaxTypeEmps.Remove(empTaxTypeEmp);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();

                EmpTaxTypeEmp data = await _context.EmpTaxTypeEmps.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpTaxTypeEmps.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        

        private bool EmpTaxTypeEmpExists(int id)
        {
            return _context.EmpTaxTypeEmps.Any(e => e.EmpTaxTypeEmpId == id);
        }
       
    }
}
