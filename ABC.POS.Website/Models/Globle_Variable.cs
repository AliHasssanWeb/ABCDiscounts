using ABC.Shared.DataConfig;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Mvc;

namespace ABC.POS.Website.Models
{
   public class Globle_Variable : ActionFilterAttribute
    {
        public override void  OnActionExecuting(ActionExecutingContext context)
        {
          var CurrentUser =  context.HttpContext.Session.GetString("userobj");
            if(CurrentUser != null)
            {
                var userDetail = JsonConvert.DeserializeObject<UserDetailAuthToken>(CurrentUser);

                Microsoft.AspNetCore.Mvc.Controller controller = context.Controller as Microsoft.AspNetCore.Mvc.Controller;
                controller.ViewData["CurrentUserDetail"] = userDetail;
            }
            else
            {
                Microsoft.AspNetCore.Mvc.Controller controller = context.Controller as Microsoft.AspNetCore.Mvc.Controller;
                controller.TempData["response"] = "Session Expired";
                //context.Result = new RedirectResult("http://45.35.97.246:5595/pos");
                context.Result = new RedirectResult("https://localhost:5001/");
                // context.Result = new RedirectResult("http://10.10.10.98:5595/pos/");

            }




        }

    }
}
