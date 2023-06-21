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
    public class EmpAllowanceConfigurationController : Controller
    {
        public IActionResult AddAllowanceConfiguration()
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

            SResponse res = RequestSender.Instance.CallAPI("api",
                 "EmpAllowanceTypes", "GET");
            if (res.Status && (res.Resp != null) && (res.Resp != ""))
            {
                ResponseBack<List<EmpAllowanceType>> response =
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
        public IActionResult AddAllowanceConfiguration(EmpAllowanceTypeEmp empAllowance)
        {
            try
            {             

                var date = empAllowance.Date;
                empAllowance.Date = Convert.ToDateTime(date);

                var body = JsonConvert.SerializeObject(empAllowance);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowanceTypeEmps", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddAllownceCon"] = "Add Successfully";
                    return RedirectToAction("ManageAllowanceConfiguration");
                }
                else
                {
                    TempData["responseofAddAllownceCon"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("ManageAllowanceConfiguration");
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
        public IActionResult ManageAllowanceConfiguration()
        {

            List<EmpAllowanceTypeEmp> responseObjectEmpAllownce;


            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowanceTypeEmps", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpAllowanceTypeEmp>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowanceTypeEmp>>>(ress.Resp);


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
        public IActionResult UpdateAllowanceConfiguration(int id)
        {
            try
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

                SResponse res = RequestSender.Instance.CallAPI("api",
                     "EmpAllowanceTypes", "GET");
                if (res.Status && (res.Resp != null) && (res.Resp != ""))
                {
                    ResponseBack<List<EmpAllowanceType>> response =
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
               "EmpAllowanceTypeEmps" + "/" + id, "GET");
                if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateAllowanceConfig>>(ress12.Resp);
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
        public IActionResult UpdateAllowanceConfiguration(int id, EmpAllowanceTypeEmp empAllowance)
        {

            try
            {

                empAllowance.EmpAllowanceTypeEmpId = id;
                var body = JsonConvert.SerializeObject(empAllowance);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAllowanceTypeEmps" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateAllownceCon"] = "Updated Successfully";
                    return RedirectToAction("ManageAllowanceConfiguration");
                }
                else
                {
                    TempData["responseUpdateAllownceCon"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageAllowanceConfiguration");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteAllowanceConfiguration(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpAllowanceTypeEmps" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteAllownceCon"] = "Delete Successfully";
                        return RedirectToAction("ManageAllowanceConfiguration");
                    }
                    else
                    {
                        TempData["MsgDeleteAllownceCon"] = "Delete Successfully";
                        return RedirectToAction("ManageAllowanceConfiguration");
                    }
                }
                return RedirectToAction("ManageAllowanceConfiguration");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //get Contract for selected employee
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
                            else
                            {
                                return Json("");
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

    }
}
