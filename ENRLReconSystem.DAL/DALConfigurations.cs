using System;
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
    public class DALConfigurations
    {
        // Save Configuration Master
        public ExceptionTypes SaveConfigMaster(DOMGR_ConfigMaster objDOMGR_ConfigMaster, out string errorMessage)
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
                sqlParam.ParameterName = "@ConfigId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOMGR_ConfigMaster.MGR_ConfigMasterId;
                parameters.Add(sqlParam);

                if (!string.IsNullOrEmpty(objDOMGR_ConfigMaster.ConfigName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ConfigName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMGR_ConfigMaster.ConfigName;
                    parameters.Add(sqlParam);
                }
                if (!string.IsNullOrEmpty(objDOMGR_ConfigMaster.ConfigValue))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ConfigValue";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMGR_ConfigMaster.ConfigValue;
                    parameters.Add(sqlParam);
                }
                if (objDOMGR_ConfigMaster.StartDate != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@StartDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMGR_ConfigMaster.StartDate;
                    parameters.Add(sqlParam);
                }
                if (objDOMGR_ConfigMaster.EndDate != null)
                {

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EndDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMGR_ConfigMaster.EndDate;
                    parameters.Add(sqlParam);
                }               

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOMGR_ConfigMaster.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOMGR_ConfigMaster.CreatedByRef;
                parameters.Add(sqlParam);

                //Extra parameter when adding or editing record for releasing lock
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (long)ScreenType.Configuration;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;               
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.USP_APP_INS_UPD_ConfigMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
               
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

        public void MapConfigMstDetails(long? TimeZone,DataTable objDataTable, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster)
        {
            lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();

            try
            {
                foreach (DataRow dr in objDataTable.Rows)
                {
                    DOMGR_ConfigMaster objDOMGR_ConfigMaster = new DOMGR_ConfigMaster();
                    if (dr.Table.Columns.Contains("MGR_ConfigMasterId"))
                    {
                        if (!DBNull.Value.Equals(dr["MGR_ConfigMasterId"]))
                        {
                            objDOMGR_ConfigMaster.MGR_ConfigMasterId = Convert.ToInt32(dr["MGR_ConfigMasterId"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ConfigName"))
                    {
                        if (!DBNull.Value.Equals(dr["ConfigName"]))
                        {
                            objDOMGR_ConfigMaster.ConfigName = dr["ConfigName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ConfigValue"))
                    {
                        if (!DBNull.Value.Equals(dr["ConfigValue"]))
                        {
                            objDOMGR_ConfigMaster.ConfigValue = dr["ConfigValue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("StartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["StartDate"]))
                        {
                            objDOMGR_ConfigMaster.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("EndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EndDate"]))
                        {
                            objDOMGR_ConfigMaster.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        }
                    }                   
                    if (dr.Table.Columns.Contains("IsActive"))
                    {
                        if (!DBNull.Value.Equals(dr["IsActive"]))
                        {
                            if (dr["IsActive"].ToString() == "True")
                                objDOMGR_ConfigMaster.IsActive = true;
                            if (dr["IsActive"].ToString() == "False")
                                objDOMGR_ConfigMaster.IsActive = false;
                        }
                    }

                    if (dr.Table.Columns.Contains("LockedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByRef"]))
                        {
                            objDOMGR_ConfigMaster.LockedByRef = (long)(dr["LockedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objDOMGR_ConfigMaster.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByName"]))
                        {
                            objDOMGR_ConfigMaster.CreatedByName = Convert.ToString(dr["CreatedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objDOMGR_ConfigMaster.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                        {
                            objDOMGR_ConfigMaster.LastUpdatedByName = Convert.ToString(dr["LastUpdatedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByName"]))
                        {
                            objDOMGR_ConfigMaster.LockedByName = Convert.ToString(dr["LockedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLockedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                        {
                            objDOMGR_ConfigMaster.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    lstDOMGR_ConfigMaster.Add(objDOMGR_ConfigMaster);
                }
            }
            catch
            {

            }
        }
        // search configuration master
        public ExceptionTypes SearchConfiguration(long? TimeZone,DOMGR_ConfigMaster objDOMGR_ConfigMaster, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster, out string errorMessage)
        {
            lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;               

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ConfigName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOMGR_ConfigMaster.ConfigName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOMGR_ConfigMaster.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.USP_APP_SEL_ConfigMaster, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapConfigMstDetails(TimeZone,dsResultData.Tables[0], out lstDOMGR_ConfigMaster);
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

        public ExceptionTypes SearchCOnfigurationID(long? TimeZone,DOMGR_ConfigMaster configMaster, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster, out string errorMessage)
        {
            lstDOMGR_ConfigMaster = new List<DOMGR_ConfigMaster>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MGR_ConfigMasterId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = configMaster.MGR_ConfigMasterId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = configMaster.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.USP_APP_SEL_ConfigMaster, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapConfigMstDetails(TimeZone,dsResultData.Tables[0], out lstDOMGR_ConfigMaster);
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
    }
}
