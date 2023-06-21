using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Driver
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public string RegisteredOn { get; set; }
        public string VehicleNumber { get; set; }
        public string Address { get; set; }
        public string Address1 { get; set; }
    }
}
