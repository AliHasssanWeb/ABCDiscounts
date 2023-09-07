using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SystemCount
    {
        public int SystemCountId { get; set; }
        public long PurchaseInvoiceCount { get; set; }
        public long SaleInvoiceCount { get; set; }
        public long CustomerAccountNoCount { get; set; }
    }
}
