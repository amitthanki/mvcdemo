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
    public class DALDepartment
    {
        //save Department Master
        public ExceptionTypes SaveDepartment(DOCMN_Department objDOCMN_Department, out string errorMessage)
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
                    sqlParam.ParameterName = "@CMN_DepartmentId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_Department.CMN_DepartmentId;
                    parameters.Add(sqlParam);
              
                if (!string.IsNullOrEmpty(objDOCMN_Department.ERSDepartmentName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ERSDepartmentName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOCMN_Department.ERSDepartmentName;
                    parameters.Add(sqlParam);
                }
             
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_Department.BusinessSegmentLkup;
                    parameters.Add(sqlParam);  
              
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DepartmentLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOCMN_Department.DepartmentLkup;
                    parameters.Add(sqlParam);

                if (objDOCMN_Department.EffectiveDate != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EffectiveDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOCMN_Department.EffectiveDate;
                    parameters.Add(sqlParam);
                }
                if (objDOCMN_Department.InactivationDate != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@InactivationDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOCMN_Department.InactivationDate;
                    parameters.Add(sqlParam);
                }            

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_Department.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_Department.CreatedByRef;
                parameters.Add(sqlParam);

                //Extra parameter when adding or editing record for releasing lock
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (long)ScreenType.Department;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                //if (objDOADM_UserMaster.ADM_UserMasterId == 0)
                //{
                    executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_UPD_Department, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                //}
                //else
                //{
                //    executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_ADM_UserMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                //}

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

        public void MapDepartmentDetails(long? TimeZone,DataTable objDataTable, out List<DOCMN_Department> lstDOCMN_Department)
        {
            lstDOCMN_Department = new List<DOCMN_Department>();

            try
            {
                foreach (DataRow dr in objDataTable.Rows)
                {
                    DOCMN_Department objDOCMN_Department = new DOCMN_Department();
                    if (dr.Table.Columns.Contains("CMN_DepartmentId"))
                    {
                        if (!DBNull.Value.Equals(dr["CMN_DepartmentId"]))
                        {
                            objDOCMN_Department.CMN_DepartmentId = Convert.ToInt32(dr["CMN_DepartmentId"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ERSDepartmentName"))
                    {
                        if (!DBNull.Value.Equals(dr["ERSDepartmentName"]))
                        {
                            objDOCMN_Department.ERSDepartmentName = dr["ERSDepartmentName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("EffectiveDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EffectiveDate"]))
                        {
                            objDOCMN_Department.EffectiveDate = Convert.ToDateTime(dr["EffectiveDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("InactivationDate"))
                    {
                        if (!DBNull.Value.Equals(dr["InactivationDate"]))
                        {
                            objDOCMN_Department.InactivationDate = Convert.ToDateTime(dr["InactivationDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("InactivationDate"))
                    {
                        if (!DBNull.Value.Equals(dr["InactivationDate"]))
                        {
                            objDOCMN_Department.InactivationDate = Convert.ToDateTime(dr["InactivationDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegmentLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegmentLkup"]))
                        {
                            objDOCMN_Department.BusinessSegmentLkup = (long)(dr["BusinessSegmentLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("DepartmentLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DepartmentLkup"]))
                        {
                            objDOCMN_Department.DepartmentLkup = (long)(dr["DepartmentLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("IsActive"))
                    {
                        if (!DBNull.Value.Equals(dr["IsActive"]))
                        {
                            if (dr["IsActive"].ToString() == "True")
                                objDOCMN_Department.IsActive = true;
                            if (dr["IsActive"].ToString() == "False")
                                objDOCMN_Department.IsActive = false;
                        }
                    }

                    if (dr.Table.Columns.Contains("LockedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByRef"]))
                        {
                            objDOCMN_Department.LockedByRef = (long)(dr["LockedByRef"]);
                        }
                    }

                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objDOCMN_Department.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByName"]))
                        {
                            objDOCMN_Department.CreatedByName = Convert.ToString(dr["CreatedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objDOCMN_Department.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                        {
                            objDOCMN_Department.LastUpdatedByName = Convert.ToString(dr["LastUpdatedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByName"]))
                        {
                            objDOCMN_Department.LockedByName = Convert.ToString(dr["LockedByName"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLockedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                        {
                            objDOCMN_Department.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                        }
                    }


                    lstDOCMN_Department.Add(objDOCMN_Department);
                }
            }
            catch
            {

            }
        }
        // Search Department 
        public ExceptionTypes SearchDepartment(long? TimeZone,DOCMN_Department objDOCMN_Department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            lstDOCMN_Department = new List<DOCMN_Department>();
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
                    sqlParam.ParameterName = "@ERSDepartmentName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOCMN_Department.ERSDepartmentName;
                    parameters.Add(sqlParam);
               
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsActive";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOCMN_Department.IsActive;
                    parameters.Add(sqlParam);
                               
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ErrorMessage";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = string.Empty;
                    sqlParam.Direction = ParameterDirection.Output;
                    sqlParam.Size = 2000;
                    parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_Department, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapDepartmentDetails(TimeZone,dsResultData.Tables[0], out lstDOCMN_Department);
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

        public ExceptionTypes CheckDuplicateDep(long? TimeZone,DOCMN_Department objDOCMN_Department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            lstDOCMN_Department = new List<DOCMN_Department>();
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
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_Department.BusinessSegmentLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DepartmentLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_Department.DepartmentLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_Department, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapDepartmentDetails(TimeZone,dsResultData.Tables[0], out lstDOCMN_Department);
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

        //Search Department By Department ID
        public ExceptionTypes SearchDepartmentById(long? TimeZone,DOCMN_Department department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            lstDOCMN_Department = new List<DOCMN_Department>();
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
                sqlParam.ParameterName = "@CMN_DepartmentId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = department.CMN_DepartmentId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = department.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_Department, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapDepartmentDetails(TimeZone,dsResultData.Tables[0], out lstDOCMN_Department);
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
