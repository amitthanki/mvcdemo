using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using System.Reflection;
using System.Net;

namespace ERSBackgroundProcess
{
    public class SendOOALetter
    {
        //Global Declaration Area
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        BLCommon objCommon = new BLCommon();
        BLOST objOST = new BLOST();
        long bgpMasterId = 0;
        ExceptionTypes _retValue;
        DOCMN_BackgroundProcessDetails _objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
        DOCMN_BackgroundProcessMaster _objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();

        /// <summary>
        /// Constructor
        /// </summary>
        public SendOOALetter()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ProcessOOALetter()
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
                    if (GetGenQueueForSendOOALetter(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage))
                    {
                        if (objDOGEN_Queue != null)
                        {
                            do
                            {
                                if (ProcessLockRecord(_lCurrentMasterUserId, (long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId, false))
                                {
                                    count++;
                                    if (SendAndProcessOOALetter(objDOGEN_Queue, out errorMessage))
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
                                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, "Getting Error while Send OOA Letter = " + objDOGEN_Queue.GEN_QueueId, errorMessage);
                                    }
                                    InsertBGPDetails(_objDOCMN_BackgroundProcessDetails);

                                    //Process Unlock Record
                                    if (!ProcessUnlockRecord((long)ScreenType.Queue, (long)objDOGEN_Queue.GEN_QueueId))
                                    {
                                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, "Getting Error While Unlock GEN_Queue Record", "");
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
                            } while (GetGenQueueForSendOOALetter(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage));
                        }
                    }
                    _objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterID;
                    _objDOCMN_BackgroundProcessMaster.BGPStatusLkup = isSuccess? (long)BackgroundProcessRecordStatus.Success : (long)BackgroundProcessRecordStatus.Failed;
                    _objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = count;
                    _objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = StartBackgroundProcess.CurrentMasterUserId;
                    UpdateBGPMaster(_objDOCMN_BackgroundProcessMaster);
                    return isSuccess;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
            }
            return isSuccess;
        }

        public bool InsertBGPMaster(out long bgpMasterId)
        {
            bool isSuccess = false;

            BLCommon objCommon = new BLCommon();
            _retValue = objCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.SendOOALetter, StartBackgroundProcess.CurrentMasterUserId, out bgpMasterId, out string errorMessage);

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
        
        private bool SendAndProcessOOALetter(DOGEN_Queue item, out string errorMessage)
        {
            errorMessage = string.Empty;
            bool isSuccess = false;
            ExceptionTypes result = new ExceptionTypes();
            DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter = new DOGEN_GPSServiceRequestParameter();
            DateTime? disEnrollmentDate;
            try
            {
                objDOGEN_GPSServiceRequestParameter.LoggedInUserId = _lCurrentMasterUserId;
                objDOGEN_GPSServiceRequestParameter.CaseNumber = item.GEN_QueueId.ToString();
                GetDisenrollmentDate(item.GEN_QueueId,item.MemberLOBLkup,item.ComplianceStartDate,out disEnrollmentDate);
                objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate= !item.DisenrollmentDate.IsNull() ? item.DisenrollmentDate.Value.ToString("yyyy-MM-dd") : disEnrollmentDate.Value.ToString("yyyy-MM-dd");
                objDOGEN_GPSServiceRequestParameter.HouseholdId = item.GPSHouseholdID;
                MaintainOutOfAreaServiceService(objDOGEN_GPSServiceRequestParameter, out errorMessage);
                if (!errorMessage.IsNullOrEmpty())
                {
                    item.objDOGEN_OSTActions.OOALetterStatusLkup = (long)OOALetterStatus.Failure;
                    item.objDOGEN_OSTActions.FirstLetterMailDate = null;
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    isSuccess = false;
                }
                else
                {
                    item.objDOGEN_OSTActions.OOALetterStatusLkup = (long)OOALetterStatus.Success;
                    item.objDOGEN_OSTActions.FirstLetterMailDate = DateTime.UtcNow;
                    isSuccess = true;
                }
                item.objDOGEN_OSTActions.ActionLkup = (long)ActionLookup.SendOOALetter;
                item.objDOGEN_OSTActions.RoleLkup = (long)RoleLkup.Admin;
                item.objDOGEN_OSTActions.GEN_QueueRef = item.GEN_QueueId;
                item.objDOGEN_OSTActions.LastUpdatedByRef = _lCurrentMasterUserId;
              

                result = objOST.SaveOSTActions(item.objDOGEN_OSTActions, out errorMessage);

                if (result != (long)ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage);
                    isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isSuccess;
        }

        private void GetDisenrollmentDate(long? gEN_QueueId, long? memberLOBLkup, DateTime? complianceStartDate, out DateTime? disEnrollmentDate)
        {
            disEnrollmentDate = new DateTime();
            int monts = 6;
            try
            {
                if (memberLOBLkup == (long?)Lob.PDP)
                {
                    monts = 12;
                }
                DateTime date = complianceStartDate.ToDateTime().AddMonths(monts);
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                disEnrollmentDate = firstDayOfMonth.AddMonths(1).AddDays(-1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get  Ers Case those are Ready For SendOOALetter 
        /// </summary>
        /// <returns></returns>
        private bool GetGenQueueForSendOOALetter(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            objDOGEN_Queue = null;
            bool isSuccess = false;
            errorMessage = string.Empty;
            ExceptionTypes result = new ExceptionTypes();
            try
            {
                result = objOST.GetQueueSendOOALetter(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMessage);
                if (result != ExceptionTypes.Success)
                {
                    if (result != ExceptionTypes.ZeroRecords)
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, errorMessage, errorMessage);
                    return false;
                }
                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isSuccess;
        }

        /// <summary>
        /// Check If DisenrollmentDate And GPS HouseHoldId is there Or Not for particular ERS CaseID
        /// </summary>
        /// <returns></returns>
        private bool ValidateBeforeSendLetterRequest(DOGEN_Queue objDOGEN_Queue,out string errorMessage)
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
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, ex.ToString(), ex.ToString());
                throw ex;
            }
            return isSuccess;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDOGEN_GPSServiceRequestParameter"></param>
        /// <param name="errorMessage"></param>
        private void MaintainOutOfAreaServiceService(DOGEN_GPSServiceRequestParameter objDOGEN_GPSServiceRequestParameter, out string errorMessage)
        {
            errorMessage = string.Empty;
            string responseData = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = objDOGEN_GPSServiceRequestParameter.LoggedInUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.MaintainOutOfAreaServiceService;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = objDOGEN_GPSServiceRequestParameter.CaseNumber.IsNull() ? 0 : objDOGEN_GPSServiceRequestParameter.CaseNumber.ToInt64();

            srvAEoutOfAreaOption.PutOutofareaoptionClient client = new srvAEoutOfAreaOption.PutOutofareaoptionClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEoutOfAreaOption.putOutOfAreaOptionRequest request = new srvAEoutOfAreaOption.putOutOfAreaOptionRequest();
            srvAEoutOfAreaOption.putOutOfAreaOptionResponse response = new srvAEoutOfAreaOption.putOutOfAreaOptionResponse();
            srvAEoutOfAreaOption.requestHeader reqHeader = new srvAEoutOfAreaOption.requestHeader();

            srvAEoutOfAreaOption.gpsSystemParametersType sysParameter = new srvAEoutOfAreaOption.gpsSystemParametersType();
            srvAEoutOfAreaOption.controlModifiersType credentials = new srvAEoutOfAreaOption.controlModifiersType();
            srvAEoutOfAreaOption.putOutOfAreaOptionRequestOutOfAreaOptionRequest reqParameter = new srvAEoutOfAreaOption.putOutOfAreaOptionRequestOutOfAreaOptionRequest();
            reqParameter.caseNumber = objDOGEN_GPSServiceRequestParameter.CaseNumber;
            reqParameter.householdId = objDOGEN_GPSServiceRequestParameter.HouseholdId;
            reqParameter.outOfAreaDisenrollmentDate = objDOGEN_GPSServiceRequestParameter.OutOfAreaDisenrollmentDate;
            reqParameter.sendFulfillmentInd = "Y";
            reqParameter.outOfAreaInd = "Y";
            request.outOfAreaOptionRequest = reqParameter;
            inputData = "caseNumber:||" + reqParameter.caseNumber + "||householdId:||" + reqParameter.householdId + "||outOfAreaDisenrollmentDate:||" + reqParameter.outOfAreaDisenrollmentDate + "||sendFulfillmentInd:||" + reqParameter.sendFulfillmentInd + "||outOfAreaInd:||" + reqParameter.outOfAreaInd;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;

            //As Per UPM
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            //reqHeader.logLevel = srvAETrrDetails.logLevel.DEBUG;
            //reqHeader.externalId = "AELLOT-kZVPi";
            credentials.gpsSystemParameters = sysParameter;
            request.outOfAreaOptionRequest.controlModifiers = credentials;
            request.requestHeader = reqHeader;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            client.Open();
            try
            {
                response = client.invokeService(request);
                if (!response.outOfAreaOption.IsNull() && response.outOfAreaOption.Length > 0 && response.outOfAreaOption[0].description == "SUCCESS")
                {
                    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||Description:||" + response.outOfAreaOption[0].description;
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                }
                else
                {
                    string statusMessage = (response.responseHeader != null && response.responseHeader.statusMessages != null && response.responseHeader.statusMessages.Length > 0) ? response.responseHeader.statusMessages[0].statusMessage1 : null;
                    responseData = "outOfAreaOption:||" + response.outOfAreaOption + "||reponseHeader:||" + statusMessage;
                    errorMessage = statusMessage != null ? statusMessage : "An error occured during web service call";
                    objDOGEN_AEGPSServiceTrace.ResponseData = responseData;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                }
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
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
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
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPSendOOALetter, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), ex.Message.ToString());
                //ErrorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }
    }
}
