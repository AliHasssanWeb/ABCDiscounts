using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class FaqValidation
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public bool? IsPublic { get; set; }

    }
    
    [MetadataType(typeof(FaqValidation))]

    public partial class Faq
    {
       
    }
}
