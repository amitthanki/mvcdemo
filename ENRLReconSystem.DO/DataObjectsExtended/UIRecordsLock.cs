using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIRecordsLock
    {
        public UIRecordsLock()
        {

        }
        public long CMN_RecordsLockedId { get; set; }
        public long ScreenLkup { get; set; }
        public String ScreenValue { get; set; }
        public long CaseId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Guid { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public string CreatedByName { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public double? LockedHours { get; set; }
        public bool IsEditInProgress { get; set; }
        public long Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
