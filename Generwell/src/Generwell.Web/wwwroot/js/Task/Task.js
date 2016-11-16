//Add field

var taskPage = {

    initialize: function (targetUrl) {
        //debugger;
        taskPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;

        //create Generic datatable
        var dataTable = $('#taskListTableId').DataTable({
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                 {
                     "targets": [7],
                     "visible": false,
                     "searchable": true
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
        $("#IsFavorite").on("change", function () {
            debugger;
            if ($(this).is(":checked")) {
                oTable
                  .columns(7)
                  .search("^" + "True" + "$", true, false, false)
                  .draw();
            } else {
                oTable
                  .columns(7)
                  .search("")
                  .draw();
            }

        });
        //On click of datatable row redirect to task details  page.
        //$('#taskListTableId tbody').on('click', 'tr', function () {
        //    debugger;
        //    var data = oTable.row(this).data();
        //    //Perform your navigation
        //    window.location.href = targetUrl + '?taskId=' + data[0] + '&taskName=' + data[1] + '&isFollow=' + data[7];
        //});
        //On checkbox click filter data tables rows
        var oTable = $('taskListTableId').DataTable();
        $("#IsFavorite").on("change", function () {
            debugger;
            if ($(this).is(":checked")) {
                oTable
                  .columns(7)
                  .search("^" + "True" + "$", true, false, false)
                  .draw();
            } else {
                oTable
                  .columns(7)
                  .search("")
                  .draw();
            }

        });
        //On click of datatable row redirect to well line report page.
        $('#taskListTableId tbody').on('click', 'tr', function () {
            debugger;
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?taskId=' + data[0] + '&taskName=' + data[1] + '&isFollow=' + data[7];
        });


        //filter particular record on filter value
        //Follow or unfollow particular well 
        $('#FilterList').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Task/FilterTask',
                data: { id: $('#FilterList option:selected').text() },
                success: function (data) {
                    debugger;
                    if (data != undefined || data != "")
                    {
                        $("#taskTableDivId").html(data);
                        $('#myPleaseWait').modal('hide');
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#myPleaseWait').modal('hide');
                }
            });
        });

    }
}


