using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    public class ConfigurationsController : Controller
    {
        public IActionResult Brand()
        {
            return View();
        }
        // Brand start
        [HttpPost]
        public IActionResult AddBrand(Brand obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/BrandCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Add Successfully";
                    return RedirectToAction("ManageBrand");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddBrand");
                }
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageBrand");
            }

        }
        //[HttpPut("UpdateBrand")]
        [HttpPost]
        public IActionResult UpdateBrand(int id, Brand obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Brand/UpdateBrand" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    return RedirectToAction("ManageBrand");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageBrand");
                }
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageBrand");
            }

        }

        [HttpGet]
        public IActionResult UpdateBrand(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Brand/BrandGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Brand>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                        return RedirectToAction("ManageBrand");
                    }
                }
                return RedirectToAction("ManageBrand");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageBrand");
            }

        }



        public IActionResult DeleteBrands(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Configuration/DeleteBrand" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageBrand");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageBrand");
                    }
                }
                return RedirectToAction("ManageBrand");
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageBrand");
            }
        }
        public IActionResult ManageBrand()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Configuration/BrandGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Brand>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Brand>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Brand> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Brand List Found. Please Enter Brand First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }
        //Brand End

        public IActionResult ItemCategory()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ItemCategory(ItemCategory itemCategory)
        {

            try
            {
                var body = JsonConvert.SerializeObject(itemCategory);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCategoryCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageItemCategory");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add.";
                    return RedirectToAction("ItemCategory");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageItemCategory");
            }
        }
        [HttpPost]
        public IActionResult UpdateItemCategory(int id, ItemCategory obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCategoryUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageItemCategory");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageItemCategory");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageItemCategory");
            }

        }

        [HttpGet]
        public IActionResult UpdateItemCategory(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemCategoryUpdateByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ItemCategory>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return RedirectToAction("ManageItemCategory");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageItemCategory");
            }

        }
        public IActionResult ManageItemCategory()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                     "Inventory/ItemCategoryGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemCategory> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Item Category List Found. Please Enter Item Category First..";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

        }
        public IActionResult DeleteItemCategory(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteItemCategory" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageItemCategory");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageItemCategory");
                    }
                }
                return RedirectToAction("ManageItemCategory");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageItemCategory");
            }
        }
        //ItemCategory end
        //ItemSubCategory Start
        public IActionResult ItemSubCategory()
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                 "Inventory/ItemCategoryGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemCategory> responseObject = response.Data;
                        ViewBag.Categories = new SelectList(responseObject, "Id", "Name");
                    }
                    else
                    {
                        List<ItemCategory> listitemCategories = new List<ItemCategory>();
                        ViewBag.Categories = new SelectList(listitemCategories, "Id", "Name");
                    }
                }
                else
                {
                    List<ItemCategory> listitemCategories = new List<ItemCategory>();
                    ViewBag.Categories = new SelectList(listitemCategories, "Id", "Name");
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageItemSubCategory");
            }
        }
        [HttpPost]
        public IActionResult ItemSubCategory(ItemSubCategory ItemSubCategory)
        {
            try
            {
                SResponse categoryress = RequestSender.Instance.CallAPI("api",
              "Inventory/ItemCategoryByID/" + ItemSubCategory.CategoryId, "GET");
                if (categoryress.Status && (categoryress.Resp != null) && (categoryress.Resp != ""))
                {
                    ResponseBack<ItemCategory> record = JsonConvert.DeserializeObject<ResponseBack<ItemCategory>>(categoryress.Resp);
                    if (record.Data != null)
                    {
                        ItemSubCategory.ParentCategoryName = record.Data.Name;
                    }
                }
                var body = JsonConvert.SerializeObject(ItemSubCategory);


                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemSubCategoryCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageItemSubCategory");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("ItemSubCategory");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ItemSubCategory");
            }
        }
        [HttpPost]
        public IActionResult UpdateItemSubCategory(int id, ItemSubCategory obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemSubCategoryUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageItemSubCategory");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("ItemSubCategory");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageItemSubCategory");
            }

        }

        [HttpGet]
        public IActionResult UpdateItemSubCategory(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemSubCategoryUpdateByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ItemSubCategory>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageItemSubCategory");
            }

        }
        public IActionResult ManageItemSubCategory()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                     "Inventory/ItemSubCategoryGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemSubCategory> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Item SubCategory List Found. Please Enter Item SubCategory First.";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

        }
        public IActionResult DeleteItemSubCategory(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteSubItemCategory" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageItemSubCategory");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageItemSubCategory");
                    }
                }
                return RedirectToAction("ManageItemSubCategory");
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageItemSubCategory");
            }
        }
        //ItemSubCategory End
        public IActionResult ArticleType()
        {
            return View();
        }
        [HttpPost]

        public IActionResult AddArticleType(ArticleType articleType)
        {
            try
            {
                var body = JsonConvert.SerializeObject(articleType);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/ArticleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageArticleType");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("ArticleType");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageArticleType");
            }

        }
        public IActionResult ManageArticleType()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                        "Configuration/ArticleTypeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ArticleType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ArticleType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ArticleType> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Article Type List Found. Please Enter Article Type First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        [HttpPost]
        public IActionResult UpdateArticleType(int id, ArticleType obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Article/ArticleUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageArticleType");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ArticleType");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageArticleType");
            }

        }
        [HttpGet]
        public IActionResult UpdateArticleType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Article/ArticleGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ArticleType>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageArticleType");
            }

        }
        public IActionResult DeleteArticleType(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Configuration/DeleteArticle" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageArticleType");
                    }
                    else
                    {
                        TempData["Msg"] = "Delete Successfully";
                        return RedirectToAction("ManageArticleType");
                    }
                }
                return RedirectToAction("ManageArticleType");
            }
            catch (Exception ex)
            {

                TempData["Msg"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageArticleType");
            }



        }
        //ArticleEnd

        //StoreStart
        public IActionResult Store()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ManageStore()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/StoreGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Store>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Store>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Store> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Store List Found. Please Enter Store First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

        }
        [HttpPost]
        public IActionResult AddStore(Store store)
        {

            try
            {
                var body = JsonConvert.SerializeObject(store);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/StoreCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageStore");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("Store");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageStore");
            }

        }
        //For Adding Store Shortcut

        [HttpPost]
        public IActionResult AddStoreShortcut(Store obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/StoreCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("PurchaseOrder", "Purchase", new { area = "Inventory" });
            }

        }


        [HttpPost]
        public IActionResult UpdateStore(int id, Store obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/StoreUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageStore");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("Store");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageStore");

            }

        }
        [HttpGet]
        public IActionResult UpdateStore(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/StoreGetByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Store>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageStore");

            }

        }
        public IActionResult DeleteStore(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteStore" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageStore");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageStore");
                    }
                }
                return RedirectToAction("ManageStore");
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageStore");
            }
        }

        //StoreEnd



        //Modelstart
        public IActionResult AddTerminal()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddTerminal(Terminal obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Terminal/TerminalCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageTerminal");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddTerminal");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageTerminal");
            }

        }

        public IActionResult UpdateTerminal(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Terminal/TerminalByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Terminal>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageTerminal");
            }

        }

        [HttpPost]
        public IActionResult UpdateTerminal(int id, Terminal obj)
        {

            try
            {
                obj.TerminalId = id;
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Terminal/TerminalUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageTerminal");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageTerminal");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageTerminal");
            }

        }

        [HttpGet]
        public IActionResult ManageTerminal()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Terminal/TerminalGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Terminal>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Terminal>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Terminal> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Terminal List Found. Please Enter Terminal First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }
        }

        public IActionResult DeleteTerminal(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Terminal/DeleteTerminal" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageTerminal");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageTerminal");
                    }
                }
                return RedirectToAction("ManageTerminal");
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageTerminal");
            }
        }

        public IActionResult WareHouses()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ManageWareHouses()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Configuration/WareHouseGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<WareHouse>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<WareHouse>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<WareHouse> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "No Ware House List Found. Please Enter Ware House First.";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Show." + ex.Message;
                return RedirectToAction("Index", "Home", new { area = "Administration" });
            }

        }
        [HttpPost]
        public IActionResult AddWareHouse(WareHouse store)
        {

            try
            {
                var body = JsonConvert.SerializeObject(store);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/WareHouseCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("ManageWareHouses");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("ManageWareHouses");
                }
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Add." + ex.Message;
                return RedirectToAction("ManageStore");
            }

        }
        [HttpPost]
        public IActionResult UpdateWareHouse(int id, WareHouse obj)
        {

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Configuration/WareHouseUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";
                    return RedirectToAction("ManageStore");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageWareHouses");
                }
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageWareHouses");

            }

        }
        [HttpGet]
        public IActionResult UpdateWareHouse(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Configuration/WareHouseByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<WareHouse>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Unable To Update";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Unable to Update." + ex.Message;
                return RedirectToAction("ManageWareHouses");

            }

        }
        public IActionResult DeleteWareHouse(string id = "")
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Configuration/DeleteWareHouse" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageWareHouses");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageWareHouses");
                    }
                }
                return RedirectToAction("ManageWareHouses");
            }
            catch (Exception ex)
            {

                TempData["response"] = "Unable to Delete." + ex.Message;
                return RedirectToAction("ManageWareHouses");
            }
        }

    }
}
