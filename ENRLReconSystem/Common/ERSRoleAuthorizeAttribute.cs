using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
namespace ENRLReconSystem
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Constructor, Inherited = true, AllowMultiple = false)]
    public class ERSRoleAuthorizeAttribute : AuthorizeAttribute
    {          
        /// <summary>
        /// Property which will hold the roles specified in attribute.
        /// </summary>
        public ERSRoleAuthorizeAttribute(params object[] roles)
        {
            if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
            {
                throw new ArgumentException(
                    "The roles parameter may only contain enums",
                    "roles");
            }

            var temp = roles.Select(r =>
                    Enum.GetName(r.GetType(), r))
                .ToList();

            Roles = string.Join(",", temp);
        }

        /// <summary>
        /// Override method to redirect the current request to Error controller if not authorized
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (System.Web.Security.Roles.IsUserInRole(ERSAuthenticationRoles.User.ToString())) //user in DB but do not have access.
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary {
                        { "action", "Unauthorized" },
                        { "controller", "Error" }
                    });
            }
            else//user not in DB
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary {
                        { "action", "Unauthenticated" },
                        { "controller", "Error" }
                    });
            }
        }
    }
}