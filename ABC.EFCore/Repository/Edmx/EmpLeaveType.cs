using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpLeaveType
    {
        public int EmpLeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string Description { get; set; }
        public int? LeaveLimit { get; set; }
        public bool? IsActive { get; set; }
    }
}
