using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class WareHouse
    {
        public int WareHouseId { get; set; }
        public string WareHouseName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ConcernedPersonName { get; set; }
    }
}
