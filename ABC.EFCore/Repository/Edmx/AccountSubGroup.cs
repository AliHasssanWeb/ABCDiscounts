using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AccountSubGroup
    {
        public AccountSubGroup()
        {
            Accounts = new HashSet<Account>();
        }

        public string AccountSubGroupId { get; set; }
        public string Title { get; set; }
        public string AccountGroupId { get; set; }

        public virtual AccountGroup AccountGroup { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
