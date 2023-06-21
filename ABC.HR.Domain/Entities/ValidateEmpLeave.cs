using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateEmpLeave
    {
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]
        public int? EmployeeId { get; set; }
        [Required]
        [Display(Name = "From Date")]
        [DataType(DataType.Date)]
        public DateTime? FromDate { get; set; }
        [Required]
        [Display(Name = "To Date")]
        [DataType(DataType.Date)]
        public DateTime? ToDate { get; set; }
        [Required]
        [Display(Name = "Reason")]
        [RegularExpression(@"^(?:[a-zA-Z0-9]+\s?)+[a-zA-Z0-9]+$", ErrorMessage = "White Space Not Allowed")]
        public string Reason { get; set; }
        [Required]
        [Display(Name = "Select Leave Type")]
        public int? EmpLeaveTypeId { get; set; }
        
        [Display(Name = "Is Approve")]
        public bool IsApprove { get; set; }
    }
}
