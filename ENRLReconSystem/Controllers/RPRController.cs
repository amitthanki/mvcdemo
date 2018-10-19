using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Models;
using ENRLReconSystem.Utility;
using ENRLReconSystem.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace ENRLReconSystem.Controllers
{
    [ERSAuthenticationAttribute(Roles = "User")]
    [Common.Filter(WorkBasketLkup = (long)WorkBasket.RPR)]
    [ValidateInput(false)]
    public class RPRController : Controller
    {
        private UIUserLogin currentUser;
        private BLReports _objBLReports;
        BLRPR _objBLRPR = new BLRPR();
        public RPRController()
        {
            if (System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] != null)
            {
                currentUser = System.Web.HttpContext.Current.Session[ConstantTexts.CurrentUserSessionKey] as UIUserLogin;
            }
        }
        USPSService _USPSService = new USPSService("641UNITE1062");
        string errorMessage = string.Empty;
        // GET: RPR
        public ActionResult Create()
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            long? WorkBasketLkp = currentUser.WorkBasketLkup;
            long CategoryType = (long)DiscripancyCategory.RPR;
            string errorMessage = string.Empty;
            ExceptionTypes result;
            try
            {

                #region Dropdowns Binding
                List<DOADM_UserMaster> lstUsers;
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                objDOADM_UserMaster.IsActive = true;
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                objDOGEN_Queue.ComplianceStartDate = DateTime.UtcNow;
                objDOGEN_Queue.DiscrepancyStartDate = objDOGEN_Queue.ComplianceStartDate.Value.AddMonths(1);
                objDOGEN_Queue.DiscrepancyStartDate = new DateTime(objDOGEN_Queue.DiscrepancyStartDate.Value.Year, objDOGEN_Queue.DiscrepancyStartDate.Value.Month,1);
                objDOGEN_Queue.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.FullName).ToList();//Filtered 1st three Users as Admin.sort list by email id
                objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                objDOGEN_Queue.lstLob = objDOGEN_Queue.lstLob.Where(xx => xx.CMN_LookupMasterId != 31004).ToList();
                objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, WorkBasketLkp);
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                objDOGEN_Queue.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                objDOGEN_Queue.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, CategoryType);
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                #endregion
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                return View(objDOGEN_Queue);
            }
            catch (Exception ex)
            {

                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Create(DOGEN_Queue objDOGEN_Queue,HttpPostedFileBase rprAttachments=null)
        {
            string errorMessage = string.Empty;
            long loginUserId = currentUser.ADM_UserMasterId;
            string returnMessage = "";
            ExceptionTypes result = new ExceptionTypes();
            long lNewQueue = 0;
            long? lRprCategory = null;
            long lCaseID=0;
            string employerId = string.Empty;
            if (objDOGEN_Queue.DiscrepancyTypeLkup == (long)DiscripancyType.SCCRPR)
                lNewQueue = (long)RPRQueue.RPRInitialSCCRPR;
            else
                lNewQueue = PgetNewQueue(objDOGEN_Queue.RPRCTMMember, objDOGEN_Queue.RPRRequestedEffectiveDate.Value,out lRprCategory);
            try
            {
                //check duplicate logic
                if (CheckDuplicateLogic(objDOGEN_Queue, out lCaseID, out errorMessage))
                {
                    if (lCaseID > 0)
                    {
                        string strMessage = "Case with same details exits. Case Id: " + lCaseID;
                        return Json(new { ID = (long)ExceptionTypes.DuplicateRecord, Message = strMessage ,Gen_QueueId = 0 });
                    }
                }
                
                if (objDOGEN_Queue.GEN_QueueId > 0)
                {
                    objDOGEN_Queue.IsChildCase = true;
                    objDOGEN_Queue.ParentQueueRef = objDOGEN_Queue.GEN_QueueId;
                }
                List<DOGEN_Attachments> lstDOGEN_Attachments = new List<DOGEN_Attachments>();
                objDOGEN_Queue.DiscrepancyCategoryLkup = (long)DiscripancyCategory.RPR;
                objDOGEN_Queue.CreatedByRef = loginUserId;
                objDOGEN_Queue.CurrentUserRef = loginUserId;
                objDOGEN_Queue.LoginUserId = loginUserId;
                objDOGEN_Queue.RoleLkup = currentUser.RoleLkup;
                objDOGEN_Queue.LastUpdatedByRef = loginUserId;
                //objDOGEN_Queue.BusinessSegmentLkup = (long)currentUser.BusinessSegmentLkup;
                objDOGEN_Queue.WorkBasketLkup = (long)WorkBasket.RPR;
                objDOGEN_Queue.IsCasePended = false;
                objDOGEN_Queue.MemberDOBId = objDOGEN_Queue.MemberDOB != null ? Convert.ToInt64(((DateTime)objDOGEN_Queue.MemberDOB).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.DiscrepancyStartDateId = objDOGEN_Queue.DiscrepancyStartDate != null ? Convert.ToInt64(((DateTime)objDOGEN_Queue.DiscrepancyStartDate).ToString("yyyyMMdd")):(long?)null;
                objDOGEN_Queue.RPRRequestedEffectiveDateId = objDOGEN_Queue.RPRRequestedEffectiveDate != null ? Convert.ToInt64(((DateTime)objDOGEN_Queue.RPRRequestedEffectiveDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.SCCRPRRequstedSubmissionDateId = objDOGEN_Queue.SCCRPRRequstedSubmissionDate != null ? Convert.ToInt64(((DateTime)objDOGEN_Queue.SCCRPRRequstedSubmissionDate).ToString("yyyyMMdd")) : (long?)null;
                objDOGEN_Queue.IsCaseResolved = false;
                objDOGEN_Queue.IsParentCase = false;
                objDOGEN_Queue.IsChildCase = false;
                objDOGEN_Queue.OOAFlagLkup = false;
                objDOGEN_Queue.IsActive = true;
                objDOGEN_Queue.ActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.ActionPerformedLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.CurrentActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.CurrentStatusLkup = (long)CurrentStatusLkup.New;
                objDOGEN_Queue.CurrentWorkQueuesLkup = lNewQueue;
                objDOGEN_Queue.RPRCategoryLkup = lRprCategory;
                objDOGEN_Queue.MostRecentActionLkup = (long)ActionLookup.Save;
                objDOGEN_Queue.MostRecentWorkQueueLkup = lNewQueue;
                objDOGEN_Queue.MostRecentStatusLkup = (long)CurrentStatusLkup.New;
                objDOGEN_Queue.DateofBirth = objDOGEN_Queue.MemberDOB;
                objDOGEN_Queue.LastName = objDOGEN_Queue.MemberLastName;
                objDOGEN_Queue.ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup;
                objDOGEN_Queue.PBPLkup = objDOGEN_Queue.MemberPBPLkup;
                objDOGEN_Queue.SourceSystemLkup = objDOGEN_Queue.CommentsSourceSystemLkup;
                //Check for attchments and add it in DOGEN_Attachments
                //if (objDOGEN_Queue.Attachment != null && !string.IsNullOrEmpty(objDOGEN_Queue.Attachment.FileName))
                if (!rprAttachments.IsNull() && !string.IsNullOrEmpty(rprAttachments.FileName))
                {
                    string tempfilePath = string.Empty;
                    string actualFilePath = string.Empty;
                    if (SaveFileToTempFolder(rprAttachments, out tempfilePath))
                    {
                        if (SaveFileToActualFolder(tempfilePath, out actualFilePath))
                        {
                            string[] arrFileName = rprAttachments.FileName.Split('\\');
                            DOGEN_Attachments objDOGEN_Attachments = new DOGEN_Attachments();
                            objDOGEN_Attachments.slno = 0;
                            objDOGEN_Attachments.GEN_AttachmentsId = 0;
                            objDOGEN_Attachments.GEN_QueueRef = 0;
                            objDOGEN_Attachments.FileName = arrFileName[arrFileName.Length - 1]; ;
                            objDOGEN_Attachments.UploadedFileName = Path.GetFileName(actualFilePath);
                            objDOGEN_Attachments.FilePath = actualFilePath;
                            objDOGEN_Attachments.IsActive = true;
                            objDOGEN_Attachments.UTCCreatedOn = DateTime.UtcNow;
                            objDOGEN_Attachments.CreatedBy = currentUser.FullName;
                            objDOGEN_Attachments.CreatedByRef = currentUser.ADM_UserMasterId;
                            objDOGEN_Attachments.UTCLastUpdatedOn = DateTime.UtcNow;
                            objDOGEN_Attachments.LastUpdatedBy = currentUser.FullName;
                            objDOGEN_Attachments.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                            lstDOGEN_Attachments.Add(objDOGEN_Attachments);
                        }
                    }
                }

                //Restricted case logic
                CommonController objCommonController = new CommonController();
                result = objCommonController.IsOnshoreOnly(objDOGEN_Queue.GPSHouseholdID, out bool isRetricted, out employerId);
                if(result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, "Error getting IsRestricted vlaue for new Case", objDOGEN_Queue.GPSHouseholdID);
                objDOGEN_Queue.IsRestricted = isRetricted;
                objCommonController = null;

                result = _objBLRPR.Create(objDOGEN_Queue, lstDOGEN_Attachments, out errorMessage);

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

                if (objDOGEN_Queue.IsRestricted)
                    returnMessage = "<b>Onshore Only Restricted </b> " + "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b>";
                else
                    returnMessage = "Case created successfully. Id : <b>" + objDOGEN_Queue.GEN_QueueId + "</b> </br> with Business segment : <b>" + objDOGEN_Queue.BusinessSegment + "</b>";
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
                }
                return Json(new { ID = result, Message = returnMessage, Gen_QueueId = objDOGEN_Queue.GEN_QueueId });
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = result, Message = "An error occured while updating DB.", Gen_QueueId = 0 });
            }
        }

        /// <summary>
        /// Duplicate Logic For RPR
        /// </summary>
        /// <param name="objDOGEN_Queue"></param>
        /// <param name="lCaseID"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        private bool CheckDuplicateLogic(DOGEN_Queue objDOGEN_Queue, out long lCaseID, out string errorMessage)
        {
            bool isDuplicate = false;
            ExceptionTypes result = new ExceptionTypes();
            errorMessage = string.Empty;
            lCaseID = 0;
            try
            {
                result = _objBLRPR.CheckDuplicate(objDOGEN_Queue.MemberCurrentHICN, objDOGEN_Queue.MemberContractIDLkup, objDOGEN_Queue.RPRRequestedEffectiveDate, objDOGEN_Queue.RPRActionRequestedLkup, out lCaseID);

                if (objDOGEN_Queue.GEN_QueueId.ToInt64() > 0 && lCaseID == objDOGEN_Queue.GEN_QueueId)//While Cloning Check for Duplicate
                {
                    //Escape for Cloning
                    isDuplicate = false;
                }
                else if (lCaseID > 0)
                {
                    isDuplicate = true;

                }
                return isDuplicate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private long PgetNewQueue(bool bRPRCTMMember, DateTime dRPRRequestedEffectiveDate, out long? lRprCategory)
        {
            DateTime today = DateTime.Today;
            lRprCategory = (long)RPRCategory.Category2;
            if (bRPRCTMMember)
            {
                return (long)RPRQueue.RPRRequestCategory2CTM;
            }

            if (dRPRRequestedEffectiveDate > today)
            {
                return (long)RPRQueue.RPRRequestCategory2;
            }

            
            int months = (dRPRRequestedEffectiveDate.Month - today.Month) + 12 * (dRPRRequestedEffectiveDate.Year - today.Year);
            if (months <= -3)
            {
                lRprCategory = (long)RPRCategory.Category3;
                return (long)RPRQueue.RPRRequestCategory3;
            }
            else
                return (long)RPRQueue.RPRRequestCategory2;
        }

        public ActionResult RPRProcessWork(string queueId,string pageName="") 
        {
            long caseId = URLEncoderDecoder.Decode(queueId).ToInt64();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSData objDOGEN_GPSData = new DOGEN_GPSData();
            BLCommon objBLCommon = new BLCommon();
            DOGEN_ManageCases objDOGEN_ManageCases = new DOGEN_ManageCases();
            objDOGEN_ManageCases.GEN_QueueRef = caseId;
            objDOGEN_ManageCases.ActionPerformedLkup = (long)ActionLookup.View;
            objDOGEN_ManageCases.CurrentUserRef = currentUser.ADM_UserMasterId;
            objDOGEN_ManageCases.CreatedByRef = currentUser.ADM_UserMasterId;
            objBLCommon.InsertManageCase(objDOGEN_ManageCases, out string ErrorMessage);
            long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
            try
            {
                if (pageName != "")
                    ViewBag.PageName = pageName;
                string errorMessage = string.Empty;
                ExceptionTypes result = _objBLRPR.GetGenQueueByID(TimeZone, caseId, out objDOGEN_Queue, out errorMessage);
                objDOGEN_Queue.lstOptionsforReopen = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Reopen);

                objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, objDOGEN_Queue.MostRecentWorkQueueLkup);
                if (objDOGEN_Queue.IsCasePended)
                {
                    objDOGEN_Queue.lstActions = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.QueueVsAction, (long)objDOGEN_Queue.PreviousWorkQueueLkup);
                    List<DOCMN_LookupMasterCorrelations> CancelPendedQueue = ((DynamicCancelPendCase[])Enum.GetValues(typeof(DynamicCancelPendCase))).Select(c => new DOCMN_LookupMasterCorrelations() { CMN_LookupMasterChildRef = (long)c, LookupMasterChildValue = c.ToString().Replace("_", " ") }).ToList();
                    objDOGEN_Queue.lstActions.AddRange(CancelPendedQueue);
                }

                TempData[caseId.NullToString()] = objDOGEN_Queue;//Keeping the object to pupulate fields in GetActionsFormByActionID method
                TempData.Keep(caseId.NullToString());

                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
                return View(objDOGEN_Queue);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }          
        }

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
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized,ex.ToString(),ex.ToString());
                throw;
            }

        }

        public ActionResult GetQueueForEdit(long DisCategory, long CaseCategory = 0)
        {
            DOGEN_RPRActions objDOGEN_RPRActions = new DOGEN_RPRActions();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                string actionViewName = string.Empty;
                actionViewName = "_EditAndInitiateWorkflow";
                objDOGEN_RPRActions.lstQueue = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVQueue, DisCategory).Where(xx => xx.IsActive == true && xx.GroupingLookupMasterRef == QueueProgressType.Processing.ToInt64()).ToList();
                //removing pended Queues
                objDOGEN_RPRActions.lstQueue = objDOGEN_RPRActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_RPRActions.lstQueue = objDOGEN_RPRActions.lstQueue.Where(xx => !Enum.GetValues(typeof(AuditFailedQueues)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                objDOGEN_RPRActions.lstQueue = objDOGEN_RPRActions.lstQueue.Where(xx => !Enum.GetValues(typeof(PendingAudit)).Cast<long>().ToList().Contains(xx.CMN_LookupMasterChildRef.ToInt64())).ToList();
                if (CaseCategory == (long)RPRCategory.Category2 || CaseCategory == 0)
                {
                    objDOGEN_RPRActions.lstQueue.RemoveAll(x => x.CMN_LookupMasterChildRef == (long)RPRQueue.RPRRequestCategory3);
                    objDOGEN_RPRActions.lstQueue.RemoveAll(x => x.CMN_LookupMasterChildRef == (long)RPRQueue.RPRSubmissionCategory3);
                }
                else
                {
                    objDOGEN_RPRActions.lstQueue.RemoveAll(x => x.CMN_LookupMasterChildRef == (long)RPRQueue.RPRRequestCategory2);
                    objDOGEN_RPRActions.lstQueue.RemoveAll(x => x.CMN_LookupMasterChildRef == (long)RPRQueue.RPRSubmissionCategory2);
                }

                return PartialView(actionViewName, objDOGEN_RPRActions);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRGetQueue, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return PartialView("");
            }
        }
        [HttpPost]
        public ActionResult GetActionsFormByActionID(PWActionsEnum actionID, string genQueueid = "")
        {
            string actionViewName = string.Empty;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
           
            DOGEN_RPRActions objDOGEN_RPRActions = new DOGEN_RPRActions();
            objDOGEN_RPRActions.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();
            try
            {
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
                switch (actionID)
                {
                    case PWActionsEnum.UpdatePlan:
                        actionViewName = "_UpdatePlan";
                        objDOGEN_RPRActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        objDOGEN_RPRActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        break;
                    case PWActionsEnum.PendCase:
                        actionViewName = "_PendCase";
                        objDOGEN_RPRActions.lstPendReasons = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsPendReason,(long)PWActionsEnum.PendCase, (long)DiscripancyCategory.RPR);
                        //objDOGEN_RPRActions.lstAdjustedCreateDateReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AdjustedCreateDateReason);
                       // objDOGEN_RPRActions.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                      //  objDOGEN_RPRActions.lstSubmissionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SubmissionType);
                        //objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                       // objDOGEN_RPRActions.lstTransactionTypeCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionTypeCode);
                       // objDOGEN_RPRActions.lstVerifiedRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.VerifiedRootCause);
                       // objDOGEN_RPRActions.lstExplanationOfRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ExplanationoftheRootCause);
                       // objDOGEN_RPRActions.lstElectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ElectionType);
                       // objDOGEN_RPRActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                      //  objDOGEN_RPRActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        break;
                    case PWActionsEnum.SendtoPeerAudit:
                        actionViewName = "_SendToPeerAudit";
                        objDOGEN_RPRActions.lstElectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ElectionType);
                        objDOGEN_RPRActions.lstAdjustedCreateDateReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AdjustedCreateDateReason);
                        objDOGEN_RPRActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.SendtoPeerAudit, (long)DiscripancyCategory.RPR);
                        objDOGEN_RPRActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.SendtoPeerAudit, (long)DiscripancyCategory.RPR);
                        objDOGEN_RPRActions.lstSubmissionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SubmissionType);
                        objDOGEN_RPRActions.lstTransactionTypeCode = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.TransactionTypeCode);
                        objDOGEN_RPRActions.lstVerifiedRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.VerifiedRootCause);
                        objDOGEN_RPRActions.lstExplanationOfRootCause = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ExplanationoftheRootCause);
                        objDOGEN_RPRActions.lstPlanError=CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        break;
                    case PWActionsEnum.PeerAuditCompleted:
                        actionViewName = "_PeerAuditCompleted";
                        objDOGEN_RPRActions.lstContainsErros = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.YesNo);
                        break;
                    case PWActionsEnum.CloseCase:
                        actionViewName = "_CloseCase";
                        objDOGEN_RPRActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        objDOGEN_RPRActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        break;
                    case PWActionsEnum.SubmitRPCRequest:
                        actionViewName = "_SubmitRPCRequest";
                        objDOGEN_RPRActions.RPCSubmissionDate = DateTime.Today.Date;//TO set todays date
                        //objDOGEN_RPRActions.lstElectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.ElectionType);
                        //when in CMS Account Manager sent Queue only then assign current date to CMSAccountManagerApprovalDate else null
                        objDOGEN_RPRActions.CMSAccountManagerApprovalDate = 
                            objDOGEN_Queue.MostRecentWorkQueueLkup == (long)QueueLookup.RPRCMSAccountManagerSent 
                            ? DateTime.Today.Date 
                            : (DateTime?)null;//Set todays date as default
                        break;
                    case PWActionsEnum.SubmittoAccountManager:
                        actionViewName = "_SubmitToAccountManager";
                        objDOGEN_RPRActions.CMSAccountManagerSubmissionDate = DateTime.Today.Date;//Set todays date as default
                        break;
                    case PWActionsEnum.RPCRejected:
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        actionViewName = "_RPCRejected";
                        break;
                    case PWActionsEnum.TransactionInquiryApproved:
                        actionViewName = "_TransactionInquiryApproved";
                        break;
                    case PWActionsEnum.AddComments:
                        actionViewName = "_AddComments";
                        break;
                    case PWActionsEnum.NeedTransactionInquiry:
                        actionViewName = "_NeedTransactionInquiry";
                        break;
                    case PWActionsEnum.FDRApproved:
                        actionViewName = "_FDRApproved";
                        objDOGEN_RPRActions.lstFDRStatus = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRStatus);
                        objDOGEN_RPRActions.lstFDRRejectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRRejectionType);
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup;
                        break;
                    case PWActionsEnum.Resubmission:
                        actionViewName = "_Resubmission";                        
                        objDOGEN_RPRActions.lstFDRRejectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRRejectionType);
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup;
                        break;
                    case PWActionsEnum.TrueRejection:
                        actionViewName = "_TrueRejection";
                        objDOGEN_RPRActions.lstFDRRejectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRRejectionType);
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup;
                        break;
                    case PWActionsEnum.SubmitTransactionInquiry:
                        actionViewName = "_SubmitTransactionInquiry";
                        objDOGEN_RPRActions.ActualSubmissionDate = objDOGEN_RPRActions.ActualSubmissionDate;
                        break;
                    case PWActionsEnum.RejectionOverturned:
                        actionViewName = "_RejectionOverturned";
                        objDOGEN_RPRActions.lstFDRRejectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRRejectionType);
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup;
                        break;
                    case PWActionsEnum.RejectionNotOverturned:
                        actionViewName = "_RejectionNotOverturned";
                        objDOGEN_RPRActions.lstFDRRejectionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.FDRRejectionType);
                        objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate;
                        objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived;
                        objDOGEN_RPRActions.FDRDescription = objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription;
                        objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup;
                        break;
                    case PWActionsEnum.SCCRPRRequired_SendSCCDeletiontoCMS:
                        actionViewName = "_SCCRPRRequired";
                        objDOGEN_RPRActions.LastName = objDOGEN_Queue.MemberLastName;
                        objDOGEN_RPRActions.DateofBirth = objDOGEN_Queue.MemberDOB;
                        objDOGEN_RPRActions.ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup;
                        objDOGEN_RPRActions.PBPLkup = objDOGEN_Queue.MemberPBPLkup;
                        objDOGEN_RPRActions.PBP = objDOGEN_Queue.MemberPBP;
                        // objDOGEN_RPRActions.ApplicationDate = objDOGEN_Queue.ApplicationDate;
                        objDOGEN_RPRActions.EffectiveDate = objDOGEN_Queue.RPRRequestedEffectiveDate;
                        //objDOGEN_RPRActions.EndDate = objDOGEN_Queue.objDOGEN_RPRActions.EndDate;
                        objDOGEN_RPRActions.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                        objDOGEN_RPRActions.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                        break;
                    case PWActionsEnum.IncorrectSCCInformation:
                        actionViewName = "_IncorrectSCCInformation";
                        break;
                    case PWActionsEnum.SubmitSCCRPCRequest:
                        actionViewName = "_SubmitSCCRPCRequest";
                        break;
                    case PWActionsEnum.TransactionInquiryRequired:
                        actionViewName = "_TransactionInquiryRequired";
                        break;
                    case PWActionsEnum.SubmittedTransactionInquiry:
                        actionViewName = "_SubmittedTransactionInquiry";
                        break;
                    case PWActionsEnum.ResubmitSCCRPCRequest:
                        actionViewName = "_ResubmitSCCRPCRequest";
                        break;
                    case PWActionsEnum.SCCRPRNotRequired:
                        actionViewName = "_SCCRPRNotRequired";
                        break;
                    case PWActionsEnum.SentSCCRPCResubmission:
                        actionViewName = "_SentSCCRPCResubmission";
                        break;
                    case PWActionsEnum.ReOpenAddComments:
                        actionViewName = "_AddComments";
                        break;
                    case PWActionsEnum.FDRRPRReceived:
                        actionViewName = "_FDRRPRReceived";
                        break;
                    case PWActionsEnum.FDRSCCRPRReceived:
                        actionViewName = "_SCCFDRRPRReceived";
                        break;
                    case PWActionsEnum.SendtoCategory2:
                        actionViewName = "_SendToCategory2";
                        break;
                    case PWActionsEnum.SendtoCategory3:
                        actionViewName = "_SendToCategory3";
                        break;
                    case PWActionsEnum.ReOpenCloseCase:
                        actionViewName = "_CloseCase";
                        objDOGEN_RPRActions.lstResolution = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsResolution, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        objDOGEN_RPRActions.lstRootCause = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscrepancyCategoryVsActionVsRootCause, (long)PWActionsEnum.CloseCase, (long)DiscripancyCategory.RPR);
                        break;
                    case PWActionsEnum.UpdateSubmissionType:
                        actionViewName = "_UpdateSubmissionType";
                        objDOGEN_RPRActions.lstSubmissionType = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.SubmissionType);
                        objDOGEN_RPRActions.lstAdjustedCreateDateReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AdjustedCreateDateReason);
                        break;
                    case PWActionsEnum.ReturnToRequestQueue:
                        actionViewName = "_ReturnToRequestQueue";
                        objDOGEN_RPRActions.lstAdjustedCreateDateReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AdjustedCreateDateReason);
                        break;
                    case PWActionsEnum.CancelPendCase:
                        actionViewName = "_CancelPendCase";
                        objDOGEN_RPRActions.lstAdjustedCreateDateReason = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.AdjustedCreateDateReason);
                        break;
                }
              
                return PartialView(actionViewName, objDOGEN_RPRActions);

            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return PartialView("");
            }
        }

        [HttpPost]
        public ActionResult SaveAction(DOGEN_RPRActions objDOGEN_RPRActions)
        {
            ExceptionTypes result = new ExceptionTypes();
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            string genQueueId = string.Empty;
            BLCommon objCommon = new BLCommon();
            string returnMessage = "Action Updated successfully.";
            string errorMessage = string.Empty;
            try
            {

                //Check record locked
                if (!objCommon.ValidateLockBeforeSave(currentUser.ADM_UserMasterId, (long)ScreenType.Queue, objDOGEN_RPRActions.GEN_QueueRef.ToLong()))
                {
                    ExceptionTypes result1 = new ExceptionTypes();
                    errorMessage = "Record not locked";
                    result1 = ExceptionTypes.UnlockedRecord;
                    return Json(new { ID = result1, Message = errorMessage });
                }
                //Hold the Prev gen data
                genQueueId = objDOGEN_RPRActions.GEN_QueueRef.NullToString();
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueId];
                TempData.Keep(genQueueId);
                ////////

                objDOGEN_Queue.objDOGEN_RPRActions.ActionLkup = objDOGEN_RPRActions.ActionLkup;

                //keeping old values for first time
                if (!(objDOGEN_Queue.objDOGEN_RPRActions.GEN_QueueRef.HasValue && objDOGEN_Queue.objDOGEN_RPRActions.GEN_QueueRef.Value > 0))
                {
                    objDOGEN_Queue.objDOGEN_RPRActions.GEN_QueueRef = objDOGEN_Queue.GEN_QueueId;
                    objDOGEN_Queue.objDOGEN_RPRActions.LastName = objDOGEN_Queue.MemberLastName;
                    objDOGEN_Queue.objDOGEN_RPRActions.DateofBirth = objDOGEN_Queue.MemberDOB;
                    objDOGEN_Queue.objDOGEN_RPRActions.ContractIDLkup = objDOGEN_Queue.MemberContractIDLkup;
                    objDOGEN_Queue.objDOGEN_RPRActions.PBPLkup = objDOGEN_Queue.MemberPBPLkup;
                    objDOGEN_Queue.objDOGEN_RPRActions.EffectiveDate = objDOGEN_Queue.RPRRequestedEffectiveDate;
                    objDOGEN_Queue.objDOGEN_RPRActions.EndDate = objDOGEN_Queue.EndDate;
                    objDOGEN_Queue.objDOGEN_RPRActions.RequestedSCC = objDOGEN_Queue.RequestedSCC;
                    objDOGEN_Queue.objDOGEN_RPRActions.RequestedZIP = objDOGEN_Queue.RequestedZIP;
                    objDOGEN_Queue.objDOGEN_RPRActions.ResubmissionDate = objDOGEN_Queue.ResubmissionDate;
                    objDOGEN_Queue.objDOGEN_RPRActions.IsActive = true; 
                }

                objDOGEN_Queue.objDOGEN_RPRActions.UTCLastUpdatedOn = DateTime.UtcNow;
                objDOGEN_Queue.objDOGEN_RPRActions.LastUpdatedByRef = currentUser.ADM_UserMasterId;
                objDOGEN_Queue.objDOGEN_RPRActions.LoginUserId = currentUser.ADM_UserMasterId;
                objDOGEN_Queue.objDOGEN_RPRActions.RoleLkup = currentUser.RoleLkup;
                objDOGEN_Queue.objDOGEN_RPRActions.Comments = objDOGEN_RPRActions.Comments;
                objDOGEN_Queue.objDOGEN_RPRActions.CommentsSourceSystemLkup = objDOGEN_RPRActions.CommentsSourceSystemLkup;
                objDOGEN_Queue.objDOGEN_RPRActions.ReopenQueueLKUP = objDOGEN_RPRActions.ReopenQueueLKUP;
                objDOGEN_Queue.objDOGEN_RPRActions.OptionLkup = objDOGEN_RPRActions.OptionLkup;


                if (objDOGEN_RPRActions.ActionLkup != null)
                {
                    PWActionsEnum actionID = (PWActionsEnum)objDOGEN_RPRActions.ActionLkup;

                    switch (actionID)
                    {
                        case PWActionsEnum.UpdatePlan:
                        case PWActionsEnum.CloseCase:
                            objDOGEN_Queue.objDOGEN_RPRActions.ResolutionLkup = objDOGEN_RPRActions.ResolutionLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.RootCauseLkup = objDOGEN_RPRActions.RootCauseLkup;
                            break;
                        case PWActionsEnum.SendtoPeerAudit:
                            objDOGEN_Queue.objDOGEN_RPRActions.ResolutionLkup = objDOGEN_RPRActions.ResolutionLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                            objDOGEN_Queue.objDOGEN_RPRActions.RootCauseLkup = objDOGEN_RPRActions.RootCauseLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.ElectionTypeLkup = objDOGEN_RPRActions.ElectionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.ApplicationDate = objDOGEN_RPRActions.ApplicationDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = objDOGEN_RPRActions.SubmissionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.PotentialSubmissionDate = objDOGEN_RPRActions.PotentialSubmissionDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.EffectiveDate = objDOGEN_RPRActions.EffectiveDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.TransactionTypeCodeLkup = objDOGEN_RPRActions.TransactionTypeCodeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.VerifiedRootCauseLkup = objDOGEN_RPRActions.VerifiedRootCauseLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.ExplanationOfRootCauseLkup = objDOGEN_RPRActions.ExplanationOfRootCauseLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.PlanError = objDOGEN_RPRActions.PlanError;
                            objDOGEN_Queue.objDOGEN_RPRActions.IncludeInTodaysSubmission = objDOGEN_RPRActions.IncludeInTodaysSubmission;
                            objDOGEN_Queue.objDOGEN_RPRActions.EndDate = objDOGEN_RPRActions.EndDate;
                            break;
                        case PWActionsEnum.PendCase:
                            objDOGEN_Queue.objDOGEN_RPRActions.PendReasonLkup = objDOGEN_RPRActions.PendReasonLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                            //objDOGEN_Queue.objDOGEN_RPRActions.PotentialSubmissionDate = objDOGEN_RPRActions.PotentialSubmissionDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = objDOGEN_RPRActions.SubmissionTypeLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.ResolutionLkup = objDOGEN_RPRActions.ResolutionLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.RootCauseLkup = objDOGEN_RPRActions.RootCauseLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.ElectionTypeLkup = objDOGEN_RPRActions.ElectionTypeLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.ApplicationDate = objDOGEN_RPRActions.ApplicationDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.EffectiveDate = objDOGEN_RPRActions.EffectiveDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.TransactionTypeCodeLkup = objDOGEN_RPRActions.TransactionTypeCodeLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.VerifiedRootCauseLkup = objDOGEN_RPRActions.VerifiedRootCauseLkup;
                            //objDOGEN_Queue.objDOGEN_RPRActions.ExplanationOfRootCauseLkup = objDOGEN_RPRActions.ExplanationOfRootCauseLkup;
                            break;
                        case PWActionsEnum.PeerAuditCompleted:
                            objDOGEN_Queue.objDOGEN_RPRActions.ContainsErrorsLkup = objDOGEN_RPRActions.ContainsErrorsLkup;
                            break;
                        case PWActionsEnum.SubmitRPCRequest:
                            objDOGEN_Queue.objDOGEN_RPRActions.CMSAccountManagerApprovalDate =
                                objDOGEN_Queue.MostRecentWorkQueueLkup == (long)QueueLookup.RPRCMSAccountManagerSent
                                ? objDOGEN_RPRActions.CMSAccountManagerApprovalDate
                                : objDOGEN_Queue.objDOGEN_RPRActions.CMSAccountManagerApprovalDate;//if current queue is RPRCMSAccountManagerSent only then update value
                            objDOGEN_Queue.objDOGEN_RPRActions.RPCSubmissionDate = objDOGEN_RPRActions.RPCSubmissionDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.IncludeInTodaysSubmission = objDOGEN_RPRActions.IncludeInTodaysSubmission;
                            break;
                        case PWActionsEnum.SubmittoAccountManager:
                            objDOGEN_Queue.objDOGEN_RPRActions.CMSAccountManagerSubmissionDate = objDOGEN_RPRActions.CMSAccountManagerSubmissionDate;
                            break;
                        case PWActionsEnum.FDRApproved:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup = objDOGEN_RPRActions.FDRStatusLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.OtherFDRStatus = objDOGEN_RPRActions.OtherFDRStatus;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.Resubmission:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionTypeLkup = objDOGEN_RPRActions.FDRRejectionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.TrueRejection:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionTypeLkup = objDOGEN_RPRActions.FDRRejectionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.RejectionOverturned:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionTypeLkup = objDOGEN_RPRActions.FDRRejectionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.RejectionNotOverturned:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionTypeLkup = objDOGEN_RPRActions.FDRRejectionTypeLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.SubmitTransactionInquiry:
                            objDOGEN_Queue.objDOGEN_RPRActions.ActualSubmissionDate = objDOGEN_RPRActions.ActualSubmissionDate;
                            break;
                        case PWActionsEnum.RPCRejected:
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = objDOGEN_RPRActions.FDRReceivedDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = objDOGEN_RPRActions.FDRCodeReceived;
                            objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = objDOGEN_RPRActions.FDRDescription;
                            break;
                        case PWActionsEnum.SCCRPRRequired_SendSCCDeletiontoCMS:
                            objDOGEN_Queue.objDOGEN_RPRActions.LastName = objDOGEN_RPRActions.LastName;
                            objDOGEN_Queue.objDOGEN_RPRActions.DateofBirth = objDOGEN_RPRActions.DateofBirth;
                            objDOGEN_Queue.objDOGEN_RPRActions.ContractIDLkup = objDOGEN_RPRActions.ContractIDLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.PBPLkup = objDOGEN_RPRActions.PBPLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.ApplicationDate = objDOGEN_RPRActions.ApplicationDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.EffectiveDate = objDOGEN_RPRActions.EffectiveDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.EndDate = objDOGEN_RPRActions.EndDate;
                            break;
                        case PWActionsEnum.SubmitSCCRPCRequest:
                            objDOGEN_Queue.objDOGEN_RPRActions.ActualSubmissionDate = objDOGEN_RPRActions.ActualSubmissionDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.ReasonSubmissionRejected = objDOGEN_RPRActions.ReasonSubmissionRejected;
                            //objDOGEN_Queue.objDOGEN_RPRActions.IncludeInTodaysSubmission = objDOGEN_RPRActions.IncludeInTodaysSubmission;
                            break;
                        case PWActionsEnum.SentSCCRPCResubmission:
                            objDOGEN_Queue.objDOGEN_RPRActions.ResubmissionDate = objDOGEN_RPRActions.ResubmissionDate;
                            //objDOGEN_Queue.objDOGEN_RPRActions.IncludeInTodaysSubmission = objDOGEN_RPRActions.IncludeInTodaysSubmission;
                            break;
                        case PWActionsEnum.ReOpenCloseCase:
                            objDOGEN_Queue.objDOGEN_RPRActions.ResolutionLkup = objDOGEN_RPRActions.ResolutionLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.RootCauseLkup = objDOGEN_RPRActions.RootCauseLkup;
                            break;
                        case PWActionsEnum.SendtoCategory2:
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = (long)SubmissionType.Category2;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = (long)AdjustedCreateDateReason.EligibilityIssue;
                            break;
                        case PWActionsEnum.SendtoCategory3:
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = (long)AdjustedCreateDateReason.EligibilityIssue;
                            objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = (long)SubmissionType.Category3;
                            break;
                        case PWActionsEnum.UpdateSubmissionType:
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                            objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = objDOGEN_RPRActions.SubmissionTypeLkup;
                            break;
                        case PWActionsEnum.ReturnToRequestQueue:
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                            break;
                        case PWActionsEnum.CancelPendCase:
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = objDOGEN_RPRActions.AdjustedCreateDate;
                            objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                            objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                            break;
                    }
                }

                result = _objBLRPR.SaveRPRActions(objDOGEN_Queue.objDOGEN_RPRActions, out errorMessage);
                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return Json(new { ID = result, Message = "An error occured while updating DB.", TrrCode = "" });
                }
                else
                {
                    //Clear the temp data if success
                    if (!TempData[genQueueId].IsNull())
                        TempData[genQueueId] = null;
                }
                //TempData.Remove("objDOGEN_Queue");
                return Json(new { ID = result, Message = returnMessage, TrrCode ="" });
            }
            catch (Exception ex)
            {
                //TempData.Remove("objDOGEN_Queue");
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                return Json(new { ID = 2, Message = "Error Call", TrrCode ="" });
            }
        }

        /// <summary>
        /// Private method to save a file to server temporary folder
        /// </summary>
        /// <param name="file"></param>
        /// <param name="tempfilePath"></param>
        /// <returns></returns>
        private bool SaveFileToTempFolder(HttpPostedFileBase file, out string tempFilePath)
        {
            bool isUploadSuccess = false;
            try
            {
                tempFilePath = string.Empty;
                string webServerTempPath =CacheUtility.GetMasterConfigurationByName(ConstantTexts.webServerTempPath);
                //path
                tempFilePath = Path.Combine(webServerTempPath, DateTime.Now.ToString("yyyyMMddHHmmss_") + Path.GetFileName(file.FileName));
                if (!Directory.Exists(webServerTempPath))
                {
                    Directory.CreateDirectory(webServerTempPath);
                }
                file.SaveAs(tempFilePath);
                isUploadSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isUploadSuccess;
        }

        /// <summary>
        /// Private method to save a attachment to actual folder path
        /// </summary>
        /// <param name="tempfilePath"></param>
        /// <param name="actualFilePath"></param>
        /// <returns></returns>
        private bool SaveFileToActualFolder(string filePath, out string newFilePath)
        {
            bool isScucess = false;
            string basePath = string.Empty;
            try
            {
                newFilePath =CacheUtility.GetMasterConfigurationByName(ConstantTexts.AttchmentFilePath);
                FileInfo fi = new FileInfo(filePath);
                basePath = newFilePath + DateTime.Now.Year + "\\" + DateTime.Now.Month + "\\" + DateTime.Now.Day;
                newFilePath = basePath + "\\" + fi.Name;
                if (!Directory.Exists(basePath))
                {
                    Directory.CreateDirectory(basePath);
                }
                fi.MoveTo(newFilePath);
                isScucess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isScucess;
        }

        private void DOCMSPostTransaction(DOGEN_RPRActions objDOGEN_RPRActions, out string errorMessage)
        {
            GPSServiceGetMethods objGPSServiceGetMethods = new GPSServiceGetMethods();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            errorMessage = string.Empty;
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            string genQueueId = string.Empty;
            try
            {
                genQueueId= objDOGEN_RPRActions.GEN_QueueRef.ToString();
                objDOGEN_Queue = (DOGEN_Queue)TempData[genQueueId];
                TempData.Keep(genQueueId);

                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = currentUser.ADM_UserMasterId;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = genQueueId;
                var contractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == objDOGEN_RPRActions.ContractIDLkup).FirstOrDefault();
                var pbpNo = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == objDOGEN_RPRActions.PBPLkup).FirstOrDefault();
                //objDOGEN_GPSServiceRequestParameter.ApplicationDate = !objDOGEN_RPRActions.ApplicationDate.IsNull() ? objDOGEN_RPRActions.ApplicationDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.BirthDate = !objDOGEN_RPRActions.DateofBirth.IsNull() ? objDOGEN_RPRActions.DateofBirth.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.ContractNumber = !contractNumber.IsNull() ? contractNumber.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = !objDOGEN_RPRActions.EffectiveDate.IsNull() ? objDOGEN_RPRActions.EffectiveDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = objDOGEN_Queue.MemberCurrentHICN;
                objDOGEN_GPSServiceRequestParameter.LastName = !objDOGEN_RPRActions.LastName.IsNull() ? objDOGEN_RPRActions.LastName : string.Empty;
                objDOGEN_GPSServiceRequestParameter.PbpNo = !pbpNo.IsNull() ? pbpNo.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();
                objDOGEN_GPSServiceRequestParameter.ERSCaseId = genQueueId;
                //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(objDOGEN_GPSServiceRequestParameter);
                //string URL = CacheUtility.GetMasterConfigurationByName(ConstantTexts.PostCMSTransaction);
                //errorMessage = CommonController.POSTCallWebAPI(URL, jsonString);
                objGPSServiceGetMethods.CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRProcessWorkflow, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// Method to get report url
        /// </summary>
        /// <returns></returns>
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


        public ActionResult UpdateCaseInfo(long discripancyCategory, long genQueueId)
        {
            try
            {

                return PartialView("_UpadateRPRCaseInfo", PUpdateCaseInfo(discripancyCategory, genQueueId));

            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.ToString();
                BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.OSTCreateSuspectCase, (long)ExceptionTypes.Uncategorized, string.Empty, ex.Message);
                return RedirectToAction("Maintenance", "Error", ex.ToString());
            }

        }

        private object PUpdateCaseInfo(long discripancyCategory,long genQueueId)
        {
            DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
            long? WorkBasketLkp = currentUser.WorkBasketLkup;
            long CategoryType = (long)DiscripancyCategory.RPR;
            string errorMessage = string.Empty;
            ExceptionTypes result;
            try
            {
                objDOGEN_Queue = !(TempData[genQueueId.NullToString()].IsNull()) ? (DOGEN_Queue)TempData[genQueueId.NullToString()] : new DOGEN_Queue();
                TempData.Keep(genQueueId.NullToString());

                List<DOADM_UserMaster> lstUsers;
                DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                objDOADM_UserMaster.IsActive = true;
                objDOGEN_Queue.IsClosedAndCreateNew = true;
                BLUserAdministration objBLUserAdministration = new BLUserAdministration();
                long? TimeZone = currentUser.ADM_UserPreference.TimezoneLkup != null ? currentUser.ADM_UserPreference.TimezoneLkup : (long)DefaultTimeZone.CentralStandardTime;
                result = objBLUserAdministration.SearchUser(TimeZone, objDOADM_UserMaster, out lstUsers, out errorMessage);
                objDOGEN_Queue.ComplianceStartDate = DateTime.UtcNow;
                objDOGEN_Queue.DiscrepancyStartDate = objDOGEN_Queue.ComplianceStartDate.Value.AddMonths(1);
                objDOGEN_Queue.DiscrepancyStartDate = new DateTime(objDOGEN_Queue.DiscrepancyStartDate.Value.Year, objDOGEN_Queue.DiscrepancyStartDate.Value.Month, 1);
                objDOGEN_Queue.lstUsers = lstUsers.Where(x => x.ADM_UserMasterId > 1000 && x.IsManager).OrderBy(x => x.Email).ToList();//Filtered 1st three Users as Admin.sort list by email id
                objDOGEN_Queue.lstLob = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.LOB);
                objDOGEN_Queue.lstLob = objDOGEN_Queue.lstLob.Where(xx => xx.CMN_LookupMasterId != 31004).ToList();
                objDOGEN_Queue.lstDiscCategary = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.WorkBasketVsDiscripancyCategory, WorkBasketLkp);
                objDOGEN_Queue.lstContractid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract);
                objDOGEN_Queue.lstPbpid = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID);
                objDOGEN_Queue.lstActionRequested = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.RPRActionRequested);
                objDOGEN_Queue.lstTaskBeingPerformed = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Taskbeingperformedwhenthisdiscrepancywasidentified);
                objDOGEN_Queue.lstDiscType = CacheUtility.GetAllLookupMasterCorrelationFromCache((long)LookupTypesCorrelation.DiscripancyCategoryVsDiscripancyType, CategoryType);
                objDOGEN_Queue.BusinessSegmentLkup = currentUser.BusinessSegmentLkup.ToLong();

                //check result for DB action
                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(currentUser.ADM_UserMasterId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.RPRCreateSuspectCase, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                }
                return objDOGEN_Queue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}