using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class ZipCode
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StateShortcut { get; set; }
    }
}
