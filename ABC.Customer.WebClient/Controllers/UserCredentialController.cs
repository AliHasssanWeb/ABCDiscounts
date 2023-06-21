using ABC.Customer.Domain.DataConfig;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCDiscountWebSite.Areas.Security.Controllers
{
    [ServiceFilter(typeof(ConfigureSession))]
    public class UserCredentialController : Controller
    {
        public IActionResult ChangePassword()
        {
            return View();
        }
    }
}
