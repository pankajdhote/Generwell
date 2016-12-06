//Add field

var picturePage = {

    initialize: function () {
        debugger;
        picturePage.attachEvents();
    },
    attachEvents: function () {
        debugger;

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
