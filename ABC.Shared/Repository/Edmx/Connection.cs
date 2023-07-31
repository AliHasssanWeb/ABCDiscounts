using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Connection
    {
        public int SecurityId { get; set; }
        public string Message { get; set; }
        public bool? Status { get; set; }
        public Guid? AuthToken { get; set; }
    }
}
