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
    public class DALAlerts
    {
        public ExceptionTypes SearchAlerts(long? TimeZone,DOADM_AlertDetails objDOADM_AlertDetails, out List<DOADM_AlertDetails> lstDOADM_AlertDetails, out string errorMessage)
        {
            lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();

                //call function to map object properties to SQL parameters for query execution
                parameters = MapAlertDetailsAlertDO(objDOADM_AlertDetails);
                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ConsiderDates";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOADM_AlertDetails.ConsiderDates;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.USP_APP_SEL_ADM_AlertDetails, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        //call function to map dataset result to object properties
                        MapAlertDOAlertDetails(TimeZone,dsResultData.Tables[0], out lstDOADM_AlertDetails);
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

        public ExceptionTypes SaveAlert(DOADM_AlertDetails objDOADM_AlertDetails, out string errorMessage)
        {

            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                
                //call function to map object properties to SQL parameters for query execution
                parameters = MapAlertDetailsAlertDO(objDOADM_AlertDetails);

                //Extra parameter when adding or editing record for releasing lock
                //not needed when searching records
                SqlParameter sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = (long)ScreenType.Alerts;
                parameters.Add(sqlParam);


                long executionResult = dah.ExecuteDMLSP(ConstantTexts.USP_APP_INS_UPD_AlertsDetails, parameters.ToArray(), out lErrocode, out lErrocode, out lErrorNumber, out errorMessage);
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
        private List<SqlParameter> MapAlertDetailsAlertDO(DOADM_AlertDetails objDOADM_AlertDetails)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            SqlParameter sqlParam;

            if (objDOADM_AlertDetails.ADM_AlertDetailsId != 0)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ADM_AlertDetailsId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.ADM_AlertDetailsId;
                parameters.Add(sqlParam);
            }

            if (!string.IsNullOrEmpty(objDOADM_AlertDetails.AlertTitle))
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertTitle";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOADM_AlertDetails.AlertTitle;
                parameters.Add(sqlParam);
            }

            if (!string.IsNullOrEmpty(objDOADM_AlertDetails.AlertDescription))
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOADM_AlertDetails.AlertDescription;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.AlertPublishedDate != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertPublishedDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOADM_AlertDetails.AlertPublishedDate;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.AlertEffectiveDate != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOADM_AlertDetails.AlertEffectiveDate;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.AlertInactivationDate != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertInactivationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOADM_AlertDetails.AlertInactivationDate;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.AlertCriticalityLkup != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AlertCriticalityLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.AlertCriticalityLkup;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.SendAlertToLkup != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SendAlertToLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.SendAlertToLkup;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.CMN_DepartmentRef != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_DepartmentRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.CMN_DepartmentRef;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.ADM_UserMasterRef != null)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ADM_UserMasterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.ADM_UserMasterRef;
                parameters.Add(sqlParam);
            }

            if (objDOADM_AlertDetails.LoginUserId != 0)
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_AlertDetails.LoginUserId;
                parameters.Add(sqlParam);
            }

            sqlParam = new SqlParameter();
            sqlParam.ParameterName = "@IsActive";
            sqlParam.SqlDbType = SqlDbType.Bit;
            sqlParam.Value = objDOADM_AlertDetails.IsActive;
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
        private void MapAlertDOAlertDetails(long? TimeZone,DataTable objDataTable, out List<DOADM_AlertDetails> lstDOADM_AlertDetails)
        {
            lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
            foreach (DataRow dr in objDataTable.Rows)
            {
                DOADM_AlertDetails objDOADM_AlertDetails = new DOADM_AlertDetails();
                if (dr.Table.Columns.Contains("ADM_AlertDetailsId"))
                {
                    if (!DBNull.Value.Equals(dr["ADM_AlertDetailsId"]))
                    {
                        objDOADM_AlertDetails.ADM_AlertDetailsId = (long)dr["ADM_AlertDetailsId"];
                    }
                }
                if (dr.Table.Columns.Contains("LockedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["LockedByRef"]))
                    {
                        objDOADM_AlertDetails.LockedByRef = (long)dr["LockedByRef"];
                    }
                }
                if (dr.Table.Columns.Contains("UTCLockedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                    {
                        objDOADM_AlertDetails.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                    }
                }
                if (dr.Table.Columns.Contains("AlertTitle"))
                {
                    if (!DBNull.Value.Equals(dr["AlertTitle"]))
                    {
                        objDOADM_AlertDetails.AlertTitle = dr["AlertTitle"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("AlertDescription"))
                {
                    if (!DBNull.Value.Equals(dr["AlertDescription"]))
                    {
                        objDOADM_AlertDetails.AlertDescription = dr["AlertDescription"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("AlertPublishedDate"))
                {
                    if (!DBNull.Value.Equals(dr["AlertPublishedDate"]))
                    {
                        objDOADM_AlertDetails.AlertPublishedDate = Convert.ToDateTime(dr["AlertPublishedDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("AlertEffectiveDate"))
                {
                    if (!DBNull.Value.Equals(dr["AlertEffectiveDate"]))
                    {
                        objDOADM_AlertDetails.AlertEffectiveDate = Convert.ToDateTime(dr["AlertEffectiveDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("AlertInactivationDate"))
                {
                    if (!DBNull.Value.Equals(dr["AlertInactivationDate"]))
                    {
                        objDOADM_AlertDetails.AlertInactivationDate = Convert.ToDateTime(dr["AlertInactivationDate"]);
                    }
                }
                if (dr.Table.Columns.Contains("IsActive"))
                {
                    if (!DBNull.Value.Equals(dr["IsActive"]))
                    {
                        objDOADM_AlertDetails.IsActive = Convert.ToBoolean(dr["IsActive"]);
                    }
                }
                if (dr.Table.Columns.Contains("UTCCreatedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                    {
                        objDOADM_AlertDetails.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                    }
                }
                if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                {
                    if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                    {
                        objDOADM_AlertDetails.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                    }
                }

                //Ref Keys
                if (dr.Table.Columns.Contains("CMN_DepartmentRef"))
                {
                    if (!DBNull.Value.Equals(dr["CMN_DepartmentRef"]))
                    {
                        objDOADM_AlertDetails.CMN_DepartmentRef = (long)dr["CMN_DepartmentRef"];
                    }
                }
                if (dr.Table.Columns.Contains("AlertCriticalityLkup"))
                {
                    if (!DBNull.Value.Equals(dr["AlertCriticalityLkup"]))
                    {
                        objDOADM_AlertDetails.AlertCriticalityLkup = (long)dr["AlertCriticalityLkup"];
                    }
                }
                if (dr.Table.Columns.Contains("SendAlertToLkup"))
                {
                    if (!DBNull.Value.Equals(dr["SendAlertToLkup"]))
                    {
                        objDOADM_AlertDetails.SendAlertToLkup = (long)dr["SendAlertToLkup"];
                    }
                }
                if (dr.Table.Columns.Contains("ADM_UserMasterRef"))
                {
                    if (!DBNull.Value.Equals(dr["ADM_UserMasterRef"]))
                    {
                        objDOADM_AlertDetails.ADM_UserMasterRef = (long)dr["ADM_UserMasterRef"];
                    }
                }
                if (dr.Table.Columns.Contains("CreatedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                    {
                        objDOADM_AlertDetails.CreatedByRef = (long)dr["CreatedByRef"];
                    }
                }
                if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                {
                    if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                    {
                        objDOADM_AlertDetails.LastUpdatedByRef = (long)dr["LastUpdatedByRef"];
                    }
                }

                //Ref key values
                if (dr.Table.Columns.Contains("CreatedByName"))
                {
                    if (!DBNull.Value.Equals(dr["CreatedByName"]))
                    {
                        objDOADM_AlertDetails.CreatedByName = dr["CreatedByName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("DepartmentName"))
                {
                    if (!DBNull.Value.Equals(dr["DepartmentName"]))
                    {
                        objDOADM_AlertDetails.DepartmentName = dr["DepartmentName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("LockedByName"))
                {
                    if (!DBNull.Value.Equals(dr["LockedByName"]))
                    {
                        objDOADM_AlertDetails.LockedByName = dr["LockedByName"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("AlertCriticalityValue"))
                {
                    if (!DBNull.Value.Equals(dr["AlertCriticalityValue"]))
                    {
                        objDOADM_AlertDetails.AlertCriticalityValue = dr["AlertCriticalityValue"].ToString();
                    }
                }
                if (dr.Table.Columns.Contains("SendAlertToValue"))
                {
                    if (!DBNull.Value.Equals(dr["SendAlertToValue"]))
                    {
                        objDOADM_AlertDetails.SendAlertToValue = dr["SendAlertToValue"].ToString();
                    }
                }
                {
                    if (!DBNull.Value.Equals(dr["IndividualUserName"]))
                    {
                        objDOADM_AlertDetails.IndividualUserName = dr["IndividualUserName"].ToString();
                    }
                }
                {
                    if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                    {
                        objDOADM_AlertDetails.LastUpdatedByName = dr["LastUpdatedByName"].ToString();
                    }
                }
                lstDOADM_AlertDetails.Add(objDOADM_AlertDetails);
                //sort list by Alert Titles
                lstDOADM_AlertDetails = lstDOADM_AlertDetails.OrderBy(x => x.AlertTitle).ToList();
            }
        }
    }
}
