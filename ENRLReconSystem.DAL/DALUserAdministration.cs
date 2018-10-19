using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace ENRLReconSystem.DAL
{
    public class DALUserAdministration
    {
        private ExceptionTypes retValue;
        public ExceptionTypes GetUserBasedOnMSID(long? TimeZone,string MSID, out DOADM_UserMaster objDOADM_UserMaster)
        {

            objDOADM_UserMaster = new DOADM_UserMaster();

            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (MSID != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MSID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = MSID;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_ERS_APP_SEL_UserMaster, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        List<DOADM_UserMaster> lstDOADM_UserMaster;
                        MapUsersDOUserDetails(TimeZone, dsTable, out lstDOADM_UserMaster);
                        if (lstDOADM_UserMaster.Count > 0)
                            objDOADM_UserMaster = lstDOADM_UserMaster[0];
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
            catch
            {
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetUserAccessPermission(string MSID, long? businessSegmentLkup, long? workBasketLkup, long? roleLkup, out UIUserLogin objUIUserLogin)
        {
            objUIUserLogin = new UIUserLogin();

            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();
                string errorMessage;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (MSID != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MSID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = MSID;
                    parameters.Add(sqlParam);
                }
                if (businessSegmentLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = businessSegmentLkup;
                    parameters.Add(sqlParam);
                }
                if (workBasketLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkBasketLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = workBasketLkup;
                    parameters.Add(sqlParam);
                }
                if (roleLkup > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RoleLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = roleLkup;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_UserAccessPermission, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapUIUserLoginDetails(dsTable, out objUIUserLogin);
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
            catch
            {
                return ExceptionTypes.UnknownError;
            }
        }

        public void MapUsersDOUserDetails(long? TimeZone,DataSet dsUsers, out List<DOADM_UserMaster> lstDOADM_UserMaster)
        {
            lstDOADM_UserMaster = new List<DOADM_UserMaster>();
            try
            {
                if (dsUsers.Tables.Count > 0 && dsUsers.Tables[0].Rows.Count > 0)
                {
                    DataTable objDataTable = dsUsers.Tables[0];
                    foreach (DataRow dr in objDataTable.Rows)
                    {
                        DOADM_UserMaster objDOADM_UserMaster = new DOADM_UserMaster();
                        if (dr.Table.Columns.Contains("ADM_UserMasterId"))
                        {
                            if (!DBNull.Value.Equals(dr["ADM_UserMasterId"]))
                            {
                                objDOADM_UserMaster.ADM_UserMasterId = (long)dr["ADM_UserMasterId"];
                            }
                        }
                        if (dr.Table.Columns.Contains("LockedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["LockedByRef"]))
                            {
                                objDOADM_UserMaster.LockedByRef = (long)dr["LockedByRef"];
                            }
                        }
                        if (dr.Table.Columns.Contains("UTCLockedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["UTCLockedOn"]))
                            {
                                objDOADM_UserMaster.UTCLockedOn = ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone);
                            }
                        }
                        if (dr.Table.Columns.Contains("Title"))
                        {
                            if (!DBNull.Value.Equals(dr["Title"]))
                            {
                                objDOADM_UserMaster.Titlelkup = (long)dr["Title"];
                            }
                        }
                        if (dr.Table.Columns.Contains("FirstName"))
                        {
                            if (!DBNull.Value.Equals(dr["FirstName"]))
                            {
                                objDOADM_UserMaster.FirstName = dr["FirstName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("LastName"))
                        {
                            if (!DBNull.Value.Equals(dr["LastName"]))
                            {
                                objDOADM_UserMaster.LastName = dr["LastName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("FullName"))
                        {
                            if (!DBNull.Value.Equals(dr["FullName"]))
                            {
                                objDOADM_UserMaster.FullName = dr["FullName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("SystemFullName"))
                        {
                            if (!DBNull.Value.Equals(dr["SystemFullName"]))
                            {
                                objDOADM_UserMaster.SystemFullName = dr["SystemFullName"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("MSID"))
                        {
                            if (!DBNull.Value.Equals(dr["MSID"]))
                            {
                                objDOADM_UserMaster.MSID = dr["MSID"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("Email"))
                        {
                            if (!DBNull.Value.Equals(dr["Email"]))
                            {
                                objDOADM_UserMaster.Email = dr["Email"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("ManagerId"))
                        {
                            if (!DBNull.Value.Equals(dr["ManagerId"]))
                            {
                                objDOADM_UserMaster.ManagerId = (long)dr["ManagerId"];
                            }
                        }
                        if (dr.Table.Columns.Contains("LocationLkup"))
                        {
                            if (!DBNull.Value.Equals(dr["LocationLkup"]))
                            {
                                objDOADM_UserMaster.LocationLkup = (long)dr["LocationLkup"];
                            }
                        }
                        if (dr.Table.Columns.Contains("NonUserLkup"))
                        {
                            if (!DBNull.Value.Equals(dr["NonUserLkup"]))
                            {
                                objDOADM_UserMaster.NonUserLkup = (long)dr["NonUserLkup"];
                            }
                        }
                        if (dr.Table.Columns.Contains("StartDate"))
                        {
                            if (!DBNull.Value.Equals(dr["StartDate"]))
                            {
                                objDOADM_UserMaster.StartDate = Convert.ToDateTime(dr["StartDate"]);
                            }
                        }
                        if (dr.Table.Columns.Contains("EndDate"))
                        {
                            if (!DBNull.Value.Equals(dr["EndDate"]))
                            {
                                objDOADM_UserMaster.EndDate = Convert.ToDateTime(dr["EndDate"]);
                            }
                        }
                        if (dr.Table.Columns.Contains("SpecialistTitle"))
                        {
                            if (!DBNull.Value.Equals(dr["SpecialistTitle"]))
                            {
                                objDOADM_UserMaster.SpecialistTitle = dr["SpecialistTitle"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("SpecialistPhone"))
                        {
                            if (!DBNull.Value.Equals(dr["SpecialistPhone"]))
                            {
                                objDOADM_UserMaster.SpecialistPhone = dr["SpecialistPhone"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("SpecialistFax"))
                        {
                            if (!DBNull.Value.Equals(dr["SpecialistFax"]))
                            {
                                objDOADM_UserMaster.SpecialistFax = dr["SpecialistFax"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("SpecialistHours"))
                        {
                            if (!DBNull.Value.Equals(dr["SpecialistHours"]))
                            {
                                objDOADM_UserMaster.SpecialistHours = dr["SpecialistHours"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("SpecialistTimeZone"))
                        {
                            if (!DBNull.Value.Equals(dr["SpecialistTimeZone"]))
                            {
                                objDOADM_UserMaster.SpecialistTimeZone = (long)dr["SpecialistTimeZone"];
                            }
                        }
                        if (dr.Table.Columns.Contains("UserAddressLine1"))
                        {
                            if (!DBNull.Value.Equals(dr["UserAddressLine1"]))
                            {
                                objDOADM_UserMaster.UserAddressLine1 = dr["UserAddressLine1"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("UserAddressLine2"))
                        {
                            if (!DBNull.Value.Equals(dr["UserAddressLine2"]))
                            {
                                objDOADM_UserMaster.UserAddressLine2 = dr["UserAddressLine2"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("UserCity"))
                        {
                            if (!DBNull.Value.Equals(dr["UserCity"]))
                            {
                                objDOADM_UserMaster.UserCity = dr["UserCity"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("UserStateLkup"))
                        {
                            if (!DBNull.Value.Equals(dr["UserStateLkup"]))
                            {
                                objDOADM_UserMaster.UserStateLkup = (long)dr["UserStateLkup"];
                            }
                        }
                        if (dr.Table.Columns.Contains("UserZip"))
                        {
                            if (!DBNull.Value.Equals(dr["UserZip"]))
                            {
                                objDOADM_UserMaster.UserZip = dr["UserZip"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("IsActive"))
                        {
                            if (!DBNull.Value.Equals(dr["IsActive"]))
                            {
                                if (dr["IsActive"].ToString() == "True")
                                    objDOADM_UserMaster.IsActive = true;
                                if (dr["IsActive"].ToString() == "False")
                                    objDOADM_UserMaster.IsActive = false;
                            }
                        }
                        if (dr.Table.Columns.Contains("IsManager"))
                        {
                            if (!DBNull.Value.Equals(dr["IsManager"]))
                            {
                                if (dr["IsManager"].ToString() == "True")
                                    objDOADM_UserMaster.IsManager = true;
                                if (dr["IsManager"].ToString() == "False")
                                    objDOADM_UserMaster.IsManager = false;
                            }
                        }
                        if (dr.Table.Columns.Contains("UTCCreatedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                            {
                                objDOADM_UserMaster.UTCCreatedOn = Convert.ToDateTime(dr["UTCCreatedOn"]);
                            }
                        }
                        if (dr.Table.Columns.Contains("CreatedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                            {
                                objDOADM_UserMaster.CreatedByRef = (long)dr["CreatedByRef"];
                            }
                        }

                        if (dr.Table.Columns.Contains("CreatedBy"))
                        {
                            if (!DBNull.Value.Equals(dr["CreatedBy"]))
                            {
                                objDOADM_UserMaster.CreatedBy = dr["CreatedBy"].ToString();
                            }
                        }
                        if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                        {
                            if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                            {
                                objDOADM_UserMaster.UTCLastUpdatedOn = Convert.ToDateTime(dr["UTCLastUpdatedOn"]);
                            }
                        }
                        if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                        {
                            if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                            {
                                objDOADM_UserMaster.LastUpdatedByRef = (long)dr["LastUpdatedByRef"];
                            }
                        }

                        if (dr.Table.Columns.Contains("LastUpdatedBy"))
                        {
                            if (!DBNull.Value.Equals(dr["LastUpdatedBy"]))
                            {
                                objDOADM_UserMaster.LastUpdatedBy = dr["LastUpdatedBy"].ToString();
                            }
                        }
                        if(dr.Table.Columns.Contains("LockedByName"))
                        {
                            if (!DBNull.Value.Equals(dr["LockedByName"]))
                            {
                                objDOADM_UserMaster.LockedByName = dr["LockedByName"].ToString();
                            }
                        }

                        objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation = new List<DOADM_AccessGroupUserCorrelation>();
                        if (dsUsers.Tables.Count > 1 && dsUsers.Tables[1].Rows.Count > 0)
                        {
                            DataTable dtUserAccessGroup = dsUsers.Tables[1];
                            foreach (DataRow drUAG in dtUserAccessGroup.Rows)
                            {
                                DOADM_AccessGroupUserCorrelation objDOADM_AccessGroupUserCorrelation = new DOADM_AccessGroupUserCorrelation();
                                if (drUAG.Table.Columns.Contains("ADM_AccessGroupUserCorrelationId"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["ADM_AccessGroupUserCorrelationId"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupUserCorrelationId = (long)drUAG["ADM_AccessGroupUserCorrelationId"];
                                    }
                                }
                                if (drUAG.Table.Columns.Contains("ADM_AccessGroupMasterRef"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["ADM_AccessGroupMasterRef"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.ADM_AccessGroupMasterRef = (long)drUAG["ADM_AccessGroupMasterRef"];
                                    }
                                }
                                if (drUAG.Table.Columns.Contains("ADM_UserMasterRef"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["ADM_UserMasterRef"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.ADM_UserMasterRef = (long)drUAG["ADM_UserMasterRef"];
                                    }
                                }
                                if (dr.Table.Columns.Contains("UTCCreatedOn"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["UTCCreatedOn"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.UTCCreatedOn = ZoneLookupUI.ConvertToTimeZone(drUAG["UTCCreatedOn"].ToDateTime(),TimeZone);
                                    }
                                }
                                if (dr.Table.Columns.Contains("CreatedByRef"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["CreatedByRef"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.CreatedByRef = (long)drUAG["CreatedByRef"];
                                    }
                                }
                                if (dr.Table.Columns.Contains("CreatedBy"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["CreatedBy"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.CreatedByName = drUAG["CreatedBy"].ToString();
                                    }
                                }
                                if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["UTCLastUpdatedOn"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.UTCLastUpdatedOn = ZoneLookupUI.ConvertToTimeZone(drUAG["UTCLastUpdatedOn"].ToDateTime(),TimeZone);
                                    }
                                }
                                if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["LastUpdatedByRef"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.LastUpdatedByRef = (long)drUAG["LastUpdatedByRef"];
                                    }
                                }

                                if (dr.Table.Columns.Contains("LastUpdatedBy"))
                                {
                                    if (!DBNull.Value.Equals(drUAG["LastUpdatedBy"]))
                                    {
                                        objDOADM_AccessGroupUserCorrelation.LastUpdatedBy = drUAG["LastUpdatedBy"].ToString();
                                    }
                                }

                                objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation.Add(objDOADM_AccessGroupUserCorrelation);
                            }
                        }
                        lstDOADM_UserMaster.Add(objDOADM_UserMaster);
                    }
                }
            }
            catch
            {

            }
        }

        public void MapUIUserLoginDetails(DataSet objDataSet, out UIUserLogin objUIUserLogin)
        {
            objUIUserLogin = new UIUserLogin();
            try
            {
                foreach (DataRow dr in objDataSet.Tables[0].Rows)
                {
                    if (dr.Table.Columns.Contains("ADM_UserMasterId"))
                    {
                        if (!DBNull.Value.Equals(dr["ADM_UserMasterId"]))
                        {
                            objUIUserLogin.ADM_UserMasterId = (long)dr["ADM_UserMasterId"];
                        }
                    }
                    if (dr.Table.Columns.Contains("FullName"))
                    {
                        if (!DBNull.Value.Equals(dr["FullName"]))
                        {
                            objUIUserLogin.FullName = dr["FullName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MSID"))
                    {
                        if (!DBNull.Value.Equals(dr["MSID"]))
                        {
                            objUIUserLogin.MSID = dr["MSID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("Email"))
                    {
                        if (!DBNull.Value.Equals(dr["Email"]))
                        {
                            objUIUserLogin.Email = dr["Email"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ManagerId"))
                    {
                        if (!DBNull.Value.Equals(dr["ManagerId"]))
                        {
                            objUIUserLogin.ManagerId = (long)dr["ManagerId"];
                        }
                    }
                    if (dr.Table.Columns.Contains("LocationLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["LocationLkup"]))
                        {
                            objUIUserLogin.LocationLkup = (long)dr["LocationLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("StartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["StartDate"]))
                        {
                            objUIUserLogin.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("EndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EndDate"]))
                        {
                            objUIUserLogin.StartDate = Convert.ToDateTime(dr["EndDate"]);
                        }
                    }

                    //User Skills
                    if (objDataSet.Tables.Count > 1)
                    {
                        objUIUserLogin.UserSkills = new List<UserSkills>();
                        foreach (DataRow dr1 in objDataSet.Tables[1].Rows)
                        {
                            UserSkills objUserSkills = new UserSkills();
                            if (dr1.Table.Columns.Contains("SkillsName"))
                            {
                                if (!DBNull.Value.Equals(dr1["SkillsName"]))
                                {
                                    objUserSkills.SkillsName = dr1["SkillsName"].ToString();
                                }
                            }
                            if (dr1.Table.Columns.Contains("RoleLkup"))
                            {
                                if (!DBNull.Value.Equals(dr1["RoleLkup"]))
                                {
                                    objUserSkills.RoleLkup = (long)dr1["RoleLkup"];
                                }
                            }
                            if (dr1.Table.Columns.Contains("BusinessSegmentLkup"))
                            {
                                if (!DBNull.Value.Equals(dr1["BusinessSegmentLkup"]))
                                {
                                    objUserSkills.BusinessSegmentLkup = (long)dr1["BusinessSegmentLkup"];
                                }
                            }
                            if (dr1.Table.Columns.Contains("WorkBasketLkup"))
                            {
                                if (!DBNull.Value.Equals(dr1["WorkBasketLkup"]))
                                {
                                    objUserSkills.WorkBasketLkup = (long)dr1["WorkBasketLkup"];
                                }
                            }
                            if (dr1.Table.Columns.Contains("WorkQueuesLkup"))
                            {
                                if (!DBNull.Value.Equals(dr1["WorkQueuesLkup"]))
                                {
                                    objUserSkills.WorkQueuesLkup = (long)dr1["WorkQueuesLkup"];
                                }
                            }
                            if (dr1.Table.Columns.Contains("CMN_DepartmentRef"))
                            {
                                if (!DBNull.Value.Equals(dr1["CMN_DepartmentRef"]))
                                {
                                    objUserSkills.CMN_DepartmentRef = (long)dr1["CMN_DepartmentRef"];
                                }
                            }
                            if (dr1.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                            {
                                if (!DBNull.Value.Equals(dr1["DiscrepancyCategoryLkup"]))
                                {
                                    objUserSkills.DiscrepancyCategoryLkup = (long)dr1["DiscrepancyCategoryLkup"];
                                }
                            }

                            if (dr1.Table.Columns.Contains("CanCreate"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanCreate"]))
                                {
                                    objUserSkills.CanCreate = Convert.ToBoolean(dr1["CanCreate"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanModify"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanModify"]))
                                {
                                    objUserSkills.CanModify = Convert.ToBoolean(dr1["CanModify"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanSearch"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanSearch"]))
                                {
                                    objUserSkills.CanSearch = Convert.ToBoolean(dr1["CanSearch"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanView"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanView"]))
                                {
                                    objUserSkills.CanView = Convert.ToBoolean(dr1["CanView"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanMassUpdate"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanMassUpdate"]))
                                {
                                    objUserSkills.CanMassUpdate = Convert.ToBoolean(dr1["CanMassUpdate"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanHistory"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanHistory"]))
                                {
                                    objUserSkills.CanHistory = Convert.ToBoolean(dr1["CanHistory"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanReassign"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanReassign"]))
                                {
                                    objUserSkills.CanReassign = Convert.ToBoolean(dr1["CanReassign"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanUnlock"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanUnlock"]))
                                {
                                    objUserSkills.CanUnlock = Convert.ToBoolean(dr1["CanUnlock"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanUpload"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanUpload"]))
                                {
                                    objUserSkills.CanUpload = Convert.ToBoolean(dr1["CanUpload"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanClone"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanClone"]))
                                {
                                    objUserSkills.CanClone = Convert.ToBoolean(dr1["CanClone"]);
                                }
                            }
                            if (dr1.Table.Columns.Contains("CanReopen"))
                            {
                                if (!DBNull.Value.Equals(dr1["CanReopen"]))
                                {
                                    objUserSkills.CanReopen = Convert.ToBoolean(dr1["CanReopen"]);
                                }
                            }
                            objUIUserLogin.UserSkills.Add(objUserSkills);
                        }
                    }

                    //User Work Queues
                    if (objDataSet.Tables.Count > 2)
                    {
                        objUIUserLogin.UserQueueList = new List<UserWorkQueues>();
                        foreach (DataRow dr2 in objDataSet.Tables[2].Rows)
                        {
                            UserWorkQueues objUserWorkQueues = new UserWorkQueues();
                            if (dr2.Table.Columns.Contains("QueueLkp"))
                            {
                                if (!DBNull.Value.Equals(dr2["QueueLkp"]))
                                {
                                    objUserWorkQueues.QueueLkp = (long)dr2["QueueLkp"];
                                }
                            }
                            if (dr2.Table.Columns.Contains("QueueName"))
                            {
                                if (!DBNull.Value.Equals(dr2["QueueName"]))
                                {
                                    objUserWorkQueues.QueueName = dr2["QueueName"].ToString();
                                }
                            }
                            objUIUserLogin.UserQueueList.Add(objUserWorkQueues);
                        }
                    }
                    //User Preference
                    if (objDataSet.Tables.Count > 3)
                    {
                        objUIUserLogin.ADM_UserPreference = new DOADM_UserPreference();
                        foreach (DataRow dr3 in objDataSet.Tables[3].Rows)
                        {
                            if (dr3.Table.Columns.Contains("ADM_UserPreferenceId"))
                            {
                                if (!DBNull.Value.Equals(dr3["ADM_UserPreferenceId"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ADM_UserPreferenceId = (long)dr3["ADM_UserPreferenceId"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("ADM_UserMasterRef"))
                            {
                                if (!DBNull.Value.Equals(dr3["ADM_UserMasterRef"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ADM_UserMasterRef = (long)dr3["ADM_UserMasterRef"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("ShowAlerts"))
                            {
                                if (!DBNull.Value.Equals(dr3["ShowAlerts"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ShowAlerts = Convert.ToBoolean(dr3["ShowAlerts"]);
                                }
                            }
                            if (dr3.Table.Columns.Contains("ShowResources"))
                            {
                                if (!DBNull.Value.Equals(dr3["ShowResources"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ShowResources = Convert.ToBoolean(dr3["ShowResources"]);
                                }
                            }
                            if (dr3.Table.Columns.Contains("BusinessSegmentLkup"))
                            {
                                if (!DBNull.Value.Equals(dr3["BusinessSegmentLkup"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.BusinessSegmentLkup = (long)dr3["BusinessSegmentLkup"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("RoleLkup"))
                            {
                                if (!DBNull.Value.Equals(dr3["RoleLkup"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.RoleLkup = (long)dr3["RoleLkup"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("TimezoneLkup"))
                            {
                                if (!DBNull.Value.Equals(dr3["TimezoneLkup"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.TimezoneLkup = (long)dr3["TimezoneLkup"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("WorkBasketLkup"))
                            {
                                if (!DBNull.Value.Equals(dr3["WorkBasketLkup"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.WorkBasketLkup = (long)dr3["WorkBasketLkup"];
                                }
                            }
                            if (dr3.Table.Columns.Contains("ShowOSTSummary"))
                            {
                                if (!DBNull.Value.Equals(dr3["ShowOSTSummary"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ShowOSTSummary = Convert.ToBoolean(dr3["ShowOSTSummary"]);
                                }
                            }
                            if (dr3.Table.Columns.Contains("ShowEligibilitySummary"))
                            {
                                if (!DBNull.Value.Equals(dr3["ShowEligibilitySummary"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ShowEligibilitySummary = Convert.ToBoolean(dr3["ShowEligibilitySummary"]);
                                }
                            }
                            if (dr3.Table.Columns.Contains("ShowRPRSummary"))
                            {
                                if (!DBNull.Value.Equals(dr3["ShowRPRSummary"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.ShowRPRSummary = Convert.ToBoolean(dr3["ShowRPRSummary"]);

                                }
                            }
                            if (dr3.Table.Columns.Contains("IsActive"))
                            {
                                if (!DBNull.Value.Equals(dr3["IsActive"]))
                                {
                                    objUIUserLogin.ADM_UserPreference.IsActive = Convert.ToBoolean(dr3["IsActive"]);
                                }
                            }
                        }
                    }
                    if (objDataSet.Tables.Count >= 4)
                    {
                        objUIUserLogin.UserAccessGroup = new List<DOADM_AccessGroupMaster>();
                        DOADM_AccessGroupMaster objDOADM_AccessGroupMaster;
                        foreach (DataRow dr4 in objDataSet.Tables[4].Rows)
                        {
                            objDOADM_AccessGroupMaster = new DOADM_AccessGroupMaster();
                            if (dr4.Table.Columns.Contains("ADM_AccessGroupMasterId"))
                            {
                                if (!DBNull.Value.Equals(dr4["ADM_AccessGroupMasterId"]))
                                {
                                    objDOADM_AccessGroupMaster.ADM_AccessGroupMasterId = (long)dr4["ADM_AccessGroupMasterId"];
                                }
                            }
                            if (dr4.Table.Columns.Contains("AccessGroupName"))
                            {
                                if (!DBNull.Value.Equals(dr4["AccessGroupName"]))
                                {
                                    objDOADM_AccessGroupMaster.AccessGroupName = dr4["AccessGroupName"].ToString();
                                }
                            }
                            if (dr4.Table.Columns.Contains("AccessGroupDescription"))
                            {
                                if (!DBNull.Value.Equals(dr4["AccessGroupDescription"]))
                                {
                                    objDOADM_AccessGroupMaster.AccessGroupDescription = dr4["AccessGroupDescription"].ToString();
                                }
                            }
                            if (dr4.Table.Columns.Contains("RoleLkup"))
                            {
                                if (!DBNull.Value.Equals(dr4["RoleLkup"]))
                                {
                                    objDOADM_AccessGroupMaster.RoleLkup = (long)dr4["RoleLkup"];
                                }
                            }
                            if (dr4.Table.Columns.Contains("WorkBasketLkup"))
                            {
                                if (!DBNull.Value.Equals(dr4["WorkBasketLkup"]))
                                {
                                    objDOADM_AccessGroupMaster.WorkBasketLkup = (long)dr4["WorkBasketLkup"];
                                }
                            }
                            if (dr4.Table.Columns.Contains("DiscrepancyCategory"))
                            {
                                if (!DBNull.Value.Equals(dr4["DiscrepancyCategory"]))
                                {
                                    objDOADM_AccessGroupMaster.DiscrepancyCategory = (long)dr4["DiscrepancyCategory"];
                                }
                            }
                            objUIUserLogin.UserAccessGroup.Add(objDOADM_AccessGroupMaster);
                        }
                    }
                    if (objDataSet.Tables.Count > 5)
                    {
                        objUIUserLogin.UserReports = new List<DORPT_ReportsMaster>();
                        DORPT_ReportsMaster objDORPT_ReportsMaster;
                        foreach (DataRow dr5 in objDataSet.Tables[5].Rows)
                        {
                            objDORPT_ReportsMaster = new DORPT_ReportsMaster();
                            if (dr5.Table.Columns.Contains("RPT_ReportsMasterId"))
                            {
                                if (!DBNull.Value.Equals(dr5["RPT_ReportsMasterId"]))
                                {
                                    objDORPT_ReportsMaster.RPT_ReportsMasterId = (long)dr5["RPT_ReportsMasterId"];
                                }
                            }

                            if (dr5.Table.Columns.Contains("ReportName"))
                            {
                                if (!DBNull.Value.Equals(dr5["ReportName"]))
                                {
                                    objDORPT_ReportsMaster.ReportName = dr5["ReportName"].ToString();
                                }
                            }

                            if (dr5.Table.Columns.Contains("ReportServer"))
                            {
                                if (!DBNull.Value.Equals(dr5["ReportServer"]))
                                {
                                    objDORPT_ReportsMaster.ReportServer = dr5["ReportServer"].ToString();
                                }
                            }

                            if (dr5.Table.Columns.Contains("ReportURL"))
                            {
                                if (!DBNull.Value.Equals(dr5["ReportURL"]))
                                {
                                    objDORPT_ReportsMaster.ReportURL = dr5["ReportURL"].ToString();
                                }
                            }

                            if (dr5.Table.Columns.Contains("ReportsCategoryLkup"))
                            {
                                if (!DBNull.Value.Equals(dr5["ReportsCategoryLkup"]))
                                {
                                    objDORPT_ReportsMaster.ReportsCategoryLkup = (long)dr5["ReportsCategoryLkup"];
                                }
                            }

                            if (dr5.Table.Columns.Contains("ViewInUI"))
                            {
                                if (!DBNull.Value.Equals(dr5["ViewInUI"]))
                                {
                                    objDORPT_ReportsMaster.ViewInUI = (bool)dr5["ViewInUI"];
                                }
                            }
                            if (dr5.Table.Columns.Contains("RoleLkup"))
                            {
                                if (!DBNull.Value.Equals(dr5["RoleLkup"]))
                                {
                                    objDORPT_ReportsMaster.RoleLkup = (long)dr5["RoleLkup"];
                                }
                            }
                            if (dr5.Table.Columns.Contains("WorkBasketLkup"))
                            {
                                if (!DBNull.Value.Equals(dr5["WorkBasketLkup"]))
                                {
                                    objDORPT_ReportsMaster.WorkBasketLkup = (long)dr5["WorkBasketLkup"];
                                }
                            }
                            objUIUserLogin.UserReports.Add(objDORPT_ReportsMaster);
                        }
                    }
                }
            }
            catch
            {

            }
        }

        public ExceptionTypes SearchUser(long? TimeZone,DOADM_UserMaster objDOADM_UserMaster, out List<DOADM_UserMaster> lstDOADM_UserMaster, out string errorMessage)
        {
            lstDOADM_UserMaster = new List<DOADM_UserMaster>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsResultData = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.MSID))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MSID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.MSID;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.FullName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FullName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.FullName;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.Email))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@Email";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.Email;
                    parameters.Add(sqlParam);
                }
                if (!objDOADM_UserMaster.IsManager.IsNull() && objDOADM_UserMaster.IsManager==true)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsManager";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserMaster.IsManager;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.ADM_UserMasterId == 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsActive";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserMaster.IsActive;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.ADM_UserMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.ADM_UserMasterId;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_ERS_APP_SEL_UserMaster, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapUsersDOUserDetails(TimeZone,dsResultData, out lstDOADM_UserMaster);
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

        public ExceptionTypes SaveUser(DOADM_UserMaster objDOADM_UserMaster, out string errorMessage)
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

                if (objDOADM_UserMaster.ADM_UserMasterId != null && objDOADM_UserMaster.ADM_UserMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_UserMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.ADM_UserMasterId;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.FirstName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FirstName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.FirstName;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.LastName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@LastName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.LastName;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.FullName))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FullName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.FullName;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.MSID))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MSID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.MSID;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.Email))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@Email";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.Email;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.StartDate != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@StartDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOADM_UserMaster.StartDate;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.EndDate != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EndDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOADM_UserMaster.EndDate;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOADM_UserMaster.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsManager";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOADM_UserMaster.IsManager;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Title";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOADM_UserMaster.Titlelkup;
                parameters.Add(sqlParam);

                if (objDOADM_UserMaster.ManagerId != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ManagerId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.ManagerId;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.LocationLkup != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@LocationLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.LocationLkup;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.SpecialistTitle))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SpecialistTitle";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.SpecialistTitle;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.SpecialistPhone))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SpecialistPhone";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.SpecialistPhone;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.SpecialistFax))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SpecialistFax";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.SpecialistFax;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.SpecialistHours))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SpecialistHours";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.SpecialistHours;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.SpecialistTimeZone != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SpecialistTimeZone";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.SpecialistTimeZone;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.UserAddressLine1))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserAddressLine1";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.UserAddressLine1;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.UserAddressLine2))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserAddressLine2";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.UserAddressLine2;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.UserCity))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserCity";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.UserCity;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.UserStateLkup != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserStateLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.UserStateLkup;
                    parameters.Add(sqlParam);
                }

                if (!string.IsNullOrEmpty(objDOADM_UserMaster.UserZip))
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UserZip";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.UserZip;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.CreatedByRef != 0 && objDOADM_UserMaster.ADM_UserMasterId == 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CreatedBy";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.CreatedByRef;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.LastUpdatedByRef != null && objDOADM_UserMaster.LastUpdatedByRef != 0 && objDOADM_UserMaster.ADM_UserMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UpdatedBy";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserMaster.LastUpdatedByRef;
                    parameters.Add(sqlParam);
                }

                if (!objDOADM_UserMaster.CreatedByRoleLkup.IsNull() && objDOADM_UserMaster.ADM_UserMasterId == 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CreatedByRoleLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.CreatedByRoleLkup;
                    parameters.Add(sqlParam);
                }

                if (!objDOADM_UserMaster.UpdatedByRoleLkup.IsNull() && objDOADM_UserMaster.ADM_UserMasterId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@UpdatedByRoleLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserMaster.UpdatedByRoleLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.ADM_UserMasterId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ScreenLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = (long)ScreenType.UserAdmin;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation != null && objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation.Count() > 0)
                {
                    DataTable userAccessGroup;
                    SetUserAccessGroup(objDOADM_UserMaster.lstDOADM_AccessGroupUserCorrelation, out userAccessGroup);
                    DataTableReader dtrUserAccessGroup = new DataTableReader(userAccessGroup);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TV_UserAccessGroup";
                    sqlParam.SqlDbType = SqlDbType.Structured;
                    sqlParam.Value = dtrUserAccessGroup;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                if (objDOADM_UserMaster.ADM_UserMasterId == 0)
                {
                    executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_INS_ADM_UserMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                }
                else
                {
                    executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_ADM_UserMaster, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                }

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

        public ExceptionTypes SaveUserPreference(DOADM_UserPreference objDOADM_UserPreference, out string errorMessage)
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

                if (objDOADM_UserPreference.ADM_UserPreferenceId != null && objDOADM_UserPreference.ADM_UserPreferenceId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_UserPreferenceId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserPreference.ADM_UserPreferenceId;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.ADM_UserMasterRef != null && objDOADM_UserPreference.ADM_UserMasterRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ADM_UserMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOADM_UserPreference.ADM_UserMasterRef;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.ShowAlerts != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ShowAlerts";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.ShowAlerts;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.ShowResources != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ShowResources";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.ShowResources;
                    parameters.Add(sqlParam);
                }
                if (objDOADM_UserPreference.BusinessSegmentLkup != null && objDOADM_UserPreference.BusinessSegmentLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@BusinessSegmentLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserPreference.BusinessSegmentLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.RoleLkup != null && objDOADM_UserPreference.RoleLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RoleLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserPreference.RoleLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.TimezoneLkup != null && objDOADM_UserPreference.TimezoneLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TimezoneLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserPreference.TimezoneLkup;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.WorkBasketLkup != null && objDOADM_UserPreference.WorkBasketLkup != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkBasketLkup";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOADM_UserPreference.WorkBasketLkup;
                    parameters.Add(sqlParam);
                }


                if (objDOADM_UserPreference.ShowOSTSummary != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ShowOSTSummary";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.ShowOSTSummary;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.ShowEligibilitySummary != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ShowEligibilitySummary";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.ShowEligibilitySummary;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.ShowRPRSummary != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ShowRPRSummary";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.ShowRPRSummary;
                    parameters.Add(sqlParam);
                }

                if (objDOADM_UserPreference.IsActive != null)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsActive";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOADM_UserPreference.IsActive;
                    parameters.Add(sqlParam);
                }




                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                if (objDOADM_UserPreference.ADM_UserPreferenceId == 0)
                {
                    //USP_APP_INS_UPD_UserPreference
                    executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_ADM_UserPreference, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                }
                else
                {
                    executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_ADM_UserPreference, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                }

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

        private void SetUserAccessGroup(List<DOADM_AccessGroupUserCorrelation> lstDOADM_AccessGroupUserCorrelation, out DataTable userAccessGroup)
        {
            userAccessGroup = new DataTable("TVP_UserAccessGroup");
            GetDataColumn(userAccessGroup);
            DataRow rowUserAccessGroup;
            try
            {
                if (lstDOADM_AccessGroupUserCorrelation != null && lstDOADM_AccessGroupUserCorrelation.Count > 0)
                {
                    foreach (DOADM_AccessGroupUserCorrelation objUserAccessGroup in lstDOADM_AccessGroupUserCorrelation)
                    {
                        rowUserAccessGroup = userAccessGroup.NewRow();
                        rowUserAccessGroup["ADM_AccessGroupUserCorrelationId"] = objUserAccessGroup.ADM_AccessGroupUserCorrelationId;
                        rowUserAccessGroup["ADM_AccessGroupMasterRef"] = objUserAccessGroup.ADM_AccessGroupMasterRef;
                        rowUserAccessGroup["ADM_UserMasterRef"] = objUserAccessGroup.ADM_UserMasterRef;
                        rowUserAccessGroup["IsActive"] = objUserAccessGroup.IsActive;

                        userAccessGroup.Rows.Add(rowUserAccessGroup);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void GetDataColumn(DataTable dtUserAccessGroup)
        {
            DataColumn dcUserAccessGroup;
            dcUserAccessGroup = new DataColumn();
            dcUserAccessGroup.DataType = System.Type.GetType("System.Int64");
            dcUserAccessGroup.ColumnName = "ADM_AccessGroupUserCorrelationId";
            dcUserAccessGroup.ReadOnly = true;
            dtUserAccessGroup.Columns.Add(dcUserAccessGroup);

            dcUserAccessGroup = new DataColumn();
            dcUserAccessGroup.DataType = System.Type.GetType("System.Int64");
            dcUserAccessGroup.ColumnName = "ADM_AccessGroupMasterRef";
            dcUserAccessGroup.ReadOnly = true;
            dtUserAccessGroup.Columns.Add(dcUserAccessGroup);

            dcUserAccessGroup = new DataColumn();
            dcUserAccessGroup.DataType = System.Type.GetType("System.Int64");
            dcUserAccessGroup.ColumnName = "ADM_UserMasterRef";
            dcUserAccessGroup.ReadOnly = true;
            dtUserAccessGroup.Columns.Add(dcUserAccessGroup);

            dcUserAccessGroup = new DataColumn();
            dcUserAccessGroup.DataType = System.Type.GetType("System.Boolean");
            dcUserAccessGroup.ColumnName = "IsActive";
            dcUserAccessGroup.ReadOnly = true;
            dtUserAccessGroup.Columns.Add(dcUserAccessGroup);
        }

        public ExceptionTypes LoginUser(string loginName)
        {
            try
            {
                DAHelper dah = new DAHelper();
                DataSet dsResult;
                string errorMsg;
                long errorCode;
                long errorNumber;
                SqlParameter[] sParameter = new SqlParameter[1];
                sParameter[0] = new SqlParameter("@LoginName", loginName);
                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_ERS_UserLogin, sParameter, out dsResult, out errorCode, out errorNumber, out errorMsg);

                if ((executionResult != (long)ExceptionTypes.Success && executionResult != (long)ExceptionTypes.ZeroRecords) || !string.IsNullOrEmpty(errorMsg))
                {

                    return ExceptionTypes.ApplicationException;
                }

                return (ExceptionTypes)executionResult;
            }
            catch (Exception ex)
            {

            }
            return ExceptionTypes.ApplicationException;
        }

        public ExceptionTypes UserLogout(string loginName)
        {
            try
            {
                DAHelper dah = new DAHelper();
                DataSet dsResult;
                string errorMsg;
                long errorCode;
                long errorNumber;
                SqlParameter[] sParameter = new SqlParameter[1];
                sParameter[0] = new SqlParameter("@LoginName", loginName);
                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_ERS_UserLogout, sParameter, out dsResult, out errorCode, out errorNumber, out errorMsg);

                if ((executionResult != (long)ExceptionTypes.Success && executionResult != (long)ExceptionTypes.ZeroRecords) || !string.IsNullOrEmpty(errorMsg))
                {
                    return ExceptionTypes.ApplicationException;
                }

                return (ExceptionTypes)executionResult;
            }
            catch (Exception ex)
            {

            }
            return ExceptionTypes.ApplicationException;
        }

        public ExceptionTypes ReassignUserList(long? TimeZone,string Gen_QueueIds, out List<DOADM_UserMaster> lstDOADM_UserMaster, out string errorMessage)
        {
            lstDOADM_UserMaster = new List<DOADM_UserMaster>();
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
                sqlParam.ParameterName = "@GEN_QueueIds";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = Gen_QueueIds.NullToString();
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_ReassignUserList, parameters.ToArray(), out dsResultData, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsResultData.Tables.Count > 0 && dsResultData.Tables[0].Rows.Count > 0)
                    {
                        MapUsersDOUserDetails(TimeZone,dsResultData, out lstDOADM_UserMaster);
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
