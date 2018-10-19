using ENRLReconSystem.DO;
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
    public class DALSkills
    {
        public ExceptionTypes SearchSkills(long? TimeZone,DOADM_SkillsMaster objDOADM_SkillsDetails, out List<DOADM_SkillsMaster> lstDOADM_SkillsDetails, out string errorMessage)
        {
            lstDOADM_SkillsDetails = new List<DOADM_SkillsMaster>();
            objDOADM_SkillsDetails.lstDOADM_SkillWorkQueuesCorrelation = new List<DOADM_SkillWorkQueuesCorrelation>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objDOADM_SkillsDetails.ADM_SkillsMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_SkillsMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_SkillsDetails.ADM_SkillsMasterId;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_SkillsDetails.SkillsName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SkillsName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.SkillsName;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.RoleLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RoleLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.RoleLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.BusinessSegmentLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.BusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }
                if (objDOADM_SkillsDetails.DiscrepancyCategoryLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.DiscrepancyCategoryLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.CMN_DepartmentRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_DepartmentRef";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.CMN_DepartmentRef;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.WorkBasketLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkBasketLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.WorkBasketLkup;
                    parameters.Add(sqlParam);
                }

                //Uncomment after User search SP modofication

                //No Null check, IsActive key boolean key by defaut will be false and cannot be null
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOADM_SkillsDetails.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_Skills, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapSkillsDOSkillsMaster(TimeZone,dsResultData, out lstDOADM_SkillsDetails);
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
        public ExceptionTypes AddSkills(DOADM_SkillsMaster objDOADM_SkillsDetails,long lLoginId, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();

                List<SqlParameter> parameters = new List<SqlParameter>();
                long lErrorCode = 0;
                long lErrorNumber = 0;
                DataSet dsResultData = new DataSet();
                SqlParameter sqlParam;

                if (objDOADM_SkillsDetails.ADM_SkillsMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_SkillsMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_SkillsDetails.ADM_SkillsMasterId;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_SkillsDetails.SkillsName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SkillsName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.SkillsName;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.RoleLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RoleLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.RoleLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.BusinessSegmentLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.BusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }
                if (objDOADM_SkillsDetails.DiscrepancyCategoryLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.DiscrepancyCategoryLkup;
                    parameters.Add(sqlParam);
                }
                

                if (objDOADM_SkillsDetails.CMN_DepartmentRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_DepartmentRef";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.CMN_DepartmentRef;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_SkillsDetails.WorkBasketLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkBasketLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_SkillsDetails.WorkBasketLkup;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_SkillsDetails.IsActive.ToString()))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsActive";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_SkillsDetails.IsActive;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lLoginId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = ScreenType.Skills;
                parameters.Add(sqlParam);

                if(objDOADM_SkillsDetails.lstDOADM_SkillWorkQueuesCorrelation.Count() > 0)
                {
                    DataTable skillsWorkQueue;
                    SetSkillsWorkQueue(objDOADM_SkillsDetails.lstDOADM_SkillWorkQueuesCorrelation, out skillsWorkQueue);
                    DataTableReader dtrSkillWorkQueues = new DataTableReader(skillsWorkQueue);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TV_Queues";
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    sqlParam.Value = dtrSkillWorkQueues;
                    parameters.Add(sqlParam);
                }


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_UPD_Skills, parameters.ToArray(), out lErrorCode, out lErrorCode, out lErrorNumber, out errorMessage);

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

        private void SetSkillsWorkQueue(List<DOADM_SkillWorkQueuesCorrelation> lstDOADM_SkillWorkQueuesCorrelation, out DataTable skillsWorkQueue)
        {
            skillsWorkQueue = new DataTable("TVP_SkillWorkQueues");
            GetDataColumn(skillsWorkQueue);
            DataRow rowLetterTemplates;
            try
            {
                if(lstDOADM_SkillWorkQueuesCorrelation != null && lstDOADM_SkillWorkQueuesCorrelation.Count > 0)
                {
                    foreach(DOADM_SkillWorkQueuesCorrelation skillWorkQueue in lstDOADM_SkillWorkQueuesCorrelation)
                    {
                        rowLetterTemplates = skillsWorkQueue.NewRow();
                        rowLetterTemplates["ADM_SkillWorkQueuesCorrelationId"] = skillWorkQueue.ADM_SkillWorkQueuesCorrelationId;
                        rowLetterTemplates["ADM_SkillsMasterRef"] = skillWorkQueue.ADM_SkillsMasterRef;
                        //rowLetterTemplates["DiscrepancyCategoryLkup"] = skillWorkQueue.DiscrepancyCategoryLkup;
                        rowLetterTemplates["WorkQueuesLkup"] = skillWorkQueue.WorkQueuesLkup;
                        rowLetterTemplates["IsActive"] = skillWorkQueue.IsActive == false ?false:true;                        
                        skillsWorkQueue.Rows.Add(rowLetterTemplates);
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void GetDataColumn(DataTable skillsWorkQueue)
        {
            DataColumn dcLetterTemplates;
            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_SkillWorkQueuesCorrelationId";
            dcLetterTemplates.ReadOnly = true;
            skillsWorkQueue.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_SkillsMasterRef";
            dcLetterTemplates.ReadOnly = true;
            skillsWorkQueue.Columns.Add(dcLetterTemplates);
            
            //dcLetterTemplates = new DataColumn();
            //dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            //dcLetterTemplates.ColumnName = "DiscrepancyCategoryLkup";
            //dcLetterTemplates.ReadOnly = true;
            //skillsWorkQueue.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "WorkQueuesLkup";
            dcLetterTemplates.ReadOnly = true;
            skillsWorkQueue.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "IsActive";
            dcLetterTemplates.ReadOnly = true;
            skillsWorkQueue.Columns.Add(dcLetterTemplates);            
        }

        private void MapSkillsDOSkillsMaster(long? TimeZone,DataSet objDataTable, out List<DOADM_SkillsMaster> lstDOADM_SkillsDetails)
        {
            lstDOADM_SkillsDetails = new List<DOADM_SkillsMaster>();            
            try
            {
                foreach (DataRow dr in objDataTable.Tables[0].Rows)
                {
                    DOADM_SkillsMaster objDOADM_SkillsMaster = new DOADM_SkillsMaster();
                    if (dr.Table.Columns.Contains("ADM_SkillsMasterId"))
                    {
                        if (!DBNull.Value.Equals(dr["ADM_SkillsMasterId"]))
                        {
                            objDOADM_SkillsMaster.ADM_SkillsMasterId = (long)dr["ADM_SkillsMasterId"];
                        }
                    }
                    if (dr.Table.Columns.Contains("SkillsName"))
                    {
                        if (!DBNull.Value.Equals(dr["SkillsName"]))
                        {
                            objDOADM_SkillsMaster.SkillsName = dr["SkillsName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RoleLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["RoleLkup"]))
                        {
                            objDOADM_SkillsMaster.RoleLkup = (long)dr["RoleLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegmentLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegmentLkup"]))
                        {
                            objDOADM_SkillsMaster.BusinessSegmentLkup = (long)dr["BusinessSegmentLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"]))
                        {
                            objDOADM_SkillsMaster.DiscrepancyCategoryLkup = (long)dr["DiscrepancyCategoryLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("CMN_DepartmentRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CMN_DepartmentRef"]))
                        {
                            objDOADM_SkillsMaster.CMN_DepartmentRef = (long)dr["CMN_DepartmentRef"];
                        }
                    }
                    if (dr.Table.Columns.Contains("WorkBasketLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["WorkBasketLkup"]))
                        {
                            objDOADM_SkillsMaster.WorkBasketLkup = (long)dr["WorkBasketLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("IsActive"))
                    {
                        if (!DBNull.Value.Equals(dr["IsActive"]))
                        {
                            objDOADM_SkillsMaster.IsActive = Convert.ToBoolean(dr["IsActive"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objDOADM_SkillsMaster.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                        {
                            objDOADM_SkillsMaster.CreatedByRef = (long)dr["CreatedByRef"];
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objDOADM_SkillsMaster.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                        {
                            objDOADM_SkillsMaster.LastUpdatedByRef = (long)dr["LastUpdatedByRef"];
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByName"]))
                        {
                            objDOADM_SkillsMaster.CreatedByName = dr["CreatedByName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("RoleValue"))
                    {
                        if (!DBNull.Value.Equals(dr["RoleValue"]))
                        {
                            objDOADM_SkillsMaster.RoleValue = dr["RoleValue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegmentValue"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegmentValue"]))
                        {
                            objDOADM_SkillsMaster.BusinessSegmentValue = dr["BusinessSegmentValue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("WorkBasketValue"))
                    {
                        if (!DBNull.Value.Equals(dr["WorkBasketValue"]))
                        {
                            objDOADM_SkillsMaster.WorkBasketValue = dr["WorkBasketValue"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DepartmentName"))
                    {
                        if (!DBNull.Value.Equals(dr["DepartmentName"]))
                        {
                            objDOADM_SkillsMaster.DepartmentName = dr["DepartmentName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                        {
                            objDOADM_SkillsMaster.LastUpdatedByName = dr["LastUpdatedByName"].ToString();
                            
                        }
                    }
                    if (dr.Table.Columns.Contains("LockedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByRef"]))
                        {
                            objDOADM_SkillsMaster.LockedByRef = Convert.ToInt64(dr["LockedByRef"].ToString());

                        }
                    }

                    if (dr.Table.Columns.Contains("LockedByName"))
                    {
                        if (!DBNull.Value.Equals(dr["LockedByName"]))
                        {
                            objDOADM_SkillsMaster.LockedByName = dr["LockedByName"].ToString();

                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLockedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                        {
                            objDOADM_SkillsMaster.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                        }
                    }

                    objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation = new List<DOADM_SkillWorkQueuesCorrelation>();

                    if (objDataTable.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow sklwrkQ in objDataTable.Tables[1].Rows)
                        {
                            DOADM_SkillWorkQueuesCorrelation objDOADM_SkillWorkQueuesCorrelation = new DOADM_SkillWorkQueuesCorrelation();
                            if (sklwrkQ.Table.Columns.Contains("ADM_SkillWorkQueuesCorrelationId"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["ADM_SkillWorkQueuesCorrelationId"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.ADM_SkillWorkQueuesCorrelationId = (long)sklwrkQ["ADM_SkillWorkQueuesCorrelationId"];
                                }
                            }

                            if (sklwrkQ.Table.Columns.Contains("ADM_SkillsMasterRef"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["ADM_SkillsMasterRef"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.ADM_SkillsMasterRef = (long)sklwrkQ["ADM_SkillsMasterRef"];
                                }
                            }

                            if (sklwrkQ.Table.Columns.Contains("WorkQueuesLkup"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["WorkQueuesLkup"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.WorkQueuesLkup = (long)sklwrkQ["WorkQueuesLkup"];
                                }
                            }

                            if (sklwrkQ.Table.Columns.Contains("LastUpdatedByRef"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["LastUpdatedByRef"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByRef = (long)sklwrkQ["LastUpdatedByRef"];
                                }
                            }

                            if (sklwrkQ.Table.Columns.Contains("LastUpdatedByName"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["LastUpdatedByName"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.LastUpdatedByName = sklwrkQ["LastUpdatedByName"].ToString();
                                }
                            }

                            if (sklwrkQ.Table.Columns.Contains("CreatedByName"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["CreatedByName"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.CreatedByName = sklwrkQ["CreatedByName"].ToString();
                                }
                            }

                            if (dr.Table.Columns.Contains("UTCCreatedOn"))
                            {
                                if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(sklwrkQ["UTCCreatedOn"].ToDateTime(),TimeZone);
                                }
                            }

                            if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                            {
                                if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(sklwrkQ["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                                }
                            }

                            //if (sklwrkQ.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                            //{
                            //    if (!DBNull.Value.Equals(sklwrkQ["DiscrepancyCategoryLkup"]))
                            //    {
                            //        objDOADM_SkillWorkQueuesCorrelation.DiscrepancyCategoryLkup = (long)sklwrkQ["DiscrepancyCategoryLkup"];
                            //    }
                            //}

                            if (sklwrkQ.Table.Columns.Contains("IsActive"))
                            {
                                if (!DBNull.Value.Equals(sklwrkQ["IsActive"]))
                                {
                                    objDOADM_SkillWorkQueuesCorrelation.IsActive = (bool)sklwrkQ["IsActive"];
                                }
                            }

                            objDOADM_SkillsMaster.lstDOADM_SkillWorkQueuesCorrelation.Add(objDOADM_SkillWorkQueuesCorrelation);
                        }
                    }
                    lstDOADM_SkillsDetails.Add(objDOADM_SkillsMaster);                    
                }                
            }
            catch
            {

            }
        }
    }
}
