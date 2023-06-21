using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.DataConfig
{
    public class UserDetailAuthToken
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DrivingLicense { get; set; }
        public string FromScreen { get; set; }
        public string State { get; set; }
        public string Token { get; set; }
        public string RoleName { get; set; }
    }
}
