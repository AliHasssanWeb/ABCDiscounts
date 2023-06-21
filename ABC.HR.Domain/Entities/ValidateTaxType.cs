using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
   public class ValidateTaxType
    {
        [Required]
        [Display(Name = "Tax Type Name")]
        [MaxLength(50)]
        public string EmpTaxTypeName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Tax Amount Percentage")]
        [MaxLength(20)]
        public string AmountLimit { get; set; }
        public bool IsActive { get; set; }

        public bool IsTax { get; set; }
        public bool IsErTax { get; set; }
        
        [Required]
        [Display(Name = "TaxType")]
        public string TaxType { get; set; }
        [Required]
        public string SalaryRange { get; set; }
    }
}
