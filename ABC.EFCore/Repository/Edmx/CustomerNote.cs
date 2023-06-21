using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class CustomerNote
    {
        public int NoteId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerNote1 { get; set; }
        public DateTime? NoteDate { get; set; }
        public string CustomerName { get; set; }
    }
}
