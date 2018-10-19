using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class FDRSubmissionRow
    {
        public string MemberContractID { get; set; }
        public string MemberPBP { get; set; }
        public string MemberCurrentHICN { get; set; }
        public string MemberLastName { get; set; }
        public string MemberFirstName { get; set; }
        public string ElectionType { get; set; }
        public string RPRRequestedEffectiveDate { get; set; }
        public string EffectiveDate { get; set; }
        public string EndDate { get; set; }
        public string ApplicationDate { get; set; }
        public long RPRActionRequestedLkup { get; set; }
        public long GEN_QueueId { get; set; }
        public string SCCRPRRequested { get; set; }
        public string SCCRPRRequestedZip { get; set; }

        public bool IsFDRSubmissionCompleted { get; set; }

        public long ResolutionLkup { get; set; }
        public string Errormessage { get; set; }


    }
}
