using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class CertificateExemptionInstruction
    {
        public int Ceiid { get; set; }
        public bool? MultistateSupplementForm { get; set; }
        public string PostalAbbreviation { get; set; }
        public bool? CertificateSinglePurchase { get; set; }
        public string InvoicePurchaseOrderNo { get; set; }
        public string PurchaserName { get; set; }
        public string BusinessAddress { get; set; }
        public string PurchaserCity { get; set; }
        public string PurchaserState { get; set; }
        public string PurchaserZipCode { get; set; }
        public string PurchaseTaxId { get; set; }
        public string StateIssue { get; set; }
        public string CountryIssue { get; set; }
        public string Fein { get; set; }
        public string DrivingLicenseNo { get; set; }
        public string SellerName { get; set; }
        public string SellerAdress { get; set; }
        public string SellerCity { get; set; }
        public string SellerState { get; set; }
        public string SellerZipCode { get; set; }
        public bool? TermsCondition { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CustomerId { get; set; }
        public string FeinCountry { get; set; }
        public byte[] Signature { get; set; }
        public string SignatureByPath { get; set; }
    }
}
