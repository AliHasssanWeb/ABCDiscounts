using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;

namespace ABC.Admin.Website.Controllers
{
    public class OrderController : Controller
    {
        private readonly ISession session;
        public OrderController(IHttpContextAccessor httpContextAccessor)
        {
            this.session = httpContextAccessor.HttpContext.Session;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult OrdersForApproval()
        {

            try
            {

                SResponse ressrole = RequestSender.Instance.CallAPI("api",
                   "UserManagement/PullerEmployeeGet", "GET");
                if (ressrole.Status && (ressrole.Resp != null) && (ressrole.Resp != ""))
                {
                    ResponseBack<List<Employee>> responseEmplo =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressrole.Resp);
                    if (responseEmplo.Data != null && responseEmplo.Data.Count() > 0)
                    {
                        List<Employee> responseObject = responseEmplo.Data.ToList();
                        ViewBag.Employee = new SelectList(responseObject.ToList(), "EmployeeId", "FullName");
                    }
                    else
                    {

                        List<Employee> responseObject = new List<Employee>();
                        ViewBag.Employee = new SelectList(responseObject, "EmployeeId", "FullName");
                        TempData["response"] = "No Puller Employee Record Exists.";
                        //return RedirectToAction("OrdersForApproval", "Order");
                    }
                }


                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/NewOrdersGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        var Record = response.Data.ToList();
                        for (int i = 0; i < Record.Count(); i++)
                        {
                            if(Record[i].Balance != "0" && Record[i].Balance != null)
                            {
                                //if (Record[i].Balance.Contains('.'))
                                //{
                                //    Record[i].Balance = Record[i].Balance.Split('.')[0];
                                //}

                                
                            }
                            double value = Convert.ToDouble(Record[i].OrderAmount);
                            value = (double)System.Math.Round(value, 2);
                            Record[i].OrderAmount = Convert.ToString(value);
                        }
                        // var list = Record.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        //for (int i = 0; i < Record.Count(); i++)
                        //{
                        //    if (CheckDays(Convert.ToDateTime(Record[i].OrderDate)) == false)
                        //    {
                        //        Record[i].OrderDaysAlert = true;
                        //    }
                        //}
                        Record = Record.OrderByDescending(x => x.OrderId).ToList();
                        return View(Record);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();  
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
    
        public IActionResult OrdersForPulled()
        {
            try
            {
                SResponse ressrole = RequestSender.Instance.CallAPI("api",
                   "UserManagement/PullerCashierGet", "GET");
                if (ressrole.Status && (ressrole.Resp != null) && (ressrole.Resp != ""))
                {
                    ResponseBack<List<Employee>> responseEmplo =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressrole.Resp);
                    if (responseEmplo.Data != null && responseEmplo.Data.Count() > 0)
                    {
                        List<Employee> responseObject = responseEmplo.Data.ToList();
                        ViewBag.CashierEmployee = new SelectList(responseObject.ToList(), "EmployeeId", "FullName");
                    }
                    else
                    {
                        List<Employee> responseObject = new List<Employee>();
                        ViewBag.CashierEmployee = new SelectList(responseObject, "EmployeeId", "FullName");
                        TempData["response"] = "No Cashier Employee Record Exists.";
                        //return RedirectToAction("OrdersForApproval", "Order");
                    }
                }
                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/AllOrderGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        var Record = response.Data.ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false).ToList();
                        var list = Record.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        list = list.OrderByDescending(x => x.OrderId).ToList();

                        return View(list);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        public IActionResult OrdersForDelivered()
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
                        var Record = response.Data.ToList().Where(x => x.IsPulled == true && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false && x.IsInvoiced == true).ToList();
                        var list = Record.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        //for (int i = 0; i < list.Count(); i++)
                        //{
                          
                        //    if (CheckDays(Convert.ToDateTime(list[i].InvoicedDate)) == false)
                        //    {
                        //        list[i].OrderDaysAlert = true;
                        //    }
                        //}
                        return View(list);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }


        }
        public IActionResult ClosedOrders()
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
                        var Record = response.Data.ToList().Where(x => x.IsPulled == true && x.Delivered == true && x.AdminStatus == true && x.IsRejected == false).ToList();
                        var list = Record.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        return View(list);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }


        } 
        
        public IActionResult RejectedOrders()
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
                        var Record = response.Data.ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == true).ToList();
                        var list = Record.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        return View(list);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }


        }

        [HttpGet]
        public IActionResult ListOfCartAminApproval(int? id,string ticketId)
        {
            try
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/GetUserCartByAdminApproval/?id=" + id+"&ticketId="+ticketId, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    return Json(response.Data);
                }
                return Json("null");
            }
            catch (Exception ex)
            {

                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult ApproveOrder(int? id,int? userid,string ordernum, string orderDate, string pulleremployeeid)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    userid = userDto.Id;
                    AccessName = userDto.UserName;
                }
                else
                {
                    return Json("false");
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/ApproveOrderById/?id=" + id+"&userid="+userid+"&ordernum="+ordernum+ "&orderDate="+orderDate + "&pullerId=" + pulleremployeeid, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    var response = JsonConvert.DeserializeObject<ResponseBack<CustomerOrder>>(resp.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        HttpContext.Session.SetString("pullerName", responseObject.PulledBy);
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessName.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Orders";
                        activity.NewDetails = "Approved Order # " + ordernum + " on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    }
                    else
                    {
                        HttpContext.Session.SetString("pullerName", "N/A");

                    }
                    //if (!ordernum.Contains("00-"))
                    //{
                    //    ordernum = "00" + ordernum;
                    //}
                    HttpContext.Session.SetString("orderIdDetail", ordernum);
                    return Json("true");
                }
                return Json("false");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult DeliverOrder(int? id,int? userid, string ordernum, string orderDate)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    userid = userDto.Id;
                    AccessName = userDto.UserName;
                }
                else
                {
                    return Json("false");
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/DeliverOrderById/?id=" + id + "&userid=" + userid + "&ordernum=" + ordernum + "&orderDate=" + orderDate, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    if(response.Status == 13)
                    {
                        return Json(response.Data);
                    }
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Orders";
                    activity.NewDetails = "Delivered Order # " + ordernum + " on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    return Json(true);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        [HttpGet]
        public IActionResult RejectOrder(int? id, int? userid, string ordernum, string reason, string reasondropdown)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    userid = userDto.Id;
                    AccessName = userDto.UserName;
                }
                else
                {
                    return Json("false");
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/RejectOrderById/?id=" + id + "&userid=" + userid + "&ordernum=" + ordernum + "&reason=" + reason + "&reasondropdown=" + reasondropdown, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    if (response.Status == 13)
                    {

                        return Json(response.Data);
                    }

                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Orders";
                    activity.NewDetails = "Closed Order # " + ordernum + " on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    return Json("true");
                }
                return Json("null");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult OrderDetail(string ticketId)
        {
            try
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/OrderDetail/?&ticketId=" + ticketId, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(resp.Resp);
                    return Json(response.Data);
                }
                return Json(null);
            }
            catch (Exception ex)
            {

                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        public IActionResult OrderSummary()
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
                        var list = response.Data.ToList();
                        var Record = list.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                        for (int i = 0; i < Record.Count(); i++)
                        {

                            double value = Convert.ToDouble(Record[i].OrderAmount) ;
                            value = (double)System.Math.Round(value, 2);
                            Record[i].OrderAmount =Convert.ToString(value);
                            

                            if (Record[i].AdminStatus == false && Record[i].IsPulled == false && Record[i].IsInvoiced == false && Record[i].Delivered == false && Record[i].IsRejected == false)
                            {
                                // order date and current date days diff
                                // call function to calculate days
                                Record[i].OrderDaysAlert = false;
                                if (CheckDays(Convert.ToDateTime(Record[i].OrderDate)) == false)
                                {
                                    Record[i].OrderDaysAlert = true;
                                }
                                Record[i].OrignalStatus = "Pending";
                            }
                            else if (Record[i].AdminStatus == true && Record[i].IsPulled == false && Record[i].IsInvoiced == false && Record[i].Delivered == false && Record[i].IsRejected == false)
                            {
                                if (CheckDays(Convert.ToDateTime(Record[i].AdminActionDate)) == false)
                                {
                                    Record[i].OrderDaysAlert = true;
                                }
                                Record[i].OrignalStatus = "Approved";
                            }
                            else if (Record[i].AdminStatus == true && Record[i].IsPulled == true && Record[i].IsInvoiced == false && Record[i].Delivered == false && Record[i].IsRejected == false)
                            {
                                if (CheckDays(Convert.ToDateTime(Record[i].PulledDate)) == false)
                                {
                                    Record[i].OrderDaysAlert = true;
                                }
                                Record[i].OrignalStatus = "Pulled";
                            }
                            else if (Record[i].AdminStatus == true && Record[i].IsPulled == true && Record[i].IsInvoiced == true && Record[i].Delivered == false && Record[i].IsRejected == false)
                            {
                                if (CheckDays(Convert.ToDateTime(Record[i].InvoicedDate)) == false)
                                {
                                    Record[i].OrderDaysAlert = true;
                                }
                                Record[i].OrignalStatus = "Ready To Shipment";
                            }
                            else if (Record[i].AdminStatus == true && Record[i].IsPulled == true && Record[i].IsInvoiced == true && Record[i].Delivered == true && Record[i].IsRejected == false)
                            {
                                if (CheckDays(Convert.ToDateTime(Record[i].DeliveredDate)) == false)
                                {
                                    Record[i].OrderDaysAlert = true;
                                }
                                Record[i].OrignalStatus = "Delivered";
                            }
                            else if (Record[i].AdminStatus == false && Record[i].IsPulled == false && Record[i].IsInvoiced == false && Record[i].Delivered == false && Record[i].IsRejected == true)
                            {
                                Record[i].OrignalStatus = "On Hold";
                            }
                            else
                            {
                                Record[i].OrignalStatus = "Invalid";
                            }
                        }
                        Record = Record.OrderByDescending(x => x.OrderId).ToList();
                        return View(Record);
                    }
                    else
                    {
                        TempData["response"] = "No Order Exist";
                    }
                }
                List<CustomerOrder> orders = new List<CustomerOrder>();
                return View(orders);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        public IActionResult OrderDetails(string id, string type)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Orders/GetOrderDetails" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data.ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Order Detail Exists.";
                        List<CartDetail> obj2 = new List<CartDetail>();
                        return View(obj2);
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

        public bool CheckDays(DateTime actionDate)
        {

            try
            {
                var CurrentDate = DateTime.Now;
                var FoundedDays = (CurrentDate - actionDate.Date).Days;
                if(FoundedDays >= 2)
                {
                    return false;
                }
                else 
                    return true;
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return false;
            }
        }


        //public IActionResult AdminOrderDetails(string id, string type)
        //{
        //    try
        //    {
        //        SResponse ress = RequestSender.Instance.CallAPI("api",
        //     "Orders/GetOrderDetails" + "/" + id, "GET");
        //        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
        //        {
        //            var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
        //            if (response.Data != null)
        //            {
        //                var responseObject = response.Data.ToList();
        //                return View(responseObject);
        //            }
        //            else
        //            {
        //                TempData["response"] = "No Order Detail Exists.";
        //                List<CartDetail> obj2 = new List<CartDetail>();
        //                return View(obj2);
        //            }
        //        }
        //        return View();
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["response"] = ex.Message + "Error Occured.";
        //        return View();
        //    }
        //}


        public IActionResult AdminOrderDetails(string id, string type)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Orders/GetOrderDetails" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data.ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Order Detail Exists.";
                        List<CartDetail> obj2 = new List<CartDetail>();
                        return View(obj2);
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

        [HttpPost]
        public IActionResult CheckSupervisorCreditAmount(string customerId, string accessPin, string comment, string creditAmount,string duedate)
        {
            try
            {


                if(accessPin == null || comment == null || creditAmount == null || duedate == null)
                {
                    return Json("false");
                }

                if(duedate != null)
                {
                    session.SetString("creditduedate", duedate);
                }
                var Msg = "";
                Supervisor supervisor = new Supervisor();
                supervisor.AccessPin = accessPin;
                supervisor.CreditLimit = creditAmount;
                var body = JsonConvert.SerializeObject(supervisor);
                SResponse ress = RequestSender.Instance.CallAPI("api", "Orders/CheckSupervisorCreditAmount/" + int.Parse(customerId), "POST", body);
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Supervisor>>(ress.Resp);
                    if (response.Status == 1)
                    {
                        Msg = "Success";
                        return Json("Success.");
                    }
                    if (response.Status == 13)
                    {
                        Msg = "ExceedLimit";
                        return Json("ExceedLimit.");
                    }
                    if (response.Status == 5)
                    {
                        Msg = "InvlaidPin";
                        return Json("InvlaidPin.");
                    }
                    if (response.Status == 14)
                    {
                        Msg = "This Supervisor Already Availed Credits.";
                        return Json("This Supervisor Already Availed Credits.");
                    }
                    if (response.Status == 15)
                    {
                        Msg = "This Manager Already Availed Credits.";
                        return Json("This Manager Already Availed Credits.");
                    }
                    if (response.Status == 16)
                    {
                        Msg = "You Do Not Have Much Limit.";
                        return Json("You Do Not Have Much Limit.");
                    }
                    else
                    {
                        TempData["response"] = "Session Expire.";
                        return Json("Error");
                    }
                }
                return Json(Msg);
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult PullOrder(int? id, string ordernum, string pulleremployeeid)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    int currentuserid = userDto.Id;
                    AccessName = userDto.UserName;
                }
                else
                {
                    return Json("false");
                }

                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/PullOrderById/?id=" + id + "&ordernum=" + ordernum + "&pullerId=" + pulleremployeeid, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Orders";
                    activity.NewDetails = "Pulled Order # " +ordernum+ " on "  + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    return Json("true");
                }
                return Json("false");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        [HttpGet]
        public IActionResult AccessApproveOrder(int? id, int? userid, string ordernum, string orderDate, string pulleremployeeid,string comment,string CreditTotal,string autherizePin,string creditduedate,string expecteddate)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                string DueDateData = session.GetString("creditduedate");
                int? currentuserid = 0;
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    currentuserid = userDto.Id;
                    AccessName = userDto.UserName;
                }
                else
                {
                    return Json("false");
                }
                if (!string.IsNullOrEmpty(DueDateData))
                {
                    creditduedate = DueDateData;
                }
                else
                {
                    return Json("Need pin validation again.");
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/AccessApproveOrderById/?id="+id+ "&userid=" + userid + "&ordernum=" + ordernum + "&orderDate=" + orderDate + "&pullerId=" + pulleremployeeid + "&comment=" + comment + "&CreditTotal=" + CreditTotal + "&autherizePin=" + autherizePin + "&creditduedate=" + creditduedate, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    var response = JsonConvert.DeserializeObject<ResponseBack<CustomerOrder>>(resp.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        HttpContext.Session.SetString("pullerName", responseObject.PulledBy);
                        HttpContext.Session.SetString("orderIdDetail", responseObject.TicketId);
                        ActivityLog activity = new ActivityLog();
                        activity.CreatedDate = DateTime.Now;
                        activity.CreatedBy = AccessName.ToString();
                        activity.Deleted = false;
                        activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                        activity.OperationName = "Pin";
                        activity.NewDetails = "Credit Pin Verified on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                        activity.Extraone = "Admin";
                        var Activitybody = JsonConvert.SerializeObject(activity);
                        SResponse Activityress = RequestSender.Instance.CallAPI("api",
                         "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);
                    }
                    else
                    {
                        HttpContext.Session.SetString("pullerName", "N/A");
                        HttpContext.Session.SetString("orderIdDetail", "0000");
                    }
                    //if (!ordernum.Contains("00-"))
                    //{
                    //    ordernum = "00" + ordernum;
                    //}
                    return Json("true");
                }
                return Json("false");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }



        public IActionResult PrintPage()
        {
            try
            {
                var id = "";
                string sessionData = session.GetString("orderIdDetail");
                if (!string.IsNullOrEmpty(sessionData))
                {
                    id = sessionData.ToString();

                }
                string sessionDataa = session.GetString("CurrentProductdata");
                if (!string.IsNullOrEmpty(sessionDataa))
                {
                    id = sessionDataa.ToString();

                }


                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Orders/GetOrderDetails" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        string pullerData = session.GetString("pullerName");
                        if (!string.IsNullOrEmpty(pullerData))
                        {
                            response.Data.ElementAt(0).CustomerOrders.ElementAt(0).PulledBy = pullerData.ToString();

                        }
                        var responseObject = response.Data.ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Order Detail Exists.";
                        List<CartDetail> obj2 = new List<CartDetail>();
                        return View(obj2);
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
        public IActionResult PrintPageBtn(string ordernum)
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Orders/GetOrderDetails" + "/" + ordernum, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {

                        var responseObject = response.Data.ToList();
                        //var srt = JsonConvert.SerializeObject(responseObject[0].TicketId);
                        //HttpContext.Session.SetString("CurrentProductdata", srt);
                        HttpContext.Session.SetString("CurrentProductdata", responseObject[0].TicketId);
                        return Json(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Order Detail Exists.";
                        List<CartDetail> obj2 = new List<CartDetail>();
                        return View(obj2);
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


        [HttpGet]
        public IActionResult ReOpenOrder(string ticketId)
        {
            try
            {
                string AccessToken = string.Empty;
                string AccessName = string.Empty;
                string userData = session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    AccessToken = userDto.RefreshToken;
                    AccessName = userDto.UserName;
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Orders/ReOpenRejectedOrder/?ticketId=" + ticketId, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ActivityLog activity = new ActivityLog();
                    activity.CreatedDate = DateTime.Now;
                    activity.CreatedBy = AccessName.ToString();
                    activity.Deleted = false;
                    activity.LogTime = DateTime.Now.TimeOfDay.ToString();
                    activity.OperationName = "Orders";
                    activity.NewDetails = "Re-Open Order # "+ ticketId  +" on " + DateTime.Now.Date + " " + "by " + AccessName.ToString();
                    activity.Extraone = "Admin";
                    var Activitybody = JsonConvert.SerializeObject(activity);
                    SResponse Activityress = RequestSender.Instance.CallAPI("api",
                     "ActivityLogs/ActivityLogsCreate", "POST", Activitybody);

                    return Json("true");
                }
                return Json("false");
            }
            catch (Exception ex)
            {

                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        public IActionResult RePrintPage(string id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Orders/GetOrderDetails" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        //string pullerData = session.GetString("pullerName");
                        //if (!string.IsNullOrEmpty(pullerData))
                        //{
                        //    response.Data.ElementAt(0).CustomerOrders.ElementAt(0).PulledBy = pullerData.ToString();

                        //}
                        var responseObject = response.Data.ToList();
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Order Detail Exists.";
                        List<CartDetail> obj2 = new List<CartDetail>();
                        return View(obj2);
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
