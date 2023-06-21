using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class JsonPaymentRelationJsonDetail
    {
        
       
    }
    [MetadataType(typeof(JsonPaymentRelationJsonDetail))]
    public partial class JsonPayment
    {
        [NotMapped]
        public JsonPaymentDetail JsonpaymentDetail { get; set; }
    }
}
