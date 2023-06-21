using ABC.EFCore.Repository.Edmx;
using ABC.POS.Domain.DataConfig;
using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ABC.POS.Domain.DataConfig.RequestSender;

namespace ABC.POS.Website.Controllers
{
    public static class HelperClass
    {
        public static void activitylog(string actionname, string controllername, int userid, string UserName, string pageName)
        {
            var activitylog = new ActivityLog();
            activitylog.OperationName = actionname;
            activitylog.PageName = pageName;
            activitylog.OperationName = actionname;
            activitylog.UserId = userid;
            activitylog.CreatedBy = UserName;
            activitylog.CreatedDate = DateTime.Now;

            var body = JsonConvert.SerializeObject(activitylog);
            SResponse resp = RequestSender.Instance.CallAPI("api", "Inventory/ActivitylogCreate", "POST", body);
            if (resp.Status && (resp.Resp != null) && (resp.Resp != ""))
            {
                ResponseBack<ActivityLog> responseobj = JsonConvert.DeserializeObject<ResponseBack<ActivityLog>>(resp.Resp);

                //var srt = JsonConvert.SerializeObject(responseobj.Data);
                //httpContextAccessor.HttpContext.Session.SetString("CurrentProductdata", srt);
                //if (responseobj != null)
                //{
                //    TempData["Msg"] = "Add Successfully";
                //}
                //TempData["Msg"] = "Add Successfully";

                //return Content("true");
            }


        }
    }
}
