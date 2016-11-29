//Add field

var wellPage = {

    initialize: function (targetUrl) {
        debugger;
        wellPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;
       
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });

        //create Generic datatable
        var dataTable = $('#wellListTableId').DataTable({
            "columnDefs": [
                { "orderable": false, "targets": 0 },
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                 {
                     "targets": [8],
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
        var oTable = $('#wellListTableId').DataTable();
        $('.iCheck-helper').on("click", function () {
            debugger;
            if ($(this).parent().attr("class").indexOf("checked") > -1) {
                oTable
                  .columns(8)
                  .search("^" + "True" + "$", true, false, false)
                  .draw();
            } else {
                oTable
                  .columns(8)
                  .search("")
                  .draw();
            }
        });
        //On click of datatable row redirect to well line report page.
        $('#wellListTableId tbody').on('click', 'tr td', function (event) {
            debugger;
            $('#processing-modal').modal("show");
            if (event.currentTarget.children[0] != undefined) {
                var followChecked = event.currentTarget.children[0].id;
                var wellId = event.currentTarget.children[0].name;
                if (followChecked != undefined) {
                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: '/Well/Follow',
                        data: { isFollow: followChecked, wellId: wellId },
                        success: function (Data) {
                            debugger;
                            var filterId = $('#FilterList option:selected').val();
                            $.ajax({
                                type: 'GET',
                                dataType: 'html',
                                url: '/Well/FilterWell',
                                data: { id: filterId },
                                success: function (data) {
                                    debugger;
                                    if (data != undefined || data != "") {
                                        $("#wellTableDivId").html(data);
                                        //display only my wells
                                        //On checkbox click filter data tables rows
                                        debugger;
                                        wellPage.mywellFilter();
                                        $('#processing-modal').modal("hide");
                                    }
                                },
                                error: function (XMLHttpRequest, textStatus, errorThrown) {
                                    $('#processing-modal').modal("hide");
                                }
                            });
                        },
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            $('#processing-modal').modal("hide");
                        }
                    });
                } 
            } else {
                var data = oTable.row($(this).parent()).data();
                //Perform your navigation
                window.location.href = targetUrl + '?wellId=' + data[0] + '&wellName=' + data[2] + '&isFollow=' + data[8];
            }
        });

        //on back button click redirect to task details report page
        $('#backTaskDetailsId').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            var targetUrl = '/TaskDetails/Index/';
            window.location.href = targetUrl;
        });

        //filter particular record on filter value
        //Follow or unfollow particular well 
        $('#FilterList').unbind().bind("change", function () {
            debugger;
            $('#processing-modal').modal("show");
            var filterId = $('#FilterList option:selected').val();
            if (filterId.indexOf("Select") > -1) {
                filterId = null;
            }
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Well/FilterWell',
                data: { id: filterId },
                success: function (data) {
                    debugger;
                    if (data != undefined || data != "") {
                        $("#wellTableDivId").html(data);
                        $('#processing-modal').modal("hide");
                        //display only my wells
                        //On checkbox click filter data tables rows
                        debugger;
                        wellPage.mywellFilter();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });
    },

    mywellFilter:function(){
        var oTable = $('#wellListTableId').DataTable();
        if ($('.iCheck-helper').parent().attr("class").indexOf("checked") > -1) {
            oTable
              .columns(8)
              .search("^" + "True" + "$", true, false, false)
              .draw();
        } else {
            oTable
              .columns(8)
              .search("")
              .draw();
        }
    },
    followedWell: function (event) {
        debugger;
        var followChecked = event.id;
        var wellId = event.name;
        $('#processing-modal').modal("show");
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: '/Well/Follow',
            async: false,
            data: { isFollow: followChecked, wellId: wellId },
            success: function (Data) {
                debugger;
                $('#processing-modal').modal("hide");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $('#processing-modal').modal("hide");
            }
        });
    }
}
