// Array Remove - By John Resig
Array.prototype.remove = function (from, to) {
    var rest = this.slice((to || from) + 1 || this.length);
    this.length = from < 0 ? this.length + from : from;
    return this.push.apply(this, rest);
};

//start and end trim for string
if (String.prototype.trim == null) {
    String.prototype.trim = function () {
        return this.replace(/^([\s\uFEFF\xA0]|(\<P\>\&nbsp\;\<\/P\>))+|([\s\uFEFF\xA0]|(\<P\>\&nbsp\;\<\/P\>))+$/g, '');
    }
}


function Get2DigitText(number) {
    var txtNum = (number).toString();
    if (number < 10) {
        txtNum = "0" + number;
    }

    return txtNum;
}

function getIntegerFromText(numString) {
    var rexNumber = /[^0]\d*/g;

    if (resultString = rexNumber.exec(numString)) {
        if (resultString[0] != "") {
            return parseInt(resultString[0]);
        }
    }
    return 0;
}

//get table column value by key
function getTableValue(list, idName, idValue, fieldName) {
    if (list != null) {
        var row = $.grep(list, function (n) {
            return n[idName] == idValue;
        });
        if (row.length > 0) {
            return row[0][fieldName];
        }
    }
    return "";
}

function RefreshWindowWidthHeight() {
    if (document.body && document.body.offsetWidth) {
        winW = document.body.offsetWidth;
        winH = document.body.offsetHeight;
    }
    if (document.compatMode == 'CSS1Compat' &&
    document.documentElement &&
    document.documentElement.offsetWidth) {
        winW = document.documentElement.offsetWidth;
        winH = document.documentElement.offsetHeight;
    }
    if (window.innerWidth && window.innerHeight) {
        winW = window.innerWidth;
        winH = window.innerHeight;
    }
}

function ShowPopup(divContent, divProgress) {
    divContent.show();
    if (divProgress != null) {
        divProgress.show();
    }
    RefreshWindowWidthHeight();
    var ofset = divContent.offset();
    var scrollTop = $(window).scrollTop();
    //alert("wid:" + winW + ",winH" + winH);
    var defaultPopupHeight = 300;
    $(divContent).offset({ top: scrollTop + (winH - defaultPopupHeight) / 2, left: (winW - divContent.width()) / 2 });
}



