using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENRLReconSystem.BL
{
    public class BLCommon
    {
        DALCommon _objDALCommon = new DALCommon();
        private string _moduleName = "BOCommon.";
        ExceptionTypes retValue;

        /// <summary>
        /// For Logging Error
        /// </summary>
        /// <param name="CurrentUserRef"></param>
        /// <param name="location"></param>
        /// <param name="sourceLkup"></param>
        /// <param name="ERSErrorCode"></param>
        /// <param name="ERSErrorMessage"></param>
        /// <param name="formattedMessage"></param>
        public static void LogError(long CurrentUserRef, string location, long sourceLkup, long ERSErrorCode, string ERSErrorMessage, string formattedMessage)
        {
            try
            {
                DALCommon.LogError(CurrentUserRef, location, sourceLkup, ERSErrorCode, ERSErrorMessage, formattedMessage);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Lock Record
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <param name="objUIRecordsLock"></param>
        /// <returns></returns>
        public ExceptionTypes GetLockedRecordOrLockRecord(long loginUserID, long screenLkup, long caseID, bool isRelockRequired, out UIRecordsLock objUIRecordsLock)
        {
            retValue = new ExceptionTypes();
            objUIRecordsLock = new UIRecordsLock();
            try
            {
                string guid = Guid.NewGuid().ToString();
                retValue = _objDALCommon.GetLockedRecordOrLockRecord(loginUserID, screenLkup, caseID, guid, isRelockRequired, out objUIRecordsLock);
                if (retValue == (long)ExceptionTypes.Success)
                {
                    if (objUIRecordsLock.CreatedByRef != loginUserID)
                    {
                        objUIRecordsLock.LockedHours = (DateTime.Now.ToUniversalTime() - objUIRecordsLock.UTCCreatedOn).TotalHours;
                    }
                    else if (guid != objUIRecordsLock.Guid.Trim())
                        objUIRecordsLock.IsEditInProgress = true;

                    objUIRecordsLock.Status = (long)ExceptionTypes.Success;
                }
            }
            catch (Exception ex)
            {
                objUIRecordsLock.Status = (long)ExceptionTypes.UnknownError;
                objUIRecordsLock.ErrorMessage = "Error while locking record.";
            }

            return retValue;
        }
        public ExceptionTypes GetBulkUploadSearchResult(long? timeZone, UIMassUpdateByTemplates objUIMassUpdateByTemplates, out List<DOGEN_BulkImport> lstDOGEN_BulkImport, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.GetBulkUploadSearchResult(timeZone, objUIMassUpdateByTemplates, out lstDOGEN_BulkImport, out errorMessage);
        }

        /// <summary>
        /// Unlock Record
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <param name="objUIRecordsLock"></param>
        /// <returns></returns>
        public ExceptionTypes UnlockRecord(long screenLkup, long caseID)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.UnlockRecord(screenLkup, caseID);
        }

        /// <summary>
        /// To validate whether record locked by current login user or not
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <param name="recordLockedCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool ValidateLockBeforeSave(long loginUserID, long screenLkup, long caseID)
        {
            long recordLockedCount = 0;
            string errorMessage = string.Empty;

            try
            {
                retValue = new ExceptionTypes();
                retValue = _objDALCommon.ValidateLockBeforeSave(loginUserID, screenLkup, caseID, out recordLockedCount, out errorMessage);

                if (retValue == ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage) && recordLockedCount == 1)
                    return true;
            }
            catch (Exception ex)
            {
                //Log Error
            }

            return false;
        }

        public ExceptionTypes SearchRecords(long? TimeZone,long pageType, long loggedInUserId,long rowCount, SearchCriteria objSearchCriteria, out List<SearchResults> lstSearchResults,out string totalCount,out string errorMessege)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.SearchRecords(TimeZone, pageType, loggedInUserId, rowCount, objSearchCriteria, out lstSearchResults,out totalCount,out errorMessege);
        }
        public ExceptionTypes SearchRecordsToUnlock(long? TimeZone,long pageType, long loggedInUserId, SearchCriteria objSearchCriteria, out List<UnlockSearchResults> lstSearchResults)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.SearchRecordsToUnlock(TimeZone,pageType, loggedInUserId, objSearchCriteria, out lstSearchResults);
        }

        public ExceptionTypes SearchRecordsToReassign(long? TimeZone,long pageType, long loggedInUserId, SearchCriteria objSearchCriteria, out List<UnlockSearchResults> lstSearchResults)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.SearchRecordsToReassign(TimeZone,pageType, loggedInUserId, objSearchCriteria, out lstSearchResults);
        }
        public ExceptionTypes ReassignQueueRecord(DOGEN_ManageCases objDOGEN_ManageCases, out string errorMessage)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.ReassignQueueRecord(objDOGEN_ManageCases, out errorMessage);
        }

        public ExceptionTypes BulkReassignQueueRecord(UIBulkAssign objUIBulkAssign, out string errorMessage)
        {
            return retValue = _objDALCommon.BulkReassignQueueRecord(objUIBulkAssign, out errorMessage);
        }

        public ExceptionTypes DownloadPWAttachments(long attachmentId, out DOGEN_Attachments objDOGEN_Attachments, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.DownloadPWAttachments(attachmentId,out objDOGEN_Attachments, out errorMessage);
        }

        #region Background Process Tracking
        public ExceptionTypes InsertBackgroundProcessMaster(long backgroundProcessType, long LoginId, out long bgpMasterId, out string errorMessage)
        {
            return retValue = _objDALCommon.InsertBackgroundProcessMaster(backgroundProcessType, LoginId, out bgpMasterId, out errorMessage);
        }
        public ExceptionTypes InsertBackgroundProcessDetails(DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails, out string errorMessage)
        {
            return retValue = _objDALCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out errorMessage);
        }

        public ExceptionTypes UpdateBackgroundProcessMaster(DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster, out string errorMessage)
        {
            return retValue = _objDALCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out errorMessage);
        }

        public ExceptionTypes GetCurrentMachineUserId(string machineName, out UIUserLogin currentServerDetails, out string errorMessage)
        {
            retValue = _objDALCommon.GetBackgroundServerDetails(machineName, out currentServerDetails, out errorMessage);
            return retValue;
        }

        #endregion

        public void InsertServiceProcessDetails(string ServiceName, string ClientMachineName, string ServiceRequestorName, long ServiceRequestedBy)
        {


        }

        public ExceptionTypes OSTActionUpdate(DOGEN_OSTActions objDOGEN_OSTActions, out string errorMessage)
        {
            return retValue = _objDALCommon.OSTActionUpdate(objDOGEN_OSTActions, out errorMessage);
        }

        public ExceptionTypes EligibilityActionUpdate(DOGEN_EligibilityActions objDOGEN_EligibilityActions, out string errorMessage)
        {
            return retValue = _objDALCommon.EligibilityActionUpdate(objDOGEN_EligibilityActions, out errorMessage);
        }

        public ExceptionTypes RPRActionUpdate(DOGEN_RPRActions objDOGEN_RPRActions, out string errorMessage)
        {
            return retValue = _objDALCommon.RPRActionUpdate(objDOGEN_RPRActions, out errorMessage);
        }

        public ExceptionTypes InsertManageCase(DOGEN_ManageCases objDOGEN_ManageCases, out string errorMessage)
        {
            return retValue = _objDALCommon.InsertManageCase(objDOGEN_ManageCases, out errorMessage);
        }

        public ExceptionTypes GetReferenceCase(long genQueueID, out DOGEN_Queue objDOGEN_Queue)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.GetReferenceCase(genQueueID, out objDOGEN_Queue);
        }

        public ExceptionTypes GetCasesToMask(out List<DOGEN_Queue> lstGenQueues)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.GetCasesToMask(out lstGenQueues);
        }


        public ExceptionTypes MaskPHIData(long gEN_QueueId, DOGEN_GPSData objDOGEN_GPSData)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.MaskPHIData(gEN_QueueId, objDOGEN_GPSData);
        }
        public ExceptionTypes ManagePWAttachments(long? TimeZone,DOGEN_Attachments objDOGEN_Attachments, long loggedInUserId, out List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.ManagePWAttachments(TimeZone,objDOGEN_Attachments, loggedInUserId, out lstDOGEN_Attachments, out errorMessage);
        }

        public ExceptionTypes UnlockQueueRecord(string gen_QueueIds, long aDM_UserMasterId,long actionPerformed,  string casesComments, out string errorMessage)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.UnlockQueueRecord(gen_QueueIds, aDM_UserMasterId,actionPerformed, casesComments, out errorMessage);

        }
        public ExceptionTypes UpdateCaseInfo(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            DALCommon _objDALCommon = new DALCommon();
            return retValue = _objDALCommon.UpdateCaseInfo(objDOGEN_Queue, out errorMessage);

        }

        public ExceptionTypes UpdateMBIValue(long gEN_QueueId, string MBI)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.UpdateMBIValue(gEN_QueueId, MBI);
        }

        public long ERSCaseIDForERNCaseNumber(string strERSCaseNumber)
        {
           return  _objDALCommon.ERSCaseIDForERNCaseNumber(strERSCaseNumber);
        }

        public ExceptionTypes EGHPExcelProcess(DataTable dt,out string errorMessage)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.EGHPExcelProcess(dt,out errorMessage);
        }

        public ExceptionTypes IsOOAEGHPExclusion(string EmployerID)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.IsOOAEGHPExclusion(EmployerID);
        }


        public ExceptionTypes IsNationalEmployerForRestriction(string contractNumber, string PBP, string employerID)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.IsNationalEmployerForRestriction(contractNumber,PBP,employerID);
        }

        public ExceptionTypes SaveBulkUpload(DOGEN_BulkImport objDOGEN_BulkImport, long loginUserID, DataTable dtFileData,out long GEN_BulkImportID, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            return retValue = _objDALCommon.SaveBulkUpload(objDOGEN_BulkImport,loginUserID,dtFileData,out  GEN_BulkImportID, out errorMessage);
        }

    }

}
