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

        $("#SaveTaskFieldDetailsId").click(function () {
           
            debugger;


            var IdArray = [];
            var ValueArray = [];
            var Content=[]
            var count = 0;
            $('.clsedit').each(function ()
            {
                var htmlType = $(this).prop('tagName')
                if (htmlType == 'SELECT')
                {
                    var txt = $(this).find(":selected").text();
                    IdArray.push(this.id);
                    ValueArray.push(txt);
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");

                }
                else if (htmlType == 'INPUT')
                {
                  IdArray.push(this.id);
                  ValueArray.push(this.value);
                  Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");

                }

                                
                //alert(Content.length)
                //alert(Content)
                count++;
            });
            
            $.ajax({
                type: "post",
                url: '/taskdetails/updatetaskfields',
                data: {Content: Content },
                datatype: "json",
                processdata: true,
                success: function (data, status, xhr) {
                    this.find('.SaveTaskFieldDetailsId').attr('value', 'Confirmed');
                },
                error: function (xhr) {
                    alert(xhr.responsetext);
                }
            });

        });

    }
}








