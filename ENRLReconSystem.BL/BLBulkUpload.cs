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
   
    public class BLBulkUpload
    {
        ExceptionTypes _exceptionTypes;
        DALBulkUpload objDALBulkUpload = new DALBulkUpload();
        public BLBulkUpload()
        {
        }
        public ExceptionTypes GetBulkImportExcelTemplate(out UIDOGEN_BulkImportExcelTemplate objUIDOGEN_BulkImportExcelTemplate,out string errorMessage)
        {
            _exceptionTypes = new ExceptionTypes();
            return _exceptionTypes = objDALBulkUpload.GetBulkImportExcelTemplate(out objUIDOGEN_BulkImportExcelTemplate, out errorMessage);
        }
        public ExceptionTypes GetBulkUploadSearchResult(long? TimeZone,UIBulkUploadSearch objUIBulkUploadSearch, out List<DOGEN_BulkImport> lstDOGEN_BulkImport, out string errorMessage)
        {
            _exceptionTypes = new ExceptionTypes();
            return _exceptionTypes = objDALBulkUpload.GetBulkUploadSearchResult(TimeZone,objUIBulkUploadSearch,out lstDOGEN_BulkImport, out errorMessage);
        }
        public ExceptionTypes SaveBulkUpload(DOGEN_BulkImport objDOGEN_BulkImport,long loginUserID,out string errorMessage)
        {
            _exceptionTypes = new ExceptionTypes();
            return _exceptionTypes = objDALBulkUpload.SaveBulkUpload(objDOGEN_BulkImport,loginUserID ,out errorMessage);
        }

       
    }
}
