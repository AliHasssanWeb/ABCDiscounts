using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    
    public class OrderValidate
    {
        [NotMapped]
        public string  CustomerNAME { get; set; }
    }

    [MetadataType(typeof(OrderValidate))]
    public partial class Order
    {
        [NotMapped]
        public string CustomerNAME { get; set; }

    }
}
