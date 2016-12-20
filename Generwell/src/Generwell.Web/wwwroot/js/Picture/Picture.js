//Add field

var picturePage = {

    initialize: function () {
        debugger;
        picturePage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        picturePage.addPicture();
        picturePage.createCheckbox();
        picturePage.swapRadioButton();
        picturePage.editPictureLabel();
        picturePage.deletePicture();
        picturePage.redirectToIndexPage();

    },
    redirectToIndexPage: function () {
        $('#cancel').click(function () {
            $('#processing-modal').modal("show");
            picturePage.redirectToPreviousPage();
        });
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
    },
    editPicture: function (fileUrl, label, comment, id, albumId) {
        debugger;
        $('#processing-modal').modal("show");
        var url = '/Picture/EditPicture' + '?fileUrl=' + Base64.encode(fileUrl) + '&label=' + Base64.encode(label != null ? label : "") + '&comment=' + Base64.encode(comment != null ? comment : "") + '&id=' + Base64.encode(id) + '&albumId=' + Base64.encode(albumId);
        window.location.href = url;
    },
    createCheckbox: function () {
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });
    },
    swapRadioButton: function () {

        $('.iCheck-helper').click(function () {
            debugger;
            if ($(this).parent().children().attr('id') == "edit") {
                $('#delete').parent().removeClass('checked');
                $('#edit').parent().addClass('checked');
                $('#editPicture').removeAttr('disabled');
                $('#editPicture').addClass('button');
                $('#deletePicture').removeClass('button');
                $('#deletePicture').attr('disabled', 'disabled');

                picturePage.editPictureLabel();

            } else {
                //remove click event
                $('#savePicture').unbind('click');

                //Swap save button
                $('#savePicture').attr('value', 'Edit');
                $('#savePicture').attr('id', 'editPicture');

                $('#edit').parent().removeClass('checked');
                $('#delete').parent().addClass('checked');
                $('#editPicture').attr('disabled', 'disabled');
                $('#deletePicture').addClass('button');
                $('#editPicture').removeClass('button');
                $('#deletePicture').removeAttr('disabled', '');

                //Disabled label and comment
                $('#label').attr('disabled', 'disabled');
                $('#comment').attr('disabled', 'disabled');

            }
        });
    },
    editPictureLabel: function () {
        debugger;
        $('#editPicture').bind('click', function () {
            debugger;
            $('#label').removeAttr('disabled');
            $('#comment').removeAttr('disabled');
            $('#editPicture').attr('value', 'Save');
            $('#editPicture').attr('id', 'savePicture');

            $('#savePicture').unbind().click(function () {
                debugger;
                picturePage.updatePicture();
            });

        });

    },
    deletePicture: function () {

        $('#deletePicture').click(function () {
            swal({
                title: "Delet Picture",
                text: "Are you sure?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#cf7f00",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            },
          function () {
              debugger;
              $('#processing-modal').modal("show");
              picturePage.deletePictureCall();
              swal("deleted!", "Picture deleted successfully", "success");
              picturePage.redirectToPreviousPage();
          });
        });

    },

    deletePictureCall: function () {
        debugger;
        var pictureId = $('#pictureId').val();
        $.ajax({
            type: "GET",
            url: '/Picture/DeletePicture',
            data: { pictureId: pictureId },
            datatype: "json",
            cache: false,
            success: function (response) {
                debugger;

            }, error: function (err) {
                $('#processing-modal').modal("hide");
            }
        });
    },
    updatePicture: function () {
        debugger;
        var content = picturePage.getViewData();
        var pictureId = $('#pictureId').val();
        $('#processing-modal').modal("show");
        debugger;
        $.ajax({
            type: "GET",
            url: '/Picture/UpdatePicture',
            data: { Content: JSON.stringify(content), pictureId: pictureId },
            datatype: "json",
            cache: false,
            success: function (response) {
                debugger;
                
                picturePage.redirectToPreviousPage();
            }, error: function (err) {
                $('#processing-modal').modal("hide");
            }
        });

    },
    redirectToPreviousPage: function () {
        debugger;
        var albumId = $('#albumId').val();
        var url = '/Picture/Index/' + Base64.encode(albumId);
        window.location.href = url;
    },
    getViewData: function () {
        debugger;
        $('#processing-modal').modal("show");
        var IdArray = new Array();
        var ValueArray = new Array();
        var Content = new Array();
        var count = 0;
        $('.clsedit').each(function () {
            debugger;
            var htmlType = $(this).prop('type');
            if (htmlType == 'text') {
                IdArray.push(this.name);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count].trim() + "\"}");
            }
            else if (htmlType == 'textarea') {
                IdArray.push(this.name);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count].trim() + "\"}");
            }
            count++;
        });
        return Content;
    }
}
