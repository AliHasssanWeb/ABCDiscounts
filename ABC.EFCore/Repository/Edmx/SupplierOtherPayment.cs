using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SupplierOtherPayment
    {
        public int OtherPaymentId { get; set; }
        public int? PaymentId { get; set; }
        public string Pobalance { get; set; }
        public string Amount { get; set; }
        public DateTime? Date { get; set; }
        public string Comments { get; set; }
        public string AddedBy { get; set; }
        public string Ponumber { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public bool? IsWire { get; set; }
        public bool? IsCashOut { get; set; }
        public bool? IsOther { get; set; }
    }
}
