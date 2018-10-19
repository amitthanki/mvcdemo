using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_LookupType
    {


        //Constructor
        public DOCMN_LookupType()
        {
            IsActive = true;

        }


        #region public properties
        public long CMN_LookupTypeId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string LookupTypeDescription { get; set; }
        public bool IsActive { get; set; }
        public DateTime? UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string  LockedByName { get; set; }
        public string  CreatedByName { get; set; }
        public string  LastUpdatedByName { get; set; }
        
        #endregion
        public List<DOCMN_LookupMaster> lstDOCMN_LookupMaster { get; set; }

    }
}
