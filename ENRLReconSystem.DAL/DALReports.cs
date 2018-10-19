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
    public class DALReports
    {
        public ExceptionTypes GetAllReports(long? lRptIdout,string sReportName, out List<DORPT_ReportsMaster> lstDORPT_ReportsMaster, out string errorMessage)
        {
            lstDORPT_ReportsMaster = new List<DORPT_ReportsMaster>();
            errorMessage = string.Empty;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (lRptIdout > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@RPT_ReportsMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lRptIdout;
                    parameters.Add(sqlParam);
                }
                if (sReportName!=string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ReportName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = sReportName;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_Reports, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapObjectProperties(dsTable, out lstDORPT_ReportsMaster);
                        if (lstDORPT_ReportsMaster.Count > 0)
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
        private void MapObjectProperties(DataSet dstTable,out List<DORPT_ReportsMaster> lstDORPT_ReportsMaster)
        {
            lstDORPT_ReportsMaster = new List<DORPT_ReportsMaster>();

            if(dstTable.Tables.Count>0)
            {
                DORPT_ReportsMaster objDORPT_ReportsMaster;
                foreach (DataRow dr in dstTable.Tables[0].Rows)
                {
                    objDORPT_ReportsMaster = new DORPT_ReportsMaster();
                    if (dr.Table.Columns.Contains("RPT_ReportsMasterId"))
                    {
                        if (!DBNull.Value.Equals(dr["RPT_ReportsMasterId"]))
                        {
                            objDORPT_ReportsMaster.RPT_ReportsMasterId = (long)dr["RPT_ReportsMasterId"];
                        }
                    }
                    
                    if (dr.Table.Columns.Contains("ReportName"))
                    {
                        if (!DBNull.Value.Equals(dr["ReportName"]))
                        {
                            objDORPT_ReportsMaster.ReportName = dr["ReportName"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("ReportServer"))
                    {
                        if (!DBNull.Value.Equals(dr["ReportServer"]))
                        {
                            objDORPT_ReportsMaster.ReportServer = dr["ReportServer"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ReportURL"))
                    {
                        if (!DBNull.Value.Equals(dr["ReportURL"]))
                        {
                            objDORPT_ReportsMaster.ReportURL = dr["ReportURL"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("ReportsCategoryLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["ReportsCategoryLkup"]))
                        {
                            objDORPT_ReportsMaster.ReportsCategoryLkup =(long) dr["ReportsCategoryLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("ViewInUI"))
                    {
                        if (!DBNull.Value.Equals(dr["ViewInUI"]))
                        {
                            if (dr["ViewInUI"].ToString() == "True")
                                objDORPT_ReportsMaster.ViewInUI = true;
                            else
                                objDORPT_ReportsMaster.ViewInUI = false;
                        }
                    }
                    if (dr.Table.Columns.Contains("IsActive"))
                    {
                        if (!DBNull.Value.Equals(dr["IsActive"]))
                        {
                            if (dr["IsActive"].ToString() == "True")
                                objDORPT_ReportsMaster.IsActive = true;
                            else
                                objDORPT_ReportsMaster.IsActive = false;
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objDORPT_ReportsMaster.UTCCreatedOn = Convert.ToDateTime(dr["UTCCreatedOn"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                        {
                            objDORPT_ReportsMaster.CreatedByRef = Convert.ToInt64(dr["CreatedByRef"]);
                        }
                    }
                    lstDORPT_ReportsMaster.Add(objDORPT_ReportsMaster);
                }
            }
        }
    }
}
