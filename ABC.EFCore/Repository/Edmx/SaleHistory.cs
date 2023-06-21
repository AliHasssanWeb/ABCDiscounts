using System;
using System.Collections.Generic;

#nullable disable

namespace ABC.EFCore.Repository.Edmx
{
    public partial class SaleHistory
    {
        public int SaleHistoryId { get; set; }
        public int? ProductId { get; set; }
        public string CartDetailId { get; set; }
        public string TransacetionId { get; set; }
        public string PosSaleId { get; set; }
        public string ItemNumber { get; set; }
        public byte[] ItemImage { get; set; }
        public string ItemImageByPath { get; set; }
        public int? QtyinStock { get; set; }
        public int? OrderQuantity { get; set; }
        public int? ShipmentLimit { get; set; }
        public string TicketId { get; set; }
        public int? CustomerId { get; set; }
        public int? RingerQuantity { get; set; }
        public int? QuantityDifference { get; set; }
    }
}
