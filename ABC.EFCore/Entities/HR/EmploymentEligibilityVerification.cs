using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Entities.HR
{ 
    public partial class EmploymentEligibilityVerification
    {
        public int? Eevid { get; set; }
        public int? EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string OtherNames { get; set; }
        public string Address { get; set; }
        public string AptNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime? Dob { get; set; }
        public string Ssn { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool? NoCitizen { get; set; }
        public bool? Lawful { get; set; }
        public string LawfulNumber { get; set; }
        public bool? AllienAuthorized { get; set; }
        public string AllienAuthorizedNumber { get; set; }
        public string AllienRegNumber { get; set; }
        public string AdmissionNumber { get; set; }
        public string ForeignPassportNumber { get; set; }
        public string CountryIssuance { get; set; }
        public string EmployeeSignature { get; set; }
        public DateTime? Date { get; set; }
        public string SignatureOfTransferer { get; set; }
        public DateTime? PrepareDate { get; set; }
        public string PrepareLastName { get; set; }
        public string PrepareFirstName { get; set; }
        public string PrepareAddress { get; set; }
        public string PrepareCity { get; set; }
        public string PrepareState { get; set; }
        public string PrepareZipCode { get; set; }
        public bool? Citizen { get; set; }
        public DateTime? Expirationdate { get; set; }
    }
}
