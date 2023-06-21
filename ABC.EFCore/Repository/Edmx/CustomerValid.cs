using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CustomerValid
    {
        [Required(ErrorMessage ="Company name is required.")]
        public string Company { get; set; }
        [Required(ErrorMessage = "First name is required.")]

        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required.")]

        public string LastName { get; set; }
        [Required(ErrorMessage = "City is required.")]

        public string City { get; set; }
        [Required(ErrorMessage = "Phone is required.")]

        public string Phone { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Customer Type is required.")]

        public string CustomerType { get; set; }
        [Required(ErrorMessage = "Owner Address is required.")]

        public string OwnerAddress { get; set; }
        [Required(ErrorMessage = "Business Address is required.")]

        public string BusinessAddress { get; set; }
        [Required(ErrorMessage = "Business Name is required.")]

        public string BusinessName { get; set; }
        [NotMapped]
        public List<CertificateBusinessType> CertificateBusinessTypes { get; set; }
        [NotMapped]
        public CertificateExemptionInstruction CertificateExemptionInstructions { get; set; }
        [NotMapped]
        public List<CertificateIdentification> CertificateIdentifications { get; set; }
        [NotMapped]
        public List<CertificateReasonExemption> CertificateReasonExemptions { get; set; }
        [NotMapped]
        public CustomerPaperWork paperWork { get; set; }
        public AspNetUser AspNetUser { get; set; }
        [NotMapped]
        [Compare("PasswordHash")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        public CustomerClassification CustomerClassification { get; set; }
        [NotMapped]
        public CustomerBillingInfo CustomerBillingInfo { get; set; }
        [NotMapped]
        public Receivable Receivable { get; set; }

        [NotMapped]
        public string CustomerNote { get; set; }
        [NotMapped]
        public string NoteDate { get; set; }

        [NotMapped]
        public ZipCode ZipCode { get; set; }

    }
    [MetadataType(typeof(CustomerValid))]
    public partial class CustomerInformation
    {
        [NotMapped]
        public List<CertificateBusinessType> CertificateBusinessTypes { get; set; }
        [NotMapped]
        public CertificateExemptionInstruction CertificateExemptionInstructions { get; set; }
        [NotMapped]
        public List<CertificateIdentification> CertificateIdentifications { get; set; }
        [NotMapped]
        public List<CertificateReasonExemption> CertificateReasonExemptions { get; set; }
        [NotMapped]
        public CustomerPaperWork paperWork { get; set; }
        [NotMapped]
        public string ConfirmPassword { get; set; }
        [NotMapped]
        public AspNetUser AspNetUser { get; set; }

        [NotMapped]
        public Receivable Receivable { get; set; }

        [NotMapped]
        public CustomerClassification CustomerClassification { get; set; }
        [NotMapped]
        public CustomerBillingInfo CustomerBillingInfo { get; set; }   
        [NotMapped]
        public EmpBusiness EmpBusinesss { get; set; }   
        [NotMapped]
        public EmpIdentification EmpIdentification { get; set; }   
        [NotMapped]
        public EmpReasonExemption EmpReasonExemption { get; set; }

        [NotMapped]
        public string CustomerNote { get; set; }
        [NotMapped]
        public string NoteDate { get; set; }
        [NotMapped]
        public ZipCode ZipCode { get; set; }

    }
}
