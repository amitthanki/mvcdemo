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
    public class BLLookup
    {
        ExceptionTypes retValue;
        private ExceptionTypes _retValue;
        private DALLookup _objDALLookup = new DALLookup();

        public ExceptionTypes GetAllLookups(long? lookupTypeId, out List<DOCMN_LookupMaster> lstDOCMN_LookupMaster)
        {
            retValue = new ExceptionTypes();
            DALLookup objDALLookup = new DALLookup();
            List<DOCMN_LookupType> lstLookupType;
            return retValue = objDALLookup.GetAllLookups(lookupTypeId, out lstLookupType, out lstDOCMN_LookupMaster);
        }
        public ExceptionTypes GetAllLookups(long? lookupTypeId, out List<DOCMN_LookupType> lstLookupType, out List<DOCMN_LookupMaster> lstDOCMN_LookupMaster)
        {
            retValue = new ExceptionTypes();
            DALLookup objDALLookup = new DALLookup();
            return retValue = objDALLookup.GetAllLookups(lookupTypeId, out lstLookupType, out lstDOCMN_LookupMaster);
        }

        public ExceptionTypes GetAllLookupTypes(long? TimeZone,string strDescription, bool isActive, out List<DOCMN_LookupType> lstDOCMN_LookupType)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookup.GetAllLookupTypes(TimeZone,strDescription, isActive, out lstDOCMN_LookupType);
        }
        public ExceptionTypes GetLookupMasterByLkupTypeID(long? lookupTypeId, out DOCMN_LookupType objDOCMN_LookupType)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookup.GetLookupMasterByLkupTypeID(lookupTypeId, out objDOCMN_LookupType);
        }
        public ExceptionTypes SaveLookupType(DOCMN_LookupType objDOCMN_LookupType, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookup.SaveLookupType(objDOCMN_LookupType, out errorMessage);
        }

        public ExceptionTypes SaveLookupMaster(DOCMN_LookupMaster objDOCMN_LookupMaster, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookup.SaveLookupMaster(objDOCMN_LookupMaster, out errorMessage);
        }
    }
}
