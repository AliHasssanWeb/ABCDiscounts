//using ABC.Customer.Domain.Configuration;
//using ABC.Customer.Domain.DataConfig;
//using ABC.EFCore.Repository.Edmx;
//using ABC.Shared.DataConfig;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using Newtonsoft.Json;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Threading.Tasks;
//using static ABC.Customer.Domain.DataConfig.RequestSender;

//namespace ABC.Customer.WebClient.Controllers
//{
//    public class CustomerHomePageControllerbk1 : Controller
//    {

//        string check = "";
//        string count = "";
//        public IActionResult Index()
//        {
//            try
//            {
//                SResponse ress = RequestSender.Instance.CallAPI("api",
//                  "Inventory/ItemGet", "GET");
//                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//                {
//                    ResponseBack<List<Product>> response =
//                                    JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
//                    if (response.Data.Count() > 0)
//                    {
//                        List<Product> responseObject = response.Data;
//                        ViewBag.data = responseObject;
//                        return View(responseObject);
//                    }
//                    else
//                    {
//                        TempData["response"] = "Server is down.";
//                    }
//                }

//                return View();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        public IActionResult ProductDetailsForCustomers(int id)
//        {
//            try
//            {
//                SResponse ress = RequestSender.Instance.CallAPI("api",
//             "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
//                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//                {
//                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);

//                    if (response.Data != null)
//                    {
//                        var responseObject = response.Data;
//                        return View(responseObject);
//                    }

//                    else
//                    {
//                        TempData["response"] = "Server is down.";
//                    }
//                }
//                return View();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        public IActionResult AddCart(CartDetail Obj, int id, string retail, string total)
//        {
//            List<CartDetail> ListCart = new List<CartDetail>();
//            try
//            {
//                var cart = HttpContext.Session.GetString("cart");
//                if (cart == null)
//                {
//                    var product = ProductDetails(Obj.Id);
//                    CartDetail obj = new CartDetail();
//                    Obj.ProductObj = product;
//                    obj.retail = retail;
//                    obj.total = total;
//                    Obj.Count = 1;
//                    ListCart.Add(Obj);
//                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
//                }
//                else
//                {
//                    ListCart = JsonConvert.DeserializeObject<List<CartDetail>>(cart);
//                    bool check = true;
//                    for (int i = 1; i < ListCart.Count; i++)
//                    {
//                        if (ListCart[i].ProductObj.Id == Obj.Id)
//                        {
//                            ListCart[i].Count++;
//                            check = false;
//                        }
//                    }
//                    if (check)
//                    {

//                        Obj.ProductObj = ProductDetails(id);
//                        //Obj.Count = 1;
//                        ListCart.Add(Obj);
//                        //ListCart.Add(new CartDetail
//                        //{
//                        //    ProductObj = ProductDetails,

//                        //});
//                        //ListCart.Add(Obj);



//                        //Product = 

//                    }
//                    HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(ListCart));
//                }

//                return Ok(ListCart.Count);
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        private Product ProductDetails(int id)
//        {
//            Product product = new Product();
//            try
//            {
//                SResponse ress = RequestSender.Instance.CallAPI("api",
//             "Inventory/ItemGetByIDWithStock" + "/" + id, "GET");
//                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//                {
//                    var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);

//                    if (response.Data != null)
//                    {
//                        var responseObject = response.Data;
//                        return responseObject;
//                    }

//                    else
//                    {
//                        TempData["response"] = "Server is down.";
//                    }
//                }
//                return product;
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }
//        public IActionResult CartOutNew()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult confirmorder(string name, string email, string phone, string street, string street2, string state_id, string country_id, string zip, string city)
//        {
//            List<CartDetail> ListCart = new List<CartDetail>();
//            Order neworder = null;
//            List<Order> FinalOrder = new List<Order>();
//            var allfoundrecord = HttpContext.Session.GetString("cart");
//            var ff = JsonConvert.DeserializeObject<List<CartDetail>>(allfoundrecord);
//            int check = ListCart.Count;
//            for (int i = 0; i < ff.Count(); i++)
//            {

//                ListCart.Add(ff[i]);
//            }

//            for (int a = 0; a < ListCart.Count(); a++)
//            {
//                neworder = new Order();
//                if (a == 0)
//                {
//                    neworder.customeraddress = new CustomerAddress();
//                    neworder.customeraddress.Name = name;
//                }
//                neworder.ProductId = ListCart[a].ProductObj.Id;
//                neworder.ProductName = ListCart[a].ProductObj.Name;
//                neworder.Quantity = ListCart[a].Quantity.ToString();

//                FinalOrder.Add(neworder);
//            }

