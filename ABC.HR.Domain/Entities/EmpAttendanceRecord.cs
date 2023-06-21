using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class EmpAttendanceRecord
    {
        public int EmployeeID { get; set; }
        public int TotalAttendance { get; set; }
        public int TotalLeaves { get; set; }
        public int TotalLate { get; set; }
        public double TotalOverTimeHours { get; set; }
        public double TotalWorkingHours { get; set; }
        public double TotalAttendHours { get; set; }
        public int TotalEarlyLeave { get; set; }
    }
}
