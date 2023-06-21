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
    public class EmployeeDeductionController : Controller
    {
        // EmpDeduction CRUD Start here
        public IActionResult AddEmployeeDeduction()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack< List<Employee>> response =
                             JsonConvert.DeserializeObject< ResponseBack < List <Employee>>>(ress.Resp);
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


            SResponse resdeduc = RequestSender.Instance.CallAPI("api",
                "EmpDeductionTypes", "GET");
            if (resdeduc.Status && (resdeduc.Resp != null) && (resdeduc.Resp != ""))
            {
                ResponseBack < List <EmpDeductionType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(resdeduc.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpDeductionType> responseObject = response.Data;

                    ViewBag.EmployeeDeduction = new SelectList(responseObject, "EmpDeductionTypeId", "EmpDeductionTypeName");
                }
                else
                {
                    List<EmpDeductionType> listEmpNo = new List<EmpDeductionType>();
                    ViewBag.EmployeeDeduction = new SelectList(listEmpNo, "EmpDeductionTypeId", "EmpDeductionTypeName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpDeductionType> listEmpNo = new List<EmpDeductionType>();
                ViewBag.EmployeeDeduction = new SelectList(listEmpNo, "EmpDeductionTypeId", "EmpDeductionTypeName");
            }

            return View();
        }

        [HttpPost]
        public IActionResult AddEmployeeDeduction(EmpDeduction empDeduction)
        {
            try
            {


                var body = JsonConvert.SerializeObject(empDeduction);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpDeductions", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddDeduction"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeDeduction");
                }
                else
                {
                    TempData["responseofAddDeduction"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeDeduction");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult UpdateEmployeeDeduction(int id)
        {
            try
            {

                SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack <List <Employee>> response =
                                 JsonConvert.DeserializeObject< ResponseBack < List <Employee>>>(ress.Resp);
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


                SResponse resdeduc = RequestSender.Instance.CallAPI("api",
                    "EmpDeductionTypes", "GET");
                if (resdeduc.Status && (resdeduc.Resp != null) && (resdeduc.Resp != ""))
                {
                    ResponseBack < List <EmpDeductionType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(resdeduc.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpDeductionType> responseObject = response.Data;

                        ViewBag.EmployeeDeduction = new SelectList(responseObject, "EmpDeductionTypeId", "EmpDeductionTypeName");
                    }
                    else
                    {
                        List<EmpDeductionType> listEmpNo = new List<EmpDeductionType>();
                        ViewBag.EmployeeDeduction = new SelectList(listEmpNo, "EmpDeductionTypeId", "EmpDeductionTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpDeductionType> listEmpNo = new List<EmpDeductionType>();
                    ViewBag.EmployeeDeduction = new SelectList(listEmpNo, "EmpDeductionTypeId", "EmpDeductionTypeName");
                }




                SResponse ressget = RequestSender.Instance.CallAPI("api",
               "EmpDeductions" + "/" + id, "GET");
                if (ressget.Status && (ressget.Resp != null) && (ressget.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateEmpDeduction>>(ressget.Resp);
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
        public IActionResult UpdateEmployeeDeduction(int id, EmpDeduction empDeduction)
        {
            try
            {

                empDeduction.EmpDeductionId = id;
                var body = JsonConvert.SerializeObject(empDeduction);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpDeductions" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateDeduction"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeDeduction");
                }
                else
                {
                    TempData["responseUpdateDeduction"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddEmployeeDeduction");
                }



            }
            catch (Exception)
            {

                throw;
            }


        }
        ResponseBack<List<EmpDeductionType>> responseDeductionss;
        public IActionResult ManageDeductionType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductionTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                 responseDeductionss =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(ress.Resp);
                if (responseDeductionss.Data.Count() > 0)
                {
                   var  responseObjectDeductionTye = responseDeductionss.Data;
                    return View(responseObjectDeductionTye);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        //this is list of emplyee
        ResponseBack<List<Employee>> response1;
        public IActionResult ListforEmployees()
        {
            SResponse ressemploye = RequestSender.Instance.CallAPI("api",
                                     "HR/EmployeeGet", "GET");
            if (ressemploye.Status && (ressemploye.Resp != null) && (ressemploye.Resp != ""))
            {
                response1 =
                           JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressemploye.Resp);


            }

            return View();
        }

        public IActionResult ManageEmployeeDeduction()
        {
            List<EmpDeduction> responseObjectEmpDed;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductions", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpDeduction>> response =
                             JsonConvert.DeserializeObject<ResponseBack < List <EmpDeduction>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpDed = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    ManageDeductionType();
                    ViewBag.EmpDeductioType = responseDeductionss.Data;




                    return View(responseObjectEmpDed);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }


            return View();
        }


        public IActionResult DeleteEmployeeDeduction(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpDeductions" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteDeduction"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeDeduction");
                    }
                    else
                    {
                        TempData["MsgDeleteDeduction"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeDeduction");
                    }
                }
                return RedirectToAction("ManageEmployeeDeduction");
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // EmpDeduction Crud End Here

        //Deduction Approvells
        public IActionResult DeductionApproval()
        {

            List<EmpDeduction> responseObjectEmpDed;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductions", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpDeduction>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeduction>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpDed = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    ManageDeductionType();
                    ViewBag.EmpDeductioType = responseDeductionss.Data;




                    return View(responseObjectEmpDed);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyDeductionApproval(bool IsApprove, int EmpDeductionId)
        {
            EmpDeduction empDeduction = new EmpDeduction();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpDeductions" + "/" + EmpDeductionId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpDeduction>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empDeduction.EmpDeductionId = EmpDeductionId;
                        empDeduction.IsApprove = IsApprove;
                        empDeduction.EmployeeId = responseObject.EmployeeId;
                        empDeduction.EmpDeductionTypeId = responseObject.EmpDeductionTypeId;
                        empDeduction.Reason = responseObject.Reason;
                        empDeduction.Date = responseObject.Date;
                        empDeduction.Amount = responseObject.Amount;

                        var body = JsonConvert.SerializeObject(empDeduction);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpDeductions" + "/" + EmpDeductionId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("DeductionApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("DeductionApproval");
                    }
                }
                return RedirectToAction("DeductionApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}