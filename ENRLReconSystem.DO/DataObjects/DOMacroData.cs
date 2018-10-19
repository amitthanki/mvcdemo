using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO.DataObjects
{
    [Serializable]
    public class DOMacroData
    {
        public long WorkItemID { get; set; }
        public string MemberSCCCode { get; set; }
        public string MemberID { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string MemberFirstName { get; set; }
        public string MemberMiddleName { get; set; }
        public string MemberLastName { get; set; }
        public string MemberContractID { get; set; }
        public string MemberDOB { get; set; }
        public string MemberPBP { get; set; }
        public string MemberLOB { get; set; }
        public string MemberGender { get; set; }
        public string DiscrepancyType { get; set; }
        public long MostRecentStatusLkup { get; set; }
        public string MostRecentStatus { get; set; }
        public long MostRecentWorkQueueLkup { get; set; }
        public string MostRecentQueue { get; set; }
        public string MostRecentAction { get; set; }
        public string GPSHouseholdID { get; set; }
        public string StateCode { get; set; }
        public string CountyCode { get; set; }
        public string PlanCode { get; set; }
        public string PotentialTermDate { get; set; }
        public string ComplianceStartDate { get; set; }    
        
        public string InsuredPlanEffectiveDate { get; set; }
        public string ErrorMessage { get; set; }
    }

    [Serializable]
    public class DOMacroUpdate
    {
        public long? GEN_QueueId { get; set; }
        //public string HouseHoldID { get; set; }
        public string Status { get; set; }
        public bool IsValid { get; set; }

        public string LoginID { get; set; }
        public string ValidationMessage { get; set; }
    }
}
