using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CustomerOrderValidate
    {
        [NotMapped]
        public string OrignalStatus { get; set; }  
        
        [NotMapped]
        public string Balance { get; set; }
        [NotMapped]
        public bool OrderDaysAlert { get; set; }

        [NotMapped]
        public double LineCounts { get; set; }

        [NotMapped]
        public string ProCode { get; set; }
        [NotMapped]
        public string ProSku { get; set; }
        [NotMapped]
        public string ProImg { get; set; }
        [NotMapped]
        public string Retail { get; set; }
        [NotMapped]
        public string InvTotal { get; set; }
    }
    [MetadataType(typeof(CustomerOrderValidate))]
    public partial class CustomerOrder
    {
        [NotMapped]
        public string OrignalStatus { get; set; }

        [NotMapped]
        public string Balance { get; set; }

        [NotMapped]
        public bool OrderDaysAlert { get; set; }
        [NotMapped]
        public double LineCounts { get; set; }

        [NotMapped]
        public string ProCode { get; set; }
        [NotMapped]
        public string ProSku { get; set; }
        [NotMapped]
        public string ProImg { get; set; }
        [NotMapped]
        public string Retail { get; set; }
        [NotMapped]
        public string InvTotal { get; set; }
    }
}
