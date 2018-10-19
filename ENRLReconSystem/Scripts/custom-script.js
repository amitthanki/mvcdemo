/*=== Expandable-Collapsible Script ===*/
//Alert!/Success, Warning!, Confirm!, Error!
var $body = $("body");
var MainLayout = {};
var isSessionPoupShowed = false;
(function ($) {
    MainLayout = {
        fnAlertMessege: function (title, Message, DoCallBack) {
            try {
                var AlertBox = $('<div style="font-size:12px;"></div>').appendTo('body')
                    .html('<div>' + Message + '</div>')
                    .dialog({
                        draggable: true,
                        modal: true,
                        resizable: false,
                        autoOpen: false,
                        hide: { effect: "scale", duration: 500 },
                        title: title,
                        buttons: {
                            OK: function () {
                                $(this).dialog('close');
                            }
                        },
                        close: function () {
                            if (typeof (DoCallBack) != 'undefined') {
                                DoCallBack();
                            }
                            $(this).dialog('close');
                        }
                    });
                return AlertBox.dialog("open");

            } catch (e) {

            }
        },
        fnConfirmDialogbox: function (title, Message, DoCallBack) {
            try {
                var confirmdialog = $('<div style="font-size:12px;"></div>').appendTo('body')
                    .html('<div>' + Message + '</div>')
                    .dialog({
                        draggable: true,
                        modal: true,
                        resizable: false,
                        autoOpen: false,
                        hide: { effect: "scale", duration: 500 },
                        title: title,
                        buttons: [{
                            text: "Yes",
                            click: function () {
                                DoCallBack(true);
                                $(this).dialog("close");
                            }
                        },
                        {
                            text: "No",
                            click: function () {
                                DoCallBack(false);
                                $(this).dialog("close");
                            }
                        }]
                    });
                return confirmdialog.dialog("open");
            } catch (e) {

            }
        },
        fnShowProgressSpinner: function () {
            $body.addClass("loadingExtended");
        },
        fnHideProgressSpinner: function () {
            $body.removeClass("loadingExtended");
        },
        fnResetValidationSummary: function () {
            try {
                $('.validation-summary-errors ul').empty();// clear the error messages
                $('.input-validation-error').addClass('input-validation-valid');
                $('.input-validation-error').removeClass('input-validation-error');
                //Removes validation message after input-fields
                $('.field-validation-error').addClass('field-validation-valid');
                $('.field-validation-error').removeClass('field-validation-error');
                //Removes validation summary 
                $('.validation-summary-errors').addClass('validation-summary-valid');
                $('.validation-summary-errors').removeClass('validation-summary-errors');
            }
            catch (e) {
            }

        },
        fnInitialiseDataTable: function (tableID) {
            try {
                var oDataTable = tableID.DataTable({
                    "dom": "<'row'<'col-sm-12'tr>>" +
                    "<'row cmnDatatablePager'<'col-md-3'l><'col-md-4'<'text-center'i>><'col-md-5'p>>",
                    "oLanguage": {
                        "sEmptyTable": "No data available"
                    },
                    stateSave: true,
                    order: [],
                    columnDefs: [{ orderable: false, targets: [0] }]
                });
                return oDataTable;

            } catch (e) {

            }
        },
        fnInitialiseDataTableforMostRecentCases: function (tableID) {
            try {
                var oDataTable = tableID.DataTable({
                    "dom": "<'row'<'col-sm-12'tr>>", 
                    "oLanguage": {
                        "sEmptyTable": "No data available"
                    },
                    stateSave: true,
                    order: [],
                    columnDefs: [{ orderable: false, targets: [0] }]
                });
                return oDataTable;

            } catch (e) {

            }
        },
        fnLoadDateTimePicker: function () {
            $(".datepicker").datepicker({
                format: "mm/dd/yy",
                yearRange: "-100:+5",
                changeMonth: true,
                changeYear: true,
                onClose: function (value) {
                    this.value = value;
                }
            });

            $('.datepicker').mask('99/99/9999', {
                placeholder: 'mm/dd/yyyy'
            });
            $('.datepicker').blur(function () {
                var correctDate = true;
                var inputValue = this.value;//$('#datepicker').val();
                var validformat = /^\d{1,2}\/\d{1,2}\/\d{4}$/ //Basic check for format validity    
                if (inputValue !== "" && inputValue !== "__/__/____" && inputValue !== "mm/dd/yyyy") {
                    if (!validformat.test(inputValue))//alert('Invalid Date Format. Please correct.')            
                        correctDate = false;
                    else { //Detailed check for valid date ranges
                        var dateArray = inputValue.split("/");
                        var dayfield = dateArray[1]
                        var monthfield = dateArray[0]
                        var yearfield = dateArray[2]

                        var dayobj = new Date(yearfield, monthfield - 1, dayfield)
                        if ((dayobj.getMonth() + 1 !== parseInt(monthfield)) || (dayobj.getDate() !== parseInt(dayfield)) || (dayobj.getFullYear() !== parseInt(yearfield)))
                            correctDate = 'Invalid Day, Month, or Year range detected. Please correct.';
                        else if (new Date(inputValue) < new Date("01/01/1900")) {
                            correctDate = false;
                        }
                        else {
                            correctDate = true;
                        }
                    }
                    if (correctDate !== true) {
                        alert('Invalid Date Format. Please correct.');
                        //this.val("");
                        //currentObserver("");
                        //$('#datepicker').val('')
                        this.value = '';
                        this.focus();
                        //$('#datepicker').focus();
                        return false;
                    }
                }
            });
            $('.timepicker').mask('99:99 AM', {
                placeholder: '__:__ AM'
            });

            $('.timepicker').keypress(function () {
                var oldValue = this.value;
                var newValue = oldValue.replace('p', 'P');
                newValue = newValue.replace('a', 'A');
                newValue = newValue.replace('m', 'M');
                if (oldValue !== newValue) {
                    //this.val(newValue);
                    this.value = newValue;
                }
            });

            $('.timepicker').blur(function () {
                var fieldValue = this.value;
                var errorMsg = "";

                // regular expression to match required time format
                re = /^([0][1-9]|[1][0-2]):([0-5]\d)\s?(?:AM|PM)$/i;

                if (fieldValue !== '') {
                    var patt = new RegExp(re);
                    if (!patt.test(fieldValue)) {
                        if (!(fieldValue === '__:__ AM' || fieldValue === '__:__ PM')) {
                            errorMsg = "Invalid time format: " + fieldValue;
                        }
                    }
                }

                if (errorMsg !== "") {
                    alert(errorMsg);
                    this.value = '';
                    this.focus();
                }
            });
        },
        fnInitialiseZipCode: function () {
            $('.zip').mask('99999?-9999')
            $('.zip').blur(function () {
                var fieldValue = this.value;
                var errorMsg = "";
                if (fieldValue != '') {
                    var txtZipcodeLen = fieldValue.length;
                    if ((txtZipcodeLen != 10) && (txtZipcodeLen != 5)) {
                        errorMsg = "Please enter a valid Zip.";
                    }
                }
                if (errorMsg != "") {
                    alert(errorMsg);
                    this.focus();
                }
            });
        },
        fnInitialisePhoneNumber: function () {
            $('.phone').mask('9?99-999-9999')
            $('.phone').blur(function () {
                var fieldValue = this.value;
                var errorMsg = "";
                if (fieldValue != '') {
                    var txtPhonecodeLen = fieldValue.length;
                    if (txtPhonecodeLen != 12) {
                        errorMsg = "Please enter a valid Phone number.";
                    }
                }
                if (errorMsg != "") {
                    alert(errorMsg);
                    this.focus();
                }
            });
        },
        fnInitialiseFaxNumber: function () {
            $('.fax').mask('9?99-999-9999')
            $('.fax').blur(function () {
                var fieldValue = this.value;
                var errorMsg = "";
                if (fieldValue != '') {
                    var txtFaxcodeLen = fieldValue.length;
                    if (txtFaxcodeLen != 12) {
                        errorMsg = "Please enter a valid Fax number.";
                    }
                }
                if (errorMsg != "") {
                    alert(errorMsg);
                    this.focus();
                }
            });
        },
        fnDaysBetween: function (date1, date2) {
            //our custom function with two parameters, each for a selected date
            diffc = date1.getTime() - date2.getTime();
            //getTime() function used to convert a date into milliseconds. This is needed in order to perform calculations.
            days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
            //this is the actual equation that calculates the number of days.
            return days;
        },
        fnUnlockRecordMIIM: function (caseId, unlockUrl) {//For MIIM popup fire and forget ajax call to unlock record
            var promptText = confirm("This window will be closed. Do you want to continue ?");
            if (!promptText) {
                event.preventDefault();
            } else {
                if (caseId != 0 && unlockUrl != "") {
                    $.ajax({
                        async: false,
                        data: {
                            'caseId': recordId,
                            'screenLkup': enums.ScreenType.Queue
                        },
                        url: unlockUrl,
                        type: "POST"
                    });
                }

                $.ajax({
                    async: false,
                    url: urlLogout,//urlLogout is avaliable from Layout page
                    type: "GET"
                });
                //window.open('', '_self').close()
            }
        },
        fnMyOpenWindow: function (winURL, winName, winFeatures, winObj) {//For history Report: check if the report is already opened in a window
            var theWin; // this will hold our opened window

            // first check to see if the window already exists
            if (winObj != null) {
                // the window has already been created, but did the user close it?
                // if so, then reopen it. Otherwise make it the active window.
                if (!winObj.closed) {
                    winObj.close();
                    theWin = window.open(winURL, winName, winFeatures);
                    theWin.focus();
                    return theWin;
                }
                // otherwise fall through to the code below to re-open the window
            }

            // if we get here, then the window hasn't been created yet, or it
            // was closed by the user.
            theWin = window.open(winURL, winName, winFeatures);
            return theWin;

        },
        //function to get enum key by value
        getEnumKey: function (set, value) {
            for (var k in set) {
                if (set.hasOwnProperty(k)) {
                    if (set[k] == value) {
                        return k;
                    }
                }
            }
            return "";
        },
        //Function to create dynamic drop downlist
        //id - jqueryselector,optionlist- array,dropdownDefaultValue - default selected text,
        //textProperty - object property name for text, valueProperty - object property name for text
        //returns count of list items
        getDropDownList: function (id, optionList, dropdownDefaultValue, textProperty, valueProperty) {
            var combo = $(id);
            combo.empty();
            combo.append("<option value=''>" + dropdownDefaultValue + "</option>");
            if (optionList.length > 0) {
                $.each(optionList, function (i, el) {
                    if (el != null && el[valueProperty] != null && el[valueProperty] != "")
                        combo.append("<option value='" + el[valueProperty] + "'>" + el[textProperty] + "</option>");
                });
                $(id).append(combo);
                return combo.length;
            } else {
                $(id).append(combo);
                return 0;
            }
        },
        fnSetLocalStorage: function (value, Key) {
            try {
                MainLayout.fnClearLocalStorage(Key);
                sessionStorage[Key] = value;

            } catch (e) {
                throw e;
            }
        },
        fnGetLocalStorage: function (Key) {
            try {
                return sessionStorage[Key];
            } catch (e) {
                throw e;
            }
        },
        fnClearLocalStorage: function (Key) {
            try {
                sessionStorage[Key] = null;
            } catch (e) {
                throw e;
            }
        },
        fnIsLocalStorageExists: function (key)
        {
            var isSuccess = false;
            try {

                if (sessionStorage[key] != undefined && sessionStorage[key] != null && sessionStorage[key] != 'null' && sessionStorage[key] != "")
                {
                    isSuccess = true;
                }
                return isSuccess;
            } catch (e) {
                throw e;
            }
        },
        fnClearStorage: function () {
            try {
                localStorage.clear();
                sessionStorage.clear();
            } catch (e) {
                throw e;
            }
        },
        fnGetPBP : function (contNo,ctrlId) {
            try {
                $.ajax({
                    data: { "ContractNumber": contNo },
                    url: urlGetPBP,
                    async: false,
                    type: "POST",
                    success: function (data) {
                        if (data != null && data.length > 0) {
                            $(ctrlId).empty().append($("<option></option>").val("").html(enums.DropdownDefaultValue));
                            $.each(data, function (index, item) {
                                $(ctrlId).append($("<option></option>").val(item.CMN_LookupMasterChildRef).html(item.LookupMasterChildValue));
                            })
                        }
                    }
                })
            } catch (e) {
                throw e;
            }
        }
///////////
    }

})(jQuery);
////Load Date Time Picker////
var _dPart = "_DPart";
var _tPart = "_TPart";
var _zPart = "_ZPart";
MainLayout.fnLoadDateTimePicker();
///////////Load ZipCode/////////////////
MainLayout.fnInitialiseZipCode();
MainLayout.fnInitialisePhoneNumber();//load phone number
MainLayout.fnInitialiseFaxNumber();//load fax number

