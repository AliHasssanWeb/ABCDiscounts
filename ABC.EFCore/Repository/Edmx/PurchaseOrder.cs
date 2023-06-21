using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PurchaseOrder
    {
        public int PurchaseorderId { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public bool? ShowAllItems { get; set; }
        public bool? Supplieritems { get; set; }
        public string TotalItems { get; set; }
        public bool? UpdateCost { get; set; }
        public bool? UpdateOscost { get; set; }
        public bool? Received { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Currrentuser { get; set; }
        public string ManualInvoiceNumber { get; set; }
        public DateTime? Podate { get; set; }
        public string PaymentTerms { get; set; }
        public string TotalTobacco { get; set; }
        public string TotalCigar { get; set; }
        public string TotalCigarette { get; set; }
        public string CigaretteStick { get; set; }
        public DateTime? DateReceived { get; set; }
        public string Notes { get; set; }
        public bool? IsReport { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string IsPostStatus { get; set; }
        public string PaidAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string SubTotal { get; set; }
        public string Freight { get; set; }
        public bool? IsTax { get; set; }
        public string Tax { get; set; }
        public string Other { get; set; }
        public string Total { get; set; }
        public string SupplierItemNumber { get; set; }
        public string StockItemNumber { get; set; }
        public string Description { get; set; }
        public string Retail { get; set; }
        public bool? IsPrice { get; set; }
        public string Price { get; set; }
        public bool? IsQty { get; set; }
        public string Qty { get; set; }
        public bool? IsCaseQty { get; set; }
        public string CaseQty { get; set; }
        public bool? IsDiscount { get; set; }
        public string Discount { get; set; }
        public string Amount { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string GrossAmount { get; set; }
        public int? ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ProductBarCode { get; set; }
        public string Sku { get; set; }
        public string RemaningPayment { get; set; }
        public string SimpleInvoiceNumber { get; set; }
    }
}
