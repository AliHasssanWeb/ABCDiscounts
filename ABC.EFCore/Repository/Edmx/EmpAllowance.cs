using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpAllowance
    {
        public int EmpAllowanceId { get; set; }
        public int? EmployeeId { get; set; }
        public int? AllowanceTypeId { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Reason { get; set; }
        public bool? IsApprove { get; set; }
    }
}
