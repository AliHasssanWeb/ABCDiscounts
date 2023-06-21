using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class JsonSale
    {
        public string SaleId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string BarCode { get; set; }
        public string Sku { get; set; }
        public string UnitPrice { get; set; }
        public string InvoiceNumber { get; set; }
        public string CreatedDate { get; set; }
        public string OnCash { get; set; }
        public string OnCredit { get; set; }
        public string DiscountPercentage { get; set; }
        public string DiscountCash { get; set; }
        public string TaxPerchange { get; set; }
        public string TaxCash { get; set; }
        public string Cost { get; set; }
        public string Damage { get; set; }
        public string Return { get; set; }
        public string Sales { get; set; }
        public string Quantity { get; set; }
        public string ItemId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string QuantityUnit { get; set; }
        public string QuantityPack { get; set; }
        public string QuantityId { get; set; }
        public string CashierName { get; set; }
        public string TotalAmount { get; set; }
        public string GrossAmount { get; set; }
        public string TerminalNumber { get; set; }

    }
}
