using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class SalesInvoicesAdp
    {
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PrintedDate { get; set; }
        public string SalesmanName { get; set; }
        public string UserName { get; set; }
        public string InvTotal { get; set; }
        public string TotalPaid { get; set; }
        public string InvBalance { get; set; }
    }
}
