using ABC.Admin.Domain.DataConfig;
using ABC.Admin.Website.Models;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;

namespace ABC.Admin.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/AllOrderGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        int NewOrders = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == false).Count();
                        int Approved = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Pulled = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Rejected = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == true).Count();
                        int Delivered = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == true && x.AdminStatus == true && x.IsRejected == false).Count();

                        ViewBag.NewOrders = NewOrders.ToString();
                        ViewBag.Approved = Approved.ToString();
                        ViewBag.Rejected = Rejected.ToString();
                        ViewBag.Delivery = Delivered.ToString();
                        ViewBag.Pulled = Pulled.ToString();
                     
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }


                SResponse Customerress = RequestSender.Instance.CallAPI("api",
                "Customers/CustomersGet", "GET");
                if (Customerress.Status && (Customerress.Resp != null) && (Customerress.Resp != ""))
                {
                    ResponseBack<List<CustomerInformation>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerInformation>>>(Customerress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        int approvedCustomer = response.Data.ToList().Where(x => x.Pending == false && x.Approved == true && x.Rejected == false).Count();
                        int pendingCustomer = response.Data.ToList().Where(x => x.Pending == true && x.Approved == false && x.Rejected == false).Count();
                        int rejectedCustomer = response.Data.ToList().Where(x => x.Pending == false && x.Approved == false && x.Rejected == true).Count();

                        ViewBag.ApprovedCustomer = approvedCustomer.ToString();
                        ViewBag.PendingCustomer = pendingCustomer.ToString();
                        ViewBag.RejectedCustomer = rejectedCustomer.ToString();
                       
                    }
                    else
                    {
                        TempData["response"] = "No Customer Record.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();

            }
        }

        public IActionResult Dashboard()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/AllOrderGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        int NewOrders = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == false).Count();
                        int Approved = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Pulled = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Rejected = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == true).Count();
                        int Delivered = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == true && x.AdminStatus == true && x.IsRejected == false).Count();

                        ViewBag.NewOrders = NewOrders.ToString();
                        ViewBag.Approved = Approved.ToString();
                        ViewBag.Rejected = Rejected.ToString();
                        ViewBag.Delivery = Delivered.ToString();
                        ViewBag.Pulled = Pulled.ToString();
                        return View();
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();

            }
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

        public IActionResult Managers()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/AllOrderGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        int NewOrders = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == false).Count();
                        int Approved = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Pulled = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).Count();
                        int Rejected = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == true).Count();
                        int Delivered = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == true && x.AdminStatus == true && x.IsRejected == false).Count();

                        ViewBag.NewOrders = NewOrders.ToString();
                        ViewBag.Approved = Approved.ToString();
                        ViewBag.Rejected = Rejected.ToString();
                        ViewBag.Delivery = Delivered.ToString();
                        ViewBag.Pulled = Pulled.ToString();
                        return View();
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();

            }
        }
    }
}
