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
    public class EmployeeLoanController : Controller
    {
        //EmpLoan Crud Start Here

        public IActionResult AddEmployeeLoan()
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

            //Dropdwon Emploeee Loan Type

            SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpLoanTypes", "GET");
            if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
            {
               ResponseBack<List<EmpLoanType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpLoanType>>>(ressloantype.Resp);
                if (empLoanTypesres.Data.Count() > 0)
                {
                    List<EmpLoanType> empLoanTypes = empLoanTypesres.Data;
                    ViewBag.EmpLoanType = new SelectList(empLoanTypes, "EmpLoanTypeId", "EmpLoanTypeName");
                }
                else
                {
                    List<EmpLoanType> listEmpLoan = new List<EmpLoanType>();
                    ViewBag.EmpLoanType = new SelectList(listEmpLoan, "EmpLoanTypeId", "EmpLoanTypeName");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpLoanType> listLoanTypes = new List<EmpLoanType>();
                ViewBag.EmpLoanType = new SelectList(listLoanTypes, "EmpLoanTypeId", "EmpLoanTypeName");
            }


            return View();
        }


        [HttpPost]
        public IActionResult AddEmployeeLoan(EmpLoan empLoan)
        {
            try
            {

                var body = JsonConvert.SerializeObject(empLoan);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLoans", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddLoan"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeLoan");
                }
                else
                {
                    TempData["responseofAddLoan"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeLoan");
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
        
        ResponseBack<List<EmpLoanType>> responseval;
        public IActionResult ManageLoanType()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoanTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                 responseval =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLoanType>>>(ress.Resp);
                if (responseval.Data.Count() > 0)
                {
                    List<EmpLoanType> responseObjectLoanType = responseval.Data;
                    return View(responseObjectLoanType);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();
        }
        public IActionResult ManageEmployeeLoan()
        {
            List<EmpLoan> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoans", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack <List<EmpLoan>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLoan>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    SResponse ressemploye = RequestSender.Instance.CallAPI("api",
                                     "HR/EmployeeGet", "GET");
                    if (ressemploye.Status && (ressemploye.Resp != null) && (ressemploye.Resp != ""))
                    {
                        ResponseBack< List<Employee>> employees =
                                  JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressemploye.Resp);

                        ViewBag.EmployeeName = employees.Data;
                    }
                    
                    ManageLoanType();
                    ViewBag.EmpLoanType = responseval.Data;




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }






            return View();
        }

        public IActionResult UpdateEmployeeLoan(int id)
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



                SResponse ressloantype = RequestSender.Instance.CallAPI("api", "EmpLoanTypes", "GET");
                if (ressloantype.Status && (ressloantype.Resp != null) && (ressloantype.Resp != ""))
                {
                    ResponseBack < List <EmpLoanType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpLoanType>>>(ressloantype.Resp);
                    if (empLoanTypesres.Data.Count() > 0)
                    {
                        List<EmpLoanType> empLoanTypes = empLoanTypesres.Data;
                        ViewBag.EmpLoanType = new SelectList(empLoanTypes, "EmpLoanTypeId", "EmpLoanTypeName");
                    }
                    else
                    {
                        List<EmpLoanType> listEmpLoan = new List<EmpLoanType>();
                        ViewBag.EmpLoanType = new SelectList(listEmpLoan, "EmpLoanTypeId", "EmpLoanTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpLoanType> listLoanTypes = new List<EmpLoanType>();
                    ViewBag.EmpLoanType = new SelectList(listLoanTypes, "EmpLoanTypeId", "EmpLoanTypeName");
                }





                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpLoans" + "/" + id, "GET");
                if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<ValidateEmpLoan>>(ress12.Resp);
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
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        public IActionResult UpdateEmployeeLoan(int id, EmpLoan empLoan)
        {

            try
            {

                empLoan.EmpLoanId = id;
                var body = JsonConvert.SerializeObject(empLoan);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLoans" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateLoan"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeLoan");
                }
                else
                {
                    TempData["responseUpdateLoan"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("AddEmployeeLoan");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteEmployeeLoan(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpLoans" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteLoan"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLoan");
                    }
                    else
                    {
                        TempData["MsgDeleteLoan"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeLoan");
                    }
                }
                return RedirectToAction("ManageEmployeeLoan");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //EmpLoan Crud End Here
        //Loan Approvells
        public IActionResult LoanApproval()
        {

            List<EmpLoan> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoans", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack <List <EmpLoan>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLoan>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;
                    ManageLoanType();
                    ViewBag.EmpLoanType = responseval.Data;




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }



            return View();
        }
        public IActionResult ModifyLoanApproval(bool IsApprove, int EmpLoanId)
        {
            EmpLoan empLoan = new EmpLoan();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpLoans" + "/" + EmpLoanId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpLoan>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empLoan.EmpLoanId = EmpLoanId;
                        empLoan.IsApprove = IsApprove;
                        empLoan.EmployeeId = responseObject.EmployeeId;
                        empLoan.EmpLoanTypeId = responseObject.EmpLoanTypeId;
                        empLoan.Reason = responseObject.Reason;
                        empLoan.Date = responseObject.Date;
                        empLoan.Amount = responseObject.Amount;

                        var body = JsonConvert.SerializeObject(empLoan);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpLoans" + "/" + EmpLoanId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("LoanApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("LoanApproval");
                    }
                }
                return RedirectToAction("LoanApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    }
}