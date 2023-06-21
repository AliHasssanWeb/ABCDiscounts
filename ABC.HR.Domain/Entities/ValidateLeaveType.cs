using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class ValidateLeaveType
    {
        [Display(Name = "Leave Type Name")]
        [Required]
        [MaxLength(50)]
        public string LeaveTypeName { get; set; }
        [Display(Name = "Details")]
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        [Display(Name = "Leave Limit")]
        [Required]
        [Range(1, 50, ErrorMessage = "Value must be between 1 to 50")]

        public int? LeaveLimit { get; set; }

        [Display(Name = "Is Active")]

        //[Compare("IsActive", ErrorMessage = "You must accept the terms and conditions")]
        public bool IsActive { get; set; }
    }
}