var enums = {
    //    PopupArguments: {
    //        pageName: 0,
    //        callBackFunction: 1,
    //        gridObserver: 2,
    //        vmContext: 3, //pass vmRow for grid edit
    //        mode: 4,  //add/edit
    //        saveCallBackFunction: 5 //save call back function
    //    },
    RowState: {
        Added: 4,
        Modified: 16,
        Detached: 1
    },
    DateParts: {
        DPart: "_DPart",
        TPart: "_TPart",
        ZPart: "_ZPart"
    },
    UserControlLkup: {
        _Gridcontrol: 57003,
        _GridColumn: 57004,
        _Panel: 57001
    },
    OperatorLkup: {
        LessThan: 61000,
        GreaterThan: 61001,
        Equal: 61002,
        LessThanOrEqual: 61003,
        GreaterThanOrEqual: 61004,
        Function: 61005,
        And: 61006,
        Or: 61007,
        SubTree: 61008,
        Plus: 61009,
        Minus: 61010,
        Multiply: 61011,
        DividedBy: 61012,
        ReturnIf: 61013,
        ElseIfLadder: 61014,
        Not: 61015,
        RootNode: 61016
    },
    OperandTypeLkup: {
        ViewModel: 62000,
        ViewModelObserver: 62001,
        ViewModelObserverValue: 62002,
        Literal: 62003,
        DecisionTreeResultAsParameter: 62004
    },
    UpdateMode: {
        ADD: 'ADD',
        EDIT: 'EDIT',
        READONLY: 'READONLY'
    },
    SlotLkup: {
        IsA: 68000,
        //InstanceOf : 68001,
        Validations: 68013,
        Columns: 68014,
        ColumnName: 68015,
        Triggers: 68009,
        TriggersEvents: 68010,
        EventsActions: 68011,
        DisplayName: 68024
    },
    SlotValueTypeLkup: {
        SingleFrame: 69001,
        DecisionTreeResult: 69003,
        FrameReference: 69004
    },
    FrameIds: {
        OnPageLoadBeforeBinding: 48709,
        OnPageLoad: 391,
        OnEditPageLoad: 4681,
        OnBeforeValidation: 69711,
        OnSaveTrigger: 38
    },
    UploadFileFormats: ",PDF,",
    //    CompareOperator: {
    //        EqualTo: 1,
    //        LessThan: 2,
    //        LessThanOrEqualTo: 3,
    //        LikeEnd: 4,
    //        LikeBeginEnd: 5,
    //        GreaterThan: 6,
    //        GreaterThanOrEqualTo: 7
    //    }
    ZoneLkups: {
        2000: { isDayLight: false, timeZoneOffset: -8, alternateLkup: 2001 },//Pacific Standard Time
        2001: { isDayLight: true, timeZoneOffset: -7, alternateLkup: 2000 },
        2002: { isDayLight: false, timeZoneOffset: -7, alternateLkup: 2003 },
        2003: { isDayLight: true, timeZoneOffset: -6, alternateLkup: 2002 },
        2004: { isDayLight: false, timeZoneOffset: -6, alternateLkup: 2005 },
        2005: { isDayLight: true, timeZoneOffset: -5, alternateLkup: 2005 },
        2006: { isDayLight: false, timeZoneOffset: -5, alternateLkup: 2007 },
        2007: { isDayLight: true, timeZoneOffset: -4, alternateLkup: 2006 },
        2008: { isDayLight: null, timeZoneOffset: 5.5, alternateLkup: null },
        2009: { isDayLight: null, timeZoneOffset: -7, alternateLkup: null }
    },
    RecordValidationLkup: {
        ValidationMet: 55000,
        ValidationNotMet: 55001,
        DataMissing: 55002
    },
    DecisionTree: {
        SearchSpecialist: 1004,
        BindSpecialistDetails: 996
    },
    CurrentUserRef: 0,
    CurrentUserName: 1,
    CompareDateToSetLetterType: '01/01/2016',
    PrintMethodLkup: {
        PrintVendor: 72002
    }
    ,
    DecisionTreeFilledFileds: {
        CustomerServiceDays: "CustomerServiceDays",
        CustomerServiceHours: "CustomerServiceHours",
        NewReturnAddress: "NewReturnAddress",
        PlanLogoURL: "PlanLogoURL",
        ErsDisclaimersLkup: "ErsDisclaimersLkup",
        ErsPeehipLogoUrl: "ErsPeehipLogoUrl",
        ErsPeehipLogoLkup: "ErsPeehipLogoLkup",
        RestrictedAddressLine1: "RestrictedAddressLine1",
        RestrictedAddressLine2: "RestrictedAddressLine2",
        RestrictedCity: "RestrictedCity",
        RestrictedState: "RestrictedState",
        RestrictedZip: "RestrictedZip",
        EmployeRgrPNbr: "EmployeRgrPNbr"
    }
    , CallbackMethod: {
        Override: 38001,
        Manual: 38006
    }
    , NotificationMethod: {
        Print: 13000,
        Fax: 13001,
        Email: 13002
    }
    , LetterTemplate: {      
        AppealExpeditedDenied: 1000,
        AppealRequestforAdditionalDocumentation: 1001,
        AppealRequestforAORPOA: 1002,
        AppealAckGrievanceExpeditedFromPlanDecision: 1003,
        AppealUphold: 1004,
        AppealResolvedorDismissed: 1005,
        AppealForwardtoIREforUntimelyDecision: 1006,
        AppealOTDecisionsbyPlanFollowingIRE: 1007,
        AppealOverturnDecision: 1008,
        AppealOverturnbyIRE: 1009,
        AppealPastTimelyFiling: 1010,
        AppealWithdrawalRequest: 1011,
        AppealGoodwillandEducate: 1012,
        PartDCoverageDetermination: 1013,
        Appeal2ndRequestforAdditionalInformation: 1016,
        RequestforReconsideration: 1017,
        RequestforRedeterminationForm: 1018,
        MedicalRecordsRelease: 1019,
        AppointmentofRepresentativeForm: 1020,
        CCCoverLetter: 1021,
        AORInstructionSheet: 1022,
        StatementofAppealWithdrawal: 1023,
        MMPAppealRequestforAdditionalDocumentation: 1026,
        MMPAppealRequestforAORPOA: 1027,
        MMPAppealUphold: 1029,
        MMPAppealResolvedorDismissed: 1030,
        MMPAppealForwardtoIREforUntimelyDecision: 1031,
        MMPAppealOTDecisionsbyPlanFollowingIRE: 1032,
        MMPAppealOverturnDecision: 1033,
        MMPAppealOverturnbyIRE: 1034,
        MMPAppealGoodwillandEducate: 1037,
        MMPWaiverofLiabilityStatementForm: 1038,
        MMPAppeal2ndRequestforAdditionalInformation: 1041,
        MMPAppealExtensionNotice: 1042,
        MMPAORInstructionSheet: 1043,
        MMPRequestforReconsideration: 1044,
        MMPRequestforRedeterminationForm: 1045,
        MMPMedicalRecordsRelease: 1046,
        MMPAppointmentofRepresentativeForm: 1047,
        MMPStatementofAppealWithdrawal: 1048,
        AppealExtensionNotice: 1049,
        MailMergeWOL1stRequest: 1050,
        MailMergeAOR1stRequest: 1051,
        MailMergeAOR2ndRequest: 1052,
        MailMergeDismissalAOR: 1053,
        MailMergeGoodCause: 1054,
        MailMergeWOL2ndRequest: 1055,
        MailMergeDismissalWOL: 1056,
        WOL1stRequest: 1057,
        AOR1stRequest: 1058,
        AOR2ndRequest: 1059,
        DismissalAOR: 1060,
        GoodCause: 1061,
        WOL2ndRequest: 1062,
        DismissalWOL: 1063,
        AppealStatusNotification: 1064,
        MMPAppealStatusNotification: 1065,
        MAMMPAppealAcknowledgement: 1067,
        NJMMPAppealAcknowledgement: 1068,
        OHIOMMPAppealAcknowledgement: 1069,
        TEXASMMPAppealAcknowledgement: 1070,
        AandGWithdrawalRequest: 1071,
        PartCInquiry: 1072,
        MedicalRecordRequestForm: 1073,
        AdhocLetter: 1074,
        DIS2_DisputePreviouslyProcessed: 1075,
        AppealGrievanceWithdrawalRequest: 1076,
        DIS3_MedicalRecordRequestLetter: 1077,
        DIS4_UnclearDisputeLetter: 1078,
        DIS6_DisputeUpheldLetterClaimPaid: 1079,
        DIS7_UpholdLetterPAOTimelyFiling: 1080,
        DIS8_OverturnLetterPAO: 1081,
        DIS11_NonParProviderInquiry: 1082,
        DIS12_MissedTimelyFilingforDisputeFinal: 1083,
        DIS13_Non_ParOverpaymentLetter: 1084,
        StatementOfWithdrawal: 1085,
        AORInstructionSheet: 1086,
        GrievanceQOSResolutionClosure: 1087,
        GrievanceQOCResolutionClosure: 1088,
        AppealGrievanceAckGADCH: 1089,
        AppealGrievanceRequestForAORPOA: 1090,
        GrievanceAppointmentofRepresentativeForm: 1091
    }
    , YesOrNoDropDown: {
        Yes: 10011066,
        No: 10011067
    },
    CCType: {
        MemberCC: 79002
    },
    AddressType: {
        MemberAddress: 80000
    }
    , LetterGenerationFailed: 19003,
    PrintFailed: 28003,
    FaxFailed: 20003,
    EmailFailed: 21003,
    PrintQueue: 72001,
    NetworkShare: 72000,
    //Regular expression to check comments.
    CommentsRegularExpression: /[a-z]+/i

};


