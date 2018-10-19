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
    public class ResourcesController : Controller
    {
        BLResources _objBLResources = new BLResources();

        private UIUserLogin currentUser;
        public ResourcesController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        private BLReports _objBLReports;
        // GET: Resources
        [HttpGet]
        public ActionResult Search()
        {
            try
            {
                //return initial page with all records
                return View(PGetResource());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }
            
        }

        [HttpPost]
        public ActionResult Search(string strName ="", string strDescription ="",string strLinkLocation ="", bool bolIsActive = true)
        {
            try
            {
                //return partial view to be shown below Resource search page with list of results
                return PartialView("_SearchResults", PGetResource(strName,strDescription,strLinkLocation,bolIsActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }
        }

        [HttpGet]
        public ActionResult Add(long ResourceId = 0,bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            DOADM_ResourceDetails objDOADM_ResourceDetails = new DOADM_ResourceDetails();
            objDOADM_ResourceDetails.IsActive = true;
            string errorMessage = string.Empty;
            try
            {
                ExceptionTypes result;
                //get list of Depatments for Department drop down
                List<DOCMN_Department> lstCMN_Department;
                BLDepartment objBLDepartment = new BLDepartment();
                DOCMN_Department objDOCMN_Department = new DOCMN_Department();
                objDOCMN_Department.IsActive = true;
                result = objBLDepartment.SearchDepartment(TimeZone,objDOCMN_Department, out lstCMN_Department, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                //sort list by Depatment name
                lstCMN_Department = lstCMN_Department.OrderBy(x => x.ERSDepartmentName).ToList();

                if (ResourceId != 0)
                {
                    //temporary list to hold search Resource results
                    List<DOADM_ResourceDetails> lstDOADM_ResourceDetails = new List<DOADM_ResourceDetails>();
                    //temporary object for search resources function
                    DOADM_ResourceDetails objDOADM_ResourceDetails_Find = new DOADM_ResourceDetails();
                    objDOADM_ResourceDetails_Find.ADM_ResourceDetailsId = ResourceId;
                    objDOADM_ResourceDetails_Find.IsActive = isActive;
                    result =  _objBLResources.SearchResources(TimeZone,objDOADM_ResourceDetails_Find, out lstDOADM_ResourceDetails, out errorMessage);
                    //check result for DB action
                    if (result != (long)ExceptionTypes.Success)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    }
                    if (lstDOADM_ResourceDetails.Count > 0)
                        objDOADM_ResourceDetails = lstDOADM_ResourceDetails.FirstOrDefault();                           
                }
                //Alert Time Zone lookup from Cache
                objDOADM_ResourceDetails.lstTimeZone = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone);
                objDOADM_ResourceDetails.lstCMN_Department = lstCMN_Department;
                return View(objDOADM_ResourceDetails);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Add(DOADM_ResourceDetails objDOADM_ResourceDetails)
        {
            string errorMessage = string.Empty;
            long loginUserId = currentUser.ADM_UserMasterId;
            string returnMessage = "";
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                //check if this call is to update record or save new record
                if (objDOADM_ResourceDetails.ADM_ResourceDetailsId > 0)
                {
                    //if old record is to be updated
                    BLCommon objCommon = new BLCommon();
                    //check if record is locked by current user
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Resources, objDOADM_ResourceDetails.ADM_ResourceDetailsId))
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
                objDOADM_ResourceDetails.LoginUserId = loginUserId;
                result = _objBLResources.SaveResource(objDOADM_ResourceDetails, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    //if not success
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB." });
            }
        }
        private List<DOADM_ResourceDetails> PGetResource(string strName = "", string strDescription = "", string strLinkLocation = "", bool bolIsActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            //temporary object for search resource function
            DOADM_ResourceDetails objDOADM_ResourceDetails = new DOADM_ResourceDetails();
            objDOADM_ResourceDetails.IsActive = bolIsActive;
            objDOADM_ResourceDetails.ResourceName = strName;
            objDOADM_ResourceDetails.ResourceDescription = strDescription;
            objDOADM_ResourceDetails.ResourceLinkLocation = strLinkLocation;
            objDOADM_ResourceDetails.ConsiderDates = false;
            List<DOADM_ResourceDetails> lstDOADM_ResourceDetails;
            string errorMessage = string.Empty;
            ExceptionTypes result = _objBLResources.SearchResources(TimeZone,objDOADM_ResourceDetails, out lstDOADM_ResourceDetails, out errorMessage);
            //check result for DB action
            if (result != (long)ExceptionTypes.Success)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Resources, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
            }
            else if (result == ExceptionTypes.ZeroRecords)
            {
                lstDOADM_ResourceDetails = new List<DOADM_ResourceDetails>();
            }
            return lstDOADM_ResourceDetails;

        }

        [HttpPost]
        public ActionResult GetResourceReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            _objBLReports = new BLReports();
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.ResourceHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

    }
}