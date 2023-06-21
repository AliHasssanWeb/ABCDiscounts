using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ManualPayRoll
    {
        public double? Allowance { get; set; }
        public double? Deduction { get; set; }
        public double? Loans { get; set; }
        public double? Taxes { get; set; }
        public double? TotalSalary { get; set; }
        public double? Attendance { get; set; }
        public string Status { get; set; }
        public List<EmpAllowance>  Allowances { get; set; }
        public List<EmpLoan> LoansList { get; set; }
        public List<EmpDeduction> Deductions { get; set; }
    }
}
