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
    public class BLUserAdministration
    {
        private ExceptionTypes retValue;
       
        public ExceptionTypes GetUserBasedOnMSID(long? TimeZone,string MSID, out DOADM_UserMaster objDOADM_UserMaster)
        {
            retValue = new ExceptionTypes();
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.GetUserBasedOnMSID(TimeZone, MSID, out objDOADM_UserMaster);
        }

        public ExceptionTypes GetUserAccessPermission(string MSID, long? businessSegmentLkup, long? workBasketLkup, long? roleLkup, out UIUserLogin objUIUserLogin)
        {
            retValue = new ExceptionTypes();
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.GetUserAccessPermission(MSID,null,null,null, out objUIUserLogin);
        }
        
        public ExceptionTypes SaveUser(DOADM_UserMaster objDOADM_UserMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.SaveUser(objDOADM_UserMaster, out errorMessage);
        }

        public ExceptionTypes SaveUserPreference(DOADM_UserPreference objDOADM_UserPreference, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.SaveUserPreference(objDOADM_UserPreference, out errorMessage);
        }

        public ExceptionTypes SearchUser(long? TimeZone,DOADM_UserMaster objDOADM_UserMaster, out List<DOADM_UserMaster> lstDOADM_UserMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.SearchUser(TimeZone,objDOADM_UserMaster, out lstDOADM_UserMaster, out errorMessage);
        }

        public ExceptionTypes LoginUser(string MSID)
        {
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.LoginUser(MSID);
        }

        public ExceptionTypes UserLogout(string MSID)
        {
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.UserLogout(MSID);
        }

        public ExceptionTypes ReassignUserList(long? TimeZone,string Gen_QueueIds, out List<DOADM_UserMaster> lstDOADM_UserMaster,out string errorMessage)
        {
            DALUserAdministration objDALUserAdministration = new DALUserAdministration();
            return retValue = objDALUserAdministration.ReassignUserList(TimeZone, Gen_QueueIds, out lstDOADM_UserMaster, out errorMessage);
        }

    }
}
