using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class InventoryStock
    {
        public int StockId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemBarCode { get; set; }
        public string Quantity { get; set; }
        public string Sku { get; set; }
        public int? ProductId { get; set; }
        public string StockItemNumber { get; set; }
        public string RemainingDays { get; set; }
    }
}
