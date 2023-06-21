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
    public class TaxTypeController : Controller
    {
        //EmpTaxType Crud Start Here
        public IActionResult AddTaxType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTaxType(EmpTaxType empTax, string IsErTax)
        {
            try
            {
                if(IsErTax == "Tax")
                {
                    empTax.IsTax = true;
                    empTax.IsErTax = false;
                }
                else
                {
                    empTax.IsTax = false;
                    empTax.IsErTax = true;
                }
                 

                var body = JsonConvert.SerializeObject(empTax);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(resp.Resp);
                    if (response.Status == 10)
                    {
                        TempData["responseofAddTax"] = "Data already exits!";
                        return RedirectToAction("AddTaxType");
                    }
                    TempData["responseofAddTaxType"] = "Add Successfully";
                    return RedirectToAction("ManageTaxType");
                }
                else
                {
                    TempData["responseofAddTaxType"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddTaxType");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult ManageTaxType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "EmpTaxTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpTaxType> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }

        public IActionResult UpdateTaxType(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
               "EmpTaxTypes" + "/" + id, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateTaxType>>(ress.Resp);
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
        public IActionResult UpdateTaxType(int id, EmpTaxType empTaxType)
        {
            try
            {

                empTaxType.EmpTaxTypeId = id;
                var body = JsonConvert.SerializeObject(empTaxType);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxTypes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(resp.Resp);
                    if (response.Status == 10)
                    {
                        TempData["responseofAddTax"] = "Data already exits!";
                        return RedirectToAction("UpdateTaxType", new { id = id });
                    }
                    TempData["responseUpdateTaxType"] = "Updated Successfully";
                    return RedirectToAction("ManageTaxType");
                }
                else
                {
                    TempData["responseUpdateTaxType"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddTaxType");
                }



            }
            catch (Exception)
            {

                throw;
            }

        }


        public IActionResult DeleteTaxType(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpTaxTypes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteTaxType"] = "Delete Successfully";
                        return RedirectToAction("ManageTaxType");
                    }
                    else
                    {
                        TempData["MsgDeleteTaxType"] = "Delete Successfully";
                        return RedirectToAction("ManageTaxType");
                    }
                }
                return RedirectToAction("ManageTaxType");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //EmpTaxType Crud End Here
    }
}