using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeLeaveEntitle
    {
        public int EleaveId { get; set; }
        public string NoofLeaves { get; set; }
        public string AvailableLeave { get; set; }
        public string PendingLeave { get; set; }
        public string ApprovedLeave { get; set; }
        public string RejectedLeave { get; set; }
        public int? LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
    }
}
