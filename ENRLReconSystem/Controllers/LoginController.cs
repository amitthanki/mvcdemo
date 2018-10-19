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
    public class LoginController : Controller
    {
        public object LastActivity
        {
            get
            {
                try
                {
                    object rtnValue = System.Web.HttpContext.Current.Session[ConstantTexts.LastActivitySessionKey];
                    return rtnValue;
                }
                catch
                {
                    return null;
                }
            }
            set
            {

                try
                {
                    System.Web.HttpContext.Current.Session[ConstantTexts.LastActivitySessionKey] = value;
                }
                catch
                {
                    //sometimes its observed that session object is null 
                    //there is a immedeate request which follows the above request with the actual data
                    //Hence the "session object is null" request must be ignored

                    //no logging for this error
                }

            }
        }



        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                //check If session exists
                if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] == null)
                {
                    string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    string domain = strLoginName[0];
                    string loginName = strLoginName[1];

                    //Checking user in Database.
                    BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                    ExceptionTypes result = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out UIUserLogin loggedInUser);
                    if (result == ExceptionTypes.ZeroRecords)
                    {
                        loggedInUser.IsAuthorizedUser = false;
                        loggedInUser.ErrorMessage = string.Format(ConstantTexts.NotPartOfERSDBError, loginName);
                        return View(loggedInUser);
                    }
                    if (result != (long)ExceptionTypes.Success)
                    {
                        BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Login, (long)ExceptionTypes.Uncategorized, string.Empty, "Error while fetching logged in user data");
                        return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Something went wrong while retriving user details from login." });
                    }
                    else
                    {
                        #region Check User AD Groups

                        System.Security.Principal.WindowsIdentity winIdnt = System.Web.HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity;
                        System.Security.Principal.IdentityReferenceCollection grps = winIdnt.Groups;
                        ////Admin
                        if (IsUserInADGroup(grps, WebConfigData.AdminSID))
                            loggedInUser.IsAdminUser = true;

                        //OST
                        if (IsUserInADGroup(grps, WebConfigData.AdminOSTSID))
                            loggedInUser.IsAdmOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerOSTSID))
                            loggedInUser.IsMgrOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorOSTSID))
                            loggedInUser.IsPrcrOSTUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerOSTSID))
                            loggedInUser.IsVwrOSTUser = true;

                        //Eligibility
                        if (IsUserInADGroup(grps, WebConfigData.AdminEligSID))
                            loggedInUser.IsAdmEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerEligSID))
                            loggedInUser.IsMgrEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorEligSID))
                            loggedInUser.IsPrcrEligUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerEligSID))
                            loggedInUser.IsVwrEligUser = true;

                        //RPR
                        if (IsUserInADGroup(grps, WebConfigData.AdminRPRSID))
                            loggedInUser.IsAdmRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ManagerRPRSID))
                            loggedInUser.IsMgrRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ProcessorRPRSID))
                            loggedInUser.IsPrcrRPRUser = true;
                        if (IsUserInADGroup(grps, WebConfigData.ViewerRPRSID))
                            loggedInUser.IsVwrRPRUser = true;

                        if (IsUserInADGroup(grps, WebConfigData.RestrictedSID))
                        {
                            loggedInUser.IsRestrictedUser = true;
                        }
                        #endregion

                        //check if user has atleast one AD group assigned
                        if (loggedInUser.IsAdminUser || loggedInUser.IsAdmOSTUser || loggedInUser.IsAdmEligUser || loggedInUser.IsAdmRPRUser
                           || loggedInUser.IsMgrOSTUser || loggedInUser.IsMgrEligUser || loggedInUser.IsMgrRPRUser || loggedInUser.IsPrcrOSTUser
                           || loggedInUser.IsPrcrEligUser || loggedInUser.IsPrcrRPRUser || loggedInUser.IsVwrOSTUser || loggedInUser.IsVwrEligUser
                           || loggedInUser.IsVwrRPRUser || loggedInUser.IsWebServiceUser || loggedInUser.IsMacroServiceUser)
                        {
                            if (loggedInUser.UserSkills != null && loggedInUser.UserSkills.Count > 0)
                            {
                                loggedInUser.IsAuthorizedUser = true;
                                loggedInUser = LoadDataForLogin(loggedInUser);
                                Session[ConstantTexts.UserSessionBeforeLoginKey] = loggedInUser;
                                return View(loggedInUser);
                            }
                            else
                            {
                                loggedInUser.IsAuthorizedUser = false;
                                loggedInUser.ErrorMessage = string.Format(ConstantTexts.NoAccessGroupAssignedError, loginName);
                                return View(loggedInUser);
                            }

                        }
                        else
                        {
                            loggedInUser.IsAuthorizedUser = false;
                            loggedInUser.ErrorMessage = string.Format(ConstantTexts.NotPartOfADGroupError,loginName);
                            return View(loggedInUser);
                        }
                    }
                }
                else
                {
                    //session exists
                    return RedirectToAction("Home", "Home");
                }

            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Login, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }

        }

        [HttpPost]
        public ActionResult Login(long BusinessSegmentLkup = 0, long WorkBasketLkup = 0, long RoleLkup = 0)
        {
            try
            {
                ReLogin: string s = string.Empty;
                if (BusinessSegmentLkup > 0 && WorkBasketLkup > 0 && RoleLkup > 0)
                {
                    string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                    string domain = strLoginName[0];
                    string loginName = strLoginName[1];

                    if (Session[ConstantTexts.UserSessionBeforeLoginKey] != null)
                    {
                        DO.UIUserLogin loggedInUser = (DO.UIUserLogin)Session[ConstantTexts.UserSessionBeforeLoginKey];

                        loggedInUser.WorkBasketLkup = WorkBasketLkup;
                        loggedInUser.BusinessSegmentLkup = BusinessSegmentLkup;
                        loggedInUser.RoleLkup = RoleLkup;

                        bool IsADGroupAssigned = true;
                        string WorkBasletADGroupRequired = "";
                        string RoleADGroupRequired = "";

                        //check if user has AD group access to slected Role and Workbasket
                        switch (RoleLkup)
                        {
                            case (long)Utility.RoleLkup.Admin:
                                RoleADGroupRequired = Utility.RoleLkup.Admin.ToString();
                                switch (WorkBasketLkup)
                                {
                                    case (long)Utility.WorkBasket.GPSvsMMR:
                                        IsADGroupAssigned = loggedInUser.IsAdmEligUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.GPSvsMMR.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.OST:
                                        IsADGroupAssigned = loggedInUser.IsAdmOSTUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.OST.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.RPR:
                                        IsADGroupAssigned = loggedInUser.IsAdmRPRUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.RPR.ToString();
                                        break;
                                }
                                break;
                            case (long)Utility.RoleLkup.Processor:
                                RoleADGroupRequired = Utility.RoleLkup.Processor.ToString();
                                switch (WorkBasketLkup)
                                {
                                    case (long)Utility.WorkBasket.GPSvsMMR:
                                        IsADGroupAssigned = loggedInUser.IsPrcrEligUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.GPSvsMMR.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.OST:
                                        IsADGroupAssigned = loggedInUser.IsPrcrOSTUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.OST.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.RPR:
                                        IsADGroupAssigned = loggedInUser.IsPrcrRPRUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.RPR.ToString();
                                        break;
                                }
                                break;
                            case (long)Utility.RoleLkup.Manager:
                                RoleADGroupRequired = Utility.RoleLkup.Manager.ToString();
                                switch (WorkBasketLkup)
                                {
                                    case (long)Utility.WorkBasket.GPSvsMMR:
                                        IsADGroupAssigned = loggedInUser.IsMgrEligUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.GPSvsMMR.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.OST:
                                        IsADGroupAssigned = loggedInUser.IsMgrOSTUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.OST.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.RPR:
                                        IsADGroupAssigned = loggedInUser.IsMgrRPRUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.RPR.ToString();
                                        break;
                                }
                                break;
                            case (long)Utility.RoleLkup.Viewer:
                                RoleADGroupRequired = Utility.RoleLkup.Viewer.ToString();
                                switch (WorkBasketLkup)
                                {
                                    case (long)Utility.WorkBasket.GPSvsMMR:
                                        IsADGroupAssigned = loggedInUser.IsVwrEligUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.GPSvsMMR.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.OST:
                                        IsADGroupAssigned = loggedInUser.IsVwrOSTUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.OST.ToString();
                                        break;
                                    case (long)Utility.WorkBasket.RPR:
                                        IsADGroupAssigned = loggedInUser.IsVwrRPRUser;
                                        WorkBasletADGroupRequired = Utility.WorkBasket.RPR.ToString();
                                        break;
                                }
                                break;
                        }

                        if (!IsADGroupAssigned)
                        {
                            //If no AD group Access return error message
                            loggedInUser.ErrorMessage = string.Format(ConstantTexts.NotPartofADgroupForRoleError, RoleADGroupRequired, WorkBasletADGroupRequired);
                            return View(loggedInUser);
                        }

                        BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                        ExceptionTypes result = objBLUserAdministration.LoginUser(loginName);
                        if (result != (long)ExceptionTypes.Success)
                        {
                            loggedInUser.ErrorMessage = ConstantTexts.LoginException;
                            return View(loggedInUser);
                        }

                        //filter skills for slected workbasket and roles
                        loggedInUser.UserSkills = loggedInUser.UserSkills.Where(x => x.WorkBasketLkup.Equals(WorkBasketLkup) && x.RoleLkup.Equals(RoleLkup)).ToList();

                        //Filter Queues avalaible to user in current access group
                        loggedInUser.UserQueueList = (from UQL in loggedInUser.UserQueueList
                                                      join US in loggedInUser.UserSkills on UQL.QueueLkp equals US.WorkQueuesLkup
                                                      where US.WorkQueuesLkup > 0
                                                      select UQL).GroupBy(x => x.QueueLkp).Select(y => y.FirstOrDefault()).ToList();

                        //Set Menu Visibilities
                        switch (WorkBasketLkup)
                        {
                            case (long)WorkBasket.GPSvsMMR:
                                loggedInUser.IsMMREligibilityMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Eligibility);
                                if(loggedInUser.IsMMREligibilityMenuVisible)
                                {
                                    loggedInUser.EligibilityMMRCanCreate = loggedInUser.UserSkills.Any(x =>x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Eligibility);
                                    loggedInUser.EligibilityMMRCanSearch = loggedInUser.UserSkills.Any(x =>x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Eligibility);
                                }
                                loggedInUser.IsDOBMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.DOB);
                                if (loggedInUser.IsDOBMenuVisible)
                                {
                                    loggedInUser.EligibilityDOBCanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.DOB);
                                    loggedInUser.EligibilityDOBCanSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.DOB);
                                }
                                loggedInUser.IsGENDERMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Gender);
                                if (loggedInUser.IsGENDERMenuVisible)
                                {
                                    loggedInUser.EligibilityGenderCanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Gender);
                                    loggedInUser.EligibilityGenderCanSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Gender);
                                }
                                break;
                            case (long)WorkBasket.OST:
                                loggedInUser.IsOOAMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA);
                                if (loggedInUser.IsOOAMenuVisible)
                                {
                                    loggedInUser.OSTOOACanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA);
                                    loggedInUser.OSTOOACanSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA);
                                }
                                loggedInUser.IsSCCMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.SCC);
                                if (loggedInUser.IsSCCMenuVisible)
                                {
                                    loggedInUser.OSTSCCCanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.SCC);
                                    loggedInUser.OSTSCCCanSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.SCC);
                                }
                                loggedInUser.IsTRRMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.TRR);
                                if (loggedInUser.IsTRRMenuVisible)
                                {
                                    loggedInUser.OSTTRRCanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.TRR);
                                    loggedInUser.OSTTRRCanSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.TRR);
                                }
                                break;
                            case (long)WorkBasket.RPR:
                                loggedInUser.IsRPRMenuVisible = loggedInUser.UserSkills.Any(x => x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.RPR);
                                if (loggedInUser.IsRPRMenuVisible)
                                {
                                    loggedInUser.RPRCanCreate = loggedInUser.UserSkills.Any(x => x.CanCreate && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.RPR);
                                    loggedInUser.RPRCanCSearch = loggedInUser.UserSkills.Any(x => x.CanSearch && x.DiscrepancyCategoryLkup == (long)DiscripancyCategory.RPR);
                                }
                                break;
                        }

                        //Set Manager Sub Menu Visibilities
                        if (RoleLkup == (long)Utility.RoleLkup.Manager || RoleLkup == (long)Utility.RoleLkup.Admin)
                        {
                            loggedInUser.CanMassReassign = loggedInUser.UserSkills.Any(x => x.CanReassign && x.WorkBasketLkup == WorkBasketLkup);
                            loggedInUser.CanMassUnlock = loggedInUser.UserSkills.Any(x => x.CanUnlock && x.WorkBasketLkup == WorkBasketLkup);
                            loggedInUser.CanMassUpdate = loggedInUser.UserSkills.Any(x => x.CanMassUpdate && x.WorkBasketLkup == WorkBasketLkup);
                            loggedInUser.CanMassUpload = loggedInUser.UserSkills.Any(x => x.CanUpload && x.WorkBasketLkup == WorkBasketLkup);
                        }

                        if(loggedInUser.LocationLkup == (long)Location.Onshore && loggedInUser.IsRestrictedUser)
                            loggedInUser.IsRestrictedUser = true;
                        else
                            loggedInUser.IsRestrictedUser = false;
                        
                        LastActivity = DateTime.Now;
                        
                        Session[ConstantTexts.CurrentUserSessionKey] = loggedInUser;
                        return RedirectToAction("Home", "Home");
                    }
                    else
                    {
                        Session[ConstantTexts.CurrentUserSessionKey] = null;
                        UIUserLogin loggedInUser;
                        if (LoadCurrentUserSession(out loggedInUser))
                        {
                            goto ReLogin;
                        }
                        return View(loggedInUser);
                    }

                }
                Session[ConstantTexts.CurrentUserSessionKey] = null;
                return RedirectToAction("Login", "Login");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Login, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        //[ERSAuthentication(Roles = "User")]
        [HttpGet]
        public ActionResult Logout()
        {
            try
            {
                string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                string domain = strLoginName[0];
                string loginName = strLoginName[1];
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                objBLUserAdministration.UserLogout(loginName);

                Session[ConstantTexts.CurrentUserSessionKey] = null;
                Session[ConstantTexts.UserSessionBeforeLoginKey] = null;
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.SetNoStore();

                return View();
            }
            catch
            {
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Something went wrong while logout." });
            }
        }

        private bool IsUserInADGroup(System.Security.Principal.IdentityReferenceCollection grps, string SIDs)
        {
            //SIDs can be multiple SID value delimated by ;
            string[] ADGroupSIDs = SIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            bool isUserInADGroup = (from g in grps
                                    where ADGroupSIDs.Any(s => s == g.Value)
                                    select g).Any();
            return isUserInADGroup;
        }

        private DO.UIUserLogin LoadDataForLogin(DO.UIUserLogin objUIUserLogin)
        {
            try
            {
                if (objUIUserLogin.UserSkills != null && objUIUserLogin.UserSkills.Count > 0)
                {
                    List<DOCMN_LookupMaster> lstAllDOCMN_LookupMaster = CacheUtility.GetAllLookupsFromCache(null);
                    List<DOCMN_LookupMaster> lstDOCMN_LookupMaster;


                    List<UserSkills> Correlations = objUIUserLogin.UserSkills.GroupBy(x => new { x.BusinessSegmentLkup, x.WorkBasketLkup, x.RoleLkup }).Select(x => x.First()).ToList();
                    objUIUserLogin.Correlations = Correlations;

                    lstDOCMN_LookupMaster = new List<DOCMN_LookupMaster>();
                    //Business Segment
                    Correlations.Select(x => x.BusinessSegmentLkup).Distinct().ToList().ForEach(BusinessSegmentId =>
                    {
                        if (BusinessSegmentId > 0)
                        {
                            lstDOCMN_LookupMaster.Add(lstAllDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterId.Equals(BusinessSegmentId)).FirstOrDefault());
                        }
                    });
                    //Work Basket
                    Correlations.Select(x => x.WorkBasketLkup).Distinct().ToList().ForEach(WorkBasketId =>
                    {
                        if (WorkBasketId > 0)
                        {
                            lstDOCMN_LookupMaster.Add(lstAllDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterId.Equals(WorkBasketId)).FirstOrDefault());
                        }
                    });
                    //Role
                    Correlations.Select(x => x.RoleLkup).Distinct().ToList().ForEach(RoleId =>
                    {
                        if (RoleId > 0)
                        {
                            lstDOCMN_LookupMaster.Add(lstAllDOCMN_LookupMaster.Where(x => x.CMN_LookupMasterId.Equals(RoleId)).FirstOrDefault());
                        }
                    });
                    objUIUserLogin.LookUps = lstDOCMN_LookupMaster;
                }
                if (objUIUserLogin.ADM_UserPreference != null)
                {
                    objUIUserLogin.BusinessSegmentLkup = objUIUserLogin.ADM_UserPreference.BusinessSegmentLkup;
                    objUIUserLogin.WorkBasketLkup = objUIUserLogin.ADM_UserPreference.WorkBasketLkup;
                    objUIUserLogin.RoleLkup = objUIUserLogin.ADM_UserPreference.RoleLkup;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Login, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return objUIUserLogin;
        }

        /// <summary>
        /// load session while session timeout in Login Page
        /// </summary>
        /// <returns></returns>
        private bool LoadCurrentUserSession(out UIUserLogin loggedInUser)
        {
            bool isSuccess = false;
            BLUserAdministration objBLUserAdministration = new BLUserAdministration();

            try
            {
                string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
                string domain = strLoginName[0];
                string loginName = strLoginName[1];

                ExceptionTypes result = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out loggedInUser);
                if (result == ExceptionTypes.ZeroRecords)
                {
                    loggedInUser.IsAuthorizedUser = false;
                    loggedInUser.ErrorMessage = ConstantTexts.NotPartOfERSDBError;
                    return isSuccess;

                }
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Login, (long)ExceptionTypes.Uncategorized, string.Empty, "Error while fetching logged in user data");
                    return isSuccess;
                }
                else
                {
                    #region Check User AD Groups

                    System.Security.Principal.WindowsIdentity winIdnt = System.Web.HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity;
                    System.Security.Principal.IdentityReferenceCollection grps = winIdnt.Groups;
                    ////Admin
                    if (IsUserInADGroup(grps, WebConfigData.AdminSID))
                        loggedInUser.IsAdminUser = true;

                    //OST
                    if (IsUserInADGroup(grps, WebConfigData.AdminOSTSID))
                        loggedInUser.IsAdmOSTUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ManagerOSTSID))
                        loggedInUser.IsMgrOSTUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ProcessorOSTSID))
                        loggedInUser.IsPrcrOSTUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ViewerOSTSID))
                        loggedInUser.IsVwrOSTUser = true;

                    //Eligibility
                    if (IsUserInADGroup(grps, WebConfigData.AdminEligSID))
                        loggedInUser.IsAdmEligUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ManagerEligSID))
                        loggedInUser.IsMgrEligUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ProcessorEligSID))
                        loggedInUser.IsPrcrEligUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ViewerEligSID))
                        loggedInUser.IsVwrEligUser = true;

                    //RPR
                    if (IsUserInADGroup(grps, WebConfigData.AdminRPRSID))
                        loggedInUser.IsAdmRPRUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ManagerRPRSID))
                        loggedInUser.IsMgrRPRUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ProcessorRPRSID))
                        loggedInUser.IsPrcrRPRUser = true;
                    if (IsUserInADGroup(grps, WebConfigData.ViewerRPRSID))
                        loggedInUser.IsVwrRPRUser = true;
                    #endregion

                    //check if user has atleast one AD group assigned
                    if (loggedInUser.IsAdminUser || loggedInUser.IsAdmOSTUser || loggedInUser.IsAdmEligUser || loggedInUser.IsAdmRPRUser
                       || loggedInUser.IsMgrOSTUser || loggedInUser.IsMgrEligUser || loggedInUser.IsMgrRPRUser || loggedInUser.IsPrcrOSTUser
                       || loggedInUser.IsPrcrEligUser || loggedInUser.IsPrcrRPRUser || loggedInUser.IsVwrOSTUser || loggedInUser.IsVwrEligUser
                       || loggedInUser.IsVwrRPRUser || loggedInUser.IsWebServiceUser || loggedInUser.IsMacroServiceUser)
                    {
                        if (loggedInUser.UserSkills != null && loggedInUser.UserSkills.Count > 0)
                        {
                            loggedInUser.IsAuthorizedUser = true;
                            loggedInUser = LoadDataForLogin(loggedInUser);
                            Session[ConstantTexts.UserSessionBeforeLoginKey] = loggedInUser;
                            isSuccess = true;
                        }
                        else
                        {
                            loggedInUser.IsAuthorizedUser = false;
                            loggedInUser.ErrorMessage = ConstantTexts.NoAccessGroupAssignedError;
                        }

                    }
                    else
                    {
                        loggedInUser.IsAuthorizedUser = false;
                        loggedInUser.ErrorMessage = ConstantTexts.NotPartOfADGroupError;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }

        public ActionResult DownloadTemplate()
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;
            string ProductionAccessSteps = CacheUtility.GetMasterConfigurationByName(ConstantTexts.ProductionAccessSteps);
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(ProductionAccessSteps);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }

        }

        public ActionResult UserManualDownload()
        {
            byte[] fileBytes = null;
            string fileName = string.Empty;
            string UserManual = CacheUtility.GetMasterConfigurationByName(ConstantTexts.UserManual);
            try
            {
                fileBytes = System.IO.File.ReadAllBytes(UserManual);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }

        }

    }
}