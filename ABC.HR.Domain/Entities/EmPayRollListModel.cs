using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace ABC.HR.Domain.Entities
{
    public class EmPayRollListModel
    {
        public List<EmpAllowance> empAllowance { get; set; }

        public Employee employee { get; set; }
       
        public List<EmpTax> empTax { get; set; }
        public EmpPayRoll empPayRoll { get; set; }
        public List<EmpDeduction> empDeduction { get; set; }
        public List<EmpLoan> empLoan { get; set; }

        public string empLeaveTypeName { get; set; }
        public List<EmpDeductionType> empDeductionTypeName { get; set; }

        public  string empLoanTypeName { get; set; }
        public List<EmpTaxType> empTaxTypeName { get; set; }
        public List<EmpLeaveType> empLeaveTypes { get; set; }       
       public  EmployeeContract employeeContract { get; set; }       

        public EmpAttendanceRecord attendanceRecord { get; set; }

        public double YTDAllowance { get; set; }

        public double YTDSalary { get; set; }

        public List<YTDuniqueTax> YTDTaxes { get; set; }
        public List<YTDuniqueDeduction> YTDDeductinss { get; set; }
        public EmppayrollTaxes EmppayrollTaxes { get; set; }

        
    }


    public class YTDuniqueDeduction
    {
        public int EmployeeID { get; set; }

        public int? Empdedctiontypeid { get; set; }

        public double ytddeductionamout { get; set; }
    }


    public class YTDuniqueTax
    {
        public int EmployeeID { get; set; }

        public int? EmpTaxtypeid { get; set; }

        public double ytdtaxamout { get; set; }

        public bool? taxstatus { get; set; }
        public bool? ertaxstatus { get; set; }

    }
}
