using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class SupervisorCredit
    {
        public int CreditId { get; set; }
        public int? SupervisorId { get; set; }
        public int? RoleId { get; set; }
        public string RoleName { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? CreditDate { get; set; }
        public string CreditAmount { get; set; }
        public bool? PaymentStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string SaleId { get; set; }
        public string Comment { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
    }
}
