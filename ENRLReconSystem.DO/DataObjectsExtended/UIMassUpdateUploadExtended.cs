using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIMassUpdateUploadExtended
    {

        public UIMassUpdateUploadExtended()
        {
            lstActions = new List<DOCMN_LookupMasterCorrelations>();
            objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
        }
        public long? ActionLkup { get; set; }
        public long? GEN_BulkImportRef { get; set; }
        public long? QueueLkup { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstActions { get; set; }
    }
}
