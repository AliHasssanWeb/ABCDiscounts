using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.EFCore.Repository.Edmx
{
    public class CashierCounters
    {
        [NotMapped]
        public int OnlineOrders { get; set; }

        [NotMapped]
        public int TodaysSale { get; set; }
    }
}
