using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SaleHistoryValidate
    {
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string ProductName { get; set; }

    }
    [MetadataType(typeof(SaleHistoryValidate))]
    public partial class SaleHistory
    {
        [NotMapped]
        public string CustomerName { get; set; }
        [NotMapped]
        public string ProductName { get; set; }
    }
}
