using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class SecurityKey
    {
        public int SecurityKeyId { get; set; }
        public string KeyPin { get; set; }
        public int? UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public DateTime? CreadtedDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Type { get; set; }
        public bool? Active { get; set; }
    }
}
