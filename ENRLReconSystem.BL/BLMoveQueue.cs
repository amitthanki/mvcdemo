using System;
using ENRLReconSystem.DAL;
using ENRLReconSystem.Utility;


namespace ENRLReconSystem.BL
{
    public class BLMoveQueue
    {
        DAMoveQueue _objDAMoveQueue = new DAMoveQueue();
        public BLMoveQueue()
        {

        }
        public ExceptionTypes BProcessMoveQueue(string executeSp,out string errorMessage)
        {
            errorMessage = string.Empty;
            return _objDAMoveQueue.DProcessMoveQueue(executeSp,out errorMessage);
        }

        public ExceptionTypes BProcessQueueMoveforMacro(long MacroType, long LoginUserID, string constSPName, out string errorMessage)
        {
            errorMessage = string.Empty;
            return _objDAMoveQueue.DProcessQueueMoveforMacro(MacroType,LoginUserID,constSPName, out errorMessage);
        }

        public bool UnlockRecords(out string errorMessage)
        {
            errorMessage = string.Empty;
            return _objDAMoveQueue.UnlockRecords(out errorMessage);
        }
    }
}
