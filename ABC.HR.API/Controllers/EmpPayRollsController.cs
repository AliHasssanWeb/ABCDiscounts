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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.HR.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpPayRollsController : ControllerBase
    {
        private readonly ABCDiscountsContext _context;

        public EmpPayRollsController(ABCDiscountsContext context)
        {
            _context = context;
        }

        // GET: api/EmpPayRolls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmpPayRoll>>> GetEmpPayRolls()
        {
            //return await _context.EmpPayRolls.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpPayRoll>>();
                var record = await _context.EmpPayRolls.ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmpContractPayRolls")]
        public async Task<ActionResult<IEnumerable<EmpPayRoll>>> GetEmpContractPayRolls()
        {
            //return await _context.EmpPayRolls.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpPayRoll>>();
                var record = await _context.EmpPayRolls.Where(x => x.EmpContractName == "Contract").ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmpPermanantPayRolls")]
        public async Task<ActionResult<IEnumerable<EmpPayRoll>>> GetEmpPermanantPayRolls()
        {
            //return await _context.EmpPayRolls.ToListAsync();
            try
            {
                var Response = ResponseBuilder.BuildWSResponse<List<EmpPayRoll>>();
                var record = await _context.EmpPayRolls.Where(x => x.EmpContractName == "Permanent").ToListAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // GET: api/EmpPayRolls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmpPayRoll>> GetEmpPayRoll(int id)
        {
            //var empPayRoll = await _context.EmpPayRolls.FindAsync(id);

            //if (empPayRoll == null)
            //{
            //    return NotFound();
            //}

            //return empPayRoll;

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                var record = await _context.EmpPayRolls.FindAsync(id);
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
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/EmpPayRolls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpPayRoll(int id, EmpPayRoll empPayRoll)
        {
            //if (id != empPayRoll.EmpPayRollId)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(empPayRoll).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!EmpPayRollExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return Ok(empPayRoll);

            try
            {

                var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (id != empPayRoll.EmpPayRollId)
                {
                    return BadRequest();
                }
                var record = await _context.EmpPayRolls.FindAsync(id);
                if (record.Allowances != empPayRoll.Allowances)
                {
                    record.Allowances = empPayRoll.Allowances;
                }
                if (record.BasicSalary != empPayRoll.BasicSalary)
                {
                    record.BasicSalary = empPayRoll.BasicSalary;
                }
                if (record.Deductions != empPayRoll.Deductions)
                {
                    record.Deductions = empPayRoll.Deductions;
                }
                if (record.DisperseDate != empPayRoll.DisperseDate)
                {
                    record.DisperseDate = empPayRoll.DisperseDate;
                }
                if (record.EmployeeId != empPayRoll.EmployeeId)
                {
                    record.EmployeeId = empPayRoll.EmployeeId;
                }
                if (record.FromDate != empPayRoll.FromDate)
                {
                    record.FromDate = empPayRoll.FromDate;
                }
                if (record.IsApprove != empPayRoll.IsApprove)
                {
                    record.IsApprove = empPayRoll.IsApprove;
                }
                if (record.Loans != empPayRoll.Loans)
                {
                    record.Loans = empPayRoll.Loans;
                }
                if (record.Month != empPayRoll.Month)
                {
                    record.Month = empPayRoll.Month;
                }
                if (record.Taxes != empPayRoll.Taxes)
                {
                    record.Taxes = empPayRoll.Taxes;
                }
                if (record.ToDate != empPayRoll.ToDate)
                {
                    record.ToDate = empPayRoll.ToDate;
                }
                if (record.TotalSalary != empPayRoll.TotalSalary)
                {
                    record.TotalSalary = empPayRoll.TotalSalary;
                }
                if (record.Year != empPayRoll.Year)
                {
                    record.Year = empPayRoll.Year;
                }


                empPayRoll = record;
                _context.Entry(empPayRoll).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // POST: api/EmpPayRolls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmpPayRoll>> PostEmpPayRoll(EmpPayRoll empPayRoll)
        {
            //_context.EmpPayRolls.Add(empPayRoll);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpPayRoll", new { id = empPayRoll.EmpPayRollId }, empPayRoll);

            try
            {


                CultureInfo culture = new CultureInfo("en-US");
                DateTime date = DateTime.Parse(empPayRoll.FromDate, culture);
                var enddate = date.AddDays(13);
                string isoDate = enddate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                empPayRoll.ToDate = isoDate;
                var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                var empLoan = _context.EmpLoans.Where(x => x.EmployeeId == empPayRoll.EmployeeId && x.IsPaid == false)?.FirstOrDefault();

                if (empLoan != null)
                {
                    var newloan = decimal.Parse(empLoan.Amount) - decimal.Parse(empPayRoll.Loans);
                    if (newloan == 0)
                    {
                        empLoan.IsPaid = true;
                    }
                    empLoan.PaidDate = DateTime.Now;
                }
                var empDeduction = _context.EmpDeductions.Where(x => x.EmployeeId == empPayRoll.EmployeeId && x.IsClaim == false)?.FirstOrDefault();

                if (empDeduction != null)
                {
                    empDeduction.IsClaim = true;
                    empDeduction.ClaimDate = DateTime.Now;
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                //Check Payroll Exist
                var validflag = false;
                var allpayroll = _context.EmpPayRolls.ToList();
                var employees = _context.Employees.ToList();
                List<Employee> empwithPayroll = new List<Employee>();
                foreach (DateTime day in EachCalendarDay(date, enddate))
                {
                    foreach (var item in allpayroll)
                    {
                        foreach (var employee in employees)
                        {
                            if (Convert.ToDateTime(item.ToDate) == day && item.EmpContractName == empPayRoll.EmpContractName && item.EmployeeId == employee.EmployeeId)
                            {
                                validflag = false;
                                empwithPayroll.Add(employee);
                            }

                            if (Convert.ToDateTime(item.FromDate) == day && item.EmpContractName == empPayRoll.EmpContractName && item.EmployeeId == employee.EmployeeId)
                            {
                                validflag = false;
                                empwithPayroll.Add(employee);
                            }
                        }


                    }

                }

                var employeepayroll = empwithPayroll.Where(x => x.EmployeeId == empPayRoll.EmployeeId)?.FirstOrDefault();
                if (employeepayroll == null)
                {
                    _context.EmpPayRolls.Add(empPayRoll);
                    await _context.SaveChangesAsync();
                    if(empPayRoll.EmpContractName == "Contract")
                    {

                        ResponseBuilder.SetWSResponse(Response, StatusCodes.Customer_Profile_Not_Found, null, null);
                        return Ok(Response);
                    }
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                    return Ok(Response);
                }
                ResponseBuilder.SetWSResponse(Response, StatusCodes.Already_Exists, null, null);
                return Ok(Response);

            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/EmpPayRolls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpPayRoll(int id)
        {
            //var empPayRoll = await _context.EmpPayRolls.FindAsync(id);
            //if (empPayRoll == null)
            //{
            //    return NotFound();
            //}

            //_context.EmpPayRolls.Remove(empPayRoll);
            //await _context.SaveChangesAsync();

            //return NoContent();

            try
            {
                var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();

                EmpPayRoll data = await _context.EmpPayRolls.FindAsync(id);

                if (data == null)
                {
                    return NotFound();
                }
                _context.EmpPayRolls.Remove(data);
                await _context.SaveChangesAsync();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, null);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.FIELD_REQUIRED, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }
        }





        private bool EmpPayRollExists(int id)
        {
            return _context.EmpPayRolls.Any(e => e.EmpPayRollId == id);
        }




        /////////////////////////////////////////////////
        // GET: Generate Payroll
        [HttpGet("GeneratePayrollByDate")]
        public async Task<IActionResult> AttendanceByID(DateTime fromDate, DateTime toDate, string contractname)
        {
            try
            {
                toDate = fromDate.AddDays(13);
                var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                var id = await GenerateAutoManualPayroll(fromDate, toDate, contractname);
                //var record = _context.EmpPayRolls.Where(x => x.EmpPayRollId == id.Result).FirstOrDefault();
                var record = _context.EmpPayRolls.Where(x => x.EmpPayRollId == id).FirstOrDefault();
                ResponseBuilder.SetWSResponse(Response, StatusCodes.SUCCESS_CODE, null, record);
                return Ok(Response);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.")
                {
                    var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
                    ResponseBuilder.SetWSResponse(Response, StatusCodes.ERROR_INVALID_METHOD, null, null);
                    return Ok(Response);
                }
                return BadRequest(ex.Message);
            }

        }

        private async Task<int> GeneratePayroll(DateTime FromDate, DateTime ToDate, String Contractname)
        {
            int outputid = 0;
            bool validflag = true;
            // check payroll date already available



            DateTime StartDate = FromDate;

            DateTime EndDate = ToDate;

            var allpayroll = _context.EmpPayRolls.ToList();
            foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
            {
                foreach (var item in allpayroll)
                {

                    if (Convert.ToDateTime(item.ToDate) == day && item.EmpContractName == Contractname)
                    {
                        validflag = false;
                    }

                    if (Convert.ToDateTime(item.FromDate) == day && item.EmpContractName == Contractname)
                    {
                        validflag = false;
                    }

                    //if (Convert.ToDateTime(item.ToDate) == day || Convert.ToDateTime(item.FromDate) == day)
                    //{
                    //    validflag = false;
                    //}
                    //if (item.EmpContractName == Contractname)
                    //{
                    //    validflag = false;
                    //}
                    //else if (Contractname != null)
                    //{
                    //    validflag = true;
                    //}
                }

            }

            if (validflag == true)
            {
                var allemp = _context.Employees.ToList();
                foreach (var item in allemp)
                {
                    /////////////////////////
                    //var yeartodatesalatot = GetYTDSalary(item.EmployeeId, EndDate);
                    ////////////////////


                    var empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId).FirstOrDefault(); ;
                    if (Contractname == null)
                    {
                        empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId).FirstOrDefault();

                    }
                    else
                    {
                        empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId && y.ContractName == Contractname).FirstOrDefault();

                    }

                    //var empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId && y.ContractName == Contractname).FirstOrDefault();
                    if (empcontract != null)
                    {
                        if (empcontract.WorkingTimeIn != null && empcontract.WorkingTimeOut != null)
                        {


                            var empattandance = (empcontract.DailyWages == true) ? GetAttendanceByID(item.EmployeeId, StartDate, EndDate, true) : GetAttendanceByID(item.EmployeeId, StartDate, EndDate, false);
                            if (empattandance.TotalAttendance > 0)
                            {

                                // check attandance
                                var totalatthr = empattandance.TotalAttendHours;


                                var dailywage = (empcontract.DailyWages == true) ? empcontract.DailyWagesChargesAmount : ((Convert.ToDouble(empcontract.Salary) / 30) / 8).ToString();

                                var expsalary = Convert.ToDouble(Convert.ToDouble(totalatthr) * Convert.ToDouble(dailywage));



                                // add allownces

                                //var allownces = GetAllowanceByID(item.EmployeeId, StartDate, EndDate);

                                var lstallowncetype = _context.EmpAllowanceTypeEmps.Where(z => z.EmployeeId == item.EmployeeId).ToList();

                                List<EmpAllowance> lstempallowance = new List<EmpAllowance>();
                                foreach (var tt in lstallowncetype)
                                {
                                    EmpAllowance empAllowance = new EmpAllowance();
                                    double allowancepar = 0;
                                    if (tt.IsPerFix == true)
                                    {
                                        // add fix
                                        allowancepar = Convert.ToDouble(tt.AllowanceTypeFix);
                                        empAllowance.Amount = Convert.ToString(expsalary + allowancepar);
                                    }
                                    else
                                    {
                                        // add per
                                        allowancepar = Convert.ToDouble(Convert.ToDouble(tt.AllowanceTypePer) / 100);
                                        empAllowance.Amount = Convert.ToString(expsalary * allowancepar);
                                    }

                                    empAllowance.Date = EndDate;
                                    empAllowance.EmployeeId = item.EmployeeId;
                                    empAllowance.AllowanceTypeId = tt.EmpAllowanceTypeId;
                                    empAllowance.IsApprove = false;
                                    empAllowance.Reason = "Auto Genrated";



                                    lstempallowance.Add(empAllowance);

                                }

                                double totalallownce = 0;

                                foreach (var additem in lstempallowance)
                                {

                                    totalallownce = totalallownce + Convert.ToDouble(additem.Amount);
                                    _context.EmpAllowances.Add(additem);
                                    await _context.SaveChangesAsync();
                                }


                                // add deduction

                                var deduction = GetDeductionByID(item.EmployeeId, StartDate, EndDate);

                                //add loan 
                                var loans = GetLoanByID(item.EmployeeId, StartDate, EndDate);

                                // add taxes
                                //var lsttaxtype = _context.EmpTaxTypeEmps.Where(z => z.EmployeeId == item.EmployeeId).ToList();
                                double totaltax = 0;
                                if (Contractname == "Permanent")
                                {

                                    var lsttaxtype = _context.EmpTaxTypes.ToList();

                                    List<EmpTax> lstemptax = new List<EmpTax>();
                                    foreach (var tt in lsttaxtype)
                                    {
                                        EmpTax emptax = new EmpTax();

                                        var taxpar = Convert.ToDouble(Convert.ToDouble(tt.AmountLimit) / 100);

                                        emptax.Date = EndDate;
                                        emptax.EmployeeId = item.EmployeeId;
                                        emptax.EmpTaxTypeId = tt.EmpTaxTypeId;
                                        emptax.IsApprove = false;
                                        emptax.Reason = "Auto Genrated";
                                        // emptax.Amount = Convert.ToString(expsalary * taxpar);
                                        emptax.Amount = tt.AmountLimit;

                                        lstemptax.Add(emptax);

                                    }



                                    foreach (var additem in lstemptax)
                                    {

                                        totaltax = totaltax + Convert.ToDouble(additem.Amount);
                                        _context.EmpTaxes.Add(additem);
                                        await _context.SaveChangesAsync();
                                    }

                                }
                                // add payroll

                                EmpPayRoll finalpayroll = new EmpPayRoll();

                                finalpayroll.EmpContractName = empcontract.ContractName;
                                finalpayroll.PayWithoutDeduction = expsalary.ToString();


                                finalpayroll.Allowances = totalallownce.ToString();
                                finalpayroll.BasicSalary = expsalary.ToString();
                                finalpayroll.Deductions = deduction.TotalDeductions.ToString();

                                //finalpayroll.DisperseDate = DateTime.Now.ToString("MM-dd-yyyy");
                                finalpayroll.DisperseDate = EndDate.ToString("MM-dd-yyyy");
                                finalpayroll.EmployeeId = item.EmployeeId;
                                finalpayroll.FromDate = StartDate.ToString("MM-dd-yyyy");
                                finalpayroll.IsApprove = false;
                                finalpayroll.Loans = loans.TotalLoans.ToString();
                                finalpayroll.Taxes = totaltax.ToString();
                                finalpayroll.ToDate = EndDate.ToString("MM-dd-yyyy");

                                var fintolsal = expsalary;
                                fintolsal = fintolsal + totalallownce;
                                fintolsal = fintolsal - deduction.TotalDeductions;
                                fintolsal = fintolsal - loans.TotalLoans;
                                fintolsal = fintolsal - totaltax;

                                finalpayroll.TotalSalary = fintolsal.ToString();
                                var idfornewpay = _context.EmpPayRolls.Add(finalpayroll);
                                await _context.SaveChangesAsync();
                                outputid = Convert.ToInt32(idfornewpay.Entity.EmpPayRollId);
                            }
                        }
                    }
                }

            }



            return outputid;


        }
        private IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        {
            for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
            return date;
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
        //////////////////////////
        private double GetYTDSalary(int empid, DateTime todate)
        {

            double totalallsalary = 0;



            //List<EmpPayRoll> empallowance = _context.EmpPayRolls.Where(x => x.EmployeeId == empid && (Convert.ToDateTime(x.DisperseDate)).Year == todate.Year).ToList();
            // List<EmpPayRoll> empallowance = _context.EmpPayRolls.Where(x => x.EmployeeId == empid && Convert.ToDateTime(x.DisperseDate).Year == todate.Year).ToList();
            //List<EmpPayRoll> empallowance = _context.EmpPayRolls.Where(x => x.EmployeeId == empid ).ToList();
            List<EmpPayRoll> empallowance = _context.EmpPayRolls.Where(x => (Convert.ToDateTime(x.DisperseDate)).Year == todate.Year).ToList();




            if (empallowance.Any()) //prevent IndexOutOfRangeException for empty list
            {
                //empallowance.RemoveAt(empallowance.Count - 1);
            }
            foreach (var item in empallowance)
            {
                totalallsalary += Convert.ToDouble(item.PayWithoutDeduction);
            }



            return totalallsalary;


        }
        [HttpGet("GenerateManualPayroll")]
        public ManualPayRoll GenerateManualPayroll(int empid, DateTime startDate)
        {
            ManualPayRoll payroll = new();
            var salary = TotalSalary(startDate, empid);
            var Fwttaxpercent = 0.0;
            var Loan = 0.0;
            var Deduction = 0.0;
            var Allowance = 0.0;
            var Taxes = 0.0;
            var WorkingHours = Attendance(startDate, empid);
            var employee = _context.Employees.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            if (employee != null)
            {
                var marriedStatus = employee.MartialStatus;
                if (marriedStatus != null || marriedStatus != "")
                {
                    if (marriedStatus == "Married" || marriedStatus == "MarriedFilingJointly")
                    {
                        if (salary >= 0 && salary <= 333)
                        {
                            Fwttaxpercent = 0;
                        }
                        if (salary > 333 && salary <= 1050)
                        {
                            Fwttaxpercent = 10;
                        }
                        if (salary > 1050 && salary <= 3252)
                        {
                            Fwttaxpercent = 15;
                        }
                        if (salary > 3252 && salary <= 6221)
                        {
                            Fwttaxpercent = 25;
                        }
                        if (salary > 6221 && salary <= 9308)
                        {
                            Fwttaxpercent = 28;
                        }
                        if (salary > 9308 && salary <= 16360)
                        {
                            Fwttaxpercent = 33;
                        }
                        if (salary > 16360 && salary <= 18437)
                        {
                            Fwttaxpercent = 35;
                        }
                        if (salary > 18437)
                        {
                            Fwttaxpercent = 39.6;
                        }
                    }
                    else if (marriedStatus == "Single" || marriedStatus == "SingleorMarriedFilingSeparately")
                    {
                        if (salary >= 0 && salary <= 88)
                        {
                            Fwttaxpercent = 0;
                        }
                        if (salary > 88 && salary <= 447)
                        {
                            Fwttaxpercent = 10;
                        }
                        if (salary > 447 && salary <= 1548)
                        {
                            Fwttaxpercent = 15;
                        }
                        if (salary > 1548 && salary <= 3623)
                        {
                            Fwttaxpercent = 25;
                        }
                        if (salary > 3623 && salary <= 7460)
                        {
                            Fwttaxpercent = 28;
                        }
                        if (salary > 7460 && salary <= 16115)
                        {
                            Fwttaxpercent = 33;
                        }
                        if (salary > 16115 && salary <= 16181)
                        {
                            Fwttaxpercent = 35;
                        }
                        if (salary > 16181)
                        {
                            Fwttaxpercent = 39.6;
                        }
                    }
                    //else if(marriedStatus == "HeadofHousehold")
                    //{

                    //}
                }

            }
            var fwtAmount = 0.0;
            var sswhAmount = 0.0;
            var mcwhAmount = 0.0;
            var erssAmount = 0.0;
            var ermcAmount = 0.0;
            var futaAmount = 0.0;
            var ncAmount = 0.0;
            var sutaAmount = 0.0;
            if (Fwttaxpercent > 0)
            {
                fwtAmount = salary * (Fwttaxpercent / 100);
            }
            sswhAmount = salary * (6.2 / 100);
            mcwhAmount = salary * (1.45 / 100);
            erssAmount = salary * (6.2 / 100);
            ermcAmount = salary * (1.45 / 100);
            var empLoan = _context.EmpLoans.Where(x => x.EmployeeId == empid && x.IsPaid == false)?.FirstOrDefault();
            if (empLoan != null)
            {
                if (empLoan.IsApprove == true)
                {
                    Loan = double.Parse(empLoan.Amount);
                }

            }
            var empDeduction = _context.EmpDeductions.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            if (empDeduction != null)
            {
                if (empDeduction.IsApprove == true)
                {
                    Deduction = double.Parse(empDeduction.Amount);
                }

            }
            var empAllowance = _context.EmpAllowances.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            if (empAllowance != null)
            {
                if (empAllowance.IsApprove == true)
                {
                    Allowance = double.Parse(empAllowance.Amount);
                }

            }

            var taxesById = TaxesById(empid, startDate);
            fwtAmount = salary * (taxesById.Fwt / 100);
            futaAmount = salary * (taxesById.FUTA / 100);
            ncAmount = salary * (taxesById.NCState / 100);
            sutaAmount = salary * (taxesById.NCSUTA / 100);

            Taxes = fwtAmount + ncAmount + sswhAmount + mcwhAmount;
            var empContract = _context.EmployeeContracts.Where(x => x.EmployeeId == empid && x.ContractName== "Contract")?.FirstOrDefault();
            if (empContract != null)
            {
               
                    Taxes = 0;
              

            }
            salary = salary - Taxes;
            salary = salary - Loan;
            salary = salary - Deduction;
            salary = salary + Allowance;

            payroll.TotalSalary = salary;
            payroll.Loans = Loan;
            payroll.Deduction = Deduction;
            payroll.Attendance = WorkingHours;
            payroll.Allowance = Allowance;
            payroll.Status = "";
            payroll.Taxes = Taxes;
            payroll.Allowances = GetAllowanceByID(empid);
            payroll.Deductions = GetDeductionByID(empid);
            payroll.LoansList = GetLoanByID(empid);

            return payroll;
        }
        public double TotalSalary(DateTime startDate, int empid)
        {

            var rateperHour = 0.0;
            var totalHours = Attendance(startDate, empid);
            var empContract = _context.EmployeeContracts.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            if (empContract != null)
            {
                if (empContract.DailyWages == true && empContract.DailyWagesChargesAmount != null)
                {
                    rateperHour = double.Parse(empContract.DailyWagesChargesAmount);
                }
            }
            var totalSalary = rateperHour * totalHours;
            return totalSalary;
        }
        public double Attendance(DateTime startDate, int empid)
        {
            var endDate = startDate.AddDays(14);
            var totalHours = 0.0;

            for (int i = 1; i <= 13; i++)
            {
                var date = startDate.AddDays(i);
                var attendance = _context.EmpAttendances.Where(x => x.EmployeeId == empid && x.AttendanceDate == date)?.FirstOrDefault();
                if (attendance != null)
                {
                    var timein = DateTime.Parse(attendance.TimeIn);
                    var timeout = DateTime.Parse(attendance.TimeOut);
                    var perDayhours = (timeout - timein).TotalHours;
                    totalHours = totalHours + perDayhours;
                }
            }
            return totalHours;
        }
        private List<EmpLoan> GetLoanByID(int empid)
        {
            List<EmpLoan> emploan = new List<EmpLoan>();
            emploan = _context.EmpLoans.Where(x => x.EmployeeId == empid && x.IsPaid == false).ToList();
            return emploan;

        }
        private List<EmpDeduction> GetDeductionByID(int empid)
        {
            List<EmpDeduction> empdeduction = new List<EmpDeduction>();
            empdeduction = _context.EmpDeductions.Where(x => x.EmployeeId == empid && x.IsClaim == false).ToList();
            return empdeduction;

        }
        private List<EmpAllowance> GetAllowanceByID(int empid)
        {
            List<EmpAllowance> emploan = new List<EmpAllowance>();
            emploan = _context.EmpAllowances.Where(x => x.EmployeeId == empid).ToList();
            return emploan;

        }
        [HttpGet("TaxesById")]
        public EmppayrollTaxes TaxesById(int empid, DateTime startdate)
        {
            var employee = _context.Employees.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            EmppayrollTaxes emptaxes = new EmppayrollTaxes();
            if (employee != null)
            {

                var marriedStatus = employee.MartialStatus;
                var fwtamount = 0.0;
                var fwttax = 0.0;
                var lastAmount = 0.0;
                var weeKsalary = 0.0;
                if (marriedStatus != null)
                {
                    weeKsalary = TotalSalary(startdate, empid);
                    var salary = 0.0;
                    salary = weeKsalary * 26;
                    if (marriedStatus.ToLower() == "married" || marriedStatus.ToLower() == "marriedfilingjointly")
                    {
                        salary = weeKsalary;
                        if (salary >= 0 && salary < 965)
                        {
                            fwtamount = 0;
                            fwttax = 0;
                            lastAmount = 0;
                        }
                        if (salary >= 965 && salary < 1731)
                        {
                            fwtamount = 0;
                            fwttax = 10;
                            lastAmount = 965;
                        }
                        if (salary >= 1731 && salary < 4083)
                        {
                            fwtamount = 76.60;
                            fwttax = 12;
                            lastAmount = 1731;
                        }
                        if (salary >= 4083 && salary < 7610)
                        {
                            fwtamount = 358.84;
                            fwttax = 22;
                            lastAmount = 4083;
                        }
                        if (salary >= 7610 && salary < 13652)
                        {
                            fwtamount = 1134.78;
                            fwttax = 24;
                            lastAmount = 7610;
                        }
                        if (salary >= 13652 && salary < 17075)
                        {
                            fwtamount = 2584.86;
                            fwttax = 32;
                            lastAmount = 13652;
                        }
                        if (salary >= 17075 && salary < 25131)
                        {
                            fwtamount = 3680.22;
                            fwttax = 35;
                            lastAmount = 17075;
                        }

                        if (salary >= 25131)
                        {
                            fwtamount = 6499.82;
                            fwttax = 37;
                            lastAmount = 25131;
                        }

                    }
                    else if (marriedStatus.ToLower() == "single" || marriedStatus.ToLower() == "singleormarriedfilingseparately")
                    {
                        salary = weeKsalary;
                        if (salary >= 0 && salary < 483)
                        {
                            fwtamount = 0;
                            fwttax = 0;
                            lastAmount = 0;
                        }
                        if (salary >= 483 && salary < 865)
                        {
                            fwtamount = 0;
                            fwttax = 10;
                            lastAmount = 483;
                        }
                        if (salary >= 865 && salary < 2041)
                        {
                            fwtamount = 38.20;
                            fwttax = 12;
                            lastAmount = 865;
                        }
                        if (salary >= 2041 && salary < 3805)
                        {
                            fwtamount = 179.32;
                            fwttax = 22;
                            lastAmount = 2041;
                        }
                        if (salary >= 3805 && salary < 6826)
                        {
                            fwtamount = 567.40;
                            fwttax = 24;
                            lastAmount = 3805;
                        }
                        if (salary >= 6826 && salary < 8538)
                        {
                            fwtamount = 1292.44;
                            fwttax = 32;
                            lastAmount = 6826;
                        }
                        if (salary >= 8538 && salary < 20621)
                        {
                            fwtamount = 1840.28;
                            fwttax = 35;
                            lastAmount = 8538;
                        }

                        if (salary >= 20621)
                        {
                            fwtamount = 6069.33;
                            fwttax = 37;
                            lastAmount = 20621;
                        }


                    }
                    else if (marriedStatus.ToLower() == "headofhousehold")
                    {
                        salary = weeKsalary;
                        if (salary >= 0 && salary < 723)
                        {
                            fwtamount = 0;
                            fwttax = 0;
                            lastAmount = 0;
                        }
                        if (salary >= 723 && salary < 1269)
                        {
                            fwtamount = 0;
                            fwttax = 10;
                            lastAmount = 723;
                        }
                        if (salary >= 1269 && salary < 2808)
                        {
                            fwtamount = 54.60;
                            fwttax = 12;
                            lastAmount = 1269;
                        }
                        if (salary >= 2808 && salary < 4044)
                        {
                            fwtamount = 239.28;
                            fwttax = 22;
                            lastAmount = 2808;
                        }
                        if (salary >= 4044 && salary < 7065)
                        {
                            fwtamount = 511.20;
                            fwttax = 24;
                            lastAmount = 4044;
                        }
                        if (salary >= 7065 && salary < 8777)
                        {
                            fwtamount = 1236.24;
                            fwttax = 32;
                            lastAmount = 7065;
                        }
                        if (salary >= 8777 && salary < 20862)
                        {
                            fwtamount = 1784.08;
                            fwttax = 35;
                            lastAmount = 8777;
                        }

                        if (salary >= 20862)
                        {
                            fwtamount = 6069.33;
                            fwttax = 37;
                            lastAmount = 20862;
                        }
                    }
                }
                else
                {
                    emptaxes.Fwt = 0;
                    emptaxes.NCState = 0;
                    emptaxes.NCSUTA = 0;
                }
                var amountfwt = 0.0;
                //fwtamount = fwtamount / 13;
                //emptaxes.Fwt = (fwtamount / weeKsalary) * 100;
                //emptaxes.Fwt = Math.Round(emptaxes.Fwt, 2);
                if (weeKsalary > 0)
                {
                    amountfwt = weeKsalary - lastAmount;
                    if (fwttax > 0)
                    {
                        amountfwt = amountfwt * (fwttax / 100);
                        amountfwt = amountfwt + fwtamount;
                        if (amountfwt > 0)
                        {
                            emptaxes.Fwt = (amountfwt / weeKsalary) * 100;
                        }
                    }
                    else
                    {
                        emptaxes.Fwt = 0;
                    }




                }
                else
                {
                    emptaxes.Fwt = 0;
                }


            }
            var empwithHold = _context.EmployeeWithHoldingTaxes.Where(x => x.EmployeeId == empid)?.FirstOrDefault();
            if (empwithHold != null)
            {
                if (empwithHold.FirstName != null)
                {
                    emptaxes.Fwt = 0;
                }

            }
            emptaxes.FUTA = 0.6;
            emptaxes.SSWH = 6.2;
            emptaxes.MCWH = 1.45;
            emptaxes.ERSS = 6.2;
            emptaxes.ERMC = 1.45;
            emptaxes.NCState = 5.25;
            emptaxes.NCSUTA = 2.27;
            return emptaxes;

        }
        private async Task<int> GenerateAutoManualPayroll(DateTime FromDate, DateTime ToDate, String Contractname)
        {
            int outputid = 0;
            bool validflag = true;
            // check payroll date already available


            ToDate = FromDate.AddDays(13);
            DateTime StartDate = FromDate;

            DateTime EndDate = ToDate;

            var allpayroll = _context.EmpPayRolls.ToList();
            var employees = _context.Employees.ToList();
            List<Employee> empwithPayroll = new List<Employee>();
            foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
            {
                foreach (var item in allpayroll)
                {
                    foreach (var employee in employees)
                    {
                        if (Convert.ToDateTime(item.ToDate) == day && item.EmpContractName == Contractname && item.EmployeeId == employee.EmployeeId)
                        {
                            validflag = false;
                            empwithPayroll.Add(employee);
                        }

                        if (Convert.ToDateTime(item.FromDate) == day && item.EmpContractName == Contractname && item.EmployeeId == employee.EmployeeId)
                        {
                            validflag = false;
                            empwithPayroll.Add(employee);
                        }
                    }


                    //if (Convert.ToDateTime(item.ToDate) == day || Convert.ToDateTime(item.FromDate) == day)
                    //{
                    //    validflag = false;
                    //}
                    //if (item.EmpContractName == Contractname)
                    //{
                    //    validflag = false;
                    //}
                    //else if (Contractname != null)
                    //{
                    //    validflag = true;
                    //}
                }

            }
            validflag = true;
            if (validflag == true)
            {
                var allemp = _context.Employees.ToList();
                foreach (var item in allemp)
                {
                    /////////////////////////
                    //var yeartodatesalatot = GetYTDSalary(item.EmployeeId, EndDate);
                    ////////////////////


                    var empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId).FirstOrDefault(); ;
                    if (Contractname == null)
                    {
                        empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId).FirstOrDefault();

                    }
                    else
                    {
                        empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId && y.ContractName == Contractname).FirstOrDefault();

                    }

                    //var empcontract = _context.EmployeeContracts.Where(y => y.EmployeeId == item.EmployeeId && y.ContractName == Contractname).FirstOrDefault();
                    if (empcontract != null)
                    {
                        var data = GenerateManualPayroll(item.EmployeeId, FromDate);
                        EmpPayRoll pay = new EmpPayRoll();
                        pay.EmployeeId = item.EmployeeId;
                        pay.DisperseDate = (DateTime.Now).ToString();
                        pay.Allowances = data.Allowance.ToString();
                        pay.Deductions = data.Deduction.ToString();
                        pay.Loans = data.Loans.ToString();
                        pay.Taxes = data.Taxes.ToString();
                        pay.TotalSalary = data.TotalSalary.ToString();
                        pay.IsApprove = false;
                        pay.FromDate = FromDate.ToString();
                        pay.ToDate = ToDate.ToString();
                        pay.EmpContractName = Contractname;
                        var rateperHour = 0.0;
                        var empContract = _context.EmployeeContracts.Where(x => x.EmployeeId == item.EmployeeId)?.FirstOrDefault();
                        if (empContract != null)
                        {
                            if (empContract.DailyWages == true && empContract.DailyWagesChargesAmount != null)
                            {
                                rateperHour = double.Parse(empContract.DailyWagesChargesAmount);
                            }
                        }
                        pay.BasicSalary = rateperHour.ToString();
                        var employeepayroll = empwithPayroll.Where(x => x.EmployeeId == pay.EmployeeId)?.FirstOrDefault();
                        if (employeepayroll == null)
                        {

                            if (data.Attendance > 0)
                            {
                                var output = await SaveEmpPayRoll(pay);
                                outputid = (int)output.EmpPayRollId;
                            }
                        }
                    }
                }

            }



            return outputid;


        }

        public async Task<EmpPayRoll> SaveEmpPayRoll(EmpPayRoll empPayRoll)
        {
            //_context.EmpPayRolls.Add(empPayRoll);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetEmpPayRoll", new { id = empPayRoll.EmpPayRollId }, empPayRoll);


            CultureInfo culture = new CultureInfo("en-US");
            DateTime date = DateTime.Parse(empPayRoll.FromDate, culture);
            var enddate = date.AddDays(13);
            string isoDate = enddate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            empPayRoll.ToDate = isoDate;
            empPayRoll.DisperseDate = DateTime.Now.ToString();
            var Response = ResponseBuilder.BuildWSResponse<EmpPayRoll>();
            var empLoan = _context.EmpLoans.Where(x => x.EmployeeId == empPayRoll.EmployeeId && x.IsPaid == false)?.FirstOrDefault();

            if (empLoan != null)
            {
                var newloan = decimal.Parse(empLoan.Amount) - decimal.Parse(empPayRoll.Loans);
                if (newloan == 0)
                {
                    empLoan.IsPaid = true;
                }
                empLoan.PaidDate = DateTime.Now;
            }
            var empDeduction = _context.EmpDeductions.Where(x => x.EmployeeId == empPayRoll.EmployeeId && x.IsClaim == false)?.FirstOrDefault();

            if (empDeduction != null)
            {
                empDeduction.IsClaim = true;
                empDeduction.ClaimDate = DateTime.Now;
            }

            await _context.EmpPayRolls.AddAsync(empPayRoll);
            await _context.SaveChangesAsync();
            return empPayRoll;


        }
    }
}