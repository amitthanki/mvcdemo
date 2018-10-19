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
    public class BLReports
    {
        private ExceptionTypes retVal;
        public ExceptionTypes GetAllReports(long? lRptIdout, string sReportName, out List<DORPT_ReportsMaster> lstDORPT_ReportsMaster, out string errorMessage)
        {
            DALReports objDALReports = new DALReports();
            return retVal = objDALReports.GetAllReports(lRptIdout, sReportName, out lstDORPT_ReportsMaster, out errorMessage);
        }
    }
}
