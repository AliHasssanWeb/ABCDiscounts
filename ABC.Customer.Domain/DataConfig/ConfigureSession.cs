using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Customer.Domain.DataConfig
{
    public class ConfigureSession : ActionFilterAttribute
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            throw new NotImplementedException();

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string sessionData = filterContext.HttpContext.Session.GetString("userData");
            if (string.IsNullOrEmpty(sessionData))
            {
                Controller controller = filterContext.Controller as Controller;
                controller.TempData["sessionExpired"] = "Your session expired, Please login again";
                // var url = $"{filterContext.HttpContext.Request.Scheme}://{filterContext.HttpContext.Request.Host.Value}";
                // var url = "http://localhost:25601/";
                var url = "http://199.231.160.216/cms-qa/";
                filterContext.Result = new RedirectResult("http://abc-discounts.com/Security/Account/Login");
            }
        }
    }
}
