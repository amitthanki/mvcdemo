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
    public class DALLookupCorrelations
    {
        DAHelper _daHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;

        public ExceptionTypes GetAllLookupTypeCorrelations(long? TimeZone,DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations,out string  errorMessage)
        {
            lstDOCMN_LookupTypeCorrelations = new List<DOCMN_LookupTypeCorrelations>();
            List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations;

            ExceptionTypes returnValue = GetAllLookupTypeCorrelations(TimeZone,objDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupMasterCorrelations,out errorMessage);
            return returnValue;
        }

        /// <summary>
        /// Search LookupTypeCorrelations
        /// </summary>
        /// <param name="lstDOCMN_LookupTypeCorrelations"></param>
        /// <returns></returns>
        public ExceptionTypes GetAllLookupTypeCorrelations(long? TimeZone,DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations,
                                                           out List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations, out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            lstDOCMN_LookupTypeCorrelations = new List<DOCMN_LookupTypeCorrelations>();
            lstDOCMN_LookupMasterCorrelations = new List<DOCMN_LookupMasterCorrelations>();
            try
            {

                if (objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId;
                    _lstParameters.Add(sqlParam);

                }
                if (objDOCMN_LookupTypeCorrelations.CorrelationDescription != null && objDOCMN_LookupTypeCorrelations.CorrelationDescription != string.Empty)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CorrelationDescription";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOCMN_LookupTypeCorrelations.CorrelationDescription;
                    _lstParameters.Add(sqlParam);
                }
                if (objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeParentRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_LookupTypeCorrelations.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_LookupsCorrelation, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapAllLookupTypeCorrelations(TimeZone,_dsResult, out lstDOCMN_LookupTypeCorrelations);
                    MapAllLookupMasterCorrelations(TimeZone,_dsResult, out lstDOCMN_LookupMasterCorrelations);
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

        /// <summary>
        /// Get Correlation Master Popup Data
        /// </summary>
        /// <param name="lkupCorelationMasterID"></param>
        /// <param name="objDOCMN_LookupMasterCorrelationsExtended"></param>
        /// <returns></returns>
        public ExceptionTypes GetCorrelationMasterByID(long lkupCorelationTypeID, long lkupCorelationMasterID, out DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended,out string  errorMessage)
        {
            _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            objDOCMN_LookupMasterCorrelationsExtended = new DOCMN_LookupMasterCorrelationsExtended();
            try
            {
                if (lkupCorelationTypeID > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lkupCorelationTypeID;
                    _lstParameters.Add(sqlParam);
                }
                if (lkupCorelationMasterID > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupMasterCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lkupCorelationMasterID;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_LookupsCorrelation, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapLookupCorelationMasterByID(_dsResult, lkupCorelationTypeID, lkupCorelationMasterID, out objDOCMN_LookupMasterCorrelationsExtended);
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



        /// <summary>
        /// Get LookuptypeCorrelation and Master as list by lookupTypeCorrelationsId
        /// </summary>
        /// <param name="lookupTypeCorrelationsId"></param>
        /// <param name="objDOCMN_LookupTypeCorrelations"></param>
        /// <returns></returns>
        public ExceptionTypes GetLookupCorelationByID(long lookupTypeCorrelationsId, out DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations,out string errorMessage)
        {

                _dsResult = new DataSet();
                _lstParameters = new List<SqlParameter>();
                SqlParameter sqlParam;
                long lErrocode = 0;
                long lErrorNumber = 0;
                errorMessage = string.Empty;
                objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
                try
                {

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lookupTypeCorrelationsId;
                    _lstParameters.Add(sqlParam);

                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ErrorMessage";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = string.Empty;
                    sqlParam.Direction = ParameterDirection.Output;
                    sqlParam.Size = 2000;
                    _lstParameters.Add(sqlParam);

                    long executionResult = _daHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_LookupsCorrelation, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                    sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                    if (sqlParam != null && sqlParam.Value != null)
                    {
                        errorMessage += sqlParam.Value.ToString();
                    }

                    if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                    {
                        MapLookupCorelationByID(_dsResult, out objDOCMN_LookupTypeCorrelations);
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
        /// <summary>
        /// Save LookupType Correlation
        /// </summary>
        /// <param name="objDOCMN_LookupTypeCorrelations"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes SaveLookupTypeCorrelation(DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;

            try
            {
                if (objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupTypeCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CorrelationDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_LookupTypeCorrelations.CorrelationDescription;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupTypeParentRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupTypeChildRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupTypeCorrelations.CMN_LookupTypeChildRef;
                _lstParameters.Add(sqlParam);



                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId > 0) ? objDOCMN_LookupTypeCorrelations.LastUpdatedByRef : objDOCMN_LookupTypeCorrelations.CreatedByRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_LookupTypeCorrelations.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ScreenLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = ScreenType.LookupTypeCorrelation;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_UPD_LookupTypeCorrelation, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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
        public ExceptionTypes SaveCorrelationMaster(DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended, out string errorMessage)
        {
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            long lRowsEffected = 0;

            try
            {
                if (objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterCorrelationsId > 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_LookupMasterCorrelationsId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterCorrelationsId;
                    _lstParameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupTypeCorrelationsRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CorrelationDescription";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CorrelationDescription;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupMasterParentRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterParentRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMN_LookupMasterChildRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterChildRef;
                _lstParameters.Add(sqlParam);



                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = (objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterCorrelationsId > 0) ? objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.LastUpdatedByRef : objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CreatedByRef;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.IsActive;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _daHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_UPD_LookupMasterCorrelation, _lstParameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

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

        private void MapLookupCorelationByID(DataSet dsResult, out DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations)
        {
            objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
            objDOCMN_LookupTypeCorrelations.lstDOCMN_LookupMasterCorrelations = new List<DOCMN_LookupMasterCorrelations>();
            try
            {
                if (_dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = _dsResult.Tables[0].Rows[0];
                    objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId = (!dr["CMN_LookupTypeCorrelationsId"].IsNull()) ? dr["CMN_LookupTypeCorrelationsId"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef = (!dr["CMN_LookupTypeParentRef"].IsNull()) ? dr["CMN_LookupTypeParentRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentValue = (!dr["CMN_LookupTypeParentValue"].IsNull()) ? dr["CMN_LookupTypeParentValue"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.CMN_LookupTypeChildRef = (!dr["CMN_LookupTypeChildRef"].IsNull()) ? dr["CMN_LookupTypeChildRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.CMN_LookupTypeChildValue = (!dr["CMN_LookupTypeChildValue"].IsNull()) ? dr["CMN_LookupTypeChildValue"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.GroupingLookupTypeRef = (!dr["GroupingLookupTypeRef"].IsNull()) ? dr["GroupingLookupTypeRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.GroupingLookupTypeValue = (!dr["GroupingLookupTypeValue"].IsNull()) ? dr["GroupingLookupTypeValue"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.CorrelationDescription = (!dr["CorrelationDescription"].IsNull()) ? dr["CorrelationDescription"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.LockedByName = (!dr["LockedByName"].IsNull()) ? dr["LockedByName"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : DateTime.UtcNow;
                    objDOCMN_LookupTypeCorrelations.IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOCMN_LookupTypeCorrelations.UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : DateTime.UtcNow;
                    objDOCMN_LookupTypeCorrelations.CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty;
                    objDOCMN_LookupTypeCorrelations.UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : DateTime.UtcNow;
                    objDOCMN_LookupTypeCorrelations.LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOCMN_LookupTypeCorrelations.LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty;

                }
                if (_dsResult != null && _dsResult.Tables.Count > 1 && _dsResult.Tables[1].Rows.Count > 0)
                {
                    objDOCMN_LookupTypeCorrelations.lstDOCMN_LookupMasterCorrelations = _dsResult.Tables[1].AsEnumerable().Select(dr => new DOCMN_LookupMasterCorrelations
                    {
                        CMN_LookupMasterCorrelationsId = (!dr["CMN_LookupMasterCorrelationsId"].IsNull()) ? dr["CMN_LookupMasterCorrelationsId"].ToInt64() : 0,
                        CMN_LookupTypeCorrelationsRef = (!dr["CMN_LookupTypeCorrelationsRef"].IsNull()) ? dr["CMN_LookupTypeCorrelationsRef"].ToInt64() : 0,
                        CMN_LookupMasterParentRef = (!dr["CMN_LookupMasterParentRef"].IsNull()) ? dr["CMN_LookupMasterParentRef"].ToInt64() : 0,
                        LookupMasterParentValue = (!dr["LookupMasterParentValue"].IsNull()) ? dr["LookupMasterParentValue"].NullToString() : string.Empty,
                        CMN_LookupMasterChildRef = (!dr["CMN_LookupMasterChildRef"].IsNull()) ? dr["CMN_LookupMasterChildRef"].ToInt64() : 0,
                        LookupMasterChildValue = (!dr["LookupMasterChildValue"].IsNull()) ? dr["LookupMasterChildValue"].NullToString() : string.Empty,
                        GroupingLookupMasterRef = (!dr["GroupingLookupMasterRef"].IsNull()) ? dr["GroupingLookupMasterRef"].ToInt64() : 0,
                        GroupingLookupMasterValue = (!dr["GroupingLookupMasterValue"].IsNull()) ? dr["GroupingLookupMasterValue"].NullToString() : string.Empty,
                        CorrelationDescription = (!dr["CorrelationDescription"].IsNull()) ? dr["CorrelationDescription"].NullToString() : string.Empty,
                        DisplayOrder = (!dr["DisplayOrder"].IsNull()) ? dr["DisplayOrder"].ToInt64() : 0,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : DateTime.UtcNow,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : DateTime.UtcNow,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                //need log
                throw ex;
            }
        }

        private void MapAllLookupTypeCorrelations(long? TimeZone,DataSet dsResult, out List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations)
        {
            lstDOCMN_LookupTypeCorrelations = new List<DOCMN_LookupTypeCorrelations>();
            try
            {
                if (_dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    lstDOCMN_LookupTypeCorrelations = _dsResult.Tables[0].AsEnumerable().Select(dr => new DOCMN_LookupTypeCorrelations
                    {
                        CMN_LookupTypeCorrelationsId = (!dr["CMN_LookupTypeCorrelationsId"].IsNull()) ? dr["CMN_LookupTypeCorrelationsId"].ToInt64() : 0,
                        CMN_LookupTypeParentRef = (!dr["CMN_LookupTypeParentRef"].IsNull()) ? dr["CMN_LookupTypeParentRef"].ToInt64() : 0,
                        CMN_LookupTypeParentValue = (!dr["CMN_LookupTypeParentValue"].IsNull()) ? dr["CMN_LookupTypeParentValue"].NullToString() : string.Empty,
                        CMN_LookupTypeChildRef = (!dr["CMN_LookupTypeChildRef"].IsNull()) ? dr["CMN_LookupTypeChildRef"].ToInt64() : 0,
                        CMN_LookupTypeChildValue = (!dr["CMN_LookupTypeChildValue"].IsNull()) ? dr["CMN_LookupTypeChildValue"].NullToString() : string.Empty,
                        GroupingLookupTypeRef = (!dr["GroupingLookupTypeRef"].IsNull()) ? dr["GroupingLookupTypeRef"].ToInt64() : 0,
                        GroupingLookupTypeValue = (!dr["GroupingLookupTypeValue"].IsNull()) ? dr["GroupingLookupTypeValue"].NullToString() : string.Empty,
                        CorrelationDescription = (!dr["CorrelationDescription"].IsNull()) ? dr["CorrelationDescription"].NullToString() : string.Empty,
                        LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0,
                        LockedByName = (!dr["LockedByName"].IsNull()) ? dr["LockedByName"].NullToString() : string.Empty,
                        UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLockedOn"].ToDateTime(),TimeZone) : DateTime.UtcNow,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : DateTime.UtcNow,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : DateTime.UtcNow,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false


                    }).ToList();
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }

        private void MapAllLookupMasterCorrelations(long? TimeZone,DataSet dsResult, out List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations)
        {
            lstDOCMN_LookupMasterCorrelations = new List<DOCMN_LookupMasterCorrelations>();
            try
            {
                if (_dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[1].Rows.Count > 0)
                {
                    lstDOCMN_LookupMasterCorrelations = _dsResult.Tables[1].AsEnumerable().Select(dr => new DOCMN_LookupMasterCorrelations
                    {
                        CMN_LookupMasterCorrelationsId = (!dr["CMN_LookupMasterCorrelationsId"].IsNull()) ? dr["CMN_LookupMasterCorrelationsId"].ToInt64() : 0,
                        CMN_LookupTypeCorrelationsRef = (!dr["CMN_LookupTypeCorrelationsRef"].IsNull()) ? dr["CMN_LookupTypeCorrelationsRef"].ToInt64() : 0,
                        CMN_LookupMasterParentRef = (!dr["CMN_LookupMasterParentRef"].IsNull()) ? dr["CMN_LookupMasterParentRef"].ToInt64() : 0,
                        LookupMasterParentValue = (!dr["LookupMasterParentValue"].IsNull()) ? dr["LookupMasterParentValue"].NullToString() : string.Empty,
                        CMN_LookupMasterChildRef = (!dr["CMN_LookupMasterChildRef"].IsNull()) ? dr["CMN_LookupMasterChildRef"].ToInt64() : 0,
                        LookupMasterChildValue = (!dr["LookupMasterChildValue"].IsNull()) ? dr["LookupMasterChildValue"].NullToString() : string.Empty,
                        GroupingLookupMasterRef = (!dr["GroupingLookupMasterRef"].IsNull()) ? dr["GroupingLookupMasterRef"].ToInt64() : 0,
                        GroupingLookupMasterValue = (!dr["GroupingLookupMasterValue"].IsNull()) ? dr["GroupingLookupMasterValue"].NullToString() : string.Empty,
                        CorrelationDescription = (!dr["CorrelationDescription"].IsNull()) ? dr["CorrelationDescription"].NullToString() : string.Empty,
                        DisplayOrder = (!dr["DisplayOrder"].IsNull()) ? dr["DisplayOrder"].ToInt64() : 0,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : DateTime.UtcNow,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedByName = (!dr["CreatedByName"].IsNull()) ? dr["CreatedByName"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : DateTime.UtcNow,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedByName = (!dr["LastUpdatedByName"].IsNull()) ? dr["LastUpdatedByName"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MapLookupCorelationMasterByID(DataSet dsResult, long corTypeID, long corMasterID, out DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended)
        {
            objDOCMN_LookupMasterCorrelationsExtended = new DOCMN_LookupMasterCorrelationsExtended();
            try
            {
                if (_dsResult != null && _dsResult.Tables.Count > 0 && _dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = _dsResult.Tables[0].Rows[0];
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeCorrelationsId = dr["CMN_LookupTypeCorrelationsId"].ToInt64();
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeParentRef = dr["CMN_LookupTypeParentRef"].ToInt64();
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupTypeCorrelations.CMN_LookupTypeChildRef = (!dr["CMN_LookupTypeChildRef"].IsNull()) ? dr["CMN_LookupTypeChildRef"].ToInt64() : 0;

                }
                if (_dsResult != null && _dsResult.Tables.Count > 1 && _dsResult.Tables[1].Rows.Count > 0 && corMasterID > 0)
                {
                    DataRow dr = _dsResult.Tables[1].Rows[0];
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterCorrelationsId = dr["CMN_LookupMasterCorrelationsId"].ToInt64();
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupTypeCorrelationsRef = dr["CMN_LookupTypeCorrelationsRef"].ToInt64();
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterParentRef = (!dr["CMN_LookupMasterParentRef"].IsNull()) ? dr["CMN_LookupMasterParentRef"].ToInt64() : 0;
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CMN_LookupMasterChildRef = (!dr["CMN_LookupMasterChildRef"].IsNull()) ? dr["CMN_LookupMasterChildRef"].ToInt64() : 0;
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.CorrelationDescription = (!dr["CorrelationDescription"].IsNull()) ? dr["CorrelationDescription"].NullToString() : string.Empty;
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.DisplayOrder = (!dr["DisplayOrder"].IsNull()) ? dr["DisplayOrder"].ToInt64() : 0;
                    objDOCMN_LookupMasterCorrelationsExtended.objDOCMN_LookupMasterCorrelations.IsActive = dr["IsActive"].ToBoolean();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
