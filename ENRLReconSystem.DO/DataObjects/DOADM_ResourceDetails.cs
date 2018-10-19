using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_ResourceDetails
    {


        //Constructor
        public DOADM_ResourceDetails()
        {

        }


        #region public properties
        public long ADM_ResourceDetailsId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string ResourceName { get; set; }
        public string ResourceDescription { get; set; }
        public string ResourceLinkLocation { get; set; }
        public DateTime? ResourceEffectiveDate { get; set; }
        public DateTime? ResourceInactivationDate { get; set; }
        public long? CMN_DepartmentRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long LastUpdatedByRef { get; set; }
        public long LoginUserId { get; set; }
        public bool ConsiderDates { get; set; }

        //Ref coloumn values
        public String CreatedByName { get; set; }
        public String CMN_DepartmentValue { get; set; }
        public List<DOCMN_LookupMaster> lstTimeZone { get; set; }
        public List<DOCMN_Department> lstCMN_Department { get; set; }
        public string LockedByName { get; set; }
        public string LastUpdatedByName { get; set; }

        public string ResourceEffectiveDateTimeZone { get; set; }
        public string ResourceInactivationDateTimeZone { get; set; }
        #endregion

    }
}
