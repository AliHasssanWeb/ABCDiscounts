using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Customer.Domain.Entities
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ImageByPath { get; set; }
        public string CustomerCode { get; set; }
        public string DrivingLicense { get; set; }
        public string Discount { get; set; }
        public bool? TaxExempt { get; set; }
        public string BusinessName { get; set; }
        public string CustomerType { get; set; }
        public string TobaccoLicenseNumber { get; set; }
        public string CigratteLicenceNumber { get; set; }
        public string DrivingLicenseState { get; set; }
        public string City { get; set; }
        public string RegistrationType { get; set; }
        public string MailingAddress { get; set; }
        public string BusinessAddress { get; set; }
        public bool? TermsCondition { get; set; }
        public string PostalCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountId { get; set; }
        public string AccountTitle { get; set; }
        public bool? AdminApproval { get; set; }
        public string CustomerState { get; set; }
        public string StateResaleTaxId { get; set; }
        public string FromScreen { get; set; }
        public string PasswordHash { get; set; }
    }
}
