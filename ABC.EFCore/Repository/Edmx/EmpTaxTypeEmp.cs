using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpTaxTypeEmp
    {
        public int EmpTaxTypeEmpId { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmpTaxTypeId { get; set; }
        public string TaxPer { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsApprove { get; set; }
    }
}
