//Add field

var wellPage = {

    initialize: function (wellId) {
        //debugger;
        wellPage.attachEvents(wellId);
    },
    attachEvents: function (wellId) {
        debugger;

        $('#followWellId').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: '/WellLineReport/Follow',
                data: { id: wellId, isFollow: $('#followWellId').prop('checked') },
                success: function (Data) {
                    $('#myPleaseWait').modal('hide');
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                }
            });
            //$.ajax({
            //    url: "/WellLineReport/Follow",
            //    type: "POST",
            //    contentType: "application/json; charset=utf-8",
            //    data:{isFollow:true},
            //    success: function (data) {
            //        debugger;
            //        $('#myPleaseWait').modal('hide');
            //    },
            //    error: function (data) {
            //    }
            //});
        });
    }
  
}
