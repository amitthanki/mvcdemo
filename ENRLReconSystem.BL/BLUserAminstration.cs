using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.BL
{
    public class BLUserAminstration
    {
        public ExceptionTypes retValue;
        public ExceptionTypes GetAllLookups(long? lookupTypeId, out List<DOCMN_LookupMaster> lstDOCMN_LookupMaster)
        {
            retValue = new ExceptionTypes();
            LookupDAL objLookupDAL = new LookupDAL();
            return retValue = objLookupDAL.GetAllLookups(lookupTypeId, out lstDOCMN_LookupMaster);
        }

        public ExceptionTypes GetUserBasedOnMSID(string MSID,out DOADM_UserMaster objDOADM_UserMaster)
        {
            retValue = new ExceptionTypes();
            UserAdministrationDAL objUserAdministrationDAL = new UserAdministrationDAL();
            return retValue = objUserAdministrationDAL.GetUserBasedOnMSID(MSID, out objDOADM_UserMaster);
        }
    }
}
