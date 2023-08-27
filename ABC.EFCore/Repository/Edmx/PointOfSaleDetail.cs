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
        public int? Quantity { get; set; }
        public double? InDiscount { get; set; }
        public double? OutDiscount { get; set; }
        public double? InUnit { get; set; }
        public double? OutUnit { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }
        public double? RingerQty { get; set; }

        public virtual Product Item { get; set; }
        public virtual PointOfSale PointOfSale { get; set; }
    }
}
