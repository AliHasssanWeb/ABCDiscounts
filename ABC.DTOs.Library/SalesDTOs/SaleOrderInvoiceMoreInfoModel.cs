using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public class SaleOrderInvoiceMoreInfoModel
    {
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string PreviousBalance { get; set; }
        public string InvoiceTotal { get; set; }
        public string AmountDue { get; set; }
        public string InvoiceBalance { get; set; }
    }
}
