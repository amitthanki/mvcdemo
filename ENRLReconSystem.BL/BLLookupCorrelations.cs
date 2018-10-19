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
    public class BLLookupCorrelations
    {
        private ExceptionTypes _retValue;
        private DALLookupCorrelations _objDALLookupCorrelations = new DALLookupCorrelations();

        public ExceptionTypes GetAllLookupTypeCorrelations(long? TimeZone,DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.GetAllLookupTypeCorrelations(TimeZone,objDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupTypeCorrelations,out errorMessage);
        }

        public ExceptionTypes GetAllLookupTypeCorrelations(long? TimeZone,DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out List<DOCMN_LookupTypeCorrelations> lstDOCMN_LookupTypeCorrelations,
            out List<DOCMN_LookupMasterCorrelations> lstDOCMN_LookupMasterCorrelations,out string  errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.GetAllLookupTypeCorrelations(TimeZone,objDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupTypeCorrelations, out lstDOCMN_LookupMasterCorrelations,out errorMessage);
        }

        public ExceptionTypes GetLookupCorelationByID(long lookupTypeCorrelationsId, out DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations,out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.GetLookupCorelationByID(lookupTypeCorrelationsId, out objDOCMN_LookupTypeCorrelations,out errorMessage);
        }



        public ExceptionTypes GetCorrelationMasterByID(long lkupCorelationTypeID, long lkupCorelationMasterID, out DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended,out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.GetCorrelationMasterByID(lkupCorelationTypeID, lkupCorelationMasterID, out objDOCMN_LookupMasterCorrelationsExtended,out errorMessage);
        }
        public ExceptionTypes SaveLookupTypeCorrelation(DOCMN_LookupTypeCorrelations objDOCMN_LookupTypeCorrelations, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.SaveLookupTypeCorrelation(objDOCMN_LookupTypeCorrelations, out errorMessage);
        }

        public ExceptionTypes SaveCorrelationMaster(DOCMN_LookupMasterCorrelationsExtended objDOCMN_LookupMasterCorrelationsExtended, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALLookupCorrelations.SaveCorrelationMaster(objDOCMN_LookupMasterCorrelationsExtended, out errorMessage);
        }
    }


}
