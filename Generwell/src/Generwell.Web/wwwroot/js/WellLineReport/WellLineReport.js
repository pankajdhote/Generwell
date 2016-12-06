//Add field

var wellLineReportPage = {

    initialize: function (targetUrl, currentWellId) {
        //debugger;
        wellLineReportPage.attachEvents(targetUrl, currentWellId);
    },
    attachEvents: function (targetUrl, currentWellId) {
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
            window.location.href = targetUrl + '?reportId=' + Base64.encode(data[0]);
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
                cache: false,
                success: function (Data) {
                    debugger;
                    $('#processing-modal').modal("hide");
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });
    }
}
