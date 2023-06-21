using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public class StockEvaluation
    {

        public int StockId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string ItemBarCode { get; set; }
        public Double Quantity { get; set; }
        public string Sku { get; set; }
        public int? ProductId { get; set; }
        public string UnitCharge { get; set; }
        public Double TotalAmount { get; set; }
        public Double GrossAmount { get; set; }
        public Double TotalQuantityInHand { get; set; }


    }
}
