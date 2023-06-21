using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public string InvoiceNumber { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public string DetailAccountId { get; set; }
        public string ClosingBalance { get; set; }
    }
}
