using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ENRLReconSystem.Common
{
    public class Filter: ActionFilterAttribute
    {
        public long WorkBasketLkup { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                var objUserSession = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as ENRLReconSystem.DO.UIUserLogin;
                if (objUserSession.WorkBasketLkup != WorkBasketLkup.ToLong())
                {
                    //for time out handling
                    filterContext.Result = new RedirectToRouteResult(
                         new RouteValueDictionary
                        {
                       { "action", "Login"  },
                       { "controller", "Login" }
                        });
                }

            }
        }

    }
}