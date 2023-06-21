using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.Admin
{
    public class RegisterCustomer
    {

        public CustomerInformation Customer { get; set; }
        public EmpBusiness EmpBusinesss { get; set; }
        public EmpReasonExemption EmpReasonExemption { get; set; }
        public EmpIdentification EmpIdentification { get; set; }
    

    }
}
