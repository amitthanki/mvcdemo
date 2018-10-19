using ENRLReconSystem.BL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net;

namespace ERSBackgroundProcess
{
    public class CreateCMSTransaction
    {
        //Global Declaration Area
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        BLCommon objCommon = new BLCommon();
        BLOST objOST = new BLOST();
        BLEligibility objEligibility = new BLEligibility();
        long bgpMasterId = 0;
        ExceptionTypes _retValue;
        DOCMN_BackgroundProcessDetails _objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
        DOCMN_BackgroundProcessMaster _objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateCMSTransaction()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ProcessCMSTransaction()
        {
            DOGEN_Queue objDOGEN_Queue;
            string errorMessage = string.Empty;
            StringBuilder strGEN_QueueIdsToSkip = new StringBuilder();
            long count = 0;
            bool isSuccess = true;
            try
            {
                if (InsertBGPMaster(out long bgpMasterID))
                {
                    if (GetGenQueueForCMSTransaction(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage))
                    {
                        if (objDOGEN_Queue != null)
                        {
                            do
                            {
                                if (ProcessLockRecord(_lCurrentMasterUserId, (long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId, false))
                                {
                                    count++;
                                    if (CreateAndProcessCMSTransaction(objDOGEN_Queue, out errorMessage))
                                    {
                                        _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = bgpMasterID;
                                        _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                                        _objDOCMN_BackgroundProcessDetails.GEN_QueueRef = objDOGEN_Queue.GEN_QueueId;
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = bgpMasterID;
                                        _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                                        _objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                                        _objDOCMN_BackgroundProcessDetails.GEN_QueueRef = objDOGEN_Queue.GEN_QueueId;

                                        strGEN_QueueIdsToSkip.Append(objDOGEN_Queue.GEN_QueueId.ToString() + ",");
                                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, "Getting Error while Create CMS Transaction = " + objDOGEN_Queue.GEN_QueueId, errorMessage);
                                    }
                                    InsertBGPDetails(_objDOCMN_BackgroundProcessDetails);

                                    //Process Unlock Record
                                    if (!ProcessUnlockRecord((long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId))
                                    {
                                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, "Getting Error While Unlock GEN_Queue Record", "");
                                    }
                                }
                                else
                                {
                                    isSuccess = false;
                                    _objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = bgpMasterID;
                                    _objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                                    _objDOCMN_BackgroundProcessDetails.FailureReason = "Unable to lock the Record";
                                    _objDOCMN_BackgroundProcessDetails.GEN_QueueRef = objDOGEN_Queue.GEN_QueueId;
                                    InsertBGPDetails(_objDOCMN_BackgroundProcessDetails);
                                }
                            } while (GetGenQueueForCMSTransaction(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage));
                        }
                    }
                    _objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterID;
                    _objDOCMN_BackgroundProcessMaster.BGPStatusLkup = isSuccess ? (long)BackgroundProcessRecordStatus.Success : (long)BackgroundProcessRecordStatus.Failed;
                    _objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = count;
                    _objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = StartBackgroundProcess.CurrentMasterUserId;
                    UpdateBGPMaster(_objDOCMN_BackgroundProcessMaster);
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
            }
            return isSuccess;
        }

        public bool InsertBGPMaster(out long bgpMasterId)
        {
            bool isSuccess = false;

            BLCommon objCommon = new BLCommon();
            _retValue = objCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.CreateCMSTransaction, StartBackgroundProcess.CurrentMasterUserId, out bgpMasterId, out string errorMessage);

            if (_retValue == ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                isSuccess = true;

            return isSuccess;
        }

        public bool UpdateBGPMaster(DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster)
        {
            bool isSuccess = false;
            //Insert BGP Master Row
            BLCommon objCommon = new BLCommon();
            //Insert BGP Master Row
            _retValue = objCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out string errorMessage);

            if (_retValue == ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                isSuccess = true;

            return isSuccess;
        }

        public bool InsertBGPDetails(DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails)
        {
            bool isSuccess = false;
            //Insert BGP Master Row
            BLCommon objCommon = new BLCommon();
            //Insert BGP Master Row
            objCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out string errorMessage);

            if (_retValue == ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                isSuccess = true;

            return isSuccess;

        }

        private bool CreateAndProcessCMSTransaction(DOGEN_Queue item, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isSuccess = false;
            ExceptionTypes result = new ExceptionTypes();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = _lCurrentMasterUserId;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = item.GEN_QueueId.ToString();
                
                var contractNumber = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.Contract).Where(x => x.CMN_LookupMasterId == (item.objDOGEN_OSTActions != null && item.objDOGEN_OSTActions.ContractIDLkup != null && item.objDOGEN_OSTActions.ContractIDLkup != 0? item.objDOGEN_OSTActions.ContractIDLkup :  item.MemberContractIDLkup)).FirstOrDefault();
                var pbpNo = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.PBPID).Where(x => x.CMN_LookupMasterId == (item.objDOGEN_OSTActions != null && item.objDOGEN_OSTActions.PBPLkup != null && item.objDOGEN_OSTActions.PBPLkup != 0 ? item.objDOGEN_OSTActions.PBPLkup : item.MemberPBPLkup)).FirstOrDefault();
                objDOGEN_GPSServiceRequestParameter.ApplicationDate = (item.objDOGEN_OSTActions != null && item.objDOGEN_OSTActions.ApplicationDate != null) ? item.objDOGEN_OSTActions.ApplicationDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                objDOGEN_GPSServiceRequestParameter.BirthDate = (item.objDOGEN_OSTActions != null && item.objDOGEN_OSTActions.DateofBirth != null) ? item.objDOGEN_OSTActions.DateofBirth.Value.ToString("yyyy-MM-dd") : item.MemberDOB.Value.ToString("yyyy-MM-dd");
                objDOGEN_GPSServiceRequestParameter.ContractNumber = !contractNumber.IsNull() ? contractNumber.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.EffectiveEndDate = (item.objDOGEN_OSTActions != null && item.objDOGEN_OSTActions.EndDate != null) ? item.objDOGEN_OSTActions.EndDate.Value.ToString("yyyy-MM-dd") : (!(item.EndDate.IsNull())? item.EndDate.Value.ToString("yyyy-MM-dd") : string.Empty);
                //if TRC code is 16 then effective date will be TimelineEffectiveDate from MQ TRR record this field we are capturing in Gen_Queue while we are processing MQ tRR record
                if ((!(item.TransactionReplyCodeLkup.IsNull()) && item.TransactionReplyCodeLkup == TRCCode.TRC16.ToInt64()) && !(item.TimelineEffectiveDate.IsNull()))
                {
                    objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = item.TimelineEffectiveDate.ToDateTime().ToString("yyyy-MM-dd");
                }
                else
                {
                    objDOGEN_GPSServiceRequestParameter.EffectiveStartDate = new DateTime(DateTime.UtcNow.AddMonths(1).Year, DateTime.UtcNow.AddMonths(1).Month, 1).ToString("yyyy-MM-dd");
                }
                //objDOGEN_GPSServiceRequestParameter.ElectionType = "E";
                objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber = item.MemberCurrentHICN;
                objDOGEN_GPSServiceRequestParameter.LastName = (item.objDOGEN_OSTActions != null && !(item.objDOGEN_OSTActions.LastName.IsNullOrEmpty())) ? item.objDOGEN_OSTActions.LastName : item.MemberLastName;
                objDOGEN_GPSServiceRequestParameter.PbpNo = !pbpNo.IsNull() ? pbpNo.LookupValue : string.Empty;
                objDOGEN_GPSServiceRequestParameter.TransactionCode = ((long)CMSTransactionCode.TRR76).ToString();

