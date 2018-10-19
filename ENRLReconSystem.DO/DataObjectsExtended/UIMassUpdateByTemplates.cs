using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIMassUpdateByTemplates
    {
        public UIMassUpdateByTemplates()
        {
            lstDiscCategary = new List<DOCMN_LookupMasterCorrelations>();
            lstWorkbasket = new List<DOCMN_LookupMaster>();
            lstQueue = new List<DOCMN_LookupMasterCorrelations>();
            objDOGEN_BulkImportExcelTemplateMaster = new DOGEN_BulkImportExcelTemplateMaster();
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();


        }
        public long? Gen_BulkImportId { get; set; }
        public long? WorkBasketLkup { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public long? Queue { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscCategary { get; set; }
        public List<DOCMN_LookupMaster> lstWorkbasket { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstQueue { get; set; }
        public DOGEN_BulkImportExcelTemplateMaster objDOGEN_BulkImportExcelTemplateMaster { get; set; }
        public List<DOGEN_BulkImport> lstDOGEN_BulkImport { get; set; }
        public long TemplateTypeLkup { get; set; }
    }
}
