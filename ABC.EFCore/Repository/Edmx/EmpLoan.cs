using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpLoan
    {
        public int EmpLoanId { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmpLoanTypeId { get; set; }
        public DateTime? Date { get; set; }
        public string Reason { get; set; }
        public string Amount { get; set; }
        public bool? IsApprove { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
    }
}
