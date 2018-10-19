using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIDOGEN_BulkImportExcelTemplate
    {
        public UIDOGEN_BulkImportExcelTemplate()
        {
            lstDOGEN_BulkImportExcelTemplateMaster = new List<DOGEN_BulkImportExcelTemplateMaster>();
            lstDOGEN_BulkImportColumnsMapping = new List<DOGEN_BulkImportColumnsMapping>();
        }
        public List<DOGEN_BulkImportExcelTemplateMaster> lstDOGEN_BulkImportExcelTemplateMaster { get; set; }
        public List<DOGEN_BulkImportColumnsMapping> lstDOGEN_BulkImportColumnsMapping { get; set; }
    }
}
