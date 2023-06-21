using Microsoft.AspNetCore.Mvc;

namespace ABC.POS.Website.Controllers
{
    public class Reporting : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MSAReport()
        {
            return View();
        }
    }
}
