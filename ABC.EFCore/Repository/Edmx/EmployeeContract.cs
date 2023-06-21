using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeContract
    {
        public int ContractId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public bool? OnContract { get; set; }
        public bool? Permanent { get; set; }
        public bool? DailyWages { get; set; }
        public string Salary { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime? ContractDate { get; set; }
        public bool? IsProbation { get; set; }
        public DateTime? ProbationSdate { get; set; }
        public DateTime? ProbationEdate { get; set; }
        public string ProbationSalary { get; set; }
        public string ContractName { get; set; }
        public byte[] ContractDocument { get; set; }
        public string ContractDocumentByPath { get; set; }
        public string DailyWagesChargesAmount { get; set; }
        public string Iban { get; set; }
        public string PaymentMethod { get; set; }
        public string WorkingTimeIn { get; set; }
        public string WorkingTimeOut { get; set; }
        public string AccountNo { get; set; }
        public string RoutingNo { get; set; }
        public bool? IsApprove { get; set; }
    }
}
