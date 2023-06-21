using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.Website.Models
{
    public class InvoiceModel
    {
        public List<PurchaseOrder> PurchaseOrders { get; set; }
        public InvoiceTotal InvoiceTotal    { get; set; }
    }
   
}
