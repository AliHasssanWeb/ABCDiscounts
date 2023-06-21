using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ABC.EFCore.Entities.Admin
{
    public class ChangePassword
    {
        [Required(ErrorMessage = "Please Enter Password.")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Confirm Password.")]
        [Compare("PasswordHash", ErrorMessage = "Password & Confirm Password does not match")]
        public string ConfirmPasswordHash { get; set; }
        [Required(ErrorMessage = "Please Enter Old Password.")]
        public string CurrentPasswordHash { get; set; }
       
        public string Email { get; set; }
    }
}