//Add field

var wellDetailsPage = {

    initialize: function (wellId, targetUrl) {
        //debugger;
        wellDetailsPage.attachEvents(wellId, targetUrl);
    },
    attachEvents: function (wellId, targetUrl) {
        debugger;

        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-star",
            radioClass: "iradio_square-star"
        });

        //start datatable
        var dataTable = $('#wellDetailsListTableId').DataTable({
            "bPaginate": false,
            "bInfo" : false,
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                 {
                     "targets": [3],
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
        //End datatable

        //on back button click redirect to well line report page
        $('#backDetailsPageId').on('click', function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/WellLineReport/Index/';
            window.location.href = targetUrl;
        });

        //on task button click redirect to task page
        $('#taskPageId').on('click', function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/Task/Index?isWellId="1"';
            window.location.href = targetUrl;
        });

        //Follow or unfollow particular well 
        $('.iCheck-helper').click(function () {
            debugger;
            var followChecked = $('#followCheckDiv').find('div').hasClass('checked');
            $('#processing-modal').modal("show");
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/WellDetails/Follow',
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

        //Move page up on image click
        $("#moveTop").on("click", function () {
            $("body").scrollTop(0);
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
