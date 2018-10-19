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
    public class DALLookup
    {
        DAHelper _daHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;

        /// <summary>
        /// Get All Lookup For LookUp Type Search
        /// </summary>
        /// <param name="strDescription"></param>
        /// <param name="isActive"></param>
        /// <param name="lstDOCMN_LookupType"></param>
        /// <returns></returns>
        public ExceptionTypes GetAllLookupTypes(long? TimeZone,string strDescription, bool isActive, out List<DOCMN_LookupType> lstDOCMN_LookupType)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            string errorMessage = string.Empty;
            lstDOCMN_LookupType = new List<DOCMN_LookupType>();
            try
            {
                if (strDescription != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@LookupTypeDescription";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = strDescription;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = isActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Lookups, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapAllLookupTypes(TimeZone,_dsResult, out lstDOCMN_LookupType);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception)
            {
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }
        /// <summary>
        /// GetLookupMasterByLkupTypeID 
        /// </summary>
        /// <param name="lookupTypeId"></param>
        /// <param name="lstDOCMN_LookupType"></param>
        /// <returns></returns>
        public ExceptionTypes GetLookupMasterByLkupTypeID(long? lookupTypeId, out DOCMN_LookupType objDOCMN_LookupType)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            string errorMessage = string.Empty;
            objDOCMN_LookupType = new DOCMN_LookupType();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupTypeId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lookupTypeId;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Lookups, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                if (executionResult == 0)
                    return MapLookupMaster(_dsResult, out objDOCMN_LookupType);

                return ExceptionTypes.SqlException;

            }
            catch (Exception)
            {

                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstParameters = null;
            }
        }
        public ExceptionTypes GetAllLookups(long? lookupTypeId, out List<DOCMN_LookupType> lstLookupType, out List<DOCMN_LookupMaster> lstLookupMaster) //out List<DOCMN_LookupMaster> lstDOCMN_LookupMaster)
        {
            lstLookupType = new List<DOCMN_LookupType>();
            lstLookupMaster = new List<DOCMN_LookupMaster>();

            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                _dsResult = new DataSet();
                string errorMessage;

                _lstParameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                if (lookupTypeId != null && lookupTypeId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeId";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = lookupTypeId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Lookups, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (_dsResult.Tables.Count > 0 && _dsResult.Tables[1].Rows.Count > 0)
                    {
                        MapAllLookups(_dsResult, out lstLookupType, out lstLookupMaster);
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
            catch
            {
                //need log
                return ExceptionTypes.UnknownError;
            }
        }
        /// <summary>
        /// Save/Update LookupType
        /// </summary>
        /// <param name="objDOCMN_LookupType"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes SaveLookupType(DOCMN_LookupType objDOCMN_LookupType, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;

            try
            {
                if (objDOCMN_LookupType.CMN_LookupTypeId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupType.CMN_LookupTypeId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LookupTypeDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_LookupType.LookupTypeDescription;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_LookupType.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = ScreenType.LookupType;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (objDOCMN_LookupType.CMN_LookupTypeId > 0) ? objDOCMN_LookupType.LastUpdatedByRef : objDOCMN_LookupType.CreatedByRef;
                _lstParameters.Add(sqlParam);



                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_UPD_LookupType, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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
        public ExceptionTypes SaveLookupMaster(DOCMN_LookupMaster objDOCMN_LookupMaster, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;
            try
            {
                if (objDOCMN_LookupMaster.CMN_LookupMasterId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupMasterId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupMaster.CMN_LookupMasterId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupTypeRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupMaster.CMN_LookupTypeRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LookupValue";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_LookupMaster.LookupValue;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LookupDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_LookupMaster.LookupDescription;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DisplayOrder";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupMaster.DisplayOrder;
                _lstParameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = (objDOCMN_LookupMaster.CMN_LookupMasterId > 0) ? objDOCMN_LookupMaster.LastUpdatedByRef : objDOCMN_LookupMaster.CreatedByRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_LookupMaster.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_UPD_LookupMaster, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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

        /// <summary>
        /// This method is for Caching purpose (Get all lookup type & lookup Master)
        /// </summary>
        /// <param name="objDataTable"></param>
        /// <param name="lstLookupType"></param>
        /// <param name="lstLookupMaster"></param>
        private void MapAllLookups(DataSet dsResult, out List<DOCMN_LookupType> lstLookupType, out List<DOCMN_LookupMaster> lstLookupMaster)
        {
            lstLookupType = new List<DOCMN_LookupType>();
            lstLookupMaster = new List<DOCMN_LookupMaster>();

            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    lstLookupType = dsResult.Tables[0].AsEnumerable().Select(dr => new DOCMN_LookupType
                    {
                        CMN_LookupTypeId = dr["CMN_LookupTypeId"].ToInt64(),
                        LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0,
                        UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLockedOn"]) : (DateTime?)null,
                        LookupTypeDescription = (!dr["LookupTypeDescription"].IsNull()) ? Convert.ToString(dr["LookupTypeDescription"]) : string.Empty,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? Convert.ToInt64(dr["LastUpdatedByRef"]) : 0,
                        LockedByName = (!dr["LockedByName"].IsNull()) ? dr["LockedByName"].NullToString() : string.Empty,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = dr["IsActive"].ToBoolean()
                    }).ToList();
                }

                lstLookupMaster = dsResult.Tables[1].AsEnumerable().Select(dr => new DOCMN_LookupMaster
                {
                    CMN_LookupMasterId = dr["CMN_LookupMasterId"].ToInt64(),
                    CMN_LookupTypeRef = dr["CMN_LookupTypeRef"].ToInt64(),
                    LookupDescription = (!dr["LookupDescription"].IsNull()) ? Convert.ToString(dr["LookupDescription"]) : string.Empty,
                    LookupValue = (!dr["LookupValue"].IsNull()) ? dr["LookupValue"].NullToString() : string.Empty,
                    LookupValue1 = (!dr["LookupValue1"].IsNull()) ? dr["LookupValue1"].NullToString() : string.Empty,
                    LookupValue2 = (!dr["LookupValue2"].IsNull()) ? dr["LookupValue2"].NullToString() : string.Empty,
                    DisplayOrder = (!dr["DisplayOrder"].IsNull()) ? dr["DisplayOrder"].ToInt64() : (Int64?)null,
                    UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow,
                    CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                    UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : DateTime.UtcNow,
                    LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : (Int64?)null,
                    IsActive = dr["IsActive"].ToBoolean()
                }).ToList();
            }
            catch (Exception ex)
            {
                //need log
                throw ex;
            }
        }

        /// <summary>
        /// For Lookup Screen (To get Lookpu type & related lookup Master)
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="lstDOCMN_LookupType"></param>
        /// <returns></returns>
        private void MapAllLookupTypes(long? TimeZone,DataSet dsResult, out List<DOCMN_LookupType> lstDOCMN_LookupType)
        {
            lstDOCMN_LookupType = new List<DOCMN_LookupType>();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    lstDOCMN_LookupType = dsResult.Tables[0].AsEnumerable().Select(dr => new DOCMN_LookupType
                    {
                        CMN_LookupTypeId = dr["CMN_LookupTypeId"].ToInt64(),
                        LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() :0,
                        UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LookupTypeDescription = (!dr["LookupTypeDescription"].IsNull()) ? dr["LookupTypeDescription"].NullToString() : string.Empty,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(), TimeZone) : (DateTime?)null,                       
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64(): 0,
                        UTCLastUpdatedOn = Convert.ToDateTime(dr["UTCLastUpdatedOn"]),
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : (Int64?)null,
                        LockedByName = (!dr["LockedByName"].IsNull()) ? dr["LockedByName"].NullToString() : string.Empty,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = dr["IsActive"].ToBoolean()
                        
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// For Lookup Screen (To get Lookpu type & related lookup Master)
        /// </summary>
        /// <param name="dsResult"></param>
        /// <param name="objDOCMN_LookupType"></param>
        /// <returns></returns>
        private ExceptionTypes MapLookupMaster(DataSet dsResult, out DOCMN_LookupType objDOCMN_LookupType)
        {
            objDOCMN_LookupType = new DOCMN_LookupType();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];
                    objDOCMN_LookupType.CMN_LookupTypeId = dr["CMN_LookupTypeId"].ToInt64();
                    objDOCMN_LookupType.LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOCMN_LookupType.UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLockedOn"]) : (DateTime?)null;
                    objDOCMN_LookupType.LookupTypeDescription = (!dr["LookupTypeDescription"].IsNull()) ? dr["LookupTypeDescription"].NullToString() : string.Empty;
                    objDOCMN_LookupType.UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow;
                    objDOCMN_LookupType.CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOCMN_LookupType.UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : DateTime.UtcNow;
                    objDOCMN_LookupType.LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOCMN_LookupType.LockedByName = (!dr["LockedByName"].IsNull()) ? dr["LockedByName"].NullToString() : string.Empty;
                    objDOCMN_LookupType.CreatedByName = (!dr["CreatedByName"].IsNull()) ?dr["CreatedByName"].NullToString() : string.Empty;
                    objDOCMN_LookupType.LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty;
                    objDOCMN_LookupType.IsActive = Convert.ToBoolean(dr["IsActive"]);
                }
                if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    objDOCMN_LookupType.lstDOCMN_LookupMaster = dsResult.Tables[1].AsEnumerable().Select(dr => new DOCMN_LookupMaster
                    {
                        CMN_LookupMasterId = Convert.ToInt64(dr["CMN_LookupMasterId"]),
                        CMN_LookupTypeRef = Convert.ToInt64(dr["CMN_LookupTypeRef"]),
                        LookupDescription = (!dr["LookupDescription"].IsNull()) ? dr["LookupDescription"].NullToString() : string.Empty,
                        LookupValue = (!dr["LookupValue"].IsNull()) ? dr["LookupValue"].NullToString() : string.Empty,
                        LookupValue1 = (!dr["LookupValue1"].IsNull()) ? dr["LookupValue1"].NullToString() : string.Empty,
                        LookupValue2 = (!dr["LookupValue2"].IsNull()) ? dr["LookupValue2"].NullToString() : string.Empty,
                        DisplayOrder = (!dr["DisplayOrder"].IsNull()) ? dr["DisplayOrder"].ToInt64() : (Int64?)null,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCCreatedOn"]) : DateTime.UtcNow,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? Convert.ToDateTime(dr["UTCLastUpdatedOn"]) : DateTime.UtcNow,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : (Int64?)null,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = dr["IsActive"].ToBoolean()
                    }).ToList();
                }
                return ExceptionTypes.Success;
            }
            catch (Exception ex)
            {
                //need log
                throw ex;
            }
        }
    }
}
