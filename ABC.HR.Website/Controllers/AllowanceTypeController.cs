using ABC.EFCore.Repository.Edmx;
using ABC.HR.Domain.DataConfig;
using ABC.HR.Domain.Entities;
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
using static ABC.HR.Domain.DataConfig.RequestSender;
using EmpAllowanceType = ABC.EFCore.Repository.Edmx.EmpAllowanceType;

namespace ABC.HR.Website.Controllers
{
    //[Area("PayRoll")]
    public class AllowanceTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //AllownceType Crud Start Here

        public IActionResult AddAllowanceType()
        {
            return View();
        }

        [HttpPost]

        public IActionResult AddAllowanceType(EmpAllowanceType allowanceType)
        {
            try
            {
                var body = JsonConvert.SerializeObject(allowanceType);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowanceTypes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddAllownceType"] = "Added Successfully";
                    return RedirectToAction("ManageAllowanceType");
                }
                else
                {
                    TempData["responseofAddAllownceType"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddAllowanceType");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //Get List of AllownceType
        ResponseBack <List<EmpAllowanceType>> responseOfAllownceType;
        public IActionResult ManageAllowanceType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowanceTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                responseOfAllownceType =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowanceType>>>(ress.Resp);
                if (responseOfAllownceType.Data.Count() > 0)
                {
                    List<EmpAllowanceType> responseObject = responseOfAllownceType.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }


        //get the row want to update
        public IActionResult UpdateAllowanceType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "EmpAllowanceTypes" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateAllowanceType>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["Msg"] = "Server is down.";
                    }
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public IActionResult UpdateAllowanceType(int id, EmpAllowanceType empAllowanceType)
        {


            try
            {

               /// empAllowanceType.EmpAllowanceTypeId = id;
                var body = JsonConvert.SerializeObject(empAllowanceType);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowanceTypes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateAllowanceType"] = "Updated Successfully";
                    return RedirectToAction("ManageAllowanceType");
                }
                else
                {
                    TempData["UpdateAllowanceType"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddAllowanceType");
                }

                //COMMENT AFTER OTHER CODE 
                //return View();

            }
            catch (Exception)
            {

                throw;
            }

        }

        //[HttpDelete]
        public IActionResult DeleteAllowanceType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpAllowanceTypes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteAllowanceType"] = "Delete Successfully";
                        return RedirectToAction("ManageAllowanceType");
                    }
                    else
                    {
                        TempData["MsgDeleteAllowanceType"] = "Delete Successfully";
                        return RedirectToAction("ManageAllowanceType");
                    }
                }
                return RedirectToAction("ManageAllowanceType");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        //AllownceType Crud End Here
    }
}