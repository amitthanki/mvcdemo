using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class DOADM_UserMaster
    {
        public DOADM_UserMaster()
        {
            lstLocation = new List<DOCMN_LookupMaster>();
            lstTimeZone = new List<DOCMN_LookupMaster>();
            lstState = new List<DOCMN_LookupMaster>();
            lstYesNo = new List<DOCMN_LookupMaster>();
            lstSalutation = new List<DOCMN_LookupMaster>();
            lstManagers = new List<DOADM_UserMaster>();
            lstDOADM_AccessGroupUserCorrelation = new List<DOADM_AccessGroupUserCorrelation>();
        }
        #region public properties

        #region public properties
        [DataMember]
        public long ADM_UserMasterId { get; set; }
        [DataMember]
        public long? LockedByRef { get; set; }
        [DataMember]
        public DateTime? UTCLockedOn { get; set; }
        [DataMember]
        public long Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string SystemFullName { get; set; }
        [DataMember]
        public string MSID { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public long? ManagerId { get; set; }
        [DataMember]
        public long? LocationLkup { get; set; }
        [DataMember]
        public long? NonUserLkup { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime EndDate { get; set; }
        [DataMember]
        public string SpecialistTitle { get; set; }
        [DataMember]
        public string SpecialistPhone { get; set; }
        [DataMember]
        public string SpecialistFax { get; set; }
        [DataMember]
        public string SpecialistHours { get; set; }
        [DataMember]
        public long? SpecialistTimeZone { get; set; }
        [DataMember]
        public string UserAddressLine1 { get; set; }
        [DataMember]
        public string UserAddressLine2 { get; set; }
        [DataMember]
        public string UserCity { get; set; }
        [DataMember]
        public long? UserStateLkup { get; set; }
        [DataMember]
        public string UserZip { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsManager { get; set; }
        [DataMember]
        public DateTime UTCCreatedOn { get; set; }
        [DataMember]
        public long CreatedByRef { get; set; }
        [DataMember]
        public DateTime? UTCLastUpdatedOn { get; set; }
        [DataMember]
        public long? LastUpdatedByRef { get; set; }

        public long? CreatedByRoleLkup { get; set; }
        public long? UpdatedByRoleLkup { get; set; }
        public string CreatedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public string LockedByName { get; set; }
        public string ConfirmEmail { get; set; }
        public long Titlelkup { get; set; } //this property is replcaing Title since Title property collides with HTML Title
        public List<DOCMN_LookupMaster> lstLocation { get; set; }
        public List<DOCMN_LookupMaster> lstTimeZone { get; set; }
        public List<DOCMN_LookupMaster> lstState { get; set; }
        public List<DOCMN_LookupMaster> lstYesNo { get; set; }
        public List<DOCMN_LookupMaster> lstSalutation { get; set; }
        public List<DOADM_UserMaster> lstManagers { get; set; }

        #endregion

        public List<DOADM_AccessGroupUserCorrelation> lstDOADM_AccessGroupUserCorrelation { get; set; }

        public List<DOADM_AlertDetails> ADM_AlertDetailss { get; set; }     
        #endregion
    }
}
