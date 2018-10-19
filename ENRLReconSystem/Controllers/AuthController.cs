using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    /// This controller is used only to redirect to login, if user session not present and user try to access other pages.
    /// So no need to add any authentication or authorize attributes to this controller.
    public class AuthController : Controller
    {
        /// <summary>
        /// This controller is used only to redirect to login, if user session not present and user try to access other pages.
        /// So no need to add any authentication or authorize attributes to this controller.
        /// </summary>
        /// <returns></returns>
        public ActionResult PreAuth()
        {
            if (Session[ConstantTexts.CurrentUserSessionKey].IsNull())
            {
                ViewBag.Error = "Your session is expired.";
            }
            return RedirectToAction("Login", "Login");
        }
        /// <summary>
        /// Session timeout Action when request ia a Ajax Request
        /// </summary>
        /// <returns></returns>
        public ActionResult SessionTimeOut()
        {
            return Json(new { ID = ExceptionTypes.SessionTimeOut, Message = "Your session is expired. Please Re-Login the application." });
        }
    }

}