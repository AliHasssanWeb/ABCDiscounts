using ABC.EFCore.Repository.Edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Customer.Domain.Configuration
{
    public class GlobalAccess
    {


        public static List<CartDetail> _listcart;
        public static List<CartDetail> listcart
        {
            set { _listcart = value; }
            get { return _listcart; }

        }

        public static List<Product> _loadedProducts = new List<Product>();
        public static List<Product> loadedProducts
        {
            set { _loadedProducts = value; }
            get { return _loadedProducts; }

        }

        public static List<InventoryStock> _loadstock = new List<InventoryStock>();
        public static List<InventoryStock> loadstock
        {
            set { _loadstock = value; }
            get { return _loadstock; }

        }
    }
}
