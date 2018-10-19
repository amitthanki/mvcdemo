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
    public class DALMQ
    {
        public ExceptionTypes InsertMQTRRRecord(DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, long CurrentMasterUserId, out string errorMessage)
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
                sqlParam.ParameterName = "@MQSourceTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOMQTRRWorkQueueItems.MQSourceTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WQTrackingNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOMQTRRWorkQueueItems.WQTrackingNumber.IsNullOrEmpty() ? "00000" : objDOMQTRRWorkQueueItems.WQTrackingNumber;
                parameters.Add(sqlParam);

                if (!objDOMQTRRWorkQueueItems.MemberID.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MemberId";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.MemberID;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.IndividualID.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IndividualID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.IndividualID;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.HICN.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@HICN";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.HICN;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.HouseHoldID.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@HouseholdID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.HouseHoldID;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.Contract.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@Contract";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.Contract;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.PBP.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@PBP";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.PBP;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.FirstName.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FirstName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.FirstName;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.MiddleName.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MiddleName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.MiddleName;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.LastName.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@LastName";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.LastName;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.DOB.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DOB";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.DOB;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.SCCCode.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SCCCode";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.SCCCode;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.LOB.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@LOB";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.LOB;
                    parameters.Add(sqlParam);
                }

                if (objDOMQTRRWorkQueueItems.DisenrollementPeriod != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@DisenrollementPeriod";
                    sqlParam.SqlDbType = SqlDbType.Int;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.DisenrollementPeriod;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.TRC.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TRC";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.TRC;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.TRCTypeCode.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TRCTypeCode";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.TRCTypeCode;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.TimelineEffectiveDate.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TimelineEffectiveDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.TimelineEffectiveDate;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.TRRFileReceiptDate.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TRRFileReceiptDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.TRRFileReceiptDate;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.PlanTerminationDate.IsNullOrEmpty() && objDOMQTRRWorkQueueItems.PlanTerminationDate != new DateTime())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@PlanTerminationDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.PlanTerminationDate;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.ReasonDescription.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ReasonDescription";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.ReasonDescription;
                    parameters.Add(sqlParam);
                }

                if (objDOMQTRRWorkQueueItems.ERSCaseNumber != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@ERSCaseNumber";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.ERSCaseNumber;
                    parameters.Add(sqlParam);
                }

                if (objDOMQTRRWorkQueueItems.CMN_BackgroundProcessMasterRef != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@CMN_BackgroundProcessMasterRef";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.CMN_BackgroundProcessMasterRef;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.TrrRecordID.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@TrrRecordID";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.TrrRecordID;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.GPSProposedEffectiveDate.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GPSProposedEffectiveDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.GPSProposedEffectiveDate;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.GPSPDPAutoEnroleeIndicator.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GPSPDPAutoEnroleeIndicator";
                    sqlParam.SqlDbType = SqlDbType.Bit;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.GPSPDPAutoEnroleeIndicator;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.GPSApplicationStatus.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GPSApplicationStatus";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.GPSApplicationStatus;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.GPSContract.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GPSContract";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.GPSContract;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.GPSPBP.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GPSPBP";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.GPSPBP;
                    parameters.Add(sqlParam);
                }

                if (!objDOMQTRRWorkQueueItems.EmployerId.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@EmployerId";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOMQTRRWorkQueueItems.EmployerId;
                    parameters.Add(sqlParam);
                }

                if (CurrentMasterUserId != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@SystemId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = CurrentMasterUserId;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOMQTRRWorkQueueItems.IsRestricted;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsNationalEmployee";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOMQTRRWorkQueueItems.IsNationalEmployee;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQTRRWorkQueueItemId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INS_MQTRRWorkQueueItems, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@MQTRRWorkQueueItemId");
                if (sqlParam != null && sqlParam.Value != null && Int64.TryParse(sqlParam.Value.ToString(), out long Id))
                {
                    objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId = Id;
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
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes GetNationalEmployerGroups(out List<DOMQTRRWorkQueueItems> lstEmployerNationalGroup, out string errorMessage)
        {
            lstEmployerNationalGroup = new List<DOMQTRRWorkQueueItems>();
            DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems;
            try
            {
                DAHelper dah = new DAHelper();
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                long executionResult = 0;

                string query = string.Format(ConstantTexts.Query_Select_NationalEmployerGroups);

                executionResult = dah.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet MQTRRRecordDetailsDataSet, out string erorrMessage, true);

                if (executionResult == (long)ExceptionTypes.Success)
                {
                    if (!MQTRRRecordDetailsDataSet.IsNullOrEmpty())
                    {
                        if (MQTRRRecordDetailsDataSet.Tables.Count > 0 && MQTRRRecordDetailsDataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in MQTRRRecordDetailsDataSet.Tables[0].Rows)
                            {
                                objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems();
                                if (row.Table.Columns.Contains("Contract_Number"))
                                {
                                    if (!DBNull.Value.Equals(row["Contract_Number"]))
                                    {
                                        objDOMQTRRWorkQueueItems.Contract = row["Contract_Number"].NullToString();
                                    }
                                }
                                if (row.Table.Columns.Contains("PBP"))
                                {
                                    if (!DBNull.Value.Equals(row["PBP"]))
                                    {
                                        objDOMQTRRWorkQueueItems.PBP = row["PBP"].NullToString();
                                    }
                                }
                                if (row.Table.Columns.Contains("GPSEmployerID"))
                                {
                                    if (!DBNull.Value.Equals(row["GPSEmployerID"]))
                                    {
                                        objDOMQTRRWorkQueueItems.EmployerId = row["GPSEmployerID"].NullToString();
                                    }
                                }
                                if (row.Table.Columns.Contains("StateAbbreviation"))
                                {
                                    if (!DBNull.Value.Equals(row["StateAbbreviation"]))
                                    {
                                        objDOMQTRRWorkQueueItems.StateAbbreviation = row["StateAbbreviation"].NullToString();
                                    }
                                }
                                lstEmployerNationalGroup.Add(objDOMQTRRWorkQueueItems);
                            }
                            return ExceptionTypes.Success;
                        }
                        return ExceptionTypes.ZeroRecords;
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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }

        }
        public ExceptionTypes SaveXMLMessage(MQMessagesRecieved objMQMessagesRecieved, out string errorMessage)
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
                sqlParam.ParameterName = "@BackgroundProcessMasterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.CMN_BackgroundProcessMasterRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQSourceTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.MQSourceTypeLkup;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objMQMessagesRecieved.MQMessage;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SystemId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.SystemId;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam.ParameterName = "@MQMessagesRecievedId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INS_MQMessagesRecieved, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@MQMessagesRecievedId");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    objMQMessagesRecieved.MQMessagesRecievedId = Convert.ToInt64(sqlParam.Value);
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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes UpdateXMLMessage(MQMessagesRecieved objMQMessagesRecieved, out string errorMessage)
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
                sqlParam.ParameterName = "@MQMessagesRecievedId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.MQMessagesRecievedId;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsProcessed";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objMQMessagesRecieved.IsProcessed;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ProcessedResult";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objMQMessagesRecieved.ProcessedResult;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SystemId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.SystemId;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQTRRWorkQueueItemRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objMQMessagesRecieved.MQTRRWorkQueueItemRef;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ProcessingFailReason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objMQMessagesRecieved.ProcessingFailReason;
                parameters.Add(sqlParam);
                sqlParam = new SqlParameter();


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_UPD_MQMessagesRecieved, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes UpdatMQTRRCaseDetails(long lMQTRRWorkQueueItemId, long lMQSourceTypeLkup, long lUpdateCMSTransactionCaseNumber, long CurrentUserId, out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;
                long executionResult = 0;

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQTRRWorkQueueItemsId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lMQTRRWorkQueueItemId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MQSourceTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = lMQSourceTypeLkup;
                parameters.Add(sqlParam);

                if (lUpdateCMSTransactionCaseNumber != 0)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@GEN_QueueId";
                    sqlParam.SqlDbType = SqlDbType.BigInt;
                    sqlParam.Value = lUpdateCMSTransactionCaseNumber;
                    parameters.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = CurrentUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_BGP_INS_UPD_MQTRRProcess, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes MQTRRRecordsToProcess(long savedMessagesBGPId, out List<DOMQTRRWorkQueueItems> lstDOMQTRRWorkQueueItems, out string errorMessage)
        {
            lstDOMQTRRWorkQueueItems = new List<DOMQTRRWorkQueueItems>();
            DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems;
            try
            {
                DAHelper dah = new DAHelper();
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                long executionResult = 0;

                string query = string.Format(ConstantTexts.Query_Select_MQTRRRecords, savedMessagesBGPId);

                executionResult = dah.ExecuteSQL(query, new List<SqlParameter>().ToArray(), out DataSet MQTRRRecordDetailsDataSet, out string erorrMessage, true);

                if (executionResult == (long)ExceptionTypes.Success)
                {
                    if (!MQTRRRecordDetailsDataSet.IsNullOrEmpty())
                    {
                        if (MQTRRRecordDetailsDataSet.Tables.Count > 0 && MQTRRRecordDetailsDataSet.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in MQTRRRecordDetailsDataSet.Tables[0].Rows)
                            {
                                objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems();
                                if (row.Table.Columns.Contains("MQTRRWorkQueueItemId"))
                                {
                                    if (!DBNull.Value.Equals(row["MQTRRWorkQueueItemId"]))
                                    {
                                        objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId = Convert.ToInt64(row["MQTRRWorkQueueItemId"]);
                                    }
                                }
                                if (row.Table.Columns.Contains("MQSourceTypeLkup"))
                                {
                                    if (!DBNull.Value.Equals(row["MQSourceTypeLkup"]))
                                    {
                                        objDOMQTRRWorkQueueItems.MQSourceTypeLkup = Convert.ToInt64(row["MQSourceTypeLkup"]);
                                    }
                                }
                                if (row.Table.Columns.Contains("ERSCaseNumber"))
                                {
                                    if (!DBNull.Value.Equals(row["ERSCaseNumber"]))
                                    {
                                        objDOMQTRRWorkQueueItems.ERSCaseNumber = Convert.ToInt64(row["ERSCaseNumber"]);
                                    }
                                }
                                lstDOMQTRRWorkQueueItems.Add(objDOMQTRRWorkQueueItems);
                            }
                            return ExceptionTypes.Success;
                        }
                        return ExceptionTypes.ZeroRecords;
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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        public ExceptionTypes SetCurrentBatchStatus(out string errorMessage)
        {
            try
            {
                DAHelper dah = new DAHelper();
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;

                long executionResult = 0;

                executionResult = dah.ExecuteScalar(ConstantTexts.Query_Update_MQTRRBatchStatus, new List<SqlParameter>().ToArray(), out object obj, out string erorrMessage);

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
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

    }
}
