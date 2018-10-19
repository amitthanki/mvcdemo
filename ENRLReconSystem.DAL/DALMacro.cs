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
using System.Reflection;


namespace ENRLReconSystem.DAL
{
    public class DALMacro
    {
        public ExceptionTypes GetOpenNotMacro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType, out List<DOMacroData> Queues,out string errorMessage)
        {
            List<DOMacroData> lstOpenNotMacro = new List<DOMacroData>();
            p_HouseholdID = ((p_HouseholdID == null)||(p_HouseholdID == "null")) ? null : p_HouseholdID;
            p_HICN = ((p_HICN == null) || (p_HICN == "null")) ? null : p_HICN;
            p_Contract = ((p_Contract == null) || (p_Contract == "null")) ? null : p_Contract;
            p_PBP = ((p_PBP == null) || (p_PBP == "null")) ? null : p_PBP;
            p_EffectiveDate = ((p_EffectiveDate == "null")||(p_EffectiveDate=="01/01/0001")|| (p_EffectiveDate == null)) ? null : p_EffectiveDate.Replace('-', '/');
            p_DiscrepancyType = ((p_DiscrepancyType == null) || (p_DiscrepancyType == "null")) ? null : p_DiscrepancyType;

            errorMessage = string.Empty;
            Queues = new List<DOMacroData>();
            DataSet dsResultData = new DataSet();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                SqlParameter sqlParam = new SqlParameter();
                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HouseholdID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HouseholdID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HICN;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Contract";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_Contract;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBP";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_PBP;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyType";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_DiscrepancyType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);


                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_NOT_Macro, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == 0 && string.IsNullOrEmpty(errorMessage))
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
                DALCommon.LogError(23, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.Macro, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }

        }

        private void MappingDatasetToObject(DataSet ds, out List<DOMacroData> lstDOMacroData)
        {
            lstDOMacroData = new List<DOMacroData>();
            try
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DOMacroData objDOMacro = new DOMacroData();
                    if (row.Table.Columns.Contains("WorkItemID") && !row.IsNull("WorkItemID"))
                    {
                        objDOMacro.WorkItemID = ExtensionMethods.ToLong(row["WorkItemID"]);
                    }
                    if (row.Table.Columns.Contains("MemberSCCCode") && !row.IsNull("MemberSCCCode"))
                    {
                        objDOMacro.MemberSCCCode = row["MemberSCCCode"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberID") && !row.IsNull("MemberID"))
                    {
                        objDOMacro.MemberID = row["MemberID"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberCurrentHICN") && !row.IsNull("MemberCurrentHICN"))
                    {
                        objDOMacro.MemberCurrentHICN = row["MemberCurrentHICN"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberFirstName") && !row.IsNull("MemberFirstName"))
                    {
                        objDOMacro.MemberID = row["MemberFirstName"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberMiddleName") && !row.IsNull("MemberMiddleName"))
                    {
                        objDOMacro.MemberMiddleName = row["MemberMiddleName"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberLastName") && !row.IsNull("MemberLastName"))
                    {
                        objDOMacro.MemberLastName = row["MemberLastName"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberContractID") && !row.IsNull("MemberContractID"))
                    {
                        objDOMacro.MemberContractID = row["MemberContractID"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberDOB") && !row.IsNull("MemberDOB"))
                    {
                        objDOMacro.MemberDOB = ExtensionMethods.ToDateTime(row["MemberDOB"]).ToShortDateString();
                    }
                    if (row.Table.Columns.Contains("MemberPBP") && !row.IsNull("MemberPBP"))
                    {
                        objDOMacro.MemberPBP = row["MemberPBP"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberLOB") && !row.IsNull("MemberLOB"))
                    {
                        objDOMacro.MemberLOB = row["MemberLOB"].ToString();
                    }
                    if (row.Table.Columns.Contains("MemberGender") && !row.IsNull("MemberGender"))
                    {
                        objDOMacro.MemberGender = row["MemberGender"].ToString();
                    }
                    if (row.Table.Columns.Contains("DiscrepancyType") && !row.IsNull("DiscrepancyType"))
                    {
                        objDOMacro.DiscrepancyType = row["DiscrepancyType"].ToString();
                    }
                    if (row.Table.Columns.Contains("MostRecentStatusLkup") && !row.IsNull("MostRecentStatusLkup"))
                    {
                        objDOMacro.MostRecentStatusLkup = ExtensionMethods.ToLong(row["MostRecentStatusLkup"]);
                    }
                    if (row.Table.Columns.Contains("MostRecentStatus") && !row.IsNull("MostRecentStatus"))
                    {
                        objDOMacro.MostRecentStatus = row["MostRecentStatus"].ToString();
                    }
                    if (row.Table.Columns.Contains("MostRecentWorkQueueLkup") && !row.IsNull("MostRecentWorkQueueLkup"))
                    {
                        objDOMacro.MostRecentWorkQueueLkup = ExtensionMethods.ToLong(row["MostRecentWorkQueueLkup"]);
                    }
                    if (row.Table.Columns.Contains("MostRecentQueue") && !row.IsNull("MostRecentQueue"))
                    {
                        objDOMacro.MostRecentQueue = row["MostRecentQueue"].ToString();
                    }
                    if (row.Table.Columns.Contains("MostRecentAction") && !row.IsNull("MostRecentAction"))
                    {
                        objDOMacro.MostRecentAction = row["MostRecentAction"].ToString();
                    }
                    if (row.Table.Columns.Contains("GPSHouseholdID") && !row.IsNull("GPSHouseholdID"))
                    {
                        objDOMacro.GPSHouseholdID = row["GPSHouseholdID"].ToString();
                    }
                    if (row.Table.Columns.Contains("ComplianceStartDate") && !row.IsNull("ComplianceStartDate"))
                    {
                        objDOMacro.ComplianceStartDate = ExtensionMethods.ToDateTime(row["ComplianceStartDate"]).ToShortDateString();
                    }
                    if (row.Table.Columns.Contains("StateCode") && !row.IsNull("StateCode"))
                    {
                        objDOMacro.StateCode = row["StateCode"].ToString();
                    }
                    if (row.Table.Columns.Contains("CountyCode") && !row.IsNull("CountyCode"))
                    {
                        objDOMacro.CountyCode  = row["CountyCode"].ToString();
                    }
                    if (row.Table.Columns.Contains("PlanCode") && !row.IsNull("PlanCode"))
                    {
                        objDOMacro.PlanCode = row["PlanCode"].ToString();
                    }
                    if (row.Table.Columns.Contains("PotentialTermDate") && !row.IsNull("PotentialTermDate"))
                    {
                        objDOMacro.PotentialTermDate = row["PotentialTermDate"].ToString();
                    }
                    if (row.Table.Columns.Contains("InsuredPlanEffectiveDate") && !row.IsNull("InsuredPlanEffectiveDate"))
                    {
                        objDOMacro.InsuredPlanEffectiveDate = row["InsuredPlanEffectiveDate"].ToString();
                    }
                    if (row.Table.Columns.Contains("ErrorMessage") && !row.IsNull("ErrorMessage"))
                    {
                        objDOMacro.ErrorMessage = row.IsNull("ErrorMessage") == true ? string.Empty : row["ErrorMessage"].ToString();
                    }
                    lstDOMacroData.Add(objDOMacro);
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Method to get the cases for FTT Macros
        /// </summary>
        /// <param name="p_HouseholdID"></param>
        /// <param name="p_HICN"></param>
        /// <param name="p_Contract"></param>
        /// <param name="p_PBP"></param>
        /// <param name="p_EffectiveDate"></param>
        /// <param name="p_DiscrepancyType"></param>
        /// <param name="Queues"></param>
        /// <returns></returns>
        public ExceptionTypes GetCasesForFTTMacro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType, out List<DOMacroData> Queues, out string errorMessage)
        {
            List<DOMacroData> lstDoMacroFTTData = new List<DOMacroData>();
            p_HouseholdID = ((p_HouseholdID == null) || (p_HouseholdID == "null")) ? null : p_HouseholdID;
            p_HICN = ((p_HICN == null) || (p_HICN == "null")) ? null : p_HICN;
            p_Contract = ((p_Contract == null) || (p_Contract == "null")) ? null : p_Contract;
            p_PBP = ((p_PBP == null) || (p_PBP == "null")) ? null : p_PBP;
            p_EffectiveDate = ((p_EffectiveDate == "null") || (p_EffectiveDate == "01/01/0001") || (p_EffectiveDate == null)) ? null : p_EffectiveDate.Replace('-', '/');
            p_DiscrepancyType = ((p_DiscrepancyType == null) || (p_DiscrepancyType == "null")) ? null : p_DiscrepancyType;

            errorMessage = string.Empty;
            Queues = new List<DOMacroData>();
            DataSet dsResultData = new DataSet();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                SqlParameter sqlParam = new SqlParameter();
                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HouseholdID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HouseholdID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HICN;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Contract";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_Contract;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBP";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_PBP;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyType";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_DiscrepancyType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);


                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_FTT_Macro, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == 0 && string.IsNullOrEmpty(errorMessage))
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
                DALCommon.LogError(23, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.Macro, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }

        }

        /// <summary>
        /// Method to get the cases for TRC155 macros
        /// </summary>
        /// <param name="p_HouseholdID"></param>
        /// <param name="p_HICN"></param>
        /// <param name="p_Contract"></param>
        /// <param name="p_PBP"></param>
        /// <param name="p_EffectiveDate"></param>
        /// <param name="p_DiscrepancyType"></param>
        /// <param name="Queues"></param>
        /// <returns></returns>
        public ExceptionTypes GetCasesForTRC155Macro(string p_HouseholdID, string p_HICN, string p_Contract, String p_PBP, string p_EffectiveDate, string p_DiscrepancyType, out List<DOMacroData> Queues, out string errorMessage)
        {
            List<DOMacroData> lstDoMacroTRCData = new List<DOMacroData>();
            p_HouseholdID = ((p_HouseholdID == null) || (p_HouseholdID == "null")) ? null : p_HouseholdID;
            p_HICN = ((p_HICN == null) || (p_HICN == "null")) ? null : p_HICN;
            p_Contract = ((p_Contract == null) || (p_Contract == "null")) ? null : p_Contract;
            p_PBP = ((p_PBP == null) || (p_PBP == "null")) ? null : p_PBP;
            p_EffectiveDate = ((p_EffectiveDate == "null") || (p_EffectiveDate == "01/01/0001") || (p_EffectiveDate == null)) ? null : p_EffectiveDate.Replace('-', '/');
            p_DiscrepancyType = ((p_DiscrepancyType == null) || (p_DiscrepancyType == "null")) ? null : p_DiscrepancyType;

            errorMessage = string.Empty;
            Queues = new List<DOMacroData>();
            DataSet dsResultData = new DataSet();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                SqlParameter sqlParam = new SqlParameter();
                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HouseholdID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HouseholdID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_HICN;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Contract";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_Contract;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBP";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_PBP;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyType";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = p_DiscrepancyType;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);


                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_TRC155_Macro, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == 0 && string.IsNullOrEmpty(errorMessage))
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
                DALCommon.LogError(23, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.Macro, (long)ExceptionTypes.Exception, ex.InnerException.Message, "");
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }

        }

        public ExceptionTypes UpdateOpenNotMacro(DOMacroUpdate objDOMacroUpdate, long userid, out string errormessage)
        {
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            errormessage = string.Empty;
            SqlParameter sqlParam = new SqlParameter();
            List<SqlParameter> _lstParameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@GEN_QueueId";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOMacroUpdate.GEN_QueueId;
            _lstParameters.Add(sqlParam);           

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Status";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = objDOMacroUpdate.Status;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@CurrentUser";
            //sqlParam.SqlDbType = SqlDbType.VarChar;
            //sqlParam.Value = objDOMacroUpdate.LoginID;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            _lstParameters.Add(sqlParam);

            long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_UPD_NOT_Macro, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out long lRowsEffected, out string errorMessage);
            sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
            if (sqlParam != null && sqlParam.Value != null)
            {
                errormessage += sqlParam.Value.ToString();
            }
            if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
            {
                return ExceptionTypes.Success;
            }
            else
                return ExceptionTypes.UnknownError;
        }

        public ExceptionTypes UpdateFTTMacro(DOMacroUpdate objDOMacroUpdate, long userid, out string errormessage)
        {
            DAHelper dah = new DAHelper();
            long lErrocode = 0;
            long lErrorNumber = 0;
            SqlParameter sqlParam = new SqlParameter();
            errormessage = string.Empty;
            List<SqlParameter> _lstParameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@GEN_QueueId";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOMacroUpdate.GEN_QueueId;
            _lstParameters.Add(sqlParam);          

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Status";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = objDOMacroUpdate.Status;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@CurrentUser";
            //sqlParam.SqlDbType = SqlDbType.VarChar;
            //sqlParam.Value = objDOMacroUpdate.LoginID;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            _lstParameters.Add(sqlParam);

            long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_UPD_FTT_Macro, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out long lRowsEffected, out string errorMessage);
            sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
            if (sqlParam != null && sqlParam.Value != null)
            {
                errormessage += sqlParam.Value.ToString();
            }
            if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
            {
                return ExceptionTypes.Success;
            }
            else
                return ExceptionTypes.UnknownError;
        }

    
        /// <summary>
        /// Method to update the cases for TRC155 Macro
        /// </summary>
        /// <param name="objDOMacroUpdate"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ExceptionTypes UpdateCaseForTRC155Macro(DOMacroUpdate objDOMacroUpdate, long userid, out string errormessage)
        {
            DAHelper dah = new DAHelper();         
            long lErrocode = 0;
            long lErrorNumber = 0;
            errormessage = string.Empty;
            SqlParameter sqlParam = new SqlParameter();
            List<SqlParameter> _lstParameters = new List<SqlParameter>();

            //call function to map object properties to SQL parameters for query execution

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@GEN_QueueId";
            sqlParam.SqlDbType = SqlDbType.BigInt;
            sqlParam.Value = objDOMacroUpdate.GEN_QueueId;
            _lstParameters.Add(sqlParam);           

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@Status";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = objDOMacroUpdate.Status;
            _lstParameters.Add(sqlParam);

            //sqlParam = new SqlParameter();
            //sqlParam.ParameterName = "@CurrentUser";
            //sqlParam.SqlDbType = SqlDbType.VarChar;
            //sqlParam.Value = objDOMacroUpdate.LoginID;
            //_lstParameters.Add(sqlParam);

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            _lstParameters.Add(sqlParam);

            long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_UPD_TRC155_Macro, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out long lRowsEffected, out string errorMessage);
            sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
            if (sqlParam != null && sqlParam.Value != null)
            {
                errormessage += sqlParam.Value.ToString();
            }
            if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
            {
                return ExceptionTypes.Success;
            }
            else
                return ExceptionTypes.UnknownError;
        }
    }
}
