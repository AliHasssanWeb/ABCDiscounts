using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Customer.Domain.Entities
{
    public partial class CertificateIdentification
    {
        public int Ciid { get; set; }
        public int? CertificateId { get; set; }
        public int? CustomerId { get; set; }
        public string ReasonExamption { get; set; }
        public string IdentificationNumber { get; set; }
        public string State { get; set; }
    }
}
