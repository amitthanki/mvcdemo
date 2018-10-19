using System;
using System.Collections.Generic;
using System.Linq;
using IBM.WMQ;
using System.Collections;
using ENRLReconSystem.Utility;
using ENRLReconSystem.BL;
using System.Reflection;
using System.Xml.Linq;
using ENRLReconSystem.DO;
using System.IO;
using System.Net;
using System.Diagnostics;
using ERSBackgroundProcess.srvAEEmployerSummary;

namespace ERSBackgroundProcess
{
    public class MQReadQueuesandTopics
    {
        long _lCurrentMasterUserId = StartBackgroundProcess.CurrentMasterUserId;
        BLCommon objBLCommon = new BLCommon();
        BLMQ objBLMQ = new BLMQ();
        long bgpMasterId = 0;

        //Read app config value for MQ onnection
        string queueManager = string.Empty;
        string host = string.Empty;
        string port = string.Empty;
        string channel = string.Empty;
        string queueName = string.Empty;
        string topicName = string.Empty;
        string subscriptionName = string.Empty;
        string userName = string.Empty;
        string pwd = string.Empty;
        bool IsMQSecureConnection = false;
        bool queueManagerIsConnected = false;
        long _msgCount = 0;
        string _basePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + "\\";
        List<DOMQTRRWorkQueueItems> _lstEmployerNationalGroup = null;

        //MQ objects
        MQQueueManager objMQQueueManager = null;
        MQQueue objMQQueue = null;
        MQMessage objMQMessage = null;
        MQGetMessageOptions objMQGetMessageoption = null;
        MQTopic objMQTopic = null;

