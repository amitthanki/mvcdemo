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
    public class DALEligibility
    {  
        DAHelper _objDAHelper = new DAHelper();
        List<SqlParameter> _lstParameters;
        private DataSet _dsResult;
        public ExceptionTypes CreateCase(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
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


                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@ErrorMessage";
                //sqlParam.SqlDbType = SqlDbType.VarChar;
                //sqlParam.Value = string.Empty;
                //sqlParam.Direction = ParameterDirection.Output;
                //sqlParam.Size = 2000;
                //parameters.Add(sqlParam);

                if (objDOGEN_Queue.IsClosedAndCreateNew)
                {
                    sqlParam = new SqlParameter();
                    sqlParam.ParameterName = "@IsCloneCase";
                    sqlParam.DbType = DbType.Boolean;
                    sqlParam.Value = objDOGEN_Queue.IsClosedAndCreateNew;
                    parameters.Add(sqlParam);
                }

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@BusinessSegmentLkup";
                //sqlParam.DbType = DbType.Int64;
                //sqlParam.Value = objDOGEN_Queue.BusinessSegmentLkup;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@WorkBasketLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.WorkBasketLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyCategoryLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyCategoryLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyTypeLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.StartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@StartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.StartDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EndDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousAssignedToRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousAssignedToRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentActionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentWorkQueueLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentWorkQueueLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MostRecentStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MostRecentStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SourceSystemLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SourceSystemId";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SourceSystemId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancySourceLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancySourceLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyReceiptDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyReceiptDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyReceiptDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ComplianceStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.ComplianceStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ComplianceStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ComplianceStartDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyStartDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DiscrepancyEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.DiscrepancyEndDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberSCCCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberID";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberID != null ? objDOGEN_Queue.MemberID.ToUpper() : null;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberCurrentHICN.ToUpper();
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@MemberCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.MemberCurrentMBI;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GPSHouseholdID";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.GPSHouseholdID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberFirstName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberFirstName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberMiddleName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberMiddleName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLastName";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberLastName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberPBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberLOBLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberLOBLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedState";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedState;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedCountyCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.MemberVerifiedCountyCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.MemberDOB;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberDOBId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.MemberGenderLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSPBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligGPSSCCCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligGPSCurrentHICN;
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@EligGPSCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.EligGPSCurrentMBI;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanEffectiveDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanEffectiveDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanTermDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSInsuredPlanTermDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSInsuredPlanTermDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSLOBLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSLOBLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSMemberDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOB;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSMemberDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSMemberDOBId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligGPSGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligGPSGenderLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRContractIDLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPBPLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRSCCCode";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligMMRSCCCode;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRCurrentHICN";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EligMMRCurrentHICN;
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@EligMMRCurrentMBI";
                //sqlParam.DbType = DbType.String;
                //sqlParam.Value = objDOGEN_Queue.EligMMRCurrentMBI;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentStartDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentAdjustmentEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentAdjustmentEndDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRPaymentMonth";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRPaymentMonth;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRDOB";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.EligMMRDOB;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRDOBId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRDOBId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligMMRGenderLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.EligMMRGenderLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EligOOAFlagLkup";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.EligOOAFlagLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRRequestedEffectiveDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRActionRequestedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPROtherActionRequested;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRSupervisorOrRequesterRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPRReasonforRequest;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RPRTaskPerformedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPROtherTaskPerformed;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.RPRCTMMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPRCTMNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.RPREGHPMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.RPREmployerID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequested";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequested;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequestedZip";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequestedZip;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPRRequstedSubmissionDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPRRequstedSubmissionDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveStartDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveStartDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveStartDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveEndDate";
                sqlParam.DbType = DbType.DateTime;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SCCRPREffectiveEndDateId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.SCCRPREffectiveEndDateId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsParentCase";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsParentCase;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsChildCase";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsChildCase;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ParentQueueRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ParentQueueRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.Comments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReAssignUserRef";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.ReAssignUserRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CasesComments";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.CasesComments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.RoleLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousActionLkup;
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@PreviousWorkQueuesLkup";
                //sqlParam.DbType = DbType.Int64;
                //sqlParam.Value = objDOGEN_Queue.PreviousWorkQueuesLkup;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PreviousStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.PreviousStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentActionLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentActionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentWorkQueuesLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentWorkQueuesLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CurrentStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.CurrentStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_Queue.LoginUserId;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = string.Empty;
                parameters.Add(sqlParam);

                //sqlParam = new SqlParameter();
                //sqlParam.ParameterName = "@GEN_QueueId";
                //sqlParam.DbType = DbType.Int64;
                //sqlParam.Value = 0;
                //parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsRestricted";
                sqlParam.DbType = DbType.Boolean;
                sqlParam.Value = objDOGEN_Queue.IsRestricted;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EmployerGroupNumber";
                sqlParam.DbType = DbType.String;
                sqlParam.Value = objDOGEN_Queue.EmployeerGroupNumber;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@BusinessSegmentLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = 0;
                sqlParam.Direction = ParameterDirection.Output;
                parameters.Add(sqlParam);

                long executionResult = 0;
                //if (objDOADM_UserMaster.ADM_UserMasterId == 0)
                //{
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_APP_INS_CreateSuspectCase, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@GEN_QueueId");
                if (sqlParam != null)
                {
                    objDOGEN_Queue.GEN_QueueId = sqlParam.Value != null ? objDOGEN_Queue.GEN_QueueId = sqlParam.Value.ToInt64() : 0;
                }
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@BusinessSegmentLkup");
                if (!sqlParam.IsNull())
                {
                    objDOGEN_Queue.BusinessSegmentLkup = 0;
                    objDOGEN_Queue.BusinessSegmentLkup = (!sqlParam.Value.IsNull()) ? objDOGEN_Queue.BusinessSegmentLkup = sqlParam.Value.ToInt64() : 0;
                }
                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    //MapGenQueue(_dsResult, out objDOGEN_Queue);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;

                //if (executionResult == 0)
                //{
                //    return ExceptionTypes.Success;
                //}
                //else
                //{
                //    return ExceptionTypes.UnknownError;
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }


        public ExceptionTypes GetGenQueueByID(long? TimeZone,long genQueueID, out DOGEN_Queue objDOGEN_Queue)
        {
            
             _dsResult = new DataSet();
            _lstParameters = new List<SqlParameter>();
            SqlParameter sqlParam;
            long lErrocode = 0;
            long lErrorNumber = 0;
            string errorMessage = string.Empty;
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@GEN_QueueId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = genQueueID;
                _lstParameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                _lstParameters.Add(sqlParam);

                long executionResult = _objDAHelper.ExecuteSelectSP(ConstantTexts.SP_APP_SEL_Case, _lstParameters.ToArray(), out _dsResult, out lErrocode, out lErrorNumber, out errorMessage);

                sqlParam = _lstParameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

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
                _lstParameters = null;
            }
        }
        private void MapGenQueue(long? TimeZone, DataSet dsResult, out DOGEN_Queue objDOGEN_Queue)
        {
            objDOGEN_Queue = new DOGEN_Queue();
            try
            {
                if (dsResult != null && dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[0].Rows[0];
                    objDOGEN_Queue.GEN_QueueId = (!dr["GEN_QueueId"].IsNull()) ? dr["GEN_QueueId"].ToInt64() : 0;
                    objDOGEN_Queue.BusinessSegment = (!dr["BusinessSegment"].IsNull()) ? dr["BusinessSegment"].NullToString() : string.Empty;
                    objDOGEN_Queue.WorkBasketLkup = (!dr["WorkBasketLkup"].IsNull()) ? dr["WorkBasketLkup"].ToInt64() : 0;
                    objDOGEN_Queue.WorkBasket = (!dr["WorkBasket"].IsNull()) ? dr["WorkBasket"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyCategoryLkup = (!dr["DiscrepancyCategoryLkup"].IsNull()) ? dr["DiscrepancyCategoryLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyCategory = (!dr["DiscrepancyCategory"].IsNull()) ? dr["DiscrepancyCategory"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyTypeLkup = (!dr["DiscrepancyTypeLkup"].IsNull()) ? dr["DiscrepancyTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancyType = (!dr["DiscrepancyType"].IsNull()) ? dr["DiscrepancyType"].NullToString() : string.Empty;
                    objDOGEN_Queue.StartDate = (!dr["StartDate"].IsNull()) ? dr["StartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EndDate = (!dr["EndDate"].IsNull()) ? dr["EndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.PreviousWorkQueueLkup = (!dr["PreviousWorkQueueLkup"].IsNull()) ? dr["PreviousWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedToRef = (!dr["AssignedToRef"].IsNull()) ? dr["AssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.AssignedTo = (!dr["AssignedTo"].IsNull()) ? dr["AssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCAssignedOn = (!dr["UTCAssignedOn"].IsNull()) ? dr["UTCAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTAssignedOn = (!dr["CSTAssignedOn"].IsNull()) ? dr["CSTAssignedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LockedByRef = (!dr["LockedByRef"].IsNull()) ? dr["LockedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LockedBy = (!dr["LockedBy"].IsNull()) ? dr["LockedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLockedOn = (!dr["UTCLockedOn"].IsNull()) ? dr["UTCLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTLockedOn = (!dr["CSTLockedOn"].IsNull()) ? dr["CSTLockedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MostRecentActionLkup = (!dr["MostRecentActionLkup"].IsNull()) ? dr["MostRecentActionLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentAction = (!dr["MostRecentAction"].IsNull()) ? dr["MostRecentAction"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentWorkQueueLkup = (!dr["MostRecentWorkQueueLkup"].IsNull()) ? dr["MostRecentWorkQueueLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentWorkQueue = (!dr["MostRecentWorkQueue"].IsNull()) ? dr["MostRecentWorkQueue"].NullToString() : string.Empty;
                    objDOGEN_Queue.MostRecentStatusLkup = (!dr["MostRecentStatusLkup"].IsNull()) ? dr["MostRecentStatusLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MostRecentStatus = (!dr["MostRecentStatus"].IsNull()) ? dr["MostRecentStatus"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0;
                    objDOGEN_Queue.SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty;
                    objDOGEN_Queue.SourceSystemId = (!dr["SourceSystemId"].IsNull()) ? dr["SourceSystemId"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancySourceLkup = (!dr["DiscrepancySourceLkup"].IsNull()) ? dr["DiscrepancySourceLkup"].ToInt64() : 0;
                    objDOGEN_Queue.DiscrepancySource = (!dr["DiscrepancySource"].IsNull()) ? dr["DiscrepancySource"].NullToString() : string.Empty;
                    objDOGEN_Queue.DiscrepancyReceiptDate = (!dr["DiscrepancyReceiptDate"].IsNull()) ? dr["DiscrepancyReceiptDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.ComplianceStartDate = (!dr["ComplianceStartDate"].IsNull()) ? dr["ComplianceStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.Aging = (!dr["Aging"].IsNull()) ? dr["Aging"].ToInt32() : 0;
                    objDOGEN_Queue.DiscrepancyStartDate = (!dr["DiscrepancyStartDate"].IsNull()) ? dr["DiscrepancyStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.DiscrepancyEndDate = (!dr["DiscrepancyEndDate"].IsNull()) ? dr["DiscrepancyEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberSCCCode = (!dr["MemberSCCCode"].IsNull()) ? dr["MemberSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberID = (!dr["MemberID"].IsNull()) ? dr["MemberID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberCurrentHICN = (!dr["MemberCurrentHICN"].IsNull()) ? dr["MemberCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.MemberCurrentMBI = (!dr["MemberCurrentMBI"].IsNull()) ? dr["MemberCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.GPSHouseholdID = (!dr["GPSHouseholdID"].IsNull()) ? dr["GPSHouseholdID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberFirstName = (!dr["MemberFirstName"].IsNull()) ? dr["MemberFirstName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberMiddleName = (!dr["MemberMiddleName"].IsNull()) ? dr["MemberMiddleName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLastName = (!dr["MemberLastName"].IsNull()) ? dr["MemberLastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberContractIDLkup = (!dr["MemberContractIDLkup"].IsNull()) ? dr["MemberContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberContractID = (!dr["MemberContractID"].IsNull()) ? dr["MemberContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberPBPLkup = (!dr["MemberPBPLkup"].IsNull()) ? dr["MemberPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberPBP = (!dr["MemberPBP"].IsNull()) ? dr["MemberPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberLOBLkup = (!dr["MemberLOBLkup"].IsNull()) ? dr["MemberLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberLOB = (!dr["MemberLOB"].IsNull()) ? dr["MemberLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedState = (!dr["MemberVerifiedState"].IsNull()) ? dr["MemberVerifiedState"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberVerifiedCountyCode = (!dr["MemberVerifiedCountyCode"].IsNull()) ? dr["MemberVerifiedCountyCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.MemberDOB = (!dr["MemberDOB"].IsNull()) ? dr["MemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.MemberGenderLkup = (!dr["MemberGenderLkup"].IsNull()) ? dr["MemberGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.MemberGender = (!dr["MemberGender"].IsNull()) ? dr["MemberGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSContractIDLkup = (!dr["EligGPSContractIDLkup"].IsNull()) ? dr["EligGPSContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSContractID = (!dr["EligGPSContractID"].IsNull()) ? dr["EligGPSContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSPBPLkup = (!dr["EligGPSPBPLkup"].IsNull()) ? dr["EligGPSPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSPBP = (!dr["EligGPSPBP"].IsNull()) ? dr["EligGPSPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSSCCCode = (!dr["EligGPSSCCCode"].IsNull()) ? dr["EligGPSSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSCurrentHICN = (!dr["EligGPSCurrentHICN"].IsNull()) ? dr["EligGPSCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligGPSCurrentMBI = (!dr["EligGPSCurrentMBI"].IsNull()) ? dr["EligGPSCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSInsuredPlanEffectiveDate = (!dr["EligGPSInsuredPlanEffectiveDate"].IsNull()) ? dr["EligGPSInsuredPlanEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSInsuredPlanTermDate = (!dr["EligGPSInsuredPlanTermDate"].IsNull()) ? dr["EligGPSInsuredPlanTermDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSLOBLkup = (!dr["EligGPSLOBLkup"].IsNull()) ? dr["EligGPSLOBLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSLOB = (!dr["EligGPSLOB"].IsNull()) ? dr["EligGPSLOB"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligGPSMemberDOB = (!dr["EligGPSMemberDOB"].IsNull()) ? dr["EligGPSMemberDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligGPSGenderLkup = (!dr["EligGPSGenderLkup"].IsNull()) ? dr["EligGPSGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligGPSGender = (!dr["EligGPSGender"].IsNull()) ? dr["EligGPSGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRContractIDLkup = (!dr["EligMMRContractIDLkup"].IsNull()) ? dr["EligMMRContractIDLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRContractID = (!dr["EligMMRContractID"].IsNull()) ? dr["EligMMRContractID"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPBPLkup = (!dr["EligMMRPBPLkup"].IsNull()) ? dr["EligMMRPBPLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRPBP = (!dr["EligMMRPBP"].IsNull()) ? dr["EligMMRPBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRSCCCode = (!dr["EligMMRSCCCode"].IsNull()) ? dr["EligMMRSCCCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.DisenrollmentDate = (!dr["DisenrollmentDate"].IsNull()) ? dr["DisenrollmentDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRCurrentHICN = (!dr["EligMMRCurrentHICN"].IsNull()) ? dr["EligMMRCurrentHICN"].NullToString() : string.Empty;
                    //objDOGEN_Queue.EligMMRCurrentMBI = (!dr["EligMMRCurrentMBI"].IsNull()) ? dr["EligMMRCurrentMBI"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentStartDate = (!dr["EligMMRPaymentAdjustmentStartDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentAdjustmentEndDate = (!dr["EligMMRPaymentAdjustmentEndDate"].IsNull()) ? dr["EligMMRPaymentAdjustmentEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRPaymentMonth = (!dr["EligMMRPaymentMonth"].IsNull()) ? dr["EligMMRPaymentMonth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRDOB = (!dr["EligMMRDOB"].IsNull()) ? dr["EligMMRDOB"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.EligMMRGenderLkup = (!dr["EligMMRGenderLkup"].IsNull()) ? dr["EligMMRGenderLkup"].ToInt64() : 0;
                    objDOGEN_Queue.EligMMRGender = (!dr["EligMMRGender"].IsNull()) ? dr["EligMMRGender"].NullToString() : string.Empty;
                    objDOGEN_Queue.EligOOAFlagLkup = (!dr["EligOOAFlagLkup"].IsNull()) ? dr["EligOOAFlagLkup"].ToBoolean() : false;
                    //objDOGEN_Queue.EligOOAFlag = (!dr["EligOOAFlag"].IsNull()) ? dr["EligOOAFlag"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRRequestedEffectiveDate = (!dr["RPRRequestedEffectiveDate"].IsNull()) ? dr["RPRRequestedEffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.RPRActionRequestedLkup = (!dr["RPRActionRequestedLkup"].IsNull()) ? dr["RPRActionRequestedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRActionRequested = (!dr["RPRActionRequested"].IsNull()) ? dr["RPRActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRSupervisorOrRequesterRef = (!dr["RPRSupervisorOrRequesterRef"].IsNull()) ? dr["RPRSupervisorOrRequesterRef"].ToInt64() : 0;
                    objDOGEN_Queue.RPREmployerID = (!dr["RPREmployerID"].IsNull()) ? dr["RPREmployerID"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRReasonforRequest = (!dr["RPRReasonforRequest"].IsNull()) ? dr["RPRReasonforRequest"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRTaskPerformedLkup = (!dr["RPRTaskPerformedLkup"].IsNull()) ? dr["RPRTaskPerformedLkup"].ToInt64() : 0;
                    objDOGEN_Queue.RPRTaskPerformed = (!dr["RPRTaskPerformed"].IsNull()) ? dr["RPRTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPRCTMMember = (!dr["RPRCTMMember"].IsNull()) ? dr["RPRCTMMember"].ToBoolean() : false;
                    objDOGEN_Queue.RPRCTMNumber = (!dr["RPRCTMNumber"].IsNull()) ? dr["RPRCTMNumber"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPREGHPMember = (!dr["RPREGHPMember"].IsNull()) ? dr["RPREGHPMember"].ToBoolean() : false;
                    objDOGEN_Queue.SCCRPRRequested = (!dr["SCCRPRRequested"].IsNull()) ? dr["SCCRPRRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequestedZip = (!dr["SCCRPRRequestedZip"].IsNull()) ? dr["SCCRPRRequestedZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.SCCRPRRequstedSubmissionDate = (!dr["SCCRPRRequstedSubmissionDate"].IsNull()) ? dr["SCCRPRRequstedSubmissionDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveStartDate = (!dr["SCCRPREffectiveStartDate"].IsNull()) ? dr["SCCRPREffectiveStartDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.SCCRPREffectiveEndDate = (!dr["SCCRPREffectiveEndDate"].IsNull()) ? dr["SCCRPREffectiveEndDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCasePended = (!dr["IsCasePended"].IsNull()) ? dr["IsCasePended"].ToBoolean() : false;
                    objDOGEN_Queue.PendedbyRef = (!dr["PendedbyRef"].IsNull()) ? dr["PendedbyRef"].ToInt64() : 0;
                    objDOGEN_Queue.Pendedby = (!dr["Pendedby"].IsNull()) ? dr["Pendedby"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCPendedOn = (!dr["UTCPendedOn"].IsNull()) ? dr["UTCPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTPendedOn = (!dr["CSTPendedOn"].IsNull()) ? dr["CSTPendedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsCaseResolved = (!dr["IsCaseResolved"].IsNull()) ? dr["IsCaseResolved"].ToBoolean() : false;
                    objDOGEN_Queue.ResolvedByRef = (!dr["ResolvedByRef"].IsNull()) ? dr["ResolvedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.ResolvedBy = (!dr["ResolvedBy"].IsNull()) ? dr["ResolvedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCResolvedOn = (!dr["UTCResolvedOn"].IsNull()) ? dr["UTCResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CSTResolvedOn = (!dr["CSTResolvedOn"].IsNull()) ? dr["CSTResolvedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.IsParentCase = (!dr["IsParentCase"].IsNull()) ? dr["IsParentCase"].ToBoolean() : false;
                    objDOGEN_Queue.IsChildCase = (!dr["IsChildCase"].IsNull()) ? dr["IsChildCase"].ToBoolean() : false;
                    objDOGEN_Queue.ParentQueueRef = (!dr["ParentQueueRef"].IsNull()) ? dr["ParentQueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.IsActive = (!dr["IsActive"].IsNull()) ? dr["IsActive"].ToBoolean() : false;
                    objDOGEN_Queue.UTCCreatedOn = (!dr["UTCCreatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                    objDOGEN_Queue.CSTCreatedOn = (!dr["CSTCreatedOn"].IsNull()) ? dr["CSTCreatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.CreatedByRef = (!dr["CreatedByRef"].IsNull()) ? dr["CreatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.CreatedBy = (!dr["CreatedBy"].IsNull()) ? dr["CreatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.UTCLastUpdatedOn = (!dr["UTCLastUpdatedOn"].IsNull()) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCLastUpdatedOn"].ToDateTime(),TimeZone) : (DateTime?)null;
                    objDOGEN_Queue.CSTLastUpdatedOn = (!dr["CSTLastUpdatedOn"].IsNull()) ? dr["CSTLastUpdatedOn"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.LastUpdatedByRef = (!dr["LastUpdatedByRef"].IsNull()) ? dr["LastUpdatedByRef"].ToInt64() : 0;
                    objDOGEN_Queue.LastUpdatedBy = (!dr["LastUpdatedBy"].IsNull()) ? dr["LastUpdatedBy"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherActionRequested = (!dr["RPROtherActionRequested"].IsNull()) ? dr["RPROtherActionRequested"].NullToString() : string.Empty;
                    objDOGEN_Queue.RPROtherTaskPerformed = (!dr["RPROtherTaskPerformed"].IsNull()) ? dr["RPROtherTaskPerformed"].NullToString() : string.Empty;
                    objDOGEN_Queue.PreviousAssignedToRef = (!dr["PreviousAssignedToRef"].IsNull()) ? dr["PreviousAssignedToRef"].ToInt64() : 0;
                    objDOGEN_Queue.PreviousAssignedTo = (!dr["PreviousAssignedTo"].IsNull()) ? dr["PreviousAssignedTo"].NullToString() : string.Empty;
                    objDOGEN_Queue.PDPAutoEnrolleeInd = (!dr["PDPAutoEnrolleeInd"].IsNull()) ? dr["PDPAutoEnrolleeInd"].ToInt64() : 0;
                    objDOGEN_Queue.ReferencedEligibilityCaseInd = (!dr["ReferencedEligibilityCaseInd"].IsNull()) ? dr["ReferencedEligibilityCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.ReferencedSCCCaseInd = (!dr["ReferencedSCCCaseInd"].IsNull()) ? dr["ReferencedSCCCaseInd"].ToBoolean() : false;
                    objDOGEN_Queue.FileTypeLkup = (!dr["FileTypeLkup"].IsNull()) ? dr["FileTypeLkup"].ToInt64() : 0;
                    objDOGEN_Queue.FileType = (!dr["FileType"].IsNull()) ? dr["FileType"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMDocID = (!dr["ODMDocID"].IsNull()) ? dr["ODMDocID"].NullToString() : string.Empty;
                    objDOGEN_Queue.ODMAddressLink = (!dr["ODMAddressLink"].IsNull()) ? dr["ODMAddressLink"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress1 = (!dr["UndeliveredAddress1"].IsNull()) ? dr["UndeliveredAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredAddress2 = (!dr["UndeliveredAddress2"].IsNull()) ? dr["UndeliveredAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredCity = (!dr["UndeliveredCity"].IsNull()) ? dr["UndeliveredCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredState = (!dr["UndeliveredState"].IsNull()) ? dr["UndeliveredState"].NullToString() : string.Empty;
                    objDOGEN_Queue.UndeliveredZip = (!dr["UndeliveredZip"].IsNull()) ? dr["UndeliveredZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress1 = (!dr["COAOldAddress1"].IsNull()) ? dr["COAOldAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldAddress2 = (!dr["COAOldAddress2"].IsNull()) ? dr["COAOldAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldCity = (!dr["COAOldCity"].IsNull()) ? dr["COAOldCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldState = (!dr["COAOldState"].IsNull()) ? dr["COAOldState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COAOldZip = (!dr["COAOldZip"].IsNull()) ? dr["COAOldZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress1 = (!dr["COANewAddress1"].IsNull()) ? dr["COANewAddress1"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewAddress2 = (!dr["COANewAddress2"].IsNull()) ? dr["COANewAddress2"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewCity = (!dr["COANewCity"].IsNull()) ? dr["COANewCity"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewState = (!dr["COANewState"].IsNull()) ? dr["COANewState"].NullToString() : string.Empty;
                    objDOGEN_Queue.COANewZip = (!dr["COANewZip"].IsNull()) ? dr["COANewZip"].NullToString() : string.Empty;
                    objDOGEN_Queue.TurnAroundTime = (dr.Table.Columns.Contains("TurnAroundTime") && !dr["TurnAroundTime"].IsNull()) ? dr["TurnAroundTime"].ToInt64() : 0;
                }
                if (dsResult != null && dsResult.Tables.Count > 1 && dsResult.Tables[1].Rows.Count > 0)
                {
                    DataRow dr = dsResult.Tables[1].Rows[0];
                    objDOGEN_Queue.GEN_QueueRef = (!dr["GEN_QueueRef"].IsNull()) ? dr["GEN_QueueRef"].ToInt64() : 0;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.HICN = (!dr["HICN"].IsNull()) ? dr["HICN"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.LastName = (!dr["LastName"].IsNull()) ? dr["LastName"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.DateofBirth = (!dr["DateofBirth"].IsNull()) ? dr["DateofBirth"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.ApplicationDate = (!dr["ApplicationDate"].IsNull()) ? dr["ApplicationDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.EffectiveDate = (!dr["EffectiveDate"].IsNull()) ? dr["EffectiveDate"].ToDateTime() : (DateTime?)null;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.Resolution = (!dr["Resolution"].IsNull()) ? dr["Resolution"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.OtherResolution = (!dr["OtherResolution"].IsNull()) ? dr["OtherResolution"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.RootCause = (!dr["RootCause"].IsNull()) ? dr["RootCause"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.ElectionType = (!dr["ElectionType"].IsNull()) ? dr["ElectionType"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.TransactionTypeCode = (!dr["TransactionTypeCode"].IsNull()) ? dr["TransactionTypeCode"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.ContractID = (!dr["ContractID"].IsNull()) ? dr["ContractID"].NullToString() : string.Empty;                   
                    objDOGEN_Queue.objDOGEN_EligibilityActions.PBP = (!dr["PBP"].IsNull()) ? dr["PBP"].NullToString() : string.Empty;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.PendReason = (!dr["PendReason"].IsNull()) ? dr["PendReason"].NullToString() : string.Empty;
                    objDOGEN_Queue.EGHPIndicator = (!dr["EGHPIndicator"].IsNull()) ? dr["EGHPIndicator"].ToBoolean() : true;
                    objDOGEN_Queue.objDOGEN_EligibilityActions.OtherRootCause = (!dr["OtherRootCause"].IsNull()) ? dr["OtherRootCause"].NullToString() : string.Empty;
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
                        GEN_QueueRef = !(DBNull.Value.Equals(dr["GEN_QueueRef"])) ? Convert.ToInt64(dr["GEN_QueueRef"]) : 0,
                        Comments = !(DBNull.Value.Equals(dr["Comments"])) ? Convert.ToString(dr["Comments"]) : string.Empty,
                        ActionLkup = !(DBNull.Value.Equals(dr["ActionLkup"])) ? Convert.ToInt64(dr["ActionLkup"]) : 0,
                        Action = !(DBNull.Value.Equals(dr["Action"])) ? Convert.ToString(dr["Action"]) : string.Empty,
                        IsActive = Convert.ToBoolean(dr["IsActive"]),
                        UTCCreatedOn = !(DBNull.Value.Equals(dr["UTCCreatedOn"])) ? ZoneLookupUI.ConvertToTimeZone(dr["UTCCreatedOn"].ToDateTime(),TimeZone) : (DateTime?)null,
                        CreatedByRef = !(DBNull.Value.Equals(dr["CreatedByRef"])) ? Convert.ToInt64(dr["CreatedByRef"]) : 0,
                        CreatedBy = !(DBNull.Value.Equals(dr["CreatedBy"])) ? Convert.ToString(dr["CreatedBy"]) : string.Empty,
                        UTCLastUpdatedOn = !(DBNull.Value.Equals(dr["UTCLastUpdatedOn"])) ? dr["UTCLastUpdatedOn"].ToDateTime() : (DateTime?)null,
                        LastUpdatedByRef = !(DBNull.Value.Equals(dr["LastUpdatedByRef"])) ? Convert.ToInt64(dr["LastUpdatedByRef"]) : 0,
                        LastUpdatedBy = !(DBNull.Value.Equals(dr["LastUpdatedBy"])) ? Convert.ToString(dr["LastUpdatedBy"]) : string.Empty,
                        SourceSystemLkup = (!dr["SourceSystemLkup"].IsNull()) ? dr["SourceSystemLkup"].ToInt64() : 0,
                        SourceSystem = (!dr["SourceSystem"].IsNull()) ? dr["SourceSystem"].NullToString() : string.Empty,
                    }).ToList();
                }
                if (dsResult != null && dsResult.Tables.Count > 4 && dsResult.Tables[4].Rows.Count > 0)
                {
                    objDOGEN_Queue.lstDOGEN_Attachments = dsResult.Tables[4].AsEnumerable().Select(dr => new DOGEN_Attachments
                    {
                        slno = dr["SLNO"].ToInt64(),
                        GEN_AttachmentsId = dr["GEN_AttachmentsId"].ToInt64(),
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
            catch (Exception)
            {

                throw;
            }
        }

        public ExceptionTypes SaveAction(DOGEN_EligibilityActions objDOGEN_EligibilityActions, out string errorMessage)
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
                sqlParam.ParameterName = "@GEN_QueueRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.GEN_QueueRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ActionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ActionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReOpenoptionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.OptionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ReOpenQueueLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ReopenQueueLKUP;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@HICN";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.HICN;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LastName";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.LastName;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@DateofBirth";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.DateofBirth;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContractIDLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ContractIDLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PBPLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.PBPLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@TransactionTypeCodeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.TransactionTypeCodeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ApplicationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.ApplicationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ElectionTypeLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ElectionTypeLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.EffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ResolutionLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ResolutionLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherResolution";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.OtherResolution;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RootCauseLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RootCauseLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@EGHPIndicator";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.EGHPIndicator;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@PendReasonLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.PendReasonLkup;
                parameters.Add(sqlParam);


                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ContainsErrorsLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.ContainsErrorsLkup;
                parameters.Add(sqlParam);
                

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@FirstLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.FirstLetterMailDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@SecondLetterMailDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.SecondLetterMailDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Reason";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.Reason;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@InitialAddressVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.InitialAddressVerificationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberResponseVerificationDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.MemberResponseVerificationDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@MemberVerifiedStateLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.MemberVerifiedState;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@IsActive";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.IsActive;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@Comments";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.Comments;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@LoginUserId";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.LoginID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RoleLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RoleLKup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRRequestedEffectiveDate";
                sqlParam.SqlDbType = SqlDbType.DateTime;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRRequestedEffectiveDate;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRActionRequestedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRActionRequestedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherActionRequested";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPROtherActionRequested;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRSupervisorOrRequesterRef";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRSupervisorOrRequesterRef;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRReasonforRequest";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRReasonforRequest;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRTaskPerformedLkup";
                sqlParam.SqlDbType = SqlDbType.BigInt;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRTaskPerformedLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPROtherTaskPerformed";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPROtherTaskPerformed;
                parameters.Add(sqlParam);
             
                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRCTMMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPRCTMNumber";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPRCTMNumber;
                parameters.Add(sqlParam);               

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREGHPMember";
                sqlParam.SqlDbType = SqlDbType.Bit;
                sqlParam.Value = objDOGEN_EligibilityActions.RPREGHPMember;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@RPREmployerID";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.RPREmployerID;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CommentsSourceSystemLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_EligibilityActions.CommentsSourceSystemLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@CMSTransactionStatusLkup";
                sqlParam.DbType = DbType.Int64;
                sqlParam.Value = objDOGEN_EligibilityActions.CMSTransactionStatusLkup;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@OtherRootCause";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = objDOGEN_EligibilityActions.OtherRootCause;
                parameters.Add(sqlParam);

                sqlParam = new SqlParameter();
                sqlParam.ParameterName = "@ErrorMessage";
                sqlParam.SqlDbType = SqlDbType.VarChar;
                sqlParam.Value = string.Empty;
                sqlParam.Direction = ParameterDirection.Output;
                sqlParam.Size = 2000;
                parameters.Add(sqlParam);

                long executionResult = 0;
                //if (objDOADM_UserMaster.ADM_UserMasterId == 0)
                //{
                executionResult = executionResult = dah.ExecuteDMLSP(ConstantTexts.SP_USP_APP_UPD_EligibilityActions, parameters.ToArray(), out lErrocode, out lErrorNumber, out lRowsEffected, out errorMessage);
                sqlParam = parameters.FirstOrDefault(x => x.ParameterName == "@ErrorMessage");

                if (sqlParam != null && sqlParam.Value != null)
                {
                    errorMessage += sqlParam.Value.ToString();
                }

                if (executionResult == (long)ExceptionTypes.Success && string.IsNullOrEmpty(errorMessage))
                {
                    //MapGenQueue(_dsResult, out objDOGEN_Queue);
                    return ExceptionTypes.Success;
                }
                else
                    return ExceptionTypes.UnknownError;

                //if (executionResult == 0)
                //{
                //    return ExceptionTypes.Success;
                //}
                //else
                //{
                //    return ExceptionTypes.UnknownError;
                //}
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return ExceptionTypes.UnknownError;
            }
        }       

    }
}
