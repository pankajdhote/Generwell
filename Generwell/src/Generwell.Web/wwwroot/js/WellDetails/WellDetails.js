//Add field

var wellDetailsPage = {
    initialize: function () {
        wellDetailsPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        wellDetailsPage.createFollowUnfollowSymbol();
        wellDetailsPage.createDatatable();
        wellDetailsPage.moveToTop();
        wellDetailsPage.setWellFollowUnfollow();
    },
    createFollowUnfollowSymbol: function () {
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-star",
            radioClass: "iradio_square-star"
        });
    },
    createDatatable: function () {
        //start datatable
        var dataTable = $('#wellDetailsListTableId').DataTable({
            "bPaginate": false,
            "bInfo": false,
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
                    "render": function (data, type, row) {
                        return data + ' (' + row[3] + ')';
                    },
                    "targets": 0
                },
            ],
        });
    },
    moveToTop: function () {
        //Move page up on image click
        $("#moveTop").on("click", function () {
            $("body").scrollTop(0);
        });
    },
    setWellFollowUnfollow: function () {
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
