using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateLoanType
    {
        [Display(Name = "Loan Type Name")]
        [Required]
        [MaxLength(50)]
        public string EmpLoanTypeName { get; set; }

        [Display(Name = "Details")]
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        [Display(Name = "Loan Amount")]
        [Required]
        [MaxLength(50)]
        public string AmountLimit { get; set; }
        public bool IsActive { get; set; }
    }
}
