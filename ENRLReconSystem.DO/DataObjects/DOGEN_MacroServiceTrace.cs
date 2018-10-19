using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public  class DOGEN_MacroServiceTrace
    {
        //Constructor
        public DOGEN_MacroServiceTrace()
        {

        }


        #region public properties
        public long GEN_MacroServiceTraceId { get; set; }
        public long? GEN_QueueRef { get; set; }
        public long? MacroServiceMethodLkup { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string MacroServiceMethodName { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public string CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }

        public long? StatusLkup { get; set; }



        #endregion
    }
}
