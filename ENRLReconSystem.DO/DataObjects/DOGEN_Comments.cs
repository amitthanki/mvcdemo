using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_Comments
    {


        //Constructor
        public DOGEN_Comments()
        {

        }


        #region public properties
        public long GEN_CommentsId { get; set; }
        public long GEN_QueueRef { get; set; }
        public string Comments { get; set; }
        public long ActionLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string Action { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public long SourceSystemLkup { get; set; }
        public string SourceSystem { get; set; }
        #endregion

    }
}
