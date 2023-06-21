using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AspNetUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public string AccessFailedCount { get; set; }
        public string UserName { get; set; }
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
    }
}
