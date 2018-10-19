using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIDOAccessGroup : DOADM_AccessGroupMaster
    {
        public UIDOAccessGroup()
        {
            lstDOADM_AccessGroupSkillsCorrelation = new List<DOADM_AccessGroupSkillsCorrelation>();
            lstDOADM_AccessGroupReportCorrelation = new List<DOADM_AccessGroupReportCorrelation>();
        }

        public long DescripancyCatLkup { get; set; }       
        public List<DOADM_AccessGroupSkillsCorrelation> lstDOADM_AccessGroupSkillsCorrelation { get; set; }
        public List<DOADM_AccessGroupReportCorrelation> lstDOADM_AccessGroupReportCorrelation { get; set; }
        //public List<UIDOAccessGroupSkills> lstUIDOAccessGroupSkills { get; set; }
        //public List<UIDOAccessGroupReport> lstUIDOAccessGroupReport { get; set; }

        public List<DOADM_SkillsMaster> lstSkills { get; set; }

        public List<DORPT_ReportsMaster> lstReports { get; set; }
    }

    [Serializable]
    public class UIDOAccessGroupReport
    {
        public long ADM_AccessGroupReportCorrelationId { get; set; }

        public long ADM_AccessGroupMasterRef { get; set; }

        public long RPT_ReportsMasterRef { get; set; }
        public long RPT_ReportsMasterId { get; set; }
        public string ReportName { get; set; }
        public string ReportServer { get; set; }
        public string ReportURL { get; set; }
        public string ReportsCategory { get; set; }
        public string LastUpdatedByName { get; set; }
        public long LastUpdatedByRef { get; set; }
        public bool IsActive { get; set; }
    }

    [Serializable]
    public class UIDOAccessGroupSkills
    {
        public long ADM_AccessGroupSkillsCorrelationId { get; set; }
        public long ADM_AccessGroupMasterRef { get; set; }
        public long ADM_SkillsMasterRef { get; set; }
        public string SkillsName { get; set; }
        public bool CanCreate { get; set; }
        public bool CanSearch { get; set; }
        public bool CanProcess { get; set; }
        public bool CanReassign { get; set; }
        public bool CanUnlock { get; set; }
        public bool CanHistory { get; set; }
        public bool CanModify { get; set; }
        public bool CanView { get; set; }
        public bool CanMassUpdate { get; set; }
        public bool CanUpload { get; set; }
        public bool IsActive { get; set; }
        public bool CanReopen { get; set; }
    }
}
