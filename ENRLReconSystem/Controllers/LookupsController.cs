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
    [ERSAuthenticationAttribute(Roles = "User")]
    public class LookupsController : Controller
    {
        BLLookup _objBLLookup = new BLLookup();
        string errorMessage = string.Empty;
        private BLReports _objBLReports = new BLReports();
        private UIUserLogin currentUser;
        public LookupsController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }

        [HttpGet]
        public ActionResult Search()
        {
            try
            {
                return View(PGetSearchResult());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                ViewBag.ErrorMessage = ex.ToString();
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        [ValidateInput(false)]
        public ActionResult GetSearchResult(string strDescription = "", bool isActive = true)
        {
            try
            {
                return PartialView("_SearchResults", PGetSearchResult(strDescription, isActive));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while updating DB." });
            }

        }
        public ActionResult GetLookupMaster(long? lookupMasterID = 0, long lookupTypeID = 0)
        {
            try
            {
                return PartialView("_LookupMasterPopup", PGetLookupMaster(lookupMasterID, lookupTypeID));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty,ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while updating DB." });
            }
        }
        public ActionResult Add(long? lookupTypeId = 0, int flag = 0)
        {
            try
            {
                if (lookupTypeId == 0 && flag == 0)//While Accessing directly from URL//
                    return RedirectToAction("Search", "Lookups");
                TempData["PageVisibility"] = flag;
                return View(PGetLookupMasterByLkupTypeID(lookupTypeId));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        /// <summary>
        /// Save / Modify lookup Type
        /// </summary>
        /// <param name="objDOCMN_LookupType"></param>
        /// <returns></returns>
        public ActionResult SaveLookupType(DOCMN_LookupType objDOCMN_LookupType)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                long loginUserId = currentUser.ADM_UserMasterId;
                string returnMessage = string.Empty;

                if (objDOCMN_LookupType.CMN_LookupTypeId > 0)  //Edit Mode
                {
                    BLCommon objCommon = new BLCommon();
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.LookupType, objDOCMN_LookupType.CMN_LookupTypeId))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }

                    objDOCMN_LookupType.LastUpdatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    objDOCMN_LookupType.CreatedByRef = loginUserId;  //Add Mode
                    returnMessage = "Record saved successfully.";
                }

                result = _objBLLookup.SaveLookupType(objDOCMN_LookupType, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB." });
            }

        }
        public ActionResult SaveLookupMaster(DOCMN_LookupMaster objDOCMN_LookupMaster)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                long loginUserId = currentUser.ADM_UserMasterId;
                string returnMessage = string.Empty;

                if (objDOCMN_LookupMaster.CMN_LookupMasterId > 0)
                {
                    BLCommon objCommon = new BLCommon();
                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.LookupType, objDOCMN_LookupMaster.CMN_LookupTypeRef))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }

                    objDOCMN_LookupMaster.LastUpdatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    objDOCMN_LookupMaster.CreatedByRef = loginUserId;
                    returnMessage = "Record saved successfully.";
                }

                result = _objBLLookup.SaveLookupMaster(objDOCMN_LookupMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                return Json(new { ID = result, Message = returnMessage });

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB." });
            }
        }
        /// <summary>
        /// Validate LookupType description exists or not
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateLookupType(long lookupTypeId=0,string lookupTypeDesc="")
        {
            try
            {
                CacheUtility.ClearCache(ConstantTexts.LookupTypeCacheKey);
                if (CacheUtility.GetAllLookuptypeOrById().Where(xx => xx.LookupTypeDescription.ToUpper() == lookupTypeDesc.ToUpper() && xx.CMN_LookupTypeId!= lookupTypeId).Count() > 0)
                {
                    return Json(new { ID = 1, Message = "Lookup Description already exists." });
                }
                return Json(new { ID = 0, Message = "" });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while validating" });
            }

        }
        public ActionResult ValidateLookupMaster(long lookupTypeId, long lookupMasterID = 0, string lookupMasterValue = "", int priority=0)
        {
            try
            {
                CacheUtility.ClearCache(ConstantTexts.LookupMasterCacheKey);
                List<DOCMN_LookupMaster> lstDOCMN_LookupMaster = new List<DOCMN_LookupMaster>();
                lstDOCMN_LookupMaster = CacheUtility.GetAllLookupsFromCache(lookupTypeId);


                if(lstDOCMN_LookupMaster.Where(xx => xx.LookupValue.ToUpper()== lookupMasterValue.ToUpper() && xx.CMN_LookupMasterId!= lookupMasterID).Count()>0)
                {
                    return Json(new { ID = 1, Message = "Lookup value already exists." });
                }

                else if (lstDOCMN_LookupMaster.Where(xx => xx.DisplayOrder==priority && xx.CMN_LookupMasterId != lookupMasterID).Count()>0)
                {
                    return Json(new { ID = 1, Message = "Priority value already exists." });
                }

                return Json(new { ID = 0, Message = "" });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while validating" });
            }

        }

        private List<DOCMN_LookupType> PGetSearchResult(string strDescription = "", bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOCMN_LookupType> lstDOCMN_LookupType = new List<DOCMN_LookupType>();
            try
            {
                ExceptionTypes result = _objBLLookup.GetAllLookupTypes(TimeZone,strDescription, isActive, out lstDOCMN_LookupType);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log Error
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty,"Something Went Wrong while Searching");
                }
                return lstDOCMN_LookupType;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        private DOCMN_LookupType PGetLookupMasterByLkupTypeID(long? lookupTypeId)
        {
            DOCMN_LookupType objDOCMN_LookupType = new DOCMN_LookupType();
            try
            {
                ExceptionTypes result = _objBLLookup.GetLookupMasterByLkupTypeID(lookupTypeId, out objDOCMN_LookupType);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log Error
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Lookups, (long)ExceptionTypes.Uncategorized, string.Empty, "Something Went Wrong in PGetLookupMasterByLkupTypeID");

                }
                return objDOCMN_LookupType;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        private DOCMN_LookupMaster PGetLookupMaster(long? lookupMasterID, long lookupTypeID)
        {
            DOCMN_LookupMaster objDOCMN_LookupMaster = new DOCMN_LookupMaster();
            try
            {
                if (lookupMasterID > 0)
                {
                    CacheUtility.ClearCache(ConstantTexts.LookupMasterCacheKey);
                    objDOCMN_LookupMaster = CacheUtility.GetLookupById(lookupMasterID);
                }
                objDOCMN_LookupMaster.CMN_LookupTypeRef = lookupTypeID;
                return objDOCMN_LookupMaster;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GetLookupsReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.LookupHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

    }
}