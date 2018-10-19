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
    public class BLOST
    {
        public ExceptionTypes _retValue;
        DALOST _objDALOST = new DALOST();

        public ExceptionTypes SaveOST(DOGEN_Queue objDOGEN_Queue, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALOST.SaveOST(objDOGEN_Queue, out errorMessage);
        }

        public ExceptionTypes GetGenQueueByID(long? TimeZone, long genQueueID, out DOGEN_Queue objDOGEN_Queue,out string errorMsg)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALOST.GetGenQueueByID(TimeZone,genQueueID, out objDOGEN_Queue,out errorMsg);
        }

        public ExceptionTypes SaveOSTActions(DOGEN_OSTActions objDOGEN_OSTActions, out string errorMessage)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALOST.SaveOSTActions(objDOGEN_OSTActions, out errorMessage);
        }

        public ExceptionTypes GetQueueSendOOALetter(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMsg)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALOST.GetQueueSendOOALetter(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMsg);
        }

        //
        public ExceptionTypes GetQueueCMSTransaction(StringBuilder strGEN_QueueIdsToSkip, out DOGEN_Queue objDOGEN_Queue, out string errorMsg)
        {
            _retValue = new ExceptionTypes();
            return _retValue = _objDALOST.GetQueueCMSTransaction(strGEN_QueueIdsToSkip, out objDOGEN_Queue, out errorMsg);
        }
    }
}
