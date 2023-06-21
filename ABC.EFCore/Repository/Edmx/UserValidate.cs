using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class UserValid
    {

        [NotMapped]
        public string RefreshToken { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string ConfirmPasswordHash { get; set; }
        [NotMapped]
        public string lastsale { get; set; }
    }
    [MetadataType(typeof(UserValid))]
    public partial class AspNetUser
    {
        [NotMapped]
        public string RefreshToken { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string ConfirmPasswordHash { get; set; }

        [NotMapped]
        public string lastsale { get; set; }
    }
}
