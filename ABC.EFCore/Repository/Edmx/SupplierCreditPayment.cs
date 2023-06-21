using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SupplierCreditPayment
    {
        public int CreditPaymentId { get; set; }
        public int? PaymentId { get; set; }
        public string Pobalance { get; set; }
        public string Ponumber { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CreditAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? CreditDate { get; set; }
        public string TotalCredit { get; set; }
        public string Amount { get; set; }
    }
}
