using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.DAL;
using System.Data;
using System.Data.SqlClient;
using ENRLReconSystem.Utility;

namespace ENRLReconSystem.DAL
{
   public  class DALMIIMIntegration
    {
        public ExceptionTypes GetMIIMQueueDetailsByHICN(string MemberHICN,out List<DOMIIMGetQueue> Queues)
        {
            List<DOMIIMGetQueue> lstDOMIIMGetQueue = new List<DOMIIMGetQueue>();
            string errorMessage = string.Empty;
            Queues = new List<DOMIIMGetQueue>();
            DataSet dsResultData = new DataSet();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = MemberHICN;
                parameters.Add(sqlParam);
                
                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_GetQueueDetailsByHICN, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MappingDatasetToObject(dsResultData, out Queues);
                        return ExceptionTypes.Success;
                    }
                    else
                    {
                        return ExceptionTypes.ZeroRecords;
                    }
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

        public ExceptionTypes GetQueueIdFromMIIMRefernceId(string strMIIMReferenceId, out string strErsCaseId, out string errorMessage)
        {
            DAHelper dah = new DAHelper();
            errorMessage = string.Empty;
            strErsCaseId = string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@strMIIMReferenceId";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = strMIIMReferenceId;
            parameters.Add(sqlParam);

            string query = "Select GEN_QueueId from Gen_Queue where MIIMReferenceId = @strMIIMReferenceId";
            long executionResult = dah.ExecuteSQL(query, parameters.ToArray(), out DataSet GEN_QueueIds, out string erorrMessage,true);
            strErsCaseId = (!GEN_QueueIds.IsNull()) ? String.Join(",", GEN_QueueIds.Tables[0].AsEnumerable().Select(x => x.Field<Int64>("GEN_QueueId").ToString()).ToArray()) : string.Empty;
            return (ExceptionTypes)executionResult;
        }

        public ExceptionTypes UpdateOOAMIIMComments(List<DOMIIMOOACommentUpdate> lstDOMIIMOOACommentUpdate, long userid)
        {
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            SqlParameter sqlParam = new SqlParameter();
            List<SqlParameter> _lstParameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution
            if (lstDOMIIMOOACommentUpdate.Count() > 0)
            {
                DataSet ds = new DataSet("tbl_Comments");
                ds.Tables.Add(lstDOMIIMOOACommentUpdate.Where(x => x.ERSCaseId != 0).ToList().ToDataTable());
                string xmlData = ds.GetXml();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@XMLComments";
                sqlParam.SqlDbType = SqlDbType.NVarChar;
                sqlParam.Value = xmlData;
                _lstParameters.Add(sqlParam);
            }

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@LoginUserId";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = userid;
            _lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            _lstParameters.Add(sqlParam);

            long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_UpdateOOAComments, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out long lRowsEffected, out string errorMessage);
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
                return ExceptionTypes.UnknownError;
        }

        public ExceptionTypes GetCaseDiscrepancyCategory(long lQueueID, out long lDiscrepancyCategory, out string errorMessage)
        {
            DAHelper dah = new DAHelper();
            errorMessage = string.Empty;

            List<SqlParameter> parameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution
            SqlParameter sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@lQueueID";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = lQueueID;
            parameters.Add(sqlParam);

            string query = "Select DiscrepancyCategoryLkup from Gen_Queue where Gen_QueueId=@lQueueID";
            long executionResult = dah.ExecuteScalar(query, parameters.ToArray(), out object DiscrepancyCategoryLkup, out string erorrMessage);
            lDiscrepancyCategory = (!DiscrepancyCategoryLkup.IsNull()) ? DiscrepancyCategoryLkup.ToInt64() : 0;
            return (ExceptionTypes)executionResult;
        }

        private void MappingDatasetToObject(DataSet ds, out List<DOMIIMGetQueue> lstDOMIIMGetQueue)
        {
            lstDOMIIMGetQueue = new List<DOMIIMGetQueue>();
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DOMIIMGetQueue objDOMIIMGetQueue = new DOMIIMGetQueue();
                    if (row.Table.Columns.Contains("QueueId") && !row.IsNull("QueueId"))
                    {
                        objDOMIIMGetQueue.CaseId = ExtensionMethods.ToLong(row["QueueId"]);
                    }
                    if (row.Table.Columns.Contains("CreatedOn") && !row.IsNull("CreatedOn"))
                    {
                        objDOMIIMGetQueue.CreationDate = ExtensionMethods.ToDateTime(row["CreatedOn"]).ToShortDateString();
                    }
                    if (row.Table.Columns.Contains("StatusRef") && !row.IsNull("StatusRef"))
                    {
                        objDOMIIMGetQueue.StatusRef = ExtensionMethods.ToLong(row["StatusRef"]);
                    }
                    if (row.Table.Columns.Contains("Status") && !row.IsNull("Status"))
                    {
                        objDOMIIMGetQueue.Status = row["Status"].ToString();
                    }
                    if (row.Table.Columns.Contains("CaseTypeRef") && !row.IsNull("CaseTypeRef"))
                    {
                        objDOMIIMGetQueue.CaseType = row["CaseTypeRef"].ToString();
                    }
                    if (row.Table.Columns.Contains("CaseType") && !row.IsNull("CaseType"))
                    {
                        objDOMIIMGetQueue.CaseTypeRef = ExtensionMethods.ToLong(row["CaseType"]);
                    }
                    if (row.Table.Columns.Contains("CaseCategoryType") && !row.IsNull("CaseCategoryType"))
                    {
                        objDOMIIMGetQueue.CaseCategoryTypeRef = ExtensionMethods.ToLong(row["CaseCategoryType"]);
                    }
                    if (row.Table.Columns.Contains("CaseCategoryTypeRef") && !row.IsNull("CaseCategoryTypeRef"))
                    {
                        objDOMIIMGetQueue.CaseCategoryType = row["CaseCategoryTypeRef"].ToString();
                    }
                    lstDOMIIMGetQueue.Add(objDOMIIMGetQueue);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
