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
    public class EmpAllowanceTypeEmpsController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpAllowanceTypeEmpsController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpAllowanceTypeEmps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpAllowanceTypeEmp>>> GetEmpAllowanceTypeEmps()
        {
            //return await _context.EmpTaxTypeEmps.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpAllowanceTypeEmp>>();
                var record = await _context.EmpAllowanceTypeEmps.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpAllowanceTypeEmps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpAllowanceTypeEmp>> GetEmpAllowanceTypeEmp(int id)
        {
            //var empAllowanceTypeEmp = await _context.EmpAllowanceTypeEmps.FindAsync(id);

            //if (empAllowanceTypeEmp == null)
            //{
            //    return NotFound();
            //}

            //return empAllowanceTypeEmp;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                var record = await _context.EmpAllowanceTypeEmps.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpAllowanceTypeEmps/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpAllowanceTypeEmp(int id, EmpAllowanceTypeEmp empAllowanceTypeEmp)
        {
            //if (id != empAllowanceTypeEmp.EmpAllowanceTypeEmpID)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empAllowanceTypeEmp).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpAllowanceTypeEmps(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empAllowanceTypeEmp);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empAllowanceTypeEmp.EmpAllowanceTypeEmpId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpAllowanceTypeEmps.FindAsync(id);
                if (record.Date != empAllowanceTypeEmp.Date)
                {
                    record.Date = empAllowanceTypeEmp.Date;
                }
                if (record.EmployeeId != empAllowanceTypeEmp.EmployeeId)
                {
                    record.EmployeeId = empAllowanceTypeEmp.EmployeeId;
                }
                if (record.EmpAllowanceTypeId != empAllowanceTypeEmp.EmpAllowanceTypeId)
                {
                    record.EmpAllowanceTypeId = empAllowanceTypeEmp.EmpAllowanceTypeId;
                }
                if (record.IsApprove != empAllowanceTypeEmp.IsApprove)
                {
                    record.IsApprove = empAllowanceTypeEmp.IsApprove;
                }
                if (record.AllowanceTypeFix != empAllowanceTypeEmp.AllowanceTypeFix)
                {
                    record.AllowanceTypeFix = empAllowanceTypeEmp.AllowanceTypeFix;
                }
                if (record.AllowanceTypePer != empAllowanceTypeEmp.AllowanceTypePer)
                {
                    record.AllowanceTypePer = empAllowanceTypeEmp.AllowanceTypePer;
                }
                if (record.IsPerFix != empAllowanceTypeEmp.IsPerFix)
                {
                    record.IsPerFix = empAllowanceTypeEmp.IsPerFix;
                }


                empAllowanceTypeEmp = record;
                _context.Entry(empAllowanceTypeEmp).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpAllowanceTypeEmps
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpAllowanceTypeEmp>> PostEmpAllowanceTypeEmp(EmpAllowanceTypeEmp empAllowanceTypeEmp)
        {
            //_context.EmpAllowanceTypeEmps.Add(empAllowanceTypeEmp);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpAllowanceTypeEmp", new { id = empAllowanceTypeEmp.EmpAllowanceTypeEmpId }, empAllowanceTypeEmp);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpAllowanceTypeEmps.Add(empAllowanceTypeEmp);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpAllowanceTypeEmps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpAllowanceTypeEmp(int id)
        {
            //var empAllowanceTypeEmp = await _context.EmpAllowanceTypeEmps.FindAsync(id);
            //if (empAllowanceTypeEmp == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpAllowanceTypeEmps.Remove(empAllowanceTypeEmp);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();

                EmpAllowanceTypeEmp data = await _context.EmpAllowanceTypeEmps.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpAllowanceTypeEmps.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceTypeEmp>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        

        private bool EmpAllowanceTypeEmpExists(int id)
        {
            return _context.EmpAllowanceTypeEmps.Any(e => e.EmpAllowanceTypeEmpId == id);
        }
       
    }
}
