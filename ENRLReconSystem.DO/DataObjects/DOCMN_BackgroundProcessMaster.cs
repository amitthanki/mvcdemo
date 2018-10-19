using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_BackgroundProcessMaster
    {
        #region public properties
        public long CMN_BackgroundProcessMasterId { get; set; }
        public long BackgroundProcessTypeLkup { get; set; }
        public DateTime UTCStartDate { get; set; }
        public DateTime? UTCEndDate { get; set; }
        public Double? TotalDuration { get; set; }
        public long? TotalRecordProcessed { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long LastUpdatedByRef { get; set; }

        public long BGPStatusLkup { get; set; }

        public List<DOCMN_BackgroundProcessDetails> CMN_BackgroundProcessDetails { get; set; }


        public System.Data.DataRowState RowState { get; set; }
        #endregion
    }
}
