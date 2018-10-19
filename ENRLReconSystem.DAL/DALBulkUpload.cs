using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data.SqlClient;
using System.Data;

namespace ENRLReconSystem.DAL
{
    public class DALBulkUpload
    {
        DAHelper _daHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;

        public DALBulkUpload()
        {

        }

        public ExceptionTypes GetBulkImportExcelTemplate(out UIDOGEN_BulkImportExcelTemplate objUIDOGEN_BulkImportExcelTemplate, out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            objUIDOGEN_BulkImportExcelTemplate = new UIDOGEN_BulkImportExcelTemplate();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_BulkImportExcelTemplate, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapGEN_BulkImportExcelTemplate(_dsResult, out objUIDOGEN_BulkImportExcelTemplate);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }
        public ExceptionTypes GetBulkUploadSearchResult(long? TimeZone,UIBulkUploadSearch objUIBulkUploadSearch, out List<DOGEN_BulkImport> lstDOGEN_BulkImport, out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            try
            {
                if (!objUIBulkUploadSearch.BulkImportID.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_BulkImportId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkUploadSearch.BulkImportID;
                    _lstParameters.Add(sqlParam);
                }
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TemplateTypeLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkUploadSearch.TemplateTypeLkup;
                    _lstParameters.Add(sqlParam);

                if (!objUIBulkUploadSearch.WorkbasketLkup.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@WorkbasketLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkUploadSearch.WorkbasketLkup;
                    _lstParameters.Add(sqlParam);
                }
                if (!objUIBulkUploadSearch.DiscrepancyCategoryLkup.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objUIBulkUploadSearch.DiscrepancyCategoryLkup;
                    _lstParameters.Add(sqlParam);
                }

                if (!objUIBulkUploadSearch.StartDate.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@StartDate";
                    sqlParam.SqlDbType = SqlDbType.Date;
                    sqlParam.Value = objUIBulkUploadSearch.StartDate;
                    _lstParameters.Add(sqlParam);
                }
                if (!objUIBulkUploadSearch.EndDate.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EndDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objUIBulkUploadSearch.EndDate;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_BulkImport, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);
                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapGetBulkImport(TimeZone,_dsResult, out lstDOGEN_BulkImport);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }

        public ExceptionTypes SaveBulkUpload(DOGEN_BulkImport objDOGEN_BulkImport,long loginUserID, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.WorkBasketLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.DiscrepancyCategoryLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_BulkImportExcelTemplateMasterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.GEN_BulkImportExcelTemplateMasterRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.ExcelFileName;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DuplicateFileName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.DuplicateFileName;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ExcelFilelPath";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_BulkImport.ExcelFilelPath;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ImportStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_BulkImport.ImportStatusLkup;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = loginUserID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_BulkImportID";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Direction = ParameterDirection.Output;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_GEN_BulkImport, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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

        private void MapGEN_BulkImportExcelTemplate(DataSet dsResult, out UIDOGEN_BulkImportExcelTemplate objUIDOGEN_BulkImportExcelTemplate)
        {
            try
            {
                objUIDOGEN_BulkImportExcelTemplate = new UIDOGEN_BulkImportExcelTemplate();
                if (dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportExcelTemplateMaster = _dsResult.Tables[0].AsEnumerable().Select(dr => new DOGEN_BulkImportExcelTemplateMaster
                    {
                        GEN_BulkImportExcelTemplateMasterId = (!dr["GEN_BulkImportExcelTemplateMasterId"].IsNull()) ? dr["GEN_BulkImportExcelTemplateMasterId"].ToInt64() : 0,
                        WorkBasketLkup = (!dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                        ExcelTemplateName = (!dr["ExcelTemplateName"].IsNull()) ? dr["ExcelTemplateName"].NullToString() : string.Empty,
                        ExcelTemplateDescription = (!dr["ExcelTemplateDescription"].IsNull()) ? dr["ExcelTemplateDescription"].NullToString() : string.Empty,
                        SheetName = (!dr["SheetName"].IsNull()) ? dr["SheetName"].NullToString() : string.Empty,
                        StartRow = (!dr["StartRow"].IsNull()) ? dr["StartRow"].ToInt32() : 0,
                        StartColumn = (!dr["StartColumn"].IsNull()) ? dr["StartColumn"].ToInt32() : 0,
                        ExcelDirectoryPath = (!dr["ExcelDirectoryPath"].IsNull()) ? dr["ExcelDirectoryPath"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        CustomValidationSP = (!dr["CustomValidationSP"].IsNull()) ? dr["CustomValidationSP"].NullToString() : string.Empty,
                        StagingTableName = (!dr["StagingTableName"].IsNull()) ? dr["StagingTableName"].NullToString() : string.Empty,
                        StagingInsertSPName = (!dr["StagingInsertSPName"].IsNull()) ? dr["StagingInsertSPName"].NullToString() : string.Empty,
                        StagingUpdateSPName = (!dr["StagingUpdateSPName"].IsNull()) ? dr["StagingUpdateSPName"].NullToString() : string.Empty,
                        StagingInsertTVPName = (!dr["StagingInsertTVPName"].IsNull()) ? dr["StagingInsertTVPName"].NullToString() : string.Empty,
                        StagingUpdateTVPName = (!dr["StagingUpdateTVPName"].IsNull()) ? dr["StagingUpdateTVPName"].NullToString() : string.Empty,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        TemplateTypeLkup = (!dr["TemplateTypeLkup"].IsNull()) ? dr["TemplateTypeLkup"].ToInt64() : (long?)null,
                        BusinessSegmentLkup =  (!dr["BusinessSegmentLkup"].IsNull()) ? dr["BusinessSegmentLkup"].ToInt64() : (long?)null
                    }).ToList();
                }
                if (dsResult != null && _dsResult.Tables.Count > 1 && _dsResult.Tables[1].Rows.Count > 0)
                {
                    objUIDOGEN_BulkImportExcelTemplate.lstDOGEN_BulkImportColumnsMapping = _dsResult.Tables[1].AsEnumerable().Select(dr => new DOGEN_BulkImportColumnsMapping
                    {
                        GEN_BulkImportColumnsMappingId = (!dr["GEN_BulkImportColumnsMappingId"].IsNull()) ? dr["GEN_BulkImportColumnsMappingId"].ToInt64() : 0,
                        GEN_BulkImportColumnsMappingParentRef = (!dr["GEN_BulkImportColumnsMappingParentRef"].IsNull()) ? dr["GEN_BulkImportColumnsMappingParentRef"].ToInt64() : 0,
                        GEN_BulkImportExcelTemplateMasterRef = (!dr["GEN_BulkImportExcelTemplateMasterRef"].IsNull()) ? dr["GEN_BulkImportExcelTemplateMasterRef"].ToInt64() : 0,
                        ColumnTypeLkup = (!dr["ColumnTypeLkup"].IsNull()) ? dr["ColumnTypeLkup"].ToInt64() : 0,
                        DBColumnName = (!dr["DBColumnName"].IsNull()) ? dr["DBColumnName"].NullToString() : string.Empty,
                        ColumnDisplayName = (!dr["ColumnDisplayName"].IsNull()) ? dr["ColumnDisplayName"].NullToString() : string.Empty,
                        MaxLength = (!dr["MaxLength"].IsNull()) ? dr["MaxLength"].ToInt32() : 0,
                        ColumnSequence = (!dr["ColumnSequence"].IsNull()) ? dr["ColumnSequence"].ToInt32() : 0,
                        ControlTypeLkup = (!dr["ControlTypeLkup"].IsNull()) ? dr["ControlTypeLkup"].ToInt64() : 0,
                        ControlLkupValue = (!dr["ControlLkupValue"].IsNull()) ? dr["ControlLkupValue"].ToInt64() : 0,
                        IsUniqueKey = (!dr["IsUniqueKey"].IsNull()) ? dr["IsUniqueKey"].ToBoolean() : false,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        TemplateTypeLkup = (!dr["TemplateTypeLkup"].IsNull()) ? dr["TemplateTypeLkup"].ToInt64() : (long?)null,
                        BusinessSegmentLkup = (!dr["BusinessSegmentLkup"].IsNull()) ? dr["BusinessSegmentLkup"].ToInt64() : (long?)null
                    }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void MapGetBulkImport(long? TimeZone,DataSet dsResult, out List<DOGEN_BulkImport> lstDOGEN_BulkImport)
        {
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
            try
            {
                if (dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    lstDOGEN_BulkImport = _dsResult.Tables[0].AsEnumerable().Select(dr => new DOGEN_BulkImport
                    {
                        GEN_BulkImportId = (!dr["GEN_BulkImportId"].IsNull()) ? dr["GEN_BulkImportId"].ToInt64() : 0,
                        WorkBasketLkup = (!dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0,
                        WorkBasket = (!dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        GEN_BulkImportExcelTemplateMasterRef = (!dr["GEN_BulkImportExcelTemplateMasterRef"].IsNull()) ? dr["GEN_BulkImportExcelTemplateMasterRef"].ToInt64() : 0,
                        LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0,
                        UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null,
                        ExcelFileName = (!dr["ExcelFileName"].IsNull()) ? dr["ExcelFileName"].NullToString() : string.Empty,
                        DuplicateFileName = (!dr["DuplicateFileName"].IsNull()) ? dr["DuplicateFileName"].NullToString() : string.Empty,
                        ExcelFilelPath = (!dr["ExcelFilelPath"].IsNull()) ? dr["ExcelFilelPath"].NullToString() : string.Empty,
                        TotalRecordsCount = (!dr["TotalRecordsCount"].IsNull()) ? dr["TotalRecordsCount"].ToInt32() : 0,
                        ValidRecordsCount = (!dr["ValidRecordsCount"].IsNull()) ? dr["ValidRecordsCount"].ToInt32() : 0,
                        InvalidRecordsCount = (!dr["InvalidRecordsCount"].IsNull()) ? dr["InvalidRecordsCount"].ToInt32() : 0,
                        DuplicateRecordCount = (!dr["DuplicateRecordCount"].IsNull()) ? dr["DuplicateRecordCount"].ToInt32() : 0,
                        ErrorDescription = (!dr["ErrorDescription"].IsNull()) ? dr["ErrorDescription"].NullToString() : string.Empty,
                        ExcelStatusLkup = (!dr["ExcelStatusLkup"].IsNull()) ? dr["ExcelStatusLkup"].ToInt64() : 0,
                        ExcelStatus = (!dr["ExcelStatus"].IsNull()) ? dr["ExcelStatus"].NullToString() : string.Empty,
                        ImportStatusLkup = (!dr["ImportStatusLkup"].IsNull()) ? dr["ImportStatusLkup"].ToInt64() : 0,
                        ImportStatus = (!dr["ImportStatus"].IsNull()) ? dr["ImportStatus"].NullToString() : string.Empty,
                        CMN_AppErrorLogRef = (!dr["CMN_AppErrorLogRef"].IsNull()) ? dr["CMN_AppErrorLogRef"].ToInt64() : 0,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
