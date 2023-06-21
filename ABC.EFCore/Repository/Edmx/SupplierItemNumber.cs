using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SupplierItemNumber
    {
        public int SupplierItemNumberId { get; set; }
        public int? VendorId { get; set; }
        public int? ProductId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SupplierItemNum { get; set; }
    }
}
