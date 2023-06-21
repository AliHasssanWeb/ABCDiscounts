using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Customer.Domain.Entities
{
    public partial class CertificateReasonExemptionbk
    {
        public int ReasonId { get; set; }
        public int? CertificateId { get; set; }
        public int? CustomerId { get; set; }
        public string Reason { get; set; }
        public string Text { get; set; }
    }
}
