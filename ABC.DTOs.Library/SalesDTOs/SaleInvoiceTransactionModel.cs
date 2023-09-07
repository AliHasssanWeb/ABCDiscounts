using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public partial class SaleInvoiceTransactionModel
    {
        public string AmountPaid { get; set; }
        public string ChequeNumber { get; set; }
        public string AmountAllocate { get; set; }
        public string PaymentType { get; set; }
        public DateTime? HoldDate { get; set; }
        public string InvoiceNumber { get; set; }

    }
}
