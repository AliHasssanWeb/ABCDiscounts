using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CustomerDocumentValid
    {
        [NotMapped]
        public string UserName { get; set; }
    }
    [MetadataType(typeof(CustomerDocumentValid))]
    public partial class CustomerDocument
    {
        
        [NotMapped]
        public string UserName { get; set; }

    }
}