////

//Reseting form data
jQuery.fn.deserialize = function (data) {
    var f = this,
        map = {},
        find = function (selector) { return f.is("form") ? f.find(selector) : f.filter(selector); };
    //Get map of values
    jQuery.each(data.split("&"), function () {
        var nv = this.split("="),
            n = decodeURIComponent(nv[0]),
            v = nv.length > 1 ? decodeURIComponent(nv[1]) : null;
        if (!(n in map)) {
            map[n] = [];
        }
        map[n].push(v);
    })
    //Set values for all form elements in the data
    jQuery.each(map, function (n, v) {
        find("[name='" + n + "']").val(v);
    })
    //Clear all form elements not in form data
    find("input:text,select,textarea").each(function () {
        if (!(jQuery(this).attr("name") in map)) {
            jQuery(this).val("");
        }
    })
    find("input:checkbox:checked,input:radio:checked").each(function () {
        if (!(jQuery(this).attr("name") in map)) {
            this.checked = false;
        }
    })
    return this;
};


$(document).ready(function () {
    // Tggle plus minus icon on show hide of collapse element
    $(".collapse").on('show.bs.collapse', function () {
        $(this).parent().find(".fa").removeClass("fa-angle-down").addClass("fa-angle-up");
    }).on('hide.bs.collapse', function () {
        $(this).parent().find(".fa").removeClass("fa-angle-up").addClass("fa-angle-down");
    });
});

