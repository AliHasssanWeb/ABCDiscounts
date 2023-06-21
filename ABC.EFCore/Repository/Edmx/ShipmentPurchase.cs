using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ShipmentPurchase
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime? CreateDate { get; set; }
        public string ShipNumber { get; set; }
        public string Reference { get; set; }
    }
}
