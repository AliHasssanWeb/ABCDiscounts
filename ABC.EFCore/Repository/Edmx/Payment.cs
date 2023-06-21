using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string InvoiceNumber { get; set; }
        public string TotalBill { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string TotalPaid { get; set; }
        public string TotalAlloc { get; set; }
        public string Change { get; set; }
        public bool? IsBalanceToChange { get; set; }
        public string Balance { get; set; }
        public bool? IsEmailCopy { get; set; }
        public bool? IsTextCopy { get; set; }
        public bool? IsStandardReceipt { get; set; }
        public string OrderBy { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string ByUser { get; set; }
        public int? ByUserId { get; set; }
    }
}