//get 12 hour format
function getFormattedTime(hours24, minutes24) {
    var hours = hours24 == 0 ? 12 : hours24 > 12 ? hours24 - 12 : hours24;
    if (hours < 10) {
        hours = "0" + hours;
    }
    var minutes = (minutes24 < 10 ? "0" : "") + minutes24;
    var ampm = hours24 < 12 ? "AM" : "PM";
    var formattedTime = hours + ":" + minutes + " " + ampm;
    return formattedTime;
}

function get2DigitString(num) {
    var result = num;
    if (num <= 9) {
        result = "0" + num;
    }
    return result;
}

//call this method only for timezones which has day light saving shifting
function getLocalTime(utcTime, timeZone) {
    var result = new Object();
    //find time zone offset
    var zoneStd = enums.ZoneLkups[timeZone];
    var zoneStdLkup = timeZone;
    if (zoneStd.isDayLight) {
        zoneStdLkup = zoneStd.alternateLkup;
        zoneStd = enums.ZoneLkups[zoneStd.alternateLkup];
    }

    //time without day light saving
    var localTime1 = new Date(utcTime.getTime());
    localTime1.setMinutes(utcTime.getMinutes() + zoneStd.timeZoneOffset * 60);
    if (zoneStd.isDayLight == null) {
        //day light savings are not applicable in this time zone
        result.time = localTime1;
        result.isDayLight = null;
        result.timeZoneLkup = timeZone;
        return result;
    }
    //is day light saving?
    //find date of second Sunday in March 2:00 AM
    //var days = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'];
    //calculate day light start time
    var dayLighStart = new Date(localTime1.getYear(), 2, 8, 2, 0, 0, 0);
    //search for second sunday in march
    var dayOfWeek = dayLighStart.getDay();

    while (dayOfWeek != 0) {
        //move to next day
        dayLighStart.setMinutes(dayLighStart.getMinutes() + 1440);
        dayOfWeek = dayLighStart.getDay();
    }
    //calculate day light end time
    var dayLightEnd = new Date(localTime1.getYear(), 10, 1, 1, 0, 0, 0);
    //search for first sunday of november
    dayOfWeek = dayLightEnd.getDay();
    while (dayOfWeek != 0) {
        //move to next day
        dayLightEnd.setMinutes(dayLightEnd.getMinutes() + 1440);
        dayOfWeek = dayLightEnd.getDay();
    }

    if (localTime1 < dayLighStart) {
        result.time = localTime1;
        result.isDayLight = false;
        result.timeZoneLkup = zoneStdLkup;
        return result;
    }
    else if (localTime1 < dayLightEnd) {
        //add one hour
        localTime1.setMinutes(localTime1.getMinutes() + 60);
        result.time = localTime1;
        result.isDayLight = true;
        result.timeZoneLkup = zoneStd.alternateLkup;
        return result;
    }
    else {
        result.time = localTime1;
        result.isDayLight = false;
        result.timeZoneLkup = zoneStdLkup;
        return result;
    }
}

