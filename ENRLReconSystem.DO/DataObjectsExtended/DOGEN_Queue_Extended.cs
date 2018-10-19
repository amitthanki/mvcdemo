using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_Queue_Extended : DOGEN_Queue
    {
        public string Urgency { get; set; }

        public long? CaseAge { get; set; }

        public string Reason { get; set; }

        public new long?  PendReasonLkup { get; set; }

        public string UserEscalationReason { get; set; }
    }
}
