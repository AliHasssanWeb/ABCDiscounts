using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    class StockWithItemDetails
    {
    }
    [MetadataType(typeof(StockWithItemDetails))]
    public partial class InventoryStock
    {
        [NotMapped]
        public Product items { get; set; }
    }

}

