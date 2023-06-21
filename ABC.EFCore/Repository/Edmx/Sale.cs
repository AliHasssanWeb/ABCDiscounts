using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Sale
    {
        public int SaleId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string BarCode { get; set; }
        public string Sku { get; set; }
        public string UnitPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public string CreatedDate { get; set; }
        public bool? OnCash { get; set; }
        public bool? OnCredit { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountCash { get; set; }
        public string TaxPerchange { get; set; }
        public string TaxCash { get; set; }
        public string Cost { get; set; }
        public string Damage { get; set; }
        public bool? Return { get; set; }
        public string Sales { get; set; }
        public string Quantity { get; set; }
        public int? ItemId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string QuantityUnit { get; set; }
        public string QuantityPack { get; set; }
        public int? QuantityId { get; set; }
        public string CashierName { get; set; }
        public string TotalAmount { get; set; }
        public string GrossAmount { get; set; }
        public string TerminalNumber { get; set; }
        public DateTime? SaleDate { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public int? WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public int? SupervisorId { get; set; }
        public string SupervisorCreditAllow { get; set; }
        public int? SalesManagerId { get; set; }
        public string SalesManagerCreditAllow { get; set; }
    }
}
