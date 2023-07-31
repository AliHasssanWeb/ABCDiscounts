using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class ItemCostChange
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int? ItemId { get; set; }
        public string PoInvoiceNumber { get; set; }
        public string ItemNumber { get; set; }
        public string OldPrice { get; set; }
        public string NewPrice { get; set; }
        public string StockTemNumber { get; set; }
        public string PriceDiff { get; set; }
        public string PricePercentage { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CretedBy { get; set; }
    }
}
