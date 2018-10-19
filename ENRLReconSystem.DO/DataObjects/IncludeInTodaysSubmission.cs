using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    public class IncludeInTodaysSubmission
    {
        public IncludeInTodaysSubmission()
        {
        }
        public long Gen_QueueId { get; set; }
        public string MemberContractId { get; set; }
        public string MemberMedicareId { get; set; }
        public string SubmissionFileName { get; set; }
    }
}
