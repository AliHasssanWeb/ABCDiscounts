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
    public class EmployeeAllowanceController : Controller
    {
        //EmpAllownce Crud Start Here

        public IActionResult AddEmployeeAllowance()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List <Employee>> response =
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

            SResponse res = RequestSender.Instance.CallAPI("api",
                 "EmpAllowanceTypes", "GET");
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                ResponseBack < List <EmpAllowanceType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowanceType>>>(res.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpAllowanceType> responseObject = response.Data;
                    ViewBag.AllownceTypes = new SelectList(responseObject, "EmpAllowanceTypeId", "AllowanceTypeName");
                }
                else
                {
                    List<EmpAllowanceType> listAllownceTypes = new List<EmpAllowanceType>();
                    ViewBag.AllownceTypes = new SelectList(listAllownceTypes, "EmpAllowanceTypeId", "AllowanceTypeName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpAllowanceType> listAllownceTypes = new List<EmpAllowanceType>();
                ViewBag.AllownceTypes = new SelectList(listAllownceTypes, "EmpAllowanceTypeId", "AllowanceTypeName");
            }
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployeeAllowance(int EmployeeId, int AllowanceTypeId, string Amount, DateTime Date, string Reason, bool IsApprove, EmpAllowance empAllowance)
        {
            try
            {
                empAllowance.EmployeeId = EmployeeId;
                empAllowance.AllowanceTypeId = AllowanceTypeId;
                empAllowance.Amount = Amount;
                empAllowance.Date = Date;
                empAllowance.Reason = Reason;
                empAllowance.IsApprove = IsApprove;

                var date = empAllowance.Date;
                empAllowance.Date = Convert.ToDateTime(date);

                var body = JsonConvert.SerializeObject(empAllowance);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowances", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddAllownce"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeAllowance");
                }
                else
                {
                    TempData["responseofAddAllownce"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeAllowance");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //Get List of AllownceType
        ResponseBack<List<EmpAllowanceType>> responseOfAllownceType;
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
        public IActionResult ManageEmployeeAllowance()
        {

            List<EmpAllowance> responseObjectEmpAllownce;


            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpAllowance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    ManageAllowanceType();
                    ViewBag.EmpAllownceType = responseOfAllownceType.Data;




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();

        }
        public IActionResult UpdateEmployeeAllowance(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack < List <Employee>> response =
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

                SResponse res = RequestSender.Instance.CallAPI("api",
                     "EmpAllowanceTypes", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack < List <EmpAllowanceType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowanceType>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpAllowanceType> responseObject = response.Data;
                        ViewBag.AllownceTypes = new SelectList(responseObject, "EmpAllowanceTypeId", "AllowanceTypeName");
                    }
                    else
                    {
                        List<EmpAllowanceType> listAllownceTypes = new List<EmpAllowanceType>();
                        ViewBag.AllownceTypes = new SelectList(listAllownceTypes, "EmpAllowanceTypeId", "AllowanceTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpAllowanceType> listAllownceTypes = new List<EmpAllowanceType>();
                    ViewBag.AllownceTypes = new SelectList(listAllownceTypes, "EmpAllowanceTypeId", "AllowanceTypeName");
                }







                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpAllowances" + "/" + id, "GET");
                if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateAllowance>>(ress12.Resp);
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
        public IActionResult UpdateEmployeeAllowance(int id, EmpAllowance empAllowance)
        {

            try
            {

                empAllowance.EmpAllowanceId = id;
                var body = JsonConvert.SerializeObject(empAllowance);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowances" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateAllownce"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeAllowance");
                }
                else
                {
                    TempData["responseUpdateAllownce"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageEmployeeAllowance");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteEmployeeAllowance(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpAllowances" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteAllownce"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeAllowance");
                    }
                    else
                    {
                        TempData["MsgDeleteAllownce"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeAllowance");
                    }
                }
                return RedirectToAction("ManageEmployeeAllowance");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //EmpAllownce Crud End Here

        //Allowance Approvells
        public IActionResult AllowanceApproval()
        {

            List<EmpAllowance> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpAllowance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    ManageAllowanceType();
                    ViewBag.EmpAllownceType = responseOfAllownceType.Data;




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyAllowanceType(bool IsApprove, int EmpAllowanceId)
        {
            EmpAllowance empAllowance = new EmpAllowance();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpAllowances" + "/" + EmpAllowanceId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpAllowance>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empAllowance.EmpAllowanceId = EmpAllowanceId;
                        empAllowance.IsApprove = IsApprove;
                        empAllowance.AllowanceTypeId = responseObject.AllowanceTypeId;
                        empAllowance.EmployeeId = responseObject.EmployeeId;
                        empAllowance.Date = responseObject.Date;
                        empAllowance.Amount = responseObject.Amount;
                        empAllowance.Reason = responseObject.Reason;

                        var body = JsonConvert.SerializeObject(empAllowance);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowances" + "/" + EmpAllowanceId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("AllowanceApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("AllowanceApproval");
                    }
                }
                return RedirectToAction("AllowanceApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}