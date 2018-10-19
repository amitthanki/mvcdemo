using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthentication(Roles ="User")]
    public class UserPreferenceController : Controller
    {
        private UIUserLogin currentUser;
        public UserPreferenceController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }         
        }
      
        /// <summary>
        /// To load User Prefernce page
        /// </summary>
        /// <returns></returns>
        public ActionResult UserPreference()
        {
            try
            {
                DOADM_UserPreference objDOADM_UserPreference = new DOADM_UserPreference();
                if (currentUser.ADM_UserPreference != null)
                {
                    objDOADM_UserPreference = currentUser.ADM_UserPreference;
                    ViewBag.ShowOSTQueueSummary = currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.OST;
                    ViewBag.ShowEligQueueSummary = currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR;
                    ViewBag.ShowRPRQueueSummary = currentUser.RoleLkup == (long)RoleLkup.Admin || currentUser.RoleLkup == (long)RoleLkup.Manager || currentUser.WorkBasketLkup == (long)WorkBasket.RPR;
                }
                LoadCommonData();
                return View(objDOADM_UserPreference);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.UserPreference, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
           
        }
         
        /// <summary>
        /// To save User Preferences data.
        /// </summary>
        /// <param name="objUserPreference">DOADM_UserPreference</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveUserPreference(DOADM_UserPreference objUserPreference)
        {
            try
            {                              
                string errorMessage= string.Empty;
                ExceptionTypes result = new ExceptionTypes();               
                objUserPreference.ADM_UserMasterRef = currentUser.ADM_UserMasterId;
               BLUserAdministration objUserAdministration = new BLUserAdministration();
               result = objUserAdministration.SaveUserPreference(objUserPreference, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.UserPreference, (long)ExceptionTypes.Uncategorized, "An error occured while saving userpreference.", "An error occured while saving userpreference.");
                    return Json(new { ID = result, Message = "An error occured while saving." });
                }
                if (currentUser != null)
                {
                    currentUser.ADM_UserPreference = objUserPreference;
                    System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] = currentUser;
                }
                string returnMessage = "User Preferences Saved Successfully.";
                return Json(new { ID = 0, Message = returnMessage });               
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.UserPreference, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }


        }

        /// <summary>
        /// To load all drop down values.
        /// </summary>
        private void LoadCommonData()
        {
            ViewBag.Correlations = currentUser.Correlations;
            ViewBag.LookUps = currentUser.LookUps;
            ViewBag.TimeZoneList = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone);
        }

        public ActionResult UserAccess()
        {
            ENRLReconSystem.DO.UIUserLogin loggedInUser = System.Web.HttpContext.Current.Session[ENRLReconSystem.Utility.ConstantTexts.CurrentUserSessionKey] as ENRLReconSystem.DO.UIUserLogin;
            ViewBag.userRole = ((ENRLReconSystem.Utility.RoleLkup)loggedInUser.RoleLkup).ToString();
            ViewBag.workBasket = ((ENRLReconSystem.Utility.WorkBasket)loggedInUser.WorkBasketLkup).ToString();

            ViewBag.businessSegment = CacheUtility.GetAllLookupsFromCache((long)ENRLReconSystem.Utility.LookupTypes.BusinessSegment).Where(x => x.CMN_LookupMasterId.Equals(@loggedInUser.BusinessSegmentLkup)).FirstOrDefault().LookupValue;

            string strAccessGroups = string.Empty;
            if (loggedInUser.UserAccessGroup != null && loggedInUser.UserAccessGroup.Count > 0)
            {
                foreach (var item in loggedInUser.UserAccessGroup.Where(x => x.RoleLkup == loggedInUser.RoleLkup && x.WorkBasketLkup == loggedInUser.WorkBasketLkup).ToList())
                {
                    if (strAccessGroups.Length == 0)
                        strAccessGroups = item.AccessGroupName;
                    else
                        strAccessGroups = strAccessGroups + "," + item.AccessGroupName;
                }
            }
            ViewBag.strAccessGroups = strAccessGroups;
            return PartialView("_UserAccess");
        }
    }
}