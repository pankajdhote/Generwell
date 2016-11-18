//Add field

var wellPage = {

    initialize: function (targetUrl) {
        //debugger;
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
                     "targets": [7],
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
            if ($(this).parent().attr("class").indexOf("checked")>-1) {
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
        $('#wellListTableId tbody').on('click', 'tr', function () {
            debugger;
             $('#processing-modal').modal("show");
            var data = oTable.row(this).data();
            //Perform your navigation
            window.location.href = targetUrl + '?wellId=' + data[0] + '&wellName=' + data[1] + '&isFollow=' + data[7];
        });


        //filter particular record on filter value
        //Follow or unfollow particular well 
        $('#FilterList').change(function () {
            debugger;
            $('#processing-modal').modal("show");
            var filterId = $('#FilterList option:selected').text();
            if (filterId.indexOf("Select") > -1)
            {
                filterId = null;
            }
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Well/FilterWell',
                data: { id: filterId },
                success: function (data) {
                    debugger;
                    if (data != undefined || data != "")
                    {
                        $("#wellTableDivId").html(data);
                        $('#processing-modal').modal("hide");

                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });

    }
}
