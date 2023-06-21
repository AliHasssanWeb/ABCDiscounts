using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class JsonPurchase
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string ProductBarCode { get; set; }
        public string Sku { get; set; }
        public string QuantityId { get; set; }
        public string Quantity { get; set; }
        public string QuantityUnit { get; set; }
        public string QuantityPack { get; set; }
        public string PurchaseDate { get; set; }
        public string UnitPrice { get; set; }
        public string TotalAmount { get; set; }
        public string GrossAmount { get; set; }
        public string VatTax { get; set; }
        public string InvoiceNumber { get; set; }
        public string Cash { get; set; }
        public string Credit { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string WareHouseId { get; set; }
        public string WareHouseName { get; set; }
    }
}
