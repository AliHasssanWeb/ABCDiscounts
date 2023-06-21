using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class ArticleTypeValidate
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required")]
        public string ArticleTypeName { get; set; }
    }
    [MetadataType(typeof(ArticleType))]
    public partial class ArticleType
    {
        
    }
}
