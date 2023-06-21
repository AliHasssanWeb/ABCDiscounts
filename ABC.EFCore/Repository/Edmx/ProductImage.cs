using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ProductImage
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public byte[] Image { get; set; }
        public string ImagePath { get; set; }
    }
}