///*=== Datepicker Script ===*/			
//$('.datepicker').datepicker()

/*=== Modal Script ===*/
$(document).ready(function () {
    $("a.btn").click(function () {
        $("#myModal").modal('show');
    });
});

var enums = {
    ScreenType: {
        UserAdmin: 37001,
        AccessGroup: 37002,
        Skills: 37003,
        LookupType: 37004,
        LookupTypeCorrelation: 37005,
        Department: 37006,
        Alerts: 37007,
        Resources: 37008,
        Configuration: 37009,
        Queue: 37010
    },
    DiscripancyCategory: {
        OOA: 6001,
        SCC: 6002,
        TRR: 6003,
        Eligibility: 6004,
        DOB: 6005,
        Gender: 6006,
        RPR: 6007
    },
    ZoneLkups: {
        4001: { isDayLight: false, timeZoneOffset: -8, alternateLkup: 4002 },//Pacific Standard Time
        4002: { isDayLight: true, timeZoneOffset: -7, alternateLkup: 4002 },
        4003: { isDayLight: false, timeZoneOffset: -7, alternateLkup: 4004 },
        4004: { isDayLight: true, timeZoneOffset: -6, alternateLkup: 4004 },
        4005: { isDayLight: false, timeZoneOffset: -6, alternateLkup: 4006 },
        4006: { isDayLight: true, timeZoneOffset: -5, alternateLkup: 4006 },
        4007: { isDayLight: false, timeZoneOffset: -5, alternateLkup: 4008 },
        4008: { isDayLight: true, timeZoneOffset: -4, alternateLkup: 4008 },
        4009: { isDayLight: null, timeZoneOffset: -7, alternateLkup: null },
        4010: { isDayLight: null, timeZoneOffset: 5.5, alternateLkup: null }
    },
    SendAlertToLkp: {
        All: 35001,
        Department: 35002,
        Individual: 35003
    },
    DateParts: {
        DPart: "_DPart",
        TPart: "_TPart",
        ZPart: "_ZPart"
    },
    timeMask: '__:__ AM',
    ContractLOB: {
        PDP: "31007",
        MA: "31004"

    },
    WorkBasket: {
        OST: "3001",
        Eligibility: "3002",
        RPR: "3003"
    },
    OSTResolutionLkup: {
        AddedtoSCCRPRSharepoint: 18001,
        AttestedNoIncarceration: 18003,
        AutoEnrolled: 18004,
        FalseDiscrepancy: 18013,
        IA: 18014,
        MemberResponseOOATerm: 18015,
        NANCMSUpdate: 18016,
        NoResponseFTTerm: 18017,
        OOATermIncarceration: 18018,
        Termed: 18031
    },
    EligibilityResolutionLkup: {
        AddedtoSCCRPRSharepoint: 18001,
        AttestedNoIncarceration: 18003,
        AutoEnrolled: 18004,
        FalseDiscrepancy: 18013,
        IA: 18014,
        MemberResponseOOATerm: 18015,
        NANCMSUpdate: 18016,
        NoResponseFTTerm: 18017,
        OOATermIncarceration: 18018,
        Termed: 18031
    },
    PermissionType: {
        CanCreate: 1,
        CanModify: 2,
        CanSearch: 3,
        CanView: 4,
        CanMassUpdate: 5,
        CanHistory: 6,
        CanReassign: 7,
        CanUnlock: 8,
        CanUpload: 9,
        CanClone: 10,
        CanReopen: 11
    },
    ActionLkup: {
        SendSCCUpdatetoCMS: 28015,
        SendSCCUpdatetoCMS: 28017,
        UpdateCMSEligibility: 28025,
        SendSCCDeletiontoCMS: 28030,
        SendSCCUpdatetoCMSUpdateEndDate: 28043
    },
    BusinessSegmentLkup: {
        MNR : 1001,//M&R
        CNS : 1002,//M&S
        PCP : 1003//PCP
    },
    SendOOALetterLkup: {
        SendOOALetter: 28015
    },
    CommentsRegularExpression: /[a-z]+/i,
    UserPreferrenceTimeZone: 4005,
    DropdownDefaultValue: "<--------------------Select-------------------->"
};

