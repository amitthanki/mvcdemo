using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_AppErrorLog
    {


        //Constructor
        public DOCMN_AppErrorLog()
        {

        }


        #region public properties
        public long CMN_AppErrorLogId { get; set; }
        public long? ADM_UserMasterRef { get; set; }
        public string ErrorLocation { get; set; }
        public long? ErrorSourceLkup { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorDescription { get; set; }
        public DateTime? UTCErrorDateTime { get; set; }
        public DateTime UTCCreatedOn { get; set; }



        #endregion

    }
}
