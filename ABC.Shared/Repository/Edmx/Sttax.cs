using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Shared.Repository.Edmx
{
    public partial class Sttax
    {
        public int TaxId { get; set; }
        public int? ProductId { get; set; }
        public string CreatedDate { get; set; }
        public int? CompanyId { get; set; }
        public string Tax { get; set; }
        public string PerQty { get; set; }
        public string PerUcount { get; set; }
        public string PerOz { get; set; }
        public string CreatedBy { get; set; }
        public string ProductName { get; set; }
        public bool? SttaxOnCost { get; set; }
        public bool? SttaxOnSale { get; set; }
    }
}
