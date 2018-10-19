using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data;
using System.Reflection;

namespace ENRLReconSystem.BL
{
    public class BLFDR
    {
        DALFDR _objDALFDR = new DALFDR();
        ExceptionTypes _retValue;

        #region FDR Response

        public bool InsertFDRBulkImport(DOGEN_FDR objDOGEN_FDR, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();
                DOCMN_BackgroundProcessMaster bgpMaster = new DOCMN_BackgroundProcessMaster();

                ExceptionTypes result = objDAFDRUpload.InsertFDRBulkImport(objDOGEN_FDR, out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());
            }

            return isSuccess;
        }

        public bool ValidationSP(long BulkImportID, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();             

                ExceptionTypes result = objDAFDRUpload.ValidationSP(BulkImportID, out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());
            }

            return isSuccess;
        }

        public bool CheckValidRecordCount(long BulkImportID, out string errorMessage)
        {
            bool isSuccess = false;
            errorMessage = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();

                ExceptionTypes result = objDAFDRUpload.CheckValidRecordCount(BulkImportID, out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());
            }

            return isSuccess;
        }

        public bool GetQueueID(out long QueueID, out string CMSProcessDate, out string DispositionCode, out string DispositionCodeDescription, out long TransactionTypeLkup, out string errorMessage)
        {
            bool isSuccess = false;
            QueueID = 0;
            CMSProcessDate = string.Empty;
            DispositionCode = string.Empty;
            DispositionCodeDescription = string.Empty;
            TransactionTypeLkup = 0;
            errorMessage = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();

                ExceptionTypes result = objDAFDRUpload.GetQueueID(out QueueID,out CMSProcessDate,out DispositionCode,out DispositionCodeDescription,out TransactionTypeLkup, out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                
                BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());
            }

            return isSuccess;
        }

        public bool UpdateRPRQueue(long QueueID, string CMSProcessDate, string DispositionCode, string DispositionCodeDescription, long TransactionTypeLkup, out string errorMessage)
        {
            bool isSuccess = false;           
            errorMessage = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();

                ExceptionTypes result = objDAFDRUpload.UpdateRPRQueue( QueueID, CMSProcessDate, DispositionCode, DispositionCodeDescription, TransactionTypeLkup, out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {

                BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkUpload, (long)ExceptionTypes.Uncategorized, ex.Message.ToString(), errorMessage.ToString());
            }

            return isSuccess;
        }

        public ExceptionTypes GetIncludeInTodaysSubmission(out List<IncludeInTodaysSubmission> lstIncludeInTodaysSubmission,out string  errorMessage)
        {
            DALFDR objDAFDRUpload = new DALFDR();

            return  objDAFDRUpload.GetIncludeInTodaysSubmission(out lstIncludeInTodaysSubmission,out errorMessage);
        }

        public bool GetBulkImportID(out long BulkImportID, out string ExcelFilePath, out string errorMessage)
        {
         
            bool isSuccess = false;
            errorMessage = string.Empty;
            BulkImportID = 0;
            ExcelFilePath = string.Empty;
            try
            {
                DALFDR objDAFDRUpload = new DALFDR();               

                ExceptionTypes result = objDAFDRUpload.GetBulkImportID(out BulkImportID, out ExcelFilePath,out errorMessage);

                if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    return isSuccess;
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message + ex.StackTrace;
            }

            return isSuccess;
        }

        public ExceptionTypes InsertFDRStagingData(DOGEN_FDR objDOGEN_FDR, out string errorMessage)
        {
           // bool isSuccess = false;
            errorMessage = string.Empty;
            //try
            //{
                DALFDR objDAFDRUpload = new DALFDR();               

               return _retValue  = objDAFDRUpload.InsertFDRStagingData(objDOGEN_FDR, out errorMessage);

                //if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                //{
                //    return isSuccess;
                //}

                //isSuccess = true;
            //}
            //catch (Exception ex)
            //{
            //    errorMessage += ex.Message + ex.StackTrace;
            //}

           // return isSuccess;
        }

        //private bool InsertFDRDetails(long bgpMasterId, long bulkImportId, bool status, long errorLogId, string failureReason, out string errorMessage)
        //{
        //    bool isSuccess = false;
        //    errorMessage = string.Empty;

        //    try
        //    {
        //        DOCMN_BackgroundProcessDetails bgpDetails = new DOCMN_BackgroundProcessDetails();
        //        bgpDetails.CMN_BackgroundProcessMasterRef = bgpMasterId;
        //        bgpDetails.GEN_BulkImportRef = bulkImportId;
        //        bgpDetails.CMN_AppErrorLogRef = errorLogId;
        //        bgpDetails.FailureReason = failureReason;

        //        if (status)
        //        {
        //            bgpDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.BGPRecordSuccessful;
        //        }
        //        else
        //        {
        //            bgpDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.BGPRecordFailed;
        //        }

        //        QueueManager queueManager = Utility.ManagerFactory<QueueManager>();
        //        ExceptionTypes result = queueManager.InsertBackgroundProcessDetails(bgpDetails, out errorMessage);

        //        if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
        //        {
        //            // _logger.Trace(_className + MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().Name + " method terminated. Unable to track BG Process Status. Error:" + errorMessage, null);
        //            // _logger.Error(MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkImportProcess, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage, string.Empty);
        //            return isSuccess;
        //        }

        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage += ex.Message + ex.StackTrace;
        //        //_logger.Error(MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkImportProcess, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString(), ex.StackTrace);
        //        //_logger.Trace(_className + MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().Name + " method terminated.Error:" + ex.ToString(), null);
        //    }

        //    return isSuccess;
        //}

        //// Author: Sourabha Ranjan Barik
        //// Create date: 04/27/2016
        //// Method Description: This method is used to update BGP Master data with enddatetime & duration
        //// Method Name: UpdateBGPMaster
        ///// <summary>
        ///// This method is used to update BGP Master data with enddatetime & duration
        ///// </summary>
        ///// <param name="bgpMasterId"></param>
        ///// <param name="endDateTime"></param>
        ///// <param name="duration"></param>
        ///// <param name="totalRecordProcessed"></param>
        ///// <param name="errorMessage"></param>
        ///// <returns>bool</returns>
        //private bool UpdateFDRMaster(long bgpMasterId, DateTime endDateTime, double duration, long totalRecordProcessed, out string errorMessage)
        //{
        //    bool isSuccess = false;
        //    errorMessage = string.Empty;
        //    try
        //    {

        //        //Update BGPMaster Data
        //        DOCMN_BackgroundProcessMaster bgpMaster = new DOCMN_BackgroundProcessMaster();
        //        bgpMaster.CMN_BackgroundProcessMasterId = bgpMasterId;
        //        bgpMaster.UTCEndDate = endDateTime;
        //        bgpMaster.TotalDuration = duration;
        //        bgpMaster.TotalRecordProcessed = totalRecordProcessed;

        //        // QueueManager queueManager = Utility.ManagerFactory<QueueManager>();
        //        ExceptionTypes result = queueManager.UpdateBackgroundProcessMaster(bgpMaster, out errorMessage);

        //        if (result != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
        //        {
        //            //   _logger.Trace(_className + MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().Name + " method terminated. Unable to track BG Process Status. Error:" + errorMessage, null);
        //            //   _logger.Error(MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkImportProcess, (long)ExceptionTypes.Uncategorized, string.Empty, errorMessage, string.Empty);
        //            return isSuccess;
        //        }

        //        isSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMessage += ex.Message + ex.StackTrace;
        //        // _logger.Error(MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BulkImportProcess, (long)ExceptionTypes.Uncategorized, string.Empty, ex.ToString(), ex.StackTrace);
        //        //_logger.Trace(_className + MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().Name + " method terminated.Error:" + ex.ToString(), null);
        //    }

        //    return isSuccess;
        //}

        #endregion

        #region FDR Submission

        public ExceptionTypes GetCaseDetails(string strContractId, string strHICN,long lDiscrepancyType,long lTransactionTypeLkup, out List<FDRSubmissionRow> lstResults, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALFDR.GetCaseDetails(strContractId, strHICN, lDiscrepancyType, lTransactionTypeLkup, out lstResults, out errorMessage);
        }

        public ExceptionTypes UpdateFDRPackageDate(List<long> caseIds, FDRSubmissionCategory submissionCategory,long LoginUserId , out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALFDR.UpdateFDRPackageDate(caseIds, submissionCategory,LoginUserId, out errorMessage);
        }

        public ExceptionTypes InsertFDRSubmissionLog(List<FDRSubmissionRow> lstCases, long LoginUserId,bool IsFDRSubmissionCompleted,string errormessage, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALFDR.InsertFDRSubmissionLog(lstCases, LoginUserId, IsFDRSubmissionCompleted, errormessage,out errorMessage);
        }
        #endregion
    }
}
