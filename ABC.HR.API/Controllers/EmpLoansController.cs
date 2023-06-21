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
    public class EmpLoansController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpLoansController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpLoans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpLoan>>> GetEmpLoans()
        {
            // return await _context.EmpLoans.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpLoan>>();
                var record = await _context.EmpLoans.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpLoans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpLoan>> GetEmpLoan(int id)
        {
            //var empLoan = await _context.EmpLoans.FindAsync(id);

            //if (empLoan == null)
            //{
            //    return NotFound();
            //}

            //return empLoan;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                var record = await _context.EmpLoans.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpLoans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpLoan(int id, EmpLoan empLoan)
        {
            //if (id != empLoan.EmpLoanId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empLoan).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpLoanExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empLoan);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empLoan.EmpLoanId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpLoans.FindAsync(id);
                if (record.Amount != empLoan.Amount)
                {
                    record.Amount = empLoan.Amount;
                }
                if (record.Date != empLoan.Date)
                {
                    record.Date = empLoan.Date;
                }
                if (record.EmpLoanTypeId != empLoan.EmpLoanTypeId)
                {
                    record.EmpLoanTypeId = empLoan.EmpLoanTypeId;
                }
                if (record.EmployeeId != empLoan.EmployeeId)
                {
                    record.EmployeeId = empLoan.EmployeeId;
                }
                if (record.IsApprove != empLoan.IsApprove)
                {
                    record.IsApprove = empLoan.IsApprove;
                }
                if (record.Reason != empLoan.Reason)
                {
                    record.Reason = empLoan.Reason;
                }


                empLoan = record;
                _context.Entry(empLoan).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpLoans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpLoan>> PostEmpLoan(EmpLoan empLoan)
        {
            //_context.EmpLoans.Add(empLoan);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpLoan", new { id = empLoan.EmpLoanId }, empLoan);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                empLoan.IsPaid = false;
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpLoans.Add(empLoan);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpLoans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpLoan(int id)
        {
            //var empLoan = await _context.EmpLoans.FindAsync(id);
            //if (empLoan == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpLoans.Remove(empLoan);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();

                EmpLoan data = await _context.EmpLoans.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpLoans.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoan>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: Loan records
        [HttpGet("LoanByID")]
        public IActionResult LoanByID(int empid, DateTime fromDate, DateTime toDate)
        {
            //try
            //{
            //    var LoanRecord = GetLoanByID(empid, fromDate, toDate);
            //    return Ok(LoanRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLoanRecord>();
                var record = GetLoanByID(empid, fromDate, toDate);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLoanRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpLoanExists(int id)
        {
            return _context.EmpLoans.Any(e => e.EmpLoanId == id);
        }

        private EmpLoanRecord GetLoanByID(int empid, DateTime fromDate, DateTime toDate)
        {

            double totalLoans = 0;


            var emploan = _context.EmpLoans.Where(x => x.EmployeeId == empid && x.Date >= fromDate && toDate >= x.Date).ToList();
            foreach (var item in emploan)
            {
                totalLoans += Convert.ToDouble(item.Amount);
            }

            EmpLoanRecord res = new();// ABCServices.innerModel.EmpAllowanceRecord();
            res.EmployeeID = empid;
            res.TotalLoans = totalLoans;
            return res;

        }

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
