using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;

namespace ENRLReconSystem.BL
{
    public class BLConfigurations
    {
        public ExceptionTypes retValue;
        // save Configuration master
        public ExceptionTypes SaveConfigMaster(DOMGR_ConfigMaster objDOMGR_ConfigMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALConfigurations objDALConfigurations = new DALConfigurations();
            return retValue = objDALConfigurations.SaveConfigMaster(objDOMGR_ConfigMaster, out errorMessage);
        }

        //Search Configuration by Config Name and IS Active
        public ExceptionTypes SearchConfiguration(long? TimeZone,DOMGR_ConfigMaster objDOMGR_ConfigMaster, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALConfigurations objDALConfigurations = new DALConfigurations();
            return retValue = objDALConfigurations.SearchConfiguration(TimeZone, objDOMGR_ConfigMaster, out lstDOMGR_ConfigMaster, out errorMessage);
        }
        //Search Configuration by config ID
        public ExceptionTypes SearchConfigId(long? TimeZone,DOMGR_ConfigMaster configurationMst, out List<DOMGR_ConfigMaster> lstDOMGR_ConfigMaster, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALConfigurations objDALConfigurations = new DALConfigurations();
            return retValue = objDALConfigurations.SearchCOnfigurationID(TimeZone,configurationMst, out lstDOMGR_ConfigMaster, out errorMessage);
        }
    }
}
