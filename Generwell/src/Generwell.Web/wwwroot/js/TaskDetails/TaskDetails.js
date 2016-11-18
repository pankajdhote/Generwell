//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        //debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;

        //start datatable
        var dataTable = $('#taskDetailsListTableId').DataTable({
            "bPaginate": false,
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                {
                    // The `data` parameter refers to the data for the cell (defined by the
                    // `data` option, which defaults to the column being worked with, in
                    // this case `data: 0`.
                    "render": function (data, type, row) {
                        return data + ' (' + row[3] + ')';
                    },
                    "targets": 0
                },
            ],
        });
        //On checkbox click filter data tables rows
        var oTable = $('#taskDetailsListTableId').DataTable();

        //On click of datatable row redirect to well line report page.
        $('#taskDetailsListTableId tbody').on('click', 'tr', function () {
            debugger;
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?reportId=' + data[0];
        });
        //End datatable


        //Follow or unfollow particular well 
        $('#followTaskId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/TaskDetails/Follow',
                data: {isFollow: $('#followTaskId').prop('checked') },
                success: function (Data) {
                    debugger;
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#myPleaseWait').modal('hide');
                }
            });
        });

        
    }

}
