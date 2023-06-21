using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Entities.HR
{
    public partial class EmployeeDdauthorization
    {
        public int EmpDdaid { get; set; }
        public bool? Authorize { get; set; }
        public bool? Revise { get; set; }
        public bool? Cancel { get; set; }
        public string Signature { get; set; }
        public string BankNameOne { get; set; }
        public string BankNameTwo { get; set; }
        public string BankNameThree { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string AddressThree { get; set; }
        public string PhoneOne { get; set; }
        public string PhoneTwo { get; set; }
        public string PhoneThree { get; set; }
        public string AccTypeOne { get; set; }
        public string AccTypeTwo { get; set; }
        public string AccTypeThree { get; set; }
        public string BankRoutingOne { get; set; }
        public string BankRoutingTwo { get; set; }
        public string BankRoutingThree { get; set; }
        public string BankAccNumberOne { get; set; }
        public string BankAccNumberTwo { get; set; }
        public string BankAccNumberThree { get; set; }
        public string BankAmountOne { get; set; }
        public string BankAmountTwo { get; set; }
        public string BankAmountThree { get; set; }
        public string PctOne { get; set; }
        public string PctTwo { get; set; }
        public string PctThree { get; set; }
        public string Total { get; set; }
        public string BankName { get; set; }
        public string Bank { get; set; }
        public string PayTo { get; set; }
        public string Memo { get; set; }
        public DateTime? Date { get; set; }
        public int? EmployeeId { get; set; }
    }
}
