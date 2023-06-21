using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.MiddlewareModel
{
    public class Inventory
    {
        public int ID { get; set; }

        public string Item_Number { get; set; } = null!;

        public bool Active { get; set; }

        public string Class { get; set; }

        public string Category { get; set; }

        public string Sub_Category { get; set; }

        public string Family { get; set; }

        public bool IsCigarette { get; set; }

        public bool IsTobacco { get; set; }

        public string StateReport { get; set; }

        public string NPM_BrandCode { get; set; }

        public string Description { get; set; }

        public bool Promotion { get; set; }

        public string PDescription { get; set; }

        public string PromotionCode { get; set; }

        public string PromoType { get; set; }

        public string Manufacturer { get; set; }

        public string Model { get; set; }

        public DateTime LastInventoryDate { get; set; }

        public decimal Cost { get; set; }

        public decimal MfgPromotion { get; set; }

        public bool CostFixed { get; set; }

        public bool CostPerQty { get; set; }

        public bool NoSuchDisc { get; set; }

        public bool SellBelowCost { get; set; }

        public decimal Discount { get; set; }

        public DateTime DiscFrom { get; set; }

        public DateTime DiscTo { get; set; }

        public bool DiscR { get; set; }

        public bool DiscA { get; set; }

        public bool DiscB { get; set; }

        public bool DiscC { get; set; }

        public bool DiscD { get; set; }

        public bool CustDisc { get; set; }

        public decimal Retail { get; set; }

        public decimal PriceA { get; set; }

        public decimal PriceB { get; set; }

        public decimal PriceC { get; set; }

        public decimal PriceD { get; set; }

        public short HighA { get; set; }

        public short HighB { get; set; }

        public short HighC { get; set; }

        public short HighD { get; set; }

        public decimal PriceHA { get; set; }

        public decimal PriceHB { get; set; }

        public decimal PriceHC { get; set; }

        public decimal PriceHD { get; set; }

        public DateTime CostAsOf { get; set; }

        public string CostInfo { get; set; }

        public DateTime PriceAsOf { get; set; }

        public string PriceUser { get; set; }

        public string GroupCode { get; set; }

        public decimal SalePrice { get; set; }

        public decimal SalePriceOOS { get; set; }

        public int SaleQty { get; set; }

        public int SaleQtyOOS { get; set; }

        public decimal SaleQtyPrice { get; set; }

        public decimal SaleQtyPriceOOS { get; set; }

        public DateTime SaleFrom { get; set; }

        public DateTime SaleTo { get; set; }

        public decimal SRP { get; set; }

        public bool AutoSRP { get; set; }

        public decimal AddValue1 { get; set; }

        public decimal AddValue2 { get; set; }

        public decimal Case_Pack { get; set; }

        public short UCount { get; set; }

        public string Location { get; set; }

        public string Location2 { get; set; }

        public string Location3 { get; set; }

        public string Location4 { get; set; }

        public decimal Weight { get; set; }

        public decimal Wt_1 { get; set; }

        public decimal Wt_2 { get; set; }

        public string WeightUnit { get; set; }

        public string IColor { get; set; }

        public string UPC_Code { get; set; }

        public bool Serialized { get; set; }

        public bool No_Returns { get; set; }

        public bool AskForPrice { get; set; }

        public bool AskForDesc { get; set; }

        public bool Taxable { get; set; }

        public decimal AddToCost { get; set; }

        public decimal AddToCostPer { get; set; }

        public bool PTaxable { get; set; }

        public decimal PTax { get; set; }

        public decimal Quantity_in_Stock { get; set; }

        public decimal QuantityAdj { get; set; }

        public decimal Reorder_Point { get; set; }

        public decimal Reorder_Quantity { get; set; }

        public decimal Order_Quantity { get; set; }

        public decimal PrevSales { get; set; }

        public decimal PrevPurchases { get; set; }

        public decimal PrevAdj { get; set; }

        public decimal CommRate { get; set; }

        public short OrderSeq { get; set; }

        public bool PhyInv { get; set; }

        public DateTime InvCreated { get; set; }

        public DateTime LastUsed { get; set; }

        public bool NewItem { get; set; }

        public decimal OSCost { get; set; }

        public decimal OSRetail { get; set; }

        public int OSQty { get; set; }

        public decimal OSQtyRetail { get; set; }

        public bool isWeb { get; set; }

        public decimal WebRetail { get; set; }

        public bool isBestDeal { get; set; }

        public string WebDetail { get; set; }

        public string StateTaxJurisdiction { get; set; }

        public bool isAutoUpdate { get; set; }

        public string LabelDesc { get; set; }

        public bool isLabel { get; set; }

        public bool isCMPP { get; set; }

        public string InvoiceUPC { get; set; }

        public int CaseQuantity { get; set; }

        public decimal ProfitPercent { get; set; }

        public decimal OSProfitPercent { get; set; }

        public int PalletQuantity { get; set; }

        public DateTime LastSales { get; set; }

        public decimal QtyLastInvAdj { get; set; }

        public decimal TaxedWeight { get; set; }

        public short QtyBuy { get; set; }

        public short QtyGet { get; set; }

        public decimal QtyDiscount { get; set; }

        public DateTime QtyGetFrom { get; set; }

        public DateTime QtyGetTo { get; set; }

        public decimal DiscountPer { get; set; }

        public decimal PrevCost { get; set; }

        public decimal PrevRetail { get; set; }

        public decimal UnitCharge { get; set; }

        public int SalesLimit { get; set; }

        public string InvDescription { get; set; }

        public bool ItemDetailsView { get; set; }

        public string ItemDetails { get; set; }

        public int RepZioQty { get; set; }

        public DateTime RepZioDate { get; set; }

        public short HighR { get; set; }

        public decimal PriceHR { get; set; }

        public string TaxedWeightUnit { get; set; }
    }
}
