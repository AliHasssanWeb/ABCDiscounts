using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpLoanType
    {
        public int EmpLoanTypeId { get; set; }
        public string EmpLoanTypeName { get; set; }
        public string Description { get; set; }
        public string AmountLimit { get; set; }
        public bool? IsActive { get; set; }
    }
}
