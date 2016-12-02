//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;

        //start datatable
        //var dataTable = $('#taskDetailsListTableId').DataTable({
        //    //"bPaginate": false,
        //    //"bInfo": false,
        //    //"searching": false,
        //    //"columnDefs": [       
        //        {
        //            "targets": [0],
        //            "visible": false,
        //            "searchable": false
        //        },
        //         {
        //             "targets": [3],
        //             "visible": false,
        //             "searchable": false
        //         },
        //        {
        //            // The `data` parameter refers to the data for the cell (defined by the
        //            // `data` option, which defaults to the column being worked with, in
        //            // this case `data: 0`.
        //            "render": function (data, type, row) {
        //                return data + ' (' + row[3] + ')';
        //            },
        //            "targets": 0
        //        },
        //    ],
        //});
        //On checkbox click filter data tables rows
        //var oTable = $('#taskDetailsListTableId').DataTable();       
        //$('#taskDetailsListTableId tbody').on('click', 'tr', function () {
        //    debugger;
        //    var data = oTable.row(this).data();
        //    //Perform your navigation
        //    window.location.href = targetUrl + '?reportId=' + data[0];
        //});
        //End datatable


        //Follow or unfollow particular task 
        $('#followTaskId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/TaskDetails/Follow',
                data: { isFollow: $('#followTaskId').prop('checked') },
                success: function (Data) {
                    debugger;
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#myPleaseWait').modal('hide');
                }
            });
        });
        $('.datepicker').datepicker({
            format: 'mm/dd/yyyy',
            startDate: '-3d'
        });

        $("#SaveTaskFieldDetailsId").click(function () {
            debugger;
            //var fieldId = "6";      
            var fieldId = document.getElementById("FieldId").value;
            //var value = document.getElementById("item_displayValue").value;
            var value = document.getElementById("displayValue").value;
            $.ajax({
                type: "POST",
                url: '/TaskDetails/UpdateTaskFields',
                data: { fieldId: fieldId, value: value },
                dataType: "json",
                processData: true,
                success: function (data, status, xhr) {
                    alert(status);
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        });
    }
}








