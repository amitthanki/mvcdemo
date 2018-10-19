using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using ENRLReconSystem.BL;

namespace ENRLReconSystem
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Constructor, Inherited = true, AllowMultiple = false)]
    public class ERSAuthenticationAttribute: AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var obj = httpContext.Session[ConstantTexts.CurrentUserSessionKey];
            base.AuthorizeCore(httpContext);
            if (obj == null)
            {
                return false;
            }
            return true;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var obj = filterContext.HttpContext.Session[ConstantTexts.CurrentUserSessionKey];
            if (obj == null)
            {
                //for time out handling
                filterContext.Result = new RedirectToRouteResult(
                     new RouteValueDictionary
                    {
                       { "action", "Login"  },
                       { "controller", "Login" }
                    });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                { "action", "Login" },
                { "controller", "Login" }
              });
            }
        } 
}
}