fnLockRecord = function (url, id, screentype, urlToNavigate) {
    var relockRecord = false;
    MainLayout.fnShowProgressSpinner();
    $.ajax({
        data: { "caseId": id, "screenLkup": screentype, "isForcedLocked": false },
        url: url,
        type: "POST",
        success: function (response) {
            if (response.Status == 0) {
                //Locked By Same user
                if (response.IsEditInProgress == true) {
                    MainLayout.fnHideProgressSpinner();
                    MainLayout.fnConfirmDialogbox("Confirm!", "The record is opened for editing in another window. Do you want to Continue?", function (sts) {
                        if (sts) {
                            if (fnForcedLock(url, id, screentype)) {
                                window.location.href = urlToNavigate;
                            }
                        }
                    });
                }
                else if (response.LockedHours != null) {
                    MainLayout.fnHideProgressSpinner();
                    if (response.LockedHours > 1) {
                        MainLayout.fnConfirmDialogbox("Confirm!", "The record is locked for editing by user " + response.CreatedByName + " for more than 1 hour.  Do you want to hold the lock?", function (sts) {
                            if (sts) {
                                if (fnForcedLock(url, id, screentype)) {
                                    window.location.href = urlToNavigate;
                                }
                            }
                        });
                    }
                    else {
                        MainLayout.fnAlertMessege("Alert!", "The record is locked for editing by user " + response.CreatedByName);
                    }
                }
                else {
                    window.location.href = urlToNavigate;
                }
            }
            else {
                MainLayout.fnHideProgressSpinner();
                if (response.Message != null && response.Message!='undefined') {
                    MainLayout.fnAlertMessege("Alert!", response.Message);
                }
                else if (response.ErrorMessage != null && response.ErrorMessage != 'undefined')
                {
                    MainLayout.fnAlertMessege("Alert!", response.ErrorMessage);
                }
                else {
                    MainLayout.fnAlertMessege("Alert!", "An Error Occured.");
                }
            }
        },
        error: function (result) {
            MainLayout.fnHideProgressSpinner();
            MainLayout.fnAlertMessege("Alert!", "Error:" + result.toString());
        },
        async: false
    });
};

