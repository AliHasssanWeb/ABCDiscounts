using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Terminal
    {
        public int TerminalId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string SecurityPin { get; set; }
    }
}
