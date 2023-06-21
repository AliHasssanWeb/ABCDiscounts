using ABC.Customer.Domain.Configuration;
using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    public class CustomerHomePageController : Controller
    {
        private static IHttpContextAccessor httpContextAccessor;
        public CustomerHomePageController(IHttpContextAccessor accessor)
        {
            httpContextAccessor = accessor;
        }
        string check = "";
        string count = "";
        public IActionResult Index()
        {
            try
            {
                //     string abc =  httpContextAccessor.HttpContext.Session.GetString("userobj");
                int userId = 0;
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    userId = userDto.Id;
                    ViewBag.LastLogin = userDto.LastLogin;
                    ViewBag.LastChangePwdDate = userDto.LastChangePwdDate;
                }
                SResponse ressOrder = RequestSender.Instance.CallAPI("api", "Customer/GetUserOrderFromCart/" + userId, "GET");
                if (ressOrder.Status && (ressOrder.Resp != null) && (ressOrder.Resp != ""))
                {
                    ResponseBack<List<CustomerOrder>> responseOrder =
                                 JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ressOrder.Resp);
                    if (responseOrder.Data != null && responseOrder.Data.Count() > 0)
                    {
                        //ViewBag.LastOrderDate = responseOrder.Data[0].OrderDate.;
                        //var lastOrder = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.LastOrDefault()).ToList().Where(x => x.UserId == userId).LastOrDefault().OrderDate;
                        var lastOrder = responseOrder.Data.ToList().OrderBy(x => x.OrderId).Where(x => x.UserId == userId).LastOrDefault().OrderDate;
                        int NewOrders = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == false && x.UserId == userId).Count();
                        int Approved = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false && x.UserId == userId).Count();
                        int Pulled = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == false && x.AdminStatus == true && x.IsRejected == false && x.UserId == userId).Count();
                        int Rejected = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == false && x.Delivered == false && x.AdminStatus == false && x.IsRejected == true && x.UserId == userId).Count();
                        int Delivered = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Where(x => x.IsPulled == true && x.Delivered == true && x.AdminStatus == true && x.IsRejected == false && x.UserId == userId).Count();
                        int Previous = responseOrder.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList().Count();
                        ViewBag.NewOrders = NewOrders.ToString();
                        ViewBag.Approved = Approved.ToString();
                        ViewBag.Rejected = Rejected.ToString();
                        ViewBag.Delivery = Delivered.ToString();
                        ViewBag.Pulled = Pulled.ToString();
                        ViewBag.Previous = Previous.ToString();
                        ViewBag.lastOrder = lastOrder;
                    }
                    else
                    {
                        ViewBag.NewOrders = "0";
                        ViewBag.Approved = "0";
                        ViewBag.Rejected = "0";
                        ViewBag.Delivery = "0";
                        ViewBag.Pulled = "0";
                        ViewBag.Previous = "0";
                    }
                }
                else
                {
                    ViewBag.NewOrders = "0";
                    ViewBag.Approved = "0";
                    ViewBag.Rejected = "0";
                    ViewBag.Delivery = "0";
                    ViewBag.Pulled = "0";
                    ViewBag.Previous = "0";
                }
                SResponse respCart = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + userId, "GET");
                if (respCart.Status && (respCart.Resp != null) && (respCart.Resp != ""))
                {
                    var responseCart = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(respCart.Resp);
                    if (responseCart.Data != null)
                    {
                        httpContextAccessor.HttpContext.Session.SetString("Incart", responseCart.Data.Count().ToString());
                    }
                    else
                    {
                        httpContextAccessor.HttpContext.Session.SetString("Incart", "");
                    }
                }
                else
                {
                    httpContextAccessor.HttpContext.Session.SetString("Incart", "");
                }
                List<Product> responseObject = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null)
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemFetch", "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<Product>> response =
                                        JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                        if (response.Data.Count() > 0)
                        {
                             responseObject = response.Data;
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responseObject));
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }
                }
                else
                {
                    return View(FoundSession_Result);
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        public IActionResult ProductDetailsForCustomers(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
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
        public IActionResult AddCart(CartDetail Obj, int id)
        {
            List<CartDetail> ListCart = new List<CartDetail>();
            try
            {
                //var cart = HttpContext.Session.GetString("cart");
                //if (cart == null)
                //{
                //    var product = ProductDetails(Obj.Id);
                //    CartDetail obj = new CartDetail();
                //    Obj.ProductObj = product;
                //    Obj.Count = 1;
                //    ListCart.Add(Obj);
                //    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
                //}
                //else
                //{
                //    ListCart = JsonConvert.DeserializeObject<List<CartDetail>>(cart);
                //    bool check = true;
                //    for (int i = 1; i < ListCart.Count; i++)
                //    {
                //        if (ListCart[i].ProductObj.Id == Obj.Id)
                //        {
                //            ListCart[i].Count++;
                //            check = false;
                //        }
                //    }
                //    if (check)
                //    {
                //        Obj.ProductObj = ProductDetails(id);
                //        ListCart.Add(Obj);
                //    }
                //    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
                // }

                //  return Ok(ListCart.Count);
                int userId = 0;
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    userId = userDto.Id;
                    Obj.UserId = userId;


                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                var body = JsonConvert.SerializeObject(Obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/AddToCart", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(resp.Resp);
                    if (response.Message == "Cannot Exceed Credit Limit.")
                    {
                        TempData["response"] = "This Item Is Out of Stock";
                        return Json("stock");
                    }
                    TempData["response"] = "Success";
                    return RedirectToAction("GenerateOrder");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("#");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        [HttpGet]
        public IActionResult DeleteItemCart(int? id, string ticketid)
        {
            Product product = new Product();
            try
            {
                //if (!ticketid.Contains("00-"))
                //{
                //    ticketid = "00" + ticketid;
                //}
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/DeleteCartItemById" + "/?id=" + id + "&ticketId=" + ticketid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<CartDetail>>(ress.Resp);
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
        public IActionResult GetStockById(int? id)
        {
            InventoryStock product = new InventoryStock();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/GetStockById" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
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
        public IActionResult RemoveItemFromCart(int? id, string ticketid)
        {
            InventoryStock product = new InventoryStock();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/RemoveItemFromCart" + "/?id=" + id + "&ticketId=" + ticketid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);

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
        public IActionResult AdddItemFromCart(int? id, string ticketid)
        {
            InventoryStock product = new InventoryStock();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/AddItemInCart" + "/?id=" + id + "&ticketId=" + ticketid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);

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
        public IActionResult GetTotalCart(int? id)
        {

            try
            {
                int userId = 0;
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    userId = userDto.Id;

                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                SResponse respCart = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + userId, "GET");
                if (respCart.Status && (respCart.Resp != null) && (respCart.Resp != ""))
                {
                    var responseCart = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(respCart.Resp);

                    if (responseCart.Data != null)
                    {
                        httpContextAccessor.HttpContext.Session.SetString("Incart", responseCart.Data.Count().ToString());
                        return Json(responseCart.Data);
                    }
                    else
                    {
                        httpContextAccessor.HttpContext.Session.SetString("Incart", "");
                        return Json("false");
                    }
                }
                else
                {
                    httpContextAccessor.HttpContext.Session.SetString("Incart", "");
                    return Json("false");

                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }
        private Product ProductDetails(int? id)
        {
            Product product = new Product();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return responseObject;
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return product;
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return null;
            }
        }


        [HttpPost]
        public IActionResult confirmorder(CustomerOrder customer)
        {

            List<CustomerOrder> ListCart = new List<CustomerOrder>();
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    customer.UserId = userDto.Id;
                    customer.BillingAddress = userDto.Address;
                    customer.Phone = userDto.PhoneNumber;
                    if (userDto.Firstname != null && userDto.Lastname != null)
                    {
                        customer.CustomerName = userDto.Firstname + userDto.Lastname;
                    }
                    else if (userDto.Firstname != null)
                    {
                        customer.CustomerName = userDto.Firstname;
                    }
                    else
                    {
                        customer.CustomerName = userDto.UserName;
                    }
                    customer.Email = userDto.Email;
                    customer.City = userDto.City;
                    customer.Zipcode = userDto.ZipCode;
                    customer.Country = userDto.State;
                    customer.AdminStatus = false;
                    customer.Delivered = false;
                    customer.OrderDate = DateTime.Now;
                    customer.IsRejected = false;
                    customer.IsPulled = false;
                    customer.IsInvoiced = false;
                    var body = JsonConvert.SerializeObject(customer);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/SaveCustomerOrder", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerOrder>>(resp.Resp);
                        if (response.Message == "Success." || response.Message == "Success But Email Not Send.")
                        {
                            TempData["response"] = "Order Saved Successfully and placed for approval";
                            return Json("true");
                        }
                        else
                        {
                            return Json("false");
                        }
                    }
                    else
                    {
                        TempData["response"] = resp.Resp + " " + "Unable To generate";
                        return View();
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult ListOfCart(int? id)
        {
            try
            {
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + id, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    return View(response.Data);
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        //public IActionResult ListCart()
        //{
        //    var cart = HttpContext.Session.GetString("cart");
        //    if (cart != null)
        //    {
        //        List<CartDetail> DataCart = JsonConvert.DeserializeObject<List<CartDetail>>(cart);
        //        if (DataCart.Count > 0)
        //        {
        //            ViewBag.CartBag = DataCart.Count;
        //            return View();
        //        }
        //    }
        //    return RedirectToAction("Index");
        //}

        //public IActionResult AddtoCart(Product pro)
        //{
        //    // var currentCart = HttpContext.
        //    try
        //    {

        //        SResponse ress = RequestSender.Instance.CallAPI("api",
        //        "Inventory/ItemGet", "GET");
        //        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
        //        {
        //            ResponseBack<List<Product>> response =
        //                               JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
        //            if (response.Data.Count() > 0)
        //            {
        //                List<Product> ObjProduct = response.Data;
        //                CartDetail ObjCartDetail = null;
        //                List<CartDetail> ObjListCart = new List<CartDetail>();
        //                for (int i = 0; i < ObjProduct.Count; i++)
        //                {
        //                    ObjCartDetail = new CartDetail();
        //                    ObjCartDetail.Name = ObjProduct[i].Name;
        //                    ObjCartDetail.UnitCharge = ObjProduct[i].UnitCharge;
        //                    ObjCartDetail.ImagePath = ObjProduct[i].ItemImageByPath;
        //                    ObjListCart.Add(ObjCartDetail);
        //                }
        //            }
        //            else
        //            {
        //                TempData["response"] = "Invalid Request.";
        //                return null;
        //            }
        //        }
        //        else
        //        {
        //            TempData["response"] = "Server not responding.";
        //            return null;
        //        }
        //        return RedirectToAction();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //public IActionResult CartCheck(CartDetail cartDetail)
        //{
        //    HttpContext.Session.SetString("Cart", "null");
        //    check = HttpContext.Session.GetString("Cart");

        //    string num = "1";
        //    HttpContext.Session.SetString("Count", num);
        //    count = HttpContext.Session.GetString("Count");
        //    try
        //    {
        //        //check = ;
        //        List<CartDetail> li = new List<CartDetail>();
        //        if (HttpContext.Session.GetString("Cart") == "null")
        //        {
        //            li.Add(cartDetail);
        //            check = li.ToString();
        //            ViewBag.cart = li.Count();
        //            HttpContext.Session.SetString("Cart", li.ToString());
        //            var getcount = HttpContext.Session.GetString("Count");
        //            var Json = JsonConvert.SerializeObject(HttpContext.Session.GetString("Cart"));
        //            if (Json != null)
        //            {
        //                List<CartDetail> list = JsonConvert.DeserializeObject<List<CartDetail>>(Json);
        //            }
        //        }
        //        else
        //        {


        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public IActionResult OrderHistory()
        {
            try
            {
                var id = 1;
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserOrderFromCart/" + id, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    return View(response.Data);
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult mail()
        {
            return View();
        }
        public IActionResult GenerateOrder(int? id)
        {
            try
            {
                int userId = 0;
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    userId = userDto.Id;

                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + userId, "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                    return View(response.Data);
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
        public IActionResult GetProducts()
        {
            try
            {

                List<Product> responseObject = new List<Product>();
                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null)
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/ItemFetch", "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);

                        if (response.Data != null)
                        {
                            responseObject = response.Data;
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responseObject));
                            return Json(responseObject);
                        }

                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }
                }
                else
                {
                    return Json(FoundSession_Result.ToList());
                }
              
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        public IActionResult GetCart(int? id)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    id = userDto.Id;
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + id, "GET");
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                        //return View(response.Data);
                        return Json(response.Data);
                    }
                    return View();
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult GetCartInvoiceNo()
        {

            try
            {

                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    int id = userDto.Id;
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartById/" + id, "GET");
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                        if (response.Data != null && response.Data.Count() > 0)
                        {
                            ViewBag.InvoiceNo = response.Data.ElementAt(0).TicketId;
                            ViewBag.username = userDto.UserName;
                            var jsonResult = Json(ViewBag.InvoiceNo + "," + ViewBag.username);
                            return jsonResult;
                        }
                        else
                        {
                            AspNetUser InneruserDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                            SResponse ress = RequestSender.Instance.CallAPI("api",
                            "Customer/GenerateCartInvoiceNo", "GET");
                            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                            {
                                var Innerresponse = JsonConvert.DeserializeObject<ResponseBack<CurrentInvoice>>(ress.Resp);
                                if (Innerresponse.Data != null)
                                {

                                    var InnerresponseObject = Innerresponse.Data;
                                    ViewBag.InvoiceNo = InnerresponseObject.InvoiceNo;
                                    ViewBag.username = InneruserDto.UserName;
                                    var jsonResult = Json(ViewBag.InvoiceNo + "," + ViewBag.username);

                                    return jsonResult;
                                }
                            }
                            else
                            {
                                var jsonResult = Json(ress.Status);
                                return jsonResult;
                            }
                        }
                    }
                    else
                    {
                        AspNetUser InneruserDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                        SResponse ress = RequestSender.Instance.CallAPI("api",
                        "Customer/GenerateCartInvoiceNo", "GET");
                        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                        {
                            var response = JsonConvert.DeserializeObject<ResponseBack<CurrentInvoice>>(ress.Resp);
                            if (response.Data != null)
                            {

                                var responseObject = response.Data;
                                ViewBag.InvoiceNo = responseObject.InvoiceNo;
                                ViewBag.username = InneruserDto.UserName;
                                var jsonResult = Json(ViewBag.InvoiceNo + "," + ViewBag.username);

                                return jsonResult;
                            }
                        }
                        else
                        {
                            var jsonResult = Json(ress.Status);
                            return jsonResult;
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
               
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }



        public IActionResult AddToCartFromMainScreen(int id)
        {
            CartDetail obj = new CartDetail();
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    obj.Id = id;
                    obj.UserId = userDto.Id;
                    var body = JsonConvert.SerializeObject(obj);
                    // var body = sr.Serialize(obj);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/AddToCartFromScreenAPI", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(resp.Resp);
                        if (response.Status == 13)
                        {
                            TempData["ErrorMsg"] = "Quantity Out of Stock";
                            return RedirectToAction("ProductDetailsForCustomers", new { id = obj.Id });
                        }
                        TempData["response"] = "Success";
                        return RedirectToAction("GenerateOrder");
                    }
                    else
                    {
                        TempData["response"] = resp.Resp + " " + "Unable To Updates";
                        return RedirectToAction("#");
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }



        public IActionResult PreviousOrder(int id, CustomerOrder ord)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetUserOrderFromCart" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            double amount = 0;
                            var responseObject = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).ToList();
                            for (int i = 0; i < responseObject.Count(); i++)
                            {
                                if (responseObject[i].AdminStatus == false && responseObject[i].IsPulled == false && responseObject[i].IsInvoiced == false && responseObject[i].Delivered == false && responseObject[i].IsRejected == false)
                                {
                                    responseObject[i].OrignalStatus = "Pending";
                                }
                                else if (responseObject[i].AdminStatus == true && responseObject[i].IsPulled == false && responseObject[i].IsInvoiced == false && responseObject[i].Delivered == false && responseObject[i].IsRejected == false)
                                {
                                    responseObject[i].OrignalStatus = "Approved";
                                }
                                else if (responseObject[i].AdminStatus == true && responseObject[i].IsPulled == true && responseObject[i].IsInvoiced == false && responseObject[i].Delivered == false && responseObject[i].IsRejected == false)
                                {
                                    responseObject[i].OrignalStatus = "Package Preparing";
                                }
                                else if (responseObject[i].AdminStatus == true && responseObject[i].IsPulled == true && responseObject[i].IsInvoiced == true && responseObject[i].Delivered == false && responseObject[i].IsRejected == false)
                                {
                                    responseObject[i].OrignalStatus = "Ready To Shipment";
                                }
                                else if (responseObject[i].AdminStatus == true && responseObject[i].IsPulled == true && responseObject[i].IsInvoiced == true && responseObject[i].Delivered == true && responseObject[i].IsRejected == false)
                                {
                                    responseObject[i].OrignalStatus = "Delivered";
                                }
                                else if (responseObject[i].AdminStatus == false && responseObject[i].IsPulled == false && responseObject[i].IsInvoiced == false && responseObject[i].Delivered == false && responseObject[i].IsRejected == true)
                                {
                                    responseObject[i].OrignalStatus = "OnHold";
                                }
                                else
                                {
                                    responseObject[i].OrignalStatus = "Invalid";
                                }
                            }
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Order Exists.";
                            List<CustomerOrder> obj2 = new List<CustomerOrder>();
                            return View(obj2);
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                List<CustomerOrder> obj = new List<CustomerOrder>();
                return View(obj);
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
                // ViewBag.Logo = Url.Content("~/images/PendingOrder.png");
                //id = 1;
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customer/GetUserCartByAdminApproval" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        if (type == "Pending")
                        {
                            //https://204.2.194.223/abcdweb/images/PendingOrder.png
                            response.Data.ElementAt(0).ImageURL = "https://204.2.194.223/abcdweb/images/PendingOrder.png";
                            response.Data.ElementAt(0).Type = "Pending Orders";
                        }
                        else if (type == "Previous")
                        {
                            response.Data.ElementAt(0).ImageURL = "https://204.2.194.223/abcdweb/images/DeliveredOrder.png";
                            response.Data.ElementAt(0).Type = "Order Detail";
                        }
                        else if (type == "Rejected")
                        {
                            response.Data.ElementAt(0).ImageURL = "https://204.2.194.223/abcdweb/images/order-cancel.png";
                            response.Data.ElementAt(0).Type = "OnHold Orders";
                        }
                        else if (type == "Closed")
                        {
                            response.Data.ElementAt(0).ImageURL = "https://204.2.194.223/abcdweb/images/ClosedOrder.png";
                            response.Data.ElementAt(0).Type = "Closed Orders";
                        }

                        else if (type == "InProcess")
                        {
                            response.Data.ElementAt(0).ImageURL = "https://204.2.194.223/abcdweb/images/ClosedOrder.png";
                            response.Data.ElementAt(0).Type = "InProcess Orders";
                        }
                        var responseObject = response.Data.ToList();
                        if (responseObject.ElementAt(0).CustomerOrders == null)
                        {
                            return RedirectToAction("NoDetail");
                        }
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

        public IActionResult NoDetail()
        {

            return View();

        }
        public IActionResult PendingOrder(int id, CustomerOrder ord)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetUserOrderFromCart" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.Delivered == false && x.AdminStatus == false && x.IsPulled == false && x.IsRejected == false).ToList();
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Order Exists.";
                            List<CustomerOrder> obj2 = new List<CustomerOrder>();
                            return View(obj2);
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                List<CustomerOrder> obj = new List<CustomerOrder>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult InProcessOrder(int id, CustomerOrder ord)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetUserOrderFromCart" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.AdminStatus == true && x.IsRejected == false && x.Delivered == false).ToList();
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Order Exists.";
                            List<CustomerOrder> obj2 = new List<CustomerOrder>();
                            return View(obj2);
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                List<CustomerOrder> obj = new List<CustomerOrder>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult ClosedOrder(int id, CustomerOrder ord)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetUserOrderFromCart" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.Delivered == true && x.AdminStatus == true && x.IsPulled == true && x.IsRejected == false).ToList();
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Order Exists.";
                            List<CustomerOrder> obj2 = new List<CustomerOrder>();
                            return View(obj2);
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                List<CustomerOrder> obj = new List<CustomerOrder>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult RejectedOrder(int id, CustomerOrder ord)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetUserOrderFromCart" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);
                        if (response.Data != null)
                        {
                            var responseObject = response.Data.ToList().GroupBy(x => x.TicketId).Select(i => i.FirstOrDefault()).Where(x => x.Delivered == false && x.AdminStatus == false && x.IsPulled == false && x.IsRejected == true).ToList();
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Order Exists.";
                            List<CustomerOrder> obj2 = new List<CustomerOrder>();
                            return View(obj2);
                        }
                    }
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                List<CustomerOrder> obj = new List<CustomerOrder>();
                return View(obj);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        public IActionResult GetCustomerinfo(int id)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customer/GetCustomerinfoo" + "/" + userDto.Id, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<CustomerInformation>>(ress.Resp);
                        if (response.Data != null)
                        {

                            var responseObject = response.Data;
                            return View(responseObject);
                        }
                        else
                        {
                            TempData["response"] = "No Customer Detail Exists.";
                            List<CustomerInformation> obj2 = new List<CustomerInformation>();
                            return View(obj2);
                        }
                    }
                    else
                    {
                        TempData["response"] = "Session Expired";
                        return RedirectToAction("Login", "Account");
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
        public IActionResult UpdateCustomerinfo(CustomerInformation obj)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    var body = JsonConvert.SerializeObject(obj);

                    SResponse ress = RequestSender.Instance.CallAPI("api",
                 "Customer/UpdateCustomerinfoo/" + userDto.Id, "POST", body);
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        var response = JsonConvert.SerializeObject(userDto);
                        if (response != null)
                        {
                            TempData["response"] = "Profile Updated Successfully!.";
                            return RedirectToAction("GetCustomerinfo", "CustomerHomePage");
                        }
                        else
                        {
                            TempData["response"] = "No Customer Detail Exists.";

                        }
                    }

                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }
                return RedirectToAction("GetCustomerinfo");
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }

        }


        [HttpGet]
        public IActionResult AddCustomQuantityInCart(int? id, string ticketid, string quantity)
        {
            InventoryStock product = new InventoryStock();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/AddQuantityInCart" + "/?id=" + id + "&ticketId=" + ticketid + "&quantity=" + quantity, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);
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
        public IActionResult GetOrderItems(int? id, string ticketId)
        {
            try
            {
                string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
                if (!string.IsNullOrEmpty(userData))
                {
                    AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                    id = userDto.Id;
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetUserCartItems" + "/?id=" + id + "&ticketID=" + ticketId, "GET");
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<List<CartDetail>>>(resp.Resp);
                        if (response.Message == "Record Not Found.")
                        {
                            return Json("NotFound");
                        }
                        //return View(response.Data);
                        return Json("true");

                    }
                    return View();
                }
                else
                {
                    TempData["response"] = "Session Expired";
                    return RedirectToAction("Login", "Account");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }


        [HttpPost]
        public IActionResult ApproveItemCustomQty(int? id, int? productId, string ticketid, string plateNum, string licenseNo, IFormFile scannedDocument)
        {
            string userData = httpContextAccessor.HttpContext.Session.GetString("userobj");
            if (!string.IsNullOrEmpty(userData))
            {
                AspNetUser userDto = JsonConvert.DeserializeObject<AspNetUser>(userData);
                id = userDto.Id;
                try
                {
                    AutherizeOrderLimit autherizeOrderLimit = new AutherizeOrderLimit();
                    if (plateNum != null)
                    {
                        autherizeOrderLimit.PlateNumber = plateNum;
                    }
                    else
                    {
                        autherizeOrderLimit.PlateNumber = "NA";
                    }
                    if (licenseNo != null)
                    {
                        autherizeOrderLimit.DrivingLicenseNumber = licenseNo;
                    }
                    else
                    {
                        autherizeOrderLimit.DrivingLicenseNumber = "NA";
                    }
                    if (ticketid != null)
                    {
                        autherizeOrderLimit.TicketId = ticketid;
                    }
                    else
                    {
                        autherizeOrderLimit.TicketId = "NA";
                    }

                    autherizeOrderLimit.UserId = id;
                    autherizeOrderLimit.ProductId = productId;

                    if (scannedDocument != null)
                    {
                        var input = scannedDocument.OpenReadStream();
                        byte[] byteData = null, buffer = new byte[input.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            }
                            byteData = ms.ToArray();
                            autherizeOrderLimit.UploadFile = byteData;
                        }

                    }

                    var body = JsonConvert.SerializeObject(autherizeOrderLimit);
                    SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/AuthorizeQuantityInCart", "POST", body);
                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                    {
                        var response = JsonConvert.DeserializeObject<ResponseBack<AutherizeOrderLimit>>(resp.Resp);
                        if (response != null)
                        {
                            return Json("true");
                        }
                        else
                        {
                            return Json("false");
                        }
                    }
                    else
                    {
                        TempData["response"] = resp.Resp + " " + "Unable To generate";
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    TempData["response"] = ex.Message + "Error Occured.";
                    return View();
                }
            }
            else
            {
                TempData["response"] = "Session Expired";
                return RedirectToAction("Login", "Account");
            }
        }


        [HttpGet]
        public IActionResult RemoveCurrentCart(string ticketid)
        {
            InventoryStock product = new InventoryStock();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/EmptyCurrentCart" + "/?ticketId=" + ticketid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    var response = JsonConvert.DeserializeObject<ResponseBack<InventoryStock>>(ress.Resp);
                    if (response.Message == "Record Not Found.")
                    {
                        return Json("false");
                    }
                    else
                    {
                        return Json("true");
                    }
                }
                else
                {
                    TempData["response"] = ress.Resp + " " + "Unable To Updates";
                    return RedirectToAction("#");
                }

            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult ProductDetailbyid(int? id)
        {
            Product product = new Product();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/ItemGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }

                    else
                    {
                        TempData["response"] = "Server is down.";
                    }
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return null;
            }
        }

       
        public IActionResult CurrentOrderByTicketId(string TicketId)
        {
           
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customer/GetCurrentOrderByTicketId" + "/" + TicketId, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<CustomerOrder>>>(ress.Resp);

                    if (response.Data != null)
                    {
                        var responseObject = response.Data.ToList();
                        ViewBag.LineCount = responseObject.Count().ToString();
                        ViewBag.TotalCost = responseObject[0].OrderAmount;
                        ViewBag.InvoiceNo = responseObject[0].TicketId;
                        return View(responseObject);

                    }

                    else
                    {
                        TempData["response"] = "No record Found.";
                    }
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return null;
            }
        }


        [HttpGet]
        public IActionResult GetitemCategory()
        {
            try
            {

                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetitemCategories", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(resp.Resp);
                    return Json(response.Data);

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
        public IActionResult GetproductByCategoryID(string name)
        {
            try
            {
                //"Customer/GetProductsByCategory?name=" + name + "&id=" + id, "GET");
                //"Customer/GetproductByCatID" + "/?id=" + id
                SResponse resp = RequestSender.Instance.CallAPI("api", "Customer/GetProductsByCategory?name=" + @HttpUtility.UrlEncodeUnicode(name) + "&id=" + "0", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(resp.Resp);
                    return Json(response.Data);

                }
                return Json("false");

            }
            catch (Exception ex)
            {
                TempData["response"] = ex.Message + "Error Occured.";
                return View();
            }
        }



        public IActionResult CustomerProductsByKeyword(string name, int pageNo = 1)
        {
            int totalPage = 0;
            int totalRecord = 0, pageSize;
            pageSize = 6;
            List<ItemCategory> ResItemCategroy = new List<ItemCategory>();
            List<ItemSubCategory> itemSubCategoriesList = new List<ItemSubCategory>();
            List<Product> responseObject = new List<Product>();

            try
            {

                var FoundSession = HttpContext.Session.GetString("loadedProducts");
                List<Product> FoundSession_Result = new List<Product>();
                if (FoundSession != null)
                {
                    FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
                }
                if (FoundSession_Result.Count() > 0)
                {
                    totalRecord = FoundSession_Result.ToList().Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList().Count();
                    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    if (totalRecord > 1 && totalPage == 0)
                    {
                        totalPage = 1;
                    }
                    var record = (from a in FoundSession_Result.ToList().Where(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                                  select a).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    responseObject.AddRange(record);
                }
                else
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetProductsByKeyword?name=" + name, "GET");
                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<Product>> response =
                                       JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                        if (response.Data.Count() > 0)
                        {
                            totalRecord = response.Data.Count();
                            totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                            if (totalRecord > 1 && totalPage == 0)
                            {
                                totalPage = 1;
                            }
                            var record = (from a in response.Data
                                          select a).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                            responseObject = record;
                            HttpContext.Session.SetString("loadedProductsByKeyName", JsonConvert.SerializeObject(response.Data));

                        }
                    }
                }


                SResponse ItemCategoryGet = RequestSender.Instance.CallAPI("api",
                "Security/OpenItemCategoryGet", "GET");
                if (ItemCategoryGet.Status && (ItemCategoryGet.Resp != null) && (ItemCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> rescat =
                        JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                    if (rescat.Data.Count() > 0)
                    {

                        ViewBag.ItemCategory = rescat.Data;
                    }

                }
                SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
                "Security/OpenItemSubCategoryGet", "GET");
                if (ItemSubCategoryGet.Status && (ItemSubCategoryGet.Resp != null) && (ItemSubCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> ResSubCategory =
                                     JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ItemSubCategoryGet.Resp);

                    if (ResSubCategory.Data.Count() > 0)
                    {
                        ViewBag.ItemsubCategory = ResSubCategory.Data;
                    }
                }


                ViewBag.TotalPages = totalPage;
                if (totalPage == 0)
                {
                    ViewBag.MinPages = 0;
                    ViewBag.MaxPages = 0;
                }
                else if (totalPage == 1)
                {
                    ViewBag.MinPages = 0;
                    ViewBag.MaxPages = 0;
                }
                else
                {
                    ViewBag.MinPages = pageNo - 1;
                    //double Checkk = Convert.ToDouble((Convert.ToDouble(totalRecord) / 6));
                    //ViewBag.MaxPages = Math.Ceiling(Checkk);
                    double Checkk = Convert.ToDouble((Convert.ToDouble(totalRecord) / 6));
                    double CCC = Math.Ceiling(Checkk);

                    if ((pageNo + 6) < CCC)
                    {
                        ViewBag.MaxPages = pageNo + 6;
                    }
                    else
                    {
                        var max = CCC - pageNo;
                        ViewBag.MaxPages = pageNo + max;
                    }
                }

                ViewBag.KeyName = name;

                return View(responseObject);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
