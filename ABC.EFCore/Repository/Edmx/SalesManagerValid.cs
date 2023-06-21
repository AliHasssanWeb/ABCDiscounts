using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class SalesManagerValid
    {
        public AspNetUser AspNetUser { get; set; }
    }
    [MetadataType(typeof(SalesManagerValid))]
    public partial class SalesManager
    {
        [NotMapped]
        public AspNetUser AspNetUser { get; set; }


    }
}
