using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIBulkUploadSearch
    {
        public UIBulkUploadSearch()
        {
            lstDiscCategary = new List<DOCMN_LookupMasterCorrelations>();
            lstWorkbasket = new List<DOCMN_LookupMaster>();
            lstDOGEN_BulkImportExcelTemplateMaster = new List<DOGEN_BulkImportExcelTemplateMaster>();
            lstDOGEN_BulkImport = new List<DOGEN_BulkImport>();
        }

        public long? TemplateTypeLkup { get; set; }
        public long? BulkImportID { get; set; }
        public long? WorkbasketLkup { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDiscCategary { get; set; }
        public List<DOCMN_LookupMaster> lstWorkbasket { get; set; }
        public List<DOGEN_BulkImportExcelTemplateMaster> lstDOGEN_BulkImportExcelTemplateMaster { get; set; }
        public List<DOGEN_BulkImport> lstDOGEN_BulkImport { get; set; }

    }
}
