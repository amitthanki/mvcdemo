using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_MIIMServiceTrace
    {
        public DOGEN_MIIMServiceTrace()
        {
            IsActive = true;
        }
        public long MIIMServiceTraceId { get; set; }
        public long WebServiceMethodLkup { get; set; }
        public string WebServiceMethodName { get; set; }
        public long TarceMethodLkup { get; set; }
        public string RequestInputData { get; set; }
        public string ResponseStatusMessage { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public string CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
