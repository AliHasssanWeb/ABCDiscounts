using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class ReceivingAdp
    {
        public string InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public string Days { get; set; }
        public string InvTotal { get; set; }
        public string TotalPaid { get; set; }
        public string InvBalance { get; set; }
    }
}
