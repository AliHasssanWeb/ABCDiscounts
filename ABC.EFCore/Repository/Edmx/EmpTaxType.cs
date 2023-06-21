using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpTaxType
    {
        public int EmpTaxTypeId { get; set; }
        public string EmpTaxTypeName { get; set; }
        public string Description { get; set; }
        public string AmountLimit { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTax { get; set; }
        public bool? IsErTax { get; set; }
        public string TaxType { get; set; }
        public string SalaryRange { get; set; }
    }
}
