using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOCMN_LookupMasterCorrelations
    {


        //Constructor
        public DOCMN_LookupMasterCorrelations()
        {
            IsActive = true;

        }


        #region public properties
        public long CMN_LookupMasterCorrelationsId { get; set; }
        public long CMN_LookupTypeCorrelationsRef { get; set; }
        public long CMN_LookupMasterParentRef { get; set; }

        public string LookupMasterParentValue { get; set; }
        public long CMN_LookupMasterChildRef { get; set; }
        public string LookupMasterChildValue { get; set; }
        public long? GroupingLookupMasterRef { get; set; }
        public string GroupingLookupMasterValue { get; set; }
        public string CorrelationDescription { get; set; }
        public long? DisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public string CreatedByName { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime? UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public string LastUpdatedByName { get; set; }
        #endregion

    }

    [Serializable]
    public class DOCMN_LookupMasterCorrelationsExtended
    {
        public DOCMN_LookupMasterCorrelationsExtended()
        {
            objDOCMN_LookupTypeCorrelations = new DOCMN_LookupTypeCorrelations();
            objDOCMN_LookupMasterCorrelations = new DOCMN_LookupMasterCorrelations();
            lstDOCMN_LookupMasterParent = new List<DOCMN_LookupMaster>();
            lstDOCMN_LookupMasterChild = new List<DOCMN_LookupMaster>();

        }
        public DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations { get; set; }
        public DOCMN_LookupMasterCorrelations objDOCMN_LookupMasterCorrelations { get; set; }
        public List<DOCMN_LookupMaster> lstDOCMN_LookupMasterParent { get; set; }
        public List<DOCMN_LookupMaster> lstDOCMN_LookupMasterChild { get; set; }
    }
}
