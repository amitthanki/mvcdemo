using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIUserLogin
    {
        public UIUserLogin()
        {
            ADM_UserMasterId = 0;
        }

        [DataMember]
        public long UserSessionId
        {
            get;
            set;
        }
        [DataMember]
        public long ADM_UserMasterId { get;set;}
        [DataMember]
        public string FullName { get;set;}
        [DataMember]
        public string MSID { get;set;}
        [DataMember]
        public string Email { get;set;}
        [DataMember]
        public long ManagerId { get;set;}
        [DataMember]
        public long LocationLkup { get;set;}
        [DataMember]
        public DateTime StartDate { get;set;}
        [DataMember]
        public DateTime EndDate { get;set;}
        [DataMember]
        public bool IsAdminUser { get; set; }
        [DataMember]
        public bool IsAdmOSTUser { get; set; }
        [DataMember]
        public bool IsAdmEligUser { get; set; }
        [DataMember]
        public bool IsAdmRPRUser { get; set; }
        [DataMember]
        public bool IsMgrOSTUser { get; set; }
        [DataMember]
        public bool IsMgrEligUser { get; set; }
        [DataMember]
        public bool IsMgrRPRUser { get; set; }
        [DataMember]
        public bool IsPrcrOSTUser { get; set; }
        [DataMember]
        public bool IsPrcrEligUser { get; set; }
        [DataMember]
        public bool IsPrcrRPRUser { get; set; }
        [DataMember]
        public bool IsVwrOSTUser { get; set; }
        [DataMember]
        public bool IsVwrEligUser { get; set; }
        [DataMember]
        public bool IsVwrRPRUser { get; set; }
        [DataMember]
        public bool IsWebServiceUser { get; set; }
        [DataMember]
        public bool IsMacroServiceUser { get; set; }
        [DataMember]
        public bool IsRestrictedUser { get; set; }

        //Properties to decide menu Visibility 
        [DataMember]
        public bool IsOOAMenuVisible { get; set; }
        [DataMember]
        public bool IsSCCMenuVisible { get; set; }
        [DataMember]
        public bool IsTRRMenuVisible { get; set; }
        [DataMember]
        public bool IsMMREligibilityMenuVisible { get; set; }
        [DataMember]
        public bool IsDOBMenuVisible { get; set; }
        [DataMember]
        public bool IsGENDERMenuVisible { get; set; }
        [DataMember]
        public bool IsRPRMenuVisible { get; set; }

        [DataMember]
        public bool OSTOOACanCreate { get; set; }
        [DataMember]
        public bool OSTSCCCanCreate { get; set; }
        [DataMember]
        public bool OSTTRRCanCreate { get; set; }
        [DataMember]
        public bool EligibilityMMRCanCreate { get; set; }
        [DataMember]
        public bool EligibilityDOBCanCreate { get; set; }
        [DataMember]
        public bool EligibilityGenderCanCreate { get; set; }
        [DataMember]
        public bool RPRCanCreate { get; set; }

        [DataMember]
        public bool OSTOOACanSearch { get; set; }
        [DataMember]
        public bool OSTSCCCanSearch { get; set; }
        [DataMember]
        public bool OSTTRRCanSearch { get; set; }
        [DataMember]
        public bool EligibilityMMRCanSearch { get; set; }
        [DataMember]
        public bool EligibilityDOBCanSearch { get; set; }
        [DataMember]
        public bool EligibilityGenderCanSearch { get; set; }
        [DataMember]
        public bool RPRCanCSearch { get; set; }

        public bool CanMassReassign { get; set; }
        [DataMember]
        public bool CanMassUnlock { get; set; }
        [DataMember]
        public bool CanMassUpdate { get; set; }
        [DataMember]
        public bool CanMassUpload { get; set; }


        //Properties used to get user selected Values from login page drop down      
        public long? BusinessSegmentLkup { get; set; }
        [DataMember]
        public long? WorkBasketLkup { get; set; }
        [DataMember]
        public long? RoleLkup { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public bool IsAuthorizedUser { get; set; }
        public List<UserSkills> UserSkills { get;set;}
        public List<UserWorkQueues> UserQueueList { get;set;}
        public DOADM_UserPreference ADM_UserPreference { get; set; }

        public List<DOADM_AccessGroupMaster> UserAccessGroup { get; set; }

        public List<DORPT_ReportsMaster> UserReports { get; set; }
        public List<UserSkills> Correlations { get; set; }//to save all the business,workbasket and role correlations for current user
        public List<DOCMN_LookupMaster> LookUps { get; set; }//to save all the lookup values for correlations
    }
    [Serializable]
    public class UserSkills
    {
        [DataMember]
        public string SkillsName { get;set;}
        [DataMember]
        public long DiscrepancyCategoryLkup { get; set; }
        [DataMember]
        public long RoleLkup { get;set;}
        [DataMember]
        public long BusinessSegmentLkup { get;set;}
        [DataMember]
        public long WorkBasketLkup { get;set;}
        [DataMember]
        public long WorkQueuesLkup { get; set; }
        [DataMember]
        public long CMN_DepartmentRef { get;set;}
        [DataMember]
        public bool CanCreate { get;set;}
        [DataMember]
        public bool CanModify { get;set;}
        [DataMember]
        public bool CanSearch { get;set;}
        [DataMember]
        public bool CanView { get;set;}
        [DataMember]
        public bool CanMassUpdate { get;set;}
        [DataMember]
        public bool CanHistory { get;set;}
        [DataMember]
        public bool CanReassign { get;set;}
        [DataMember]
        public bool CanUnlock { get;set;}
        [DataMember]
        public bool CanUpload { get;set;}
        [DataMember]
        public bool CanClone { get; set; }
        [DataMember]
        public bool CanReopen { get; set; }
    }
    [Serializable]
    public class UserWorkQueues
    {
        [DataMember]
        public long QueueLkp { get;set;}
        [DataMember]
        public string QueueName { get;set;}
    }
}