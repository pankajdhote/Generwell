//Add field

var wellLineReportPage = {

    initialize: function (targetUrl) {
        //debugger;
        wellLineReportPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;

        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-star",
            radioClass: "iradio_square-star"
        });

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
            $('#processing-modal').modal("show");
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?reportId=' + data[0];
        });
        //End datatable


        //Follow or unfollow particular well 
        $('.iCheck-helper').click(function () {
            debugger;
            var followChecked = $('#followCheckDiv').find('div').hasClass('checked');
            $('#processing-modal').modal("show");
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/WellLineReport/Follow',
                data: { isFollow: followChecked },
                success: function (Data) {
                    debugger;
                    $('#processing-modal').modal("hide");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });

        //On back button click redirect to well page
        $('#backLineReportId').on('click', function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/Well/Index/';
            window.location.href = targetUrl;
        });

        //on task button click redirect to task page
        $('#taskPageId').on('click', function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/Task/Index?isWellId="1"';
            window.location.href = targetUrl;
        });
    }

}
