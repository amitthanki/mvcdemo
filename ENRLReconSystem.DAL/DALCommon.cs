using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO.DataObjects;
namespace ENRLReconSystem.DAL
{
    public class DALCommon
    {
        private string systemName;
        private string runningUserId;
        DAHelper _objDAHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        DataSet _dsResult;
        public DALCommon()
        {
            try
            {
                runningUserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                systemName = System.Environment.MachineName;
            }
            catch
            {

            }
        }
        /// <summary>
        /// insert to error log
        /// </summary>
        /// <param name="runningUserId"></param>
        /// <param name="runningSystemName"></param>
        /// <param name="location"></param>
        /// <param name="sourceLkup"></param>
        /// <param name="ERSErrorCode"></param>
        /// <param name="ERSErrorMessage"></param>
        /// <param name="formattedMessage"></param>
        /// <param name="errorLookups"></param>
        public static void LogError(long CurrentUserRef, string location, long sourceLkup, long ERSErrorCode, string ERSErrorMessage, string formattedMessage)
        {
            long dbortnvalue = 0;
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowEffected = 0;
            string sErrorMsg = string.Empty;
            string runningUserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string runningSystemName = System.Environment.MachineName;

            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorLocation";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = location;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorSourceLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = sourceLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ERSErrorCode";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = ERSErrorCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ERSErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = ERSErrorMessage;
                parameters.Add(sqlParam);

                string errorMessage = string.Format("System Name:{0},\n\t\tRunning User Id:{1},\n\t\tErrorDescription:{2}", runningSystemName, runningUserId, formattedMessage);
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = errorMessage;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UserRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = CurrentUserRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_AppErrorLogId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                dbortnvalue = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_ErrorLog, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowEffected, out sErrorMsg);
                if (parameters != null && parameters.Count > 0)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }

        public ExceptionTypes GetBulkUploadSearchResult(long? timeZone, UIMassUpdateByTemplates objUIMassUpdateByTemplates, out List<DOGEN_BulkImport> lstDOGEN_BulkImport, out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            try
            {
                if (objUIMassUpdateByTemplates.Gen_BulkImportId != null && objUIMassUpdateByTemplates.Gen_BulkImportId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@Gen_BulkImportId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIMassUpdateByTemplates.Gen_BulkImportId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TemplateTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objUIMassUpdateByTemplates.TemplateTypeLkup;
                _lstParameters.Add(sqlParam);

                if (!objUIMassUpdateByTemplates.WorkBasketLkup.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkbasketLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIMassUpdateByTemplates.WorkBasketLkup;
                    _lstParameters.Add(sqlParam);
                }
                if (!objUIMassUpdateByTemplates.DiscrepancyCategoryLkup.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIMassUpdateByTemplates.DiscrepancyCategoryLkup;
                    _lstParameters.Add(sqlParam);
                }

                if (!objUIMassUpdateByTemplates.StartDate.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@StartDate";
                    sqlParam.SqlDbType = SqlDbType.Date;
                    sqlParam.Value = objUIMassUpdateByTemplates.StartDate;
                    _lstParameters.Add(sqlParam);
                }
                if (!objUIMassUpdateByTemplates.EndDate.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EndDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objUIMassUpdateByTemplates.EndDate;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_BulkImport, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapMassUpdateTemplateResult(timeZone, _dsResult, out lstDOGEN_BulkImport);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }

        /// <summary>
        /// Lock record
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <param name="objUIRecordsLock"></param>
        /// <returns></returns>
        public ExceptionTypes GetLockedRecordOrLockRecord(long loginUserID, long screenLkup, long caseID, string guid, bool isRelockRequired, out UIRecordsLock objRecordsLock)
        {
            List<SqlParameter> lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            string errorMessage = string.Empty;
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            objRecordsLock = new UIRecordsLock();

            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loginUserID;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = screenLkup;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CaseId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = caseID;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Guid";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = guid;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRelockRequired";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = isRelockRequired;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                lstParameters.Add(sqlParam);

                DataSet dsResults = new DataSet();
                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_INS_UPD_LockRecord, lstParameters.ToArray(), out dsResults, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapRecordsLockedDetails(dsResults, out objRecordsLock);
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                lstParameters = null;
            }
        }

        public ExceptionTypes DownloadPWAttachments(long attachmentId, out DOGEN_Attachments objDOGEN_Attachments, out string errorMessage)
        {
            List<SqlParameter> lstParameters = new List<SqlParameter>();
            objDOGEN_Attachments = new DOGEN_Attachments();
            SqlParameter sqlParam;
            errorMessage = string.Empty;
            long lErrorNumber = 0;
            long lRowsEffected = 0;

            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_AttachmentsId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = attachmentId;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                lstParameters.Add(sqlParam);

                DataSet dsResults = new DataSet();
                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_DownloadAttachments, lstParameters.ToArray(), out dsResults, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapDownloadPWAttachments(dsResults, out objDOGEN_Attachments);
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                lstParameters = null;
            }
        }

       

        /// <summary>
        /// OST Actions MassUpdate
        /// </summary>
        /// <param name="objDOGEN_OSTActions"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes OSTActionUpdate(DOGEN_OSTActions objDOGEN_OSTActions, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();
            errorMessage = string.Empty;
            try
            {
                if (!objDOGEN_OSTActions.GEN_BulkImportRef.IsNull() && objDOGEN_OSTActions.GEN_BulkImportRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_BulkImportRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_OSTActions.GEN_BulkImportRef;
                    _lstParameters.Add(sqlParam);
                }
                if (!objDOGEN_OSTActions.QueueLkup.IsNull() && objDOGEN_OSTActions.QueueLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@QueueLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_OSTActions.QueueLkup;
                    _lstParameters.Add(sqlParam);
                }
                if (!objDOGEN_OSTActions.BusinessSegmentLkup.IsNull() && objDOGEN_OSTActions.BusinessSegmentLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_OSTActions.BusinessSegmentLkup;
                    _lstParameters.Add(sqlParam);
                }
                if (!objDOGEN_OSTActions.DiscrepancyCategoryLkup.IsNull() && objDOGEN_OSTActions.DiscrepancyCategoryLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_OSTActions.DiscrepancyCategoryLkup;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ActionLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.LastName;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DateofBirth";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.DateofBirth;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ContractIDLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.PBPLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ApplicationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.ApplicationDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.EffectiveDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.EndDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FirstLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.FirstLetterMailDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SecondLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SecondLetterMailDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResidentialDocumentationRequired";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.ResidentialDocumentationRequired;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CountyAttestationRequired";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.CountyAttestationRequired;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.PendReasonLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContainsErrorsLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ContainsErrorsLkup;
                _lstParameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ResolutionLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Reason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.Reason;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@InitialAddressVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.InitialAddressVerificationDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberResponseVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.MemberResponseVerificationDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.MemberVerifiedState;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SCCLetterMailDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.Comments;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.RoleLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.LastUpdatedByRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.RPRRequestedEffectiveDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRActionRequestedLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPROtherActionRequested;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRSupervisorOrRequesterRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPRReasonforRequest;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRTaskPerformedLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPROtherTaskPerformed;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.RPRCTMMember;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPRCTMNumber;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.RPREGHPMember;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPREmployerID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequested;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequestedZip";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequestedZip;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequstedSubmissionDate;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@QueueIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.Gen_QueueIds;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_OSTActions.CommentsSourceSystemLkup;
                _lstParameters.Add(sqlParam);
               
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MARxAddressResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.MARxAddressCompletedLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PDPAutoEnrolleeInd";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.PDPAutoEnrolleeInd;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);


                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_MassOSTActions, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _lstParameters = null;
            }
        }

        public long ERSCaseIDForERNCaseNumber(string strERSCaseNumber)
        {
            DAHelper dah = new DAHelper();
            //errorMessage = string.Empty;
            //strErsCaseId = string.Empty;

            string query = "SELECT TOP 1 GEN_QUEUEID FROM GEN_QUEUE WHERE ERNCaseID = '{0}'";
            query = string.Format(query, strERSCaseNumber);
            long executionResult = dah.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet dsTable, out string erorrMessage, true);
            if (executionResult == 0)
            { 
                var dr = dsTable.Tables[0].Rows[0];
                return (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                }
            else
                return 0;
        }

        public ExceptionTypes UpdateMBIValue(long gEN_QueueId, string MBI)
        {
            DAHelper dah = new DAHelper();
            //errorMessage = string.Empty;
            //strErsCaseId = string.Empty;

            string query = "UPDATE GEN_Queue SET MemberMedicareId = '{0}',IsMBIProcessed = '{1}',UTCMBIProcessedOn = GetUTCDate() ,MBIProcessedByRef = '{2}' where GEN_QueueId = '{3}'";
            query = string.Format(query, MBI, 1, 1, gEN_QueueId);
            long executionResult = dah.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet dsTable, out string erorrMessage, true);
            if (executionResult == 0)
            {
                return ExceptionTypes.Success;

            }
            else if (executionResult == 2)
            {
                return ExceptionTypes.ZeroRecords;
            }
            else
            {
                return ExceptionTypes.UnknownError;
            }
        }
        /// <summary>
        /// Process EGHP
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public ExceptionTypes EGHPExcelProcess(DataTable dt,out string errorMessage)
        {
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();
            errorMessage = string.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            try
            {
                DataSet ds = new DataSet("EGHPExcel");
                ds.Tables.Add(dt);
                string xmlData = ds.GetXml();


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@XMLData";
                sqlParam.SqlDbType = SqlDbType.NVarChar;
                sqlParam.Value = xmlData;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_BGP_INS_UPD_OOA_EGHP_Exclusion, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;

            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return ExceptionTypes.UnknownError;
            }
        }

