using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateAllowanceConfig
    {
        [Required(ErrorMessage = "Please Select Employee")]
        [Display(Name = "Select Employee")]

        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "Please Select Allowance")]
        [Display(Name = "Select Allownce Type")]
        public int EmpAllowanceTypeId { get; set; }

                

        [Required]
         [DataType(DataType.Date)]
         public DateTime? Date { get; set; }
        
        [Display(Name = "Allowance Percentage")]
        public string AllowanceTypePer { get; set; }
        public string AllowanceTypeFix { get; set; }
        [Display(Name = "Check If Want to Add Accordiong to Total Pay %")]
        public bool IsPerFix { get; set; }
       
        public bool IsApprove { get; set; }
    }

    //[ModelMetadataTypeAttribute(typeof(ValidateAllowanceConfig))]
    //public partial class EmpAllowanceTypeEmp
    //{

    //}
}
