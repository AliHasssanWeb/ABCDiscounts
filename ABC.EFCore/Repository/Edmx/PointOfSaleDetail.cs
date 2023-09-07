using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PointOfSaleDetail
    {
        public int PosSaleDetailId { get; set; }
        public int? PointOfSaleId { get; set; }
        public int ItemId { get; set; }
        public string Quantity { get; set; }
        public string InDiscount { get; set; }
        public string OutDiscount { get; set; }
        public string InUnit { get; set; }
        public string OutUnit { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string RingerQty { get; set; }

        public virtual Product Item { get; set; }
        public virtual PointOfSale PointOfSale { get; set; }
    }
}
