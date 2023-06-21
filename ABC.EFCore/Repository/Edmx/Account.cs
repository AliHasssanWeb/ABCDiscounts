using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Account
    {
        public string AccountId { get; set; }
        public string Title { get; set; }
        public string AccountSubGroupId { get; set; }
        public int Status { get; set; }

        public virtual AccountSubGroup AccountSubGroup { get; set; }
    }
}
