//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;

        //Follow or unfollow particular task 
        $('#followTaskId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/TaskDetails/Follow',
                data: { isFollow: $('#followTaskId').prop('checked') },
                cache:false,
                success: function (Data) {
                    debugger;
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#myPleaseWait').modal('hide');
                }
            });
        });

        $("#SaveTaskFieldDetailsId").click(function () {
            debugger;
            var fieldId = document.getElementById("FieldId").value;
            var value = document.getElementById("displayValue").value;
            $.ajax({
                type: "POST",
                url: '/TaskDetails/UpdateTaskFields',
                data: { fieldId: fieldId, value: value },
                dataType: "json",
                processData: true,
                cache:false,
                success: function (data, status, xhr) {
                    alert(status);
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        });
    }
}








