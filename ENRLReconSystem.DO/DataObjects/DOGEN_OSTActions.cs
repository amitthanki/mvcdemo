using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_OSTActions
    {


        //Constructor
        public DOGEN_OSTActions()
        {
            lstResolution = new List<DOCMN_LookupMasterCorrelations>();
            lstPendReasons = new List<DOCMN_LookupMasterCorrelations>();
            IsActive = true;
            //lstPbpid = new List<DOCMN_LookupMasterCorrelations>();
        }


        #region public properties
        public long GEN_OSTActionsId { get; set; }
        public long? GEN_QueueRef { get; set; }
        public long? ActionLkup { get; set; }
        public string LastName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public long? ContractIDLkup { get; set; }
        public long? PBPLkup { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? FirstLetterMailDate { get; set; }
        public DateTime? FirstLetterMailDateNoResponseTerm { get; set; }
        public DateTime? SecondLetterMailDate { get; set; }
        public long? ResidentialDocumentationRequired { get; set; }
        public long? CountyAttestationRequired { get; set; }
        public long? PendReasonLkup { get; set; }
        public long? ContainsErrorsLkup { get; set; }
        public long? ResolutionLkup { get; set; }
        public string Reason { get; set; }
        public DateTime? InitialAddressVerificationDate { get; set; }
        public DateTime? MemberResponseVerificationDate { get; set; }
        public string MemberVerifiedState { get; set; }
        public DateTime? SCCLetterMailDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public long? RoleLkup { get; set; }
        public long CommentsSourceSystemLkup { get; set; }

        public long OptionLkup { get; set; }

        public long? ReopenQueueLKUP { get; set; }

        public long? OOALetterStatusLkup { get; set; }

        public long? CMSTransactionStatusLkup { get; set; }

        public string GPSHouseholdID { get; set; }


        public DateTime? AdjustedDiscrepancyReceiptDate { get; set; }

        public DateTime? AdjustedDisenrollmentDate { get; set; }
        public DateTime? AdjustedComplianceStartDate { get; set; }

        public long? MARxAddressCompletedLkup { get; set; }

        public string MARxAddressResolution { get; set; }

        public List<DOCMN_LookupMaster> lstPDPAutoEnrolleeInd { get; set; }

        public bool PDPAutoIndicator { get; set; }

        public List<DOCMN_LookupMaster> lstCountryAttestationRequired { get; set; }
        public List<DOCMN_LookupMaster> lstResidentialDocRequired { get; set; }



        #endregion


        #region SCC Case
        public string ContractNumber { get; set; }
        public string PBPValue { get; set; }
        public string Comments { get; set; }
        #endregion


        public List<DOCMN_LookupMasterCorrelations> lstResolution { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstPendReasons { get; set; }
        public List<DOCMN_LookupMaster> lstContractid { get; set; }
        public List<DOCMN_LookupMaster> lstPbpid { get; set; }
        public List<DOCMN_LookupMaster> lstContainsErros { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstQueue { get; set; }
        public List<DOCMN_LookupMaster> lstState { get; set; }

        public List<DOCMN_LookupMaster> lstMarxAddresCompleted { get; set; }
        public string PendReason { get; set; }
        public string Action { get; set; }
        public string ContractID { get; set; }
        public string PBP { get; set; }
        public string ContainsErrors { get; set; }
        public string Resolution { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public long? PDPAutoEnrolleeInd { get; set; }

        public long? GEN_BulkImportRef { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public long? QueueLkup { get; set; }
        public long? BusinessSegmentLkup { get; set; }

        #region SCCRPR

        public List<DOCMN_LookupMaster> lstActionRequested { get; set; }
        public List<DOCMN_LookupMaster> lstTaskBeingPerformed { get; set; }
        public List<DOADM_UserMaster> lstUsers { get; set; }

        public DateTime? RPRRequestedEffectiveDate { get; set; }
        public long? RootCauseLkup { get; set; }
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
        public long? RPRActionRequestedLkup { get; set; }
        public string RPROtherActionRequested { get; set; }
        public long? RPRSupervisorOrRequesterRef { get; set; }
        public string RPRReasonforRequest { get; set; }
        public long? RPRTaskPerformedLkup { get; set; }
        public string RPROtherTaskPerformed { get; set; }
        public string Gen_QueueIds { get; set; }
        #endregion
    }



}
