using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string DrivingLisence { get; set; }
        public string State { get; set; }
        public string EmployeeCode { get; set; }
        public bool? AdminApproval { get; set; }
        public bool? AccessAccount { get; set; }
        public string MartialStatus { get; set; }
        public string SpouseName { get; set; }
        public string NoofChildren { get; set; }
        public byte[] ProfileImage { get; set; }
        public string ProfileImagePath { get; set; }
        public bool? AdminStatus { get; set; }
        public bool? IsActive { get; set; }
        public string EmployeeCity { get; set; }
        public string EmployeeZipCode { get; set; }
        public string FederalEmployeeId { get; set; }
        public string PayrolAddress { get; set; }
        public string Extention { get; set; }
        public string Fax { get; set; }
        public string Ssn { get; set; }
        public bool? YesContractor { get; set; }
        public string EmployerZipCode { get; set; }
        public DateTime? DateofHire { get; set; }
        public DateTime? Dob { get; set; }
        public string EmployerName { get; set; }
        public string EmployerState { get; set; }
        public string EmployerEmail { get; set; }
        public string City { get; set; }
        public string EmployerPhone { get; set; }
        public bool? NoContractor { get; set; }
        public string EmployeeOfficeCode { get; set; }
        public string Ein { get; set; }
        public DateTime? Expirationdate { get; set; }
        public int? RoleId { get; set; }
    }
}
