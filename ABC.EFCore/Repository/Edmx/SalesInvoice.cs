using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SalesInvoice
    {
        public int Id { get; set; }
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
        public bool? IsInvoicedPaid { get; set; }
        public string Charges { get; set; }
    }
}
