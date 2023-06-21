using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeSettlement
    {
        public int SettlementId { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string PendingSalary { get; set; }
        public string PendingLeave { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string TerminationNode { get; set; }
        public bool? AssetsReturn { get; set; }
        public bool? ConfidentialClear { get; set; }
        public bool? IsLoan { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
