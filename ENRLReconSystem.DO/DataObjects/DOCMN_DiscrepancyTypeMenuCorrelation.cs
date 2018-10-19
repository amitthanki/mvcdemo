using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_DiscrepancyTypeMenuCorrelation
    {


        //Constructor
        public DOCMN_DiscrepancyTypeMenuCorrelation()
        {

        }


        #region public properties
        public long CMN_DiscrepancyTypeMenuCorrelationId { get; set; }
        public long CMN_MenuMasterRef { get; set; }
        public long DiscrepancyTypeLkup { get; set; }
        public long RoleLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }



        #endregion

    }
}
