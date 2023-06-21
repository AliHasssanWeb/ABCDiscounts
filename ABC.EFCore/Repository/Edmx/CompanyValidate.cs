using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CompanyValidate
    {
       
        [NotMapped]
        public List<Product> product { get; set; }
    }
    [MetadataType(typeof(CompanyValidate))]
    public partial class Employer
    {
    
        [NotMapped]
        public List<Product> product { get; set; }
    }
}
