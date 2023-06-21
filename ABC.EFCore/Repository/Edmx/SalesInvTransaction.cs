using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SalesInvTransaction
    {
        public int Id { get; set; }
        public int? SalesInvoiceId { get; set; }
        public string AmountPaid { get; set; }
        public string AmountAllocate { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? HoldDate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}
