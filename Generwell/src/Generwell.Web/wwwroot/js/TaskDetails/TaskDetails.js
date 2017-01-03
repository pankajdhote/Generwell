//Add field

var TaskDetailsPage = {

    initialize: function (taskId) {
        TaskDetailsPage.attachEvents(taskId);
    },
    attachEvents: function (taskId) {
        debugger;
        TaskDetailsPage.completeEvent();
        TaskDetailsPage.updateTaskFields();
        TaskDetailsPage.createMyFilterCheckbox();
        TaskDetailsPage.changeButtonEvent();
        TaskDetailsPage.bindEvent();
    },
    updateTaskFields: function () {

    },
    completeTask: function () {
        var content = [];
        content.push("{ \"op\": \"replace\", \"path\": \"/Completed\", \"value\": true}");
        //var Content = '[{ "op": "replace", "path": "/Completed", "value": true }]';
        $('#processing-modal').modal("hide");

        TaskDetailsPage.callUpdateTask(content);
    },
    callUpdateTask: function (Content) {
        $('#processing-modal').modal("show");
        $.ajax({
            type: "GET",
            url: '/taskdetails/updatetaskfields',
            data: { Content: JSON.stringify(Content) },
            datatype: "json",
            async: false,
            cache: false,
            success: function (data, status, xhr) {
                // location.reload();
                debugger;
                $('#processing-modal').modal("hide");
                if (data != "") {
                    var obj = jQuery.parseJSON(data);
                    var getTable = TaskDetailsPage.createTable(obj);
                    swal({
                        title: "Validation Rule Errors",
                        text: getTable,
                        html: true
                    });
                }

                $('#processing-modal').modal("hide");
                $("#completeTask").css("display", "block");
                $("#ReSaveTaskFieldDetailsId").css("display", "none");
                TaskDetailsPage.changeButtonEvent();
            },
            error: function (xhr) {
                $('#processing-modal').modal("hide");
            }
        });
    },
    completeEvent: function () {
        $('#completeTask').click(function () {
            swal({
                title: "Task Complete",
                text: "YES/NO complete Re-activated task?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#cf7f00",
                confirmButtonText: "Yes, complete it!",
                closeOnConfirm: false
            },
            function () {
                $('#processing-modal').modal("hide");
                TaskDetailsPage.completeTask();
                swal("updated!", "Data updated successfully", "success");
            });
        });
    },
    createTable: function (model) {
        /* Note that the whole content variable is just a string */
        var content = '<div class="ibox-content form-group">';
        content += "<table class='table'>";
        for (i = 0; i < model.length; i++) {
            content += '<tr><th colspan="2"><label><b>' + model[i].fieldDesc + '</b></label></th></tr>';
            content += '<tr><td><label>Old Value: </label></td><td><label><b>' + model[i].oldValue + '</b></label></td></tr>';
            content += '<tr><td><label>New Value: </label></td><td><label><b>' + model[i].newValue + '</b></label></td></tr>';
            content += '<tr><td><label>Instructions: </label></td><td><label><b>' + model[i].instructions + '</b></label></td></tr>';
        }
        content += "</div>";
        content += "</table>";
        return content;
    },
    getViewData: function () {
        debugger;
        var IdArray = [];
        var ValueArray = [];
        var Content = [];
        var count = 0;
        var flag = [];
        $('.clsedit').each(function () {
            var htmlType = $(this).prop('type');
            if (htmlType == 'select-one') {
                debugger;
                var txt = $(this).find(":selected").val();
                var lookupText = $(this).find(":selected").text();
                IdArray.push(this.id);
                ValueArray.push(txt);
                var id = this.id;
                var lookupText = $(this).find(":selected").text();
                //LookupValidation
                if (lookupText == 'Please select one') {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + this.id + "\", \"value\": " + "\"" + "" + "\"}");
                    $('#dropdownErrorMessage_' + id).hide();
                }
                else {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + this.id + "\", \"value\": " + "\"" + txt + "\"}");
                    $('#dropdownErrorMessage_' + id).hide();
                }
            }
            else if (htmlType == 'text') {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                //Date Valdiation
                if (this.name == "date") {
                    var id = this.id;
                    var DateText = this.value;
                    var startDate = "Jan 01,1990";

                    if (new Date(DateText) < new Date(startDate)) {
                        $('#dateErrorMessage_' + id).show();
                        Content.length = 0;
                        return false;
                    }
                    else if (DateText.length > 11) {
                        $('#inValidDateErrorMessage_' + id).show();
                        Content.length = 0;
                        return false;
                    }
                    else if (DateText == "") {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  " + null + "  }");
                        $('#dateErrorMessage_' + id).hide();
                        $('#inValidDateErrorMessage_' + id).hide();
                    }
                    else {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  " + Date.parse(ValueArray[count]) / 1000 + "  }");
                        $('#dateErrorMessage_' + id).hide();
                        $('#inValidDateErrorMessage_' + id).hide();
                    }
                }
                else {
                    var id = this.id;
                    var text = this.value;
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                    $('#textErrorMessage_' + id).hide();
                    $('#memoErrorMessage_' + id).hide();
                }
            }
            else if (htmlType == "checkbox") {
                var checkboxValue = $('input[name=checkbox]:checked');
                IdArray.push(this.id);
                ValueArray.push(this.value);
                if (checkboxValue.length == 0) {
                    $('#checkboxErrorMessage_' + this.id).show();
                    Content.length = 0;
                    return false;
                }
                else {
                    $('#checkboxErrorMessage_' + this.id).hide();
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + $(this).prop('checked') + "   }");
                }
            }
            else if (htmlType == "number") {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                var id = this.id;
                var number = this.value;
                //Number Validation
                if (number > 100) {
                    $('#numberErrorMessage_' + id).show();
                    Content.length = 0;
                    return false;
                }
                else {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                    $('#numberErrorMessage_' + id).hide();
                }
            }
            count++;

        });

        return Content;

    },
    createMyFilterCheckbox: function () {
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });
    },
    changeButtonEvent: function () {
        debugger;
        var status = $('#reactivatedTask').val();
        if (status.toLowerCase() == "re-activated") {
            $("#completeTask").css("display", "block");
            $("#savedDetails").css("display", "none");
        } else {
            $("#completeTask").css("display", "none");
            $("#savedDetails").css("display", "block");
        }
        $('.iCheck-helper').on('click', function () {
            $("#completeTask").css("display", "none");
            $("#savedDetails").css("display", "block");
        });
    },
    getPictureAlbum: function (albumId, fieldId) {
        debugger;
        var id = Base64.encode(albumId != null ? albumId.toString() : "");
        var fieldId = Base64.encode(fieldId != undefined ? fieldId.toString() : "");
        $('#processing-modal').modal("show");
        var url = "/Picture/Index" + '?id=' + id + '&fieldId=' + fieldId;
        window.location.href = url;
    },
    limitText: function (limitField, limitNum) {
        $("#completeTask").css("display", "none");
        $("#savedDetails").css("display", "block");
        if (limitField.value.length > limitNum) {
            limitField.value = limitField.value.substring(0, limitNum);
            return false;
        }
    },
    limitText: function (limitField, limitNum) {
        $("#completeTask").css("display", "none");
        $("#savedDetails").css("display", "block");
        if (limitField.value.length > limitNum) {
            limitField.value = limitField.value.substring(0, limitNum);
            return false;
        }
    },
    check: function (e, value, fieldDecimal) {
        $("#completeTask").css("display", "none");
        $("#savedDetails").css("display", "block");
        if (!e.target.validity.valid) {
            e.target.value = value.substring(0, value.length - 1);
            return false;
        }
        var charCode = (e.which) ? e.which : e.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }
        var idx = value.indexOf('.');
        if (idx > 0 && value.length - idx > fieldDecimal) {
            e.target.value = value.substring(0, value.length - 1);
            return false;
        }
        return true;
    },
    bindEvent: function () {
        $('.chk').find(".iCheck-helper").click(function () {
            TaskDetailsPage.setWellFollowUnfollow();
        });
        $('#taskDetailsListTableId input,select').unbind().on('keyup change', function () {
            //JS Code
            debugger;

            $("#savedDetails").unbind().click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                if (Content.length > 0) {
                    TaskDetailsPage.callUpdateTask(Content);
                }
                else {

                }
            });
            TaskDetailsPage.setWellFollowUnfollow();

            $("#completeTask").css("display", "none");
            $("#savedDetails").css("display", "block");

        });
    },
    setWellFollowUnfollow: function () {
        //Follow or unfollow particular well 
        debugger;
        var followChecked = $('#isFollow').val();
        if (followChecked == "") {
            followChecked = "true";
            $('#isFollow').attr('value', 'checked');
            debugger;
            $.ajax({
                type: 'GET',
                dataType: 'text',
                url: '/WellLineReport/Follow',
                data: { isFollow: followChecked },
                cache: false,
                success: function (data) {
                    debugger;
                    swal("Favorites", "The corresponding well will be added to your favorite wells.");
                    $('#taskDetailsListTableId input,select').unbind();
                    $('.chk').find(".iCheck-helper").unbind();
                    $('#processing-modal').modal("hide");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        }
    }
}








