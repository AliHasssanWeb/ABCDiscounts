using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AccountGroup
    {
        public AccountGroup()
        {
            AccountSubGroups = new HashSet<AccountSubGroup>();
        }

        public string AccountGroupId { get; set; }
        public string Title { get; set; }
        public string AccountClassId { get; set; }

        public virtual AccountClass AccountClass { get; set; }
        public virtual ICollection<AccountSubGroup> AccountSubGroups { get; set; }
    }
}
