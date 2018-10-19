using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace ENRLReconSystem.DAL
{
    public class DALFDR
    {
        DAHelper _objDAHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;
        public ExceptionTypes InsertFDRBulkImport(DOGEN_FDR objDOGEN_FDR, out string errorMessage)
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
                sqlParam.ParameterName = "@SubmissionId";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.SubmissionID;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.ContractNumber;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionType";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.TransactionTypeLValue;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.ExcelFileName;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFilePath";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.ExcelFilePath;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ImportStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (long)ImportStatus.ReadyForImport;
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
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INS_FDR_BulkImport, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        public ExceptionTypes ValidationSP(long BulkImportID, out string errorMessage)
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
                sqlParam.ParameterName = "@FDRImportId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = BulkImportID;
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
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_SEL_FDRValidation, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        public ExceptionTypes GetIncludeInTodaysSubmission(out List<IncludeInTodaysSubmission> lstIncludeInTodaysSubmission,out string errorMessage)
        {
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();
            errorMessage = string.Empty;
            lstIncludeInTodaysSubmission = new List<IncludeInTodaysSubmission>();
            errorMessage = string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            try
            {             
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_BGP_SEL_IncludeInTodaysSubmission, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        public ExceptionTypes CheckValidRecordCount(long BulkImportID, out string errorMessage)
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
                sqlParam.ParameterName = "@GEN_FDRBulkImportId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = BulkImportID;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ImportStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (long)ImportStatus.ImportSuccessful;
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
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_UPD_FDRTotalRecordsCount, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        public ExceptionTypes GetQueueID(out long QueueID, out string CMSProcessDate, out string DispositionCode,out string DispositionCodeDescription,out long TransactionTypeLkup, out string errorMessage)
        {
            QueueID = 0;
            CMSProcessDate = "";
            DispositionCode = "";
            DispositionCodeDescription = "";
            TransactionTypeLkup = 0;
            errorMessage = string.Empty;
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
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 200000000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSProcessDate";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DispositionCode";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DispositionCodeDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 20000000;
                parameters.Add(sqlParam);                

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_SEL_FDR_Queue, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@GEN_QueueId");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    QueueID = Convert.ToInt64(sqlParam.Value);
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@CMSProcessDate");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    CMSProcessDate = Convert.ToString(sqlParam.Value);
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@DispositionCode");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    DispositionCode = Convert.ToString(sqlParam.Value);
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@DispositionCodeDescription");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    DispositionCodeDescription = Convert.ToString(sqlParam.Value);
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@TransactionTypeLkup");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    TransactionTypeLkup = Convert.ToInt64(sqlParam.Value);
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

        public ExceptionTypes UpdateRPRQueue(long QueueID, string CMSProcessDate, string DispositionCode, string DispositionCodeDescription, long TransactionTypeLkup, out string errorMessage)
        {
             errorMessage = string.Empty;
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
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;             
                sqlParam.Value = QueueID;
                parameters.Add(sqlParam);
               

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSProcessDate";
                sqlParam.SqlDbType = SqlDbType.VarChar;              
                sqlParam.Value = CMSProcessDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DispositionCode";
                sqlParam.SqlDbType = SqlDbType.VarChar;               
                sqlParam.Value = DispositionCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DispositionCodeDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;               
                sqlParam.Value = DispositionCodeDescription;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;              
                sqlParam.Value = TransactionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_UPD_RPR_Queue, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        public ExceptionTypes GetBulkImportID(out long BulkImportID, out string ExcelFilePath,out string errorMessage)
        {
            BulkImportID = 0;
            ExcelFilePath = string.Empty;
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
                sqlParam.ParameterName = "@GEN_FDRBulkImportId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 200000000;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFilePath";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_SEL_FDR_BulkImportPath, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@GEN_FDRBulkImportId");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    BulkImportID = Convert.ToInt64(sqlParam.Value);
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ExcelFilePath");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    ExcelFilePath = Convert.ToString(sqlParam.Value);
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

        public ExceptionTypes InsertFDRStagingData(DOGEN_FDR objDOGEN_FDR, out string errorMessage)
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
                sqlParam.ParameterName = "@GEN_FDRBulkImportId";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_FDR.FDRBulkImportID;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                //DataTableReader drTable = new DataTableReader(objDOGEN_FDR.dtExcelData);
                DataSet ds = new DataSet("TVP_FDRStaging");
                ds.Tables.Add(objDOGEN_FDR.dtExcelData);
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


                DataSet dsResult = new DataSet();
                long executionResult = 0;
                executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INS_FDR_Staging, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
               // executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_BGP_INS_FDR_Staging, parameters.ToArray(),out dsResult, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        #region FDR Submission
        public ExceptionTypes GetCaseDetails(string strContractId, string strHICN, long lDiscrepancyType, long lTransactionTypeLkup, out List<FDRSubmissionRow> lstResults, out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            lstResults = new List<FDRSubmissionRow>();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentHICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strHICN;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strContractId;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lTransactionTypeLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lDiscrepancyType;
                _lstParameters.Add(sqlParam); 

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                //Current Queues(RPR - Re - Submission, RPR - SCC RPR Re - Submission, RPR - SCC RPR Sent, RPR - Sent to RPC)
                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_BGP_SEL_FDRGetCaseDetails, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapCaseDetails(_dsResult, out lstResults);
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
        public ExceptionTypes UpdateFDRPackageDate(List<long> caseIds, FDRSubmissionCategory submissionCategory,long LoginUserId, out string errorMessage)
        {
            string strCaseIds = String.Concat(caseIds.Select(x => x.ToString() + ","));
            errorMessage = string.Empty;
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
                sqlParam.ParameterName = "@CaseIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strCaseIds;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FDRSubmissionCategoryLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (long)submissionCategory;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = LoginUserId;
                parameters.Add(sqlParam); 

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_UPD_FDRPackageDates, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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
        private void MapCaseDetails(DataSet dsResult, out List<FDRSubmissionRow> lstFDRSubmissionRow)
        {
            lstFDRSubmissionRow = new List<FDRSubmissionRow>();
            if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    DataRow dr = dsResult.Tables[0].Rows[i];
                    FDRSubmissionRow objFDRSubmissionRow = new FDRSubmissionRow();
                    objFDRSubmissionRow.GEN_QueueId = !(DBNull.Value.Equals(dr["GEN_QueueId"])) ? Convert.ToInt64(dr["GEN_QueueId"]) : 0;
                    objFDRSubmissionRow.RPRRequestedEffectiveDate = !(DBNull.Value.Equals(dr["RPRRequestedEffectiveDate"])) ? Convert.ToDateTime(dr["RPRRequestedEffectiveDate"]).ToString("MM/dd/yyyy") : string.Empty;
                    objFDRSubmissionRow.EffectiveDate = !(DBNull.Value.Equals(dr["EffectiveDate"])) ? Convert.ToDateTime(dr["EffectiveDate"]).ToString("MM/dd/yyyy") : string.Empty;
                    objFDRSubmissionRow.EndDate = !(DBNull.Value.Equals(dr["EndDate"])) ? Convert.ToDateTime(dr["EndDate"]).ToString("MM/dd/yyyy") : string.Empty;
                    objFDRSubmissionRow.ApplicationDate = !(DBNull.Value.Equals(dr["ApplicationDate"])) ? Convert.ToDateTime(dr["ApplicationDate"]).ToString("MM/dd/yyyy") : string.Empty;
                    objFDRSubmissionRow.MemberCurrentHICN = !(DBNull.Value.Equals(dr["MemberCurrentHICN"])) ? Convert.ToString(dr["MemberCurrentHICN"]) : string.Empty;
                    objFDRSubmissionRow.MemberFirstName = !(DBNull.Value.Equals(dr["MemberFirstName"])) ? Convert.ToString(dr["MemberFirstName"]) : string.Empty;
                    objFDRSubmissionRow.MemberLastName = !(DBNull.Value.Equals(dr["MemberLastName"])) ? Convert.ToString(dr["MemberLastName"]) : string.Empty;
                    objFDRSubmissionRow.MemberContractID = !(DBNull.Value.Equals(dr["MemberContractID"])) ? Convert.ToString(dr["MemberContractID"]) : string.Empty;
                    objFDRSubmissionRow.MemberPBP = !(DBNull.Value.Equals(dr["MemberPBP"])) ? Convert.ToString(dr["MemberPBP"]) : string.Empty;
                    objFDRSubmissionRow.ElectionType = !(DBNull.Value.Equals(dr["ElectionType"])) ? Convert.ToString(dr["ElectionType"]) : string.Empty;
                    objFDRSubmissionRow.RPRActionRequestedLkup = !(DBNull.Value.Equals(dr["RPRActionRequestedLkup"])) ? Convert.ToInt64(dr["RPRActionRequestedLkup"]) : 0;
                    objFDRSubmissionRow.SCCRPRRequested = !(DBNull.Value.Equals(dr["SCCRPRRequested"])) ? Convert.ToString(dr["SCCRPRRequested"]) : string.Empty;
                    objFDRSubmissionRow.SCCRPRRequestedZip = !(DBNull.Value.Equals(dr["SCCRPRRequestedZip"])) ? Convert.ToString(dr["SCCRPRRequestedZip"]) : string.Empty;
                    objFDRSubmissionRow.ResolutionLkup = !(DBNull.Value.Equals(dr["ResolutionLkup"])) ? Convert.ToInt64(dr["ResolutionLkup"]) : 0;
                    lstFDRSubmissionRow.Add(objFDRSubmissionRow);
                }
            }
        }

        public ExceptionTypes InsertFDRSubmissionLog(List<FDRSubmissionRow> lstCases, long LoginUserId, bool IsFDRSubmissionCompleted, string errormessage, out string errorMessage)
        {
            FDRSubmissionRow objFDRSubmissionRow = new FDRSubmissionRow();
            try
            {
                DAHelper dah = new DAHelper();

                List<SqlParameter> parameters = new List<SqlParameter>();
                long lErrorCode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();
                SqlParameter sqlParam;
                
                if (lstCases.Count() > 0)
                {
                    DataTable dtCases;
                    SetFDRSubmissionLog(lstCases, IsFDRSubmissionCompleted, errormessage, out dtCases);
                    DataTableReader dtrFDRSubmissionLog = new DataTableReader(dtCases);
                    //DataSet ds = new DataSet("TVP_FDRStaging");
                    //ds.Tables.Add(dtCases);
                    //string xmlData = ds.GetXml();

                    //sqlParam = new SqlParameter();
                    //sqlParam.ParameterName = "@XMLData";
                    //sqlParam.SqlDbType = SqlDbType.NVarChar;
                    //sqlParam.Value = xmlData;
                    //parameters.Add(sqlParam);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TVP_FDRSubmissionLog";
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    sqlParam.Value = dtrFDRSubmissionLog;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = LoginUserId;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_FDRSubmissionLog, parameters.ToArray(), out lErrorCode, out lErrorCode, out lErrorNumber, out errorMessage);

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
        private void SetFDRSubmissionLog(List<FDRSubmissionRow> lstFDRSubmissionRow, bool IsFDRSubmissionCompleted, string errormessage, out DataTable dtCases)
        {
            dtCases = new DataTable("TVP_FDRSubmissionLog");
            GetDataColumn(dtCases);
            DataRow dr;
            try
            {
                if (lstFDRSubmissionRow != null && lstFDRSubmissionRow.Count > 0)
                {
                    foreach (FDRSubmissionRow fDRSubmissionRow in lstFDRSubmissionRow)
                    {
                        dr = dtCases.NewRow();
                        dr["GEN_QueueRef"] = fDRSubmissionRow.GEN_QueueId;
                        dr["MemberFirstName"] = fDRSubmissionRow.MemberFirstName;
                        dr["MemberLastName"] = fDRSubmissionRow.MemberLastName;
                        dr["MemberContractID"] = fDRSubmissionRow.MemberContractID;
                        dr["MemberPBP"] = fDRSubmissionRow.MemberPBP;
                        dr["RPRRequestedEffectiveDate"] = fDRSubmissionRow.RPRRequestedEffectiveDate;
                        dr["RPRActionRequestedLkup"] = fDRSubmissionRow.RPRActionRequestedLkup;
                        dr["SCCRPRRequested"] = fDRSubmissionRow.SCCRPRRequested;
                        dr["SCCRPRRequestedZip"] = fDRSubmissionRow.SCCRPRRequestedZip;
                        dr["RPRActionRequested"] = null;
                        dr["MemberCurrentHICN"] = fDRSubmissionRow.MemberCurrentHICN;
                        dr["ApplicationDate"] = fDRSubmissionRow.ApplicationDate;
                        dr["EffectiveDate"] = fDRSubmissionRow.RPRRequestedEffectiveDate;
                        dr["EndDate"] = fDRSubmissionRow.EndDate;
                        dr["ElectionType"] = fDRSubmissionRow.ElectionType;
                        dr["IsFDRSubmissionCompleted"] = IsFDRSubmissionCompleted;
                        dr["ErrorDescription"] = errormessage;
                        dtCases.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetDataColumn(DataTable dt)
        {
            DataColumn dtColumn;
            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.Int64");
            dtColumn.ColumnName = "GEN_QueueRef";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "MemberFirstName";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "MemberLastName";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "MemberContractID";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "MemberPBP";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "RPRRequestedEffectiveDate";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.Int64");
            dtColumn.ColumnName = "RPRActionRequestedLkup";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "SCCRPRRequested";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "SCCRPRRequestedZip";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "RPRActionRequested";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "MemberCurrentHICN";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "ApplicationDate";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "EffectiveDate";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "EndDate";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "ElectionType";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.Boolean");
            dtColumn.ColumnName = "IsFDRSubmissionCompleted";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);

            dtColumn = new DataColumn();
            dtColumn.DataType = System.Type.GetType("System.String");
            dtColumn.ColumnName = "ErrorDescription";
            dtColumn.ReadOnly = true;
            dt.Columns.Add(dtColumn);
        }
        #endregion


    }
}
