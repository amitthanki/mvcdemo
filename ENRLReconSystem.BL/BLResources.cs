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
    public class BLResources
    {
        DALResources _objDALResources = new DALResources();
        ExceptionTypes _retValue;

        public ExceptionTypes SearchResources(long? TimeZone,DOADM_ResourceDetails objDOADM_ResourceDetails, out List<DOADM_ResourceDetails> lstDOADM_ResourceDetails, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALResources.SearchResources(TimeZone,objDOADM_ResourceDetails, out lstDOADM_ResourceDetails, out errorMessage);
        }

        public ExceptionTypes SaveResource(DOADM_ResourceDetails objDOADM_ResourceDetails, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALResources.SaveResource(objDOADM_ResourceDetails,  out errorMessage);
        }
    }
}
