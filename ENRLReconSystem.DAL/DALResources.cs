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
    public class DALResources
    {
        public ExceptionTypes SearchResources(long? TimeZone,DOADM_ResourceDetails objDOADM_ResourceDetails, out List<DOADM_ResourceDetails> lstDOADM_ResourceDetails, out string errorMessage)
        {
            lstDOADM_ResourceDetails = new List<DOADM_ResourceDetails>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                parameters = MapResourceDetailsResourceDO(objDOADM_ResourceDetails);
                //Is it required
                SqlParameter sqlParam= new SqlParameter();
                sqlParam.ParameterName = "@ConsiderDates";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOADM_ResourceDetails.ConsiderDates;
                parameters.Add(sqlParam); 

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_ERS_APP_SEL_ResourceDetails, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        //call function to map dataset result to object properties
                        MapResourceDOResourceDetails(TimeZone,dsResultData.Tables[0], out lstDOADM_ResourceDetails);
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

        public ExceptionTypes SaveResource(DOADM_ResourceDetails objDOADM_ResourceDetails, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                parameters = MapResourceDetailsResourceDO(objDOADM_ResourceDetails);

                //Extra parameter when adding or editing record for releasing lock
                //not needed when searching records
                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = (long)ScreenType.Resources;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.USP_APP_INS_UPD_ResourceDetails, parameters.ToArray(), out lErrorNumber, out lErrocode, out lErrorNumber, out errorMessage);
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
        private List<SqlParameter> MapResourceDetailsResourceDO(DOADM_ResourceDetails objDOADM_ResourceDetails)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            if (objDOADM_ResourceDetails.ADM_ResourceDetailsId != 0)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ADM_ResourceDetailsId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_ResourceDetails.ADM_ResourceDetailsId;
                parameters.Add(sqlParam);
            }

            if (!string.IsNullOrEmpty(objDOADM_ResourceDetails.ResourceName))
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResourceName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOADM_ResourceDetails.ResourceName;
                parameters.Add(sqlParam);
            }

            if (!string.IsNullOrEmpty(objDOADM_ResourceDetails.ResourceDescription))
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResourceDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOADM_ResourceDetails.ResourceDescription;
                parameters.Add(sqlParam);
            }

            if (!string.IsNullOrEmpty(objDOADM_ResourceDetails.ResourceLinkLocation))
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResourceLinkLocation";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOADM_ResourceDetails.ResourceLinkLocation;
                parameters.Add(sqlParam);
            }

            if (objDOADM_ResourceDetails.ResourceEffectiveDate != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResourceEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOADM_ResourceDetails.ResourceEffectiveDate;
                parameters.Add(sqlParam);
            }

            if (objDOADM_ResourceDetails.ResourceInactivationDate != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResourceInactivationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOADM_ResourceDetails.ResourceInactivationDate;
                parameters.Add(sqlParam);
            }

            if (objDOADM_ResourceDetails.CMN_DepartmentRef != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_DepartmentRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_ResourceDetails.CMN_DepartmentRef;
                parameters.Add(sqlParam);
            }

            if (objDOADM_ResourceDetails.LoginUserId != 0)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_ResourceDetails.LoginUserId;
                parameters.Add(sqlParam);
            }

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@IsActive";
            sqlParam.SqlDbType = SqlDbType.Bit;
            sqlParam.Value = objDOADM_ResourceDetails.IsActive;
            parameters.Add(sqlParam);

           

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@ErrorMessage";
            sqlParam.SqlDbType = SqlDbType.VarChar;
            sqlParam.Value = string.Empty;
            sqlParam.Direction = ParameterDirection.Output;
            sqlParam.Size = 2000;
            parameters.Add(sqlParam);

            return parameters;
        }

        //function to map SQL parameters to object properties
        private void MapResourceDOResourceDetails(long? TimeZone,DataTable objDataTable, out List<DOADM_ResourceDetails> lstDOADM_ResourceDetails)
        {
            lstDOADM_ResourceDetails = new List<DOADM_ResourceDetails>();
            foreach (DataRow dr in objDataTable.Rows)
            {
                DOADM_ResourceDetails objDOADM_ResourceDetails = new DOADM_ResourceDetails();
                if (dr.Table.Columns.Contains("ADM_ResourceDetailsId"))
                {
                    if (!DBNull.Value.Equals(dr["ADM_ResourceDetailsId"]))
                    {
                        objDOADM_ResourceDetails.ADM_ResourceDetailsId = (long)dr["ADM_ResourceDetailsId"];
                    }
                }
                if (dr.Table.Columns.Contains("LockedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["LockedByRef"]))
                    {
                        objDOADM_ResourceDetails.LockedByRef = (long)dr["LockedByRef"];
                    }
                }
                if (dr.Table.Columns.Contains("UTCLockedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                    {
                        objDOADM_ResourceDetails.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                    }
                }
                if (dr.Table.Columns.Contains("ResourceName"))
                {
                    if (!DBNull.Value.Equals(dr["ResourceName"]))
                    {
                        objDOADM_ResourceDetails.ResourceName = dr["ResourceName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("ResourceDescription"))
                {
                    if (!DBNull.Value.Equals(dr["ResourceDescription"]))
                    {
                        objDOADM_ResourceDetails.ResourceDescription = dr["ResourceDescription"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("ResourceLinkLocation"))
                {
                    if (!DBNull.Value.Equals(dr["ResourceLinkLocation"]))
                    {
                        objDOADM_ResourceDetails.ResourceLinkLocation = dr["ResourceLinkLocation"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("ResourceEffectiveDate"))
                {
                    if (!DBNull.Value.Equals(dr["ResourceEffectiveDate"]))
                    {
                        objDOADM_ResourceDetails.ResourceEffectiveDate = Convert.ToDateTime(dr["ResourceEffectiveDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("ResourceInactivationDate"))
                {
                    if (!DBNull.Value.Equals(dr["ResourceInactivationDate"]))
                    {
                        objDOADM_ResourceDetails.ResourceInactivationDate = Convert.ToDateTime(dr["ResourceInactivationDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("CMN_DepartmentRef"))
                {
                    if (!DBNull.Value.Equals(dr["CMN_DepartmentRef"]))
                    {
                        objDOADM_ResourceDetails.CMN_DepartmentRef = (long)dr["CMN_DepartmentRef"];
                    }
                }
                if (dr.Table.Columns.Contains("IsActive"))
                {
                    if (!DBNull.Value.Equals(dr["IsActive"]))
                    {
                        objDOADM_ResourceDetails.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
                if (dr.Table.Columns.Contains("UTCCreatedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                    {
                        objDOADM_ResourceDetails.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                    }
                }
                if (dr.Table.Columns.Contains("CreatedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                    {
                        objDOADM_ResourceDetails.CreatedByRef = (long)dr["CreatedByRef"];
                    }
                }
                if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                    {
                        objDOADM_ResourceDetails.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                    }
                }
                if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                    {
                        objDOADM_ResourceDetails.LastUpdatedByRef = (long)dr["LastUpdatedByRef"];
                    }
                }

                //Ref key names
                if (dr.Table.Columns.Contains("CreatedByName"))
                {
                    if (!DBNull.Value.Equals(dr["CreatedByName"]))
                    {
                        objDOADM_ResourceDetails.CreatedByName = dr["CreatedByName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("CMN_DepartmentValue"))
                {
                    if (!DBNull.Value.Equals(dr["CMN_DepartmentValue"]))
                    {
                        objDOADM_ResourceDetails.CMN_DepartmentValue = dr["CMN_DepartmentValue"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("LockedByName"))
                {
                    if (!DBNull.Value.Equals(dr["LockedByName"]))
                    {
                        objDOADM_ResourceDetails.LockedByName = dr["LockedByName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("LastUpdatedByName"))
                {
                    if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                    {
                        objDOADM_ResourceDetails.LastUpdatedByName = dr["LastUpdatedByName"].ToString();
                    }
                }
                lstDOADM_ResourceDetails.Add(objDOADM_ResourceDetails);
                //sort list by resource names
                lstDOADM_ResourceDetails = lstDOADM_ResourceDetails.OrderBy(x => x.ResourceName).ToList();
            }
        }
    }
}
