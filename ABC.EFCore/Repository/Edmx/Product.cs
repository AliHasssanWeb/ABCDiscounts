using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Product
    {
        public Product()
        {
            PointOfSaleDetails = new HashSet<PointOfSaleDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? ItemCategoryId { get; set; }
        public int? BrandId { get; set; }
        public int? ArticleId { get; set; }
        public int? StoreId { get; set; }
        public string Unit { get; set; }
        public string ProductCode { get; set; }
        public string BarCode { get; set; }
        public string Size { get; set; }
        public int? ColorId { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string UnitRetail { get; set; }
        public string SaleRetail { get; set; }
        public bool? TaxExempt { get; set; }
        public bool? ShippingEnable { get; set; }
        public bool? AllowECommerce { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string OldPrice { get; set; }
        public string MsareportAs { get; set; }
        public string StateReportAs { get; set; }
        public string ReportingWeight { get; set; }
        public int? FamilyId { get; set; }
        public string Family { get; set; }
        public string QtyUnit { get; set; }
        public string UnitsInPack { get; set; }
        public string RetailPackPrice { get; set; }
        public string SalesLimit { get; set; }
        public string Adjustment { get; set; }
        public string ProfitPercentage { get; set; }
        public string Cost { get; set; }
        public string MfgPromotion { get; set; }
        public string AddtoCostPercenatge { get; set; }
        public string UnitCharge { get; set; }
        public string OutofstateCost { get; set; }
        public string OutofstateRetail { get; set; }
        public bool? TaxonSale { get; set; }
        public bool? TaxOnPurchase { get; set; }
        public string Location { get; set; }
        public int? GroupId { get; set; }
        public string ItemNumber { get; set; }
        public string QtyinStock { get; set; }
        public int? ItemSubCategoryId { get; set; }
        public int? ModelId { get; set; }
        public string ModelName { get; set; }
        public string CategoryName { get; set; }
        public string SubCatName { get; set; }
        public string GroupName { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public byte[] ItemImage { get; set; }
        public string ItemImageByPath { get; set; }
        public string Variations { get; set; }
        public string DiscountPrice { get; set; }
        public string Rating { get; set; }
        public string MinOrderQty { get; set; }
        public string MaxOrderQty { get; set; }
        public string Retail { get; set; }
        public string QuantityCase { get; set; }
        public string QuantityPallet { get; set; }
        public string SingleUnitMsa { get; set; }
        public int? MisPickId { get; set; }
        public string MisPickName { get; set; }
        public string OrderQuantity { get; set; }
        public string Units { get; set; }
        public string WeightOz { get; set; }
        public string WeightLb { get; set; }
        public string LocationTwo { get; set; }
        public string LocationThree { get; set; }
        public string LocationFour { get; set; }
        public string MaintainStockForDays { get; set; }
        public bool? IsActive { get; set; }
        public string StockItemNumber { get; set; }
        public string ShipmentLimit { get; set; }
        public string BarCodePath { get; set; }
        public bool? NeedHighAuthorization { get; set; }
        public string HighlimitOn { get; set; }
        public bool? SttaxOnCost { get; set; }
        public bool? SttaxOnSale { get; set; }
        public string SttaxRate { get; set; }
        public string SttaxPerQty { get; set; }
        public string PriceDiff { get; set; }
        public string PriceDiffPercentage { get; set; }

        public virtual ICollection<PointOfSaleDetail> PointOfSaleDetails { get; set; }
    }
}
