using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class QueueSummary
    {
        public QueueSummary()
        {
            StartDate = DateTime.Now.AddDays(-90);
            EndDate = DateTime.Now;
            objOOAQueueSummary = new OOAQueueSummary();
            objSCCQueueSummary = new SCCQueueSummary();
            objTRRQueueSummary = new TRRQueueSummary();
            objEligibilityQueueSummary = new EligibilityQueueSummary();
            objDOBQueueSummary = new DOBQueueSummary();
            objGenderQueueSummary = new GenderQueueSummary();
            objRPRQueueSummary = new RPRQueueSummary();
            objDOGEN_Queue = new DOGEN_Queue();
            lstDOGEN_Queue = new List<DOGEN_Queue>();
            lstMostRecentItem = new List<MostRecentItem>();
            lstDOADM_AlertDetails = new List<DOADM_AlertDetails>();
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public OOAQueueSummary objOOAQueueSummary { get; set; }
        public SCCQueueSummary objSCCQueueSummary { get; set; }
        public TRRQueueSummary objTRRQueueSummary { get; set; }
        public EligibilityQueueSummary objEligibilityQueueSummary { get; set; }
        public DOBQueueSummary objDOBQueueSummary { get; set; }
        public GenderQueueSummary objGenderQueueSummary { get; set; }
        public RPRQueueSummary objRPRQueueSummary { get; set; }
        public DOGEN_Queue objDOGEN_Queue { get; set; }
        public List<DOGEN_Queue> lstDOGEN_Queue { get; set; }
        public List<DOADM_AlertDetails> lstDOADM_AlertDetails { get; set; }
        public List<DOADM_ResourceDetails> lstDOADM_ResourceDetails { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
        public List<MostRecentItem> lstMostRecentItem { get; set; }

        public long? BusinessSegment { get; set; }

        public bool ShowOSTQueueSummary { get; set; }
        public bool ShowEligQueueSummary { get; set; }
        public bool ShowRPRQueueSummary { get; set; }
        public bool ShowAlerts { get; set; }
        public bool ShowResources { get; set; }
    }

    [Serializable]
    public class OOAQueueSummary
    {
        public OOAQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long OOAAddressScrub { get; set; }
        public long OOACMSAccepted { get; set; }
        public long OOACMSRejected { get; set; }
        public long OOACompleted { get; set; }
        public long OOAMARxAddressLetter { get; set; }
        public long OOAMIIMUpdated { get; set; }
        public long OOANewCase { get; set; }
        public long OOAOpenDisenroll { get; set; }
        public long OOAOpenNOT { get; set; }
        public long OOAPended { get; set; }
        public long OOAPendingAudit { get; set; }
        public long OOAPendingFTT { get; set; }
        public long OOAPendingNOT { get; set; }
        public long OOASubmitToCMS { get; set; }
        public long OOAUpdateSentToCMS { get; set; }
        public long OOAPeerAuditFailed { get; set; }

        public long OOAUpdateSenttoCMSFAILED { get; set; }

        public long OOALetterSentFAILED { get; set; }
        public long OOAProcessingTotal { get; set; }
        public long OOACompletedTotal { get; set; }
        public long OOAHoldingTotal { get; set; }
        public long OOATotal { get; set; }

        public long OOANeedEGHPReview { get; set; }

        public long OOAOpenNOTMacro { get; set; }

        public long OOAOpenDisenrollMacro { get; set; }
       
        public long OOAPendingSCCRPR { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }

    }

    [Serializable]
    public class SCCQueueSummary
    {
        public SCCQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long SCCAddressScrub { get; set; }
        public long SCCCMSAccepted { get; set; }
        public long SCCCMSRejected { get; set; }
        public long SCCCompleted { get; set; }
        public long SCCMARxAddressLetter { get; set; }
        public long SCCMIIMUpdated { get; set; }
        public long SCCNewCase { get; set; }
        public long SCCOpenDisenroll { get; set; }
        public long SCCOpenNOT { get; set; }
        public long SCCPended { get; set; }
        public long SCCPendingAudit { get; set; }
        public long SCCPendingFTT { get; set; }
        public long SCCPendingNOT { get; set; }
        public long SCCPendingSCCRPR { get; set; }
        public long SCCSubmitToCMS { get; set; }
        public long SCCUpdateSentToCMS { get; set; }
        public long SCCPeerAuditFailed { get; set; }
        public long SCCProcessingTotal { get; set; }
        public long SCCCompletedTotal { get; set; }
        public long SCCHoldingTotal { get; set; }
        public long SCCTotal { get; set; }

        public long SCCPotentialSCCRPRDay1 { get; set; }

        public long SCCPotentialSCCRPRDay2 { get; set; }

        public long SCCNeedEGHPReview { get; set; }

        public long SCCUpdateSenttoCMSFAILED { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class TRRQueueSummary
    {
        public TRRQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long TRRCMSRejected { get; set; }
        public long TRRCMSRejectedDeletionCode { get; set; }
        public long TRRCompleted { get; set; }
        public long TRREscalated { get; set; }
        public long TRRFalloutTRC085 { get; set; }
        public long TRRFalloutTRC155 { get; set; }
        public long TRRMARxAddressLetter { get; set; }
        public long TRRAddressScrub { get; set; }        
        public long TRRPendingNOT { get; set; }        
        public long TRRPendingFTT { get; set; }
        public long TRROpenNOT { get; set; }
        public long TRROpenDisenroll { get; set; }        
        public long TRRPendingAudit { get; set; }
        public long TRRSubmitToCMS { get; set; }
        public long TRRSubmitToCMSDeletionCode { get; set; }
        public long TRRTRC085 { get; set; }
        public long TRRTRC15476 { get; set; }

        public long TRRTRC15401 { get; set; }
        public long TRRTRC155 { get; set; }
        public long TRRTRC282 { get; set; }
        public long TRRUpdateSentToCMS { get; set; }
        public long TRRUpdateSentToCMSDeletionCode { get; set; }
        public long TRRPended { get; set; }
        public long TRRPendingSCCRPR { get; set; }
        public long TRRCMSAccepted { get; set; }
        public long TRRCMSAcceptedDeletionCode { get; set; }
        public long TRRPeerAuditFailed { get; set; }
        public long TRRProcessingTotal { get; set; }
        public long TRRCompletedTotal { get; set; }
        public long TRRHoldingTotal { get; set; }
        public long TRRTotal { get; set; }

        public long TRRNeedEGHPReview { get; set; }

        public long TRRUpdateSenttoCMSFAILED { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class EligibilityQueueSummary
    {
        public EligibilityQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long EligCMSAccepted { get; set; }
        public long EligCMSRejected { get; set; }
        public long EligCompleted { get; set; }
        public long EligNewCase { get; set; }
        public long EligPended { get; set; }
        public long EligPendingAudit { get; set; }
        public long EligSubmitToCMS { get; set; }
        public long EligUpdateSentToCMS { get; set; }
        public long EligPeerAuditFailed { get; set; }
        public long EligProcessingTotal { get; set; }
        public long EligCompletedTotal { get; set; }
        public long EligHoldingTotal { get; set; }
        public long EligTotal { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class DOBQueueSummary
    {
        public DOBQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long DOBCompleted { get; set; }
        public long DOBNewCase { get; set; }
        public long DOBPended { get; set; }
        public long DOBPendingAudit { get; set; }
        public long DOBPeerAuditFailed { get; set; }
        public long DOBProcessingTotal { get; set; }
        public long DOBCompletedTotal { get; set; }
        public long DOBHoldingTotal { get; set; }
        public long DOBTotal { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class GenderQueueSummary
    {
        public GenderQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long GenderCompleted { get; set; }
        public long GenderNewCase { get; set; }
        public long GenderPended { get; set; }
        public long GenderPendingAudit { get; set; }
        public long GenderPeerAuditFailed { get; set; }
        public long GenderProcessingTotal { get; set; }
        public long GenderCompletedTotal { get; set; }
        public long GenderHoldingTotal { get; set; }
        public long GenderTotal { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class RPRQueueSummary
    {
        public RPRQueueSummary()
        {
            lstUserAccessQueueLkups = new List<long>();
        }
        public long RPRCMSAccountManagerSent { get; set; }
        public long RPRCMSRejectedDeletionCode { get; set; }
        public long RPRCompleted { get; set; }
        public long RPRInitialSCCRPR { get; set; }
        public long RPRReceivedRPCFDR { get; set; }
        //public long RPRReceivedTRC282 { get; set; }
        public long RPRPeerAudit { get; set; }
        public long RPRPeerAuditFailed { get; set; }
        public long RPRRejected { get; set; }
        public long RPRRequestCategory2 { get; set; }
        public long RPRRequestCategory3 { get; set; }
        public long RPRReSubmission { get; set; }
        public long RPRSubmissionCategory2 { get; set; }
        public long RPRSubmissionCategory3 { get; set; }
        public long RPRPended { get; set; }
        public long RPRSCCRPRFDRReceived { get; set; }
        public long RPRSCCRPRReSubmission { get; set; }
        public long RPRSCCRPRSent { get; set; }
        public long RPRSCCRPRSubmission { get; set; }
        public long RPRSCCRPRTransactionInquiry { get; set; }
        public long RPRSentToRPC { get; set; }
        //public long RPRSubmitToCMSDeletionCode { get; set; }
        //public long RPRUpdateSentToCMSDeletionCode { get; set; }
        public long RPREligibilityUpdateInMARx { get; set; }

        public long RPRTrend_2 { get; set; }

        public long TransactionInquire { get; set; }

        public long RPRRequestCategory2CTM { get; set; }

        public long RPRProcessingTotal { get; set; }
        public long RPRCompletedTotal { get; set; }
        //public long RPRHoldingTotal { get; set; }
        public long RPRTotal { get; set; }
        public List<long> lstUserAccessQueueLkups { get; set; }
    }

    [Serializable]
    public class MostRecentItem
    {
        public long Gen_QueueId { get; set; }
        public long BusinessSegmentLkup { get; set; }
        public string BusinessSegment { get; set; }
        public long WorkBasketLkup { get; set; }
        public string WorkBasket { get; set; }
        public long DiscrepancyCategoryLkup { get; set; }
        public string DiscrepancyCategory { get; set; }
        public long DiscrepancyTypeLkup { get; set; }
        public string DiscrepancyType { get; set; }
        public long? AssignedToRef { get; set; }
        public long? PendedByRef { get; set; }
        public string AssignedTo { get; set; }
        public DateTime? UTCAssignedOn { get; set; }
        public DateTime? CSTAssignedOn { get; set; }
        public long? LockedByRef { get; set; }
        public string LockedBy { get; set; }
        public DateTime? CSTLockedOn { get; set; }
        public long? MostRecentActionLkup { get; set; }
        public string MostRecentAction { get; set; }
        public long? MostRecentWorkQueueLkup { get; set; }
        public string MostRecentWorkQueue { get; set; }
        public long? MostRecentStatusLkup { get; set; }
        public string MostRecentStatus { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public long LastUpdatedRef { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string EncryptedCaseID { get; set; }
        public bool EditActionVisibility { get; set; }
        public long? CMSTransactionStatusLkup { get; set; }
        public long? OOALetterStatusLkup { get; set; }
        public long? QueueProgressTypeLkup { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string GPSHouseholdID { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberContract { get; set; }
        public string MemberPBP { get; set; }
    }
}
