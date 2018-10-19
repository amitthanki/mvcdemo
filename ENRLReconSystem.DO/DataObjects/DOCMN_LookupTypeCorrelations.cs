using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_LookupTypeCorrelations
    {


        //Constructor
        public DOCMN_LookupTypeCorrelations()
        {
            IsActive = true;

        }


        #region public properties
        public long CMN_LookupTypeCorrelationsId { get; set; }
        public long CMN_LookupTypeParentRef { get; set; }
        public string CMN_LookupTypeParentValue { get; set; }
        public long CMN_LookupTypeChildRef { get; set; }
        public string CMN_LookupTypeChildValue { get; set; }
        public long? GroupingLookupTypeRef { get; set; }
        public string GroupingLookupTypeValue { get; set; }
        public string CorrelationDescription { get; set; }
        public long? LockedByRef { get; set; }
        public string LockedByName { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedByName { get; set; }
        #endregion

        public List<DOCMN_LookupType> lstDOCMN_LookupType { get; set; }
        public List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations { get; set; }

    }
}
