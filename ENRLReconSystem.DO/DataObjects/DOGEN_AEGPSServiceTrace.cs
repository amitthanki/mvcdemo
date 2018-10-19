using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOGEN_AEGPSServiceTrace
    {


        //Constructor
        public DOGEN_AEGPSServiceTrace()
        {

        }


        #region public properties
        public long GEN_AEGPSServiceTraceId { get; set; }
        public long GEN_QueueRef { get; set; }
        public long? WebServiceMethodLkup { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string WebServiceMethodName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public long? StatusLkup { get; set; }



        #endregion

    }
}
