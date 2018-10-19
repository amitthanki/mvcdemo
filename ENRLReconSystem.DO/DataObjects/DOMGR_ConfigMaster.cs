using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOMGR_ConfigMaster
    {


        //Constructor
        public DOMGR_ConfigMaster()
        {

        }


        #region public properties
        public long MGR_ConfigMasterId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string ConfigName { get; set; }
        public string ConfigValue { get; set; }
        public long? Version { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }

        public string CreatedByName { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public string LastUpdatedByName { get; set; }
        public string LockedByName { get; set; }


        #endregion

        public List<DOCMN_LookupMaster> lstTimeZone { get; set; }

        public long? ConfigurationEffectiveDateTimeZone { get; set; }
        public long? ConfigurationInactivationDateTimeZone { get; set; }

    }
}
