using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_UserPreference
    {


        //Constructor
        public DOADM_UserPreference()
        {

        }


        #region public properties
        [DataMember]
        public long ADM_UserPreferenceId { get; set; }
        [DataMember]
        public long ADM_UserMasterRef { get; set; }
        [DataMember]
        public bool ShowAlerts { get; set; }
        [DataMember]
        public bool ShowResources { get; set; }
        [DataMember]
        public long? BusinessSegmentLkup { get; set; }
        [DataMember]
        public long? RoleLkup { get; set; }
        [DataMember]
        public long? TimezoneLkup { get; set; }
        [DataMember]
        public long? WorkBasketLkup { get; set; }
        [DataMember]
        public bool ShowOSTSummary { get; set; }
        [DataMember]
        public bool ShowEligibilitySummary { get; set; }
        [DataMember]
        public bool ShowRPRSummary { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public DateTime UTCCreatedOn { get; set; }
        [DataMember]
        public long CreatedByRef { get; set; }
        [DataMember]
        public DateTime? UTCLastUpdatedOn { get; set; }
        [DataMember]
        public long? LastUpdatedByRef { get; set; }



        #endregion

    }
}
