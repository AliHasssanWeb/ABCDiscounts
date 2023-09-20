using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public class InvStockModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ItemName { get; set; }
        public int? ItemCategoryId { get; set; }
        public string ItemBarCode { get; set; }
        public string BarCode { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string UnitRetail { get; set; }
        public string SaleRetail { get; set; }
        public bool? ShippingEnable { get; set; }
        public string OldPrice { get; set; }
        public string SalesLimit { get; set; }
        public string Cost { get; set; }
        public string ItemNumber { get; set; }
        public int StockId { get; set; }
        public int? ItemId { get; set; }
        public string DiscountPrice { get; set; }
        public string Retail { get; set; }
        public string StockItemNumber { get; set; }
        public string ShipmentLimit { get; set; }
        public  int FixedCost { get; set; }
        public string Quantity { get; set; }
        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public int stTax { get; set; }
        public string Price { get; set; }
        public string Profit { get; set; }
        public bool? NeedHighAuthorization { get; set; }
        public string HighlimitOn { get; set; }
    }
}
