using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ENRLReconSystem.Utility
{
    public enum ExceptionTypes
    {
        Success = 0,
        UnknownError = 1,
        ZeroRecords = 2,
        AccessViolationException = 10,
        AppDomainUnloadedException = 11,
        ApplicationException = 12,
        ArgumentException = 13,
        ArgumentNullException = 14,
        ArgumentOutOfRangeException = 15,
        ArithameticException = 16,
        ArrayTypeMismatchException = 17,
        BadImageFormatException = 18,
        CannotUnloadAppDomainException = 19,
        ContextMarshalException = 20,
        DataMisalignedException = 21,
        DivideByZeroException = 22,
        DllNotFoundException = 23,
        DuplicateWaitObjectException = 24,
        EntryPointNotFoundException = 25,
        Exception = 26,
        ExecutionEngineException = 27,
        FieldAccessException = 28,
        FormatException = 29,
        IndexOutOfRangeException = 30,
        InsufficientMemmoryException = 31,
        InvalidCastException = 32,
        InvalidOperationException = 33,
        InvalidProgramException = 34,
        MemberAccessException = 35,
        MethodAccessException = 36,
        MissingFieldException = 37,
        MissingMemberException = 38,
        MissingMethodException = 39,
        MulticastNotSupportedException = 40,
        NotFiniteNumberException = 41,
        NotImplementedException = 42,
        NotSupportedException = 43,
        NullReferenceException = 44,
        ObjectDisposedException = 45,
        OperationCanceledException = 46,
        OutOfMemmoryException = 47,
        OverflowException = 48,
        PlatformNotSupportedException = 49,
        RankException = 50,
        StackOverflowException = 51,
        SystemException = 52,
        TimeoutException = 53,
        TypeInitializationException = 54,
        TypeLoadException = 55,
        TypeUnloadException = 56,
        UnauthorizedAccessException = 57,  //System
        SqlException = 58,  //System.Data.SqlClient
        ConstraintException = 59,
        DataException = 60,
        DBConcurrencyException = 61,
        DeletedRowInaccessibleException = 62,
        DuplicateNameException = 63,
        EvaluvateException = 64,
        InvalidConstraintException = 65,
        InvalidExpressionException = 66,
        MissingPrimaryKeyException = 67,
        NoNullAllowedException = 68,
        OperationAbortedException = 69,
        ReadOnlyException = 70,
        RowNotInTableException = 71,
        StrongTypingException = 72,
        SyntaxErrorException = 73,
        TypedDataSetGeneratorException = 74,
        VersionNotFoundException = 75,
        InvalidParameter = 76,
        DuplicateRecord = 77,
        RemoteCallException = 78,
        EditInProgressException = 79,
        SaveFailed = 80,
        AuthenticationChanged = 81,
        TaskAlreadyCompleted = 82,
        NotLocked = 83,
        AssignedToOtherUser = 84,
        Uncategorized = 85,
        PrimaryKeyVoilation = 2627,
        StoredProcedureNotFound = 2812,
        SessionTimeOut = 401,
        UnlockedRecord = -1
    }

    public enum ERSAuthenticationRoles
    {
        AdminUser,
        AdmOSTUser,
        AdmEligUser,
        AdmRPRUser,
        MgrOSTUser,
        MgrEligUser,
        MgrRPRUser,
        PrcrOSTUser,
        PrcrEligUser,
        PrcrRPRUser,
        VwrOSTUser,
        VwrEligUser,
        VwrRPRUser,
        WebServiceUser,
        MacroServiceUser,
        /// <summary>
        /// All logged in users will have this role
        /// </summary>
        User,
        /// <summary>
        /// Users not in DB will have this role
        /// </summary>
        Unauthenticated
    }
    public enum RPRResolution
    {
       DisenrollmentDateChangeDODError=18010,
       EffectiveDateChange=18011,
       EnrollmentCancellation=18012,
       PBPChange=18019,
       PlanErrorReinstatement=18020,
       ReinstatementDeathABTerm=18022,
       ReinstatementOther=18023,
       ReinstatementTRC14=18024,
       RetroDisenrollment=18025,
       RetroEnrollment=18026
    }
    public enum YesOrNoDropDownValues
    {
        Yes = 16001,
        No = 16002,
        NA = 16003
    }
    public enum WorkBasket
    {
        OST = 3001,
        GPSvsMMR = 3002,
        RPR = 3003
    }

    public enum ErrorModuleName : long
    {
        ManageUsers = 45001,
        ManageAccessGroups = 45002,
        ManageSkills = 45003,
        Alerts = 45004,
        Configurations = 45005,
        Department = 45006,
        Lookups = 45007,
        LookupsCorrelation = 45008,
        Resources = 45009,
        OSTCreateSuspectCase = 45010,
        OSTGetQueue = 45011,
        EligibilityCreateSuspectCase = 45012,
        EligibilityGetQueue = 45013,
        RPRCreateSuspectCase = 45014,
        RPRGetQueue = 45015,
        Search = 45016,
        OSTProcessWorkflow = 45017,
        EligibilityProcessWorkflow = 45018,
        RPRProcessWorkflow = 45019,
        BulkUpload = 45020,
        Unlock = 45021,
        Reassign = 45022,
        MassUpdate = 45023,
        Reports = 45024,
        Attachments = 45025,
        GPSWebservice = 45026,
        MIIMService = 45027,
        UserPreference = 45028,
        MIIMConnector = 45029,
        Home = 45030,
        BGPFDRSubmission = 45031,
        BGPFDRResponseProcessing = 45032,
        BackgroundProcess = 45033,
        Common = 45034,
        Login = 45035,
        RecordsLocked = 45036,
        ErrorHandler = 45037,
        BGPSendOOALetter = 45038,
        BGPMQProcess = 45039,
        GetGPSServiceData = 45040,
        BGPCreateCMSTransaction = 45041,
        Macro = 45042,
        MassUpdateByTemplate= 45043
    }

    public enum LookupTypes : long
    {
        BusinessSegment = 1,
        Role = 2,
        WorkBasket = 3,
        Timezone = 4,
        State = 5,
        DiscripancyCategory = 6,
        DiscripancyType = 7,
        Department = 8,
        QueueProgressType = 9,
        Queue = 10,
        Contract = 11,
        PBPID = 12,
        DiscrepancySource = 13,
        RPRActionRequested = 14,
        Taskbeingperformedwhenthisdiscrepancywasidentified = 15,
        YesNo = 16,
        SubmissionType = 17,
        Resolution = 18,
        RPRReasonforRequest = 19,
        Status = 20,
        Gender = 21,
        TransactionReplyCode = 22,
        ElectionType = 23,
        RootCause = 24,
        ExplanationoftheRootCause = 25,
        FDRStatus = 26,
        FDRRejectionType = 27,
        Action = 28,
        TransactionTypeCode = 29,
        PendReason = 30,
        LOB = 31,
        AdjustedCreateDateReason = 32,
        Location = 33,
        Salutation = 34,
        SendAlertTo = 35,
        AlertCriticality = 36,
        DBMasterTables = 37,
        SourceSystem = 38,
        UserEscalationReason = 39,
        FileType = 40,
        ImportStatus = 41,
        Reopen = 48,
        VerifiedRootCause = 57,
        MARxAddressCompleted = 58,
        OnshoreOnlyEmployers = 60
    }
    public enum LookupMasters : long
    {
        DiscrepancySourceLkup = 13004
    }

    public enum ScreenType : long
    {
        UserAdmin = 37001,
        AccessGroup = 37002,
        Skills = 37003,
        LookupType = 37004,
        LookupTypeCorrelation = 37005,
        Department = 37006,
        Alerts = 37007,
        Resources = 37008,
        Configuration = 37009,
        Queue = 37010,
        FDR = 37011,
    }

    public enum BackgroundProcessType : long
    {
        FDRSubmissionCat2 = 44001,
        FDRSubmissionCat2CTM = 44002,
        FDRSubmissionCat3 = 44003,
        FDRResubmission = 44004,
        FDRResponseProcessing = 44005,
        FDRSubmissionSCC = 44006,
        SendOOALetter = 44007,
        MQReadQueuesandTopics = 44008,
        CreateCMSTransaction = 44009,
        PendFTTToAddScrub = 44010,
        PendNOTToOpenNOT = 44011,
        PendFTTToOpenDisEnroll = 44012,
        PendFTTToMARxAdd = 44013,
        AutoUnlockRecords = 44014,
        EGHPExclusion = 44015,
        MoveNOTMacro = 44016,
        MoveFTTMacro = 44017,
        MoveTRC155Macro = 44018
    }

    public enum LookupTypesCorrelation
    {
        WorkBasketVsDiscripancyCategory = 1,
        DiscripancyCategoryVsDiscripancyType = 2,
        DiscripancyCategoryVQueue = 3,
        QueueVsAction = 4,
        DiscrepancyCategoryVsActionVsResolution = 5,
        DiscrepancyCategoryVsActionVsRootCause = 6,
        DiscrepancyCategoryVsActionVsPendReason = 7,
        BusinessSegmentVsContractNumber = 8,
        BusinessSegmentVsPBP = 9,
        BusinessSegmentVsContractNumberVsPBP = 10,
        StateVsTimeZone = 1000
    }

    public enum RoleLkup : long
    {
        Admin = 2001,
        Manager = 2002,
        Processor = 2003,
        Viewer = 2004
    }

    public enum DiscripancyCategory : long
    {
        OOA = 6001,
        SCC = 6002,
        TRR = 6003,
        Eligibility = 6004,
        DOB = 6005,
        Gender = 6006,
        RPR = 6007
    }

    public enum CurrentStatusLkup
    {
        New = 20001,
        InProgress = 20002,
        ResolvedComplted = 20003
    }
    public enum QueueLookup : long
    {
        OOANewCase = 10007,
        SCCNewCase = 10022,
        TRRNewCase = 10047,
        EligNewCase = 10054,
        DOBNewCase = 10060,
        GenderNewCase = 10064,
        RPRCMSAccountManagerSent = 10067,
        RPRRequestCategory2 = 10076,
        RPRRequestCategory3 = 10077
    }
    public enum ActionLookup : long
    {
        AddComments = 28001,
        CloseCase = 28002,
        ExtendTracking = 28003,
        FDRApproved = 28004,
        IncorrectSCCInformation = 28005,
        PeerAuditCompleted = 28006,
        PendCase = 28007,
        ResidentialDocRequiredOrCountyAttestationRequired = 28008,
        ResubmitSCCRPCRequest = 28009,
        RPCRejected = 28010,
        SCCRPRNotRequired = 28011,
        SCCRPRRequest = 28012,
        SCCRPRRequired = 28013,
        SendNotificationofTerminationLetter = 28014,
        SendOOALetter = 28015,
        SendSCCLetter = 28016,
        SendSCCUpdatetoCMS = 28017,
        SendtoPeerAudit = 28018,
        SubmitRPCRequest = 28019,
        SubmitSCCRPCRequest = 28020,
        SubmittoAccountManager = 28021,
        SubmittedTransactionInquiry = 28022,
        TransactionInquiryApproved = 28023,
        TransactionInquiryRequired = 28024,
        UpdateCMSEligibility = 28025,
        UpdateGPS = 28026,
        UpdatePlan = 28027,
        UpdatePlanAndCreateRPR = 28028,
        CloseAndMailingAddressNotVerified = 28029,
        SendSCCDeletiontoCMS = 28030,
        TRRAudit = 28031,
        Save = 28032,
        View = 28033,
        Print = 28034,
        Unlock = 28035,
        Audit = 28036,
        Cancelled = 28037,
        Import = 28038,
        ReOpen = 28039,
        ReAssign = 28040,
        SentSCCRPCResubmission = 28041,
        SendSCCUpdatetoCMSUpdateEndDate = 28043,
        ReceivedRPCFDR = 28044,
        SCCRPRFDRReceived = 28045,
        MARxAddressCompleted = 28046,
        AddressScrubCompleted = 28047,
        UpdateSenttoCMS = 28048,
        CMSAccepted = 28049,
        CMSRejected = 28050,
        LastMonthofTracking = 28051,
        RequiresMARxAddressLetter = 28052,
        RequiresAddressScrub = 28053,
        UpdatePDPAutoEnrolleeIndicator = 28074
    }
    public enum TimeZoneUTCDiffernece : int
    {
        PacificStandardTime = -480,
        PacificDaylightTime = -420,
        MountainStandardTime = -360,
        MountainDaylightTime = -360,
        CentralStandardTime = -360,
        CentralDaylightTime = -300,
        EasternStandardTime = -300,
        EasternDaylightTime = -240,
        ArizonaTime = -420,
        India = 330

    }

    public enum PWActionsEnum : long
    {
        AddComments = 28001,
        CloseCase,
        ExtendTracking,
        FDRApproved,
        IncorrectSCCInformation,
        PeerAuditCompleted,
        PendCase,
        ResidentialDocRequired_CountyAttestationRequired,
        ResubmitSCCRPCRequest,
        RPCRejected,
        SCCRPRNotRequired,
        SCCRPRRequest,
        SCCRPRRequired_SendSCCDeletiontoCMS,
        SendNotificationofTerminationLetter,
        SendOOALetter,
        SendSCCLetter,
        SendSCCUpdatetoCMS,
        SendtoPeerAudit,
        SubmitRPCRequest,
        SubmitSCCRPCRequest,
        SubmittoAccountManager,
        SubmittedTransactionInquiry,
        TransactionInquiryApproved,
        TransactionInquiryRequired,
        UpdateCMSEligibility,
        UpdateGPS,
        UpdatePlan,
        UpdatePlan_CreateRPR,
        Close_MailingAddressNotVerified,
        TRR_Audit = 28031,
        Save,
        View,
        Print,
        Unlock,
        Audit,
        Cancelled,
        Import,
        Re_Open,
        Re_Assign,
        SentSCCRPCResubmission = 28041,
        SendSCCUpdatetoCMSUpdateEndDate = 28043,
        ReceivedRPCFDR,
        RPRFDRReceived,
        MARxAddressCompleted,
        AddressScrubCompleted,
        UpdateSenttoCMS,
        CMSAccepted,
        CMSRejected,
        LastMonthofTracking,
        RequiresMARxAddressLetter,
        RequiresAddressScrub,
        ReOpenCloseCase = 28057,
        ReOpenAddComments = 28058,
        CloseCase_CreateRPR = 28065,
        SendtoCategory2 = 28066,
        SendtoCategory3 = 28067,
        NoOOALetter = 28068,
        KeepinTrackingNOT = 28069,
        KeepinTrackingFTT = 28070,
        UpdateSubmissionType = 28071,
        CloseCaseNoAction = 28072,
        ReturnToRequestQueue = 28073,
        UpdatePDPAutoEnrolleeIndicator = 28074,
        PendingNOT = 28080,
        PendingFTT = 28081,
        OpenNOT = 28082,
        OpenDisenroll = 28083,
        RouteToNewSCCCase = 28085,
        ReviewandReturntoQueue = 28084,
        PotentialSCCRPRDay1 = 28086,
        NeedsEGHPReview = 28087,
        SendSCCDeletiontoCMS = 28042,
        UpdateComplianceStartDate = 28089,
        CancelPendCase = 28091,
        IncarceratedRPRRequested = 28090,
        SendOOALetterandResDocCtyAttestRequired = 28093,
        NeedTransactionInquiry = 28094,
        Resubmission = 28095,
        TrueRejection = 28096,
        SubmitTransactionInquiry = 28097,
        RejectionOverturned = 28098,
        RejectionNotOverturned = 28099,
        FDRRPRReceived = 28100,
        FDRSCCRPRReceived = 28101
    }

    public enum OOAQueue : long
    {
        OOAAddressScrub = 10001,
        OOACMSAccepted = 10002,
        OOACMSRejected = 10003,
        OOACompleted = 10004,
        OOAMARxAddressLetter = 10005,
        OOAMIIMUpdated = 10006,
        OOANewCase = 10007,
        OOAOpenDisenroll = 10008,
        OOAOpenNOT = 10009,
        OOAPended = 10010,
        OOAPendingAudit = 10011,
        OOAPendingFTT = 10012,
        OOAPendingNOT = 10013,
        OOASubmitToCMS = 10014,
        OOAUpdateSentToCMS = 10015,
        OOAPeerAuditFailed = 10094,
        OOAPendingSCCRPR = 10107,
        OOAUpdateSentoCMSFAILED = 10114,
        OOALetterSentFAILED = 10115,
        OOANeedsEGHPReview = 10118,
        OOAOpenNOTMacro = 10120,
        OOAOpenDisenrollMacro = 10121
    }

    public enum SCCQueue : long
    {
        SCCAddressScrub = 10016,
        SCCCMSAccepted = 10017,
        SCCCMSRejected = 10018,
        SCCCompleted = 10019,
        SCCMARxAddressLetter = 10020,
        SCCMIIMUpdated = 10021,
        SCCNewCase = 10022,
        SCCOpenDisenroll = 10023,
        SCCOpenNOT = 10024,
        SCCPended = 10025,
        SCCPendingAudit = 10026,
        SCCPendingFTT = 10027,
        SCCPendingNOT = 10028,
        SCCPendingSCCRPR = 10029,
        SCCSubmitToCMS = 10031,
        SCCUpdateSentToCMS = 10032,
        SCCPeerAuditFailed = 10095,
        SCCPotentialSCCRPRDay1 = 10111,
        SCCNeedsEGHPReview = 10112,
        SCCPotentialSCCRPRDay2 = 10113,
        SCCUpdateSenttoCMSFAILED = 10116
    }

    public enum TRRQueue : long
    {
        TRRCMSRejected = 10033,
        TRRCMSRejectedDeletionCode = 10034,
        TRRCompleted = 10036,
        TRREscalated = 10037,
        TRRFalloutTRC085 = 10038,
        TRRFalloutTRC155 = 10039,
        TRRAddressScrub = 10100,
        TRRMARxAddressLetter = 10040,
        TRROpenNOT = 10103,
        TRROpenDisenroll = 10102,
        TRRPendingNOT = 10041,
        TRRPendingFTT = 10104,
        TRRPendingAudit = 10042,
        TRRSubmitToCMS = 10043,
        TRRSubmitToCMSDeletionCode = 10044,
        TRRTRC085 = 10045,
        TRRTRC15476 = 10046,
        TRRTRC155 = 10047,
        TRRTRC282 = 10048,
        TRRUpdateSentToCMS = 10049,
        TRRUpdateSentToCMSDeletionCode = 10050,
        TRRPended = 10090,
        TRRPendingSCCRPR = 10091,
        TRRCMSAccepted = 10092,
        TRRCMSAcceptedDeletionCode = 10093,
        TRRPeerAuditFailed = 10096,
        TRRTRC15401 = 10108,
        TRRUpdateSenttoCMSFAILED = 10117,
        TRRNeedsEGHPReview = 10119
    }
    //OST Holding Queues
    public enum OSTHoldingQueue : long
    {

        OOAPendingSCCRPR = 10107,
        SCCPendingSCCRPR = 10029,
        TRRPendingSCCRPR = 10091,
        OOAPendingNOT = 10013,
        SCCPendingNOT = 10028,
        TRRPendingNOT = 10041,
        OOAPendingFTT = 10012,
        SCCPendingFTT = 10027,
        TRRPendingFTT = 10104
    }
    //Used for Search screen edit button visibility
    public enum OSTHoldingQueues : long
    {
        OOAPendingNOT = 10013,
        OOAPendingFTT = 10012,
        OOAPendingSCCRPR = 10107,
        SCCPendingSCCRPR = 10029,
        TRRPendingSCCRPR = 10091,
        OOASubmittoCMS = 10014,
        OOAUpdateSenttoCMS = 10015,
        TRRSubmittoCMS = 10043,
        TRRSubmittoCMSDeletionCode = 10044,
        TRRUpdateSenttoCMS = 10049,
        TRRUpdateSenttoCMSDeletionCode = 10050,
        SCCSubmittoCMS = 10031,
        SCCUpdateSenttoCMS = 10032
    }

    public enum EligibilityQueue : long
    {
        EligCMSAccepted = 10051,
        EligCMSRejected = 10052,
        EligCompleted = 10053,
        EligNewCase = 10054,
        EligPended = 10055,
        EligPendingAudit = 10056,
        EligSubmitToCMS = 10057,
        EligUpdateSentToCMS = 10058,
        EligPeerAuditFailed = 10097
    }

    public enum DOBQueue : long
    {
        DOBCompleted = 10059,
        DOBNewCase = 10060,
        DOBPended = 10061,
        DOBPendingAudit = 10062,
        DOBPeerAuditFailed = 10099
    }

    public enum GenderQueue : long
    {
        GenderCompleted = 10063,
        GenderNewCase = 10064,
        GenderPended = 10065,
        GenderPendingAudit = 10066,
        GenderPeerAuditFailed = 10098
    }

    public enum RPRQueue : long
    {
        RPRCMSAccountManagerSent = 10067,
        RPRCMSRejectedDeletionCode = 10068,
        RPRCompleted = 10069,
        RPRInitialSCCRPR = 10070,
        RPRReceivedRPCFDR = 10071,
        RPRReceivedTRC282 = 10072,
        RPRPeerAudit = 10073,
        RPRPeerAuditFailed = 10074,
        RPRRejected = 10075,
        RPRRequestCategory2 = 10076,
        RPRRequestCategory3 = 10077,
        RPRReSubmission = 10078,
        RPRSubmissionCategory2 = 10079,
        RPRSubmissionCategory3 = 10080,
        RPRPended = 10081,
        RPRSCCRPRFDRReceived = 10082,
        RPRSCCRPRReSubmission = 10083,
        RPRSCCRPRSent = 10084,
        RPRSCCRPRSubmission = 10085,
        RPRSCCRPRTransactionInquiry = 10086,
        RPRSentToRPC = 10087,
        RPRSubmitToCMSDeletionCode = 10088,
        RPRUpdateSentToCMSDeletionCode = 10089,
        RPREligibilityUpdateInMARx = 10106,
        RPRTrend_2 = 10109,
        RPRRequestCategory2CTM = 10110,
        TransactionInquiry = 10122
    }

    public enum PermissionType : long
    {
        CanCreate = 1,
        CanModify = 2,
        CanSearch = 3,
        CanView = 4,
        CanMassUpdate = 5,
        CanHistory = 6,
        CanReassign = 7,
        CanUnlock = 8,
        CanUpload = 9,
        CanClone = 10,
        CanReopen = 11
    }

    public enum RPRActionRequested : long
    {
        DisenrollmentDateChangeDODError = 14001,
        EffectiveDateChange,
        EnrollmentCancellations,
        EnrollmentafterABDRejection,
        PBPChange,
        PlanErrorReinstatement,
        ABReinstatement,
        ReinstatementOther,
        RetroDisenrollment,
        RetroEnrollment,
        GoodCauseReinstatement,
        ReinstatementTRC14,
        Other,
        Disenrollment,
        Enrollment,
        Reinstatement,
        SCCRPR
    }
    public enum PendingAudit : long
    {
        OOAPendingAudit = 10011,
        SCCPendingAudit = 10026,
        TRRPendingAudit = 10042,
        EligPendingAudit = 10056,
        DOBPendingAudit = 10062,
        GenderPendingAudit = 10066,
        RPRPeerAudit = 10073
    }
    public enum FileType : long
    {
        UndeliveredMail = 40001,
        COA = 40002
    }
    public enum ImportStatus : long
    {
        ReadyForImport = 41001,
        ImportInprogress = 41002,
        ImportSuccessful = 41003,
        ImportFailed = 41004
    }
    public enum DiscrepancySource : long
    {
        TRR = 13001,
        MMR = 13002,
        Other = 13003,
        SingleCaseCreation = 13004,
        Xerox = 13005,
        GreyhairUSPS = 13006
    }

    public enum FDRSubmissionExcelSheets : int
    {
        INSTRUCTIONS = 1,
        Ret_Disenrl = 2,
        PBP = 3,
        Ret_Enrl = 4,
        REINSTMT = 5,
    }

    public enum SCCFDRSubmissionExcelSheets : int
    {
        INSTRUCTIONS = 1,
        SCC = 2,
    }

    public enum DiscripancyType : long
    {
        OOA = 7001,
        InArea = 7002,
        Incarcerated = 7003,
        CMSYPlanN = 7004,
        CMSNPlanY = 7005,
        Contract = 7006,
        PBP = 7007,
        DOB = 7008,
        Gender = 7009,
        RPR = 7010,
        SCCRPR = 7011
    }
    public enum ReportId : long
    {
        HomePageReport = 1,
        ErrorLogReport = 2,
        LockedRecordsReport = 3,
        AlertsHistoryReport = 4,
        SkillsHistoryReport = 5,
        UserAdminHistoryReport = 6,
        DepartmentHistoryReport = 7,
        LookupHistoryReport = 8,
        LookupCorrelationHistoryReport = 9,
        ResourceHistoryReport = 10,
        ConfigurationHistoryReport = 11,
        AccessGroupHistoryReport = 12,
        CommonHistoryReport = 13,
        GetQueueReport = 22,
        GetQueueSearchReport = 26,
        MassUpdateByTemplateReport = 33
    }

    public enum BackgroundProcessRecordStatus : long
    {
        Success = 56001,
        Failed = 56002
    }

    public enum QueueProgressType : long
    {
        Processing = 9001,
        Holding = 9002,
        Completed = 9003
    }
    public enum PendingQueues : long
    {
        OOAPended = 10010,
        SCCPended = 10025,
        EligPended = 10055,
        DOBPended = 10061,
        GenderPended = 10065,
        RPRPended = 10081,
        TRRPended = 10090
    }
    public enum AuditFailedQueues : long
    {
        RPRPeerAuditFailed = 10074,
        OOAPeerAuditFailed = 10094,
        SCCPeerAuditFailed = 10095,
        TRRPeerAuditFailed = 10096,
        EligPeerAuditFailed = 10097,
        GenderPeerAuditFailed = 10098,
        DOBPeerAuditFailed = 10099,
    }

    public enum MIIMUpdated : long
    {
        OOAMIIMUpdated = 10006,
        SCCMIIMUpdated = 10021
    }

    public enum FDRTransactionType : long
    {
        Reinstmt = 43001,
        RetDis = 43002,
        RetEnrl = 43003,
        SCC = 43004,
        PBP = 43005
    }


    public enum SourceSystemLkup : long
    {
        GPS = 38001,
        MIIM = 38002,
        UndeliveredMail = 38003,
        COA = 38004,
        ERN = 38005,
        ERS = 38006
    }
    public enum CMSTransactionCode : long
    {
        TRR76 = 76
    }
    public enum WebserviceStatus : long
    {
        Success = 46001,
        Failed = 46002
    }
    public enum WebserviceMethod : long
    {
        GetMemberDemographicalDetails = 47001,
        GetMemberEligibilityService = 47002,
        GetTRRDetails = 47003,
        GetTRRSummaryInfoService = 47004,
        CreateCMSTransactionService = 47005,
        MaintainOutOfAreaServiceService = 47006,
        GetEmployerSummary = 47007
    }

    public enum RPRCategory : long
    {
        Category2 = 49001,
        Category3 = 49002
    }

    public enum FDRSubmissionCategory : long
    {
        Category2 = 50001,
        Category2CTM = 50002,
        Category3 = 50003,
        ReSubmission = 50004,
        SCC = 50005
    }
    public enum TarceMethodLkup : long
    {
        New = 51001,
        InProgress = 51002,
        Completed = 51003,
        Failed = 51004
    }

    public enum MIIMServiceMethod : long
    {
        GetERSAllQueueDetailsByHICN = 52001,
        GetQueueIdFromMIIMReferenceId = 52002,
        PostOOAPermanentAddressTrackComments = 52003,
        GetCaseDetails = 52004,
        CreateRPRCase = 52005
    }

    public enum OOALetterStatus : long
    {
        Ready = 53001,
        InProgress = 53002,
        Success = 53003,
        Failure = 53004
    }

    public enum MQSourceTypeLkup : long
    {
        DataUpload = 54001,
        Queue = 54002,
        Topic = 54003
    }

    public enum CMSTransactionStatus : long
    {
        Ready = 55001,
        InProgress = 55002,
        Success = 55003,
        Failure = 55004
    }

    public enum BusinessSegment : long
    {
        MNR = 1001,//M&R
        CNS = 1002,//M&S
        PCP = 1003,//PCP
    }

    public enum Location : long
    {
        Onshore = 33001,
        Airoli = 33002,
        Bangalore = 33003,
        Cebu = 33004,
        Chennai = 33005,
        Hyderabad = 33006,
        Noida = 33007,
        Other = 33008
    }

    public enum SubmissionType : long
    {
        Category2 = 17001,
        Category3 = 17002,
        PlanErrorReinstatement = 17003,
        Other = 17004
    }

    public enum AdjustedCreateDateReason : long
    {
        EligibilityIssue = 32005,
    }

    public enum DefaultTimeZone : long
    {
        CentralStandardTime = 4005,
    }

    public enum ReOpenOOAHoldingQueue : long
    {
        PendingSCCRPR = 10107,
        SubmittoCMS = 10014,
        UpdateSenttoCMS = 10015,

    }

    public enum Lob : long
    {
        Erickson = 31001,
        Evercare = 31002,
        HarvardPilgrim = 31003,
        MA = 31004,
        Medica = 31005,
        Oxford = 31006,
        PDP = 31007,
        SCO = 31008,
        SCOT = 31009,
        SecureHorizons = 31010
    }

    public enum TRCCode : long
    {
        TRC16 = 22001,
        TRC17 = 22002,
        TRC85 = 22003,
        TRC138 = 22004,
        TRC154 = 22005,
        TRC155 = 22006,
        TRC257 = 22007,
        TRC258 = 22008,
        TRC259 = 22009,
        TRC260 = 22010,
        TRC261 = 22011,
        TRC265 = 22012,
        TRC266 = 22013,
        TRC282 = 22014,
        TRC283 = 22015,
        TRC305 = 22016
    } 

    public enum CreateBy : long
    {
        CreateByRef = 3,

    }
    public enum RequiredQueueList : long
    {
        PendingFTT = 10012,
        PendingNOT = 10013
    }

    public enum MacroType : long
    {
        NOTMacro = 61001,
        FTTMacro = 61002,
        TRC155Macro = 61003
    }

    public enum RemoveActionForMassUpdate : long
    {
        SendSCCUpdatetoCMS = 28017,
        UpdateCMSEligibility = 28025,
        UpdateSenttoCMS = 28048,
        SCCRPRRequired_SendSCCDeletiontoCMS = 28013,
        ReviewandReturntoQueue = 28084,
        OpenNOT = 28082,
        OpenDisenroll = 28083,
        PendingNOT = 28080,
        PendingFTT = 28081,
        UpdateComplianceStartDate = 28089,
        SendOOALetterandResDocCtyAttestRequired = 28093,
        ResidentialDocRequiredCountyAttestationRequired = 28008
    }

    public enum DynamicCancelPendCase : long
    {
       
        Cancel_Pend_Case = 28091
     
    }


    public enum TemplateType : long
    {
        BulkUpload= 62001,
        MassUpdate=62002
    }
    public enum BulkImportExcelTemplateMaster : long
    {
        OOATemplate = 1,
        SCCTemplate=2,
        TRRTemplate=3,
        EligibilityTemplate=4,
        DOBTemplate=5,
        GenderTemplate=6,
        MassUpdateTemplate=7
    }
}
