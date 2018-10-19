using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_ManageCases
    {


        //Constructor
        public DOGEN_ManageCases()
        {

        }


        #region public properties
        public long GEN_ManageCasesId { get; set; }
        public long GEN_QueueRef { get; set; }
        public long ActionPerformedLkup { get; set; }
        public long CurrentUserRef { get; set; }
        public string CasesComments { get; set; }
        public long? ReAssignUserRef { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }



        #endregion

    }
}
