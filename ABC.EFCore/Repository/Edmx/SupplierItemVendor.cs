using Microsoft.AspNetCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class SupplierItemVendor
    {
        [Required(ErrorMessage = "Email Code is Required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Code is Required")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Address Code is Required")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Company Code is Required")]
        public string Company { get; set; }
        [Required(ErrorMessage = "First Name  is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name  is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Credit Limit is Required")]
        public string CreditLimit { get; set; }
        [Required(ErrorMessage = "WebSite is Required")]
        public string Website { get; set; }
        [Required(ErrorMessage = "Ledger is Required")]
        public string Ledger { get; set; }
        [Required(ErrorMessage = "Ledger Code is Required")]
        public string LedgerCode { get; set; }
        public SupplierItemNumber supplierItemNumber { get; set; }
        public PurchaseOrder purchaseOrder { get; set; }
        [NotMapped]
        public string LastChequeNum { get; set; }
        [NotMapped]
        public string PayablesFound { get; set; }
        [NotMapped]
        public string CustomerNote { get; set; }
        [NotMapped]
        public string NoteDate { get; set; }
    }
    [MetadataType(typeof(SupplierItemVendor))]
    public partial class Vendor
    {
        [NotMapped]
        public SupplierItemNumber supplierItemNumber { get; set; }
        [NotMapped]
        public PurchaseOrder purchaseOrder { get; set; }
        [NotMapped]
        public string LastChequeNum { get; set; }
        [NotMapped]
        public string PayablesFound { get; set; }
        [NotMapped]
        public string CustomerNote { get; set; }
        [NotMapped]
        public string NoteDate { get; set; }
    }

}
