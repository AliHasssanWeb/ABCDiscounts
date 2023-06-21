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
    public class EmpAllowancesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpAllowancesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpAllowances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpAllowance>>> GetEmpAllowances()
        {
            // return await _context.EmpAllowances.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpAllowance>> ();
                var record = await _context.EmpAllowances.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }


        }

        // GET: api/EmpAllowances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpAllowance>> GetEmpAllowance(int id)
        {
            //var empAllowance = await _context.EmpAllowances.FindAsync(id);

            //if (empAllowance == null)
            //{
            //    return NotFound();
            //}

            //return empAllowance;



            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                var record = await _context.EmpAllowances.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpAllowances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpAllowance(int id, EmpAllowance empAllowance)
        {
            //if (id != empAllowance.EmpAllowanceId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empAllowance).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpAllowanceExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empAllowance);


            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empAllowance.EmpAllowanceId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpAllowances.FindAsync(id);
                if (record.AllowanceTypeId != empAllowance.AllowanceTypeId)
                {
                    record.AllowanceTypeId = empAllowance.AllowanceTypeId;
                }
                if (record.Amount != empAllowance.Amount)
                {
                    record.Amount = empAllowance.Amount;
                }
                if (record.Date != empAllowance.Date)
                {
                    record.Date = empAllowance.Date;
                }
                if (record.EmployeeId != empAllowance.EmployeeId)
                {
                    record.EmployeeId = empAllowance.EmployeeId;
                }
                if (record.IsApprove != empAllowance.IsApprove)
                {
                    record.IsApprove = empAllowance.IsApprove;
                }
                if (record.Reason != empAllowance.Reason)
                {
                    record.Reason = empAllowance.Reason;
                }

                empAllowance = record;
                _context.Entry(empAllowance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpAllowances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpAllowance>> PostEmpAllowance(EmpAllowance empAllowance)
        {
            //_context.EmpAllowances.Add(empAllowance);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpAllowance", new { id = empAllowance.EmpAllowanceId }, empAllowance);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpAllowances.Add(empAllowance);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpAllowances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpAllowance(int id)
        {
            //var empAllowance = await _context.EmpAllowances.FindAsync(id);
            //if (empAllowance == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpAllowances.Remove(empAllowance);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();

                EmpAllowance data = await _context.EmpAllowances.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpAllowances.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: attendance records
        [HttpGet("AllowanceByID")]
       // [Route("TotalAllownceAmount")]
        public IActionResult AllowanceByID(int empid, DateTime fromDate, DateTime toDate)
        {
            //try
            //{
            //    var AllowanceRecord = GetAllowanceByID(empid, fromDate, toDate);
            //    return Ok(AllowanceRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceRecord>();
                var record = GetAllowanceByID(empid, fromDate, toDate);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAllowanceRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpAllowanceExists(int id)
        {
            return _context.EmpAllowances.Any(e => e.EmpAllowanceId == id);
        }

        private EmpAllowanceRecord GetAllowanceByID(int empid, DateTime fromDate, DateTime toDate)
        {
            
            double totalAllowances = 0;


            var empallowance = _context.EmpAllowances.Where(x => x.EmployeeId == empid && x.Date >= fromDate && toDate >= x.Date).ToList();
            foreach (var item in empallowance)
            {
                totalAllowances += Convert.ToDouble(item.Amount);
            }

            EmpAllowanceRecord res = new();// ABCServices.innerModel.EmpAllowanceRecord();
            res.EmployeeID = empid;
            res.TotalAllowances = totalAllowances;
            return res;

        }            

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
