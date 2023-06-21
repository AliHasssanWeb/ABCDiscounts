using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeAuthorizedRepresentative
    {
        public int EmpAuthRepId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DocumentTitle { get; set; }
        public string IssuingAuthority { get; set; }
        public string DocumentNumber { get; set; }
        public string Signature { get; set; }
        public string EmpTitle { get; set; }
        public string OrgName { get; set; }
        public string OrgAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? Date { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? FirstDate { get; set; }
        public string EmpAuthDocumentTitle { get; set; }
        public string EmpAuthIssuingAuthority { get; set; }
        public string EmpAuthDocumentNumber { get; set; }
        public DateTime? EmpAuthExpirationDate { get; set; }
    }
}
