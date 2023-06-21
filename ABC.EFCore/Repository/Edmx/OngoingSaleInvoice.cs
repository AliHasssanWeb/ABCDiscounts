using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class OngoingSaleInvoice
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public bool? GetSaleDiscount { get; set; }
        public string InDiscount { get; set; }
        public string OutDiscount { get; set; }
        public int? SalesmanId { get; set; }
        public string SaleManName { get; set; }
        public string PaymentTerms { get; set; }
        public int? ShippedViaId { get; set; }
        public string ShippedName { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public string Weight { get; set; }
        public string Count { get; set; }
        public string CurrentInvoiceNumber { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string PreviousBalance { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Other { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }
        public string InvoiceTotal { get; set; }
        public string AmountDue { get; set; }
        public int? ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string Quantity { get; set; }
        public string AmountRetail { get; set; }
        public string ItemInDiscount { get; set; }
        public string ItemOutDiscount { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string PaidAmount { get; set; }
        public string IsPostStatus { get; set; }
        public string InUnits { get; set; }
        public string OutUnits { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public bool? OnCash { get; set; }
        public bool? OnCredit { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public int? WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public int? SupervisorId { get; set; }
        public string SupervisorCreditAllow { get; set; }
        public int? SalesManagerId { get; set; }
        public string SalesManagerCreditAllow { get; set; }
        public string ItemName { get; set; }
        public string CashierName { get; set; }
    }
}
