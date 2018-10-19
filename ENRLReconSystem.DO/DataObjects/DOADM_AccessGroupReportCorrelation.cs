using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AccessGroupReportCorrelation
    {


        //Constructor
        public DOADM_AccessGroupReportCorrelation()
        {

        }


        #region public properties
        public long ADM_AccessGroupReportCorrelationId { get; set; }
        public long ADM_AccessGroupMasterRef { get; set; }
        public long RPT_ReportsMasterRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }



        #endregion

    }
}