fnForcedLock = function (url, id, screentype) {
    var isLocked = false;
    $.ajax({
        data: { "caseId": id, "screenLkup": screentype, "isForcedLocked": true },
        url: url,
        type: "POST",
        success: function (response) {
            if (response.Status == 0) {                
                if (response.LockedHours != null && response.LockedHours < 1) {
                    alert("The record is locked for editing by user " + response.CreatedByName);
                }
                else{
                    isLocked = true;
                }
            }
            else {
                if (response.Message != null && response.Message != 'undefined') {
                    alert(response.Message);
                }
                else if (response.ErrorMessage != null && response.ErrorMessage != 'undefined') {
                    alert(response.ErrorMessage);
                }
                else {
                    MainLayout.fnAlertMessege("Alert!", "An Error Occured.");
                }
            }
        },
        error: function (result) {
            MainLayout.fnAlertMessege("Error!", result.toString());
        },
        async: false
    });

    return isLocked;
}

fnUnlockRecord = function (url, idToUnlock, screenType, urlToNavigate) {
    if (idToUnlock == null || idToUnlock == undefined || idToUnlock == 0) {
        window.location.href = urlToNavigate;
    }
    else {
        var unlockSuccess = false;
        $.ajax({
            data: { "caseId": idToUnlock, "screenLkup": screenType },
            url: url,
            type: "POST",
            success: function (response) {
                if (response.Status == 0) {
                    window.location.href = urlToNavigate;
                }
                else {
                    if (response.Message != null && response.Message != 'undefined') {
                        alert(response.Message);
                    }
                    else if (response.ErrorMessage != null && response.ErrorMessage != 'undefined') {
                        alert(response.ErrorMessage);
                    }
                    else if (response.ErrMsg != null && response.ErrMsg != 'undefined') {
                        alert(response.ErrMsg);
                    }
                    else {
                        MainLayout.fnAlertMessege("Error!", "An Error Occured.");
                    }
                }
            },
            error: function (result) {
                MainLayout.fnAlertMessege("Error!", result.toString());
            },
            async: false
        });
    }
};
fnViewReferenceCaseInfo = function (View, Id) {

    View.find("div ul li a[href$='#sectionA']").attr("href", "#sectionA" + Id);
    View.find("div ul li a[href$='#sectionB']").attr("href", "#sectionB" + Id);
    View.find("div ul li a[href$='#sectionC']").attr("href", "#sectionC" + Id);
    View.find("div ul li a[href$='#sectionD']").attr("href", "#sectionD" + Id);
    View.find("div ul li a[href$='#sectionE']").attr("href", "#sectionE" + Id);
    View.find("div ul li a[href$='#sectionF']").attr("href", "#sectionF" + Id);
    View.find("div ul li a[href$='#sectionG']").attr("href", "#sectionG" + Id);
    View.find("div ul li a[href$='#sectionH']").attr("href", "#sectionH" + Id);
    View.find("div ul li a[href$='#sectionJ']").attr("href", "#sectionJ" + Id);
    View.find("div ul li a[href$='#SectionTrrData']").attr("href", "#SectionTrrData" + Id);
    View.find("div ul li a[href$='#sectionSummaryInformation']").attr("href", "#sectionSummaryInformation" + Id);
    View.find("div ul li a[href$='#sectionBadTransactionHistory']").attr("href", "#sectionBadTransactionHistory" + Id);
    View.find("div ul li a[href$='#sectionBadTransactionPending']").attr("href", "#sectionBadTransactionPending" + Id);
    View.find("div ul li a[href$='#sectionResponse']").attr("href", "#sectionResponse" + Id);

    View.find("div .tab-content div[id$='sectionA']").attr("id", "sectionA" + Id);
    View.find("div .tab-content div[id$='sectionB']").attr("id", "sectionB" + Id);
    View.find("div .tab-content div[id$='sectionC']").attr("id", "sectionC" + Id);
    View.find("div .tab-content div[id$='sectionD']").attr("id", "sectionD" + Id);
    View.find("div .tab-content div[id$='sectionE']").attr("id", "sectionE" + Id);
    View.find("div .tab-content div[id$='sectionF']").attr("id", "sectionF" + Id);
    View.find("div .tab-content div[id$='sectionG']").attr("id", "sectionG" + Id);
    View.find("div .tab-content div[id$='sectionH']").attr("id", "sectionH" + Id);
    View.find("div .tab-content div[id$='sectionJ']").attr("id", "sectionJ" + Id);
    View.find("div .tab-content div[id$='SectionTrrData']").attr("id", "SectionTrrData" + Id);
    View.find("div .tab-content div[id$='sectionSummaryInformation']").attr("id", "sectionSummaryInformation" + Id);
    View.find("div .tab-content div[id$='sectionBadTransactionHistory']").attr("id", "sectionBadTransactionHistory" + Id);
    View.find("div .tab-content div[id$='sectionBadTransactionPending']").attr("id", "sectionBadTransactionPending" + Id);
    View.find("div .tab-content div[id$='sectionResponse']").attr("id", "sectionResponse" + Id);

}

