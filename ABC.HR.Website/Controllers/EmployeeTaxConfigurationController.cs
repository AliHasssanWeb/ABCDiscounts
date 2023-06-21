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
    public class EmployeeTaxConfigurationController : Controller
    {
        //EmpTax Crud Start Here
        public IActionResult AddTaxConfiguration()
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


                    // ViewBag.EmployeeNo = new SelectList(responseObject, "EmployeeId", "FullName");

                    ViewBag.EmployeeNo = new SelectList((from s in responseObject select new { empid = s.EmployeeId,
                    
                    fullName = s.EmployeeCode + "- " + s.FullName }), "empid", "fullName");
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
        public IActionResult AddTaxConfiguration(EmpTaxTypeEmp empTax)
        {
            try
            {
                var body = JsonConvert.SerializeObject(empTax);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxTypeEmps", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddTaxCon"] = "Add Successfully";
                    return RedirectToAction("ManageTaxConfiguration");
                }
                else
                {
                    TempData["responseofAddTaxCon"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddTaxConfiguration");
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IActionResult ManageTaxConfiguration()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxTypeEmps", "GET");
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

                ResponseBack<List<EmpTaxTypeEmp>> response =
                         JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxTypeEmp>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmpTaxTypeEmp> responseObject = response.Data;
                    return View(responseObject);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }
            return View();

        }
        public IActionResult UpdateTaxConfiguration(int id)
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

                    ViewBag.EmployeeNo = new SelectList(responseObject, "EmployeeId", "FullName");
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
               "EmpTaxTypeEmps" + "/" + id, "GET");
            if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
            {
                var response = JsonConvert.DeserializeObject<ResponseBack<ValidateTaxConfig>>(ress12.Resp);
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
        public IActionResult UpdateTaxConfiguration(int id, EmpTaxTypeEmp empTax)
        {
            try
            {

                empTax.EmpTaxTypeEmpId = id;
                var body = JsonConvert.SerializeObject(empTax);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpTaxTypeEmps" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateTaxCon"] = "Updated Successfully";
                    return RedirectToAction("ManageTaxConfiguration");
                }
                else
                {
                    TempData["responseUpdatetaxCon"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("UpdateTaxConfiguration");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }
        public IActionResult DeleteTaxConfiguration(int id)
        {

            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpTaxTypeEmps" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteTaxCon"] = "Delete Successfully";
                        return RedirectToAction("ManageTaxConfiguration");
                    }
                    else
                    {
                        TempData["MsgDeleteTaxCon"] = "Delete Successfully";
                        return RedirectToAction("ManageTaxConfiguration");
                    }
                }
                return RedirectToAction("ManageTaxConfiguration");
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
