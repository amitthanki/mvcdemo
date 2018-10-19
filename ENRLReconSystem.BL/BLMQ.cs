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
    public class BLMQ
    {
        DALMQ _objDALMQ = new DALMQ();
        ExceptionTypes _retValue;

        public ExceptionTypes InsertMQTRRRecord(DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, long CurrentMasterUserId, out string errorMessage)
        {
            if (objDOMQTRRWorkQueueItems.LOB == "PDP")
                objDOMQTRRWorkQueueItems.DisenrollementPeriod = 12;
            else
                objDOMQTRRWorkQueueItems.DisenrollementPeriod = 6;
            return _retValue = _objDALMQ.InsertMQTRRRecord(objDOMQTRRWorkQueueItems, CurrentMasterUserId, out errorMessage);
        }

        public ExceptionTypes SaveXMLMessage(MQMessagesRecieved objMQMessagesRecieved, out string errorMessage)
        {
            return _retValue = _objDALMQ.SaveXMLMessage(objMQMessagesRecieved, out errorMessage);
        }

        public ExceptionTypes UpdateXMLMessage(MQMessagesRecieved objMQMessagesRecieved, out string errorMessage)
        {
            return _retValue = _objDALMQ.UpdateXMLMessage(objMQMessagesRecieved, out errorMessage);
        }

        public ExceptionTypes UpdatMQTRRCaseDetails(long lMQTRRWorkQueueItemId, long lMQSourceTypeLkup, long lUpdateCMSTransactionCaseNumber, long CurrentUserId, out string errorMessage)
        {
            if (lMQSourceTypeLkup == (long)MQSourceTypeLkup.Queue)
                lUpdateCMSTransactionCaseNumber = 0;
            return _retValue = _objDALMQ.UpdatMQTRRCaseDetails(lMQTRRWorkQueueItemId, lMQSourceTypeLkup, lUpdateCMSTransactionCaseNumber, CurrentUserId, out errorMessage);
        }

        public ExceptionTypes MQTRRRecordsToProcess(long savedMessagesBGPId, out List<DOMQTRRWorkQueueItems> lstDOMQTRRWorkQueueItems, out string errorMessage)
        {
            return _retValue = _objDALMQ.MQTRRRecordsToProcess(savedMessagesBGPId, out lstDOMQTRRWorkQueueItems, out errorMessage);
        }

        public ExceptionTypes SetCurrentBatchStatus(out string errorMessage)
        {
            return _retValue = _objDALMQ.SetCurrentBatchStatus(out errorMessage);
        }

        public ExceptionTypes GetNationalEmployerGroups(out List<DOMQTRRWorkQueueItems> lstEmployerNationalGroup, out string errorMessage)
        {
            return _objDALMQ.GetNationalEmployerGroups(out lstEmployerNationalGroup, out errorMessage);
        }
    }
}
