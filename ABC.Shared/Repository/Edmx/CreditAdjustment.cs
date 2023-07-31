using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class CreditAdjustment
    {
        public int AdjustmentId { get; set; }
        public string UserName { get; set; }
        public int? UserId { get; set; }
        public string CompanyName { get; set; }
        public string InvoiceNo { get; set; }
        public string CreditAmount { get; set; }
        public string RemainingAmount { get; set; }
        public DateTime? CreditDate { get; set; }
        public string Type { get; set; }
        public string Comments { get; set; }
    }
}
