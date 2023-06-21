using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.Customer.Domain.Entities
{
    public partial class CertificateBusinessType
    {
        public int Ctbid { get; set; }
        public int? CertificateId { get; set; }
        public int? CustomerId { get; set; }
        public string Type { get; set; }
    }
}
