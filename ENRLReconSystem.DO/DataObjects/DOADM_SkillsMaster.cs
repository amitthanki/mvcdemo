using System;
using System.Collections.Generic;
using System.Text;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_SkillsMaster
    {


        //Constructor
        public DOADM_SkillsMaster()
        {
            lstDOADM_SkillWorkQueuesCorrelation = new List<DOADM_SkillWorkQueuesCorrelation>();
        }


        #region public properties
        public long ADM_SkillsMasterId { get; set; }
        public long? LockedByRef { get; set; }
        public DateTime? UTCLockedOn { get; set; }
        public string SkillsName { get; set; }
        public long RoleLkup { get; set; }
        public long BusinessSegmentLkup { get; set; }
        public long CMN_DepartmentRef { get; set; }
        public long WorkBasketLkup { get; set; }
        public bool IsActive { get; set; }
        public DateTime UTCCreatedOn { get; set; }
        public long CreatedByRef { get; set; }
        public DateTime UTCLastUpdatedOn { get; set; }
        public long? LastUpdatedByRef { get; set; }
        public String CreatedByName { get; set; }
        public String BusinessSegmentValue { get; set; }
        public string LockedByName { get; set; }
        public string RoleValue { get; set; }
        public string DepartmentName { get; set; }
        public string WorkBasketValue { get; set; }
        public string LastUpdatedByName { get; set; }
        public long DiscrepancyCategoryLkup { get; set; }
        

        public List<DOADM_SkillWorkQueuesCorrelation> lstDOADM_SkillWorkQueuesCorrelation { get; set; }
        #endregion

    }
    [Serializable]
    public class DOADM_SkillMasterExtended : DOADM_SkillsMaster
    {
        public List<DOCMN_LookupMaster> lstRoles{ get; set; }
        public List<DOCMN_LookupMaster> lstBusinessSegment { get; set; }
        public List<DOCMN_Department> lstDepartment { get; set; }
        public List<DOCMN_LookupMaster> lstWorkBasket { get; set; }
        public List<DOADM_SkillsMaster> lstSkillsMaster { get; set; }

    }
}
