/////////////////Variables///////////////////////


//////////////////Events/////////////////////
//Open Attachment Dialog///
$(document).on("click", "#btnAddPWAttachment", function () {
    try {

        $.ajax({
            data: {},
            url: urlUploadPWAttachment,
            type: "POST",
            success: function (data) {
                if (data != null && typeof data != "undefined") {
                    $("#divModalPWAttachment").empty().html(data);
                    $("#myPWAttachmentModal").modal('show');
                }
                else {
                    
                }
            },
            error: function (x) {
               
            }
        });

    } catch (e) {

    }
});

////Upload file Templocation Server////////////
$(document).on("click", "#btnUploadPwAttchmentFile", function () {
    try {
        if (fnCheckPWAttachmentFile($("#filePWAttachmentBrowse"))) {
            $.ajax({
                url: urlAddPWAttachment,
                type: "POST",
                data: function () {
                    var data = new FormData();
                    data.append("genQueueId", $("#GEN_QueueId").val());
                    data.append("file", $("#filePWAttachmentBrowse").get(0).files[0]);
                    return data;
                }(),
                contentType: false,
                processData: false,
                success: function (data) {
                    if (data != "") {
                        $("#divPWAttachment").empty().html(data);
                        MainLayout.fnInitialiseDataTable($('#tblPWAttachments'));
                        $("#myPWAttachmentModal").modal('hide');
                    }
                    else {
                        MainLayout.fnAlertMessege("Alert!", "An error occoured.");
                    }
                },
                error: function (jqXHR, textStatus, errorMessage) {
                    MainLayout.fnAlertMessege("Alert!", "An error occoured.");
                }
            });
        }

    } catch (e) {
        MainLayout.fnAlertMessege("Error!", "An error occoured!");
    }
});

$(document).on("click", ".deletePWAttachmentIco", function () {
    try {
        var curObject = $(this);
        var attachmentId = curObject.closest("tr").find(".deletePWAttachment").val();
        var genRef = curObject.closest("tr").find(".deleteGenQueueRef").val();
        MainLayout.fnConfirmDialogbox("Confirmation", "Do you want to delete?", function (IsTrue) {
            if (IsTrue) {
                $.ajax({
                    data: { "attachmentId": attachmentId, "genRef": genRef},
                    url: urlDeletePWAttchments,
                    type: "POST",
                    success: function (data) {
                        if (data != "") {
                            $("#divPWAttachment").empty().html(data);
                            MainLayout.fnInitialiseDataTable($('#tblPWAttachments'));
                            $("#myPWAttachmentModal").modal('hide');
                        }
                        else {
                            MainLayout.fnAlertMessege("Error!", "An error occoured while deleting!", function () {
                                $("#myPWAttachmentModal").modal('hide');
                            });
                        }
                    },
                    error: function (x) {
                        MainLayout.fnAlertMessege("Error!", "An error occoured while deleting!", function () {
                            $("#myPWAttachmentModal").modal('hide');
                        });
                    }
                });
               
            }
        });

    } catch (e) {
        MainLayout.fnAlertMessege("Error!", "An error occoured!");
    }
});






///////////////////////Methods///////////////////////
////Validate
fnCheckPWAttachmentFile = function (sender) {
    try {
        var $summary = $("#frmPWAttachmentUpload").find("[data-valmsg-summary=true]"),
            $ul = $summary.find("ul").empty();
        //   var validExts = new Array(".xlsx",".csv");

        if (sender.val() == "")
        {
            $('#filePWAttachmentBrowse').removeClass("valid").addClass("input-validation-error");
            $("<li />").html("Please select a file.").prependTo($ul);
            $summary.removeClass("validation-summary-valid").addClass("validation-summary-errors");
            return false;
        }
        var validExts = new Array(".pdf");
        var fileExt = sender.val();
        fileExt = fileExt.substring(fileExt.lastIndexOf('.'));
        if (validExts.indexOf(fileExt) < 0) {
            $('#filePWAttachmentBrowse').removeClass("valid").addClass("input-validation-error");
            $("<li />").html("Invalid file selected.").prependTo($ul);
            $summary.removeClass("validation-summary-valid").addClass("validation-summary-errors");
            return false;
        }
        else if (sender.prop("files")[0].size > 8388608) {
            $('#filePWAttachmentBrowse').removeClass("valid").addClass("input-validation-error");
            $("<li />").html("File size should not exceed 8 MB.").prependTo($ul);
            $summary.removeClass("validation-summary-valid").addClass("validation-summary-errors");
            return false;
        }
        else return true;
    }
    catch (e) {
        throw e;
    }
};