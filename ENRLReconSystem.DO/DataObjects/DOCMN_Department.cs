using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_Department
    {


        //Constructor
        public DOCMN_Department()
        {

        }


        #region public properties
        public long CMN_DepartmentId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string ERSDepartmentName { get; set; }
        public long? BusinessSegmentLkup { get; set; }
        public long? DepartmentLkup { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? InactivationDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public string CreatedByName { get; set; }

        public string LastUpdatedByName { get; set; }
        public string LockedByName { get; set; }

        // Ref Column Value


        public String CMN_DepartmentValue { get; set; }
        public List<DOCMN_LookupMaster> lstBusinessSegment { get; set; }
        public List<DOCMN_LookupMaster> lstCMN_Department { get; set; }

        public List<DOCMN_LookupMaster> lstTimeZone { get; set; }


        public long? DepartmentEffectiveDateTimeZone { get; set; }
        public long? DepartmentInactivationDateTimeZone { get; set; }
        #endregion

    }
}
