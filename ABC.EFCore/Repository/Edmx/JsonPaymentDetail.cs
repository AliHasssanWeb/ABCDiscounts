using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class JsonPaymentDetail
    {
        public string PaymentDetailId { get; set; }
        public string PaymentId { get; set; }
        public string InvoiceNumber { get; set; }
        public string AmountPaid { get; set; }
        public string AmountAlloc { get; set; }
        public string PaymentType { get; set; }
        public string CkcardNumber { get; set; }
        public string HoldDate { get; set; }
        public string PaymentDate { get; set; }
    }
}
