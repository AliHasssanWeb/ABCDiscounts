using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.Website.Models
{
    public class InvoiceTotal
    {
        public string InvoiceNumber { get; set; }
        public string SupplierNumber { get; set; }
        public string BusinessAddress { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string TotalItems { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string Total { get; set; }
        public string Other { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }
        public string GrossTotal { get; set; }
        public string ItemPrice { get; set; }
        public string Charges { get; set; }
    }
}
