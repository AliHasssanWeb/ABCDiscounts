using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Receivable
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public string AccountNumber { get; set; }
        public string Amount { get; set; }
        public string AccountName { get; set; }
    }
}