SetCurrentDateTime = function (controlName) {
    if ($('#' + controlName + _dPart) != null) {
        var today = new Date();
        var timeZone = UserPreferrenceTimeZone;
        if ($('#' + controlName + _tPart) != null && $('#' + controlName + _zPart) != null) {
            timeZone = UserPreferrenceTimeZone;
        }
        var localTime = getCurrentLocalTime(timeZone);
        today = localTime.time;

        var strDate = Get2DigitText((today.getMonth() + 1)) + "/" + Get2DigitText(today.getDate()) + "/" + today.getFullYear();
        $('#' + controlName + _dPart).val(strDate);
        $('#' + controlName + _dPart).trigger("change")

        var strTime = "";
        if ($('#' + controlName + _tPart) != null) {
            var hours = today.getHours();
            var period = "AM";
            if (hours >= 12) {
                period = "PM";
            }
            if (hours == 0) {
                hours = 12;
            }
            else {
                hours = ((hours > 12) ? hours - 12 : hours);
            }
            strTime = Get2DigitText(hours) + ":" + Get2DigitText(today.getMinutes()) + " " + period;
            $('#' + controlName + _tPart).val(strTime);

            if ($('#' + controlName + _zPart) != null) {
                $('#' + controlName + _zPart).val(localTime.timeZoneLkup);
            }
        }
    }
    return false;
}

