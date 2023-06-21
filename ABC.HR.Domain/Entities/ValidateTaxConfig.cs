using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateTaxConfig
    {
         
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]
        public int? EmployeeId { get; set; }

        [Required(ErrorMessage = "Please Select Tax")]
        [Display(Name = "Select Tax Type")]
        public int? EmpTaxTypeId { get; set; }

        [Display(Name = "Tax Amount Percentage")]
        [Required]
        [MaxLength(20)]
        
        public string TaxPer { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        public bool? IsApprove { get; set; }
    }
}
