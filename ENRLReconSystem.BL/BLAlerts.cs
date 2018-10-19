using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using ENRLReconSystem.DAL;

namespace ENRLReconSystem.BL
{
    public class BLAlerts
    {
        DALAlerts _objDALAlerts = new DALAlerts();
        ExceptionTypes _retValue;
        public ExceptionTypes SearchAlerts(long? TimeZone,DOADM_AlertDetails objDOADM_AlertDetails, out List<DOADM_AlertDetails> lstDOADM_AlertDetails, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALAlerts.SearchAlerts(TimeZone,objDOADM_AlertDetails, out lstDOADM_AlertDetails, out errorMessage);
        }

        public ExceptionTypes SaveAlert(DOADM_AlertDetails objDOADM_AlertDetails, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALAlerts.SaveAlert(objDOADM_AlertDetails, out errorMessage);
        }
    }
}
