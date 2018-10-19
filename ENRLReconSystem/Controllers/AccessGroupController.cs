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
    public class AccessGroupController : Controller
    {
        private UIUserLogin currentUser;
        private BLReports _objBLReports;
        public AccessGroupController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }

        [ValidateInput(false)]
        [HttpGet]
        public ActionResult Search()
        {
            try
            {
                return View(GetAccessGroupSearchResult());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Search(DOADM_AccessGroupMaster objDOADM_AccessGroupMaster)
        {
            try
            {
                return PartialView("_SearchResults", GetAccessGroupSearchResult(objDOADM_AccessGroupMaster.AccessGroupName, objDOADM_AccessGroupMaster.AccessGroupDescription,objDOADM_AccessGroupMaster.IsActive));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        private List<DOADM_AccessGroupMaster> GetAccessGroupSearchResult(string AccessGroupName = "", string AccessGroupDescription = "", bool IsActive = true)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            BLAccessGroup objBLAccessGroup = new BLAccessGroup();
            DOADM_AccessGroupMaster objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
            objDOADM_AccessGroupMaster.AccessGroupName = AccessGroupName;
            objDOADM_AccessGroupMaster.AccessGroupDescription = AccessGroupDescription;
            objDOADM_AccessGroupMaster.IsActive = IsActive;

            List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster = new List<DOADM_AccessGroupMaster>();
            try
            {
                ExceptionTypes result = objBLAccessGroup.GetAccessGroupBasedOnSearch(TimeZone,objDOADM_AccessGroupMaster, out lstDOADM_AccessGroupMaster);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, "error while retriving search results");
                }
                return lstDOADM_AccessGroupMaster;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return null;
            }
        }

        [HttpGet]
        public ActionResult Add(long ADM_AccessGroupMasterId = 0, bool isActive = true)
        {
            try
            {
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                Session[ConstantTexts.AccessGroupSkillSessionKey] = null;
                Session[ConstantTexts.AccessGroupReportSessionKey] = null;
                ViewBag.RoleList = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Role);
                ViewBag.WorkBasketList = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.WorkBasket);
                ViewBag.Reports = ViewBag.Reports = CacheUtility.GetAllReportsFromCache(null, "");
                UIDOAccessGroup objUIDOAccessGroup = new UIDOAccessGroup();
                objUIDOAccessGroup.IsActive = true;
                objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation = new List<DOADM_AccessGroupSkillsCorrelation>();
                objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation = new List<DOADM_AccessGroupReportCorrelation>();

                if (ADM_AccessGroupMasterId != 0)
                {
                    DOADM_AccessGroupMaster objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
                    BLAccessGroup objBLAccessGroup = new BLAccessGroup();

                    objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId = ADM_AccessGroupMasterId;
                    objDOADM_AccessGroupMaster.IsActive = isActive;
                    ExceptionTypes result = objBLAccessGroup.GetAccessGroupForEdit(objDOADM_AccessGroupMaster, out objUIDOAccessGroup);
                    Session[ConstantTexts.AccessGroupSkillSessionKey] = objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation;
                    Session[ConstantTexts.AccessGroupReportSessionKey] = objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation;

                    DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                    List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                    BLSkills objBLSkills = new BLSkills();
                    objDOADM_SkillsMaster.IsActive = true;
                    objDOADM_SkillsMaster.WorkBasketLkup = objUIDOAccessGroup.WorkBasketLkup;
                    objDOADM_SkillsMaster.RoleLkup = objUIDOAccessGroup.RoleLkup;

                    ExceptionTypes result1 = objBLSkills.SearchSkills(TimeZone,objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out string errorMesssage);
                    ViewBag.Skills = lstDOADM_SkillsMaster.Where(x => x.IsActive == true).ToList();
                }
                return View(objUIDOAccessGroup);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
            
        }
              
        public ActionResult CheckExistsAccessGroup(string accessGroupsName)
        {
            string strErrorMessage = string.Empty;
            string strMessage = string.Empty;
            bool accessGroupExists = false;
            try
            {
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                BLAccessGroup _objBLAccessGroups = new BLAccessGroup();
                DOADM_AccessGroupMaster objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
                objDOADM_AccessGroupMaster.AccessGroupName = accessGroupsName;
                objDOADM_AccessGroupMaster.IsActive = true;
                List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupsMaster;

                ExceptionTypes result = _objBLAccessGroups.GetAccessGroupBasedOnSearch(TimeZone,objDOADM_AccessGroupMaster, out lstDOADM_AccessGroupsMaster);
                if (lstDOADM_AccessGroupsMaster.Count > 0)
                    accessGroupExists = true;
                else
                {
                    objDOADM_AccessGroupMaster.IsActive = false;//search again to check for not active records
                    result = _objBLAccessGroups.GetAccessGroupBasedOnSearch(TimeZone,objDOADM_AccessGroupMaster, out lstDOADM_AccessGroupsMaster);
                    if (lstDOADM_AccessGroupsMaster.Count > 0)
                        accessGroupExists = true;
                    else
                        accessGroupExists = false;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
            return Json(new { accessGroupExists = accessGroupExists, strMessage = strMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetSkillsBasedOnRoleAndWB(long lWorkBasketLkup, long lRoleLkup)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string errorMessage = string.Empty;
            try
            {
                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                BLSkills objBLSkills = new BLSkills();
                objDOADM_SkillsMaster.WorkBasketLkup = lWorkBasketLkup;
                objDOADM_SkillsMaster.RoleLkup = lRoleLkup;
                objDOADM_SkillsMaster.IsActive = true;
                objBLSkills.SearchSkills(TimeZone,objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out errorMessage);
                return Json(lstDOADM_SkillsMaster);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddSkillsToSession(DOADM_AccessGroupSkillsCorrelation objDOADM_AccessGroupSkillsCorrelation, long lPreviousSkill = 0, long lWorkBasketLkup = 0, long lRoleLkup = 0, int mode = 0)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOADM_AccessGroupSkillsCorrelation> lstDOADM_AccessGroupSkillsCorrelation = new List<DOADM_AccessGroupSkillsCorrelation>();
            try
            {
                DOADM_AccessGroupSkillsCorrelation objOldDOADM_AccessGroupSkillsCorrelation = new DOADM_AccessGroupSkillsCorrelation();
                if (Session[ConstantTexts.AccessGroupSkillSessionKey] != null)
                    lstDOADM_AccessGroupSkillsCorrelation = Session[ConstantTexts.AccessGroupSkillSessionKey] as List<DOADM_AccessGroupSkillsCorrelation>;
                switch (mode)
                {
                    case 0://Add new skill to the list
                        if (lstDOADM_AccessGroupSkillsCorrelation.Where(xx => xx.ADM_SkillsMasterRef == objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef).Count() == 0)
                        {
                            objDOADM_AccessGroupSkillsCorrelation.IsActive = true;
                            objDOADM_AccessGroupSkillsCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupSkillsCorrelation.CreatedByName = currentUser.FullName;
                            objDOADM_AccessGroupSkillsCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupSkillsCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupSkillsCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupSkillsCorrelation.LastUpdatedByName = currentUser.FullName;
                            lstDOADM_AccessGroupSkillsCorrelation.Add(objDOADM_AccessGroupSkillsCorrelation);
                        }
                        else
                        {
                            objOldDOADM_AccessGroupSkillsCorrelation = lstDOADM_AccessGroupSkillsCorrelation.Where(xx => xx.ADM_SkillsMasterRef == objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef).FirstOrDefault();
                            objOldDOADM_AccessGroupSkillsCorrelation.CanCreate = objDOADM_AccessGroupSkillsCorrelation.CanCreate;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanClone = objDOADM_AccessGroupSkillsCorrelation.CanClone;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanHistory = objDOADM_AccessGroupSkillsCorrelation.CanHistory;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanMassUpdate = objDOADM_AccessGroupSkillsCorrelation.CanMassUpdate;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanModify = objDOADM_AccessGroupSkillsCorrelation.CanModify;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanReassign = objDOADM_AccessGroupSkillsCorrelation.CanReassign;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanReopen = objDOADM_AccessGroupSkillsCorrelation.CanReopen;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanSearch = objDOADM_AccessGroupSkillsCorrelation.CanSearch;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanUnlock = objDOADM_AccessGroupSkillsCorrelation.CanUnlock;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanUpload = objDOADM_AccessGroupSkillsCorrelation.CanUpload;
                            objOldDOADM_AccessGroupSkillsCorrelation.CanView = objDOADM_AccessGroupSkillsCorrelation.CanView;
                            objOldDOADM_AccessGroupSkillsCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objOldDOADM_AccessGroupSkillsCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupSkillsCorrelation.LastUpdatedByName = currentUser.FullName;
                            objOldDOADM_AccessGroupSkillsCorrelation.IsActive = true;
                        }
                        break;
                    case 1://delete skill from added list
                        if (lstDOADM_AccessGroupSkillsCorrelation.Where(xx => xx.ADM_SkillsMasterRef == objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef).Count() > 0)
                        {
                            objDOADM_AccessGroupSkillsCorrelation = lstDOADM_AccessGroupSkillsCorrelation.Where(xx => xx.ADM_SkillsMasterRef == objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef).FirstOrDefault();
                            objDOADM_AccessGroupSkillsCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupSkillsCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupSkillsCorrelation.IsActive = false;
                        }
                        break;
                    case 2://update existing skill
                        objOldDOADM_AccessGroupSkillsCorrelation = lstDOADM_AccessGroupSkillsCorrelation.Where(x => x.ADM_SkillsMasterRef == lPreviousSkill).FirstOrDefault();
                        if (objOldDOADM_AccessGroupSkillsCorrelation.ADM_AccessGroupSkillsCorrelationId == 0)//updating a queue for new record
                        {
                            objOldDOADM_AccessGroupSkillsCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                        }
                        objOldDOADM_AccessGroupSkillsCorrelation.CanCreate = objDOADM_AccessGroupSkillsCorrelation.CanCreate;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanClone = objDOADM_AccessGroupSkillsCorrelation.CanClone;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanHistory = objDOADM_AccessGroupSkillsCorrelation.CanHistory;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanMassUpdate = objDOADM_AccessGroupSkillsCorrelation.CanMassUpdate;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanModify = objDOADM_AccessGroupSkillsCorrelation.CanModify;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanReassign = objDOADM_AccessGroupSkillsCorrelation.CanReassign;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanReopen = objDOADM_AccessGroupSkillsCorrelation.CanReopen;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanSearch = objDOADM_AccessGroupSkillsCorrelation.CanSearch;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanUnlock = objDOADM_AccessGroupSkillsCorrelation.CanUnlock;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanUpload = objDOADM_AccessGroupSkillsCorrelation.CanUpload;
                        objOldDOADM_AccessGroupSkillsCorrelation.CanView = objDOADM_AccessGroupSkillsCorrelation.CanView;
                        objOldDOADM_AccessGroupSkillsCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                        objOldDOADM_AccessGroupSkillsCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                        objOldDOADM_AccessGroupSkillsCorrelation.LastUpdatedByName = currentUser.FullName;
                        objOldDOADM_AccessGroupSkillsCorrelation.IsActive = true;
                        objOldDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef = objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef;
                        break;
                    case 3://removing all the lists from Session
                        foreach (var item in lstDOADM_AccessGroupSkillsCorrelation)
                        {
                            item.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            item.UTCLastUpdatedOn = DateTime.UtcNow;
                            item.IsActive = false;
                        }
                        break;
                }

                DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                List<DOADM_SkillsMaster> lstDOADM_SkillsMaster = new List<DOADM_SkillsMaster>();
                BLSkills objBLSkills = new BLSkills();
                objDOADM_SkillsMaster.WorkBasketLkup = lWorkBasketLkup;
                objDOADM_SkillsMaster.RoleLkup = lRoleLkup;
                objDOADM_SkillsMaster.IsActive = true;
                objBLSkills.SearchSkills(TimeZone,objDOADM_SkillsMaster, out lstDOADM_SkillsMaster, out string errorMessage);
                ViewBag.Skills = lstDOADM_SkillsMaster;

                Session[ConstantTexts.AccessGroupSkillSessionKey] = lstDOADM_AccessGroupSkillsCorrelation;
                return PartialView("_Skills", lstDOADM_AccessGroupSkillsCorrelation.Where(xx => xx.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult AddReportsToSession(DOADM_AccessGroupReportCorrelation objDOADM_AccessGroupReportCorrelation, long lPreviousReport = 0, int mode = 0)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOADM_AccessGroupReportCorrelation> lstDOADM_AccessGroupReportCorrelation = new List<DOADM_AccessGroupReportCorrelation>();
            try
            {
                if (Session[ConstantTexts.AccessGroupReportSessionKey] != null)
                    lstDOADM_AccessGroupReportCorrelation = Session[ConstantTexts.AccessGroupReportSessionKey] as List<DOADM_AccessGroupReportCorrelation>;
                switch (mode)
                {
                    case 0://Add new Report to the list
                        if (lstDOADM_AccessGroupReportCorrelation.Where(xx => xx.RPT_ReportsMasterRef == objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef).Count() == 0)
                        {
                            objDOADM_AccessGroupReportCorrelation.IsActive = true;
                            objDOADM_AccessGroupReportCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupReportCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupReportCorrelation.CreatedByName = currentUser.FullName;
                            objDOADM_AccessGroupReportCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupReportCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupReportCorrelation.LastUpdatedByName = currentUser.FullName;
                            lstDOADM_AccessGroupReportCorrelation.Add(objDOADM_AccessGroupReportCorrelation);
                        }
                        else
                        {
                            objDOADM_AccessGroupReportCorrelation = lstDOADM_AccessGroupReportCorrelation.Where(xx => xx.RPT_ReportsMasterRef == objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef).FirstOrDefault();
                            objDOADM_AccessGroupReportCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupReportCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupReportCorrelation.LastUpdatedByName = currentUser.FullName;
                            objDOADM_AccessGroupReportCorrelation.IsActive = true;
                        }
                        break;
                    case 1://delete Report from added list
                        if (lstDOADM_AccessGroupReportCorrelation.Where(xx => xx.RPT_ReportsMasterRef == objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef).Count() > 0)
                        {
                            objDOADM_AccessGroupReportCorrelation = lstDOADM_AccessGroupReportCorrelation.Where(xx => xx.RPT_ReportsMasterRef == objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef).FirstOrDefault();
                            objDOADM_AccessGroupReportCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupReportCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupReportCorrelation.IsActive = false;
                        }
                        break;
                    case 2://update existing Report
                        DOADM_AccessGroupReportCorrelation objOldDOADM_AccessGroupReportCorrelation = lstDOADM_AccessGroupReportCorrelation.Where(x => x.RPT_ReportsMasterRef == lPreviousReport).FirstOrDefault();
                        if (objOldDOADM_AccessGroupReportCorrelation.ADM_AccessGroupReportCorrelationId == 0)//updating a report for new record
                        {
                            objOldDOADM_AccessGroupReportCorrelation.CreatedByName = currentUser.FullName;
                            objOldDOADM_AccessGroupReportCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                        }
                        objOldDOADM_AccessGroupReportCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                        objOldDOADM_AccessGroupReportCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                        objDOADM_AccessGroupReportCorrelation.LastUpdatedByName = currentUser.FullName;
                        objOldDOADM_AccessGroupReportCorrelation.IsActive = true;
                        objOldDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef = objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef;
                        break;
                    case 3://removing all the lists from Session
                        foreach (var item in lstDOADM_AccessGroupReportCorrelation)
                        {
                            item.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            item.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            item.IsActive = false;
                        }
                        break;
                }

                ViewBag.Reports = ViewBag.Reports = CacheUtility.GetAllReportsFromCache(null, "");

                Session[ConstantTexts.AccessGroupReportSessionKey] = lstDOADM_AccessGroupReportCorrelation;
                return PartialView("_Reports", lstDOADM_AccessGroupReportCorrelation.Where(xx => xx.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Save(UIDOAccessGroup objUIDOAccessGroup)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();

            try
            {
                //check if this call is to update record or save new record
                if (objUIDOAccessGroup.ADM_AccessGroupMasterId > 0)
                {
                    //if old record is to be updated
                    BLCommon objCommon = new BLCommon();
                    //check if record is locked by current user
                    if (!objCommon.ValidateLockBeforeSave(currentUser.ADM_UserMasterId, (long)ScreenType.AccessGroup, objUIDOAccessGroup.ADM_AccessGroupMasterId))
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

                objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation = new List<DOADM_AccessGroupSkillsCorrelation>();
                objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation = new List<DOADM_AccessGroupReportCorrelation>();

                if (Session[ConstantTexts.AccessGroupSkillSessionKey] != null)
                    objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation = Session[ConstantTexts.AccessGroupSkillSessionKey] as List<DOADM_AccessGroupSkillsCorrelation>;
                if (Session[ConstantTexts.AccessGroupReportSessionKey] != null)
                    objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation = Session[ConstantTexts.AccessGroupReportSessionKey] as List<DOADM_AccessGroupReportCorrelation>;

                BLAccessGroup objBLAccessGroup = new BLAccessGroup();
                result = objBLAccessGroup.AddEditAccessGroup(currentUser.ADM_UserMasterId, objUIDOAccessGroup, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                Session[ConstantTexts.AccessGroupSkillSessionKey] = null;
                Session[ConstantTexts.AccessGroupReportSessionKey] = null;
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult GetAccessGroupReportURL()
        {
            _objBLReports = new BLReports();
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.AccessGroupHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

    } 
}