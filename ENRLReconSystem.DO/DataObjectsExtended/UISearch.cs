using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class SearchCriteria
    {
        //Long fields
        public long? WorkItemId { get; set; }
        public long? BusinessSegmentLkup { get; set; }
        public long? WorkBasketLkup { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public long? DiscrepancyTypeLkup { get; set; }
        public long? DiscrepancySourceLkup { get; set; }
        public long? LOBLkup { get; set; }
        public long? LastUpdatedOperator { get; set; }
        public long? AssignedTo { get; set; }
        public long? RPRActionRequested { get; set; }
        public long? SupervisiorofthePersonEnteringtheRequest { get; set; }
        public long? RPRCTMMember { get; set; }
        public long? RPREGHPMember { get; set; }
        public long? FDRStatus { get; set; }
        public long? VerifiedRootCause { get; set; }
        public long? TaskBeingPerformedWhenThisDiscrepancyWasIdentified { get; set; }
        public long? SubmissionTypeLkup { get; set; }
        public long? PendReason { get; set; }
        public long? Resolution { get; set; }
        public long? RPRRequestor { get; set; }
        public long? Queue { get; set; }
        public long? Status { get; set; }
        public long? GenderLkup { get; set; }
        public long? CaseAgeFrom { get; set; }
        public long? CaseAgeTo { get; set; }
        public long? ContractIDLkup { get; set; }
        public long? PBPLkup { get; set; }

        //String fileds
        public string CurrentHICN { get; set; }
        public string RPRCTMNumber { get; set; }
        public string MemberSCCCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FDRCodeReceived { get; set; }
        public string RPREmployerID { get; set; }

        public long? TransactionReplyCode { get; set; }

        public string EmployerGroupNumber { get; set; }

        public string MemberVerifiedState { get; set; }
        public string MemberVerifiedCountyCode { get; set; }

        public long? PDPAutoEnrolleeInd { get; set; }

        //Date fields
        public DateTime? DiscrepancyStartDate { get; set; }
        public DateTime? DiscrepancyEndDate { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime? FirstLetterMailStartDate { get; set; }
        public DateTime? FirstLetterMailEndDate { get; set; }
        public DateTime? SecondLetterMailStartDate { get; set; }
        public DateTime? SecondLetterMailEndDate { get; set; }

        public DateTime? DisenrollmentFromDate { get; set; }

        public DateTime? DisenrollmentToDate { get; set; }

        public DateTime? ComplianceStartDate { get; set; }
        public DateTime? ComplianceEndDate { get; set; }
        public DateTime? CaseCreationStartDate { get; set; }
        public DateTime? CaseCreationEndDate { get; set; }
        public DateTime? LastUpdatedStartDate { get; set; }
        public DateTime? LastUpdatedEndDate { get; set; }
        public DateTime? MemberResponseVerificationStartDate { get; set; }
        public DateTime? MemberResponseVerificationEndDate { get; set; }
        public DateTime? RequestedEffectiveStartDate { get; set; }
        public DateTime? RequestedEffectiveEndDate { get; set; }
        public DateTime? AdjustedCreateStartDate { get; set; }
        public DateTime? AdjustedCreateEndDate { get; set; }
        public DateTime? RPCSubmissionStartDate { get; set; }
        public DateTime? RPCSubmissionEndDate { get; set; }
        public DateTime? CMSAccountManagerApprovalStartDate { get; set; }
        public DateTime? CMSAccountManagerApprovalEndDate { get; set; }
        public DateTime? FDRReceivedStartDate { get; set; }
        public DateTime? FDRReceivedEndDate { get; set; }
        public DateTime? PeerAuditCompletionStartDate { get; set; }
        public DateTime? PeerAuditCompletionEndDate { get; set; }



        //DateId fileds used for where condition id Database
        public long LastUpdatedStartDateId { get; set; }
        public long LastUpdatedEndDateId { get; set; }
        public long? DiscrepancyStartDateId { get; set; }
        public long? DiscrepancyEndDateId { get; set; }
        public long? DOBId { get; set; }
        public long? FirstLetterMailStartDateId { get; set; }
        public long? FirstLetterMailEndDateId { get; set; }
        public long? SecondLetterMailStartDateId { get; set; }

        public long? DisenrollmentFromDateId { get; set; }

        public long? DisenrollmentToDateId { get; set; }

        public long? SecondLetterMailEndDateId { get; set; }
        public long? ComplianceStartDateId { get; set; }
        public long? ComplianceEndDateId { get; set; }
        public long? CaseCreationStartDateId { get; set; }
        public long? CaseCreationEndDateId { get; set; }
        public long? MemberResponseVerificationStartDateId { get; set; }
        public long? MemberResponseVerificationEndDateId { get; set; }
        public long? RequestedEffectiveStartDateId { get; set; }
        public long? RequestedEffectiveEndDateId { get; set; }
        public long? PotentionSubmissionStartDateId { get; set; }
        public long? PotentionSubmissionEndDateId { get; set; }
        public long? AdjustedCreateStartDateId { get; set; }
        public long? AdjustedCreateEndDateId { get; set; }
        public long? RPCSubmissionStartDateId { get; set; }
        public long? RPCSubmissionEndDateId { get; set; }
        public long? CMSAccountManagerApprovalStartDateId { get; set; }
        public long? CMSAccountManagerApprovalEndDateId { get; set; }
        public long? FDRReceivedStartDateId { get; set; }
        public long? FDRReceivedEndDateId { get; set; }
        public long? PeerAuditCompletionStartDateId { get; set; }
        public long? PeerAuditCompletionEndDateId { get; set; }

        public string GPSHouseholdID { get; set; }
        public bool IsRestricted { get; set; }

        public long? StatusNot { get; set; }
        public long? DiscrepancyTypeLkupNot { get; set; }
        public long? MemberPBPLkup { get; set; }
        public bool IsUnlock { get; set; }
        public bool IsReAssign { get; set; }
        public bool IsMassUpdate { get; set; }

        public bool IsSearchScreen { get; set; }



        public SearchCriteria()
        {
            IsUnlock = false;
            IsReAssign = false;
            IsMassUpdate = false;
            IsSearchScreen = false;
        }

    }

    [Serializable]
    public class SearchResults
    {
        public long? WorkItemID { get; set; }
        public long? Aging { get; set; }
        public long? Urgency { get; set; }
        public string AssignedTo { get; set; }
        public string MemberSCCCode { get; set; }
        public string MemberID { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string GPSHouseholdID { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberContractID { get; set; }
        public DateTime? MemberDOB { get; set; }
        public string MemberPBP { get; set; }
        public string MemberLOB { get; set; }
        public string MemberGender { get; set; }
        public string DiscrepancyType { get; set; }
        public string MostRecentStatus { get; set; } 
        public long MostRecentStatusLkup { get; set; }
        public string MostRecentAction { get; set; }
        public long MostRecentQueueLkup { get; set; }
        public string MostRecentQueue { get; set; }
        public string CurrentHICN { get; set; }
        public string GPSHICN { get; set; }
        public string MMRHICN { get; set; }
        public string FirstName { get; set; }
        public string DiscrepancyCategory { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public DateTime? DiscrepancyStartDate { get; set; }
        public DateTime? DiscrepancyEndDate { get; set; }
        public DateTime? ComplianceStartDate { get; set; }
        public DateTime? AdjustedComplianceStartDate { get; set; }
        public string Reason { get; set; }
        public string Resolution { get; set; }
        public string ReferencedEligibilityCaseIndicator { get; set; }
        public string MMRPBP { get; set; }
        public string GPSIndividualID { get; set; }
        public string DiscrepancySource { get; set; }
        public string NTID { get; set; }
        public string SubmissionType { get; set; }
        public string RPRCTMMember { get; set; }
        public string RPREGHPMember { get; set; }
        public DateTime? RPRRequestedEffectiveDate { get; set; }
        public string RPRActionRequested { get; set; }
        public DateTime? PotentialSubmissionDate { get; set; }
        public DateTime? RPCSubmissionDate { get; set; }
        public DateTime? FDRReceivedDate { get; set; }
        public string FDRCodeReceived { get; set; }
        public string FDRStatus { get; set; }
        public string RPRRequestor { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime EndDate { get; set; }
        public long? RPRActionRequestedLkup { get; set; }
        public string ElectionType { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long? CreatedByRef { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public long? LockedByRef { get; set; }
        public string LockedBy { get; set; }
        public long PendedByRef { get; set; }
        public string PendedBy { get; set; }
        public long? AssignedToRef { get; set; }
        public long? QueueProgressTypeLkup { get; set; }
        public string QueueProgressType { get; set; }

        public string BusinessSegment { get; set; }

        public long? workBasketLkup;
        public long? OOALetterStatusLkup { get; set; }
        public string OOALetterStatus { get; set; }

        public long? CMSTransactionStatusLkup { get; set; }

        public bool EditActionVisibility { get; set; }
        public string EncryptedCaseID { get; set; }

        public string MemberVerifiedState { get; set; }
        public string MemberVerifiedCountyCode { get; set; }

        public string PendReason { get; set; }
        


        public SearchResults()
        {
            EditActionVisibility = false;
        }
    }

    [Serializable]
    public class UISearch
    {
        public UISearch()
        {
            SearchPanel = new List<SearchResults>();
            UnlockSearchPanel = new List<UnlockSearchResults>();
        }
        public List<DOCMN_LookupMaster> LookupMaster { get; set; }
        public List<DOCMN_LookupMasterCorrelations> LookupMasterCorrelation { get; set; }
        public List<DOADM_UserMaster> UserMasterList { get; set; }
        public List<DOCMN_Department> DepartmentList { get; set; }
        public SearchCriteria SearchCriteria { get; set; }
        public List<SearchResults> SearchPanel { get; set; }
        public List<UnlockSearchResults> UnlockSearchPanel { get; set; }
        public long? TimeZoneLkup;
        public long? CurrentUserRef;
        public long? workBasketLkup;

    }

    [Serializable]
    public class UnlockSearchResults
    {
        public string CreatedBy { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public string LockedBy { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public long? WorkItemID { get; set; }
        public long? Urgency { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string CurrentHICN { get; set; }
        public string GPSHICN { get; set; }
        public string MMRHICN { get; set; }
        public string FirstName { get; set; }
        public string DiscrepancyCategory { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public DateTime? DiscrepancyStartDate { get; set; }
        public DateTime? DiscrepancyEndDate { get; set; }
        public string Reason { get; set; }
        public string Resolution { get; set; }
        public string LineofBusiness { get; set; }
        public string ReferencedEligibilityCaseIndicator { get; set; }
        public string MMRPBP { get; set; }
        public string GPSIndividualID { get; set; }
        public string DiscrepancySource { get; set; }
        public string NTID { get; set; }
        public string SubmissionType { get; set; }
        public string RPRCTMMember { get; set; }
        public string RPREGHPMember { get; set; }
        public DateTime? RPRRequestedEffectiveDate { get; set; }
        public string RPRActionRequested { get; set; }
        public DateTime? PotentialSubmissionDate { get; set; }
        public DateTime? RPCSubmissionDate { get; set; }
        public DateTime? FDRReceivedDate { get; set; }
        public string FDRCodeReceived { get; set; }
        public string FDRStatus { get; set; }
        public string RPRRequestor { get; set; }
        public string MostRecentStatus { get; set; }
        public long CMSTransactionStatusLkup { get; set; }
        public string OOALetterStatus { get; set; }
        public long OOALetterStatusLkup { get; set; }
        public string PendedBy { get; set; }
        public long PendedByRef { get; set; }
        public long LockedByRef { get; set; }
        public long LastUpdatedByRef { get; set; }
        public long CreatedByRef { get; set; }
        public string ElectionType { get; set; }
        public long? RPRActionRequestedLkup { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string MostRecentAction { get; set; }
        public string MostRecentQueue { get; set; }
        public long MostRecentQueueLkup { get; set; }
        public long MostRecentStatusLkup { get; set; }
        public string DiscrepancyType { get; set; }
        public string MemberGender { get; set; }
        public string MemberLOB { get; set; }
        public string MemberPBP { get; set; }
        public DateTime? MemberDOB { get; set; }
        public string MemberContractID { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string GPSHouseholdID { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string MemberID { get; set; }
        public string MemberSCCCode { get; set; }
        public string BusinessSegment { get; set; }
        public string QueueProgressType { get; set; }
        public long? QueueProgressTypeLkup { get; set; }
        public long? AssignedToRef { get; set; }
        public long Aging { get; set; }
    }
}

