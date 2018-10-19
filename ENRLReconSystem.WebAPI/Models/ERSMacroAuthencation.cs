using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Threading;
using System.Threading.Tasks;

namespace ENRLReconSystem.WebAPI.Models
{

    public class ERSMacroAuthencation : AuthorizeAttribute
    {


        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    base.OnActionExecuting(actionContext);
        //    actionContext.Request.Properties[actionContext.ActionDescriptor.ActionName] = Stopwatch.StartNew();
        //}

        //public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        //{
        //    base.OnActionExecuted(actionExecutedContext);
        //    Stopwatch stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[actionExecutedContext.ActionContext.ActionDescriptor.ActionName];
        //    Trace.WriteLine(actionExecutedContext.ActionContext.ActionDescriptor.ActionName + "Rr" + stopwatch.Elapsed);
        //}

        public string loginName { get; set; }
        public string dbError = string.Empty;

        public override void OnAuthorization(HttpActionContext context)
        {
            try
            {
                bool result = false;
                List<string> userMemberOf = QueryAd(context);

                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "Returned from Query Add", "");
                //base.IsAuthorized(context);
                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "Base authorize call succesful", "");

                //if (userMemberOf != null && userMemberOf.Count > 0 && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && !String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                //{
                //    BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "User member count greater than 0", "");

                //    UIUserLogin loggedInUser;

                //    context.Request.Properties.Add("userDetailId", HttpContext.Current.User.Identity.Name.Replace("MS\\", ""));
                //    loginName = HttpContext.Current.User.Identity.Name.Replace("MS\\", "");
                //    BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                //    ExceptionTypes res = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out loggedInUser);
                //    if (res == ExceptionTypes.ZeroRecords)
                //    {
                //        dbError = "You are not part of ERS DB, Please contact your Administrator.";
                //        result = false;
                //    }
                //    else
                //    {
                //        result = true;
                //    }

                //}
                //else
                //{
                //    BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "User member count smaller than 0", "");
                //    result = false;
                //}

                if (userMemberOf != null && userMemberOf.Count > 0)
                {
                    //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "userMemberOf != null && userMemberOf.Count > 0", "");
                    List<string> roles = this.Roles.Split(',').ToList();
                    //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "roles.Count" + roles.Count, "");

                    if (roles != null && roles.Count > 0)
                    {
                        if (roles.Any(m => userMemberOf.Contains(m)))
                            result = true;
                        else
                            result = false;
                    }
                }

                if (!result)
                    HandleUnauthorizedRequest(context);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "exception" + ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// Handle UnAuthorized Request for IVR user login
        /// </summary>
        /// <param name="actionContext"></param>
        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (dbError != "")
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response.Content = new StringContent(dbError);
            }
            else
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                actionContext.Response.Content = new StringContent("You are not part of any AD Groups, Please contact your Administrator.");
            }



        }

        /// <summary>
        /// Fetch ACL Groups from DB and check Logged In User belon to which all ACL groups
        /// </summary>
        /// <param name="filterContext">Current context</param>
        /// <returns>UserPrincipal Object</returns>
        private List<string> QueryAd(HttpActionContext context)
        {

            List<string> userMemberOf = new List<string>();
            try
            {
                string ntGroup = ConfigurationManager.AppSettings["AdmOSTSID"].ToString();

                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "NT Group Sid: "+ ntGroup, "");

                string loggedInUserMsid = (HttpContext.Current.User != null
                                && HttpContext.Current.User.Identity != null
                                && !String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                            ? HttpContext.Current.User.Identity.Name.Replace("MS\\", "") : "";

                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "Logged User Id: " + loggedInUserMsid, "");
                WindowsIdentity wi = HttpContext.Current.User.Identity as WindowsIdentity;
                var grp = wi.Groups.ToList();
                bool rtnValue = false;
                rtnValue = grp.Exists(p => p.Value == ntGroup);

                if (rtnValue)
                {
                    //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "Return Value: " + rtnValue, "");
                    userMemberOf.Add(ConfigurationManager.AppSettings["MacroRole"]);
                }
                //BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, "Return Value: " + rtnValue, "");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
            }
            return userMemberOf;
        }
    }
}
