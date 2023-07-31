using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class EmpDeduction
    {
        public int EmpDeductionId { get; set; }
        public int? EmpDeductionTypeId { get; set; }
        public int? EmployeeId { get; set; }
        public string Reason { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsClaim { get; set; }
        public DateTime? ClaimDate { get; set; }
    }
}
