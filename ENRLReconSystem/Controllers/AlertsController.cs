using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Models;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace ENRLReconSystem.Controllers
{
    public class AlertsController : Controller
    {
        BLAlerts _objBLAlerts = new BLAlerts();
        BLReports _objBLReports = new BLReports();
        private UIUserLogin currentUser;
        public AlertsController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        // GET: Alerts
        [HttpGet]
        public ActionResult Search()
        {
            try
            {
                //return initial page with all records
                return View(PGetAlerts());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
            
        }

        [HttpPost]
        public ActionResult Search(string strTitle = "",string strDescription = "", bool bolIsActive = true)
        {
            try
            {
                //return partial view to be shown below Alerts search page with list of results
                return PartialView("_SearchResults", PGetAlerts(strTitle, strDescription, bolIsActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        private List<DOADM_AlertDetails> PGetAlerts(string strTitle = "", string strDescription ="", bool bolIsActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            //temporary object for search alert function
            DOADM_AlertDetails objDOADM_AlertDetails = new DOADM_AlertDetails();

            objDOADM_AlertDetails.IsActive = bolIsActive;
            objDOADM_AlertDetails.AlertTitle = strTitle;
            objDOADM_AlertDetails.AlertDescription = strDescription;
            objDOADM_AlertDetails.ConsiderDates = false;
            List<DOADM_AlertDetails> lstDOADM_AlertDetails;
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes result = _objBLAlerts.SearchAlerts(TimeZone, objDOADM_AlertDetails, out lstDOADM_AlertDetails, out errorMessage);
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.AlertsHistoryReport,string.Empty,out lstDORPT_ReportsMaster,out errorMessage);
           
            //check result for DB action
            if (result != (long)ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
            }
            else if (result == ExceptionTypes.ZeroRecords)
            {
                lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
            }
            objDOADM_AlertDetails.lstReports = lstDORPT_ReportsMaster;
            return lstDOADM_AlertDetails;
        }

        [HttpPost]
        public ActionResult GetAlertsReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.AlertsHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
           return Json(new { Data = urlData, JsonRequestBehavior.AllowGet } );
        }

        [HttpGet]
        public ActionResult Add(long AlertId = 0, bool isActive = true)
        {
            
            DOADM_AlertDetails objDOADM_AlertDetails = new DOADM_AlertDetails();
            objDOADM_AlertDetails.IsActive = true;
            string errorMessage = string.Empty;
            try
            {
                ExceptionTypes result;

                //get list of users for Send ALert to Indiviual drop down
                List<DOADM_UserMaster> lstUsers;
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                objDOADM_UserMaster.IsActive = true;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                //Filtered 1st three Users as Admin.sort list by email id 
                lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000).OrderBy(x => x.Email).ToList();

                //get list of Depatments for Send ALert to Department drop down
                List<DOCMN_Department> lstCMN_Department;
                BLDepartment objBLDepartment = new BLDepartment();
                DOCMN_Department objDOCMN_Department =  new DOCMN_Department();
                objDOCMN_Department.IsActive = true;
                result = objBLDepartment.SearchDepartment(TimeZone,objDOCMN_Department, out lstCMN_Department, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                //sort list by Depatment name
                lstCMN_Department = lstCMN_Department.OrderBy(x => x.ERSDepartmentName).ToList();
              
                if (AlertId != 0)
                {
                    //temporary list to hold search alerts results
                    List<DOADM_AlertDetails> lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
                    //temporary object for search alerts function
                    DOADM_AlertDetails objDOADM_AlertDetails_Find = new DOADM_AlertDetails();
                    objDOADM_AlertDetails_Find.ADM_AlertDetailsId = AlertId;
                    objDOADM_AlertDetails_Find.IsActive = isActive;
                    result = _objBLAlerts.SearchAlerts(TimeZone,objDOADM_AlertDetails_Find, out lstDOADM_AlertDetails, out errorMessage);
                    //check result for DB action
                    if (result != (long)ExceptionTypes.Success)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    }
                    if (lstDOADM_AlertDetails.Count > 0)
                        objDOADM_AlertDetails = lstDOADM_AlertDetails.FirstOrDefault();
                }

                objDOADM_AlertDetails.lstUsers = lstUsers;
                objDOADM_AlertDetails.lstCMN_Department = lstCMN_Department;
                //Alert criticality lookup from Cache
                objDOADM_AlertDetails.lstAlertCriticalityLkup = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AlertCriticality);
                //Send Alert To lookup from Cache
                objDOADM_AlertDetails.lstSendAlertToLkup = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SendAlertTo);
                //Alert Time Zone lookup from Cache
                objDOADM_AlertDetails.lstTimeZone = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone);
                return View(objDOADM_AlertDetails);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult Add(DOADM_AlertDetails objDOADM_AlertDetails)
        {
            string errorMessage = string.Empty;
            long loginUserId = currentUser.ADM_UserMasterId;
            string returnMessage = "";
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                //check if this call is to update record or save new record
                if (objDOADM_AlertDetails.ADM_AlertDetailsId > 0)
                {
                    //if old record is to be updated
                    BLCommon objCommon = new BLCommon();
                    //check if record is locked by current user
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Alerts, objDOADM_AlertDetails.ADM_AlertDetailsId))
                    {
                        //if record is not locked by current user dont save record and return error.
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }
                    
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    //If new record to be saved
                    returnMessage = "Record saved successfully.";
                }
                objDOADM_AlertDetails.LoginUserId = loginUserId;
                result = _objBLAlerts.SaveAlert(objDOADM_AlertDetails, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB." });
            }
        }

        [HttpGet]
        public ActionResult ReadAlert(long AlertId = 0)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            DOADM_AlertDetails objDOADM_AlertDetails = new DOADM_AlertDetails();
            objDOADM_AlertDetails.IsActive = true;
            string errorMessage = string.Empty;
            try
            {
                ExceptionTypes result;
                if (AlertId != 0)
                {
                    //temporary list to hold search alerts results
                    List<DOADM_AlertDetails> lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
                    //temporary object for search alerts function
                    DOADM_AlertDetails objDOADM_AlertDetails_Find = new DOADM_AlertDetails();
                    objDOADM_AlertDetails_Find.ADM_AlertDetailsId = AlertId;
                    objDOADM_AlertDetails_Find.IsActive = true;
                    result = _objBLAlerts.SearchAlerts(TimeZone,objDOADM_AlertDetails_Find, out lstDOADM_AlertDetails, out errorMessage);
                    //check result for DB action
                    if (result != (long)ExceptionTypes.Success)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    }
                    if (lstDOADM_AlertDetails.Count > 0)
                        objDOADM_AlertDetails = lstDOADM_AlertDetails.FirstOrDefault();
                }
                return PartialView("_ReadAlert", objDOADM_AlertDetails);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Alerts, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
    }
}