//Add field

var dataTable = {

    initialize: function (dataTableId,targetUrl) {
        //debugger;
        dataTable.attachEvents(dataTableId, targetUrl);
    },
    attachEvents: function (dataTableId, targetUrl) {
        debugger;

        //create Generic datatable
        var dataTable = $('#' + dataTableId.id).DataTable({
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                 //{
                 //    "targets": [7],
                 //    "visible": false,
                 //    "searchable": false
                 //},
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
        var oTable = $('#' + dataTableId.id).DataTable();

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
        $('#' + dataTableId.id + ' tbody').on('click', 'tr', function () {
            debugger;
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?wellId=' + data[0] + '&wellName=' + data[1],'isFollow=' + data[7];
        });

    }
}
