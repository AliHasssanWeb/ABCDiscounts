using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class SalesInvoicesJson
    {
        public int? SalesInvoiceId { get; set; }
        public string AmountPaid { get; set; }
        public string AmountAllocate { get; set; }
        public string PaymentType { get; set; }
        public string ChequeNumber { get; set; }
        public DateTime? HoldDate { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string TotalPaid { get; set; }
        public string TotalAmount { get; set; }
        public string Change { get; set; }
        public string Balance { get; set; }
        public string InvoiceBalance { get; set; }
        public string PreviousBalance { get; set; }
        public string Buyer { get; set; }
    }
}
