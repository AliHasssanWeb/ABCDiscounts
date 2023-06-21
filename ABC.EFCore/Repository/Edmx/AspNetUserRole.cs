using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AspNetUserRole
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RolesId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
    }
}
