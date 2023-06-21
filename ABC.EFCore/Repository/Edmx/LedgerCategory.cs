using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class LedgerCategory
    {
        public int Ledger { get; set; }
        public string Category { get; set; }
        public bool? Wages { get; set; }
        public bool? Payable { get; set; }
        public bool? Recievable { get; set; }
        public bool? Adjustment { get; set; }
        public bool? ProfitLoss { get; set; }
    }
}
