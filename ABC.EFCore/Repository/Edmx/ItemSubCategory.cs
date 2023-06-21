using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ItemSubCategory
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string SubCategory { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
