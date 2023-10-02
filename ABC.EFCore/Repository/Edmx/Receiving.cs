using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Receiving
    {
        public int ReceivingId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public string AccountId { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public string InvoiceTypeId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string TaxAmount { get; set; }
        public string PaymentType { get; set; }
        public string Note { get; set; }
        public string PayFromAccountId { get; set; }
        public string PayFromAccount { get; set; }
        public string PayFromAccountNumber { get; set; }
        public string CheckNumber { get; set; }
        public string CheckTitle { get; set; }
        public DateTime? CheckDate { get; set; }
        public string CashBalance { get; set; }
        public string NetAmount { get; set; }
        public string CashierUser { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public DateTime? DueDate { get; set; }
        public string Change { get; set; }
        public string SubTotal { get; set; }
        public string InvTotal { get; set; }
        public string InvBalance { get; set; }
        public string PreBalance { get; set; }
        public string Other { get; set; }
        public string Freight { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsPaid { get; set; }
        public bool? IsPaidFirst { get; set; }
        public int? UserId { get; set; }

        public virtual CustomerInformation Customer { get; set; }
        public virtual AspNetUser User { get; set; }
    }
}
