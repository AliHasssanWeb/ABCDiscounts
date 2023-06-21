using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class ReportStockInventory
    {

        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemBarCode { get; set; }
        public string Quantity { get; set; }
        public string Sku { get; set; }
    }
}
