using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.Customer.WebClient.Models
{
    public class Login
    {
        [Required(ErrorMessage ="User Email Is Required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required.")]
        public string PasswordHash { get; set; }
    }
}
