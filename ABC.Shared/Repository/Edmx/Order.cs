using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public string ProductVariation { get; set; }
        public string DeliveryAddress { get; set; }
        public string MailingAddress { get; set; }
        public string PaymentType { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string RetailPrice { get; set; }
        public int? UserId { get; set; }
        public string InvoiceNumber { get; set; }
        public bool? IsDelivered { get; set; }
        public string ImageByPath { get; set; }
        public string TotalUnits { get; set; }
        public string GrossAmount { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public bool? CloseStatus { get; set; }
        public string Cashier { get; set; }
        public string TerminalNumber { get; set; }
        public string UnitCharge { get; set; }
    }
}
