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
    public class EmpTaxesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpTaxesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpTaxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpTax>>> GetEmpTaxes()
        {
            //return await _context.EmpTaxes.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpTax>>();
                var record = await _context.EmpTaxes.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpTaxes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpTax>> GetEmpTax(int id)
        {
            //var empTax = await _context.EmpTaxes.FindAsync(id);

            //if (empTax == null)
            //{
            //    return NotFound();
            //}

            //return empTax;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                var record = await _context.EmpTaxes.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpTaxes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpTax(int id, EmpTax empTax)
        {
            //if (id != empTax.EmpTaxId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empTax).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpTaxExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empTax);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var dataExists = _context.EmpTaxes.Where(x => x.EmployeeId == empTax.EmployeeId && x.EmpTaxTypeId == empTax.EmpTaxTypeId).FirstOrDefault();
                if (dataExists != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                if (id != empTax.EmpTaxId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpTaxes.FindAsync(id);
                if (record.Amount != empTax.Amount)
                {
                    record.Amount = empTax.Amount;
                }
                if (record.Date != empTax.Date)
                {
                    record.Date = empTax.Date;
                }
                if (record.EmployeeId != empTax.EmployeeId)
                {
                    record.EmployeeId = empTax.EmployeeId;
                }
                if (record.EmpTaxTypeId != empTax.EmpTaxTypeId)
                {
                    record.EmpTaxTypeId = empTax.EmpTaxTypeId;
                }
                if (record.IsApprove != empTax.IsApprove)
                {
                    record.IsApprove = empTax.IsApprove;
                }
                if (record.Reason != empTax.Reason)
                {
                    record.Reason = empTax.Reason;
                }

                empTax = record;
                _context.Entry(empTax).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpTaxes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpTax>> PostEmpTax(EmpTax empTax)
        {
            //_context.EmpTaxes.Add(empTax);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpTax", new { id = empTax.EmpTaxId }, empTax);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var dataExists = _context.EmpTaxes.Where(x => x.EmployeeId == empTax.EmployeeId && x.EmpTaxTypeId == empTax.EmpTaxTypeId).FirstOrDefault();
                if(dataExists != null)
                {
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                    return Ok(Response);
                }
                _context.EmpTaxes.Add(empTax);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpTaxes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpTax(int id)
        {
            //var empTax = await _context.EmpTaxes.FindAsync(id);
            //if (empTax == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpTaxes.Remove(empTax);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTax>();

                EmpTax data = await _context.EmpTaxes.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpTaxes.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTax>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: Tax records
        [HttpGet("TaxByID")]
        public IActionResult TaxByID(int empid, DateTime fromDate, DateTime toDate)
        {
            //try
            //{
            //    var TaxRecord = GetTaxByID(empid, fromDate, toDate);
            //    return Ok(TaxRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpTaxRecord>();
                var record = GetTaxByID(empid, fromDate, toDate);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpTaxRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpTaxExists(int id)
        {
            return _context.EmpTaxes.Any(e => e.EmpTaxId == id);
        }

        private EmpTaxRecord GetTaxByID(int empid, DateTime fromDate, DateTime toDate)
        {

            double totalTaxes = 0;


            var emptax = _context.EmpTaxes.Where(x => x.EmployeeId == empid && x.Date >= fromDate && toDate >= x.Date).ToList();
            foreach (var item in emptax)
            {
                totalTaxes += Convert.ToDouble(item.Amount);
            }

            EmpTaxRecord res = new();// ABCServices.innerModel.EmpAllowanceRecord();
            res.EmployeeID = empid;
            res.TotalTaxes = totalTaxes;
            return res;

        }

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
