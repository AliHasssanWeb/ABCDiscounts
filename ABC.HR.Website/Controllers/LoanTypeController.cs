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
    public class LoanTypeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //EmpLoanType Crud Start Here

        public IActionResult AddLoanType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddLoanType(EmpLoanType empLoan)
        {

            try
            {
                var body = JsonConvert.SerializeObject(empLoan);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLoanTypes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddLoanType"] = "Add Successfully";
                    return RedirectToAction("ManageLoanType");
                }
                else
                {
                    TempData["response"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddLoanType");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        List<EmpLoanType> responseObjectLoanType;
        public IActionResult ManageLoanType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoanTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack <List<EmpLoanType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLoanType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    responseObjectLoanType = response.Data;
                    return View(responseObjectLoanType);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult UpdateLoanType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "EmpLoanTypes" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateLoanType>>(ress.Resp);
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
        public IActionResult UpdateLoanType(int id, EmpLoanType empLoanType)
        {
            try
            {

                empLoanType.EmpLoanTypeId = id;
                var body = JsonConvert.SerializeObject(empLoanType);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLoanTypes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateLoanType"] = "Updated Successfully";
                    return RedirectToAction("ManageLoanType");
                }
                else
                {
                    TempData["responseUpdateLoanType"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddLoanType");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteLoanType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpLoanTypes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {

                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteLoanType"] = "Delete Successfully";
                        return RedirectToAction("ManageLoanType");
                    }
                    else
                    {
                        TempData["MsgDeleteLoanType"] = "Delete Successfully";
                        return RedirectToAction("ManageLoanType");
                    }
                }
                return RedirectToAction("ManageLoanType");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //EmpLoanType Crud End Here
    }
}