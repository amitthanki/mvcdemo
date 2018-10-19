using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DORPT_ReportsMaster
    {


        //Constructor
        public DORPT_ReportsMaster()
        {

        }


        #region public properties
        public long RPT_ReportsMasterId { get; set; }
        public string ReportName { get; set; }
        public string ReportServer { get; set; }
        public string ReportURL { get; set; }
        public long ReportsCategoryLkup { get; set; }
        public bool ViewInUI { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }

        public long WorkBasketLkup { get; set; }
        public long RoleLkup { get; set; }

        public long? BusinessSegment { get; set; }


        #endregion

    }
}
