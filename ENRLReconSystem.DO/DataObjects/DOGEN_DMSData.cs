using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_DMSData
    {


        //Constructor
        public DOGEN_DMSData()
        {

        }


        #region public properties
        public long GEN_DMSDataId { get; set; }
        public long GEN_QueueRef { get; set; }
        public string DMSDocId { get; set; }
        public string DMSFileName { get; set; }
        public string DMSFilePath { get; set; }
        public string DMSUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }



        #endregion

    }
}
