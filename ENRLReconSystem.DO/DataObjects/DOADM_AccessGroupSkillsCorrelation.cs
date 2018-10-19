using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AccessGroupSkillsCorrelation
    {


        //Constructor
        public DOADM_AccessGroupSkillsCorrelation()
        {

        }


        #region public properties
        public long ADM_AccessGroupSkillsCorrelationId { get; set; }
        public long ADM_AccessGroupMasterRef { get; set; }
        public long ADM_SkillsMasterRef { get; set; }
        public bool CanCreate { get; set; }
        public bool CanModify { get; set; }
        public bool CanSearch { get; set; }
        public bool CanView { get; set; }
        public bool CanMassUpdate { get; set; }
        public bool CanHistory { get; set; }
        public bool CanReassign { get; set; }
        public bool CanUnlock { get; set; }
        public bool CanUpload { get; set; }
        public bool CanClone { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public bool CanReopen { get; set; }
        public string LastUpdatedByName { get; set; }
        public string CreatedByName { get; set; }



        #endregion

    }
}
