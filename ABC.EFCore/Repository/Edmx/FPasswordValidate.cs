using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class FPasswordValidate
    {
        public string Token { get; set; }

        public string Password { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]

        public string Email { get; set; }

        [Required(ErrorMessage = "New Password is Required")]
        [NotMapped]
        public string NewPasswords { get; set; }

        [Required(ErrorMessage = " Confirm new password is required")]
        [Compare("NewPasswords", ErrorMessage = "Your confirm Password is not matched with password")]
        [NotMapped]
        public string ConfirmNewPassword { get; set; }
        public string FromScreen { get; set; }
        public bool? Status { get; set; }
    }
    [MetadataType(typeof(FPasswordValidate))]
    public partial class ForgetPassword
    {
        public string Token { get; set; }

        public string Password { get; set; }

        [Required(ErrorMessage = "New Password is Required")]
        [NotMapped]
        public string NewPasswords { get; set; }

        [Required(ErrorMessage = " Confirm new password is required")]
        [Compare("NewPasswords", ErrorMessage = "Your confirm Password is not matched with password")]
        [NotMapped]
        public string ConfirmNewPassword { get; set; }
    }
}
