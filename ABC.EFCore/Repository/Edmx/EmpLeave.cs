using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpLeave
    {
        public int EmpLeaveId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Reason { get; set; }
        public int? EmpLeaveTypeId { get; set; }
        public bool? IsApprove { get; set; }
    }
}
