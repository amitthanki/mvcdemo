using System;
using ENRLReconSystem.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;

namespace ENRLReconSystem.Controllers
{
    [ERSRoleAuthorize(ERSAuthenticationRoles.User)]
    public class ERSAdminController : Controller
    {
        // GET: ERSAdmin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult  ClearApplicationCache(string key="")
        {
            try
            {
                if (key != "")
                {
                    System.Web.HttpContext.Current.Cache.Remove(key);
                }
                else
                {
                    foreach (DictionaryEntry dEntry in System.Web.HttpContext.Current.Cache)
                    {
                        System.Web.HttpContext.Current.Cache.Remove(dEntry.Key.ToString());
                    }
                }
                
                return Json(new { ID = 0, Message = "Cache cleared successfully."});

            }
            catch (Exception)
            {
                return Json(new { ID = 1, Message = "An error occoured." });


            }
           
        }
    }
}