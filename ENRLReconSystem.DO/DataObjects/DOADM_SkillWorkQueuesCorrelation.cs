using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_SkillWorkQueuesCorrelation
    {


        //Constructor
        public DOADM_SkillWorkQueuesCorrelation()
        {

        }


        #region public properties
        public long ADM_SkillWorkQueuesCorrelationId { get; set; }
        public long ADM_SkillsMasterRef { get; set; }
        //public long DiscrepancyCategoryLkup { get; set; }
        public long WorkQueuesLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }

        public string CreatedByName { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedByName { get; set; }



        #endregion

    }
}
