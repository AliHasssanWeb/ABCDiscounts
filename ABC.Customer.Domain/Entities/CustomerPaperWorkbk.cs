﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Customer.Domain.Entities
{
    public partial class CustomerPaperWorkbk
    {
        public int PaperId { get; set; }
        public int? UserId { get; set; }
        public int? CustomerId { get; set; }
        public byte[] FederalForm { get; set; }
        public byte[] SalesTaxId { get; set; }
        public byte[] DrivingLicenseId { get; set; }
        public string FederalFormPath { get; set; }
        public string SalesTaxIdpath { get; set; }
        public string DrivingLicenseIdpath { get; set; }
        public DateTime? UploadedDate { get; set; }
        public string FromScreen { get; set; }
    }
}
