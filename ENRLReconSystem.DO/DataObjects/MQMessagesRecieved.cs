using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENRLReconSystem.DO
{
    [Serializable]
    public class MQMessagesRecieved
    {
        public long MQMessagesRecievedId {get; set;}
        public long CMN_BackgroundProcessMasterRef { get; set; }
        public long MQSourceTypeLkup { get; set; }
        public string MQMessage { get; set; }
        public bool IsProcessed { get; set; }
        public string ProcessedResult { get; set; }
        public long MQTRRWorkQueueItemRef { get; set; }
        public string ProcessingFailReason { get; set; }
        public long SystemId { get; set; }
    }
}
