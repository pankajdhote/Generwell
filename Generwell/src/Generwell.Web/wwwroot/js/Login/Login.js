//Add field

var loginPage = {

    initialize: function () {
        loginPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        loginPage.setFieldsForGoogleMap();
        loginPage.validateForm();
        loginPage.showSupportDialogBox();
        loginPage.keepLicenseAlive();
    },
    setFieldsForGoogleMap: function () {
        //on page unload get datatable rows and store in collection
        $('#menu li').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            var menuName = $(this).text().trim();
            if (menuName.indexOf("MAP") > -1) {
                var filterId = $('#FilterList option:selected').val();
                if (filterId != undefined && filterId.indexOf("Select") > -1) {
                    filterId = null;
                }
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
                    cache: false,
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
    },
    validateForm: function () {
        //  Bind the event handler to the "submit" JavaScript event
        $('form[name=loginForm]').submit(function () {
            debugger;
            $('#error').text("");
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
        });
    },
    showSupportDialogBox: function () {
        $('#supportId').click(function () {
            debugger;
            $('#processing-modal').modal("show");
            $.ajax({
                url: "/Accounts/Support",
                type: "GET",
                cache: false,
                async: true,
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
    },
    keepLicenseAlive: function () {
        window.setInterval(function () {
            debugger;
            /// call your function here
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Base/KeepLicenseAlive',
                async: true,
                cache: false,
                success: function (data) {
                    debugger;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }, 50000);
    }
}
