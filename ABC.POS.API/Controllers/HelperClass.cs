using ABC.EFCore.Repository.Edmx;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABC.POS.API.Controllers
{
    public class HelperClass 
    {
        public readonly ABCDiscountsContext db;
      
        public static void saveactivelogs(string operation, string username, string fromscreen, string date, int user_id )
        {
            
        }
    }
}
