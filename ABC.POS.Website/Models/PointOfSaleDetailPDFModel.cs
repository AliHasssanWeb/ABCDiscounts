using iTextSharp.text;


namespace ABC.POS.Website.Models
{
    public class PointOfSaleDetailPDFModel
    {

        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemNumber { get; set; }
        public string Quantity { get; set; }
        public string InDiscount { get; set; }
        public string OutDiscount { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
        public string RingerQty { get; set; }
        // Item Name Description
        public string Description { get; set; }
        public string AmountRetail { get; set; }

    }
}
