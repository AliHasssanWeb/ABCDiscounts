using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABC.EFCore.Repository.Edmx;

namespace ABC.EFCore.Repository.Edmx
{
    public class EmployeeWithHold
    {
        public EmployeeWithHoldingTax EmployeeWithHoldingTax { get; set; }
        public EmploymentEligibilityVerification EmploymentEligibilityVerification { get; set; }
        public EmployeeAuthorizedRepresentative employeeAuthorizedRepresentative { get; set; }
        public EmployeeReverificationAndRehire EmployeeReverificationAndRehire { get; set; }
        public ABC.EFCore.Entities.HR.EmployeeDdauthorization EmployeeDdauthorization { get; set; }
       
    }
    [MetadataType(typeof(EmployeeWithHold))]
    public partial class Employee
    {
        [NotMapped]
        public EmployeeWithHoldingTax EmployeeWithHoldingTax { get; set; }
        [NotMapped]
        public EmploymentEligibilityVerification EmploymentEligibilityVerification { get; set; }
        [NotMapped]

        public EmployeeAuthorizedRepresentative employeeAuthorizedRepresentative { get; set; }
        [NotMapped]

        public EmployeeReverificationAndRehire EmployeeReverificationAndRehire { get; set; }
        [NotMapped]
        public EmployeeDdauthorization EmployeeDdauthorization { get; set; }



    }
}
