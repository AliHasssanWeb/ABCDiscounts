using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class CustomerInformation
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public bool? CheckAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Cell { get; set; }
        public int? ProviderId { get; set; }
        public string Provider { get; set; }
        public string Other { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string TaxIdfein { get; set; }
        public string StateIdnumber { get; set; }
        public string TobaccoLicenseNumber { get; set; }
        public string Vendor { get; set; }
        public string CigaretteLicenseNumber { get; set; }
        public string Dea { get; set; }
        public string Memo { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerType { get; set; }
        public DateTime? Dob { get; set; }
        public string Ssn { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public int? DrivingLicenseStateId { get; set; }
        public string DrivingLicenseState { get; set; }
        public string OwnerAddress { get; set; }
        public string BusinessAddress { get; set; }
        public string VehicleNumber { get; set; }
        public bool? Authorized { get; set; }
        public string CustomerCode { get; set; }
        public string Balance { get; set; }
        public string Gender { get; set; }
        public int? StateId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountId { get; set; }
        public string AccountTitle { get; set; }
        public bool? AdminApproved { get; set; }
        public int? UserId { get; set; }
        public string FromScreen { get; set; }
        public string FullName { get; set; }
        public string BusinessName { get; set; }
        public string Mobile { get; set; }
        public string CustomerState { get; set; }
        public string StateResaleTaxId { get; set; }
        public string DrivingLicense { get; set; }
        public string CigratteLicenceNumber { get; set; }
        public string PostalCode { get; set; }
        public string RegistrationType { get; set; }
        public string Address { get; set; }
        public string MailingAddress { get; set; }
        public bool? TaxExempt { get; set; }
        public bool? Approved { get; set; }
        public bool? Rejected { get; set; }
        public bool? Pending { get; set; }
        public string Suite { get; set; }
    }
}
