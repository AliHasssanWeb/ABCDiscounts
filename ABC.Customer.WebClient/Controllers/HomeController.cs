using ABC.Customer.Domain.Configuration;
using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int pageNo = 1)
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
                if (FoundSession_Result.Count() < 1)
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Security/OpenItemFetch", "GET");
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
                            HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(response.Data));

                            // GlobalAccess.loadedProducts.AddRange(responseObject);
                        }
                    }
                }
                else
                {
                    //for (int i = 0; i < GlobalAccess.loadedProducts.Count(); i++)
                    //{
                    //    responseObject.Add(GlobalAccess.loadedProducts[i]);
                    //}
                    totalRecord = FoundSession_Result.Count();
                    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    if (totalRecord > 1 && totalPage == 0)
                    {
                        totalPage = 1;
                    }
                    var record = (from a in FoundSession_Result
                                  select a).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    responseObject.AddRange(record);
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
                    else
                    {
                        List<ItemCategory> responseObject1 = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
                    }
                    //ResponseBack<List<ItemCategory>> ResItemCategroy =
                    //             JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                    //if (ResItemCategroy.Data.Count() > 0)
                    //{
                    //    ViewBag.ItemCategory = ResItemCategroy.Data;
                    //}
                }
                else
                {
                    List<ItemCategory> responseObject1 = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
                }
                SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
                "Security/OpenItemSubCategoryGet", "GET");
                if (ItemSubCategoryGet.Status && (ItemSubCategoryGet.Resp != null) && (ItemSubCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> ResSubCategory =
                                     JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ItemSubCategoryGet.Resp);

                    if (ResSubCategory.Data.Count() > 0)
                    {
                        //List<ItemSubCategory> rep = ResSubCategory.Data;
                        //for (int i = 0; i < rep.Count(); i++)
                        //{
                        //    ItemSubCategory a = new ItemSubCategory();
                        //    a = rep.Where(x => x.CategoryId == ResItemCategroy[i].Id).FirstOrDefault();
                        //    itemSubCategoriesList.Add(a);
                        //}
                        //List<ItemSubCategory> ResSubCategoryObj = ResSubCategory;
                        ViewBag.ItemsubCategory = ResSubCategory.Data;
                    }
                    else
                    {
                        List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                        ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
                    }
                }
                else
                {
                    List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                    ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
                }

                // ViewBag.Totalpages = Math.Ceiling(responseObject.ToList().Count() / 6.0);
                // int page = int.Parse(Page == null ? "1" : Page);

                // ViewBag.page = page;
                //var responseObjectLast = responseObject.ToList().Skip((page - 1) * 6).Take(6);
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
                    ViewBag.MaxPages = pageNo + 6;
                }
                return View(responseObject);

                //else
                //{
                //    TempData["response"] = "Server is down.";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
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

        public IActionResult PreviousOrder()
        {
            return View();
        }

        public IActionResult ProductDetail(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Security/OpenItemGetByIDWithStock" + "/" + id, "GET");
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
            catch (Exception)
            {
                throw;
            }
        }
        //public JsonResult GetEmployerJson()
        //{
        //    try
        //    {
        //        SResponse ress = RequestSender.Instance.CallAPI("api",
        //         "Inventory/ItemGet", "GET");
        //        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
        //        {
        //            var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
        //            if (response.Data != null)
        //            {
        //                var responseObject = response.Data;
        //                return Json(responseObject);
        //            }
        //            else
        //            {
        //                TempData["response"] = "Unable to get details of Items.";
        //                return Json(JsonConvert.DeserializeObject("false."));
        //            }
        //        }
        //        return Json(JsonConvert.DeserializeObject("false."));
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(JsonConvert.DeserializeObject("false." + ex.Message));
        //    }
        //}

        public IActionResult GetBrands()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
             "Customer/GetBrandsAll" , "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<Brand>>>(ress.Resp);
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
                return Json(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ProductsByBrand(int id , string name , int pageNo = 1)
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
                    totalRecord = FoundSession_Result.ToList().Where(x => x.BrandId == id || x.BrandName == name).Count();
                    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    if (totalRecord > 1 && totalPage == 0)
                    {
                        totalPage = 1;
                    }
                    var record = (from a in FoundSession_Result.ToList().Where(x => x.BrandId == id || x.BrandName == name)
                                  select a).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    responseObject.AddRange(record);
                }
                else
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetProductsByBrandId?Id=" + id + "&name=" + name, "GET");
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
                    else
                    {
                        List<ItemCategory> responseObject1 = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
                    }
                   
                }
                else
                {
                    List<ItemCategory> responseObject1 = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
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
                    else
                    {
                        List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                        ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
                    }
                }
                else
                {
                    List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                    ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
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
                    // ViewBag.MaxPages = (pageNo - 1) + 6;
                    //int CheckPage = (pageNo - 1);
                    //if (CheckPage < (totalRecord - 6))
                    //{
                    //    ViewBag.MaxPages = CheckPage;
                    //}
                    //else
                    //{
                    //    ViewBag.MaxPages = (totalRecord / 6);
                    //}

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
                ViewBag.BrandName = name;
                ViewBag.BrandID = id;
                return View(responseObject);

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProductsByKeyword(string name, int pageNo = 1)
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
                if (FoundSession_Result != null && FoundSession_Result.Count() > 0)
                {
                    totalRecord = FoundSession_Result.ToList().Where(x=>x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase)).ToList().Count();
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
                    else
                    {
                        List<ItemCategory> responseObject1 = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
                    }

                }
                else
                {
                    List<ItemCategory> responseObject1 = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(responseObject1.ToList());
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
                    else
                    {
                        List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                        ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
                    }
                }
                else
                {
                    List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                    ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList());
                }


                ViewBag.TotalPages = totalPage;
                if(totalPage == 0)
                {
                    ViewBag.MinPages = 0;
                    ViewBag.MaxPages = 0;
                }else if(totalPage == 1)
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


        public IActionResult ProductsByCategory(string name, int id, int pageNo = 1)
        {
            int totalPage = 0;
            int totalRecord = 0, pageSize;
            pageSize = 6;
            name = name.Trim();
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
                if (FoundSession_Result != null && FoundSession_Result.Count() > 0)
                {
                    totalRecord = FoundSession_Result.ToList().Where(x => x.CategoryName == name || x.ItemCategoryId == id).ToList().Count();
                    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                    if (totalRecord > 1 && totalPage == 0)
                    {
                        totalPage = 1;
                    }
                    var record = (from a in FoundSession_Result.ToList().Where(x => x.CategoryName == name || x.ItemCategoryId == id)
                                  select a).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    responseObject.AddRange(record);
                }
                else
                {
                    SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Customer/GetProductsByCategory?name=" + name + "&id=" + id, "GET");
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
                            HttpContext.Session.SetString("loadedProductsByCat", JsonConvert.SerializeObject(response.Data));

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
                    else
                    {
                        List<ItemCategory> rescatobj = new List<ItemCategory>();
                        ViewBag.ItemCategory = rescatobj;
                    }

                }
                else
                {
                    List<ItemCategory> rescat = new List<ItemCategory>();
                    ViewBag.ItemCategory = rescat;
                }
                SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
                //"Security/OpenItemSubCategoryGetById?id=" + id , "GET");
                "Security/OpenItemSubCategoryGet", "GET");
                if (ItemSubCategoryGet.Status && (ItemSubCategoryGet.Resp != null) && (ItemSubCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> ResSubCategory =
                                     JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ItemSubCategoryGet.Resp);

                    if (ResSubCategory.Data.Count() > 0)
                    {

                        List<ItemSubCategory> responseObject1 = ResSubCategory.Data;
                        ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList(), "Id", "SubCategory");

                    }
                    else
                    {
                        List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                        ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList(), "Id", "SubCategory");
                    }
                }
                else
                {
                    List<ItemSubCategory> responseObject1 = new List<ItemSubCategory>();
                    ViewBag.ItemsubCategory = new SelectList(responseObject1.ToList(), "Id", "SubCategory");
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
                    double Checkk = Convert.ToDouble((Convert.ToDouble(totalRecord) / 6));
                    double CCC = Math.Ceiling(Checkk);

                    if((pageNo + 6) < CCC)
                    {
                        ViewBag.MaxPages = pageNo + 6;
                    }
                    else
                    {
                        var max = CCC - pageNo;
                        ViewBag.MaxPages = pageNo + max;
                    }
                }
                ViewBag.CatName = name;
                ViewBag.CatID = id;
                return View(responseObject);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ProductsBySubCatName(string name)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "Customer/GetProductsBySubCatName" + "/" + name , "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
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
                return Json(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
