using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateEmpPayRoll
    {
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Selcet Employee")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage ="Select Contract Type")]
        public string Month { get; set; }

        public string Year { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "From Date")]
        public DateTime FromDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "To Date")]
        public DateTime ToDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Disperse Date")]
        public string DisperseDate { get; set; }
        public string BasicSalary { get; set; }
        public string Allowances { get; set; }
        public string Deductions { get; set; }
        public string Loans { get; set; }
        public string Taxes { get; set; }
        public string TotalSalary { get; set; }
        public bool IsApprove { get; set; }

        public string PayWithoutDeduction { get; set; }

        public string EmpContractName { get; set; }
    }
}
