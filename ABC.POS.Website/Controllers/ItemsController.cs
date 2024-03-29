﻿using ABC.DTOs.Library.Adaptors;
using ABC.EFCore.Entities.POS;
using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.POS.Domain.DataConfig.Configurations;
using ABC.POS.Website.Models;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    [ServiceFilter(typeof(Globle_Variable))]
    public class ItemsController : Controller
    {
        private static IHttpContextAccessor httpContextAccessor;
        SqlConnection con = new SqlConnection();
        //private static SqlConnection con;
        public string controllername;
        public ItemsController(IHttpContextAccessor accessor)  //SqlConnection _con)
        {

            httpContextAccessor = accessor;
            //con = _con;

        }


        public IActionResult Index()
        {
            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Product>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Product> responseObject = response.Data;

                    var actionname = "Get Item";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);
                    return View(responseObject);
                }
                else
                {
                    var actionname = "Unable to Get Item";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    TempData["response"] = "Server is down.";
                    return RedirectToAction("SessionExpire", "Home");
                }

            }
            TempData["response"] = "Server is down.";
            return RedirectToAction("SessionExpire", "Home");
        }
        public IActionResult AddItem()
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
                   "Inventory/ArticleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ArticleType>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<ArticleType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ArticleType> responseObject = response.Data;
                        ViewBag.family = new SelectList(responseObject.ToList(), "Id", "ArticleTypeName");

                    }
                    else
                    {
                        List<ArticleType> responseObject = new List<ArticleType>();
                        ViewBag.family = new SelectList(responseObject, "Id", "ArticleTypeName");
                    }
                    var actionname = "Get Article";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<ArticleType> listArticleType = new List<ArticleType>();
                    ViewBag.family = new SelectList(listArticleType, "Id", "ArticleTypeName");
                }


                //ViewBag.NoOfDays = limit;
                SResponse Colorres = RequestSender.Instance.CallAPI("api",
                      "Inventory/ColorGet", "GET");
                if (Colorres.Status && (Colorres.Resp != null) && (Colorres.Resp != ""))
                {
                    ResponseBack<List<Color>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Color>>>(Colorres.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Color> responseObject = response.Data;
                        ViewBag.Color = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Color> responseObject = new List<Color>();
                        ViewBag.Color = new SelectList(responseObject, "Id", "Name");
                    }
                    var actionname = "Get Colour";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<Color> listColor = new List<Color>();
                    ViewBag.Color = new SelectList(listColor, "Id", "Name");
                }

                SResponse Groupress = RequestSender.Instance.CallAPI("api",
                    "Inventory/GroupGet", "GET");
                if (Groupress.Status && (Groupress.Resp != null) && (Groupress.Resp != ""))
                {
                    ResponseBack<List<Group>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(Groupress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Group> responseObject = response.Data;
                        ViewBag.Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Group> responseObject = new List<Group>();
                        ViewBag.Group = new SelectList(responseObject, "Id", "Name");
                    }

                    var actionname = "Get Group";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<Group> listGroup = new List<Group>();
                    ViewBag.Group = new SelectList(listGroup, "Id", "Name");
                }

                SResponse Modelress = RequestSender.Instance.CallAPI("api",
                     "Inventory/ModelGet", "GET");
                if (Modelress.Status && (Modelress.Resp != null) && (Modelress.Resp != ""))
                {
                    ResponseBack<List<Model>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Model>>>(Modelress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Model> responseObject = response.Data;
                        ViewBag.Model = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Model> responseObject = new List<Model>();
                        ViewBag.Model = new SelectList(responseObject, "Id", "Name");
                    }

                    var actionname = "Get Model";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<Model> listModel = new List<Model>();
                    ViewBag.Model = new SelectList(listModel, "Id", "Name");
                }
                SResponse ItemCategoryGet = RequestSender.Instance.CallAPI("api",
                     "Inventory/ItemCategoryGet", "GET");
                if (ItemCategoryGet.Status && (ItemCategoryGet.Resp != null) && (ItemCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemCategory> responseObject = response.Data;
                        ViewBag.ItemCategory = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<ItemCategory> responseObject = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject, "Id", "Name");
                    }

                    var actionname = "Get ItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<ItemCategory> listItemCategory = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(listItemCategory, "Id", "Name");
                }
                SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemSubCategoryGet", "GET");
                if (ItemSubCategoryGet.Status && (ItemSubCategoryGet.Resp != null) && (ItemSubCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ItemSubCategoryGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemSubCategory> responseObject = response.Data;
                        ViewBag.ItemSubCategory = new SelectList(responseObject.ToList(), "Id", "SubCategory");
                    }
                    else
                    {
                        List<ItemSubCategory> responseObject = new List<ItemSubCategory>();
                        ViewBag.ItemSubCategory = new SelectList(responseObject, "Id", "SubCategory");
                    }

                    var actionname = "Get ItemSubCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<ItemSubCategory> listItemSubCategory = new List<ItemSubCategory>();
                    ViewBag.ItemSubCategory = new SelectList(listItemSubCategory, "Id", "SubCategory");
                }
                SResponse BrandGet = RequestSender.Instance.CallAPI("api",
                    "Inventory/BrandGet", "GET");
                if (BrandGet.Status && (BrandGet.Resp != null) && (BrandGet.Resp != ""))
                {
                    ResponseBack<List<Brand>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Brand>>>(BrandGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Brand> responseObject = response.Data;
                        ViewBag.BrandGet = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Brand> responseObject = new List<Brand>();
                        ViewBag.BrandGet = new SelectList(responseObject.ToList(), "Id", "Name");
                    }

                    var actionname = "Get Brand";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                }
                else
                {
                    List<Brand> listBrand = new List<Brand>();
                    ViewBag.BrandGet = new SelectList(listBrand, "Id", "Name");
                }



                Product Model = new Product();
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemGet", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<List<Product>> record = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(resp.Resp);
                    if (record.Data != null && record.Data.Count() > 0)
                    {
                        Product newitems = new Product();
                        var fullcode = "";
                        if (record.Data[0].ItemNumber != null && record.Data[0].ItemNumber != "string" && record.Data[0].ItemNumber != "")
                        {
                            int large, small;
                            int salesID = 0;
                            large = Convert.ToInt32(record.Data[0].ItemNumber.Split('-')[1]);
                            small = Convert.ToInt32(record.Data[0].ItemNumber.Split('-')[1]);
                            for (int i = 0; i < record.Data.Count(); i++)
                            {
                                if (record.Data[i].ItemNumber != null)
                                {
                                    var t = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);
                                    if (Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]) > large)
                                    {
                                        salesID = record.Data[i].Id;
                                        large = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);

                                    }
                                    else if (Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]) < small)
                                    {
                                        small = Convert.ToInt32(record.Data[i].ItemNumber.Split('-')[1]);
                                    }
                                    else
                                    {
                                        if (large < 2)
                                        {
                                            salesID = record.Data[i].Id;
                                        }
                                    }
                                }
                            }
                            newitems = record.Data.ToList().Where(x => x.Id == Convert.ToInt32(salesID)).FirstOrDefault();
                            if (newitems != null)
                            {
                                if (newitems.ItemNumber != null)
                                {
                                    var VcodeSplit = newitems.ItemNumber.Split('-');
                                    int code = Convert.ToInt32(VcodeSplit[1]) + 1;
                                    fullcode = "00" + "-" + Convert.ToString(code);
                                }
                                else
                                {
                                    fullcode = "00" + "-" + "1";
                                }
                            }
                            else
                            {
                                fullcode = "00" + "-" + "1";
                            }
                        }
                        else
                        {
                            fullcode = "00" + "-" + "1";
                        }

                        ViewBag.ItemNumber = fullcode;
                    }
                    else
                    {
                        ViewBag.ItemNumber = "00" + "-" + "1";
                    }
                    Model.ItemNumber = ViewBag.ItemNumber;
                }
                else if (resp.Status && resp.Resp == "")
                {
                    ViewBag.ItemNumber = "00" + "-" + "1";
                    Model.ItemNumber = ViewBag.ItemNumber;
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Get Item Number";


                    var actionname1 = "Unable to Get Items";
                    var pageName1 = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname1, controllername, userDetail.Id, userDetail.UserName, pageName1);

                    return RedirectToAction("Index", "Home");
                }

                var actionname2 = "Get Items";
                var pageName2 = RouteData.Values["action"].ToString();
                HelperClass.activitylog(actionname2, controllername, userDetail.Id, userDetail.UserName, pageName2);

                return View(Model);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public IActionResult AddItem(Product obj, IFormFile file)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                if (file != null)
                {
                    var input = file.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    obj.ItemImage = byteData;
                }
                var body = JsonConvert.SerializeObject(obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Add Successfully";
                    return RedirectToAction("InventoryProfessional");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("InventoryProfessional");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult Purchase()
        {
            return View();
        }
        //Modelstart
        public IActionResult AddModel()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddModel(Model obj)
        {
            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            try
            {
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ModelCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {

                    var actionname = "Create Model";
                    var pageName = RouteData.Values["action"].ToString();

                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    TempData["response"] = "Add Successfully";
                    return RedirectToAction("AddModel");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    return RedirectToAction("AddModel");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult UpdateModel(int id)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ModelByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Model>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        var actionname = "Get Model By ID";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult UpdateModel(int id, Model obj)
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ModelUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";


                    var actionname = "Update Model";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("ManageModel");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageModel");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public IActionResult ManageModel()
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/ModelGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Model>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Model>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Model> responseObject = response.Data;

                        var actionname = "Get Models";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult DeleteModel(string id = "")
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteModel" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        var actionname = "Delete Model";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        return RedirectToAction("ManageModel");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        var actionname = "Delete Model";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        return RedirectToAction("ManageModel");
                    }
                }
                return RedirectToAction("ManageModel");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        //ModelEND
        //GroupSTART
        public IActionResult AddGroup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGroup(Group obj)
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/GroupCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";
                    var actionname = "Add Group";
                    var pageName = RouteData.Values["action"].ToString();

                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("AddGroup");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    var actionname = "Unable To  Group";
                    var pageName = RouteData.Values["action"].ToString();

                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("AddGroup");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult UpdateGroup(int id)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GroupByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Group>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        var actionname = "Get Group by id";
                        var pageName = RouteData.Values["action"].ToString();

                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult UpdateGroup(int id, Group obj)
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;


                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/GroupUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";

                    var actionname = "Update Group";
                    var pageName = RouteData.Values["action"].ToString();

                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("ManageGroup");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";
                    var actionname = "Unable To Update Group";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("ManageGroup");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public IActionResult ManageGroup()
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/GroupGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Group>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Group> responseObject = response.Data;

                        var actionname = "Get all Groups";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult DeleteGroup(string id = "")
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;


                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteGroup" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageGroup");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";
                        return RedirectToAction("ManageGroup");
                    }
                }

                var actionname = "Deleta Groups";
                var pageName = RouteData.Values["action"].ToString();
                HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);


                return RedirectToAction("ManageGroup");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }



        }

        //ModelEND



        //ColorSTART
        public IActionResult AddColor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddColor(Color obj)
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ColorCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Add Successfully";

                    var actionname = "Add Color";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);


                    return RedirectToAction("AddColor");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Add";
                    var actionname = "Unable To Add Color";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);


                    return RedirectToAction("AddColor");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult UpdateColor(int id)
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ColorByID" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Color>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;

                        var actionname = "Get Colour By id";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        public IActionResult UpdateColor(int id, Color obj)
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ColorUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["response"] = "Update Successfully";

                    var actionname = "Update Color";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("ManageColor");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Update";

                    var actionname = "Unable to update Color";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("ManageColor");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpGet]
        public IActionResult ManageColor()
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "Inventory/ColorGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<Color>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Color>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Color> responseObject = response.Data;

                        var actionname = "Get all Colours";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

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
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }

        }

        public IActionResult DeleteColor(string id = "")
        {

            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteColor" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";

                        var actionname = "Delete Color";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        return RedirectToAction("ManageColor");
                    }
                    else
                    {
                        TempData["response"] = "Delete Successfully";

                        var actionname = "Unable To Delete Color";
                        var pageName = RouteData.Values["action"].ToString();
                        HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        return RedirectToAction("ManageColor");
                    }
                }
                return RedirectToAction("ManageColor");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }



        }

        //ColorEND

        public JsonResult AddItemCategory(ItemCategory obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;
                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCategoryCreate", "POST", body);
                if (resp.Resp == "Already Exists")
                {
                    Msg = "Category Already Added";

                    var actionname = "Add ItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);
                    //if(resp.Status.)
                    //{
                    //    Msg = "Already Added";
                    //}
                    return Json(Msg);
                }
                else if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    var actionname = "Add ItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }

                else
                {
                    var actionname = "Unable to Add ItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddSubItemCategory(ItemSubCategory obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemSubCategoryCreate", "POST", body);
                if (resp.Resp == "Already Exists")
                {
                    Msg = "SubCategory Already Added";

                    var actionname = "Add ItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    var actionname = "Unable to Create SubItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";

                    var actionname = "Unable to Create SubItemCategory";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddBrand(Brand obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/BrandCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add Brand";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";

                    var actionname = "Unable to Add Brand";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddArticle(ArticleType obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ArticleCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add Article";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";


                    var actionname = "Unable To Add Article";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddGroupJson(Group obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/GroupCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add Group Jaon";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add Jsaon";
                    var actionname = "Unable to Add Group Jsaon";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddModelJson(Model obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ModelCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add Model Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";

                    var actionname = "Unable to Add Model Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddMispickJson(MisPick obj)
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/MisPickCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add MisPick Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";

                    var actionname = "Unable To Add MisPick Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AddColorJson(Color obj)
        {
            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var Msg = "";
                var body = JsonConvert.SerializeObject(obj);

                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ColorCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";

                    var actionname = "Add Color Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(Msg);
                }
                else
                {

                    var actionname = "Unable To Add Color Json";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json(JsonConvert.DeserializeObject("false." + Msg));
                }
            }
            catch (Exception ex)
            {
                return Json(JsonConvert.DeserializeObject("false." + ex.Message));
            }
        }
        public JsonResult AutoCompleteSearchItem()
        {
            string Msg = "";

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            List<Product> responseObject = new List<Product>();
            var FoundSession = HttpContext.Session.GetString("loadedProducts");
            List<Product> FoundSession_Result = new List<Product>();
            if (FoundSession != null && FoundSession.Count() > 0 && FoundSession != "null")
            {
                FoundSession_Result = JsonConvert.DeserializeObject<List<Product>>(FoundSession);
            }
            if (FoundSession_Result != null && FoundSession_Result.Count() < 1)
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/OptimizeItems", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {

                    ResponseBack<List<Product>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        responseObject = response.Data;
                        //var Name = (from N in responseObject
                        //            where N.Name.ToLower().Contains(Prefix.ToLower())
                        //            select new { N.Name, N.Stock.Quantity });

                        //var actionname = "Get Items";
                        // var pageName = RouteData.Values["action"].ToString();
                        //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);
                        HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(responseObject));
                        return Json(responseObject);
                    }
                    else
                    {
                        return Json(Msg);
                    }
                }
                else
                {
                    return Json(responseObject);
                }
            }
            else
            {
                return Json(FoundSession_Result);
            }
            return Json(Msg);
        }
        public IActionResult InventoryProfessional()
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
                   "Inventory/ArticleGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ArticleType>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<ArticleType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ArticleType> responseObject = response.Data;
                        ViewBag.family = new SelectList(responseObject.ToList(), "Id", "ArticleTypeName");

                    }
                    else
                    {
                        List<ArticleType> responseObject = new List<ArticleType>();
                        ViewBag.family = new SelectList(responseObject, "Id", "ArticleTypeName");
                    }
                }
                else
                {
                    List<ArticleType> listArticleType = new List<ArticleType>();
                    ViewBag.family = new SelectList(listArticleType, "Id", "ArticleTypeName");
                }

                SResponse ressSD = RequestSender.Instance.CallAPI("api",
                "Inventory/SupplierDocumentTypeGet", "GET");
                if (ressSD.Status && (ressSD.Resp != null) && (ressSD.Resp != ""))
                {
                    ResponseBack<List<SupplierDocumentType>> response = JsonConvert.DeserializeObject<ResponseBack<List<SupplierDocumentType>>>(ressSD.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<SupplierDocumentType> responseObject = response.Data;
                        ViewBag.SupDocType = new SelectList(responseObject, "DocumentTypeId", "DocumentType");
                    }
                    else
                    {
                        List<SupplierDocumentType> listEmpDocType = new List<SupplierDocumentType>();
                        ViewBag.SupDocType = new SelectList(listEmpDocType, "DocumentTypeId", "DocumentType");
                    }
                }
                else
                {
                    List<SupplierDocumentType> listEmpNo = new List<SupplierDocumentType>();
                    ViewBag.SupDocType = new SelectList(listEmpNo, "DocumentTypeId", "DocumentType");
                }

                var actionname = "Get Article";
                var pageName = RouteData.Values["action"].ToString();
                //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                SResponse MisRess = RequestSender.Instance.CallAPI("api",
                   "Inventory/MisPickGet", "GET");
                if (MisRess.Status && (MisRess.Resp != null) && (MisRess.Resp != ""))
                {
                    ResponseBack<List<MisPick>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<MisPick>>>(MisRess.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<MisPick> responseObject = response.Data;
                        ViewBag.Mispick = new SelectList(responseObject.ToList(), "MisPickId", "MisPickName");

                    }
                    else
                    {
                        List<MisPick> responseObject = new List<MisPick>();
                        ViewBag.Mispick = new SelectList(responseObject, "MisPickId", "MisPickName");
                    }
                }
                else
                {
                    List<MisPick> listmispick = new List<MisPick>();
                    ViewBag.Mispick = new SelectList(listmispick, "MisPickId", "MisPickName");
                }
                actionname = "Get Mispick";
                //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);


                SResponse Colorres = RequestSender.Instance.CallAPI("api",
                      "Inventory/ColorGet", "GET");
                if (Colorres.Status && (Colorres.Resp != null) && (Colorres.Resp != ""))
                {
                    ResponseBack<List<Color>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Color>>>(Colorres.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Color> responseObject = response.Data;
                        ViewBag.Color = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Color> responseObject = new List<Color>();
                        ViewBag.Color = new SelectList(responseObject, "Id", "Name");
                    }
                }
                else
                {
                    List<Color> listColor = new List<Color>();
                    ViewBag.Color = new SelectList(listColor, "Id", "Name");
                }
                actionname = "Get Color";
                //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);



                SResponse Groupress = RequestSender.Instance.CallAPI("api",
                    "Inventory/GroupGet", "GET");
                if (Groupress.Status && (Groupress.Resp != null) && (Groupress.Resp != ""))
                {
                    ResponseBack<List<Group>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Group>>>(Groupress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Group> responseObject = response.Data;
                        ViewBag.Group = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Group> responseObject = new List<Group>();
                        ViewBag.Group = new SelectList(responseObject, "Id", "Name");
                    }
                }
                else
                {
                    List<Group> listGroup = new List<Group>();
                    ViewBag.Group = new SelectList(listGroup, "Id", "Name");
                }
                actionname = "Get Color";
                // HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);


                SResponse Modelress = RequestSender.Instance.CallAPI("api",
                     "Inventory/ModelGet", "GET");
                if (Modelress.Status && (Modelress.Resp != null) && (Modelress.Resp != ""))
                {
                    ResponseBack<List<Model>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Model>>>(Modelress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Model> responseObject = response.Data;
                        ViewBag.Model = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Model> responseObject = new List<Model>();
                        ViewBag.Model = new SelectList(responseObject, "Id", "Name");
                    }
                }
                else
                {
                    List<Model> listModel = new List<Model>();
                    ViewBag.Model = new SelectList(listModel, "Id", "Name");
                }
                actionname = "Get Model";
                //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                SResponse ItemCategoryGet = RequestSender.Instance.CallAPI("api",
                     "Inventory/ItemCategoryGet", "GET");
                if (ItemCategoryGet.Status && (ItemCategoryGet.Resp != null) && (ItemCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemCategory>> response =
                                  JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ItemCategoryGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemCategory> responseObject = response.Data;
                        ViewBag.ItemCategory = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<ItemCategory> responseObject = new List<ItemCategory>();
                        ViewBag.ItemCategory = new SelectList(responseObject, "Id", "Name");
                    }
                }
                else
                {
                    List<ItemCategory> listItemCategory = new List<ItemCategory>();
                    ViewBag.ItemCategory = new SelectList(listItemCategory, "Id", "Name");
                }

                actionname = "Get ItemCategory";
                // HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                SResponse ItemSubCategoryGet = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemSubCategoryGet", "GET");
                if (ItemSubCategoryGet.Status && (ItemSubCategoryGet.Resp != null) && (ItemSubCategoryGet.Resp != ""))
                {
                    ResponseBack<List<ItemSubCategory>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ItemSubCategory>>>(ItemSubCategoryGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ItemSubCategory> responseObject = response.Data;
                        ViewBag.ItemSubCategory = new SelectList(responseObject.ToList(), "Id", "SubCategory");
                    }
                    else
                    {
                        List<ItemSubCategory> responseObject = new List<ItemSubCategory>();
                        ViewBag.ItemSubCategory = new SelectList(responseObject, "Id", "SubCategory");
                    }
                }
                else
                {
                    List<ItemSubCategory> listItemSubCategory = new List<ItemSubCategory>();
                    ViewBag.ItemSubCategory = new SelectList(listItemSubCategory, "Id", "SubCategory");
                }

                actionname = "Get ItemSubCategory";
                // HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                SResponse BrandGet = RequestSender.Instance.CallAPI("api",
                    "Inventory/BrandGet", "GET");
                if (BrandGet.Status && (BrandGet.Resp != null) && (BrandGet.Resp != ""))
                {
                    ResponseBack<List<Brand>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Brand>>>(BrandGet.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<Brand> responseObject = response.Data;
                        ViewBag.BrandGet = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                    else
                    {
                        List<Brand> responseObject = new List<Brand>();
                        ViewBag.BrandGet = new SelectList(responseObject.ToList(), "Id", "Name");
                    }
                }
                else
                {
                    List<Brand> listBrand = new List<Brand>();
                    ViewBag.BrandGet = new SelectList(listBrand, "Id", "Name");
                }

                actionname = "Get Brand";
                //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                List<Product> limit = new List<Product>
                     {
                    new Product() {MaintainStockForDays = "7-14 Days" },
                    new Product(){MaintainStockForDays="14-21 Days"},
                   new Product(){MaintainStockForDays = "21-28 Days" },
                   new Product(){MaintainStockForDays="OneMonth"},
                    new Product(){MaintainStockForDays = "TwoMonths" },
                   new Product(){MaintainStockForDays="ThreeMohtns"},


                    };



                ViewBag.NoOfDays = new SelectList(limit.ToList(), "MaintainStockForDays", "MaintainStockForDays");
                Product Model = new Product();
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/NextItemCode", "GET");
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<string> record = JsonConvert.DeserializeObject<ResponseBack<string>>(resp.Resp);
                    if (record.Data != null)
                    {
                        Product newitems = new Product();
                        ViewBag.ItemNumber = record.Data;

                        var random = new Random();
                        //string brcode = string.Empty;
                        //var alreadyexist = record.Data.FirstOrDefault();
                        //while (alreadyexist != null)
                        //{
                        //    for (int i = 0; i < 12; i++)
                        //    {
                        //        brcode = String.Concat(brcode, random.Next(10).ToString());
                        //    }

                        //    alreadyexist = record.Data.Where(x => x.BarCode == brcode).FirstOrDefault();
                        //}
                        //ViewBag.Barcode = brcode;

                        actionname = "Get Items";
                        //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    }
                    else
                    {
                        //var random = new Random();
                        //string brcode = string.Empty;
                        //for (int i = 0; i < 12; i++)
                        //{
                        //    brcode = String.Concat(brcode, random.Next(10).ToString());
                        //}
                        //ViewBag.Barcode = brcode;
                        ViewBag.ItemNumber = "0000" + "1";
                    }
                    Model.ItemNumber = ViewBag.ItemNumber;
                }
                else if (resp.Status && resp.Resp == "")
                {
                    ViewBag.ItemNumber = "0000" + "1";
                    Model.ItemNumber = ViewBag.ItemNumber;
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Get Item Number";
                    return RedirectToAction("Index", "Home");
                }
                return View(Model);
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }



        }


        [HttpPost]
        public IActionResult InventoryProfessional(Product obj, IFormFile ItemImage)
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;


                if (ItemImage != null)
                {
                    var input = ItemImage.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    obj.ItemImage = byteData;
                }
                else
                {
                    obj.ItemImage = null;
                }
                //"{\"Stock\":null,\"Financial\":{\"FinancialId\":0,\"Cost\":\"50\",\"Profit\":\"37.50\",\"MsgPromotion\":null,\"AddToCost\":null,\"UnitCharge\":null,\"FixedCost\":null,\"CostPerQuantity\":null,\"St\":null,\"Tax\":null,\"OutOfStateCost\":null,\"OutOfStateRetail\":null,\"Price\":null,\"Quantity\":null,\"QuantityPrice\":null,\"SuggestedRetailPrice\":null,\"AutoSetSrp\":false,\"ItemNumber\":null,\"QuantityInStock\":null,\"Adjustment\":null,\"AskForPricing\":false,\"AskForDescrip\":false,\"Serialized\":false,\"TaxOnSales\":false,\"Purchase\":false,\"NoSuchDiscount\":false,\"NoReturns\":false,\"SellBelowCost\":false,\"OutOfState\":null,\"CodeA\":false,\"CodeB\":false,\"CodeC\":false,\"CodeD\":false,\"AddCustomersDiscount\":null,\"ItemName\":null,\"Retail\":\"80\",\"ItemId\":null},\"Id\":0,\"Name\":\"pepsi\",\"ItemCategoryId\":12,\"BrandId\":5,\"ArticleId\":null,\"StoreId\":null,\"Unit\":null,\"ProductCode\":null,\"BarCode\":null,\"Size\":null,\"ColorId\":null,\"Sku\":null,\"Description\":null,\"UnitRetail\":\"40.00\",\"SaleRetail\":null,\"TaxExempt\":null,\"ShippingEnable\":null,\"AllowECommerce\":null,\"CreatedDate\":null,\"CreatedBy\":null,\"ModifiedBy\":null,\"ModifiedDate\":null,\"OldPrice\":null,\"MsareportAs\":null,\"StateReportAs\":null,\"ReportingWeight\":null,\"FamilyId\":3,\"Family\":null,\"QtyUnit\":null,\"UnitsInPack\":null,\"RetailPackPrice\":null,\"SalesLimit\":null,\"Adjustment\":null,\"ProfitPercentage\":null,\"Cost\":null,\"MfgPromotion\":null,\"AddtoCostPercenatge\":null,\"UnitCharge\":null,\"OutofstateCost\":null,\"OutofstateRetail\":null,\"TaxonSale\":null,\"TaxOnPurchase\":null,\"Location\":null,\"GroupId\":2,\"ItemNumber\":null,\"QtyinStock\":null,\"ItemSubCategoryId\":8,\"ModelId\":null,\"ModelName\":null,\"CategoryName\":null,\"SubCatName\":null,\"GroupName\":null,\"BrandName\":null,\"ColorName\":null,\"ItemImage\":null,\"ItemImageByPath\":null,\"Variations\":null,\"DiscountPrice\":null,\"Rating\":null,\"MinOrderQty\":\"undefined\",\"MaxOrderQty\":null,\"Retail\":\"80\",\"QuantityCase\":null,\"QuantityPallet\":null,\"SingleUnitMsa\":null,\"MisPickId\":null,\"MisPickName\":null,\"OrderQuantity\":null,\"Units\":\"undefined\",\"WeightOz\":null,\"WeightLb\":null,\"LocationTwo\":null,\"LocationThree\":null,\"LocationFour\":null,\"MaintainStockForDays\":null,\"IsActive\":null}"
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<Product> responseobj = JsonConvert.DeserializeObject<ResponseBack<Product>>(resp.Resp);

                    //var srt = JsonConvert.SerializeObject(responseobj.Data);
                    //httpContextAccessor.HttpContext.Session.SetString("CurrentProductdata", srt);
                    if (responseobj != null)
                    {
                        TempData["Msg"] = "Add Successfully";
                    }
                    TempData["Msg"] = "Add Successfully";
                    // GlobalPOS.listcart = new List<Product>();

                    HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(null));
                    //HttpContext.Session.SetString("loadedItemGet", JsonConvert.SerializeObject(null));

                    //var actionname = "Add Item";
                    //var pageName = RouteData.Values["action"].ToString();
                    //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);



                    return Content("true");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Add";

                    //var actionname = "Unable To Add Item";
                    //var pageName = RouteData.Values["action"].ToString();
                    //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("InventoryProfessional");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        //[HttpPost]
        //public IActionResult InventoryProfessional(Product obj, IFormFile file)
        //{
        //    try
        //    {

        //        controllername = RouteData.Values["controller"].ToString();
        //        var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;


        //        if (file != null)
        //        {
        //            var input = file.OpenReadStream();
        //            byte[] byteData = null, buffer = new byte[input.Length];
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                int read;
        //                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //                {
        //                    ms.Write(buffer, 0, read);
        //                }
        //                byteData = ms.ToArray();
        //            }
        //            obj.ItemImage = byteData;
        //        }
        //        //"{\"Stock\":null,\"Financial\":{\"FinancialId\":0,\"Cost\":\"50\",\"Profit\":\"37.50\",\"MsgPromotion\":null,\"AddToCost\":null,\"UnitCharge\":null,\"FixedCost\":null,\"CostPerQuantity\":null,\"St\":null,\"Tax\":null,\"OutOfStateCost\":null,\"OutOfStateRetail\":null,\"Price\":null,\"Quantity\":null,\"QuantityPrice\":null,\"SuggestedRetailPrice\":null,\"AutoSetSrp\":false,\"ItemNumber\":null,\"QuantityInStock\":null,\"Adjustment\":null,\"AskForPricing\":false,\"AskForDescrip\":false,\"Serialized\":false,\"TaxOnSales\":false,\"Purchase\":false,\"NoSuchDiscount\":false,\"NoReturns\":false,\"SellBelowCost\":false,\"OutOfState\":null,\"CodeA\":false,\"CodeB\":false,\"CodeC\":false,\"CodeD\":false,\"AddCustomersDiscount\":null,\"ItemName\":null,\"Retail\":\"80\",\"ItemId\":null},\"Id\":0,\"Name\":\"pepsi\",\"ItemCategoryId\":12,\"BrandId\":5,\"ArticleId\":null,\"StoreId\":null,\"Unit\":null,\"ProductCode\":null,\"BarCode\":null,\"Size\":null,\"ColorId\":null,\"Sku\":null,\"Description\":null,\"UnitRetail\":\"40.00\",\"SaleRetail\":null,\"TaxExempt\":null,\"ShippingEnable\":null,\"AllowECommerce\":null,\"CreatedDate\":null,\"CreatedBy\":null,\"ModifiedBy\":null,\"ModifiedDate\":null,\"OldPrice\":null,\"MsareportAs\":null,\"StateReportAs\":null,\"ReportingWeight\":null,\"FamilyId\":3,\"Family\":null,\"QtyUnit\":null,\"UnitsInPack\":null,\"RetailPackPrice\":null,\"SalesLimit\":null,\"Adjustment\":null,\"ProfitPercentage\":null,\"Cost\":null,\"MfgPromotion\":null,\"AddtoCostPercenatge\":null,\"UnitCharge\":null,\"OutofstateCost\":null,\"OutofstateRetail\":null,\"TaxonSale\":null,\"TaxOnPurchase\":null,\"Location\":null,\"GroupId\":2,\"ItemNumber\":null,\"QtyinStock\":null,\"ItemSubCategoryId\":8,\"ModelId\":null,\"ModelName\":null,\"CategoryName\":null,\"SubCatName\":null,\"GroupName\":null,\"BrandName\":null,\"ColorName\":null,\"ItemImage\":null,\"ItemImageByPath\":null,\"Variations\":null,\"DiscountPrice\":null,\"Rating\":null,\"MinOrderQty\":\"undefined\",\"MaxOrderQty\":null,\"Retail\":\"80\",\"QuantityCase\":null,\"QuantityPallet\":null,\"SingleUnitMsa\":null,\"MisPickId\":null,\"MisPickName\":null,\"OrderQuantity\":null,\"Units\":\"undefined\",\"WeightOz\":null,\"WeightLb\":null,\"LocationTwo\":null,\"LocationThree\":null,\"LocationFour\":null,\"MaintainStockForDays\":null,\"IsActive\":null}"
        //        var body = JsonConvert.SerializeObject(obj);
        //        SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemCreate", "POST", body);
        //        if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
        //        {
        //            ResponseBack<Product> responseobj = JsonConvert.DeserializeObject<ResponseBack<Product>>(resp.Resp);

        //            //var srt = JsonConvert.SerializeObject(responseobj.Data);
        //            //httpContextAccessor.HttpContext.Session.SetString("CurrentProductdata", srt);
        //            if (responseobj != null)
        //            {
        //                TempData["Msg"] = "Add Successfully";
        //            }
        //            TempData["Msg"] = "Add Successfully";

        //            var actionname = "Add Item";
        //            var pageName = RouteData.Values["action"].ToString();
        //            HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);



        //            return Content("true");
        //        }
        //        else
        //        {
        //            TempData["Msg"] = resp.Resp + " " + "Unable To Add";

        //            var actionname = "Unable To Add Item";
        //            var pageName = RouteData.Values["action"].ToString();
        //            HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

        //            return RedirectToAction("InventoryProfessional");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Msg"] = "Error. " + ex.Message;
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        [HttpPost]
        public IActionResult InventoryProfessionalUpdate(Product obj, IFormFile ItemImage)
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var id = obj.Id;

                if (ItemImage != null)
                {
                    var input = ItemImage.OpenReadStream();
                    byte[] byteData = null, buffer = new byte[input.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        byteData = ms.ToArray();
                    }
                    obj.ItemImage = byteData;
                }
                else
                {
                    obj.ItemImage = null;
                }
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemUpdate" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";
                    HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(null));

                    //var actionname = "Update Item";
                    //var pageName = RouteData.Values["action"].ToString();
                    //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json("true");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Update";

                    //var actionname = "Unable To Update Item";
                    //var pageName = RouteData.Values["action"].ToString();
                    //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("InventoryProfessional");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
        //[HttpPost]
        //public IActionResult InventoryProfessionalUpdate(Product obj, IFormFile file)
        //{
        //    try
        //    {

        //        controllername = RouteData.Values["controller"].ToString();
        //        var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

        //        var id = obj.Id;

        //        if (file != null)
        //        {
        //            var input = file.OpenReadStream();
        //            byte[] byteData = null, buffer = new byte[input.Length];
        //            using (MemoryStream ms = new MemoryStream())
        //            {
        //                int read;
        //                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        //                {
        //                    ms.Write(buffer, 0, read);
        //                }
        //                byteData = ms.ToArray();
        //            }
        //            obj.ItemImage = byteData;
        //        }
        //        var body = JsonConvert.SerializeObject(obj);
        //        SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemUpdate" + "/" + id, "PUT", body);

        //        if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
        //        {
        //            TempData["Msg"] = "Update Successfully";

        //            var actionname = "Update Item";
        //            var pageName = RouteData.Values["action"].ToString();
        //            HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

        //            return Json("true");
        //        }
        //        else
        //        {
        //            TempData["Msg"] = resp.Resp + " " + "Unable To Update";

        //            var actionname = "Unable To Update Item";
        //            var pageName = RouteData.Values["action"].ToString();
        //            HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

        //            return RedirectToAction("InventoryProfessional");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["Msg"] = "Error. " + ex.Message;
        //        return RedirectToAction("Index", "Home");
        //    }
        //}
        public JsonResult GetStockItemDataByItemID(int id)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetStockAlertByID/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<List<InventoryStock>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    var actionname = "Get StockAlert By ID";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    TempData["Msg"] = "Stock Level Not found.";
                }
            }
            return Json(Msg);
        }
        public JsonResult CheapestValueFind(int id)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/CheckCheapProduct" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<List<Product>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    var actionname = "Get Cheap Product by Id";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetItemByID(int id)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetByID" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    var actionname = "Get Item by Id";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "No Record Found.";
                }
            }
            return Json(Msg);
        }
        public JsonResult GetItemList()
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                    "Inventory/ItemCategoryGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<ItemCategory>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<ItemCategory>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<ItemCategory> responseObject = response.Data;

                    var actionname = "Get Item Categories";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }

                else
                {
                    Msg = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public IActionResult InventoryFinancialUpdate(Financial obj, IFormFile file)
        {
            try
            {

                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                var body = JsonConvert.SerializeObject(obj);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemUpdate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["Msg"] = "Update Successfully";


                    var actionname = "Update Item";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Msg"] = resp.Resp + " " + "Unable To Update";

                    var actionname = "Unable To Update Item";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return RedirectToAction("InventoryProfessional");
                }
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult DeleteItem(string id = "")
        {

            try
            {
                controllername = RouteData.Values["controller"].ToString();
                var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteItem" + "/" + id, "Delete");
                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["response"] = "Delete Successfully";
                        HttpContext.Session.SetString("loadedProducts", JsonConvert.SerializeObject(null));

                        //var actionname = "Delete Item";
                        //var pageName = RouteData.Values["action"].ToString();
                        //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        return Json(ress.Status);
                        //return RedirectToAction("InventoryProfessional");
                    }
                    else
                    {

                        //var actionname = "Unable To Delete Item";
                        //var pageName = RouteData.Values["action"].ToString();
                        //HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                        TempData["response"] = "Unable To Delete";
                        return Json(ress.Status);

                        // return RedirectToAction("InventoryProfessional");
                    }
                }
                return RedirectToAction("InventoryProfessional");
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Error. " + ex.Message;
                return RedirectToAction("Index", "Home");
            }



        }



        public IActionResult Suppliers()
        {


            return View();

        }

        [HttpGet]
        public JsonResult GetItemStockByID(int id)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
             "Inventory/ItemGetByIDWithStockAndFinancial" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    Product responseObject = response.Data;
                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            else
            {
                return Json("false");
            }
        }
        public JsonResult GetItemStockFinancialByID(int id)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/ItemGetByIDWithStockAndFinancial" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    if (responseObject != null)
                    {
                        SResponse ressPO = RequestSender.Instance.CallAPI("api",
                             "Purchase/PurchaseByproductID" + "/" + responseObject.Id, "GET");
                        if (ressPO.Status && (ressPO.Resp != null) && (ressPO.Resp != ""))
                        {
                            var responsePO = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ressPO.Resp);
                            List<PurchaseOrder> POData = responsePO.Data;
                            if (POData.Count() > 0)
                            {
                                responseObject.PurchaseOrders = new List<PurchaseOrder>();
                                for (int i = 0; i < POData.Count(); i++)
                                {
                                    responseObject.PurchaseOrders.Add(POData[i]);
                                }
                            }
                        }

                        SResponse ressSO = RequestSender.Instance.CallAPI("api",
                             "Inventory/SaleByproductID" + "/" + responseObject.Id, "GET");
                        if (ressSO.Status && (ressSO.Resp != null) && (ressSO.Resp != ""))
                        {
                            var responseSO = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(ressSO.Resp);
                            List<PosSale> SOData = responseSO.Data;
                            if (SOData.Count() > 0)
                            {
                                responseObject.SaleOrders = new List<PosSale>();
                                for (int i = 0; i < SOData.Count(); i++)
                                {
                                    SOData[i].InvDate = SOData[i].InvoiceDate.Value.Date.ToString("MM/dd/yyy");
                                    responseObject.SaleOrders.Add(SOData[i]);
                                }
                            }
                        }

                        SResponse ressVe = RequestSender.Instance.CallAPI("api",
                             "Inventory/InventoryVendorGetbyProduct" + "/" + responseObject.Id, "GET");
                        if (ressVe.Status && (ressVe.Resp != null) && (ressVe.Resp != ""))
                        {
                            var responseVe = JsonConvert.DeserializeObject<ResponseBack<List<Vendor>>>(ressVe.Resp);
                            List<Vendor> VEData = responseVe.Data;
                            if (VEData.Count() > 0)
                            {
                                responseObject.Vendors = new List<Vendor>();
                                for (int i = 0; i < VEData.Count(); i++)
                                {
                                    //SOData[i].InvDate = SOData[i].InvoiceDate.Value.Date.ToString("MM/dd/yyy");
                                    responseObject.Vendors.Add(VEData[i]);
                                }
                            }
                        }

                    } 
                    
                    SResponse ressDoc = RequestSender.Instance.CallAPI("api",
                             "Inventory/DocumentGetByItemID" + "/" + responseObject.Id, "GET");
                        if (ressDoc.Status && (ressDoc.Resp != null) && (ressDoc.Resp != ""))
                        {
                            var responseVe = JsonConvert.DeserializeObject<ResponseBack<List<ItemDocument>>>(ressDoc.Resp);
                            List<ItemDocument> VEDataDoc = responseVe.Data;
                            if (VEDataDoc != null && VEDataDoc.Count() > 0)
                            {
                                responseObject.ItemDocuments = new List<ItemDocument>();
                                for (int i = 0; i < VEDataDoc.Count(); i++)
                                {
                                    //SOData[i].InvDate = SOData[i].InvoiceDate.Value.Date.ToString("MM/dd/yyy");
                                    responseObject.ItemDocuments.Add(VEDataDoc[i]);
                                }
                            }
                        }

                    var actionname = "Get Item by id with stock and financial";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }


        public JsonResult GetItemStockFinancialByIDForModels(int id)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/GetproductByStockName" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<Product>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    if(responseObject.ItemQuantity == null)
                    {
                        responseObject.ItemQuantity = "0.00";
                    }
                    return Json(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return Json(Msg);
        }
        public JsonResult InventoryGetPurchaseDetail(string InvoiceNumber)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/InventoryPurchaseGetByInvoiceNumber" + "/" + InvoiceNumber, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<PurchaseOrder>> response = JsonConvert.DeserializeObject<ResponseBack<List<PurchaseOrder>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;
                    for (int i = 0; i < responseObject.Count(); i++)
                    {
                        responseObject[i].POrderDate = responseObject[i].Podate.Value.ToString("MM/dd/yyyy");
                        responseObject[i].RecievedOrderDate = responseObject[i].DateReceived.Value.ToString("MM/dd/yyyy");
                    }

                    // var actionname = "Get Item by id with stock and Get inventory purchase by invoice number";
                    // var pageName = RouteData.Values["action"].ToString();
                    // HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);
                    var body = JsonConvert.SerializeObject(responseObject);
                    TempData["productsExisting"] = body;
                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }

        public JsonResult InventoryGetSaleOrderDetail(string InvoiceNumber)
        {

            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/InventorySaleGetByInvoiceNumber" + "/" + InvoiceNumber, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<PosSale>> response = JsonConvert.DeserializeObject<ResponseBack<List<PosSale>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    var actionname = "Get inventory sale by invoice number";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }



        public JsonResult InventorySupplierDetail(int id)
        {
            controllername = RouteData.Values["controller"].ToString();
            var userDetail = ViewData["CurrentUserDetail"] as UserDetailAuthToken;

            var Msg = "";
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/InventorySupplierGetByVendorid" + "/" + id, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Vendor>> response = JsonConvert.DeserializeObject<ResponseBack<List<Vendor>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;


                    var actionname = "Get inventory supplier by vendor id";
                    var pageName = RouteData.Values["action"].ToString();
                    HelperClass.activitylog(actionname, controllername, userDetail.Id, userDetail.UserName, pageName);

                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }

        public JsonResult SalesItemsGet(int ItemId)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "Inventory/GetInventoryItemsSales" + "/" + ItemId, "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<InventoryItemsSalesAdp>> response = JsonConvert.DeserializeObject<ResponseBack<List<InventoryItemsSalesAdp>>>(ress.Resp);
                if (response.Data != null)
                {
                    var responseObject = response.Data;

                    return Json(responseObject);
                }
                else
                {
                    return Json("false");
                }
            }
            return Json("false");
        }

        public IActionResult SyncData()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SyncData(SyncSQL data, IFormFile filetosync)
        {

            string constr = "Data Source=.; Initial Catalog=master; Integrated Security=true;Connection Timeout=180;";
            try
            {
                string strQuery = string.Empty;

                SqlConnection con4 = new SqlConnection(constr);
                SqlCommand cmd4 = new SqlCommand();
                strQuery = string.Empty;
                strQuery = "RESTORE FILELISTONLY FROM DISK= N'" + filetosync.Name + "'";
                con4.Open();
                cmd4.CommandText = strQuery;
                cmd4.Connection = con4;

                SqlDataAdapter da = new SqlDataAdapter();
                DataTable dt = new DataTable();
                da.SelectCommand = cmd4;
                da.Fill(dt);
                con4.Close();
                dt.AcceptChanges();
                string LogicalName1 = dt.Rows[0].ItemArray[0].ToString();
                string LogicalName2 = dt.Rows[1].ItemArray[0].ToString();

                con = new SqlConnection(constr);
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                strQuery = " use master RESTORE DATABASE " + filetosync.Name + "  FROM DISK='" + filetosync.Name + "' WITH REPLACE, MOVE N'" + LogicalName1.ToString() + "' TO N'" + filetosync.Name + "\\Database\\" + filetosync.Name + "_Data.mdf',  MOVE N'" + LogicalName2.ToString() + "' TO N'" + LogicalName2.ToString() + "\\Database\\" + filetosync.Name + "_log.ldf'";

                SqlCommand cmd = new SqlCommand(strQuery, con);
                cmd.CommandText = strQuery;
                cmd.ExecuteNonQuery();
                con.Close();
                return Json(true);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);

            }
            return Json(true);
        }


        [HttpPost]
        public IActionResult AdditemsDocument(ItemDocument obj, IFormFile file)
        {
            try
            {
                var Msg = "";
                if (file != null && file.Length > 0)
                {
                    if (file != null)
                    {
                        var input = file.OpenReadStream();
                        var extension = Path.GetExtension(file.FileName);
                        if (extension == ".pdf" || extension == ".PDF")
                        {
                            byte[] byteData = null, buffer = new byte[input.Length];
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;
                                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, read);
                                }
                                byteData = ms.ToArray();
                            }
                            obj.Image = byteData;
                        }
                    }
                }
                //"{\"DocumentId\":0,\"DocumentType\":\"2\",\"DocumentName\":\"texting\",\"Image\":\"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zzzzzzzzzzzzzzzzzzzzzzzwf2+YU129esIMD8+tcMNWI9Z7z/3/AP8ATzLPDrL7vnTzzzzz3xPz/AaDj2IDvqfPPPPPPPPPPPPPPPPPPPPPPPPPAbZ7Cqvqf6wgwP8A+sNf90ox2pvXHjr/AG04EKCCCCCCCCCCCCCCCCCdVu+Zj8t08888888888888888888888888Du1Qpo8m3PEJF67H/tN9BvFFBV+vOfeMOt5yyyyyyyyyyyyyyyyyhHGk34/9m9999999999999999999999999Ab+UkF0hUpA587ibzxxs6nfuN9+//AP8A/wC/gDBDDDDDDDDDDDDDDDD4ADQgxz888888888888888888888888888RC7X49BBepA+/rUKDX9vP1jxEW/4HTJr1rHD3Vaxnf9ed2AAAAAAAAAAAAA888888888888888888888888888BCIes8BdcTC9/rUPV/wDVlUwlSnP/AL//AP8A/qwAAAAAAAAAAFQAAAAAAAAAAAAPPPPPPPPPPPPPPPPPPPPPPPPPPPAQqP8A3wH3wEL3+tSSvXzuV2tU17//AP8A/wD/APrAAAAAAAAAAAAAAAAAAAAAAAAA888888888888888888888888888NCg+55B98BC9/rWR6W8pPCt6a54311wx2zx4x27x18xw/0z27yzwxx4xw1wyIBAAABJBEJFBNAABNBEBEBBBOJ6h+/wC4AffAyvc50ZV3v6QFHibIX8dFVf8AvvR3zMxnRrHu8rjFyvXB2m2P6VHAGXD0Lj4Fmb2sIF87aNJaBDREMItfbvukHXxT70Pvfqv3+wsMBtew63x5w8845p3+f857gBEILC+qbNNP24N/gEv6zpM0wrF01bcznzzmc6CKfaMD6tatelXf5C5Mwwpf/wC89/rBBBF/999999999BA8++KCCCpCWBYt+ec+qr+rDr/Wke7lpB+2WX+/+/oX+Xf/AK/EXQVaUwE3/tfhX96V/wD77PjQ8/6zzyU88AABrLOMEQFHHGGxx6Z6BPMIHlkjzy/EDH2havRPhHDbq2UQJEq/cNV2WwkGphpOndFIy+BABI8xunz/AG01544088888888+9y9CgD/ACdpAfKci0UkfRvluyKX0qQ1Bjj73vvDa7XfVPB2lNsgF64iwyAYMVqgm1zbfLMMi4oc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MSAwIFIvTGFzdCAxMSAwIFIKPj4KZW5kb2JqCgoxMSAwIG9iago8PC9Db3VudCAwL1RpdGxlPEZFRkYwMDUwMDA2MTAwNjcwMDY1MDAyMDAwMzE+Ci9EZXN0WzEgMCBSL1hZWiAwIDc5MiAwXS9QYXJlbnQgMTAgMCBSPj4KZW5kb2JqCgo3IDAgb2JqCjw8L1R5cGUvUGFnZXMKL1Jlc291cmNlcyA5IDAgUgovTWVkaWFCb3hbIDAgMCA2MTIgNzkyIF0KL0tpZHNbIDEgMCBSIF0KL0NvdW50IDE+PgplbmRvYmoKCjEyIDAgb2JqCjw8L1R5cGUvQ2F0YWxvZy9QYWdlcyA3IDAgUgovT3BlbkFjdGlvblsxIDAgUiAvWFlaIG51bGwgbnVsbCAwXQovT3V0bGluZXMgMTAgMCBSCj4+CmVuZG9iagoKMTMgMCBvYmoKPDwvQ3JlYXRvcjxGRUZGMDA0NDAwNzIwMDYxMDA3Nz4KL1Byb2R1Y2VyPEZFRkYwMDRDMDA2OTAwNjIwMDcyMDA2NTAwNEYwMDY2MDA2NjAwNjkwMDYzMDA2NTAwMjAwMDM2MDAyRTAwMzQ+Ci9DcmVhdGlvbkRhdGUoRDoyMDIxMTExNzEzNDE0MyswMycwMCcpPj4KZW5kb2JqCgp4cmVmCjAgMTQKMDAwMDAwMDAwMCA2NTUzNSBmIAowMDAwMTg3MDMxIDAwMDAwIG4gCjAwMDAwMDAwMTkgMDAwMDAgbiAKMDAwMDAwMDIxMCAwMDAwMCBuIAowMDAwMDAwMjMwIDAwMDAwIG4gCjAwMDAxODU2NzMgMDAwMDAgbiAKMDAwMDE4Njg5MSAwMDAwMCBuIAowMDAwMTg3MzM0IDAwMDAwIG4gCjAwMDAxODY5MTIgMDAwMDAgbiAKMDAwMDE4NjkzNCAwMDAwMCBuIAowMDAwMTg3MTczIDAwMDAwIG4gCjAwMDAxODcyMjkgMDAwMDAgbiAKMDAwMDE4NzQzMiAwMDAwMCBuIAowMDAwMTg3NTMzIDAwMDAwIG4gCnRyYWlsZXIKPDwvU2l6ZSAxNC9Sb290IDEyIDAgUgovSW5mbyAxMyAwIFIKL0lEIFsgPDAxQjc5OEFDOTRBN0JERDE0M0JDNThBNzE1NTY1MzhBPgo8MDFCNzk4QUM5NEE3QkREMTQzQkM1OEE3MTU1NjUzOEE+IF0KL0RvY0NoZWNrc3VtIC9ENkFEMzM1RkQxODEwN0ZFMjMyNzYxQ0Q4MEM4MDQwQQo+PgpzdGFydHhyZWYKMTg3NzAwCiUlRU9GCg==\",\"ImageByPath\":null,\"SupplierId\":7}"
                var body = JsonConvert.SerializeObject(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ItemDocumentCreate", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    Msg = "Add Successfully";
                    var jsonreturn = Json(resp.Status);
                    return jsonreturn;
                }
                else
                {
                    Msg = resp.Resp + " " + "Unable To Add";
                    return Json("false");
                }
            }
            catch (Exception ex)
            {
                return Json("false");
            }
        }

        public IActionResult DeleteAttachement(int id)
        {
            try
            {
                var Msgg = "";
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "Inventory/DeleteItemsAttachement" + "/" + id, "Delete");
                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        Msgg = "Add Successfully";
                        return Json(Msgg);
                    }
                    else
                    {
                        Msgg = "Add Successfully";
                        return Json(Msgg);
                    }
                }
                return RedirectToAction("InventoryProfessional");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

}
