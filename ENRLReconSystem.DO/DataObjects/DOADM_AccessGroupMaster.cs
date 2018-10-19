using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AccessGroupMaster
    {


        //Constructor
        public DOADM_AccessGroupMaster()
        {

        }


        #region public properties
        public long ADM_AccessGroupMasterId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string AccessGroupName { get; set; }
        public string AccessGroupDescription { get; set; }
        public long RoleLkup { get; set; }
        public long WorkBasketLkup { get; set; }
        public long DiscrepancyCategory { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string CreatedByName { get; set; }
        public string LastUpdateByName { get; set; }
        public string LockedByName { get; set; }


        public List<DOADM_SkillsMaster> Skills { get; set; }
        public List<DORPT_ReportsMaster> Reports { get; set; }
        #endregion

    }
}
