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
    public class DALOST
    {
        DAHelper _objDAHelper = new DAHelper();
        List<SqlParameter> _lstobjDOGEN_Queue;
        private DataSet _dsResult;

        /// <summary>
        /// Save OST Cases
        /// </summary>
        /// <param name="objDOGEN_Queue"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ExceptionTypes SaveOST(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            try
            {
                _lstobjDOGEN_Queue = new List<SqlParameter>();
                SqlParameter sqlParam;
                long lErrocode = 0;
                long lErrorNumber = 0;
                long lRowsEffected = 0;
                DataSet dsTable = new DataSet();
                errorMessage = string.Empty;


                if (objDOGEN_Queue.IsClosedAndCreateNew)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsCloneCase";
                    sqlParam.DbType = DbType.Boolean;
                    sqlParam.Value = objDOGEN_Queue.IsClosedAndCreateNew;
                    _lstobjDOGEN_Queue.Add(sqlParam);
                }
                

                objDOGEN_Queue.GEN_QueueId = 0;

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@BusinessSegmentLkup";
                //sqlParam.DbType = DbType.Int64;
                //sqlParam.Value = 0;
                //_lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.WorkBasketLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyCategoryLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyTypeLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyTypeLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.StartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.StartDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EndDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EndDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousAssignedToRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousAssignedToRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentActionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentWorkQueueLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentWorkQueueLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentStatusLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SourceSystemLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SourceSystemId";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SourceSystemId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancySourceLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancySourceLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyReceiptDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyReceiptDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ComplianceStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.ComplianceStartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ComplianceStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ComplianceStartDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DisenrollmentDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DisenrollmentDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberSCCCode;
                _lstobjDOGEN_Queue.Add(sqlParam);


                if (!objDOGEN_Queue.MemberID.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@MemberID";
                    sqlParam.DbType = DbType.String;
                    sqlParam.Value = objDOGEN_Queue.MemberID.ToUpper();
                    _lstobjDOGEN_Queue.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberCurrentHICN.ToUpper();
                _lstobjDOGEN_Queue.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@MemberCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.MemberCurrentMBI;
                //_lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GPSHouseholdID";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.GPSHouseholdID;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberFirstName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberFirstName;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberMiddleName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberMiddleName;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLastName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberLastName;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberContractIDLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberPBPLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLOBLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberLOBLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedState;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedCountyCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedCountyCode;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.MemberDOB;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberDOBId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberGenderLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSContractIDLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSPBPLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligGPSSCCCode;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligGPSCurrentHICN;
                _lstobjDOGEN_Queue.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@EligGPSCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.EligGPSCurrentMBI;
                //_lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanTermDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanTermDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSLOBLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSLOBLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSMemberDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOB;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSMemberDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOBId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSGenderLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRContractIDLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPBPLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligMMRSCCCode;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligMMRCurrentHICN;
                _lstobjDOGEN_Queue.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@EligMMRCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.EligMMRCurrentMBI;
                //_lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentMonth";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentMonth;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRDOB;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRDOBId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRGenderLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligOOAFlagLkup";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.EligOOAFlagLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRActionRequestedLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPROtherActionRequested;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRSupervisorOrRequesterRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPRReasonforRequest;
                _lstobjDOGEN_Queue.Add(sqlParam);
                
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRTaskPerformedLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPROtherTaskPerformed;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.RPRCTMMember;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPRCTMNumber;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.RPREGHPMember;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPREmployerID;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequested";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequested;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequestedZip";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequestedZip;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDateId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsParentCase";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsParentCase;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsChildCase";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsChildCase;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ParentQueueRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ParentQueueRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.Comments;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReAssignUserRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ReAssignUserRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CasesComments";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.CasesComments;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RoleLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousActionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@PreviousWorkQueuesLkup";
                //sqlParam.DbType = DbType.Int64;
                //sqlParam.Value = objDOGEN_Queue.PreviousWorkQueuesLkup;
                //_lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousStatusLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentActionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentWorkQueuesLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentWorkQueuesLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentStatusLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.LoginUserId;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TRCLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.TransactionReplyCode;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = string.Empty;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CommentsSourceSystemLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsRestricted;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PDPAutoEnrolleeInd";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.PDPAutoEnrolleeInd;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EmployeerGroupNumber;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                _lstobjDOGEN_Queue.Add(sqlParam);


                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_INS_CreateSuspectCase, _lstobjDOGEN_Queue.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = _lstobjDOGEN_Queue.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");
                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    sqlParam = _lstobjDOGEN_Queue.FirstOrDefault(x => x.ParameterName == "@GEN_QueueId");
                    if (!sqlParam.IsNull())
                    {
                        objDOGEN_Queue.GEN_QueueId = (!sqlParam.Value.IsNull()) ? objDOGEN_Queue.GEN_QueueId = sqlParam.Value.ToInt64() : 0;
                    }
                    sqlParam = _lstobjDOGEN_Queue.FirstOrDefault(x => x.ParameterName == "@BusinessSegmentLkup");
                    if (!sqlParam.IsNull())
                    {
                        objDOGEN_Queue.BusinessSegmentLkup = 0;
                        objDOGEN_Queue.BusinessSegmentLkup = (!sqlParam.Value.IsNull()) ? objDOGEN_Queue.BusinessSegmentLkup = sqlParam.Value.ToInt64() : 0;
                    }
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
                _lstobjDOGEN_Queue = null;
            }
        }

        public ExceptionTypes SaveOSTActions(DOGEN_OSTActions objDOGEN_OSTActions, out string errorMessage)
        {
            _lstobjDOGEN_Queue = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            long lRowsEffected = 0;
            DataSet dsTable = new DataSet();
            errorMessage = string.Empty;
            try
            {
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.GEN_QueueRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ActionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReOpenoptionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.OptionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReOpenQueueLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ReopenQueueLKUP;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.LastName;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DateofBirth";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.DateofBirth;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ContractIDLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.PBPLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ApplicationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.ApplicationDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.EffectiveDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.EndDate;
                _lstobjDOGEN_Queue.Add(sqlParam);



                if (!objDOGEN_OSTActions.FirstLetterMailDateNoResponseTerm.IsNull())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FirstLetterMailDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOGEN_OSTActions.FirstLetterMailDateNoResponseTerm;
                    _lstobjDOGEN_Queue.Add(sqlParam);
                }
                else
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@FirstLetterMailDate";
                    sqlParam.SqlDbType = SqlDbType.DateTime;
                    sqlParam.Value = objDOGEN_OSTActions.FirstLetterMailDate;
                    _lstobjDOGEN_Queue.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SecondLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SecondLetterMailDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

               

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResidentialDocumentationRequired";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.ResidentialDocumentationRequired;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CountyAttestationRequired";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.CountyAttestationRequired;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.PendReasonLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContainsErrorsLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ContainsErrorsLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.ResolutionLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Reason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.Reason;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@InitialAddressVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.InitialAddressVerificationDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberResponseVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.MemberResponseVerificationDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.MemberVerifiedState;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SCCLetterMailDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                if (!objDOGEN_OSTActions.Comments.IsNullOrEmpty())
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@Comments";
                    sqlParam.SqlDbType = SqlDbType.VarChar;
                    sqlParam.Value = objDOGEN_OSTActions.Comments;
                    _lstobjDOGEN_Queue.Add(sqlParam);
                }

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RoleLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.LastUpdatedByRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.IsActive;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.RPRRequestedEffectiveDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRActionRequestedLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPROtherActionRequested;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRSupervisorOrRequesterRef;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPRReasonforRequest;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.RPRTaskPerformedLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPROtherTaskPerformed;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.RPRCTMMember;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPRCTMNumber;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.RPREGHPMember;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.RPREmployerID;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequested;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequestedZip";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequestedZip;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.SCCRPRRequstedSubmissionDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_OSTActions.CommentsSourceSystemLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OOALetterStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.OOALetterStatusLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSTransactionStatusLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.CMSTransactionStatusLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GPSHouseholdId";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_OSTActions.GPSHouseholdID;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AdjustedDiscrepancyReceiptDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AdjustedComplianceStartDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.AdjustedComplianceStartDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MARxAddressResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_OSTActions.MARxAddressCompletedLkup;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PDPAutoEnrolleeInd";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_OSTActions.PDPAutoEnrolleeInd;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@AdjustedDisenrollmentDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_OSTActions.AdjustedDisenrollmentDate;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstobjDOGEN_Queue.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteDMLSP(ConstantTexts.SP_APP_UPD_OSTActions, _lstobjDOGEN_Queue.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);

                sqlParam = _lstobjDOGEN_Queue.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

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
                _lstobjDOGEN_Queue = null;
            }
        }

        /// <summary>
        /// Get GenQ By ID
        /// </summary>
        /// <param name="genQueueID"></param>
        /// <param name="objDOGEN_Queue"></param>
        /// <returns></returns>
        public ExceptionTypes GetGenQueueByID(long? TimeZone,long genQueueID, out DOGEN_Queue objDOGEN_Queue,out string errorMessage)
        {
            _dsResult = new DataSet();
            _lstobjDOGEN_Queue = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            errorMessage = string.Empty;
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = genQueueID;
                _lstobjDOGEN_Queue.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstobjDOGEN_Queue.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Case, _lstobjDOGEN_Queue.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstobjDOGEN_Queue.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    MapGenQueue(TimeZone,_dsResult, out objDOGEN_Queue);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;
            }
            catch (Exception ex)
            {
                //need log ex
                return ExceptionTypes.UnknownError;
            }
            finally
            {
                _dsResult = null;
                _lstobjDOGEN_Queue = null;
            }
        }

        private void MapGenQueue(long? TimeZone,DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];
                    objDOGEN_Queue.GEN_QueueId = (dr.Table.Columns.Contains("GEN_QueueId") && !dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                    objDOGEN_Queue.BusinessSegment = (dr.Table.Columns.Contains("BusinessSegment") && !dr["BusinessSegment"].IsNull()) ? dr["BusinessSegment"].NullToString() : string.Empty;
                    objDOGEN_Queue.WorkBasketLkup = (dr.Table.Columns.Contains("WorkBasketLkup") && !dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0;
                    objDOGEN_Queue.WorkBasket = (dr.Table.Columns.Contains("WorkBasket") && !dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyCategoryLkup = (dr.Table.Columns.Contains("DiscrepancyCategoryLkup") && !dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyCategory = (dr.Table.Columns.Contains("DiscrepancyCategory") && !dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyTypeLkup = (dr.Table.Columns.Contains("DiscrepancyTypeLkup") && !dr["DiscrepancyTypeLkup"].IsNull()) ? dr["DiscrepancyTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyType = (dr.Table.Columns.Contains("DiscrepancyType") && !dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty;
                    objDOGEN_Queue.StartDate = (dr.Table.Columns.Contains("StartDate") && !dr["StartDate"].IsNull()) ? dr["StartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.PreviousWorkQueueLkup = (dr.Table.Columns.Contains("PreviousWorkQueueLkup") && !dr["PreviousWorkQueueLkup"].IsNull()) ? dr["PreviousWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedToRef = (dr.Table.Columns.Contains("AssignedToRef") && !dr["AssignedToRef"].IsNull()) ? dr["AssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedTo = (dr.Table.Columns.Contains("AssignedTo") && !dr["AssignedTo"].IsNull()) ? dr["AssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCAssignedOn = (dr.Table.Columns.Contains("UTCAssignedOn") && !dr["UTCAssignedOn"].IsNull()) ? dr["UTCAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTAssignedOn = (dr.Table.Columns.Contains("CSTAssignedOn") && !dr["CSTAssignedOn"].IsNull()) ? dr["CSTAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LockedByRef = (dr.Table.Columns.Contains("LockedByRef") && !dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LockedBy = (dr.Table.Columns.Contains("LockedBy") && !dr["LockedBy"].IsNull()) ? dr["LockedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLockedOn = (dr.Table.Columns.Contains("UTCLockedOn") && !dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTLockedOn = (dr.Table.Columns.Contains("CSTLockedOn") && !dr["CSTLockedOn"].IsNull()) ? dr["CSTLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MostRecentActionLkup = (dr.Table.Columns.Contains("MostRecentActionLkup") && !dr["MostRecentActionLkup"].IsNull()) ? dr["MostRecentActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentAction = (dr.Table.Columns.Contains("MostRecentAction") && !dr["MostRecentAction"].IsNull()) ? dr["MostRecentAction"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (dr.Table.Columns.Contains("MostRecentWorkQueueLkup") && !dr["MostRecentWorkQueueLkup"].IsNull()) ? dr["MostRecentWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentWorkQueue = (dr.Table.Columns.Contains("MostRecentWorkQueue") && !dr["MostRecentWorkQueue"].IsNull()) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentStatusLkup = (dr.Table.Columns.Contains("MostRecentStatusLkup") && !dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentStatus = (dr.Table.Columns.Contains("MostRecentStatus") && !dr["MostRecentStatus"].IsNull()) ? dr["MostRecentStatus"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemLkup = (dr.Table.Columns.Contains("SourceSystemLkup") && !dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0;
                    objDOGEN_Queue.SourceSystem = (dr.Table.Columns.Contains("SourceSystem") && !dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemId = (dr.Table.Columns.Contains("SourceSystemId") && !dr["SourceSystemId"].IsNull()) ? dr["SourceSystemId"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancySourceLkup = (dr.Table.Columns.Contains("DiscrepancySourceLkup") && !dr["DiscrepancySourceLkup"].IsNull()) ? dr["DiscrepancySourceLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancySource = (dr.Table.Columns.Contains("DiscrepancySource") && !dr["DiscrepancySource"].IsNull()) ? dr["DiscrepancySource"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyReceiptDate = (dr.Table.Columns.Contains("DiscrepancyReceiptDate") && !dr["DiscrepancyReceiptDate"].IsNull()) ? dr["DiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.ComplianceStartDate = (dr.Table.Columns.Contains("ComplianceStartDate") && !dr["ComplianceStartDate"].IsNull()) ? dr["ComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.Aging = (dr.Table.Columns.Contains("Aging") && !dr["Aging"].IsNull()) ? dr["Aging"].ToInt32() : 0;
                    objDOGEN_Queue.DiscrepancyStartDate = (dr.Table.Columns.Contains("DiscrepancyStartDate") && !dr["DiscrepancyStartDate"].IsNull()) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.DiscrepancyEndDate = (dr.Table.Columns.Contains("DiscrepancyEndDate") && !dr["DiscrepancyEndDate"].IsNull()) ? dr["DiscrepancyEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberSCCCode = (dr.Table.Columns.Contains("MemberSCCCode") && !dr["MemberSCCCode"].IsNull()) ? dr["MemberSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberID = (dr.Table.Columns.Contains("MemberID") && !dr["MemberID"].IsNull()) ? dr["MemberID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberCurrentHICN = (dr.Table.Columns.Contains("MemberCurrentHICN") && !dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.MemberCurrentMBI = (!dr["MemberCurrentMBI"].IsNull()) ? dr["MemberCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.GPSHouseholdID = (dr.Table.Columns.Contains("GPSHouseholdID") && !dr["GPSHouseholdID"].IsNull()) ? dr["GPSHouseholdID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberFirstName = (dr.Table.Columns.Contains("MemberFirstName") && !dr["MemberFirstName"].IsNull()) ? dr["MemberFirstName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberMiddleName = (dr.Table.Columns.Contains("MemberMiddleName") && !dr["MemberMiddleName"].IsNull()) ? dr["MemberMiddleName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLastName = (dr.Table.Columns.Contains("MemberLastName") && !dr["MemberLastName"].IsNull()) ? dr["MemberLastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberContractIDLkup = (dr.Table.Columns.Contains("MemberContractIDLkup") && !dr["MemberContractIDLkup"].IsNull()) ? dr["MemberContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberContractID = (dr.Table.Columns.Contains("MemberContractID") && !dr["MemberContractID"].IsNull()) ? dr["MemberContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberPBPLkup = (dr.Table.Columns.Contains("MemberPBPLkup") && !dr["MemberPBPLkup"].IsNull()) ? dr["MemberPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberPBP = (dr.Table.Columns.Contains("MemberPBP") && !dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLOBLkup = (dr.Table.Columns.Contains("MemberLOBLkup") && !dr["MemberLOBLkup"].IsNull()) ? dr["MemberLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberLOB = (dr.Table.Columns.Contains("MemberLOB") && !dr["MemberLOB"].IsNull()) ? dr["MemberLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedCountyCode = (dr.Table.Columns.Contains("MemberVerifiedCountyCode") && !dr["MemberVerifiedCountyCode"].IsNull()) ? dr["MemberVerifiedCountyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberDOB = (dr.Table.Columns.Contains("MemberDOB") && !dr["MemberDOB"].IsNull()) ? dr["MemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberGenderLkup = (dr.Table.Columns.Contains("MemberGenderLkup") && !dr["MemberGenderLkup"].IsNull()) ? dr["MemberGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberGender = (dr.Table.Columns.Contains("MemberGender") && !dr["MemberGender"].IsNull()) ? dr["MemberGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSContractIDLkup = (dr.Table.Columns.Contains("EligGPSContractIDLkup") && !dr["EligGPSContractIDLkup"].IsNull()) ? dr["EligGPSContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSContractID = (dr.Table.Columns.Contains("EligGPSContractID") && !dr["EligGPSContractID"].IsNull()) ? dr["EligGPSContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSPBPLkup = (dr.Table.Columns.Contains("EligGPSPBPLkup") && !dr["EligGPSPBPLkup"].IsNull()) ? dr["EligGPSPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSPBP = (dr.Table.Columns.Contains("EligGPSPBP") && !dr["EligGPSPBP"].IsNull()) ? dr["EligGPSPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSSCCCode = (dr.Table.Columns.Contains("EligGPSSCCCode") && !dr["EligGPSSCCCode"].IsNull()) ? dr["EligGPSSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSCurrentHICN = (dr.Table.Columns.Contains("EligGPSCurrentHICN") && !dr["EligGPSCurrentHICN"].IsNull()) ? dr["EligGPSCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligGPSCurrentMBI = (!dr["EligGPSCurrentMBI"].IsNull()) ? dr["EligGPSCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanEffectiveDate") && !dr["EligGPSInsuredPlanEffectiveDate"].IsNull()) ? dr["EligGPSInsuredPlanEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSInsuredPlanTermDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanTermDate") && !dr["EligGPSInsuredPlanTermDate"].IsNull()) ? dr["EligGPSInsuredPlanTermDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSLOBLkup = (dr.Table.Columns.Contains("EligGPSLOBLkup") && !dr["EligGPSLOBLkup"].IsNull()) ? dr["EligGPSLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSLOB = (dr.Table.Columns.Contains("EligGPSLOB") && !dr["EligGPSLOB"].IsNull()) ? dr["EligGPSLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSMemberDOB = (dr.Table.Columns.Contains("EligGPSMemberDOB") && !dr["EligGPSMemberDOB"].IsNull()) ? dr["EligGPSMemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSGenderLkup = (dr.Table.Columns.Contains("EligGPSGenderLkup") && !dr["EligGPSGenderLkup"].IsNull()) ? dr["EligGPSGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSGender = (dr.Table.Columns.Contains("EligGPSGender") && !dr["EligGPSGender"].IsNull()) ? dr["EligGPSGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRContractIDLkup = (dr.Table.Columns.Contains("EligMMRContractIDLkup") && !dr["EligMMRContractIDLkup"].IsNull()) ? dr["EligMMRContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRContractID = (dr.Table.Columns.Contains("EligMMRContractID") && !dr["EligMMRContractID"].IsNull()) ? dr["EligMMRContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPBPLkup = (dr.Table.Columns.Contains("EligMMRPBPLkup") && !dr["EligMMRPBPLkup"].IsNull()) ? dr["EligMMRPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRPBP = (dr.Table.Columns.Contains("EligMMRPBP") && !dr["EligMMRPBP"].IsNull()) ? dr["EligMMRPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRSCCCode = (dr.Table.Columns.Contains("EligMMRSCCCode") && !dr["EligMMRSCCCode"].IsNull()) ? dr["EligMMRSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.DisenrollmentDate = (dr.Table.Columns.Contains("DisenrollmentDate") && !dr["DisenrollmentDate"].IsNull()) ? dr["DisenrollmentDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRCurrentHICN = (dr.Table.Columns.Contains("EligMMRCurrentHICN") && !dr["EligMMRCurrentHICN"].IsNull()) ? dr["EligMMRCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligMMRCurrentMBI = (!dr["EligMMRCurrentMBI"].IsNull()) ? dr["EligMMRCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentStartDate") && !dr["EligMMRPaymentAdjustmentStartDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentEndDate") && !dr["EligMMRPaymentAdjustmentEndDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentMonth = (dr.Table.Columns.Contains("EligMMRPaymentMonth") && !dr["EligMMRPaymentMonth"].IsNull()) ? dr["EligMMRPaymentMonth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRDOB = (dr.Table.Columns.Contains("EligMMRDOB") && !dr["EligMMRDOB"].IsNull()) ? dr["EligMMRDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRGenderLkup = (dr.Table.Columns.Contains("EligMMRGenderLkup") && !dr["EligMMRGenderLkup"].IsNull()) ? dr["EligMMRGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRGender = (dr.Table.Columns.Contains("EligMMRGender") && !dr["EligMMRGender"].IsNull()) ? dr["EligMMRGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligOOAFlagLkup = (dr.Table.Columns.Contains("EligOOAFlagLkup") && !dr["EligOOAFlagLkup"].IsNull()) ? dr["EligOOAFlagLkup"].ToBoolean() : false;
                    //objDOGEN_Queue.EligOOAFlag = (!dr["EligOOAFlag"].IsNull()) ? dr["EligOOAFlag"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRRequestedEffectiveDate = (dr.Table.Columns.Contains("RPRRequestedEffectiveDate") && !dr["RPRRequestedEffectiveDate"].IsNull()) ? dr["RPRRequestedEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.RPRActionRequestedLkup = (dr.Table.Columns.Contains("RPRActionRequestedLkup") && !dr["RPRActionRequestedLkup"].IsNull()) ? dr["RPRActionRequestedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRActionRequested = (dr.Table.Columns.Contains("RPRActionRequested") && !dr["RPRActionRequested"].IsNull()) ? dr["RPRActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRSupervisorOrRequesterRef = (dr.Table.Columns.Contains("RPRSupervisorOrRequesterRef") && !dr["RPRSupervisorOrRequesterRef"].IsNull()) ? dr["RPRSupervisorOrRequesterRef"].ToInt64() : 0;
                    objDOGEN_Queue.RPREmployerID = (dr.Table.Columns.Contains("RPREmployerID") && !dr["RPREmployerID"].IsNull()) ? dr["RPREmployerID"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRReasonforRequest = (dr.Table.Columns.Contains("RPRReasonforRequest") && !dr["RPRReasonforRequest"].IsNull()) ? dr["RPRReasonforRequest"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRTaskPerformedLkup = (dr.Table.Columns.Contains("RPRTaskPerformedLkup") && !dr["RPRTaskPerformedLkup"].IsNull()) ? dr["RPRTaskPerformedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRTaskPerformed = (dr.Table.Columns.Contains("RPRTaskPerformed") && !dr["RPRTaskPerformed"].IsNull()) ? dr["RPRTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRCTMMember = (dr.Table.Columns.Contains("RPRCTMMember") && !dr["RPRCTMMember"].IsNull()) ? dr["RPRCTMMember"].ToBoolean() : false;
                    objDOGEN_Queue.RPRCTMNumber = (dr.Table.Columns.Contains("RPRCTMNumber") && !dr["RPRCTMNumber"].IsNull()) ? dr["RPRCTMNumber"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPREGHPMember = (dr.Table.Columns.Contains("RPREGHPMember") && !dr["RPREGHPMember"].IsNull()) ? dr["RPREGHPMember"].ToBoolean() : false;
                    objDOGEN_Queue.SCCRPRRequested = (dr.Table.Columns.Contains("SCCRPRRequested") && !dr["SCCRPRRequested"].IsNull()) ? dr["SCCRPRRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequestedZip = (dr.Table.Columns.Contains("SCCRPRRequestedZip") && !dr["SCCRPRRequestedZip"].IsNull()) ? dr["SCCRPRRequestedZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequstedSubmissionDate = (dr.Table.Columns.Contains("SCCRPRRequstedSubmissionDate") && !dr["SCCRPRRequstedSubmissionDate"].IsNull()) ? dr["SCCRPRRequstedSubmissionDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveStartDate = (dr.Table.Columns.Contains("SCCRPREffectiveStartDate") && !dr["SCCRPREffectiveStartDate"].IsNull()) ? dr["SCCRPREffectiveStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveEndDate = (dr.Table.Columns.Contains("SCCRPREffectiveEndDate") && !dr["SCCRPREffectiveEndDate"].IsNull()) ? dr["SCCRPREffectiveEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCasePended = (dr.Table.Columns.Contains("IsCasePended") && !dr["IsCasePended"].IsNull()) ? dr["IsCasePended"].ToBoolean() : false;
                    objDOGEN_Queue.PendedbyRef = (dr.Table.Columns.Contains("PendedbyRef") && !dr["PendedbyRef"].IsNull()) ? dr["PendedbyRef"].ToInt64() : 0;
                    objDOGEN_Queue.Pendedby = (dr.Table.Columns.Contains("Pendedby") && !dr["Pendedby"].IsNull()) ? dr["Pendedby"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCPendedOn = (dr.Table.Columns.Contains("UTCPendedOn") && !dr["UTCPendedOn"].IsNull()) ? dr["UTCPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTPendedOn = (dr.Table.Columns.Contains("CSTPendedOn") && !dr["CSTPendedOn"].IsNull()) ? dr["CSTPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCaseResolved = (dr.Table.Columns.Contains("IsCaseResolved") && !dr["IsCaseResolved"].IsNull()) ? dr["IsCaseResolved"].ToBoolean() : false;
                    objDOGEN_Queue.ResolvedByRef = (dr.Table.Columns.Contains("ResolvedByRef") && !dr["ResolvedByRef"].IsNull()) ? dr["ResolvedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.ResolvedBy = (dr.Table.Columns.Contains("ResolvedBy") && !dr["ResolvedBy"].IsNull()) ? dr["ResolvedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCResolvedOn = (dr.Table.Columns.Contains("UTCResolvedOn") && !dr["UTCResolvedOn"].IsNull()) ? dr["UTCResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTResolvedOn = (dr.Table.Columns.Contains("CSTResolvedOn") && !dr["CSTResolvedOn"].IsNull()) ? dr["CSTResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsParentCase = (dr.Table.Columns.Contains("IsParentCase") && !dr["IsParentCase"].IsNull()) ? dr["IsParentCase"].ToBoolean() : false;
                    objDOGEN_Queue.IsChildCase = (dr.Table.Columns.Contains("IsChildCase") && !dr["IsChildCase"].IsNull()) ? dr["IsChildCase"].ToBoolean() : false;
                    objDOGEN_Queue.ParentQueueRef = (dr.Table.Columns.Contains("ParentQueueRef") && !dr["ParentQueueRef"].IsNull()) ? dr["ParentQueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                    objDOGEN_Queue.CSTCreatedOn = (dr.Table.Columns.Contains("CSTCreatedOn") && !dr["CSTCreatedOn"].IsNull()) ? dr["CSTCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                    objDOGEN_Queue.CSTLastUpdatedOn = (dr.Table.Columns.Contains("CSTLastUpdatedOn") && !dr["CSTLastUpdatedOn"].IsNull()) ? dr["CSTLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherActionRequested = (dr.Table.Columns.Contains("RPROtherActionRequested") && !dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherTaskPerformed = (dr.Table.Columns.Contains("RPROtherTaskPerformed") && !dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.PreviousAssignedToRef = (dr.Table.Columns.Contains("PreviousAssignedToRef") && !dr["PreviousAssignedToRef"].IsNull()) ? dr["PreviousAssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.PreviousAssignedTo = (dr.Table.Columns.Contains("PreviousAssignedTo") && !dr["PreviousAssignedTo"].IsNull()) ? dr["PreviousAssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.PDPAutoEnrolleeInd = (dr.Table.Columns.Contains("PDPAutoEnrolleeInd") && !dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : (long?)null;
                    objDOGEN_Queue.ReferencedEligibilityCaseInd = (dr.Table.Columns.Contains("ReferencedEligibilityCaseInd") && !dr["ReferencedEligibilityCaseInd"].IsNull()) ? dr["ReferencedEligibilityCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.ReferencedSCCCaseInd = (dr.Table.Columns.Contains("ReferencedSCCCaseInd") && !dr["ReferencedSCCCaseInd"].IsNull()) ? dr["ReferencedSCCCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.FileTypeLkup = (dr.Table.Columns.Contains("FileTypeLkup") && !dr["FileTypeLkup"].IsNull()) ? dr["FileTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.FileType = (dr.Table.Columns.Contains("FileType") && !dr["FileType"].IsNull()) ? dr["FileType"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMDocID = (dr.Table.Columns.Contains("ODMDocID") && !dr["ODMDocID"].IsNull()) ? dr["ODMDocID"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMAddressLink = (dr.Table.Columns.Contains("ODMAddressLink") && !dr["ODMAddressLink"].IsNull()) ? dr["ODMAddressLink"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress1 = (dr.Table.Columns.Contains("UndeliveredAddress1") && !dr["UndeliveredAddress1"].IsNull()) ? dr["UndeliveredAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress2 = (dr.Table.Columns.Contains("UndeliveredAddress2") && !dr["UndeliveredAddress2"].IsNull()) ? dr["UndeliveredAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredCity = (dr.Table.Columns.Contains("UndeliveredCity") && !dr["UndeliveredCity"].IsNull()) ? dr["UndeliveredCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredState = (dr.Table.Columns.Contains("UndeliveredState") && !dr["UndeliveredState"].IsNull()) ? dr["UndeliveredState"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredZip = (dr.Table.Columns.Contains("UndeliveredZip") && !dr["UndeliveredZip"].IsNull()) ? dr["UndeliveredZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress1 = (dr.Table.Columns.Contains("COAOldAddress1") && !dr["COAOldAddress1"].IsNull()) ? dr["COAOldAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress2 = (dr.Table.Columns.Contains("COAOldAddress2") && !dr["COAOldAddress2"].IsNull()) ? dr["COAOldAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldCity = (dr.Table.Columns.Contains("COAOldCity") && !dr["COAOldCity"].IsNull()) ? dr["COAOldCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldState = (dr.Table.Columns.Contains("COAOldState") && !dr["COAOldState"].IsNull()) ? dr["COAOldState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldZip = (dr.Table.Columns.Contains("COAOldZip") && !dr["COAOldZip"].IsNull()) ? dr["COAOldZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress1 = (dr.Table.Columns.Contains("COANewAddress1") && !dr["COANewAddress1"].IsNull()) ? dr["COANewAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress2 = (dr.Table.Columns.Contains("COANewAddress2") && !dr["COANewAddress2"].IsNull()) ? dr["COANewAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewCity = (dr.Table.Columns.Contains("COANewCity") && !dr["COANewCity"].IsNull()) ? dr["COANewCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewState = (dr.Table.Columns.Contains("COANewState") && !dr["COANewState"].IsNull()) ? dr["COANewState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewZip = (dr.Table.Columns.Contains("COANewZip") && !dr["COANewZip"].IsNull()) ? dr["COANewZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.TurnAroundTime = (dr.Table.Columns.Contains("TurnAroundTime") && !dr["TurnAroundTime"].IsNull()) ? dr["TurnAroundTime"].ToInt64() : 0;
                    objDOGEN_Queue.TransactionReplyCode = (dr.Table.Columns.Contains("TransactionReplyCode") && !dr["TransactionReplyCode"].IsNull()) ? dr["TransactionReplyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.TransactionReplyCodeLkup = (dr.Table.Columns.Contains("TRCLkup") && !dr["TRCLkup"].IsNull()) ? dr["TRCLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EmployeerGroupNumber = (dr.Table.Columns.Contains("EmployerGroupNumber") && !dr["EmployerGroupNumber"].IsNull()) ? dr["EmployerGroupNumber"].NullToString() : string.Empty;
                    objDOGEN_Queue.TimelineEffectiveDate = (dr.Table.Columns.Contains("TimelineEffectiveDate") && !dr["TimelineEffectiveDate"].IsNull()) ? (dr["TimelineEffectiveDate"].ToDateTime()) : (DateTime?)null;
                }
                if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[1].Rows[0];
                    objDOGEN_Queue.objDOGEN_OSTActions.GEN_QueueRef = (dr.Table.Columns.Contains("GEN_QueueRef") && !dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ActionLkup = (dr.Table.Columns.Contains("ActionLkup") && !dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Action = (dr.Table.Columns.Contains("Action") && !dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastName = (dr.Table.Columns.Contains("LastName") && !dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.DateofBirth = (dr.Table.Columns.Contains("DateofBirth") && !dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractIDLkup = (dr.Table.Columns.Contains("ContractIDLkup") && !dr["ContractIDLkup"].IsNull()) ? dr["ContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractID = (dr.Table.Columns.Contains("ContractID") && !dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBPLkup = (dr.Table.Columns.Contains("PBPLkup") && !dr["PBPLkup"].IsNull()) ? dr["PBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBP = (dr.Table.Columns.Contains("PBP") && !dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ApplicationDate = (dr.Table.Columns.Contains("ApplicationDate") && !dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EffectiveDate = (dr.Table.Columns.Contains("EffectiveDate") && !dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate = (dr.Table.Columns.Contains("FirstLetterMailDate") && !dr["FirstLetterMailDate"].IsNull()) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate = (dr.Table.Columns.Contains("SecondLetterMailDate") && !dr["SecondLetterMailDate"].IsNull()) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired = (dr.Table.Columns.Contains("ResidentialDocumentationRequired") && !dr["ResidentialDocumentationRequired"].IsNull()) ? dr["ResidentialDocumentationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired = (dr.Table.Columns.Contains("CountyAttestationRequired") && !dr["CountyAttestationRequired"].IsNull()) ? dr["CountyAttestationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReasonLkup = (dr.Table.Columns.Contains("PendReasonLkup") && !dr["PendReasonLkup"].IsNull()) ? dr["PendReasonLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReason = (dr.Table.Columns.Contains("PendReason") && !dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrorsLkup = (dr.Table.Columns.Contains("ContainsErrorsLkup") && !dr["ContainsErrorsLkup"].IsNull()) ? dr["ContainsErrorsLkup"].ToInt64() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.PDPAutoEnrolleeInd = (dr.Table.Columns.Contains("PDPAutoEnrolleeInd") && !dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : (long?)null; 
                    //objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrors = (!dr["ContainsErrors"].IsNull()) ? dr["ContainsErrors"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResolutionLkup = (dr.Table.Columns.Contains("ResolutionLkup") && !dr["ResolutionLkup"].IsNull()) ? dr["ResolutionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Resolution = (dr.Table.Columns.Contains("Resolution") && !dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.Reason = (dr.Table.Columns.Contains("Reason") && !dr["Reason"].IsNull()) ? dr["Reason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.InitialAddressVerificationDate = (dr.Table.Columns.Contains("InitialAddressVerificationDate") && !dr["InitialAddressVerificationDate"].IsNull()) ? dr["InitialAddressVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberResponseVerificationDate = (dr.Table.Columns.Contains("MemberResponseVerificationDate") && !dr["MemberResponseVerificationDate"].IsNull()) ? dr["MemberResponseVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.SCCLetterMailDate = (dr.Table.Columns.Contains("SCCLetterMailDate") && !dr["SCCLetterMailDate"].IsNull()) ? dr["SCCLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedComplianceStartDate = (dr.Table.Columns.Contains("AdjustedComplianceStartDate") && !dr["AdjustedComplianceStartDate"].IsNull()) ? dr["AdjustedComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate = (dr.Table.Columns.Contains("AdjustedDiscrepancyReceiptDate") && !dr["AdjustedDiscrepancyReceiptDate"].IsNull()) ? dr["AdjustedDiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MARxAddressCompletedLkup = (dr.Table.Columns.Contains("MARxAddressResolutionLkup") && !dr["MARxAddressResolutionLkup"].IsNull()) ? dr["MARxAddressResolutionLkup"].ToInt64() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.PDPAutoIndicator = (dr.Table.Columns.Contains("PDPAutoEnrolleeInd") && !dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToBoolean() : false;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedDisenrollmentDate = (dr.Table.Columns.Contains("AdjustedDisenrollmentDate") && !dr["AdjustedDisenrollmentDate"].IsNull()) ? dr["AdjustedDisenrollmentDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MARxAddressResolution = (dr.Table.Columns.Contains("MARxAddressResolution") && !dr["MARxAddressResolution"].IsNull()) ? dr["MARxAddressResolution"].NullToString() : string.Empty;
                }
                if (dsResult != null && dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_QueueRefferencedCases = dsResult.Tables[2].AsEnumerable().Select(dr => new DOGEN_QueueRefferencedCases
                    {
                        Gen_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty,
                        MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentWorkQueue"])) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty,
                        MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? dr["MostRecentStatus"].NullToString() : string.Empty,
                        DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null,
                        MemberContract = (!dr["MemberContract"].IsNull()) ? dr["MemberContract"].NullToString() : string.Empty,
                        MemberPBP = (!dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty,
                        MemberCurrentHICN = (!dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty,
                        FirstLetterMailDate = !(DBNull.Value.Equals(dr["FirstLetterMailDate"])) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null,
                        SecondLetterMailDate = !(DBNull.Value.Equals(dr["SecondLetterMailDate"])) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null,
                        ParentQueueRef = dr["ParentQueueRef"].ToInt64(),
                        UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(), TimeZone) : (DateTime?)null,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = !(DBNull.Value.Equals(dr["UTCLastUpdatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(), TimeZone) : (DateTime?)null,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 3 && dsResult.Tables[3].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Comments = dsResult.Tables[3].AsEnumerable().Select(dr => new DOGEN_Comments
                    {
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        Comments = (!dr["Comments"].IsNull()) ? dr["Comments"].NullToString() : string.Empty,
                        ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0,
                        Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(), TimeZone): (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                        SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0,
                        SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 4 && dsResult.Tables[4].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Attachments = dsResult.Tables[4].AsEnumerable().Select(dr => new DOGEN_Attachments
                    {
                        GEN_AttachmentsId =  dr["GEN_AttachmentsId"].ToInt64(),
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        UploadedFileName = (!dr["UploadedFileName"].IsNull()) ? dr["UploadedFileName"].NullToString() : string.Empty,
                        FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty,
                        FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty,
                        GEN_DMSDataRef = (!dr["GEN_DMSDataRef"].IsNull()) ? dr["GEN_DMSDataRef"].ToInt64() : 0,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 6 && dsResult.Tables[6].Rows.Count > 0)
                {

                    objDOGEN_Queue.lstDOGEN_QueueWorkFlowCorrelation = dsResult.Tables[6].AsEnumerable().Select(dr => new DOGEN_QueueWorkFlowCorrelation
                    {
                        GEN_QueueRef = !(DBNull.Value.Equals(dr["GEN_QueueRef"])) ? Convert.ToInt64(dr["GEN_QueueRef"]) : 0,
                        RoleLkup = !(DBNull.Value.Equals(dr["RoleLkup"])) ? Convert.ToInt64(dr["RoleLkup"]) : 0,
                        Role = !(DBNull.Value.Equals(dr["Role"])) ? Convert.ToString(dr["Role"]) : string.Empty,
                        WorkBasketLkup = !(DBNull.Value.Equals(dr["WorkBasketLkup"])) ? Convert.ToInt64(dr["WorkBasketLkup"]) : 0,
                        WorkBasket = !(DBNull.Value.Equals(dr["WorkBasket"])) ? Convert.ToString(dr["WorkBasket"]) : string.Empty,
                        DiscripancyCategoryLkup = !(DBNull.Value.Equals(dr["DiscripancyCategoryLkup"])) ? Convert.ToInt64(dr["DiscripancyCategoryLkup"]) : 0,
                        DiscripancyCategory = !(DBNull.Value.Equals(dr["DiscripancyCategory"])) ? Convert.ToString(dr["DiscripancyCategory"]) : "NA",
                        PreviousActionLkup = !(DBNull.Value.Equals(dr["PreviousActionLkup"])) ? Convert.ToInt64(dr["PreviousActionLkup"]) : 0,
                        PreviousAction = !(DBNull.Value.Equals(dr["PreviousAction"])) ? Convert.ToString(dr["PreviousAction"]) : "NA",
                        PreviousWorkQueuesLkup = !(DBNull.Value.Equals(dr["PreviousWorkQueuesLkup"])) ? Convert.ToInt64(dr["PreviousWorkQueuesLkup"]) : 0,
                        PreviousWorkQueues = !(DBNull.Value.Equals(dr["PreviousWorkQueues"])) ? Convert.ToString(dr["PreviousWorkQueues"]) : "NA",
                        PreviousStatusLkup = !(DBNull.Value.Equals(dr["PreviousStatusLkup"])) ? Convert.ToInt64(dr["PreviousStatusLkup"]) : 0,
                        PreviousStatus = !(DBNull.Value.Equals(dr["PreviousStatus"])) ? Convert.ToString(dr["PreviousStatus"]) : "NA",
                        CurrentActionLkup = !(DBNull.Value.Equals(dr["CurrentActionLkup"])) ? Convert.ToInt64(dr["CurrentActionLkup"]) : 0,
                        CurrentAction = !(DBNull.Value.Equals(dr["CurrentAction"])) ? Convert.ToString(dr["CurrentAction"]) : "NA",
                        CurrentWorkQueuesLkup = !(DBNull.Value.Equals(dr["CurrentWorkQueuesLkup"])) ? Convert.ToInt64(dr["CurrentWorkQueuesLkup"]) : 0,
                        CurrentWorkQueues = !(DBNull.Value.Equals(dr["CurrentWorkQueues"])) ? Convert.ToString(dr["CurrentWorkQueues"]) : "NA",
                        CurrentStatusLkup = !(DBNull.Value.Equals(dr["CurrentStatusLkup"])) ? Convert.ToInt64(dr["CurrentStatusLkup"]) : 0,
                        CurrentStatus = !(DBNull.Value.Equals(dr["CurrentStatus"])) ? Convert.ToString(dr["CurrentStatus"]) : "NA",
                        IsActive = !(DBNull.Value.Equals(dr["IsActive"])) ? Convert.ToBoolean(dr["IsActive"]) : false,
                        UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                        CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : "NA"

                    }).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void MapGenQueue1(DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];
                    objDOGEN_Queue.GEN_QueueId = (dr.Table.Columns.Contains("GEN_QueueId") && !dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                    objDOGEN_Queue.BusinessSegment = (dr.Table.Columns.Contains("BusinessSegment") && !dr["BusinessSegment"].IsNull()) ? dr["BusinessSegment"].NullToString() : string.Empty;
                    objDOGEN_Queue.WorkBasketLkup = (dr.Table.Columns.Contains("WorkBasketLkup") && !dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0;
                    objDOGEN_Queue.WorkBasket = (dr.Table.Columns.Contains("WorkBasket") && !dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyCategoryLkup = (dr.Table.Columns.Contains("DiscrepancyCategoryLkup") && !dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyCategory = (dr.Table.Columns.Contains("DiscrepancyCategory") && !dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyTypeLkup = (dr.Table.Columns.Contains("DiscrepancyTypeLkup") && !dr["DiscrepancyTypeLkup"].IsNull()) ? dr["DiscrepancyTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyType = (dr.Table.Columns.Contains("DiscrepancyType") && !dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty;
                    objDOGEN_Queue.StartDate = (dr.Table.Columns.Contains("StartDate") && !dr["StartDate"].IsNull()) ? dr["StartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.PreviousWorkQueueLkup = (dr.Table.Columns.Contains("PreviousWorkQueueLkup") && !dr["PreviousWorkQueueLkup"].IsNull()) ? dr["PreviousWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedToRef = (dr.Table.Columns.Contains("AssignedToRef") && !dr["AssignedToRef"].IsNull()) ? dr["AssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedTo = (dr.Table.Columns.Contains("AssignedTo") && !dr["AssignedTo"].IsNull()) ? dr["AssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCAssignedOn = (dr.Table.Columns.Contains("UTCAssignedOn") && !dr["UTCAssignedOn"].IsNull()) ? dr["UTCAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTAssignedOn = (dr.Table.Columns.Contains("CSTAssignedOn") && !dr["CSTAssignedOn"].IsNull()) ? dr["CSTAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LockedByRef = (dr.Table.Columns.Contains("LockedByRef") && !dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LockedBy = (dr.Table.Columns.Contains("LockedBy") && !dr["LockedBy"].IsNull()) ? dr["LockedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLockedOn = (dr.Table.Columns.Contains("UTCLockedOn") && !dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTLockedOn = (dr.Table.Columns.Contains("CSTLockedOn") && !dr["CSTLockedOn"].IsNull()) ? dr["CSTLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MostRecentActionLkup = (dr.Table.Columns.Contains("MostRecentActionLkup") && !dr["MostRecentActionLkup"].IsNull()) ? dr["MostRecentActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentAction = (dr.Table.Columns.Contains("MostRecentAction") && !dr["MostRecentAction"].IsNull()) ? dr["MostRecentAction"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (dr.Table.Columns.Contains("MostRecentWorkQueueLkup") && !dr["MostRecentWorkQueueLkup"].IsNull()) ? dr["MostRecentWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentWorkQueue = (dr.Table.Columns.Contains("MostRecentWorkQueue") && !dr["MostRecentWorkQueue"].IsNull()) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentStatusLkup = (dr.Table.Columns.Contains("MostRecentStatusLkup") && !dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentStatus = (dr.Table.Columns.Contains("MostRecentStatus") && !dr["MostRecentStatus"].IsNull()) ? dr["MostRecentStatus"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemLkup = (dr.Table.Columns.Contains("SourceSystemLkup") && !dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0;
                    objDOGEN_Queue.SourceSystem = (dr.Table.Columns.Contains("SourceSystem") && !dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemId = (dr.Table.Columns.Contains("SourceSystemId") && !dr["SourceSystemId"].IsNull()) ? dr["SourceSystemId"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancySourceLkup = (dr.Table.Columns.Contains("DiscrepancySourceLkup") && !dr["DiscrepancySourceLkup"].IsNull()) ? dr["DiscrepancySourceLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancySource = (dr.Table.Columns.Contains("DiscrepancySource") && !dr["DiscrepancySource"].IsNull()) ? dr["DiscrepancySource"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyReceiptDate = (dr.Table.Columns.Contains("DiscrepancyReceiptDate") && !dr["DiscrepancyReceiptDate"].IsNull()) ? dr["DiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.ComplianceStartDate = (dr.Table.Columns.Contains("ComplianceStartDate") && !dr["ComplianceStartDate"].IsNull()) ? dr["ComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.Aging = (dr.Table.Columns.Contains("Aging") && !dr["Aging"].IsNull()) ? dr["Aging"].ToInt32() : 0;
                    objDOGEN_Queue.DiscrepancyStartDate = (dr.Table.Columns.Contains("DiscrepancyStartDate") && !dr["DiscrepancyStartDate"].IsNull()) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.DiscrepancyEndDate = (dr.Table.Columns.Contains("DiscrepancyEndDate") && !dr["DiscrepancyEndDate"].IsNull()) ? dr["DiscrepancyEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberSCCCode = (dr.Table.Columns.Contains("MemberSCCCode") && !dr["MemberSCCCode"].IsNull()) ? dr["MemberSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberID = (dr.Table.Columns.Contains("MemberID") && !dr["MemberID"].IsNull()) ? dr["MemberID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberCurrentHICN = (dr.Table.Columns.Contains("MemberCurrentHICN") && !dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.MemberCurrentMBI = (!dr["MemberCurrentMBI"].IsNull()) ? dr["MemberCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.GPSHouseholdID = (dr.Table.Columns.Contains("GPSHouseholdID") && !dr["GPSHouseholdID"].IsNull()) ? dr["GPSHouseholdID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberFirstName = (dr.Table.Columns.Contains("MemberFirstName") && !dr["MemberFirstName"].IsNull()) ? dr["MemberFirstName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberMiddleName = (dr.Table.Columns.Contains("MemberMiddleName") && !dr["MemberMiddleName"].IsNull()) ? dr["MemberMiddleName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLastName = (dr.Table.Columns.Contains("MemberLastName") && !dr["MemberLastName"].IsNull()) ? dr["MemberLastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberContractIDLkup = (dr.Table.Columns.Contains("MemberContractIDLkup") && !dr["MemberContractIDLkup"].IsNull()) ? dr["MemberContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberContractID = (dr.Table.Columns.Contains("MemberContractID") && !dr["MemberContractID"].IsNull()) ? dr["MemberContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberPBPLkup = (dr.Table.Columns.Contains("MemberPBPLkup") && !dr["MemberPBPLkup"].IsNull()) ? dr["MemberPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberPBP = (dr.Table.Columns.Contains("MemberPBP") && !dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLOBLkup = (dr.Table.Columns.Contains("MemberLOBLkup") && !dr["MemberLOBLkup"].IsNull()) ? dr["MemberLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberLOB = (dr.Table.Columns.Contains("MemberLOB") && !dr["MemberLOB"].IsNull()) ? dr["MemberLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedCountyCode = (dr.Table.Columns.Contains("MemberVerifiedCountyCode") && !dr["MemberVerifiedCountyCode"].IsNull()) ? dr["MemberVerifiedCountyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberDOB = (dr.Table.Columns.Contains("MemberDOB") && !dr["MemberDOB"].IsNull()) ? dr["MemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberGenderLkup = (dr.Table.Columns.Contains("MemberGenderLkup") && !dr["MemberGenderLkup"].IsNull()) ? dr["MemberGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberGender = (dr.Table.Columns.Contains("MemberGender") && !dr["MemberGender"].IsNull()) ? dr["MemberGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSContractIDLkup = (dr.Table.Columns.Contains("EligGPSContractIDLkup") && !dr["EligGPSContractIDLkup"].IsNull()) ? dr["EligGPSContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSContractID = (dr.Table.Columns.Contains("EligGPSContractID") && !dr["EligGPSContractID"].IsNull()) ? dr["EligGPSContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSPBPLkup = (dr.Table.Columns.Contains("EligGPSPBPLkup") && !dr["EligGPSPBPLkup"].IsNull()) ? dr["EligGPSPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSPBP = (dr.Table.Columns.Contains("EligGPSPBP") && !dr["EligGPSPBP"].IsNull()) ? dr["EligGPSPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSSCCCode = (dr.Table.Columns.Contains("EligGPSSCCCode") && !dr["EligGPSSCCCode"].IsNull()) ? dr["EligGPSSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSCurrentHICN = (dr.Table.Columns.Contains("EligGPSCurrentHICN") && !dr["EligGPSCurrentHICN"].IsNull()) ? dr["EligGPSCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligGPSCurrentMBI = (!dr["EligGPSCurrentMBI"].IsNull()) ? dr["EligGPSCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanEffectiveDate") && !dr["EligGPSInsuredPlanEffectiveDate"].IsNull()) ? dr["EligGPSInsuredPlanEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSInsuredPlanTermDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanTermDate") && !dr["EligGPSInsuredPlanTermDate"].IsNull()) ? dr["EligGPSInsuredPlanTermDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSLOBLkup = (dr.Table.Columns.Contains("EligGPSLOBLkup") && !dr["EligGPSLOBLkup"].IsNull()) ? dr["EligGPSLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSLOB = (dr.Table.Columns.Contains("EligGPSLOB") && !dr["EligGPSLOB"].IsNull()) ? dr["EligGPSLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSMemberDOB = (dr.Table.Columns.Contains("EligGPSMemberDOB") && !dr["EligGPSMemberDOB"].IsNull()) ? dr["EligGPSMemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSGenderLkup = (dr.Table.Columns.Contains("EligGPSGenderLkup") && !dr["EligGPSGenderLkup"].IsNull()) ? dr["EligGPSGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSGender = (dr.Table.Columns.Contains("EligGPSGender") && !dr["EligGPSGender"].IsNull()) ? dr["EligGPSGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRContractIDLkup = (dr.Table.Columns.Contains("EligMMRContractIDLkup") && !dr["EligMMRContractIDLkup"].IsNull()) ? dr["EligMMRContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRContractID = (dr.Table.Columns.Contains("EligMMRContractID") && !dr["EligMMRContractID"].IsNull()) ? dr["EligMMRContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPBPLkup = (dr.Table.Columns.Contains("EligMMRPBPLkup") && !dr["EligMMRPBPLkup"].IsNull()) ? dr["EligMMRPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRPBP = (dr.Table.Columns.Contains("EligMMRPBP") && !dr["EligMMRPBP"].IsNull()) ? dr["EligMMRPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRSCCCode = (dr.Table.Columns.Contains("EligMMRSCCCode") && !dr["EligMMRSCCCode"].IsNull()) ? dr["EligMMRSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.DisenrollmentDate = (dr.Table.Columns.Contains("DisenrollmentDate") && !dr["DisenrollmentDate"].IsNull()) ? dr["DisenrollmentDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRCurrentHICN = (dr.Table.Columns.Contains("EligMMRCurrentHICN") && !dr["EligMMRCurrentHICN"].IsNull()) ? dr["EligMMRCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligMMRCurrentMBI = (!dr["EligMMRCurrentMBI"].IsNull()) ? dr["EligMMRCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentStartDate") && !dr["EligMMRPaymentAdjustmentStartDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentEndDate") && !dr["EligMMRPaymentAdjustmentEndDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentMonth = (dr.Table.Columns.Contains("EligMMRPaymentMonth") && !dr["EligMMRPaymentMonth"].IsNull()) ? dr["EligMMRPaymentMonth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRDOB = (dr.Table.Columns.Contains("EligMMRDOB") && !dr["EligMMRDOB"].IsNull()) ? dr["EligMMRDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRGenderLkup = (dr.Table.Columns.Contains("EligMMRGenderLkup") && !dr["EligMMRGenderLkup"].IsNull()) ? dr["EligMMRGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRGender = (dr.Table.Columns.Contains("EligMMRGender") && !dr["EligMMRGender"].IsNull()) ? dr["EligMMRGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligOOAFlagLkup = (dr.Table.Columns.Contains("EligOOAFlagLkup") && !dr["EligOOAFlagLkup"].IsNull()) ? dr["EligOOAFlagLkup"].ToBoolean() : false;
                    //objDOGEN_Queue.EligOOAFlag = (!dr["EligOOAFlag"].IsNull()) ? dr["EligOOAFlag"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRRequestedEffectiveDate = (dr.Table.Columns.Contains("RPRRequestedEffectiveDate") && !dr["RPRRequestedEffectiveDate"].IsNull()) ? dr["RPRRequestedEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.RPRActionRequestedLkup = (dr.Table.Columns.Contains("RPRActionRequestedLkup") && !dr["RPRActionRequestedLkup"].IsNull()) ? dr["RPRActionRequestedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRActionRequested = (dr.Table.Columns.Contains("RPRActionRequested") && !dr["RPRActionRequested"].IsNull()) ? dr["RPRActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRSupervisorOrRequesterRef = (dr.Table.Columns.Contains("RPRSupervisorOrRequesterRef") && !dr["RPRSupervisorOrRequesterRef"].IsNull()) ? dr["RPRSupervisorOrRequesterRef"].ToInt64() : 0;
                    objDOGEN_Queue.RPREmployerID = (dr.Table.Columns.Contains("RPREmployerID") && !dr["RPREmployerID"].IsNull()) ? dr["RPREmployerID"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRReasonforRequest = (dr.Table.Columns.Contains("RPRReasonforRequest") && !dr["RPRReasonforRequest"].IsNull()) ? dr["RPRReasonforRequest"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRTaskPerformedLkup = (dr.Table.Columns.Contains("RPRTaskPerformedLkup") && !dr["RPRTaskPerformedLkup"].IsNull()) ? dr["RPRTaskPerformedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRTaskPerformed = (dr.Table.Columns.Contains("RPRTaskPerformed") && !dr["RPRTaskPerformed"].IsNull()) ? dr["RPRTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRCTMMember = (dr.Table.Columns.Contains("RPRCTMMember") && !dr["RPRCTMMember"].IsNull()) ? dr["RPRCTMMember"].ToBoolean() : false;
                    objDOGEN_Queue.RPRCTMNumber = (dr.Table.Columns.Contains("RPRCTMNumber") && !dr["RPRCTMNumber"].IsNull()) ? dr["RPRCTMNumber"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPREGHPMember = (dr.Table.Columns.Contains("RPREGHPMember") && !dr["RPREGHPMember"].IsNull()) ? dr["RPREGHPMember"].ToBoolean() : false;
                    objDOGEN_Queue.SCCRPRRequested = (dr.Table.Columns.Contains("SCCRPRRequested") && !dr["SCCRPRRequested"].IsNull()) ? dr["SCCRPRRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequestedZip = (dr.Table.Columns.Contains("SCCRPRRequestedZip") && !dr["SCCRPRRequestedZip"].IsNull()) ? dr["SCCRPRRequestedZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequstedSubmissionDate = (dr.Table.Columns.Contains("SCCRPRRequstedSubmissionDate") && !dr["SCCRPRRequstedSubmissionDate"].IsNull()) ? dr["SCCRPRRequstedSubmissionDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveStartDate = (dr.Table.Columns.Contains("SCCRPREffectiveStartDate") && !dr["SCCRPREffectiveStartDate"].IsNull()) ? dr["SCCRPREffectiveStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveEndDate = (dr.Table.Columns.Contains("SCCRPREffectiveEndDate") && !dr["SCCRPREffectiveEndDate"].IsNull()) ? dr["SCCRPREffectiveEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCasePended = (dr.Table.Columns.Contains("IsCasePended") && !dr["IsCasePended"].IsNull()) ? dr["IsCasePended"].ToBoolean() : false;
                    objDOGEN_Queue.PendedbyRef = (dr.Table.Columns.Contains("PendedbyRef") && !dr["PendedbyRef"].IsNull()) ? dr["PendedbyRef"].ToInt64() : 0;
                    objDOGEN_Queue.Pendedby = (dr.Table.Columns.Contains("Pendedby") && !dr["Pendedby"].IsNull()) ? dr["Pendedby"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCPendedOn = (dr.Table.Columns.Contains("UTCPendedOn") && !dr["UTCPendedOn"].IsNull()) ? dr["UTCPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTPendedOn = (dr.Table.Columns.Contains("CSTPendedOn") && !dr["CSTPendedOn"].IsNull()) ? dr["CSTPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCaseResolved = (dr.Table.Columns.Contains("IsCaseResolved") && !dr["IsCaseResolved"].IsNull()) ? dr["IsCaseResolved"].ToBoolean() : false;
                    objDOGEN_Queue.ResolvedByRef = (dr.Table.Columns.Contains("ResolvedByRef") && !dr["ResolvedByRef"].IsNull()) ? dr["ResolvedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.ResolvedBy = (dr.Table.Columns.Contains("ResolvedBy") && !dr["ResolvedBy"].IsNull()) ? dr["ResolvedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCResolvedOn = (dr.Table.Columns.Contains("UTCResolvedOn") && !dr["UTCResolvedOn"].IsNull()) ? dr["UTCResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTResolvedOn = (dr.Table.Columns.Contains("CSTResolvedOn") && !dr["CSTResolvedOn"].IsNull()) ? dr["CSTResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsParentCase = (dr.Table.Columns.Contains("IsParentCase") && !dr["IsParentCase"].IsNull()) ? dr["IsParentCase"].ToBoolean() : false;
                    objDOGEN_Queue.IsChildCase = (dr.Table.Columns.Contains("IsChildCase") && !dr["IsChildCase"].IsNull()) ? dr["IsChildCase"].ToBoolean() : false;
                    objDOGEN_Queue.ParentQueueRef = (dr.Table.Columns.Contains("ParentQueueRef") && !dr["ParentQueueRef"].IsNull()) ? dr["ParentQueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTCreatedOn = (dr.Table.Columns.Contains("CSTCreatedOn") && !dr["CSTCreatedOn"].IsNull()) ? dr["CSTCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? (dr["UTCLastUpdatedOn"].ToDateTime()) : (DateTime?)null;
                    objDOGEN_Queue.CSTLastUpdatedOn = (dr.Table.Columns.Contains("CSTLastUpdatedOn") && !dr["CSTLastUpdatedOn"].IsNull()) ? dr["CSTLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherActionRequested = (dr.Table.Columns.Contains("RPROtherActionRequested") && !dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherTaskPerformed = (dr.Table.Columns.Contains("RPROtherTaskPerformed") && !dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.PreviousAssignedToRef = (dr.Table.Columns.Contains("PreviousAssignedToRef") && !dr["PreviousAssignedToRef"].IsNull()) ? dr["PreviousAssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.PreviousAssignedTo = (dr.Table.Columns.Contains("PreviousAssignedTo") && !dr["PreviousAssignedTo"].IsNull()) ? dr["PreviousAssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.PDPAutoEnrolleeInd = (dr.Table.Columns.Contains("PDPAutoEnrolleeInd") && !dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : 0;
                    objDOGEN_Queue.ReferencedEligibilityCaseInd = (dr.Table.Columns.Contains("ReferencedEligibilityCaseInd") && !dr["ReferencedEligibilityCaseInd"].IsNull()) ? dr["ReferencedEligibilityCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.ReferencedSCCCaseInd = (dr.Table.Columns.Contains("ReferencedSCCCaseInd") && !dr["ReferencedSCCCaseInd"].IsNull()) ? dr["ReferencedSCCCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.FileTypeLkup = (dr.Table.Columns.Contains("FileTypeLkup") && !dr["FileTypeLkup"].IsNull()) ? dr["FileTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.FileType = (dr.Table.Columns.Contains("FileType") && !dr["FileType"].IsNull()) ? dr["FileType"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMDocID = (dr.Table.Columns.Contains("ODMDocID") && !dr["ODMDocID"].IsNull()) ? dr["ODMDocID"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMAddressLink = (dr.Table.Columns.Contains("ODMAddressLink") && !dr["ODMAddressLink"].IsNull()) ? dr["ODMAddressLink"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress1 = (dr.Table.Columns.Contains("UndeliveredAddress1") && !dr["UndeliveredAddress1"].IsNull()) ? dr["UndeliveredAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress2 = (dr.Table.Columns.Contains("UndeliveredAddress2") && !dr["UndeliveredAddress2"].IsNull()) ? dr["UndeliveredAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredCity = (dr.Table.Columns.Contains("UndeliveredCity") && !dr["UndeliveredCity"].IsNull()) ? dr["UndeliveredCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredState = (dr.Table.Columns.Contains("UndeliveredState") && !dr["UndeliveredState"].IsNull()) ? dr["UndeliveredState"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredZip = (dr.Table.Columns.Contains("UndeliveredZip") && !dr["UndeliveredZip"].IsNull()) ? dr["UndeliveredZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress1 = (dr.Table.Columns.Contains("COAOldAddress1") && !dr["COAOldAddress1"].IsNull()) ? dr["COAOldAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress2 = (dr.Table.Columns.Contains("COAOldAddress2") && !dr["COAOldAddress2"].IsNull()) ? dr["COAOldAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldCity = (dr.Table.Columns.Contains("COAOldCity") && !dr["COAOldCity"].IsNull()) ? dr["COAOldCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldState = (dr.Table.Columns.Contains("COAOldState") && !dr["COAOldState"].IsNull()) ? dr["COAOldState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldZip = (dr.Table.Columns.Contains("COAOldZip") && !dr["COAOldZip"].IsNull()) ? dr["COAOldZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress1 = (dr.Table.Columns.Contains("COANewAddress1") && !dr["COANewAddress1"].IsNull()) ? dr["COANewAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress2 = (dr.Table.Columns.Contains("COANewAddress2") && !dr["COANewAddress2"].IsNull()) ? dr["COANewAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewCity = (dr.Table.Columns.Contains("COANewCity") && !dr["COANewCity"].IsNull()) ? dr["COANewCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewState = (dr.Table.Columns.Contains("COANewState") && !dr["COANewState"].IsNull()) ? dr["COANewState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewZip = (dr.Table.Columns.Contains("COANewZip") && !dr["COANewZip"].IsNull()) ? dr["COANewZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.TurnAroundTime = (dr.Table.Columns.Contains("TurnAroundTime") && !dr["TurnAroundTime"].IsNull()) ? dr["TurnAroundTime"].ToInt64() : 0;
                    objDOGEN_Queue.TransactionReplyCode = (dr.Table.Columns.Contains("TransactionReplyCode") && !dr["TransactionReplyCode"].IsNull()) ? dr["TransactionReplyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.EmployeerGroupNumber = (dr.Table.Columns.Contains("EmployerGroupNumber") && !dr["EmployerGroupNumber"].IsNull()) ? dr["EmployerGroupNumber"].NullToString() : string.Empty;
                }
                if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[1].Rows[0];
                    objDOGEN_Queue.objDOGEN_OSTActions.GEN_QueueRef = (dr.Table.Columns.Contains("GEN_QueueRef") && !dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ActionLkup = (dr.Table.Columns.Contains("ActionLkup") && !dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Action = (dr.Table.Columns.Contains("Action") && !dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastName = (dr.Table.Columns.Contains("LastName") && !dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.DateofBirth = (dr.Table.Columns.Contains("DateofBirth") && !dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractIDLkup = (dr.Table.Columns.Contains("ContractIDLkup") && !dr["ContractIDLkup"].IsNull()) ? dr["ContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractID = (dr.Table.Columns.Contains("ContractID") && !dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBPLkup = (dr.Table.Columns.Contains("PBPLkup") && !dr["PBPLkup"].IsNull()) ? dr["PBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBP = (dr.Table.Columns.Contains("PBP") && !dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ApplicationDate = (dr.Table.Columns.Contains("ApplicationDate") && !dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EffectiveDate = (dr.Table.Columns.Contains("EffectiveDate") && !dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate = (dr.Table.Columns.Contains("FirstLetterMailDate") && !dr["FirstLetterMailDate"].IsNull()) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate = (dr.Table.Columns.Contains("SecondLetterMailDate") && !dr["SecondLetterMailDate"].IsNull()) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired = (dr.Table.Columns.Contains("ResidentialDocumentationRequired") && !dr["ResidentialDocumentationRequired"].IsNull()) ? dr["ResidentialDocumentationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired = (dr.Table.Columns.Contains("CountyAttestationRequired") && !dr["CountyAttestationRequired"].IsNull()) ? dr["CountyAttestationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReasonLkup = (dr.Table.Columns.Contains("PendReasonLkup") && !dr["PendReasonLkup"].IsNull()) ? dr["PendReasonLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReason = (dr.Table.Columns.Contains("PendReason") && !dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrorsLkup = (dr.Table.Columns.Contains("ContainsErrorsLkup") && !dr["ContainsErrorsLkup"].IsNull()) ? dr["ContainsErrorsLkup"].ToInt64() : (long?)null;
                    //objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrors = (!dr["ContainsErrors"].IsNull()) ? dr["ContainsErrors"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResolutionLkup = (dr.Table.Columns.Contains("ResolutionLkup") && !dr["ResolutionLkup"].IsNull()) ? dr["ResolutionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Resolution = (dr.Table.Columns.Contains("Resolution") && !dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.Reason = (dr.Table.Columns.Contains("Reason") && !dr["Reason"].IsNull()) ? dr["Reason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.InitialAddressVerificationDate = (dr.Table.Columns.Contains("InitialAddressVerificationDate") && !dr["InitialAddressVerificationDate"].IsNull()) ? dr["InitialAddressVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberResponseVerificationDate = (dr.Table.Columns.Contains("MemberResponseVerificationDate") && !dr["MemberResponseVerificationDate"].IsNull()) ? dr["MemberResponseVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.SCCLetterMailDate = (dr.Table.Columns.Contains("SCCLetterMailDate") && !dr["SCCLetterMailDate"].IsNull()) ? dr["SCCLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedComplianceStartDate = (dr.Table.Columns.Contains("AdjustedComplianceStartDate") && !dr["AdjustedComplianceStartDate"].IsNull()) ? dr["AdjustedComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate = (dr.Table.Columns.Contains("AdjustedDiscrepancyReceiptDate") && !dr["AdjustedDiscrepancyReceiptDate"].IsNull()) ? dr["AdjustedDiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                }
                if (dsResult != null && dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_QueueRefferencedCases = dsResult.Tables[2].AsEnumerable().Select(dr => new DOGEN_QueueRefferencedCases
                    {
                        Gen_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0,
                        DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty,
                        DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null,
                        DiscrepancySource = !(DBNull.Value.Equals(dr["DiscrepancySource"])) ? dr["DiscrepancySource"].NullToString() : string.Empty,
                        MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentWorkQueue"])) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty,
                        MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? dr["MostRecentStatus"].NullToString() : string.Empty,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 3 && dsResult.Tables[3].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Comments = dsResult.Tables[3].AsEnumerable().Select(dr => new DOGEN_Comments
                    {
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        Comments = (!dr["Comments"].IsNull()) ? dr["Comments"].NullToString() : string.Empty,
                        ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0,
                        Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                        SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0,
                        SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 4 && dsResult.Tables[4].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Attachments = dsResult.Tables[4].AsEnumerable().Select(dr => new DOGEN_Attachments
                    {
                        GEN_AttachmentsId = dr["GEN_AttachmentsId"].ToInt64(),
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        UploadedFileName = (!dr["UploadedFileName"].IsNull()) ? dr["UploadedFileName"].NullToString() : string.Empty,
                        FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty,
                        FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty,
                        GEN_DMSDataRef = (!dr["GEN_DMSDataRef"].IsNull()) ? dr["GEN_DMSDataRef"].ToInt64() : 0,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 6 && dsResult.Tables[6].Rows.Count > 0)
                {

                    objDOGEN_Queue.lstDOGEN_QueueWorkFlowCorrelation = dsResult.Tables[6].AsEnumerable().Select(dr => new DOGEN_QueueWorkFlowCorrelation
                    {
                        GEN_QueueRef = !(DBNull.Value.Equals(dr["GEN_QueueRef"])) ? Convert.ToInt64(dr["GEN_QueueRef"]) : 0,
                        RoleLkup = !(DBNull.Value.Equals(dr["RoleLkup"])) ? Convert.ToInt64(dr["RoleLkup"]) : 0,
                        Role = !(DBNull.Value.Equals(dr["Role"])) ? Convert.ToString(dr["Role"]) : string.Empty,
                        WorkBasketLkup = !(DBNull.Value.Equals(dr["WorkBasketLkup"])) ? Convert.ToInt64(dr["WorkBasketLkup"]) : 0,
                        WorkBasket = !(DBNull.Value.Equals(dr["WorkBasket"])) ? Convert.ToString(dr["WorkBasket"]) : string.Empty,
                        DiscripancyCategoryLkup = !(DBNull.Value.Equals(dr["DiscripancyCategoryLkup"])) ? Convert.ToInt64(dr["DiscripancyCategoryLkup"]) : 0,
                        DiscripancyCategory = !(DBNull.Value.Equals(dr["DiscripancyCategory"])) ? Convert.ToString(dr["DiscripancyCategory"]) : "NA",
                        PreviousActionLkup = !(DBNull.Value.Equals(dr["PreviousActionLkup"])) ? Convert.ToInt64(dr["PreviousActionLkup"]) : 0,
                        PreviousAction = !(DBNull.Value.Equals(dr["PreviousAction"])) ? Convert.ToString(dr["PreviousAction"]) : "NA",
                        PreviousWorkQueuesLkup = !(DBNull.Value.Equals(dr["PreviousWorkQueuesLkup"])) ? Convert.ToInt64(dr["PreviousWorkQueuesLkup"]) : 0,
                        PreviousWorkQueues = !(DBNull.Value.Equals(dr["PreviousWorkQueues"])) ? Convert.ToString(dr["PreviousWorkQueues"]) : "NA",
                        PreviousStatusLkup = !(DBNull.Value.Equals(dr["PreviousStatusLkup"])) ? Convert.ToInt64(dr["PreviousStatusLkup"]) : 0,
                        PreviousStatus = !(DBNull.Value.Equals(dr["PreviousStatus"])) ? Convert.ToString(dr["PreviousStatus"]) : "NA",
                        CurrentActionLkup = !(DBNull.Value.Equals(dr["CurrentActionLkup"])) ? Convert.ToInt64(dr["CurrentActionLkup"]) : 0,
                        CurrentAction = !(DBNull.Value.Equals(dr["CurrentAction"])) ? Convert.ToString(dr["CurrentAction"]) : "NA",
                        CurrentWorkQueuesLkup = !(DBNull.Value.Equals(dr["CurrentWorkQueuesLkup"])) ? Convert.ToInt64(dr["CurrentWorkQueuesLkup"]) : 0,
                        CurrentWorkQueues = !(DBNull.Value.Equals(dr["CurrentWorkQueues"])) ? Convert.ToString(dr["CurrentWorkQueues"]) : "NA",
                        CurrentStatusLkup = !(DBNull.Value.Equals(dr["CurrentStatusLkup"])) ? Convert.ToInt64(dr["CurrentStatusLkup"]) : 0,
                        CurrentStatus = !(DBNull.Value.Equals(dr["CurrentStatus"])) ? Convert.ToString(dr["CurrentStatus"]) : "NA",
                        IsActive = !(DBNull.Value.Equals(dr["IsActive"])) ? Convert.ToBoolean(dr["IsActive"]) : false,
                        UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                        CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : "NA"

                    }).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void MapGenQueue2(DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];
                    objDOGEN_Queue.GEN_QueueId = (dr.Table.Columns.Contains("GEN_QueueId") && !dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                    objDOGEN_Queue.BusinessSegment = (dr.Table.Columns.Contains("BusinessSegment") && !dr["BusinessSegment"].IsNull()) ? dr["BusinessSegment"].NullToString() : string.Empty;
                    objDOGEN_Queue.WorkBasketLkup = (dr.Table.Columns.Contains("WorkBasketLkup") && !dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0;
                    objDOGEN_Queue.WorkBasket = (dr.Table.Columns.Contains("WorkBasket") && !dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyCategoryLkup = (dr.Table.Columns.Contains("DiscrepancyCategoryLkup") && !dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyCategory = (dr.Table.Columns.Contains("DiscrepancyCategory") && !dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyTypeLkup = (dr.Table.Columns.Contains("DiscrepancyTypeLkup") && !dr["DiscrepancyTypeLkup"].IsNull()) ? dr["DiscrepancyTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyType = (dr.Table.Columns.Contains("DiscrepancyType") && !dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty;
                    objDOGEN_Queue.StartDate = (dr.Table.Columns.Contains("StartDate") && !dr["StartDate"].IsNull()) ? dr["StartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.PreviousWorkQueueLkup = (dr.Table.Columns.Contains("PreviousWorkQueueLkup") && !dr["PreviousWorkQueueLkup"].IsNull()) ? dr["PreviousWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedToRef = (dr.Table.Columns.Contains("AssignedToRef") && !dr["AssignedToRef"].IsNull()) ? dr["AssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedTo = (dr.Table.Columns.Contains("AssignedTo") && !dr["AssignedTo"].IsNull()) ? dr["AssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCAssignedOn = (dr.Table.Columns.Contains("UTCAssignedOn") && !dr["UTCAssignedOn"].IsNull()) ? dr["UTCAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTAssignedOn = (dr.Table.Columns.Contains("CSTAssignedOn") && !dr["CSTAssignedOn"].IsNull()) ? dr["CSTAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LockedByRef = (dr.Table.Columns.Contains("LockedByRef") && !dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LockedBy = (dr.Table.Columns.Contains("LockedBy") && !dr["LockedBy"].IsNull()) ? dr["LockedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLockedOn = (dr.Table.Columns.Contains("UTCLockedOn") && !dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTLockedOn = (dr.Table.Columns.Contains("CSTLockedOn") && !dr["CSTLockedOn"].IsNull()) ? dr["CSTLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MostRecentActionLkup = (dr.Table.Columns.Contains("MostRecentActionLkup") && !dr["MostRecentActionLkup"].IsNull()) ? dr["MostRecentActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentAction = (dr.Table.Columns.Contains("MostRecentAction") && !dr["MostRecentAction"].IsNull()) ? dr["MostRecentAction"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (dr.Table.Columns.Contains("MostRecentWorkQueueLkup") && !dr["MostRecentWorkQueueLkup"].IsNull()) ? dr["MostRecentWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentWorkQueue = (dr.Table.Columns.Contains("MostRecentWorkQueue") && !dr["MostRecentWorkQueue"].IsNull()) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentStatusLkup = (dr.Table.Columns.Contains("MostRecentStatusLkup") && !dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentStatus = (dr.Table.Columns.Contains("MostRecentStatus") && !dr["MostRecentStatus"].IsNull()) ? dr["MostRecentStatus"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemLkup = (dr.Table.Columns.Contains("SourceSystemLkup") && !dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0;
                    objDOGEN_Queue.SourceSystem = (dr.Table.Columns.Contains("SourceSystem") && !dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemId = (dr.Table.Columns.Contains("SourceSystemId") && !dr["SourceSystemId"].IsNull()) ? dr["SourceSystemId"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancySourceLkup = (dr.Table.Columns.Contains("DiscrepancySourceLkup") && !dr["DiscrepancySourceLkup"].IsNull()) ? dr["DiscrepancySourceLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancySource = (dr.Table.Columns.Contains("DiscrepancySource") && !dr["DiscrepancySource"].IsNull()) ? dr["DiscrepancySource"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyReceiptDate = (dr.Table.Columns.Contains("DiscrepancyReceiptDate") && !dr["DiscrepancyReceiptDate"].IsNull()) ? dr["DiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.ComplianceStartDate = (dr.Table.Columns.Contains("ComplianceStartDate") && !dr["ComplianceStartDate"].IsNull()) ? dr["ComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.Aging = (dr.Table.Columns.Contains("Aging") && !dr["Aging"].IsNull()) ? dr["Aging"].ToInt32() : 0;
                    objDOGEN_Queue.DiscrepancyStartDate = (dr.Table.Columns.Contains("DiscrepancyStartDate") && !dr["DiscrepancyStartDate"].IsNull()) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.DiscrepancyEndDate = (dr.Table.Columns.Contains("DiscrepancyEndDate") && !dr["DiscrepancyEndDate"].IsNull()) ? dr["DiscrepancyEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberSCCCode = (dr.Table.Columns.Contains("MemberSCCCode") && !dr["MemberSCCCode"].IsNull()) ? dr["MemberSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberID = (dr.Table.Columns.Contains("MemberID") && !dr["MemberID"].IsNull()) ? dr["MemberID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberCurrentHICN = (dr.Table.Columns.Contains("MemberCurrentHICN") && !dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.MemberCurrentMBI = (!dr["MemberCurrentMBI"].IsNull()) ? dr["MemberCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.GPSHouseholdID = (dr.Table.Columns.Contains("GPSHouseholdID") && !dr["GPSHouseholdID"].IsNull()) ? dr["GPSHouseholdID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberFirstName = (dr.Table.Columns.Contains("MemberFirstName") && !dr["MemberFirstName"].IsNull()) ? dr["MemberFirstName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberMiddleName = (dr.Table.Columns.Contains("MemberMiddleName") && !dr["MemberMiddleName"].IsNull()) ? dr["MemberMiddleName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLastName = (dr.Table.Columns.Contains("MemberLastName") && !dr["MemberLastName"].IsNull()) ? dr["MemberLastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberContractIDLkup = (dr.Table.Columns.Contains("MemberContractIDLkup") && !dr["MemberContractIDLkup"].IsNull()) ? dr["MemberContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberContractID = (dr.Table.Columns.Contains("MemberContractID") && !dr["MemberContractID"].IsNull()) ? dr["MemberContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberPBPLkup = (dr.Table.Columns.Contains("MemberPBPLkup") && !dr["MemberPBPLkup"].IsNull()) ? dr["MemberPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberPBP = (dr.Table.Columns.Contains("MemberPBP") && !dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLOBLkup = (dr.Table.Columns.Contains("MemberLOBLkup") && !dr["MemberLOBLkup"].IsNull()) ? dr["MemberLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberLOB = (dr.Table.Columns.Contains("MemberLOB") && !dr["MemberLOB"].IsNull()) ? dr["MemberLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedCountyCode = (dr.Table.Columns.Contains("MemberVerifiedCountyCode") && !dr["MemberVerifiedCountyCode"].IsNull()) ? dr["MemberVerifiedCountyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberDOB = (dr.Table.Columns.Contains("MemberDOB") && !dr["MemberDOB"].IsNull()) ? dr["MemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberGenderLkup = (dr.Table.Columns.Contains("MemberGenderLkup") && !dr["MemberGenderLkup"].IsNull()) ? dr["MemberGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberGender = (dr.Table.Columns.Contains("MemberGender") && !dr["MemberGender"].IsNull()) ? dr["MemberGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSContractIDLkup = (dr.Table.Columns.Contains("EligGPSContractIDLkup") && !dr["EligGPSContractIDLkup"].IsNull()) ? dr["EligGPSContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSContractID = (dr.Table.Columns.Contains("EligGPSContractID") && !dr["EligGPSContractID"].IsNull()) ? dr["EligGPSContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSPBPLkup = (dr.Table.Columns.Contains("EligGPSPBPLkup") && !dr["EligGPSPBPLkup"].IsNull()) ? dr["EligGPSPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSPBP = (dr.Table.Columns.Contains("EligGPSPBP") && !dr["EligGPSPBP"].IsNull()) ? dr["EligGPSPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSSCCCode = (dr.Table.Columns.Contains("EligGPSSCCCode") && !dr["EligGPSSCCCode"].IsNull()) ? dr["EligGPSSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSCurrentHICN = (dr.Table.Columns.Contains("EligGPSCurrentHICN") && !dr["EligGPSCurrentHICN"].IsNull()) ? dr["EligGPSCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligGPSCurrentMBI = (!dr["EligGPSCurrentMBI"].IsNull()) ? dr["EligGPSCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanEffectiveDate") && !dr["EligGPSInsuredPlanEffectiveDate"].IsNull()) ? dr["EligGPSInsuredPlanEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSInsuredPlanTermDate = (dr.Table.Columns.Contains("EligGPSInsuredPlanTermDate") && !dr["EligGPSInsuredPlanTermDate"].IsNull()) ? dr["EligGPSInsuredPlanTermDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSLOBLkup = (dr.Table.Columns.Contains("EligGPSLOBLkup") && !dr["EligGPSLOBLkup"].IsNull()) ? dr["EligGPSLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSLOB = (dr.Table.Columns.Contains("EligGPSLOB") && !dr["EligGPSLOB"].IsNull()) ? dr["EligGPSLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSMemberDOB = (dr.Table.Columns.Contains("EligGPSMemberDOB") && !dr["EligGPSMemberDOB"].IsNull()) ? dr["EligGPSMemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSGenderLkup = (dr.Table.Columns.Contains("EligGPSGenderLkup") && !dr["EligGPSGenderLkup"].IsNull()) ? dr["EligGPSGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSGender = (dr.Table.Columns.Contains("EligGPSGender") && !dr["EligGPSGender"].IsNull()) ? dr["EligGPSGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRContractIDLkup = (dr.Table.Columns.Contains("EligMMRContractIDLkup") && !dr["EligMMRContractIDLkup"].IsNull()) ? dr["EligMMRContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRContractID = (dr.Table.Columns.Contains("EligMMRContractID") && !dr["EligMMRContractID"].IsNull()) ? dr["EligMMRContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPBPLkup = (dr.Table.Columns.Contains("EligMMRPBPLkup") && !dr["EligMMRPBPLkup"].IsNull()) ? dr["EligMMRPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRPBP = (dr.Table.Columns.Contains("EligMMRPBP") && !dr["EligMMRPBP"].IsNull()) ? dr["EligMMRPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRSCCCode = (dr.Table.Columns.Contains("EligMMRSCCCode") && !dr["EligMMRSCCCode"].IsNull()) ? dr["EligMMRSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.DisenrollmentDate = (dr.Table.Columns.Contains("DisenrollmentDate") && !dr["DisenrollmentDate"].IsNull()) ? dr["DisenrollmentDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRCurrentHICN = (dr.Table.Columns.Contains("EligMMRCurrentHICN") && !dr["EligMMRCurrentHICN"].IsNull()) ? dr["EligMMRCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligMMRCurrentMBI = (!dr["EligMMRCurrentMBI"].IsNull()) ? dr["EligMMRCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentStartDate") && !dr["EligMMRPaymentAdjustmentStartDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = (dr.Table.Columns.Contains("EligMMRPaymentAdjustmentEndDate") && !dr["EligMMRPaymentAdjustmentEndDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentMonth = (dr.Table.Columns.Contains("EligMMRPaymentMonth") && !dr["EligMMRPaymentMonth"].IsNull()) ? dr["EligMMRPaymentMonth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRDOB = (dr.Table.Columns.Contains("EligMMRDOB") && !dr["EligMMRDOB"].IsNull()) ? dr["EligMMRDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRGenderLkup = (dr.Table.Columns.Contains("EligMMRGenderLkup") && !dr["EligMMRGenderLkup"].IsNull()) ? dr["EligMMRGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRGender = (dr.Table.Columns.Contains("EligMMRGender") && !dr["EligMMRGender"].IsNull()) ? dr["EligMMRGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligOOAFlagLkup = (dr.Table.Columns.Contains("EligOOAFlagLkup") && !dr["EligOOAFlagLkup"].IsNull()) ? dr["EligOOAFlagLkup"].ToBoolean() : false;
                    //objDOGEN_Queue.EligOOAFlag = (!dr["EligOOAFlag"].IsNull()) ? dr["EligOOAFlag"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRRequestedEffectiveDate = (dr.Table.Columns.Contains("RPRRequestedEffectiveDate") && !dr["RPRRequestedEffectiveDate"].IsNull()) ? dr["RPRRequestedEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.RPRActionRequestedLkup = (dr.Table.Columns.Contains("RPRActionRequestedLkup") && !dr["RPRActionRequestedLkup"].IsNull()) ? dr["RPRActionRequestedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRActionRequested = (dr.Table.Columns.Contains("RPRActionRequested") && !dr["RPRActionRequested"].IsNull()) ? dr["RPRActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRSupervisorOrRequesterRef = (dr.Table.Columns.Contains("RPRSupervisorOrRequesterRef") && !dr["RPRSupervisorOrRequesterRef"].IsNull()) ? dr["RPRSupervisorOrRequesterRef"].ToInt64() : 0;
                    objDOGEN_Queue.RPREmployerID = (dr.Table.Columns.Contains("RPREmployerID") && !dr["RPREmployerID"].IsNull()) ? dr["RPREmployerID"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRReasonforRequest = (dr.Table.Columns.Contains("RPRReasonforRequest") && !dr["RPRReasonforRequest"].IsNull()) ? dr["RPRReasonforRequest"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRTaskPerformedLkup = (dr.Table.Columns.Contains("RPRTaskPerformedLkup") && !dr["RPRTaskPerformedLkup"].IsNull()) ? dr["RPRTaskPerformedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRTaskPerformed = (dr.Table.Columns.Contains("RPRTaskPerformed") && !dr["RPRTaskPerformed"].IsNull()) ? dr["RPRTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRCTMMember = (dr.Table.Columns.Contains("RPRCTMMember") && !dr["RPRCTMMember"].IsNull()) ? dr["RPRCTMMember"].ToBoolean() : false;
                    objDOGEN_Queue.RPRCTMNumber = (dr.Table.Columns.Contains("RPRCTMNumber") && !dr["RPRCTMNumber"].IsNull()) ? dr["RPRCTMNumber"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPREGHPMember = (dr.Table.Columns.Contains("RPREGHPMember") && !dr["RPREGHPMember"].IsNull()) ? dr["RPREGHPMember"].ToBoolean() : false;
                    objDOGEN_Queue.SCCRPRRequested = (dr.Table.Columns.Contains("SCCRPRRequested") && !dr["SCCRPRRequested"].IsNull()) ? dr["SCCRPRRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequestedZip = (dr.Table.Columns.Contains("SCCRPRRequestedZip") && !dr["SCCRPRRequestedZip"].IsNull()) ? dr["SCCRPRRequestedZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequstedSubmissionDate = (dr.Table.Columns.Contains("SCCRPRRequstedSubmissionDate") && !dr["SCCRPRRequstedSubmissionDate"].IsNull()) ? dr["SCCRPRRequstedSubmissionDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveStartDate = (dr.Table.Columns.Contains("SCCRPREffectiveStartDate") && !dr["SCCRPREffectiveStartDate"].IsNull()) ? dr["SCCRPREffectiveStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveEndDate = (dr.Table.Columns.Contains("SCCRPREffectiveEndDate") && !dr["SCCRPREffectiveEndDate"].IsNull()) ? dr["SCCRPREffectiveEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCasePended = (dr.Table.Columns.Contains("IsCasePended") && !dr["IsCasePended"].IsNull()) ? dr["IsCasePended"].ToBoolean() : false;
                    objDOGEN_Queue.PendedbyRef = (dr.Table.Columns.Contains("PendedbyRef") && !dr["PendedbyRef"].IsNull()) ? dr["PendedbyRef"].ToInt64() : 0;
                    objDOGEN_Queue.Pendedby = (dr.Table.Columns.Contains("Pendedby") && !dr["Pendedby"].IsNull()) ? dr["Pendedby"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCPendedOn = (dr.Table.Columns.Contains("UTCPendedOn") && !dr["UTCPendedOn"].IsNull()) ? dr["UTCPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTPendedOn = (dr.Table.Columns.Contains("CSTPendedOn") && !dr["CSTPendedOn"].IsNull()) ? dr["CSTPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCaseResolved = (dr.Table.Columns.Contains("IsCaseResolved") && !dr["IsCaseResolved"].IsNull()) ? dr["IsCaseResolved"].ToBoolean() : false;
                    objDOGEN_Queue.ResolvedByRef = (dr.Table.Columns.Contains("ResolvedByRef") && !dr["ResolvedByRef"].IsNull()) ? dr["ResolvedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.ResolvedBy = (dr.Table.Columns.Contains("ResolvedBy") && !dr["ResolvedBy"].IsNull()) ? dr["ResolvedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCResolvedOn = (dr.Table.Columns.Contains("UTCResolvedOn") && !dr["UTCResolvedOn"].IsNull()) ? dr["UTCResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTResolvedOn = (dr.Table.Columns.Contains("CSTResolvedOn") && !dr["CSTResolvedOn"].IsNull()) ? dr["CSTResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsParentCase = (dr.Table.Columns.Contains("IsParentCase") && !dr["IsParentCase"].IsNull()) ? dr["IsParentCase"].ToBoolean() : false;
                    objDOGEN_Queue.IsChildCase = (dr.Table.Columns.Contains("IsChildCase") && !dr["IsChildCase"].IsNull()) ? dr["IsChildCase"].ToBoolean() : false;
                    objDOGEN_Queue.ParentQueueRef = (dr.Table.Columns.Contains("ParentQueueRef") && !dr["ParentQueueRef"].IsNull()) ? dr["ParentQueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTCreatedOn = (dr.Table.Columns.Contains("CSTCreatedOn") && !dr["CSTCreatedOn"].IsNull()) ? dr["CSTCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? (dr["UTCLastUpdatedOn"].ToDateTime()) : (DateTime?)null;
                    objDOGEN_Queue.CSTLastUpdatedOn = (dr.Table.Columns.Contains("CSTLastUpdatedOn") && !dr["CSTLastUpdatedOn"].IsNull()) ? dr["CSTLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherActionRequested = (dr.Table.Columns.Contains("RPROtherActionRequested") && !dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherTaskPerformed = (dr.Table.Columns.Contains("RPROtherTaskPerformed") && !dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.PreviousAssignedToRef = (dr.Table.Columns.Contains("PreviousAssignedToRef") && !dr["PreviousAssignedToRef"].IsNull()) ? dr["PreviousAssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.PreviousAssignedTo = (dr.Table.Columns.Contains("PreviousAssignedTo") && !dr["PreviousAssignedTo"].IsNull()) ? dr["PreviousAssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.PDPAutoEnrolleeInd = (dr.Table.Columns.Contains("PDPAutoEnrolleeInd") && !dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : 0;
                    objDOGEN_Queue.ReferencedEligibilityCaseInd = (dr.Table.Columns.Contains("ReferencedEligibilityCaseInd") && !dr["ReferencedEligibilityCaseInd"].IsNull()) ? dr["ReferencedEligibilityCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.ReferencedSCCCaseInd = (dr.Table.Columns.Contains("ReferencedSCCCaseInd") && !dr["ReferencedSCCCaseInd"].IsNull()) ? dr["ReferencedSCCCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.FileTypeLkup = (dr.Table.Columns.Contains("FileTypeLkup") && !dr["FileTypeLkup"].IsNull()) ? dr["FileTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.FileType = (dr.Table.Columns.Contains("FileType") && !dr["FileType"].IsNull()) ? dr["FileType"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMDocID = (dr.Table.Columns.Contains("ODMDocID") && !dr["ODMDocID"].IsNull()) ? dr["ODMDocID"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMAddressLink = (dr.Table.Columns.Contains("ODMAddressLink") && !dr["ODMAddressLink"].IsNull()) ? dr["ODMAddressLink"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress1 = (dr.Table.Columns.Contains("UndeliveredAddress1") && !dr["UndeliveredAddress1"].IsNull()) ? dr["UndeliveredAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress2 = (dr.Table.Columns.Contains("UndeliveredAddress2") && !dr["UndeliveredAddress2"].IsNull()) ? dr["UndeliveredAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredCity = (dr.Table.Columns.Contains("UndeliveredCity") && !dr["UndeliveredCity"].IsNull()) ? dr["UndeliveredCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredState = (dr.Table.Columns.Contains("UndeliveredState") && !dr["UndeliveredState"].IsNull()) ? dr["UndeliveredState"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredZip = (dr.Table.Columns.Contains("UndeliveredZip") && !dr["UndeliveredZip"].IsNull()) ? dr["UndeliveredZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress1 = (dr.Table.Columns.Contains("COAOldAddress1") && !dr["COAOldAddress1"].IsNull()) ? dr["COAOldAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress2 = (dr.Table.Columns.Contains("COAOldAddress2") && !dr["COAOldAddress2"].IsNull()) ? dr["COAOldAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldCity = (dr.Table.Columns.Contains("COAOldCity") && !dr["COAOldCity"].IsNull()) ? dr["COAOldCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldState = (dr.Table.Columns.Contains("COAOldState") && !dr["COAOldState"].IsNull()) ? dr["COAOldState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldZip = (dr.Table.Columns.Contains("COAOldZip") && !dr["COAOldZip"].IsNull()) ? dr["COAOldZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress1 = (dr.Table.Columns.Contains("COANewAddress1") && !dr["COANewAddress1"].IsNull()) ? dr["COANewAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress2 = (dr.Table.Columns.Contains("COANewAddress2") && !dr["COANewAddress2"].IsNull()) ? dr["COANewAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewCity = (dr.Table.Columns.Contains("COANewCity") && !dr["COANewCity"].IsNull()) ? dr["COANewCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewState = (dr.Table.Columns.Contains("COANewState") && !dr["COANewState"].IsNull()) ? dr["COANewState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewZip = (dr.Table.Columns.Contains("COANewZip") && !dr["COANewZip"].IsNull()) ? dr["COANewZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.TurnAroundTime = (dr.Table.Columns.Contains("TurnAroundTime") && !dr["TurnAroundTime"].IsNull()) ? dr["TurnAroundTime"].ToInt64() : 0;
                    objDOGEN_Queue.TransactionReplyCode = (dr.Table.Columns.Contains("TransactionReplyCode") && !dr["TransactionReplyCode"].IsNull()) ? dr["TransactionReplyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.TransactionReplyCodeLkup = (dr.Table.Columns.Contains("TRCLkup") && !dr["TRCLkup"].IsNull()) ? dr["TRCLkup"].ToInt64() : (long?)null;
                    objDOGEN_Queue.TimelineEffectiveDate= (dr.Table.Columns.Contains("TimelineEffectiveDate") && !dr["TimelineEffectiveDate"].IsNull()) ? (dr["TimelineEffectiveDate"].ToDateTime()) : (DateTime?)null;
                }
                if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[1].Rows[0];
                    objDOGEN_Queue.objDOGEN_OSTActions.GEN_QueueRef = (dr.Table.Columns.Contains("GEN_QueueRef") && !dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ActionLkup = (dr.Table.Columns.Contains("ActionLkup") && !dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Action = (dr.Table.Columns.Contains("Action") && !dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastName = (dr.Table.Columns.Contains("LastName") && !dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.DateofBirth = (dr.Table.Columns.Contains("DateofBirth") && !dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractIDLkup = (dr.Table.Columns.Contains("ContractIDLkup") && !dr["ContractIDLkup"].IsNull()) ? dr["ContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContractID = (dr.Table.Columns.Contains("ContractID") && !dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBPLkup = (dr.Table.Columns.Contains("PBPLkup") && !dr["PBPLkup"].IsNull()) ? dr["PBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PBP = (dr.Table.Columns.Contains("PBP") && !dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ApplicationDate = (dr.Table.Columns.Contains("ApplicationDate") && !dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EffectiveDate = (dr.Table.Columns.Contains("EffectiveDate") && !dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.EndDate = (dr.Table.Columns.Contains("EndDate") && !dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate = (dr.Table.Columns.Contains("FirstLetterMailDate") && !dr["FirstLetterMailDate"].IsNull()) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate = (dr.Table.Columns.Contains("SecondLetterMailDate") && !dr["SecondLetterMailDate"].IsNull()) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired = (dr.Table.Columns.Contains("ResidentialDocumentationRequired") && !dr["ResidentialDocumentationRequired"].IsNull()) ? dr["ResidentialDocumentationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired = (dr.Table.Columns.Contains("CountyAttestationRequired") && !dr["CountyAttestationRequired"].IsNull()) ? dr["CountyAttestationRequired"].ToInt32() : (long?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReasonLkup = (dr.Table.Columns.Contains("PendReasonLkup") && !dr["PendReasonLkup"].IsNull()) ? dr["PendReasonLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.PendReason = (dr.Table.Columns.Contains("PendReason") && !dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrorsLkup = (dr.Table.Columns.Contains("ContainsErrorsLkup") && !dr["ContainsErrorsLkup"].IsNull()) ? dr["ContainsErrorsLkup"].ToInt64() : (long?)null;
                    //objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrors = (!dr["ContainsErrors"].IsNull()) ? dr["ContainsErrors"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.ResolutionLkup = (dr.Table.Columns.Contains("ResolutionLkup") && !dr["ResolutionLkup"].IsNull()) ? dr["ResolutionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.Resolution = (dr.Table.Columns.Contains("Resolution") && !dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.Reason = (dr.Table.Columns.Contains("Reason") && !dr["Reason"].IsNull()) ? dr["Reason"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.InitialAddressVerificationDate = (dr.Table.Columns.Contains("InitialAddressVerificationDate") && !dr["InitialAddressVerificationDate"].IsNull()) ? dr["InitialAddressVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberResponseVerificationDate = (dr.Table.Columns.Contains("MemberResponseVerificationDate") && !dr["MemberResponseVerificationDate"].IsNull()) ? dr["MemberResponseVerificationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.MemberVerifiedState = (dr.Table.Columns.Contains("MemberVerifiedState") && !dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.SCCLetterMailDate = (dr.Table.Columns.Contains("SCCLetterMailDate") && !dr["SCCLetterMailDate"].IsNull()) ? dr["SCCLetterMailDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedComplianceStartDate = (dr.Table.Columns.Contains("AdjustedComplianceStartDate") && !dr["AdjustedComplianceStartDate"].IsNull()) ? dr["AdjustedComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.AdjustedDiscrepancyReceiptDate = (dr.Table.Columns.Contains("AdjustedDiscrepancyReceiptDate") && !dr["AdjustedDiscrepancyReceiptDate"].IsNull()) ? dr["AdjustedDiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.IsActive = (dr.Table.Columns.Contains("IsActive") && !dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCCreatedOn = (dr.Table.Columns.Contains("UTCCreatedOn") && !dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedByRef = (dr.Table.Columns.Contains("CreatedByRef") && !dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.CreatedBy = (dr.Table.Columns.Contains("CreatedBy") && !dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_OSTActions.UTCLastUpdatedOn = (dr.Table.Columns.Contains("UTCLastUpdatedOn") && !dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedByRef = (dr.Table.Columns.Contains("LastUpdatedByRef") && !dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedBy = (dr.Table.Columns.Contains("LastUpdatedBy") && !dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                }
                if (dsResult != null && dsResult.Tables.Count > 2 && dsResult.Tables[2].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_QueueRefferencedCases = dsResult.Tables[2].AsEnumerable().Select(dr => new DOGEN_QueueRefferencedCases
                    {
                        Gen_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0,
                        DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty,
                        DiscrepancyStartDate = !(DBNull.Value.Equals(dr["DiscrepancyStartDate"])) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null,
                        DiscrepancySource = !(DBNull.Value.Equals(dr["DiscrepancySource"])) ? dr["DiscrepancySource"].NullToString() : string.Empty,
                        MostRecentWorkQueue = !(DBNull.Value.Equals(dr["MostRecentWorkQueue"])) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty,
                        MostRecentStatus = !(DBNull.Value.Equals(dr["MostRecentStatus"])) ? dr["MostRecentStatus"].NullToString() : string.Empty,
                        DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty,
                        DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 3 && dsResult.Tables[3].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Comments = dsResult.Tables[3].AsEnumerable().Select(dr => new DOGEN_Comments
                    {
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        Comments = (!dr["Comments"].IsNull()) ? dr["Comments"].NullToString() : string.Empty,
                        ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0,
                        Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                        SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0,
                        SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 4 && dsResult.Tables[4].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Attachments = dsResult.Tables[4].AsEnumerable().Select(dr => new DOGEN_Attachments
                    {
                        GEN_AttachmentsId = dr["GEN_AttachmentsId"].ToInt64(),
                        GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0,
                        UploadedFileName = (!dr["UploadedFileName"].IsNull()) ? dr["UploadedFileName"].NullToString() : string.Empty,
                        FileName = (!dr["FileName"].IsNull()) ? dr["FileName"].NullToString() : string.Empty,
                        FilePath = (!dr["FilePath"].IsNull()) ? dr["FilePath"].NullToString() : string.Empty,
                        GEN_DMSDataRef = (!dr["GEN_DMSDataRef"].IsNull()) ? dr["GEN_DMSDataRef"].ToInt64() : 0,
                        IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false,
                        UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0,
                        UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0,
                        CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty,
                        LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 6 && dsResult.Tables[6].Rows.Count > 0)
                {

                    objDOGEN_Queue.lstDOGEN_QueueWorkFlowCorrelation = dsResult.Tables[6].AsEnumerable().Select(dr => new DOGEN_QueueWorkFlowCorrelation
                    {
                        GEN_QueueRef = !(DBNull.Value.Equals(dr["GEN_QueueRef"])) ? Convert.ToInt64(dr["GEN_QueueRef"]) : 0,
                        RoleLkup = !(DBNull.Value.Equals(dr["RoleLkup"])) ? Convert.ToInt64(dr["RoleLkup"]) : 0,
                        Role = !(DBNull.Value.Equals(dr["Role"])) ? Convert.ToString(dr["Role"]) : string.Empty,
                        WorkBasketLkup = !(DBNull.Value.Equals(dr["WorkBasketLkup"])) ? Convert.ToInt64(dr["WorkBasketLkup"]) : 0,
                        WorkBasket = !(DBNull.Value.Equals(dr["WorkBasket"])) ? Convert.ToString(dr["WorkBasket"]) : string.Empty,
                        DiscripancyCategoryLkup = !(DBNull.Value.Equals(dr["DiscripancyCategoryLkup"])) ? Convert.ToInt64(dr["DiscripancyCategoryLkup"]) : 0,
                        DiscripancyCategory = !(DBNull.Value.Equals(dr["DiscripancyCategory"])) ? Convert.ToString(dr["DiscripancyCategory"]) : "NA",
                        PreviousActionLkup = !(DBNull.Value.Equals(dr["PreviousActionLkup"])) ? Convert.ToInt64(dr["PreviousActionLkup"]) : 0,
                        PreviousAction = !(DBNull.Value.Equals(dr["PreviousAction"])) ? Convert.ToString(dr["PreviousAction"]) : "NA",
                        PreviousWorkQueuesLkup = !(DBNull.Value.Equals(dr["PreviousWorkQueuesLkup"])) ? Convert.ToInt64(dr["PreviousWorkQueuesLkup"]) : 0,
                        PreviousWorkQueues = !(DBNull.Value.Equals(dr["PreviousWorkQueues"])) ? Convert.ToString(dr["PreviousWorkQueues"]) : "NA",
                        PreviousStatusLkup = !(DBNull.Value.Equals(dr["PreviousStatusLkup"])) ? Convert.ToInt64(dr["PreviousStatusLkup"]) : 0,
                        PreviousStatus = !(DBNull.Value.Equals(dr["PreviousStatus"])) ? Convert.ToString(dr["PreviousStatus"]) : "NA",
                        CurrentActionLkup = !(DBNull.Value.Equals(dr["CurrentActionLkup"])) ? Convert.ToInt64(dr["CurrentActionLkup"]) : 0,
                        CurrentAction = !(DBNull.Value.Equals(dr["CurrentAction"])) ? Convert.ToString(dr["CurrentAction"]) : "NA",
                        CurrentWorkQueuesLkup = !(DBNull.Value.Equals(dr["CurrentWorkQueuesLkup"])) ? Convert.ToInt64(dr["CurrentWorkQueuesLkup"]) : 0,
                        CurrentWorkQueues = !(DBNull.Value.Equals(dr["CurrentWorkQueues"])) ? Convert.ToString(dr["CurrentWorkQueues"]) : "NA",
                        CurrentStatusLkup = !(DBNull.Value.Equals(dr["CurrentStatusLkup"])) ? Convert.ToInt64(dr["CurrentStatusLkup"]) : 0,
                        CurrentStatus = !(DBNull.Value.Equals(dr["CurrentStatus"])) ? Convert.ToString(dr["CurrentStatus"]) : "NA",
                        IsActive = !(DBNull.Value.Equals(dr["IsActive"])) ? Convert.ToBoolean(dr["IsActive"]) : false,
                        UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? dr["UTCCreatedOn"].ToDateTime() : (DateTime?)null,
                        CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                        CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : "NA"

                    }).ToList();

                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ExceptionTypes GetQueueSendOOALetter(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            objDOGEN_Queue = null;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueIdsToSkip";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strGEN_QueueIdsToSkip.ToString();
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueSendOOALetter, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapGenQueue1(dsTable, out objDOGEN_Queue);
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
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }

        private void MapQueueSendOOALetter(DataSet dstTable, out List<DOGEN_Queue> lstGenQueue)
        {
            lstGenQueue = new List<DOGEN_Queue>();
            if (dstTable.Tables.Count > 0)
            {
                foreach (DataRow dr in dstTable.Tables[0].Rows)
                {
                    DOGEN_Queue objDOGEN_Queue = new DOGEN_Queue();
                    if (dr.Table.Columns.Contains("GEN_QueueId"))
                    {
                        if (!DBNull.Value.Equals(dr["GEN_QueueId"]))
                        {
                            objDOGEN_Queue.GEN_QueueId = (long?)dr["GEN_QueueId"];
                        }
                    }
                    if (dr.Table.Columns.Contains("BusinessSegmentLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["BusinessSegmentLkup"]))
                        {
                            objDOGEN_Queue.BusinessSegmentLkup = (long)dr["BusinessSegmentLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("WorkBasketLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["WorkBasketLkup"]))
                        {
                            objDOGEN_Queue.WorkBasketLkup = (long)dr["WorkBasketLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategoryLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategoryLkup"]))
                        {
                            objDOGEN_Queue.DiscrepancyCategoryLkup = (long)dr["DiscrepancyCategoryLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyTypeLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyTypeLkup"]))
                        {
                            objDOGEN_Queue.DiscrepancyTypeLkup = (long)dr["DiscrepancyTypeLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("StartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["StartDate"]))
                        {
                            objDOGEN_Queue.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("EndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["EndDate"]))
                        {
                            objDOGEN_Queue.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentActionLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentActionLkup"]))
                        {
                            objDOGEN_Queue.MostRecentActionLkup = (long)dr["MostRecentActionLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentWorkQueueLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentWorkQueueLkup"]))
                        {
                            objDOGEN_Queue.MostRecentWorkQueueLkup = (long)dr["MostRecentWorkQueueLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("MostRecentStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MostRecentStatusLkup"]))
                        {
                            objDOGEN_Queue.MostRecentStatusLkup = (long)dr["MostRecentStatusLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("SourceSystemLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["SourceSystemLkup"]))
                        {
                            objDOGEN_Queue.SourceSystemLkup = (long)dr["SourceSystemLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancySourceLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancySourceLkup"]))
                        {
                            objDOGEN_Queue.DiscrepancySourceLkup = (long)dr["DiscrepancySourceLkup"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyReceiptDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyReceiptDate"]))
                        {
                            objDOGEN_Queue.DiscrepancyReceiptDate = Convert.ToDateTime(dr["DiscrepancyReceiptDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ComplianceStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["ComplianceStartDate"]))
                        {
                            objDOGEN_Queue.ComplianceStartDate = Convert.ToDateTime(dr["ComplianceStartDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("DisenrollmentDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DisenrollmentDate"]))
                        {
                            objDOGEN_Queue.DisenrollmentDate = Convert.ToDateTime(dr["DisenrollmentDate"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("Aging"))
                    {
                        if (!DBNull.Value.Equals(dr["Aging"]))
                        {
                            objDOGEN_Queue.Aging = (long)dr["Aging"];
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyStartDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyStartDate"]))
                        {
                            objDOGEN_Queue.DiscrepancyStartDate = Convert.ToDateTime(dr["DiscrepancyStartDate"]);
                        }
                    }

                    if (dr.Table.Columns.Contains("DiscrepancyEndDate"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyEndDate"]))
                        {
                            objDOGEN_Queue.DiscrepancyEndDate = Convert.ToDateTime(dr["DiscrepancyEndDate"]);
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberSCCCode"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberSCCCode"]))
                        {
                            objDOGEN_Queue.MemberSCCCode = dr["MemberSCCCode"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberID"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberID"]))
                        {
                            objDOGEN_Queue.MemberID = dr["MemberID"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberCurrentHICN"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberCurrentHICN"]))
                        {
                            objDOGEN_Queue.MemberCurrentHICN = dr["MemberCurrentHICN"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("GPSHouseholdID"))
                    {
                        if (!DBNull.Value.Equals(dr["GPSHouseholdID"]))
                        {
                            objDOGEN_Queue.GPSHouseholdID = dr["GPSHouseholdID"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberFirstName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberFirstName"]))
                        {
                            objDOGEN_Queue.MemberFirstName = dr["MemberFirstName"].ToString();
                        }
                    }

                    if (dr.Table.Columns.Contains("MemberMiddleName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberMiddleName"]))
                        {
                            objDOGEN_Queue.MemberMiddleName = dr["MemberMiddleName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberLastName"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLastName"]))
                        {
                            objDOGEN_Queue.MemberLastName = dr["MemberLastName"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberContractIDLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberContractIDLkup"]))
                        {
                            objDOGEN_Queue.MemberContractIDLkup = Convert.ToInt64(dr["MemberContractIDLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberPBPLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberPBPLkup"]))
                        {
                            objDOGEN_Queue.MemberPBPLkup = Convert.ToInt64(dr["MemberPBPLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberLOBLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberLOBLkup"]))
                        {
                            objDOGEN_Queue.MemberLOBLkup = Convert.ToInt64(dr["MemberLOBLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberVerifiedState"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberVerifiedState"]))
                        {
                            objDOGEN_Queue.MemberVerifiedState = dr["MemberVerifiedState"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberVerifiedCountyCode"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberVerifiedCountyCode"]))
                        {
                            objDOGEN_Queue.MemberVerifiedCountyCode = dr["MemberVerifiedCountyCode"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("DiscrepancyCategory"))
                    {
                        if (!DBNull.Value.Equals(dr["DiscrepancyCategory"]))
                        {
                            objDOGEN_Queue.DiscrepancyCategory = dr["DiscrepancyCategory"].ToString();
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberDOB"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberDOB"]))
                        {
                            objDOGEN_Queue.MemberDOB = Convert.ToDateTime(dr["MemberDOB"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("MemberGenderLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["MemberGenderLkup"]))
                        {
                            objDOGEN_Queue.MemberGenderLkup = Convert.ToInt64(dr["MemberGenderLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("IsParentCase"))
                    {
                        if (!DBNull.Value.Equals(dr["IsParentCase"]))
                        {
                            objDOGEN_Queue.IsParentCase = Convert.ToBoolean(dr["IsParentCase"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("IsChildCase"))
                    {
                        if (!DBNull.Value.Equals(dr["IsChildCase"]))
                        {
                            objDOGEN_Queue.IsParentCase = Convert.ToBoolean(dr["IsChildCase"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("ParentQueueRef"))
                    {
                        if (!DBNull.Value.Equals(dr["ParentQueueRef"]))
                        {
                            objDOGEN_Queue.ParentQueueRef = Convert.ToInt64(dr["ParentQueueRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("IsActive"))
                    {
                        if (!DBNull.Value.Equals(dr["IsActive"]))
                        {
                            objDOGEN_Queue.IsActive = Convert.ToBoolean(dr["IsActive"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCCreatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCCreatedOn"]))
                        {
                            objDOGEN_Queue.UTCCreatedOn = Convert.ToDateTime(dr["UTCCreatedOn"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("CreatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["CreatedByRef"]))
                        {
                            objDOGEN_Queue.CreatedByRef = Convert.ToInt64(dr["CreatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("UTCLastUpdatedOn"))
                    {
                        if (!DBNull.Value.Equals(dr["UTCLastUpdatedOn"]))
                        {
                            objDOGEN_Queue.UTCLastUpdatedOn = Convert.ToDateTime(dr["UTCLastUpdatedOn"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("LastUpdatedByRef"))
                    {
                        if (!DBNull.Value.Equals(dr["LastUpdatedByRef"]))
                        {
                            objDOGEN_Queue.LastUpdatedByRef = Convert.ToInt64(dr["LastUpdatedByRef"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("OOALetterStatusLkup"))
                    {
                        if (!DBNull.Value.Equals(dr["OOALetterStatusLkup"]))
                        {
                            objDOGEN_Queue.OOALetterStatusLkup = Convert.ToInt64(dr["OOALetterStatusLkup"]);
                        }
                    }
                    if (dr.Table.Columns.Contains("IsRestricted"))
                    {
                        if (!DBNull.Value.Equals(dr["IsRestricted"]))
                        {
                            objDOGEN_Queue.IsRestricted = Convert.ToBoolean(dr["IsRestricted"]);
                        }
                    }
                    ////////////////////////////////////////////////////////////////////

                    objDOGEN_Queue.objDOGEN_OSTActions = new DOGEN_OSTActions();
                    if (dr.Table.Columns.Contains("GEN_OSTActionsId"))
                    {
                        if (!DBNull.Value.Equals(dr["GEN_OSTActionsId"]))
                        {
                            objDOGEN_Queue.objDOGEN_OSTActions.GEN_OSTActionsId = Convert.ToInt64(dr["GEN_OSTActionsId"]);

                            objDOGEN_Queue.objDOGEN_OSTActions.GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.ActionLkup = (!dr["ActionLkup"].IsNull()) ? dr["ActionLkup"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.Action = (!dr["Action"].IsNull()) ? dr["Action"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.LastName = (!dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.DateofBirth = (!dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.ContractIDLkup = (!dr["ContractIDLkup"].IsNull()) ? dr["ContractIDLkup"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.ContractID = (!dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.PBPLkup = (!dr["PBPLkup"].IsNull()) ? dr["PBPLkup"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.PBP = (!dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.ApplicationDate = (!dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.EffectiveDate = (!dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.EndDate = (!dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.FirstLetterMailDate = (!dr["FirstLetterMailDate"].IsNull()) ? dr["FirstLetterMailDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.SecondLetterMailDate = (!dr["SecondLetterMailDate"].IsNull()) ? dr["SecondLetterMailDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.ResidentialDocumentationRequired = (!dr["ResidentialDocumentationRequired"].IsNull()) ? dr["ResidentialDocumentationRequired"].ToInt32() :(long?) null;
                            objDOGEN_Queue.objDOGEN_OSTActions.CountyAttestationRequired = (!dr["CountyAttestationRequired"].IsNull()) ? dr["CountyAttestationRequired"].ToInt32() : (long?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.PendReasonLkup = (!dr["PendReasonLkup"].IsNull()) ? dr["PendReasonLkup"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.PendReason = (!dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.ContainsErrorsLkup = (!dr["ContainsErrorsLkup"].IsNull()) ? dr["ContainsErrorsLkup"].ToInt64() : (long?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.ResolutionLkup = (!dr["ResolutionLkup"].IsNull()) ? dr["ResolutionLkup"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.Resolution = (!dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.Reason = (!dr["Reason"].IsNull()) ? dr["Reason"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.InitialAddressVerificationDate = (!dr["InitialAddressVerificationDate"].IsNull()) ? dr["InitialAddressVerificationDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.MemberResponseVerificationDate = (!dr["MemberResponseVerificationDate"].IsNull()) ? dr["MemberResponseVerificationDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.MemberVerifiedState = (!dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                            objDOGEN_Queue.objDOGEN_OSTActions.SCCLetterMailDate = (!dr["SCCLetterMailDate"].IsNull()) ? dr["SCCLetterMailDate"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.IsActive = (!dr["OSTIsActive"].IsNull()) ? dr["OSTIsActive"].ToBoolean() : false;
                            objDOGEN_Queue.objDOGEN_OSTActions.UTCCreatedOn = (!dr["OSTUTCCreatedOn"].IsNull()) ? dr["OSTUTCCreatedOn"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.CreatedByRef = (!dr["OSTCreatedByRef"].IsNull()) ? dr["OSTCreatedByRef"].ToInt64() : 0;
                            objDOGEN_Queue.objDOGEN_OSTActions.UTCLastUpdatedOn = (!dr["OSTUTCLastUpdatedOn"].IsNull()) ? dr["OSTUTCLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                            objDOGEN_Queue.objDOGEN_OSTActions.LastUpdatedByRef = (!dr["OSTLastUpdatedByRef"].IsNull()) ? dr["OSTLastUpdatedByRef"].ToInt64() : 0;
                        }
                        else
                        {
                            objDOGEN_Queue.objDOGEN_OSTActions = null;
                        }
                    }
                    
                    lstGenQueue.Add(objDOGEN_Queue);
                }
            }
        }

        public ExceptionTypes GetQueueCMSTransaction(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            objDOGEN_Queue = null;
            try
            {
                DAHelper dah = new DAHelper();
                long lErrocode = 0;
                long lErrorNumber = 0;
                DataSet dsTable = new DataSet();

                List<SqlParameter> parameters = new List<SqlParameter>();
                SqlParameter sqlParam;

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueIdsToSkip";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = strGEN_QueueIdsToSkip.ToString();
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = dah.ExecuteSelectSP(ConstantTexts.SP_USP_APP_SEL_QueueCMSTransaction, parameters.ToArray(), out dsTable, out lErrocode, out lErrorNumber, out errorMessage);
                if (executionResult == 0)
                {
                    if (dsTable.Tables.Count > 0 && dsTable.Tables[0].Rows.Count > 0)
                    {
                        MapGenQueue2(dsTable, out objDOGEN_Queue);
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
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }
    }
}
