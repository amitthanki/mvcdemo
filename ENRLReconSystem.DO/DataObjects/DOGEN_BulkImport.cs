using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_BulkImport
    {
        public DOGEN_BulkImport()
        {
            

        }
        public long? GEN_BulkImportId { get; set; }
        public long? WorkBasketLkup { get; set; }
        public string WorkBasket { get; set; }
        public long? DiscrepancyCategoryLkup { get; set; }
        public string DiscrepancyCategory { get; set; }
        public long? GEN_BulkImportExcelTemplateMasterRef { get; set; }
        public long? LockedByRef { get; set; }
        public string LockedBy { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string ExcelFileName { get; set; }
        public string DuplicateFileName { get; set; }
        public string ExcelFilelPath { get; set; }
        public int TotalRecordsCount { get; set; }
        public int ValidRecordsCount { get; set; }
        public int InvalidRecordsCount { get; set; }
        public int DuplicateRecordCount { get; set; }
        public string ErrorDescription { get; set; }
        public long? ExcelStatusLkup { get; set; }
        public string ExcelStatus { get; set; }
        public long? ImportStatusLkup { get; set; }
        public string ImportStatus { get; set; }
        public long? CMN_AppErrorLogRef { get; set; }
        public string CMN_AppErrorLog { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long? CreatedByRef { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
