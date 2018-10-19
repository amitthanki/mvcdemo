using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_TableSequenceCounts
    {


        //Constructor
        public DOCMN_TableSequenceCounts()
        {

        }


        #region public properties
        public long CMN_TableSequenceCountsId { get; set; }
        public long Sequence { get; set; }
        public string SequenceDescription { get; set; }



        #endregion

    }
}
