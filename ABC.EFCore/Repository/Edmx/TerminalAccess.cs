using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class TerminalAccess
    {
        public int TerminalAccessId { get; set; }
        public DateTime? AccessDate { get; set; }
        public string AccessByUser { get; set; }
        public int? AccessUserId { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalNumber { get; set; }
        public string CloseTime { get; set; }
        public string StartTime { get; set; }
    }
}
