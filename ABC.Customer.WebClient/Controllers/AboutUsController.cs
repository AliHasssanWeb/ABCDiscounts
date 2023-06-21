using ABC.Customer.Domain.DataConfig;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Customer.Domain.DataConfig.RequestSender;

namespace ABC.Customer.WebClient.Controllers
{
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            List<ItemCategory> ResItemCategroy = new List<ItemCategory>();
            List<ItemSubCategory> itemSubCategoriesList = new List<ItemSubCategory>();
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "Inventory/ItemGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Product>> response =
                                   JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Product> responseObject = response.Data;

                        SResponse ItemCategoryGet = RequestSender.Instance.CallAPI("api",
              "Inventory/ItemCategoryGet", "GET");
                        if (ItemCategoryGet.Status && (ItemCategoryGet.Resp != null) && (ItemCategoryGet.Resp != ""))
                        {
                            ResponseBack<List<ItemCategory>> rescat =
                                JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                            if (rescat.Data.Count() > 0)
                            {

                                ViewBag.ItemCategory = rescat.Data;
                            }
                            //ResponseBack<List<ItemCategory>> ResItemCategroy =
                            //             JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                            //if (ResItemCategroy.Data.Count() > 0)
                            //{
                            //    ViewBag.ItemCategory = ResItemCategroy.Data;
                            //}
                        }
                        SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
              "Inventory/ItemSubCategoryGet", "GET");
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
                        }
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
                throw ex;
            }
        }
       
    }
}
