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
    public class EmployeeLeaveController : Controller
    {
        // EmpLeave CRUD Start here
        public IActionResult AddEmployeeLeave()
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
                "EmpLeaveTypes", "GET");
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                ResponseBack <List <EmpLeaveType>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(res.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpLeaveType> responseObject = response.Data;

                    ViewBag.EmployeeLeaveName = new SelectList(responseObject, "EmpLeaveTypeId", "LeaveTypeName");
                }
                else
                {
                    List<EmpLeaveType> listEmpNo = new List<EmpLeaveType>();
                    ViewBag.EmployeeLeaveName = new SelectList(listEmpNo, "EmpLeaveTypeId", "LeaveTypeName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpLeaveType> listEmpNo = new List<EmpLeaveType>();
                ViewBag.EmployeeLeaveName = new SelectList(listEmpNo, "EmpLeaveTypeId", "LeaveTypeName");
            }
            return View();
        }


        [HttpPost]
        public IActionResult AddEmployeeLeave(EmpLeave empLeave)
        {
            try
            {

                var body = JsonConvert.SerializeObject(empLeave);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLeaves", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddLeave"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeLeave");
                }
                else
                {
                    TempData["responseofAddLeave"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeLeave");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
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

        ResponseBack<List<EmpLeaveType>> responselaveee;
        public IActionResult ManageLeaveType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLeaveTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpLeaveType>> responselaveee =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(ress.Resp);
                if (responselaveee.Data.Count() > 0)
                {
                    var responseObjectLeaveType = responselaveee.Data;
                    return View(responseObjectLeaveType);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }

        public IActionResult ManageEmployeeLeave()
        {
            List<EmpLeave> responseObjectEmpLeave;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLeaves", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
               ResponseBack<List < EmpLeave>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLeave>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpLeave = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    SResponse ressleavetype = RequestSender.Instance.CallAPI("api",
                 "EmpLeaveTypes", "GET");
                    if (ressleavetype.Status && (ressleavetype.Resp != null) && (ressleavetype.Resp != ""))
                    {
                        ResponseBack<List<EmpLeaveType>> responselaveee =
                                     JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(ressleavetype.Resp);
                        if (responselaveee.Data.Count() > 0)
                        {
                            ViewBag.EmpLeaveType = responselaveee.Data;
                            
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }
                    




                    return View(responseObjectEmpLeave);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }

        public IActionResult UpdateEmployeeLeave(int id)
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
                "EmpLeaveTypes", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack < List <EmpLeaveType>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(res.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpLeaveType> responseObject = response.Data;

                        ViewBag.EmployeeLeaveName = new SelectList(responseObject, "EmpLeaveTypeId", "LeaveTypeName");
                    }
                    else
                    {
                        List<EmpLeaveType> listEmpNo = new List<EmpLeaveType>();
                        ViewBag.EmployeeLeaveName = new SelectList(listEmpNo, "EmpLeaveTypeId", "LeaveTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpLeaveType> listEmpNo = new List<EmpLeaveType>();
                    ViewBag.EmployeeLeaveName = new SelectList(listEmpNo, "EmpLeaveTypeId", "LeaveTypeName");
                }




                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpLeaves" + "/" + id, "GET");
                if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateEmpLeave>>(ress12.Resp);
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
        public IActionResult UpdateEmployeeLeave(int id, EmpLeave empLeave)
        {

            try
            {

                empLeave.EmpLeaveId = id;
                var body = JsonConvert.SerializeObject(empLeave);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLeaves" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateLeave"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeLeave");
                }
                else
                {
                    TempData["responseUpdateLeave"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddEmployeeLeave");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteEmployeeLeave(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpLeaves" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteLeave"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLeave");
                    }
                    else
                    {
                        TempData["MsgDeleteLeave"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLeave");
                    }
                }
                return RedirectToAction("ManageEmployeeLeave");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        //End EmpLeave
        //Leave Approvells
        public IActionResult LeaveApproval()
        {

            List<EmpLeave> responseObjectEmpLeave;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLeaves", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpLeave>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLeave>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpLeave = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    SResponse ressleavetype = RequestSender.Instance.CallAPI("api",
                 "EmpLeaveTypes", "GET");
                    if (ressleavetype.Status && (ressleavetype.Resp != null) && (ressleavetype.Resp != ""))
                    {
                        ResponseBack<List<EmpLeaveType>> responselaveee =
                                     JsonConvert.DeserializeObject<ResponseBack<List<EmpLeaveType>>>(ressleavetype.Resp);
                        if (responselaveee.Data.Count() > 0)
                        {
                            ViewBag.EmpLeaveType = responselaveee.Data;
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }
                     
                    




                    return View(responseObjectEmpLeave);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyLeaveApproval(bool IsApprove, int EmpLeaveId)
        {
            EmpLeave empLeave = new EmpLeave();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpLeaves" + "/" + EmpLeaveId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpLeave>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empLeave.EmpLeaveId = EmpLeaveId;
                        empLeave.IsApprove = IsApprove;
                        empLeave.EmployeeId = responseObject.EmployeeId;
                        empLeave.EmpLeaveTypeId = responseObject.EmpLeaveTypeId;
                        empLeave.Reason = responseObject.Reason;
                        empLeave.FromDate = responseObject.FromDate;
                        empLeave.ToDate = responseObject.ToDate;

                        var body = JsonConvert.SerializeObject(empLeave);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLeaves" + "/" + EmpLeaveId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("LeaveApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("LeaveApproval");
                    }
                }
                return RedirectToAction("LeaveApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}