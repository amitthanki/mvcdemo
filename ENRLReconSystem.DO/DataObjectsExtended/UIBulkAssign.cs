using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIBulkAssign
    {
        public List<long> QueueIds { get; set; }
        public DOGEN_ManageCases DOGEN_ManageCases { get; set; }
    }
}
