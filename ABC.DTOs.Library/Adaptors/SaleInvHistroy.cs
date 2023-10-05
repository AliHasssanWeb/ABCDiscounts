using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class SaleInvHistroy
    {
        public string InvoiceNumber { get; set; }
        public DateTime? HoldDate  { get; set; }
        public string AmountAllocate { get; set; }
        public string TotalAmount { get; set; }
        public string UserName { get; set; }
    }
}
