using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class UIUserSearch
    {
        public string FullName { get; set; }
        public string MSID { get; set; }
        public string Email { get; set; }
        public bool? IsActive { get; set; }
        public List<DOADM_UserMaster> lstDOADM_UserMaster { get; set; }
        public List<DOCMN_LookupMaster> lstYesNo { get; set; }
    }
}
