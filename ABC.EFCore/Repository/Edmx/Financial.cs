using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Financial
    {
        public int FinancialId { get; set; }
        public string Cost { get; set; }
        public string Profit { get; set; }
        public string MsgPromotion { get; set; }
        public string AddToCost { get; set; }
        public string UnitCharge { get; set; }
        public bool? FixedCost { get; set; }
        public bool? CostPerQuantity { get; set; }
        public string St { get; set; }
        public string Tax { get; set; }
        public string OutOfStateCost { get; set; }
        public string OutOfStateRetail { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public string QuantityPrice { get; set; }
        public string SuggestedRetailPrice { get; set; }
        public bool? AutoSetSrp { get; set; }
        public string ItemNumber { get; set; }
        public string QuantityInStock { get; set; }
        public string Adjustment { get; set; }
        public bool? AskForPricing { get; set; }
        public bool? AskForDescrip { get; set; }
        public bool? Serialized { get; set; }
        public bool? TaxOnSales { get; set; }
        public bool? Purchase { get; set; }
        public bool? NoSuchDiscount { get; set; }
        public bool? NoReturns { get; set; }
        public bool? SellBelowCost { get; set; }
        public string OutOfState { get; set; }
        public bool? CodeA { get; set; }
        public bool? CodeB { get; set; }
        public bool? CodeC { get; set; }
        public bool? CodeD { get; set; }
        public bool? AddCustomersDiscount { get; set; }
        public string ItemName { get; set; }
        public string Retail { get; set; }
        public int? ItemId { get; set; }
    }
}
