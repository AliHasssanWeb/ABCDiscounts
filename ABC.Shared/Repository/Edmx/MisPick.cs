using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class MisPick
    {
        public int MisPickId { get; set; }
        public string MisPickName { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
    }
}
