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
    public class BLEligibility
    {
        public ExceptionTypes retValue;
        private DALEligibility objDALEligibility = new DALEligibility();
        public ExceptionTypes CreateEligibilityCase(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            retValue = new ExceptionTypes();          
            return retValue = objDALEligibility.CreateCase(objDOGEN_Queue, out errorMessage);
        }

        public ExceptionTypes GetGenQueueByID(long? TimeZone, long genQueueID, out DOGEN_Queue objDOGEN_Queue)
        {
            retValue = new ExceptionTypes();
            return retValue = objDALEligibility.GetGenQueueByID(TimeZone,genQueueID, out objDOGEN_Queue);
        }

        public ExceptionTypes SaveAction(DOGEN_EligibilityActions objDOGEN_EligibilityActions, out string errorMessage)
        {
            retValue = new ExceptionTypes();           
            return retValue = objDALEligibility.SaveAction(objDOGEN_EligibilityActions, out errorMessage);
        }       
    }
}
