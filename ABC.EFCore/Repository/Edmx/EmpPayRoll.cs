using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpPayRoll
    {
        public int EmpPayRollId { get; set; }
        public int? EmployeeId { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string DisperseDate { get; set; }
        public string BasicSalary { get; set; }
        public string Allowances { get; set; }
        public string Deductions { get; set; }
        public string Loans { get; set; }
        public string Taxes { get; set; }
        public string TotalSalary { get; set; }
        public bool? IsApprove { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PayWithoutDeduction { get; set; }
        public string EmpContractName { get; set; }
    }
}
