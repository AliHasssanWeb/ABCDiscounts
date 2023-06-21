using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateDeductionType
    {
        [Display(Name = "Detuction Type Name")]
        [Required]
        [MaxLength(50)]
        public string EmpDeductionTypeName { get; set; }

        [Display(Name = "Deduction Type Details")]
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        [Display(Name = "Deduction Ammount")]
        [Required]
        [MaxLength(30)]
        public string AmountLimit { get; set; }


        public bool IsActive { get; set; }
    }
}
