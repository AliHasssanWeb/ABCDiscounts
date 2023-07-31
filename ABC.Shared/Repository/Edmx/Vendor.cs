using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Vendor
    {
        public int VendorId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string StateId { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ImageByPath { get; set; }
        public string VendorCode { get; set; }
        public string DrivingLicense { get; set; }
        public string Discount { get; set; }
        public string Irs { get; set; }
        public bool? TaxExempt { get; set; }
        public string FullName { get; set; }
        public string StateName { get; set; }
        public string DrivingLicenseState { get; set; }
        public string AccountNumber { get; set; }
        public string AccountId { get; set; }
        public string AccountTitle { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Suite { get; set; }
        public string Country { get; set; }
        public string Fax { get; set; }
        public string PayTerms { get; set; }
        public string CreditLimit { get; set; }
        public string FedTaxId { get; set; }
        public string StateTaxId { get; set; }
        public string Website { get; set; }
        public string Ledger { get; set; }
        public string LedgerCode { get; set; }
        public string CheckMemo { get; set; }
        public string Comments { get; set; }
        public bool? SupplierChecked { get; set; }
        public string VenderType { get; set; }
        public bool? OutOfStateSupplier { get; set; }
        public bool? LocalTextPaidBySupplier { get; set; }
        public bool? ReportTaxesToStateNc { get; set; }
        public string StateDiscount { get; set; }
        public bool? PrintYtdonChecks { get; set; }
        public string AccountByVendor { get; set; }
    }
}
