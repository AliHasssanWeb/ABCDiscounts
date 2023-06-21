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
    public class EmpDeductionsController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpDeductionsController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpDeductions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpDeduction>>> GetEmpDeductions()
        {
            // return await _context.EmpDeductions.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpDeduction>>();
                var record = await _context.EmpDeductions.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpDeductions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpDeduction>> GetEmpDeduction(int id)
        {
            //var empDeduction = await _context.EmpDeductions.FindAsync(id);

            //if (empDeduction == null)
            //{
            //    return NotFound();
            //}

            //return empDeduction;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                var record = await _context.EmpDeductions.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpDeductions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpDeduction(int id, EmpDeduction empDeduction)
        {
            //if (id != empDeduction.EmpDeductionId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empDeduction).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpDeductionExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empDeduction);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empDeduction.EmpDeductionId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpDeductions.FindAsync(id);
                if (record.Amount != empDeduction.Amount)
                {
                    record.Amount = empDeduction.Amount;
                }
                if (record.Date != empDeduction.Date)
                {
                    record.Date = empDeduction.Date;
                }
                if (record.EmpDeductionTypeId != empDeduction.EmpDeductionTypeId)
                {
                    record.EmpDeductionTypeId = empDeduction.EmpDeductionTypeId;
                }
                if (record.EmployeeId != empDeduction.EmployeeId)
                {
                    record.EmployeeId = empDeduction.EmployeeId;
                }
                if (record.IsApprove != empDeduction.IsApprove)
                {
                    record.IsApprove = empDeduction.IsApprove;
                }
                if (record.Reason != empDeduction.Reason)
                {
                    record.Reason = empDeduction.Reason;
                }



                empDeduction = record;
                _context.Entry(empDeduction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpDeductions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpDeduction>> PostEmpDeduction(EmpDeduction empDeduction)
        {
            //_context.EmpDeductions.Add(empDeduction);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpDeduction", new { id = empDeduction.EmpDeductionId }, empDeduction);

            try
            {
                empDeduction.IsClaim = false;
                var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpDeductions.Add(empDeduction);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpDeductions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpDeduction(int id)
        {
            //var empDeduction = await _context.EmpDeductions.FindAsync(id);
            //if (empDeduction == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpDeductions.Remove(empDeduction);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();

                EmpDeduction data = await _context.EmpDeductions.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpDeductions.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeduction>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: Deduction records
        [HttpGet("DeductionByID")]
        public IActionResult DeductionByID(int empid, DateTime fromDate, DateTime toDate)
        {
            //try
            //{
            //    var AllowanceRecord = GetDeductionByID(empid, fromDate, toDate);
            //    return Ok(AllowanceRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpDeductionRecord>();
                var record = GetDeductionByID(empid, fromDate, toDate);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpDeductionRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpDeductionExists(int id)
        {
            return _context.EmpDeductions.Any(e => e.EmpDeductionId == id);
        }

        private EmpDeductionRecord GetDeductionByID(int empid, DateTime fromDate, DateTime toDate)
        {

            double totalDeductions = 0;


            var empdeduction = _context.EmpDeductions.Where(x => x.EmployeeId == empid && x.Date >= fromDate && toDate >= x.Date).ToList();
            foreach (var item in empdeduction)
            {
                totalDeductions += Convert.ToDouble(item.Amount);
            }

            EmpDeductionRecord res = new();// ABCServices.innerModel.EmpAllowanceRecord();
            res.EmployeeID = empid;
            res.TotalDeductions = totalDeductions;
            return res;

        }

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
