using ENRLReconSystem.BL;
using ENRLReconSystem.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERSBackgroundProcess
{
    public class MoveQueue
    {
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        BLMoveQueue _objBLMoveQueue = new BLMoveQueue();
        
        public MoveQueue()
        {

        }

        internal bool ProcessPendFTTToAddScrub()
        {
            bool isSuccess = false;
            string errorMessage = string.Empty;
            try
            {
                if (ProcessQueueMove(ConstantTexts.SP_APP_UPD_HoldingQueues_PendingFTT_AddressScrubLetter, out errorMessage) != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", errorMessage);
                }
                else {
                    isSuccess = true;
                }
               
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", ex.StackTrace.ToString());
                return isSuccess;
            }
            return isSuccess;

        }

        internal bool ProcessPendNOTToOpenNOT()
        {
            bool isSuccess = false;
            string errorMessage = string.Empty;
            try
            {
                if (ProcessQueueMove(ConstantTexts.SP_APP_UPD_HoldingQueues_PendingNOT_OpenNOT, out errorMessage) != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", errorMessage);
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving Pending NOT T0 Open NOT", ex.StackTrace.ToString());
                return isSuccess;
            }
            return isSuccess;
        }
        internal bool ProcessPendFTTToOpenDisEnroll()
        {
            bool isSuccess = false;
            string errorMessage = string.Empty;
            try
            {
                if (ProcessQueueMove(ConstantTexts.SP_APP_UPD_HoldingQueues_PendingFTT_OpenDisEnroll, out errorMessage) != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", errorMessage);
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving Pending FTT To Open DisEnroll", ex.StackTrace.ToString());
                return isSuccess;
            }
            return isSuccess;
        }
        internal bool ProcessPendFTTToMARxAdd()
        {
            bool isSuccess = false;
            string errorMessage = string.Empty;
            try
            {
                if (ProcessQueueMove(ConstantTexts.SP_APP_UPD_HoldingQueues_PendingFTT_MARxAddressLetter, out errorMessage) != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", errorMessage);
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving Pending FTT To MARx Address", ex.StackTrace.ToString());
                return isSuccess;
            }
            return isSuccess;
        }

        internal void UnlockRecords()
        {
            string errorMessage = string.Empty;
            errorMessage = string.Empty;
            try
            {
                _objBLMoveQueue.UnlockRecords(out errorMessage);
                if(!errorMessage.IsNullOrEmpty())
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, "Error Auto Unlocking records : " , errorMessage);
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.Unlock, (long)ExceptionTypes.Uncategorized, "Exception Auto Unlocking records : " + errorMessage, ex.StackTrace.ToString());
            }
        }

        internal bool ProcessUpdateMacroQueue(long MacroTypeLkup)
        {
            bool isSuccess = false;          
            string errorMessage = string.Empty;
            try
            {
                if (ProcessQueueMoveforMacro(MacroTypeLkup, _lCurrentMasterUserId, ConstantTexts.SP_USP_APP_UPD_MacroUpdate, out errorMessage) != ExceptionTypes.Success)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving QueuePending FTT To AddScrub", errorMessage);
                }
                else
                {
                    isSuccess = true;
                }
            }
            catch (Exception ex)
            {

                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPFDRSubmission, (long)ExceptionTypes.Uncategorized, "Exception Start Moving Pending FTT To MARx Address", ex.StackTrace.ToString());
                return isSuccess;
            }
            return isSuccess;
        }

        private ExceptionTypes ProcessQueueMove(string constSPName,out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _objBLMoveQueue.BProcessMoveQueue(constSPName, out errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ExceptionTypes ProcessQueueMoveforMacro(long MacroType, long LoginUserID, string constSPName,out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                return _objBLMoveQueue.BProcessQueueMoveforMacro(MacroType,LoginUserID,constSPName, out errorMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
