using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Entities.Admin
{
    public class CustomerPaperWork
    {
        public int PaperID { get; set; }
        public int UserID { get; set; }
        public int CustomerID { get; set; }
        public byte[] FederalForm { get; set; }
        public byte[] SalesTaxID { get; set; }
        public byte[] DrivingLicenseID { get; set; }
        public string FederalFormPath { get; set; }
        public string SalesTaxIDPath { get; set; }
        public string DrivingLicenseIDPath { get; set; }
        public Nullable<DateTime> UploadedDate { get; set; }

    }
}
