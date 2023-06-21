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
    public class EmployeeTaxController : Controller
    {
        //EmpTax Crud Start Here
        public IActionResult AddEmployeeTax()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Employee>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Employee> responseObject = response.Data;

                    //ViewBag.EmployeeNo = new SelectList(responseObject, "EmployeeId", "FullName");
                    ViewBag.EmployeeNo = new SelectList((from s in responseObject
                                                         select new
                                                         {
                                                             empid = s.EmployeeId,

                                                             fullName = s.EmployeeCode + "- " + s.FullName
                                                         }), "empid", "fullName");
                }
                else
                {
                    List<Employee> listEmpNo = new List<Employee>();
                    ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<Employee> listEmpNo = new List<Employee>();
                ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
            }

            //Dropdwon Emploeee Loan Type

            SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
            if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantype.Resp);
                if (empLoanTypesres.Data.Count() > 0)
                {
                    List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                    ViewBag.EmpTaxType = new SelectList(empLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                }
                else
                {
                    List<EmpTaxType> listEmpLoan = new List<EmpTaxType>();
                    ViewBag.EmpTaxType = new SelectList(listEmpLoan, "EmpTaxTypeId", "EmpTaxTypeName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpTaxType> listLoanTypes = new List<EmpTaxType>();
                ViewBag.EmpTaxType = new SelectList(listLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
            }

            return View();
        }
        [HttpPost]
        public IActionResult AddEmployeeTax(EmpTax empTax)
        {
            try
            {
                var body = JsonConvert.SerializeObject(empTax);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxes", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                  var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(resp.Resp);
                    if (response.Status == 10)
                    {
                        TempData["responseofAddTax"] = "Data already exits!";
                        return RedirectToAction("AddEmployeeTax");
                    }
                    TempData["responseofAddTax"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeTax");
                }
                else
                {
                    TempData["responseofAddTax"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeTax");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult ManageEmployeeTax()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                SResponse ressp = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeGet", "GET");
                if (ressp.Status && (ressp.Resp != null) && (ressp.Resp != ""))
                {
                    ResponseBack<List<Employee>> responsee =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressp.Resp);
                    if (responsee.Data.Count() > 0)
                    {
                        List<Employee> responseObject = responsee.Data;

                        ViewBag.EmployeeNo = responseObject;
                    }
                    else
                    {
                        List<Employee> listEmpNo = new List<Employee>();
                        ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<Employee> listEmpNo = new List<Employee>();
                    ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                }

                SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
                if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantype.Resp);
                    if (empLoanTypesres.Data.Count() > 0)
                    {
                        List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                        ViewBag.EmpTaxType = empLoanTypes;

                    }
                    else
                    {
                        List<EmpTaxType> listEmpLoan = new List<EmpTaxType>();
                        ViewBag.EmpTaxType = new SelectList(listEmpLoan, "EmpTaxTypeId", "EmpTaxTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpTaxType> listLoanTypes = new List<EmpTaxType>();
                    ViewBag.EmpTaxType = new SelectList(listLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                }

                ResponseBack<List<EmpTax>> response =
                         JsonConvert.DeserializeObject<ResponseBack<List<EmpTax>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpTax> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();

        }
        public IActionResult UpdateEmployeeTax(int id)
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Employee>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Employee> responseObject = response.Data;

                    //ViewBag.EmployeeNo = new SelectList(responseObject, "EmployeeId", "FullName");
                    ViewBag.EmployeeNo = new SelectList((from s in responseObject
                                                         select new
                                                         {
                                                             empid = s.EmployeeId,

                                                             fullName = s.EmployeeCode + "- " + s.FullName
                                                         }), "empid", "fullName");
                }
                else
                {
                    List<Employee> listEmpNo = new List<Employee>();
                    ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<Employee> listEmpNo = new List<Employee>();
                ViewBag.EmployeeNo = new SelectList(listEmpNo, "EmployeeId", "FullName");
            }

            //Dropdwon Emploeee Loan Type

            SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
            if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantype.Resp);
                if (empLoanTypesres.Data.Count() > 0)
                {
                    List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                    ViewBag.EmpTaxType = new SelectList(empLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                }
                else
                {
                    List<EmpTaxType> listEmpLoan = new List<EmpTaxType>();
                    ViewBag.EmpTaxType = new SelectList(listEmpLoan, "EmpTaxTypeId", "EmpTaxTypeName");
                    TempData["response"] = "Server is down.";
                }
            }

            SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpTaxes" + "/" + id, "GET");
            if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<ValidateEmpTax>>(ress12.Resp);
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
        [HttpPost]
        public IActionResult UpdateEmployeeTax(int id, EmpTax empTax)
        {
            try
            {

                empTax.EmpTaxId = id;
                var body = JsonConvert.SerializeObject(empTax);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxes" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<Employee>>(resp.Resp);
                    if (response.Status == 10)
                    {
                        TempData["responseofAddTax"] = "Data already exits!";
                        return RedirectToAction("UpdateEmployeeTax", new { id = id });
                    }
                    TempData["responseUpdateTax"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeTax");
                }
                else
                {
                    TempData["responseUpdatetax"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddEmployeeTax");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult DeleteEmployeeTax(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpTaxes" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteTax"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeTax");
                    }
                    else
                    {
                        TempData["MsgDeleteTax"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeTax");
                    }
                }
                return RedirectToAction("ManageEmployeeTax");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //get salary for selected employee
        public IActionResult GetEmplyeeSalary(int empidforsal)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeContractGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmployeeContract>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmployeeContract>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {


                        for (int i = 0; i < response.Data.Count(); i++)
                        {
                            var responseObject = response.Data.Where(x => x.EmployeeId == empidforsal);
                            if (responseObject.Count() > 0)
                            {
                                return Json(responseObject);
                            }
                        }




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
        [HttpGet]
        public IActionResult GetEmplyeeDetails(int empid)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeDetail/" + empid, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ress.Resp);
                    if (response.Data != null)
                    {
                        if (response.Data.Count() > 0)
                        {
                            var responseObject = response.Data;
                            return Json(responseObject);
                        }
                        else
                        {
                            return Json(null);
                        }

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
        [HttpGet]
        public IActionResult GetTaxValue(int taxidval)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/TaxDetail/" + taxidval, "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<EmpTaxType> response = JsonConvert.DeserializeObject<ResponseBack<EmpTaxType>>(ress.Resp);
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
                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        public IActionResult CalculatEmployeeTax(int empidforallonce)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                  "EmpTaxTypes", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {


                        for (int i = 0; i < response.Data.Count(); i++)
                        {
                            var responseObject = response.Data.Where(x => x.EmpTaxTypeId == empidforallonce);
                            if (responseObject.Count() > 0)
                            {
                                return Json(responseObject);
                            }
                        }




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

        //EmpTax Crud End Here
        //Tax Approvells
        public IActionResult TaxApproval()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                             "EmpTaxes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                SResponse ressp = RequestSender.Instance.CallAPI("api",
                 "HR/EmployeeGet", "GET");
                if (ressp.Status && (ressp.Resp != null) && (ressp.Resp != ""))
                {
                    ResponseBack<List<Employee>> responsee =
                                 JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressp.Resp);
                    if (responsee.Data.Count() > 0)
                    {
                        List<Employee> responseObject = responsee.Data;

                        ViewBag.EmployeeNo = responseObject;
                    }
                }


                SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
                if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantype.Resp);
                    if (empLoanTypesres.Data.Count() > 0)
                    {
                        List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                        ViewBag.EmpTaxType = empLoanTypes;

                    }
                }

                ResponseBack<List<EmpTax>> response =
                         JsonConvert.DeserializeObject<ResponseBack<List<EmpTax>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpTax> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyTaxApproval(bool IsApprove, int EmpTaxId)
        {
            EmpTax empTax = new EmpTax();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpTaxes" + "/" + EmpTaxId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpTax>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empTax.EmpTaxId = EmpTaxId;
                        empTax.IsApprove = IsApprove;
                        empTax.EmployeeId = responseObject.EmployeeId;
                        empTax.Date = responseObject.Date;
                        empTax.Amount = responseObject.Amount;
                        empTax.EmpTaxTypeId = responseObject.EmpTaxTypeId;
                        empTax.Reason = responseObject.Reason;

                        var body = JsonConvert.SerializeObject(empTax);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxes" + "/" + EmpTaxId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("TaxApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("TaxApproval");
                    }
                }
                return RedirectToAction("TaxApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}