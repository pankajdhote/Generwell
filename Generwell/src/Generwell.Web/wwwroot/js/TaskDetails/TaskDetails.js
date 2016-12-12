//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
        
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.updateTaskFields();
        TaskDetailsPage.completeTask();
        TaskDetailsPage.completeEvent();
    },
    updateTaskFields: function () {
        $("#SaveTaskFieldDetailsId").click(function () {
            debugger;
            $('#processing-modal').modal("show");
            var IdArray = [];
            var ValueArray = [];
            var Content = []
            var count = 0;
            $('.clsedit').each(function () {
                var htmlType = $(this).prop('tagName')
                //var fieldTypeId = document.getElementById("fieldTypeId").value;
                if (htmlType == 'SELECT') {
                    var txt = $(this).find(":selected").val();
                    IdArray.push(this.id);
                    ValueArray.push(txt);
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                }
                else if (htmlType == 'INPUT') {
                    IdArray.push(this.id);
                    ValueArray.push(this.value);

                    if (this.name == "date2") {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  1481020522  }");
                    }
                    else if (this.name == "checkbox") {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + ValueArray[count] + "   }");
                    }
                    else {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                    }
                }
                count++;
            });
            TaskDetailsPage.callUpdateTask(Content);
        });
    },
    completeTask: function () {
        $("#Complete_Yes").click(function () {
            TaskDetailsPage.updateTaskFields();
        });
    },
    callUpdateTask: function (Content) {
        $.ajax({
            type: "GET",
            url: '/taskdetails/updatetaskfields',
            data: { Content: JSON.stringify(Content) },
            datatype: "json",
            cache: false,
            success: function (data, status, xhr) {
                debugger;
                $('#processing-modal').modal("show");
            },
            error: function (xhr) {
                debugger;
                $('#processing-modal').modal("hide");
            }
        });
    },
    completeEvent: function () {
        $(function () {
            $("#SaveTaskFieldDetailsId2").click(function () {
                debugger;
                $("#dialog1").dialog('open');
            });
        });
        $(function () {
            $("#SaveTaskFieldDetailsId2").click(function () {
                $("#dialog1").dialog('open');
            });
        });
        $("#Complete_No").click(function () {
        });
        $("#taskDetailsListTableId1").change(function () {
            document.getElementById("SaveTaskFieldDetailsId2").value = "Save";
            document.getElementById("SaveTaskFieldDetailsId2").id = "SaveTaskFieldDetailsId";
        });
    }
}








