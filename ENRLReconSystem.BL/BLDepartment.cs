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
    public class BLDepartment
    {
        public ExceptionTypes retValue;       
        public ExceptionTypes SaveDepartment(DOCMN_Department objDOCMN_Department, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALDepartment objDALDepartment = new DALDepartment();
            return retValue = objDALDepartment.SaveDepartment(objDOCMN_Department, out errorMessage);
        }
        //Search Department by Department Name and IS Active
        public ExceptionTypes SearchDepartment(long? TimeZone,DOCMN_Department objDOCMN_Department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALDepartment objDALDepartment = new DALDepartment();
            return retValue = objDALDepartment.SearchDepartment(TimeZone,objDOCMN_Department, out lstDOCMN_Department, out errorMessage);
        }
        //Search Department by Department ID
        public ExceptionTypes SearchDepartmentById(long? TimeZone,DOCMN_Department department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALDepartment objDALDepartment = new DALDepartment();
            return retValue = objDALDepartment.SearchDepartmentById(TimeZone, department, out lstDOCMN_Department, out errorMessage);
        }

        //Search Duplicate Department
        public ExceptionTypes CheckDuplicateDep(long? TimeZone,DOCMN_Department objDOCMN_Department, out List<DOCMN_Department> lstDOCMN_Department, out string errorMessage)
        {
            retValue = new ExceptionTypes();
            DALDepartment objDALDepartment = new DALDepartment();
            return retValue = objDALDepartment.CheckDuplicateDep(TimeZone, objDOCMN_Department, out lstDOCMN_Department, out errorMessage);
        }
    }
}
