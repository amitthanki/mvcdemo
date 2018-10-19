using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using ENRLReconSystem.Common;
using System.Web.Routing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Formatting;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Http.Headers;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    [ValidateInput(false)]
    public class CommonController : Controller
    {
        private UIUserLogin currentUser;
        private BLReports _objBLReports;

        private List<long> objpendingQueues = Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList();
        private List<long> objOSTHoldingQueues = Enum.GetValues(typeof(OSTHoldingQueues)).Cast<long>().ToList();

        
        BLOST _objBLOST = new BLOST();
        BLRPR _objBLRPR = new BLRPR();

        /// <summary>
        /// Constructor to assign currentUser instance.
        /// </summary>
        public CommonController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDOGEN_Queue"></param>
        /// <returns></returns>
        public ActionResult UpdateCaseInfo(DOGEN_Queue objDOGEN_Queue)
        {
            BLCommon objBLCommon = new BLCommon();
            string errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            BLCommon objCommon = new BLCommon();
            try
            {
                objDOGEN_Queue.LoginUserId = currentUser.ADM_UserMasterId;
                objDOGEN_Queue.ActionPerformedLkup = (long)ActionLookup.Save;//save 
                objDOGEN_Queue.RoleLkup = currentUser.RoleLkup;

               
                if (!objCommon.ValidateLockBeforeSave(currentUser.ADM_UserMasterId, (long)ScreenType.Queue, objDOGEN_Queue.GEN_QueueId.ToLong()))
                {
                    errorMessage = "Record not locked";
                    result = ExceptionTypes.UnlockedRecord;
                    return Json(new { ID = result, Message = errorMessage });
                }



                result = objBLCommon.UpdateCaseInfo(objDOGEN_Queue, out errorMessage);
                if (result != (long)ExceptionTypes.Success && errorMessage != string.Empty)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = 0, Message = "An error occured while updating DB.", Gen_QueueId = objDOGEN_Queue.GEN_QueueId });
                }
                returnMessage = "Case updated successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b>";
                return Json(new { ID = 1, Message = returnMessage, Gen_QueueId = objDOGEN_Queue.GEN_QueueId });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = 0, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
            }
        }


        /// <summary>
        /// To clone existing record to create new record.
        /// </summary>
        /// <param name="Id">QueueId</param>
        /// <param name="data">discCatId:OOA,SCC,TRR,DOB,Gender,Eligibility,RPR</param>
        /// <returns>ActionResult</returns>
        public ActionResult CloneRecord(long GenQueueId, long discCategoryID)
        {
            try
            {
                DOGEN_Queue objDOGEN_Queue = PGetProcessWork(GenQueueId, discCategoryID);
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                objDOGEN_Queue.IsClosedAndCreateNew = true;
                long workBasketLkup;
                string errorMessage = string.Empty;
                if (discCategoryID == (long)DiscripancyCategory.OOA)
                {
                    ViewBag.caseType = "Create OOA Case";
                }
                else if (discCategoryID == (long)DiscripancyCategory.SCC)
                {
                    ViewBag.caseType = "Create SCC Case";
                }
                else if (discCategoryID == (long)DiscripancyCategory.TRR)
                {
                    ViewBag.caseType = "Create TRR Case";
                }
                else if (discCategoryID == (long)DiscripancyCategory.Eligibility)
                {
                    ViewBag.caseType = "Create Eligibility Case";
                }
                else if (discCategoryID == (long)DiscripancyCategory.DOB)
                {
                    ViewBag.caseType = "Create DOB Case";
                }
                else if (discCategoryID == (long)DiscripancyCategory.Gender)
                {
                    ViewBag.caseType = "Create Gender Case";
                }
                switch (discCategoryID)
                {
                    case (long)DiscripancyCategory.OOA:
                    case (long)DiscripancyCategory.SCC:
                    case (long)DiscripancyCategory.TRR:
                        workBasketLkup = (long)WorkBasket.OST;
                        objDOGEN_Queue.DiscrepancyCategoryLkup = discCategoryID;
                        objDOGEN_Queue.DiscrepancySourceLkup = DiscrepancySource.SingleCaseCreation.ToInt64();
                        objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkup);
                        objDOGEN_Queue.lstDiscSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscrepancySource);
                        objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discCategoryID);
                        objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                        objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_Queue.lstSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SourceSystem);
                        objDOGEN_Queue.lstMemberVerifiedState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                        objDOGEN_Queue.lstTransactionReplyCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionReplyCode);
                        objDOGEN_Queue.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        return View("~/Views/OST/Create.cshtml", objDOGEN_Queue);
                    case (long)DiscripancyCategory.Eligibility:
                    case (long)DiscripancyCategory.DOB:
                    case (long)DiscripancyCategory.Gender:
                        workBasketLkup = (long)WorkBasket.GPSvsMMR;
                        objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkup);
                        objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discCategoryID);
                        objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                        objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                        objDOGEN_Queue.DiscrepancyCategoryLkup = discCategoryID;
                        objDOGEN_Queue.lstOOAFlag = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        objDOGEN_Queue.lstGender = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Gender);
                        return View("~/Views/Eligibility/Create.cshtml", objDOGEN_Queue);
                    case (long)DiscripancyCategory.RPR:
                        workBasketLkup = (long)WorkBasket.RPR;
                        ExceptionTypes result;
                        long CategoryType = (long)DiscripancyCategory.RPR;
                        List<DOADM_UserMaster> lstUsers;
                        DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                        objDOADM_UserMaster.IsActive = true;
                        BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                        long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                        result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                        objDOGEN_Queue.ComplianceStartDate = DateTime.UtcNow;
                        objDOGEN_Queue.DiscrepancyStartDate = objDOGEN_Queue.ComplianceStartDate.Value.AddMonths(1);
                        objDOGEN_Queue.DiscrepancyStartDate = new DateTime(objDOGEN_Queue.DiscrepancyStartDate.Value.Year, objDOGEN_Queue.DiscrepancyStartDate.Value.Month, 1);
                        objDOGEN_Queue.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();//Filtered 1st three Users as Admin.sort list by email id
                        objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                        objDOGEN_Queue.lstLob = objDOGEN_Queue.lstLob.Where(xx => xx.CMN_LookupMasterId != 31004).ToList();
                        objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkup);
                        objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_Queue.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                        objDOGEN_Queue.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                        objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, CategoryType);
                        return View("~/Views/RPR/Create.cshtml", objDOGEN_Queue);
                    default:
                        return View("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Invalid discrepancycategory while clone the record." });
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        public ActionResult ViewRecord(long Id, long data)
        {
            try
            {
                BLCommon objBLCommon = new BLCommon();
                ViewBag.PageName = "Search";
                ViewBag.ViewName = "View";
                switch (data)
                {
                    case (long)DiscripancyCategory.OOA: return View("~/Views/OST/OOAProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.SCC: return View("~/Views/OST/SCCProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.TRR: return View("~/Views/OST/TRRProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.Eligibility: return View("~/Views/Eligibility/EligibilityProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.DOB: return View("~/Views/Eligibility/DOBProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.Gender: return View("~/Views/Eligibility/GenderProcessWork.cshtml", PGetProcessWork(Id, data));
                    case (long)DiscripancyCategory.RPR: return View("~/Views/RPR/RPRProcessWork.cshtml", PGetProcessWork(Id, data));
                    default: return View("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Invalid discrepancycategory while view the record." });
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        /// <summary>
        /// To check user permission to respective action
        /// </summary>
        /// <param name="Id">permissionType:Eg-Clone,View,Save,Modify</param>
        /// <param name="data">discCatId:OOA,SCC,TRR,DOB,Gender,Eligibility,RPR</param>
        /// <returns>bool:true/false;</returns>
        public JsonResult CheckUserPermission(long Id, long data, long WorkQueue = 0)
        {
            return new JsonResult { Data = CacheUtility.CheckUserPermission(currentUser, Id, data, WorkQueue) };
        }
        /// <summary>
        /// To retrive GEN_Queue object based on GEN_QueueId
        /// </summary>
        /// <param name="genQueueID"></param>
        /// <returns>DOGEN_Queue</returns>
        private DOGEN_Queue PGetProcessWork(long genQueueID, long data)
        {
            BLCommon objBLCommon = new BLCommon();
            DOGEN_ManageCases objDOGEN_ManageCases = new DOGEN_ManageCases();
            objDOGEN_ManageCases.GEN_QueueRef = genQueueID;
            objDOGEN_ManageCases.ActionPerformedLkup = (long)ActionLookup.View;
            objDOGEN_ManageCases.CurrentUserRef = currentUser.ADM_UserMasterId;
            objDOGEN_ManageCases.CreatedByRef = currentUser.ADM_UserMasterId;
            objBLCommon.InsertManageCase(objDOGEN_ManageCases, out string ErrorMessage);
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            ExceptionTypes result = ExceptionTypes.UnknownError;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string errorMessage = string.Empty;
            try
            {
                switch (data)
                {
                    case (long)DiscripancyCategory.OOA:
                    case (long)DiscripancyCategory.SCC:
                    case (long)DiscripancyCategory.TRR:
                     
                        result = _objBLOST.GetGenQueueByID(TimeZone, genQueueID, out objDOGEN_Queue, out errorMessage);
                        if (objDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAMIIMUpdated || objDOGEN_Queue.MostRecentWorkQueueLkup == (long)SCCQueue.SCCMIIMUpdated)
                        {
                            ViewBag.MiimComment = objDOGEN_Queue.lstDOGEN_Comments.OrderByDescending(x => x.UTCCreatedOn).Where(x => x.SourceSystemLkup == (long)SourceSystemLkup.MIIM).FirstOrDefault()?.Comments ?? string.Empty;
                        }
                        break;
                    case (long)DiscripancyCategory.Eligibility:
                    case (long)DiscripancyCategory.DOB:
                    case (long)DiscripancyCategory.Gender:
                        BLEligibility _objBLEligibility = new BLEligibility();
                        result = _objBLEligibility.GetGenQueueByID(TimeZone, genQueueID, out objDOGEN_Queue);
                        break;
                    case (long)DiscripancyCategory.RPR:
                        BLRPR _objBLRPR = new BLRPR();
                        result = _objBLRPR.GetGenQueueByID(TimeZone, genQueueID, out objDOGEN_Queue, out errorMessage);
                        break;
                }
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                objDOGEN_Queue.lstActions = new List<DOCMN_LookupMasterCorrelations>();
                objDOGEN_Queue.lstOptionsforReopen = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Reopen);
                #region service Call
                //objDOGEN_GPSServiceRequestParameter.ERSCaseId = objDOGEN_Queue.GEN_QueueId.NullToString();
                //objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                //if (!objDOGEN_Queue.MemberCurrentHICN.IsNull())
                //{
                //    objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = objDOGEN_Queue.MemberCurrentHICN;
                //    //web service call for gps data
                //    objGPSServiceGetMethods.GetMemberDemographicalDetails(objDOGEN_GPSServiceRequestParameter, out objDOGEN_GPSData, out errorMessage);
                //    if (errorMessage.IsNullOrEmpty())
                //    {
                //        objDOGEN_GPSData.HICN = objDOGEN_Queue.MemberCurrentHICN.Trim();
                //        objDOGEN_Queue.objDOGEN_GPSData = objDOGEN_GPSData;
                //    }
                //    else
                //        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                //}
                //if (!objDOGEN_GPSData.IndividualId.IsNull())
                //{
                //    objDOGEN_GPSServiceRequestParameter.IndividualId = objDOGEN_GPSData.IndividualId.NullToString();
                //    //web service call for TRR data
                //    objGPSServiceGetMethods.GetTRRSummaryInfoService(objDOGEN_GPSServiceRequestParameter, ref objDOGEN_Queue, out errorMessage);
                //}
                #endregion
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Used to check user search permission
        /// </summary>
        /// <returns></returns>
        public JsonResult CheckSearchPermission()
        {
            bool canSearch = false;
            try
            {
                if (currentUser.WorkBasketLkup == (long)WorkBasket.OST)
                {
                    canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.OOA);
                    if (!canSearch)
                        canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.SCC);
                    if (!canSearch)
                        canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.TRR);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.GPSvsMMR)
                {
                    canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.Eligibility);
                    if (!canSearch)
                        canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.DOB);
                    if (!canSearch)
                        canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.Gender);
                }
                else if (currentUser.WorkBasketLkup == (long)WorkBasket.RPR)
                {
                    canSearch = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanSearch, (long)DiscripancyCategory.RPR);
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return new JsonResult { Data = canSearch };

        }
        //public JsonResult CheckCanReopenPermission()
        //{
        //    bool canReopen = false;
        //    try
        //    {
        //        if (currentUser.WorkBasketLkup == (long)WorkBasket.OST)
        //        {
        //            canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.OOA);
        //            if (!canReopen)
        //                canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.SCC);
        //            if (!canReopen)
        //                canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.TRR);
        //        }
        //        else if (currentUser.WorkBasketLkup == (long)WorkBasket.Eligibility)
        //        {
        //            canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.Eligibility);
        //            if (!canReopen)
        //                canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.DOB);
        //            if (!canReopen)
        //                canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.Gender);
        //        }
        //        else if (currentUser.WorkBasketLkup == (long)WorkBasket.RPR)
        //        {
        //            canReopen = CacheUtility.CheckUserPermission(currentUser, (long)PermissionType.CanReopen, (long)DiscripancyCategory.RPR);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
        //    }
        //    return new JsonResult { Data = canReopen };

        //}

        /// <summary>
        /// EditRecordVisibility
        /// </summary>
        /// <param name="lstSearchResults"></param>
        private void EditRecordVisibility(ref List<SearchResults> lstSearchResults)
        {
            foreach (var item in lstSearchResults)
            {
                item.EncryptedCaseID = URLEncoderDecoder.Encode(item.WorkItemID.NullToString());

                if (CheckEditButtonVisibility(item.QueueProgressTypeLkup, item.MostRecentQueueLkup, item.AssignedToRef, item.PendedByRef,item.OOALetterStatusLkup, item.CMSTransactionStatusLkup))
                    item.EditActionVisibility = true;

            }
        }
        /// <summary>
        /// EditButtonVisibility
        /// </summary>
        /// <param name="QueueProgressTypeLkup"></param>
        /// <param name="MostRecentQueueLkup"></param>
        /// <param name="assignToRef"></param>
        /// <param name="OOALetterStatusLkup"></param>
        /// <param name="CMSTransactionStatusLkup"></param>
        /// <returns></returns>
        public bool CheckEditButtonVisibility(long? QueueProgressTypeLkup, long? MostRecentQueueLkup, long? assignToRef,long? pendedByref, long? OOALetterStatusLkup, long? CMSTransactionStatusLkup)
        {
            try
            {
                if (QueueProgressTypeLkup == QueueProgressType.Processing.ToInt64() && !(objpendingQueues.Contains(MostRecentQueueLkup.ToInt64())) 
                                                                                    && !(OOALetterStatusLkup.ToLong() == 53001 && MostRecentQueueLkup.ToInt64() == 10007)
                                                                                    && CMSTransactionStatusLkup.ToLong() != 55001 
                                                                                    && (assignToRef.IsNull() || assignToRef == currentUser.ADM_UserMasterId))
                    return true;
                else if (QueueProgressTypeLkup == QueueProgressType.Processing.ToInt64() && (objpendingQueues.Contains(MostRecentQueueLkup.ToInt64()) && pendedByref == currentUser.ADM_UserMasterId))//Queue in Proceesing and pending pending
                    return true;
                else if (QueueProgressTypeLkup == QueueProgressType.Holding.ToInt64() && objOSTHoldingQueues.Contains(MostRecentQueueLkup.ToInt64()))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
       
        }
        public ActionResult Search(DiscripancyCategory discCat)
        {
            try
            {
                ViewBag.SearchPage = "Search";
                ViewBag.AdvSearchPage = "Search Results";
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                LoadDropDownData((long)discCat);
                UISearch objUISearch = new UISearch();
                objUISearch.SearchCriteria = new SearchCriteria();
                objUISearch.SearchCriteria.DiscrepancyCategoryLkup = (long)discCat;
                objUISearch.SearchCriteria.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                objUISearch.SearchPanel = new List<SearchResults>();
                objUISearch.SearchCriteria.IsSearchScreen = true;
                ViewBag.TotalCount = "";
                return View("~/Views/Shared/Search.cshtml", objUISearch);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Search, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                throw;
            }

        }
        /// <summary>
        /// Load search Partialview on Search Click
        /// </summary>
        /// <param name="objSearchCriteria"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Search(SearchCriteria objSearchCriteria)
        {
            LoadDropDownData();
            UISearch objUISearch = new UISearch();
            string totalCount = string.Empty;
            string errorMessege = string.Empty;

            try
            {
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;

                long rowCount = CacheUtility.GetMasterConfigurationByName(ConstantTexts.NoOfGridRows).ToInt64();
                if (objSearchCriteria.WorkItemId == null &&
                    objSearchCriteria.DiscrepancyStartDate == DateTime.MinValue &&
                    objSearchCriteria.DiscrepancyEndDate == DateTime.MinValue &&
                    objSearchCriteria.CurrentHICN == null &&
                    objSearchCriteria.DiscrepancyCategoryLkup == null &&
                    objSearchCriteria.DiscrepancyTypeLkup == null
                    )
                {
                    objUISearch.SearchCriteria = new SearchCriteria();
                    objUISearch.SearchPanel = new List<SearchResults>();
                    ViewBag.Error = "To perform a search, you must enter one of the following:\n - Work ItemID\n - DiscrepancyStartDate\n - DiscrepancyEndDate\n - CurrentHICN \n - DiscrepancyCategory \n - DiscrepancyType";
                }
                else
                {
                    List<SearchResults> lstSearchResults;
                    objSearchCriteria.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                    objSearchCriteria.WorkBasketLkup = currentUser.WorkBasketLkup;
                    objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
                    objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
                    objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
                    objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
                    objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
                    objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
                    objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
                    objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
                    objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
                    objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
                    objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
                    objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
                    objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
                    objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
                    objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
                    objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                    objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                    objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
                    objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
                    objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
                    objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
                    objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
                    objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
                    objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
                    objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
                    objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
                    objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
                    objSearchCriteria.DisenrollmentFromDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentFromDate);
                    objSearchCriteria.DisenrollmentToDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentToDate);
                    objSearchCriteria.IsRestricted = currentUser.IsRestrictedUser;                  
                    long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                    BLCommon objBLCommon = new BLCommon();
                    ExceptionTypes result = objBLCommon.SearchRecords(TimeZone, (long)currentUser.WorkBasketLkup, currentUser.ADM_UserMasterId, rowCount, objSearchCriteria, out lstSearchResults, out totalCount,out errorMessege);
                    if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Search, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessege);
                        return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance."+ errorMessege });
                    }
                    ViewBag.TotalCount = totalCount;
                    objUISearch.SearchCriteria = objSearchCriteria;

                    if (lstSearchResults != null && lstSearchResults.Count > 0)
                    {
                        ///Edit Button Visibility Condition///
                        EditRecordVisibility(ref lstSearchResults);
                        ////////
                        objUISearch.SearchPanel = lstSearchResults;
                    }
                    else
                        objUISearch.SearchPanel = new List<SearchResults>();
                }
                objUISearch.SearchCriteria.IsSearchScreen = true;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Search, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
            }
            return PartialView("~/Views/Shared/_SearchResultsPanel.cshtml", objUISearch.SearchPanel);
        }

        #region Search Methods from Home
        /// <summary>
        /// Used to show search page from home page, on click of each count.
        /// </summary>
        /// <param name="objSearchCriteria">SearchCriteria</param>
        /// <param name="discCatId">long</param>
        /// <returns></returns>
        public ActionResult SearchFromHome(SearchCriteria id, long data, long screenType = 0)
        {
            try
            {
                string totalCount = string.Empty;
                long rowCount = CacheUtility.GetMasterConfigurationByName(ConstantTexts.NoOfGridRows).ToInt64();
                ViewBag.CurrentWorkBasket = currentUser.WorkBasketLkup;
                string errorMessage = string.Empty;

                ViewBag.pageName = "Home";
                ViewBag.screenType = screenType;
                ViewBag.SearchPage = "Search";
                ViewBag.AdvSearchPage = "Search Results";
                SearchCriteria objSearchCriteria = id;
                LoadDropDownData(data);
                UISearch objUISearch = new UISearch();
                List<SearchResults> lstSearchResults;

                objSearchCriteria.DiscrepancyCategoryLkup = data;
                objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
                objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
                objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
                objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
                objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
                objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
                objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
                objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
                objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
                objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
                objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
                objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
                objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
                objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
                objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
                objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
                objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
                objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
                objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
                objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
                objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
                objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
                objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
                objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
                objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
                objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
                objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
                objSearchCriteria.WorkBasketLkup = (long)currentUser.WorkBasketLkup;
                objSearchCriteria.IsRestricted = currentUser.IsRestrictedUser;
                objSearchCriteria.BusinessSegmentLkup = currentUser.BusinessSegmentLkup;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.SearchRecords(TimeZone, (long)currentUser.WorkBasketLkup, currentUser.ADM_UserMasterId, rowCount, objSearchCriteria, out lstSearchResults, out totalCount,out errorMessage);
                if (result != (long)ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Search, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:Error occured while fetching search records." });
                }
                ViewBag.TotalCount = totalCount;

                objUISearch.SearchCriteria = objSearchCriteria;
                if (lstSearchResults != null && lstSearchResults.Count > 0)
                {
                    ///Edit Button Visibility Condition///
                    EditRecordVisibility(ref lstSearchResults);
                    ////////
                    objUISearch.SearchPanel = lstSearchResults.Where(x => x.DiscrepancyCategoryLkup.Equals(data)).ToList();
                }
                else
                    objUISearch.SearchPanel = new List<SearchResults>();

                objUISearch.SearchCriteria.IsSearchScreen = true;
                return View("~/Views/Shared/Search.cshtml", objUISearch);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Search, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }
        }

        #endregion

        //public ActionResult GPSHouseholdIDSearch(string GPSHouseHoldID)
        //{
        //    try
        //    {
        //        List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
        //        lstDOGEN_GPSData = GetMemberInfoByHouseHoldID(GPSHouseHoldID).ToList();
        //        if (lstDOGEN_GPSData.Count()>1)
        //        {
        //            return PartialView("_GPSHouseHoldIDPopUp", lstDOGEN_GPSData);
        //        }
        //        else
        //        {
        //            return Json(new { GPSData = lstDOGEN_GPSData, flag = "Json" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
        //        return Json("");
        //    }
        //}


        /// <summary>
        /// Used to load all drop down default data.
        /// </summary>
        private void LoadDropDownData(long discCat = 0)
        {
            try
            {
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                objDOADM_UserMaster.IsActive = true;
                List<DOADM_UserMaster> lstUsers = new List<DOADM_UserMaster>();
                string errorMessage;
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                ExceptionTypes result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error while retriving users from Data base");
                }


                //Loading Disccategory as per Business Segment
                List<DOCMN_LookupMaster> lstDOCMN_LMDiscCat = new List<DOCMN_LookupMaster>();
                List<DOCMN_LookupMaster> lstDiscripancyCategory = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscripancyCategory);
                var lookupCorrelations = CacheUtility.GetAllLookupMasterCorrelationFromCache(null, (long)currentUser.WorkBasketLkup).ToList();
                lookupCorrelations.Select(x => x.CMN_LookupMasterChildRef).Distinct().ToList().ForEach(CMN_LookupMasterChildRef =>
                {
                    if (CMN_LookupMasterChildRef > 0)
                    {
                        lstDOCMN_LMDiscCat.Add(lstDiscripancyCategory.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault());
                    }
                });
                ViewBag.DiscrepancyCategory = lstDOCMN_LMDiscCat;

                //Loading Discrepancy Type as per Discrepancy Category
                List<DOCMN_LookupMaster> lstDOCMN_LMDiscCatType = new List<DOCMN_LookupMaster>();
                List<DOCMN_LookupMaster> lstDiscripancyType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscripancyType);
                // lstDOCMN_LMDiscCatType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discCat);
                lstDOCMN_LMDiscCat.Select(x => x.CMN_LookupMasterId).Distinct().ToList().ForEach(CMN_LookupMasterId =>
                {
                    lookupCorrelations = CacheUtility.GetAllLookupMasterCorrelationFromCache(null, CMN_LookupMasterId).ToList();
                    lookupCorrelations.Select(x => x.CMN_LookupMasterChildRef).Distinct().ToList().ForEach(CMN_LookupMasterChildRef =>
                    {
                        if (CMN_LookupMasterChildRef > 0)
                        {
                            var item = lstDiscripancyType.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault();
                            if (item != null && (lstDOCMN_LMDiscCatType.Contains(item) == false))
                                lstDOCMN_LMDiscCatType.Add(lstDiscripancyType.Where(x => x.CMN_LookupMasterId.Equals(CMN_LookupMasterChildRef)).FirstOrDefault());
                        }
                    });
                });
                ViewBag.DiscrepancyType = lstDOCMN_LMDiscCatType;


                //Loading Queues as per Discrepancy Category
                List<DOCMN_LookupMasterCorrelations> lstDOCMN_LMTest = new List<DOCMN_LookupMasterCorrelations>();
                List<DOCMN_LookupMaster> lstQueue = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Queue);
                lstDOCMN_LMTest = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, discCat).ToList();
                ViewBag.Queue = lstDOCMN_LMTest;

                ViewBag.DiscrepancySource = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscrepancySource);
                ViewBag.LOB = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                ViewBag.ContractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                ViewBag.PBP = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                ViewBag.RPRActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                ViewBag.FDRStatus = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRStatus);
                ViewBag.SubmissionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SubmissionType);
                ViewBag.PendReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PendReason);
                ViewBag.Resolution = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Resolution);
                ViewBag.Status = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Status);
                ViewBag.Gender = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Gender);
                ViewBag.RPREGHPMember = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                ViewBag.VerifiedRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RootCause);
                ViewBag.VerifiedState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                ViewBag.lstTransactionReplyCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionReplyCode);
                ViewBag.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                ViewBag.PerformcedTaskList = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                if (lstUsers != null && lstUsers.Count() > 0)
                {
                    lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000).OrderBy(a => a.FullName).ToList();
                    ViewBag.LastUpdatedOperator = lstUsers;
                    ViewBag.AssignedTo = lstUsers;
                    ViewBag.SupervisiorList = lstUsers.Where(x => x.IsManager == true).OrderBy(a => a.FullName).ToList();
                    ViewBag.RPRRequestor = lstUsers;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult LoadQueue(long categoryID)
        {

            List<DOCMN_LookupMasterCorrelations> lstQueue = new List<DOCMN_LookupMasterCorrelations>();
            try
            {
                lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, categoryID).Where(xx => xx.IsActive == true).ToList();
                return Json(lstQueue, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.MassUpdate, (long)ExceptionTypes.Uncategorized, ex.ToString(), "Error:" + ex.ToString());
            }
            return null;
        }

        /// <summary>
        /// Used to convert date into long dateId value.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private long ConvertDateToLong(DateTime? date)
        {
            long retVal = 0;
            try
            {
                if (date != null && date != DateTime.MinValue)
                {
                    string strId = date.Value.ToString("yyyyMMdd");
                    if (long.TryParse(strId, out retVal) == false)
                    {
                        retVal = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        
        // Get Common report URL
        [HttpPost]
        public ActionResult GetCommonReportURL(long? reportId=null)
        {
            _objBLReports = new BLReports();
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            try
            {
                if (reportId.IsNull())
                {
                    reportId= (long)ReportId.CommonHistoryReport;
                }

                ExceptionTypes resultReports = _objBLReports.GetAllReports(reportId, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
                var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
                return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });

            }
            catch (Exception)
            {

                throw;
            }
   
        }

        [HttpPost]
        public ActionResult GetSearchQueueReport(SearchCriteria objSearchCriteria)
        {
            objSearchCriteria.DiscrepancyStartDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyStartDate);
            objSearchCriteria.DiscrepancyEndDateId = ConvertDateToLong(objSearchCriteria.DiscrepancyEndDate);
            objSearchCriteria.DOBId = ConvertDateToLong(objSearchCriteria.DOB);
            objSearchCriteria.FirstLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailStartDate);
            objSearchCriteria.FirstLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.FirstLetterMailEndDate);
            objSearchCriteria.SecondLetterMailStartDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailStartDate);
            objSearchCriteria.SecondLetterMailEndDateId = ConvertDateToLong(objSearchCriteria.SecondLetterMailEndDate);
            objSearchCriteria.ComplianceStartDateId = ConvertDateToLong(objSearchCriteria.ComplianceStartDate);
            objSearchCriteria.ComplianceEndDateId = ConvertDateToLong(objSearchCriteria.ComplianceEndDate);
            objSearchCriteria.CaseCreationStartDateId = ConvertDateToLong(objSearchCriteria.CaseCreationStartDate);
            objSearchCriteria.CaseCreationEndDateId = ConvertDateToLong(objSearchCriteria.CaseCreationEndDate);
            objSearchCriteria.LastUpdatedStartDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedStartDate);
            objSearchCriteria.LastUpdatedEndDateId = ConvertDateToLong(objSearchCriteria.LastUpdatedEndDate);
            objSearchCriteria.MemberResponseVerificationStartDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationStartDate);
            objSearchCriteria.MemberResponseVerificationEndDateId = ConvertDateToLong(objSearchCriteria.MemberResponseVerificationEndDate);
            objSearchCriteria.RequestedEffectiveStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
            objSearchCriteria.RequestedEffectiveEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
            objSearchCriteria.PotentionSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveStartDate);
            objSearchCriteria.PotentionSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RequestedEffectiveEndDate);
            objSearchCriteria.AdjustedCreateStartDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateStartDate);
            objSearchCriteria.AdjustedCreateEndDateId = ConvertDateToLong(objSearchCriteria.AdjustedCreateEndDate);
            objSearchCriteria.RPCSubmissionStartDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionStartDate);
            objSearchCriteria.RPCSubmissionEndDateId = ConvertDateToLong(objSearchCriteria.RPCSubmissionEndDate);
            objSearchCriteria.CMSAccountManagerApprovalStartDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalStartDate);
            objSearchCriteria.CMSAccountManagerApprovalEndDateId = ConvertDateToLong(objSearchCriteria.CMSAccountManagerApprovalEndDate);
            objSearchCriteria.FDRReceivedStartDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedStartDate);
            objSearchCriteria.FDRReceivedEndDateId = ConvertDateToLong(objSearchCriteria.FDRReceivedEndDate);
            objSearchCriteria.PeerAuditCompletionStartDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionStartDate);
            objSearchCriteria.PeerAuditCompletionEndDateId = ConvertDateToLong(objSearchCriteria.PeerAuditCompletionEndDate);
            objSearchCriteria.DisenrollmentFromDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentFromDate);
            objSearchCriteria.DisenrollmentToDateId = ConvertDateToLong(objSearchCriteria.DisenrollmentToDate);
            objSearchCriteria.EmployerGroupNumber = objSearchCriteria.EmployerGroupNumber;          
            _objBLReports = new BLReports();
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.GetQueueSearchReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new
            {
                Data = urlData,
                LastUpdatedStartDateId = objSearchCriteria.LastUpdatedStartDateId,
                LastUpdatedEndDateId = objSearchCriteria.LastUpdatedEndDateId,
                MemberResponseVerificationStartDateId = objSearchCriteria.MemberResponseVerificationStartDateId,
                MemberResponseVerificationEndDateId = objSearchCriteria.MemberResponseVerificationEndDateId,
                RequestedEffectiveStartDateId = objSearchCriteria.RequestedEffectiveStartDateId,
                RequestedEffectiveEndDateId = objSearchCriteria.RequestedEffectiveEndDateId,
                PotentionSubmissionStartDateId = objSearchCriteria.PotentionSubmissionStartDateId,
                PotentionSubmissionEndDateId = objSearchCriteria.PotentionSubmissionEndDateId,
                AdjustedCreateStartDateId = objSearchCriteria.AdjustedCreateStartDateId,
                AdjustedCreateEndDateId = objSearchCriteria.AdjustedCreateEndDateId,
                RPCSubmissionStartDateId = objSearchCriteria.RPCSubmissionStartDateId,
                RPCSubmissionEndDateId = objSearchCriteria.RPCSubmissionEndDateId,
                CMSAccountManagerApprovalStartDateId = objSearchCriteria.CMSAccountManagerApprovalStartDateId,
                CMSAccountManagerApprovalEndDateId = objSearchCriteria.CMSAccountManagerApprovalEndDateId,
                FDRReceivedStartDateId = objSearchCriteria.FDRReceivedStartDateId,
                FDRReceivedEndDateId = objSearchCriteria.FDRReceivedEndDateId,
                PeerAuditCompletionStartDateId = objSearchCriteria.PeerAuditCompletionStartDateId,
                PeerAuditCompletionEndDateId = objSearchCriteria.PeerAuditCompletionEndDateId,
                WorkBasketLkup = currentUser.WorkBasketLkup,
                DiscrepancyStartDateId = objSearchCriteria.DiscrepancyStartDateId,
                DiscrepancyEndDateId = objSearchCriteria.DiscrepancyEndDateId,
                CaseCreationStartDateId = objSearchCriteria.CaseCreationStartDateId,
                CaseCreationEndDateId = objSearchCriteria.CaseCreationEndDateId,
                FirstLetterMailStartDateId = objSearchCriteria.FirstLetterMailStartDateId,
                FirstLetterMailEndDateId = objSearchCriteria.FirstLetterMailEndDateId,
                DOBId = objSearchCriteria.DOB,
                SecondLetterMailStartDateId = objSearchCriteria.SecondLetterMailStartDateId,
                SecondLetterMailEndDateId = objSearchCriteria.SecondLetterMailEndDateId,
                ComplianceStartDateId = objSearchCriteria.ComplianceStartDateId,
                ComplianceEndDateId = objSearchCriteria.ComplianceEndDateId,
                IsRestricted = currentUser.IsRestrictedUser,
                GPSHouseholdId = objSearchCriteria.GPSHouseholdID,
                PDPAutoEnrolleeInd = objSearchCriteria.PDPAutoEnrolleeInd,
                DisenrollmentFromDateId = objSearchCriteria.DisenrollmentFromDateId,
                DisenrollmentToDateId = objSearchCriteria.DisenrollmentToDateId,
                EmployerGroupNumber = objSearchCriteria.EmployerGroupNumber,
                JsonRequestBehavior.AllowGet
            });
        }
        [HttpPost]
        public JsonResult CheckReopenCase(long GenQueueId)
        {
            long mmostrecentStatus = 0;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            BLCommon objBLCommon = new BLCommon();
            ExceptionTypes result = objBLCommon.GetReferenceCase(GenQueueId, out objDOGEN_Queue);
            if (objDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Count > 0)
            {
                var queuid = objDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Where(xx => xx.ParentQueueRef == GenQueueId && xx.MostRecentStatusLkup != (long)CurrentStatusLkup.ResolvedComplted).FirstOrDefault();
                if (queuid != null)
                {
                    mmostrecentStatus = 1;
                    //mmostrecentStatus = queuid.ReferenceMOstQueueStatus;
                }
                else
                {
                    mmostrecentStatus = 0;
                }
            }
            return Json(new { Data = mmostrecentStatus });
        }

        /// <summary>
        /// Get Discrepancy and TRR data  based on Tab clicked
        /// </summary>
        /// <param name="GEN_QueueId"></param>
        /// <param name="TabType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDiscrepancyOrTRRDataByTab(string GEN_QueueId, string TabType)
        {
            string erMessage = string.Empty;
            string errorMessage = string.Empty;
            string errorMessage1 = string.Empty;
            string actionViewName = string.Empty;

            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
    
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;

            actionViewName = (TabType == "discrepancydata") ? "_DiscrepancyData" : "_TRRData";

            try
            {
                ExceptionTypes result = _objBLOST.GetGenQueueByID(TimeZone, Convert.ToInt64(GEN_QueueId), out objDOGEN_Queue, out erMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, erMessage);
                }

                objDOGEN_GPSServiceRequestParameter.ERSCaseId = objDOGEN_Queue.GEN_QueueId.NullToString();
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objDOGEN_GPSServiceRequestParameter.HouseholdId = objDOGEN_Queue.GPSHouseholdID;
                if (!objDOGEN_Queue.MemberCurrentHICN.IsNull())
                {
                    objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = objDOGEN_Queue.MemberCurrentHICN;
                    //web service call for gps data
                    List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
                    //string GetHouseHoldID = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetHouseHoldID);
                    //string apiUrl = GetHouseHoldID + objDOGEN_GPSServiceRequestParameter.HouseholdId + "&UserID=" + currentUser.ADM_UserMasterId;
                    //var task = GETCallWebAPI<DOGEN_GPSData>(apiUrl);
                    //lstDOGEN_GPSData = await task; 
                    objGPSServiceGetMethods.GetMemberEligibilityService(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                    if (errorMessage.IsNullOrEmpty())
                    {
                        if (lstDOGEN_GPSData.Count > 1)
                        {
                            objDOGEN_GPSData = lstDOGEN_GPSData.Where(x => x.HouseholdId == objDOGEN_GPSServiceRequestParameter.HouseholdId).FirstOrDefault();
                        }
                        else
                        {
                            objDOGEN_GPSData = lstDOGEN_GPSData.FirstOrDefault();
                        }

                        if (objDOGEN_GPSData.IsNull())
                        {
                            objDOGEN_GPSData = new DOGEN_GPSData();
                        }

                        objDOGEN_GPSData.HICN = objDOGEN_Queue.MemberCurrentHICN.Trim();
                        objDOGEN_Queue.objDOGEN_GPSData = objDOGEN_GPSData;
                    }
                    else
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                if (!objDOGEN_GPSData.IndividualId.IsNull() && TabType == "TrrData")
                {
                    objDOGEN_GPSServiceRequestParameter.IndividualId = objDOGEN_GPSData.IndividualId.NullToString();
                    //string GetTRRData = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetTRRData);
                    //string apiUrl1 = GetTRRData + objDOGEN_GPSServiceRequestParameter.IndividualId + "&UserID=" + currentUser.ADM_UserMasterId;
                    //objDOGEN_Queue = (DOGEN_Queue)GETCallWebAPIByClass<DOGEN_Queue>(apiUrl1);

                    //web service call for TRR data
                     objGPSServiceGetMethods.GetTRRSummaryInfoService(objDOGEN_GPSServiceRequestParameter, ref objDOGEN_Queue, out errorMessage1);
                    if (!errorMessage.IsNullOrEmpty())
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage1);
                    }

                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return RedirectToAction("Maintenance", "Error", new { Error = MethodBase.GetCurrentMethod().Name + " Action terminated and redirected to Maintenance. Error:" + ex.ToString() });
            }

            return PartialView(actionViewName, objDOGEN_Queue);
        }

        public ActionResult CheckDuplicateForReopen(long queueId)
        {
            string errorMessage = string.Empty;
            long existingCaseID = 0;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            List<SearchResults> lstSearchResults = new List<SearchResults>();
            string strMessage = string.Empty;
            long lCaseID = 0;
            try
            {
                ExceptionTypes result = _objBLOST.GetGenQueueByID(TimeZone, Convert.ToInt64(queueId), out objDOGEN_Queue, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = -1, Message = "An error occured." });

                }

                if (PCheckDuplicateLogic(objDOGEN_Queue, out lstSearchResults, out lCaseID, out errorMessage))
                {
                    if (objDOGEN_Queue.WorkBasketLkup == WorkBasket.OST.ToInt64() || objDOGEN_Queue.WorkBasketLkup == WorkBasket.GPSvsMMR.ToInt64())
                    {
                        long? caseId = lstSearchResults.FirstOrDefault().WorkItemID.Value;
                        if (caseId.ToInt64() > 0)
                            existingCaseID = caseId.ToInt64();
                    }
                    else
                    {
                        if(lCaseID>0)
                        existingCaseID = lCaseID;
                    }
                }
                return Json(new { ID = existingCaseID, Message = strMessage });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = -1, Message = "An error occured." });
            }
        }

        private bool PCheckDuplicateLogic(DOGEN_Queue objDOGEN_Queue, out List<SearchResults> lstSearchResults,out long lCaseID, out string errorMessage)
        {
            bool isDuplicate = false;
            ExceptionTypes result = new ExceptionTypes();
            BLCommon objBLCommon = new BLCommon();
            lstSearchResults = new List<SearchResults>();
            errorMessage = string.Empty;
            string totalCount = string.Empty;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            SearchCriteria objSearchCriteria=null;
            lCaseID = 0;
            try
            {
                if (objDOGEN_Queue.WorkBasketLkup == WorkBasket.OST.ToInt64())
                {
                    objSearchCriteria = new SearchCriteria()
                    {
                        CurrentHICN = objDOGEN_Queue.MemberCurrentHICN,
                        ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup,
                        PBPLkup = objDOGEN_Queue.MemberPBPLkup,
                        WorkBasketLkup = objDOGEN_Queue.WorkBasketLkup,
                        DiscrepancyTypeLkup = objDOGEN_Queue.DiscrepancyTypeLkup,
                        DiscrepancyCategoryLkup = objDOGEN_Queue.DiscrepancyCategoryLkup,
                        StatusNot = (long)CurrentStatusLkup.ResolvedComplted
                    };
                }
                else if (objDOGEN_Queue.WorkBasketLkup == WorkBasket.GPSvsMMR.ToInt64())
                {
                    objSearchCriteria = new SearchCriteria()
                    {
                        CurrentHICN = objDOGEN_Queue.MemberCurrentHICN,
                        DiscrepancyStartDate = objDOGEN_Queue.DiscrepancyStartDate,
                        DiscrepancyCategoryLkup = objDOGEN_Queue.DiscrepancyCategoryLkup,
                        WorkBasketLkup = objDOGEN_Queue.WorkBasketLkup,
                        DiscrepancyTypeLkup = objDOGEN_Queue.DiscrepancyTypeLkup,
                        StatusNot = (long)CurrentStatusLkup.ResolvedComplted
                    };
                }

                if (objDOGEN_Queue.WorkBasketLkup != WorkBasket.RPR.ToInt64())
                {
                    result = objBLCommon.SearchRecords(TimeZone, (long)currentUser.WorkBasketLkup, 0, ConstantTexts.DefaultCount, objSearchCriteria, out lstSearchResults, out totalCount, out errorMessage);
                    if (lstSearchResults.Count > 0 && objDOGEN_Queue.GEN_QueueId.ToInt64() > 0)
                    {
                        if (lstSearchResults.Where(xx => xx.WorkItemID != objDOGEN_Queue.GEN_QueueId.ToInt64()).Count() > 0)
                        {
                            var resItem = lstSearchResults.Where(xx => xx.WorkItemID == objDOGEN_Queue.GEN_QueueId.ToInt64()).FirstOrDefault();
                            lstSearchResults.Remove(resItem);
                            isDuplicate = true;
                        }

                    }
                    else if (lstSearchResults.Count > 0 && objDOGEN_Queue.GEN_QueueId.ToInt64() == 0)
                    {
                        isDuplicate = true;
                    }

                }
                else {
                    result = _objBLRPR.CheckDuplicate(objDOGEN_Queue.MemberCurrentHICN, objDOGEN_Queue.MemberContractIDLkup, objDOGEN_Queue.RPRRequestedEffectiveDate, objDOGEN_Queue.RPRActionRequestedLkup, out lCaseID);
                    if (objDOGEN_Queue.GEN_QueueId.ToInt64() > 0 && lCaseID == objDOGEN_Queue.GEN_QueueId)//While Cloning Check for Duplicate
                    {
                        isDuplicate = false;
                    }
                    else if (lCaseID > 0)
                    {
                        isDuplicate = true;

                    }
                }
                return isDuplicate;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtpStartDate"></param>
        /// <param name="dtpEndDate"></param>
        /// <param name="queueLkup"></param>
        /// <param name="queueIdToSkip"></param>
        /// <param name="DiscrepancyCategory"></param>
        /// <returns></returns>
        public ActionResult GMURecordCommon(long queueLkup, long? queueIdToSkip,DateTime StartDate, DateTime EndDate)
        {
            BLQueueSummary objBLQueueSummary = new BLQueueSummary();
            BLCommon objCommon = new BLCommon();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            string strErrorMessage = string.Empty;
            DateTime dtpStartDate = StartDate;
            DateTime dtpEndDate = EndDate;
            bool isForcedLock = false;
            try
            {
                if (queueIdToSkip > 0)
                {
                    ExceptionTypes exceptionResult = objCommon.UnlockRecord((long)ScreenType.Queue, (long)queueIdToSkip);
                    if (exceptionResult != ExceptionTypes.Success)
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, strErrorMessage, strErrorMessage);
                        return Json(new { ID = ExceptionTypes.UnknownError, Gen_QueuId = "", Message = "An error occoured" });
                    }
                }

                int lockCount = 1;
                do
                {
                    ExceptionTypes dbresult = objBLQueueSummary.GetGMURecord(dtpStartDate, dtpEndDate, (long)currentUser.BusinessSegmentLkup, queueLkup, queueIdToSkip, currentUser.ADM_UserMasterId, currentUser.IsRestrictedUser, out objDOGEN_Queue, out strErrorMessage);
                    if (dbresult == ExceptionTypes.Success)
                    {
                        //Locking  the record.
                        if (objDOGEN_Queue.LockedByRef == currentUser.ADM_UserMasterId)
                            isForcedLock = true;
                        UIRecordsLock objRecordsLocked;
                        ExceptionTypes exceptionResult = objCommon.GetLockedRecordOrLockRecord(currentUser.ADM_UserMasterId, (long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId, false, out objRecordsLocked);

                        if (exceptionResult == (long)ExceptionTypes.Success && objRecordsLocked != null)
                        {
                            if (objRecordsLocked.CreatedByRef != currentUser.ADM_UserMasterId)
                            {
                                lockCount = lockCount + 1;
                                continue;
                            }

                            lockCount = 11;
                        }
                        else
                        {
                            lockCount = lockCount + 1;
                        }
                    }
                    else
                    {
                        lockCount = lockCount + 1;
                    }
                } while (lockCount <= 10);

                string queueId = (!objDOGEN_Queue.GEN_QueueId.IsNull() && objDOGEN_Queue.GEN_QueueId > 0) ? URLEncoderDecoder.Encode(objDOGEN_Queue.GEN_QueueId.NullToString()) : string.Empty;
                return Json(new { ID = ExceptionTypes.Success, Gen_QueuId = queueId, Message = "Success" });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTGetQueue, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = ExceptionTypes.UnknownError, Gen_QueuId = "", Message = "An error occoured" });
            }
        }
        /// <summary>
        /// Just a ping to refresh the session
        /// </summary>
        /// <returns></returns>
        public JsonResult DoRefreshSession()
        {
            try
            {
                return Json(new { ID = ExceptionTypes.Success });
            }
            catch (Exception)
            {
                return Json(new { ID = ExceptionTypes.UnknownError });
            }
    
        }

        public ExceptionTypes IsOnshoreOnly(string HouseholdId, out bool isRestricted,out string employerId)
        {
            isRestricted = false;
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.HouseholdId = HouseholdId;
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            string errorMessage = string.Empty;
            employerId = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                //string GetEmployerSummary = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetEmployerSummary);
                //string apiUrl1 = GetEmployerSummary + objDOGEN_GPSServiceRequestParameter.HouseholdId + "&UserID=" + currentUser.ADM_UserMasterId;
                //objDOGEN_GPSData = (DOGEN_GPSData)GETCallWebAPIByClass<DOGEN_GPSData>(apiUrl1);
                ExceptionTypes result = objGPSServiceGetMethods.GetEmployerSummary(objDOGEN_GPSServiceRequestParameter, out objDOGEN_GPSData, out errorMessage);
                if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    isRestricted = false;
                    return result;
                }
                if (!objDOGEN_GPSData.EmployerId.IsNullOrEmpty())
                {
                    if (objDOGEN_GPSData.EmployerCloseDate.HasValue && objDOGEN_GPSData.EmployerCloseDate < DateTime.UtcNow)
                    {
                        isRestricted = false;
                    }
                    else
                    {
                        List<DOCMN_LookupMaster> lstEmployers = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.OnshoreOnlyEmployers);
                        isRestricted = lstEmployers.Exists(x => x.LookupValue == objDOGEN_GPSData.EmployerId);
                        employerId = objDOGEN_GPSData.EmployerId;
                    }
                }
                return ExceptionTypes.Success;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return ExceptionTypes.UnknownError;
            }
        }

        public bool ClearTempdataByGenId(string genQueueId)
        {
            bool issuccess = false;
            try
            {
                if (!TempData[genQueueId].IsNull())
                {
                    TempData[genQueueId] = null;
                }
                return issuccess;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, "Error while clearing temp data by genId", ex.Message);
                throw;
            }
        }
     

    
        /// <summary>
        /// OST,Eligibility,RPR Create case Page population from Eligibility service
        /// </summary>
        /// <param name="uID"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public ActionResult PersonSearch(string uID,string flag)
        {
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            try
            {
                if (flag == "MBI")
                {                    
                    //string GetMBI = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetMBI);
                    //string apiUrl = GetMBI + uID + "&UserID=" + currentUser.ADM_UserMasterId;
                    //var task = GETCallWebAPI<DOGEN_GPSData>(apiUrl);
                    //lstDOGEN_GPSData = await task;
                     lstDOGEN_GPSData = PPersonSearch(uID).ToList();


                    if (!lstDOGEN_GPSData.IsNull() && lstDOGEN_GPSData.Count() > 1)
                    {
                        return PartialView("_GPSHouseHoldIDPopUp", lstDOGEN_GPSData);
                    }
                    else if (!lstDOGEN_GPSData.IsNull() && lstDOGEN_GPSData.Count() == 1)
                    {
                        //string GetHouseHoldID = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetHouseHoldID);
                        //string apiUrl1 = GetHouseHoldID + lstDOGEN_GPSData[0].HouseholdId + "&UserID=" + currentUser.ADM_UserMasterId;
                        //var task1 = GETCallWebAPI<DOGEN_GPSData>(apiUrl1);
                        //lstDOGEN_GPSData = await task1;
                        //return Json(new { GPSData = lstDOGEN_GPSData, flag = "Json" }, JsonRequestBehavior.AllowGet);
                        return Json(new { GPSData = GetMemberInfoByHouseHoldID(lstDOGEN_GPSData[0].HouseholdId).ToList(), flag = "Json" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if(flag == "HHID" || flag=="MHHID" )
                {                    
                    //string GetHouseHoldID = CacheUtility.GetMasterConfigurationByName(ConstantTexts.GetHouseHoldID);
                    //string apiUrl2 = GetHouseHoldID + uID + "&UserID=" + currentUser.ADM_UserMasterId;
                    //var task3 = GETCallWebAPI<DOGEN_GPSData>(apiUrl2);
                    //lstDOGEN_GPSData = await task3;
                     lstDOGEN_GPSData = GetMemberInfoByHouseHoldID(uID).ToList();
                }

                return Json(new { GPSData = lstDOGEN_GPSData, flag = "Json" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { GPSData = lstDOGEN_GPSData, flag = "Json" }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hICNNumber"></param>
        /// <returns></returns>
        private List<DOGEN_GPSData> PPersonSearch(string hICNNumber)
        {
            string errorMessage = string.Empty;
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = hICNNumber;
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objGPSServiceGetMethods.GetMemberDemographicalDetails(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return lstDOGEN_GPSData;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }

        }
        /// <summary>
        /// GetMemberInfoByHouseHoldID
        /// </summary>
        /// <param name="gpsHouseholdId"></param>
        /// <returns></returns>
        private List<DOGEN_GPSData> GetMemberInfoByHouseHoldID(string gpsHouseholdId)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.HouseholdId = gpsHouseholdId;
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            string errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objGPSServiceGetMethods.GetMemberEligibilityService(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return lstDOGEN_GPSData;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="employerID"></param>
        /// <returns></returns>
        public bool IsOOAEGHPExclusion(string employerID)
        {
            bool isExist = false;
            // DB Operation
            try
            {
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.IsOOAEGHPExclusion(employerID);
                if (result != (long)ExceptionTypes.Success)
                {
                    isExist = false;
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error while retriving users from Data base");
                }
                else
                {
                    isExist = true;
                }
            }
            catch(Exception ex)
            {
                isExist = false;
            }
            return isExist;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ContractNumber"></param>
        /// <param name="PBP"></param>
        /// <param name="EmployerID"></param>
        /// <returns></returns>
        public bool IsNationalEmployerForRestriction(string contractNumber,string PBP,string employerID)
        {
            bool isExist = false;
            // DB Operation
            try
            {
                BLCommon objBLCommon = new BLCommon();
                ExceptionTypes result = objBLCommon.IsNationalEmployerForRestriction(contractNumber,PBP, employerID);
                if (result != (long)ExceptionTypes.Success)
                {
                    isExist = false;
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Common, (long)ExceptionTypes.Uncategorized, "Error while retriving users from Data base", "Error while retriving users from Data base");
                }
                else
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                isExist = false;
            }
            return isExist;
        }

        [HttpPost]
        public ActionResult GetPBP(long ContractNumber)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
            return Json(objDOGEN_Queue.lstPbpid);
        }



        ///////Common Method for Calling WEB API for AE Service////////////////////////////

       public static async Task<List<T>> GETCallWebAPI<T>(string URL)
        {
         
            List<T> items = null;
            string apiUrl = URL;
            //HttpClientHandler authHandler = new HttpClientHandler()
            //{
            //    Credentials = System.Net.CredentialCache.DefaultNetworkCredentials
            //};
            try
            {
                X509Certificate2 clientCert = GetClientCertificate();
                WebRequestHandler requestHandler = new WebRequestHandler();
                requestHandler.ClientCertificates.Add(clientCert);
                requestHandler.UseDefaultCredentials = true;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;              
                HttpClient client = new HttpClient(requestHandler);
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = response.Content.ReadAsStringAsync()
                                           .Result
                                           .Replace("\\", "")
                                           .Trim(new char[1] { '"' });
                    items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonString);

                }
            }
            catch(Exception ex)
            {
                BLCommon.LogError(1, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
            return items;
        }      

       public static string POSTCallWebAPI(string URL,string Data)
        {
            HttpResponseMessage response = null;
            string responsemessage = string.Empty;
            try
            {
                
                X509Certificate2 clientCert = GetClientCertificate();
                WebRequestHandler requestHandler = new WebRequestHandler();
                requestHandler.ClientCertificates.Add(clientCert);
                requestHandler.UseDefaultCredentials = true;

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                using (HttpClient client = new HttpClient(requestHandler))
                {
                    client.BaseAddress = new Uri(URL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    System.Net.Http.Formatting.MediaTypeFormatter jsonFormatter = new System.Net.Http.Formatting.JsonMediaTypeFormatter();
                    var content = new StringContent(Data, Encoding.UTF8, "application/json");
                    response = client.PostAsync(URL, content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string dataobj = response.Content.ReadAsStringAsync().Result;
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic item = serializer.Deserialize<object>(dataobj);
                        if (item != null)
                        {
                            responsemessage = item["data"];

                        }
                        else
                        {
                            responsemessage = "Not unable to perform Transaction please contact administration";
                        }
                        // responsemessage = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(dataobj);
                    }
                   

                }
            }
            catch(Exception ex)
            {
               
                throw ex;
            }
            return responsemessage;
        }

        
        public static object GETCallWebAPIByClass<T>(string URL)
        {
            T obj = default(T);
            string jsonString = null;
            string apiUrl = URL;
            X509Certificate2 clientCert = GetClientCertificate();
            WebRequestHandler requestHandler = new WebRequestHandler();
            requestHandler.ClientCertificates.Add(clientCert);
            requestHandler.UseDefaultCredentials = true;

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpClient client = new HttpClient(requestHandler);          
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync()
                                       .Result
                                       .Replace("\\", "")
                                       .Trim(new char[1] { '"' });
                if (jsonString != "[]")
                {
                    obj = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
                }
                else
                {
                    obj = default(T);
                }

            }
            return obj;
        }

        private static X509Certificate2 GetClientCertificate()
        {
            //X509Store userCaStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            //userCaStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            X509Store userCaStore = new X509Store(StoreName.CertificateAuthority, StoreLocation.CurrentUser);

            try
            {
                userCaStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certificatesInStore = userCaStore.Certificates;
                X509Certificate2Collection findResult = certificatesInStore.Find(X509FindType.FindBySubjectName, "UnitedHealth Group Issuing CA3", true);
                X509Certificate2 clientCertificate = null;
                if (findResult.Count >= 1)
                {
                    clientCertificate = findResult[0];
                }
                else
                {
                    throw new Exception("Unable to locate the correct client certificate.");
                }
                return clientCertificate;
            }
            catch
            {

                throw;
            }
            finally
            {
                userCaStore.Close();
            }
        }

        public static DOGEN_Queue GETCallWebAPIByDOGEN_Queue(string URL)
        {
            DOGEN_Queue obj = null;
            string jsonString = null;
            string apiUrl = URL;
            HttpClientHandler authHandler = new HttpClientHandler()
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials
            };
            HttpClient client = new HttpClient(authHandler);
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync()
                                       .Result
                                       .Replace("\\", "")
                                       .Trim(new char[1] { '"' });
                if (jsonString != "[]")
                {
                    obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DOGEN_Queue>(jsonString);
                }
                else
                {
                    obj = new DOGEN_Queue();
                }

            }
            return obj;
        }

        public static DOGEN_GPSData GETCallWebAPIByDOGEN_GPSData(string URL)
        {
            DOGEN_GPSData obj = null;
            string jsonString = null;
            string apiUrl = URL;
            HttpClientHandler authHandler = new HttpClientHandler()
            {
                Credentials = System.Net.CredentialCache.DefaultNetworkCredentials
            };
            HttpClient client = new HttpClient(authHandler);
            client.BaseAddress = new Uri(apiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                jsonString = response.Content.ReadAsStringAsync()
                                       .Result
                                       .Replace("\\", "")
                                       .Trim(new char[1] { '"' });
                //var settings = new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore,
                //    MissingMemberHandling = MissingMemberHandling.Ignore
                //};
                //var jsonModel = JsonConvert.DeserializeObject<DOGEN_GPSData>(jsonString, settings);
                if (jsonString != "[]")
                {
                    obj = Newtonsoft.Json.JsonConvert.DeserializeObject<DOGEN_GPSData>(jsonString);
                }
                else
                {
                    obj = new DOGEN_GPSData();
                }

            }
            return obj;
        }
    }
}