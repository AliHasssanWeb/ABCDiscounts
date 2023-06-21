using ABC.Admin.Domain.DataConfig;
using ABC.EFCore.Entities.HR;
using ABC.EFCore.Repository.Edmx;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ABC.Admin.Domain.DataConfig.RequestSender;
namespace ABC.Admin.Website.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TerminalLogs()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                                 "Terminal/TerminalAcccessGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<TerminalAccess>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<TerminalAccess>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<TerminalAccess> responseObject = response.Data;
                        return View(responseObject);
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult ActivityLogs()
        {
            try
            {
                SResponse ress = RequestSender.Instance.CallAPI("api",
                                 "ActivityLogs/AdminActivityLogsGet", "GET");
                if (ress.Status && (ress.Resp != null) && (ress.Resp != ""))
                {
                    ResponseBack<List<ActivityLog>> response =
                                 JsonConvert.DeserializeObject<ResponseBack<List<ActivityLog>>>(ress.Resp);
                    if (response.Data.Count() > 0)
                    {
                        List<ActivityLog> responseObject = response.Data;
                        return View(responseObject.OrderByDescending(x=>x.Id).ToList());
                    }
                    else
                    {
                        TempData["response"] = "Request not completed.";
                        return RedirectToAction("Index", "Home");
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["response"] = "Request not completed." + ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
