using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PointOfSale
    {
        public PointOfSale()
        {
            PointOfSaleDetails = new HashSet<PointOfSaleDetail>();
        }

        public int PointOfSaleId { get; set; }
        public int CustomerId { get; set; }
        public bool? GetSaleDiscount { get; set; }
        public int? SalesManId { get; set; }
        public string PaymentTerms { get; set; }
        public int? ShippedViaId { get; set; }
        public int? DriverId { get; set; }
        public double? Weight { get; set; }
        public int? Count { get; set; }
        public string InvoiceNumber { get; set; }
        public string PreviousBalance { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Other { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }
        public string InvoiceTotal { get; set; }
        public string AmountDue { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool? IsPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public double? PaidAmount { get; set; }
        public string IsPostStatus { get; set; }
        public bool? IsOpen { get; set; }
        public bool? IsClose { get; set; }
        public bool? OnCash { get; set; }
        public bool? OnCredit { get; set; }
        public int? StoreId { get; set; }
        public int? WareHouseId { get; set; }
        public int? SupervisorId { get; set; }
        public string SupervisorCreditAllow { get; set; }
        public int? SaleManagerId { get; set; }
        public string SaleManagerCreditAllow { get; set; }
        public string CashierName { get; set; }
        public string Cost { get; set; }
        public string ReturnQty { get; set; }
        public string DemageQty { get; set; }
        public string ShipmentLimit { get; set; }
        public string OrderTime { get; set; }
        public string FromScreen { get; set; }
        public string Charges { get; set; }

        public virtual CustomerInformation Customer { get; set; }
        public virtual ICollection<PointOfSaleDetail> PointOfSaleDetails { get; set; }
    }
}
