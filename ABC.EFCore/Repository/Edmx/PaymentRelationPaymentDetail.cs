using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public  class PaymentRelationPaymentDetail
    {
    
    }
    [MetadataType(typeof(PaymentRelationPaymentDetail))]
    public partial class Payment
    {
        [NotMapped]
        public PaymentDetail paymentDetail { get; set; }
    }
}
