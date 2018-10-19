using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_LookupMaster
    {


        //Constructor
        public DOCMN_LookupMaster()
        {
            IsActive = true;
        }
        #region public properties
        public long CMN_LookupMasterId { get; set; }
        public long CMN_LookupTypeRef { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string LookupValue { get; set; }
        public string LookupDescription { get; set; }
        public string LookupValue1 { get; set; }
        public string LookupValue2 { get; set; }
        public long? DisplayOrder { get; set; }
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
