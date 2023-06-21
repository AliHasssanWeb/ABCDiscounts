using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpDeductionType
    {
        public int EmpDeductionTypeId { get; set; }
        public string EmpDeductionTypeName { get; set; }
        public string Description { get; set; }
        public string AmountLimit { get; set; }
        public bool? IsActive { get; set; }
    }
}
