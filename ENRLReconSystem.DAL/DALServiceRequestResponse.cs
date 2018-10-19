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
    public class DALServiceRequestResponse
    {
        DAHelper _objDAHelper = new DAHelper();

        public ExceptionTypes InsertAEGPSServiceTrace(DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace)
        {

            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            string errorMessage = string.Empty;
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.GEN_QueueRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WebServiceMethodLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WebServiceMethodName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.WebServiceMethodName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestData";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.RequestData;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResponseData";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.ResponseData;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CreatedByRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.CreatedByRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Statuslkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_AEGPSServiceTrace.StatusLkup;
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@ErrorMessage";
                //sqlParam.SqlDbType = SqlDbType.VarChar;
                //sqlParam.Value = string.Empty;
                //sqlParam.Direction = ParameterDirection.Output;
                //sqlParam.Size = 2000;
                //parameters.Add(sqlParam);

                long executionResult = 0;

                executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_GEN_AEGPSServiceTrace, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes InsertMacroServiceTrace(DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace)
        {

            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            string errorMessage = string.Empty;
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MacroServiceTrace.GEN_QueueRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MacroServiceMethodLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MacroServiceTrace.MacroServiceMethodLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MacroServiceMethodName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MacroServiceTrace.MacroServiceMethodName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestData";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MacroServiceTrace.RequestData;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResponseData";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MacroServiceTrace.ResponseData;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MacroServiceTrace.CreatedByRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Statuslkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MacroServiceTrace.StatusLkup;
                parameters.Add(sqlParam);

              
                long executionResult = 0;

                executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_GEN_MacroServiceTrace, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes MIIMServiceLog(DOGEN_MIIMServiceTrace objDOGEN_MIIMServiceTrace)
        {
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();

            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            string errorMessage = string.Empty;
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WebServiceMethodLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.WebServiceMethodLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WebServiceMethodName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.WebServiceMethodName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TarceMethodLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.TarceMethodLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RequestInputData";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.RequestInputData;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResponseStatusMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.ResponseStatusMessage;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoggedInUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_MIIMServiceTrace.CreatedByRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;

                executionResult = executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_GEN_MIIMServiceTrace, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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
                return ExceptionTypes.UnknownError;
            }
        }
    }
}

