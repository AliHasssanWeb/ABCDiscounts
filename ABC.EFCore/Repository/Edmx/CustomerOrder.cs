using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class CustomerOrder
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public string TicketId { get; set; }
        public string CustomerName { get; set; }
        public string BillingAddress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string Country { get; set; }
        public bool? AdminStatus { get; set; }
        public bool? Delivered { get; set; }
        public string OrderAmount { get; set; }
        public string TaxAmount { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool? IsPulled { get; set; }
        public bool? IsRejected { get; set; }
        public string AdminActionBy { get; set; }
        public DateTime? AdminActionDate { get; set; }
        public string TerminalNumber { get; set; }
        public string AdminActionTime { get; set; }
        public string PulledBy { get; set; }
        public string DeliveredBy { get; set; }
        public DateTime? PulledDate { get; set; }
        public string PulledTime { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string DeliveredTime { get; set; }
        public string RejectReason { get; set; }
        public string PaymentMode { get; set; }
        public string CardTax { get; set; }
        public bool? IsInvoiced { get; set; }
        public string InvoicedBy { get; set; }
        public DateTime? InvoicedDate { get; set; }
        public string Quantity { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public string InvoicedTime { get; set; }
        public int? PullerEmployeeId { get; set; }
        public string CustomerCode { get; set; }
        public int? InvoiceEmployeeId { get; set; }
        public string DueDate { get; set; }
        public bool? IsPaid { get; set; }
        public string RejectComments { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public bool? NeedApproval { get; set; }
        public bool? AcceptApproval { get; set; }
        public int? ChangeQuantity { get; set; }
        public string Comment { get; set; }
    }
}
