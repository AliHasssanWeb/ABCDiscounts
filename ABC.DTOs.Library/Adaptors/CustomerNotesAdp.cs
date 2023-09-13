using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.Adaptors
{
    public class CustomerNotesAdp
    {
        public int NoteId { get; set; }
        public string CustomerNote { get; set; }
        public DateTime? NoteDate { get; set; }
    }
}
