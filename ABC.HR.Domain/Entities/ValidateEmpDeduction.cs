using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateEmpDeduction
    {
        [Required(ErrorMessage ="Please Select Deduction ")]
        [Display(Name = "Employee Deduction Type Id")]
        public int? EmpDeductionTypeId { get; set; }
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]
        public int? EmployeeId { get; set; }
        [Required]
        [Display(Name = "Reason")]
        [RegularExpression(@"^(?:[a-zA-Z0-9]+\s?)+[a-zA-Z0-9]+$", ErrorMessage = "White Space Not Allowed")]
        public string Reason { get; set; }
        [Required]
        [Display(Name = "Amount")]
        public string Amount { get; set; }
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        [Required]
        [Display(Name = "Is Approve")]
        public bool IsApprove { get; set; }
    }
}
