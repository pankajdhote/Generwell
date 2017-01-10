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
                    data: { isMyAssets: isMyWell, filterId: filterId },
                    success: function (data) {
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
         
            $('#processing-modal').modal("show");
            $.ajax({
                type: "GET",
                url: "/Base/DisplaySupportPopup",
                datatype: "html",
                cache:false,
                success: function (data) {
                 
                    if (data != undefined || data != "" || data != null) {
                        $('#myModalContent').html(data);
                        $('#processing-modal').modal("hide");
                        $('#myModal').modal('show');
                    }
                },
                error: function (data) {
                    swal("Error", "Something went wrong.")
                }
            });
        });
    },
    keepLicenseAlive: function () {
        window.setInterval(function () {
         
            /// call your function here
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Base/KeepLicenseAlive',
                async: true,
                cache: false,
                success: function (data) {
                   
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                }
            });
        }, 50000);
    },
    capLock: function (e) {
       
        kc = e.keyCode ? e.keyCode : e.which;
        sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
        if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {
            $('#capsCheck').attr('style', 'display:block');
        }
        else {
            $('#capsCheck').attr('style', 'display:none');
        }
    }
}