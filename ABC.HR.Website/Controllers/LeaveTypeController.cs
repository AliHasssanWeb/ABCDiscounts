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
    public class LeaveTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //EmpLeaveType Crud Strat Here


        public IActionResult AddLeaveType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLeaveType(EmpLeaveType empLeave)
        {


            try
            {
                var body = JsonConvert.SerializeObject(empLeave);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLeaveTypes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddLeaveType"] = "Add Successfully";
                    return RedirectToAction("ManageLeaveType");
                }
                else
                {
                    TempData["responseofAddLeaveType"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddLeaveType");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        List<EmpLeaveType> responseObjectLeaveType;
        public IActionResult ManageLeaveType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLeaveTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpLeaveType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    responseObjectLeaveType = response.Data;
                    return View(responseObjectLeaveType);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }

        public IActionResult UpdateLeaveType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "EmpLeaveTypes" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateLeaveType>>(ress.Resp);
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
        public IActionResult UpdateLeaveType(int id, EmpLeaveType empLeaveType)
        {
            try
            {

                empLeaveType.EmpLeaveTypeId = id;
                var body = JsonConvert.SerializeObject(empLeaveType);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLeaveTypes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateLeaveType"] = "Updated Successfully";
                    return RedirectToAction("ManageLeaveType");
                }
                else
                {
                    TempData["responseUpdateLeaveType"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddLeaveType");
                }



            }
            catch (Exception)
            {

                throw;
            }

        }


        public IActionResult DeleteLeaveType(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpLeaveTypes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteLeaveType"] = "Delete Successfully";
                        return RedirectToAction("ManageLeaveType");
                    }
                    else
                    {
                        TempData["MsgDeleteLeaveType"] = "Delete Successfully";
                        return RedirectToAction("ManageLeaveType");
                    }
                }
                return RedirectToAction("ManageLeaveType");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //EmpLeaveType Crud End Here
    }
}