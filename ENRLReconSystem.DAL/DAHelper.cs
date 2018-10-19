using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.Utility;
using System.Data;

namespace ENRLReconSystem.DAL
{
    public class DAHelper
    {
        private string ConnectionString;

        public DAHelper(string connectionstr)
        {
            ConnectionString = connectionstr;
        }
        public DAHelper()
        {
            try
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["ERSDbProvider"].ConnectionString;

            }
            catch
            {

            }
        }

        /// <summary>
        /// ExecuteSelectSP: Input parameters: Connection  string, stored procedure name, string spparameters[] -  stored procedure parameters in database parameters eg: �string�, 23, etc., output: dataset, error code, error number. 
        /// Action: execute the stored procedure which will return a dataset
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name</param>
        /// <param name="paramsForStoredProcedure">stored procedure parameters</param>
        /// <param name="resultData">output: dataset</param>
        /// <param name="errorCode">error code</param>
        /// <param name="errorNumber">error number</param>
        /// <returns></returns>
        public long ExecuteSelectSP(string storedProcedureName, SqlParameter[] spParameters, out DataSet resultData, out long errorCode, out long errorNumber, out string errorMessage)
        {
            long retValue = (long)ExceptionTypes.Success;
            errorCode = 0;
            errorNumber = 0;
            errorMessage = string.Empty;
            resultData = null;
            string strParameters = string.Empty;
            SqlCommand cmd = null;
            SqlConnection MyConnection = null;

            try
            {
                // Initializing dataset if it is null
                if (resultData == null)
                    resultData = new DataSet();

                // Initializing SqlConnection
                MyConnection = new SqlConnection(ConnectionString);

                // Initializing SqlDataAdapter
                SqlDataAdapter MyCommand = new SqlDataAdapter();

                // check for stored procedure name
                if (storedProcedureName.Trim().Equals(string.Empty))
                {
                    retValue = (long)ExceptionTypes.StoredProcedureNotFound;
                    //BOServerLog.Error(userDetails, "DAHelper.ExecuteDMLSP", "Please Provide Stored Procedure Name", retValue);
                    return retValue;
                }

                // Initializing SqlCommand
                cmd = new SqlCommand(storedProcedureName, MyConnection);

                //Set command execution time
                //cmd.CommandTimeout = WebConfigData.SelectSPTimeout;

                // Setting Command Type
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding Parameters to command object

                if (spParameters != null && spParameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    for (int k = 0; k < spParameters.Length; k++)
                    {
                        cmd.Parameters.Add(spParameters[k]);

                        // this is for error logging
                        if (spParameters[k].Value == null)
                            strParameters += spParameters[k].ParameterName + " = null,";
                        else
                            strParameters += spParameters[k].ParameterName + " = '" + spParameters[k].Value.ToString() + "',";
                    }
                }

                // Set Command object
                MyCommand.SelectCommand = cmd;

                // Fetch data from the database
                MyCommand.Fill(resultData);

                // Setting return value to ExceptionTypes.ZeroRecords incase of zero records 
                if (resultData.Tables.Count > 0 && resultData.Tables[0].Rows.Count == 0)
                {
                    retValue = (long)ExceptionTypes.ZeroRecords;
                }
            }
            catch (SqlException sqlex)
            {
                errorCode = sqlex.ErrorCode;
                errorNumber = sqlex.Number;

                // Checking for known errors
                try
                {
                    ExceptionTypes etKnownError = (ExceptionTypes)sqlex.Number;
                    retValue = sqlex.Number;
                }
                catch
                {
                    retValue = (long)ExceptionTypes.SqlException;
                }

                // eliminating the last comma
                if (strParameters != string.Empty && strParameters.EndsWith(","))
                    strParameters = strParameters.Substring(0, strParameters.Length - 1);

                errorMessage = sqlex.Message + " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), errorMessage + sqlex.Message, null);
            }
            catch (Exception ex)
            {
                retValue = (long)ExceptionTypes.UnknownError;

                // eliminating the last comma
                if (strParameters != string.Empty && strParameters.EndsWith(","))
                    strParameters = strParameters.Substring(0, strParameters.Length - 1);

                errorMessage = ex.Message + " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), ex.Message, ex);
            }
            finally
            {
                // Closing connection if it is opened
                if (MyConnection != null && MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
            //ServerLog.MethodExit("DAHelper.ExecuteSelectSP");
            //BOServerLog.MethodExit(userDetails, "DAHelper.ExecuteSelectSP");
            return retValue;
        }

        /// <summary>
        /// Execute SQL string, used to get data form bpo report data
        /// </summary>
        /// <param name="dsUserDetails"></param>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="resultData"></param>
        /// <param name="ErrorMessage"></param>
        /// <param name="isSelectSQL"></param>
        /// <returns></returns>
        public long ExecuteSQL(string sql, SqlParameter[] parameters, out DataSet resultData, out string ErrorMessage, bool isSelectSQL)
        {
            long retValue = (long)ExceptionTypes.Success;
            resultData = null;
            SqlConnection conn = null;
            ErrorMessage = string.Empty;

            try
            {
                // Initializing dataset if it is null
                if (resultData == null)
                    resultData = new DataSet();

                // Initializing SqlConnection
                conn = new SqlConnection(ConnectionString);

                if (isSelectSQL)
                {
                    // Initializing SqlDataAdapter
                    SqlDataAdapter cmd = new SqlDataAdapter(sql, conn);

                    if (parameters != null)
                        cmd.SelectCommand.Parameters.AddRange(parameters);

                    // Fetch data from the database

                    cmd.Fill(resultData);

                    // Setting return value to ExceptionTypes.ZeroRecords incase of zero records 
                    if (resultData != null && resultData.Tables.Count > 0)
                    {
                        if (resultData.Tables[0].Rows.Count == 0)
                        {
                            retValue = (long)ExceptionTypes.ZeroRecords;
                        }
                    }
                }
                else
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    //cmd.CommandTimeout = WebConfigData.OtherSQLCommandTimeout;
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        retValue = (long)ExceptionTypes.ZeroRecords;
                }
            }
            catch (SqlException sqlex)
            {
                // Checking for known errors
                try
                {
                    ExceptionTypes etKnownError = (ExceptionTypes)sqlex.Number;
                    retValue = sqlex.Number;
                }
                catch
                {
                    retValue = (long)ExceptionTypes.SqlException;
                }
                ErrorMessage = sqlex.Message + " Query: " + sql;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), ErrorMessage, sqlex);
            }
            catch (Exception ex)
            {
                retValue = (long)ExceptionTypes.DataException;
                ErrorMessage = ex.Message;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), ex.Message, ex);
            }
            finally
            {
                // Closing connection if it is opened
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return retValue;
        }

        /// <summary>
        /// Execute Scalar query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <param name="resultObj"></param>
        /// <param name="errorCode"></param>
        /// <param name="errorNumber"></param>
        /// <returns></returns>
        public long ExecuteScalar(string query, SqlParameter[] parameters, out object resultObj, out string ErrorMessage)
        {
            //ServerLog.MethodEntry("DAHelper.ExecuteScalar");
            long retValue = (long)ExceptionTypes.Success;
            ErrorMessage = string.Empty;
            SqlConnection MyConnection = null;
            resultObj = null;

            try
            {
                // Initializing SqlConnection
                MyConnection = new SqlConnection(ConnectionString);
                MyConnection.Open();

                SqlCommand cmd = new SqlCommand(query, MyConnection);
                //cmd.CommandTimeout = WebConfigData.OtherSQLCommandTimeout;
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                resultObj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();

            }
            catch (SqlException sqlex)
            {
                // Checking for known errors
                try
                {
                    ExceptionTypes etKnownError = (ExceptionTypes)sqlex.Number;
                    retValue = sqlex.Number;
                }
                catch
                {
                    retValue = (long)ExceptionTypes.SqlException;
                }
                ErrorMessage = sqlex.Message + " Query: " + query;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), ErrorMessage, sqlex);
            }
            catch (Exception ex)
            {
                retValue = (long)ExceptionTypes.DataException;
                ErrorMessage = ex.Message;
                //Utility.ErrorLog(user, MethodInfo.GetCurrentMethod(), ex.Message, ex);
            }
            finally
            {
                // Closing connection if it is opened
                if (MyConnection != null && MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
            return retValue;
        }

        /// <summary>
        /// ExecuteDMLSP: Input parameters: Connection string, stored procedure name, string[] spparameters[] – stored procedure parameters in database format, output: error code, error number.
        /// Action: execute the stored procedure and make the changes in the database
        /// </summary>
        /// <param name="storedProcedureName">stored procedure name</param>
        /// <param name="spParameters">stored procedure parameters</param>
        /// <param name="errorCode">error code</param>
        /// <param name="errorNumber">error number</param>
        /// <returns></returns>
        public long ExecuteDMLSP(string storedProcedureName, SqlParameter[] spParameters, out long errorCode, out long errorNumber, out long rowseffected, out string errorMessage)
        {
            errorMessage = "";
            rowseffected = 0;
            long retValue = (long)ExceptionTypes.Success;
            errorCode = 0;
            errorNumber = 0;
            string strParameters = string.Empty;
            SqlCommand cmd = null;
            SqlConnection MyConnection = null;
            try
            {
                // Initializing SqlConnection
                MyConnection = new SqlConnection(ConnectionString);

                // check for stored procedure name
                if (storedProcedureName.Trim().Equals(string.Empty))
                {
                    retValue = (long)ExceptionTypes.StoredProcedureNotFound;
                    return retValue;
                }

                // Initializing SqlCommand
                cmd = new SqlCommand(storedProcedureName, MyConnection);
                //cmd.CommandTimeout = WebConfigData.OtherSQLCommandTimeout;
                // Setting Command Type
                cmd.CommandType = CommandType.StoredProcedure;

                // Adding Parameters to command object
                if (spParameters.Length > 0)
                {
                    cmd.Parameters.Clear();
                    for (int k = 0; k < spParameters.Length; k++)
                    {
                        cmd.Parameters.Add(spParameters[k]);

                        // this is for error logging
                        if (spParameters[k].Value == null)
                            strParameters += spParameters[k].ParameterName + " - null,";
                        else
                            strParameters += spParameters[k].ParameterName + " - " + spParameters[k].Value.ToString() + ",";

                    }
                }

                string traceMessage = " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;

                // Open connection
                MyConnection.Open();

                // Execute command
                rowseffected = cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlex)
            {
                errorCode = sqlex.ErrorCode;
                errorNumber = sqlex.Number;

                // Checking for known errors
                try
                {
                    ExceptionTypes etKnownError = (ExceptionTypes)sqlex.Number;
                    retValue = sqlex.Number;
                }
                catch
                {
                    retValue = (long)ExceptionTypes.SqlException;
                }

                // eliminating the last comma
                if (strParameters != string.Empty && strParameters.EndsWith(","))
                    strParameters = strParameters.Substring(0, strParameters.Length - 1);

                //string errorMessage = sqlex.Message + " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;
                errorMessage = sqlex.Message + " Stach Trace: " + sqlex.StackTrace + " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;
                //BOServerLog.Error(userDetails, "DAHelper.ExecuteSelectSP", errorMessage, errorNumber, errorCode);

            }
            catch (Exception ex)
            {
                retValue = (long)ExceptionTypes.UnknownError;

                // eliminating the last comma
                if (strParameters != string.Empty && strParameters.EndsWith(","))
                    strParameters = strParameters.Substring(0, strParameters.Length - 1);

                errorMessage = ex.Message + " ~ Stored Procedure Name : " + storedProcedureName + " ~ Parameters : " + strParameters;

                //BOServerLog.Error(userDetails, "DAHelper.ExecuteSelectSP", errorMessage, retValue);
            }
            finally
            {
                // Closing connection if it is opened
                if (MyConnection != null && MyConnection.State == ConnectionState.Open)
                    MyConnection.Close();
            }
            //BOServerLog.MethodExit(userDetails, "DAHelper.ExecuteDMLSP");
            return retValue;
        }
    }
}
