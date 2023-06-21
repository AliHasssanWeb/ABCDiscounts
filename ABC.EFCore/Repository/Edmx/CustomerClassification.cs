using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class CustomerClassification
    {
        public int Id { get; set; }
        public int? CustomerInfoId { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? SubGroupId { get; set; }
        public string SubGroupName { get; set; }
        public int? ZoneId { get; set; }
        public string Zone { get; set; }
        public int? BusinessTypeId { get; set; }
        public string BusinessType { get; set; }
        public int? SalesmanId { get; set; }
        public string Salesman { get; set; }
        public int? ShippedViaId { get; set; }
        public string ShippedVia { get; set; }
        public int? RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDeliveryDay { get; set; }
        public int? DriverId { get; set; }
        public string DriverName { get; set; }
        public string OtherCustomerReference { get; set; }
        public int? ShiptoReferenceId { get; set; }
        public string ShiptoReference { get; set; }
        public bool? OutOfStateCustomer { get; set; }
        public bool? AddtoMaillingList { get; set; }
        public bool? AddtoemailTextList { get; set; }
        public bool? RejectPromotion { get; set; }
        public bool? ViewInvoicePrevBalance { get; set; }
        public bool? ViewRetailandDiscount { get; set; }
        public int? BarCodeId { get; set; }
        public string BarCode { get; set; }
        public string DefaultInvoiceCopies { get; set; }
        public bool? SpecialInvoiceCustom { get; set; }
        public string InvoiceMemo { get; set; }
        public bool? UseDefaultInvMemo { get; set; }
        public string CustomerCode { get; set; }
    }
}
