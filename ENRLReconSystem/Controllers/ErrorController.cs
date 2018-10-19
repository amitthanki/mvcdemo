using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    [ERSRoleAuthorize(ERSAuthenticationRoles.User)]
    public class ErrorController : Controller
    {
        //not given authorization attribute as it may cause recursive calling of the action
        public ActionResult Unauthenticated(string Error)
        {
            ViewBag.ErrorMessage = Error;
            return View();
        }

        //not given authorization attribute as it may cause recursive calling of the action
        public ActionResult Unauthorized(string Error)
        {
            ViewBag.ErrorMessage = Error;
            return View();
        }

       
        public ActionResult Maintenance(string Error)
        {
            ViewBag.ErrorMessage = Error;
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.ErrorMessage = "Error Occoured";
            return View("Maintenance");
        }
        public ActionResult NotFound()
        {
            ViewBag.ErrorMessage = "Not Found";
            return View("Maintenance");
        }
        public ActionResult BadRequest()
        {
            ViewBag.ErrorMessage = "Bad Request";
            return View("Maintenance");
        }
    }
}