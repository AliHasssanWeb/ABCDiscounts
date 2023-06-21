using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateAllowanceType
    {
        
            [Display(Name = "Allowance Type Name")]
            [Required]
            [MaxLength(50)]
            [RegularExpression(@"^[a-zA-Z 0-9]+$", ErrorMessage = "WhiteSpace Not Allowed")]
            public string AllowanceTypeName { get; set; }

            [Display(Name = "Allowance Type Description")]
            [Required]
            [MaxLength(150)]
            [RegularExpression(@"^[a-zA-Z 0-9]+$", ErrorMessage = "WhiteSpace Not Allowed")]

            public string Description { get; set; }

            [Display(Name = "Allowance Ammount")]
            [Required]

            [Range(1, 1000000000, ErrorMessage = "Value must be between 1 to 1000000000")]
            public int? AmountLimit { get; set; }

            public bool? IsActive { get; set; }

    }

    //[MetadataType(typeof(ValidateAllowanceType))]
    //public partial class EmpAllowanceType
    //{

    //}

}