        public long GetMQMessages()
        {
            Console.WriteLine("Starting MQ read process.. ");
            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Started GET MQ Messages", "Logger for debug");
            string errorMessage = string.Empty;
            long lQueueMessagesRead = 0;
            long lTopicMessagesRead = 0;
            try
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Entered try block 1", "Logger for debug");
                long lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                //Insert BGP Master Row
                objBLCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.MQReadQueuesandTopics, _lCurrentMasterUserId, out bgpMasterId, out errorMessage);
                try
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Entered try block 2", "Logger for debug");
                    //Read values from config master table
                    _msgCount = Convert.ToInt32(CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQmsgsToRead));
                    queueManager = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.QueueManager);
                    host = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQHost);
                    port = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQPort);
                    queueName = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.GetQueueName);
                    topicName = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.GetTopicName);
                    subscriptionName = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.GetSubscriptionName);
                    IsMQSecureConnection = ExtensionMethods.ToBoolean(CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.IsMQSecureConnection));
                    if (IsMQSecureConnection)
                    {
                        channel = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQSecureChannel);
                        userName = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQSecureChannelUser);
                        pwd = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQSecureChannelPwd);
                        Console.WriteLine("using secure channel.. ");
                    }
                    else
                    {
                        channel = CacheUtility.GetConfigrationValueByConfigName(ConstantTexts.MQNonSecureChannel);
                        Console.WriteLine("Using non secure channel.. ");
                    }
                    //condition to swap between java and .net code to connect to MQ
                    Boolean UseJavaMQClient = AppConfigData.UseJavaMQClient;

                    //get National Employer Group List for National Employees
                    ExceptionTypes result = objBLMQ.GetNationalEmployerGroups(out _lstEmployerNationalGroup, out errorMessage);
                    if (result != ExceptionTypes.Success || result != ExceptionTypes.ZeroRecords)
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, "Error Getting National Employer Group");

                    if (UseJavaMQClient)
                    {
                        Console.WriteLine("will use java client.. ");
                        if (File.Exists(Path.Combine(_basePath, @"JarFileToReadMQ\\ERSMQKEY.jks")) && File.Exists(Path.Combine(_basePath, "JarFileToReadMQ\\ErsMqBgp.jar")))
                        {

                            //creating java command to excecute
                            string strArguments = string.Format(ConstantTexts.MQJavaCmdArguments, Path.Combine(_basePath, @"JarFileToReadMQ\\ERSMQKEY.jks"), Path.Combine(_basePath, "JarFileToReadMQ\\ErsMqBgp.jar"));
                            strArguments = strArguments + " " + host;
                            strArguments = strArguments + " " + port;
                            strArguments = strArguments + " " + channel;
                            strArguments = strArguments + " " + queueManager;
                            strArguments = strArguments + " " + userName;
                            strArguments = strArguments + " " + pwd;
                            strArguments = strArguments + " " + queueName;
                            strArguments = strArguments + " " + _msgCount;
                            strArguments = strArguments + " " + topicName;
                            strArguments = strArguments + " " + subscriptionName;

                            //Console.WriteLine("Jar file arguments : " + strArguments);
                            Console.WriteLine("Will Read and Process " + _msgCount + " Messages");
                            //" quesvc8e5 2246 ERSUAT_SECCLNT WMQS29  ersmqnp  4BgdiVw5 ERS.UAT.OVAWESB_BATCH_DATA  1";

                            //Clean the existing directory for messages
                            if (Directory.Exists(_basePath + "QueueMessages"))
                                Array.ForEach(Directory.GetFiles(_basePath + "QueueMessages"), delegate (string path) { File.Delete(path); });
                            if (Directory.Exists(_basePath + "TopicMessages"))
                                Array.ForEach(Directory.GetFiles(_basePath + "TopicMessages"), delegate (string path) { File.Delete(path); });

                            ProcessStartInfo procStartInfo = new ProcessStartInfo("java", strArguments);

                            // The following commands are needed to redirect the standard output.
                            // This means that it will be redirected to the Process.StandardOutput StreamReader.
                            procStartInfo.RedirectStandardOutput = true;
                            procStartInfo.UseShellExecute = false;
                            // Do not create the black window.
                            procStartInfo.CreateNoWindow = false;
                            // Now we create a process, assign its ProcessStartInfo and start it
                            Process proc = new Process();
                            proc.StartInfo = procStartInfo;
                            proc.Start();

                            Console.WriteLine("Running java client to get messages.....");

                            // Create directory to save java execution result if doesnt exist clean jar reults older than 5 days
                            if (Directory.Exists(_basePath + "JavaExecutionResult"))
                                Array.ForEach(Directory.GetFiles(_basePath + "JavaExecutionResult"), delegate (string path) { if (File.GetCreationTime(path) < DateTime.Today.AddDays(-5)) File.Delete(path); });
                            else
                                Directory.CreateDirectory(_basePath + "JavaExecutionResult");

                            //saving outpup to file
                            File.AppendAllText(_basePath + "JavaExecutionResult\\" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + "jarLog.txt", proc.StandardOutput.ReadToEnd());

                            Console.WriteLine("Running java client finished reading messages.....");

                            List<string> messageFiles = Directory.GetFiles(_basePath + "QueueMessages", "*.xml").ToList();
                            Console.WriteLine("Total Queue messages : " + messageFiles.Count);
                            if (messageFiles.Count > 0)
                            {
                                foreach (var file in messageFiles)
                                {
                                    Console.WriteLine("Processing Queue Message number : " + lQueueMessagesRead + 1.ToInt16());
                                    string xmlMessage = File.ReadAllText(file);
                                    lQueueMessagesRead = ProcessXMLMessage(lQueueMessagesRead, xmlMessage, MQSourceTypeLkup.Queue);
                                }
                            }

                            messageFiles = Directory.GetFiles("TopicMessages").ToList();
                            Console.WriteLine("Total Topic messages : " + messageFiles.Count);
                            foreach (var file in messageFiles)
                            {
                                Console.WriteLine("Processing Topic Message number : " + lTopicMessagesRead + 1.ToInt16());
                                string xmlMessage = File.ReadAllText(file);
                                lTopicMessagesRead = ProcessXMLMessage(lTopicMessagesRead, xmlMessage, MQSourceTypeLkup.Topic);
                            }
                            lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                        }
                        else
                        {
                            Console.WriteLine(@"Missing required JAVA files : - JarFileToReadMQ\ERSMQKEY.jks OR JarFileToReadMQ\ErsMqBgp.jar");
                            lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                            BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Missing Files", @"Missing java files JarFileToReadMQ\ERSMQKEY.jks OR JarFileToReadMQ\ErsMqBgp.jar");
                        }
                    }
                    else
                    {
                        Console.WriteLine("will use .Net client.. ");
                        Hashtable properties = new Hashtable();
                        //properties.Add(MQC.MQCACH_SSL_CIPHER_SUITE, "SSL_RSA_WITH_RC4_128_SHA"); Not required Temp
                        //properties.Add(MQC.MQCA_SSL_KEY_REPOSITORY, "C:\\tempers\\key"); Not required Temp

                        properties.Add(MQC.CHANNEL_PROPERTY, channel);
                        properties.Add(MQC.HOST_NAME_PROPERTY, host);
                        properties.Add(MQC.PORT_PROPERTY, port);
                        properties.Add(MQC.TRANSPORT_PROPERTY, MQC.TRANSPORT_MQSERIES_MANAGED);
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Adding Hash properties", "Logger for debug");
                        Console.Write("Connecting queue manager.. ");
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Creating new MQQueueManager instance", "Logger for debug");

                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        objMQQueueManager = new MQQueueManager(queueManager, properties);
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "MQQueueManager instance created", "Logger for debug");
                        Console.WriteLine("done");

                        if (objMQQueueManager.IsConnected)
                        {
                            queueManagerIsConnected = true;
                            Console.WriteLine(queueManager + " : Connected Sucessfully");

                            //SetCurrentBatchStatus();//set all previous record get is current batch = o
                            lQueueMessagesRead = QueueGetMessages(); //get queue messages 
                            lTopicMessagesRead = TopicGetMessages();//get topic messages
                                                                    //UpdatCaseDetails();//Execute SP to cases for current Batch of MQ messages

                            lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;

                            // disconnecting queue manager
                            Console.Write("Disconnecting queue manager.. ");
                            objMQQueueManager.Disconnect();
                            Console.WriteLine("done");
                        }
                    }
                }
                catch (Exception ex)
                {
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Entered catch block 2", "Logger for debug");
                    Console.WriteLine("Error : " + ex.Message);
                    lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
                }
                finally
                {
                    _lstEmployerNationalGroup = null;
                }
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Completed try catch block 2", "Logger for debug");
                //log BGP run status
                Console.WriteLine("Inserting Background process run details and status.");
                DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();
                objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterId;
                objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = lQueueMessagesRead + lTopicMessagesRead;
                objDOCMN_BackgroundProcessMaster.BGPStatusLkup = lBGPStatusLkup;
                objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = _lCurrentMasterUserId;
                objBLCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out errorMessage);
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Completed try block 1", "Logger for debug");
            }
            catch (Exception ex)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Entered catch block 1", "Logger for debug");
                Console.WriteLine("Error : " + ex.Message);
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
            }
            return bgpMasterId;
        }

        #region .Net Client MQ Code
        public long QueueGetMessages()
        {
            long msgsRead = 0;
            if (queueManagerIsConnected)
            {
                Console.WriteLine(queueManager + " : Accessing Queue");
                objMQQueue = objMQQueueManager.AccessQueue(queueName, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);

                Console.WriteLine(queueManager + " : Will read and process " + _msgCount + " messages");

                for (int i = 1; i <= _msgCount; i++)
                {
                    objMQMessage = new MQMessage();
                    objMQMessage.Format = MQC.MQFMT_STRING;
                    objMQGetMessageoption = new MQGetMessageOptions();
                    try
                    {
                        Console.WriteLine(queueManager + " : Geting message - " + i);
                        objMQQueue.Get(objMQMessage, objMQGetMessageoption);
                        var strMessage = objMQMessage.ReadString(objMQMessage.MessageLength).ToString();
                        msgsRead = ProcessXMLMessage(msgsRead, strMessage, MQSourceTypeLkup.Queue);
                        objMQMessage.ClearMessage();
                        Console.WriteLine(queueManager + " : Finished processing messasge - " + i);
                    }
                    catch (MQException mqe)
                    {
                        Console.WriteLine("Error : " + mqe.Message);
                        if (mqe.ReasonCode == 2033)
                        {
                            Console.WriteLine(queueManager + " : No more messages in Queue.");
                            break;//stop to loop of reading message since no more Messages available on Queue
                        }
                        else
                        {
                            Console.WriteLine("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message);
                            continue;//Skip this message and continue to next
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error : " + ex.Message);
                        throw ex;
                    }
                }
                // closing topic
                Console.Write("Closing Queue.. ");
                objMQQueue.Close();
                Console.WriteLine("done");
            }
            return msgsRead;
        }

        public long TopicGetMessages()
        {
            string errorMessage = string.Empty;
            long msgsRead = 0;
            if (queueManagerIsConnected)
            {
                Console.WriteLine(queueManager + " : Accessing Topic");
                //objMQTopic = mqQueueManager.AccessTopic("uhg_sq/ovations/elg_recon", null, MQC.MQSO_CREATE | MQC.MQSO_FAIL_IF_QUIESCING | MQC.MQSO_DURABLE | MQC.MQSO_RESUME, null, "DurableSubscriptionName");
                objMQTopic = objMQQueueManager.AccessTopic(topicName, null, MQC.MQSO_CREATE | MQC.MQSO_FAIL_IF_QUIESCING | MQC.MQSO_DURABLE | MQC.MQSO_RESUME, null, "DurableSubscriptionName");
                Console.WriteLine(queueManager + " : Will read and process " + _msgCount + " messages");

                for (int i = 1; i <= _msgCount; i++)
                {
                    // creating a message object
                    objMQMessage = new MQMessage();
                    try
                    {
                        Console.WriteLine(queueManager + " : Geting message - " + i);
                        objMQTopic.Get(objMQMessage);
                        var strMessage = objMQMessage.ReadString(objMQMessage.MessageLength).ToString();

                        msgsRead = ProcessXMLMessage(msgsRead, strMessage, MQSourceTypeLkup.Topic);
                        objMQMessage.ClearMessage();
                        Console.WriteLine(queueManager + " : Finished processing messasge - " + i);
                    }
                    catch (MQException mqe)
                    {
                        Console.WriteLine("Error : " + mqe.Message);
                        if (mqe.ReasonCode == 2033)
                        {
                            Console.WriteLine(queueManager + " : No more messages on Topic.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("MQException caught: {0} - {1}", mqe.ReasonCode, mqe.Message);
                            continue;
                        }
                    }
                }

                // closing topic
                Console.Write("Closing topic.. ");
                objMQTopic.Close();
                Console.WriteLine("done");

            }
            return msgsRead;
        }
        #endregion

        private long ProcessXMLMessage(long msgsRead, string strMessage, MQSourceTypeLkup MQSourceType)
        {
            ExceptionTypes result;
            string errorMessage = string.Empty;
            //save the just read messsage to DB
            MQMessagesRecieved objMQMessagesRecieved = new MQMessagesRecieved();
            objMQMessagesRecieved.CMN_BackgroundProcessMasterRef = bgpMasterId;
            objMQMessagesRecieved.MQMessage = strMessage;
            objMQMessagesRecieved.SystemId = _lCurrentMasterUserId;
            objMQMessagesRecieved.MQSourceTypeLkup = (long)MQSourceType;
            Console.WriteLine("Saving XML Message to DB...");
            result = objBLMQ.SaveXMLMessage(objMQMessagesRecieved, out errorMessage);
            if (result != ExceptionTypes.Success)
            {
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, "Error while saving sml message to DB");
                Console.WriteLine("Error while saving sml message to DB : " + errorMessage);
            }
            else
                Console.WriteLine("Saved XML Message to DB - MQMessagesRecievedId : " + objMQMessagesRecieved.MQMessagesRecievedId);

            //create record detail object
            DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
            objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = bgpMasterId;
            objDOCMN_BackgroundProcessDetails.GEN_QueueRef = 0;
            objDOCMN_BackgroundProcessDetails.UploadFileName = "MQMessagesRecievedRef : " + objMQMessagesRecieved.MQMessagesRecievedId;
            Console.WriteLine("Parsing and Saving message details as MQ TRR Record...");

            if (!strMessage.IsNullOrEmpty())
            {
                DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems(XDocument.Parse(strMessage));
                objDOMQTRRWorkQueueItems.MQSourceTypeLkup = (long)MQSourceType;
                objDOMQTRRWorkQueueItems.CMN_BackgroundProcessMasterRef = bgpMasterId;
                objDOMQTRRWorkQueueItems.MQMessagesRecievedRef = objMQMessagesRecieved.MQMessagesRecievedId;
                result = MessageParseAndSave(ref objDOMQTRRWorkQueueItems, out errorMessage);
                if (result == ExceptionTypes.Success)
                {
                    Console.WriteLine("Parsing and Saving message details successful - MQTRRWorkQueueItemId : " + objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId);
                    objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                    objMQMessagesRecieved.IsProcessed = true;
                    objMQMessagesRecieved.MQTRRWorkQueueItemRef = objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId;
                    objMQMessagesRecieved.ProcessedResult = objDOMQTRRWorkQueueItems.ObjToJsonString();// ToString();
                    msgsRead++;
                }
                else
                {
                    //Console.WriteLine(queueManager + " : Parsing and Saving message details failed - " + i + errorMessage);
                    objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                    objMQMessagesRecieved.IsProcessed = false;
                    objMQMessagesRecieved.ProcessingFailReason = errorMessage;
                }
            }
            else
            {
                //Console.WriteLine(queueManager + " : Parsing and Saving message details failed - " + i + errorMessage);
                objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                objMQMessagesRecieved.IsProcessed = false;
                objMQMessagesRecieved.ProcessingFailReason = "Empty XML Message";
                Console.WriteLine("MQ message error : Empty XML Message");
            }
            //Update read message status
            Console.WriteLine("updating XML message status in DB - MQMessagesRecievedId : " + objMQMessagesRecieved.MQMessagesRecievedId);
            objBLMQ.UpdateXMLMessage(objMQMessagesRecieved, out errorMessage);

            //insert into BGP details table
            objBLCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out errorMessage);
            return msgsRead;
        }

        public ExceptionTypes MessageParseAndSave(ref DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, out string errorMessage)
        {
            errorMessage = string.Empty;
            ExceptionTypes result;
            try
            {

                if (objDOMQTRRWorkQueueItems.MQSourceTypeLkup == (long)MQSourceTypeLkup.Queue)
                {
                    Console.WriteLine("Getting TRR Details from web service...");
                    result = GetTRRDetails(objDOMQTRRWorkQueueItems.TrrRecordID, ref objDOMQTRRWorkQueueItems, out errorMessage);
                    if (result != ExceptionTypes.Success)
                    {
                        Console.WriteLine("Error : Getting TRR Details from web service for " + objDOMQTRRWorkQueueItems.TrrRecordID.ToString());
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error : Getting TRR Details from web service for " + objDOMQTRRWorkQueueItems.TrrRecordID.ToString(), errorMessage);
                    }
                }
                else if (objDOMQTRRWorkQueueItems.MQSourceTypeLkup == (long)MQSourceTypeLkup.Topic)
                {
                    Console.WriteLine("Getting Member  Detais from ERS DB...");
                    result = GetCaseDetailsByID(ref objDOMQTRRWorkQueueItems, out errorMessage);
                    if (result != ExceptionTypes.Success)
                    {
                        Console.WriteLine("Error : Getting Member  Detais from ERS DB... " + objDOMQTRRWorkQueueItems.ERSCaseNumber.ToString() + " - " + errorMessage);
                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error : Getting Member  Detais from ERS DB... " + objDOMQTRRWorkQueueItems.ERSCaseNumber.ToString(), errorMessage);
                    }

                }
                Console.WriteLine("Getting Member Eligibility Detais from web service...");
                result = GetMemberEligibility(objDOMQTRRWorkQueueItems.HouseHoldID, ref objDOMQTRRWorkQueueItems, out errorMessage);
                if (result != ExceptionTypes.Success)
                {
                    Console.WriteLine("Error : Getting Member Eligibility Detais from web service... " + (objDOMQTRRWorkQueueItems.HouseHoldID ?? "" ) + " - " + errorMessage);
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error : Getting Member Eligibility Detais from web service... " + (objDOMQTRRWorkQueueItems.HouseHoldID ?? "" ), errorMessage);
                }

                Console.WriteLine("Getting Is Restricted Details from web service...");
                result = GetEmployerSummary(objDOMQTRRWorkQueueItems.HouseHoldID, ref objDOMQTRRWorkQueueItems, out errorMessage);
                if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                {
                    Console.WriteLine("Error : Getting Is Restricted Details from web service... " + (objDOMQTRRWorkQueueItems.HouseHoldID ?? "" ) + " - " + errorMessage);
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error : Getting Employer Summary Detais from web service... " + (objDOMQTRRWorkQueueItems.HouseHoldID ?? "" ), errorMessage);
                }
                
                Console.WriteLine("Saving MQ TRR Record to DB...");
                result = objBLMQ.InsertMQTRRRecord(objDOMQTRRWorkQueueItems, _lCurrentMasterUserId, out errorMessage);
                if (result != ExceptionTypes.Success)
                {
                    Console.WriteLine("Error : Saving MQ TRR Record to DB... " + objDOMQTRRWorkQueueItems.MQMessagesRecievedRef.ToString() + " - " + errorMessage);
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error : Saving MQ TRR Record to DB... " + objDOMQTRRWorkQueueItems.MQMessagesRecievedRef.ToString(), errorMessage);
                }

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Exception While saving MQ Queue Message", ex.ToString());
                errorMessage = "Exception while saving";
                objDOMQTRRWorkQueueItems = null;
                return ExceptionTypes.Exception;
            }
        }

        public void UpdatCaseDetails(long savedMessagesBGPId)
        {
            string errorMessage = string.Empty;
            long lRecordsProcessed = 0;
            ExceptionTypes result;
            try
            {
                long lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                //Insert BGP Master Row
                objBLCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.MQReadQueuesandTopics, _lCurrentMasterUserId, out bgpMasterId, out errorMessage);
                try
                {
                    result = new ExceptionTypes();
                    result = objBLMQ.MQTRRRecordsToProcess(savedMessagesBGPId, out List<DOMQTRRWorkQueueItems> lstDOMQTRRWorkQueueItems, out errorMessage);

                    if (result != ExceptionTypes.Success && result != ExceptionTypes.ZeroRecords)
                    {
                        lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    }
                    else
                    {
                        if (lstDOMQTRRWorkQueueItems.Count > 0)
                        {
                            foreach (DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems in lstDOMQTRRWorkQueueItems)
                            {
                                if (objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId != 0)
                                {
                                    //create record detail object
                                    DOCMN_BackgroundProcessDetails objDOCMN_BackgroundProcessDetails = new DOCMN_BackgroundProcessDetails();
                                    objDOCMN_BackgroundProcessDetails.CMN_BackgroundProcessMasterRef = bgpMasterId;
                                    objDOCMN_BackgroundProcessDetails.GEN_QueueRef = 0;
                                    objDOCMN_BackgroundProcessDetails.UploadFileName = "MQTRRWorkQueueItemsRef : " + objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId;
                                    result = objBLMQ.UpdatMQTRRCaseDetails(objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId, objDOMQTRRWorkQueueItems.MQSourceTypeLkup, objDOMQTRRWorkQueueItems.ERSCaseNumber, _lCurrentMasterUserId, out errorMessage);
                                    if (result != ExceptionTypes.Success)
                                    {
                                        Console.WriteLine("Processing MQ TRR Record - " + objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId + " failed." + errorMessage);
                                        objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                                        objDOCMN_BackgroundProcessDetails.FailureReason = errorMessage;
                                        BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, "Error While Updating Case details.", errorMessage);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Processing MQ TRR Record - " + objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId + " Successful.");
                                        objDOCMN_BackgroundProcessDetails.BGPRecordStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                                        lRecordsProcessed++;
                                    }

                                    Console.WriteLine(queueManager + " : Insert background process record details - " + objDOMQTRRWorkQueueItems.MQTRRWorkQueueItemId);
                                    //insert into BGP details table
                                    objBLCommon.InsertBackgroundProcessDetails(objDOCMN_BackgroundProcessDetails, out errorMessage);
                                }
                            }
                        }
                        lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                    lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                    BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
                }
                //log BGP run status
                Console.WriteLine("Inserting Background process run details and status.");
                DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();
                objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterId;
                objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = lRecordsProcessed;
                objDOCMN_BackgroundProcessMaster.BGPStatusLkup = lBGPStatusLkup;
                objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = _lCurrentMasterUserId;
                objBLCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out errorMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
            }
        }

        public long SetCurrentBatchStatus()
        {
            string errorMessage = string.Empty;
            ExceptionTypes result;
            result = objBLMQ.SetCurrentBatchStatus(out errorMessage);
            return (long)result;
        }

        #region Helper GPS Service Methods to get extra details
        public ExceptionTypes GetCaseDetailsByID(ref DOMQTRRWorkQueueItems dOMQTRRWorkQueueItems, out string errorMessage)
        {
            errorMessage = "";
            long? TimeZone = (long)DefaultTimeZone.CentralStandardTime;
            ExceptionTypes result = ExceptionTypes.UnknownError;
            try
            {
                BLOST objBLOST = new BLOST();
                if (long.TryParse(dOMQTRRWorkQueueItems.StrERSCaseNumber, out long ErsCaseNumber))
                    dOMQTRRWorkQueueItems.ERSCaseNumber = ErsCaseNumber;
                else
                    dOMQTRRWorkQueueItems.ERSCaseNumber = objBLCommon.ERSCaseIDForERNCaseNumber(dOMQTRRWorkQueueItems.StrERSCaseNumber);
                if (dOMQTRRWorkQueueItems.ERSCaseNumber != 0)
                {
                    result = objBLOST.GetGenQueueByID(TimeZone, dOMQTRRWorkQueueItems.ERSCaseNumber, out DOGEN_Queue objDOGEN_Queue, out errorMessage);
                    dOMQTRRWorkQueueItems.MemberID = objDOGEN_Queue.MemberID;
                    dOMQTRRWorkQueueItems.HICN = objDOGEN_Queue.MemberCurrentHICN;
                    dOMQTRRWorkQueueItems.HouseHoldID = objDOGEN_Queue.GPSHouseholdID;
                    dOMQTRRWorkQueueItems.Contract = objDOGEN_Queue.MemberContractID;
                    dOMQTRRWorkQueueItems.PBP = objDOGEN_Queue.MemberPBP;
                    dOMQTRRWorkQueueItems.FirstName = objDOGEN_Queue.MemberFirstName;
                    dOMQTRRWorkQueueItems.MiddleName = objDOGEN_Queue.MemberMiddleName;
                    dOMQTRRWorkQueueItems.LastName = objDOGEN_Queue.MemberLastName;
                    dOMQTRRWorkQueueItems.DOB = objDOGEN_Queue.MemberDOB;
                    dOMQTRRWorkQueueItems.LOB = objDOGEN_Queue.MemberLOB;
                    dOMQTRRWorkQueueItems.TimelineEffectiveDate = objDOGEN_Queue.ComplianceStartDate;
                    dOMQTRRWorkQueueItems.TRRFileReceiptDate = objDOGEN_Queue.ComplianceStartDate;
                    dOMQTRRWorkQueueItems.SCCCode = objDOGEN_Queue.MemberSCCCode;
                    result = ExceptionTypes.Success;
                }
                else
                    result = ExceptionTypes.ZeroRecords;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message.ToString();
                result = ExceptionTypes.Exception;
            }
            return result;
        }

        public ExceptionTypes GetTRRDetails(string strTransactionId, ref DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, out string errorMessage)
        {
            ExceptionTypes result = ExceptionTypes.UnknownError;
            errorMessage = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            srvAEGetTRRDetails.SearchTrrdetailClient client = new srvAEGetTRRDetails.SearchTrrdetailClient();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetTRRDetails;
            objDOGEN_AEGPSServiceTrace.CreatedByRef = _lCurrentMasterUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = 0;

            inputData = "TRR Transcation Id:||" + strTransactionId;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            try
            {
                //Header
                client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
                srvAEGetTRRDetails.searchTrrDetailRequest request = new srvAEGetTRRDetails.searchTrrDetailRequest();
                srvAEGetTRRDetails.searchTrrDetailRequestSearch requestParameter = new srvAEGetTRRDetails.searchTrrDetailRequestSearch();
                srvAEGetTRRDetails.searchTrrDetailResponse response = new srvAEGetTRRDetails.searchTrrDetailResponse();
                srvAEGetTRRDetails.controlModifiersType credentials = new srvAEGetTRRDetails.controlModifiersType();
                srvAEGetTRRDetails.gpsSystemParametersType sysParameter = new srvAEGetTRRDetails.gpsSystemParametersType();
                srvAEGetTRRDetails.requestHeader reqHeader = new srvAEGetTRRDetails.requestHeader();
                requestParameter.transactionId = strTransactionId;
                ////As Per UPM
                sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
                sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
                reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
                reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
                credentials.gpsSystemParameters = sysParameter;
                requestParameter.controlModifier = credentials;
                request.requestHeader = reqHeader;
                request.search = requestParameter;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Open();
                objDOGEN_AEGPSServiceTrace.ResponseData = "";

                response = client.invokeService(request);
                if (response != null && !response.trrDetail.IsNull())
                {
                    var responseOutput = response.trrDetail;
                    if (!responseOutput.IsNull())
                    {
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                        if (responseOutput.transaction != null)
                        {
                            objDOMQTRRWorkQueueItems.WQTrackingNumber = responseOutput.transaction.gpsTrackingNumber.IsNullOrEmpty() ? "00000" : responseOutput.transaction.gpsTrackingNumber;
                            objDOMQTRRWorkQueueItems.TRCTypeCode = responseOutput.transaction.transactionCode.code;
                            objDOMQTRRWorkQueueItems.TRRFileReceiptDate = Convert.ToDateTime(responseOutput.transaction.processDate);
                            objDOMQTRRWorkQueueItems.GPSContract = responseOutput.transaction.plan.contractNo;
                            objDOMQTRRWorkQueueItems.GPSPBP = Convert.ToInt32(responseOutput.transaction.plan.pbpNo).ToString("D3");

                            if(objDOMQTRRWorkQueueItems.HICN.IsNullOrEmpty())
                                objDOMQTRRWorkQueueItems.HICN = responseOutput.transaction.medicareClaimNumber ?? "";

                        }
                        result = ExceptionTypes.Success;
                    }
                    else
                    {
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        result = ExceptionTypes.ZeroRecords;
                    }
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                    result = ExceptionTypes.RemoteCallException;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
            return result;
        }

        public ExceptionTypes GetMemberEligibility(string strAccountId, ref DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, out string errorMessage)
        {
            ExceptionTypes result = ExceptionTypes.UnknownError;
            errorMessage = string.Empty;
            string inputData = string.Empty;
            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            srvAEMemberEligibility.ReadOvationsMemberEligibilityClient client = new srvAEMemberEligibility.ReadOvationsMemberEligibilityClient();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetMemberEligibilityService;
            objDOGEN_AEGPSServiceTrace.CreatedByRef = _lCurrentMasterUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = 0;
            inputData = "Account Id:||" + strAccountId;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            try
            {
                //Header
                client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
                srvAEMemberEligibility.readOvationsMemberEligibilityRequest request = new srvAEMemberEligibility.readOvationsMemberEligibilityRequest();
                srvAEMemberEligibility.readOvationsMemberEligibilityResponse response = new srvAEMemberEligibility.readOvationsMemberEligibilityResponse();
                
                srvAEMemberEligibility.accountIdSearchCriteriaType accSearchType = new srvAEMemberEligibility.accountIdSearchCriteriaType();
                srvAEMemberEligibility.requestHeader reqHeader = new srvAEMemberEligibility.requestHeader();
                srvAEMemberEligibility.gpsSystemParametersType sysParameter = new srvAEMemberEligibility.gpsSystemParametersType();
                srvAEMemberEligibility.controlModifiersType controlModifiers = new srvAEMemberEligibility.controlModifiersType();

                srvAEMemberEligibility.employerIdSearchCriteriaType empSerachDetails = new srvAEMemberEligibility.employerIdSearchCriteriaType();
                accSearchType.accountId = strAccountId;
                request.searchType = "ACCOUNT";
                request.householdSearchCriteria = accSearchType;

                //srvAEMemberEligibility.planBenefitSearchCriteriaType planSearchType = new srvAEMemberEligibility.planBenefitSearchCriteriaType();
                //planSearchType.contractNumber = objDOMQTRRWorkQueueItems.Contract;
                //planSearchType.planBenefitPackage = objDOMQTRRWorkQueueItems.PBP;
                //planSearchType.medicareClaimNumber = objDOMQTRRWorkQueueItems.HICN;
                //request.searchType = "PLAN";
                //request.planSearchCriteria = planSearchType;

                sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
                sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
                reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
                reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
                controlModifiers.gpsSystemParameters = sysParameter;
                controlModifiers.getPlanProfileIndicator = true;
                controlModifiers.includeApplicationData = true;
                controlModifiers.includeApplicationDataSpecified = true;
                request.controlModifiers = controlModifiers;
                request.requestHeader = reqHeader;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                client.Open();

                response = client.invokeService(request);
                if (response != null)
                {

                    var responseOutput = response.account;
                    if (!responseOutput[0].IsNull())
                    {
                        objDOMQTRRWorkQueueItems.HouseHoldID = responseOutput[0].accountId;
                        var household = responseOutput[0].householdIndividual[0];
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                        objDOMQTRRWorkQueueItems.MemberID = household.memberProfile[0].memberNumber;

                        if (!household.name.IsNull())
                        {
                            objDOMQTRRWorkQueueItems.FirstName = household.name.firstName.NullToString();
                            objDOMQTRRWorkQueueItems.MiddleName = household.name.middleName.NullToString();
                            objDOMQTRRWorkQueueItems.LastName = household.name.lastName.NullToString();
                        }

                        objDOMQTRRWorkQueueItems.IndividualID = household.individualId;
                        objDOMQTRRWorkQueueItems.DOB = Convert.ToDateTime(household.dateOfBirth);

                        //Macra MBI changes 
                        if(objDOMQTRRWorkQueueItems.HICN.IsNullOrEmpty())
                            objDOMQTRRWorkQueueItems.HICN = household.medicareClaimNumber.NullToString();

                        if (household.application != null && household.application.Length > 0)
                        {
                            objDOMQTRRWorkQueueItems.GPSProposedEffectiveDate = Convert.ToDateTime(household.application[0].proposedEffectiveDate);
                            objDOMQTRRWorkQueueItems.PlanTerminationDate = Convert.ToDateTime(responseOutput[0].terminationDate);
                            objDOMQTRRWorkQueueItems.GPSApplicationStatus = household.application[0].status;
                            if (!objDOMQTRRWorkQueueItems.Contract.IsNullOrEmpty())
                            {
                                if (objDOMQTRRWorkQueueItems.Contract.ToCharArray()[0] == 'S')
                                    objDOMQTRRWorkQueueItems.LOB = "PDP";
                                else
                                    objDOMQTRRWorkQueueItems.LOB = "MA";
                            }
                            else
                            {
                                    objDOMQTRRWorkQueueItems.LOB = household.application[0].planCategory;
                            }
                            
                            if (!household.application[0].enrollmentSourceCode.IsNullOrEmpty())
                            {
                                char ESC = Convert.ToChar(household.application[0].enrollmentSourceCode.ToUpper());
                                if (ESC == 'A' || ESC == 'C' || ESC == 'H')
                                    objDOMQTRRWorkQueueItems.GPSPDPAutoEnroleeIndicator = true;
                                else
                                    objDOMQTRRWorkQueueItems.GPSPDPAutoEnroleeIndicator = false;
                            }
                            else
                                objDOMQTRRWorkQueueItems.GPSPDPAutoEnroleeIndicator = false;

                        }
                        result = ExceptionTypes.Success;
                    }
                    else
                    {
                        objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                        objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                        result = ExceptionTypes.ZeroRecords;
                    }
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                    objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                    result = ExceptionTypes.RemoteCallException;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
            return result;
        }

        public ExceptionTypes GetEmployerSummary(string strAccountId, ref DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems, out string errorMessage)
        {
            ExceptionTypes result;
            errorMessage = string.Empty;
            string responseData1 = string.Empty;
            string inputData = string.Empty;

            BLServiceRequestResponse objBLServiceRequestResponse = new BLServiceRequestResponse();
            DOGEN_AEGPSServiceTrace objDOGEN_AEGPSServiceTrace = new DOGEN_AEGPSServiceTrace();
            objDOGEN_AEGPSServiceTrace.CreatedByRef = _lCurrentMasterUserId;
            objDOGEN_AEGPSServiceTrace.WebServiceMethodName = MethodBase.GetCurrentMethod().ToString();
            objDOGEN_AEGPSServiceTrace.WebServiceMethodLkup = (long)WebserviceMethod.GetEmployerSummary;
            objDOGEN_AEGPSServiceTrace.GEN_QueueRef = 0;

            srvAEEmployerSummary.ReadEmployerClient client = new srvAEEmployerSummary.ReadEmployerClient();
            client.Endpoint.EndpointBehaviors.Add(new GPSHeaderService.CustomEndpointBehavior());
            srvAEEmployerSummary.readEmployerRequest request = new srvAEEmployerSummary.readEmployerRequest();
            srvAEEmployerSummary.readEmployerResponse response = new srvAEEmployerSummary.readEmployerResponse();
            srvAEEmployerSummary.requestHeader reqHeader = new srvAEEmployerSummary.requestHeader();
            srvAEEmployerSummary.gpsSystemParametersType sysParameter = new srvAEEmployerSummary.gpsSystemParametersType();
            srvAEEmployerSummary.readInputMetaType readInputMeta = new srvAEEmployerSummary.readInputMetaType();
            srvAEEmployerSummary.readEmployerRequestReadInput readInput = new srvAEEmployerSummary.readEmployerRequestReadInput();
            readInput.householdId = strAccountId;
            sysParameter.clientId = System.Configuration.ConfigurationManager.AppSettings["AEClientId"].ToString();
            sysParameter.userId = System.Configuration.ConfigurationManager.AppSettings["AEUserId"].ToString();
            reqHeader.applicationInstanceName = System.Configuration.ConfigurationManager.AppSettings["ApplicationInstantName"].ToString();
            reqHeader.applicationName = System.Configuration.ConfigurationManager.AppSettings["ApplicationName"].ToString();
            readInputMeta.gpsSystemParameters = sysParameter;
            readInput.readInputMeta = readInputMeta;
            request.readInput = readInput;
            request.requestHeader = reqHeader;

            inputData = "GPSHouseHoldId:||" + strAccountId;
            objDOGEN_AEGPSServiceTrace.RequestData = inputData;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.Open();
                response = client.invokeService(request);
                objDOGEN_AEGPSServiceTrace.ResponseData = response.ObjToJsonString();
                //response.readOutput.employer.employerSummary = new employerSummaryType();

                if (response != null && !response.readOutput.IsNull() && !response.readOutput.employer.IsNull() && !response.readOutput.employer.employerSummary.IsNull())
                {
                    employerSummaryType employerSummary = response.readOutput.employer.employerSummary;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;

                    //employerSummary.employerId = "24597";
                    //objDOMQTRRWorkQueueItems.PBP = "801";
                    //objDOMQTRRWorkQueueItems.Contract = "H1537";
                    //objDOMQTRRWorkQueueItems.GPSContract = "H1537";
                    //objDOMQTRRWorkQueueItems.GPSPBP = "801";

                    if (!employerSummary.employerId.IsNullOrEmpty())
                    {
                        objDOMQTRRWorkQueueItems.EmployerId = employerSummary.employerId;

                        List<DOCMN_LookupMaster> lstEmployers = CacheUtility.GetAllLookupsFromCache((long)LookupTypes.OnshoreOnlyEmployers);
                        if (!employerSummary.employerCloseDate.IsNullOrEmpty() && DateTime.TryParse(employerSummary.employerCloseDate, out DateTime empCloseDate))
                        {
                            if (empCloseDate < DateTime.UtcNow)
                                objDOMQTRRWorkQueueItems.IsRestricted = false;
                            else
                                objDOMQTRRWorkQueueItems.IsRestricted = lstEmployers.Exists(x => x.LookupValue == employerSummary.employerId);
                        }
                        else
                        {
                            objDOMQTRRWorkQueueItems.IsRestricted = lstEmployers.Exists(x => x.LookupValue == employerSummary.employerId);
                        }

                        //logic to check employer National group
                        if(!_lstEmployerNationalGroup.IsNull() && _lstEmployerNationalGroup.Count > 0 )
                        {
                            string tempEmployerId = objDOMQTRRWorkQueueItems.EmployerId;
                            string tempContract = objDOMQTRRWorkQueueItems.Contract;
                            string tempPBP = objDOMQTRRWorkQueueItems.PBP;
                            if (_lstEmployerNationalGroup.Exists(x => x.EmployerId == tempEmployerId && x.Contract == tempContract && x.PBP == tempPBP))
                                objDOMQTRRWorkQueueItems.IsNationalEmployee = true;
                            else
                                objDOMQTRRWorkQueueItems.IsNationalEmployee = false;
                        }
                    }
                    result = ExceptionTypes.Success;
                }
                else
                {
                    errorMessage = response.responseHeader.statusMessages[0].statusMessage1;
                    objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Success;
                    result = ExceptionTypes.ZeroRecords;
                }
            }
            catch (System.ServiceModel.FaultException ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                objDOGEN_AEGPSServiceTrace.StatusLkup = (long)WebserviceStatus.Failed;
                objDOGEN_AEGPSServiceTrace.ResponseData = errorMessage;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().ToString(), (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Exception, ex.ToString(), ex.ToString());
                result = ExceptionTypes.Exception;
            }
            finally
            {
                objBLServiceRequestResponse.InsertAEGPSServiceTrace(objDOGEN_AEGPSServiceTrace);
                client.Close();
            }
            return result;
        }

        #endregion

        #region Testing methods
        //testing methods from XML Files
        public long TestQueueParseAndSave()
        {
            long lQueueMessagesRead = 0;
            long lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
            //Insert BGP Master Row
            objBLCommon.InsertBackgroundProcessMaster((long)BackgroundProcessType.MQReadQueuesandTopics, _lCurrentMasterUserId, out bgpMasterId, out string errorMessage);
            try
            {
                ExceptionTypes result = objBLMQ.GetNationalEmployerGroups(out _lstEmployerNationalGroup, out errorMessage);
                List<string> messageFiles = Directory.GetFiles(_basePath + "QueueMessages", "*.xml").ToList();
                Console.WriteLine("Total Queue messages : " + messageFiles.Count);
                if (messageFiles.Count > 0)
                {
                    foreach (var file in messageFiles)
                    {
                        Console.WriteLine("Processing Queue Message number : " + lQueueMessagesRead);
                        string xmlMessage = File.ReadAllText(file);
                        lQueueMessagesRead = ProcessXMLMessage(lQueueMessagesRead, xmlMessage, MQSourceTypeLkup.Queue);
                    }
                }
                Console.WriteLine("Messages Processed : " + lQueueMessagesRead);
                lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
                lBGPStatusLkup = (long)BackgroundProcessRecordStatus.Failed;
                BLCommon.LogError(_lCurrentMasterUserId, MethodBase.GetCurrentMethod().Name, (long)ErrorModuleName.BGPMQProcess, (long)ExceptionTypes.Uncategorized, errorMessage, ex.ToString());
            }
            finally
            {
                _lstEmployerNationalGroup = null;
            }
            //log BGP run status
            Console.WriteLine("Inserting Background process run details and status.");
            DOCMN_BackgroundProcessMaster objDOCMN_BackgroundProcessMaster = new DOCMN_BackgroundProcessMaster();
            objDOCMN_BackgroundProcessMaster.CMN_BackgroundProcessMasterId = bgpMasterId;
            objDOCMN_BackgroundProcessMaster.TotalRecordProcessed = lQueueMessagesRead;
            objDOCMN_BackgroundProcessMaster.BGPStatusLkup = lBGPStatusLkup;
            objDOCMN_BackgroundProcessMaster.LastUpdatedByRef = _lCurrentMasterUserId;
            objBLCommon.UpdateBackgroundProcessMaster(objDOCMN_BackgroundProcessMaster, out errorMessage);
            return bgpMasterId;
        }

        public void TestTopicParseAndSave()
        {
            string xmlMessage = "";
            xmlMessage = File.ReadAllText(@"C:\Desktop\Work\MQs\MQupdateCMSTransaction2.xml");
            DOMQTRRWorkQueueItems objDOMQTRRWorkQueueItems = new DOMQTRRWorkQueueItems(XDocument.Parse(xmlMessage));
            objDOMQTRRWorkQueueItems.MQSourceTypeLkup = (long)MQSourceTypeLkup.Topic;
            objDOMQTRRWorkQueueItems.CMN_BackgroundProcessMasterRef = 0;
            ExceptionTypes result = MessageParseAndSave(ref objDOMQTRRWorkQueueItems, out string errorMessage);
        }
        #endregion

    }
}
