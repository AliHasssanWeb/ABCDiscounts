using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.Website.Models
{
    public class InvoiceSaleModel
    {
        public List<PosSale> SaleOrders { get; set; }
        public InvoiceTotal InvoiceTotal { get; set; }
    }
}
