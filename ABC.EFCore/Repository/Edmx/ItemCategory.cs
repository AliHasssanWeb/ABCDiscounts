using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] CategoryImage { get; set; }
        public string CategoryImageByPath { get; set; }
        public string Color { get; set; }
    }
}
