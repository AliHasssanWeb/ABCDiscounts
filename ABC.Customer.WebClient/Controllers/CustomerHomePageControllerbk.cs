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
//using System.Threading.Tasks;
//using static ABC.Customer.Domain.DataConfig.RequestSender;

//namespace ABC.Customer.WebClient.Controllers
//{

//    public class CustomerHomePageControllerbk : Controller
//    {

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

//                        if (response.Data != null)
//                        {
//                            var responseObject = response.Data;
//                            return View(responseObject);
//                        }

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
//        public IActionResult AddtoCart(Product pro)
//        {
//           // var currentCart = HttpContext.
//            try
//            {

//                SResponse ress = RequestSender.Instance.CallAPI("api",
//                "Inventory/ItemGet", "GET");
//                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
//                {
//                    ResponseBack<List<Product>> response =
//                                       JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
//                    if (response.Data.Count() > 0)
//                    {
//                        List<Product> ObjProduct = response.Data;
//                        CartDetail ObjCartDetail = null;
//                        List<CartDetail> ObjListCart = new List<CartDetail>();
//                        for (int i = 0; i < ObjProduct.Count; i++)
//                        {
//                            ObjCartDetail = new CartDetail();
//                            ObjCartDetail.Name = ObjProduct[i].Name;
//                            ObjCartDetail.retail = ObjProduct[i].Retail;
//                            ObjCartDetail.ImagePath = ObjProduct[i].ItemImageByPath;
//                            ObjListCart.Add(ObjCartDetail);
//                        }
//                    }
//                    else
//                    {
//                        TempData["response"] = "Invalid Request.";
//                        return null;
//                    }
//                }
//                else
//                {
//                    TempData["response"] = "Server not responding.";
//                    return null;
//                }
//                return RedirectToAction();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//        }

//    }
//}
