using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_EligibilityActions
    {


        //Constructor
        public DOGEN_EligibilityActions()
        {

        }


        #region public properties
        public long GEN_EligibilityActionsId { get; set; }

        public long? DiscrepancyCatLkup { get; set; }

        public long? DiscrepancyTypeLkup { get; set; }
        public long? GEN_QueueRef { get; set; }
        public long? ActionLkup { get; set; }
        public string HICN { get; set; }

        public string Comments { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public long? ContractIDLkup { get; set; }
        public long? PBPLkup { get; set; }
        public long? TransactionTypeCodeLkup { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public long? ElectionTypeLkup { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public long? ResolutionLkup { get; set; }
        public long? RootCauseLkup { get; set; }
        public bool EGHPIndicator { get; set; }
        public long? PendReasonLkup { get; set; }
        public long? ContainsErrorsLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public long? CaseNumber { get; set; }

        public long? LoginID { get; set; }

        public long? RoleLKup { get; set; }

        public string OtherResolution { get; set; }

        public String MBI { get; set; }

        public string Resolution { get; set; }

        public string RootCause { get; set; }

        public string Reason { get; set; }

        public String TransactionTypeCode { get; set; }

        public string ElectionType { get; set; }

        public string ContractID { get; set; }

        public string PBP { get; set; }

        public string MemberVerifiedState { get; set; }

        public DateTime? FirstLetterMailDateNoResponceTerm { get; set; }

        public DateTime? InitialAddressVerificationDate;

        public DateTime? FirstLetterMailDate;

        public DateTime? SecondLetterMailDate;

        public DateTime? MemberResponseVerificationDate;

        public string PendReason { get; set; }

        public DOGEN_Queue objDOGEN_Queue;

        public DateTime? RPRRequestedEffectiveDate { get; set; }
        public long? RPRRequestedEffectiveDateId { get; set; }
        public long? RPRActionRequestedLkup { get; set; }
        public string RPROtherActionRequested { get; set; }
        public long? RPRSupervisorOrRequesterRef { get; set; }
        public string RPRReasonforRequest { get; set; }
        public long? RPRTaskPerformedLkup { get; set; }
        public string RPROtherTaskPerformed { get; set; }
        public bool RPRCTMMember { get; set; }
        public string RPRCTMNumber { get; set; }
        public bool RPREGHPMember { get; set; }
        public string RPREmployerID { get; set; }

        public string SCCRPRRequested { get; set; }
        public string SCCRPRRequestedZip { get; set; }
        public DateTime? SCCRPRRequstedSubmissionDate { get; set; }
        public long? SCCRPRRequstedSubmissionDateId { get; set; }
        public DateTime? SCCRPREffectiveStartDate { get; set; }
        public long? SCCRPREffectiveStartDateId { get; set; }
        public DateTime? SCCRPREffectiveEndDate { get; set; }
        public long? SCCRPREffectiveEndDateId { get; set; }
        public long CommentsSourceSystemLkup { get; set; }

        public long? CMSTransactionStatusLkup { get; set; }

        public string OtherRootCause { get; set; }

        // Added Properties for Lookups

        public List<DOCMN_LookupMaster> lstContractid { get; set; }
        public List<DOCMN_LookupMaster> lstPbpid { get; set; }
        public List<DOCMN_LookupMaster> lstLob { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscCategary { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscType { get; set; }
        public List<DOCMN_LookupMaster> lstTransactionTypeCode { get; set; }
        public List<DOCMN_LookupMaster> lstContainsError { get; set; }
        public List<DOCMN_LookupMaster> lstElectionType { get; set; }
        public List<DOADM_UserMaster> lstUsers { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstPendReason { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstResolution { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstRootCause { get; set; }

        public List<DOCMN_LookupMaster> lstActionRequested { get; set; }      
        public List<DOCMN_LookupMaster> lstTaskBeingPerformed { get; set; }

        public List<DOCMN_LookupMaster> lstState { get; set; }
        public string Gen_QueueIds { get; set; }

        public long OptionLkup { get; set; }

        public List<DOCMN_LookupMasterCorrelations> lstQueue { get; set; }

        public long? ReopenQueueLKUP { get; set; }
        public long? GEN_BulkImportRef { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public long? QueueLkup { get; set; }
        public long? BusinessSegmentLkup { get; set; }

        #endregion

    }
}
