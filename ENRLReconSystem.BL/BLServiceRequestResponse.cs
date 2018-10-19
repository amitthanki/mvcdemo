using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DAL;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data;
using System.Reflection;

namespace ENRLReconSystem.BL
{
    public class BLServiceRequestResponse
    {
        DALServiceRequestResponse _objDALServiceRequestResponse = new DALServiceRequestResponse();
        ExceptionTypes _retValue;
        public ExceptionTypes InsertAEGPSServiceTrace(DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
        }
        public ExceptionTypes InsertMacroServiceTrace(DOGEN_MacroServiceTrace objDOGEN_MacroServiceTrace)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALServiceRequestResponse.InsertMacroServiceTrace(objDOGEN_MacroServiceTrace);
        }
        public ExceptionTypes MIIMServiceLog(DOGEN_MIIMServiceTrace objDOGEN_MIIMServiceTrace)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALServiceRequestResponse.MIIMServiceLog(objDOGEN_MIIMServiceTrace);
        }
    }
}