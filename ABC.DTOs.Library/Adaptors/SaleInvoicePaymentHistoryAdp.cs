using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class SaleInvoicePaymentHistoryAdp
    {
        public string AmountPaid { get; set; }
        public string AmountAllocate { get; set; }
        public string PaymentType { get; set; }
        public string CardNumber { get; set; }
        public DateTime? HoldDate { get; set; }
    }
}
