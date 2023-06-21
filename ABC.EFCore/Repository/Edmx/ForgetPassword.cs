using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ForgetPassword
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Otp { get; set; }
        public string Email { get; set; }
        public string FromScreen { get; set; }
        public bool? Status { get; set; }
    }
}
