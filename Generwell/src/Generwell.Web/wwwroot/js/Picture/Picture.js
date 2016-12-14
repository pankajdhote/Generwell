//Add field

var picturePage = {

    initialize: function () {
        debugger;
        picturePage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        picturePage.addPicture();

    },
    addPicture: function () {
        $('#addPicture').click(function () {
            debugger;
            $.ajax({
                url: '/Picture/AddPicture',
                type: 'POST',
                dataType: 'json',
                cache: false,
                success: function (response) {
                    debugger;

                }, error: function (err) {
                    debugger;
                }
            });
        });
    }
}
