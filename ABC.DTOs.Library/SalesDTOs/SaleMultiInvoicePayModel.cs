﻿using ABC.DTOs.Library.Adaptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.DTOs.Library.SalesDTOs
{
    public class SaleMultiInvoicePayModel
    {
        public SaleMultiInvoicePayModel()
        {
            multiInvoiceTransaction = new HashSet<SaleMultiInvoiceTransactionAdp>();
        }

        public string CustomerId { get; set; }
        public string TotalPaidAmount { get; set; }
        public string TotalAmountAllocation { get; set; }
        public string ChangeAmount { get; set; }
        public string CustomerBalanceDue { get; set; }

        public ICollection<SaleMultiInvoiceTransactionAdp> multiInvoiceTransaction { get; set; }
    }
}