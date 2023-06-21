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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.HR.Domain.DataConfig.RequestSender;
using EmpAllowanceType = ABC.EFCore.Repository.Edmx.EmpAllowanceType;

namespace ABC.HR.Website.Controllers
{
    //[Area("PayRoll")]
    public class EmployeePayRollController : Controller
    {
        //EmpPayRoll Crud Start Here

        public IActionResult AddContractualPayroll()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
               "HR/EmployeeContractDropDown", "GET");
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



            SResponse resdeduc = RequestSender.Instance.CallAPI("api",
                "EmpDeductionTypes", "GET");
            if (resdeduc.Status && (resdeduc.Resp != null) && (resdeduc.Resp != ""))
            {
                ResponseBack<List<EmpDeductionType>> response =
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

            SResponse ressloantyp = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
            if (ressloantyp.Status && (ressloantyp.Resp != null) && (ressloantyp.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantyp.Resp);
                if (empLoanTypesres.Data.Count() > 0)
                {
                    List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                    ViewBag.EmpTaxType = new SelectList(empLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                }
                else
                {
                    List<EmpLoanType> listEmpLoan = new List<EmpLoanType>();
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
        public IActionResult AddPayRoll()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
                "HR/EmployeeGetDropDown", "GET");
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



            SResponse resdeduc = RequestSender.Instance.CallAPI("api",
                "EmpDeductionTypes", "GET");
            if (resdeduc.Status && (resdeduc.Resp != null) && (resdeduc.Resp != ""))
            {
                ResponseBack<List<EmpDeductionType>> response =
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

            SResponse ressloantyp = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
            if (ressloantyp.Status && (ressloantyp.Resp != null) && (ressloantyp.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantyp.Resp);
                if (empLoanTypesres.Data.Count() > 0)
                {
                    List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                    ViewBag.EmpTaxType = new SelectList(empLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                }
                else
                {
                    List<EmpLoanType> listEmpLoan = new List<EmpLoanType>();
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
        public IActionResult AddPayRoll(EmpPayRoll empPayrol)
        {
            try
            {

                var body = JsonConvert.SerializeObject(empPayrol);
                // var body = sr.Serialize(obj);
           
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpPayRolls", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    ResponseBack<EmpPayRoll> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<EmpPayRoll>>(resp.Resp);
                    if(empLoanTypesres.Status == 10)
                    {

                        TempData["responseofAddpayroll"] = "Payroll Already Exists";
                        return RedirectToAction("AddPayRoll");
                    }
                    if(empLoanTypesres.Status == 12)
                    {

                        TempData["responseofAddpayroll"] = "Add Successfully";
                        return RedirectToAction("ManageContractPayRoll");
                    }
                    TempData["responseofAddpayroll"] = "Add Successfully";
                    return RedirectToAction("ManagePayRoll");
                }
                else
                {
                    TempData["responseofAddpayroll"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("ManagePayRoll");
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

        public IActionResult ManagePayRoll()
        {
            List<EmpPayRoll> responseObjectEmpLeave;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpPayRolls/GetEmpPermanantPayRolls", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpPayRoll>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpLeave = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;


                    //  var pdfResult = new ViewAsPdf(responseObjectEmpLeave)
                    //  {
                    //      CustomSwitches =
                    //  "--footer-center \"  Printed Date: " +
                    //DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                    //" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
                    //  };
                    //  return pdfResult;
                    //or
                    //return new ViewAsPdf(responseObjectEmpLeave);


                    return View(responseObjectEmpLeave);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ManageContractPayRoll()
        {
            List<EmpPayRoll> responseObjectEmpLeave;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpPayRolls/GetEmpContractPayRolls", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpPayRoll>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpLeave = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;


                    //  var pdfResult = new ViewAsPdf(responseObjectEmpLeave)
                    //  {
                    //      CustomSwitches =
                    //  "--footer-center \"  Printed Date: " +
                    //DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
                    //" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
                    //  };
                    //  return pdfResult;
                    //or
                    //return new ViewAsPdf(responseObjectEmpLeave);


                    return View(responseObjectEmpLeave);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }



        public class test
        {
            public string fromdate { get; set; }
        }
        public IActionResult PaySlipByDate()
        {
            SResponse ress = RequestSender.Instance.CallAPI("api",
             "EmpPayRolls", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpPayRoll>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<string> a = new List<string>();
                    List<EmpPayRoll> responseObject = response.Data;


                    //var uniquefromdate = responseObject.GroupBy(x => x.FromDate).Select(y => y.First()).ToList();
                    // var uniquetodate = responseObject.GroupBy(x => x.ToDate).Select(y => y.First()).ToList();

                    var query = (from t in responseObject
                                 group t by new { t.FromDate, t.ToDate, t.EmpContractName }
             into grp
                                 select new
                                 {

                                     grp.Key.FromDate,
                                     grp.Key.ToDate,
                                     grp.Key.EmpContractName,

                                     //Quantity = grp.Sum(t => t.Quantity)
                                 }).ToList();


                    List<EmpPayRoll> testList = new List<EmpPayRoll>();
                    //asif
                    //for (int i=0;i< uniquefromdate.Count();i++)
                    //{
                    //    a.Add(uniquefromdate[i].FromDate);
                    //}
                    //    for (int j=0;j< uniquetodate.Count();j++)
                    //    {
                    //        EmpPayRoll obj = new EmpPayRoll();
                    //        obj.FromDate= a[j] +"----- " + " to" + "------ "+ uniquetodate[j].ToDate;
                    //        testList.Add(obj);

                    //    }
                    //ViewBag.Payrollfromdate = new SelectList(testList, "FromDate", "FromDate");
                    //asif
                    ViewBag.Payrollfromdate = new SelectList((from s in query
                                                              select new
                                                              {
                                                                  empid = s.FromDate + "," + s.ToDate + "," + s.EmpContractName,

                                                                  fullName = s.FromDate + " -----to------ " + s.ToDate + " -------- " + s.EmpContractName
                                                              }), "empid", "fullName");

                    //ViewBag.Payrollfromdate = new SelectList(uniquefromdate, "FromDate", "FromDate");

                    //ViewBag.Payrolltodate = new SelectList(uniquetodate, "ToDate", "ToDate");
                }
                else
                {
                    List<EmpPayRoll> listEmpNo = new List<EmpPayRoll>();
                    ViewBag.Payrollfromdate = new SelectList(listEmpNo, "FromDate", "FromDate");
                    ViewBag.Payrolltodate = new SelectList(listEmpNo, "ToDate", "ToDate");
                    TempData["response"] = "Server is down.";
                }
            }
            else
            {
                List<EmpPayRoll> listEmpNo = new List<EmpPayRoll>();
                ViewBag.Payrollfromdate = new SelectList(listEmpNo, "FromDate", "FromDate");
                ViewBag.Payrolltodate = new SelectList(listEmpNo, "ToDate", "ToDate");
            }



            return View();
        }

        [HttpPost]
        public IActionResult PaySlipByDate(ABC.HR.Domain.Entities.EmPayRollViewModel model, string FromDate)
        {
            string[] words = FromDate.Split(',');
            model.FromDate = DateTime.Parse(words[0]);
            model.ToDate = DateTime.Parse(words[1]);
            string contractnammee = words[2];
            ABC.HR.Domain.Entities.EmPayRollViewModel objmodel = new EmPayRollViewModel();
            if (model.FromDate > Convert.ToDateTime("0001/01/01") && model.ToDate > Convert.ToDateTime("0001/01/01"))
            {

                SResponse resspayrolfromto = RequestSender.Instance.CallAPI("api",
            "EmpPayRolls", "GET");
                if (resspayrolfromto.Status && (resspayrolfromto.Resp != null) && (resspayrolfromto.Resp != ""))
                {
                    ResponseBack<List<EmpPayRoll>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(resspayrolfromto.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpPayRoll> responseObject = response.Data;

                        var query = (from t in responseObject
                                     group t by new { t.FromDate, t.ToDate, t.EmpContractName }
                         into grp
                                     select new
                                     {

                                         grp.Key.FromDate,
                                         grp.Key.ToDate,
                                         grp.Key.EmpContractName,


                                     }).ToList();


                        ViewBag.Payrollfromdate = new SelectList((from s in query
                                                                  select new
                                                                  {
                                                                      empid = s.FromDate + "," + s.ToDate + "," + s.EmpContractName,

                                                                      fullName = s.FromDate + " -----to------ " + s.ToDate + " -------- " + s.EmpContractName
                                                                  }), "empid", "fullName");



                    }
                    else
                    {
                        List<EmpPayRoll> listEmpNo = new List<EmpPayRoll>();
                        ViewBag.Payrollfromdate = new SelectList(listEmpNo, "FromDate", "FromDate");
                        ViewBag.Payrolltodate = new SelectList(listEmpNo, "ToDate", "ToDate");
                        TempData["response"] = "Server is down.";
                    }
                }
                else
                {
                    List<EmpPayRoll> listEmpNo = new List<EmpPayRoll>();
                    ViewBag.Payrollfromdate = new SelectList(listEmpNo, "FromDate", "FromDate");
                    ViewBag.Payrolltodate = new SelectList(listEmpNo, "ToDate", "ToDate");
                }






                if (model.FromDate < model.ToDate)
                {
                    List<EmpPayRoll> responseObjectEmpLeave;

                    List<ABC.HR.Domain.Entities.EmPayRollListModel> objPRLModel = new List<ABC.HR.Domain.Entities.EmPayRollListModel>();


                    SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls", "GET");

                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<EmpPayRoll>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);

                        if (response.Data.Count() > 0)
                        {
                            responseObjectEmpLeave = response.Data.Where(x => Convert.ToDateTime(x.FromDate) == model.FromDate && Convert.ToDateTime(x.ToDate) == model.ToDate && x.EmpContractName == contractnammee).ToList();
                            foreach (var item in responseObjectEmpLeave)
                            {
                                ABC.HR.Domain.Entities.EmPayRollListModel objPayRollList = new EmPayRollListModel();

                                //get emp payroll
                                objPayRollList.empPayRoll = item;
                                // get emp info
                                objPayRollList.employee = GetEmpInfobyID(Convert.ToInt32(item.EmployeeId));
                                // get allownces
                                var aaa = GetEmpAllowncesbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.FromDate), Convert.ToDateTime(model.ToDate));
                                if (aaa.Count > 0)
                                {
                                    objPayRollList.empAllowance = aaa;
                                }

                                // get deduction
                                var bbb = GetEmpDeductionbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.FromDate), Convert.ToDateTime(model.ToDate));
                                if (bbb.Count > 0)
                                {
                                    objPayRollList.empDeduction = bbb;
                                }

                                // get tax
                                var ccc = GetEmpTaxbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.FromDate), Convert.ToDateTime(model.ToDate));
                                if (ccc.Count > 0)
                                {
                                    objPayRollList.empTax = ccc;
                                }
                                // get loan
                                var ddd = GetEmpLoanbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.FromDate), Convert.ToDateTime(model.ToDate));
                                if (ddd.Count > 0)
                                {
                                    objPayRollList.empLoan = ddd;
                                }
                                // get emp contact
                                objPayRollList.employeeContract = GetEmpContractbyID(Convert.ToInt32(item.EmployeeId));
                                //for attendence
                                objPayRollList.attendanceRecord = GetEmpAttendencebyDate(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.FromDate), Convert.ToDateTime(model.ToDate), Convert.ToBoolean(false));
                                //
                                objPayRollList.YTDAllowance = GetYTDAllowanceByID(Convert.ToInt32(item.EmployeeId));
                                //
                                objPayRollList.YTDSalary = GetYTDSalary(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.ToDate));
                                //
                                objPayRollList.YTDTaxes = GetYTDTaxess(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(model.ToDate));
                                ////
                                objPayRollList.YTDDeductinss = GetYTDuniqueDeductionss(Convert.ToInt32(item.EmployeeId));


                                var deductionanema = GetDeductionsName();
                                if (deductionanema.Count > 0)
                                {
                                    objPayRollList.empDeductionTypeName = deductionanema;
                                }
                                //
                                objPayRollList.empLeaveTypeName = "Default";
                                //
                                var taxnamedd = GetTaxesName();
                                if (taxnamedd.Count > 0)
                                {
                                    objPayRollList.empTaxTypeName = taxnamedd;
                                }

                                //
                                objPayRollList.empLoanTypeName = "Default";



                                objPRLModel.Add(objPayRollList);

                            }

                            if (objPRLModel.Count > 0)
                            {
                                objmodel.AllPayRoll = objPRLModel;
                            }
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }

                }
            }



