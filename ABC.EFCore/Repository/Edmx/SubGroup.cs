using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SubGroup
    {
        public int Id { get; set; }
        public int? GroupId { get; set; }
        public string SubGroupName { get; set; }
        public string ParentGroupName { get; set; }
    }
}
