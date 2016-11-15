//Add field

var wellDetailsPage = {

    initialize: function (wellId,targetUrl) {
        //debugger;
        wellDetailsPage.attachEvents(wellId, targetUrl);
    },
    attachEvents: function (wellId, targetUrl) {
        debugger;

        //start datatable
        var dataTable = $('#wellDetailsListTableId').DataTable({
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
        var oTable = $('#wellDetailsListTableId').DataTable();
        
        //On click of datatable row redirect to well line report page.
        //$('#wellDetailsListTableId tbody').on('click', 'tr', function () {
        //    debugger;
        //    var data = oTable.row(this).data();
        //    //Perform your navigation
        //    window.location.href = targetUrl + '?reportId=' + data[0];
        //});
        //End datatable

    }
  
}
