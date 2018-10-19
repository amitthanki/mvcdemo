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
    public class LookupCorrelationController : Controller
    {
        BLLookupCorrelations _objBLLookupCorrelations = new BLLookupCorrelations();
        BLReports _objBLReports = new BLReports();
        string errorMessage = string.Empty;
        private UIUserLogin currentUser;
        public LookupCorrelationController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        /// <summary>
        /// Initial page load for Lookup Corrrelation Search page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search()
        {
            try
            {
                var tplSearch = new Tuple<List<DOCMN_LookupType>, List<DOCMN_LookupTypeCorrelations>>(PGetAllLookupTypeList(), PGetAllLookupTypeCorrelations());
                return View(tplSearch);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        /// <summary>
        /// Ajax Call To fill the search result based on Parameters
        /// </summary>
        /// <param name="strDescription"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult GetSearchResult(string strDescription = "", int ddlLookupType=0, bool isActive = true)
        {
            try
            {
                return PartialView("_SearchResults", PGetAllLookupTypeCorrelations(strDescription, ddlLookupType, isActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                //need log
                return Json(new { ID = 1, Message = "An error occured while validating" });
            }

        }
        /// <summary>
        /// View Add/Modify Corelation Type
        /// </summary>
        /// <returns></returns>
        public ActionResult Add(long lookupTypeCorrelationsId=0,int flag=0)
       {
            try
            {
                if(lookupTypeCorrelationsId==0 && flag ==0)
                    return RedirectToAction("Search", "LookupCorrelation");

                TempData["PageVisibility"] = flag;
                return View(PGetLookupCorelation(lookupTypeCorrelationsId));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }  
        }
        /// <summary>
        /// Load Search result as partial view
        /// </summary>
        /// <param name="strDescription"></param>
        /// <param name="ddlLookupType"></param>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public ActionResult GetLkupCoRelationSearchResult(string strDescription = "", int ddlLookupType = 0, bool isActive = true)
        {
            try
            {
                return PartialView("_SearchResults", PGetAllLookupTypeCorrelations(strDescription, ddlLookupType, isActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }
        }
        public ActionResult GetCorelationMaster(long lkupCorelationTypeID,long lkupCorelationMasterID= 0)
        {
            try
            {
                return PartialView("_LookupMasterCorrelationrPopup", PGetCorrelationMasterByID(lkupCorelationTypeID, lkupCorelationMasterID));

            }
            catch (Exception ex)
            {

                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return PartialView("");
            }
        }
        /// <summary>
        /// Save / Update LookupTypeCrrelation 
        /// </summary>
        /// <param name="objDOCMN_LookupTypeCorrelations"></param>
        /// <returns></returns>
        public ActionResult SaveLookupTypeCorrelation(DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            long loginUserId = currentUser.ADM_UserMasterId;
            string returnMessage = string.Empty;
            BLCommon objCommon = new BLCommon();    
            try
            {
                if (objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId > 0)
                {

                    if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.LookupTypeCorrelation, objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }

                    objDOCMN_LookupTypeCorrelations.LastUpdatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    objDOCMN_LookupTypeCorrelations.CreatedByRef = loginUserId;
                    returnMessage = "Record saved successfully.";
                }

                result=_objBLLookupCorrelations.SaveLookupTypeCorrelation(objDOCMN_LookupTypeCorrelations, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured." });
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = result, Message = "An error occured." });
            }

        }
        public ActionResult SaveCorrelationMaster(DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            long loginUserId = currentUser.ADM_UserMasterId;
            string returnMessage = string.Empty;
            try
            {
                if (objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterCorrelationsId > 0)
                {
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.LastUpdatedByRef = loginUserId;
                    returnMessage = "Record updated successfully.";
                }
                else
                {
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CreatedByRef = loginUserId;
                    returnMessage = "Record saved successfully.";
                }

                result = _objBLLookupCorrelations.SaveCorrelationMaster(objDOCMN_LookupMasterCorrelationsExtended, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured." });
                }
                return Json(new { ID = result, Message = returnMessage});
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = result, Message = "An error occured." });
            }

        }
        public ActionResult ValidateLookupTypeCorrelation(long lookupCorelationTypeID=0,string groupName="",long parentID=0,long childID=0)
        {
            try
            {
                List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations = new List<DOCMN_LookupTypeCorrelations>();
                lstDOCMN_LookupTypeCorrelations = PGetAllLookupTypeCorrelations();
                if (lstDOCMN_LookupTypeCorrelations.Where(xx => xx.CorrelationDescription == groupName && xx.CMN_LookupTypeCorrelationsId != lookupCorelationTypeID).Count() > 0)
                {
                    return Json(new { ID = 1, Message = "Group name already exists." });
                }
                else if (lstDOCMN_LookupTypeCorrelations.Where(xx=>xx.CMN_LookupTypeParentRef == parentID && xx.CMN_LookupTypeChildRef == childID && xx.CMN_LookupTypeCorrelationsId != lookupCorelationTypeID).Count() > 0)
                {
                    return Json(new { ID = 1, Message = "Parent & child corelation already exists." });
                }
                return Json(new { ID = 0, Message = "" });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while validating" });
            }


        }
        public ActionResult ValidateLookupMasterCorrelation(long lookupCorelationTypeID, long lookupCorelationMasterID = 0, string groupName = "", long parentID = 0, long childID = 0)
        {
            try
            {
                List<DOCMN_LookupMasterCorrelations> lstDOCMNLookupMasterCorrelations = new List<DOCMN_LookupMasterCorrelations>();
                lstDOCMNLookupMasterCorrelations = PGetLookupCorelation(lookupCorelationTypeID).lstDOCMN_LookupMasterCorrelations;
                if (lstDOCMNLookupMasterCorrelations.Where(xx => xx.CorrelationDescription == groupName && xx.CMN_LookupMasterCorrelationsId != lookupCorelationMasterID).Count() > 0)
                {
                    return Json(new { ID = 1, Message = "Correlation Group name already exists." });
                }
                else if (lstDOCMNLookupMasterCorrelations.Where(xx => xx.CMN_LookupMasterParentRef == parentID && xx.CMN_LookupMasterChildRef == childID && xx.CMN_LookupMasterCorrelationsId != lookupCorelationMasterID).Count() > 0)
                {
                    return Json(new { ID = 1, Message = "Parent & child correlation master already exists." });
                }
                return Json(new { ID = 0, Message = "" });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 1, Message = "An error occured while validating" });
            }


        }

        private List<DOCMN_LookupType> PGetAllLookupTypeList()
        {
            List<DOCMN_LookupType> lstDOCMN_LookupType = new List<DOCMN_LookupType>();
            try
            {
                var lstLookupType = CacheUtility.GetAllLookuptypeOrById().OrderBy(x => x.LookupTypeDescription); ;
                foreach (var item in lstLookupType)
                {
                    lstDOCMN_LookupType.Add(new DOCMN_LookupType
                    {
                        CMN_LookupTypeId= item.CMN_LookupTypeId,
                        LookupTypeDescription = item.LookupTypeDescription.ToString()
                    });
                }
                return lstDOCMN_LookupType;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }

        }
        private List<DOCMN_LookupTypeCorrelations> PGetAllLookupTypeCorrelations(string strDescription="", int ddlLookupType=0, bool isActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations = new List<DOCMN_LookupTypeCorrelations>();
            DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
            string errorMessage = string.Empty;

            try
            {
                objDOCMN_LookupTypeCorrelations.CorrelationDescription = strDescription;
                objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef = ddlLookupType;
                objDOCMN_LookupTypeCorrelations.IsActive = isActive;
                ExceptionTypes result = _objBLLookupCorrelations.GetAllLookupTypeCorrelations(TimeZone,objDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupTypeCorrelations,out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                return lstDOCMN_LookupTypeCorrelations;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }
        private DOCMN_LookupTypeCorrelations PGetLookupCorelation(long lookupTypeCorrelationsId)
        {
            DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
            string errorMessage = string.Empty;
            try
            {
                ExceptionTypes result = _objBLLookupCorrelations.GetLookupCorelationByID(lookupTypeCorrelationsId, out objDOCMN_LookupTypeCorrelations , out errorMessage);
                objDOCMN_LookupTypeCorrelations.lstDOCMN_LookupType = PGetAllLookupTypeList();
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                return objDOCMN_LookupTypeCorrelations;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;

            }
        }
        private DOCMN_LookupMasterCorrelationsExtended PGetCorrelationMasterByID(long lkupCorelationTypeID,long lkupCorelationMasterID)
        {
            DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended = new DOCMN_LookupMasterCorrelationsExtended();
            string errorMessage = string.Empty;
            try
            {

                ExceptionTypes result = _objBLLookupCorrelations.GetCorrelationMasterByID(lkupCorelationTypeID,lkupCorelationMasterID, out objDOCMN_LookupMasterCorrelationsExtended,out errorMessage);
                objDOCMN_LookupMasterCorrelationsExtended.lstDOCMN_LookupMasterParent = CacheUtility.GetAllLookupsFromCache(objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef);
                objDOCMN_LookupMasterCorrelationsExtended.lstDOCMN_LookupMasterChild = CacheUtility.GetAllLookupsFromCache(objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeChildRef);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }


                return objDOCMN_LookupMasterCorrelationsExtended;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.LookupsCorrelation, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult GetLookupsCorrelationReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.LookupCorrelationHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

    }
}