            return View(objmodel);
        }

        private Employee GetEmpInfobyID(int employeeid)
        {
            Employee emp = new Employee();

            SResponse ress = RequestSender.Instance.CallAPI("api",
                  "HR/EmployeeGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<Employee>> response = JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<Employee> responseObject = response.Data;
                    emp = responseObject.Where(x => x.EmployeeId == employeeid).FirstOrDefault();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }


            return emp;
        }

        private List<EmpAllowance> GetEmpAllowncesbyID(int employeeid, DateTime fromdate, DateTime todate)
        {
            List<EmpAllowance> empallowance = new List<EmpAllowance>();

            List<EmpAllowance> responseObjectEmpAllownce;


            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpAllowance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    empallowance = responseObjectEmpAllownce.Where(x => x.EmployeeId == employeeid && x.Date >= fromdate && todate >= x.Date).ToList();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }



            return empallowance;
        }

        private List<EmpDeduction> GetEmpDeductionbyID(int employeeid, DateTime fromdate, DateTime todate)
        {
            List<EmpDeduction> empdeduction = new List<EmpDeduction>();

            List<EmpDeduction> responseObjectEmpDed;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductions", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpDeduction>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<EmpDeduction>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpDed = response.Data;
                    empdeduction = responseObjectEmpDed.Where(x => x.EmployeeId == employeeid && x.Date >= fromdate && todate >= x.Date).ToList();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return empdeduction;
        }

        private List<EmpTax> GetEmpTaxbyID(int employeeid, DateTime fromdate, DateTime todate)
        {
            List<EmpTax> emptax = new List<EmpTax>();

            List<EmpTax> responseObjectEmpTax;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxes", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpTax>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpTax>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpTax = response.Data;
                    emptax = responseObjectEmpTax.Where(x => x.EmployeeId == employeeid && x.Date >= fromdate && todate >= x.Date).ToList();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return emptax;
        }

        private List<EmpLoan> GetEmpLoanbyID(int employeeid, DateTime fromdate, DateTime todate)
        {
            List<EmpLoan> emploan = new List<EmpLoan>();

            List<EmpLoan> responseObjectEmpLoan;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoans", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpLoan>> response =
                              JsonConvert.DeserializeObject<ResponseBack<List<EmpLoan>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpLoan = response.Data;
                    emploan = responseObjectEmpLoan.Where(x => x.EmployeeId == employeeid && x.Date >= fromdate && todate >= x.Date).ToList();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return emploan;
        }

        private EmployeeContract GetEmpContractbyID(int employeeid)
        {
            EmployeeContract empcontract = new EmployeeContract();

            SResponse ress = RequestSender.Instance.CallAPI("api",
                     "HR/EmployeeContractGet", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmployeeContract>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmployeeContract>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    List<EmployeeContract> responseObject = response.Data;
                    empcontract = responseObject.Where(x => x.EmployeeId == employeeid).FirstOrDefault();
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }


            return empcontract;
        }

        private EmpAttendanceRecord GetEmpAttendencebyDate(int empidforalonce, DateTime month, DateTime year, bool IncludeSS)
        {


            EmpAttendanceRecord empAttendanceRecord = new EmpAttendanceRecord();


        //http://localhost:37474/api/EmpAttendances/AttendanceByID?empid=22&fromDate=2021-12-17&toDate=2021-12-21&IncludeSS=true
        http://localhost:37474/api/EmpAttendances/AttendanceByID?empid=22&fromDate=2021-12-17&toDate=2021-12-21&IncludeSS=true

            SResponse ress = RequestSender.Instance.CallAPI("api", "EmpAttendances/AttendanceByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year + "&IncludeSS=true", "GET");


            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {


                var response = JsonConvert.DeserializeObject<ResponseBack<EmpAttendanceRecord>>(ress.Resp);
                if (response.Data != null)
                {
                    empAttendanceRecord = response.Data;
                    return empAttendanceRecord;
                }


            }


            return empAttendanceRecord;


        }

        //YTD Yet To Date Get Allownce
        private double GetYTDAllowanceByID(int empid)
        {

            double totalAllowances = 0;

            List<EmpAllowance> responseObjectEmpAllownce;


            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpAllowance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    var empallowance = responseObjectEmpAllownce.Where(x => x.EmployeeId == empid).ToList();
                    foreach (var item in empallowance)
                    {
                        totalAllowances += Convert.ToDouble(item.Amount);
                    }



                    return totalAllowances;

                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return totalAllowances;



        }
        //YTD Yet To Date Get Salary

        private double GetYTDSalary(int empid, DateTime todate)
        {

            double totalallsalary = 0;

            DateTime myDateTime = DateTime.Now;
            string year = myDateTime.Year.ToString();


            List<EmpPayRoll> responseObjectEmppayroll;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpPayRolls", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpPayRoll>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmppayroll = response.Data;

                    List<EmpPayRoll> empallowance = responseObjectEmppayroll.Where(x => x.EmployeeId == empid && (Convert.ToDateTime(x.DisperseDate).Year) == todate.Year).ToList();
                    if (empallowance.Any()) //prevent IndexOutOfRangeException for empty list
                    {
                        //empallowance.RemoveAt(empallowance.Count - 1);
                    }
                    foreach (var item in empallowance)
                    {
                        totalallsalary += Convert.ToDouble(item.PayWithoutDeduction);
                    }



                    return totalallsalary;

                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return totalallsalary;



        }

        // public readonly int? unique;


        int? unique = 0;
        //YTD TAXes
        private List<YTDuniqueTax> GetYTDTaxess(int empid, DateTime todate)
        {

            List<YTDuniqueTax> ListYtDtaxes = new List<YTDuniqueTax>();

            //YTDuniqueTax yTDuniqueTax = new YTDuniqueTax();
            //List<double> ListYtDtaxes = new List<double> { new double {} };
            int totalalltaxes = 0;
            DateTime myDateTime = DateTime.Now;
            string curryear = myDateTime.Year.ToString();

            List<EmpTax> responseObjectEmppayroll;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxes", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpTax>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpTax>>>(ress.Resp);

                SResponse resstax = RequestSender.Instance.CallAPI("api",
                  "EmpTaxTypes", "GET");

                if (resstax.Status && (resstax.Resp != null) && (resstax.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> responsetaxtype =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(resstax.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpTaxType> responseObjecttaxtype = responsetaxtype.Data;
                        var taxtype = responseObjecttaxtype.ToList();

                        double total = 0.0;


                        foreach (var item in taxtype)
                        {
                            responseObjectEmppayroll = response.Data;
                            var emptaxess = responseObjectEmppayroll.Where(x => x.EmployeeId == empid && x.EmpTaxTypeId == item.EmpTaxTypeId && (Convert.ToDateTime(x.Date).Year) == todate.Year).ToList();
                            foreach (var item1 in emptaxess)
                            {
                                total += Convert.ToDouble(item1.Amount);
                            }
                            if (emptaxess.Count > 0)
                            {
                                YTDuniqueTax yTDuniqueTax = new YTDuniqueTax();

                                yTDuniqueTax.EmployeeID = empid;
                                yTDuniqueTax.EmpTaxtypeid = item.EmpTaxTypeId;
                                yTDuniqueTax.taxstatus = item.IsTax;
                                yTDuniqueTax.ertaxstatus = item.IsErTax;
                                yTDuniqueTax.ytdtaxamout = total;

                                ListYtDtaxes.Add(yTDuniqueTax);
                                total = 0.0;
                            }

                        }
                        return ListYtDtaxes;



                    }
                    //if (response.Data.Count() > 0)
                    //{
                    //    responseObjectEmppayroll = response.Data;
                    //    var emptaxess = responseObjectEmppayroll.Where(x => x.EmployeeId == empid && x. EmpTaxTypeId == ).ToList();

                    //    unique = 0;
                    //    for (int item=0;item<emptaxess.Count();item++)
                    //    {
                    //        ///////////////////////////////////////
                    //        if (item == 0)
                    //        {
                    //            unique = emptaxess[item].EmpTaxTypeId;
                    //            totalalltaxes += Convert.ToInt32(emptaxess[item].Amount);
                    //        }
                    //        else {

                    //            if (unique == emptaxess[item].EmpTaxTypeId)
                    //            {
                    //                totalalltaxes += Convert.ToInt32(emptaxess[item].Amount);

                    //            }
                    //            else
                    //            {
                    //                yTDuniqueTax.EmployeeID = empid;
                    //                yTDuniqueTax.EmpTaxtypeid = unique;
                    //                yTDuniqueTax.ytdtaxamout = totalalltaxes;

                    //                ListYtDtaxes.Add(yTDuniqueTax);
                    //                totalalltaxes = 0;
                    //                unique = Convert.ToInt32(emptaxess[item].EmpTaxTypeId) + Convert.ToInt32(1);
                    //            }
                    //        }




                    //    }




                    //    return ListYtDtaxes;

                    //}
                    //else
                    //{
                    //    TempData["response"] = "Server is down.";
                    //}
                }



            }
            return ListYtDtaxes;
        }

        //YtD dectuction
        private List<YTDuniqueDeduction> GetYTDuniqueDeductionss(int empid)
        {
            List<YTDuniqueDeduction> ListYtDeduction = new List<YTDuniqueDeduction>();

            List<EmpDeduction> responseObjectEmppayroll;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductions", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpDeduction>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeduction>>>(ress.Resp);

                SResponse resstax = RequestSender.Instance.CallAPI("api",
                  "EmpDeductionTypes", "GET");

                if (resstax.Status && (resstax.Resp != null) && (resstax.Resp != ""))
                {
                    ResponseBack<List<EmpDeductionType>> responsetaxtype =
                                 JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(resstax.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<EmpDeductionType> responseObjecttaxtype = responsetaxtype.Data;
                        var taxtype = responseObjecttaxtype.ToList();

                        double total = 0.0;


                        foreach (var item in taxtype)
                        {
                            responseObjectEmppayroll = response.Data;
                            var emptaxess = responseObjectEmppayroll.Where(x => x.EmployeeId == empid && x.EmpDeductionTypeId == item.EmpDeductionTypeId).ToList();
                            foreach (var item1 in emptaxess)
                            {
                                total += Convert.ToDouble(item1.Amount);
                            }
                            if (emptaxess.Count > 0)
                            {
                                YTDuniqueDeduction yTDuniqueDeduc = new YTDuniqueDeduction();

                                yTDuniqueDeduc.EmployeeID = empid;
                                yTDuniqueDeduc.Empdedctiontypeid = item.EmpDeductionTypeId;
                                yTDuniqueDeduc.ytddeductionamout = total;

                                ListYtDeduction.Add(yTDuniqueDeduc);
                                total = 0.0;
                            }

                        }
                        return ListYtDeduction;



                    }

                }



            }
            return ListYtDeduction;


        }

        //Get name of tax 

        private List<EmpTaxType> GetTaxesName()
        {
            List<EmpTaxType> empTaxType = new List<EmpTaxType>();

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpTaxType>> response =
                         JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    var responseObject = response.Data;
                    empTaxType = responseObject;
                    return empTaxType;
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }


            return empTaxType;
        }

        //Get Name of deduction
        private List<EmpDeductionType> GetDeductionsName()
        {
            List<EmpDeductionType> empdeductionType = new List<EmpDeductionType>();

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductionTypes", "GET");
            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpDeductionType>> response =
                         JsonConvert.DeserializeObject<ResponseBack<List<EmpDeductionType>>>(ress.Resp);
                if (response.Data.Count() > 0)
                {
                    var responseObject = response.Data;
                    empdeductionType = responseObject;
                    return empdeductionType;
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }


            return empdeductionType;
        }



        //individual report
        public IActionResult EmployeePayRollDetail(int id, DateTime FromDate, DateTime ToDate)
        {

            ABC.HR.Domain.Entities.EmPayRollViewModel objmodel = new EmPayRollViewModel();
            if (FromDate > Convert.ToDateTime("0001/01/01") && ToDate > Convert.ToDateTime("0001/01/01"))
            {
                if (FromDate < ToDate)
                {
                    List<EmpPayRoll> responseObjectEmpLeave;

                    List<ABC.HR.Domain.Entities.EmPayRollListModel> objPRLModel = new List<ABC.HR.Domain.Entities.EmPayRollListModel>();


                    SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls", "GET");

                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<EmpPayRoll>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                        
                        if (response.Data.Count() > 0)
                        {
                            responseObjectEmpLeave = response.Data.Where(x => x.EmpPayRollId == id && Convert.ToDateTime(x.FromDate) == FromDate && Convert.ToDateTime(x.ToDate) == ToDate).ToList();
                            
                            foreach (var item in responseObjectEmpLeave)
                            {
                                SResponse ress1 = RequestSender.Instance.CallAPI("api", "EmpPayRolls/TaxesById?empid=" + item.EmployeeId + "&startdate=" + FromDate + "", "GET");
                                EmppayrollTaxes response1 = new EmppayrollTaxes();
                                if (ress1.Status && (ress1.Resp != null) && (ress1.Resp != ""))
                                {
                                     response1 = JsonConvert.DeserializeObject<EmppayrollTaxes>(ress1.Resp);
                                }
                                ABC.HR.Domain.Entities.EmPayRollListModel objPayRollList = new EmPayRollListModel();

                                //get emp payroll
                                objPayRollList.empPayRoll = item;
                                // get emp info
                                objPayRollList.employee = GetEmpInfobyID(Convert.ToInt32(item.EmployeeId));
                                // get allownces
                                var aaa = GetEmpAllowncesbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (aaa.Count > 0)
                                {
                                    objPayRollList.empAllowance = aaa;
                                }

                                // get deduction
                                var bbb = GetEmpDeductionbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (bbb.Count > 0)
                                {
                                    objPayRollList.empDeduction = bbb;
                                }

                                // get tax
                                var ccc = GetEmpTaxbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ccc.Count > 0)
                                {
                                    objPayRollList.empTax = ccc;
                                }
                                // get loan
                                var ddd = GetEmpLoanbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ddd.Count > 0)
                                {
                                    objPayRollList.empLoan = ddd;
                                }
                                // get emp contact
                                objPayRollList.employeeContract = GetEmpContractbyID(Convert.ToInt32(item.EmployeeId));
                                //for attendence
                                objPayRollList.attendanceRecord = GetEmpAttendencebyDate(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Convert.ToBoolean(false));
                                //
                                objPayRollList.YTDAllowance = GetYTDAllowanceByID(Convert.ToInt32(item.EmployeeId));
                                //
                                objPayRollList.YTDSalary = GetYTDSalary(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                //
                                objPayRollList.YTDTaxes = GetYTDTaxess(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                ////
                                objPayRollList.YTDDeductinss = GetYTDuniqueDeductionss(Convert.ToInt32(item.EmployeeId));


                                var deductionanema = GetDeductionsName();
                                if (deductionanema.Count > 0)
                                {
                                    objPayRollList.empDeductionTypeName = deductionanema;
                                }
                                //
                                objPayRollList.empLeaveTypeName = "Default";
                                //
                                var taxnamedd = GetTaxesName();
                                if (taxnamedd.Count > 0)
                                {
                                    objPayRollList.empTaxTypeName = taxnamedd;
                                }

                                //
                                objPayRollList.empLoanTypeName = "Default";

                                objPayRollList.EmppayrollTaxes =(response1);

                                objPRLModel.Add(objPayRollList);

                            }

                            if (objPRLModel.Count > 0)
                            {
                                objmodel.AllPayRoll = objPRLModel;
                            }
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }

                }
            }



            return View(objmodel);
        }
        public IActionResult EmployeeContractPayRollDetail(int id, DateTime FromDate, DateTime ToDate)
        {

            ABC.HR.Domain.Entities.EmPayRollViewModel objmodel = new EmPayRollViewModel();
            if (FromDate > Convert.ToDateTime("0001/01/01") && ToDate > Convert.ToDateTime("0001/01/01"))
            {
                if (FromDate < ToDate)
                {
                    List<EmpPayRoll> responseObjectEmpLeave;

                    List<ABC.HR.Domain.Entities.EmPayRollListModel> objPRLModel = new List<ABC.HR.Domain.Entities.EmPayRollListModel>();


                    SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls", "GET");

                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<EmpPayRoll>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                        
                        if (response.Data.Count() > 0)
                        {
                            responseObjectEmpLeave = response.Data.Where(x => x.EmpPayRollId == id && Convert.ToDateTime(x.FromDate) == FromDate && Convert.ToDateTime(x.ToDate) == ToDate).ToList();
                            
                            foreach (var item in responseObjectEmpLeave)
                            {
                                SResponse ress1 = RequestSender.Instance.CallAPI("api", "EmpPayRolls/TaxesById?empid=" + item.EmployeeId + "&startdate=" + FromDate + "", "GET");
                                EmppayrollTaxes response1 = new EmppayrollTaxes();
                                if (ress1.Status && (ress1.Resp != null) && (ress1.Resp != ""))
                                {
                                     response1 = JsonConvert.DeserializeObject<EmppayrollTaxes>(ress1.Resp);
                                }
                                ABC.HR.Domain.Entities.EmPayRollListModel objPayRollList = new EmPayRollListModel();

                                //get emp payroll
                                objPayRollList.empPayRoll = item;
                                // get emp info
                                objPayRollList.employee = GetEmpInfobyID(Convert.ToInt32(item.EmployeeId));
                                // get allownces
                                var aaa = GetEmpAllowncesbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (aaa.Count > 0)
                                {
                                    objPayRollList.empAllowance = aaa;
                                }

                                // get deduction
                                var bbb = GetEmpDeductionbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (bbb.Count > 0)
                                {
                                    objPayRollList.empDeduction = bbb;
                                }

                                // get tax
                                var ccc = GetEmpTaxbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ccc.Count > 0)
                                {
                                    objPayRollList.empTax = ccc;
                                }
                                // get loan
                                var ddd = GetEmpLoanbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ddd.Count > 0)
                                {
                                    objPayRollList.empLoan = ddd;
                                }
                                // get emp contact
                                objPayRollList.employeeContract = GetEmpContractbyID(Convert.ToInt32(item.EmployeeId));
                                //for attendence
                                objPayRollList.attendanceRecord = GetEmpAttendencebyDate(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Convert.ToBoolean(false));
                                //
                                objPayRollList.YTDAllowance = GetYTDAllowanceByID(Convert.ToInt32(item.EmployeeId));
                                //
                                objPayRollList.YTDSalary = GetYTDSalary(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                //
                                objPayRollList.YTDTaxes = GetYTDTaxess(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                ////
                                objPayRollList.YTDDeductinss = GetYTDuniqueDeductionss(Convert.ToInt32(item.EmployeeId));


                                var deductionanema = GetDeductionsName();
                                if (deductionanema.Count > 0)
                                {
                                    objPayRollList.empDeductionTypeName = deductionanema;
                                }
                                //
                                objPayRollList.empLeaveTypeName = "Default";
                                //
                                var taxnamedd = GetTaxesName();
                                if (taxnamedd.Count > 0)
                                {
                                    objPayRollList.empTaxTypeName = taxnamedd;
                                }

                                //
                                objPayRollList.empLoanTypeName = "Default";

                                objPayRollList.EmppayrollTaxes =(response1);

                                objPRLModel.Add(objPayRollList);

                            }

                            if (objPRLModel.Count > 0)
                            {
                                objmodel.AllPayRoll = objPRLModel;
                            }
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }

                }
            }



            return View(objmodel);
        }
        public IActionResult GetAllContractualPayRoll()
        {
            return View();
        }
        public IActionResult GetAllPayRoll()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AllEmployeePayRollDetail(DateTime FromDate, DateTime ToDate)
        {
            ToDate = FromDate.AddDays(13);
            ABC.HR.Domain.Entities.EmPayRollViewModel objmodel = new EmPayRollViewModel();
            if (FromDate > Convert.ToDateTime("0001/01/01") && ToDate > Convert.ToDateTime("0001/01/01"))
            {
                if (FromDate < ToDate)
                {
                    List<EmpPayRoll> responseObjectEmpLeave;

                    List<ABC.HR.Domain.Entities.EmPayRollListModel> objPRLModel = new List<ABC.HR.Domain.Entities.EmPayRollListModel>();


                    SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls/GetEmpPermanantPayRolls", "GET");

                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<EmpPayRoll>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                        
                        if (response.Data.Count() > 0)
                        {
                            responseObjectEmpLeave = response.Data.Where(x => Convert.ToDateTime(x.FromDate) == FromDate && Convert.ToDateTime(x.ToDate) == ToDate).ToList();
                            
                            foreach (var item in responseObjectEmpLeave)
                            {
                                SResponse ress1 = RequestSender.Instance.CallAPI("api", "EmpPayRolls/TaxesById?empid=" + item.EmployeeId + "&startdate=" + FromDate + "", "GET");
                                EmppayrollTaxes response1 = new EmppayrollTaxes();
                                if (ress1.Status && (ress1.Resp != null) && (ress1.Resp != ""))
                                {
                                     response1 = JsonConvert.DeserializeObject<EmppayrollTaxes>(ress1.Resp);
                                }
                                ABC.HR.Domain.Entities.EmPayRollListModel objPayRollList = new EmPayRollListModel();

                                //get emp payroll
                                objPayRollList.empPayRoll = item;
                                // get emp info
                                objPayRollList.employee = GetEmpInfobyID(Convert.ToInt32(item.EmployeeId));
                                // get allownces
                                var aaa = GetEmpAllowncesbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (aaa.Count > 0)
                                {
                                    objPayRollList.empAllowance = aaa;
                                }

                                // get deduction
                                var bbb = GetEmpDeductionbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (bbb.Count > 0)
                                {
                                    objPayRollList.empDeduction = bbb;
                                }

                                // get tax
                                var ccc = GetEmpTaxbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ccc.Count > 0)
                                {
                                    objPayRollList.empTax = ccc;
                                }
                                // get loan
                                var ddd = GetEmpLoanbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ddd.Count > 0)
                                {
                                    objPayRollList.empLoan = ddd;
                                }
                                // get emp contact
                                objPayRollList.employeeContract = GetEmpContractbyID(Convert.ToInt32(item.EmployeeId));
                                //for attendence
                                objPayRollList.attendanceRecord = GetEmpAttendencebyDate(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Convert.ToBoolean(false));
                                //
                                objPayRollList.YTDAllowance = GetYTDAllowanceByID(Convert.ToInt32(item.EmployeeId));
                                //
                                objPayRollList.YTDSalary = GetYTDSalary(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                //
                                objPayRollList.YTDTaxes = GetYTDTaxess(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                ////
                                objPayRollList.YTDDeductinss = GetYTDuniqueDeductionss(Convert.ToInt32(item.EmployeeId));


                                var deductionanema = GetDeductionsName();
                                if (deductionanema.Count > 0)
                                {
                                    objPayRollList.empDeductionTypeName = deductionanema;
                                }
                                //
                                objPayRollList.empLeaveTypeName = "Default";
                                //
                                var taxnamedd = GetTaxesName();
                                if (taxnamedd.Count > 0)
                                {
                                    objPayRollList.empTaxTypeName = taxnamedd;
                                }

                                //
                                objPayRollList.empLoanTypeName = "Default";
                                EmppayrollTaxes obj = new EmppayrollTaxes();
                                obj = response1;
                                objPayRollList.EmppayrollTaxes = (obj);
                               // objPayRollList.EmppayrollTaxes.Add(response1);

                                objPRLModel.Add(objPayRollList);

                            }

                            if (objPRLModel.Count > 0)
                            {
                                objmodel.AllPayRoll = objPRLModel;
                            }
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }

                }
            }



            return View(objmodel);
        }
        [HttpPost]
        public IActionResult AllContractualEmployeePayRollDetail(DateTime FromDate, DateTime ToDate)
        {
            ToDate = FromDate.AddDays(13);
            ABC.HR.Domain.Entities.EmPayRollViewModel objmodel = new EmPayRollViewModel();
            if (FromDate > Convert.ToDateTime("0001/01/01") && ToDate > Convert.ToDateTime("0001/01/01"))
            {
                if (FromDate < ToDate)
                {
                    List<EmpPayRoll> responseObjectEmpLeave;

                    List<ABC.HR.Domain.Entities.EmPayRollListModel> objPRLModel = new List<ABC.HR.Domain.Entities.EmPayRollListModel>();


                    SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls/GetEmpContractPayRolls", "GET");

                    if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                    {
                        ResponseBack<List<EmpPayRoll>> response = JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                        
                        if (response.Data.Count() > 0)
                        {
                            responseObjectEmpLeave = response.Data.Where(x => Convert.ToDateTime(x.FromDate) == FromDate && Convert.ToDateTime(x.ToDate) == ToDate).ToList();
                            
                            foreach (var item in responseObjectEmpLeave)
                            {
                                SResponse ress1 = RequestSender.Instance.CallAPI("api", "EmpPayRolls/TaxesById?empid=" + item.EmployeeId + "&startdate=" + FromDate + "", "GET");
                                EmppayrollTaxes response1 = new EmppayrollTaxes();
                                if (ress1.Status && (ress1.Resp != null) && (ress1.Resp != ""))
                                {
                                     response1 = JsonConvert.DeserializeObject<EmppayrollTaxes>(ress1.Resp);
                                }
                                ABC.HR.Domain.Entities.EmPayRollListModel objPayRollList = new EmPayRollListModel();

                                //get emp payroll
                                objPayRollList.empPayRoll = item;
                                // get emp info
                                objPayRollList.employee = GetEmpInfobyID(Convert.ToInt32(item.EmployeeId));
                                // get allownces
                                var aaa = GetEmpAllowncesbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (aaa.Count > 0)
                                {
                                    objPayRollList.empAllowance = aaa;
                                }

                                // get deduction
                                var bbb = GetEmpDeductionbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (bbb.Count > 0)
                                {
                                    objPayRollList.empDeduction = bbb;
                                }

                                // get tax
                                var ccc = GetEmpTaxbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ccc.Count > 0)
                                {
                                    objPayRollList.empTax = ccc;
                                }
                                // get loan
                                var ddd = GetEmpLoanbyID(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate));
                                if (ddd.Count > 0)
                                {
                                    objPayRollList.empLoan = ddd;
                                }
                                // get emp contact
                                objPayRollList.employeeContract = GetEmpContractbyID(Convert.ToInt32(item.EmployeeId));
                                //for attendence
                                objPayRollList.attendanceRecord = GetEmpAttendencebyDate(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(FromDate), Convert.ToDateTime(ToDate), Convert.ToBoolean(false));
                                //
                                objPayRollList.YTDAllowance = GetYTDAllowanceByID(Convert.ToInt32(item.EmployeeId));
                                //
                                objPayRollList.YTDSalary = GetYTDSalary(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                //
                                objPayRollList.YTDTaxes = GetYTDTaxess(Convert.ToInt32(item.EmployeeId), Convert.ToDateTime(ToDate));
                                ////
                                objPayRollList.YTDDeductinss = GetYTDuniqueDeductionss(Convert.ToInt32(item.EmployeeId));


                                var deductionanema = GetDeductionsName();
                                if (deductionanema.Count > 0)
                                {
                                    objPayRollList.empDeductionTypeName = deductionanema;
                                }
                                //
                                objPayRollList.empLeaveTypeName = "Default";
                                //
                                var taxnamedd = GetTaxesName();
                                if (taxnamedd.Count > 0)
                                {
                                    objPayRollList.empTaxTypeName = taxnamedd;
                                }

                                //
                                objPayRollList.empLoanTypeName = "Default";
                                EmppayrollTaxes obj = new EmppayrollTaxes();
                                obj = response1;
                                objPayRollList.EmppayrollTaxes = (obj);
                               // objPayRollList.EmppayrollTaxes.Add(response1);

                                objPRLModel.Add(objPayRollList);

                            }

                            if (objPRLModel.Count > 0)
                            {
                                objmodel.AllPayRoll = objPRLModel;
                            }
                        }
                        else
                        {
                            TempData["response"] = "Server is down.";
                        }
                    }

                }
            }



            return View(objmodel);
        }





        //public IActionResult PayRollPdfView()
        //{
        //    SResponse ressallownce = RequestSender.Instance.CallAPI("api",
        //        "EmpAllowances", "GET");

        //    // 
        //    List<EmpAllowance> responseallownce =
        //                 JsonConvert.DeserializeObject < ResponseBack<List<EmpAllowance>>(ressallownce.Resp);

        //    SResponse ressemploye = RequestSender.Instance.CallAPI("api",
        //      "HR/EmployeeGet", "GET");
        //    //
        //    List<Employee> responseemployee =
        //                 JsonConvert.DeserializeObject < ResponseBack<List<Employee>>(ressemploye.Resp);

        //    SResponse resspayroll = RequestSender.Instance.CallAPI("api",
        //          "EmpPayRolls", "GET");
        //    //

        //    List<EmpPayRoll> responsepayroll =
        //                 JsonConvert.DeserializeObject < ResponseBack<List<EmpPayRoll>>(resspayroll.Resp);


        //    SResponse resscontract = RequestSender.Instance.CallAPI("api",
        //         "HR/EmployeeContractGet", "GET");
        //    //
        //    List<EmployeeContract> responsecontract =
        //                 JsonConvert.DeserializeObject < ResponseBack<List<EmployeeContract>>(resscontract.Resp);

        //    SResponse ressTaxes = RequestSender.Instance.CallAPI("api",
        //        "EmpTaxes", "GET");

        //    List<EmpTax> responsetaxes =
        //                 JsonConvert.DeserializeObject < ResponseBack<List<EmpTax>>(ressTaxes.Resp);

        //    //List<Employee> employees = db.Employees.ToList();
        //    //List<Department> departments = db.Departments.ToList();
        //    //List<Incentive> incentives = db.Incentives.ToList();

        //    //var employeeRecord = from e in responseemployee
        //    //                     join a in responseallownce on e.EmployeeId equals a.EmployeeId into table1
        //    //                     from a in table1.ToList()
        //    //                     join i in responsepayroll on e.EmployeeId equals i.EmployeeId into table2
        //    //                     from i in table2.ToList()
        //    //                     join c in responsecontract on e.EmployeeId equals c.EmployeeId into table3
        //    //                     from c in table3.ToList()
        //    //                     join t in responsetaxes on e.EmployeeId equals t.EmployeeId into table4
        //    //                     from t in table4.ToList()
        //    //                     select new EmPayRollViewModel
        //    //                     {
        //    //                         empPayRoll = i,
        //    //                         empAllowance = a,
        //    //                         employee = e,
        //    //                         employeeContract= c,
        //    //                         empTax = t
        //    //                     };
        //    //return View(employeeRecord);

        //    //List<EmPayRollViewModel> emPayRollViewModel = new List<EmPayRollViewModel>();
        //    //EmPayRollViewModel emPayRollViewModelSingle = new EmPayRollViewModel();

        //    //foreach (var item in responseemployee)
        //    //{
        //    //    emPayRollViewModelSingle.employee = item;



        //    //}


        //    //emPayRollViewModel.employee
        //    return View();
        //}
        //public IActionResult PayRollDetails(int id, DateTime FromDate, DateTime ToDate)
        //{
        //    try
        //    {
        //        SResponse resspayroll = RequestSender.Instance.CallAPI("api",
        //     "EmpPayRolls", "GET");
        //        if (resspayroll.Status && (resspayroll.Resp != null) && (resspayroll.Resp != ""))
        //        {

        //            List<EmpPayRoll> response =
        //                         JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>(resspayroll.Resp);


        //            if (response.Count() > 0)
        //            {
        //                if (id != 0)
        //                {
        //                    List<EmpPayRoll> resss = response;
        //                    for (int i = 0; i < response.Count(); i++)
        //                    {
        //                        //var responseObject = response.Where(x => x.EmployeeId == id && x.FromDate >= FromDate && FromDate > x.FromDate && x.ToDate>= ToDate && ToDate > x.ToDate);
        //                        var responseObject = response.Where(x => x.EmployeeId == id && x.FromDate == FromDate && x.ToDate == ToDate);
        //                        if (responseObject.Count() > 0)
        //                        {
        //                            return new ViewAsPdf(responseObject);
        //                        }
        //                        else
        //                        {
        //                            return Json("");
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    List<EmpPayRoll> resss = response;
        //                    for (int i = 0; i < response.Count(); i++)
        //                    {
        //                        var responseObject = response.Where(x => x.FromDate >= FromDate && ToDate >= x.ToDate);
        //                        if (responseObject.Count() > 0)
        //                        {
        //                            return new ViewAsPdf(responseObject);
        //                        }
        //                        else
        //                        {
        //                            return Json("");
        //                        }
        //                    }

        //                }

        //                //  var pdfResult = new ViewAsPdf(responseObject)
        //                //  {
        //                //      CustomSwitches =
        //                //  "--footer-center \"  Printed Date: " +
        //                //DateTime.Now.Date.ToString("dd/MM/yyyy") + "  Page: [page]/[toPage]\"" +
        //                //" --footer-line --footer-font-size \"12\" --footer-spacing 1 --footer-font-name \"Segoe UI\""
        //                //  };
        //                //  return pdfResult;
        //                //or
        //                //return new ViewAsPdf(responseObject);
        //                //return View(responseObject);
        //            }
        //            else
        //            {
        //                TempData["Msg"] = "Server is down.";
        //            }
        //        }
        //        return View();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public IActionResult UpdatePayRoll(int id)
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



                SResponse resdeduc = RequestSender.Instance.CallAPI("api",
                    "EmpDeductionTypes", "GET");
                if (resdeduc.Status && (resdeduc.Resp != null) && (resdeduc.Resp != ""))
                {
                    ResponseBack<List<EmpDeductionType>> response =
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

                SResponse ressloantyp = RequestSender.Instance.CallAPI("api", "EmpTaxTypes", "GET");
                if (ressloantyp.Status && (ressloantyp.Resp != null) && (ressloantyp.Resp != ""))
                {
                    ResponseBack<List<EmpTaxType>> empLoanTypesres = JsonConvert.DeserializeObject<ResponseBack<List<EmpTaxType>>>(ressloantyp.Resp);
                    if (empLoanTypesres.Data.Count() > 0)
                    {
                        List<EmpTaxType> empLoanTypes = empLoanTypesres.Data;
                        ViewBag.EmpTaxType = new SelectList(empLoanTypes, "EmpTaxTypeId", "EmpTaxTypeName");
                    }
                    else
                    {
                        List<EmpLoanType> listEmpLoan = new List<EmpLoanType>();
                        ViewBag.EmpTaxType = new SelectList(listEmpLoan, "EmpTaxTypeId", "EmpTaxTypeName");
                        TempData["response"] = "Server is down.";
                    }
                }




                SResponse resspayroll = RequestSender.Instance.CallAPI("api",
               "EmpPayRolls" + "/" + id, "GET");
                if (resspayroll.Status && (resspayroll.Resp != null) && (resspayroll.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpPayRoll>>(resspayroll.Resp);
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
        public IActionResult UpdatePayRoll(int id, EmpPayRoll emppayrol)
        {

            try
            {

                emppayrol.EmpPayRollId = id;
                var body = JsonConvert.SerializeObject(emppayrol);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpPayRolls" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdatePayRoll"] = "Updated Successfully";
                    return RedirectToAction("ManagePayRoll");
                }
                else
                {
                    TempData["responseUpdatePayRoll"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManagePayRoll");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeletePayRoll(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpPayRolls" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeletePayRoll"] = "Delete Successfully";
                        return RedirectToAction("ManagePayRoll");
                    }
                    else
                    {
                        TempData["MsgDeletePayroll"] = "Delete Successfully";
                        return RedirectToAction("ManagePayRoll");
                    }
                }
                return RedirectToAction("ManagePayRoll");
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
                            else
                            {
                                return Json(null);
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

        //get allowance selected Employee

        public IActionResult EmployeeAllownceTotal(int empidforalonce, DateTime month, DateTime year)
        {
            try
            {


                //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");

                //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());


                // SResponse ress = RequestSender.Instance.CallAPI("api",
                //"AllowanceByID" + "/" + empidforalonce + "/" + tfromdate + "/" + ttodate, "GET");

                //SResponse ress = RequestSender.Instance.CallAPI("api", "EmpAllowances/AllowanceByID?empid=4&fromDate=2021-10-01&toDate=2021-10-30","GET");
                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpAllowances/AllowanceByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year, "GET");



                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpAllowanceRecord>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
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


        // getEmplyee Dedduction Totall

        public IActionResult EmployeeDeductionTotal(int empidforalonce, DateTime month, DateTime year)
        {
            try
            {


                //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
                ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
                //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());


                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpDeductions/DeductionByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year, "GET");


                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpDeductionRecord>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
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

        //get employee Tax total

        public IActionResult EmployeeTaxTotal(int empidforalonce, DateTime month, DateTime year)
        {
            try
            {


                //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
                ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
                //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());


                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpTaxes/TaxByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year, "GET");


                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpTaxRecord>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
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

        //get employee loans total

        public IActionResult EmployeeLoanTotal(int empidforalonce, DateTime month, DateTime year)
        {
            try
            {


                //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
                ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
                //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());


                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpLoans/LoanByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year, "GET");


                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpLoanRecord>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
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


        //list Allownce specific

        public IActionResult AllowncebyDate(int empidforalonce, DateTime month, DateTime year)
        {

            //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
            ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
            //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());

            List<EmpAllowance> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAllowances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpAllowance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAllowance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        var responseObject = response.Data.Where(x => x.EmployeeId == empidforalonce && x.Date >= month && year >= x.Date);
                        if (responseObject.Count() > 0)
                        {
                            return Json(responseObject);
                        }
                        else
                        {
                            return Json("");
                        }

                    }



                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return Json(ress);
        }


        //list Deductions specific

        public IActionResult DeductionsbyDate(int empidforalonce, DateTime month, DateTime year)
        {

            //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
            ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
            //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());

            List<EmpDeduction> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpDeductions", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpDeduction>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpDeduction>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        var responseObject = response.Data.Where(x => x.EmployeeId == empidforalonce && x.Date >= month && year >= x.Date);
                        if (responseObject.Count() > 0)
                        {
                            return Json(responseObject);
                        }
                        else
                        {
                            return Json("");
                        }
                    }




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    return Json("");
                }
            }

            return View();
        }

        //list Loan specific

        public IActionResult LoanbyDate(int empidforalonce, DateTime month, DateTime year)
        {

            //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
            ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
            //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());

            List<EmpLoan> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpLoans", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpLoan>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpLoan>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        var responseObject = response.Data.Where(x => x.EmployeeId == empidforalonce && x.Date >= month && year >= x.Date);
                        if (responseObject.Count() > 0)
                        {
                            return Json(responseObject);
                        }
                        else
                        {
                            return Json("");
                        }
                    }




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    return Json("");
                }
            }

            return View();
        }

        //list Taxes specific

        public IActionResult TaxesbyDate(int empidforalonce, DateTime month, DateTime year)
        {

            //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
            //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
            ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
            //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());

            List<EmpTax> responseObjectEmpAllownce;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpTaxes", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpTax>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpTax>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAllownce = response.Data;
                    for (int i = 0; i < response.Data.Count(); i++)
                    {
                        var responseObject = response.Data.Where(x => x.EmployeeId == empidforalonce && x.Date >= month && year >= x.Date);
                        if (responseObject.Count() > 0)
                        {
                            return Json(responseObject);
                        }
                        else
                        {
                            return Json("");
                        }
                    }




                    return View(responseObjectEmpAllownce);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }



        //list Attendencespecific

        public IActionResult AttendencebyDate(int empidforalonce, DateTime month, DateTime year, bool IncludeSS)
        {

            try
            {


                //int countdays = DateTime.DaysInMonth(Convert.ToInt32(year), Convert.ToInt32(month));
                //DateTime tfromdate = Convert.ToDateTime(year + "-" + month + "-" + "01");
                ////DateTime ttodate = Convert.ToDateTime("2021-10-01");
                //DateTime ttodate = Convert.ToDateTime(year + "-" + month + "-" + countdays.ToString());

                year = month.AddDays(13);


                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpAttendances/AttendanceByID?empid=" + empidforalonce + "&fromDate=" + month + "&toDate=" + year + "&IncludeSS=true", "GET");


                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpAttendanceRecord>>(ress.Resp);
                    if (response.Data != null)
                    {
                        var responseObject = response.Data;
                        return Json(responseObject);
                    }
                    else
                    {
                        return Json("");
                    }
                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        //EmpPayRoll Crud End Here

        //get tax
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


        //PAyroll Apprval
        public IActionResult PayRollApproval()
        {
            List<EmpPayRoll> responseObjectEmpLeave;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpPayRolls", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack<List<EmpPayRoll>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                SResponse ressemploye = RequestSender.Instance.CallAPI("api",
                                     "HR/EmployeeGet", "GET");
                if (ressemploye.Status && (ressemploye.Resp != null) && (ressemploye.Resp != ""))
                {
                    ResponseBack<List<Employee>> response1 =
                               JsonConvert.DeserializeObject<ResponseBack<List<Employee>>>(ressemploye.Resp);

                    responseObjectEmpLeave = response.Data;

                    ViewBag.EmployeeName = response1.Data;

                    return View(responseObjectEmpLeave);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyPayRollApproval(bool IsApprove, int EmpPayRollId)
        {
            EmpPayRoll empPayRoll = new EmpPayRoll();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpPayRolls" + "/" + EmpPayRollId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpPayRoll>>(ress12.Resp);

                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empPayRoll.EmpPayRollId = EmpPayRollId;
                        empPayRoll.IsApprove = IsApprove;
                        empPayRoll.Allowances = responseObject.Allowances;
                        empPayRoll.EmployeeId = responseObject.EmployeeId;
                        empPayRoll.BasicSalary = responseObject.BasicSalary;
                        empPayRoll.Deductions = responseObject.Deductions;
                        empPayRoll.DisperseDate = responseObject.DisperseDate;
                        empPayRoll.FromDate = responseObject.FromDate;
                        empPayRoll.Loans = responseObject.Loans;
                        empPayRoll.Month = responseObject.Month;
                        empPayRoll.Taxes = responseObject.Taxes;
                        empPayRoll.ToDate = responseObject.ToDate;
                        empPayRoll.TotalSalary = responseObject.TotalSalary;
                        empPayRoll.Year = responseObject.Year;

                        var body = JsonConvert.SerializeObject(empPayRoll);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpPayRolls" + "/" + EmpPayRollId, "PUT", body);

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

        public IActionResult GeneratePayRoll()
        {
            return View();
        }
        public IActionResult GenerateContractPayRoll()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GeneratePayRoll(EmpPayRoll empPayRoll)
        {
            var empidforalonce = empPayRoll.FromDate;
            var month = empPayRoll.FromDate;
            var contracttype = empPayRoll.Month;



            SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls/GeneratePayrollByDate?fromDate=" + empidforalonce + "&toDate=" + month + "&contractname=" + contracttype, "GET");


            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                //ResponseBack<List<EmpPayRoll>> response =
                //             JsonConvert.DeserializeObject<ResponseBack<List<EmpPayRoll>>>(ress.Resp);
                var response = JsonConvert.DeserializeObject<ResponseBack<EmpPayRoll>>(ress.Resp);

                if (response.Data == null)
                {
                    //responseObjectEmpAllownce = response.Data;

                    TempData["RecordGenerate"] = "Not Added Record Already Exist";
                    return View();

                }
                else
                {
                    if(response.Data.EmpContractName == "Contract")
                    {
                        TempData["RecordGenerate"] = "Added Successfully";
                        return RedirectToAction("ManageContractPayRoll");
                    }
                    TempData["RecordGenerate"] = "Added Successfully";
                    return RedirectToAction("ManagePayRoll");
                }

            }
            return View();
        }
        public IActionResult EmployeemManualPayroll(int empidforalonce, DateTime startdate)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api", "EmpPayRolls/GenerateManualPayroll?empid=" + empidforalonce + "&startDate=" + startdate + "", "GET");

                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ManualPayRoll>(ress.Resp);


                    if (response != null)
                    {
                        var responseObject = response;
                        return Json(responseObject);
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

                throw;
            }


        }
    }
}