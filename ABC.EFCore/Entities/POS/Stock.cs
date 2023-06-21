using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.POS
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public int ItemId { get; set; }
        [Required(ErrorMessage = "Please Enter Item Name")]
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "Please Enter Quntity")]
        public string ItemBarCode { get; set; }
        public string Quantity { get; set; }
        public string Sku { get; set; }
        public string MSAReport { get; set; }
    }
}
