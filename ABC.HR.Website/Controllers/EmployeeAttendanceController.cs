using ABC.EFCore.Repository.Edmx;
using ABC.HR.Domain.DataConfig;
using ABC.HR.Domain.Entities;
using ABC.Shared.DataConfig;
using ExcelDataReader;
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

namespace ABC.HR.Website.Controllers
{
    //[Area("PayRoll")]
    public class EmployeeAttendanceController : Controller
    {
        //EmpAttendance Crud Start Here

        public IActionResult AddEmployeeAttendance()
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

            return View();
        }


        [HttpPost]
        public IActionResult AddEmployeeAttendance(EmpAttendance empAttendance)
        {
            try
            {


                var body = JsonConvert.SerializeObject(empAttendance);
                // var body = sr.Serialize(obj);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances", "POST", body);
                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseofAddAttendances"] = "Add Successfully";
                    return RedirectToAction("ManageEmployeeAttendance");
                }
                else
                {
                    TempData["responseofAddAttendances"] = resp.Resp + " " + "Unable To Updates";
                    return RedirectToAction("AddEmployeeAttendance");
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
        public IActionResult ManageEmployeeAttendance()
        {

            List<EmpAttendance> responseObjectEmpAttendance;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAttendances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpAttendance>> response =
                             JsonConvert.DeserializeObject<ResponseBack < List <EmpAttendance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAttendance = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;


                    return View(responseObjectEmpAttendance);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }

        public IActionResult UpdateEmployeeAttendance(int id)
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




                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpAttendances" + "/" + id, "GET");
                if (ress12.Status && (ress12.Resp != null) && (ress12.Resp != ""))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack <ValidateEmpAttendance>>(ress12.Resp);
                    if (response != null)
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
        public IActionResult UpdateEmployeeAttendance(int id, EmpAttendance empAttendance)
        {

            try
            {

                empAttendance.EmpAttendanceId = id;
                var body = JsonConvert.SerializeObject(empAttendance);
                SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances" + "/" + id, "PUT", body);

                if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                {
                    TempData["responseUpdateAttendance"] = "Updated Successfully";
                    return RedirectToAction("ManageEmployeeAttendance");
                }
                else
                {
                    TempData["responseUpdateAttendance"] = resp.Resp + " " + "Unable To Update";
                    return RedirectToAction("ManageEmployeeAttendance");
                }



            }
            catch (Exception)
            {

                throw;
            }
        }


        public IActionResult DeleteEmployeeAttendance(int id)
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
              "EmpAttendances" + "/" + id, "Delete");


                if (ress.Status && (ress.Resp != null))
                {
                    if (ress.Status != false)
                    {
                        TempData["MsgDeleteAttendance"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeAttendance");
                    }
                    else
                    {
                        TempData["MsgDeleteAttendance"] = "Delete Successfully";
                        return RedirectToAction("ManageEmployeeAttendance");
                    }
                }
                return RedirectToAction("ManageEmployeeAttendance");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //EmpAttendance Crud End Here

        //ATTANDANCE Approvells
        public IActionResult AttendanceApproval()
        {

            List<EmpAttendance> responseObjectEmpAttendance;

            SResponse ress = RequestSender.Instance.CallAPI("api",
                 "EmpAttendances", "GET");

            if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
            {
                ResponseBack < List <EmpAttendance>> response =
                             JsonConvert.DeserializeObject<ResponseBack<List<EmpAttendance>>>(ress.Resp);


                if (response.Data.Count() > 0)
                {
                    responseObjectEmpAttendance = response.Data;
                    ListforEmployees();
                    ViewBag.EmployeeName = response1.Data;


                    return View(responseObjectEmpAttendance);
                }
                else
                {
                    TempData["response"] = "Server is down.";
                }
            }

            return View();
        }
        public IActionResult ModifyAttendanceApproval(bool IsApprove, int EmpAttendanceId)
        {
            EmpAttendance empAttendance = new EmpAttendance();
            try
            {
                SResponse ress12 = RequestSender.Instance.CallAPI("api",
               "EmpAttendances" + "/" + EmpAttendanceId, "GET");


                if (ress12.Status && (ress12.Resp != null))
                {
                    var response = JsonConvert.DeserializeObject<ResponseBack<EmpAttendance>>(ress12.Resp);




                    if (ress12.Status != false)
                    {
                        var responseObject = response.Data;
                        empAttendance.EmpAttendanceId = EmpAttendanceId;
                        empAttendance.IsApprove = IsApprove;
                        empAttendance.EmployeeId = responseObject.EmployeeId;
                        empAttendance.TimeIn = responseObject.TimeIn;
                        empAttendance.TimeOut = responseObject.TimeOut;
                        empAttendance.Late = responseObject.Late;
                        empAttendance.AttendanceDate = responseObject.AttendanceDate;

                        var body = JsonConvert.SerializeObject(empAttendance);
                        SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances" + "/" + EmpAttendanceId, "PUT", body);

                        TempData["MsgApprovalAllownce"] = "Added Successfully";
                        return RedirectToAction("AttendanceApproval");
                    }
                    else
                    {
                        TempData["MsgApprovalAllownce"] = "Not Work";
                        return RedirectToAction("AttendanceApproval");
                    }
                }
                return RedirectToAction("AttendanceApproval");
            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
    
        public IActionResult UploadAttendance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadAttendance(EmpAttendance model,IFormFile AttendanceFile)
        {
            List<EmpAttendance> empAttendances = new List<EmpAttendance>();

            string uniqueFileName = null;
            int row = 0;
           if (model != null)
            {
                string path1 = @"wwwroot";

                string fullPath;

                fullPath = Path.GetFullPath(path1);
                string uploadsFolder = fullPath + "/ExcelFiles";
                uniqueFileName = Guid.NewGuid().ToString() + "_" + AttendanceFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    AttendanceFile.CopyTo(fileStream);
                }
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                //open file and returns as Stream
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    //Below is for the XLS format
                    //ExcelReaderFactory.CreateBinaryReader 

                    //below is for both
                    using (var reader = ExcelReaderFactory.CreateReader(stream))                   
                    {

                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = false
                            }
                        };

                        var dataSet = reader.AsDataSet(conf);

                        // Now you can get data from each sheet by its index or its "name"
                        var dtfileexcel = dataSet.Tables[0];
                        var dtfileexcel11 = dataSet.Tables[0].Rows;

                        DateTime attendendate = DateTime.Now;
                        DateTime attendenfirstdate = DateTime.Now;
                        DateTime attendenlastdate = DateTime.Now;

                        //////////////////////////////////////////////////////////
                        ///by forecah read file
                        ///sammple
                        List<object> rowDataList = null;
                        List<object> allRowsList = new List<object>();
                         foreach (System.Data.DataRow item in dtfileexcel11)
                        {
                            
                            string getvalue = "";

                            rowDataList = item.ItemArray.ToList(); //list of each rows

                            for (int i = 0; i < rowDataList.Count; i++)
                            {
                                if (!DBNull.Value.Equals(rowDataList[i]))
                                {
                                     getvalue = rowDataList[i].ToString();
                                    allRowsList.Add(getvalue);
                                }
                            }
                            var dataasdasa = rowDataList[1].GetType();
                            //allRowsList.Add(rowDataList); //adding the above list of each row to another list
                        }
                        //////////////////////////////////////////////////////////////
                        var punchdate = Convert.ToString(dtfileexcel.Rows[0][0]);
                        string[] temp1 = punchdate.Split("\n");
                        var datepunchh = temp1[1];
                        string[] date = datepunchh.Split("-");
                        DateTime attendateee = DateTime.Parse(date[0]);

                        attendenfirstdate = DateTime.Parse(date[0]);
                        CultureInfo culture = new CultureInfo("en-US");
                        attendenlastdate  = DateTime.Parse(date[1],culture);

                        var termintateread = attendenlastdate.Day;

                        string mothofattendance = (attendateee.Month).ToString();
                        string yearofattance = (attendateee.Year).ToString();
                        ////////////////////////////////////////////////////////////
                        int getiddd = 0;
                         var dayofateend = "";

                            for (int roww = 2; roww < dtfileexcel.Rows.Count; roww = roww + 4)
                            {



                                var empidff = "";
                                string[] temp = { "" };
                                if (temp[0] == "Enroll")
                                {
                                    getiddd = 0;
                                }

                                
                                for (int column = 0; column <= dtfileexcel.Columns.Count - 1; column++)
                                {


                                ListforEmployees();
                                var empgetalldata = response1.Data;
                                 
                                


                                   


                                if (!DBNull.Value.Equals(dtfileexcel.Rows[roww][column]))
                                    {

                                        empidff = Convert.ToString(dtfileexcel.Rows[roww][column]);
                                        temp = empidff.Split(' ');
                                        if (temp[0] == "Enroll")
                                        {
                                            getiddd = Convert.ToInt32(temp[2]);
                                        }



                                    }
                                int? checkemloyeeexist = 0;
                                foreach (var empgetalldatame in empgetalldata)

                                {
                                    model.EmployeeId = 0; 

                                    var empcodeee = empgetalldatame.EmployeeCode;
                                    string[] takeidcodeid = empcodeee.Split('-');


                               if (takeidcodeid[1] == getiddd.ToString())
                               {
                                model.EmployeeId = empgetalldatame.EmployeeId;
                                        checkemloyeeexist = model.EmployeeId;
                                        break;
                               }
                                    checkemloyeeexist = model.EmployeeId;


                                }

                                if (!DBNull.Value.Equals(dtfileexcel.Rows[roww + 1][column]))
                                    {
                                         dayofateend = dtfileexcel.Rows[roww + 1][column].ToString();

                                        DateTime finaldatae = DateTime.Parse(yearofattance + "-" + mothofattendance + "-" + dayofateend);

                                        model.AttendanceDate = finaldatae;
                                    }
                                    if (!DBNull.Value.Equals(dtfileexcel.Rows[roww + 2][column]))
                                    {
                                        var timeoouutt = dtfileexcel.Rows[roww + 2][column].ToString();
                                        if (timeoouutt == "")
                                        {
                                            model.TimeIn = ("00:00");
                                            model.TimeOut = ("00:00");
                                        }
                                        else
                                        {
                                            string[] gettime = timeoouutt.Split(' ');
                                        //var timein = gettime[0];
                                        //var timeout = gettime[1];

                                        //model.TimeIn = timein;
                                        //model.TimeOut = timeout;


                                        if (gettime[0] == "")
                                        {
                                            //model.TimeIn = ("00:00");
                                            model.TimeIn = gettime[1];
                                        }
                                        else
                                        {
                                            model.TimeIn = gettime[0];
                                        }
                                        if (gettime[1] == "")
                                        {
                                            model.TimeOut = gettime[0];
                                        }
                                        else
                                        {
                                            model.TimeOut = gettime[1];
                                        }
                                    }


                                    }
                                    model.IsApprove = false;
                                    model.Late = "0";
                                    model.OverTime = "0";

                                if(checkemloyeeexist != 0) { 
                                    var body = JsonConvert.SerializeObject(model);
                                    // var body = sr.Serialize(obj);
                                    SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances", "POST", body);
                                    if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                                    {
                                        TempData["responseoffileAttendances"] = "Add Successfully";

                                    }
                                    else
                                    {
                                        TempData["responseoffileAttendances"] = resp.Resp + " " + "Unable To Add";

                                    }

                                }
                                if (termintateread == Convert.ToInt32(dayofateend)) { break; }

                                  



                            }




                            }

                        

                        /////////////////////////////////////////////////////////////
                        /// sample file read
                        //for (row = 0; row <= dtfileexcel.Rows.Count - 1; row++)
                        //{ 


                        //    ListforEmployees();
                        //    var empgetalldata = response1.Data;
                        //    var empiddd = Convert.ToString(dtfileexcel.Rows[row][0]);
                        //    foreach (var empgetalldatame in empgetalldata)
                                 
                        //    {
                                

                        //        if (empgetalldatame.EmployeeCode == empiddd)
                        //        {
                        //            model.EmployeeId = empgetalldatame.EmployeeId;
                        //            //
                        //            var timeiinn = DateTime.Parse(dtfileexcel.Rows[row][1].ToString());
                        //            //var timeinextraa = timeiinn.ToString("t");
                        //            model.TimeIn = timeiinn.ToString("t");
                        //            //
                        //            var timeoouutt = DateTime.Parse(dtfileexcel.Rows[row][2].ToString());
                        //            model.TimeOut = timeoouutt.ToString("t");
                        //            // var timeouttextra = timeoouutt.ToString("t");
                        //            //var lateorover = Convert.ToInt32(timeouttextra) - Convert.ToInt32(timeinextraa);

                        //            attendendate = DateTime.Parse(dtfileexcel.Rows[row][3].ToString());
                        //            model.AttendanceDate = attendendate;

                        //            model.IsApprove = false;
                        //            model.Late = "0";
                        //            model.OverTime = "0";
                        //            if (row == 0)
                        //            {
                        //                attendenfirstdate = attendendate;
                        //            }
                        //            if (row == dtfileexcel.Rows.Count - 1)
                        //            {
                        //                attendenlastdate = attendendate;
                        //            }


                        //            var body = JsonConvert.SerializeObject(model);
                        //            // var body = sr.Serialize(obj);
                        //            SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances", "POST", body);
                        //            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                        //            {
                        //                TempData["responseoffileAttendances"] = "Add Successfully";

                        //            }
                        //            else
                        //            {
                        //                TempData["responseoffileAttendances"] = resp.Resp + " " + "Unable To Add";

                        //            }


                        //        }
                        //        else if(Convert.ToString(empgetalldatame.EmployeeId) == empiddd)
                        //        {
                        //            model.EmployeeId = Convert.ToInt32(empiddd);
                        //            //
                        //            var timeiinn = DateTime.Now;
                        //            if (dtfileexcel.Rows[row][2] != null )
                        //            {
                        //                 timeiinn = DateTime.Parse(dtfileexcel.Rows[row][1].ToString());
                        //            }
                        //            if (dtfileexcel.Rows[row][3] != null)
                        //            {
                        //                timeiinn = DateTime.Parse(dtfileexcel.Rows[row][1].ToString());
                        //            }
                        //            else
                        //            {
                        //                timeiinn = DateTime.Parse("00:00:00");
                        //            }
                        //            //var timeinextraa = timeiinn.ToString("t");
                        //            model.TimeIn = timeiinn.ToString("t");
                        //            //
                        //            var timeoouutt = DateTime.Parse(dtfileexcel.Rows[row][2].ToString());
                        //            model.TimeOut = timeoouutt.ToString("t");
                        //            // var timeouttextra = timeoouutt.ToString("t");
                        //            //var lateorover = Convert.ToInt32(timeouttextra) - Convert.ToInt32(timeinextraa);

                        //            attendendate = DateTime.Parse(dtfileexcel.Rows[row][3].ToString());
                        //            model.AttendanceDate = attendendate;

                        //            model.IsApprove = false;
                        //            model.Late = "0";
                        //            model.OverTime = "0";
                        //            if (row == 0)
                        //            {
                        //                attendenfirstdate = attendendate;
                        //            }
                        //            if (row == dtfileexcel.Rows.Count - 1)
                        //            {
                        //                attendenlastdate = attendendate;
                        //            }


                        //            var body = JsonConvert.SerializeObject(model);
                        //            // var body = sr.Serialize(obj);
                        //            SResponse resp = RequestSender.Instance.CallAPI("api", "EmpAttendances", "POST", body);
                        //            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
                        //            {
                        //                TempData["responseoffileAttendances"] = "Add Successfully";

                        //            }
                        //            else
                        //            {
                        //                TempData["responseoffileAttendances"] = resp.Resp + " " + "Unable To Add";

                        //            }

                        //        }
                        //    }

                                                                            
                                                
                             



                        //}

                        empAttendances.Add(model);
                        List<EmpAttendance> responseObjectEmpAttendance;

                        SResponse ress = RequestSender.Instance.CallAPI("api",
                             "EmpAttendances", "GET");

                        if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                        {
                            ResponseBack<List<EmpAttendance>> response =
                                         JsonConvert.DeserializeObject<ResponseBack<List<EmpAttendance>>>(ress.Resp);


                            if (response.Data.Count() > 0)
                            {
                                //
                                responseObjectEmpAttendance = response.Data;

                                var responseallcon = responseObjectEmpAttendance.Where(x => x.AttendanceDate >= attendenfirstdate && attendenlastdate  >= x.AttendanceDate).ToList();
                                ListforEmployees();
                                ViewBag.EmployeeName = response1.Data;
                                return View(responseallcon);
                            }

                        }



                    }
                }

            }
           

            //}
            //Set("FileUploadNotify", "File Uploaded Successfully", 5);
            return View();

        }
    }
}