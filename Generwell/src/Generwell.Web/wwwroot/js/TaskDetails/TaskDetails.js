//Add field

var TaskDetailsPage = {

    initialize: function (taskId) {
        debugger;
        TaskDetailsPage.attachEvents(taskId);
    },
    attachEvents: function (taskId) {
        debugger;
        TaskDetailsPage.completeEvent();
        TaskDetailsPage.updateTaskFields();
        TaskDetailsPage.createMyFilterCheckbox();
        TaskDetailsPage.changeButtonEvent();
    },

    updateTaskFields: function () {
        debugger;


        //$('.dropdownErrorMessage1').on('change', function () {
        //    var count = 0;
        //    $('.clsedit').each(function () {
        //        var htmlType = $(this).prop('type');
        //        if (htmlType == 'select-one') {
        //            var lookupText = $(this).find(":selected").text();
        //            if (lookupText == 'Please select one') {
        //                $('#dropdownErrorMessage').show();
        //                setTimeout(function () { $('#dropdownErrorMessage').hide(); }, 5000);
        //            }
        //        }
        //        count++;
        //    });
        //});

        //$("#taskDetailsListTableId").change(function () {
        //    var count = 0;
        //    $('.clsedit').each(function () {
        //        var htmlType = $(this).prop('type');

        //        if (htmlType == 'select-one') {
        //            var id = this.id;
        //            var lookupText = $(this).find(":selected").text();
        //            if (lookupText == 'Please select one') {
        //                $('#dropdownErrorMessage_' + id).show();
        //            }
        //        }
        //        if (htmlType == 'text') {
        //            if (this.name == "date") {
        //                debugger;
        //                var id = this.id;
        //                var DateText = this.value;
        //                var startDate = "Jan 01,1990";

        //                if (new Date(DateText) < new Date(startDate))
        //                {
        //                    $('#dateErrorMessage_'+id).show();
        //                }
        //            }
        //            else {

        //                var Text = this.value;
        //                if (Text == '') {
        //                     var id = this.id;
        //                     $('#TextErrorMessage__' + id).show();
        //                }
        //            }
        //        }
        //        count++;
        //    });

        //});
    },
    completeTask: function () {
        var Content = TaskDetailsPage.getViewData();
        $('#processing-modal').modal("hide");
        TaskDetailsPage.callUpdateTask(Content);
    },
    callUpdateTask: function (Content) {
        $('#processing-modal').modal("show");
        $.ajax({
            type: "GET",
            url: '/taskdetails/updatetaskfields',
            data: { Content: JSON.stringify(Content) },
            datatype: "json",
            cache: false,
            success: function (data, status, xhr) {
                $('#newCmpMessage').show();
                setTimeout(function () { $('#newCmpMessage').hide(); }, 3000);
                debugger;
                $('#processing-modal').modal("hide");
                $("#completeTask").css("display", "block");
                $("#ReSaveTaskFieldDetailsId").css("display", "none");
                TaskDetailsPage.changeButtonEvent();
            },
            error: function (xhr) {
                debugger;
                $('#processing-modal').modal("hide");
            }
        });
    },
    completeEvent: function () {
        $('#completeTask').click(function () {
            debugger;
            swal({
                title: "Task Complete",
                text: "YES/NO complete Re-activated task?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#cf7f00",
                confirmButtonText: "Yes, update it!",
                closeOnConfirm: false
            },
            function () {
                debugger;
                $('#processing-modal').modal("hide");
                TaskDetailsPage.completeTask();
                swal("updated!", "Data updated successfully", "success");
            });
        });
    },
    getViewData: function () {
        //$('#processing-modal').modal("show");
        var IdArray = [];
        var ValueArray = [];
        var Content = [];
        var count = 0;
        var flag = [];
        $('.clsedit').each(function () {
            debugger;
            var htmlType = $(this).prop('type');
            if (htmlType == 'select-one') {
                var txt = $(this).find(":selected").val();
                var lookupText = $(this).find(":selected").text();
                IdArray.push(this.id);
                ValueArray.push(txt);
                var id = this.id;
                var lookupText = $(this).find(":selected").text();
                //LookupValidation
                if (lookupText == 'Please select one') {
                    $('#dropdownErrorMessage_' + id).show();
                    //flag.push(1);
                    Content.length = 0;
                    return false;
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

                    if (new Date(DateText) < new Date(startDate) || DateText == "") {
                        $('#dateErrorMessage_' + id).show();
                        Content.length = 0;
                        return false;
                    }
                    else if(DateText.length>11)
                    {
                        $('#inValidDateErrorMessage_' + id).show();
                        Content.length = 0;
                        return false;
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
                    //Text Validation
                    if (text == "") {
                        $('#textErrorMessage_' + id).show();
                        Content.length = 0;
                        return false;
                    }
                    else {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                        $('#textErrorMessage_' + id).hide();
                    }
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
                if (number < 1 || number > 100 || number == "") {
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
        var status = $('#reactivatedTask').val();
        if (status.toLowerCase() == "re-activated") {
            $("#completeTask").css("display", "block");
            $("#savedDetails").css("display", "none");
        } else {
            $("#completeTask").css("display", "none");
            $("#savedDetails").css("display", "block");
        }

        $('#taskDetailsListTableId input,select').change(function () {
            debugger;
            $("#completeTask").css("display", "none");
            $("#savedDetails").css("display", "block");

            $("#savedDetails").click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                //var flag = TaskDetailsPage.getViewData();
                if (Content.length > 0) {
                    TaskDetailsPage.callUpdateTask(Content);
                }
                else {

                }
            });
        });
    },
    getPictureAlbum: function (id) {
        debugger;
        var id = Base64.encode(id.toString());
        $('#processing-modal').modal("show");
        var url = "/Picture/Index" + '?id=' + id;
        window.location.href = url;
    },

    limitText: function (limitField, limitNum) {
        if (limitField.value.length > limitNum) {
            limitField.value = limitField.value.substring(0, limitNum);
        }
    },
    isNumberKey: function (event) {
        $("#completeTask").css("display", "none");
        $("#savedDetails").css("display", "block");
        var charCode = (event.which) ? event.which : event.keyCode
        if (charCode > 31 && (charCode < 48 || charCode > 57) || event.length > 2)
            return false;
        return true;
    },
    check: function (e, value) {
     $("#completeTask").css("display", "none");
        $("#savedDetails").css("display", "block");
        if (!e.target.validity.valid) {
            e.target.value = value.substring(0, value.length - 1);
            return false;
        }
        var idx = value.indexOf('.');
        if (idx >= 0 && value.length - idx > 3) {
            e.target.value = value.substring(0, value.length - 1);
            return false;
        }
        return true;
    }
    //isKeyup: function (event) {
    //    if ($(this).val() < 100) {
    //        alert("No numbers above 100");
    //        $(this).val('100');
    //    }
    //}
}








