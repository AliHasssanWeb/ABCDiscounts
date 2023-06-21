using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.HR.Domain.Entities
{
    public class LeaveTypes
    {
        public int LeaveTypeID { get; set; }
        [Required(ErrorMessage = "Leave Type is required",
            AllowEmptyStrings = false)]
        [StringLength(50)]
        [RegularExpression(@"^\S*$", ErrorMessage = "No white space allowed")]
        public string TypeName { get; set; }
    }
}
