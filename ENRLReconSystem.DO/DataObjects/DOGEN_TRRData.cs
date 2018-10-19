using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_TRRData
    {
        //Constructor
        public DOGEN_TRRData()
        {

        }
        #region public properties
        public DateTime? ApplicationDate { get; set; }
        public string DistrictOffice { get; set; }
        public string MedicareClaimNumber { get; set; }
        public string RXId { get; set; }
        public string ContractYear { get; set; }
        public string PBPNumber { get; set; }
        public string PreviousPBP { get; set; }
        public string CoPayCategory { get; set; }
        public string EGHBIndicator { get; set; }
        public string PremiumAmountValue { get; set; }
        public string ProcessingTime { get; set; }
        public string SystemtrackingId { get; set; }
        public string TransactionReplyCode { get; set; }
        public string TransactionShortName { get; set; }
        public string ChangeInitiatorIndicator { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string RXBin { get; set; }
        public string RXPCN { get; set; }
        public string PartCPremium { get; set; }
        public string PlanId { get; set; }
        public string ProductCode { get; set; }
        public DateTime? CoPayEffectiveDate { get; set; }
        public string EmpSubsidyOverrideIndicator { get; set; }
        public string PremiumWitholdOption { get; set; }
        public string RecordType { get; set; }
        public string TransactionCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string ChangeOrgName { get; set; }
        public string GPSTrackingNumber { get; set; }
        public string RXGroup { get; set; }
        public string ContractNumber { get; set; }
        public string PartDPremium { get; set; }
        public string PreviousContractNumber { get; set; }
        public string SNPFlag { get; set; }
        public string CreditableCoverageIndicator { get; set; }
        public string NotCoveredMonths { get; set; }
        public DateTime? ProcessDate { get; set; }
        public string SourceIndicator { get; set; }
        public string TransactionCodeDescription { get; set; }
        public string TransactionId { get; set; }
        #endregion
    }
}
