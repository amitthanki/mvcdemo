using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class QueueSummaryDetails
    {
        public string Queue { get; set; }

        public long QueueLkup { get; set; }

        public long Count { get; set; }
    }
}
