using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SupplierDocument
    {
        public int DocumentId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string DocumentName { get; set; }
        public byte[] Image { get; set; }
        public string ImageByPath { get; set; }
        public int? SupplierId { get; set; }
    }
}
