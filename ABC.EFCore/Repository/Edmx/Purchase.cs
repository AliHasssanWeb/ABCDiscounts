using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Purchase
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public string Sku { get; set; }
        public int? QuantityId { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string QuantityPack { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string UnitPrice { get; set; }
        public string TotalAmount { get; set; }
        public string GrossAmount { get; set; }
        public string VatTax { get; set; }
        public string InvoiceNumber { get; set; }
        public bool? Cash { get; set; }
        public bool? Credit { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public int? WareHouseId { get; set; }
        public string WareHouseName { get; set; }
    }
}
