using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.HR
{
    public class EmployeeRegister
    {
        public int EmployeeID { get; set; }
        public int RoleId { get; set; }

        public int SupervisorId { get; set; }
        public string RoleName { get; set; }
        public string AccessPin { get; set; }
        public string CreditLimit { get; set; }
        public string UserId { get; set; }
    }
}
