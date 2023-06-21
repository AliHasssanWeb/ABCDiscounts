using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeWithHoldingTax
    {
        public int Eacid { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Ssn { get; set; }
        public string Address { get; set; }
        public string MarriedStatus { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string NameDiffSsn { get; set; }
        public DateTime? Date { get; set; }
        public string EmployeeSignature { get; set; }
        public string Country { get; set; }
        public string NoofAllowances { get; set; }
        public string AdditionalAmount { get; set; }
        public bool? NoTaxLaibility { get; set; }
        public bool? MilatrySpouseExempt { get; set; }
        public bool? Domiciled { get; set; }
        public bool? NoExemptions { get; set; }
        public bool? EmployerWithHold { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeOfficeCode { get; set; }
        public string Ein { get; set; }
        public string FullName { get; set; }
    }
}
