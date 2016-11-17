//Add field

var taskPage = {

    initialize: function (taskId) {
        //debugger;
        wellPage.attachEvents(taskId);
    },
    attachEvents: function (taskId) {
        debugger;

        $('#followTaskId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/TaskDetails/Follow',
                data: { id: taskId, isFollow: $('#followTaskId').prop('checked') },
                success: function (Data) {
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });           
        });
    }

}
