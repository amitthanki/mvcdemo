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
    public class DALAccessGroup
    {
        public ExceptionTypes GetAccessGroupBasedOnSearch(long? TimeZone,DOADM_AccessGroupMaster objDOADM_AccessGroupMaster, out List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster)
        {
            lstDOADM_AccessGroupMaster = new List<DOADM_AccessGroupMaster>();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId;
                    parameters.Add(sqlParam);
                }
                if (!string.IsNullOrEmpty(objDOADM_AccessGroupMaster.AccessGroupName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_AccessGroupMaster.AccessGroupName;
                    parameters.Add(sqlParam);
                }
                if (!string.IsNullOrEmpty(objDOADM_AccessGroupMaster.AccessGroupDescription))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupDescription";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_AccessGroupMaster.AccessGroupDescription;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_AccessGroupMaster.IsActive != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsActive";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_AccessGroupMaster.IsActive;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_AccesssGroup, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapObjectProperties(TimeZone,dsTable, out lstDOADM_AccessGroupMaster);
                        if (lstDOADM_AccessGroupMaster.Count > 0)
                            return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
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

        public void MapObjectProperties(long? TimeZone,DataSet dsTable, out List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster)
        {
            lstDOADM_AccessGroupMaster = new List<DOADM_AccessGroupMaster>();
            try
            {
                if (dsTable.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsTable.Tables[0].Rows)
                    {
                        DOADM_AccessGroupMaster objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
                        if (dr.Table.Columns.Contains("AccessGroupMasterId"))
                        {
                            if (!DBNull.Value.Equals(dr["AccessGroupMasterId"]))
                            {
                                objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId = (long)dr["AccessGroupMasterId"];
                            }
                        }
                        if (dr.Table.Columns.Contains("AccessGroupName"))
                        {
                            if (!DBNull.Value.Equals(dr["AccessGroupName"]))
                            {
                                objDOADM_AccessGroupMaster.AccessGroupName = dr["AccessGroupName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("CreatedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                            {
                                objDOADM_AccessGroupMaster.CreatedByRef = (long)dr["CreatedByRef"];
                            }
                        }
                        if (dr.Table.Columns.Contains("CreatedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["CreatedOn"]))
                            {
                                objDOADM_AccessGroupMaster.UTCCreatedOn =ZoneLookupUI.ConvertToTimeZone(dr["CreatedOn"].ToDateTime(),TimeZone);
                            }
                        }
                        if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                            {
                                objDOADM_AccessGroupMaster.LastUpdatedByRef = (long)dr["LastUpdatedByRef"];
                            }
                        }
                        if (dr.Table.Columns.Contains("LastUpdatedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["LastUpdatedOn"]))
                            {
                                objDOADM_AccessGroupMaster.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(dr["LastUpdatedOn"].ToDateTime(),TimeZone);
                            }
                        }
                        if (dr.Table.Columns.Contains("CreatedByName"))
                        {
                            if (!DBNull.Value.Equals(dr["CreatedByName"]))
                            {
                                objDOADM_AccessGroupMaster.CreatedByName = dr["CreatedByName"].ToString(); ;
                            }
                        }
                        if (dr.Table.Columns.Contains("LastUpdatedByName"))
                        {
                            if (!DBNull.Value.Equals(dr["LastUpdatedByName"]))
                            {
                                objDOADM_AccessGroupMaster.LastUpdateByName = dr["LastUpdatedByName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("LockedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["LockedByRef"]))
                            {
                                objDOADM_AccessGroupMaster.LockedByRef = Convert.ToInt64(dr["LockedByRef"]);
                            }
                        }
                        if (dr.Table.Columns.Contains("UTCLockedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                            {
                                objDOADM_AccessGroupMaster.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                            }
                        }
                        if (dr.Table.Columns.Contains("LockedBy"))
                        {
                            if (!DBNull.Value.Equals(dr["LockedBy"]))
                            {
                                objDOADM_AccessGroupMaster.LockedByName = dr["LockedBy"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("IsActive"))
                        {
                            if (!DBNull.Value.Equals(dr["IsActive"]))
                            {
                                if (dr["IsActive"].ToString() == "True")
                                    objDOADM_AccessGroupMaster.IsActive = true;
                                else
                                    objDOADM_AccessGroupMaster.IsActive = false;
                            }
                        }
                        lstDOADM_AccessGroupMaster.Add(objDOADM_AccessGroupMaster);
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }

        public ExceptionTypes AddEditAccessGroup(long lLoggedInUserId, UIDOAccessGroup objUIDOAccessGroup, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lNumberOfRowsEffected = 0;
                DataSet dsTable = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objUIDOAccessGroup.ADM_AccessGroupMasterId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_AccessGroupMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIDOAccessGroup.ADM_AccessGroupMasterId;
                    parameters.Add(sqlParam);
                }
                if (objUIDOAccessGroup.AccessGroupName != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objUIDOAccessGroup.AccessGroupName;
                    parameters.Add(sqlParam);
                }
                if (objUIDOAccessGroup.AccessGroupDescription != string.Empty)
                {
                    if (objUIDOAccessGroup.AccessGroupDescription == null)
                    {
                        objUIDOAccessGroup.AccessGroupDescription = "";
                    }
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupDescription";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objUIDOAccessGroup.AccessGroupDescription;
                    parameters.Add(sqlParam);
                }
                if (objUIDOAccessGroup.RoleLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RoleLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIDOAccessGroup.RoleLkup;
                    parameters.Add(sqlParam);
                }
                if (objUIDOAccessGroup.WorkBasketLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkBasketLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIDOAccessGroup.WorkBasketLkup;
                    parameters.Add(sqlParam);
                }
                if (objUIDOAccessGroup.DescripancyCatLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIDOAccessGroup.DescripancyCatLkup;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objUIDOAccessGroup.IsActive;
                parameters.Add(sqlParam);
                                
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lLoggedInUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = ScreenType.AccessGroup;
                parameters.Add(sqlParam);

                if (objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation.Count() > 0)
                {
                    DataTable accessGroupSkills;
                    SetAccessGroupSkills(objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation, out accessGroupSkills);
                    DataTableReader dtrAccessGroupSkills = new DataTableReader(accessGroupSkills);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TV_Skills";
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    sqlParam.Value = dtrAccessGroupSkills;
                    parameters.Add(sqlParam);
                }

                if (objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation.Count() > 0)
                {
                    DataTable accessGroupReports;
                    SetAccessGroupReports(objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation, out accessGroupReports);
                    DataTableReader dtrAccessGroupReport = new DataTableReader(accessGroupReports);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TV_Reports";
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    sqlParam.Value = dtrAccessGroupReport;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_UPD_AccessGroup, parameters.ToArray(), out lErrocode, out lErrorNumber, out lNumberOfRowsEffected, out errorMessage);
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
                    return ExceptionTypes.UnknownError;

            }
            catch (Exception ex)
            {
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetAccessGroupForEdit(DOADM_AccessGroupMaster objDOADM_AccessGroupMaster, out UIDOAccessGroup objUIDOAccessGroup)
        {
            objUIDOAccessGroup = new UIDOAccessGroup();
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@AccessGroupMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_AccesssGroup, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapUIDOAccessGroupObjectProperties(dsTable, out objUIDOAccessGroup);
                        if (objUIDOAccessGroup != null)
                            return ExceptionTypes.Success;
                    }
                    return ExceptionTypes.ZeroRecords;
                }
                else if (executionResult == 2)
                {
                    return ExceptionTypes.ZeroRecords;
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

        private void MapUIDOAccessGroupObjectProperties(DataSet dstTable, out UIDOAccessGroup objUIDOAccessGroup)
        {
            objUIDOAccessGroup = new UIDOAccessGroup();
            try
            {
                if (dstTable.Tables.Count > 0)
                {

                    foreach (DataRow drAG in dstTable.Tables[0].Rows)
                    {
                        if (drAG.Table.Columns.Contains("AccessGroupMasterId"))
                        {
                            if (!DBNull.Value.Equals(drAG["AccessGroupMasterId"]))
                            {
                                objUIDOAccessGroup.ADM_AccessGroupMasterId = (long)drAG["AccessGroupMasterId"];
                            }
                        }
                        if (drAG.Table.Columns.Contains("WorkBasketLkup"))
                        {
                            if (!DBNull.Value.Equals(drAG["WorkBasketLkup"]))
                            {
                                objUIDOAccessGroup.WorkBasketLkup = (long)drAG["WorkBasketLkup"];
                            }
                        }
                        if (drAG.Table.Columns.Contains("DescripancyCatLkup"))
                        {
                            if (!DBNull.Value.Equals(drAG["DescripancyCatLkup"]))
                            {
                                objUIDOAccessGroup.DescripancyCatLkup = (long)drAG["DescripancyCatLkup"];
                            }
                        }
                        if (drAG.Table.Columns.Contains("RoleLkup"))
                        {
                            if (!DBNull.Value.Equals(drAG["RoleLkup"]))
                            {
                                objUIDOAccessGroup.RoleLkup = (long)drAG["RoleLkup"];
                            }
                        }
                        if (drAG.Table.Columns.Contains("AccessGroupName"))
                        {
                            if (!DBNull.Value.Equals(drAG["AccessGroupName"]))
                            {
                                objUIDOAccessGroup.AccessGroupName = drAG["AccessGroupName"].ToString();
                            }
                        }
                        if (drAG.Table.Columns.Contains("AccessGroupDescription"))
                        {
                            if (!DBNull.Value.Equals(drAG["AccessGroupDescription"]))
                            {
                                objUIDOAccessGroup.AccessGroupDescription = drAG["AccessGroupDescription"].ToString();
                            }
                        }
                        if (drAG.Table.Columns.Contains("IsActive"))
                        {
                            if (!DBNull.Value.Equals(drAG["IsActive"]))
                            {
                                if (drAG["IsActive"].ToString() == "True")
                                    objUIDOAccessGroup.IsActive = true;
                                else
                                    objUIDOAccessGroup.IsActive = false;
                            }
                        }
                    }
                    
                    objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation = new List<DOADM_AccessGroupSkillsCorrelation>();
                    foreach (DataRow drSkl in dstTable.Tables[1].Rows)
                    {
                        DOADM_AccessGroupSkillsCorrelation objDOADM_AccessGroupSkillsCorrelation = new DOADM_AccessGroupSkillsCorrelation();
                        if (drSkl.Table.Columns.Contains("ADM_AccessGroupSkillsCorrelationId"))
                        {
                            if (!DBNull.Value.Equals(drSkl["ADM_AccessGroupSkillsCorrelationId"]))
                            {
                                objDOADM_AccessGroupSkillsCorrelation.ADM_AccessGroupSkillsCorrelationId = (long)drSkl["ADM_AccessGroupSkillsCorrelationId"];
                            }
                        }
                        if (drSkl.Table.Columns.Contains("ADM_AccessGroupMasterRef"))
                        {
                            if (!DBNull.Value.Equals(drSkl["ADM_AccessGroupMasterRef"]))
                            {
                                objDOADM_AccessGroupSkillsCorrelation.ADM_AccessGroupMasterRef = (long)drSkl["ADM_AccessGroupMasterRef"];
                            }
                        }
                        if (drSkl.Table.Columns.Contains("ADM_SkillsMasterRef"))
                        {
                            if (!DBNull.Value.Equals(drSkl["ADM_SkillsMasterRef"]))
                            {
                                objDOADM_AccessGroupSkillsCorrelation.ADM_SkillsMasterRef = (long)drSkl["ADM_SkillsMasterRef"];
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanCreate"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanCreate"]))
                            {
                                if (drSkl["CanCreate"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanCreate = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanCreate = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanSearch"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanSearch"]))
                            {
                                if (drSkl["CanSearch"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanSearch = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanSearch = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanReassign"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanReassign"]))
                            {
                                if (drSkl["CanReassign"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanReassign = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanReassign = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanUnlock"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanUnlock"]))
                            {
                                if (drSkl["CanUnlock"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanUnlock = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanUnlock = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanHistory"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanHistory"]))
                            {
                                if (drSkl["CanHistory"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanHistory = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanHistory = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanModify"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanModify"]))
                            {
                                if (drSkl["CanModify"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanModify = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanModify = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanView"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanView"]))
                            {
                                if (drSkl["CanView"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanView = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanView = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanMassUpdate"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanMassUpdate"]))
                            {
                                if (drSkl["CanMassUpdate"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanMassUpdate = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanMassUpdate = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanUpload"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanUpload"]))
                            {
                                if (drSkl["CanUpload"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanUpload = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanUpload = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanClone"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanClone"]))
                            {
                                if (drSkl["CanClone"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanClone = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanClone = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("CanReopen"))
                        {
                            if (!DBNull.Value.Equals(drSkl["CanReopen"]))
                            {
                                if (drSkl["CanReopen"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.CanReopen = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.CanReopen = false;
                            }
                        }
                        if (drSkl.Table.Columns.Contains("UTCLastUpdatedOn"))
                        {
                            if (!DBNull.Value.Equals(drSkl["UTCLastUpdatedOn"]))
                            {
                                objDOADM_AccessGroupSkillsCorrelation.UTCLastUpdatedOn = Convert.ToDateTime(drSkl["UTCLastUpdatedOn"]);
                            }
                        }
                        if (drSkl.Table.Columns.Contains("LastUpdatedByName"))
                        {
                            if (!DBNull.Value.Equals(drSkl["LastUpdatedByName"]))
                            {
                                objDOADM_AccessGroupSkillsCorrelation.LastUpdatedByName = drSkl["LastUpdatedByName"].ToString();
                            }
                        }
                        if (drSkl.Table.Columns.Contains("IsActive"))
                        {
                            if (!DBNull.Value.Equals(drSkl["IsActive"]))
                            {
                                if (drSkl["IsActive"].ToString() == "True")
                                    objDOADM_AccessGroupSkillsCorrelation.IsActive = true;
                                else
                                    objDOADM_AccessGroupSkillsCorrelation.IsActive = false;
                            }
                        }
                        objUIDOAccessGroup.lstDOADM_AccessGroupSkillsCorrelation.Add(objDOADM_AccessGroupSkillsCorrelation);
                    }
                    
                    objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation = new List<DOADM_AccessGroupReportCorrelation>();
                    foreach (DataRow drRpt in dstTable.Tables[2].Rows)
                    {
                        DOADM_AccessGroupReportCorrelation objDOADM_AccessGroupReportCorrelation = new DOADM_AccessGroupReportCorrelation();
                        if (drRpt.Table.Columns.Contains("ADM_AccessGroupReportCorrelationId"))
                        {
                            if (!DBNull.Value.Equals(drRpt["ADM_AccessGroupReportCorrelationId"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.ADM_AccessGroupReportCorrelationId = (long)drRpt["ADM_AccessGroupReportCorrelationId"];
                            }
                        }
                        if (drRpt.Table.Columns.Contains("ADM_AccessGroupMasterRef"))
                        {
                            if (!DBNull.Value.Equals(drRpt["ADM_AccessGroupMasterRef"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.ADM_AccessGroupMasterRef = (long)drRpt["ADM_AccessGroupMasterRef"];
                            }
                        }
                        if (drRpt.Table.Columns.Contains("RPT_ReportsMasterRef"))
                        {
                            if (!DBNull.Value.Equals(drRpt["RPT_ReportsMasterRef"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.RPT_ReportsMasterRef = (long)drRpt["RPT_ReportsMasterRef"];
                            }
                        }
                        if (drRpt.Table.Columns.Contains("CreatedByRef"))
                        {
                            if (!DBNull.Value.Equals(drRpt["CreatedByRef"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.CreatedByRef = (long)drRpt["CreatedByRef"];
                            }
                        }
                        if (drRpt.Table.Columns.Contains("UTCCreatedOn"))
                        {
                            if (!DBNull.Value.Equals(drRpt["UTCCreatedOn"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.UTCCreatedOn = Convert.ToDateTime(drRpt["UTCCreatedOn"]);
                            }
                        }
                        if (drRpt.Table.Columns.Contains("UTCLastUpdatedOn"))
                        {
                            if (!DBNull.Value.Equals(drRpt["UTCLastUpdatedOn"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.UTCLastUpdatedOn = Convert.ToDateTime(drRpt["UTCLastUpdatedOn"]);
                            }
                        }
                        if (drRpt.Table.Columns.Contains("LastUpdatedByRef"))
                        {
                            if (!DBNull.Value.Equals(drRpt["LastUpdatedByRef"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.LastUpdatedByRef = (long)drRpt["LastUpdatedByRef"];
                            }
                        }
                        if (drRpt.Table.Columns.Contains("CreatedByName"))
                        {
                            if (!DBNull.Value.Equals(drRpt["CreatedByName"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.CreatedByName = drRpt["CreatedByName"].ToString();
                            }
                        }
                        if (drRpt.Table.Columns.Contains("LastUpdatedByName"))
                        {
                            if (!DBNull.Value.Equals(drRpt["LastUpdatedByName"]))
                            {
                                objDOADM_AccessGroupReportCorrelation.LastUpdatedByName = drRpt["LastUpdatedByName"].ToString();
                            }
                        }
                        if (drRpt.Table.Columns.Contains("IsActive"))
                        {
                            if (!DBNull.Value.Equals(drRpt["IsActive"]))
                            {
                                if (drRpt["IsActive"].ToString() == "True")
                                    objDOADM_AccessGroupReportCorrelation.IsActive = true;
                                else
                                    objDOADM_AccessGroupReportCorrelation.IsActive = false;
                            }
                        }
                        objUIDOAccessGroup.lstDOADM_AccessGroupReportCorrelation.Add(objDOADM_AccessGroupReportCorrelation);
                    }
                }
            }
            catch (Exception ex)
            {
                //log
            }
        }

        private void SetAccessGroupSkills(List<DOADM_AccessGroupSkillsCorrelation> lstDOADM_AccessGroupSkillsCorrelation, out DataTable accessGroupSkills)
        {
            accessGroupSkills = new DataTable("TVP_AccessGroupSkills");
            GetDataColumn(accessGroupSkills);
            DataRow rowLetterTemplates;
            try
            {
                if (lstDOADM_AccessGroupSkillsCorrelation != null && lstDOADM_AccessGroupSkillsCorrelation.Count > 0)
                {
                    foreach (DOADM_AccessGroupSkillsCorrelation accessGroupSkill in lstDOADM_AccessGroupSkillsCorrelation)
                    {
                        rowLetterTemplates = accessGroupSkills.NewRow();
                        rowLetterTemplates["ADM_AccessGroupSkillsCorrelationId"] = accessGroupSkill.ADM_AccessGroupSkillsCorrelationId;
                        rowLetterTemplates["ADM_AccessGroupMasterRef"] = accessGroupSkill.ADM_AccessGroupMasterRef;
                        rowLetterTemplates["ADM_SkillsMasterRef"] = accessGroupSkill.ADM_SkillsMasterRef;
                        rowLetterTemplates["CanCreate"] = accessGroupSkill.CanCreate;
                        rowLetterTemplates["CanModify"] = accessGroupSkill.CanModify;
                        rowLetterTemplates["CanSearch"] = accessGroupSkill.CanSearch;
                        rowLetterTemplates["CanView"] = accessGroupSkill.CanView;
                        rowLetterTemplates["CanMassUpdate"] = accessGroupSkill.CanMassUpdate;
                        rowLetterTemplates["CanHistory"] = accessGroupSkill.CanHistory;
                        rowLetterTemplates["CanReassign"] = accessGroupSkill.CanReassign;
                        rowLetterTemplates["CanUnlock"] = accessGroupSkill.CanUnlock;
                        rowLetterTemplates["CanUpload"] = accessGroupSkill.CanUpload;
                        rowLetterTemplates["CanClone"] = accessGroupSkill.CanClone;
                        rowLetterTemplates["CanReopen"] = accessGroupSkill.CanReopen;
                        rowLetterTemplates["IsActive"] = accessGroupSkill.IsActive;

                        accessGroupSkills.Rows.Add(rowLetterTemplates);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetDataColumn(DataTable accessGroupSkills)
        {

            DataColumn dcLetterTemplates;
            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_AccessGroupSkillsCorrelationId";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_AccessGroupMasterRef";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_SkillsMasterRef";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanCreate";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanModify";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanSearch";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanView";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanMassUpdate";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanHistory";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanReassign";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanUnlock";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanUpload";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanClone";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "CanReopen";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "IsActive";
            dcLetterTemplates.ReadOnly = true;
            accessGroupSkills.Columns.Add(dcLetterTemplates);
        }

        private void SetAccessGroupReports(List<DOADM_AccessGroupReportCorrelation> lstDOADM_AccessGroupReportCorrelation, out DataTable accessGroupReports)
        {
            accessGroupReports = new DataTable("TVP_AccessGroupReports");
            GetDataColumnForReports(accessGroupReports);
            DataRow rowLetterTemplates;
            try
            {
                if (lstDOADM_AccessGroupReportCorrelation != null && lstDOADM_AccessGroupReportCorrelation.Count > 0)
                {
                    foreach (DOADM_AccessGroupReportCorrelation accessGroupReport in lstDOADM_AccessGroupReportCorrelation)
                    {
                        rowLetterTemplates = accessGroupReports.NewRow();
                        rowLetterTemplates["ADM_AccessGroupReportCorrelationId"] = accessGroupReport.ADM_AccessGroupReportCorrelationId;
                        rowLetterTemplates["ADM_AccessGroupMasterRef"] = accessGroupReport.ADM_AccessGroupMasterRef;
                        rowLetterTemplates["RPT_ReportsMasterRef"] = accessGroupReport.RPT_ReportsMasterRef;
                        rowLetterTemplates["IsActive"] = accessGroupReport.IsActive;
                        accessGroupReports.Rows.Add(rowLetterTemplates);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetDataColumnForReports(DataTable accessGroupReports)
        {

            DataColumn dcLetterTemplates;
            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_AccessGroupReportCorrelationId";
            dcLetterTemplates.ReadOnly = true;
            accessGroupReports.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "ADM_AccessGroupMasterRef";
            dcLetterTemplates.ReadOnly = true;
            accessGroupReports.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Int64");
            dcLetterTemplates.ColumnName = "RPT_ReportsMasterRef";
            dcLetterTemplates.ReadOnly = true;
            accessGroupReports.Columns.Add(dcLetterTemplates);

            dcLetterTemplates = new DataColumn();
            dcLetterTemplates.DataType = System.Type.GetType("System.Boolean");
            dcLetterTemplates.ColumnName = "IsActive";
            dcLetterTemplates.ReadOnly = true;
            accessGroupReports.Columns.Add(dcLetterTemplates);
        }
    }
}
