using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Website.Models;
using ABC.POS.Website.Service;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISession session;
        private readonly IEmailService _emailService;
        private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IRazorViewToStringRenderer razorViewToStringRenderer)
        {
            _logger = logger;
            this.session = httpContextAccessor.HttpContext.Session;
            _emailService = emailService;
            _razorViewToStringRenderer = razorViewToStringRenderer;
        }
        public async Task<ViewResult> Index()    
        {

            return View();
        }
        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult CashierIndex()
        {
            string userData = session.GetString("userobj");
            if (!string.IsNullOrEmpty(userData))
            {
                AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                ViewBag.LastLogin = userDto.LastLogin;

                SResponse ress = RequestSender.Instance.CallAPI("api",
              "Sale/CashierCounters" + "/" + userDto.EmployeeId, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<CashierCounters>>(ress.Resp);
                    if (response.Data != null)
                    {
                        ViewBag.Online = response.Data.OnlineOrders;
                        ViewBag.Sales = response.Data.TodaysSale;

                    }
                }
            }
            else
            {

            }
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult SessionExpire()
        {
            return View();
        }
    }
}
