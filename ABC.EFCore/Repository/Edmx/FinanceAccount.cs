using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class FinanceAccount
    {
        public int AccountId { get; set; }
        public int? UserId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public string UserName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
