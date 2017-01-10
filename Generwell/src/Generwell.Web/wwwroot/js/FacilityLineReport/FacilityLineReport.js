//Add field

var facilityLineReportPage = {

    initialize: function (targetUrl) {
        facilityLineReportPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;
        facilityLineReportPage.createFollowUnfollowSymbol();
        facilityLineReportPage.createDatatable();
        facilityLineReportPage.redirectFacilityDetailsPage(targetUrl);
        facilityLineReportPage.setFacilityFollowUnfollow();
    },
    createFollowUnfollowSymbol: function ()
    {
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-star",
            radioClass: "iradio_square-star"
        });
    },
    createDatatable: function () {
        //start datatable
        var dataTable = $('#facilityLineReportListTableId').DataTable({
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
    },
    redirectFacilityDetailsPage: function (targetUrl) {
        //On checkbox click filter data tables rows
        var oTable = $('#facilityLineReportListTableId').DataTable();
        //On click of datatable row redirect to well line report page.
        $('#facilityLineReportListTableId tbody').on('click', 'tr', function () {
            debugger;
            $('#processing-modal').modal("show");
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?reportId=' + Base64.encode(data[0]);
        });
    },
    setFacilityFollowUnfollow: function () {
        //Follow or unfollow particular well 
        $('.iCheck-helper').click(function () {
            debugger;
            var followChecked = $('#followCheckDiv').find('div').hasClass('checked');
            $('#processing-modal').modal("show");
           
            $.ajax({
                type: 'GET',
                dataType: 'text',
                url: '/FacilityLineReport/Follow',
                data: { isFollow: followChecked },
                cache: false,
                success: function (data) {
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
