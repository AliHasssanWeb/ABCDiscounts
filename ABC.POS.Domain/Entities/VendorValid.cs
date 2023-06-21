using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.POS.Domain.Entities
{
    public class VendorValid
    {
        public string VendorIdentity { get; set; }
    }
    [MetadataType(typeof(VendorValid))]
    public partial class Vendor
    {
        public string VendorIdentity { get; set; }
    }
}
