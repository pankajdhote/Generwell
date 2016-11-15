//Add field

var wellPage = {

    initialize: function (wellId,targetUrl) {
        //debugger;
        wellPage.attachEvents(wellId,targetUrl);
    },
    attachEvents: function (wellId, targetUrl) {
        debugger;

        //start datatable
        var dataTable = $('#wellLineReportListTableId').DataTable({
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
        var oTable = $('#wellLineReportListTableId').DataTable();
        
        //On click of datatable row redirect to well line report page.
        $('#wellLineReportListTableId tbody').on('click', 'tr', function () {
            debugger;
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?reportId=' + data[0];
        });
        //End datatable

        $('#FilterList').change(function () {
            debugger;
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Well/FilterWell',
                data: { id: $("#FilterList option:selected").text() },
                success: function (Data) {
                    debugger;
                    if (data != undefined || data != "" || data != null) {
                        $('#myModalContent').html(data);
                        $('#myPleaseWait').modal('hide');
                        $('#myModal').modal('show');
                    }
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    debugger;
                }
            });
        });

        $('#followWellId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/WellLineReport/Follow',
                data: { id: wellId, isFollow: $('#followWellId').prop('checked') },
                success: function (Data) {
                    debugger;                    
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#myPleaseWait').modal('hide');
                }
            });
           
            $('#wellListTableId tbody').on('click', 'tr', function () {
                debugger;
                var data = oTable.row(this).data();
                //Perform your navigation
                window.location.href = targetUrl + '?wellId=' + data[0] + '&wellName=' + data[1], 'isFollow=' + data[7];
            });


        });
    }
  
}
