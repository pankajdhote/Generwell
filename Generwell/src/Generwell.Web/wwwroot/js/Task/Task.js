//Add field

var taskPage = {

    initialize: function (targetUrl) {
        debugger;
        taskPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;

        //create Generic datatable
        var dataTable = $('#taskListTableId').DataTable({
            "columnDefs": [
                { "orderable": false, "targets": 0 },
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

        //On click of datatable row redirect to well line report page.
        var oTable = $('#taskListTableId').DataTable();
        $('#taskListTableId tbody').on('click', 'tr', function () {
            debugger;
            $('#processing-modal').modal("show");
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?taskId=' + data[0] + '&taskName=' + data[2] + '&isFollow=' + data[7];
        });




    }
}
