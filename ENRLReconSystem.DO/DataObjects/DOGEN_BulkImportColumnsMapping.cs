using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_BulkImportColumnsMapping
    {
        public DOGEN_BulkImportColumnsMapping()
        {

        }
        public long? GEN_BulkImportColumnsMappingId { get; set; }
        public long? GEN_BulkImportColumnsMappingParentRef { get; set; }
        public long? GEN_BulkImportExcelTemplateMasterRef { get; set; }
        public long? ColumnTypeLkup { get; set; }
        public string DBColumnName { get; set; }
        public string ColumnDisplayName { get; set; }
        public bool IsRequired { get; set; }
        public int MaxLength { get; set; }
        public int ColumnSequence { get; set; }
        public long? ControlTypeLkup { get; set; }
        public long? ControlLkupValue { get; set; }
        public bool IsUniqueKey { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long? CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public long? TemplateTypeLkup { get; set; }
        public long? BusinessSegmentLkup { get; set; }
    }
}
