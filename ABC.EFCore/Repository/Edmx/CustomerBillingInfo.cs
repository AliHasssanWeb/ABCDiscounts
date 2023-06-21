using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class CustomerBillingInfo
    {
        public int Id { get; set; }
        public int? CustomerInformationId { get; set; }
        public int? CustomerClassificationId { get; set; }
        public string CustomerCode { get; set; }
        public bool? IsTaxExempt { get; set; }
        public int? PricingId { get; set; }
        public string Pricing { get; set; }
        public bool? IsGetSalesDiscounts { get; set; }
        public bool? IsOutOfStateCustomer { get; set; }
        public string RetailPlusPercentage { get; set; }
        public string RetailPlus { get; set; }
        public string AdditionalInvoiceCharge { get; set; }
        public string AdditionalInvoiceDiscount { get; set; }
        public DateTime? ScheduleMessageFromDate { get; set; }
        public DateTime? ScheduleMessageToDate { get; set; }
        public string ScheduleMessage { get; set; }
        public int? PaymentTermsId { get; set; }
        public string PaymentTerms { get; set; }
        public string CreditLimit { get; set; }
        public bool? IsCreditHold { get; set; }
        public bool? IsBillToBill { get; set; }
        public bool? IsNoCheckAccepted { get; set; }
        public bool? IsExclude { get; set; }
        public string ThirdPartyCheckCharge { get; set; }
        public bool? IsCashBackBalance { get; set; }
        public string CashBackBalance { get; set; }
        public bool? IsPopupMessage { get; set; }
        public string PopupMessage { get; set; }
    }
}