ClearDateTime = function (controlName) {
    if ($('#' + controlName + _dPart) != null) {
        $('#' + controlName + _dPart).val('');
        if ($('#' + controlName + _tPart) != null) {
            $('#' + controlName + _tPart).val('');
        }
        $('#' + controlName + _dPart).trigger("change")
    }
    return false;
}

function Get2DigitText(number) {
    var txtNum = (number).toString();
    if (number < 10) {
        txtNum = "0" + number;
    }

    return txtNum;
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
};

//get field date time object
function getFieldUTCTime(controlName) {
    //date time serialize
    var dPart = "";
    if ($('#' + controlName + enums.DateParts.DPart) != null) {
        dPart = $('#' + controlName + enums.DateParts.DPart).val().split("/");
    }
    if (dPart.length >= 3) {
        var tPart = null;
        var zoneLkup = UserPreferrenceTimeZone;
        if ($('#' + controlName + enums.DateParts.TPart) != null) {
            //to convert time to UTC
            if ($('#' + controlName + enums.DateParts.ZPart) != null) {
                zoneLkup = $('#' + controlName + enums.DateParts.ZPart).val();
                if (zoneLkup == null || zoneLkup == "") {
                    zoneLkup = UserPreferrenceTimeZone;
                }
            }
            tPart = $('#' + controlName + enums.DateParts.TPart).val();
        }

        var tPartArray = null;
        var tPartArray2 = null;
        var dtResult = "";

        if (tPart != null && tPart != enums.timeMask && tPart != "") {
            tPartArray = tPart.split(" ");
            tPartArray2 = tPartArray[0].split(":");
            var hoursResult = parseInt(tPartArray2[0]);

            if (tPartArray[1] == "AM") {
                if (hoursResult == 12) {
                    hoursResult = 0;
                }
            }
            else {
                if (hoursResult < 12) {
                    hoursResult = hoursResult + 12;
                }
            }
            var enteredDateTime = new Date(dPart[2], getIntegerFromText(dPart[0]) - 1, dPart[1], hoursResult, getIntegerFromText(tPartArray2[1]), 0);
            var utcTime = getUTCTime(enteredDateTime, zoneLkup);
            return utcTime;
        }
        else {
            var enteredDateTime = new Date(dPart[2], getIntegerFromText(dPart[0]) - 1, dPart[1], 0, 0, 0);
            return enteredDateTime;
        }
    }
    else {
        //set date field to null
        return null;
    }
};

function getIntegerFromText(numString) {
    var rexNumber = /[^0]\d*/g;

    if (resultString = rexNumber.exec(numString)) {
        if (resultString[0] != "") {
            return parseInt(resultString[0]);
        }
    }
    return 0;
}

function getUTCTime(localTime, timeZone) {
    var zoneStd = enums.ZoneLkups[timeZone];

    var utcTime = new Date(localTime.getTime());
    utcTime.setMinutes(localTime.getMinutes() - zoneStd.timeZoneOffset * 60);
    return utcTime;
}

function getDateTimeInMMDDYYYYHHMM(date, isTime) {
    date = new Date(date);
    var strDate = Get2DigitText((date.getMonth() + 1)) + "/" + Get2DigitText(date.getDate()) + "/" + date.getFullYear();

    var strTime = "";
    if (isTime) {
        var hours = date.getHours();
        var period = "AM";
        if (hours >= 12) {
            period = "PM";
        }
        if (hours == 0) {
            hours = 12;
        }
        else {
            hours = ((hours > 12) ? hours - 12 : hours);
        }
        strTime = Get2DigitText(hours) + ":" + Get2DigitText(date.getMinutes()) + " " + period;

        strDate = strDate + " " + strTime;
    }
    return strDate;
}

function getDateTimeInMMDDYYYY(date) {
    date = new Date(date);
    var strDate = Get2DigitText((date.getMonth() + 1)) + "/" + Get2DigitText(date.getDate()) + "/" + date.getFullYear();
    return strDate;
}

function getDateTimeInHHMM(date) {
    date = new Date(date);

    var strTime = "";
    var hours = date.getHours();
    var period = "AM";
    if (hours >= 12) {
        period = "PM";
    }
    if (hours == 0) {
        hours = 12;
    }
    else {
        hours = ((hours > 12) ? hours - 12 : hours);
    }
    strTime = Get2DigitText(hours) + ":" + Get2DigitText(date.getMinutes()) + " " + period;

    return strTime;
}

