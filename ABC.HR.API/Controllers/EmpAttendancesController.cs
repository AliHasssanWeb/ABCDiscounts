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
    public class EmpAttendancesController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpAttendancesController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpAttendances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpAttendance>>> GetEmpAttendances()
        {           
           // return await _context.EmpAttendances.ToListAsync();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpAttendance>>();
                var record = await _context.EmpAttendances.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpAttendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpAttendance>> GetEmpAttendance(int id)
        {
            //var empAttendance = await _context.EmpAttendances.FindAsync(id);

            //if (empAttendance == null)
            //{
            //    return NotFound();
            //}

            //return empAttendance;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                var record = await _context.EmpAttendances.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpAttendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpAttendance(int id, EmpAttendance empAttendance)
        {
            //if (id != empAttendance.EmpAttendanceId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empAttendance).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpAttendanceExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empAttendance);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empAttendance.EmpAttendanceId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpAttendances.FindAsync(id);
                if (record.AttendanceDate != empAttendance.AttendanceDate)
                {
                    record.AttendanceDate = empAttendance.AttendanceDate;
                }
                if (record.EmployeeId != empAttendance.EmployeeId)
                {
                    record.EmployeeId = empAttendance.EmployeeId;
                }
                if (record.IsApprove != empAttendance.IsApprove)
                {
                    record.IsApprove = empAttendance.IsApprove;
                }
                if (record.Late != empAttendance.Late)
                {
                    record.Late = empAttendance.Late;
                }
                if (record.OverTime != empAttendance.OverTime)
                {
                    record.OverTime = empAttendance.OverTime;
                }
                if (record.TimeIn != empAttendance.TimeIn)
                {
                    record.TimeIn = empAttendance.TimeIn;
                }
                if (record.TimeOut != empAttendance.TimeOut)
                {
                    record.TimeOut = empAttendance.TimeOut;
                }



                empAttendance = record;
                _context.Entry(empAttendance).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpAttendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpAttendance>> PostEmpAttendance(EmpAttendance empAttendance)
        {
            //_context.EmpAttendances.Add(empAttendance);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpAttendance", new { id = empAttendance.EmpAttendanceId }, empAttendance);

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                var attendance = _context.EmpAttendances.Where(x => x.EmployeeId == empAttendance.EmployeeId && x.AttendanceDate == empAttendance.AttendanceDate)?.FirstOrDefault();
                if(attendance != null)
                {
                    attendance.TimeIn = empAttendance.TimeIn;
                    attendance.TimeOut = empAttendance.TimeOut;
                    attendance.Late = empAttendance.Late;
                    attendance.OverTime = empAttendance.OverTime;
                    attendance.IsApprove = empAttendance.IsApprove;
                    await _context.SaveChangesAsync();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }
            
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _context.EmpAttendances.Add(empAttendance);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpAttendances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpAttendance(int id)
        {
            //var empAttendance = await _context.EmpAttendances.FindAsync(id);
            //if (empAttendance == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpAttendances.Remove(empAttendance);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();

                EmpAttendance data = await _context.EmpAttendances.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpAttendances.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendance>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: attendance records
        [HttpGet("AttendanceByID")]
        public IActionResult AttendanceByID(int empid, DateTime fromDate, DateTime toDate, bool IncludeSS)
        {
            //try
            //{
            //    var AttendanceRecord = GetAttendanceByID(empid, fromDate, toDate, IncludeSS);
            //    return Ok(AttendanceRecord);
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpAttendanceRecord>();
                var record = GetAttendanceByID(empid, fromDate, toDate, IncludeSS);
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpAttendanceRecord>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private bool EmpAttendanceExists(int id)
        {
            return _context.EmpAttendances.Any(e => e.EmpAttendanceId == id);
        }

        private EmpAttendanceRecord GetAttendanceByID(int empid, DateTime fromDate, DateTime toDate, bool IncludeSS)
        {

            int perdaylate = 0;
            double perdayhours = 0.0;
            int totalattandance = 0;
            double perdayovertime = 0.0;
            int perdayleave = 0;
            int perdayearlyleave = 0;
            double perdayworkinghours = 0.0;


            var empcontracts = _context.EmployeeContracts.Where(x => x.EmployeeId == empid).FirstOrDefault();
            if (empcontracts != null)
            {
                var workingtimein = DateTime.Parse(empcontracts.WorkingTimeIn);
                var workingtimeout = DateTime.Parse(empcontracts.WorkingTimeOut);
                var workinghoursperday = (workingtimeout - workingtimein).TotalHours;

                var empattandence = _context.EmpAttendances.Where(x => x.EmployeeId == empid && x.AttendanceDate >= fromDate && toDate >= x.AttendanceDate).ToList();

                DateTime StartDate = fromDate;
                DateTime EndDate = toDate;

                foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
                {
                    if (IncludeSS == true)
                    {
                        perdayhours += workinghoursperday;
                    }
                    else
                    {
                        if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                        {
                            perdayhours += workinghoursperday;
                        }

                     }

                        //Console.WriteLine("Date is : " + day.ToString("dd-MM-yyyy"));
                        var empattandencebydate = _context.EmpAttendances.Where(x => x.EmployeeId == empid && x.AttendanceDate == day).ToList();
                    if (empattandencebydate.Count > 0)
                    {
                        foreach (var item in empattandencebydate)
                        {
                            if (IncludeSS == true)
                            {

                                totalattandance++;
                                if (DateTime.Parse(item.TimeIn) > workingtimein)
                                {
                                    perdaylate++;
                                }

                                var daytimein = DateTime.Parse(item.TimeIn);
                                var daytimeout = DateTime.Parse(item.TimeOut);
                                var dayhr = (daytimeout - daytimein).TotalHours;
                                double dayovertime = 0.0;
                                perdayworkinghours += dayhr;



                                if (dayhr > workinghoursperday)
                                {
                                    dayovertime = dayhr - workinghoursperday;
                                    perdayovertime += dayovertime;

                                }

                                if (daytimeout < workingtimeout)
                                {
                                    perdayearlyleave++;
                                }

                                // update attandence table with late and overtime
                            }
                            else
                            {
                                if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    totalattandance++;
                                    if (DateTime.Parse(item.TimeIn) > workingtimein)
                                    {
                                        perdaylate++;
                                    }

                                    var daytimein = DateTime.Parse(item.TimeIn);
                                    var daytimeout = DateTime.Parse(item.TimeOut);
                                    var dayhr = (daytimeout - daytimein).TotalHours;
                                    double dayovertime = 0.0;
                                    perdayworkinghours += dayhr;

                                    if (dayhr > workinghoursperday)
                                    {
                                        dayovertime = dayhr - workinghoursperday;
                                        perdayovertime += dayovertime;

                                    }

                                    if (daytimeout < workingtimeout)
                                    {
                                        perdayearlyleave++;
                                    }

                                    // update attandence table with late and overtime
                                }

                            }

                        }
                    }
                    else
                    {
                        if (IncludeSS == true)
                        {

                            perdayleave++;
                        }
                        else
                        {
                            if (day.DayOfWeek != DayOfWeek.Saturday && day.DayOfWeek != DayOfWeek.Sunday)
                            {
                                perdayleave++;
                            }

                         }
                    }
                }
            }

            EmpAttendanceRecord EmpRecord = new EmpAttendanceRecord();

            EmpRecord.EmployeeID = empid;
            EmpRecord.TotalAttendance = totalattandance;
            EmpRecord.TotalLeaves = perdayleave;
            EmpRecord.TotalLate = perdaylate;
            EmpRecord.TotalOverTimeHours = perdayovertime;
            EmpRecord.TotalWorkingHours = perdayhours;
            EmpRecord.TotalAttendHours = perdayworkinghours;
            EmpRecord.TotalEarlyLeave = perdayearlyleave;

            return EmpRecord;
        }

        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
        }
    }
}
