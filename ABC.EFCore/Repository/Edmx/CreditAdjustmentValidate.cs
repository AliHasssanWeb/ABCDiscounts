using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CreditAdjustmentValidate
    {
        [NotMapped]
        public string createdDateonly { get; set; }
        [NotMapped]
        public double amount { get; set; }
    }
    [MetadataType(typeof(CreditAdjustmentValidate))]
    public partial class CreditAdjustment
    {

        [NotMapped]
        public string createdDateonly { get; set; }
        [NotMapped]
        public double amount { get; set; }


    }
}
