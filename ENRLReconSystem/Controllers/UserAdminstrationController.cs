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
using System.Web.Script.Serialization;
using System.DirectoryServices;
using System.Text;
using System.Text.RegularExpressions;
using ENRLReconSystem.Common;

namespace ENRLReconSystem.Controllers
{
    [ERSRoleAuthorize(ERSAuthenticationRoles.AdmOSTUser, ERSAuthenticationRoles.AdmEligUser,ERSAuthenticationRoles.AdmRPRUser)]
    public class UserAdminstrationController : Controller
    {
        private UIUserLogin currentUser;
        public UserAdminstrationController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        private BLReports _objBLReports;

        [HttpGet]        
        public ActionResult Search()
        {
            List<DOADM_UserMaster> lstDOADM_UserMaster = new List<DOADM_UserMaster>();            
            string strErrorMessage = string.Empty;
            DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
            objDOADM_UserMaster.IsActive = true;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            lstDOADM_UserMaster = GetUserSearchResult(TimeZone, objDOADM_UserMaster, out strErrorMessage);           
            if (strErrorMessage != "")
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty,"error occred while retriving search results");
                lstDOADM_UserMaster = new List<DOADM_UserMaster>();
            }
            return View(lstDOADM_UserMaster);
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Search(string strMSID = "", string strEmail = "", string strFullname = "", bool bIsActive = true)
        {
            try
            {
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                List<DOADM_UserMaster> lstDOADM_UserMaster = new List<DOADM_UserMaster>();
                string strErrorMessage = string.Empty;
                objDOADM_UserMaster.MSID = strMSID;
                objDOADM_UserMaster.Email = strEmail;
                objDOADM_UserMaster.FullName = strFullname;
                objDOADM_UserMaster.IsActive = bIsActive;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                lstDOADM_UserMaster = GetUserSearchResult(TimeZone,objDOADM_UserMaster, out strErrorMessage);
                if (strErrorMessage != "")
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, "error occred while retriving search results");
                    lstDOADM_UserMaster = new List<DOADM_UserMaster>();
                }
                return PartialView("_SearchResults", lstDOADM_UserMaster);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        public List<DOADM_UserMaster> GetUserSearchResult(long? TimeZone,DOADM_UserMaster objDOADM_UserMaster, out string strErrorMessage)
        {
            BLUserAdministration objBLUserAdministration = new BLUserAdministration();
            List<DOADM_UserMaster> lstDOADM_UserMaster = new List<DOADM_UserMaster>();
            try
            {
                ExceptionTypes result = objBLUserAdministration.SearchUser(TimeZone,objDOADM_UserMaster, out lstDOADM_UserMaster, out strErrorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    //Log Error
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, "Error Occured while doing User Search");
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                strErrorMessage = ex.ToString();
            }
            return lstDOADM_UserMaster;
        }

