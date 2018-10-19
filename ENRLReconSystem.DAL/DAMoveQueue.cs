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
    public class DAMoveQueue
    {
        DAHelper _daHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        public DAMoveQueue()
        {

        }

        public ExceptionTypes DProcessMoveQueue(string executeSp,out string errorMessage)
        {
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            try
            {
                _lstParameters = new List<SqlParameter>();
                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@ErrorMessage";
                //sqlParam.SqlDbType = SqlDbType.VarChar;
                //sqlParam.Value = string.Empty;
                //sqlParam.Direction = ParameterDirection.Output;
                //sqlParam.Size = 2000;
                //_lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(executeSp, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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

        public ExceptionTypes DProcessQueueMoveforMacro(long MacroType, long LoginUserID, string constSPName, out string errorMessage)
        {
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MacroTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = MacroType;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = LoginUserID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(constSPName, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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

        public bool UnlockRecords(out string errorMessage)
        {
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            try
            {
                _lstParameters = new List<SqlParameter>();
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_AutoUnlockRecords, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                _lstParameters = null;
            }
        }
    }
}
