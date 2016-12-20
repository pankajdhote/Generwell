﻿//Add field

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
        picturePage.redirectToPreviousPage();

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
    editPicture: function (fileUrl, label, comment) {
        debugger;
        $('#processing-modal').modal("show");
        var url = '/Picture/EditPicture' + '?fileUrl=' + Base64.encode(fileUrl) + '&label=' + Base64.encode(label) + '&comment=' + Base64.encode(comment);
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
        $('#editPicture').click(function () {
            $('#label').removeAttr('disabled');
            $('#comment').removeAttr('disabled');
            $('#editPicture').attr('value', 'Save');
            $('#editPicture').attr('id', 'savePicture');

            $('#savePicture').click(function () {
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
              $('#processing-modal').modal("hide");
              swal("deleted!", "Picture deleted successfully", "success");
          });
        });

    },
    updatePicture: function () {
        debugger;
        var content=getViewData();

        $('#processing-modal').modal("show");
        debugger;
        $.ajax({
            type: "GET",
            url: '/Picture/UpdatePicture',
            data: { Content: JSON.stringify(content) },
            datatype: "json",
            cache: false,
            success: function (response) {
                debugger;
                $('#processing-modal').modal("hide");

            }, error: function (err) {
                $('#processing-modal').modal("hide");
            }
        });

    },
    redirectToPreviousPage: function () {
        var url = '/Picture/Index';
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
                IdArray.push(this.id);
                ValueArray.push(this.value);
                Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
            }
            count++;
        });
        return Content;
    },
}
