using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public partial class SaleInvoicesModel
    {
        public SaleInvoicesModel()
        {
            saleInvoiceTransactionModel = new HashSet<SaleInvoiceTransactionModel>();
        }


        public int? SalesInvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? Date { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string AccountId { get; set; }
        public string TotalPaid { get; set; }
        public string TotalAmount { get; set; }
        public string Change { get; set; }
        public string InvBalance { get; set; }
        public string InvoiceTotal { get; set; }
        public string PreviousBalance { get; set; }
        public string Buyer { get; set; }
        public string SubTotal { get; set; }
        public string Other { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string Freight { get; set; }

        public virtual ICollection<SaleInvoiceTransactionModel> saleInvoiceTransactionModel { get; set; }
        public virtual ICollection<PointOfSaleDetailModel> itemsdetails { get; set; }
    }
}
