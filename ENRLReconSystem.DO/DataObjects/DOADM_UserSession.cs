using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_UserSession
    {


        //Constructor
        public DOADM_UserSession()
        {

        }


        #region public properties
        public long ADM_UserSessionId { get; set; }
        public long ADM_UserMasterRef { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoffTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }



        #endregion

    }
}
