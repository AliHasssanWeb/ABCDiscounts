using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ActivityLog
    {
        public int Id { get; set; }
        public string OperationName { get; set; }
        public string PageName { get; set; }
        public string LogTime { get; set; }
        public int? UserId { get; set; }
        public string TableName { get; set; }
        public string NewDetails { get; set; }
        public string OldDetails { get; set; }
        public bool? Deleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Extraone { get; set; }
        public string Extratwo { get; set; }
    }
}
