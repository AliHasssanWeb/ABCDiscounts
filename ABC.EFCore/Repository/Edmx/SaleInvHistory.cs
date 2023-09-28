using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SaleInvHistory
    {
        public int SaleInvHistoryId { get; set; }
        public int? SaleInvTransactionId { get; set; }
        public string InvoiceNumber { get; set; }
        public string AmountAllocate { get; set; }

        public virtual SalesInvTransaction SaleInvTransaction { get; set; }
    }
}