//            var body = JsonConvert.SerializeObject(FinalOrder);
//            //api
//           // SResponse ress = RequestSender.Instance.CallAPI("api",
//           //"OrderManagement/AddOrder", "POST", body);
//           // if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//           // {
//           // }
//            return Ok();
//        }
//        public IActionResult ListOfCart()
//        {
//            var cart = HttpContext.Session.GetString("cart");
//            try
//            {
//                return View(JsonConvert.DeserializeObject<List<CartDetail>>(cart));
//            }
//            catch (Exception ex)
//            {
//                throw;
//            }
//        }
//        public IActionResult sendmail()
//        {
//            if (ModelState.IsValid)
//            {

//                var senderEmail = new MailAddress("shah@studentfintech.com", "studentfintech.app");

//                var recievermaill = new MailAddress("munirahmad20786@gmail.com", "Reciever");

//                var pass = "Usman123@@!!";

//                var sub = "Order Place Successfully";
//                var body = "Hi customer ! Your Order has been placed succefully!!   © 2022 abcDiscount.app | Alright Reserved.";
//                var smtp = new SmtpClient
//                {
//                    Host = "smtp.gmail.com",
//                    Port = 587,
//                    EnableSsl = true,
//                    DeliveryMethod = SmtpDeliveryMethod.Network,
//                    UseDefaultCredentials = false,
//                    Credentials = new NetworkCredential(senderEmail.Address, pass)





//                };
//                using (var message = new MailMessage(senderEmail, recievermaill)

//                {
//                    Subject = sub,
//                    Body = body

//                }
//                    )

//                {
//                    smtp.Send(message);
//                }


//            }
//            return Ok();
//        }
//        //public IActionResult ListCart()
//        //{
//        //    var cart = HttpContext.Session.GetString("cart");
//        //    if (cart != null)
//        //    {
//        //        List<CartDetail> DataCart = JsonConvert.DeserializeObject<List<CartDetail>>(cart);
//        //        if (DataCart.Count > 0)
//        //        {
//        //            ViewBag.CartBag = DataCart.Count;
//        //            return View();
//        //        }
//        //    }
//        //    return RedirectToAction("Index");
//        //}

//        //public IActionResult AddtoCart(Product pro)
//        //{
//        //    // var currentCart = HttpContext.
//        //    try
//        //    {

//        //        SResponse ress = RequestSender.Instance.CallAPI("api",
//        //        "Inventory/ItemGet", "GET");
//        //        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//        //        {
//        //            ResponseBack<List<Product>> response =
//        //                               JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
//        //            if (response.Data.Count() > 0)
//        //            {
//        //                List<Product> ObjProduct = response.Data;
//        //                CartDetail ObjCartDetail = null;
//        //                List<CartDetail> ObjListCart = new List<CartDetail>();
//        //                for (int i = 0; i < ObjProduct.Count; i++)
//        //                {
//        //                    ObjCartDetail = new CartDetail();
//        //                    ObjCartDetail.Name = ObjProduct[i].Name;
//        //                    ObjCartDetail.UnitCharge = ObjProduct[i].UnitCharge;
//        //                    ObjCartDetail.ImagePath = ObjProduct[i].ItemImageByPath;
//        //                    ObjListCart.Add(ObjCartDetail);
//        //                }
//        //            }
//        //            else
//        //            {
//        //                TempData["response"] = "Invalid Request.";
//        //                return null;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            TempData["response"] = "Server not responding.";
//        //            return null;
//        //        }
//        //        return RedirectToAction();
//        //    }
//        //    catch (Exception)
//        //    {
//        //        throw;
//        //    }
//        //}
//        //public IActionResult CartCheck(CartDetail cartDetail)
//        //{
//        //    HttpContext.Session.SetString("Cart", "null");
//        //    check = HttpContext.Session.GetString("Cart");

//        //    string num = "1";
//        //    HttpContext.Session.SetString("Count", num);
//        //    count = HttpContext.Session.GetString("Count");
//        //    try
//        //    {
//        //        //check = ;
//        //        List<CartDetail> li = new List<CartDetail>();
//        //        if (HttpContext.Session.GetString("Cart") == "null")
//        //        {
//        //            li.Add(cartDetail);
//        //            check = li.ToString();
//        //            ViewBag.cart = li.Count();
//        //            HttpContext.Session.SetString("Cart", li.ToString());
//        //            var getcount = HttpContext.Session.GetString("Count");
//        //            var Json = JsonConvert.SerializeObject(HttpContext.Session.GetString("Cart"));
//        //            if (Json != null)
//        //            {
//        //                List<CartDetail> list = JsonConvert.DeserializeObject<List<CartDetail>>(Json);
//        //            }
//        //        }
//        //        else
//        //        {


//        //        }
//        //        return RedirectToAction("Index", "Home");
//        //    }
//        //    catch (Exception)
//        //    {
//        //        throw;
//        //    }
//        //}
//    }
//}
