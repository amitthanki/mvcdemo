using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Reflection;
using ENRLReconSystem.Models;
namespace ENRLReconSystem.Controllers
{
    public class ConfigurationsController : Controller
    {

        private UIUserLogin currentUser;
        public ConfigurationsController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        private BLReports _objBLReports;
        //public UIUserLogin objUIUserLogin
        //{
        //    get
        //    {
        //        return !Session[ConstantTexts.CurrentUserSessionKey].IsNull() ? Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin : new UIUserLogin();
        //    }
        //}
        // GET: Configurations
        [HttpGet]
        public ActionResult Search()
        {
           
            return View(PGetConfiguration());
        }
     
        public ActionResult Search(string strConfigName ="",bool isActive = true)
        {           
            try
            {               
                return PartialView("_SearchResults", PGetConfiguration(strConfigName,isActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }          
        }
        [HttpGet]
        public ActionResult Add(long ConfigId = 0, bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;          
            // Bind Dropdown List.
            string errorMessage = string.Empty;
       
            List<DOCMN_LookupMaster> lstTimezone;       
            lstTimezone = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone); 
            BLConfigurations objBLConfigurations = new BLConfigurations();
            List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
            DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
            objDOMGR_ConfigMaster.IsActive = true;
            if (ConfigId != 0)
            {
                // Fetch Record By Configuration ID.                       
                objDOMGR_ConfigMaster.MGR_ConfigMasterId = ConfigId;
                objDOMGR_ConfigMaster.IsActive = isActive;
                ExceptionTypes result = objBLConfigurations.SearchConfigId(TimeZone, objDOMGR_ConfigMaster, out lstDOMGR_ConfigMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log Error
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                }
                if (lstDOMGR_ConfigMaster.Count > 0)
                {
                    objDOMGR_ConfigMaster = lstDOMGR_ConfigMaster.FirstOrDefault();                   
                }
                
            }
            else
            {
                objDOMGR_ConfigMaster.StartDate = DateTime.UtcNow;
            }
            objDOMGR_ConfigMaster.lstTimeZone = lstTimezone;
            return View("Add", objDOMGR_ConfigMaster);
        }

        // Check Duplicate Department and Business Segment.
        [HttpPost]
        public JsonResult CheckDuplicateConfigName(string ConfigName)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            var data = "";
            BLConfigurations objBLConfigurations = new BLConfigurations();
            List<DOMGR_ConfigMaster> lstDDOMGR_ConfigMaster;

            DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
            objDOMGR_ConfigMaster.ConfigName = ConfigName;
            objDOMGR_ConfigMaster.IsActive = true;

            string errorMessage = string.Empty;
            ExceptionTypes result = objBLConfigurations.SearchConfiguration(TimeZone,objDOMGR_ConfigMaster, out lstDDOMGR_ConfigMaster, out errorMessage);

            if (result != (long)ExceptionTypes.Success)
            {
                //Log error
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
            }
            else if (result == ExceptionTypes.ZeroRecords)
            {
                lstDDOMGR_ConfigMaster.Add(objDOMGR_ConfigMaster);
                lstDDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
            }

            int i = lstDDOMGR_ConfigMaster.Count;
            if (i > 0)
            {
                data = "Configuration Name already exists.";
            }
            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Add(DOMGR_ConfigMaster ConfigMaster)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                long loginUserId = currentUser.ADM_UserMasterId;
                BLConfigurations objBLConfigurations = new BLConfigurations();
                DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
                objDOMGR_ConfigMaster.MGR_ConfigMasterId = ConfigMaster.MGR_ConfigMasterId;
                if (objDOMGR_ConfigMaster.MGR_ConfigMasterId > 0) // Edit Mode
                {
                    BLCommon objCommon = new BLCommon();
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Configuration, objDOMGR_ConfigMaster.MGR_ConfigMasterId))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }                                   
                    objDOMGR_ConfigMaster.CreatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";                   
                        
                }
                else
                {
                    objDOMGR_ConfigMaster.CreatedByRef = loginUserId;  //Add Mode
                    returnMessage = "Record saved successfully.";
                }                          
                    objDOMGR_ConfigMaster.ConfigName = ConfigMaster.ConfigName;
                    objDOMGR_ConfigMaster.ConfigValue = ConfigMaster.ConfigValue;
                    objDOMGR_ConfigMaster.StartDate = ConfigMaster.StartDate;
                    objDOMGR_ConfigMaster.EndDate = ConfigMaster.EndDate;
                    objDOMGR_ConfigMaster.IsActive = ConfigMaster.IsActive;
                    result = objBLConfigurations.SaveConfigMaster(objDOMGR_ConfigMaster, out errorMessage);
            
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }

                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while performing Save action." });
            }

        }
        public ActionResult Edit()
        {
            return View();
        }
       internal List<DOMGR_ConfigMaster> PGetConfiguration(string strConfigName = "", bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            BLConfigurations objBLConfigurations = new BLConfigurations();
            List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster;
            try
            {
                DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
                objDOMGR_ConfigMaster.ConfigName = strConfigName;
                if (isActive)
                {
                    objDOMGR_ConfigMaster.IsActive = Convert.ToBoolean(isActive);
                }
                else
                {
                    objDOMGR_ConfigMaster.IsActive = false;
                }
                string errorMessage = string.Empty;
                ExceptionTypes result = objBLConfigurations.SearchConfiguration(TimeZone,objDOMGR_ConfigMaster, out lstDOMGR_ConfigMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log error
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, errorMessage.ToString(), errorMessage.ToString());
                }
                else if (result == ExceptionTypes.ZeroRecords)
                {
                    lstDOMGR_ConfigMaster.Add(objDOMGR_ConfigMaster);
                    lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
                }
                return lstDOMGR_ConfigMaster;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Configurations, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GetConfigReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            _objBLReports = new BLReports();
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.ConfigurationHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }
    }
}