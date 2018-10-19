using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_BackgroundProcessDetails
    {
        #region public properties
        public long CMN_BackgroundProcessDetailsId { get; set; }
        public long CMN_BackgroundProcessMasterRef { get; set; }
        public long? GEN_QueueRef { get; set; }
        public long? GEN_BulkImportRef { get; set; }
        public long? NGS_PrintMetadataFileMasterRef { get; set; }
        public long? NGS_VendorReturnFileMasterRef { get; set; }
        public string UploadFileName { get; set; }
        public long? BGPRecordStatusLkup { get; set; }
        public long? CMN_AppErrorLogRef { get; set; }
        public string FailureReason { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long? GEN_AutoFaxTaskRef { get; set; }



        public System.Data.DataRowState RowState { get; set; }
        public long GEN_FDRUploadStagingRef { get; set; }
        #endregion
    }
}
