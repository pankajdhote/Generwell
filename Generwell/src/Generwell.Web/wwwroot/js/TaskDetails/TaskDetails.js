//Add field

var TaskDetailsPage = {

    initialize: function (taskId) {
        TaskDetailsPage.attachEvents(taskId);
    },
    attachEvents: function (taskId) {
        debugger;
        TaskDetailsPage.completeEvent();
        TaskDetailsPage.createMyFilterCheckbox();
        TaskDetailsPage.changeButtonEvent();
        TaskDetailsPage.bindEvent();
        TaskDetailsPage.createDatetime();
    },
    createDatetime: function () {
        debugger;
        $('.datepicker').datepicker({
            format: 'M dd,yyyy',
            autoclose: true,
            todayBtn: true
        }); //Initialise any date pickers
        $('.datetimepicker').datetimepicker({
            format: 'MMM DD,YYYY hh:mm A',
        });
    },
    completeTask: function () {
        var content = [];
        content.push("{ \"op\": \"replace\", \"path\": \"/Completed\", \"value\": true}");
        $('#processing-modal').modal("hide");
        var response = TaskDetailsPage.callCompleteTask(content);
        return response;
    },
    callUpdateTask: function (Content) {
        var response = false;
        $('#processing-modal').modal("show");
        $.ajax({
            type: "GET",
            url: '/taskdetails/updatetaskfields',
            data: { Content: JSON.stringify(Content) },
            datatype: "json",
            cache: false,
            async: true,
            success: function (data, status, xhr) {
                debugger;
                if (data == "") {
                    TaskDetailsPage.showSweetAlert("Task saved successfully.");
                    response = true;
                }
                var checkJson = TaskDetailsPage.checkJsonFormat(data);
                if (checkJson) {
                    response = true;
                    var obj = jQuery.parseJSON(data);
                    var getTable = TaskDetailsPage.createTable(obj);
                    TaskDetailsPage.displayValidationMsg(getTable);
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
        return response;
    },
    displayValidationMsg: function (getTable) {
        swal({
            title: "<h4>Validation Rule Errors</h4>",
            text: getTable,
            html: true,
            type: "error",
            confirmButtonColor: "#cf7f00",
            confirmButtonText: "Ok",
            closeOnConfirm: true
        },
        function () {
            debugger;
            TaskDetailsPage.closeSweetAlert();
            $('#processing-modal').modal("show");
            location.reload();
        });
    },
    closeSweetAlert: function () {
        swal({
            title: "", type: "error",
            confirmButtonColor: "#cf7f00",
            confirmButtonText: "Ok",
            timer: 1
        });
    },
    callCompleteTask: function (Content) {
        var response = false;
        TaskDetailsPage.closeSweetAlert();
        $('#processing-modal').modal("show");
        $.ajax({
            type: "GET",
            url: '/taskdetails/updatetaskfields',
            data: { Content: JSON.stringify(Content) },
            datatype: "json",
            cache: false,
            async: true,
            success: function (data, status, xhr) {
                debugger;
                $('#processing-modal').modal("hide");
                if (data == "") {
                    response = true;
                    TaskDetailsPage.showSweetAlert("Task completed successfully.");
                    $("#completeTask").css("display", "none");
                    $("#savedDetails").css("display", "block");
                } else {
                    TaskDetailsPage.showDangerAlert();
                }
            },
            error: function (xhr) {
                $('#processing-modal').modal("hide");
            }
        });
        return response;
    },
    showDangerAlert: function () {
        $('#newCmpMessage').html("<button aria-hidden='true' data-dismiss='alert' class='close' type='button'>×</button>Task not marked as complete. Completion criteria not met.");
        $('#newCmpMessage').addClass("alert-danger");
        $('#newCmpMessage').show();
        setTimeout(function () {
            $('#newCmpMessage').hide();
        }, 3000);
    },
    checkJsonFormat: function (str) {
        try {
            JSON.parse(str);
        } catch (e) {
            return false;
        }
        return true;
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
                debugger;
                var response = TaskDetailsPage.completeTask();
            });
        });
    },
    showSweetAlert: function (message) {
        $('#newCmpMessage').html("<button aria-hidden='true' data-dismiss='alert' class='close' type='button'>×</button>" + message);
        $('#newCmpMessage').addClass("alert-success");
        $('#newCmpMessage').removeClass("alert-danger");
        $('#newCmpMessage').show();
        setTimeout(function () {
            $('#newCmpMessage').hide();
        }, 3000);
    },
    createTable: function (model) {
        /* Note that the whole content variable is just a string */
        var content = '<div class="row">';
        content += '<div class="col-md-12 col-lg-12 col-sm-12 col-xs-12 v-scroll">';
        for (i = 0; i < model.length; i++) {
            content += '<div id="validation" class="form-group border-bottom text-left ibox-content col-md-12 col-lg-12 col-sm-12 col-xs-12">';
            content += '<label class="text-red">' + model[i].fieldDesc + '</label>';
            content += '<br /><label class="validationLable">Old Value: </label>';
            content += model[i].oldValue;
            content += '<br /><label class="validationLable">New Value: </label>';
            content += model[i].newValue;
            content += '<br /><label class="validationLable">Instructions: </label>';
            content += model[i].instructions;
            content += '</div>';
        }
        content += '</div>';
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
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + this.id + "\", \"value\": " + "\"" + txt + "\"}");
            }
            else if (htmlType == 'text') {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                //Date Valdiation
                if (this.name == "date") {
                    var id = this.id;
                    var DateText = this.value;
                    if (DateText != "") {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  " + Date.parse(ValueArray[count]) / 1000 + "  }");
                    } else {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  " + null + "  }");
                    }
                }
                else {
                    var id = this.id;
                    var text = this.value;
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                }
            }
            else if (htmlType == "checkbox") {
                var checkboxValue = $('input[name=checkbox]:checked');
                IdArray.push(this.id);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + $(this).prop('checked') + "   }");
            }
            else if (htmlType == "number") {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                var id = this.id;
                var number = this.value;
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
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
    checkPeriod: function (e, value, event) {
        debugger;
        if (value != "") {
            var decNumber = event.name;
            if (decNumber == "0" && e.keyCode == 46) {
                e.target.value = value;
                return false;
            }
        }
    },
    check: function (e, value, event) {
        debugger;
        if (value != "") {
            var decNumber = event.name;
            if (parseFloat(value) > event.max) {
                if (value.indexOf('.') > -1) {
                    var findValue = value.substring(0, parseInt(value).toString().length - 1);
                    var findDecValue = value.substring(value.indexOf("."));
                    e.target.value = findValue + "" + findDecValue;
                    return false;
                } else {
                    e.target.value = value.substring(0, parseInt(value).toString().length - 1);
                    return false;
                }
            }

            if (value.indexOf('.') > -1) {
                var decCount = value.substring(value.indexOf('.') + 1);
                if (decCount.length > decNumber) {
                    e.target.value = value.substring(0, value.indexOf('.') + 1 + decNumber);
                    return false;
                }
            }
        }
    },
    bindEvent: function () {
        $('.chk').find(".iCheck-helper").click(function () {
            TaskDetailsPage.setWellFollowUnfollow();
            $("#savedDetails").unbind().click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                if (Content.length > 0) {
                    var response = TaskDetailsPage.callUpdateTask(Content);
                }
            });
        });
        $('#taskDetailsListTableId input,select').unbind().on('keyup change', function () {
            //JS Code
            debugger;
            $("#savedDetails").unbind().click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                if (Content.length > 0) {
                    var response = TaskDetailsPage.callUpdateTask(Content);
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