                CreateCMSTransactionService(objDOGEN_GPSServiceRequestParameter, out errorMessage);

                if (!errorMessage.IsNullOrEmpty())
                {
                    item.objDOGEN_OSTActions.CMSTransactionStatusLkup = (long)CMSTransactionStatus.Failure;
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    isSuccess = false;
                }
                else
                {
                    item.objDOGEN_OSTActions.CMSTransactionStatusLkup = (long)CMSTransactionStatus.Success;
                    isSuccess = true;
                }
                item.objDOGEN_OSTActions.GEN_QueueRef = item.GEN_QueueId;
                item.objDOGEN_OSTActions.ActionLkup = (long)ActionLookup.SendSCCUpdatetoCMS;
                item.objDOGEN_OSTActions.LastName = item.MemberLastName;
                item.objDOGEN_OSTActions.DateofBirth = item.MemberDOB;
                item.objDOGEN_OSTActions.ContractIDLkup = item.MemberContractIDLkup;
                item.objDOGEN_OSTActions.PBPLkup = item.MemberPBPLkup;
                if ((!(item.TransactionReplyCodeLkup.IsNull()) && item.TransactionReplyCodeLkup == TRCCode.TRC16.ToInt64()) && !(item.TimelineEffectiveDate.IsNull()))
                {
                    item.objDOGEN_OSTActions.EffectiveDate = item.TimelineEffectiveDate.ToDateTime();
                }
                else
                {
                    item.objDOGEN_OSTActions.EffectiveDate = new DateTime(DateTime.UtcNow.AddMonths(1).Year, DateTime.UtcNow.AddMonths(1).Month, 1);
                }
                //item.objDOGEN_OSTActions.EffectiveDate = new DateTime(DateTime.UtcNow.AddMonths(1).Year, DateTime.UtcNow.AddMonths(1).Month, 1);
                item.objDOGEN_OSTActions.EndDate = item.EndDate;
                item.objDOGEN_OSTActions.RoleLkup = (long)RoleLkup.Admin;
                item.objDOGEN_OSTActions.LastUpdatedByRef = _lCurrentMasterUserId;
                result = objOST.SaveOSTActions(item.objDOGEN_OSTActions, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        /// <summary>
        /// Get  Ers Case those are Ready For Create CMS Transaction 
        /// </summary>
        /// <returns></returns>
        private bool GetGenQueueForCMSTransaction(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            objDOGEN_Queue = null;
            bool isSuccess = false;
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                result = objOST.GetQueueCMSTransaction(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage);
                if (result != ExceptionTypes.Success)
                {
                    if (result != ExceptionTypes.ZeroRecords)
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return false;
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// Check If DisenrollmentDate And GPS HouseHoldId is there Or Not for particular ERS CaseID
        /// </summary>
        /// <returns></returns>
        private bool ValidateBeforeSendLetterRequest(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isSuccess = false;
            try
            {
                if (objDOGEN_Queue.DisenrollmentDate.IsNull() || objDOGEN_Queue.GPSHouseholdID.IsNull())
                {
                    if (objDOGEN_Queue.DisenrollmentDate.IsNull())
                        errorMessage = "DisenrollmentId for ERS Case Id " + objDOGEN_Queue.GEN_QueueId + " is missing From BGP.";
                    if (objDOGEN_Queue.GPSHouseholdID.IsNull())
                        errorMessage = "GPSHousehold ID for ERS Case Id " + objDOGEN_Queue.GEN_QueueId + " is Missing From BGP.";

                    return isSuccess;
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isSuccess;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDOGEN_GPSServiceRequestParameter"></param>
        /// <param name="errorMessage"></param>
        public void CreateCMSTransactionService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out string errorMessage)
        {
            string responseData = string.Empty;
            string inputData = string.Empty;
            errorMessage = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.CreateCMSTransactionService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.CaseNumber.IsNull() ? 0 : objDOGEN_GPSServiceRequestParameter.CaseNumber.ToInt64();

            srvAECMSTransaction.PostCmstransactionClient client = new srvAECMSTransaction.PostCmstransactionClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAECMSTransaction.postCmstransactionRequest request = new srvAECMSTransaction.postCmstransactionRequest();
            srvAECMSTransaction.postCmstransactionResponse response = new srvAECMSTransaction.postCmstransactionResponse();
            srvAECMSTransaction.requestHeader reqHeader = new srvAECMSTransaction.requestHeader();
            srvAECMSTransaction.gpsSystemParametersType sysParameter = new srvAECMSTransaction.gpsSystemParametersType();
            srvAECMSTransaction.controlModifiersType credentials = new srvAECMSTransaction.controlModifiersType();
            srvAECMSTransaction.cmstransaction requestDetails = new srvAECMSTransaction.cmstransaction();

            //requestDetails.applicationDate = objDOGEN_GPSServiceRequestParameter.ApplicationDate;
            requestDetails.birthDate = objDOGEN_GPSServiceRequestParameter.BirthDate;
            requestDetails.caseNumber = objDOGEN_GPSServiceRequestParameter.CaseNumber;
            requestDetails.contractNumber = objDOGEN_GPSServiceRequestParameter.ContractNumber;
            //requestDetails.effectiveEndDate = objDOGEN_GPSServiceRequestParameter.EffectiveEndDate;
            requestDetails.effectiveStartDate = objDOGEN_GPSServiceRequestParameter.EffectiveStartDate;
            //requestDetails.electionType = objDOGEN_GPSServiceRequestParameter.ElectionType;
            requestDetails.medicareClaimNumber = objDOGEN_GPSServiceRequestParameter.MedicareClaimNumber;
            requestDetails.lastName = objDOGEN_GPSServiceRequestParameter.LastName;
            requestDetails.pbpNo = objDOGEN_GPSServiceRequestParameter.PbpNo;
            requestDetails.transactionCode = objDOGEN_GPSServiceRequestParameter.TransactionCode;
            inputData = "birthDate:||" + requestDetails.birthDate + "||caseNumber:||" + requestDetails.caseNumber + "||contractNumber:||" + requestDetails.contractNumber + "||effectiveStartDate:||" + requestDetails.effectiveStartDate + "||medicareClaimNumber:||" + requestDetails.medicareClaimNumber + "||lastName:||" + requestDetails.lastName + "||pbpNo:||" + requestDetails.pbpNo + "||transactionCode:||" + requestDetails.transactionCode;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            request.controlModifiers = credentials;
            request.requestHeader = reqHeader;
            request.cmstransaction = requestDetails;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.Open();
            try
            {
                response = client.invokeService(request);
                if (!response.result.IsNull())
                {

                    responseData = "caseNumber:||" + response.caseNumber + "||medicareClaimNumber:||" + response.medicareClaimNumber + "||result:||" + response.result + "||reponseHeader:||" + "Accept";
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                }
                else
                {
                    responseData = "caseNumber:||" + response.caseNumber + "||medicareClaimNumber:||" + response.medicareClaimNumber + "||result:||" + response.result + "||reponseHeader:||" + response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1.NullToString();
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                }

                //if (!response.outOfAreaOption.IsNull() && response.outOfAreaOption.Length > 0 && response.outOfAreaOption[0].description == "SUCCESS")
                //{
                //    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||Description:||" + response.outOfAreaOption[0].description;
                //    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                //    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                //}
                //else
                //{
                //    string statusMessage = (response.responseHeader != null && response.responseHeader.statusMessages != null && response.responseHeader.statusMessages.Length > 0) ? response.responseHeader.statusMessages[0].statusMessage1 : null;
                //    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||reponseHeader:||" + statusMessage;
                //    errorMessage = statusMessage != null ? statusMessage : "An error occured during web service call";
                //    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                //    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                //}

            }
            catch (System.ServiceModel.FaultException ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(objDOGEN_GPSServiceRequestParameter.LoggedInUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.GPSWebservice, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
        }

        //Process Lock Record
        public bool ProcessLockRecord(long UserID, long ScreenType, long CaseID, bool IsRelockRequired)
        {
            UIRecordsLock objRecordsLocked = new UIRecordsLock();
            bool isSuccess = false;
            try
            {
                BLCommon objBLCommon = new BLCommon();
                _retValue = objBLCommon.GetLockedRecordOrLockRecord(UserID, ScreenType, CaseID, IsRelockRequired, out objRecordsLocked);
                if (_retValue == ExceptionTypes.Success && objRecordsLocked.Status == (long)ExceptionTypes.Success && objRecordsLocked.CreatedByRef == UserID)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                //ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }

        //Process Unlock Record
        public bool ProcessUnlockRecord(long ScreenType, long CaseID)
        {
            UIRecordsLock objRecordsLocked = new UIRecordsLock();
            bool isSuccess = false;
            try
            {
                BLCommon objBLCommon = new BLCommon();
                _retValue = objBLCommon.UnlockRecord(ScreenType, CaseID);
                if (_retValue == ExceptionTypes.Success && objRecordsLocked.Status == (long)ExceptionTypes.Success)
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPCreateCMSTransaction, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                //ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }
    }
}
