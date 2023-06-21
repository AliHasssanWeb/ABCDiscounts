using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class JsonPurchaseOrder
    {
        public string PurchaseorderId { get; set; }
        public string Supplieritems { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public string ShowAllItems { get; set; }
        public string TotalItems { get; set; }
        public string UpdateCost { get; set; }
        public string UpdateOscost { get; set; }
        public string Received { get; set; }
        public string ReceivedDate { get; set; }
        public string Currrentuser { get; set; }
        public string ManualInvoiceNumber { get; set; }
        public string Podate { get; set; }
        public string PaymentTerms { get; set; }
        public string TotalTobacco { get; set; }
        public string TotalCigar { get; set; }
        public string TotalCigarette { get; set; }
        public string CigaretteStick { get; set; }
        public string DateReceived { get; set; }
        public string Notes { get; set; }
        public string IsReport { get; set; }
        public string IsPaid { get; set; }
        public string PaidDate { get; set; }
        public string IsPostStatus { get; set; }
        public string PaidAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public string SubTotal { get; set; }
        public string Freight { get; set; }
        public string IsTax { get; set; }
        public string Tax { get; set; }
        public string Other { get; set; }
        public string Total { get; set; }
        public string SupplierItemNumber { get; set; }
        public string StockItemNumber { get; set; }
        public string Description { get; set; }
        public string Retail { get; set; }
        public string IsPrice { get; set; }
        public string Price { get; set; }
        public string IsQty { get; set; }
        public string Qty { get; set; }
        public string IsCaseQty { get; set; }
        public string CaseQty { get; set; }
        public string IsDiscount { get; set; }
        public string Discount { get; set; }
        public string Amount { get; set; }
        public string InvoiceDate { get; set; }
        public string GrossAmount { get; set; }
        public string ItemId { get; set; }
        public bool IsPrintinvoice {get;set;}
        public string ItemNumber {get;set;}
        public string SupplierInvoiceNumber { get; set; }
        public string SimpleInvoiceNumber { get; set; }

        public DateTime? CheckDate { get; set; }

        public string CheckNumber { get; set; }

        public string CheckTitle { get; set; }
    }
}
