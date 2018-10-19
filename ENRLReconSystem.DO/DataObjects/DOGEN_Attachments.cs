using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_Attachments
    {


        //Constructor
        public DOGEN_Attachments()
        {

        }
        #region public properties
        public long slno { get; set; }
        public long GEN_AttachmentsId { get; set; }
        public long GEN_QueueRef { get; set; }
        public string FileName { get; set; }
        public string UploadedFileName { get; set; }
        public string FilePath { get; set; }
        public long? GEN_DMSDataRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedBy { get; set; }
        #endregion

    }
}
