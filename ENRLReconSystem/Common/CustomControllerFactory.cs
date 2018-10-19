using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ENRLReconSystem.Common
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// This code is used to redirect to login page, 
        /// if user try to access any other page with out login.Even it will hide controller initialization.
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                ///Avoid multiple time login on same browser when session already exists////
                if (requestContext.RouteData.Values["Controller"].Equals("Login") && requestContext.RouteData.Values["action"].Equals("Login"))
                {
                    var defaultRoute = requestContext.RouteData.Values;
                    defaultRoute["controller"] = "Home";
                    defaultRoute["action"] = "Home";
                    return base.CreateController(requestContext, "Home");
                }
                return base.CreateController(requestContext, controllerName);
            }
            else if (controllerName == "Login" || controllerName == "MIIMConnector")
            {
                return base.CreateController(requestContext, controllerName);
            }
            else if (requestContext.HttpContext.Request.IsAjaxRequest() && System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey].IsNull())
            {
                //Ajax request doesn't return to login page, it just returns 401 error.
                var defaultRoute = requestContext.RouteData.Values;
                defaultRoute["action"] = "SessionTimeOut";
                return base.CreateController(requestContext, "Auth");

            }


            var routeValues = requestContext.RouteData.Values;
            routeValues["action"] = "PreAuth";
            return base.CreateController(requestContext, "Auth");
        }
    }
}