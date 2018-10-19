﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data;
using System.Data.SqlClient;

namespace ENRLReconSystem.DAL
{
    public class DALRPR
    {
        DAHelper _objDAHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;
        public ExceptionTypes Create(DOGEN_Queue objDOGEN_Queue, List<DOGEN_Attachments> lstDOGEN_Attachments, out string errorMessage)
        {
            try
            {
                _lstParameters = new List<SqlParameter>();

                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                SqlParameter sqlParam = new SqlParameter();
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;
                _lstParameters = MapGenQueueObjToParam(objDOGEN_Queue);



                if (objDOGEN_Queue.IsClosedAndCreateNew)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsCloneCase";
                    sqlParam.DbType = DbType.Boolean;
                    sqlParam.Value = objDOGEN_Queue.IsClosedAndCreateNew;
                    _lstParameters.Add(sqlParam);
                }

                if (lstDOGEN_Attachments.Count() > 0)
                {
                    DataTable dtAttachment;
                    string[] selectedColumns = new[] { "GEN_AttachmentsId", "GEN_QueueRef", "UploadedFileName", "FileName", "FilePath", "GEN_DMSDataRef", "IsActive" };
                    var dt = lstDOGEN_Attachments.ToDataTable();
                    dtAttachment = new DataView(dt).ToTable(false, selectedColumns);
                    DataTableReader dtAttachments = new DataTableReader(dtAttachment);
                    if (dtAttachments.HasRows)
                    {
                        
                        sqlParam.ParameterName = "@TV_Attachments";
                        sqlParam.SqlDbType = SqlDbType.Structured;
                        sqlParam.Value = dtAttachments;
                        _lstParameters.Add(sqlParam);
                    }
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsRestricted;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EmployeerGroupNumber;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_CreateSuspectCase, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                
                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@GEN_QueueId");
                if (sqlParam != null)
                {
                    objDOGEN_Queue.GEN_QueueId = sqlParam.Value != null ? objDOGEN_Queue.GEN_QueueId = sqlParam.Value.ToInt64() : 0;
                }
                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@BusinessSegmentLkup");
                if (!sqlParam.IsNull())
                {
                    objDOGEN_Queue.BusinessSegmentLkup = 0;
                    objDOGEN_Queue.BusinessSegmentLkup = (!sqlParam.Value.IsNull()) ? objDOGEN_Queue.BusinessSegmentLkup = sqlParam.Value.ToInt64() : 0;
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

        public ExceptionTypes CheckDuplicate(string strMemberCurrentHICN, long? lMemberContractIDLkup, DateTime? dtRPRRequestedEffectiveDate, long? lRPRActionRequestedLkup, out long lCaseID)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            string errorMessage = string.Empty;
            lCaseID = 0;
            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberCurrentHICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strMemberCurrentHICN;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lMemberContractIDLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestedEffectiveDateId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = 0;
                if (dtRPRRequestedEffectiveDate != null && dtRPRRequestedEffectiveDate != DateTime.MinValue)
                {
                    string strId = dtRPRRequestedEffectiveDate.Value.ToString("yyyyMMdd");
                    if (long.TryParse(strId, out long retVal) == true)
                    {
                        sqlParam.Value = retVal;
                    }
                }
                
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lRPRActionRequestedLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_RPRCheckDuplicateCase, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    if (_dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = _dsResult.Tables[0].Rows[0];
                        lCaseID = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                    }
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

        private List<SqlParameter> MapGenQueueObjToParam(DOGEN_Queue objDOGEN_Queue)
        {
            _lstParameters = new List<SqlParameter>();

            SqlParameter sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@BusinessSegmentLkup";
            //sqlParam.DbType = DbType.Int64;
            //sqlParam.Value = objDOGEN_Queue.BusinessSegmentLkup;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@WorkBasketLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.WorkBasketLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyCategoryLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyTypeLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyTypeLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@StartDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.StartDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@StartDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.StartDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EndDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EndDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EndDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EndDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@PreviousAssignedToRef";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.PreviousAssignedToRef;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MostRecentActionLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MostRecentActionLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MostRecentWorkQueueLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MostRecentWorkQueueLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MostRecentStatusLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MostRecentStatusLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SourceSystemLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.SourceSystemLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SourceSystemId";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.SourceSystemId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancySourceLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancySourceLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyReceiptDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyReceiptDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ComplianceStartDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.ComplianceStartDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ComplianceStartDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.ComplianceStartDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyStartDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DisenrollmentDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.DisenrollmentDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyStartDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyEndDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@DiscrepancyEndDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberSCCCode";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberSCCCode;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberID";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberID != null ? objDOGEN_Queue.MemberID.ToUpper() : null;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberCurrentHICN";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberCurrentHICN.ToUpper();
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@MemberCurrentMBI";
            //sqlParam.DbType = DbType.String;
            //sqlParam.Value = objDOGEN_Queue.MemberCurrentMBI;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@GPSHouseholdID";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.GPSHouseholdID;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberFirstName";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberFirstName;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberMiddleName";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberMiddleName;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberLastName";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberLastName;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberContractIDLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MemberContractIDLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberPBPLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MemberPBPLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberLOBLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MemberLOBLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberVerifiedState";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberVerifiedState;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberVerifiedCountyCode";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MemberVerifiedCountyCode;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberDOB";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.MemberDOB;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberDOBId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MemberDOBId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MemberGenderLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.MemberGenderLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSContractIDLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSContractIDLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSPBPLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSPBPLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSSCCCode";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.EligGPSSCCCode;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSCurrentHICN";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.EligGPSCurrentHICN;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@EligGPSCurrentMBI";
            //sqlParam.DbType = DbType.String;
            //sqlParam.Value = objDOGEN_Queue.EligGPSCurrentMBI;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSInsuredPlanTermDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSInsuredPlanTermDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSLOBLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSLOBLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSMemberDOB";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOB;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSMemberDOBId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOBId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligGPSGenderLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligGPSGenderLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRContractIDLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRContractIDLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPBPLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRPBPLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRSCCCode";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.EligMMRSCCCode;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRCurrentHICN";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.EligMMRCurrentHICN;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@EligMMRCurrentMBI";
            //sqlParam.DbType = DbType.String;
            //sqlParam.Value = objDOGEN_Queue.EligMMRCurrentMBI;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRPaymentMonth";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligMMRPaymentMonth;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRDOB";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.EligMMRDOB;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRDOBId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRDOBId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligMMRGenderLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.EligMMRGenderLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@EligOOAFlagLkup";
            sqlParam.DbType = DbType.Boolean;
            sqlParam.Value = objDOGEN_Queue.EligOOAFlagLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRRequestedEffectiveDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRActionRequestedLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RPRActionRequestedLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPROtherActionRequested";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.RPROtherActionRequested;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RPRSupervisorOrRequesterRef;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRReasonforRequest";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.RPRReasonforRequest;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRTaskPerformedLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RPRTaskPerformedLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPROtherTaskPerformed";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.RPROtherTaskPerformed;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRCTMMember";
            sqlParam.DbType = DbType.Boolean;
            sqlParam.Value = objDOGEN_Queue.RPRCTMMember;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRCTMNumber";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.RPRCTMNumber;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPREGHPMember";
            sqlParam.DbType = DbType.Boolean;
            sqlParam.Value = objDOGEN_Queue.RPREGHPMember;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPREmployerID";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.RPREmployerID;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPRRequested";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.SCCRPRRequested;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPRRequestedZip";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.SCCRPRRequestedZip;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPREffectiveStartDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPREffectiveStartDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPREffectiveEndDate";
            sqlParam.DbType = DbType.DateTime;
            sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDate;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@SCCRPREffectiveEndDateId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDateId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@IsParentCase";
            sqlParam.DbType = DbType.Boolean;
            sqlParam.Value = objDOGEN_Queue.IsParentCase;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@IsChildCase";
            sqlParam.DbType = DbType.Boolean;
            sqlParam.Value = objDOGEN_Queue.IsChildCase;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ParentQueueRef";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.ParentQueueRef;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Comments";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.Comments;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ReAssignUserRef";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.ReAssignUserRef;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CasesComments";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.CasesComments;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RoleLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RoleLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@PreviousActionLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.PreviousActionLkup;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@PreviousWorkQueuesLkup";
            //sqlParam.DbType = DbType.Int64;
            //sqlParam.Value = objDOGEN_Queue.PreviousWorkQueuesLkup;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@PreviousStatusLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.PreviousStatusLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CurrentActionLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.CurrentActionLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CurrentWorkQueuesLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.CurrentWorkQueuesLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CurrentStatusLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.CurrentStatusLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@LoginUserId";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.LoginUserId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CommentsSourceSystemLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.CommentsSourceSystemLkup;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@MIIMReferenceId";
            sqlParam.DbType = DbType.String;
            sqlParam.Value = objDOGEN_Queue.MIIMReferenceId;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@RPRCategoryLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_Queue.RPRCategoryLkup;
            _lstParameters.Add(sqlParam);

            return _lstParameters;
        }

        public ExceptionTypes GetGenQueueByID(long? TimeZone,long genQueueID, out DOGEN_Queue objDOGEN_Queue,out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
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

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Case, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapGenQueue(TimeZone,_dsResult, out objDOGEN_Queue);
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

        private void MapGenQueue(long? TimeZone,DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsResult.Tables[0].Rows[0];
                objDOGEN_Queue.GEN_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                objDOGEN_Queue.BusinessSegment = (!dr["BusinessSegment"].IsNull()) ? dr["BusinessSegment"].NullToString() : string.Empty;
                objDOGEN_Queue.WorkBasketLkup = (!dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0;
                objDOGEN_Queue.WorkBasket = (!dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty;
                objDOGEN_Queue.DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0;
                objDOGEN_Queue.DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty;
                objDOGEN_Queue.DiscrepancyTypeLkup = (!dr["DiscrepancyTypeLkup"].IsNull()) ? dr["DiscrepancyTypeLkup"].ToInt64() : 0;
                objDOGEN_Queue.DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty;
                objDOGEN_Queue.StartDate = (!dr["StartDate"].IsNull()) ? dr["StartDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EndDate = (!dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.PreviousWorkQueueLkup = (!dr["PreviousWorkQueueLkup"].IsNull()) ? dr["PreviousWorkQueueLkup"].ToInt64() : 0;
                objDOGEN_Queue.AssignedToRef = (!dr["AssignedToRef"].IsNull()) ? dr["AssignedToRef"].ToInt64() : 0;
                objDOGEN_Queue.AssignedTo = (!dr["AssignedTo"].IsNull()) ? dr["AssignedTo"].NullToString() : string.Empty;
                objDOGEN_Queue.UTCAssignedOn = (!dr["UTCAssignedOn"].IsNull()) ? dr["UTCAssignedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.CSTAssignedOn = (!dr["CSTAssignedOn"].IsNull()) ? dr["CSTAssignedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                objDOGEN_Queue.LockedBy = (!dr["LockedBy"].IsNull()) ? dr["LockedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.CSTLockedOn = (!dr["CSTLockedOn"].IsNull()) ? dr["CSTLockedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.MostRecentActionLkup = (!dr["MostRecentActionLkup"].IsNull()) ? dr["MostRecentActionLkup"].ToInt64() : 0;
                objDOGEN_Queue.MostRecentAction = (!dr["MostRecentAction"].IsNull()) ? dr["MostRecentAction"].NullToString() : string.Empty;
                objDOGEN_Queue.MostRecentWorkQueueLkup = (!dr["MostRecentWorkQueueLkup"].IsNull()) ? dr["MostRecentWorkQueueLkup"].ToInt64() : 0;
                objDOGEN_Queue.MostRecentWorkQueue = (!dr["MostRecentWorkQueue"].IsNull()) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty;
                objDOGEN_Queue.MostRecentStatusLkup = (!dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0;
                objDOGEN_Queue.MostRecentStatus = (!dr["MostRecentStatus"].IsNull()) ? dr["MostRecentStatus"].NullToString() : string.Empty;
                objDOGEN_Queue.SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0;
                objDOGEN_Queue.SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty;
                objDOGEN_Queue.SourceSystemId = (!dr["SourceSystemId"].IsNull()) ? dr["SourceSystemId"].NullToString() : string.Empty;
                objDOGEN_Queue.DiscrepancySourceLkup = (!dr["DiscrepancySourceLkup"].IsNull()) ? dr["DiscrepancySourceLkup"].ToInt64() : 0;
                objDOGEN_Queue.DiscrepancySource = (!dr["DiscrepancySource"].IsNull()) ? dr["DiscrepancySource"].NullToString() : string.Empty;
                objDOGEN_Queue.DiscrepancyReceiptDate = (!dr["DiscrepancyReceiptDate"].IsNull()) ? dr["DiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.ComplianceStartDate = (!dr["ComplianceStartDate"].IsNull()) ? dr["ComplianceStartDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.Aging = (!dr["Aging"].IsNull()) ? dr["Aging"].ToInt32() : 0;
                objDOGEN_Queue.DiscrepancyStartDate = (!dr["DiscrepancyStartDate"].IsNull()) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.DiscrepancyEndDate = (!dr["DiscrepancyEndDate"].IsNull()) ? dr["DiscrepancyEndDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.MemberSCCCode = (!dr["MemberSCCCode"].IsNull()) ? dr["MemberSCCCode"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberID = (!dr["MemberID"].IsNull()) ? dr["MemberID"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberCurrentHICN = (!dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty;
                //objDOGEN_Queue.MemberCurrentMBI = (!dr["MemberCurrentMBI"].IsNull()) ? dr["MemberCurrentMBI"].NullToString() : string.Empty;
                objDOGEN_Queue.GPSHouseholdID = (!dr["GPSHouseholdID"].IsNull()) ? dr["GPSHouseholdID"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberFirstName = (!dr["MemberFirstName"].IsNull()) ? dr["MemberFirstName"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberMiddleName = (!dr["MemberMiddleName"].IsNull()) ? dr["MemberMiddleName"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberLastName = (!dr["MemberLastName"].IsNull()) ? dr["MemberLastName"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberContractIDLkup = (!dr["MemberContractIDLkup"].IsNull()) ? dr["MemberContractIDLkup"].ToInt64() : 0;
                objDOGEN_Queue.MemberContractID = (!dr["MemberContractID"].IsNull()) ? dr["MemberContractID"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberPBPLkup = (!dr["MemberPBPLkup"].IsNull()) ? dr["MemberPBPLkup"].ToInt64() : 0;
                objDOGEN_Queue.MemberPBP = (!dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberLOBLkup = (!dr["MemberLOBLkup"].IsNull()) ? dr["MemberLOBLkup"].ToInt64() : 0;
                objDOGEN_Queue.MemberLOB = (!dr["MemberLOB"].IsNull()) ? dr["MemberLOB"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberVerifiedState = (!dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberVerifiedCountyCode = (!dr["MemberVerifiedCountyCode"].IsNull()) ? dr["MemberVerifiedCountyCode"].NullToString() : string.Empty;
                objDOGEN_Queue.MemberDOB = (!dr["MemberDOB"].IsNull()) ? dr["MemberDOB"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.MemberGenderLkup = (!dr["MemberGenderLkup"].IsNull()) ? dr["MemberGenderLkup"].ToInt64() : 0;
                objDOGEN_Queue.MemberGender = (!dr["MemberGender"].IsNull()) ? dr["MemberGender"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSContractIDLkup = (!dr["EligGPSContractIDLkup"].IsNull()) ? dr["EligGPSContractIDLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligGPSContractID = (!dr["EligGPSContractID"].IsNull()) ? dr["EligGPSContractID"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSPBPLkup = (!dr["EligGPSPBPLkup"].IsNull()) ? dr["EligGPSPBPLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligGPSPBP = (!dr["EligGPSPBP"].IsNull()) ? dr["EligGPSPBP"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSSCCCode = (!dr["EligGPSSCCCode"].IsNull()) ? dr["EligGPSSCCCode"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSCurrentHICN = (!dr["EligGPSCurrentHICN"].IsNull()) ? dr["EligGPSCurrentHICN"].NullToString() : string.Empty;
                //objDOGEN_Queue.EligGPSCurrentMBI = (!dr["EligGPSCurrentMBI"].IsNull()) ? dr["EligGPSCurrentMBI"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = (!dr["EligGPSInsuredPlanEffectiveDate"].IsNull()) ? dr["EligGPSInsuredPlanEffectiveDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligGPSInsuredPlanTermDate = (!dr["EligGPSInsuredPlanTermDate"].IsNull()) ? dr["EligGPSInsuredPlanTermDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligGPSLOBLkup = (!dr["EligGPSLOBLkup"].IsNull()) ? dr["EligGPSLOBLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligGPSLOB = (!dr["EligGPSLOB"].IsNull()) ? dr["EligGPSLOB"].NullToString() : string.Empty;
                objDOGEN_Queue.EligGPSMemberDOB = (!dr["EligGPSMemberDOB"].IsNull()) ? dr["EligGPSMemberDOB"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligGPSGenderLkup = (!dr["EligGPSGenderLkup"].IsNull()) ? dr["EligGPSGenderLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligGPSGender = (!dr["EligGPSGender"].IsNull()) ? dr["EligGPSGender"].NullToString() : string.Empty;
                objDOGEN_Queue.EligMMRContractIDLkup = (!dr["EligMMRContractIDLkup"].IsNull()) ? dr["EligMMRContractIDLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligMMRContractID = (!dr["EligMMRContractID"].IsNull()) ? dr["EligMMRContractID"].NullToString() : string.Empty;
                objDOGEN_Queue.EligMMRPBPLkup = (!dr["EligMMRPBPLkup"].IsNull()) ? dr["EligMMRPBPLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligMMRPBP = (!dr["EligMMRPBP"].IsNull()) ? dr["EligMMRPBP"].NullToString() : string.Empty;
                objDOGEN_Queue.EligMMRSCCCode = (!dr["EligMMRSCCCode"].IsNull()) ? dr["EligMMRSCCCode"].NullToString() : string.Empty;
                objDOGEN_Queue.DisenrollmentDate = (!dr["DisenrollmentDate"].IsNull()) ? dr["DisenrollmentDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligMMRCurrentHICN = (!dr["EligMMRCurrentHICN"].IsNull()) ? dr["EligMMRCurrentHICN"].NullToString() : string.Empty;
                //objDOGEN_Queue.EligMMRCurrentMBI = (!dr["EligMMRCurrentMBI"].IsNull()) ? dr["EligMMRCurrentMBI"].NullToString() : string.Empty;
                objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = (!dr["EligMMRPaymentAdjustmentStartDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentStartDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = (!dr["EligMMRPaymentAdjustmentEndDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentEndDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligMMRPaymentMonth = (!dr["EligMMRPaymentMonth"].IsNull()) ? dr["EligMMRPaymentMonth"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligMMRDOB = (!dr["EligMMRDOB"].IsNull()) ? dr["EligMMRDOB"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.EligMMRGenderLkup = (!dr["EligMMRGenderLkup"].IsNull()) ? dr["EligMMRGenderLkup"].ToInt64() : 0;
                objDOGEN_Queue.EligMMRGender = (!dr["EligMMRGender"].IsNull()) ? dr["EligMMRGender"].NullToString() : string.Empty;
                objDOGEN_Queue.EligOOAFlagLkup = (!dr["EligOOAFlagLkup"].IsNull()) ? dr["EligOOAFlagLkup"].ToBoolean() : false;
                //objDOGEN_Queue.EligOOAFlag = (!dr["EligOOAFlag"].IsNull()) ? dr["EligOOAFlag"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRRequestedEffectiveDate = (!dr["RPRRequestedEffectiveDate"].IsNull()) ? dr["RPRRequestedEffectiveDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.RPRActionRequestedLkup = (!dr["RPRActionRequestedLkup"].IsNull()) ? dr["RPRActionRequestedLkup"].ToInt64() : 0;
                objDOGEN_Queue.RPRActionRequested = (!dr["RPRActionRequested"].IsNull()) ? dr["RPRActionRequested"].NullToString() : string.Empty;
                objDOGEN_Queue.RPROtherActionRequested = (!dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRSupervisorOrRequesterRef = (!dr["RPRSupervisorOrRequesterRef"].IsNull()) ? dr["RPRSupervisorOrRequesterRef"].ToInt64() : 0;
                objDOGEN_Queue.RPRSupervisorOrRequester = (!dr["RPRSupervisorOrRequester"].IsNull()) ? dr["RPRSupervisorOrRequester"].NullToString() : string.Empty;
                objDOGEN_Queue.RPREmployerID = (!dr["RPREmployerID"].IsNull()) ? dr["RPREmployerID"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRReasonforRequest = (!dr["RPRReasonforRequest"].IsNull()) ? dr["RPRReasonforRequest"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRTaskPerformedLkup = (!dr["RPRTaskPerformedLkup"].IsNull()) ? dr["RPRTaskPerformedLkup"].ToInt64() : 0;
                objDOGEN_Queue.RPRTaskPerformed = (!dr["RPRTaskPerformed"].IsNull()) ? dr["RPRTaskPerformed"].NullToString() : string.Empty;
                objDOGEN_Queue.RPROtherTaskPerformed = (!dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRCTMMember = (!dr["RPRCTMMember"].IsNull()) ? dr["RPRCTMMember"].ToBoolean() : false;
                objDOGEN_Queue.RPRCTMNumber = (!dr["RPRCTMNumber"].IsNull()) ? dr["RPRCTMNumber"].NullToString() : string.Empty;
                objDOGEN_Queue.RPREGHPMember = (!dr["RPREGHPMember"].IsNull()) ? dr["RPREGHPMember"].ToBoolean() : false;
                objDOGEN_Queue.SCCRPRRequested = (!dr["SCCRPRRequested"].IsNull()) ? dr["SCCRPRRequested"].NullToString() : string.Empty;
                objDOGEN_Queue.SCCRPRRequestedZip = (!dr["SCCRPRRequestedZip"].IsNull()) ? dr["SCCRPRRequestedZip"].NullToString() : string.Empty;
                objDOGEN_Queue.SCCRPRRequstedSubmissionDate = (!dr["SCCRPRRequstedSubmissionDate"].IsNull()) ? dr["SCCRPRRequstedSubmissionDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.SCCRPREffectiveStartDate = (!dr["SCCRPREffectiveStartDate"].IsNull()) ? dr["SCCRPREffectiveStartDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.SCCRPREffectiveEndDate = (!dr["SCCRPREffectiveEndDate"].IsNull()) ? dr["SCCRPREffectiveEndDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.IsCasePended = (!dr["IsCasePended"].IsNull()) ? dr["IsCasePended"].ToBoolean() : false;
                objDOGEN_Queue.PendedbyRef = (!dr["PendedbyRef"].IsNull()) ? dr["PendedbyRef"].ToInt64() : 0;
                objDOGEN_Queue.Pendedby = (!dr["Pendedby"].IsNull()) ? dr["Pendedby"].NullToString() : string.Empty;
                objDOGEN_Queue.UTCPendedOn = (!dr["UTCPendedOn"].IsNull()) ? dr["UTCPendedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.CSTPendedOn = (!dr["CSTPendedOn"].IsNull()) ? dr["CSTPendedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.IsCaseResolved = (!dr["IsCaseResolved"].IsNull()) ? dr["IsCaseResolved"].ToBoolean() : false;
                objDOGEN_Queue.ResolvedByRef = (!dr["ResolvedByRef"].IsNull()) ? dr["ResolvedByRef"].ToInt64() : 0;
                objDOGEN_Queue.ResolvedBy = (!dr["ResolvedBy"].IsNull()) ? dr["ResolvedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.UTCResolvedOn = (!dr["UTCResolvedOn"].IsNull()) ? dr["UTCResolvedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.CSTResolvedOn = (!dr["CSTResolvedOn"].IsNull()) ? dr["CSTResolvedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.IsParentCase = (!dr["IsParentCase"].IsNull()) ? dr["IsParentCase"].ToBoolean() : false;
                objDOGEN_Queue.IsChildCase = (!dr["IsChildCase"].IsNull()) ? dr["IsChildCase"].ToBoolean() : false;
                objDOGEN_Queue.ParentQueueRef = (!dr["ParentQueueRef"].IsNull()) ? dr["ParentQueueRef"].ToInt64() : 0;
                objDOGEN_Queue.IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                objDOGEN_Queue.UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                objDOGEN_Queue.CSTCreatedOn = (!dr["CSTCreatedOn"].IsNull()) ? dr["CSTCreatedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                objDOGEN_Queue.CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                objDOGEN_Queue.CSTLastUpdatedOn = (!dr["CSTLastUpdatedOn"].IsNull()) ? dr["CSTLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                objDOGEN_Queue.LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.RPROtherActionRequested = (!dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                objDOGEN_Queue.RPROtherTaskPerformed = (!dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                objDOGEN_Queue.PreviousAssignedToRef = (!dr["PreviousAssignedToRef"].IsNull()) ? dr["PreviousAssignedToRef"].ToInt64() : 0;
                objDOGEN_Queue.PreviousAssignedTo = (!dr["PreviousAssignedTo"].IsNull()) ? dr["PreviousAssignedTo"].NullToString() : string.Empty;
                objDOGEN_Queue.PDPAutoEnrolleeInd = (!dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : 0;
                objDOGEN_Queue.ReferencedEligibilityCaseInd = (!dr["ReferencedEligibilityCaseInd"].IsNull()) ? dr["ReferencedEligibilityCaseInd"].ToBoolean() : false;
                objDOGEN_Queue.ReferencedSCCCaseInd = (!dr["ReferencedSCCCaseInd"].IsNull()) ? dr["ReferencedSCCCaseInd"].ToBoolean() : false;
                objDOGEN_Queue.FileTypeLkup = (!dr["FileTypeLkup"].IsNull()) ? dr["FileTypeLkup"].ToInt64() : 0;
                objDOGEN_Queue.FileType = (!dr["FileType"].IsNull()) ? dr["FileType"].NullToString() : string.Empty;
                objDOGEN_Queue.ODMDocID = (!dr["ODMDocID"].IsNull()) ? dr["ODMDocID"].NullToString() : string.Empty;
                objDOGEN_Queue.ODMAddressLink = (!dr["ODMAddressLink"].IsNull()) ? dr["ODMAddressLink"].NullToString() : string.Empty;
                objDOGEN_Queue.UndeliveredAddress1 = (!dr["UndeliveredAddress1"].IsNull()) ? dr["UndeliveredAddress1"].NullToString() : string.Empty;
                objDOGEN_Queue.UndeliveredAddress2 = (!dr["UndeliveredAddress2"].IsNull()) ? dr["UndeliveredAddress2"].NullToString() : string.Empty;
                objDOGEN_Queue.UndeliveredCity = (!dr["UndeliveredCity"].IsNull()) ? dr["UndeliveredCity"].NullToString() : string.Empty;
                objDOGEN_Queue.UndeliveredState = (!dr["UndeliveredState"].IsNull()) ? dr["UndeliveredState"].NullToString() : string.Empty;
                objDOGEN_Queue.UndeliveredZip = (!dr["UndeliveredZip"].IsNull()) ? dr["UndeliveredZip"].NullToString() : string.Empty;
                objDOGEN_Queue.COAOldAddress1 = (!dr["COAOldAddress1"].IsNull()) ? dr["COAOldAddress1"].NullToString() : string.Empty;
                objDOGEN_Queue.COAOldAddress2 = (!dr["COAOldAddress2"].IsNull()) ? dr["COAOldAddress2"].NullToString() : string.Empty;
                objDOGEN_Queue.COAOldCity = (!dr["COAOldCity"].IsNull()) ? dr["COAOldCity"].NullToString() : string.Empty;
                objDOGEN_Queue.COAOldState = (!dr["COAOldState"].IsNull()) ? dr["COAOldState"].NullToString() : string.Empty;
                objDOGEN_Queue.COAOldZip = (!dr["COAOldZip"].IsNull()) ? dr["COAOldZip"].NullToString() : string.Empty;
                objDOGEN_Queue.COANewAddress1 = (!dr["COANewAddress1"].IsNull()) ? dr["COANewAddress1"].NullToString() : string.Empty;
                objDOGEN_Queue.COANewAddress2 = (!dr["COANewAddress2"].IsNull()) ? dr["COANewAddress2"].NullToString() : string.Empty;
                objDOGEN_Queue.COANewCity = (!dr["COANewCity"].IsNull()) ? dr["COANewCity"].NullToString() : string.Empty;
                objDOGEN_Queue.COANewState = (!dr["COANewState"].IsNull()) ? dr["COANewState"].NullToString() : string.Empty;
                objDOGEN_Queue.COANewZip = (!dr["COANewZip"].IsNull()) ? dr["COANewZip"].NullToString() : string.Empty;
                objDOGEN_Queue.RPRCategoryLkup = (!dr["RPRCategoryLkup"].IsNull()) ? dr["RPRCategoryLkup"].ToInt64() : (Int64?)null;
                objDOGEN_Queue.RPRCategory = (!dr["RPRCategory"].IsNull()) ? dr["RPRCategory"].NullToString() : string.Empty;
                objDOGEN_Queue.TurnAroundTime = (dr.Table.Columns.Contains("TurnAroundTime") && !dr["TurnAroundTime"].IsNull()) ? dr["TurnAroundTime"].ToInt64() : 0;
            }
            if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
            {
                DataRow dr = dsResult.Tables[1].Rows[0];
                objDOGEN_Queue.objDOGEN_RPRActions.GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.ResolutionLkup = (!dr["ResolutionLkup"].IsNull()) ? dr["ResolutionLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.Resolution = (!dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.RootCauseLkup = (!dr["RootCauseLkup"].IsNull()) ? dr["RootCauseLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.RootCause = (!dr["RootCause"].IsNull()) ? dr["RootCause"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.PendReasonLkup = (!dr["PendReasonLkup"].IsNull()) ? dr["PendReasonLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.PendReason = (!dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDate = (!dr["AdjustedCreateDate"].IsNull()) ? dr["AdjustedCreateDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReasonLkup = (!dr["AdjustedCreateDateReasonLkup"].IsNull()) ? dr["AdjustedCreateDateReasonLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.AdjustedCreateDateReason = (!dr["AdjustedCreateDateReason"].IsNull()) ? dr["AdjustedCreateDateReason"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.OtherAdjustedCreateDateReason = (!dr["OtherAdjustedCreateDateReason"].IsNull()) ? dr["OtherAdjustedCreateDateReason"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.SubmissionTypeLkup = (!dr["SubmissionTypeLkup"].IsNull()) ? dr["SubmissionTypeLkup"].ToInt64() : (Int64?)null;
                objDOGEN_Queue.objDOGEN_RPRActions.SubmissionType = (!dr["SubmissionType"].IsNull()) ? dr["SubmissionType"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.ContainsErrorsLkup = (!dr["ContainsErrorsLkup"].IsNull()) ? dr["ContainsErrorsLkup"].ToInt64() : (Int64?)null;
                //objDOGEN_Queue.objDOGEN_RPRActions.ContainsErrors = (!dr["ContainsErrors"].IsNull()) ? dr["ContainsErrors"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.RPCSubmissionDate = (!dr["RPCSubmissionDate"].IsNull()) ? dr["RPCSubmissionDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.CMSAccountManagerSubmissionDate = (!dr["CMSAccountManagerSubmissionDate"].IsNull()) ? dr["CMSAccountManagerSubmissionDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.CMSAccountManagerApprovalDate = (!dr["CMSAccountManagerApprovalDate"].IsNull()) ? dr["CMSAccountManagerApprovalDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.ElectionTypeLkup = (!dr["ElectionTypeLkup"].IsNull()) ? dr["ElectionTypeLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.ElectionType = (!dr["ElectionType"].IsNull()) ? dr["ElectionType"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRStatusLkup = (!dr["FDRStatusLkup"].IsNull()) ? dr["FDRStatusLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRStatus = (!dr["FDRStatus"].IsNull()) ? dr["FDRStatus"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.OtherFDRStatus = (!dr["OtherFDRStatus"].IsNull()) ? dr["OtherFDRStatus"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRReceivedDate = (!dr["FDRReceivedDate"].IsNull()) ? dr["FDRReceivedDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRCodeReceived = (!dr["FDRCodeReceived"].IsNull()) ? dr["FDRCodeReceived"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRDescription = (!dr["FDRDescription"].IsNull()) ? dr["FDRDescription"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.CMSProcessDate = (!dr["CMSProcessDate"].IsNull()) ? dr["CMSProcessDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.TransactionType = (!dr["TransactionType"].IsNull()) ? dr["TransactionType"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.TransactionTypeCode = (!dr["TransactionTypeCode"].IsNull()) ? dr["TransactionTypeCode"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.VerifiedRootCause = (!dr["VerifiedRootCause"].IsNull()) ? dr["VerifiedRootCause"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.ExplanationOfRootCause = (!dr["ExplanationOfRootCause"].IsNull()) ? dr["ExplanationOfRootCause"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.TransactionTypeCodeLkup = (!dr["TransactionTypeCodeLkup"].IsNull()) ? dr["TransactionTypeCodeLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.VerifiedRootCauseLkup = (!dr["VerifiedRootCauseLkup"].IsNull()) ? dr["VerifiedRootCauseLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.ExplanationOfRootCauseLkup = (!dr["ExplanationOfRootCauseLkup"].IsNull()) ? dr["ExplanationOfRootCauseLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionTypeLkup = (!dr["FDRRejectionTypeLkup"].IsNull()) ? dr["FDRRejectionTypeLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.FDRRejectionType = (!dr["FDRRejectionType"].IsNull()) ? dr["FDRRejectionType"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.LastName = (!dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.DateofBirth = (!dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.ContractIDLkup = (!dr["ContractIDLkup"].IsNull()) ? dr["ContractIDLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.ContractID = (!dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.PBPLkup = (!dr["PBPLkup"].IsNull()) ? dr["PBPLkup"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.PBP = (!dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.ApplicationDate = (!dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.EffectiveDate = (!dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.EndDate = (!dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.ActualSubmissionDate = (!dr["ActualSubmissionDate"].IsNull()) ? dr["ActualSubmissionDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.ReasonSubmissionRejected = (!dr["ReasonSubmissionRejected"].IsNull()) ? dr["ReasonSubmissionRejected"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.RequestedSCC = (!dr["RequestedSCC"].IsNull()) ? dr["RequestedSCC"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.RequestedZIP = (!dr["RequestedZIP"].IsNull()) ? dr["RequestedZIP"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.ResubmissionDate = (!dr["ResubmissionDate"].IsNull()) ? dr["ResubmissionDate"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                objDOGEN_Queue.objDOGEN_RPRActions.UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null ;
                objDOGEN_Queue.objDOGEN_RPRActions.PotentialSubmissionDate = (!dr["PotentialSubmissionDate"].IsNull()) ? dr["PotentialSubmissionDate"].ToDateTime() : (DateTime?)null;
                objDOGEN_Queue.objDOGEN_RPRActions.LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                objDOGEN_Queue.objDOGEN_RPRActions.LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                objDOGEN_Queue.objDOGEN_RPRActions.PlanError = (!dr["PlanError"].IsNull()) ? dr["PlanError"].ToInt32() : (int?)null;
                objDOGEN_Queue.objDOGEN_RPRActions.IncludeInTodaysSubmission = dr["IncludeInTodaysSubmission"].ToBoolean();
            }
            if (dsResult != null && dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
            {
                objDOGEN_Queue.lstDOGEN_QueueRefferencedCases = dsResult.Tables[2].AsEnumerable().Select(dr => new DOGEN_QueueRefferencedCases
                {
                    Gen_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0,
                    DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                    DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                    DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty,
                    MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentWorkQueue"])) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty,
                    MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? dr["MostRecentStatus"].NullToString() : string.Empty,
                    DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null,
                    MemberContract = (!dr["MemberContract"].IsNull()) ? dr["MemberContract"].NullToString() : string.Empty,
                    MemberPBP = (!dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty,
                    MemberCurrentHICN = (!dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty,
                    FirstLetterMailDate = !(DBNull.Value.Equals(dr["FirstLetterMailDate"])) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null,
                    SecondLetterMailDate = !(DBNull.Value.Equals(dr["SecondLetterMailDate"])) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null,
                    ParentQueueRef = dr["ParentQueueRef"].ToInt64(),
                    UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(), TimeZone) : (DateTime?)null,
                    CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                    UTCLastUpdatedOn = !(DBNull.Value.Equals(dr["UTCLastUpdatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(), TimeZone) : (DateTime?)null,
                    LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty
                }).ToList();
            }
            if (dsResult != null && dsResult.Tables.Count > 3 && dsResult.Tables[3].Rows.Count > 0)
            {
                objDOGEN_Queue.lstDOGEN_Comments = dsResult.Tables[3].AsEnumerable().Select(dr => new DOGEN_Comments
                {
                    GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                    Comments = (!dr["Comments"].IsNull()) ? dr["Comments"].NullToString() : string.Empty,
                    ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0,
                    Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty,
                    IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                    UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null ,
                    CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                    CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                    UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null ,
                    LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                    LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0,
                    SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty,
                }).ToList();
            }
            if (dsResult != null && dsResult.Tables.Count > 4 && dsResult.Tables[4].Rows.Count > 0)
            {
                objDOGEN_Queue.lstDOGEN_Attachments = dsResult.Tables[4].AsEnumerable().Select(dr => new DOGEN_Attachments
                {
                    slno = dr["SLNO"].ToInt64(),
                    GEN_AttachmentsId = dr["GEN_AttachmentsId"].ToInt64(),
                    GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                    UploadedFileName = (!dr["UploadedFileName"].IsNull()) ? dr["UploadedFileName"].NullToString() : string.Empty,
                    FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty,
                    FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty,
                    GEN_DMSDataRef = (!dr["GEN_DMSDataRef"].IsNull()) ? dr["GEN_DMSDataRef"].ToInt64() : 0,
                    IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                    UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                    CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                    UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                    LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                    CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                    LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                }).ToList();
            }
            if (dsResult != null && dsResult.Tables.Count > 6 && dsResult.Tables[6].Rows.Count > 0)
            {
                objDOGEN_Queue.lstDOGEN_QueueWorkFlowCorrelation = dsResult.Tables[6].AsEnumerable().Select(dr => new DOGEN_QueueWorkFlowCorrelation
                {
                    GEN_QueueRef = !(DBNull.Value.Equals(dr["GEN_QueueRef"])) ? Convert.ToInt64(dr["GEN_QueueRef"]) : 0,
                    RoleLkup = !(DBNull.Value.Equals(dr["RoleLkup"])) ? Convert.ToInt64(dr["RoleLkup"]) : 0,
                    Role = !(DBNull.Value.Equals(dr["Role"])) ? Convert.ToString(dr["Role"]) : string.Empty,
                    WorkBasketLkup = !(DBNull.Value.Equals(dr["WorkBasketLkup"])) ? Convert.ToInt64(dr["WorkBasketLkup"]) : 0,
                    WorkBasket = !(DBNull.Value.Equals(dr["WorkBasket"])) ? Convert.ToString(dr["WorkBasket"]) : string.Empty,
                    DiscripancyCategoryLkup = !(DBNull.Value.Equals(dr["DiscripancyCategoryLkup"])) ? Convert.ToInt64(dr["DiscripancyCategoryLkup"]) : 0,
                    DiscripancyCategory = !(DBNull.Value.Equals(dr["DiscripancyCategory"])) ? Convert.ToString(dr["DiscripancyCategory"]) : "NA",
                    PreviousActionLkup = !(DBNull.Value.Equals(dr["PreviousActionLkup"])) ? Convert.ToInt64(dr["PreviousActionLkup"]) : 0,
                    PreviousAction = !(DBNull.Value.Equals(dr["PreviousAction"])) ? Convert.ToString(dr["PreviousAction"]) : "NA",
                    PreviousWorkQueuesLkup = !(DBNull.Value.Equals(dr["PreviousWorkQueuesLkup"])) ? Convert.ToInt64(dr["PreviousWorkQueuesLkup"]) : 0,
                    PreviousWorkQueues = !(DBNull.Value.Equals(dr["PreviousWorkQueues"])) ? Convert.ToString(dr["PreviousWorkQueues"]) : "NA",
                    PreviousStatusLkup = !(DBNull.Value.Equals(dr["PreviousStatusLkup"])) ? Convert.ToInt64(dr["PreviousStatusLkup"]) : 0,
                    PreviousStatus = !(DBNull.Value.Equals(dr["PreviousStatus"])) ? Convert.ToString(dr["PreviousStatus"]) : "NA",
                    CurrentActionLkup = !(DBNull.Value.Equals(dr["CurrentActionLkup"])) ? Convert.ToInt64(dr["CurrentActionLkup"]) : 0,
                    CurrentAction = !(DBNull.Value.Equals(dr["CurrentAction"])) ? Convert.ToString(dr["CurrentAction"]) : "NA",
                    CurrentWorkQueuesLkup = !(DBNull.Value.Equals(dr["CurrentWorkQueuesLkup"])) ? Convert.ToInt64(dr["CurrentWorkQueuesLkup"]) : 0,
                    CurrentWorkQueues = !(DBNull.Value.Equals(dr["CurrentWorkQueues"])) ? Convert.ToString(dr["CurrentWorkQueues"]) : "NA",
                    CurrentStatusLkup = !(DBNull.Value.Equals(dr["CurrentStatusLkup"])) ? Convert.ToInt64(dr["CurrentStatusLkup"]) : 0,
                    CurrentStatus = !(DBNull.Value.Equals(dr["CurrentStatus"])) ? Convert.ToString(dr["CurrentStatus"]) : "NA",
                    IsActive = !(DBNull.Value.Equals(dr["IsActive"])) ? Convert.ToBoolean(dr["IsActive"]) : false,
                    UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                    CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                    CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : "NA"

                }).ToList();
            }
        }

        public ExceptionTypes SaveRPRActions(DOGEN_RPRActions objDOGEN_RPRActions, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                parameters = MapRPRActionsDOToSqlParam(objDOGEN_RPRActions);

                //Extra parameter when adding or editing record for releasing lock

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.USP_APP_UPD_RPRActions, parameters.ToArray(), out lErrorNumber, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
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

        //function to map object properties to SQL parameters
        private List<SqlParameter> MapRPRActionsDOToSqlParam(DOGEN_RPRActions objDOGEN_RPRActions)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@GEN_QueueRef";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOGEN_RPRActions.GEN_QueueRef;
            parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ActionLkup";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOGEN_RPRActions.ActionLkup;
            parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ReOpenoptionLkup";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOGEN_RPRActions.OptionLkup;
            parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ReOpenQueueLkup";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOGEN_RPRActions.ReopenQueueLKUP;
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
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CommentsSourceSystemLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_RPRActions.CommentsSourceSystemLkup;
            parameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@CMSTransactionStatusLkup";
            sqlParam.DbType = DbType.Int64;
            sqlParam.Value = objDOGEN_RPRActions.CMSTransactionStatusLkup;
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


            return parameters;
        }
    }
}
