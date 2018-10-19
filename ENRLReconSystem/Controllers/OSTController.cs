using ENRLReconSystem.BL;
using ENRLReconSystem.Common;
using ENRLReconSystem.DO;
using ENRLReconSystem.Models;
using ENRLReconSystem.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ENRLReconSystem.Controllers
{

    [ERSAuthenticationAttribute(Roles = "User")]
    [Common.Filter(WorkBasketLkup=(long)WorkBasket.OST)]
    [ValidateInput(false)]
    public class OSTController : Controller
    {
        private UIUserLogin currentUser;
        public OSTController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            else
                currentUser = new UIUserLogin();
        }
        BLOST _objBLOST = new BLOST();
        string errorMessage = string.Empty;
        private BLReports _objBLReports;
        USPSService _USPSService = new USPSService("641UNITE1062");
        /// <summary>
        /// Initial Load Create OOA Suspect Case
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateOOACase()
        {
            try
            {
                ViewBag.caseType = "Create OOA Case";
                return View("Create", PGetOSTCreateCase((long)DiscripancyCategory.OOA));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }

        }
        /// <summary>
        /// Initial Load Create TRR Case
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateTRRCase()
        {
            try
            {
                ViewBag.caseType = "Create TRR Case";
                return View("Create", PGetOSTCreateCase((long)DiscripancyCategory.TRR));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        /// <summary>
        /// Initial Load For Create SCC Case
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CreateSCCCase()
        {
            try
            {
                ViewBag.caseType = "Create SCC Case";
                return View("Create", PGetOSTCreateCase((long)DiscripancyCategory.SCC));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }

        }
        /// <summary>
        /// Update Case Info , From Process work screen
        /// </summary>
        /// <param name="discripancyCategory"></param>
        /// <param name="genQueueId"></param>
        /// <returns></returns>
        
        public ActionResult UpdateCaseInfo(long discripancyCategory,long genQueueId)
        {
            try
            {

                return PartialView("_UpdateOSTCaseInfo", PUpdateCaseInfo(discripancyCategory, genQueueId));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }

        }

        /// <summary>
        /// To Load OOA Process Work can be called from search page, Queue Summary
        /// </summary>
        /// <param name="queueId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public ActionResult OOAProcessWork(string queueId, string pageName = "")
        {
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                //Load PW screen based on GenQueue ID
                return View(PGetProcessWork(URLEncoderDecoder.Decode(queueId).ToInt64(), pageName));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        /// <summary>
        ///  To Load TRR Process Work can be called from search page, Queue Summary
        /// </summary>
        /// <param name="queueId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public ActionResult TRRProcessWork(string queueId, string pageName = "")
        {
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                return View(PGetProcessWork(URLEncoderDecoder.Decode(queueId).ToInt64(), pageName));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
        /// <summary>
        ///  To Load SCC Process Work can be called from search page, Queue Summary
        /// </summary>
        /// <param name="queueId"></param>
        /// <param name="pageName"></param>
        /// <returns></returns>
        public ActionResult SCCProcessWork(string queueId, string pageName = "")
        {
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                return View(PGetProcessWork(URLEncoderDecoder.Decode(queueId).ToInt64(), pageName));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }
        /// <summary>
        /// Save For OST(OOA,SCC,TRR) Cases 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveOSTCases(DOGEN_Queue objDOGEN_Queue)
        {
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            List<SearchResults> lstSearchResults = new List<SearchResults>();
            CommonController objCommonController = new CommonController();
            string employerId = string.Empty;
            try
            {
                long loginUserId = currentUser.ADM_UserMasterId;
                long workBasketLkp = currentUser.WorkBasketLkup.ToInt64();
                long businessSegmentLkup = currentUser.BusinessSegmentLkup.ToInt64();
                //check duplicate logic for case HICN, Contract # and PBP in discrepancy record is matched with the HICN, Contract# and PBP in the Case

                if (CheckDuplicateLogic(objDOGEN_Queue, out lstSearchResults, out errorMessage))
                {
                    long lCaseID = lstSearchResults.FirstOrDefault().WorkItemID.Value;
                    string strMessage = "Already case with <strong> ERS Case ID_" + lCaseID + " </strong> exists";
                    return Json(new { ID = (long)ExceptionTypes.DuplicateRecord, Message = strMessage, Gen_QueueId = 0 });
                }
                if (objDOGEN_Queue.GEN_QueueId > 0)
                {
                    objDOGEN_Queue.IsChildCase = true;
                    objDOGEN_Queue.ParentQueueRef = objDOGEN_Queue.GEN_QueueId;
                }

                objDOGEN_Queue.LoginUserId = loginUserId;
                objDOGEN_Queue.RoleLkup = currentUser.RoleLkup;
               // objDOGEN_Queue.BusinessSegmentLkup = businessSegmentLkup;
                objDOGEN_Queue.WorkBasketLkup = workBasketLkp;
                objDOGEN_Queue.IsParentCase = false;
                objDOGEN_Queue.IsChildCase = false;
                objDOGEN_Queue.EligOOAFlagLkup = false;
                objDOGEN_Queue.CommentsSourceSystemLkup = (long)SourceSystemLkup.ERS;
                objDOGEN_Queue.MemberDOBId = !(objDOGEN_Queue.MemberDOB.IsNull()) ? ((DateTime)objDOGEN_Queue.MemberDOB).ToString("yyyyMMdd").ToInt64() : (long?)null;
                objDOGEN_Queue.DiscrepancyReceiptDateId = !(objDOGEN_Queue.DiscrepancyReceiptDate.IsNull()) ? ((DateTime)objDOGEN_Queue.DiscrepancyReceiptDate).ToString("yyyyMMdd").ToInt64() : (long?)null;
                objDOGEN_Queue.ComplianceStartDateId = !(objDOGEN_Queue.ComplianceStartDate.IsNull()) ? ((DateTime)objDOGEN_Queue.ComplianceStartDate).ToString("yyyyMMdd").ToInt64() : (long?)null;
                objDOGEN_Queue.StartDateId = !(objDOGEN_Queue.StartDate.IsNull()) ? ((DateTime)objDOGEN_Queue.StartDate).ToString("yyyyMMdd").ToInt64() : (long?)null;
                objDOGEN_Queue.EndDateId = !(objDOGEN_Queue.EndDate.IsNull()) ? ((DateTime)objDOGEN_Queue.EndDate).ToString("yyyyMMdd").ToInt64() : (long?)null;
                if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA)
                {
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (long)QueueLookup.OOANewCase;
                    objDOGEN_Queue.CurrentWorkQueuesLkup = (long)QueueLookup.OOANewCase;
                }
                else if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.SCC)
                {
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (long)QueueLookup.SCCNewCase;
                    objDOGEN_Queue.CurrentWorkQueuesLkup = (long)QueueLookup.SCCNewCase;
                }
                else if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.TRR)
                {
                    long? trc = GetTransactionReplyCode(objDOGEN_Queue.TransactionReplyCode.ToInt64());
                    objDOGEN_Queue.MostRecentWorkQueueLkup = trc;
                    objDOGEN_Queue.CurrentWorkQueuesLkup = trc;
                }
                objDOGEN_Queue.MostRecentActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.MostRecentStatusLkup = (long)CurrentStatusLkup.New;
                objDOGEN_Queue.CurrentStatusLkup = (long)CurrentStatusLkup.New;
                objDOGEN_Queue.ActionLkup = (long)ActionLookup.Save;//to save
                objDOGEN_Queue.CurrentActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.ActionPerformedLkup = (long)ActionLookup.Save;//save      
                objDOGEN_Queue.IsActive = true;
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == objDOGEN_Queue.MemberContractIDLkup).ToList();
                string ContractNumber = objDOGEN_Queue.lstContractid.Select(x => x.LookupValue).FirstOrDefault();

                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == objDOGEN_Queue.MemberPBPLkup).ToList();
                string PBP = objDOGEN_Queue.lstPbpid.Select(x => x.LookupValue).FirstOrDefault();
                // objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == objDOGEN_Queue.MemberContractIDLkup).ToList();
                //objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.BusinessSegmentVsContractNumber, currentUser.BusinessSegmentLkup).Where(x => x.CMN_LookupMasterChildRef == objDOGEN_Queue.MemberContractIDLkup).ToList();               
                //  long ContractNumberID = objDOGEN_Queue.lstContractid.Select(x => x.LookupValue).FirstOrDefault();

                //List<DOCMN_LookupMaster> lst = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == ContractNumberID).ToList();
                // string ContractNumber = lst.Select(x => x.LookupValue).FirstOrDefault();

                // objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == objDOGEN_Queue.MemberPBPLkup).ToList();
                //objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.BusinessSegmentVsPBP, currentUser.BusinessSegmentLkup).Where(x => x.CMN_LookupMasterChildRef == objDOGEN_Queue.MemberPBPLkup).ToList();
                // long PBPID = objDOGEN_Queue.lstPbpid.Select(x => x.CMN_LookupMasterChildRef).FirstOrDefault();
                // List<DOCMN_LookupMaster> lst1 = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == PBPID).ToList();
                //  string PBP = lst1.Select(x => x.LookupValue).FirstOrDefault();
                //restricted case logic/EGHP Exclusion/National

                result = objCommonController.IsOnshoreOnly(objDOGEN_Queue.GPSHouseholdID, out bool isRetricted, out employerId);

                if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, "Error getting IsRestricted/EGHP/National vlaue for new Case", objDOGEN_Queue.GPSHouseholdID);
                objDOGEN_Queue.IsRestricted = isRetricted;

                if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA && objCommonController.IsOOAEGHPExclusion(employerId))
                {
                    objCommonController = null;
                    string strMessage = ConstantTexts.MsgForOOAEGHPExclusion;
                    return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = strMessage, Gen_QueueId = 0 });
                }
                // Restriction logic for contract number,PBP and Employer ID  member is associated with national plan.
                if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA && objCommonController.IsNationalEmployerForRestriction(ContractNumber, PBP, employerId))
                {
                    objCommonController = null;
                    string strMessage = ConstantTexts.MsgForOOANationalEmployer;
                    return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = strMessage, Gen_QueueId = 0 });
                }
                objCommonController = null;
                //
                result = _objBLOST.SaveOST(objDOGEN_Queue, out errorMessage);
                if (result != (long)ExceptionTypes.Success && objDOGEN_Queue.GEN_QueueId == 0)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
                }
                long issamebusinesssegment = 0;
                if (objDOGEN_Queue.BusinessSegmentLkup == (long)BusinessSegment.MNR)
                {
                    objDOGEN_Queue.BusinessSegment = "M & R";
                }
                else if(objDOGEN_Queue.BusinessSegmentLkup == (long)BusinessSegment.CNS)
                {
                    objDOGEN_Queue.BusinessSegment = "C & S";
                }
                else
                {
                    objDOGEN_Queue.BusinessSegment = "PCP";
                }
                // if Business Segment is same then display Confirm Message
                if (objDOGEN_Queue.IsRestricted && (objDOGEN_Queue.BusinessSegmentLkup == currentUser.BusinessSegmentLkup.ToLong()))
                {
                    issamebusinesssegment = 0;
                    returnMessage = "<b>Onshore Only Restricted </b> Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b></br> with Business segment : <b>" +  objDOGEN_Queue.BusinessSegment + "</b> Do you want to process it?";
                }
                else if (objDOGEN_Queue.IsRestricted && !(objDOGEN_Queue.BusinessSegmentLkup == currentUser.BusinessSegmentLkup.ToLong()))
                {
                    issamebusinesssegment = 1;
                    returnMessage = "<b>Onshore Only Restricted </b> Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" +  objDOGEN_Queue.BusinessSegment + "</b>";
                }
                else if (!objDOGEN_Queue.IsRestricted && (objDOGEN_Queue.BusinessSegmentLkup == currentUser.BusinessSegmentLkup.ToLong()))
                {
                    issamebusinesssegment = 0;
                    returnMessage = "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" +  objDOGEN_Queue.BusinessSegment + "</b> Do you want to process it?";
                }
                else
                {
                    issamebusinesssegment = 1;
                    returnMessage = "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" +  objDOGEN_Queue.BusinessSegment + "</b>";
                }
                //Save and return
                return Json(new { ID = result, Message = returnMessage, Gen_QueueId= objDOGEN_Queue.GEN_QueueId , businessSegmentLkup = objDOGEN_Queue.BusinessSegment , issamebusinesssegment = issamebusinesssegment });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
            }
        }

        /// <summary>
        /// All OST(OOA,SCC,TRR) Action Save 
        /// </summary>
        /// <param name="objDOGEN_OSTActions"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveOSTActions(DOGEN_OSTActions objDOGEN_OSTActions)
        {
            errorMessage = string.Empty;
            string errorMsg = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            string returnMessage = string.Empty;
            BLCommon objCommon = new BLCommon();
            try
            {
                DOGEN_Queue objPrevDOGEN_Queue = new DOGEN_Queue();
                string genQueueId = objDOGEN_OSTActions.GEN_QueueRef.NullToString();
                long loginUserId = currentUser.ADM_UserMasterId;
                long? roleLkup = currentUser.RoleLkup;
                if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Queue, objDOGEN_OSTActions.GEN_QueueRef.ToLong()))
                {
                    errorMessage = "Record not locked.";
                    result = ExceptionTypes.UnlockedRecord;
                    return Json(new { ID = result, Message = errorMessage });
                }
                //get the Previous data
                objPrevDOGEN_Queue = !(TempData[genQueueId].IsNull()) ? (DOGEN_Queue)TempData[genQueueId] : new DOGEN_Queue();
                TempData.Keep(genQueueId);
                //
                PUpdateOstAction(ref objDOGEN_OSTActions);//Reload with Default data From Temp memory
                objDOGEN_OSTActions.LastUpdatedByRef = loginUserId;
                objDOGEN_OSTActions.RoleLkup = roleLkup;
                returnMessage = "Record updated successfully.";
                //Check record is locked or not if not locked send messsage to UI


                //Check if the existing GPSHouseHoldId Is Modified
                if (!objPrevDOGEN_Queue.IsNullOrEmpty() && !objPrevDOGEN_Queue.GPSHouseholdID.IsNullOrEmpty() && !objDOGEN_OSTActions.GPSHouseholdID.IsNullOrEmpty()
                    && (objPrevDOGEN_Queue.GPSHouseholdID.Trim() != objDOGEN_OSTActions.GPSHouseholdID.Trim()))
                {
                    errorMessage = "Mismatch GPS Household ID from existing ERS Case ID.";
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = errorMessage, TrrCode = ExceptionTypes.UnknownError.ToInt32() });
                }
                //For CMSTransaction
                if (objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendSCCUpdatetoCMS || (objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendSCCDeletiontoCMS && objPrevDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.TRR))
                {
                    DOCMSPostTransaction(objDOGEN_OSTActions, out errorMessage);
                    objDOGEN_OSTActions.CMSTransactionStatusLkup = (long)CMSTransactionStatus.Success;
                    if (!errorMessage.IsNullOrEmpty())
                    {
                        string message = GetFormattedText(errorMessage);
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                        return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = message, TrrCode = CMSTransactionCode.TRR76.ToInt32() });
                    }
                }
                ////For Send OOA Letter In OOA Cases
                else if ((objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendOOALetter) || (objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendOOALetterandResDocCtyAttestRequired))
                {
                    string message = errorMsg;
                    if (objPrevDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.OOA)
                    {
                        objDOGEN_OSTActions.OOALetterStatusLkup = (long)OOALetterStatus.Success;
                        DOCMSPostTransaction(objDOGEN_OSTActions, out errorMsg);
                        if (!errorMsg.IsNullOrEmpty())
                        {
                            BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, errorMsg, errorMessage);
                            return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = errorMsg, TrrCode = ExceptionTypes.UnknownError.ToInt32() });
                        }
                    }
                    else
                    {//For EGHP exclusion in SCC and TRR cases
                        if (CheckEGHPOnSendOOALetterSCCOrTRR(objPrevDOGEN_Queue.GPSHouseholdID, out message))
                        {
                            return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = message, TrrCode = ExceptionTypes.UnknownError.ToInt32() });
                        }
                    }
                }
                //Save OST Actions
                result = _objBLOST.SaveOSTActions(objDOGEN_OSTActions, out errorMessage);


                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured.", TrrCode = "" });
                }
                else
                {
                    //Clear the temp data if success
                    if (!TempData[genQueueId].IsNull())
                        TempData[genQueueId] = null;
                }
                return Json(new { ID = result, Message = returnMessage });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = "An error occured.", TrrCode = "" });
            }
        }

        public ActionResult GetQueueForEdit(long DisCategory)
        {
            DOGEN_OSTActions objDOGEN_OSTActions = new DOGEN_OSTActions();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                string actionViewName = string.Empty;
                actionViewName = "_EditAndInitiateWorkflow";
                if (DisCategory == (long)DiscripancyCategory.OOA)
                {
                    objDOGEN_OSTActions.lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, DisCategory).Where(xx => xx.IsActive == true && xx.GroupingLookupMasterRef != QueueProgressType.Completed.ToInt64()).ToList();
                    objDOGEN_OSTActions.lstQueue = objDOGEN_OSTActions.lstQueue.Where(xx => !Enum.GetValues(typeof(ReOpenOOAHoldingQueue)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                }
                else
                {
                    objDOGEN_OSTActions.lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, DisCategory).Where(xx => xx.IsActive == true && xx.GroupingLookupMasterRef == QueueProgressType.Processing.ToInt64()).ToList();
                }

                //removing pended Queues
                objDOGEN_OSTActions.lstQueue = objDOGEN_OSTActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_OSTActions.lstQueue = objDOGEN_OSTActions.lstQueue.Where(xx => !Enum.GetValues(typeof(AuditFailedQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_OSTActions.lstQueue = objDOGEN_OSTActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingAudit)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_OSTActions.lstQueue = objDOGEN_OSTActions.lstQueue.Where(xx => !Enum.GetValues(typeof(MIIMUpdated)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();

                return PartialView(actionViewName, objDOGEN_OSTActions);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return PartialView("");
            }
        }

        /// <summary>
        /// Load All the partialviews
        /// </summary>
        /// <param name="actionID"></param>
        /// <returns></returns>
        public ActionResult GetActionsByActionID(PWActionsEnum actionID, long discipancyCategory, string source = "",string genQueueid="")
        {
            string actionViewName = string.Empty;       
            DOGEN_OSTActions objDOGEN_OSTActions = new DOGEN_OSTActions();
            objDOGEN_OSTActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
            DOGEN_Queue objPrevDOGEN_Queue = new DOGEN_Queue();
   
            #region Get SCCRPR Action Require Fields
            string errorMessage = string.Empty;
            List<DOADM_UserMaster> lstUsers;
            ExceptionTypes result;
            DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
            objDOADM_UserMaster.IsActive = true;
            BLUserAdministration objBLUserAdministration = new BLUserAdministration();
            #endregion

            try
            {
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                if (!genQueueid.IsNullOrEmpty())
                {
                    //Check the tempdata is null or not if Null force the user to cancel the operation
                    if (TempData[genQueueid].IsNull())
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, "Case was opened in Multiple windows", "Case was opened in Multiple windows");
                        return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = ConstantTexts.MsgForMultipleWindowOpenedCases });
                    }
                    //load the genQueue data to fill Action details
                    objPrevDOGEN_Queue = (DOGEN_Queue)TempData[genQueueid];
                    TempData.Keep(genQueueid);
                }
                if (actionID == PWActionsEnum.SendOOALetter
                 && (discipancyCategory == (long)DiscripancyCategory.SCC || discipancyCategory == (long)DiscripancyCategory.TRR) 
                 && source == "")
                {
                    if (CheckDuplicateWhileCloseAndCreate(objPrevDOGEN_Queue, out errorMessage))
                    {
                        return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = errorMessage });

                    }
                }
                switch (actionID)
                {
                 
                    case PWActionsEnum.SendSCCUpdatetoCMS:
                        objDOGEN_OSTActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_OSTActions.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_OSTActions.LastName = objPrevDOGEN_Queue.MemberLastName;
                        objDOGEN_OSTActions.PBPLkup = objPrevDOGEN_Queue.MemberPBPLkup;
                        objDOGEN_OSTActions.PBP = objPrevDOGEN_Queue.MemberPBP;
                        objDOGEN_OSTActions.ContractIDLkup = objPrevDOGEN_Queue.MemberContractIDLkup;
                        objDOGEN_OSTActions.ApplicationDate = objPrevDOGEN_Queue.ApplicationDate;                        
                        //if TRC code is 16 then effective date will be TimelineEffectiveDate from MQ TRR record this field we are capturing in Gen_Queue while we are processing MQ tRR record
                        if ((!(objPrevDOGEN_Queue.TransactionReplyCodeLkup.IsNull()) && objPrevDOGEN_Queue.TransactionReplyCodeLkup == TRCCode.TRC16.ToInt64()))
                        {
                            objDOGEN_OSTActions.EffectiveDate = objPrevDOGEN_Queue.TimelineEffectiveDate;
                        }
                        else
                        {
                            objDOGEN_OSTActions.EffectiveDate = DateTime.UtcNow.Date.AddMonths(1);
                            objDOGEN_OSTActions.EffectiveDate = new DateTime(objDOGEN_OSTActions.EffectiveDate.Value.Year, objDOGEN_OSTActions.EffectiveDate.Value.Month, 1);
                        }
                        objDOGEN_OSTActions.EndDate = objPrevDOGEN_Queue.EndDate;
                        objDOGEN_OSTActions.DateofBirth = objPrevDOGEN_Queue.MemberDOB;
                        actionViewName = "_SendSCCUpdateToCMS";
                        break;
                    case PWActionsEnum.SendSCCDeletiontoCMS:
                        DateTime today = DateTime.Today;
                        DateTime lastDayOfNextMonth = new DateTime(today.Year, today.Month, 1).AddMonths(2).AddDays(-1);
                        objDOGEN_OSTActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_OSTActions.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_OSTActions.LastName = objPrevDOGEN_Queue.MemberLastName;
                        objDOGEN_OSTActions.PBPLkup = objPrevDOGEN_Queue.MemberPBPLkup;
                        objDOGEN_OSTActions.PBP = objPrevDOGEN_Queue.MemberPBP;
                        objDOGEN_OSTActions.ContractIDLkup = objPrevDOGEN_Queue.MemberContractIDLkup;
                        objDOGEN_OSTActions.ApplicationDate = objPrevDOGEN_Queue.ApplicationDate;
                        if ((!(objPrevDOGEN_Queue.TransactionReplyCodeLkup.IsNull()) && objPrevDOGEN_Queue.TransactionReplyCodeLkup == TRCCode.TRC16.ToInt64()))
                        {
                            objDOGEN_OSTActions.EffectiveDate = objPrevDOGEN_Queue.TimelineEffectiveDate;
                        }
                        else
                        {
                            objDOGEN_OSTActions.EffectiveDate = DateTime.UtcNow.Date.AddMonths(1);
                            objDOGEN_OSTActions.EffectiveDate = new DateTime(objDOGEN_OSTActions.EffectiveDate.Value.Year, objDOGEN_OSTActions.EffectiveDate.Value.Month, 1);
                        }
                        objDOGEN_OSTActions.EndDate = !objPrevDOGEN_Queue.EndDate.IsNull()? objPrevDOGEN_Queue.EndDate: lastDayOfNextMonth;
                        objDOGEN_OSTActions.DateofBirth = objPrevDOGEN_Queue.MemberDOB;
                        actionViewName = "_SendSCCDeletiontoCMS";
                        break;
                    case PWActionsEnum.SendOOALetter:
                        if (source != "")
                        {
                            ViewBag.source = "MassUpdate";

                        }
                        actionViewName = "_SendOOALetter";
                        objDOGEN_OSTActions.FirstLetterMailDate = (discipancyCategory!= (long)DiscripancyCategory.OOA)?(DateTime?)null: ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow,TimeZone);
                        objDOGEN_OSTActions.GPSHouseholdID = objPrevDOGEN_Queue.GPSHouseholdID;

                        break;
                    case PWActionsEnum.SendNotificationofTerminationLetter:
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        if (source == "")
                        {
                            if (objDOGEN_OSTActions.FirstLetterMailDate == null)
                            {
                                ViewBag.sendNotification = "sendNotification";
                            }
                        }
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        actionViewName = "_SendNotificationTerminationLetter";
                        break;
                    case PWActionsEnum.UpdatePDPAutoEnrolleeIndicator:
                        if (source == "")
                        {
                            if (objPrevDOGEN_Queue.MemberLOBLkup != 31004) //MA
                            {                           
                                objDOGEN_OSTActions.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                                actionViewName = "_UpdatePDPAutoEnrolleeIndicator";
                            }
                            else //MA
                            {
                                ViewBag.UpdatePDP = "UpdatePDP";
                                actionViewName = "_UpdatePDPAutoEnrolleeIndicator";
                            }                          
                        }
                        else
                        {
                            objDOGEN_OSTActions.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                            actionViewName = "_UpdatePDPAutoEnrolleeIndicator";
                        }
                       
                        break;
                    case PWActionsEnum.ExtendTracking:
                        if (source != "")
                        {
                            ViewBag.source = "MassUpdate";

                        }
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;                     
                        actionViewName = "_ExtendTracking";
                        break;
                    case PWActionsEnum.ResidentialDocRequired_CountyAttestationRequired:
                        objDOGEN_OSTActions.lstResidentialDocRequired= CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        objDOGEN_OSTActions.lstCountryAttestationRequired = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        objDOGEN_OSTActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_OSTActions.ResidentialDocumentationRequired= objPrevDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired;
                        objDOGEN_OSTActions.CountyAttestationRequired= objPrevDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired;
                        objDOGEN_OSTActions.ContractIDLkup = objPrevDOGEN_Queue.MemberContractIDLkup;
                        actionViewName = "_ResidentialDocRequired";
                        break;
                    case PWActionsEnum.PendCase:
                        objDOGEN_OSTActions.lstPendReasons = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsPendReason, (long)PWActionsEnum.PendCase, discipancyCategory);
                        actionViewName = "_PendCase";
                        break;
                    case PWActionsEnum.SendtoPeerAudit:
                        actionViewName = "_SendToPeerAudit";
                        break;
                    case PWActionsEnum.PeerAuditCompleted:
                        objDOGEN_OSTActions.lstContainsErros = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        actionViewName = "_PeerAuditCompleted";
                        break;
                    case PWActionsEnum.CloseCase:
                        if (source == "")
                        {
                            if (discipancyCategory == (long)DiscripancyCategory.OOA && (objPrevDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAPendingFTT
                                                                                        || objPrevDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAOpenDisenroll
                                                                                        || objPrevDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAMARxAddressLetter
                                                                                        || objPrevDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAAddressScrub))
                            {
                                if (objDOGEN_OSTActions.FirstLetterMailDate == null || objDOGEN_OSTActions.SecondLetterMailDate == null)
                                {
                                    ViewBag.FirstSecLetter = "FirstSecLetter";
                                }

                            }
                        }                    
                        objDOGEN_OSTActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, discipancyCategory);
                        objDOGEN_OSTActions.lstState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                        objDOGEN_OSTActions.FirstLetterMailDateNoResponseTerm = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        actionViewName = "_CloseCase";
                        break;
                    case PWActionsEnum.MARxAddressCompleted:
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        if (source == "")
                        {
                            if (objDOGEN_OSTActions.FirstLetterMailDate == null || objDOGEN_OSTActions.SecondLetterMailDate == null)
                            {
                                ViewBag.FirstSecLetter = "FirstSecLetter";
                            }
                        }
                        objDOGEN_OSTActions.lstMarxAddresCompleted = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.MARxAddressCompleted);
                        objDOGEN_OSTActions.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        actionViewName = "_MARxAddressCompleted";
                        break;
                    case PWActionsEnum.AddressScrubCompleted:
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        if (source == "")
                        {
                            if (objDOGEN_OSTActions.FirstLetterMailDate == null || objDOGEN_OSTActions.SecondLetterMailDate == null)
                            {
                                ViewBag.FirstSecLetter = "FirstSecLetter";
                            }
                        }
                        actionViewName = "_AddressScrubCompleted";
                        break;
                    case PWActionsEnum.SendSCCLetter:
                        actionViewName = "_SendSCCLetter";
                        break;
                    case PWActionsEnum.UpdateGPS:
                        actionViewName = "_UpdateGPS";
                        break;
                    case PWActionsEnum.SCCRPRRequest:
                        objDOGEN_OSTActions.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                        objDOGEN_OSTActions.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                        result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                        objDOGEN_OSTActions.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();
                        if (discipancyCategory == (long)DiscripancyCategory.SCC)
                        {
                            ViewBag.SCCRPR = "SCCRPR";
                            objDOGEN_OSTActions.RPRReasonforRequest = "SCC RPR";
                            objDOGEN_OSTActions.RPRActionRequestedLkup = 14017; // SCC RPR
                        }
                        actionViewName = "_SCCRPRRequest";
                        break;
                    case PWActionsEnum.Close_MailingAddressNotVerified:
                        actionViewName = "_CloseAndMailingAddressNotVerified";
                        break;
                    case PWActionsEnum.AddComments:
                        actionViewName = "_AddComments";
                        break;
                    case PWActionsEnum.ReOpenAddComments:
                        actionViewName = "_ReOpenAddComments";
                        break;
                    case PWActionsEnum.NoOOALetter:
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        actionViewName = "_NoOOALetter";
                        break;
                    case PWActionsEnum.KeepinTrackingNOT:
                    objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        if (source == "")
                        {
                            if (objDOGEN_OSTActions.FirstLetterMailDate == null)
                            {
                                ViewBag.FirstLetter = "FirstLetter";
                            }
                        }
                     actionViewName = "_KeepInTrackingNOT";
                        break;
                    case PWActionsEnum.KeepinTrackingFTT:
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        if (source == "")
                        {
                            if (objDOGEN_OSTActions.FirstLetterMailDate == null || objDOGEN_OSTActions.SecondLetterMailDate == null)
                            {
                                ViewBag.FirstSecLetter = "FirstSecLetter";
                            }
                        }
                      
                        actionViewName = "_KeepInTrackingFTT";
                        break;
                    case PWActionsEnum.ReOpenCloseCase:
                        objDOGEN_OSTActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, discipancyCategory);
                        objDOGEN_OSTActions.lstState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                        objDOGEN_OSTActions.FirstLetterMailDateNoResponseTerm = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.FirstLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate;
                        objDOGEN_OSTActions.SecondLetterMailDate = objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate;
                        actionViewName = "_CloseCase";
                        break;
                    case PWActionsEnum.PendingNOT:
                        long PreviousCat = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.DiscrepancyCategoryLkup).FirstOrDefault();                       
                        if (PreviousCat == (long)DiscripancyCategory.OOA)
                        {
                            DateTime? FirstLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.FirstLetterMailDate).SingleOrDefault();                           
                            if (FirstLetterMailDate == null)
                            {
                                ViewBag.MailDate = "MailDate";
                            }
                            else
                            {
                                ViewBag.PreviousCat = "OOACat";
                            }
                        }
                        actionViewName = "_PendingNOT";
                        break;
                    case PWActionsEnum.PendingFTT:
                        long PreviousNOTCat = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.DiscrepancyCategoryLkup).FirstOrDefault();
                        if (PreviousNOTCat == (long)DiscripancyCategory.OOA)
                        {
                            DateTime? FirstLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.FirstLetterMailDate).SingleOrDefault();
                            DateTime? secondLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.SecondLetterMailDate).SingleOrDefault();
                            if (FirstLetterMailDate == null || secondLetterMailDate==null)
                            {
                                ViewBag.MailDate = "MailDate";
                            }
                            else
                            {
                                ViewBag.PreviousCat = "OOACat";
                            }
                        }
                        actionViewName = "_PendingFTT";
                        break;
                    case PWActionsEnum.OpenNOT:
                        long PreviousOpenNOTCat = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.DiscrepancyCategoryLkup).FirstOrDefault();
                        if (PreviousOpenNOTCat == (long)DiscripancyCategory.OOA)
                        {
                            DateTime? FirstLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.FirstLetterMailDate).SingleOrDefault();
                            if (FirstLetterMailDate == null)
                            {
                                ViewBag.MailDate = "MailDate";
                            }
                            else
                            {
                                ViewBag.PreviousCat = "OOACat";
                            }
                        }
                        actionViewName = "_OpenNOT";
                        break;
                    case PWActionsEnum.OpenDisenroll:
                        long PreviousOpenDisCat = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.DiscrepancyCategoryLkup).FirstOrDefault();
                        if (PreviousOpenDisCat == (long)DiscripancyCategory.OOA)
                        {
                            DateTime? FirstLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.FirstLetterMailDate).SingleOrDefault();
                            DateTime? secondLetterMailDate = objPrevDOGEN_Queue.lstDOGEN_QueueRefferencedCases.Select(y => y.SecondLetterMailDate).SingleOrDefault();
                            if (FirstLetterMailDate == null || secondLetterMailDate==null)
                            {
                                ViewBag.MailDate = "MailDate";
                            }
                            else
                            {
                                ViewBag.PreviousCat = "OOACat";
                            }
                        }
                        actionViewName = "_OpenDisenroll";
                        break;
                    case PWActionsEnum.RouteToNewSCCCase:
                        actionViewName = "_RouteToNewSCCCase";
                        break;
                    case PWActionsEnum.ReviewandReturntoQueue:
                        actionViewName = "_ReviewandReturntoQueue";
                        break;
                    case PWActionsEnum.PotentialSCCRPRDay1:
                        actionViewName = "_PotentialSCCRPRDay1";
                        break;
                    case PWActionsEnum.NeedsEGHPReview:
                        actionViewName = "_NeedsEGHPReview";
                        break;
                    case PWActionsEnum.UpdateComplianceStartDate:
                        actionViewName = "_UpdateComplianceStartDate";
                        break;
                    case PWActionsEnum.CancelPendCase:
                        actionViewName = "_CancelPendCase";
                        break;
                    case PWActionsEnum.IncarceratedRPRRequested:
                        objDOGEN_OSTActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, discipancyCategory);
                        objDOGEN_OSTActions.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                        objDOGEN_OSTActions.RPRActionRequestedLkup =(long)RPRActionRequested.Disenrollment;
                        objDOGEN_OSTActions.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                        result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                        objDOGEN_OSTActions.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.FullName).ToList();
                        actionViewName = "_IncarceratedRPRRequested";
                        break;
                    case PWActionsEnum.SendOOALetterandResDocCtyAttestRequired:
                        objDOGEN_OSTActions.lstResidentialDocRequired = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        objDOGEN_OSTActions.lstCountryAttestationRequired = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        objDOGEN_OSTActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_OSTActions.ResidentialDocumentationRequired = objPrevDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired;
                        objDOGEN_OSTActions.CountyAttestationRequired = objPrevDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired;
                        objDOGEN_OSTActions.ContractIDLkup = objPrevDOGEN_Queue.MemberContractIDLkup;
                        objDOGEN_OSTActions.FirstLetterMailDate = (discipancyCategory != (long)DiscripancyCategory.OOA) ? (DateTime?)null : ZoneLookupUI.ConvertToTimeZone(DateTime.UtcNow, TimeZone);
                        objDOGEN_OSTActions.GPSHouseholdID = objPrevDOGEN_Queue.GPSHouseholdID;
                        actionViewName = "_SendOOALetterandResDocCtyAttestRequired";
                        break;
                }

                return PartialView(actionViewName, objDOGEN_OSTActions);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return PartialView("");
            }
        }
        /// <summary>
        /// Check for duplicate case while send OOA letter action 
        /// performed from SCC/TRR cases
        /// </summary>
        /// <returns></returns>
        public bool CheckDuplicateWhileCloseAndCreate(DOGEN_Queue objDoGen_Queue,out string message)
        {
            message = string.Empty;
            bool isSuccess = false;
            long genQueueId = 0;
            try
            {
                if (!objDoGen_Queue.IsNull() && !objDoGen_Queue.lstDOGEN_QueueRefferencedCases.IsNull() && objDoGen_Queue.lstDOGEN_QueueRefferencedCases.Count > 0)
                {
                    if (!objDoGen_Queue.MemberCurrentHICN.IsNullOrEmpty() && !objDoGen_Queue.MemberCurrentHICN.IsNullOrEmpty() && !objDoGen_Queue.MemberContractID.IsNullOrEmpty())
                    {
                        var res = objDoGen_Queue.lstDOGEN_QueueRefferencedCases
                                                .Where(xx => xx.DiscrepancyCategoryLkup == DiscripancyCategory.OOA.ToInt64()
                                                 && xx.MemberCurrentHICN == objDoGen_Queue.MemberCurrentHICN
                                                 && xx.MemberContract == objDoGen_Queue.MemberContractID
                                                 && xx.MemberPBP == objDoGen_Queue.MemberPBP
                                                 && xx.MostRecentStatusLkup != CurrentStatusLkup.ResolvedComplted.ToInt64()).ToList();

                        if (!res.IsNull() && res.Count() > 0)
                        {
                            genQueueId = res[0].Gen_QueueId;
                            message = "ERS Case ID: " + genQueueId + " alredy exists in Out of area Case.";
                            isSuccess = true;
                        }


                    }


                }
                return isSuccess;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

  
        // Get Common report URL
        [HttpPost]
        public ActionResult GetCommonReportURL()
        {
            _objBLReports = new BLReports();
            List<DORPT_ReportsMaster> lstDORPT_ReportsMaster;
            string errorMessage = string.Empty;
            ExceptionTypes resultReports = _objBLReports.GetAllReports((long)ReportId.CommonHistoryReport, string.Empty, out lstDORPT_ReportsMaster, out errorMessage);
            var urlData = lstDORPT_ReportsMaster.FirstOrDefault().ReportURL;
            return Json(new { Data = urlData, JsonRequestBehavior.AllowGet });
        }

        [HttpGet]
        public ActionResult GetXeroxFile(string filePath)
        {
            try
            {
                string strFileName = filePath.Split('\\').LastOrDefault().ToString();
                byte[] fileByteArray = System.IO.File.ReadAllBytes(filePath);
                return File(fileByteArray, "image/jpeg", strFileName);
            }
            catch (System.IO.FileNotFoundException nfEX)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, nfEX.Message);
                return Content("File Not Available.");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new
                {
                    Error = ex.ToString()
                });
            }
        }

        /// <summary>
        /// USPS Address validation
        /// </summary>
        /// <param name="lookup"></param>
        /// <param name="address1"></param>
        /// <param name="address2"></param>
        /// <param name="city"></param>
        /// <param name="state"></param>
        /// <param name="zipCode"></param>
        /// <returns></returns>
        public JsonResult VerifyUSPSAddress(string lookup, string address1, string address2, string city, string state, string zipCode = "")
        {
            try
            {
                string response = string.Empty;
                string zip5 = string.Empty;
                string zip4 = string.Empty;
                if (zipCode.Length == 5)
                {
                    zip5 = zipCode;
                }
                if (zipCode.Length == 4)
                {
                    zip4 = zipCode;
                }
                if (zipCode.Length == 9)
                {
                    zip5 = zipCode.Substring(0, 5); ;
                    zip4 = zipCode.Substring(5, 4);
                }

                switch (lookup)
                {
                    case "1":
                        response = _USPSService.AddressValidateRequest(address1, address2, city, state, zip5, zip4);
                        break;
                    case "2":
                        response = _USPSService.ZipcodeLookupRequest(address1, address2, city, state);
                        break;
                    case "3":
                        response = _USPSService.CityStateLookupRequest(zipCode);
                        break;
                }
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);
                string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw;
            }

        }

        /// <summary>
        /// GetPDPAutoEnrolleeIndicator
        /// </summary>
        /// <param name="GPSHouseHoldID"></param>
        /// <returns></returns>
        private int GetPDPAutoEnrolleeIndicator(string GPSHouseHoldID)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            objDOGEN_GPSServiceRequestParameter.HouseholdId = GPSHouseHoldID;
            List<DOGEN_GPSData> lstDOGEN_GPSData = new List<DOGEN_GPSData>();
            errorMessage = string.Empty;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objGPSServiceGetMethods.GetMemberEligibilityService(objDOGEN_GPSServiceRequestParameter, out lstDOGEN_GPSData, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                return lstDOGEN_GPSData.FirstOrDefault().PDPAutoEntrolleeIndicator;

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Get Queue
        /// </summary>
        /// <returns></returns>
        private DOGEN_Queue PGetOSTCreateCase(long discCategoryID)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            long workBasketLkp = currentUser.WorkBasketLkup.ToInt64();
            try
            {
                objDOGEN_Queue.DiscrepancyCategoryLkup = discCategoryID;
                objDOGEN_Queue.DiscrepancySourceLkup = DiscrepancySource.SingleCaseCreation.ToInt64();
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkp);
                objDOGEN_Queue.lstDiscSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscrepancySource);
                objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discCategoryID);
                objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                objDOGEN_Queue.lstSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SourceSystem);
                objDOGEN_Queue.lstMemberVerifiedState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                objDOGEN_Queue.lstTransactionReplyCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionReplyCode);
                objDOGEN_Queue.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                objDOGEN_Queue.ComplianceStartDate = DateTime.UtcNow;
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// GetProcessWork for OOA,SCC,TRR 
        /// </summary>
        /// <param name="genQueueID"></param>
        /// <returns></returns>
        private DOGEN_Queue PGetProcessWork(long genQueueID, string pageName)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            BLCommon objBLCommon = new BLCommon();
            DOGEN_ManageCases objDOGEN_ManageCases = new DOGEN_ManageCases();
            objDOGEN_ManageCases.GEN_QueueRef = genQueueID;
            objDOGEN_ManageCases.ActionPerformedLkup = (long)ActionLookup.View;
            objDOGEN_ManageCases.CurrentUserRef = currentUser.ADM_UserMasterId;
            objDOGEN_ManageCases.CreatedByRef = currentUser.ADM_UserMasterId;
            errorMessage = string.Empty;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            string strMiimComment = string.Empty;

            try
            {
                objBLCommon.InsertManageCase(objDOGEN_ManageCases, out string ErrorMessage);
                ExceptionTypes result = _objBLOST.GetGenQueueByID(TimeZone, genQueueID, out objDOGEN_Queue, out errorMessage);
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                }
                objDOGEN_Queue.lstOptionsforReopen = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Reopen);

                objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, objDOGEN_Queue.MostRecentWorkQueueLkup);
                //only for incarsirated cases
                if (objDOGEN_Queue.DiscrepancyTypeLkup != DiscripancyType.Incarcerated.ToLong())
                {
                    objDOGEN_Queue.lstActions.Remove(objDOGEN_Queue.lstActions.Where(x => x.CMN_LookupMasterChildRef == (long)PWActionsEnum.IncarceratedRPRRequested).FirstOrDefault());
                }
                if (objDOGEN_Queue.IsCasePended)
                {
                    objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)objDOGEN_Queue.PreviousWorkQueueLkup);
                    List<DOCMN_LookupMasterCorrelations> CancelPendedQueue = ((DynamicCancelPendCase[])Enum.GetValues(typeof(DynamicCancelPendCase))).Select(c => new DOCMN_LookupMasterCorrelations() { CMN_LookupMasterChildRef = (long)c, LookupMasterChildValue = c.ToString().Replace("_"," ") }).ToList();
                    objDOGEN_Queue.lstActions.AddRange(CancelPendedQueue);
                   
                }
                //to remove all actions for MIIM system
                if (pageName == ConstantTexts.MiimPageName)
                {
                    objDOGEN_Queue.lstActions.RemoveAll(x => x.CMN_LookupMasterChildRef != (long)PWActionsEnum.AddComments);
                }

                //to add comment from MIIM system on page top
                if (objDOGEN_Queue.MostRecentWorkQueueLkup == (long)OOAQueue.OOAMIIMUpdated || objDOGEN_Queue.MostRecentWorkQueueLkup == (long)SCCQueue.SCCMIIMUpdated)
                {
                    strMiimComment = objDOGEN_Queue.lstDOGEN_Comments.OrderByDescending(x => x.UTCCreatedOn).Where(x => x.SourceSystemLkup == (long)SourceSystemLkup.MIIM).FirstOrDefault()?.Comments ?? string.Empty;
                }


                //Storing the Gen_Queue intial load data  for action details  
                TempData[genQueueID.NullToString()] = objDOGEN_Queue;//Store the variable for performing action details

                ViewBag.LOB = objDOGEN_Queue.MemberLOBLkup;
                ViewBag.MiimComment = strMiimComment;
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// Reload with Default data From Temp memory / Compaire previous OSTAction Object with new OST Action Objects
        /// </summary>
        /// <param name="objDOGEN_OSTActions"></param>
        private void PUpdateOstAction(ref DOGEN_OSTActions objDOGEN_OSTActions)
        {
            try
            {
                string genQueueRef = objDOGEN_OSTActions.GEN_QueueRef.NullToString();
                DOGEN_Queue objPrevDOGEN_Queue = new DOGEN_Queue();
                objPrevDOGEN_Queue = (DOGEN_Queue)TempData[genQueueRef];
                TempData.Keep(genQueueRef);

                objDOGEN_OSTActions.ActionLkup = (objDOGEN_OSTActions.ActionLkup).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ActionLkup : objDOGEN_OSTActions.ActionLkup;

                objDOGEN_OSTActions.LastName = (objDOGEN_OSTActions.LastName).IsNullOrEmpty() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.LastName : objDOGEN_OSTActions.LastName;
                objDOGEN_OSTActions.DateofBirth = (objDOGEN_OSTActions.DateofBirth).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.DateofBirth : objDOGEN_OSTActions.DateofBirth;
                objDOGEN_OSTActions.ContractIDLkup = (objDOGEN_OSTActions.ContractIDLkup).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ContractIDLkup : objDOGEN_OSTActions.ContractIDLkup;
                objDOGEN_OSTActions.PBPLkup = (objDOGEN_OSTActions.PBPLkup).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.PBPLkup : objDOGEN_OSTActions.PBPLkup;
                objDOGEN_OSTActions.ApplicationDate = (objDOGEN_OSTActions.ApplicationDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ApplicationDate : objDOGEN_OSTActions.ApplicationDate;
                objDOGEN_OSTActions.EffectiveDate = (objDOGEN_OSTActions.EffectiveDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.EffectiveDate : objDOGEN_OSTActions.EffectiveDate;
                objDOGEN_OSTActions.EndDate = (objDOGEN_OSTActions.EndDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.EndDate : objDOGEN_OSTActions.EndDate;
                objDOGEN_OSTActions.FirstLetterMailDate = (objDOGEN_OSTActions.FirstLetterMailDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate : objDOGEN_OSTActions.FirstLetterMailDate;
                objDOGEN_OSTActions.SecondLetterMailDate = (objDOGEN_OSTActions.SecondLetterMailDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate : objDOGEN_OSTActions.SecondLetterMailDate;
                objDOGEN_OSTActions.ResidentialDocumentationRequired = (objDOGEN_OSTActions.ResidentialDocumentationRequired.IsNull()) ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired : objDOGEN_OSTActions.ResidentialDocumentationRequired;
                objDOGEN_OSTActions.CountyAttestationRequired = (objDOGEN_OSTActions.CountyAttestationRequired.IsNull()) ? objPrevDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired : objDOGEN_OSTActions.CountyAttestationRequired;
                objDOGEN_OSTActions.PendReasonLkup = (objDOGEN_OSTActions.PendReasonLkup).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.PendReasonLkup : objDOGEN_OSTActions.PendReasonLkup;
                objDOGEN_OSTActions.ContainsErrorsLkup = (objDOGEN_OSTActions.ContainsErrorsLkup.IsNull()) ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ContainsErrorsLkup : objDOGEN_OSTActions.ContainsErrorsLkup;
                objDOGEN_OSTActions.ResolutionLkup = (objDOGEN_OSTActions.ResolutionLkup).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.ResolutionLkup : objDOGEN_OSTActions.ResolutionLkup;
                objDOGEN_OSTActions.Reason = (objDOGEN_OSTActions.Reason).IsNullOrEmpty() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.Reason : objDOGEN_OSTActions.Reason;
                objDOGEN_OSTActions.InitialAddressVerificationDate = (objDOGEN_OSTActions.InitialAddressVerificationDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.InitialAddressVerificationDate : objDOGEN_OSTActions.InitialAddressVerificationDate;
                objDOGEN_OSTActions.MemberResponseVerificationDate = (objDOGEN_OSTActions.MemberResponseVerificationDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.MemberResponseVerificationDate : objDOGEN_OSTActions.MemberResponseVerificationDate;
                objDOGEN_OSTActions.MemberVerifiedState = (objDOGEN_OSTActions.MemberVerifiedState).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.MemberVerifiedState : objDOGEN_OSTActions.MemberVerifiedState;
                objDOGEN_OSTActions.SCCLetterMailDate = (objDOGEN_OSTActions.SCCLetterMailDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.SCCLetterMailDate : objDOGEN_OSTActions.SCCLetterMailDate;
                objDOGEN_OSTActions.GPSHouseholdID = (objDOGEN_OSTActions.GPSHouseholdID).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.GPSHouseholdID : objDOGEN_OSTActions.GPSHouseholdID;
                objDOGEN_OSTActions.AdjustedComplianceStartDate = (objDOGEN_OSTActions.AdjustedComplianceStartDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.AdjustedComplianceStartDate : objDOGEN_OSTActions.AdjustedComplianceStartDate;
                objDOGEN_OSTActions.AdjustedDisenrollmentDate = (objDOGEN_OSTActions.AdjustedDisenrollmentDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.AdjustedDisenrollmentDate : objDOGEN_OSTActions.AdjustedDisenrollmentDate;
                objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate = (objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate : objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate;
                objDOGEN_OSTActions.PDPAutoEnrolleeInd = (objDOGEN_OSTActions.PDPAutoEnrolleeInd).IsNull() ? objPrevDOGEN_Queue.objDOGEN_OSTActions.PDPAutoEnrolleeInd : objDOGEN_OSTActions.PDPAutoEnrolleeInd;
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
        /// <param name="objDOGEN_Queue"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckDuplicateLogic(DOGEN_Queue objDOGEN_Queue, out List<SearchResults> lstSearchResults, out string errorMessage)
        {
            bool isDuplicate = false;
            ExceptionTypes result = new ExceptionTypes();
            BLCommon objBLCommon = new BLCommon();
            lstSearchResults = new List<SearchResults>();
            errorMessage = string.Empty;
            string totalCount = string.Empty;
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            try
            {
                SearchCriteria objSearchCriteria = new SearchCriteria()
                {
                    CurrentHICN = objDOGEN_Queue.MemberCurrentHICN,
                    ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup,
                    PBPLkup = objDOGEN_Queue.MemberPBPLkup,
                    WorkBasketLkup = objDOGEN_Queue.WorkBasketLkup,
                    DiscrepancyTypeLkup = objDOGEN_Queue.DiscrepancyTypeLkup,
                    DiscrepancyCategoryLkup = objDOGEN_Queue.DiscrepancyCategoryLkup,
                    StatusNot = (long)CurrentStatusLkup.ResolvedComplted
                };
                result = objBLCommon.SearchRecords(TimeZone, (long)currentUser.WorkBasketLkup, 0, ConstantTexts.DefaultCount, objSearchCriteria, out lstSearchResults, out totalCount,out errorMessage);


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
                return isDuplicate;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Get TRC from UI and Set it as most Recent Queue Lkup
        /// </summary>
        /// <param name="transactionReplyCode"></param>
        /// <returns></returns>
        private long GetTransactionReplyCode(long transactionReplyCode)
        {

            try
            {
                switch (transactionReplyCode)
                {
                    case 22001:
                    case 22005:
                        transactionReplyCode = TRRQueue.TRREscalated.ToInt64();
                        break;
                    case 22006://trc 155
                        transactionReplyCode = TRRQueue.TRRTRC155.ToInt64();
                        break;
                    case 22010://trc 260
                    case 22011://trc 261
                    case 22013:
                        transactionReplyCode = TRRQueue.TRRCMSRejected.ToInt64();
                        break;
                    case 22014://trc 266
                        transactionReplyCode = TRRQueue.TRRTRC282.ToInt64();
                        break;
                    case 22015://trc 266
                        transactionReplyCode = TRRQueue.TRRCMSRejectedDeletionCode.ToInt64();
                        break;
                }

                return transactionReplyCode;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
        private void  DOCMSPostTransaction(DOGEN_OSTActions objDOGEN_OSTActions, out string errorMessage)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            CommonController objcontroller = new CommonController();
            errorMessage = string.Empty;
            try
            {
                string gen_QueueId= objDOGEN_OSTActions.GEN_QueueRef.NullToString();
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = gen_QueueId;
                DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
                objDOGEN_Queue = (DOGEN_Queue)TempData[gen_QueueId];
                TempData.Keep(gen_QueueId);
                //Doing CMS Transaction
                if (objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendSCCUpdatetoCMS || objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendSCCDeletiontoCMS)
                {
                    var contractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == objDOGEN_OSTActions.ContractIDLkup).FirstOrDefault();
                    var pbpNo = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == objDOGEN_OSTActions.PBPLkup).FirstOrDefault();
                    objDOGEN_GPSServiceRequestParameter.ApplicationDate = !objDOGEN_OSTActions.ApplicationDate.IsNull() ? objDOGEN_OSTActions.ApplicationDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.BirthDate = !objDOGEN_OSTActions.DateofBirth.IsNull() ? objDOGEN_OSTActions.DateofBirth.Value.ToString("yyyy-MM-dd") : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.ContractNumber = !contractNumber.IsNull() ? contractNumber.LookupValue : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.EffectiveEndDate = !objDOGEN_OSTActions.EndDate.IsNull() ? objDOGEN_OSTActions.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = !objDOGEN_OSTActions.EffectiveDate.IsNull() ? objDOGEN_OSTActions.EffectiveDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                    //objDOGEN_GPSServiceRequestParameter.ElectionType = "E";
                    objDOGEN_GPSServiceRequestParameter.ActionLkup = objDOGEN_OSTActions.ActionLkup.ToInt64();
                    objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = objDOGEN_Queue.MemberCurrentHICN;
                    objDOGEN_GPSServiceRequestParameter.LastName = !objDOGEN_OSTActions.LastName.IsNull() ? objDOGEN_OSTActions.LastName : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.PbpNo = !pbpNo.IsNull() ? pbpNo.LookupValue : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();
                    objDOGEN_GPSServiceRequestParameter.ERSCaseId = gen_QueueId;
                    //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objDOGEN_GPSServiceRequestParameter);                  
                    //string URL = CacheUtility.GetMasterConfigurationByName(ConstantTexts.PostCMSTransaction);
                    //errorMessage =   CommonController.POSTCallWebAPI(URL, jsonString);                
                    objGPSServiceGetMethods.CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);//Do CMS Post Transaction
                }
                //Send OOA letter
                else if (objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendOOALetter || objDOGEN_OSTActions.ActionLkup == (long)PWActionsEnum.SendOOALetterandResDocCtyAttestRequired)
                {
                    objDOGEN_GPSServiceRequestParameter.HouseholdId = objDOGEN_OSTActions.GPSHouseholdID;
                    objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate = !objDOGEN_Queue.DisenrollmentDate.IsNull() ? objDOGEN_Queue.DisenrollmentDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                    objDOGEN_GPSServiceRequestParameter.ERSCaseId = gen_QueueId;
                    //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objDOGEN_GPSServiceRequestParameter);                   
                    //string URL = CacheUtility.GetMasterConfigurationByName(ConstantTexts.PostOutOfAreaService);
                    //errorMessage = CommonController.POSTCallWebAPI(URL, jsonString);
                    objGPSServiceGetMethods.MaintainOutOfAreaServiceService(objDOGEN_GPSServiceRequestParameter, out errorMessage);//Send OOA Letter
                }

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }

        private string GetFormattedText(string fullText)
        {
            string finalText = string.Empty;
            try
            {
                int Pos1 = fullText.LastIndexOf(":") + 1;
                int Pos2 = fullText.Length;
                finalText = fullText.Substring(Pos1, Pos2 - Pos1);
                if (finalText.Contains(']'))
                    finalText = finalText.Replace("]", "");
                return finalText;
            }
            catch (Exception)
            {

                return finalText;
            }
        }

        private DOGEN_Queue PUpdateCaseInfo(long discripancyCategory,long genQueueId)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            long workBasketLkp = currentUser.WorkBasketLkup.ToInt64();
            try
            {
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueId.NullToString()];
                TempData.Keep(genQueueId.NullToString());
                objDOGEN_Queue.IsCaseUpdate = true;
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, workBasketLkp);
                objDOGEN_Queue.lstDiscSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.DiscrepancySource);
                objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, objDOGEN_Queue.DiscrepancyCategoryLkup);
                objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                objDOGEN_Queue.lstSourceSystem = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SourceSystem);
                objDOGEN_Queue.lstMemberVerifiedState = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.State);
                objDOGEN_Queue.lstTransactionReplyCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionReplyCode);
                objDOGEN_Queue.lstPDPAutoEnrolleeInd = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
        /// <summary>
        /// Check for restriction logic while doing send OOA letter from SCC and TRR because a related OOA case is opening at that time
        /// </summary>
        /// <param name="hhID"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool CheckEGHPOnSendOOALetterSCCOrTRR(string hhID,out string msg)
        {
            string employerId = string.Empty;
            bool isEGHP = false;
            CommonController objCommonController = new CommonController();
            ExceptionTypes result = new ExceptionTypes();
            msg = string.Empty;
            try
            {
                if (hhID.IsNullOrEmpty())
                {
                    return false;
                }
                result = objCommonController.IsOnshoreOnly(hhID, out bool isRetricted, out employerId);

                if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, "Error getting IsRestricted/EGHP/National vlaue for new Case", hhID);

                if (objCommonController.IsOOAEGHPExclusion(employerId))
                {
                    isEGHP = true;
                     msg = ConstantTexts.MsgForOOAEGHPExclusionOOALetter;
                }
                return isEGHP;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                objCommonController = null;
            }
       
        }


    }
}