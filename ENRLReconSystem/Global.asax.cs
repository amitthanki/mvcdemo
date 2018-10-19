
using ENRLReconSystem.Common;
using ENRLReconSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ENRLReconSystem
{
    public class MvcApplication : System.Web.HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
        }
        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;///Added to avoid version disclose from response header
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(typeof(CustomControllerFactory));
        }
    }
}
