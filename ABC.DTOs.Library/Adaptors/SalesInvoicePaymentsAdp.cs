using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class SalesInvoicePaymentsAdp
    {
        public DateTime? PaidDate { get; set; }
        public string Time { get; set; }
        public DateTime? HoldDate { get; set; }
        public int? SID { get; set; }
        public string SalemanName { get; set; }
        public string PaymentTypeName { get; set; }
        public string ChequeNumber { get; set; }
        public string AmountPaid { get; set; }
        public string AmountAllocate { get; set; }
        public string Change { get; set; }
    }
}
