var Common = {

    url: window.location.href,
    pageType: window.location.href.split('/')[3].toString(),
    id: window.location.href.split('/')[5] == undefined ? '' : window.location.href.split('/')[5].toString(),

    // The approach to datatables is variable throughout the project with.  I have created this function which takes a selector and applies a couple of key standards. 
    // please reuse it, and extend as necessary.  we should have one or two calls to datatables in the entire project - what way also allows us to easiy switch out to 
    // another library
    //
    // 1. If there is < 11 rows, do not show a search or a sort window
    // 2. Enable sorting
    loadDataTableStandard: function (tableSelector, noDataSelector) {

        // Count rows
        var fieldTablerowCount = $(tableSelector + ' tr').length;

        // If a no-data selector used
        if (noDataSelector) {

            // If rows found, hide no data message otherwise show it
            if (fieldTablerowCount > 1) {
                $(noDataSelector).hide();
            }
            else {
                $(noDataSelector).show();
            }
        }

        //        "order": [[0, "desc"]],
        //"AutoWidth": true,
        //        "tableTools": {
        //            "sSwfPath": "../../scripts/plugins/dataTables/swf/copy_csv_xls_pdf.swf"
        //        }

        // Disable paging, searching and export if 10 or less rows
        if (fieldTablerowCount < 11) {
            $(tableSelector).DataTable({
                destroy: true,
                "paging": false,
                "searching": false,
                "order": [[0, "desc"]]
            });
        }
        else {
            $(tableSelector).DataTable({
                destroy: true,
                "paging": true,
                "order": [[0, "desc"]],
                "tableTools": {
                    "sSwfPath": "../../scripts/plugins/dataTables/swf/copy_csv_xls_pdf.swf"
                }

            });
        }
    },


    loadDataTableGrid: function () {

        // By default the base datatable assumes the last column is an actions column not requiring sort functionlaity
        $('.dataTables-example').DataTable({
            "bDestroy": true,
            "paging": true,
            "dom": 'T<"clear">lfrtip',
            "tableTools": {
                "sSwfPath": "../../scripts/plugins/dataTables/swf/copy_csv_xls_pdf.swf"
            },
            "columnDefs": [
                { orderable: false, targets: -1 }
            ]
        });


    },
    ubloadDataTableGrid: function () {

        $('.dataTables-example').dataTable().fnDestroy();
    },

    loadDatePicker: function () {
        $('.input-group.date').datepicker({
            todayBtn: "linked",
            keyboardNavigation: false,
            forceParse: false,
            calendarWeeks: true,
            autoclose: true,
            format: 'dd-M-yy',
        });
    },

    loadClockPickers: function () {
        $('.clockpicker').clockpicker();
    },
    loadiCheck: function () {
        $('.i-checks').iCheck({
            checkboxClass: 'icheckbox_square-green',
            radioClass: 'iradio_square-green',
        });
    },
    loadDraggableEvent: function () {
        $('#external-events div.external-event').each(function () {
            // store data so the calendar knows to render an event upon drop
            $(this).data('event', {
                title: $.trim($(this).text()), // use the element's text as the event title
                stick: true // maintain when user navigates (see docs on the renderEvent method)
            });
            // make the event draggable using jQuery UI
            $(this).draggable({
                zIndex: 1111999,
                revert: true,      // will cause the event to go back to its
                revertDuration: 0  //  original position after the drag
            });

        });
    },
    loadDataTableGrid1: function () {
        console.log('base load-1');
        var rowCount = $('.dataTables-example tr').length;
        if (rowCount < 10) {
            $('.dataTables-example').DataTable({
                // "aaSorting": [],
                "bDestroy": true,
                "paging": false,
                "dom": 'T<"clear">lfrtip',
                "order": [[0, "desc"]],
                "tableTools": {
                    "sSwfPath": "../../scripts/plugins/dataTables/swf/copy_csv_xls_pdf.swf"
                }
            });
        }
        else {
            $('.dataTables-example').DataTable({
                // "aaSorting": [],
                "bDestroy": true,
                "paging": true,
                "dom": 'T<"clear">lfrtip',
                "order": [[0, "desc"]],
                "tableTools": {
                    "sSwfPath": "../../scripts/plugins/dataTables/swf/copy_csv_xls_pdf.swf"
                }
            });
        }

        // By default the base datatable assumes the last column is an actions column not requiring sort functionlaity      

        $('.dataTables-example').on('draw.dt', function () {
            $('#links').removeClass('sorting');
        });
        $('#links').removeClass('sorting');

    },

    //This method displays notification bar on top of the page
    ShowNotification: function (status, message) {
        var content;
        var closeBtnAndMessage = '<button aria-hidden="true" data-dismiss="alert" class="close" type="button">×</button>' + message;
        if (status == Constants.Notification_Success) {
            content = '<div class="alert alert-success alert-dismissable">' + closeBtnAndMessage + '</div>';
        }
        else if (status == Constants.Notification_Error) {
            content = '<div class="alert alert-danger alert-dismissable">' + closeBtnAndMessage + '</div>';
        }
        else if (status == Constants.Notification_Info) {
            content = '<div class="alert alert-info alert-dismissable">' + closeBtnAndMessage + '</div>';
        }
        $('.notification-container').html(content);
    },

    // this method removes notification bar from page.
    HideNotification: function (status, message) {
        $('.notification-container').html('');
    },

    //this method shows error content in the specified container
    ShowErrorContent: function (container) {
        var content = '<h1 class="error">500 Error:</h1>'
                      + '<h1 class="error">Oops! Something went wrong. Please try again later.</h1>';
        $(container).html(content);
    }
};

function displayDateFormat(date) {    
    var dateFormat = "";
    return dateFormat;
}





