﻿//Add field

var loginPage = {

    initialize: function (data) {
        //debugger;
        loginPage.attachEvents();
    },
    attachEvents: function () {
        debugger;

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
    },
    MakeAjax: function (requestUrl, parameter1, event) {
        //show loading... image
        debugger;
        $('#processing-modal').modal("show");
        var param1 = loginPage.serializeObject($(parameter1).serializeArray());
        $.ajax({
            url: requestUrl,
            method: 'POST',
            dataType: 'json',
            async: true,
            data: { model: JSON.stringify(param1) },
            success: function (data, status, jqXHR) {
                $('#processing-modal').modal("hide");
                debugger;
            },
            error: function (jqXHR, status, err) {
                debugger;
                $('#processing-modal').modal("hide");
            },
            complete: function (jqXHR, status) {
                debugger;
                $('#processing-modal').modal("show");
            }
        });
    },
    serializeObject: function (serializeArray) {
        var object = {};
        var array = serializeArray;
        $.each(array, function () {
            if (object[this.name] !== undefined) {
                if (!object[this.name].push) {
                    object[this.name] = [object[this.name]];
                }
                object[this.name].push(this.value || '');
            } else {
                object[this.name] = this.value || '';
            }
        })
        return object;
    }
}