function getUTCTime(localTime, timeZone) {
    var zoneStd = enums.ZoneLkups[timeZone];

    var utcTime = new Date(localTime.getTime());
    utcTime.setMinutes(localTime.getMinutes() - zoneStd.timeZoneOffset * 60);
    return utcTime;
}

function getCurrentLocalTime(timeZone) {
    var utcTime = getCurrentUTCTime();
    return getLocalTime(utcTime, timeZone);
}
//to get current UTC time
function getCurrentUTCTime() {
    var tmLoc = new Date();
    //The offset is in minutes -- convert it to ms
    var utcTime = new Date(tmLoc.getTime() + tmLoc.getTimezoneOffset() * 60000);
    return utcTime;
}

//returns true if dateTimeForCheck <= current date
function checkIfFutureDateUTC(UTCDateTimeForCheck) {
    var CurrentDateTime = getCurrentUTCTime();
    if (UTCDateTimeForCheck != '' && new Date(UTCDateTimeForCheck) != new Date(CurrentDateTime) && new Date(UTCDateTimeForCheck) <= new Date(CurrentDateTime)) {
        return false;
    }
    else {
        return true;
    }
}

//returns true if dateTimeForCheck < current date
function checkIfCurrentOrFutureDateUTC(UTCDateTimeForCheck) {
    var CurrentDateTime = getCurrentUTCTime();
    if (UTCDateTimeForCheck != '' && new Date(UTCDateTimeForCheck) != new Date(CurrentDateTime) && new Date(UTCDateTimeForCheck) < new Date(CurrentDateTime)) {
        return false;
    }
    else {
        return true;
    }
}


//Setting MaxLength for Multiline textBox
function MaxLength(id, maxLength) {
    if (id.value.length < maxLength) {
        return true;
    }
    if (id.value.length == maxLength) {
        return true;
    }
    if (id.value.length > maxLength) {
        id.value = id.value.substring(0, maxLength);
        return false;
    }
}

