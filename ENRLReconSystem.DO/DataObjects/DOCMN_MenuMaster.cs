using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_MenuMaster
    {


        //Constructor
        public DOCMN_MenuMaster()
        {

        }


        #region public properties
        public long CMN_MenuMasterId { get; set; }
        public string MenuName { get; set; }
        public string MenuDescription { get; set; }
        public string Level1 { get; set; }
        public string Level2 { get; set; }
        public string Level3 { get; set; }
        public string Level4 { get; set; }
        public string MenuUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }



        #endregion

    }
}
