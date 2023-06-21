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
    public class EmpLeavesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpLeavesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpLeaves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpLeave>>> GetEmpLeaves()
        {
            // return await _context.EmpLeaves.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpLeave>>();
                var record = await _context.EmpLeaves.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpLeaves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpLeave>> GetEmpLeave(int id)
        {
            //var empLeave = await _context.EmpLeaves.FindAsync(id);

            //if (empLeave == null)
            //{
            //    return NotFound();
            //}

            //return empLeave;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                var record = await _context.EmpLeaves.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpLeaves/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpLeave(int id, EmpLeave empLeave)
        {
            //if (id != empLeave.EmpLeaveId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empLeave).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpLeaveExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empLeave);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empLeave.EmpLeaveId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpLeaves.FindAsync(id);
                if (record.EmpLeaveTypeId != empLeave.EmpLeaveTypeId)
                {
                    record.EmpLeaveTypeId = empLeave.EmpLeaveTypeId;
                }
                if (record.EmployeeId != empLeave.EmployeeId)
                {
                    record.EmployeeId = empLeave.EmployeeId;
                }
                if (record.FromDate != empLeave.FromDate)
                {
                    record.FromDate = empLeave.FromDate;
                }
                if (record.IsApprove != empLeave.IsApprove)
                {
                    record.IsApprove = empLeave.IsApprove;
                }
                if (record.Reason != empLeave.Reason)
                {
                    record.Reason = empLeave.Reason;
                }
                if (record.ToDate != empLeave.ToDate)
                {
                    record.ToDate = empLeave.ToDate;
                }


                empLeave = record;
                _context.Entry(empLeave).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpLeaves
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpLeave>> PostEmpLeave(EmpLeave empLeave)
        {
            //_context.EmpLeaves.Add(empLeave);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpLeave", new { id = empLeave.EmpLeaveId }, empLeave);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpLeaves.Add(empLeave);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpLeaves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpLeave(int id)
        {
            //var empLeave = await _context.EmpLeaves.FindAsync(id);
            //if (empLeave == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpLeaves.Remove(empLeave);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();

                EmpLeave data = await _context.EmpLeaves.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpLeaves.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeave>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: attendance records
        [HttpGet("LeaveByID")]
        public IActionResult LeaveByID(int empid, DateTime fromDate, DateTime toDate)
        {
            //try
            //{
            //    var leaveRecord = GetLeaveByID(empid, fromDate, toDate);
            //    return Ok(leaveRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpLeaveRecord>();
                var record = GetLeaveByID(empid, fromDate, toDate);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpLeaveRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpLeaveExists(int id)
        {
            return _context.EmpLeaves.Any(e => e.EmpLeaveId == id);
        }

        private EmpLeaveRecord GetLeaveByID(int empid, DateTime fromDate, DateTime toDate)
        {

            int totalLeaves = 0;

            DateTime StartDate = fromDate;
            DateTime EndDate = toDate;

            foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
            { 
              var empleave= _context.EmpLeaves.Where(x => x.EmployeeId == empid && x.FromDate >= day && day >= x.ToDate).ToList();
                if (empleave.Count>0)
                {
                    totalLeaves++;
                }

            }
            EmpLeaveRecord res = new();// ABCServices.innerModel.EmpAllowanceRecord();
            res.EmployeeID = empid;
            res.TotalLeaves = totalLeaves;
            return res;

        }

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
