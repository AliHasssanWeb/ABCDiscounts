using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class Faq
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool? IsPublic { get; set; }
    }
}
