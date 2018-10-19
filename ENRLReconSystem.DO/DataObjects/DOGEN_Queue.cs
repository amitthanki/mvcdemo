using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_Queue
    {
        //Constructor
        public DOGEN_Queue()
        {
            lstDOGEN_QueueWorkFlowCorrelation = new List<DOGEN_QueueWorkFlowCorrelation>();
            lstDOGEN_Comments = new List<DOGEN_Comments>();
            lstDOGEN_QueueRefferencedCases = new List<DOGEN_QueueRefferencedCases>();
            lstDOGEN_Attachments = new List<DOGEN_Attachments>();
            objDOGEN_OSTActions = new DOGEN_OSTActions();
            objDOGEN_EligibilityActions = new DOGEN_EligibilityActions();
            objDOGEN_RPRActions = new DOGEN_RPRActions();
            objDOGEN_GPSData = new DOGEN_GPSData();
            objDOGEN_TRRData = new DOGEN_TRRData();
            lstTransactionReplyCode = new List<DOCMN_LookupMaster>();
            lstTrrSummaryTransaction = new List<DOGEN_TRRData>();
            lstBadHistoryTransaction = new List<DOGEN_TRRData>();
            lstBadPendingTransaction = new List<DOGEN_TRRData>();
          
        }


        #region public properties
        public long? GEN_QueueId { get; set; }
        public long BusinessSegmentLkup { get; set; }
        public long WorkBasketLkup { get; set; }
        public long DiscrepancyCategoryLkup { get; set; }
        public long DiscrepancyTypeLkup { get; set; }
        public DateTime? StartDate { get; set; }
        public long? StartDateId { get; set; }
        public DateTime? EndDate { get; set; }
        public long? EndDateId { get; set; }
        public long? AssignedToRef { get; set; }
        public DateTime? UTCAssignedOn { get; set; }
        public long? UTCAssignedOnId { get; set; }
        public int? UTCAssignedOnYear { get; set; }
        public int? UTCAssignedOnMonth { get; set; }
        public int? UTCAssignedOnDay { get; set; }
        public DateTime? CSTAssignedOn { get; set; }
        public long? CSTAssignedOnId { get; set; }
        public int? CSTAssignedOnYear { get; set; }
        public int? CSTAssignedOnMonth { get; set; }
        public int? CSTAssignedOnDay { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public long? UTCLockedOnId { get; set; }
        public int? UTCLockedOnYear { get; set; }
        public int? UTCLockedOnMonth { get; set; }
        public int? UTCLockedOnDay { get; set; }
        public DateTime? CSTLockedOn { get; set; }
        public long? CSTLockedOnId { get; set; }
        public int? CSTLockedOnYear { get; set; }
        public int? CSTLockedOnMonth { get; set; }
        public int? CSTLockedOnDay { get; set; }
        public long? MostRecentActionLkup { get; set; }
        public long? MostRecentWorkQueueLkup { get; set; }
        public long? MostRecentStatusLkup { get; set; }
        public long? SourceSystemLkup { get; set; }
        public string SourceSystemId { get; set; }
        public long? DiscrepancySourceLkup { get; set; }
        public DateTime? DiscrepancyReceiptDate { get; set; }
        public long? DiscrepancyReceiptDateId { get; set; }
        public DateTime? ComplianceStartDate { get; set; }
        public long? ComplianceStartDateId { get; set; }
        public long? Aging { get; set; }
        public DateTime? DiscrepancyStartDate { get; set; }
        public long? DiscrepancyStartDateId { get; set; }
        public DateTime? DiscrepancyEndDate { get; set; }
        public long? DiscrepancyEndDateId { get; set; }
        public string MemberSCCCode { get; set; }
        public DateTime? DisenrollmentDate { get; set; }
        public string MemberID { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string GPSHouseholdID { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public long? MemberContractIDLkup { get; set; }
        public long? MemberPBPLkup { get; set; }
        public long? MemberLOBLkup { get; set; }
        public DateTime? MemberDOB { get; set; }
        public long? MemberDOBId { get; set; }
        public long? MemberGenderLkup { get; set; }
        public string MemberVerifiedState { get; set; }
        public string MemberVerifiedCountyCode { get; set; }

        public string EmployeerGroupNumber { get; set; }

        public long? EligGPSContractIDLkup { get; set; }
        public long? EligGPSPBPLkup { get; set; }
        public string EligGPSSCCCode { get; set; }
        public string EligGPSCurrentHICN { get; set; }
        public DateTime? EligGPSInsuredPlanEffectiveDate { get; set; }
        public long? EligGPSInsuredPlanEffectiveDateId { get; set; }
        public DateTime? EligGPSInsuredPlanTermDate { get; set; }
        public long? EligGPSInsuredPlanTermDateId { get; set; }
        public long? EligGPSLOBLkup { get; set; }
        public DateTime? EligGPSMemberDOB { get; set; }
        public long? EligGPSMemberDOBId { get; set; }
        public long? EligGPSGenderLkup { get; set; }
        public long? EligMMRContractIDLkup { get; set; }
        public long? EligMMRPBPLkup { get; set; }
        public string EligMMRSCCCode { get; set; }
        public string EligMMRCurrentHICN { get; set; }
        public DateTime? EligMMRPaymentAdjustmentStartDate { get; set; }
        public long? EligMMRPaymentAdjustmentStartDateId { get; set; }
        public DateTime? EligMMRPaymentAdjustmentEndDate { get; set; }
        public long? EligMMRPaymentAdjustmentEndDateId { get; set; }
        public DateTime? EligMMRPaymentMonth { get; set; }
        public DateTime? EligMMRDOB { get; set; }
        public long? EligMMRDOBId { get; set; }
        public long? EligMMRGenderLkup { get; set; }
        public bool? OOAFlagLkup { get; set; }
        public long? OOAFlagLkupValue { get; set; }
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
        public bool IsCasePended { get; set; }
        public long? PendedbyRef { get; set; }
        public DateTime? UTCPendedOn { get; set; }
        public long? UTCPendedOnId { get; set; }
        public int? UTCPendedOnYear { get; set; }
        public int? UTCPendedOnMonth { get; set; }
        public int? UTCPendedOnDay { get; set; }
        public DateTime? CSTPendedOn { get; set; }
        public long? CSTPendedOnId { get; set; }
        public int? CSTPendedOnYear { get; set; }
        public int? CSTPendedOnMonth { get; set; }
        public int? CSTPendedOnDay { get; set; }
        public bool IsCaseResolved { get; set; }
        public long? ResolvedByRef { get; set; }
        public DateTime? UTCResolvedOn { get; set; }
        public long? UTCResolvedOnId { get; set; }
        public int? UTCResolvedOnYear { get; set; }
        public int? UTCResolvedOnMonth { get; set; }
        public int? UTCResolvedOnDay { get; set; }
        public DateTime? CSTResolvedOn { get; set; }
        public long? CSTResolvedOnId { get; set; }
        public int? CSTResolvedOnYear { get; set; }
        public int? CSTResolvedOnMonth { get; set; }
        public int? CSTResolvedOnDay { get; set; }
        public bool IsParentCase { get; set; }
        public bool IsChildCase { get; set; }
        public long? ParentQueueRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long UTCCreatedOnId { get; set; }
        public int UTCCreatedOnYear { get; set; }
        public int UTCCreatedOnMonth { get; set; }
        public int UTCCreatedOnDay { get; set; }
        public DateTime? CSTCreatedOn { get; set; }
        public long CSTCreatedOnId { get; set; }
        public int CSTCreatedOnYear { get; set; }
        public int CSTCreatedOnMonth { get; set; }
        public int CSTCreatedOnDay { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? UTCLastUpdatedOnId { get; set; }
        public int? UTCLastUpdatedOnYear { get; set; }
        public int? UTCLastUpdatedOnMonth { get; set; }
        public int? UTCLastUpdatedOnDay { get; set; }
        public DateTime? CSTLastUpdatedOn { get; set; }
        public long? CSTLastUpdatedOnId { get; set; }
        public int? CSTLastUpdatedOnYear { get; set; }
        public int? CSTLastUpdatedOnMonth { get; set; }
        public int? CSTLastUpdatedOnDay { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string Comments { get; set; }
        public long? OOALetterStatusLkup { get; set; }
        public bool IsRestricted { get; set; }

        // Added Properties According to SP.
        public long CommentsSourceSystemLkup { get; set; }
        public long? ActionLkup { get; set; }

        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }
        public long? ContractIDLkup { get; set; }
        public long? PBPLkup { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? FirstLetterMailDate { get; set; }
        public DateTime? SecondLetterMailDate { get; set; }
        public bool ResidentialDocumentationRequired { get; set; }
        public bool CountyAttestationRequired { get; set; }
        public long? PendReasonLkup { get; set; }
        public long? ContainsErrorsLkup { get; set; }
        public long? ResolutionLkup { get; set; }
        public DateTime? InitialAddressVerificationDate { get; set; }

        public DateTime? MemberResponseVerificationDate { get; set; }
        public DateTime? SCCLetterMailDate { get; set; }
        public string HICN { get; set; }
        public long? TransactionTypeCodeLkup { get; set; }

        public long? ElectionTypeLkup { get; set; }
        public long? RootCauseLkup { get; set; }
        public bool EGHPIndicator { get; set; }
        public DateTime? AdjustedCreateDate { get; set; }
        public long? AdjustedCreateDateReasonLkup { get; set; }
        public long? SubmissionTypeLkup { get; set; }
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
        public DateTime? ActualSubmissionDate { get; set; }
        public string ReasonSubmissionRejected { get; set; }
        public string RequestedSCC { get; set; }
        public string RequestedZIP { get; set; }
        public DateTime? ResubmissionDate { get; set; }
        public long? GEN_AttachmentsId { get; set; }
        public long? GEN_QueueRef { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long? GEN_DMSDataRef { get; set; }
        public long? ActionPerformedLkup { get; set; }
        public long? CurrentUserRef { get; set; }
        public string CasesComments { get; set; }
        public long? ReAssignUserRef { get; set; }
        public long? RoleLkup { get; set; }
        public long? PreviousActionLkup { get; set; }
        public long? PreviousWorkQueueLkup { get; set; }

        public long? PreviousStatusLkup { get; set; }
        public long? CurrentActionLkup { get; set; }
        public long? CurrentWorkQueuesLkup { get; set; }
        public long? CurrentStatusLkup { get; set; }
        public long? GEN_EligibilityActionsId { get; set; }
        public long? GEN_OSTActionsId { get; set; }
        public long? GEN_RPRActionsId { get; set; }
        public long? LoginUserId { get; set; }
        public long? OptionLkup { get; set; }
        public string MIIMReferenceId { get; set; }
        public long? RPRCategoryLkup { get; set; }
        public string BusinessSegment { get; set; }
        public string QueueProcessType { get; set; }

        public long? TurnAroundTime { get; set; }

        public bool IsClosedAndCreateNew { get; set; }

        public bool IsCaseUpdate { get; set; }


        //lookups
        public List<DOCMN_LookupMaster> lstContractid { get; set; }
      
        public List<DOCMN_LookupMaster> lstPbpid { get; set; }
        public List<DOCMN_LookupMaster> lstLob { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscCategary { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscType { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstActions { get; set; }
        public List<DOCMN_LookupMaster> lstSourceSystem { get; set; }
        public List<DOCMN_LookupMaster> lstDiscSourceSystem { get; set; }
        public List<DOCMN_LookupMaster> lstActionRequested { get; set; }
        public List<DOADM_UserMaster> lstUsers { get; set; }
        public List<DOCMN_LookupMaster> lstTaskBeingPerformed { get; set; }
        public List<DOCMN_LookupMaster> lstGender { get; set; }

        public List<DOCMN_LookupMaster> lstOOAFlag { get; set; }

        public List<DOCMN_LookupMaster> lstOptionsforReopen { get; set; }

        public List<DOCMN_LookupMaster> lstMostRecentStatus { get; set; }
        //attachment for create case
        //public HttpPostedFileBase Attachment { get; set; }


        public DOGEN_OSTActions objDOGEN_OSTActions { get; set; }

        public DOGEN_EligibilityActions objDOGEN_EligibilityActions { get; set; }
        public DOGEN_RPRActions objDOGEN_RPRActions { get; set; }
        public List<DOCMN_LookupMaster> lstMemberVerifiedState { get; set; }


        

        public DOGEN_GPSData objDOGEN_GPSData { get; set; }

        public DOGEN_TRRData objDOGEN_TRRData { get; set; }

        #endregion
        #region ListOf WorkflowLog
        public List<DOGEN_QueueWorkFlowCorrelation> lstDOGEN_QueueWorkFlowCorrelation { get; set; }

        #endregion
        #region ListOf PWComments
        public List<DOGEN_Comments> lstDOGEN_Comments { get; set; }

        #endregion
        #region ListOf Refferenced Resources
        public List<DOGEN_QueueRefferencedCases> lstDOGEN_QueueRefferencedCases { get; set; }

        #endregion
        #region Aditional Properties
        public string WorkBasket { get; set; }
        public string DiscrepancyCategory { get; set; }
        public string DiscrepancyType { get; set; }
        public string AssignedTo { get; set; }
        public string LockedBy { get; set; }
        public string MostRecentAction { get; set; }
        public string MostRecentWorkQueue { get; set; }
        public string MostRecentStatus { get; set; }
        public string SourceSystem { get; set; }
        public string DiscrepancySource { get; set; }
        public string MemberContractID { get; set; }
        public string MemberPBP { get; set; }
        public string MemberLOB { get; set; }
        public string MemberGender { get; set; }
        public string EligGPSContractID { get; set; }
        public string EligGPSPBP { get; set; }
        public string EligGPSLOB { get; set; }
        public string EligGPSGender { get; set; }
        public string EligMMRContractID { get; set; }
        public string EligMMRPBP { get; set; }
        public string EligMMRGender { get; set; }
        public string OOAFlag { get; set; }
        public string RPRActionRequested { get; set; }
        public string RPRTaskPerformed { get; set; }
        public string Pendedby { get; set; }
        public string ResolvedBy { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string MemberCurrentMBI { get; set; }
        public string EligGPSCurrentMBI { get; set; }
        public string EligMMRCurrentMBI { get; set; }
        public bool EligOOAFlagLkup { get; set; }
        public string EligOOAFlag { get; set; }
        public long PreviousAssignedToRef { get; set; }
        public string PreviousAssignedTo { get; set; }
        public long? PDPAutoEnrolleeInd { get; set; }
        public bool ReferencedEligibilityCaseInd { get; set; }
        public bool ReferencedSCCCaseInd { get; set; }
        public long FileTypeLkup { get; set; }
        public string FileType { get; set; }
        public string ODMDocID { get; set; }
        public string ODMAddressLink { get; set; }
        public string UndeliveredAddress1 { get; set; }
        public string UndeliveredAddress2 { get; set; }
        public string UndeliveredCity { get; set; }
        public string UndeliveredZip { get; set; }
        public string UndeliveredState { get; set; }
        public string COAOldAddress1 { get; set; }
        public string COAOldAddress2 { get; set; }
        public string COAOldCity { get; set; }
        public string COAOldState { get; set; }
        public string COAOldZip { get; set; }
        public string COANewAddress1 { get; set; }
        public string COANewAddress2 { get; set; }
        public string COANewCity { get; set; }
        public string COANewState { get; set; }
        public string COANewZip { get; set; }
        public string PendReason { get; set; }
        public string RPRSupervisorOrRequester { get; set; }
        public string RPRCategory { get; set; }

        //USPS Address Validation
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        #endregion


        #region Attachment
        public List<DOGEN_Attachments> lstDOGEN_Attachments { get; set; }
        #endregion
        #region List Of TRC
        public List<DOCMN_LookupMaster> lstTransactionReplyCode { get; set; }
        public string TransactionReplyCode { get; set; }
        public long? TransactionReplyCodeLkup { get; set; }
        public DateTime? TimelineEffectiveDate { get; set; }
        #endregion

        public List<DOCMN_LookupMaster> lstPDPAutoEnrolleeInd { get; set; }
        public List<DOGEN_TRRData> lstTrrSummaryTransaction { get; set; }
        public List<DOGEN_TRRData> lstBadHistoryTransaction { get; set; }
        public List<DOGEN_TRRData> lstBadPendingTransaction { get; set; }

    }

    [Serializable]
    public class DOGEN_QueueRefferencedCases
    {
        public DOGEN_QueueRefferencedCases()
        {
        }
        public long Gen_QueueId { get; set; }
        public string BusinessSegment { get; set; }
        public string WorkBasket { get; set; }
        public string DiscrepancyType { get; set; }
        public DateTime? DiscrepancyStartDate { get; set; }
        public long DiscrepancyCategoryLkup { get; set; }
        public string DiscrepancyCategory { get; set; }
        public string MemberContract { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string MemberPBP { get; set; }
        public long? ParentQueueRef { get; set; }
        public string MostRecentWorkQueue { get; set; }
        public string MostRecentStatus { get; set; }
        public DateTime? FirstLetterMailDate { get; set; }
        public DateTime? SecondLetterMailDate { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public string DiscrepancySource { get; set; }
        public string QueueProgressType { get; set; }
        public long MostRecentStatusLkup { get; set; }
    }
}
