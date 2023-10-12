using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public class CustomerDetailModel
    {
        // Customer Info Table 
        public int CustomerId { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Cell { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Provider { get; set; }
        public bool? CheckAddress { get; set; }
        public string Email { get; set; }
        public string CustomerCode { get; set; }
        public string AccountId { get; set; }
        public string BusinessAddress { get; set; }
        public string TaxIdfein { get; set; }
        public bool? TaxExempt { get; set; }

        // Customer Classification Table

        public string Salesman { get; set; }
        public bool? AddtoMaillingList { get; set; }
        public bool? ViewInvoicePrevBalance { get; set; }
        public string ShippedVia { get; set; }
        public string DriverName { get; set; }
        public string Amount { get; set; }

        // Customer Billing Info
        public bool? IsBillToBill { get; set; }
        public bool? IsCreditHold { get; set; }
        public string AdditionalInvoiceCharge { get; set; }
        public bool? IsExclude { get; set; }
        public bool? IsPopupMessage { get; set; }
        public string PopupMessage { get; set; }
        public bool? IsGetSalesDiscounts { get; set; }
        public string PaymentTerms { get; set; }

        public List<PointOfSaleModel> PointOfSale { get; set; }
    }
}
