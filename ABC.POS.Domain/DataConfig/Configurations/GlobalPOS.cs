using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.POS.Domain.DataConfig.Configurations
{
    public class GlobalPOS
    {
        public static List<Product> _listcart = new List<Product>();
        public static List<Product> listcart
        {
            set { _listcart = value; }
            get { return _listcart; }

        }

        public static List<InventoryStock> _inventorylist = new List<InventoryStock>();
        public static List<InventoryStock> inventorylist
        {
            set { _inventorylist = value; }
            get { return _inventorylist; }

        }

        public static List<CustomerInformation> _customerinformation = new List<CustomerInformation>();
        public static List<CustomerInformation> customerinformation
        {
            set { _customerinformation = value; }
            get { return _customerinformation; }

        }
    }
}
