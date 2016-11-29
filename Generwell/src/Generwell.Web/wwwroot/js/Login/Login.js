//Add field

var loginPage = {

    initialize: function () {
        //debugger;
        loginPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        //on page unload get datatable rows and store in collection
        $('#menu li').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            var menuName = $(this).text().trim();
            if (menuName.indexOf("MAP") > -1) {
                var filterId = $('#FilterList option:selected').val();
                var isMyWell;
                if ($('.iCheck-helper').parent().attr("class") != undefined && $('.iCheck-helper').parent().attr("class").indexOf("checked") > -1) {
                    isMyWell = true;
                } else {
                    isMyWell = false;
                }
                $.ajax({
                    type: 'GET',
                    dataType: 'html',
                    url: '/Map/SetGooleMapObjects',
                    async: false,
                    data: { isMyWell: isMyWell, filterId: filterId },
                    success: function (data) {
                        debugger;
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $('#processing-modal').modal("hide");
                    }
                });
            }
        });

        //  Bind the event handler to the "submit" JavaScript event
        $('form').submit(function () {
            debugger;

            // Get the Login Name value and trim it
            var name = $.trim($('#UserName').val());
            var password = $.trim($('#Password').val());
            var webApiUrl = $.trim($('#WebApiUrl').val());

            // Check if empty of not
            if (name === '') {
                return false;
            }
            if (password === '') {
                return false;
            }
            if (webApiUrl === '') {
                return false;
            }

            $('#processing-modal').modal("show");
            //$('.modal-open').removeClass();

        });

        $('#supportId').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            $.ajax({
                url: "/Accounts/Support",
                type: "GET",
                cache: false,
                async: false,
                contentType: "application/json; charset=utf-8",
                datatype: "json",
                success: function (data) {
                    debugger;
                    if (data != undefined || data != "" || data != null) {
                        $('#myModalContent').html(data);
                        $('#processing-modal').modal("hide");
                        $('#myModal').modal('show');
                    }
                },
                error: function (data) {
                    alert("Error");
                }
            });
        });
    }
}
