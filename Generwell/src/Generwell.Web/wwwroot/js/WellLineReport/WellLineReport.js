//Add field

var wellLineReportPage = {

    initialize: function (targetUrl,currentWellId) {
        //debugger;
        wellLineReportPage.attachEvents(targetUrl,currentWellId);
    },
    attachEvents: function (targetUrl,currentWellId) {
        debugger;

        //on page unload get datatable rows and store in collection
        $(window).unload(function () {
            debugger;
            //store my well checkbox value and filter id.
            var filterId = $('#FilterList option:selected').val();

            var isMyWell;
            if ($('.iCheck-helper').parent().attr("class").indexOf("checked") > -1) {
                isMyWell = true;
            } else {
                isMyWell = false;
            }

            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Map/SetGooleMapObjects',
                async: false,
                data: { isMyWell: isMyWell, filterId: filterId, previousPage: "2" },
                success: function (data) {
                    debugger;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });

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
            var targetUrl = '/Task/Index?currentWellId=' + currentWellId + '';
            window.location.href = targetUrl;
        });

        //on task button click redirect to task page
        $('#locationPageId').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/Map/Index';
            window.location.href = targetUrl;
        });
        

    }

}