function isSpecialKey(KeyCode) {

    var rtn = false;

    switch (KeyCode) {

        case 13:
            rtn = true;
            break;
        case 96://`
            rtn = true;
            break;
        case 126://~
            rtn = true;
            break;
        case 33://!
            rtn = true;
            break;
        case 64://@
            rtn = true;
            break;
        case 35://#
            rtn = true;
            break;
        case 36://$
            rtn = true;
            break;
        case 37://%
            rtn = true;
            break;
        case 94://^
            rtn = true;
            break;
        case 38://&
            rtn = true;
            break;
        case 42://*
            rtn = true;
            break;
        case 40://(
            rtn = true;
            break;
        case 41://)
            rtn = true;
            break;
        case 45://-
            rtn = true;
            break;
        case 95://_
            rtn = true;
            break;
        case 43://+
            rtn = true;
            break;
        case 61://=
            rtn = true;
            break;
        case 91://[
            rtn = true;
            break;
        case 93://]
            rtn = true;
            break;
        case 123://{
            rtn = true;
            break;
        case 125://}
            rtn = true;
            break;
        case 59://;
            rtn = true;
            break;
        case 58:
            rtn = true;
            break;
        case 39:
            rtn = true;
            break;
        case 34://"
            rtn = true;
            break;
        case 44:
            rtn = true;
            break;
        case 60://<
            rtn = true;
            break;
        case 62://>
            rtn = true;
            break;
        case 92:
            rtn = true;
            break;
        case 47:///
            rtn = true;
            break;
        case 63://?
            rtn = true;
            break;
    }

    return rtn;
}

function isCaseValidStars(CaseNumber) {
    if (CaseNumber.toLowerCase().substring(0, 2) == 'st') {
        if (CaseNumber.length != 10) {
            return false;
        }
        else {
            for (var i = 2; i < CaseNumber.length; i++) {
                var charcode = CaseNumber.charCodeAt(i);
                if (isCharAlphaNumeric(charcode) == false) {
                    return false;
                }
            }
        }
        return true;
    }
    else {
        return false;
    }
}

function isCaseValidNavigator(CaseNumber) {
    var NavCaseLen = 11;//can be moved to config
    if (CaseNumber.substr(3, 1) == '-') {
        if (CaseNumber.length <= (Number(NavCaseLen) + 1) && CaseNumber.length >= 9) {
            CaseNumber = CaseNumber.substr(0, 3) + CaseNumber.substr(4, (Number(NavCaseLen) - 3));
        }
        else {
            return false;
        }
        if (CaseNumber.toLowerCase().substring(0, 3) != 'mbr') {
            return false;
        }
        else {
            for (var i = 3; i < CaseNumber.length; i++) {
                var charcode = CaseNumber.charCodeAt(i);
                if (isCharAlphaNumeric(charcode) == false) {
                    return false;
                }
            }
        }

        return true;
    }
    else if (CaseNumber.substr(3, 1) != '-') {
        if (CaseNumber.length > Number(NavCaseLen) || CaseNumber.length < 8)
            return false;
        if (CaseNumber.toLowerCase().substring(0, 3) != 'mbr') {
            return false;
        }
        else {
            for (var i = 3; i < CaseNumber.length; i++) {
                var charcode = CaseNumber.charCodeAt(i);
                if (isCharAlphaNumeric(charcode) == false) {
                    return false;
                }
            }
        }
        return true;
    }
    else {
        return false;
    }
}
function isCaseValidiCare(CaseNumber) {
    if (CaseNumber.toLowerCase().substring(0, 2) != 'c-') {
        return false;
    }
    else {
        for (var i = 2; i < CaseNumber.length; i++) {
            var char = CaseNumber[i];
            if (isNumeric(char) == false) {
                return false;
            }
        }
    }
}
function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}
function isCharAlphaNumeric(chrCode) {
    if (!((chrCode >= 48 && chrCode <= 57) || (chrCode >= 65 && chrCode <= 90) || (chrCode >= 97 && chrCode <= 122) || chrCode == 32)) {
        return false;
    }
    return true;
}

function validateMultipleEmailsCommaSeparated(email, seperator) {
    var value = email;
    if (value != '') {
        var result = value.split(seperator);
        for (var i = 0; i < result.length; i++) {
            if (result[i] != '') {
                if (!isProperEmailFormat(result[i])) {
                    return false;
                }
            }
        }
    }
    return true;
}

function isProperEmailFormat(email) {
    var regex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,5}$/;
    return regex.test(email);
}

function clone(val) {
    var strJson = JSON.stringify(val);
    return $.parseJSON(strJson);

}