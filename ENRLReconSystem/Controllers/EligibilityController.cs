using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Common;
using ENRLReconSystem.Models;
using ENRLReconSystem.Utility;
using Newtonsoft.Json;
using System.Reflection;
using System.Xml;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    [Common.Filter(WorkBasketLkup = (long)WorkBasket.GPSvsMMR)]
    [ValidateInput(false)]
    public class EligibilityController : Controller
    {
        private UIUserLogin currentUser;
        private BLReports _objBLReports;
        public EligibilityController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }

        string errorMessage = string.Empty;
        // GET: Eligibility

        [HttpGet]
        public ActionResult CreateEligibilityCase()
        {
            try
            {
                long EligbilityLookUp = ((long)DiscripancyCategory.Eligibility);
                ViewBag.CaseType = "Create Eligibility Case";
                return View("Create", PGetEligibilityCreateCase(EligbilityLookUp));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }

        [HttpGet]
        public ActionResult CreateDOBCase()
        {
            try
            {
                long DOBLookUp = ((long)DiscripancyCategory.DOB);
                ViewBag.CaseType = "Create DOB Case";
                return View("Create", PGetEligibilityCreateCase(DOBLookUp));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        [HttpGet]
        public ActionResult CreateGenderCase()
        {
            try
            {
                long GenderLookUp = ((long)DiscripancyCategory.Gender);
                ViewBag.CaseType = "Create Gender Case";
                return View("Create", PGetEligibilityCreateCase(GenderLookUp));
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        [HttpPost]
        public ActionResult Create(DOGEN_Queue objDOGEN_Queue)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            BLEligibility objBLEligibility = new BLEligibility();
            long loginUserId = currentUser.ADM_UserMasterId;
            string strPermPath = string.Empty;
            string totalCount = string.Empty;
            List<SearchResults> lstSearchResults = new List<SearchResults>();
            string employerId = string.Empty;
            try
            {

                objDOGEN_Queue.LoginUserId = loginUserId;
                objDOGEN_Queue.RoleLkup = currentUser.RoleLkup;
                //objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToInt64();
                objDOGEN_Queue.WorkBasketLkup = currentUser.WorkBasketLkup.ToInt64();
                objDOGEN_Queue.CreatedByRef = loginUserId;
                objDOGEN_Queue.CurrentUserRef = loginUserId;
                objDOGEN_Queue.IsCasePended = false;
                objDOGEN_Queue.IsCaseResolved = false;
                objDOGEN_Queue.CommentsSourceSystemLkup = (long)SourceSystemLkup.ERS;
                objDOGEN_Queue.UTCCreatedOn = DateTime.UtcNow;
                objDOGEN_Queue.MemberDOBId = objDOGEN_Queue.MemberDOB != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.MemberDOB).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.DiscrepancyReceiptDateId = Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.DiscrepancyReceiptDate).ToString("yyyyMMdd"));
                objDOGEN_Queue.ComplianceStartDateId = Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.ComplianceStartDate).ToString("yyyyMMdd"));
                objDOGEN_Queue.DiscrepancyStartDateId = Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.DiscrepancyStartDate).ToString("yyyyMMdd"));
                objDOGEN_Queue.DiscrepancyEndDateId = Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.DiscrepancyEndDate).ToString("yyyyMMdd"));

                objDOGEN_Queue.EligGPSInsuredPlanEffectiveDateId = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.EligGPSInsuredPlanTermDateId = objDOGEN_Queue.EligGPSInsuredPlanTermDate != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligGPSInsuredPlanTermDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.EligGPSMemberDOBId = objDOGEN_Queue.EligGPSMemberDOB != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligGPSMemberDOB).ToString("yyyyMMdd")) : (long?)null;

                objDOGEN_Queue.EligMMRPaymentAdjustmentStartDateId = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.EligMMRPaymentAdjustmentEndDateId = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.EligMMRDOBId = objDOGEN_Queue.EligMMRDOB != null ? Convert.ToInt64(Convert.ToDateTime(objDOGEN_Queue.EligMMRDOB).ToString("yyyyMMdd")) : (long?)null;

                objDOGEN_Queue.IsParentCase = false;
                objDOGEN_Queue.IsChildCase = false;
                if (objDOGEN_Queue.OOAFlagLkupValue == 1)
                {
                    objDOGEN_Queue.EligOOAFlagLkup = true;
                }
                else
                {
                    objDOGEN_Queue.EligOOAFlagLkup = false;
                }
                objDOGEN_Queue.IsActive = true;
                objDOGEN_Queue.ActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.ActionPerformedLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.CurrentActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.CurrentStatusLkup = (long)CurrentStatusLkup.New;
                if (objDOGEN_Queue.GEN_QueueId > 0)
                {
                    objDOGEN_Queue.IsChildCase = true;
                    objDOGEN_Queue.ParentQueueRef = objDOGEN_Queue.GEN_QueueId;
                }
                if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Eligibility) // Eligibility
                {
                    objDOGEN_Queue.CurrentWorkQueuesLkup = (long)QueueLookup.EligNewCase;
                    objDOGEN_Queue.MostRecentActionLkup = (long)ActionLookup.Save;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (long)QueueLookup.EligNewCase;
                    objDOGEN_Queue.MostRecentStatusLkup = (long)CurrentStatusLkup.New;
                }
                else if (objDOGEN_Queue.DiscrepancyCategoryLkup == (long)DiscripancyCategory.Gender) // Gender
                {
                    objDOGEN_Queue.CurrentWorkQueuesLkup = (long)QueueLookup.GenderNewCase;
                    objDOGEN_Queue.MostRecentActionLkup = (long)ActionLookup.Save;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (long)QueueLookup.GenderNewCase;
                    objDOGEN_Queue.MostRecentStatusLkup = (long)CurrentStatusLkup.New;
                }
                else // DOB
                {
                    objDOGEN_Queue.CurrentWorkQueuesLkup = (long)QueueLookup.DOBNewCase;
                    objDOGEN_Queue.MostRecentActionLkup = (long)ActionLookup.Save;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (long)QueueLookup.DOBNewCase;
                    objDOGEN_Queue.MostRecentStatusLkup = (long)CurrentStatusLkup.New;
                }
                //Check Duplicate Logic
                if (CheckDuplicateLogic(objDOGEN_Queue, out lstSearchResults, out errorMessage))
                {
                    long lCaseID = lstSearchResults.FirstOrDefault().WorkItemID.Value;
                    string strMessage = "Already record with <strong> ERS Case ID_" + lCaseID + " </strong> exists";
                    return Json(new { ID = (long)ExceptionTypes.DuplicateRecord, Message = strMessage, Gen_QueueId = 0 });
                }

                //restricted case logic
                CommonController objCommonController = new CommonController();
                result = objCommonController.IsOnshoreOnly(objDOGEN_Queue.GPSHouseholdID, out bool isRetricted,out employerId);
                if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, "Error getting IsRestricted vlaue for new Case", objDOGEN_Queue.GPSHouseholdID);
                objDOGEN_Queue.IsRestricted = isRetricted;
                objCommonController = null;

                result = objBLEligibility.CreateEligibilityCase(objDOGEN_Queue, out errorMessage);
                
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
                }

                long issamebusinesssegment = 0;
                if (objDOGEN_Queue.BusinessSegmentLkup == (long)BusinessSegment.MNR)
                {
                    objDOGEN_Queue.BusinessSegment = "M & R";
                }
                else if (objDOGEN_Queue.BusinessSegmentLkup == (long)BusinessSegment.CNS)
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
                    returnMessage = "<b>Onshore Only Restricted </b> Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b></br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b> Do you want to process it?";
                }
                else if (objDOGEN_Queue.IsRestricted && !(objDOGEN_Queue.BusinessSegmentLkup == currentUser.BusinessSegmentLkup.ToLong()))
                {
                    issamebusinesssegment = 1;
                    returnMessage = "<b>Onshore Only Restricted </b> Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b>";
                }
                else if (!objDOGEN_Queue.IsRestricted && (objDOGEN_Queue.BusinessSegmentLkup == currentUser.BusinessSegmentLkup.ToLong()))
                {
                    issamebusinesssegment = 0;
                    returnMessage = "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b> Do you want to process it?";
                }
                else
                {
                    issamebusinesssegment = 1;
                    returnMessage = "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b>";
                }
                //Save and return
                return Json(new { ID = result, Message = returnMessage, Gen_QueueId = objDOGEN_Queue.GEN_QueueId, businessSegmentLkup = objDOGEN_Queue.BusinessSegment, issamebusinesssegment = issamebusinesssegment });

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = result, Message = "An error occured while performing Save action.", Gen_QueueId = 0 });
            }

        }



        public ActionResult UpdateCaseInfo(long discripancyCategory, long genQueueId)
        {
            try
            {
                return PartialView("_UpdateEligbilityCaseInfo", PUpdateEligbilityCaseInfo(discripancyCategory, genQueueId));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }
        }

        private DOGEN_Queue PUpdateEligbilityCaseInfo(long discripancyCategory,long genQueue)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueue.NullToString()];
                TempData.Keep(genQueue.NullToString());
                long WorkBasketID = (long)WorkBasket.GPSvsMMR;
                objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, WorkBasketID);
                objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discripancyCategory);
                objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                objDOGEN_Queue.DiscrepancyCategoryLkup = discripancyCategory;
                objDOGEN_Queue.lstOOAFlag = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                objDOGEN_Queue.lstGender = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Gender);
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Process Work Flow
        public ActionResult EligibilityProcessWork(string queueId, string pageName = "")
        {
            long caseId = URLEncoderDecoder.Decode(queueId).ToInt64();
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                return View(PGetProcessWork(caseId));

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        public ActionResult GenderProcessWork(string queueId, string pageName = "")
        {
            long caseId = URLEncoderDecoder.Decode(queueId).ToInt64();
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                return View(PGetProcessWork(caseId));

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }
        }

        public ActionResult DOBProcessWork(string queueId, string pageName = "")
        {
            long caseId = URLEncoderDecoder.Decode(queueId).ToInt64();
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                return View(PGetProcessWork(caseId));

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", new { Error = ex.ToString() });
            }

        }
        public ActionResult GetQueueForEdit(long DisCategory)
        {
            DOGEN_EligibilityActions objDOGEN_EligibilityActions = new DOGEN_EligibilityActions();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                string actionViewName = string.Empty;
                actionViewName = "_EditAndInitiateWorkflow";
                objDOGEN_EligibilityActions.lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, DisCategory).Where(xx => xx.IsActive == true && xx.GroupingLookupMasterRef == QueueProgressType.Processing.ToInt64()).ToList();
                //removing pended Queues
                objDOGEN_EligibilityActions.lstQueue = objDOGEN_EligibilityActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_EligibilityActions.lstQueue = objDOGEN_EligibilityActions.lstQueue.Where(xx => !Enum.GetValues(typeof(AuditFailedQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_EligibilityActions.lstQueue = objDOGEN_EligibilityActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingAudit)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();

                return PartialView(actionViewName, objDOGEN_EligibilityActions);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return PartialView("");
            }
        }
        public ActionResult GetActionsByActionID(long actionID, long DisCategory, string genQueueid = "")
        {
            DOGEN_EligibilityActions objDOGEN_EligibilityActions = new DOGEN_EligibilityActions();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            
            try
            {
                string actionViewName = string.Empty;
                if (!genQueueid.IsNullOrEmpty())
                {
                    //Check the tempdata is null or not if Null force the user to cancel the operation
                    if (TempData[genQueueid].IsNull())
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, "Case was opened in Multiple windows", "Case was opened in Multiple windows");
                        return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = ConstantTexts.MsgForMultipleWindowOpenedCases });
                    }
                    //load the genQueue data to fill Action details
                    objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueid];
                    TempData.Keep(genQueueid);
                }
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                switch (actionID)
                {
                    case (long)PWActionsEnum.AddComments:
                        actionViewName = "_AddComments";
                        break;
                    case (long)PWActionsEnum.CloseCase:
                        if (DisCategory == (long)DiscripancyCategory.Eligibility)
                        {
                            actionViewName = "_CloseCase";
                            objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, DisCategory);
                            objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, DisCategory);
                        }
                        else
                        {
                            actionViewName = "_CloseCase";
                            ViewBag.DisCat = "disCat";
                        }
                        break;
                    case (long)PWActionsEnum.CloseCaseNoAction:
                        if (DisCategory == (long)DiscripancyCategory.Eligibility)
                        {
                            actionViewName = "_CloseCase";
                            objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCaseNoAction, DisCategory);
                            objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCaseNoAction, DisCategory);
                        }
                        else
                        {
                            actionViewName = "_CloseCase";
                            ViewBag.DisCat = "disCat";
                        }
                        break;
                    case (long)PWActionsEnum.PeerAuditCompleted:
                        actionViewName = "_PeerAuditCompleted";
                        objDOGEN_EligibilityActions.lstContainsError = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        break;
                    case (long)PWActionsEnum.PendCase:
                        actionViewName = "_PendCase";
                        objDOGEN_EligibilityActions.lstPendReason = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsPendReason, (long)PWActionsEnum.PendCase, DisCategory);
                        break;
                    case (long)PWActionsEnum.SendtoPeerAudit:
                        actionViewName = "_SendToPeerAudit";
                        break;
                    case (long)PWActionsEnum.UpdateCMSEligibility:

                        objDOGEN_EligibilityActions.HICN = objDOGEN_Queue.MemberCurrentHICN;
                        objDOGEN_EligibilityActions.LastName = objDOGEN_Queue.MemberLastName;
                        objDOGEN_EligibilityActions.DateofBirth = objDOGEN_Queue.MemberDOB;
                        objDOGEN_EligibilityActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_EligibilityActions.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        objDOGEN_EligibilityActions.lstTransactionTypeCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionTypeCode);
                        objDOGEN_EligibilityActions.lstElectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ElectionType);
                        objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.UpdateCMSEligibility, DisCategory);
                        objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.UpdateCMSEligibility, DisCategory);
                        objDOGEN_EligibilityActions.ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup;
                        objDOGEN_EligibilityActions.PBPLkup = objDOGEN_Queue.MemberPBPLkup;
                        actionViewName = "_UpdateCMSEligibility";
                        break;
                    case (long)PWActionsEnum.UpdatePlan:
                        objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.UpdatePlan, DisCategory);
                        objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.UpdatePlan, DisCategory);
                        actionViewName = "_UpdatePlan";
                        break;
                    case (long)PWActionsEnum.UpdatePlan_CreateRPR:
                        string errorMessage = string.Empty;
                        List<DOADM_UserMaster> lstUsers;
                        ExceptionTypes result;
                        DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                        objDOADM_UserMaster.IsActive = true;
                        BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                        objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.UpdatePlan_CreateRPR, DisCategory);
                        objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.UpdatePlan_CreateRPR, DisCategory);
                        objDOGEN_EligibilityActions.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                        objDOGEN_EligibilityActions.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                        result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                        objDOGEN_EligibilityActions.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();
                        actionViewName = "_UpdatePlanAndCreateRPR";
                        break;
                    case (long)PWActionsEnum.CloseCase_CreateRPR:
                        string errorMsg = string.Empty;
                        List<DOADM_UserMaster> lstUser;
                        DOADM_UserMaster objDOADM_UserMasters = new DOADM_UserMaster();
                        objDOADM_UserMasters.IsActive = true;
                        BLUserAdministration objBLUserAdministrations = new BLUserAdministration();
                        objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase_CreateRPR, DisCategory);
                        objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase_CreateRPR, DisCategory);
                        objDOGEN_EligibilityActions.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                        objDOGEN_EligibilityActions.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                        result = objBLUserAdministrations.SearchUser(TimeZone, objDOADM_UserMasters, out lstUser, out errorMessage);
                        objDOGEN_EligibilityActions.lstUsers = lstUser.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();
                        actionViewName = "_CloseCaseAndCreateRPR";
                        break;
                    case (long)PWActionsEnum.UpdateGPS:
                        actionViewName = "_UpdateGPS";
                        break;
                    case (long)PWActionsEnum.ReOpenAddComments:
                        actionViewName = "_AddComments";
                        break;
                    case (long)PWActionsEnum.ReOpenCloseCase:
                        actionViewName = "_CloseCase";
                        objDOGEN_EligibilityActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, DisCategory);
                        objDOGEN_EligibilityActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, DisCategory);
                        break;
                    case (long)PWActionsEnum.CancelPendCase:
                        actionViewName = "_CancelPendCase";
                        break;

                }
                return PartialView(actionViewName, objDOGEN_EligibilityActions);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return PartialView("");
            }
        }
        private DOGEN_Queue PGetEligibilityCreateCase(long discCategoryID)
        {
            // Bind Dropdown List for Discrepancy Catgory,Type,LOB  and PBP.
            string errorMessage = string.Empty;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();

            long WorkBasketID = (long)WorkBasket.GPSvsMMR;

            objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, WorkBasketID);

            objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, discCategoryID);

            objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
            objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
            objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
            objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
            objDOGEN_Queue.DiscrepancyCategoryLkup = discCategoryID;
            objDOGEN_Queue.lstOOAFlag = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
            objDOGEN_Queue.lstGender = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Gender);
            objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
            return objDOGEN_Queue;
        }

        [HttpPost]
        public ActionResult SaveAction(DOGEN_EligibilityActions objDOGEN_EligibilityActions)
        {
            string errorMessage = string.Empty;
            string returnMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            BLEligibility objBLEligibility = new BLEligibility();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            long loginUserId = currentUser.ADM_UserMasterId;
            BLCommon objCommon = new BLCommon();
            returnMessage = "Record updated successfully.";
            string genQueueId = string.Empty;
            try
            {
                if (!objCommon.ValidateLockBeforeSave(loginUserId, (long)ScreenType.Queue, objDOGEN_EligibilityActions.GEN_QueueRef.ToLong()))
                {
                    errorMessage = "Record not locked";
                    result = ExceptionTypes.UnlockedRecord;
                    return Json(new { ID = result, Message = errorMessage });
                }
                //Hold the prev GenQueueData
                genQueueId = objDOGEN_EligibilityActions.GEN_QueueRef.NullToString();
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueId];
                TempData.Keep(genQueueId);
                //
                objDOGEN_EligibilityActions.LoginID = loginUserId;
                objDOGEN_EligibilityActions.RoleLKup = currentUser.RoleLkup;
                objDOGEN_EligibilityActions.GEN_QueueRef = objDOGEN_Queue.GEN_QueueId;
                objDOGEN_EligibilityActions.HICN = objDOGEN_EligibilityActions.HICN != null ? (objDOGEN_EligibilityActions.HICN) : objDOGEN_Queue.MemberCurrentHICN;
                objDOGEN_EligibilityActions.LastName = objDOGEN_EligibilityActions.LastName != null ? (objDOGEN_EligibilityActions.LastName) : objDOGEN_Queue.MemberLastName;
                objDOGEN_EligibilityActions.DateofBirth = objDOGEN_EligibilityActions.DateofBirth != null ? (objDOGEN_EligibilityActions.DateofBirth) : objDOGEN_Queue.MemberDOB;
                objDOGEN_EligibilityActions.ContractIDLkup = objDOGEN_EligibilityActions.ContractIDLkup != null ? (objDOGEN_EligibilityActions.ContractIDLkup) : objDOGEN_Queue.MemberContractIDLkup;
                objDOGEN_EligibilityActions.PBPLkup = objDOGEN_EligibilityActions.PBPLkup != null ? (objDOGEN_EligibilityActions.PBPLkup) : objDOGEN_Queue.MemberPBPLkup;
                objDOGEN_EligibilityActions.IsActive = true;
                objDOGEN_EligibilityActions.LastUpdatedByRef = loginUserId;

                if (objDOGEN_EligibilityActions.ActionLkup == (long)PWActionsEnum.UpdateCMSEligibility)
                {
                    DOCMSPostTransaction(objDOGEN_EligibilityActions, out errorMessage);
                    if (!errorMessage.IsNullOrEmpty())
                    {
                        BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                        //return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = errorMessage, TrrCode = CMSTransactionCode.TRR76.NullToString() });
                        objDOGEN_EligibilityActions.CMSTransactionStatusLkup = (long)CMSTransactionStatus.Failure;
                        result = objBLEligibility.SaveAction(objDOGEN_EligibilityActions, out errorMessage);
                        return Json(new { ID = (long)ExceptionTypes.UnknownError, Message = errorMessage, TrrCode = CMSTransactionCode.TRR76.NullToString() });
                    }
                    else
                    {
                        objDOGEN_EligibilityActions.CMSTransactionStatusLkup = (long)CMSTransactionStatus.Success;
                        result = objBLEligibility.SaveAction(objDOGEN_EligibilityActions, out errorMessage);
                    }
                }
                else
                {
                    result = objBLEligibility.SaveAction(objDOGEN_EligibilityActions, out errorMessage);
                }

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB.", TrrCode = "" });
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
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return Json(new { ID = result, Message = "An error occured while performing Save action.", TrrCode = "" });
            }
        }

        internal DOGEN_Queue PGetProcessWork(long genQueueID = 0)
        {
            BLCommon objBLCommon = new BLCommon();
            DOGEN_ManageCases objDOGEN_ManageCases = new DOGEN_ManageCases();
            objDOGEN_ManageCases.GEN_QueueRef = genQueueID;
            objDOGEN_ManageCases.ActionPerformedLkup = (long)ActionLookup.View;
            objDOGEN_ManageCases.CurrentUserRef = currentUser.ADM_UserMasterId;
            objDOGEN_ManageCases.CreatedByRef = currentUser.ADM_UserMasterId;
            objBLCommon.InsertManageCase(objDOGEN_ManageCases, out string ErrorMessage);
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            errorMessage = string.Empty;
            try
            {
                BLEligibility objBLEligibility = new BLEligibility();
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                ExceptionTypes result = objBLEligibility.GetGenQueueByID(TimeZone, genQueueID, out objDOGEN_Queue);
                if (objDOGEN_Queue.MostRecentStatusLkup == (long)CurrentStatusLkup.ResolvedComplted)
                {
                    objDOGEN_Queue.lstOptionsforReopen = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Reopen);
                    objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)objDOGEN_Queue.MostRecentWorkQueueLkup);
                    // objDOGEN_Queue.lstMostRecentStatus = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, categoryID).Where(xx => xx.IsActive == true && xx.GroupingLookupMasterRef == QueueProgressType.Processing.ToInt64()).ToList();
                    //        //removing pended Queues
                    //        objDOGEN_Queue.lstMostRecentStatus = = lstQueue.Where(xx => !Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();

                }
                else
                {
                    objDOGEN_Queue.lstOptionsforReopen = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Reopen);
                    if (objDOGEN_Queue.IsCasePended)
                    {
                        objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)objDOGEN_Queue.PreviousWorkQueueLkup);
                        List<DOCMN_LookupMasterCorrelations> CancelPendedQueue = ((DynamicCancelPendCase[])Enum.GetValues(typeof(DynamicCancelPendCase))).Select(c => new DOCMN_LookupMasterCorrelations() { CMN_LookupMasterChildRef = (long)c, LookupMasterChildValue = c.ToString().Replace("_", " ") }).ToList();
                        objDOGEN_Queue.lstActions.AddRange(CancelPendedQueue);
                    }
                    else
                        objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)objDOGEN_Queue.MostRecentWorkQueueLkup);
                }
                //long lCurrentQueue = (long)objDOGEN_Queue.MostRecentWorkQueueLkup;
                //if (lCurrentQueue == (long)EligibilityQueue.EligPended ||
                //    lCurrentQueue == (long)DOBQueue.DOBPended ||
                //    lCurrentQueue == (long)GenderQueue.GenderPended)
                //{
                //    lCurrentQueue = (long)objDOGEN_Queue.PreviousWorkQueueLkup;
                //}
                //objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, lCurrentQueue);



                //service Call
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
                TempData[genQueueID.NullToString()] = objDOGEN_Queue;
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }

        public static DateTime FirstDayOfNextMonth(DateTime dt)
        {
            DateTime ss = new DateTime(dt.Year, dt.Month, 1);
            return ss.AddMonths(1);
        }

        public static DateTime LastDayOfNextMonth(DateTime dt)
        {
            DateTime ss = new DateTime(dt.Year, dt.Month, 30);
            return ss.AddMonths(1);
        }


        private void DOCMSPostTransaction(DOGEN_EligibilityActions objDOGEN_EligibilityActions, out string errorMessage)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            errorMessage = string.Empty;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            string genQueueId = string.Empty;
          
            try
            {
                genQueueId = objDOGEN_EligibilityActions.GEN_QueueRef.NullToString();
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueId];
                TempData.Keep(genQueueId);

                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = objDOGEN_EligibilityActions.GEN_QueueRef.ToString();
                var contractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == objDOGEN_EligibilityActions.ContractIDLkup).FirstOrDefault();
                var pbpNo = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == objDOGEN_EligibilityActions.PBPLkup).FirstOrDefault();
                objDOGEN_GPSServiceRequestParameter.ApplicationDate = !objDOGEN_EligibilityActions.ApplicationDate.IsNull() ? objDOGEN_EligibilityActions.ApplicationDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.BirthDate = !objDOGEN_EligibilityActions.DateofBirth.IsNull() ? objDOGEN_EligibilityActions.DateofBirth.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.ContractNumber = !contractNumber.IsNull() ? contractNumber.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = !objDOGEN_EligibilityActions.EffectiveDate.IsNull() ? objDOGEN_EligibilityActions.EffectiveDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = objDOGEN_EligibilityActions.HICN;
                objDOGEN_GPSServiceRequestParameter.LastName = !objDOGEN_EligibilityActions.LastName.IsNull() ? objDOGEN_EligibilityActions.LastName : string.Empty;
                objDOGEN_GPSServiceRequestParameter.PbpNo = !pbpNo.IsNull() ? pbpNo.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();
                objGPSServiceGetMethods.CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.EligibilityCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
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
                    DiscrepancyStartDate = objDOGEN_Queue.DiscrepancyStartDate,
                    DiscrepancyCategoryLkup = objDOGEN_Queue.DiscrepancyCategoryLkup,
                    WorkBasketLkup = objDOGEN_Queue.WorkBasketLkup,
                    DiscrepancyTypeLkup = objDOGEN_Queue.DiscrepancyTypeLkup,
                    StatusNot = (long)CurrentStatusLkup.ResolvedComplted
                };
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
                return isDuplicate;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTProcessWorkflow, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                throw ex;
            }
        }
    }
}