using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpAttendance
    {
        public int EmpAttendanceId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? AttendanceDate { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Late { get; set; }
        public string OverTime { get; set; }
        public bool? IsApprove { get; set; }
    }
}
