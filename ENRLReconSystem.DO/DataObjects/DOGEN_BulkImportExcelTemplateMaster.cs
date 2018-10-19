using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_BulkImportExcelTemplateMaster
    {
        public DOGEN_BulkImportExcelTemplateMaster()
        {
            IsActive = true;
           
        }
        public long? GEN_BulkImportExcelTemplateMasterId { get; set; }
        public long? WorkBasketLkup { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public string ExcelTemplateName { get; set; }
        public string ExcelTemplateDescription { get; set; }
        public string SheetName { get; set; }
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public string ExcelDirectoryPath { get; set; }
        public string CustomValidationSP { get; set; }
        public string StagingTableName { get; set; }
        public string StagingInsertSPName { get; set; }
        public string StagingUpdateSPName { get; set; }
        public string StagingInsertTVPName { get; set; }
        public string StagingUpdateTVPName { get; set; }
        public long?  TemplateTypeLkup { get; set; }
        public long?  BusinessSegmentLkup { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long? CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long?  LastUpdatedByRef { get; set; }
        public bool IsActive { get; set; }


    }
}
