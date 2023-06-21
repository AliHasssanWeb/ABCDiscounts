using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class PurchaseOrderValid
    {
        [NotMapped]
        public string POrderDate { get; set; }
        [NotMapped]
        public string RecievedOrderDate { get; set; }
        [NotMapped]
        public string InvoicestringDate { get; set; }

        [NotMapped]
        public DateTime? CheckDate { get; set; }

        [NotMapped]
        public string CheckNumber { get; set; }

        [NotMapped]
        public string CheckTitle { get; set; }
    }
    [MetadataType(typeof(CustomerValid))]
    public partial class PurchaseOrder
    {
       
        [NotMapped]
        public string POrderDate { get; set; }
        [NotMapped]
        public string RecievedOrderDate { get; set; }
        [NotMapped]
        public string InvoicestringDate { get; set; }

        [NotMapped]
        public DateTime? CheckDate { get; set; }

        [NotMapped]
        public string CheckNumber { get; set; }

        [NotMapped]
        public string CheckTitle { get; set; }  
        
        [NotMapped]
        public string ItemPrice { get; set; }

    }
}
