using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class JsonPosSale
    {
        public string PossaleId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string GetSaleDiscount { get; set; }
        public string InDiscount { get; set; }
        public string OutDiscount { get; set; }
        public string SalesmanId { get; set; }
        public string SaleManName { get; set; }
        public string PaymentTerms { get; set; }
        public string ShippedViaId { get; set; }
        public string ShippedName { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string Weight { get; set; }
        public string Count { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string PreviousBalance { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Other { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }
        public string InvoiceTotal { get; set; }
        public string AmountDue { get; set; }
        public string ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ItemDescription { get; set; }
        public string Quantity { get; set; }
        public string AmountRetail { get; set; }
        public string ItemInDiscount { get; set; }
        public string ItemOutDiscount { get; set; }
        public string CustomerId { get; set; }
        public string InvoiceDate { get; set; }
        public string IsPaid { get; set; }
        public string PaidDate { get; set; }
        public string PaidAmount { get; set; }
        public string IsPostStatus { get; set; }
        public string InUnits { get; set; }
        public string OutUnits { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string OnCash { get; set; }
        public string OnCredit { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorCreditAllow { get; set; }
        public string SalesManagerId { get; set; }
        public string SalesManagerCreditAllow { get; set; }
        public string ItemName { get; set; }
        public string CashierName { get; set; }
        public string Cost { get; set; }
        public string ReturnQty { get; set; }
        public string DamageQty { get; set; }
        public bool IsPrintinvoice { get; set; }
        public string BusinessAddress { get; set; }
        public string CustomerEmail { get; set; }
        public string RingerQuantity { get; set; }
        public string ShipmentLimit { get; set; }
        public bool AddToMailList { get; set; }
        public bool? IsClose { get; set; }
        public bool? IsOpen { get; set; }
        public string drivingLicense { get; set; }
        public string plateNo { get; set; }
        public string FromScreen { get; set; }
        public string Charges { get; set; }
    }
}
