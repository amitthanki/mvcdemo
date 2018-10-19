using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using System.Web.Mvc;
using ENRLReconSystem.Utility;
using System.Reflection;

namespace ENRLReconSystem.Controllers
{
    public class ReportsController : Controller
    {
        private UIUserLogin currentUser;       
        

        public ReportsController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
               
            }
        }
        public ActionResult Reports()
        {
            try
            {
                var businessSegment = currentUser.BusinessSegmentLkup;
                var role = currentUser.RoleLkup;
                var workBasket = currentUser.WorkBasketLkup;
                var selectacc = currentUser.UserReports.Where(x => x.RoleLkup.Equals(role) && x.WorkBasketLkup.Equals(workBasket)).ToList();
                var user = currentUser;
                BLReports objBLReports = new BLReports();
                string errorMessage = string.Empty;
                List<DORPT_ReportsMaster> reports = new List<DORPT_ReportsMaster>();
                List<DORPT_ReportsMaster> finalReports = new List<DORPT_ReportsMaster>();
                ExceptionTypes result = objBLReports.GetAllReports(0, null, out reports, out errorMessage);
                reports = reports.Where(x => x.ViewInUI == true).ToList();
                ViewBag.BusinessSegment = currentUser.BusinessSegmentLkup;
                finalReports = (from r in reports join s in selectacc.ToList() on r.RPT_ReportsMasterId equals s.RPT_ReportsMasterId select r).Distinct().OrderBy(x => x.ReportName).ToList();
              
                return View(finalReports);
            }
            catch (Exception ex)
            {

                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Reports, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
   
        }
        [HttpPost]
        public ActionResult Reports(long id)
        {
            return View();
        }
    }
}