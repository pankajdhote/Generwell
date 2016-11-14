//Add field

var loginPage = {

    initialize: function (data) {
        //debugger;
        loginPage.attachEvents();
    },
    attachEvents: function () {
        debugger;

        $('#supportId').click(function () {
            debugger;
            $('#myPleaseWait').modal('show');
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
                        $('#myPleaseWait').modal('hide');
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
        //$('#loginPageForm').validate();
        if ($('#form').has('.has-error')) {

        }
        $('#myPleaseWait').modal('show');
        var param1 = loginPage.serializeObject($(parameter1).serializeArray());
        $.ajax({
            url: requestUrl,
            method: 'POST',
            dataType: 'json',
            async: true,
            data: { model: JSON.stringify(param1) },
            success: function (data, status, jqXHR) {
                $('#myPleaseWait').modal('hide');
                debugger;
            },
            error: function (jqXHR, status, err) {
                debugger;
                $('#myPleaseWait').modal('hide');
            },
            complete: function (jqXHR, status) {
                debugger;
                $('#pleaseWaitDialog').hide();
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
    },
    showPleaseWait: function () {
        $('#pleaseWaitDialog').modal('show');
    },
    hidePleaseWait: function () {
        $('#pleaseWaitDialog').modal('hide');
    }
}
