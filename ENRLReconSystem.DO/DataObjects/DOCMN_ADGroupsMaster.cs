using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_ADGroupsMaster
    {


        //Constructor
        public DOCMN_ADGroupsMaster()
        {

        }


        #region public properties
        public long CMN_ADGroupMasterId { get; set; }
        public long RoleLkup { get; set; }
        public long WorkBasketLkup { get; set; }
        public long ADGroupNameLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }



        #endregion

    }
}