//function to create date time picker fileds and setting them values on page load
function fnSetDateToField(fieldDate, field) {
    utcDate = new Date(fieldDate);
    result = getLocalTime(utcDate, UserPreferrenceTimeZone);
    if ($('#' + field + '_DPart') != undefined)
        $('#' + field + '_DPart').val(getDateTimeInMMDDYYYY(result.time));
    if ($('#' + field + '_TPart') != undefined)
        $('#' + field + '_TPart').val(getDateTimeInHHMM(result.time));
    if ($('#' + field + '_ZPart') != undefined)
        $('#' + field + '_ZPart').val(result.timeZoneLkup);
};

//function to restrict characters other than alphanumeric, spaces and '$'
fnAvoidSpecialChar = function (event) {
    const regex = /^[a-zA-Z0-9$ ]+$/;
    if (!regex.test(event.key))
        event.preventDefault();
}


//function to restrict characters other than alphanumeric, spaces and '$'
fnAvoidSpecialCharandAllowDase = function (event) {
    const regex = /^[a-zA-Z0-9$-]+$/;
    if (!regex.test(event.key))
        event.preventDefault();
}
//function to restrict characters but not @
fnAvoidSpecialCharandAllowDasewitha = function (event) {
    const regex = /^[a-zA-Z0-9$@.-]+$/;
    if (!regex.test(event.key))
        event.preventDefault();
}

//Allow Only Numeric in Textbox
$(document).on("keydown", ".allowOnlyNumeric", function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A,Ctrl+C,Ctrl+V,Ctrl+x Command+A
        ((e.keyCode == 65 || e.keyCode == 86 || e.keyCode == 88 || e.keyCode == 67) && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a number and stop the keypress
    if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
        e.preventDefault();
    }
});
//Allow Only Alphabates in Textbox
$(document).on("keydown", ".allowOnlyAlphabates", function (e) {
    // Allow: backspace, delete, tab, escape, enter and .
    if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
        // Allow: Ctrl+A, Command+A
        ((e.keyCode == 65 || e.keyCode == 86 || e.keyCode == 88 || e.keyCode == 67) && (e.ctrlKey === true || e.metaKey === true)) ||
        // Allow: home, end, left, right, down, up
        (e.keyCode >= 35 && e.keyCode <= 40)) {
        // let it happen, don't do anything
        return;
    }
    // Ensure that it is a Alphabate and stop the keypress
    if (!((e.keyCode == 8) || (e.keyCode == 32) || (e.keyCode == 46) || (e.keyCode >= 35 && e <= 40) || (e.keyCode >= 65 && e.keyCode <= 90))) {
        e.preventDefault();
    }
});

$(document).on("click", ".dispMsg", function (e) {
    ///Clear the local storage and ask confirm popup from PW screens while click from menu
    try {        
        if (typeof(PWScreenType) != 'undefined' && PWScreenType == enums.ScreenType.Queue) {
            var promptText = confirm("Are you sure want to leave this page?");
            if (!promptText) {
                e.preventDefault();
            }
            else {
                MainLayout.fnClearStorage();
            }
        }
        else {
            MainLayout.fnClearStorage();
        }
    } catch (e) {
    }

})

$(document).on({
    ///Performs a loading animation while doing ajax call
    ajaxStart: function () {
        $body.addClass("loading");
    },
    ajaxSuccess: function (event, xhr, settings) {
        if (xhr.responseJSON != undefined && xhr.responseJSON.ID != undefined && xhr.responseJSON.ID == 401 && !isSessionPoupShowed) {
            isSessionPoupShowed = true;
            var promptText = alert(xhr.responseJSON.Message);
            window.location.href = urlLoginPage;
            return false;
        }
    },
    ajaxStop: function () {
        $body.removeClass("loading");
    },
});

//for trimming all the text boxes
fnTrim = function () {
    var allInputs = $(":input");
    allInputs.each(function () {
        $(this).val($.trim($(this).val()));
    });
}

$(":input").change(fnTrim);

//function number of days between two days.
function daysBetween(date1, date2) {
    //our custom function with two parameters, each for a selected date

    diffc = date1.getTime() - date2.getTime();
    //getTime() function used to convert a date into milliseconds. This is needed in order to perform calculations.

    days = Math.round(Math.abs(diffc / (1000 * 60 * 60 * 24)));
    //this is the actual equation that calculates the number of days.

    return days;
}

