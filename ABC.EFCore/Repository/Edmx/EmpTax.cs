using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpTax
    {
        public int EmpTaxId { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmpTaxTypeId { get; set; }
        public string Reason { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public bool? IsApprove { get; set; }
    }
}
