using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_RPRActions
    {


        //Constructor
        public DOGEN_RPRActions()
        {
           // lstPbpid = new List<DOCMN_LookupMasterCorrelations>();
        }


        #region public properties
        public long? GEN_RPRActionsId { get; set; }
        public long? GEN_QueueRef { get; set; }
        public long? ActionLkup { get; set; }
        public long? ResolutionLkup { get; set; }
        public long? RootCauseLkup { get; set; }
        public long? PendReasonLkup { get; set; }
        public DateTime? AdjustedCreateDate { get; set; }
        public long? AdjustedCreateDateReasonLkup { get; set; }
        public long? SubmissionTypeLkup { get; set; }
        public long? ContainsErrorsLkup { get; set; }
        public DateTime? RPCSubmissionDate { get; set; }
        public DateTime? CMSAccountManagerSubmissionDate { get; set; }
        public DateTime? CMSAccountManagerApprovalDate { get; set; }
        public long? FDRStatusLkup { get; set; }
        public DateTime? FDRReceivedDate { get; set; }
        public string FDRCodeReceived { get; set; }
        public string FDRDescription { get; set; }
        public DateTime? CMSProcessDate { get; set; }
        public string TransactionType { get; set; }
        public long? FDRRejectionTypeLkup { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public long? ContractIDLkup { get; set; }
        public long? PBPLkup { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ActualSubmissionDate { get; set; }
        public string ReasonSubmissionRejected { get; set; }
        public string RequestedSCC { get; set; }
        public string RequestedZIP { get; set; }
        public string TransactionTypeCode { get; set; }
        public string VerifiedRootCause { get; set; }
        public long? VerifiedRootCauseLkup { get; set; }
        public string ExplanationOfRootCause { get; set; }
        public long? ExplanationOfRootCauseLkup { get; set; }
        public long? TransactionTypeCodeLkup { get; set; }

        public DateTime? ResubmissionDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public DateTime? FDRPackageDate { get; set; }
        #endregion
        public long? FDRSubmissionCategoryLkup { get; set; }

        public int? PlanError { get; set; }

 
        //addded properties
        public string Comments { get; set; }
        public long? RPRActionRequestedLkup { get; set; }
        public DateTime? RPRRequestedEffectiveDate { get; set; }
        public long? RoleLkup { get; set; }
        public long? LoginUserId { get; set; }
        public string Action { get; set; }
        public string Resolution { get; set; }
        public string RootCause { get; set; }
        public string PendReason { get; set; }
        public string AdjustedCreateDateReason { get; set; }
        public string SubmissionType { get; set; }
        public string ContainsErrors { get; set; }
        public string FDRStatus { get; set; }
        public string FDRRejectionType { get; set; }
        public string ContractID { get; set; }
        public string PBP { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public long CommentsSourceSystemLkup { get; set; }

        public long OptionLkup { get; set; }

        public long? ReopenQueueLKUP { get; set; }

        public long? CMSTransactionStatusLkup { get; set; }

        //lookups
        public List<DOCMN_LookupMasterCorrelations> lstPendReasons { get; set; }
        public List<DOCMN_LookupMaster> lstAdjustedCreateDateReason { get; set; }
        public List<DOCMN_LookupMaster> lstSubmissionType { get; set; }
        public List<DOCMN_LookupMaster> lstExplanationOfRootCause { get; set; }
        public List<DOCMN_LookupMaster> lstVerifiedRootCause { get; set; }
        public List<DOCMN_LookupMaster> lstTransactionTypeCode { get; set; }
        public List<DOCMN_LookupMaster> lstActionRequested { get; set; }
        public List<DOCMN_LookupMaster> lstContainsErros { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstRootCause { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstResolution { get; set; }
        public List<DOCMN_LookupMaster> lstFDRStatus { get; set; }
        public List<DOCMN_LookupMaster> lstFDRRejectionType { get; set; }
        public List<DOCMN_LookupMaster> lstContractid { get; set; }
        public List<DOCMN_LookupMaster> lstPbpid { get; set; }
        public List<DOCMN_LookupMaster> lstElectionType { get; set; }

        public List<DOCMN_LookupMaster> lstPlanError { get; set; }

        public List<DOCMN_LookupMasterCorrelations> lstQueue { get; set; }
        public string OtherFDRStatus { get; set; }
        public string OtherAdjustedCreateDateReason { get; set; }
        public string ElectionType { get; set; }
        public long? ElectionTypeLkup { get; set; }
        public DateTime? PotentialSubmissionDate { get; set; }
        public bool IncludeInTodaysSubmission { get; set; }
        public string Gen_QueueIds { get; set; }
        public long? GEN_BulkImportRef { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public long? QueueLkup { get; set; }
        public long? BusinessSegmentLkup { get; set; }
    }
}
