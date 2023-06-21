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

namespace ABC.HR.Website.Controllers
{
    //[Area("PayRoll")]
    public class DeductionTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //EmpDeductionType Crud Start Here

        public IActionResult AddDeductionType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddDeductionType(EmpDeductionType empDeductionType)
        {
            try
            {
                var body = JsonConvert.SerializeObject(empDeductionType);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpDeductionTypes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddDeductionType"] = "Added Successfully";
                    return RedirectToAction("ManageDeductionType");
                }
                else
                {
                    TempData["responseofAddDeductionType"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddDeductionType");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        List<EmpDeductionType> responseObjectDeductionTye;
        public IActionResult ManageDeductionType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductionTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpDeductionType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    responseObjectDeductionTye = response.Data;
                    return View(responseObjectDeductionTye);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }

        public IActionResult UpdateDeductionType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "EmpDeductionTypes" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateDeductionType>>(ress.Resp);
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
        public IActionResult UpdateDeductionType(int id, EmpDeductionType empDeductionType)
        {

            try
            {

                empDeductionType.EmpDeductionTypeId = id;
                var body = JsonConvert.SerializeObject(empDeductionType);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpDeductionTypes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateDeductionType"] = "Updated Successfully";
                    return RedirectToAction("ManageDeductionType");
                }
                else
                {
                    TempData["responseUpdateDeductionType"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddDeductionType");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteDeductionType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpDeductionTypes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteDeductionType"] = "Delete Successfully";
                        return RedirectToAction("ManageDeductionType");
                    }
                    else
                    {
                        TempData["MsgDeleteDeductionType"] = "Delete Successfully";
                        return RedirectToAction("ManageDeductionType");
                    }
                }
                return RedirectToAction("ManageDeductionType");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //EmpDeductionType Crud End Here
    }
}