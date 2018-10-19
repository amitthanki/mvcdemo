using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AlertsRead
    {


        //Constructor
        public DOADM_AlertsRead()
        {

        }


        #region public properties
        public long CMN_AlertsReadId { get; set; }
        public long ADM_AlertDetailsRef { get; set; }
        public long ADM_UserMasterRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }



        #endregion

    }
}
