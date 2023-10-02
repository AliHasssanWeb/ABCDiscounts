using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PointOfSaleDetailModel
    {
        public int PosSaleDetailId { get; set; }
        public string PointOfSaleId { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNumber { get; set; }
        public string Quantity { get; set; }
        public string InDiscount { get; set; }
        public string OutDiscount { get; set; }
        public string InUnit { get; set; }
        public string OutUnit { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string RingerQty { get; set; }
        // Item Name Description
        public string Description { get; set; }
        public string AmountRetail { get; set; }
        public string SalesLimit { get; set; }
        public bool? NeedHighAuthorization { get; set; }
        public string HighLimitOn { get; set; }
        public string StockQty { get; set; }
        //public InventoryStock stock { get; set; }
        //  public virtual Product Item { get; set; }
        //   public virtual PointOfSaleModel PointOfSale { get; set; }
    }
}
