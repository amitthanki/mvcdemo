using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AccessGroupUserCorrelation
    {


        //Constructor
        public DOADM_AccessGroupUserCorrelation()
        {
            IsActive = true;

        }


        #region public properties
        public long ADM_AccessGroupUserCorrelationId { get; set; }
        public long ADM_AccessGroupMasterRef { get; set; }
        public long ADM_UserMasterRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public String CreatedByName { get; set; }
        public string LastUpdatedBy { get; set; }

        #endregion

    }
}
