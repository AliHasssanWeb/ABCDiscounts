using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Entities.HR
{
    public partial class EmployeeReverificationAndRehire
    {
        public int EmpReverificationId { get; set; }
        public string NewName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string DocumentTitle { get; set; }
        public string DocumentNumber { get; set; }
        public string Signature { get; set; }
        public string EmpName { get; set; }
        public DateTime? DateofRehire { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? EmployeeId { get; set; }
    }
}