        /// <summary>
        /// Eligibility Actions MassUpdate
        /// </summary>
        /// <param name="objDOGEN_EligibilityActions"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes EligibilityActionUpdate(DOGEN_EligibilityActions objDOGEN_EligibilityActions, out string errorMessage)
        {
            try
            {
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (!objDOGEN_EligibilityActions.GEN_BulkImportRef.IsNull() && objDOGEN_EligibilityActions.GEN_BulkImportRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_BulkImportRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_EligibilityActions.GEN_BulkImportRef;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_EligibilityActions.QueueLkup.IsNull() && objDOGEN_EligibilityActions.QueueLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@QueueLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_EligibilityActions.QueueLkup;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_EligibilityActions.BusinessSegmentLkup.IsNull() && objDOGEN_EligibilityActions.BusinessSegmentLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_EligibilityActions.BusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_EligibilityActions.DiscrepancyCategoryLkup.IsNull() && objDOGEN_EligibilityActions.DiscrepancyCategoryLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_EligibilityActions.DiscrepancyCategoryLkup;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ActionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.HICN;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.LastName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DateofBirth";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.DateofBirth;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.PBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeCodeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.TransactionTypeCodeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ApplicationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.ApplicationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ElectionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ElectionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ResolutionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherResolution";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.OtherResolution;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RootCauseLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RootCauseLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EGHPIndicator";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.EGHPIndicator;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.PendReasonLkup;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContainsErrorsLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ContainsErrorsLkup;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FirstLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.FirstLetterMailDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SecondLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.SecondLetterMailDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Reason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.Reason;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@InitialAddressVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.InitialAddressVerificationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberResponseVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.MemberResponseVerificationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedStateLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.MemberVerifiedState;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherRootCause";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.OtherRootCause;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.Comments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.LoginID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RoleLKup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRRequestedEffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRActionRequestedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPROtherActionRequested;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRSupervisorOrRequesterRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRReasonforRequest;
                parameters.Add(sqlParam);
                
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRTaskPerformedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPROtherTaskPerformed;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRCTMMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRCTMNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.RPREGHPMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPREmployerID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@QueueIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.Gen_QueueIds;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_EligibilityActions.CommentsSourceSystemLkup;
                parameters.Add(sqlParam);                

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_MassEligibilityActions, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }
        /// <summary>
        /// RPR Action MassUpdate
        /// </summary>
        /// <param name="objDOGEN_RPRActions"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes RPRActionUpdate(DOGEN_RPRActions objDOGEN_RPRActions, out string errorMessage)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();
            errorMessage = string.Empty;
            try
            {
                SqlParameter sqlParam;

                if (!objDOGEN_RPRActions.GEN_BulkImportRef.IsNull() && objDOGEN_RPRActions.GEN_BulkImportRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_BulkImportRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_RPRActions.GEN_BulkImportRef;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_RPRActions.QueueLkup.IsNull() && objDOGEN_RPRActions.QueueLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@QueueLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_RPRActions.QueueLkup;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_RPRActions.BusinessSegmentLkup.IsNull() && objDOGEN_RPRActions.BusinessSegmentLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_RPRActions.BusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }
                if (!objDOGEN_RPRActions.DiscrepancyCategoryLkup.IsNull() && objDOGEN_RPRActions.DiscrepancyCategoryLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_RPRActions.DiscrepancyCategoryLkup;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ActionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.RoleLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ResolutionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RootCauseLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.RootCauseLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.PendReasonLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AdjustedCreateDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.AdjustedCreateDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AdjustedCreateDateReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.AdjustedCreateDateReasonLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherAdjustedCreateDateReason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.OtherAdjustedCreateDateReason;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContainsErrorsLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ContainsErrorsLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SubmissionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.SubmissionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPCSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.RPCSubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSAccountManagerSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.CMSAccountManagerSubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSAccountManagerApprovalDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.CMSAccountManagerApprovalDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ElectionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ElectionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.FDRStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherFDRStatus";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.OtherFDRStatus;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRReceivedDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.FDRReceivedDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRCodeReceived";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.FDRCodeReceived;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.FDRDescription;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSProcessDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.CMSProcessDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionType";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.TransactionType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeCodeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.TransactionTypeCodeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExplanationOfRootCauseLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ExplanationOfRootCauseLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@VerifiedRootCauseLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.VerifiedRootCauseLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRRejectionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.FDRRejectionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.LastName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DateofBirth";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.DateofBirth;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.ContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.PBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ApplicationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.ApplicationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.EndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActualSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.ActualSubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReasonSubmissionRejected";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.ReasonSubmissionRejected;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestedSCC";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.RequestedSCC;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestedZIP";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.RequestedZIP;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.ResubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PotentialSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_RPRActions.PotentialSubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_RPRActions.LoginUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_RPRActions.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.Comments;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@QueueIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_RPRActions.Gen_QueueIds;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_RPRActions.CommentsSourceSystemLkup;
                parameters.Add(sqlParam);

                if (!objDOGEN_RPRActions.PlanError.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@PlanError";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOGEN_RPRActions.PlanError;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IncludeInTodaysSubmission";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_RPRActions.IncludeInTodaysSubmission;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_MassRPRActions, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="lstDOCMN_LookupType"></param>
        /// <returns></returns>
        private ExceptionTypes MapRecordsLockedDetails(DataSet dsResult, out UIRecordsLock objRecordsLocked)
        {
            objRecordsLocked = new UIRecordsLock();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];

                    objRecordsLocked.CMN_RecordsLockedId = Convert.ToInt64(dr["CMN_RecordsLockedId"]);
                    objRecordsLocked.ScreenLkup = Convert.ToInt64(dr["ScreenLkup"]);
                    objRecordsLocked.CaseId = Convert.ToInt64(dr["CaseId"]);
                    objRecordsLocked.StartTime = Convert.ToDateTime(dr["StartTime"]);

                    if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        objRecordsLocked.EndTime = Convert.ToDateTime(dr["UTCCreatedOn"]);

                    objRecordsLocked.Guid = Convert.ToString(dr["Guid"]);
                    objRecordsLocked.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    objRecordsLocked.UTCCreatedOn = (!DBNull.Value.Equals(dr["UTCCreatedOn"])) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow;
                    objRecordsLocked.CreatedByRef = (!DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0;
                    objRecordsLocked.CreatedByName = Convert.ToString(dr["CreatedByName"]);
                    objRecordsLocked.UTCLastUpdatedOn = (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"])) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : DateTime.UtcNow;
                    objRecordsLocked.LastUpdatedByRef = (!DBNull.Value.Equals(dr["LastUpdatedByRef"])) ? Convert.ToInt64(dr["LastUpdatedByRef"]) : 0;
                }
                return ExceptionTypes.Success;
            }
            catch (Exception ex)
            {
                //need log
                return ExceptionTypes.UnknownError;
            }
        }

        /// <summary>
        /// Unlock record
        /// </summary>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <returns></returns>
        public ExceptionTypes UnlockRecord(long screenLkup, long caseID)
        {
            List<SqlParameter> lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            string errorMessage = string.Empty;
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;

            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = screenLkup;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CaseId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = caseID;
                lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_UnlockRecord, lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                lstParameters = null;
            }
        }

        /// <summary>
        /// To get count - Record locked for current user
        /// </summary>
        /// <param name="loginUserID"></param>
        /// <param name="screenLkup"></param>
        /// <param name="caseID"></param>
        /// <param name="recordLockedCount"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes ValidateLockBeforeSave(long loginUserID, long screenLkup, long caseID, out long recordLockedCount, out string errorMessage)
        {
            recordLockedCount = 0;
            errorMessage = string.Empty;
            try
            {
                string query = @"SELECT	Count(1) As RecordLockedCount
                                 FROM	CMN_RecordsLocked RL 
                                 WHERE	RL.IsActive=1 AND RL.ScreenLkup='" + screenLkup + "' AND RL.[CaseId]='" + caseID + "' AND CreatedByRef='" + loginUserID + "'";

                DataSet dsResult;
                string errorMsg;
                long executionResult = _objDAHelper.ExecuteSQL(query, null, out dsResult, out errorMsg, true);

                if ((executionResult != (long)ExceptionTypes.Success && executionResult != (long)ExceptionTypes.ZeroRecords) || !string.IsNullOrEmpty(errorMsg))
                {
                    //ErrorLog
                    return ExceptionTypes.UnknownError;
                }

                if (executionResult == (long)ExceptionTypes.Success && dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    recordLockedCount = Convert.ToInt64(dsResult.Tables[0].Rows[0]["RecordLockedCount"]);
                }

                return (ExceptionTypes)executionResult;
            }
            catch (Exception ex)
            {
                errorMessage += ex.Message + ex.StackTrace;
                //ErrorLog
            }
            return ExceptionTypes.UnknownError;
        }

        public ExceptionTypes SearchRecords(long? TimeZone, long pageType, long loggedInUserId, long rowCount, SearchCriteria objSearchCriteria, out List<SearchResults> lstSearchResults,out string totalRecordCount,out string errorMessage)
        {
            lstSearchResults = new List<SearchResults>();
            totalRecordCount = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty; ;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objSearchCriteria.WorkItemId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkItemId"; sqlParam.Value = objSearchCriteria.WorkItemId; parameters.Add(sqlParam); }
                if (objSearchCriteria.BusinessSegmentLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@BusinessSegmentLkup"; sqlParam.Value = objSearchCriteria.BusinessSegmentLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.WorkBasketLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkBasketLkup"; sqlParam.Value = objSearchCriteria.WorkBasketLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.CurrentHICN != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CurrentHICN"; sqlParam.Value = objSearchCriteria.CurrentHICN; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyCategoryLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyCategoryLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyCategoryLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyTypeLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyTypeLkupNot > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyTypeLkupNot"; sqlParam.Value = objSearchCriteria.DiscrepancyTypeLkupNot; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstName"; sqlParam.Value = objSearchCriteria.FirstName; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastName"; sqlParam.Value = objSearchCriteria.LastName; parameters.Add(sqlParam); }
                if (objSearchCriteria.GenderLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@GenderLkup"; sqlParam.Value = objSearchCriteria.GenderLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberSCCCode != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberSCCCode"; sqlParam.Value = objSearchCriteria.MemberSCCCode; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancySourceLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancySourceLkup"; sqlParam.Value = objSearchCriteria.DiscrepancySourceLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LOBLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LOBLkup"; sqlParam.Value = objSearchCriteria.LOBLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.ContractIDLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ContractIDLkup"; sqlParam.Value = objSearchCriteria.ContractIDLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PBPLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PBPLkup"; sqlParam.Value = objSearchCriteria.PBPLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedOperator > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastUpdatedOperator"; sqlParam.Value = objSearchCriteria.LastUpdatedOperator; parameters.Add(sqlParam); }
                if (objSearchCriteria.AssignedTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AssignedTo"; sqlParam.Value = objSearchCriteria.AssignedTo; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRActionRequested > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ActionRequested"; sqlParam.Value = objSearchCriteria.RPRActionRequested; parameters.Add(sqlParam); }
                if (objSearchCriteria.SupervisiorofthePersonEnteringtheRequest > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SupervisiorofthePersonEnteringtheRequest "; sqlParam.Value = objSearchCriteria.SupervisiorofthePersonEnteringtheRequest; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMMember != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMMember"; sqlParam.Value = objSearchCriteria.RPRCTMMember == 1 ? true : false; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMNumber != string.Empty) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMNumber"; sqlParam.Value = objSearchCriteria.RPRCTMNumber; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPREGHPMember != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@EGHPMember"; sqlParam.Value = objSearchCriteria.RPREGHPMember == 1 ? true : false; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPREmployerID != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@EmployerId"; sqlParam.Value = objSearchCriteria.RPREmployerID; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRCodeReceived != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRCodeReceived"; sqlParam.Value = objSearchCriteria.FDRCodeReceived; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRStatus > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRStatus"; sqlParam.Value = objSearchCriteria.FDRStatus; parameters.Add(sqlParam); }
                if (objSearchCriteria.VerifiedRootCause > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@VerifiedRootCause"; sqlParam.Value = objSearchCriteria.VerifiedRootCause; parameters.Add(sqlParam); }
                if (objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@TaskBeingPerformedWhenThisDiscrepancyWasIdentified"; sqlParam.Value = objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified; parameters.Add(sqlParam); }
                if (objSearchCriteria.SubmissionTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SubmissionTypeLkup"; sqlParam.Value = objSearchCriteria.SubmissionTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PendReason > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PendReason"; sqlParam.Value = objSearchCriteria.PendReason; parameters.Add(sqlParam); }
                if (objSearchCriteria.Resolution > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Resolution"; sqlParam.Value = objSearchCriteria.Resolution; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRRequestor > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPRRequestor"; sqlParam.Value = objSearchCriteria.RPRRequestor; parameters.Add(sqlParam); }
                if (objSearchCriteria.Queue > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Queue"; sqlParam.Value = objSearchCriteria.Queue; parameters.Add(sqlParam); }
                if (objSearchCriteria.Status > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Status"; sqlParam.Value = objSearchCriteria.Status; parameters.Add(sqlParam); }
                if (objSearchCriteria.StatusNot > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@StatusNot"; sqlParam.Value = objSearchCriteria.StatusNot; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeFrom > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeFrom"; sqlParam.Value = objSearchCriteria.CaseAgeFrom; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeTo"; sqlParam.Value = objSearchCriteria.CaseAgeTo; parameters.Add(sqlParam); }
                //DateId Fiedls
                if (objSearchCriteria.DiscrepancyStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyStartDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyEndDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DOBId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DOBId"; sqlParam.Value = objSearchCriteria.DOBId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceStartDateId"; sqlParam.Value = objSearchCriteria.ComplianceStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceEndDateId"; sqlParam.Value = objSearchCriteria.ComplianceEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationStartDateId"; sqlParam.Value = objSearchCriteria.CaseCreationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationEndDateId"; sqlParam.Value = objSearchCriteria.CaseCreationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedStartDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedEndDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationStartDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationEndDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveStartDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveEndDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateStartDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateEndDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionStartDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionEndDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalStartDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalEndDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedStartDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedEndDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionStartDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionEndDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionEndDateId; parameters.Add(sqlParam); }

                if (objSearchCriteria.GPSHouseholdID != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@GPSHouseHoldId "; sqlParam.Value = objSearchCriteria.GPSHouseholdID; parameters.Add(sqlParam); }


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TimeZoneLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = 0;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UserLoginId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loggedInUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PageType";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = pageType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsMassUpdate";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsMassUpdate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionReplyCode";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objSearchCriteria.TransactionReplyCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objSearchCriteria.MemberVerifiedState;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedCountryCode";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objSearchCriteria.MemberVerifiedCountyCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsRestricted;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TopCount";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = rowCount;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PDPAutoEnrolleeInd";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objSearchCriteria.PDPAutoEnrolleeInd;
                parameters.Add(sqlParam);

                if (objSearchCriteria.DisenrollmentFromDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentFromDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentFromDateId;
                    parameters.Add(sqlParam);
                }
                if (objSearchCriteria.DisenrollmentToDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentToDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentToDateId;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objSearchCriteria.EmployerGroupNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueSearch, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);


                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }


                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapSearchResults(TimeZone, dsTable, objSearchCriteria.IsRestricted, out lstSearchResults,out totalRecordCount);
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    if (dsTable.Tables.Count > 1 && dsTable.Tables[1].Rows.Count > 0)
                    {
                        MapSearchResults(TimeZone, dsTable, objSearchCriteria.IsRestricted, out lstSearchResults, out totalRecordCount);
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                throw ex;
            }
        }

        private void MapSearchResults(long? TimeZone, DataSet dstTable, bool isRestricted, out List<SearchResults> lstSearchResults,out string totalRecordCount)
        {
            lstSearchResults = new List<SearchResults>();
            totalRecordCount = string.Empty;
            if (dstTable.Tables.Count > 0)
            {
                foreach (DataRow dr in dstTable.Tables[0].Rows)
                {
                    SearchResults objSearchResults = new SearchResults();
                    if (dr.Table.Columns.Contains("WorkItemID"))
                    {
                        if (!DBNull.Value.Equals(dr["WorkItemID"]))
                        {
                            objSearchResults.WorkItemID = (long?)dr["WorkItemID"];
                        }
                    }
                    if (dr.Table.Columns.Contains("AssignedToRef"))
                    {
                        if (!DBNull.Value.Equals(dr["AssignedToRef"]))
                        {
                            objSearchResults.AssignedToRef = (long?)dr["AssignedToRef"];
                        }
                    }
                    if (dr.Table.Columns.Contains("QueueProgressTypeLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["QueueProgressTypeLkup"]))
                        {
                            objSearchResults.QueueProgressTypeLkup = (long?)dr["QueueProgressTypeLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("QueueProgressType"))
                    {
                        if (!DBNull.Value.Equals(dr["QueueProgressType"]))
                        {
                            objSearchResults.QueueProgressType = Convert.ToString(dr["QueueProgressType"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("Aging"))
                    {
                        if (!DBNull.Value.Equals(dr["Aging"]))
                        {
                            long aging;
                            if (long.TryParse(dr["Aging"].ToString(), out aging))
                                objSearchResults.Aging = aging;
                        }
                    }
                    if (dr.Table.Columns.Contains("Urgency"))
                    {
                        if (!DBNull.Value.Equals(dr["Urgency"]))
                        {
                            long Urgency;
                            if (long.TryParse(dr["Urgency"].ToString(), out Urgency))
                                objSearchResults.Urgency = Urgency;
                        }
                    }
                    if (dr.Table.Columns.Contains("AssignedTo"))
                    {
                        if (!DBNull.Value.Equals(dr["AssignedTo"]))
                        {
                            objSearchResults.AssignedTo = dr["AssignedTo"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegment"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegment"]))
                        {
                            objSearchResults.BusinessSegment = dr["BusinessSegment"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberSCCCode"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberSCCCode"]))
                        {
                            objSearchResults.MemberSCCCode = dr["MemberSCCCode"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberID"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberID"]))
                        {
                            objSearchResults.MemberID = dr["MemberID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberCurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberCurrentHICN"]))
                        {
                            objSearchResults.MemberCurrentHICN = dr["MemberCurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHouseholdID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHouseholdID"]))
                        {
                            objSearchResults.GPSHouseholdID = dr["GPSHouseholdID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberFirstName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberFirstName"]))
                        {
                            objSearchResults.MemberFirstName = dr["MemberFirstName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberMiddleName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberMiddleName"]))
                        {
                            objSearchResults.MemberMiddleName = dr["MemberMiddleName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberLastName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLastName"]))
                        {
                            objSearchResults.MemberLastName = dr["MemberLastName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberContractID"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberContractID"]))
                        {
                            objSearchResults.MemberContractID = dr["MemberContractID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberDOB"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberDOB"]))
                        {
                            objSearchResults.MemberDOB = Convert.ToDateTime(dr["MemberDOB"]);
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberPBP"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberPBP"]))
                        {
                            objSearchResults.MemberPBP = dr["MemberPBP"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberLOB"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLOB"]))
                        {
                            objSearchResults.MemberLOB = dr["MemberLOB"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberGender"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberGender"]))
                        {
                            objSearchResults.MemberGender = dr["MemberGender"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("DiscrepancyType"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyType"]))
                        {
                            objSearchResults.DiscrepancyType = dr["DiscrepancyType"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentStatus"]))
                        {
                            objSearchResults.MostRecentStatus = dr["MostRecentStatus"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentStatusLkup"]))
                        {
                            objSearchResults.MostRecentStatusLkup = dr["MostRecentStatusLkup"].ToInt64();
                        }
                    }

                    if (dr.Table.Columns.Contains("MostRecentWorkQueueLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentWorkQueueLkup"]))
                        {
                            objSearchResults.MostRecentQueueLkup = dr["MostRecentWorkQueueLkup"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentQueue"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentQueue"]))
                        {
                            objSearchResults.MostRecentQueue = dr["MostRecentQueue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentAction"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentAction"]))
                        {
                            objSearchResults.MostRecentAction = dr["MostRecentAction"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("CurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["CurrentHICN"]))
                        {
                            objSearchResults.CurrentHICN = dr["CurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHICN"]))
                        {
                            objSearchResults.GPSHICN = dr["GPSHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MMRHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MMRHICN"]))
                        {
                            objSearchResults.MMRHICN = dr["MMRHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("FirstName"))
                    {
                        if (!DBNull.Value.Equals(dr["FirstName"]))
                        {
                            objSearchResults.FirstName = dr["FirstName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategory"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategory"]))
                        {
                            objSearchResults.DiscrepancyCategory = dr["DiscrepancyCategory"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"]))
                        {
                            objSearchResults.DiscrepancyCategoryLkup = (long?)dr["DiscrepancyCategoryLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyStartDate"]))
                        {
                            objSearchResults.DiscrepancyStartDate = Convert.ToDateTime(dr["DiscrepancyStartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ComplianceStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["ComplianceStartDate"]))
                        {
                            objSearchResults.ComplianceStartDate = Convert.ToDateTime(dr["ComplianceStartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("AdjustedComplianceStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["AdjustedComplianceStartDate"]))
                        {
                            objSearchResults.AdjustedComplianceStartDate = Convert.ToDateTime(dr["AdjustedComplianceStartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyEndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyEndDate"]))
                        {
                            objSearchResults.DiscrepancyEndDate = Convert.ToDateTime(dr["DiscrepancyEndDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("Reason"))
                    {
                        if (!DBNull.Value.Equals(dr["Reason"]))
                        {
                            objSearchResults.Reason = dr["Reason"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("Resolution"))
                    {
                        if (!DBNull.Value.Equals(dr["Resolution"]))
                        {
                            objSearchResults.Resolution = dr["Resolution"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ReferencedEligibilityCaseIndicator"))
                    {
                        if (!DBNull.Value.Equals(dr["ReferencedEligibilityCaseIndicator"]))
                        {
                            objSearchResults.ReferencedEligibilityCaseIndicator = dr["ReferencedEligibilityCaseIndicator"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MMRPBP"))
                    {
                        if (!DBNull.Value.Equals(dr["MMRPBP"]))
                        {
                            objSearchResults.MMRPBP = dr["MMRPBP"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSIndividualID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSIndividualID"]))
                        {
                            objSearchResults.GPSIndividualID = dr["GPSIndividualID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancySource"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancySource"]))
                        {
                            objSearchResults.DiscrepancySource = dr["DiscrepancySource"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("NTID"))
                    {
                        if (!DBNull.Value.Equals(dr["NTID"]))
                        {
                            objSearchResults.DiscrepancySource = dr["NTID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("SubmissionType"))
                    {
                        if (!DBNull.Value.Equals(dr["SubmissionType"]))
                        {
                            objSearchResults.SubmissionType = dr["SubmissionType"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRCTMMember"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRCTMMember"]))
                        {
                            objSearchResults.RPRCTMMember = dr["RPRCTMMember"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPREGHPMember"))
                    {
                        if (!DBNull.Value.Equals(dr["RPREGHPMember"]))
                        {
                            objSearchResults.RPREGHPMember = dr["RPREGHPMember"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RequestedEffectiveDate"))
                    {
                        if (!DBNull.Value.Equals(dr["RequestedEffectiveDate"]))
                        {
                            objSearchResults.RPRRequestedEffectiveDate = Convert.ToDateTime(dr["RequestedEffectiveDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRActionRequested"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRActionRequested"]))
                        {
                            objSearchResults.RPRActionRequested = dr["RPRActionRequested"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("PotentialSubmissionDate"))
                    {
                        if (!DBNull.Value.Equals(dr["PotentialSubmissionDate"]))
                        {
                            objSearchResults.PotentialSubmissionDate = Convert.ToDateTime(dr["PotentialSubmissionDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ActionRequested"))
                    {
                        if (!DBNull.Value.Equals(dr["ActionRequested"]))
                        {
                            objSearchResults.RPRActionRequested = dr["ActionRequested"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPCSubmissionDate"))
                    {
                        if (!DBNull.Value.Equals(dr["RPCSubmissionDate"]))
                        {
                            objSearchResults.RPCSubmissionDate = Convert.ToDateTime(dr["RPCSubmissionDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRReceivedDate"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRReceivedDate"]))
                        {
                            objSearchResults.FDRReceivedDate = Convert.ToDateTime(dr["FDRReceivedDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRCodeReceived"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRCodeReceived"]))
                        {
                            objSearchResults.FDRCodeReceived = dr["FDRCodeReceived"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRStatus"]))
                        {
                            objSearchResults.FDRStatus = dr["FDRStatus"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRRequestor"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRRequestor"]))
                        {
                            objSearchResults.RPRRequestor = dr["RPRRequestor"].ToString();
                        }
                    }
                    ////////////////////////////////////////////////////////////////////
                    if (dr.Table.Columns.Contains("ApplicationDate"))
                    {
                        if (!DBNull.Value.Equals(dr["ApplicationDate"]))
                        {
                            objSearchResults.ApplicationDate = Convert.ToDateTime(dr["ApplicationDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("EndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EndDate"]))
                        {
                            objSearchResults.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRActionRequestedLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRActionRequestedLkup"]))
                        {
                            objSearchResults.RPRActionRequestedLkup = (long?)dr["RPRActionRequestedLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("ElectionType"))
                    {
                        if (!DBNull.Value.Equals(dr["ElectionType"]))
                        {
                            objSearchResults.ElectionType = dr["ElectionType"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("Resolution"))
                    {
                        if (!DBNull.Value.Equals(dr["Resolution"]))
                        {
                            objSearchResults.Resolution = dr["Resolution"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objSearchResults.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                        {
                            objSearchResults.CreatedByRef = Convert.ToInt64(dr["CreatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedBy"]))
                        {
                            objSearchResults.CreatedBy = dr["CreatedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objSearchResults.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(), TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                        {
                            objSearchResults.LastUpdatedByRef = Convert.ToInt64(dr["LastUpdatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedBy"]))
                        {
                            objSearchResults.LastUpdatedBy = dr["LastUpdatedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLockedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                        {
                            objSearchResults.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(), TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByRef"]))
                        {
                            objSearchResults.LockedByRef = Convert.ToInt64(dr["LockedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedBy"]))
                        {
                            objSearchResults.LockedBy = dr["LockedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("PendedbyRef"))
                    {
                        if (!DBNull.Value.Equals(dr["PendedbyRef"]))
                        {
                            objSearchResults.PendedByRef = dr["PendedbyRef"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("PendedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["PendedBy"]))
                        {
                            objSearchResults.PendedBy = dr["PendedBy"].NullToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("OOALetterStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["OOALetterStatusLkup"]))
                        {
                            objSearchResults.OOALetterStatusLkup = dr["OOALetterStatusLkup"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("OOALetterStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["OOALetterStatus"]))
                        {
                            objSearchResults.OOALetterStatus = dr["OOALetterStatus"].NullToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("CMSTransactionStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["CMSTransactionStatusLkup"]))
                        {
                            objSearchResults.CMSTransactionStatusLkup = dr["CMSTransactionStatusLkup"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberVerifiedState"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberVerifiedState"]))
                        {
                            objSearchResults.MemberVerifiedState = dr["MemberVerifiedState"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberVerifiedCountyCode"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberVerifiedCountyCode"]))
                        {
                            objSearchResults.MemberVerifiedCountyCode = dr["MemberVerifiedCountyCode"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("PendReason"))
                    {
                        if (!DBNull.Value.Equals(dr["PendReason"]))
                        {
                            objSearchResults.PendReason = dr["PendReason"].ToString();
                        }
                    }
                    lstSearchResults.Add(objSearchResults);
                }
            }
            if (dstTable.Tables.Count > 1 && dstTable.Tables[1].Rows.Count > 0)
            {
                DataRow dr = dstTable.Tables[1].Rows[0];
                if (isRestricted)
                    totalRecordCount = "<b class='TotalCountText'>Actual Total : </b><b>" + dr["TotalRecords"].NullToString() + "</b> <b class='TotalCountText'>Total No. of records based on User Permissions : </b><b>" + (dr["UnRestrictedRecords"].ToLong() + dr["RestrictedRecords"].ToLong())+"</b>";
                else
                    totalRecordCount = "<b class='TotalCountText'>Actual Total : </b><b>" + dr["TotalRecords"].NullToString() + "</b> <b class='TotalCountText'>Total No. of records based on User Permissions : </b><b>" + dr["UnRestrictedRecords"].NullToString() + "</b>";
            }
        }

        public ExceptionTypes SearchRecordsToUnlock(long? TimeZone,long pageType, long loggedInUserId, SearchCriteria objSearchCriteria, out List<UnlockSearchResults> lstSearchResults)
        {
            lstSearchResults = new List<UnlockSearchResults>();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objSearchCriteria.WorkItemId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkItemId"; sqlParam.Value = objSearchCriteria.WorkItemId; parameters.Add(sqlParam); }
                if (objSearchCriteria.BusinessSegmentLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@BusinessSegmentLkup"; sqlParam.Value = objSearchCriteria.BusinessSegmentLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.WorkBasketLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkBasketLkup"; sqlParam.Value = objSearchCriteria.WorkBasketLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.CurrentHICN != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CurrentHICN"; sqlParam.Value = objSearchCriteria.CurrentHICN; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyCategoryLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyCategoryLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyCategoryLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyTypeLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstName"; sqlParam.Value = objSearchCriteria.FirstName; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastName"; sqlParam.Value = objSearchCriteria.LastName; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancySourceLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancySourceLkup"; sqlParam.Value = objSearchCriteria.DiscrepancySourceLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LOBLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LOBLkup"; sqlParam.Value = objSearchCriteria.LOBLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.ContractIDLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ContractNumer"; sqlParam.Value = objSearchCriteria.ContractIDLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PBPLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PBP"; sqlParam.Value = objSearchCriteria.PBPLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedOperator > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastUpdatedOperator"; sqlParam.Value = objSearchCriteria.LastUpdatedOperator; parameters.Add(sqlParam); }
                if (objSearchCriteria.AssignedTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AssignedTo"; sqlParam.Value = objSearchCriteria.AssignedTo; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRActionRequested > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ActionRequested"; sqlParam.Value = objSearchCriteria.RPRActionRequested; parameters.Add(sqlParam); }
                if (objSearchCriteria.SupervisiorofthePersonEnteringtheRequest > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SupervisiorofthePersonEnteringtheRequest "; sqlParam.Value = objSearchCriteria.SupervisiorofthePersonEnteringtheRequest; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMMember != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMMember"; sqlParam.Value = objSearchCriteria.RPRCTMMember; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMNumber != string.Empty) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMNumber"; sqlParam.Value = objSearchCriteria.RPRCTMNumber; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPREGHPMember > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@EGHPMember"; sqlParam.Value = objSearchCriteria.RPREGHPMember; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRCodeReceived != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRCodeReceived"; sqlParam.Value = objSearchCriteria.FDRCodeReceived; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRStatus > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRStatus"; sqlParam.Value = objSearchCriteria.FDRStatus; parameters.Add(sqlParam); }
                if (objSearchCriteria.VerifiedRootCause > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@VerifiedRootCause"; sqlParam.Value = objSearchCriteria.VerifiedRootCause; parameters.Add(sqlParam); }
                if (objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@TaskBeingPerformedWhenThisDiscrepancyWasIdentified"; sqlParam.Value = objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified; parameters.Add(sqlParam); }
                if (objSearchCriteria.SubmissionTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SubmissionTypeLkup"; sqlParam.Value = objSearchCriteria.SubmissionTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PendReason > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PendReason"; sqlParam.Value = objSearchCriteria.PendReason; parameters.Add(sqlParam); }
                if (objSearchCriteria.Resolution > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Resolution"; sqlParam.Value = objSearchCriteria.Resolution; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRRequestor > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPRRequestor"; sqlParam.Value = objSearchCriteria.RPRRequestor; parameters.Add(sqlParam); }
                if (objSearchCriteria.Queue > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Queue"; sqlParam.Value = objSearchCriteria.Queue; parameters.Add(sqlParam); }
                if (objSearchCriteria.Status > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Status"; sqlParam.Value = objSearchCriteria.Status; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeFrom > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeFrom"; sqlParam.Value = objSearchCriteria.CaseAgeFrom; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeTo"; sqlParam.Value = objSearchCriteria.CaseAgeTo; parameters.Add(sqlParam); }
                //DateId Fiedls
                if (objSearchCriteria.DiscrepancyStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyStartDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyEndDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DOBId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DOBId"; sqlParam.Value = objSearchCriteria.DOBId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceStartDateId"; sqlParam.Value = objSearchCriteria.ComplianceStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceEndDateId"; sqlParam.Value = objSearchCriteria.ComplianceEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationStartDateId"; sqlParam.Value = objSearchCriteria.CaseCreationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationEndDateId"; sqlParam.Value = objSearchCriteria.CaseCreationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedStartDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedEndDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationStartDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationEndDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveStartDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveEndDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateStartDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateEndDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionStartDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionEndDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalStartDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalEndDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedStartDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedEndDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionStartDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionEndDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionEndDateId; parameters.Add(sqlParam); }

                if (objSearchCriteria.GPSHouseholdID != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@GPSHouseHoldId "; sqlParam.Value = objSearchCriteria.GPSHouseholdID; parameters.Add(sqlParam); }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TimeZoneLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = 0;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UserLoginId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loggedInUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PageType";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = pageType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsUnlock";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsUnlock;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsRestricted;
                parameters.Add(sqlParam);

                if (objSearchCriteria.DisenrollmentFromDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentFromDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentFromDateId;
                    parameters.Add(sqlParam);
                }
                if (objSearchCriteria.DisenrollmentToDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentToDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentToDateId;
                    parameters.Add(sqlParam);
                }
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objSearchCriteria.EmployerGroupNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueSearch, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapUnlockSearchResults(TimeZone,dsTable, out lstSearchResults);
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExceptionTypes SearchRecordsToReassign(long? TimeZone,long pageType, long loggedInUserId, SearchCriteria objSearchCriteria, out List<UnlockSearchResults> lstSearchResults)
        {
            lstSearchResults = new List<UnlockSearchResults>();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objSearchCriteria.WorkItemId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkItemId"; sqlParam.Value = objSearchCriteria.WorkItemId; parameters.Add(sqlParam); }
                if (objSearchCriteria.BusinessSegmentLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@BusinessSegmentLkup"; sqlParam.Value = objSearchCriteria.BusinessSegmentLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.WorkBasketLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@WorkBasketLkup"; sqlParam.Value = objSearchCriteria.WorkBasketLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.CurrentHICN != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CurrentHICN"; sqlParam.Value = objSearchCriteria.CurrentHICN; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyCategoryLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyCategoryLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyCategoryLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyTypeLkup"; sqlParam.Value = objSearchCriteria.DiscrepancyTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstName"; sqlParam.Value = objSearchCriteria.FirstName; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastName != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastName"; sqlParam.Value = objSearchCriteria.LastName; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancySourceLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancySourceLkup"; sqlParam.Value = objSearchCriteria.DiscrepancySourceLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LOBLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LOBLkup"; sqlParam.Value = objSearchCriteria.LOBLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.ContractIDLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ContractNumer"; sqlParam.Value = objSearchCriteria.ContractIDLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PBPLkup != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PBP"; sqlParam.Value = objSearchCriteria.PBPLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedOperator > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@LastUpdatedOperator"; sqlParam.Value = objSearchCriteria.LastUpdatedOperator; parameters.Add(sqlParam); }
                if (objSearchCriteria.AssignedTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AssignedTo"; sqlParam.Value = objSearchCriteria.AssignedTo; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRActionRequested > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ActionRequested"; sqlParam.Value = objSearchCriteria.RPRActionRequested; parameters.Add(sqlParam); }
                if (objSearchCriteria.SupervisiorofthePersonEnteringtheRequest > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SupervisiorofthePersonEnteringtheRequest "; sqlParam.Value = objSearchCriteria.SupervisiorofthePersonEnteringtheRequest; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMMember != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMMember"; sqlParam.Value = objSearchCriteria.RPRCTMMember; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRCTMNumber != string.Empty) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CTMNumber"; sqlParam.Value = objSearchCriteria.RPRCTMNumber; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPREGHPMember > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@EGHPMember"; sqlParam.Value = objSearchCriteria.RPREGHPMember; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRCodeReceived != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRCodeReceived"; sqlParam.Value = objSearchCriteria.FDRCodeReceived; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRStatus > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRStatus"; sqlParam.Value = objSearchCriteria.FDRStatus; parameters.Add(sqlParam); }
                if (objSearchCriteria.VerifiedRootCause > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@VerifiedRootCause"; sqlParam.Value = objSearchCriteria.VerifiedRootCause; parameters.Add(sqlParam); }
                if (objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@TaskBeingPerformedWhenThisDiscrepancyWasIdentified"; sqlParam.Value = objSearchCriteria.TaskBeingPerformedWhenThisDiscrepancyWasIdentified; parameters.Add(sqlParam); }
                if (objSearchCriteria.SubmissionTypeLkup > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SubmissionTypeLkup"; sqlParam.Value = objSearchCriteria.SubmissionTypeLkup; parameters.Add(sqlParam); }
                if (objSearchCriteria.PendReason > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PendReason"; sqlParam.Value = objSearchCriteria.PendReason; parameters.Add(sqlParam); }
                if (objSearchCriteria.Resolution > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Resolution"; sqlParam.Value = objSearchCriteria.Resolution; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPRRequestor > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPRRequestor"; sqlParam.Value = objSearchCriteria.RPRRequestor; parameters.Add(sqlParam); }
                if (objSearchCriteria.Queue > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Queue"; sqlParam.Value = objSearchCriteria.Queue; parameters.Add(sqlParam); }
                if (objSearchCriteria.Status > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@Status"; sqlParam.Value = objSearchCriteria.Status; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeFrom > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeFrom"; sqlParam.Value = objSearchCriteria.CaseAgeFrom; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseAgeTo > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseAgeTo"; sqlParam.Value = objSearchCriteria.CaseAgeTo; parameters.Add(sqlParam); }
                //DateId Fiedls
                if (objSearchCriteria.DiscrepancyStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyStartDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DiscrepancyEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DiscrepancyEndDateId"; sqlParam.Value = objSearchCriteria.DiscrepancyEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.DOBId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@DOBId"; sqlParam.Value = objSearchCriteria.DOBId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FirstLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FirstLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.FirstLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailStartDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.SecondLetterMailEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@SecondLetterMailEndDateId"; sqlParam.Value = objSearchCriteria.SecondLetterMailEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceStartDateId"; sqlParam.Value = objSearchCriteria.ComplianceStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.ComplianceEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@ComplianceEndDateId"; sqlParam.Value = objSearchCriteria.ComplianceEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationStartDateId"; sqlParam.Value = objSearchCriteria.CaseCreationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CaseCreationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseCreationEndDateId"; sqlParam.Value = objSearchCriteria.CaseCreationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedStartDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.LastUpdatedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CaseLastUpdatedEndDateId"; sqlParam.Value = objSearchCriteria.LastUpdatedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationStartDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.MemberResponseVerificationEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@MemberResponseVerificationEndDateId"; sqlParam.Value = objSearchCriteria.MemberResponseVerificationEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveStartDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RequestedEffectiveEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RequestedEffectiveEndDateId"; sqlParam.Value = objSearchCriteria.RequestedEffectiveEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateStartDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.AdjustedCreateEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@AdjustedCreateEndDateId"; sqlParam.Value = objSearchCriteria.AdjustedCreateEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionStartDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.RPCSubmissionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@RPCSubmissionEndDateId"; sqlParam.Value = objSearchCriteria.RPCSubmissionEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalStartDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.CMSAccountManagerApprovalEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@CMSAccountManagerApprovalEndDateId"; sqlParam.Value = objSearchCriteria.CMSAccountManagerApprovalEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedStartDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.FDRReceivedEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@FDRReceivedEndDateId"; sqlParam.Value = objSearchCriteria.FDRReceivedEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionStartDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionStartDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionStartDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.PeerAuditCompletionEndDateId > 0) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@PeerAuditCompletionEndDateId"; sqlParam.Value = objSearchCriteria.PeerAuditCompletionEndDateId; parameters.Add(sqlParam); }
                if (objSearchCriteria.GPSHouseholdID != null) { sqlParam = new SqlParameter(); sqlParam.ParameterName = "@GPSHouseHoldId "; sqlParam.Value = objSearchCriteria.GPSHouseholdID; parameters.Add(sqlParam); }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TimeZoneLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = 0;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UserLoginId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loggedInUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PageType";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = pageType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsReAssign";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsReAssign;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objSearchCriteria.IsRestricted;
                parameters.Add(sqlParam);

                if (objSearchCriteria.DisenrollmentFromDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentFromDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentFromDateId;
                    parameters.Add(sqlParam);
                }
                if (objSearchCriteria.DisenrollmentToDateId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollmentToDateId";
                    sqlParam.Value = objSearchCriteria.DisenrollmentToDateId;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objSearchCriteria.EmployerGroupNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueSearch, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapUnlockSearchResults(TimeZone,dsTable, out lstSearchResults);
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MapUnlockSearchResults(long? TimeZone,DataSet dstTable, out List<UnlockSearchResults> lstSearchResults)
        {
            lstSearchResults = new List<UnlockSearchResults>();
            if (dstTable.Tables.Count > 0)
            {
                foreach (DataRow dr in dstTable.Tables[0].Rows)
                {
                    UnlockSearchResults objUnlockSearchResults = new UnlockSearchResults();
                    if (dr.Table.Columns.Contains("WorkItemID"))
                    {
                        if (!DBNull.Value.Equals(dr["WorkItemID"]))
                        {
                            objUnlockSearchResults.WorkItemID = (long?)dr["WorkItemID"];
                        }
                    }
                    if (dr.Table.Columns.Contains("AssignedToRef"))
                    {
                        if (!DBNull.Value.Equals(dr["AssignedToRef"]))
                        {
                            objUnlockSearchResults.AssignedToRef = (long?)dr["AssignedToRef"];
                        }
                    }
                    if (dr.Table.Columns.Contains("QueueProgressTypeLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["QueueProgressTypeLkup"]))
                        {
                            objUnlockSearchResults.QueueProgressTypeLkup = (long?)dr["QueueProgressTypeLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("QueueProgressType"))
                    {
                        if (!DBNull.Value.Equals(dr["QueueProgressType"]))
                        {
                            objUnlockSearchResults.QueueProgressType = Convert.ToString(dr["QueueProgressType"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("Aging"))
                    {
                        if (!DBNull.Value.Equals(dr["Aging"]))
                        {
                            long aging;
                            if (long.TryParse(dr["Aging"].ToString(), out aging))
                                objUnlockSearchResults.Aging = aging;
                        }
                    }
                    if (dr.Table.Columns.Contains("Urgency"))
                    {
                        if (!DBNull.Value.Equals(dr["Urgency"]))
                        {
                            long Urgency;
                            if (long.TryParse(dr["Urgency"].ToString(), out Urgency))
                                objUnlockSearchResults.Urgency = Urgency;
                        }
                    }
                    if (dr.Table.Columns.Contains("AssignedTo"))
                    {
                        if (!DBNull.Value.Equals(dr["AssignedTo"]))
                        {
                            objUnlockSearchResults.AssignedTo = dr["AssignedTo"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegment"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegment"]))
                        {
                            objUnlockSearchResults.BusinessSegment = dr["BusinessSegment"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberSCCCode"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberSCCCode"]))
                        {
                            objUnlockSearchResults.MemberSCCCode = dr["MemberSCCCode"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberID"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberID"]))
                        {
                            objUnlockSearchResults.MemberID = dr["MemberID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberCurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberCurrentHICN"]))
                        {
                            objUnlockSearchResults.MemberCurrentHICN = dr["MemberCurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHouseholdID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHouseholdID"]))
                        {
                            objUnlockSearchResults.GPSHouseholdID = dr["GPSHouseholdID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberFirstName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberFirstName"]))
                        {
                            objUnlockSearchResults.MemberFirstName = dr["MemberFirstName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberMiddleName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberMiddleName"]))
                        {
                            objUnlockSearchResults.MemberMiddleName = dr["MemberMiddleName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberLastName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLastName"]))
                        {
                            objUnlockSearchResults.MemberLastName = dr["MemberLastName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberContractID"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberContractID"]))
                        {
                            objUnlockSearchResults.MemberContractID = dr["MemberContractID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberDOB"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberDOB"]))
                        {
                            objUnlockSearchResults.MemberDOB = Convert.ToDateTime(dr["MemberDOB"]);
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberPBP"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberPBP"]))
                        {
                            objUnlockSearchResults.MemberPBP = dr["MemberPBP"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberLOB"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLOB"]))
                        {
                            objUnlockSearchResults.MemberLOB = dr["MemberLOB"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberGender"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberGender"]))
                        {
                            objUnlockSearchResults.MemberGender = dr["MemberGender"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("DiscrepancyType"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyType"]))
                        {
                            objUnlockSearchResults.DiscrepancyType = dr["DiscrepancyType"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentStatus"]))
                        {
                            objUnlockSearchResults.MostRecentStatus = dr["MostRecentStatus"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentStatusLkup"]))
                        {
                            objUnlockSearchResults.MostRecentStatusLkup = dr["MostRecentStatusLkup"].ToInt64();
                        }
                    }

                    if (dr.Table.Columns.Contains("MostRecentWorkQueueLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentWorkQueueLkup"]))
                        {
                            objUnlockSearchResults.MostRecentQueueLkup = dr["MostRecentWorkQueueLkup"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentQueue"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentQueue"]))
                        {
                            objUnlockSearchResults.MostRecentQueue = dr["MostRecentQueue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentAction"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentAction"]))
                        {
                            objUnlockSearchResults.MostRecentAction = dr["MostRecentAction"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("CurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["CurrentHICN"]))
                        {
                            objUnlockSearchResults.CurrentHICN = dr["CurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHICN"]))
                        {
                            objUnlockSearchResults.GPSHICN = dr["GPSHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MMRHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MMRHICN"]))
                        {
                            objUnlockSearchResults.MMRHICN = dr["MMRHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("FirstName"))
                    {
                        if (!DBNull.Value.Equals(dr["FirstName"]))
                        {
                            objUnlockSearchResults.FirstName = dr["FirstName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategory"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategory"]))
                        {
                            objUnlockSearchResults.DiscrepancyCategory = dr["DiscrepancyCategory"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"]))
                        {
                            objUnlockSearchResults.DiscrepancyCategoryLkup = (long?)dr["DiscrepancyCategoryLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyStartDate"]))
                        {
                            objUnlockSearchResults.DiscrepancyStartDate = Convert.ToDateTime(dr["DiscrepancyStartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyEndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyEndDate"]))
                        {
                            objUnlockSearchResults.DiscrepancyEndDate = Convert.ToDateTime(dr["DiscrepancyEndDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("Reason"))
                    {
                        if (!DBNull.Value.Equals(dr["Reason"]))
                        {
                            objUnlockSearchResults.Reason = dr["Reason"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("Resolution"))
                    {
                        if (!DBNull.Value.Equals(dr["Resolution"]))
                        {
                            objUnlockSearchResults.Resolution = dr["Resolution"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ReferencedEligibilityCaseIndicator"))
                    {
                        if (!DBNull.Value.Equals(dr["ReferencedEligibilityCaseIndicator"]))
                        {
                            objUnlockSearchResults.ReferencedEligibilityCaseIndicator = dr["ReferencedEligibilityCaseIndicator"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MMRPBP"))
                    {
                        if (!DBNull.Value.Equals(dr["MMRPBP"]))
                        {
                            objUnlockSearchResults.MMRPBP = dr["MMRPBP"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSIndividualID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSIndividualID"]))
                        {
                            objUnlockSearchResults.GPSIndividualID = dr["GPSIndividualID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancySource"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancySource"]))
                        {
                            objUnlockSearchResults.DiscrepancySource = dr["DiscrepancySource"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("NTID"))
                    {
                        if (!DBNull.Value.Equals(dr["NTID"]))
                        {
                            objUnlockSearchResults.DiscrepancySource = dr["NTID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("SubmissionType"))
                    {
                        if (!DBNull.Value.Equals(dr["SubmissionType"]))
                        {
                            objUnlockSearchResults.SubmissionType = dr["SubmissionType"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRCTMMember"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRCTMMember"]))
                        {
                            objUnlockSearchResults.RPRCTMMember = dr["RPRCTMMember"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPREGHPMember"))
                    {
                        if (!DBNull.Value.Equals(dr["RPREGHPMember"]))
                        {
                            objUnlockSearchResults.RPREGHPMember = dr["RPREGHPMember"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RequestedEffectiveDate"))
                    {
                        if (!DBNull.Value.Equals(dr["RequestedEffectiveDate"]))
                        {
                            objUnlockSearchResults.RPRRequestedEffectiveDate = Convert.ToDateTime(dr["RequestedEffectiveDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRActionRequested"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRActionRequested"]))
                        {
                            objUnlockSearchResults.RPRActionRequested = dr["RPRActionRequested"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("PotentialSubmissionDate"))
                    {
                        if (!DBNull.Value.Equals(dr["PotentialSubmissionDate"]))
                        {
                            objUnlockSearchResults.PotentialSubmissionDate = Convert.ToDateTime(dr["PotentialSubmissionDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("RPCSubmissionDate"))
                    {
                        if (!DBNull.Value.Equals(dr["RPCSubmissionDate"]))
                        {
                            objUnlockSearchResults.RPCSubmissionDate = Convert.ToDateTime(dr["RPCSubmissionDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRReceivedDate"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRReceivedDate"]))
                        {
                            objUnlockSearchResults.FDRReceivedDate = Convert.ToDateTime(dr["FDRReceivedDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRCodeReceived"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRCodeReceived"]))
                        {
                            objUnlockSearchResults.FDRCodeReceived = dr["FDRCodeReceived"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("FDRStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["FDRStatus"]))
                        {
                            objUnlockSearchResults.FDRStatus = dr["FDRStatus"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRRequestor"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRRequestor"]))
                        {
                            objUnlockSearchResults.RPRRequestor = dr["RPRRequestor"].ToString();
                        }
                    }
                    ////////////////////////////////////////////////////////////////////
                    if (dr.Table.Columns.Contains("ApplicationDate"))
                    {
                        if (!DBNull.Value.Equals(dr["ApplicationDate"]))
                        {
                            objUnlockSearchResults.ApplicationDate = Convert.ToDateTime(dr["ApplicationDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("EndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EndDate"]))
                        {
                            objUnlockSearchResults.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("RPRActionRequestedLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["RPRActionRequestedLkup"]))
                        {
                            objUnlockSearchResults.RPRActionRequestedLkup = (long?)dr["RPRActionRequestedLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("ElectionType"))
                    {
                        if (!DBNull.Value.Equals(dr["ElectionType"]))
                        {
                            objUnlockSearchResults.ElectionType = dr["ElectionType"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objUnlockSearchResults.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                        {
                            objUnlockSearchResults.CreatedByRef = Convert.ToInt64(dr["CreatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedBy"]))
                        {
                            objUnlockSearchResults.CreatedBy = dr["CreatedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objUnlockSearchResults.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                        {
                            objUnlockSearchResults.LastUpdatedByRef = Convert.ToInt64(dr["LastUpdatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedBy"]))
                        {
                            objUnlockSearchResults.LastUpdatedBy = dr["LastUpdatedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLockedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                        {
                            objUnlockSearchResults.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByRef"]))
                        {
                            objUnlockSearchResults.LockedByRef = Convert.ToInt64(dr["LockedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedBy"]))
                        {
                            objUnlockSearchResults.LockedBy = dr["LockedBy"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("PendedbyRef"))
                    {
                        if (!DBNull.Value.Equals(dr["PendedbyRef"]))
                        {
                            objUnlockSearchResults.PendedByRef = dr["PendedbyRef"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("PendedBy"))
                    {
                        if (!DBNull.Value.Equals(dr["PendedBy"]))
                        {
                            objUnlockSearchResults.PendedBy = dr["PendedBy"].NullToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("OOALetterStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["OOALetterStatusLkup"]))
                        {
                            objUnlockSearchResults.OOALetterStatusLkup = dr["OOALetterStatusLkup"].ToInt64();
                        }
                    }
                    if (dr.Table.Columns.Contains("OOALetterStatus"))
                    {
                        if (!DBNull.Value.Equals(dr["OOALetterStatus"]))
                        {
                            objUnlockSearchResults.OOALetterStatus = dr["OOALetterStatus"].NullToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("CMSTransactionStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["CMSTransactionStatusLkup"]))
                        {
                            objUnlockSearchResults.CMSTransactionStatusLkup = dr["CMSTransactionStatusLkup"].ToInt64();
                        }
                    }
                    lstSearchResults.Add(objUnlockSearchResults);
                }
            }
        }

        public ExceptionTypes UnlockQueueRecord(string gen_QueueIds, long aDM_UserMasterId,long actionPerformed, string casesComments, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long rowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();

                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = gen_QueueIds;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentUserRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = aDM_UserMasterId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = actionPerformed;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CasesComments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = casesComments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_INS_UnlockRecord, parameters.ToArray(), out lErrocode, out lErrorNumber, out rowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes ReassignQueueRecord(DOGEN_ManageCases objDOGEN_ManageCases, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long rowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objDOGEN_ManageCases.GEN_QueueRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_QueueRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_ManageCases.GEN_QueueRef;
                    parameters.Add(sqlParam);
                }
                if (objDOGEN_ManageCases.ReAssignUserRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ReAssignUserRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_ManageCases.ReAssignUserRef;
                    parameters.Add(sqlParam);
                }
                if (objDOGEN_ManageCases.ActionPerformedLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ActionPerformedLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_ManageCases.ActionPerformedLkup;
                    parameters.Add(sqlParam);
                }
                if (objDOGEN_ManageCases.CurrentUserRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CurrentUserRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOGEN_ManageCases.CurrentUserRef;
                    parameters.Add(sqlParam);
                }
                if (objDOGEN_ManageCases.CasesComments != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CasesComments";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOGEN_ManageCases.CasesComments;
                    parameters.Add(sqlParam);
                }
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_INS_ReassignRecord, parameters.ToArray(), out lErrocode, out lErrorNumber, out rowsEffected, out errorMessage);
                if (executionResult == 0 && rowsEffected > 0)
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExceptionTypes BulkReassignQueueRecord(UIBulkAssign objUIBulkAssign, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long rowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objUIBulkAssign.QueueIds.Count > 0)
                {
                    StringBuilder strQueueIds = new StringBuilder();
                    foreach (var queueId in objUIBulkAssign.QueueIds)
                    {
                        if (strQueueIds.Length > 0)
                            strQueueIds.Append(",");

                        strQueueIds.Append(queueId);
                    }
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@QueueIds";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = strQueueIds.NullToString();
                    parameters.Add(sqlParam);
                }
                if (objUIBulkAssign.DOGEN_ManageCases.ReAssignUserRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ReAssignUserRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkAssign.DOGEN_ManageCases.ReAssignUserRef;
                    parameters.Add(sqlParam);
                }
                if (objUIBulkAssign.DOGEN_ManageCases.ActionPerformedLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ActionPerformedLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkAssign.DOGEN_ManageCases.ActionPerformedLkup;
                    parameters.Add(sqlParam);
                }
                if (objUIBulkAssign.DOGEN_ManageCases.CurrentUserRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CurrentUserRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkAssign.DOGEN_ManageCases.CurrentUserRef;
                    parameters.Add(sqlParam);
                }
                if (objUIBulkAssign.DOGEN_ManageCases.CasesComments != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CasesComments";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objUIBulkAssign.DOGEN_ManageCases.CasesComments;
                    parameters.Add(sqlParam);
                }
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_INS_BulkReassignRecord, parameters.ToArray(), out lErrocode, out lErrorNumber, out rowsEffected, out errorMessage);
                if (executionResult == 0 && rowsEffected > 0)
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExceptionTypes InsertServiceProcessDetails(DOServiceProcessDetails objDOServiceProcessDetails)
        {
            ExceptionTypes result = new ExceptionTypes();
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            long rowsEffected = 0;
            DataSet dsTable = new DataSet();

            try
            {

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ServiceName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOServiceProcessDetails.ServiceName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ClientMachineName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOServiceProcessDetails.ClientMachineName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ServiceRequestedBy";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOServiceProcessDetails.ServiceRequestedBy;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ServiceRequestorName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOServiceProcessDetails.ServiceRequestorName;
                parameters.Add(sqlParam);
                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_InsertServiceProcessDetails, parameters.ToArray(), out lErrocode, out lErrorNumber, out rowsEffected, out string errorMessage);
                if (executionResult == 0 && rowsEffected > 0)
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Background Process DAL
        public ExceptionTypes InsertBackgroundProcessMaster(long backgroundProcessType, long loginId, out long bgpMasterId, out string errorMessage)
        {
            bgpMasterId = 0;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BackgroundProcessTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = backgroundProcessType;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loginId;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam.ParameterName = "@BGPMasterId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INSERT_BackgroundProcessMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@BGPMasterId");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    bgpMasterId = Convert.ToInt64(sqlParam.Value);
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes InsertBackgroundProcessDetails(DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_BackgroundProcessMasterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef;
                parameters.Add(sqlParam);

                if (objDOCMN_BackgroundProcessDetails.GEN_QueueRef.HasValue && objDOCMN_BackgroundProcessDetails.GEN_QueueRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_QueueRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_BackgroundProcessDetails.GEN_QueueRef;
                    parameters.Add(sqlParam);
                }

                if (objDOCMN_BackgroundProcessDetails.GEN_BulkImportRef.HasValue && objDOCMN_BackgroundProcessDetails.GEN_BulkImportRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_BulkImportRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_BackgroundProcessDetails.GEN_BulkImportRef;
                    parameters.Add(sqlParam);
                }

                if (!objDOCMN_BackgroundProcessDetails.GEN_FDRUploadStagingRef.IsNullOrEmpty() && objDOCMN_BackgroundProcessDetails.GEN_FDRUploadStagingRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_FDRUploadStagingRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_BackgroundProcessDetails.GEN_FDRUploadStagingRef;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UploadFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_BackgroundProcessDetails.UploadFileName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BGPRecordStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FailureReason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_BackgroundProcessDetails.FailureReason;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_BGP_INSERT_BackgroundProcessDetails, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes UpdateBackgroundProcessMaster(DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_BackgroundProcessMasterId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessMaster.LastUpdatedByRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TotalRecordProcessed";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessMaster.TotalRecordProcessed;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BGPStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_BackgroundProcessMaster.BGPStatusLkup;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_UPDATE_BackgroundProcessMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetBackgroundServerDetails(string currentServerName, out UIUserLogin currentServerDetail, out string errorMessage)
        {
            long lErrCode = 0;
            long lErrNumber = 0;
            currentServerDetail = new UIUserLogin();
            errorMessage = string.Empty;

            try
            {

                List<SqlParameter> lstSqlParameter = new List<SqlParameter>();
                SqlParameter objSqlParameter;

                objSqlParameter = new SqlParameter();
                objSqlParameter.ParameterName = "@SystemName";
                objSqlParameter.SqlDbType = SqlDbType.VarChar;
                objSqlParameter.Value = currentServerName;
                lstSqlParameter.Add(objSqlParameter);

                objSqlParameter = new SqlParameter();
                objSqlParameter.ParameterName = "@ErrorMessage";
                objSqlParameter.SqlDbType = SqlDbType.VarChar;
                objSqlParameter.Size = 2000;
                objSqlParameter.Direction = ParameterDirection.Output;
                lstSqlParameter.Add(objSqlParameter);

                DAHelper objDAHelper = new DAHelper();
                DataSet dsResult = new DataSet();
                long executionResult = objDAHelper.ExecuteSelectSP(ConstantTexts.SP_USP_BGP_SEL_ServerDetails, lstSqlParameter.ToArray(), out dsResult, out lErrCode, out lErrNumber, out errorMessage);
                objSqlParameter = lstSqlParameter.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (objSqlParameter != null && objSqlParameter.Value != null)
                {
                    errorMessage += objSqlParameter.Value.ToString();
                }

                if ((executionResult != (long)ExceptionTypes.Success && executionResult != (long)ExceptionTypes.ZeroRecords) || !string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.UnknownError;
                }

                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    currentServerDetail.ADM_UserMasterId = dsResult.Tables[0].Rows[0]["ADM_UserMasterId"] != DBNull.Value ? Convert.ToInt64(dsResult.Tables[0].Rows[0]["ADM_UserMasterId"]) : 0;
                    currentServerDetail.FullName = dsResult.Tables[0].Rows[0]["FullName"] != DBNull.Value ? Convert.ToString(dsResult.Tables[0].Rows[0]["FullName"]) : string.Empty;
                    currentServerDetail.MSID = dsResult.Tables[0].Rows[0]["MSID"] != DBNull.Value ? Convert.ToString(dsResult.Tables[0].Rows[0]["MSID"]) : string.Empty;
                    currentServerDetail.Email = dsResult.Tables[0].Rows[0]["Email"] != DBNull.Value ? Convert.ToString(dsResult.Tables[0].Rows[0]["Email"]) : string.Empty;
                }
                return (ExceptionTypes)executionResult;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message + ex.StackTrace;
                return ExceptionTypes.UnknownError;
            }
        }

        #endregion

        public ExceptionTypes InsertManageCase(DOGEN_ManageCases objDOGEN_ManageCases, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_ManageCases.GEN_QueueRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_ManageCases.ActionPerformedLkup;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentUserRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_ManageCases.CurrentUserRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CreatedByRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_ManageCases.CreatedByRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_ManageCase, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }
        public ExceptionTypes ManagePWAttachments(long? TimeZone,DOGEN_Attachments objDOGEN_Attachments, long loginUserId, out List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;


                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_AttachmentsId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Attachments.GEN_AttachmentsId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Attachments.GEN_QueueRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@UploadedFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Attachments.UploadedFileName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Attachments.FileName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FilePath";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Attachments.FilePath;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loginUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_GEN_INS_UPD_ManageAttachments, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                    errorMessage += sqlParam.Value.ToString();
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapManagePWAttachments(TimeZone,dsTable, out lstDOGEN_Attachments);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                parameters = null;
            }
        }


        public ExceptionTypes UpdateCaseInfo(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long rowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.GEN_QueueId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.WorkBasketLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyCategoryLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberFirstName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberFirstName;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam.ParameterName = "@MemberMiddleName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberMiddleName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberLastName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.MemberContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberPBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.MemberPBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLOBLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.MemberLOBLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedState;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedCountyCode";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedCountyCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberDOB";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.MemberDOB;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.ActionPerformedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyReceiptDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ComplianceStartDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.ComplianceStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DisenrollmentDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DisenrollmentDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.LoginUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_Queue.RoleLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_Queue.RPRCTMMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.RPRCTMNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_Queue.Comments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;

                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_UpdateSuspectCase, parameters.ToArray(), out lErrocode, out lErrorNumber, out rowsEffected, out errorMessage);

                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public ExceptionTypes GetReferenceCase(long genQueueID, out DOGEN_Queue objDOGEN_Queue)
        {

            DataSet  _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            string errorMessage = string.Empty;
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = genQueueID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_ReferenceCase, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapGenQueue(_dsResult, out objDOGEN_Queue);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                //need log ex
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }

        public ExceptionTypes SaveBulkUpload(DOGEN_BulkImport objDOGEN_BulkImport, long loginUserID, DataTable dtFileData,out long GEN_BulkImportID, out string errorMessage)
        {
            List<SqlParameter>  _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            GEN_BulkImportID = 0;
            try
            {
                DataSet ds = new DataSet("MassUpdate");
                ds.Tables.Add(dtFileData);
                string xmlData = ds.GetXml();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@XMLData";
                sqlParam.SqlDbType = SqlDbType.NVarChar;
                sqlParam.Value = xmlData;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.WorkBasketLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.DiscrepancyCategoryLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_BulkImportExcelTemplateMasterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.GEN_BulkImportExcelTemplateMasterRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.ExcelFileName;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DuplicateFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.DuplicateFileName;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFilelPath";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.ExcelFilelPath;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ImportStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.ImportStatusLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loginUserID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_BulkImportID";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Direction = ParameterDirection.Output;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_GEN_BulkImport, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@GEN_BulkImportID");
                    if (!sqlParam.IsNull())
                    {
                        GEN_BulkImportID = (!sqlParam.Value.IsNull()) ?  sqlParam.Value.ToInt64() : 0;
                    }
                    return ExceptionTypes.Success;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _lstParameters = null;
            }
        }
        private void MapGenQueue(DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_QueueRefferencedCases = dsResult.Tables[2].AsEnumerable().Select(dr => new DOGEN_QueueRefferencedCases
                    {
                        Gen_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0,
                        DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty,
                        DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null,
                        DiscrepancySource = !(DBNull.Value.Equals(dr["DiscrepancySource"])) ? dr["DiscrepancySource"].NullToString() : string.Empty,
                        MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentQueue"])) ? dr["MostRecentQueue"].NullToString() : string.Empty,
                        MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? dr["MostRecentStatus"].NullToString() : string.Empty,
                        QueueProgressType = !(DBNull.Value.Equals(dr["QueueProgressType"])) ? dr["QueueProgressType"].NullToString() : string.Empty,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                        MostRecentStatusLkup = (!dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0,
                    }).ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ExceptionTypes GetCasesToMask(out List<DOGEN_Queue> lstGenQueues)
        {
            lstGenQueues = new List<DOGEN_Queue>();
            DAHelper dah = new DAHelper();
            //errorMessage = string.Empty;
            //strErsCaseId = string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();

            string query = "select GEN_QueueId,MemberCurrentHICN,GPSHouseholdID from GEN_Queue where GEN_QueueId >= 795421  AND  GEN_QueueId <= 884871 order by GEN_QueueId";
            //string query = "SELECT * FROM (select GEN_QueueId,MemberCurrentHICN,GPSHouseholdID,ROW_NUMBER() OVER (ORDER BY GEN_QueueId) AS RowNum from GEN_Queue where IsActive = 1 and LEN(MemberCurrentHICN) > 9 and LEN(GPSHouseHoldId) > 11 and GEN_QueueId > 450 ) AS MyDerivedTable WHERE MyDerivedTable.RowNum > 229814 order by GEN_QueueId";
            long executionResult = dah.ExecuteSQL(query, parameters.ToArray(), out DataSet dsTable, out string erorrMessage, true);
            if (executionResult == 0)
            {
                if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                {
                    MapGetCasesToMaskResults(dsTable, out lstGenQueues);
                    return ExceptionTypes.Success;
                }
                return ExceptionTypes.ZeroRecords;
            }
            else if (executionResult == 2)
            {
                return ExceptionTypes.ZeroRecords;
            }
            else
            {
                return ExceptionTypes.UnknownError;
            }
        }
        private void MapGetCasesToMaskResults(DataSet dstTable, out List<DOGEN_Queue> lstGenQueues)
        {
            lstGenQueues = new List<DOGEN_Queue>();
            if (dstTable.Tables.Count > 0)
            {
                foreach (DataRow dr in dstTable.Tables[0].Rows)
                {
                    DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
                    if (dr.Table.Columns.Contains("GEN_QueueId"))
                    {
                        if (!DBNull.Value.Equals(dr["GEN_QueueId"]))
                        {
                            objDOGEN_Queue.GEN_QueueId = (long?)dr["GEN_QueueId"];
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberCurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberCurrentHICN"]))
                        {
                            objDOGEN_Queue.MemberCurrentHICN = dr["MemberCurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHouseholdID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHouseholdID"]))
                        {
                            objDOGEN_Queue.GPSHouseholdID = dr["GPSHouseholdID"].ToString();
                        }
                    }
                    lstGenQueues.Add(objDOGEN_Queue);
                }
            }
        }

        public ExceptionTypes MaskPHIData(long gEN_QueueId, DOGEN_GPSData objDOGEN_GPSData)
        {
            DAHelper dah = new DAHelper();
            //errorMessage = string.Empty;
            //strErsCaseId = string.Empty;

            string query = "UPDATE GEN_Queue SET MemberID = '{0}',MemberFirstName = '{1}',MemberLastName = '{2}',MemberDOB = '{3}',GPSHouseholdID = '{4}' where GEN_QueueId = '{5}'";
            query = string.Format(query, objDOGEN_GPSData.MemberId, objDOGEN_GPSData.FirstName, objDOGEN_GPSData.LastName, objDOGEN_GPSData.DOB, objDOGEN_GPSData.HouseholdId, gEN_QueueId);
            long executionResult = dah.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet dsTable, out string erorrMessage, true);
            if (executionResult == 0)
            {
                return ExceptionTypes.Success;

            }
            else if (executionResult == 2)
            {
                return ExceptionTypes.ZeroRecords;
            }
            else
            {
                return ExceptionTypes.UnknownError;
            }
        }
        private void MapManagePWAttachments(long? TimeZone,DataSet dsTable, out List<DOGEN_Attachments> lstDOGEN_Attachments)
        {
            lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            try
            {
                if (dsTable != null && dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                {
                    lstDOGEN_Attachments = dsTable.Tables[0].AsEnumerable().Select(dr => new DOGEN_Attachments
                    {
                        GEN_AttachmentsId = dr["GEN_AttachmentsId"].ToInt64(),
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        UploadedFileName = (!dr["UploadedFileName"].IsNull()) ? dr["UploadedFileName"].NullToString() : string.Empty,
                        FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty,
                        FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty,
                        GEN_DMSDataRef = (!dr["GEN_DMSDataRef"].IsNull()) ? dr["GEN_DMSDataRef"].ToInt64() : (long?)null,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void MapDownloadPWAttachments(DataSet dsResults, out DOGEN_Attachments objDOGEN_Attachments)
        {
            objDOGEN_Attachments = new DOGEN_Attachments();
            try
            {
                if (dsResults != null && dsResults.Tables.Count > 0 && dsResults.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResults.Tables[0].Rows[0];
                    objDOGEN_Attachments.GEN_AttachmentsId = dr["ID"].ToInt64();
                    objDOGEN_Attachments.FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty;
                    objDOGEN_Attachments.FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ExceptionTypes IsOOAEGHPExclusion(string EmployerID)
        {
            try
            {
                string query = string.Format(ConstantTexts.Query_Select_OOAEmployer, EmployerID);
                long executionResult = _objDAHelper.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet dsTable, out string erorrMessage, true);
                if (executionResult == 0)
                {
                    if (!dsTable.IsNull() && dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public ExceptionTypes IsNationalEmployerForRestriction(string contractNumber,string PBP,string employerID)
        {
            try
            {
                string query = string.Format(ConstantTexts.Query_Select_NationalEmployerForRestriction, contractNumber, PBP,employerID);
                long executionResult = _objDAHelper.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet dsTable, out string erorrMessage, true);
                if (executionResult == 0)
                {
                    if (!dsTable.IsNull() && dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows[0]["Count"].ToInt32() > 0)
                    {                       
                        return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
                }
                else
                {
                    return ExceptionTypes.UnknownError;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private void MapMassUpdateTemplateResult(long? timeZone, DataSet dsResult, out List<DOGEN_BulkImport> lstDOGEN_BulkImport)
        {
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            try
            {
                if (dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    lstDOGEN_BulkImport = _dsResult.Tables[0].AsEnumerable().Select(dr => new DOGEN_BulkImport
                    {
                        GEN_BulkImportId = (!dr["GEN_BulkImportId"].IsNull()) ? dr["GEN_BulkImportId"].ToInt64() : 0,
                        WorkBasketLkup = (!dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0,
                        WorkBasket = (!dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        GEN_BulkImportExcelTemplateMasterRef = (!dr["GEN_BulkImportExcelTemplateMasterRef"].IsNull()) ? dr["GEN_BulkImportExcelTemplateMasterRef"].ToInt64() : 0,
                        LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0,
                        UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null,
                        ExcelFileName = (!dr["ExcelFileName"].IsNull()) ? dr["ExcelFileName"].NullToString() : string.Empty,
                        DuplicateFileName = (!dr["DuplicateFileName"].IsNull()) ? dr["DuplicateFileName"].NullToString() : string.Empty,
                        ExcelFilelPath = (!dr["ExcelFilelPath"].IsNull()) ? dr["ExcelFilelPath"].NullToString() : string.Empty,
                        TotalRecordsCount = (!dr["TotalRecordsCount"].IsNull()) ? dr["TotalRecordsCount"].ToInt32() : 0,
                        ValidRecordsCount = (!dr["ValidRecordsCount"].IsNull()) ? dr["ValidRecordsCount"].ToInt32() : 0,
                        InvalidRecordsCount = (!dr["InvalidRecordsCount"].IsNull()) ? dr["InvalidRecordsCount"].ToInt32() : 0,
                        DuplicateRecordCount = (!dr["DuplicateRecordCount"].IsNull()) ? dr["DuplicateRecordCount"].ToInt32() : 0,
                        ErrorDescription = (!dr["ErrorDescription"].IsNull()) ? dr["ErrorDescription"].NullToString() : string.Empty,
                        ExcelStatusLkup = (!dr["ExcelStatusLkup"].IsNull()) ? dr["ExcelStatusLkup"].ToInt64() : 0,
                        ExcelStatus = (!dr["ExcelStatus"].IsNull()) ? dr["ExcelStatus"].NullToString() : string.Empty,
                        ImportStatusLkup = (!dr["ImportStatusLkup"].IsNull()) ? dr["ImportStatusLkup"].ToInt64() : 0,
                        ImportStatus = (!dr["ImportStatus"].IsNull()) ? dr["ImportStatus"].NullToString() : string.Empty,
                        CMN_AppErrorLogRef = (!dr["CMN_AppErrorLogRef"].IsNull()) ? dr["CMN_AppErrorLogRef"].ToInt64() : 0,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(), timeZone) : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(), timeZone) : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}
