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
    [ERSRoleAuthorize(ERSAuthenticationRoles.AdmOSTUser, ERSAuthenticationRoles.AdmEligUser, ERSAuthenticationRoles.AdmRPRUser)]
    public class SkillsController : Controller
    {
        private UIUserLogin currentUser;
        BLReports _objBLReports = new BLReports();
        BLSkills _objBLSkills = new BLSkills();

        public SkillsController()
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
                string strErrorMessage = string.Empty;
                BLSkills _objBLSkills = new BLSkills();
                DOADM_SkillMasterExtended objDOADM_SkillMasterExtended = new DOADM_SkillMasterExtended();
                objDOADM_SkillMasterExtended.lstRoles = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Role);
                objDOADM_SkillMasterExtended.lstBusinessSegment = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.BusinessSegment);
                objDOADM_SkillMasterExtended.lstDepartment = GetERSDepartments();
                objDOADM_SkillMasterExtended.lstWorkBasket = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.WorkBasket);
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                objDOADM_SkillsMaster.IsActive = true;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                ExceptionTypes result = _objBLSkills.SearchSkills(TimeZone, objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out strErrorMessage);
                objDOADM_SkillMasterExtended.lstSkillsMaster = lstDOADM_SkillsMaster;
                return View(objDOADM_SkillMasterExtended);
            }
            catch(Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
           
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Search(string SkillsName = "", long RoleLkup = 0, long BusinessSegmentLkup = 0, long CMN_DepartmentRef = 0, long WorkBasketLkup = 0, bool isActive = true)
        {
            try
            {
                BLSkills _objBLSkills = new BLSkills();
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster;
                string errorMessage = string.Empty;

                objDOADM_SkillsMaster.IsActive = isActive;
                objDOADM_SkillsMaster.SkillsName = SkillsName;
                objDOADM_SkillsMaster.RoleLkup = RoleLkup;
                objDOADM_SkillsMaster.BusinessSegmentLkup = BusinessSegmentLkup;
                objDOADM_SkillsMaster.CMN_DepartmentRef = CMN_DepartmentRef;
                objDOADM_SkillsMaster.WorkBasketLkup = WorkBasketLkup;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                ExceptionTypes result = _objBLSkills.SearchSkills(TimeZone ,objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized,"error while retriving search results", "error while retriving search results");
                }
                else if (result == ExceptionTypes.ZeroRecords)
                {
                    lstDOADM_SkillsMaster.Add(objDOADM_SkillsMaster);
                    lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                }

                DOADM_SkillMasterExtended objDOADM_SkillMasterExtended = new DOADM_SkillMasterExtended();
                objDOADM_SkillMasterExtended.lstRoles = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Role);
                objDOADM_SkillMasterExtended.lstBusinessSegment = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.BusinessSegment);
                objDOADM_SkillMasterExtended.lstDepartment = GetERSDepartments();
                objDOADM_SkillMasterExtended.lstWorkBasket = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.WorkBasket);
                objDOADM_SkillMasterExtended.lstSkillsMaster = lstDOADM_SkillsMaster;
                return PartialView("_SearchResults", objDOADM_SkillMasterExtended);

            }
            catch(Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
            
        }

        [HttpGet]
        public ActionResult Add(long lSkillsID = 0, bool isActive = true)
        {
            string errorMessage = string.Empty;
            try
            {
                Session[ConstantTexts.SkillWorkQueueSessionKey] = null;
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                objDOADM_SkillsMaster.IsActive = true;
                ViewBag.Roles = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Role);
                ViewBag.BusinessSegment = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.BusinessSegment);
                ViewBag.WorkBasket = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.WorkBasket);
                ViewBag.lstDiscrepancyCategories = GetDiscrepancyCategories();
                ViewBag.lstDepartments = GetERSDepartments();

                if (lSkillsID != 0)
                {
                    GetSkillForEdit(lSkillsID, isActive, out objDOADM_SkillsMaster);
                    List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMaster = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, objDOADM_SkillsMaster.DiscrepancyCategoryLkup);
                    lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterParentRef == objDOADM_SkillsMaster.DiscrepancyCategoryLkup).ToList();
                    ViewBag.WorkQueues = lstDOCMN_LookupMaster;
                    Session[ConstantTexts.SkillWorkQueueSessionKey] = objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation;
                    objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation = objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation.Where(x => x.IsActive == true).ToList();
                }
                return View(objDOADM_SkillsMaster);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }

        }

        public List<DOCMN_Department> GetERSDepartments()
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string strErrorMessage = string.Empty;
            List<DOCMN_Department> lstCMN_Department = new List<DOCMN_Department>();
            BLDepartment objBLDepartment = new BLDepartment();
            DOCMN_Department objDOCMN_Department = new DOCMN_Department();
            objDOCMN_Department.IsActive = true;
            ExceptionTypes result = objBLDepartment.SearchDepartment(TimeZone,objDOCMN_Department, out lstCMN_Department, out strErrorMessage);
            return lstCMN_Department;

        }
       
        [HttpPost]
        public ActionResult Add(DOADM_SkillsMaster objDOADM_SkillsMaster)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                string sreturnMessage = string.Empty;
                if (objDOADM_SkillsMaster.ADM_SkillsMasterId > 0)
                {
                    BLCommon objCommon = new BLCommon();
                    if (!objCommon.ValidateLockBeforeSave(currentUser.ADM_UserMasterId, (long)ScreenType.Skills, objDOADM_SkillsMaster.ADM_SkillsMasterId))
                    {
                        errorMessage = "Record not locked, please reload the page.";
                        result = ExceptionTypes.UnknownError;
                        return Json(new { ID = result, Message = errorMessage });
                    }

                    objDOADM_SkillsMaster.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                    sreturnMessage = "Record updated successfully.";

                }
                else
                {
                    objDOADM_SkillsMaster.CreatedByRef = currentUser.ADM_UserMasterId;
                    sreturnMessage = "Record saved successfully.";
                }
                BLSkills _objBLSkills = new BLSkills();
                if (Session[ConstantTexts.SkillWorkQueueSessionKey] != null)
                    objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation = Session[ConstantTexts.SkillWorkQueueSessionKey] as
                                                                                 List<DOADM_SkillWorkQueuesCorrelation>;
                result = _objBLSkills.SaveSkills(objDOADM_SkillsMaster,currentUser.ADM_UserMasterId ,out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long) ErrorModuleName.ManageSkills,(long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                Session[ConstantTexts.SkillWorkQueueSessionKey] = null;
                return Json(new { ID = result, Message = sreturnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB." });
            }
        }

        [HttpPost]
        public ActionResult GetQueues(long? lDiscrepancyCatLkup)
        {
            try
            {
                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMaster = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, lDiscrepancyCatLkup);
                lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterParentRef == lDiscrepancyCatLkup).ToList();
                return Json(lstDOCMN_LookupMaster);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error",routeValues:ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddWorkQueuesToSession(DOADM_SkillWorkQueuesCorrelation objDOADM_SkillWorkQueuesCorrelation,long lDiscrepancyCatLkup = 0 , long lPreviousWorkQueue = 0, int mode = 0)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOADM_SkillWorkQueuesCorrelation> lstDOADM_SkillWorkQueuesCorrelation = new List<DOADM_SkillWorkQueuesCorrelation>();
            try
            {
                if (Session[ConstantTexts.SkillWorkQueueSessionKey] != null)
                    lstDOADM_SkillWorkQueuesCorrelation = Session[ConstantTexts.SkillWorkQueueSessionKey] as List<DOADM_SkillWorkQueuesCorrelation>;
                switch (mode)
                {
                    case 0://Add new WQ to the list
                        if (lstDOADM_SkillWorkQueuesCorrelation.Where(xx => xx.WorkQueuesLkup == objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup).Count() == 0)
                        {
                            objDOADM_SkillWorkQueuesCorrelation.IsActive = true;
                            objDOADM_SkillWorkQueuesCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_SkillWorkQueuesCorrelation.CreatedByName = currentUser.FullName;
                            objDOADM_SkillWorkQueuesCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByName = currentUser.FullName;
                            objDOADM_SkillWorkQueuesCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            lstDOADM_SkillWorkQueuesCorrelation.Add(objDOADM_SkillWorkQueuesCorrelation);
                        }
                        else
                        {
                            objDOADM_SkillWorkQueuesCorrelation = lstDOADM_SkillWorkQueuesCorrelation.Where(xx => xx.WorkQueuesLkup == objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup).FirstOrDefault();
                            objDOADM_SkillWorkQueuesCorrelation.IsActive = true;
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByName = currentUser.FullName;
                            objDOADM_SkillWorkQueuesCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                        }
                        break;
                    case 1://delete work queue from added list
                        if (lstDOADM_SkillWorkQueuesCorrelation.Where(xx => xx.WorkQueuesLkup == objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup).Count() > 0)
                        {
                            objDOADM_SkillWorkQueuesCorrelation = lstDOADM_SkillWorkQueuesCorrelation.Where(xx => xx.WorkQueuesLkup == objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup).FirstOrDefault();
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByName = currentUser.FullName;
                            objDOADM_SkillWorkQueuesCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_SkillWorkQueuesCorrelation.IsActive = false;
                        }
                        break;
                    case 2://update existing work Queue
                        long NewWorkQueuesLkup = objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup;
                        objDOADM_SkillWorkQueuesCorrelation = lstDOADM_SkillWorkQueuesCorrelation.Where(x => x.WorkQueuesLkup == lPreviousWorkQueue).FirstOrDefault();
                        if(objDOADM_SkillWorkQueuesCorrelation.ADM_SkillsMasterRef == 0)//updating a queue for new record
                        {
                            objDOADM_SkillWorkQueuesCorrelation.CreatedByName = currentUser.FullName;
                            objDOADM_SkillWorkQueuesCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                        }
                        objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByName = currentUser.FullName;
                        objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                        objDOADM_SkillWorkQueuesCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                        objDOADM_SkillWorkQueuesCorrelation.IsActive = true;
                        objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup = NewWorkQueuesLkup;
                        break;
                    case 3://removing all queues from session
                        foreach(var item in lstDOADM_SkillWorkQueuesCorrelation)
                        {
                            item.LastUpdatedByName = currentUser.FullName;
                            item.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            item.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            item.IsActive = false;
                        }
                        break;
                }

                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMaster = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, lDiscrepancyCatLkup);
                lstDOCMN_LookupMaster = lstDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterParentRef == lDiscrepancyCatLkup).ToList();
                ViewBag.WorkQueues = lstDOCMN_LookupMaster;

                Session[ConstantTexts.SkillWorkQueueSessionKey] = lstDOADM_SkillWorkQueuesCorrelation;
                return PartialView("_WorkQueue", lstDOADM_SkillWorkQueuesCorrelation.Where(xx => xx.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
        }

        private List<DOCMN_LookupMasterCorrelations> GetDiscrepancyCategories()
        {
            try
            {
                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations = CacheUtility.GetAllLookupMasterCorrelationFromCache(
                   (long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, null);
                return lstDOCMN_LookupMasterCorrelations;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return null;
            }
           
        }
              
        public ActionResult CheckSkillExistsUsingSkillName(string skillsName)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string strErrorMessage = string.Empty;
            string strMessage = string.Empty;
            bool skillExists = false;
            try
            {
                BLSkills _objBLSkills = new BLSkills();
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                objDOADM_SkillsMaster.SkillsName = skillsName;
                objDOADM_SkillsMaster.IsActive = true;
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster;

                ExceptionTypes result = _objBLSkills.SearchSkills(TimeZone, objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out strErrorMessage);
                if (lstDOADM_SkillsMaster.Count > 0)
                    skillExists = true;
                else
                {
                    objDOADM_SkillsMaster.IsActive = false;//search again to check for not active records
                    result = _objBLSkills.SearchSkills(TimeZone, objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out strErrorMessage);
                    if (lstDOADM_SkillsMaster.Count > 0)
                        skillExists = true;
                    else
                        skillExists = false;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
            return Json(new { skillExists = skillExists, strMessage = strMessage }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult GetSkillsReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.SkillsHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

        private void GetSkillForEdit(long lSkillsID,bool isActive ,out DOADM_SkillsMaster objDOADM_SkillsMaster)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string errorMessage = string.Empty;
            ExceptionTypes result;
            List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
            objDOADM_SkillsMaster = new DOADM_SkillsMaster();
            try
            {
                objDOADM_SkillsMaster.ADM_SkillsMasterId = lSkillsID;
                objDOADM_SkillsMaster.IsActive = isActive;
                result = _objBLSkills.SearchSkills(TimeZone, objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, "error occured while retrving skill data", "error occured while retrving skill data");
                }
                if (lstDOADM_SkillsMaster.Count > 0)
                    objDOADM_SkillsMaster = lstDOADM_SkillsMaster.FirstOrDefault();
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageSkills, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
            }
        }

    }
}