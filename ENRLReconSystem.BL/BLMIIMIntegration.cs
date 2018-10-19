using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.DO.DataObjects;
using ENRLReconSystem.DAL;
using ENRLReconSystem.Utility;
using System.Data;

namespace ENRLReconSystem.BL
{
    public class BLMIIMIntegration
    {

        public List<DOMIIMGetQueue> GetQueueDetailsByHICN(string memberHICN)
        {
            DALMIIMIntegration objDALMIIMIntegration = new DALMIIMIntegration();
            ExceptionTypes result = objDALMIIMIntegration.GetMIIMQueueDetailsByHICN(memberHICN, out List<DOMIIMGetQueue> Queues);
            return Queues;
        }

        public ExceptionTypes GetQueueIdFromMIIMRefernceId(string strMIIMReferenceId, out string strErsCaseId, out string errorMessage)
        {
            DALMIIMIntegration objDALMIIMIntegration = new DALMIIMIntegration();
            ExceptionTypes retValue = new ExceptionTypes();
            return retValue = objDALMIIMIntegration.GetQueueIdFromMIIMRefernceId(strMIIMReferenceId, out strErsCaseId, out errorMessage);
        }

        public ExceptionTypes GetCaseDiscrepancyCategory(long lQueueID, out long lDiscrepancyCategory, out string errorMessage)
        {
            DALMIIMIntegration objDALMIIMIntegration = new DALMIIMIntegration();
            ExceptionTypes retValue = new ExceptionTypes();
            return retValue = objDALMIIMIntegration.GetCaseDiscrepancyCategory(lQueueID, out lDiscrepancyCategory, out errorMessage);
        }

        public long UpdateOOAMIIMComments(List<DOMIIMOOACommentUpdate> lstDOMIIMOOACommentUpdate, long userid)
        {
            DALMIIMIntegration objDALMIIMIntegration = new DALMIIMIntegration();
            ExceptionTypes result = new ExceptionTypes();
            result = objDALMIIMIntegration.UpdateOOAMIIMComments(lstDOMIIMOOACommentUpdate, userid);
            return (long)result;
        }
    }
}
