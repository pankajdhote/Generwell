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
                //swal("updated!", "Data updated successfully", "success");
                $('#newCmpMessage').show();
                setTimeout(function () { $('#newCmpMessage').hide(); }, 3000);
                //location.reload();
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
        $('#processing-modal').modal("show");
        var IdArray = [];
        var ValueArray = [];
        var Content = [];
        var count = 0;
        $('.clsedit').each(function () {
            debugger;
            var htmlType = $(this).prop('type');
            if (htmlType == 'select-one') {
                var txt = $(this).find(":selected").val();
                var lookupText = $(this).find(":selected").text();
                IdArray.push(this.id);
                ValueArray.push(txt);
              
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + this.id + "\", \"value\": " + "\"" + txt + "\"}");
            }
            else if (htmlType == 'text') {
                IdArray.push(this.id);
                ValueArray.push(this.value);

                if (this.name == "date") {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  " + Date.parse(ValueArray[count]) / 1000 + "  }");
                }
                else {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                }
            }
            else if (htmlType == "checkbox") {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + $(this).prop('checked') + "   }");
            }
            else if (htmlType == "number") {
                IdArray.push(this.id);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + this.id + "\", \"value\": " + "\"" + this.value + "\"}");
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
                TaskDetailsPage.callUpdateTask(Content);
            });
        });
    }
}








