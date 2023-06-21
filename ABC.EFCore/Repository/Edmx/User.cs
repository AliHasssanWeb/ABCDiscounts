using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class User 
    {
        public int? RoleId { get; set; }
        public byte[] UserPic { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? LastChangePwdDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? Deleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Imageupload { get; set; }
        public bool? IsCancelled { get; set; }
        public int? StateId { get; set; }
        public string DrivingLicense { get; set; }
        public string FromScreen { get; set; }
        public int? EmployeeId { get; set; }
        public bool? AdminApproval { get; set; }
        public string State { get; set; }
        public string RoleName { get; set; }
    }
}
