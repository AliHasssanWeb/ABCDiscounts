using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PaymentDetail
    {
        public int PaymentDetailId { get; set; }
        public int? PaymentId { get; set; }
        public string InvoiceNumber { get; set; }
        public string AmountPaid { get; set; }
        public string AmountAlloc { get; set; }
        public string PaymentType { get; set; }
        public string CkcardNumber { get; set; }
        public DateTime? HoldDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
