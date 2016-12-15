//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.completeEvent();
        TaskDetailsPage.updateTaskFields();
        TaskDetailsPage.createMyFilterCheckbox();
    },
      
    updateTaskFields: function () {
        document.getElementById('taskDetailsListTableId').onchange = function (event) {
            $("#SaveTaskFieldDetailsId").click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                TaskDetailsPage.callUpdateTask(Content);
            });
        };
    },
    updateTaskFields: function () {
        document.getElementById('taskDetailsListTableId1').onchange = function (event) {
            debugger;
            $("#SaveTaskFieldDetailsId").css("display", "block");
            $("#completeTask").css("display", "none");
            
            $("#SaveTaskFieldDetailsId").click(function () {
                debugger;
                var Content = TaskDetailsPage.getViewData();
                TaskDetailsPage.callUpdateTask(Content);

            });
        };
    },
    completeTask: function () {
        var Content = TaskDetailsPage.getViewData();
        $('#processing-modal').modal("hide");
        TaskDetailsPage.callUpdateTask(Content);
    },
    callUpdateTask: function (Content) {
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
                location.reload();
                debugger;
                $('#processing-modal').modal("hide");
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
                else if (this.name == "number") {
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
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
    getPictureAlbum: function (id) {
        debugger;
        var id = Base64.encode(id.toString());
        $('#processing-modal').modal("show");
        var url = "/Picture/Index" + '?id=' + id;
        window.location.href = url;
    }
}








