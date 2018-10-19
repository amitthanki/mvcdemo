using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.BL
{
    public class BLAccessGroup
    {
        ExceptionTypes retVal;
        public ExceptionTypes GetAccessGroupBasedOnSearch(long? TimeZone,DOADM_AccessGroupMaster objDOADM_AccessGroupMaster,out List<DOADM_AccessGroupMaster> lstDOADM_AccessGroupMaster)
        {
            DALAccessGroup objDALAccessGroup = new DALAccessGroup();
            return retVal = objDALAccessGroup.GetAccessGroupBasedOnSearch(TimeZone,objDOADM_AccessGroupMaster, out lstDOADM_AccessGroupMaster);
        }

        public ExceptionTypes AddEditAccessGroup(long lLoggedInUserId,UIDOAccessGroup objUIDOAccessGroup, out string errorMessage)
        {
            DALAccessGroup objDALAccessGroup = new DALAccessGroup();
            return retVal = objDALAccessGroup.AddEditAccessGroup(lLoggedInUserId, objUIDOAccessGroup,out errorMessage);
        }

        public ExceptionTypes GetAccessGroupForEdit(DOADM_AccessGroupMaster objDOADM_AccessGroupMaster, out UIDOAccessGroup objUIDOAccessGroup)
        {
            DALAccessGroup objDALAccessGroup = new DALAccessGroup();
            return retVal = objDALAccessGroup.GetAccessGroupForEdit(objDOADM_AccessGroupMaster, out objUIDOAccessGroup);
        }
    }
}
