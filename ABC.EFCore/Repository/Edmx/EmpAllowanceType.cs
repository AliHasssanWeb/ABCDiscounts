using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmpAllowanceType
    {
        public int EmpAllowanceTypeId { get; set; }
        public string AllowanceTypeName { get; set; }
        public string Description { get; set; }
        public int? AmountLimit { get; set; }
        public bool? IsActive { get; set; }
    }
}
