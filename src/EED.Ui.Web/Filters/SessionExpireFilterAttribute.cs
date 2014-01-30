using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EED.Ui.Web.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, 
        Inherited = true, AllowMultiple = true)]
    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["projectId"] == null)
            {
                filterContext.Result = new RedirectResult("/Project/List");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}