using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class PersonPin
    {
        public int PinId { get; set; }
        public int? UserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string EmployeeNumber { get; set; }
        public int? PinNumber { get; set; }
        public int? EmployeeId { get; set; }
    }
}
