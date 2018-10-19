using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using ENRLReconSystem.DO;
using System.Reflection;

namespace ENRLReconSystem.Common
{
    public class ErrorHandlerAttribute: HandleErrorAttribute
    {
       /// <summary>
       /// Used to handle excpetion
       /// </summary>
       /// <param name="filterContext"></param>
    public override void OnException(ExceptionContext filterContext)
        {
            
            try
            {
                var controlName = filterContext.RouteData.Values["controller"];
                var action = filterContext.RouteData.Values["action"];
                string exMessage = filterContext.Exception.InnerException.Message;
                string userId = filterContext.HttpContext.User.Identity.Name.ToString();

                if (userId != "" && userId != string.Empty)
                {
                    BLCommon.LogError(0, controlName + "/" + action, (long)ErrorModuleName.ErrorHandler, (long)ExceptionTypes.Uncategorized, exMessage, "");
                }
                else
                {
                    //If User id not avaliable we will insert MSID 
                    BLCommon.LogError(0, controlName + "/" + action, (long)ErrorModuleName.ErrorHandler, (long)ExceptionTypes.Uncategorized, exMessage, "");
                }
                base.OnException(filterContext);
            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString() , (long)ErrorModuleName.ErrorHandler, (long)ExceptionTypes.Uncategorized, ex.InnerException.Message, "");
            }
           
        }
    }
}