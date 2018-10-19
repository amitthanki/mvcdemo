using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENRLReconSystem.DO;
using ENRLReconSystem.Utility;
using System.Data;
using System.Data.SqlClient;
using ENRLReconSystem.BL;
using System.Configuration;
using System.Reflection;

namespace ERSBackgroundProcess
{
    public class StartBackgroundProcess
    {
        string errorMessage = string.Empty;
        BLCommon _objCommon = new BLCommon();

        public static long CurrentMasterUserId { get; set; }

        public void StartProcess(long processType, string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                UIUserLogin userLoginDetails;

                ExceptionTypes retValue = _objCommon.GetCurrentMachineUserId(Environment.MachineName, out userLoginDetails, out errorMessage);
                //ExceptionTypes retValue = ExceptionTypes.Success;

                Console.WriteLine("Trying Background Process : " + processType + " With user Id " + userLoginDetails.ADM_UserMasterId);

                if (retValue != ExceptionTypes.Success || !string.IsNullOrEmpty(errorMessage))
                {
                    BLCommon.LogError(2, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BackgroundProcess, (long)ExceptionTypes.Uncategorized, "Not able to get running user/server id.", errorMessage);
                    return;
                }

                CurrentMasterUserId = userLoginDetails.ADM_UserMasterId;

                FDRSubmission objFDRSubmmision;
                FDRResponseProcessing objFDRUpload;
                MoveQueue objMoveQueue;
               
                switch (processType)
                {
                    case (long)BackgroundProcessType.FDRSubmissionCat2:
                        objFDRSubmmision = new FDRSubmission();
                        objFDRSubmmision.CreateCategory2Submission();
                        break;
                    case (long)BackgroundProcessType.FDRSubmissionCat2CTM:
                        objFDRSubmmision = new FDRSubmission();
                        objFDRSubmmision.CreateCategory2CTMSubmission();
                        break;
                    case (long)BackgroundProcessType.FDRSubmissionCat3:
                        objFDRSubmmision = new FDRSubmission();
                        objFDRSubmmision.CreateCategory3Submission();
                        break;
                    case (long)BackgroundProcessType.FDRResubmission:
                        objFDRSubmmision = new FDRSubmission();
                        objFDRSubmmision.CreateReSubmission();
                        break;
                    case (long)BackgroundProcessType.FDRResponseProcessing:
                        objFDRUpload = new FDRResponseProcessing();
                        objFDRUpload.StartFDRResponseProcessing();
                        break;
                    case (long)BackgroundProcessType.FDRSubmissionSCC:
                        objFDRSubmmision = new FDRSubmission();
                        objFDRSubmmision.CreateSCCFDRSubmission();
                        break;
                    case (long)BackgroundProcessType.SendOOALetter:
                        SendOOALetter objSendOOALetter = new SendOOALetter();
                        objSendOOALetter.ProcessOOALetter();
                        break;
                    case (long)BackgroundProcessType.MQReadQueuesandTopics:
                        MQReadQueuesandTopics objMQReadQueuesandTopics = new MQReadQueuesandTopics();
                        BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Starting Process - Set Current Batch Status", "Logger for debug");
                        objMQReadQueuesandTopics.SetCurrentBatchStatus();
                        BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Starting Process - GET MQ Messages", "Logger for debug");
                        long savedMessagesBGP = objMQReadQueuesandTopics.GetMQMessages();
                        //long savedMessagesBGP = objMQReadQueuesandTopics.TestQueueParseAndSave();
                        BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Starting Process - Update Case details", "Logger for debug");
                        objMQReadQueuesandTopics.UpdatCaseDetails(savedMessagesBGP);
                        BLCommon.LogError(0, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Completed Process.", "Logger for debug");
                        objMQReadQueuesandTopics.SetCurrentBatchStatus();
                        break;
                    case 7777777:
                        MaskPhiData objMaskPhiData = new MaskPhiData();
                        objMaskPhiData.MaskPHIData();
                        //objMaskPhiData.updateMBIforHICN();
                        break;
                    case (long)BackgroundProcessType.CreateCMSTransaction:
                        CreateCMSTransaction objCreateCMSTransaction = new CreateCMSTransaction();
                        objCreateCMSTransaction.ProcessCMSTransaction();
                        break;
                    case (long)BackgroundProcessType.PendFTTToAddScrub:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessPendFTTToAddScrub();
                        break;
                    case (long)BackgroundProcessType.PendFTTToMARxAdd:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessPendFTTToMARxAdd();
                        break;
                    case (long)BackgroundProcessType.PendFTTToOpenDisEnroll:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessPendFTTToOpenDisEnroll();
                        break;
                    case (long)BackgroundProcessType.PendNOTToOpenNOT:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessPendNOTToOpenNOT();
                        break;
                    case (long)BackgroundProcessType.AutoUnlockRecords:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.UnlockRecords();
                        break;
                    case (long)BackgroundProcessType.EGHPExclusion:
                        OOAEGHPExclusion _OOAEGHPExclusion = new OOAEGHPExclusion();
                        _OOAEGHPExclusion.StartEGHPExcelProcess();
                        break;
                    case (long)BackgroundProcessType.MoveNOTMacro:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessUpdateMacroQueue((long)MacroType.NOTMacro);
                        break;
                    case (long)BackgroundProcessType.MoveFTTMacro:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessUpdateMacroQueue((long)MacroType.FTTMacro);
                        break;
                    case (long)BackgroundProcessType.MoveTRC155Macro:
                        objMoveQueue = new MoveQueue();
                        objMoveQueue.ProcessUpdateMacroQueue((long)MacroType.TRC155Macro);
                        break;

                    default: string s = string.Empty; break;
                }
            }
            catch (Exception ex)
            {
                BLCommon.LogError(CurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BackgroundProcess, (long)ExceptionTypes.Uncategorized, "Exception while BG Process", ex.StackTrace.ToString());
            }
        }
    }
}
