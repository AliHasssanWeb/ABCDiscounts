using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
   public class EmpLeaveEntitleValid
    {

        [Required(ErrorMessage ="No. of Leave Required.")]
        public string NoofLeaves { get; set; }
    }
    [MetadataType(typeof(EmpLeaveEntitleValid))]
    public partial class EmployeeLeaveEntitle
    {

    }
}
