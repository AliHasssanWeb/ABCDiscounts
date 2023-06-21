using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class EmployeeDocument
    {
        public int DocId { get; set; }
        public int? DocTypeId { get; set; }
        public string DocumentTypeName { get; set; }
        public byte[] Document { get; set; }
        public string DocumentByPath { get; set; }
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeNumber { get; set; }
    }
}
