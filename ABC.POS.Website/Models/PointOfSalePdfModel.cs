using iTextSharp.text;
using System.Collections.Generic;

namespace ABC.POS.Website.Models
{
    public class PointOfSalePdfModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BusinessAddress { get; set; }
        public string Count { get; set; }
        public string InvoiceNumber { get; set; }
        public string SubTotal { get; set; }
        public string Other { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }
        public string InvoiceTotal { get; set; }
        public string Charges { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public List<PointOfSaleDetailPDFModel> PointOfSaleDetails { get; set; }
    }
}
