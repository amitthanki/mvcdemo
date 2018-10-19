using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIMassUpdateExtended
    {
        public UIMassUpdateExtended()
        {
            lstActions = new List<DOCMN_LookupMasterCorrelations>();
        }
        public long? ActionLkup { get; set; }

        public long? QueueLkup { get; set; }
        public long?  DiscrepancyCategoryLkup { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstActions { get; set; }
    }
}
