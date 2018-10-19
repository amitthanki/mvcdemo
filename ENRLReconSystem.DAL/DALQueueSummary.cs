using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DAL
{
    public class DALQueueSummary
    {
        public ExceptionTypes GetQueueSummary(DateTime dtpStartDate, DateTime dtpEndDate, long lBusinessSegmentLkup, long? lDiscrepancyCategory, out QueueSummary objQueueSummary, out string strErrorMessage)
        {
            DAHelper dah = new DAHelper();
            long lErrorCode = 0;
            long lErrorNumber = 0;
            DataSet dsQueueSummary = new DataSet();
            strErrorMessage = string.Empty;
            objQueueSummary = new QueueSummary();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = dtpStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = dtpEndDate;
                parameters.Add(sqlParam);

                if (lBusinessSegmentLkup != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lBusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }

                if (lDiscrepancyCategory != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lDiscrepancyCategory;
                    parameters.Add(sqlParam);
                }
                
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strErrorMessage;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueSummary, parameters.ToArray(), out dsQueueSummary, out lErrorCode, out lErrorNumber, out strErrorMessage);

                if (executionResult == 0)
                {
                    if (dsQueueSummary.Tables.Count > 0)
                    {
                        MapToQueueSummaryObject(lDiscrepancyCategory, dsQueueSummary, out objQueueSummary);
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
                //log error
                strErrorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetMostRecentItems(long? TimeZone,long aDM_UserMasterId,long workBasketLkup, long businessSegment,out List<MostRecentItem> lstMostRecentItems, out string errorMessage)
        {
            List<SqlParameter> _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrorNumber = 0;
            long lErrorCode = 0;
            errorMessage = string.Empty;
            DataSet dsRecentItem = new DataSet();
            DAHelper _daHelper = new DAHelper();
            lstMostRecentItems = new List<MostRecentItem>();

            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = aDM_UserMasterId;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = workBasketLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = businessSegment;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_MostRecentCases, _lstParameters.ToArray(), out dsRecentItem, out lErrorCode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapMostRecentItems(TimeZone,dsRecentItem, out lstMostRecentItems);
                    return ExceptionTypes.Success;
                }
                else if(executionResult== ExceptionTypes.ZeroRecords.ToInt64()) {
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

        public ExceptionTypes GetGMURecord(DateTime dtpStartDate, DateTime dtpEndDate, long lBusinessSegmentLkup, long lQueueLkup, long? lQueueIdToSkip, long lLoginUserId, bool isRestrictedUser, out DOGEN_Queue objDOGEN_Queue, out string strErrorMessage)
        {
            DAHelper dah = new DAHelper();
            long lErrorCode = 0;
            long lErrorNumber = 0;
            DataSet dsQueue = new DataSet();
            strErrorMessage = string.Empty;
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = dtpStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = dtpEndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@QueueLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lQueueLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueIdToSkip";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lQueueIdToSkip;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lBusinessSegmentLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strErrorMessage;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lLoginUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = isRestrictedUser;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_GMU_QueueDetails, parameters.ToArray(), out dsQueue, out lErrorCode, out lErrorNumber, out strErrorMessage);

                if (executionResult == 0)
                {
                    if (dsQueue.Tables.Count > 0)
                    {
                        MapToGenQueueObject(dsQueue, out objDOGEN_Queue);
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
                //log error
                strErrorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetPendedRecords(long lPendedByRef, long lBusinessSegmentLkup, long lWorkBasketLkup, long? lDiscrepancyCategoryLkup, out List<DOGEN_Queue> lstDOGEN_Queue, out string strErrorMessage)
        {
            DAHelper dah = new DAHelper();
            long lErrorCode = 0;
            long lErrorNumber = 0;
            DataSet dsPendedRecords = new DataSet();
            lstDOGEN_Queue = new List<DOGEN_Queue>();
            strErrorMessage = string.Empty;
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;
                
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendedByRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lPendedByRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lBusinessSegmentLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lWorkBasketLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lDiscrepancyCategoryLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strErrorMessage;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_PendedRecords, parameters.ToArray(), out dsPendedRecords, out lErrorCode, out lErrorNumber, out strErrorMessage);

                if (executionResult == 0 || executionResult == 2)
                {
                    if (dsPendedRecords.Tables.Count > 0 && dsPendedRecords.Tables[0].Rows.Count > 0)
                    {
                        MapToListOfGenQueueObject(dsPendedRecords, out lstDOGEN_Queue);
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
                //log error
                strErrorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        private void MapToQueueSummaryObject(long? lDiscrepancyCategory, DataSet dsQueueSummary, out QueueSummary objQueueSummary)
        {
            objQueueSummary = new QueueSummary();
            if (dsQueueSummary.Tables.Count > 0)
            {
                if (lDiscrepancyCategory == 6001 || lDiscrepancyCategory == null)
                {
                    OOAQueueSummary objOOAQueueSummary;
                    DataTable dtOOAQueueSummary = lDiscrepancyCategory == 6001 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[0];
                    MapOOAQueueSummary(dtOOAQueueSummary, out objOOAQueueSummary);
                    objQueueSummary.objOOAQueueSummary = objOOAQueueSummary;
                }
                if (lDiscrepancyCategory == 6002 || lDiscrepancyCategory == null)
                {
                    SCCQueueSummary objSCCQueueSummary;
                    DataTable dtSCCQueueSummary = lDiscrepancyCategory == 6002 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[1];
                    MapSCCQueueSummary(dtSCCQueueSummary, out objSCCQueueSummary);
                    objQueueSummary.objSCCQueueSummary = objSCCQueueSummary;
                }
                if (lDiscrepancyCategory == 6003 || lDiscrepancyCategory == null)
                {
                    TRRQueueSummary objTRRQueueSummary;
                    DataTable dtTRRQueueSummary = lDiscrepancyCategory == 6003 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[2];
                    MapTRRQueueSummary(dtTRRQueueSummary, out objTRRQueueSummary);
                    objQueueSummary.objTRRQueueSummary = objTRRQueueSummary;
                }
                if (lDiscrepancyCategory == 6004 || lDiscrepancyCategory == null)
                {
                    EligibilityQueueSummary objEligibilityQueueSummary;
                    DataTable dtEligibilityQueueSummary = lDiscrepancyCategory == 6004 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[3];
                    MapEligibilityQueueSummary(dtEligibilityQueueSummary, out objEligibilityQueueSummary);
                    objQueueSummary.objEligibilityQueueSummary = objEligibilityQueueSummary;
                }
                if (lDiscrepancyCategory == 6005 || lDiscrepancyCategory == null)
                {
                    DOBQueueSummary objDOBQueueSummary;
                    DataTable dtDOBQueueSummary = lDiscrepancyCategory == 6005 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[4];
                    MapDOBQueueSummary(dtDOBQueueSummary, out objDOBQueueSummary);
                    objQueueSummary.objDOBQueueSummary = objDOBQueueSummary;
                }
                if (lDiscrepancyCategory == 6006 || lDiscrepancyCategory == null)
                {
                    GenderQueueSummary objGenderQueueSummary;
                    DataTable dtGenderQueueSummary = lDiscrepancyCategory == 6006 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[5];
                    MapGenderQueueSummary(dtGenderQueueSummary, out objGenderQueueSummary);
                    objQueueSummary.objGenderQueueSummary = objGenderQueueSummary;
                }
                if (lDiscrepancyCategory == 6007 || lDiscrepancyCategory == null)
                {
                    RPRQueueSummary objRPRQueueSummary;
                    DataTable dtRPRQueueSummary = lDiscrepancyCategory == 6007 ? dsQueueSummary.Tables[0] : dsQueueSummary.Tables[6];
                    MapRPRQueueSummary(dtRPRQueueSummary, out objRPRQueueSummary);
                    objQueueSummary.objRPRQueueSummary = objRPRQueueSummary;
                }
            }
        }

        private void MapOOAQueueSummary(DataTable dtOOAQueueSummary, out OOAQueueSummary objOOAQueueSummary)
        {
            objOOAQueueSummary = new OOAQueueSummary();
            if (dtOOAQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtOOAQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("OOAAddressScrub"))
                {
                    if (!DBNull.Value.Equals(dr["OOAAddressScrub"]))
                    {
                        objOOAQueueSummary.OOAAddressScrub = Convert.ToInt64(dr["OOAAddressScrub"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOACMSAccepted"))
                {
                    if (!DBNull.Value.Equals(dr["OOACMSAccepted"]))
                    {
                        objOOAQueueSummary.OOACMSAccepted = Convert.ToInt64(dr["OOACMSAccepted"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOACMSRejected"))
                {
                    if (!DBNull.Value.Equals(dr["OOACMSRejected"]))
                    {
                        objOOAQueueSummary.OOACMSRejected = Convert.ToInt64(dr["OOACMSRejected"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOACompleted"))
                {
                    if (!DBNull.Value.Equals(dr["OOACompleted"]))
                    {
                        objOOAQueueSummary.OOACompleted = Convert.ToInt64(dr["OOACompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAMARxAddressLetter"))
                {
                    if (!DBNull.Value.Equals(dr["OOAMARxAddressLetter"]))
                    {
                        objOOAQueueSummary.OOAMARxAddressLetter = Convert.ToInt64(dr["OOAMARxAddressLetter"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAMIIMUpdated"))
                {
                    if (!DBNull.Value.Equals(dr["OOAMIIMUpdated"]))
                    {
                        objOOAQueueSummary.OOAMIIMUpdated = Convert.ToInt64(dr["OOAMIIMUpdated"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOANewCase"))
                {
                    if (!DBNull.Value.Equals(dr["OOANewCase"]))
                    {
                        objOOAQueueSummary.OOANewCase = Convert.ToInt64(dr["OOANewCase"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAOpenDisenroll"))
                {
                    if (!DBNull.Value.Equals(dr["OOAOpenDisenroll"]))
                    {
                        objOOAQueueSummary.OOAOpenDisenroll = Convert.ToInt64(dr["OOAOpenDisenroll"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAOpenNOT"))
                {
                    if (!DBNull.Value.Equals(dr["OOAOpenNOT"]))
                    {
                        objOOAQueueSummary.OOAOpenNOT = Convert.ToInt64(dr["OOAOpenNOT"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPended"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPended"]))
                    {
                        objOOAQueueSummary.OOAPended = Convert.ToInt64(dr["OOAPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPendingAudit"]))
                    {
                        objOOAQueueSummary.OOAPendingAudit = Convert.ToInt64(dr["OOAPendingAudit"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPendingFTT"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPendingFTT"]))
                    {
                        objOOAQueueSummary.OOAPendingFTT = Convert.ToInt64(dr["OOAPendingFTT"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPendingNOT"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPendingNOT"]))
                    {
                        objOOAQueueSummary.OOAPendingNOT = Convert.ToInt64(dr["OOAPendingNOT"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOASubmitToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["OOASubmitToCMS"]))
                    {
                        objOOAQueueSummary.OOASubmitToCMS = Convert.ToInt64(dr["OOASubmitToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAUpdateSentToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["OOAUpdateSentToCMS"]))
                    {
                        objOOAQueueSummary.OOAUpdateSentToCMS = Convert.ToInt64(dr["OOAUpdateSentToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPeerAuditFailed"]))
                    {
                        objOOAQueueSummary.OOAPeerAuditFailed = Convert.ToInt64(dr["OOAPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAPendingSCCRPR"))
                {
                    if (!DBNull.Value.Equals(dr["OOAPendingSCCRPR"]))
                    {
                        objOOAQueueSummary.OOAPendingSCCRPR = Convert.ToInt64(dr["OOAPendingSCCRPR"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAUpdateSenttoCMSFAILED"))
                {
                    if (!DBNull.Value.Equals(dr["OOAUpdateSenttoCMSFAILED"]))
                    {
                        objOOAQueueSummary.OOAUpdateSenttoCMSFAILED = Convert.ToInt64(dr["OOAUpdateSenttoCMSFAILED"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOALetterSentFAILED"))
                {
                    if (!DBNull.Value.Equals(dr["OOALetterSentFAILED"]))
                    {
                        objOOAQueueSummary.OOALetterSentFAILED = Convert.ToInt64(dr["OOALetterSentFAILED"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOANeedEGHPReview"))
                {
                    if (!DBNull.Value.Equals(dr["OOANeedEGHPReview"]))
                    {
                        objOOAQueueSummary.OOANeedEGHPReview = Convert.ToInt64(dr["OOANeedEGHPReview"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAOpenNOTMacro"))
                {
                    if (!DBNull.Value.Equals(dr["OOAOpenNOTMacro"]))
                    {
                        objOOAQueueSummary.OOAOpenNOTMacro = Convert.ToInt64(dr["OOAOpenNOTMacro"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAOpenDisenrollMacro"))
                {
                    if (!DBNull.Value.Equals(dr["OOAOpenDisenrollMacro"]))
                    {
                        objOOAQueueSummary.OOAOpenDisenrollMacro = Convert.ToInt64(dr["OOAOpenDisenrollMacro"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["OOAProcessingTotal"]))
                    {
                        objOOAQueueSummary.OOAProcessingTotal = Convert.ToInt64(dr["OOAProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOACompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["OOACompletedTotal"]))
                    {
                        objOOAQueueSummary.OOACompletedTotal = Convert.ToInt64(dr["OOACompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOAHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["OOAHoldingTotal"]))
                    {
                        objOOAQueueSummary.OOAHoldingTotal = Convert.ToInt64(dr["OOAHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("OOATotal"))
                {
                    if (!DBNull.Value.Equals(dr["OOATotal"]))
                    {
                        objOOAQueueSummary.OOATotal = Convert.ToInt64(dr["OOATotal"]);
                    }
                }
            }
        }

        private void MapSCCQueueSummary(DataTable dtSCCQueueSummary, out SCCQueueSummary objSCCQueueSummary)
        {
            objSCCQueueSummary = new SCCQueueSummary();
            if (dtSCCQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtSCCQueueSummary.Rows[0];
                //if (dr.Table.Columns.Contains("SCCAddressScrub"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCAddressScrub"]))
                //    {
                //        objSCCQueueSummary.SCCAddressScrub = Convert.ToInt64(dr["SCCAddressScrub"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("SCCCMSAccepted"))
                {
                    if (!DBNull.Value.Equals(dr["SCCCMSAccepted"]))
                    {
                        objSCCQueueSummary.SCCCMSAccepted = Convert.ToInt64(dr["SCCCMSAccepted"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCCMSRejected"))
                {
                    if (!DBNull.Value.Equals(dr["SCCCMSRejected"]))
                    {
                        objSCCQueueSummary.SCCCMSRejected = Convert.ToInt64(dr["SCCCMSRejected"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["SCCCompleted"]))
                    {
                        objSCCQueueSummary.SCCCompleted = Convert.ToInt64(dr["SCCCompleted"]);
                    }
                }
                //if (dr.Table.Columns.Contains("SCCMARxAddressLetter"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCMARxAddressLetter"]))
                //    {
                //        objSCCQueueSummary.SCCMARxAddressLetter = Convert.ToInt64(dr["SCCMARxAddressLetter"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("SCCMIIMUpdated"))
                {
                    if (!DBNull.Value.Equals(dr["SCCMIIMUpdated"]))
                    {
                        objSCCQueueSummary.SCCMIIMUpdated = Convert.ToInt64(dr["SCCMIIMUpdated"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCNewCase"))
                {
                    if (!DBNull.Value.Equals(dr["SCCNewCase"]))
                    {
                        objSCCQueueSummary.SCCNewCase = Convert.ToInt64(dr["SCCNewCase"]);
                    }
                }
                //if (dr.Table.Columns.Contains("SCCOpenDisenroll"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCOpenDisenroll"]))
                //    {
                //        objSCCQueueSummary.SCCOpenDisenroll = Convert.ToInt64(dr["SCCOpenDisenroll"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("SCCOpenNOT"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCOpenNOT"]))
                //    {
                //        objSCCQueueSummary.SCCOpenNOT = Convert.ToInt64(dr["SCCOpenNOT"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("SCCPended"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPended"]))
                    {
                        objSCCQueueSummary.SCCPended = Convert.ToInt64(dr["SCCPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPendingAudit"]))
                    {
                        objSCCQueueSummary.SCCPendingAudit = Convert.ToInt64(dr["SCCPendingAudit"]);
                    }
                }
                //if (dr.Table.Columns.Contains("SCCPendingFTT"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCPendingFTT"]))
                //    {
                //        objSCCQueueSummary.SCCPendingFTT = Convert.ToInt64(dr["SCCPendingFTT"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("SCCPendingNOT"))
                //{
                //    if (!DBNull.Value.Equals(dr["SCCPendingNOT"]))
                //    {
                //        objSCCQueueSummary.SCCPendingNOT = Convert.ToInt64(dr["SCCPendingNOT"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("SCCPendingSCCRPR"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPendingSCCRPR"]))
                    {
                        objSCCQueueSummary.SCCPendingSCCRPR = Convert.ToInt64(dr["SCCPendingSCCRPR"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCSubmitToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["SCCSubmitToCMS"]))
                    {
                        objSCCQueueSummary.SCCSubmitToCMS = Convert.ToInt64(dr["SCCSubmitToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCUpdateSentToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["SCCUpdateSentToCMS"]))
                    {
                        objSCCQueueSummary.SCCUpdateSentToCMS = Convert.ToInt64(dr["SCCUpdateSentToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["SCCProcessingTotal"]))
                    {
                        objSCCQueueSummary.SCCProcessingTotal = Convert.ToInt64(dr["SCCProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["SCCCompletedTotal"]))
                    {
                        objSCCQueueSummary.SCCCompletedTotal = Convert.ToInt64(dr["SCCCompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["SCCHoldingTotal"]))
                    {
                        objSCCQueueSummary.SCCHoldingTotal = Convert.ToInt64(dr["SCCHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCTotal"))
                {
                    if (!DBNull.Value.Equals(dr["SCCTotal"]))
                    {
                        objSCCQueueSummary.SCCTotal = Convert.ToInt64(dr["SCCTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPeerAuditFailed"]))
                    {
                        objSCCQueueSummary.SCCPeerAuditFailed = Convert.ToInt64(dr["SCCPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("SCCPotentialSCCRPRDay1"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPotentialSCCRPRDay1"]))
                    {
                        objSCCQueueSummary.SCCPotentialSCCRPRDay1 = Convert.ToInt64(dr["SCCPotentialSCCRPRDay1"]);
                    }
                }

                if (dr.Table.Columns.Contains("SCCNeedEGHPReview"))
                {
                    if (!DBNull.Value.Equals(dr["SCCNeedEGHPReview"]))
                    {
                        objSCCQueueSummary.SCCNeedEGHPReview = Convert.ToInt64(dr["SCCNeedEGHPReview"]);
                    }
                }

                if (dr.Table.Columns.Contains("SCCPotentialSCCRPRDay2"))
                {
                    if (!DBNull.Value.Equals(dr["SCCPotentialSCCRPRDay2"]))
                    {
                        objSCCQueueSummary.SCCPotentialSCCRPRDay2 = Convert.ToInt64(dr["SCCPotentialSCCRPRDay2"]);
                    }
                }

                if (dr.Table.Columns.Contains("SCCUpdateSenttoCMSFAILED"))
                {
                    if (!DBNull.Value.Equals(dr["SCCUpdateSenttoCMSFAILED"]))
                    {
                        objSCCQueueSummary.SCCUpdateSenttoCMSFAILED = Convert.ToInt64(dr["SCCUpdateSenttoCMSFAILED"]);
                    }
                }
            }
        }

        private void MapTRRQueueSummary(DataTable dtSCCQueueSummary, out TRRQueueSummary objTRRQueueSummary)
        {
            objTRRQueueSummary = new TRRQueueSummary();
            if (dtSCCQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtSCCQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("TRRCMSRejected"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCMSRejected"]))
                    {
                        objTRRQueueSummary.TRRCMSRejected = Convert.ToInt64(dr["TRRCMSRejected"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRCMSRejectedDeletionCode"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCMSRejectedDeletionCode"]))
                    {
                        objTRRQueueSummary.TRRCMSRejectedDeletionCode = Convert.ToInt64(dr["TRRCMSRejectedDeletionCode"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCompleted"]))
                    {
                        objTRRQueueSummary.TRRCompleted = Convert.ToInt64(dr["TRRCompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRREscalated"))
                {
                    if (!DBNull.Value.Equals(dr["TRREscalated"]))
                    {
                        objTRRQueueSummary.TRREscalated = Convert.ToInt64(dr["TRREscalated"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRFalloutTRC085"))
                {
                    if (!DBNull.Value.Equals(dr["TRRFalloutTRC085"]))
                    {
                        objTRRQueueSummary.TRRFalloutTRC085 = Convert.ToInt64(dr["TRRFalloutTRC085"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRFalloutTRC155"))
                {
                    if (!DBNull.Value.Equals(dr["TRRFalloutTRC155"]))
                    {
                        objTRRQueueSummary.TRRFalloutTRC155 = Convert.ToInt64(dr["TRRFalloutTRC155"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRNeedEGHPReview"))
                {
                    if (!DBNull.Value.Equals(dr["TRRNeedEGHPReview"]))
                    {
                        objTRRQueueSummary.TRRNeedEGHPReview = Convert.ToInt64(dr["TRRNeedEGHPReview"]);
                    }
                }
                //if (dr.Table.Columns.Contains("TRRMARxAddressLetter"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRRMARxAddressLetter"]))
                //    {
                //        objTRRQueueSummary.TRRMARxAddressLetter = Convert.ToInt64(dr["TRRMARxAddressLetter"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("TRRPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["TRRPendingAudit"]))
                    {
                        objTRRQueueSummary.TRRPendingAudit = Convert.ToInt64(dr["TRRPendingAudit"]);
                    }
                }
                //if (dr.Table.Columns.Contains("TRRPendingNOT"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRRPendingNOT"]))
                //    {
                //        objTRRQueueSummary.TRRPendingNOT = Convert.ToInt64(dr["TRRPendingNOT"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("TRRSubmitToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["TRRSubmitToCMS"]))
                    {
                        objTRRQueueSummary.TRRSubmitToCMS = Convert.ToInt64(dr["TRRSubmitToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRSubmitToCMSDeletionCode"))
                {
                    if (!DBNull.Value.Equals(dr["TRRSubmitToCMSDeletionCode"]))
                    {
                        objTRRQueueSummary.TRRSubmitToCMSDeletionCode = Convert.ToInt64(dr["TRRSubmitToCMSDeletionCode"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRTRC085"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTRC085"]))
                    {
                        objTRRQueueSummary.TRRTRC085 = Convert.ToInt64(dr["TRRTRC085"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRTRC15476"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTRC15476"]))
                    {
                        objTRRQueueSummary.TRRTRC15476 = Convert.ToInt64(dr["TRRTRC15476"]);
                    }
                }

                if (dr.Table.Columns.Contains("TRRTRC15401"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTRC15401"]))
                    {
                        objTRRQueueSummary.TRRTRC15401 = Convert.ToInt64(dr["TRRTRC15401"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRTRC155"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTRC155"]))
                    {
                        objTRRQueueSummary.TRRTRC155 = Convert.ToInt64(dr["TRRTRC155"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRTRC282"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTRC282"]))
                    {
                        objTRRQueueSummary.TRRTRC282 = Convert.ToInt64(dr["TRRTRC282"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRUpdateSentToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["TRRUpdateSentToCMS"]))
                    {
                        objTRRQueueSummary.TRRUpdateSentToCMS = Convert.ToInt64(dr["TRRUpdateSentToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRUpdateSentToCMSDeletionCode"))
                {
                    if (!DBNull.Value.Equals(dr["TRRUpdateSentToCMSDeletionCode"]))
                    {
                        objTRRQueueSummary.TRRUpdateSentToCMSDeletionCode = Convert.ToInt64(dr["TRRUpdateSentToCMSDeletionCode"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRPended"))
                {
                    if (!DBNull.Value.Equals(dr["TRRPended"]))
                    {
                        objTRRQueueSummary.TRRPended = Convert.ToInt64(dr["TRRPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRPendingSCCRPR"))
                {
                    if (!DBNull.Value.Equals(dr["TRRPendingSCCRPR"]))
                    {
                        objTRRQueueSummary.TRRPendingSCCRPR = Convert.ToInt64(dr["TRRPendingSCCRPR"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRCMSAccepted"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCMSAccepted"]))
                    {
                        objTRRQueueSummary.TRRCMSAccepted = Convert.ToInt64(dr["TRRCMSAccepted"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRCMSAcceptedDeletionCode"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCMSAcceptedDeletionCode"]))
                    {
                        objTRRQueueSummary.TRRCMSAcceptedDeletionCode = Convert.ToInt64(dr["TRRCMSAcceptedDeletionCode"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["TRRPeerAuditFailed"]))
                    {
                        objTRRQueueSummary.TRRPeerAuditFailed = Convert.ToInt64(dr["TRRPeerAuditFailed"]);
                    }
                }

                if (dr.Table.Columns.Contains("TRRUpdateSenttoCMSFAILED"))
                {
                    if (!DBNull.Value.Equals(dr["TRRUpdateSenttoCMSFAILED"]))
                    {
                        objTRRQueueSummary.TRRUpdateSenttoCMSFAILED = Convert.ToInt64(dr["TRRUpdateSenttoCMSFAILED"]);
                    }
                }
                //if (dr.Table.Columns.Contains("TRRAddressScrub"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRRAddressScrub"]))
                //    {
                //        objTRRQueueSummary.TRRAddressScrub = Convert.ToInt64(dr["TRRAddressScrub"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("TRROpenDisenroll"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRROpenDisenroll"]))
                //    {
                //        objTRRQueueSummary.TRROpenDisenroll = Convert.ToInt64(dr["TRROpenDisenroll"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("TRROpenNOT"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRROpenNOT"]))
                //    {
                //        objTRRQueueSummary.TRROpenNOT = Convert.ToInt64(dr["TRROpenNOT"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("TRRPendingFTT"))
                //{
                //    if (!DBNull.Value.Equals(dr["TRRPendingFTT"]))
                //    {
                //        objTRRQueueSummary.TRRPendingFTT = Convert.ToInt64(dr["TRRPendingFTT"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("TRRProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["TRRProcessingTotal"]))
                    {
                        objTRRQueueSummary.TRRProcessingTotal = Convert.ToInt64(dr["TRRProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["TRRCompletedTotal"]))
                    {
                        objTRRQueueSummary.TRRCompletedTotal = Convert.ToInt64(dr["TRRCompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["TRRHoldingTotal"]))
                    {
                        objTRRQueueSummary.TRRHoldingTotal = Convert.ToInt64(dr["TRRHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("TRRTotal"))
                {
                    if (!DBNull.Value.Equals(dr["TRRTotal"]))
                    {
                        objTRRQueueSummary.TRRTotal = Convert.ToInt64(dr["TRRTotal"]);
                    }
                }
            }
        }

        private void MapEligibilityQueueSummary(DataTable dtEligibilityQueueSummary, out EligibilityQueueSummary objEligibilityQueueSummary)
        {
            objEligibilityQueueSummary = new EligibilityQueueSummary();
            if (dtEligibilityQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtEligibilityQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("EligCMSAccepted"))
                {
                    if (!DBNull.Value.Equals(dr["EligCMSAccepted"]))
                    {
                        objEligibilityQueueSummary.EligCMSAccepted = Convert.ToInt64(dr["EligCMSAccepted"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligCMSRejected"))
                {
                    if (!DBNull.Value.Equals(dr["EligCMSRejected"]))
                    {
                        objEligibilityQueueSummary.EligCMSRejected = Convert.ToInt64(dr["EligCMSRejected"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["EligCompleted"]))
                    {
                        objEligibilityQueueSummary.EligCompleted = Convert.ToInt64(dr["EligCompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligNewCase"))
                {
                    if (!DBNull.Value.Equals(dr["EligNewCase"]))
                    {
                        objEligibilityQueueSummary.EligNewCase = Convert.ToInt64(dr["EligNewCase"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligPended"))
                {
                    if (!DBNull.Value.Equals(dr["EligPended"]))
                    {
                        objEligibilityQueueSummary.EligPended = Convert.ToInt64(dr["EligPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["EligPendingAudit"]))
                    {
                        objEligibilityQueueSummary.EligPendingAudit = Convert.ToInt64(dr["EligPendingAudit"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligSubmitToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["EligSubmitToCMS"]))
                    {
                        objEligibilityQueueSummary.EligSubmitToCMS = Convert.ToInt64(dr["EligSubmitToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligUpdateSentToCMS"))
                {
                    if (!DBNull.Value.Equals(dr["EligUpdateSentToCMS"]))
                    {
                        objEligibilityQueueSummary.EligUpdateSentToCMS = Convert.ToInt64(dr["EligUpdateSentToCMS"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["EligPeerAuditFailed"]))
                    {
                        objEligibilityQueueSummary.EligPeerAuditFailed = Convert.ToInt64(dr["EligPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["EligProcessingTotal"]))
                    {
                        objEligibilityQueueSummary.EligProcessingTotal = Convert.ToInt64(dr["EligProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["EligCompletedTotal"]))
                    {
                        objEligibilityQueueSummary.EligCompletedTotal = Convert.ToInt64(dr["EligCompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["EligHoldingTotal"]))
                    {
                        objEligibilityQueueSummary.EligHoldingTotal = Convert.ToInt64(dr["EligHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("EligTotal"))
                {
                    if (!DBNull.Value.Equals(dr["EligTotal"]))
                    {
                        objEligibilityQueueSummary.EligTotal = Convert.ToInt64(dr["EligTotal"]);
                    }
                }
            }
        }

        private void MapDOBQueueSummary(DataTable dtDOBQueueSummary, out DOBQueueSummary objDOBQueueSummary)
        {
            objDOBQueueSummary = new DOBQueueSummary();
            if (dtDOBQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtDOBQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("DOBCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["DOBCompleted"]))
                    {
                        objDOBQueueSummary.DOBCompleted = Convert.ToInt64(dr["DOBCompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBNewCase"))
                {
                    if (!DBNull.Value.Equals(dr["DOBNewCase"]))
                    {
                        objDOBQueueSummary.DOBNewCase = Convert.ToInt64(dr["DOBNewCase"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBPended"))
                {
                    if (!DBNull.Value.Equals(dr["DOBPended"]))
                    {
                        objDOBQueueSummary.DOBPended = Convert.ToInt64(dr["DOBPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["DOBPendingAudit"]))
                    {
                        objDOBQueueSummary.DOBPendingAudit = Convert.ToInt64(dr["DOBPendingAudit"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["DOBPeerAuditFailed"]))
                    {
                        objDOBQueueSummary.DOBPeerAuditFailed = Convert.ToInt64(dr["DOBPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["DOBProcessingTotal"]))
                    {
                        objDOBQueueSummary.DOBProcessingTotal = Convert.ToInt64(dr["DOBProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["DOBCompletedTotal"]))
                    {
                        objDOBQueueSummary.DOBCompletedTotal = Convert.ToInt64(dr["DOBCompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["DOBHoldingTotal"]))
                    {
                        objDOBQueueSummary.DOBHoldingTotal = Convert.ToInt64(dr["DOBHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("DOBTotal"))
                {
                    if (!DBNull.Value.Equals(dr["DOBTotal"]))
                    {
                        objDOBQueueSummary.DOBTotal = Convert.ToInt64(dr["DOBTotal"]);
                    }
                }
            }
        }

        private void MapGenderQueueSummary(DataTable dtGenderQueueSummary, out GenderQueueSummary objGenderQueueSummary)
        {
            objGenderQueueSummary = new GenderQueueSummary();
            if (dtGenderQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtGenderQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("GenderCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["GenderCompleted"]))
                    {
                        objGenderQueueSummary.GenderCompleted = Convert.ToInt64(dr["GenderCompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderNewCase"))
                {
                    if (!DBNull.Value.Equals(dr["GenderNewCase"]))
                    {
                        objGenderQueueSummary.GenderNewCase = Convert.ToInt64(dr["GenderNewCase"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderPended"))
                {
                    if (!DBNull.Value.Equals(dr["GenderPended"]))
                    {
                        objGenderQueueSummary.GenderPended = Convert.ToInt64(dr["GenderPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderPendingAudit"))
                {
                    if (!DBNull.Value.Equals(dr["GenderPendingAudit"]))
                    {
                        objGenderQueueSummary.GenderPendingAudit = Convert.ToInt64(dr["GenderPendingAudit"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["GenderPeerAuditFailed"]))
                    {
                        objGenderQueueSummary.GenderPeerAuditFailed = Convert.ToInt64(dr["GenderPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["GenderProcessingTotal"]))
                    {
                        objGenderQueueSummary.GenderProcessingTotal = Convert.ToInt64(dr["GenderProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["GenderCompletedTotal"]))
                    {
                        objGenderQueueSummary.GenderCompletedTotal = Convert.ToInt64(dr["GenderCompletedTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderHoldingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["GenderHoldingTotal"]))
                    {
                        objGenderQueueSummary.GenderHoldingTotal = Convert.ToInt64(dr["GenderHoldingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("GenderTotal"))
                {
                    if (!DBNull.Value.Equals(dr["GenderTotal"]))
                    {
                        objGenderQueueSummary.GenderTotal = Convert.ToInt64(dr["GenderTotal"]);
                    }
                }
            }
        }

        private void MapRPRQueueSummary(DataTable dtRPRQueueSummary, out RPRQueueSummary objRPRQueueSummary)
        {
            objRPRQueueSummary = new RPRQueueSummary();
            if (dtRPRQueueSummary.Rows.Count > 0)
            {
                DataRow dr = dtRPRQueueSummary.Rows[0];
                if (dr.Table.Columns.Contains("RPRCMSAccountManagerSent"))
                {
                    if (!DBNull.Value.Equals(dr["RPRCMSAccountManagerSent"]))
                    {
                        objRPRQueueSummary.RPRCMSAccountManagerSent = Convert.ToInt64(dr["RPRCMSAccountManagerSent"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRCMSRejectedDeletionCode"))
                {
                    if (!DBNull.Value.Equals(dr["RPRCMSRejectedDeletionCode"]))
                    {
                        objRPRQueueSummary.RPRCMSRejectedDeletionCode = Convert.ToInt64(dr["RPRCMSRejectedDeletionCode"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRCompleted"))
                {
                    if (!DBNull.Value.Equals(dr["RPRCompleted"]))
                    {
                        objRPRQueueSummary.RPRCompleted = Convert.ToInt64(dr["RPRCompleted"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRInitialSCCRPR"))
                {
                    if (!DBNull.Value.Equals(dr["RPRInitialSCCRPR"]))
                    {
                        objRPRQueueSummary.RPRInitialSCCRPR = Convert.ToInt64(dr["RPRInitialSCCRPR"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRPeerAudit"))
                {
                    if (!DBNull.Value.Equals(dr["RPRPeerAudit"]))
                    {
                        objRPRQueueSummary.RPRPeerAudit = Convert.ToInt64(dr["RPRPeerAudit"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRPeerAuditFailed"))
                {
                    if (!DBNull.Value.Equals(dr["RPRPeerAuditFailed"]))
                    {
                        objRPRQueueSummary.RPRPeerAuditFailed = Convert.ToInt64(dr["RPRPeerAuditFailed"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRPended"))
                {
                    if (!DBNull.Value.Equals(dr["RPRPended"]))
                    {
                        objRPRQueueSummary.RPRPended = Convert.ToInt64(dr["RPRPended"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRReceivedRPCFDR"))
                {
                    if (!DBNull.Value.Equals(dr["RPRReceivedRPCFDR"]))
                    {
                        objRPRQueueSummary.RPRReceivedRPCFDR = Convert.ToInt64(dr["RPRReceivedRPCFDR"]);
                    }
                }
                //if (dr.Table.Columns.Contains("RPRReceivedTRC282"))
                //{
                //    if (!DBNull.Value.Equals(dr["RPRReceivedTRC282"]))
                //    {
                //        objRPRQueueSummary.RPRReceivedTRC282 = Convert.ToInt64(dr["RPRReceivedTRC282"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("RPRRejected"))
                {
                    if (!DBNull.Value.Equals(dr["RPRRejected"]))
                    {
                        objRPRQueueSummary.RPRRejected = Convert.ToInt64(dr["RPRRejected"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRRequestCategory2"))
                {
                    if (!DBNull.Value.Equals(dr["RPRRequestCategory2"]))
                    {
                        objRPRQueueSummary.RPRRequestCategory2 = Convert.ToInt64(dr["RPRRequestCategory2"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRRequestCategory3"))
                {
                    if (!DBNull.Value.Equals(dr["RPRRequestCategory3"]))
                    {
                        objRPRQueueSummary.RPRRequestCategory3 = Convert.ToInt64(dr["RPRRequestCategory3"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRReSubmission"))
                {
                    if (!DBNull.Value.Equals(dr["RPRReSubmission"]))
                    {
                        objRPRQueueSummary.RPRReSubmission = Convert.ToInt64(dr["RPRReSubmission"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSCCRPRFDRReceived"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSCCRPRFDRReceived"]))
                    {
                        objRPRQueueSummary.RPRSCCRPRFDRReceived = Convert.ToInt64(dr["RPRSCCRPRFDRReceived"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSCCRPRReSubmission"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSCCRPRReSubmission"]))
                    {
                        objRPRQueueSummary.RPRSCCRPRReSubmission = Convert.ToInt64(dr["RPRSCCRPRReSubmission"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSCCRPRSent"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSCCRPRSent"]))
                    {
                        objRPRQueueSummary.RPRSCCRPRSent = Convert.ToInt64(dr["RPRSCCRPRSent"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSCCRPRSubmission"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSCCRPRSubmission"]))
                    {
                        objRPRQueueSummary.RPRSCCRPRSubmission = Convert.ToInt64(dr["RPRSCCRPRSubmission"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSCCRPRTransactionInquiry"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSCCRPRTransactionInquiry"]))
                    {
                        objRPRQueueSummary.RPRSCCRPRTransactionInquiry = Convert.ToInt64(dr["RPRSCCRPRTransactionInquiry"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSentToRPC"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSentToRPC"]))
                    {
                        objRPRQueueSummary.RPRSentToRPC = Convert.ToInt64(dr["RPRSentToRPC"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSubmissionCategory2"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSubmissionCategory2"]))
                    {
                        objRPRQueueSummary.RPRSubmissionCategory2 = Convert.ToInt64(dr["RPRSubmissionCategory2"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRSubmissionCategory3"))
                {
                    if (!DBNull.Value.Equals(dr["RPRSubmissionCategory3"]))
                    {
                        objRPRQueueSummary.RPRSubmissionCategory3 = Convert.ToInt64(dr["RPRSubmissionCategory3"]);
                    }
                }
                //if (dr.Table.Columns.Contains("RPRSubmitToCMSDeletionCode"))
                //{
                //    if (!DBNull.Value.Equals(dr["RPRSubmitToCMSDeletionCode"]))
                //    {
                //        objRPRQueueSummary.RPRSubmitToCMSDeletionCode = Convert.ToInt64(dr["RPRSubmitToCMSDeletionCode"]);
                //    }
                //}
                //if (dr.Table.Columns.Contains("RPRUpdateSentToCMSDeletionCode"))
                //{
                //    if (!DBNull.Value.Equals(dr["RPRUpdateSentToCMSDeletionCode"]))
                //    {
                //        objRPRQueueSummary.RPRUpdateSentToCMSDeletionCode = Convert.ToInt64(dr["RPRUpdateSentToCMSDeletionCode"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("RPREligibilityUpdateInMARx"))
                {
                    if (!DBNull.Value.Equals(dr["RPREligibilityUpdateInMARx"]))
                    {
                        objRPRQueueSummary.RPREligibilityUpdateInMARx = Convert.ToInt64(dr["RPREligibilityUpdateInMARx"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRTrend_2"))
                {
                    if (!DBNull.Value.Equals(dr["RPRTrend_2"]))
                    {
                        objRPRQueueSummary.RPRTrend_2 = Convert.ToInt64(dr["RPRTrend_2"]);
                    }
                }
                if (dr.Table.Columns.Contains("TransactionInquire"))
                {
                    if (!DBNull.Value.Equals(dr["TransactionInquire"]))
                    {
                        objRPRQueueSummary.TransactionInquire = Convert.ToInt64(dr["TransactionInquire"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRRequestCategory2CTM"))
                {
                    if (!DBNull.Value.Equals(dr["RPRRequestCategory2CTM"]))
                    {
                        objRPRQueueSummary.RPRRequestCategory2CTM = Convert.ToInt64(dr["RPRRequestCategory2CTM"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRProcessingTotal"))
                {
                    if (!DBNull.Value.Equals(dr["RPRProcessingTotal"]))
                    {
                        objRPRQueueSummary.RPRProcessingTotal = Convert.ToInt64(dr["RPRProcessingTotal"]);
                    }
                }
                if (dr.Table.Columns.Contains("RPRCompletedTotal"))
                {
                    if (!DBNull.Value.Equals(dr["RPRCompletedTotal"]))
                    {
                        objRPRQueueSummary.RPRCompletedTotal = Convert.ToInt64(dr["RPRCompletedTotal"]);
                    }
                }
                //if (dr.Table.Columns.Contains("RPRHoldingTotal"))
                //{
                //    if (!DBNull.Value.Equals(dr["RPRHoldingTotal"]))
                //    {
                //        objRPRQueueSummary.RPRHoldingTotal = Convert.ToInt64(dr["RPRHoldingTotal"]);
                //    }
                //}
                if (dr.Table.Columns.Contains("RPRTotal"))
                {
                    if (!DBNull.Value.Equals(dr["RPRTotal"]))
                    {
                        objRPRQueueSummary.RPRTotal = Convert.ToInt64(dr["RPRTotal"]);
                    }
                }
            }
        }

        private void MapToGenQueueObject(DataSet dsQueue, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            if (dsQueue.Tables.Count > 0 && dsQueue.Tables[0].Rows.Count > 0)
            {
                DataTable dtQueue = dsQueue.Tables[0];
                DataRow dr = dtQueue.Rows[0];
                if (dr.Table.Columns.Contains("GEN_QueueId"))
                {
                    if (!DBNull.Value.Equals(dr["GEN_QueueId"]))
                    {
                        objDOGEN_Queue.GEN_QueueId = Convert.ToInt64(dr["GEN_QueueId"]);
                    }
                }
                if (dr.Table.Columns.Contains("MemberCurrentHICN"))
                {
                    if (!DBNull.Value.Equals(dr["MemberCurrentHICN"]))
                    {
                        objDOGEN_Queue.MemberCurrentHICN = dr["MemberCurrentHICN"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("DiscrepancyStartDate"))
                {
                    if (!DBNull.Value.Equals(dr["DiscrepancyStartDate"]))
                    {
                        objDOGEN_Queue.DiscrepancyStartDate = Convert.ToDateTime(dr["DiscrepancyStartDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("ComplianceStartDate"))
                {
                    if (!DBNull.Value.Equals(dr["ComplianceStartDate"]))
                    {
                        objDOGEN_Queue.ComplianceStartDate = Convert.ToDateTime(dr["ComplianceStartDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("MemberFirstName"))
                {
                    if (!DBNull.Value.Equals(dr["MemberFirstName"]))
                    {
                        objDOGEN_Queue.MemberFirstName = dr["MemberFirstName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("MemberLastName"))
                {
                    if (!DBNull.Value.Equals(dr["MemberLastName"]))
                    {
                        objDOGEN_Queue.MemberLastName = dr["MemberLastName"].ToString();
                    }
                }
                if(dr.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                {
                    if (!DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"]))
                    {
                        objDOGEN_Queue.DiscrepancyCategoryLkup = Convert.ToInt64(dr["DiscrepancyCategoryLkup"]);
                    }
                }
                if (dr.Table.Columns.Contains("LockedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["LockedByRef"]))
                    {
                        objDOGEN_Queue.LockedByRef = Convert.ToInt64(dr["LockedByRef"]);
                    }
                }
            }
        }

        private void MapToListOfGenQueueObject(DataSet dsResult, out List<DOGEN_Queue> lstDOGEN_Queue)
        {
            lstDOGEN_Queue = new List<DOGEN_Queue>();
            if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsResult.Tables[0].Rows[i];
                    DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();

                    objDOGEN_Queue.GEN_QueueId = !(DBNull.Value.Equals(dr["GEN_QueueId"])) ? Convert.ToInt64(dr["GEN_QueueId"]) : 0;
                    objDOGEN_Queue.WorkBasketLkup = !(DBNull.Value.Equals(dr["WorkBasketLkup"])) ? Convert.ToInt64(dr["WorkBasketLkup"]) : 0;
                    objDOGEN_Queue.WorkBasket = !(DBNull.Value.Equals(dr["WorkBasket"])) ? Convert.ToString(dr["WorkBasket"]) : string.Empty;
                    objDOGEN_Queue.BusinessSegment = !(DBNull.Value.Equals(dr["BusinessSegment"])) ? Convert.ToString(dr["BusinessSegment"]) : string.Empty;
                    objDOGEN_Queue.DiscrepancyCategoryLkup = !(DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"])) ? Convert.ToInt64(dr["DiscrepancyCategoryLkup"]) : 0;
                    objDOGEN_Queue.DiscrepancyCategory = !(DBNull.Value.Equals(dr["DiscrepancyCategory"])) ? Convert.ToString(dr["DiscrepancyCategory"]) : string.Empty;
                    objDOGEN_Queue.DiscrepancyTypeLkup = !(DBNull.Value.Equals(dr["DiscrepancyTypeLkup"])) ? Convert.ToInt64(dr["DiscrepancyTypeLkup"]) : 0;
                    objDOGEN_Queue.DiscrepancyType = !(DBNull.Value.Equals(dr["DiscrepancyType"])) ? Convert.ToString(dr["DiscrepancyType"]) : string.Empty;
                    //objDOGEN_Queue.StartDate = !(DBNull.Value.Equals(dr["StartDate"])) ? Convert.ToDateTime(dr["StartDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.EndDate = !(DBNull.Value.Equals(dr["EndDate"])) ? Convert.ToDateTime(dr["EndDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.AssignedToRef = !(DBNull.Value.Equals(dr["AssignedToRef"])) ? Convert.ToInt64(dr["AssignedToRef"]) : (long?)null;
                    //objDOGEN_Queue.AssignedTo = !(DBNull.Value.Equals(dr["AssignedTo"])) ? Convert.ToString(dr["AssignedTo"]) : string.Empty;
                   // objDOGEN_Queue.UTCAssignedOn = !(DBNull.Value.Equals(dr["UTCAssignedOn"])) ? Convert.ToDateTime(dr["UTCAssignedOn"]) : (DateTime?)null;
                    //objDOGEN_Queue.CSTAssignedOn = !(DBNull.Value.Equals(dr["CSTAssignedOn"])) ? Convert.ToDateTime(dr["CSTAssignedOn"]) : (DateTime?)null;
                    //objDOGEN_Queue.LockedByRef = !(DBNull.Value.Equals(dr["LockedByRef"])) ? Convert.ToInt64(dr["LockedByRef"]) : 0;
                   // objDOGEN_Queue.LockedBy = !(DBNull.Value.Equals(dr["LockedBy"])) ? Convert.ToString(dr["LockedBy"]) : string.Empty;
                   // objDOGEN_Queue.UTCLockedOn = !(DBNull.Value.Equals(dr["UTCLockedOn"])) ? Convert.ToDateTime(dr["UTCLockedOn"]) : (DateTime?)null;
                    //objDOGEN_Queue.CSTLockedOn = !(DBNull.Value.Equals(dr["CSTLockedOn"])) ? Convert.ToDateTime(dr["CSTLockedOn"]) : (DateTime?)null;
                    objDOGEN_Queue.MostRecentActionLkup = !(DBNull.Value.Equals(dr["MostRecentActionLkup"])) ? Convert.ToInt64(dr["MostRecentActionLkup"]) : 0;
                    objDOGEN_Queue.MostRecentAction = !(DBNull.Value.Equals(dr["MostRecentAction"])) ? Convert.ToString(dr["MostRecentAction"]) : string.Empty;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = !(DBNull.Value.Equals(dr["MostRecentWorkQueueLkup"])) ? Convert.ToInt64(dr["MostRecentWorkQueueLkup"]) : 0;
                    objDOGEN_Queue.MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentWorkQueue"])) ? Convert.ToString(dr["MostRecentWorkQueue"]) : string.Empty;
                    objDOGEN_Queue.MostRecentStatusLkup = !(DBNull.Value.Equals(dr["MostRecentStatusLkup"])) ? Convert.ToInt64(dr["MostRecentStatusLkup"]) : 0;
                    objDOGEN_Queue.MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? Convert.ToString(dr["MostRecentStatus"]) : string.Empty;
                   // objDOGEN_Queue.SourceSystemLkup = !(DBNull.Value.Equals(dr["SourceSystemLkup"])) ? Convert.ToInt64(dr["SourceSystemLkup"]) : 0;
                    //objDOGEN_Queue.SourceSystem = !(DBNull.Value.Equals(dr["SourceSystem"])) ? Convert.ToString(dr["SourceSystem"]) : string.Empty;
                   // objDOGEN_Queue.SourceSystemId = !(DBNull.Value.Equals(dr["SourceSystemId"])) ? Convert.ToString(dr["SourceSystemId"]) : string.Empty;
                   // objDOGEN_Queue.DiscrepancySourceLkup = !(DBNull.Value.Equals(dr["DiscrepancySourceLkup"])) ? Convert.ToInt64(dr["DiscrepancySourceLkup"]) : 0;
                    //objDOGEN_Queue.DiscrepancySource = !(DBNull.Value.Equals(dr["DiscrepancySource"])) ? Convert.ToString(dr["DiscrepancySource"]) : string.Empty;
                    //objDOGEN_Queue.DiscrepancyReceiptDate = !(DBNull.Value.Equals(dr["DiscrepancyReceiptDate"])) ? Convert.ToDateTime(dr["DiscrepancyReceiptDate"]) : (DateTime?)null;
                    objDOGEN_Queue.ComplianceStartDate = !(DBNull.Value.Equals(dr["ComplianceStartDate"])) ? Convert.ToDateTime(dr["ComplianceStartDate"]) : (DateTime?)null;
                    objDOGEN_Queue.DisenrollmentDate = !(DBNull.Value.Equals(dr["DisenrollmentDate"])) ? Convert.ToDateTime(dr["DisenrollmentDate"]) : (DateTime?)null;
                    objDOGEN_Queue.Aging = !(DBNull.Value.Equals(dr["Aging"])) ? Convert.ToInt64(dr["Aging"]) : 0;
                    objDOGEN_Queue.DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? Convert.ToDateTime(dr["DiscrepancyStartDate"]) : (DateTime?)null;
                    objDOGEN_Queue.DiscrepancyEndDate = !(DBNull.Value.Equals(dr["DiscrepancyEndDate"])) ? Convert.ToDateTime(dr["DiscrepancyEndDate"]) : (DateTime?)null;
                    objDOGEN_Queue.MemberSCCCode = !(DBNull.Value.Equals(dr["MemberSCCCode"])) ? Convert.ToString(dr["MemberSCCCode"]) : string.Empty;
                    objDOGEN_Queue.MemberID = !(DBNull.Value.Equals(dr["MemberID"])) ? Convert.ToString(dr["MemberID"]) : string.Empty;
                    objDOGEN_Queue.MemberCurrentHICN = !(DBNull.Value.Equals(dr["MemberCurrentHICN"])) ? Convert.ToString(dr["MemberCurrentHICN"]) : string.Empty;
                    objDOGEN_Queue.GPSHouseholdID = !(DBNull.Value.Equals(dr["GPSHouseholdID"])) ? Convert.ToString(dr["GPSHouseholdID"]) : string.Empty;
                    objDOGEN_Queue.MemberFirstName = !(DBNull.Value.Equals(dr["MemberFirstName"])) ? Convert.ToString(dr["MemberFirstName"]) : string.Empty;
                    objDOGEN_Queue.MemberMiddleName = !(DBNull.Value.Equals(dr["MemberMiddleName"])) ? Convert.ToString(dr["MemberMiddleName"]) : string.Empty;
                    objDOGEN_Queue.MemberLastName = !(DBNull.Value.Equals(dr["MemberLastName"])) ? Convert.ToString(dr["MemberLastName"]) : string.Empty;
                    objDOGEN_Queue.MemberContractIDLkup = !(DBNull.Value.Equals(dr["MemberContractIDLkup"])) ? Convert.ToInt64(dr["MemberContractIDLkup"]) : 0;
                    objDOGEN_Queue.MemberContractID = !(DBNull.Value.Equals(dr["MemberContractID"])) ? Convert.ToString(dr["MemberContractID"]) : string.Empty;
                    objDOGEN_Queue.MemberPBPLkup = !(DBNull.Value.Equals(dr["MemberPBPLkup"])) ? Convert.ToInt64(dr["MemberPBPLkup"]) : 0;
                    objDOGEN_Queue.MemberPBP = !(DBNull.Value.Equals(dr["MemberPBP"])) ? Convert.ToString(dr["MemberPBP"]) : string.Empty;
                    objDOGEN_Queue.MemberLOBLkup = !(DBNull.Value.Equals(dr["MemberLOBLkup"])) ? Convert.ToInt64(dr["MemberLOBLkup"]) : 0;
                    objDOGEN_Queue.MemberLOB = !(DBNull.Value.Equals(dr["MemberLOB"])) ? Convert.ToString(dr["MemberLOB"]) : string.Empty;
                    objDOGEN_Queue.MemberDOB = !(DBNull.Value.Equals(dr["MemberDOB"])) ? Convert.ToDateTime(dr["MemberDOB"]) : (DateTime?)null;
                    objDOGEN_Queue.MemberGenderLkup = !(DBNull.Value.Equals(dr["MemberGenderLkup"])) ? Convert.ToInt64(dr["MemberGenderLkup"]) : 0;
                    objDOGEN_Queue.MemberGender = !(DBNull.Value.Equals(dr["MemberGender"])) ? Convert.ToString(dr["MemberGender"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSContractIDLkup = !(DBNull.Value.Equals(dr["EligGPSContractIDLkup"])) ? Convert.ToInt64(dr["EligGPSContractIDLkup"]) : 0;
                    //objDOGEN_Queue.EligGPSContractID = !(DBNull.Value.Equals(dr["EligGPSContractID"])) ? Convert.ToString(dr["EligGPSContractID"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSPBPLkup = !(DBNull.Value.Equals(dr["EligGPSPBPLkup"])) ? Convert.ToInt64(dr["EligGPSPBPLkup"]) : 0;
                    //objDOGEN_Queue.EligGPSPBP = !(DBNull.Value.Equals(dr["EligGPSPBP"])) ? Convert.ToString(dr["EligGPSPBP"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSSCCCode = !(DBNull.Value.Equals(dr["EligGPSSCCCode"])) ? Convert.ToString(dr["EligGPSSCCCode"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSCurrentHICN = !(DBNull.Value.Equals(dr["EligGPSCurrentHICN"])) ? Convert.ToString(dr["EligGPSCurrentHICN"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = !(DBNull.Value.Equals(dr["EligGPSInsuredPlanEffectiveDate"])) ? Convert.ToDateTime(dr["EligGPSInsuredPlanEffectiveDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligGPSInsuredPlanTermDate = !(DBNull.Value.Equals(dr["EligGPSInsuredPlanTermDate"])) ? Convert.ToDateTime(dr["EligGPSInsuredPlanTermDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligGPSLOBLkup = !(DBNull.Value.Equals(dr["EligGPSLOBLkup"])) ? Convert.ToInt64(dr["EligGPSLOBLkup"]) : 0;
                    //objDOGEN_Queue.EligGPSLOB = !(DBNull.Value.Equals(dr["EligGPSLOB"])) ? Convert.ToString(dr["EligGPSLOB"]) : string.Empty;
                    //objDOGEN_Queue.EligGPSMemberDOB = !(DBNull.Value.Equals(dr["EligGPSMemberDOB"])) ? Convert.ToDateTime(dr["EligGPSMemberDOB"]) : DateTime.UtcNow;
                    //objDOGEN_Queue.EligGPSGenderLkup = !(DBNull.Value.Equals(dr["EligGPSGenderLkup"])) ? Convert.ToInt64(dr["EligGPSGenderLkup"]) : 0;
                    //objDOGEN_Queue.EligGPSGender = !(DBNull.Value.Equals(dr["EligGPSGender"])) ? Convert.ToString(dr["EligGPSGender"]) : string.Empty;
                    //objDOGEN_Queue.EligMMRContractIDLkup = !(DBNull.Value.Equals(dr["EligMMRContractIDLkup"])) ? Convert.ToInt64(dr["EligMMRContractIDLkup"]) : 0;
                    //objDOGEN_Queue.EligMMRContractID = !(DBNull.Value.Equals(dr["EligMMRContractID"])) ? Convert.ToString(dr["EligMMRContractID"]) : string.Empty;
                    //objDOGEN_Queue.EligMMRPBPLkup = !(DBNull.Value.Equals(dr["EligMMRPBPLkup"])) ? Convert.ToInt64(dr["EligMMRPBPLkup"]) : 0;
                    //objDOGEN_Queue.EligMMRPBP = !(DBNull.Value.Equals(dr["EligMMRPBP"])) ? Convert.ToString(dr["EligMMRPBP"]) : string.Empty;
                    //objDOGEN_Queue.EligMMRSCCCode = !(DBNull.Value.Equals(dr["EligMMRSCCCode"])) ? Convert.ToString(dr["EligMMRSCCCode"]) : string.Empty;
                    //objDOGEN_Queue.EligMMRCurrentHICN = !(DBNull.Value.Equals(dr["EligMMRCurrentHICN"])) ? Convert.ToString(dr["EligMMRCurrentHICN"]) : string.Empty;
                    //objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = !(DBNull.Value.Equals(dr["EligMMRPaymentAdjustmentStartDate"])) ? Convert.ToDateTime(dr["EligMMRPaymentAdjustmentStartDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = !(DBNull.Value.Equals(dr["EligMMRPaymentAdjustmentEndDate"])) ? Convert.ToDateTime(dr["EligMMRPaymentAdjustmentEndDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligMMRPaymentMonth = !(DBNull.Value.Equals(dr["EligMMRPaymentMonth"])) ? Convert.ToDateTime(dr["EligMMRPaymentMonth"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligMMRDOB = !(DBNull.Value.Equals(dr["EligMMRDOB"])) ? Convert.ToDateTime(dr["EligMMRDOB"]) : (DateTime?)null;
                    //objDOGEN_Queue.EligMMRGenderLkup = !(DBNull.Value.Equals(dr["EligMMRGenderLkup"])) ? Convert.ToInt64(dr["EligMMRGenderLkup"]) : 0;
                    //objDOGEN_Queue.EligMMRGender = !(DBNull.Value.Equals(dr["EligMMRGender"])) ? Convert.ToString(dr["EligMMRGender"]) : string.Empty;
                    //objDOGEN_Queue.RPRRequestedEffectiveDate = !(DBNull.Value.Equals(dr["RPRRequestedEffectiveDate"])) ? Convert.ToDateTime(dr["RPRRequestedEffectiveDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.RPRActionRequestedLkup = !(DBNull.Value.Equals(dr["RPRActionRequestedLkup"])) ? Convert.ToInt64(dr["RPRActionRequestedLkup"]) : 0;
                    //objDOGEN_Queue.RPRActionRequested = !(DBNull.Value.Equals(dr["RPRActionRequested"])) ? Convert.ToString(dr["RPRActionRequested"]) : string.Empty;
                    //objDOGEN_Queue.RPRSupervisorOrRequesterRef = !(DBNull.Value.Equals(dr["RPRSupervisorOrRequesterRef"])) ? Convert.ToInt64(dr["RPRSupervisorOrRequesterRef"]) : 0;
                    //objDOGEN_Queue.RPRReasonforRequest = !(DBNull.Value.Equals(dr["RPRReasonforRequest"])) ? Convert.ToString(dr["RPRReasonforRequest"]) : string.Empty;
                    //objDOGEN_Queue.RPRTaskPerformedLkup = !(DBNull.Value.Equals(dr["RPRTaskPerformedLkup"])) ? Convert.ToInt64(dr["RPRTaskPerformedLkup"]) : 0;
                    //objDOGEN_Queue.RPRTaskPerformed = !(DBNull.Value.Equals(dr["RPRTaskPerformed"])) ? Convert.ToString(dr["RPRTaskPerformed"]) : string.Empty;
                    //objDOGEN_Queue.RPRCTMMember = !(DBNull.Value.Equals(dr["RPRCTMMember"])) ? Convert.ToBoolean(dr["RPRCTMMember"]) : false;
                    //objDOGEN_Queue.RPRCTMNumber = !(DBNull.Value.Equals(dr["RPRCTMNumber"])) ? Convert.ToString(dr["RPRCTMNumber"]) : string.Empty;
                    //objDOGEN_Queue.RPREGHPMember = !(DBNull.Value.Equals(dr["RPREGHPMember"])) ? Convert.ToBoolean(dr["RPREGHPMember"]) : false;
                    //objDOGEN_Queue.SCCRPRRequested = !(DBNull.Value.Equals(dr["SCCRPRRequested"])) ? Convert.ToString(dr["SCCRPRRequested"]) : string.Empty;
                    //objDOGEN_Queue.SCCRPRRequestedZip = !(DBNull.Value.Equals(dr["SCCRPRRequestedZip"])) ? Convert.ToString(dr["SCCRPRRequestedZip"]) : string.Empty;
                    //objDOGEN_Queue.SCCRPRRequstedSubmissionDate = !(DBNull.Value.Equals(dr["SCCRPRRequstedSubmissionDate"])) ? Convert.ToDateTime(dr["SCCRPRRequstedSubmissionDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.SCCRPREffectiveStartDate = !(DBNull.Value.Equals(dr["SCCRPREffectiveStartDate"])) ? Convert.ToDateTime(dr["SCCRPREffectiveStartDate"]) : (DateTime?)null;
                    //objDOGEN_Queue.SCCRPREffectiveEndDate = !(DBNull.Value.Equals(dr["SCCRPREffectiveEndDate"])) ? Convert.ToDateTime(dr["SCCRPREffectiveEndDate"]) : (DateTime?)null;
                    objDOGEN_Queue.IsCasePended = !(DBNull.Value.Equals(dr["IsCasePended"])) ? Convert.ToBoolean(dr["IsCasePended"]) : false;
                    objDOGEN_Queue.PendedbyRef = !(DBNull.Value.Equals(dr["PendedbyRef"])) ? Convert.ToInt64(dr["PendedbyRef"]) : 0;
                    objDOGEN_Queue.Pendedby = !(DBNull.Value.Equals(dr["Pendedby"])) ? Convert.ToString(dr["Pendedby"]) : string.Empty;
                    objDOGEN_Queue.UTCPendedOn = !(DBNull.Value.Equals(dr["UTCPendedOn"])) ? Convert.ToDateTime(dr["UTCPendedOn"]) : (DateTime?)null;
                    //objDOGEN_Queue.IsCaseResolved = !(DBNull.Value.Equals(dr["IsCaseResolved"])) ? Convert.ToBoolean(dr["IsCaseResolved"]) : false;
                    //objDOGEN_Queue.ResolvedByRef = !(DBNull.Value.Equals(dr["ResolvedByRef"])) ? Convert.ToInt64(dr["ResolvedByRef"]) : 0;
                    //objDOGEN_Queue.ResolvedBy = !(DBNull.Value.Equals(dr["ResolvedBy"])) ? Convert.ToString(dr["ResolvedBy"]) : string.Empty;
                    //objDOGEN_Queue.UTCResolvedOn = !(DBNull.Value.Equals(dr["UTCResolvedOn"])) ? Convert.ToDateTime(dr["UTCResolvedOn"]) : (DateTime?)null;;
                    //objDOGEN_Queue.IsParentCase = !(DBNull.Value.Equals(dr["IsParentCase"])) ? Convert.ToBoolean(dr["IsParentCase"]) : false;
                    //objDOGEN_Queue.IsChildCase = !(DBNull.Value.Equals(dr["IsChildCase"])) ? Convert.ToBoolean(dr["IsChildCase"]) : false;
                    //objDOGEN_Queue.ParentQueueRef = !(DBNull.Value.Equals(dr["ParentQueueRef"])) ? Convert.ToInt64(dr["ParentQueueRef"]) : 0;
                    objDOGEN_Queue.IsActive = !(DBNull.Value.Equals(dr["IsActive"])) ? Convert.ToBoolean(dr["IsActive"]) : false;
                    objDOGEN_Queue.UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow;
                    objDOGEN_Queue.CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0;
                    objDOGEN_Queue.CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : string.Empty;
                    objDOGEN_Queue.UTCLastUpdatedOn = !(DBNull.Value.Equals(dr["UTCLastUpdatedOn"])) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : (DateTime?)null;
                    objDOGEN_Queue.LastUpdatedByRef = !(DBNull.Value.Equals(dr["LastUpdatedByRef"])) ? Convert.ToInt64(dr["LastUpdatedByRef"]) : 0;
                    objDOGEN_Queue.LastUpdatedBy = !(DBNull.Value.Equals(dr["LastUpdatedBy"])) ? Convert.ToString(dr["LastUpdatedBy"]) : string.Empty;
                    //objDOGEN_Queue.RPROtherActionRequested = !(DBNull.Value.Equals(dr["RPROtherActionRequested"])) ? Convert.ToString(dr["RPROtherActionRequested"]) : string.Empty;
                   // objDOGEN_Queue.RPROtherTaskPerformed = !(DBNull.Value.Equals(dr["RPROtherTaskPerformed"])) ? Convert.ToString(dr["RPROtherTaskPerformed"]) : string.Empty;
                    objDOGEN_Queue.PendReasonLkup = !(DBNull.Value.Equals(dr["PendReasonLkup"])) ? Convert.ToInt64(dr["PendReasonLkup"]) : 0;
                    objDOGEN_Queue.PendReason = !(DBNull.Value.Equals(dr["PendReason"])) ? Convert.ToString(dr["PendReason"]) : string.Empty;
                    lstDOGEN_Queue.Add(objDOGEN_Queue);
                }
            }
        }

        private void MapMostRecentItems(long? TimeZone,DataSet dsMostRecentItem, out List<MostRecentItem> lstMostRecentItems)
        {
            try
            {
                lstMostRecentItems = new List<MostRecentItem>();
                if (!dsMostRecentItem.IsNull() && dsMostRecentItem.Tables.Count > 0 && dsMostRecentItem.Tables[0].Rows.Count > 0)
                {
                    lstMostRecentItems = dsMostRecentItem.Tables[0].AsEnumerable().Select(item => new MostRecentItem
                    {
                        Gen_QueueId = item["Gen_QueueId"].ToInt64(),
                        MemberCurrentHICN = item["MemberCurrentHICN"].NullToString(),
                        GPSHouseholdID = item["GPSHouseholdID"].NullToString(),
                        MemberFirstName= item["MemberFirstName"].NullToString(),
                        MemberMiddleName = item["MemberMiddleName"].NullToString(),
                        MemberLastName= item["MemberLastName"].NullToString(),
                        MemberContract= item["MemberContract"].NullToString(),
                        MemberPBP= item["MemberPBP"].NullToString(),
                        BusinessSegmentLkup = item["Gen_QueueId"].ToInt64(),
                        BusinessSegment = item["BusinessSegment"].NullToString(),
                        WorkBasketLkup = item["WorkBasketLkup"].ToInt64(),
                        WorkBasket = item["WorkBasket"].NullToString(),
                        DiscrepancyCategoryLkup = item["DiscrepancyCategoryLkup"].ToInt64(),
                        DiscrepancyCategory = item["DiscrepancyCategory"].NullToString(),
                        DiscrepancyTypeLkup = item["DiscrepancyTypeLkup"].ToInt64(),
                        DiscrepancyType = item["DiscrepancyType"].NullToString(),
                        AssignedToRef = !item["AssignedToRef"].IsNull() ? item["AssignedToRef"].ToInt64() : (Int64?)null,
                        PendedByRef  = !item["PendedByRef"].IsNull() ? item["PendedByRef"].ToInt64() : (Int64?)null,
                        AssignedTo = item["AssignedTo"].NullToString(),
                        UTCAssignedOn = !item["UTCAssignedOn"].IsNull() ? item["UTCAssignedOn"].ToDateTime() : (DateTime?)null,
                        LockedByRef = !item["LockedByRef"].IsNull() ? item["LockedByRef"].ToInt64() : (Int64?)null,
                        LockedBy = item["LockedBy"].NullToString(),
                        MostRecentActionLkup = !item["MostRecentActionLkup"].IsNull() ? item["MostRecentActionLkup"].ToInt64() : (Int64?)null,
                        MostRecentAction = item["MostRecentAction"].NullToString(),
                        MostRecentWorkQueueLkup = !item["MostRecentWorkQueueLkup"].IsNull() ? item["MostRecentWorkQueueLkup"].ToInt64() : (Int64?)null,
                        MostRecentWorkQueue = item["MostRecentWorkQueue"].NullToString(),
                        MostRecentStatusLkup = !item["MostRecentStatusLkup"].IsNull() ? item["MostRecentStatusLkup"].ToInt64() : (Int64?)null,
                        MostRecentStatus = item["MostRecentStatus"].NullToString(),
                        CreatedBy = item["CreatedBy"].NullToString(),
                        UTCCreatedOn = !item["UTCCreatedOn"].IsNull() ? ZoneLookupUI.ConvertToTimeZone(item["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LastUpdatedOn = !item["UTCLastUpdatedOn"].IsNull() ? ZoneLookupUI.ConvertToTimeZone(item["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LastUpdatedBy = item["LastUpdatedBy"].NullToString(),
                        QueueProgressTypeLkup = !item["QueueProgressTypeLkup"].IsNull() ? item["QueueProgressTypeLkup"].ToInt64() : (Int64?)null,
                        CMSTransactionStatusLkup = !item["CMSTransactionStatusLkup"].IsNull() ? item["CMSTransactionStatusLkup"].ToInt64() : (Int64?)null,
                        OOALetterStatusLkup = !item["OOALetterStatusLkup"].IsNull() ? item["OOALetterStatusLkup"].ToInt64() : (Int64?)null
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