using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CartDetailValidate
    {
        [NotMapped]
        public string ImageURL { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string Balance { get; set; }
        [NotMapped]
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        [NotMapped]
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public virtual AutherizeOrderLimit AutherizeOrderLimits { get; set; }

        [NotMapped]
        public string OrderTotalBalance { get; set; }
        [NotMapped]
        public string ProductSku { get; set; }

        [NotMapped]
        public int? ChangeQuantity { get; set; }
        [NotMapped]
        public string ShipmentLimit { get; set; }
        [NotMapped]
        public bool? NeedHighAuthorization { get; set; }
        [NotMapped]
        public string HighlimitOn { get; set; }

        [NotMapped]
        public string InvTotal { get; set; }
    }
    [MetadataType(typeof(CartDetailValidate))]
    public partial class CartDetail
    {

        [NotMapped]
        public Product ProductObj { get; set; }
        [NotMapped]
        public string ImageURL { get; set; }
        [NotMapped]
        public string Type { get; set; } 
        
        [NotMapped]
        public string Balance { get; set; }
        [NotMapped]
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        [NotMapped]
        public virtual ICollection<Product> Products { get; set; }
        [NotMapped]
        public virtual AutherizeOrderLimit AutherizeOrderLimits { get; set; }

        [NotMapped]
        public string OrderTotalBalance { get; set; } 
        
        [NotMapped]
        public string ProductSku { get; set; }

        [NotMapped]
        public int? ChangeQuantity { get; set; }
        [NotMapped]
        public string ShipmentLimit { get; set; }

        [NotMapped]
        public bool? NeedHighAuthorization { get; set; }
        [NotMapped]
        public string HighlimitOn { get; set; }

        [NotMapped]
        public string InvTotal { get; set; }

    }
}