        [HttpGet]
        public ActionResult Add(long UserId = 0, bool IsActive = true)
        {
            string errorMessage = string.Empty;
            Session[ConstantTexts.UserAccessGroupSessionKey] = null;
            try
            {
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                objDOADM_UserMaster.IsManager = false;//set value since it is nullable
                objDOADM_UserMaster.StartDate = DateTime.UtcNow;
                objDOADM_UserMaster.EndDate = DateTime.UtcNow.AddYears(10);
                objDOADM_UserMaster.IsActive = true;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;

                if (UserId != 0)
                {
                    List<DOADM_UserMaster> lstDOADM_UserMaster = new List<DOADM_UserMaster>();
                    objDOADM_UserMaster.ADM_UserMasterId = UserId;
                    objDOADM_UserMaster.IsActive = IsActive;
                    objBLUserAdministration.SearchUser(TimeZone,objDOADM_UserMaster, out lstDOADM_UserMaster, out errorMessage);
                    if (lstDOADM_UserMaster.Count > 0)
                    {
                        objDOADM_UserMaster = lstDOADM_UserMaster.FirstOrDefault();
                        Session[ConstantTexts.UserAccessGroupSessionKey] = objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation;
                    }
                }

                DOADM_UserMaster objManagerSearch = new DOADM_UserMaster()
                {
                    IsActive = true,
                    IsManager = true
                };

                ExceptionTypes result = objBLUserAdministration.SearchUser(TimeZone,objManagerSearch, out List<DOADM_UserMaster> lstManagers, out errorMessage);

                objDOADM_UserMaster.lstManagers = lstManagers.OrderBy(x => x.FullName).ToList();
                if (UserId != 0)
                    objDOADM_UserMaster.lstManagers.RemoveAll(x => x.ADM_UserMasterId == UserId);

                objDOADM_UserMaster.lstLocation = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Location);
                objDOADM_UserMaster.lstTimeZone = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Timezone);
                objDOADM_UserMaster.lstState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                objDOADM_UserMaster.lstSalutation = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Salutation);
                ViewBag.AccessGroups = GetAllAccessGroups();

                return View(objDOADM_UserMaster);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        public ActionResult CheckUserExistsUsingMSID(string MSID)
        {
            string strErrorMessage = string.Empty;
            string strMessage = string.Empty;
            bool MsidExists = false;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            try
            {
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                objDOADM_UserMaster.MSID = MSID;
                objDOADM_UserMaster.IsActive = true;
                List<DOADM_UserMaster> lstDOADM_UserMaster;

                ExceptionTypes result = objBLUserAdministration.SearchUser(TimeZone,objDOADM_UserMaster, out lstDOADM_UserMaster, out strErrorMessage);
                if (lstDOADM_UserMaster.Count > 0)
                    MsidExists = true;
                else
                {
                    objDOADM_UserMaster.IsActive = false;//search again to check for not active records
                    result = objBLUserAdministration.SearchUser(TimeZone,objDOADM_UserMaster, out lstDOADM_UserMaster, out strErrorMessage);
                    if (lstDOADM_UserMaster.Count > 0)
                        MsidExists = true;
                    else
                        MsidExists = false;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
            return Json(new { MsidExists = MsidExists, strMessage = strMessage }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddAccessGroupsToSession(DOADM_AccessGroupUserCorrelation objDOADM_AccessGroupUserCorrelation, long lPreviousAccessGroup = 0, int mode = 0)
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            List<DOADM_AccessGroupUserCorrelation> lstDOADM_AccessGroupUserCorrelation = new List<DOADM_AccessGroupUserCorrelation>();
            try
            {
                if (Session[ConstantTexts.UserAccessGroupSessionKey] != null)
                    lstDOADM_AccessGroupUserCorrelation = Session[ConstantTexts.UserAccessGroupSessionKey] as List<DOADM_AccessGroupUserCorrelation>;
                switch (mode)
                {
                    case 0://Add new AccessGroup to the list
                        if (lstDOADM_AccessGroupUserCorrelation.Where(xx => xx.ADM_AccessGroupMasterRef == objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef).Count() == 0)
                        {
                            objDOADM_AccessGroupUserCorrelation.IsActive = true;
                            objDOADM_AccessGroupUserCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupUserCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                            objDOADM_AccessGroupUserCorrelation.CreatedByName = currentUser.FullName;
                            objDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            objDOADM_AccessGroupUserCorrelation.LastUpdatedBy = currentUser.FullName;
                            lstDOADM_AccessGroupUserCorrelation.Add(objDOADM_AccessGroupUserCorrelation);
                        }
                        else
                        {
                            objDOADM_AccessGroupUserCorrelation = lstDOADM_AccessGroupUserCorrelation.Where(xx => xx.ADM_AccessGroupMasterRef == objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef).FirstOrDefault();
                            objDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            objDOADM_AccessGroupUserCorrelation.LastUpdatedBy = currentUser.FullName;
                            objDOADM_AccessGroupUserCorrelation.IsActive = true;
                        }
                        break;
                    case 1://delete AccessGroup from added list
                        if (lstDOADM_AccessGroupUserCorrelation.Where(xx => xx.ADM_AccessGroupMasterRef == objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef).Count() > 0)
                        {
                            objDOADM_AccessGroupUserCorrelation = lstDOADM_AccessGroupUserCorrelation.Where(xx => xx.ADM_AccessGroupMasterRef == objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef).FirstOrDefault();
                            objDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            objDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            objDOADM_AccessGroupUserCorrelation.IsActive = false;
                        }
                        break;
                    case 2://update existing AccessGroup
                        DOADM_AccessGroupUserCorrelation objOldDOADM_AccessGroupUserCorrelation = lstDOADM_AccessGroupUserCorrelation.Where(x => x.ADM_AccessGroupMasterRef == lPreviousAccessGroup).FirstOrDefault();
                        if (objOldDOADM_AccessGroupUserCorrelation.ADM_AccessGroupUserCorrelationId == 0)//updating a AccessGroup for new record
                        {
                            objOldDOADM_AccessGroupUserCorrelation.CreatedByName = currentUser.FullName;
                            objOldDOADM_AccessGroupUserCorrelation.CreatedByRef = currentUser.ADM_UserMasterId;
                        }
                        objOldDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                        objOldDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                        objDOADM_AccessGroupUserCorrelation.LastUpdatedBy = currentUser.FullName;
                        objOldDOADM_AccessGroupUserCorrelation.IsActive = true;
                        objOldDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef = objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef;
                        break;
                    case 3://removing all the lists from Session
                        foreach (var item in lstDOADM_AccessGroupUserCorrelation)
                        {
                            item.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            item.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                            item.IsActive = false;
                        }
                        break;
                }

                ViewBag.AccessGroups = GetAllAccessGroups();

                Session[ConstantTexts.UserAccessGroupSessionKey] = lstDOADM_AccessGroupUserCorrelation;
                return PartialView("_AccessGroup", lstDOADM_AccessGroupUserCorrelation.Where(xx => xx.IsActive == true).ToList());
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageAccessGroups, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", routeValues: ex.ToString());
            }
        }

        private List<DOADM_AccessGroupMaster> GetAllAccessGroups()
        {
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            BLAccessGroup objBLAccessGroup = new BLAccessGroup();
            DOADM_AccessGroupMaster objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
            objDOADM_AccessGroupMaster.IsActive = true;

            List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster = new List<DOADM_AccessGroupMaster>();
            try
            {
                ExceptionTypes result = objBLAccessGroup.GetAccessGroupBasedOnSearch(TimeZone, objDOADM_AccessGroupMaster, out lstDOADM_AccessGroupMaster);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, "Error occured while retriving search results");
                }
                return lstDOADM_AccessGroupMaster;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return null;
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(DOADM_UserMaster objDOADM_UserMaster)
        {
            string errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                //check if this call is to update record or save new record
                if (objDOADM_UserMaster.ADM_UserMasterId > 0)
                {
                    //if old record is to be updated
                    BLCommon objCommon = new BLCommon();
                    //check if record is locked by current user
                    if (!objCommon.ValidateLockBeforeSave(currentUser.ADM_UserMasterId, (long)ScreenType.UserAdmin, objDOADM_UserMaster.ADM_UserMasterId))
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
                    objDOADM_UserMaster.CreatedByRef = currentUser.ADM_UserMasterId;
                    //If new record to be saved
                    returnMessage = "Record saved successfully.";
                }

                objDOADM_UserMaster.FullName = objDOADM_UserMaster.FirstName != null ? objDOADM_UserMaster.LastName + ", " + objDOADM_UserMaster.FirstName : objDOADM_UserMaster.LastName;
                objDOADM_UserMaster.SpecialistFax = String.IsNullOrEmpty(objDOADM_UserMaster.SpecialistFax) ? "" : String.Join("", objDOADM_UserMaster.SpecialistFax.Split('-'));
                objDOADM_UserMaster.SpecialistPhone = String.IsNullOrEmpty(objDOADM_UserMaster.SpecialistPhone) ? "" : String.Join("", objDOADM_UserMaster.SpecialistPhone.Split('-'));
                objDOADM_UserMaster.UserZip = String.IsNullOrEmpty(objDOADM_UserMaster.UserZip) ? "" : String.Join("", objDOADM_UserMaster.UserZip.Split('-'));
                objDOADM_UserMaster.IsActive = objDOADM_UserMaster.IsActive;
                objDOADM_UserMaster.IsManager = objDOADM_UserMaster.IsManager;

                objDOADM_UserMaster.CreatedByRoleLkup = currentUser.RoleLkup;
                objDOADM_UserMaster.UpdatedByRoleLkup = currentUser.RoleLkup;
                objDOADM_UserMaster.LastUpdatedByRef = currentUser.ADM_UserMasterId;

                if (Session[ConstantTexts.UserAccessGroupSessionKey] != null)
                    objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation = Session[ConstantTexts.UserAccessGroupSessionKey] as List<DOADM_AccessGroupUserCorrelation>;

                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                result = objBLUserAdministration.SaveUser(objDOADM_UserMaster, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, "error occured while saving data");
                    return Json(new { ID = result, Message = "An error occured while updating DB." });
                }
                return Json(new { ID = result, Message = returnMessage });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }
        
        [HttpPost]
        public ActionResult GetUserAdminReportURL()
        {
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            _objBLReports = new BLReports();
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.UserAdminHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

        /// <summary>
        /// Get user details from LDAP
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUserDetailsLdap(string data)
        {
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            try
            {
                DOADM_UserMaster user = null;
                bool isUserFound = GetUserDetails(data, true, out user);

                if (!isUserFound)
                {
                    isUserFound = GetUserDetails(data, false, out user);
                }

                if (isUserFound)
                {
                    result = ExceptionTypes.Success;
                }
                else
                {
                    result= ExceptionTypes.Exception;
                    returnMessage = "User not found in Active directory.";
                }
                return Json(new { ID = result, Message = returnMessage,data = user });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        /// <summary>
        /// Method to load user details by user id, Msid.
        /// </summary>
        /// <param name="strMSID"></param>
        /// <param name="isMSDomain"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool GetUserDetails(string strMSID, bool isMSDomain, out DOADM_UserMaster user)
        {
            bool isUserFound = false;
            user = new DOADM_UserMaster();
            string errorMessage = string.Empty;

            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime; //just dummy time for Search SP. Not actually used
            try
            {

                DirectoryEntry entry = null;

                if (isMSDomain)
                {
                    string serviceAccountName = WebConfigData.DmLogin;
                    string decryptPwd = WebConfigData.DmPwd;

                    string strDomain = WebConfigData.MSDomain;// "ms.ds.uhc.com";
                    entry = new DirectoryEntry("LDAP://" + strDomain, serviceAccountName, decryptPwd);
                }
                else
                {
                    string strDomain = "XLHEALTH";
                    entry = new DirectoryEntry("LDAP://" + strDomain);
                }

                DirectorySearcher search = new DirectorySearcher(entry);
                search.ReferralChasing = ReferralChasingOption.All;
                search.Filter = ("(SAMAccountName=" + strMSID + ")");
                //Searcher.Filter = ("(anr=" + strLoginName + ")");
                SearchResultCollection searchResult = search.FindAll();

                StringBuilder sbResult = new StringBuilder();
                Regex rxCnName = new Regex("CN\\=([^\\,]+)");

                if (searchResult != null)
                {

                    foreach (SearchResult sr in searchResult)
                    {
                        isUserFound = true;
                        string strUserName = (sr.GetDirectoryEntry().Name).Replace("CN=", "").Replace(@"\", "");
                        //user full name
                        PropertyValueCollection displayName = null;
                        PropertyValueCollection displayName_Fname = null;
                        PropertyValueCollection displayName_Lname = null;

                        displayName = sr.GetDirectoryEntry().Properties["displayName"];
                        displayName_Fname = sr.GetDirectoryEntry().Properties["givenname"];
                        displayName_Lname = sr.GetDirectoryEntry().Properties["sn"];

                        if (displayName != null && displayName.Value != null)
                        {
                            //user.FullName = displayName.Value.ToString();
                        }
                        if (displayName_Fname != null && displayName_Fname.Value != null && displayName_Lname != null && displayName_Lname.Value != null)
                        {
                            user.FullName = displayName_Fname.Value.ToString() + " " + displayName_Lname.Value.ToString();
                        }
                        else if ((displayName_Fname != null && displayName_Fname.Value != null) && (displayName_Lname == null && displayName_Lname.Value == null))
                        {
                            user.FullName = displayName_Fname.Value.ToString();
                        }
                        else if ((displayName_Fname == null && displayName_Fname.Value == null) && (displayName_Lname != null && displayName_Lname.Value != null))
                        {
                            user.FullName = displayName_Lname.Value.ToString();
                        }
                        else
                        {
                            user.FullName = displayName.Value.ToString();
                        }

                        if (!user.FullName.IsNullOrEmpty())
                        {
                            if (user.FullName.Split(' ').Length > 0)
                                user.FirstName = user.FullName.Split(' ')[0].IsNullOrEmpty() ? "" : user.FullName.Split(' ')[0].ToString();
                            if (user.FullName.Split(' ').Length > 1)
                                user.LastName = user.FullName.Split(' ')[1].IsNullOrEmpty() ? "" : user.FullName.Split(' ')[1].ToString();
                        }

                        //manager
                        PropertyValueCollection manager = null;
                        manager = sr.GetDirectoryEntry().Properties["manager"];
                        if (manager != null)
                        {
                            Match mManager = rxCnName.Match(manager[0].ToString());
                            string managerMsid = "";
                            if (mManager.Success)
                            {
                                managerMsid = mManager.Groups[1].Value;
                                DOADM_UserMaster mgrDOADM_UserMaster = new DOADM_UserMaster();
                                mgrDOADM_UserMaster.MSID = managerMsid;
                                mgrDOADM_UserMaster.IsActive = true;
                                //get manager id by msid
                                var listUsers = GetUserSearchResult(TimeZone,mgrDOADM_UserMaster,out errorMessage);
                                if (listUsers.Count > 0)
                                {
                                    user.ManagerId = listUsers.FirstOrDefault().ADM_UserMasterId;
                                }
                            }
                        }

                        //mail

                        if (isMSDomain)
                        {
                            PropertyValueCollection mail = null;
                            mail = sr.GetDirectoryEntry().Properties["mail"];
                            if (mail != null && mail.Value != null)
                            {
                                user.Email = mail.Value.ToString();
                            }
                        }
                        else
                        {
                            PropertyValueCollection mail = null;
                            mail = sr.GetDirectoryEntry().Properties["userPrincipalName"];
                            if (mail != null && mail.Value != null)
                            {
                                user.Email = mail.Value.ToString().Replace("local", "com");
                            }
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.ManageUsers, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }

            return isUserFound;
        }
    }
}