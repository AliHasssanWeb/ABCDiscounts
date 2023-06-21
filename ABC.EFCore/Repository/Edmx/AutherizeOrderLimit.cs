using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class AutherizeOrderLimit
    {
        public int RequiredId { get; set; }
        public string PlateNumber { get; set; }
        public string DrivingLicenseNumber { get; set; }
        public byte[] UploadFile { get; set; }
        public string UploadFilePath { get; set; }
        public DateTime? AccessDate { get; set; }
        public string AccessTime { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public string TicketId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
    }
}
