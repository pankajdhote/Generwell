﻿//Add field

var taskPage = {

    initialize: function (targetUrl, assignedName) {
        debugger;
        taskPage.attachEvents(targetUrl, assignedName);
    },
    attachEvents: function (targetUrl, assignedName) {
        debugger;

        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });
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
        //On checkbox click filter data tables rows
        var oTable = $('#taskListTableId').DataTable();
        $('.iCheck-helper').on("click", function () {
            debugger;
            if ($('.iCheck-helper').parent().attr("class").indexOf("checked") > -1) {
                oTable
                  .columns(6)
                  .search(assignedName)
                  .draw();
            } else {
                oTable
                  .columns(6)
                  .search("")
                  .draw();
            }

        });

        //On click of datatable row redirect to well line report page.
        $('#taskListTableId tbody').on('click', 'tr', function () {
            debugger;
            $('#processing-modal').modal("show");
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?taskId=' + Base64.encode(data[0]) + '&taskName=' + Base64.encode(data[2]) + '&isFollow=' + Base64.encode(data[7]);
        });

    }
}
