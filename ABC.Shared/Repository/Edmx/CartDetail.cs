using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class CartDetail
    {
        public int CartId { get; set; }
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string UnitCharge { get; set; }
        public string Retail { get; set; }
        public string Total { get; set; }
        public int? Quantity { get; set; }
        public int? Count { get; set; }
        public bool? PendingForApproval { get; set; }
        public bool? IsDelivered { get; set; }
        public string TicketId { get; set; }
        public string Tax { get; set; }
        public string TotalTaxes { get; set; }
        public DateTime? OrderDate { get; set; }
        public string DeliveredDate { get; set; }
        public string DeliveredBy { get; set; }
        public string PaymentMode { get; set; }
        public string CardTax { get; set; }
        public string Comment { get; set; }
    }
}
