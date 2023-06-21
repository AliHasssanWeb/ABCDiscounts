using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AccountClass
    {
        public AccountClass()
        {
            AccountGroups = new HashSet<AccountGroup>();
        }

        public string AccountClassId { get; set; }
        public string Title { get; set; }

        public virtual ICollection<AccountGroup> AccountGroups { get; set; }
    }
}
