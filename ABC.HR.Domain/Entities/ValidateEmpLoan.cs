using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
     public class ValidateEmpLoan
    {
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Please Select Loan")]
        [Display(Name = "Select Loan Type")]
        public int? EmpLoanTypeId { get; set; }

        [Required]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Details")]
        [Required]
        [MaxLength(150)]
        [RegularExpression(@"^[a-zA-Z 0-9]+$", ErrorMessage = "WhiteSpace Not Allowed")]
        public string Reason { get; set; }

        [Display(Name = "Loan Amount")]
        [Required]
        [MaxLength(50)]
        [DataType(DataType.Currency)]
        public string Amount { get; set; }
        public bool IsApprove { get; set; }
    }
}
