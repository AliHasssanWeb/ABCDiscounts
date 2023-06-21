using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SuppliersPayment
    {
        public int SupplierPaymentId { get; set; }
        public string Ponumber { get; set; }
        public int? SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string Pobalance { get; set; }
        public string TotalPaid { get; set; }
        public string OtherPaymentAmount { get; set; }
        public bool? IsOtherPayment { get; set; }
        public bool? IsCreditCustomer { get; set; }
        public string CreditCustomerAmount { get; set; }
        public string CheckPaidAmount { get; set; }
        public bool? IsCheckPaid { get; set; }
        public DateTime? PaidDate { get; set; }
        public string AddedBy { get; set; }
        public string Comments { get; set; }
        public string RemaningPayment { get; set; }
    }
}
