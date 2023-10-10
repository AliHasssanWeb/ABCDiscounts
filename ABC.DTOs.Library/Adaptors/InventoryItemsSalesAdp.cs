using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class InventoryItemsSalesAdp
    {
        public string InvoiceNumber { get; set; }
        public int ItemId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string Company { get; set; }
        public string Cost { get; set; }
        public string Sales { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
    }
}
