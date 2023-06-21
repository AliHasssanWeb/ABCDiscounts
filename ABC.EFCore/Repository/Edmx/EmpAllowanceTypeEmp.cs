using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpAllowanceTypeEmp
    {
        public int EmpAllowanceTypeEmpId { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmpAllowanceTypeId { get; set; }
        public string AllowanceTypePer { get; set; }
        public string AllowanceTypeFix { get; set; }
        public bool? IsPerFix { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsApprove { get; set; }
    }
}
