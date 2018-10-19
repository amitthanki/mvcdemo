using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_AlertDetails
    {
       
        //Constructor
        public DOADM_AlertDetails()
        {

        }


        #region public properties
        public long ADM_AlertDetailsId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string AlertTitle { get; set; }
        public string AlertDescription { get; set; }
        public DateTime? AlertPublishedDate { get; set; }
        public DateTime? AlertEffectiveDate { get; set; }
        public DateTime? AlertInactivationDate { get; set; }
        public long? AlertCriticalityLkup { get; set; }
        public long? SendAlertToLkup { get; set; }
        public long? CMN_DepartmentRef { get; set; }
        public long? ADM_UserMasterRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long LoginUserId { get; set; }
        public long LastUpdatedByRef { get; set; }

        //Ref coloumn values
        public String LastUpdatedByName { get; set; }
        public List<DOCMN_LookupMaster> lstTimeZone { get; set; }
        public List<DOCMN_Department> lstCMN_Department { get; set; }
        public List<DOCMN_LookupMaster> lstAlertCriticalityLkup { get; set; }
        public List<DOCMN_LookupMaster> lstSendAlertToLkup { get; set; }
        public List<DOADM_UserMaster> lstUsers { get; set; }

        public List<DORPT_ReportsMaster> lstReports { get; set; }
        public string CreatedByName { get; set; }
        public string DepartmentName { get; set; }
        public string LockedByName { get; set; }
        public string AlertCriticalityValue { get; set; }
        public string SendAlertToValue { get; set; }
        public string IndividualUserName { get; set; }
        public string AlertEffectiveDateTimeZone { get; set; }
        public string AlertInactivationDateTimeZone { get; set; }
        public string AlertPublishedDateTimeZone { get; set; }
        public bool ConsiderDates { get; set; }

        #endregion

    }
}
