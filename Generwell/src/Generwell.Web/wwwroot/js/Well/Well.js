//Add field

var wellPage = {

    initialize: function (data) {
        //debugger;
        wellPage.attachEvents();
    },
    attachEvents: function () {
        debugger;

        $('#IsFavorite').change(function () {
            debugger;
            $('#myPleaseWait').modal('show');
            $.ajax({
                url: "/Well/Index",
                type: "GET",
                contentType: "application/json; charset=utf-8",
                data: { isFavorite: $('#IsFavorite').prop('checked') },
                success: function (data) {
                    debugger;
                    var result = $(data).find("table#wellListDivId");
                    $('#wellListDivId').html(result);
                },
                error: function (data) {
                }
            });
        });
    }
  
}
