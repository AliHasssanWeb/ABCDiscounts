using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Employer
    {
        public int EmployerId { get; set; }
        public string FederalEmployeeId { get; set; }
        public string EmployerName { get; set; }
        public string EmployerEmail { get; set; }
        public string EmployerPhone { get; set; }
        public string Extention { get; set; }
        public string Fax { get; set; }
        public string PayrolAddress { get; set; }
        public string City { get; set; }
        public string EmployerZipCode { get; set; }
        public string EmployerState { get; set; }
        public string Slogan { get; set; }
        public string Branding { get; set; }
        public string Suite { get; set; }
        public string CompanyHeaderByPath { get; set; }
        public byte[] CompanyHeader { get; set; }
        public string PrintCopies { get; set; }
        public bool? PrintPreview { get; set; }
        public bool? OptionPrint { get; set; }
        public bool? OptionPreview { get; set; }
        public string Contact { get; set; }
        public string Zip { get; set; }
        public string FedTaxId { get; set; }
        public string StateTaxId { get; set; }
        public string DefaultSalesTax { get; set; }
        public string DefaultCreditCharge { get; set; }
        public string DebitCharge { get; set; }
        public string LogoByPath { get; set; }
        public byte[] LogoImage { get; set; }
        public bool? HeaderOnReport { get; set; }
        public string DefaultMap { get; set; }
        public string DatabaseYear { get; set; }
    }
}
