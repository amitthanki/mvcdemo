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
    public class MIIMConnectorController : Controller
    {
        BLMIIMIntegration _objBLMIIMIntegration = new BLMIIMIntegration();
        long _currentLoginUserId = 11;//for MIIM System will be set in Create session

        // GET: MIIMConnector
        public ActionResult GetCaseDetails(string caseId)
        {
            ViewBag.PageName = ConstantTexts.MiimPageName;
            try
            {
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMNew, _currentLoginUserId, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                if (long.TryParse(caseId, out long lQueueID))// to check if case exists in DB
                {
                    #region Create Session
                    ExceptionTypes result = CreateSession(WorkBasket.OST);
                    #endregion

                    if (result == ExceptionTypes.Success)
                    {
                        ///Log For User Authentication Success
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMUserAuthSucc, _currentLoginUserId, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);

                        #region validate Record Lock
                        UIRecordsLock objRecordsLocked = new UIRecordsLock();
                        bool isAvailable = true;
                        try
                        {

                            BLCommon objCommon = new BLCommon();
                            result = objCommon.GetLockedRecordOrLockRecord(_currentLoginUserId, (long)ScreenType.Queue, lQueueID, false, out objRecordsLocked);

                            if (result == (long)ExceptionTypes.Success)
                            {
                                isAvailable = objRecordsLocked.LockedHours.IsNullOrEmpty();
                            }
                        }
                        catch (Exception ex)
                        {
                            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, "Record locking failed", _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                            BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                            ViewBag.ErrorMessage = "\nError Locking the Record Please try Again.";
                            return View("~/Views/Shared/Error.cshtml");
                        }
                        #endregion

                        if (isAvailable)
                        {
                            #region Get Case Discrepancy Category
                            result = _objBLMIIMIntegration.GetCaseDiscrepancyCategory(lQueueID, out long lDiscrepancyCategory, out string errorMessage);
                            if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                            {
                                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMRecordNotFound, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, errorMessage, "Error occured while getting Discrepancy Category.");
                                ViewBag.ErrorMessage = "\nDetails Not available for selected Case.";
                                return View("~/Views/Shared/Error.cshtml");
                            }
                            #endregion

                            #region Redirect to Process Work Screen based on Work Basket
                            string strEncodedCaseId = URLEncoderDecoder.Encode(lQueueID.ToString());
                            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMRequestCompleted, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                            switch (lDiscrepancyCategory)
                            {
                                case (long)DiscripancyCategory.RPR:
                                    return RedirectToAction("RPRProcessWork", "RPR", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.Eligibility:
                                    return RedirectToAction("EligibilityProcessWork", "Eligibility", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.DOB:
                                    return RedirectToAction("DOBProcessWork", "Eligibility", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.Gender:
                                    return RedirectToAction("GenderProcessWork", "Eligibility", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.OOA:
                                    return RedirectToAction("OOAProcessWork", "OST", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.SCC:
                                    return RedirectToAction("SCCProcessWork", "OST", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                                case (long)DiscripancyCategory.TRR:
                                    return RedirectToAction("TRRProcessWork", "OST", new { queueId = strEncodedCaseId, pageName = ConstantTexts.MiimPageName });
                            }
                            #endregion

                        }
                        else
                        {
                            MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, "Record is locked by User Id :" + objRecordsLocked.CreatedByRef, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                            ViewBag.ErrorMessage = "\nRecord is Locked by " + objRecordsLocked.CreatedByName + " and Not Available for Processing";
                            return View("~/Views/Shared/Error.cshtml");
                        }

                    }
                    else if (result == ExceptionTypes.UnknownError)
                    {
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMUserAuthFail, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                        ViewBag.ErrorMessage = "\nYou are not part of ERS DB.\nPlease contact administrator";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    else 
                    {
                        MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ConstantTexts.MIIMUserAuthFail, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                        ViewBag.ErrorMessage = "\nYou do not have permission to access this page.\nPlease contact administrator";
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, "Invalid ERS case Id :" + caseId, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                ViewBag.ErrorMessage = "\nInvalid Case Id.";
                return View("~/Views/Shared/Error.cshtml");
            }
            catch (Exception ex)
            {
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, caseId, ex.ToString(), _currentLoginUserId, TarceMethodLkup.Failed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.Message);
                return View("~/Views/Shared/Error.cshtml");
            }
        }
        public ActionResult CreateRPRCase(string memberFName = "", string memberLName = "", string memberDOB = "", string hICN = "", string mIIMReferenceId = "")
        {
            ViewBag.PageName = ConstantTexts.MiimPageName;
            try
            {
                string requestData = "memberFName : " + memberFName + ",memberLName : " + memberLName + ",memberDOB : " + memberDOB + ",string hICN : " + hICN + ",string mIIMReferenceId : " + mIIMReferenceId;

                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, requestData, ConstantTexts.MIIMNew, _currentLoginUserId, TarceMethodLkup.New.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                #region Create Session
                ExceptionTypes result = CreateSession(WorkBasket.RPR);
                #endregion

                if (result == ExceptionTypes.Success)
                {
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, requestData, ConstantTexts.MIIMUserAuthSucc, _currentLoginUserId, TarceMethodLkup.InProgress.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                    DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
                    #region Dropdowns Binding
                    List<DOADM_UserMaster> lstUsers;
                    DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                    objDOADM_UserMaster.IsActive = true;
                    BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                    long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
                    result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out string errorMessage);
                    objDOGEN_Queue.ComplianceStartDate = DateTime.UtcNow;
                    objDOGEN_Queue.DiscrepancyStartDate = objDOGEN_Queue.ComplianceStartDate.Value.AddMonths(1);
                    objDOGEN_Queue.DiscrepancyStartDate = new DateTime(objDOGEN_Queue.DiscrepancyStartDate.Value.Year, objDOGEN_Queue.DiscrepancyStartDate.Value.Month, 1);
                    objDOGEN_Queue.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();//Filtered 1st three Users as Admin.sort list by email id
                    objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                    objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, (long)WorkBasket.RPR);
                    objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                    objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                    objDOGEN_Queue.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                    objDOGEN_Queue.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                    objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, (long)DiscripancyCategory.RPR);
                    #endregion

                    objDOGEN_Queue.MemberFirstName = memberFName;
                    objDOGEN_Queue.MemberLastName = memberLName;
                    objDOGEN_Queue.MIIMReferenceId = mIIMReferenceId;
                    bool isDobCorrect = DateTime.TryParse(memberDOB, out DateTime dtMemberDob);
                    if (isDobCorrect)
                        objDOGEN_Queue.MemberDOB = dtMemberDob.AddDays(1);
                    objDOGEN_Queue.MemberCurrentHICN = hICN;
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, requestData, ConstantTexts.MIIMRequestCompleted, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                    return View("~/Views/RPR/Create.cshtml", objDOGEN_Queue);
                }
                else if (result == ExceptionTypes.UnknownError)
                {
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, requestData, ConstantTexts.MIIMUserAuthFail, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                    ViewBag.ErrorMessage = "\nYou are not part of ERS DB.\nPlease contact administrator";
                    return View("~/Views/Shared/Error.cshtml");
                }
                else
                {
                    MIIMServiceLog(MethodBase.GetCurrentMethod().Name, requestData, ConstantTexts.MIIMUserAuthFail, _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                    ViewBag.ErrorMessage = "\nYou do not have permission to access this page.\nPlease contact administrator";
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            catch (Exception ex)
            {
                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, "", ex.ToString(), _currentLoginUserId, TarceMethodLkup.Failed.ToLong(), (long)MIIMServiceMethod.CreateRPRCase);
                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.Message);
                return View("~/Views/Shared/Error.cshtml");
            }
        }

        private ExceptionTypes CreateSession(WorkBasket objWorkBasket)
        {
            ExceptionTypes result = ExceptionTypes.UnauthorizedAccessException;
            string[] strLoginName = System.Web.HttpContext.Current.User.Identity.Name.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries);
            string domain = strLoginName[0];
            string loginName = strLoginName[1];
            string errorMessage = string.Empty;

            try
            {
                if (IsUserInADGroup())
                {
                    if (Session[ConstantTexts.CurrentUserSessionKey] == null)
                    {
                        BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                        UserAdminstrationController objUserAdminstrationController = new UserAdminstrationController();

                        //check user access
                        result = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out UIUserLogin loggedInUser);
                        if (result == ExceptionTypes.ZeroRecords)
                        {
                            //insert new User to DB, get details from LDAP
                            bool isUserFound = objUserAdminstrationController.GetUserDetails(loginName, true, out DOADM_UserMaster objDOADM_UserMaster);
                            if (isUserFound)
                            {
                                objDOADM_UserMaster.CreatedByRef = _currentLoginUserId;
                                objDOADM_UserMaster.MSID = loginName;
                                objDOADM_UserMaster.StartDate = DateTime.UtcNow.AddYears(-1);
                                objDOADM_UserMaster.EndDate = DateTime.UtcNow.AddYears(30);
                                objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation = new List<DOADM_AccessGroupUserCorrelation>();
                                //AddAccessGroups(objDOADM_UserMaster);
                                result = objBLUserAdministration.SaveUser(objDOADM_UserMaster, out errorMessage);
                            }
                            else
                            {
                                //user details not found in LDAP
                                MIIMServiceLog(MethodBase.GetCurrentMethod().Name, "", "Session Creation failed for new User. LDAP details not found", _currentLoginUserId, TarceMethodLkup.Completed.ToLong(), (long)MIIMServiceMethod.GetCaseDetails);
                                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, "Session Creartion failed, LDAP Details not found", "CREATE session failed");
                                return ExceptionTypes.UnknownError;
                            }
                            result = objBLUserAdministration.GetUserAccessPermission(loginName, null, null, null, out loggedInUser);
                        }
                        else if (result != ExceptionTypes.Success)
                        {
                            BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, string.Empty, "Error while fetching logged in user data");
                            return result;
                        }

                        // Not adding access groups to MIIM user, so they should not be able to access ERS web application.
                        //adding access groups if no access groups assigned 
                        //if (loggedInUser.UserSkills == null || loggedInUser.UserSkills.Count <= 0)
                        //{
                        //    DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                        //    objDOADM_UserMaster.IsActive = true;
                        //    objDOADM_UserMaster.MSID = loginName;
                        //    objDOADM_UserMaster = objUserAdminstrationController.GetUserSearchResult(objDOADM_UserMaster, out errorMessage).FirstOrDefault();
                        //    AddAccessGroups(objDOADM_UserMaster);
                        //    objBLUserAdministration.SaveUser(objDOADM_UserMaster, out errorMessage);
                        //}

                        _currentLoginUserId = loggedInUser.ADM_UserMasterId;
                        loggedInUser.WorkBasketLkup = (long)objWorkBasket;
                        loggedInUser.BusinessSegmentLkup = (long)BusinessSegment.MNR;
                        loggedInUser.RoleLkup = (long)RoleLkup.Processor;

                        //login user
                        result = objBLUserAdministration.LoginUser(loginName);
                        if (result != (long)ExceptionTypes.Success)
                        {
                            BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, string.Empty, "Error while fetching logged in user data");
                        }
                        //create session
                        Session[ConstantTexts.CurrentUserSessionKey] = loggedInUser;
                        return ExceptionTypes.Success;
                    }
                    else
                    {
                        UIUserLogin loggedInUser = (UIUserLogin)Session[ConstantTexts.CurrentUserSessionKey];
                        _currentLoginUserId = loggedInUser.ADM_UserMasterId;
                        //pick current user object and check access group is assigned
                        //check user has access to AD group
                        return ExceptionTypes.Success;
                    }
                }
                else
                    return ExceptionTypes.UnauthorizedAccessException;
            }
            catch(Exception ex)
            {
                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.Message);
                return ExceptionTypes.UnknownError;
            }
        }

        private void AddAccessGroups(DOADM_UserMaster objDOADM_UserMaster)
        {
            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            BLAccessGroup objBLAccessGroup = new BLAccessGroup();

            //get List of Access groups to be added
            List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster;
            objBLAccessGroup.GetAccessGroupBasedOnSearch(TimeZone,new DOADM_AccessGroupMaster {  AccessGroupName = ConstantTexts.MIIMAccessGroupSearchKey, IsActive = true }, out lstDOADM_AccessGroupMaster);

            if (lstDOADM_AccessGroupMaster.Count > 0)
            {
                foreach (DOADM_AccessGroupMaster objDOADM_AccessGroupMaster in lstDOADM_AccessGroupMaster)
                {
                    DOADM_AccessGroupUserCorrelation objDOADM_AccessGroupUserCorrelation = new DOADM_AccessGroupUserCorrelation();
                    objDOADM_AccessGroupUserCorrelation.IsActive = true;
                    objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef = objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId;
                    objDOADM_AccessGroupUserCorrelation.ADM_UserMasterRef = 0;
                    objDOADM_AccessGroupUserCorrelation.CreatedByRef = _currentLoginUserId;
                    objDOADM_AccessGroupUserCorrelation.UTCCreatedOn = DateTime.UtcNow;
                    objDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = _currentLoginUserId;
                    objDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = DateTime.UtcNow;
                    objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation.Add(objDOADM_AccessGroupUserCorrelation);
                }
            }
        }

        private bool IsUserInADGroup()
        {
            try
            {
                System.Security.Principal.WindowsIdentity winIdnt = System.Web.HttpContext.Current.User.Identity as System.Security.Principal.WindowsIdentity;
                System.Security.Principal.IdentityReferenceCollection grps = winIdnt.Groups;
                string SIDs = WebConfigData.MIIMSID;
                string[] ADGroupSIDs = SIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                bool isUserInADGroup = (from g in grps
                                        where ADGroupSIDs.Any(s => s == g.Value)
                                        select g).Any();
                return isUserInADGroup;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.Message);
                return false;
            }
        }

        private void MIIMServiceLog(string name, string requestInputData, string responseMsg, long userid, long traceLkup, long WebServiceMethodLkup)
        {
            DOGEN_MIIMServiceTrace objDOGEN_MIIMServiceTrace = new DOGEN_MIIMServiceTrace();
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            try
            {
                objDOGEN_MIIMServiceTrace.WebServiceMethodName = name;
                objDOGEN_MIIMServiceTrace.RequestInputData = requestInputData;
                objDOGEN_MIIMServiceTrace.ResponseStatusMessage = responseMsg;
                objDOGEN_MIIMServiceTrace.CreatedByRef = userid;
                objDOGEN_MIIMServiceTrace.TarceMethodLkup = traceLkup;
                objDOGEN_MIIMServiceTrace.WebServiceMethodLkup = WebServiceMethodLkup;
                objBLServiceRequestResponse.MIIMServiceLog(objDOGEN_MIIMServiceTrace);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(_currentLoginUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MIIMConnector, (long)ExceptionTypes.Uncategorized, ex.ToString(), "Error occured while MIIMServiceLog.");
            }
        }
    }
}