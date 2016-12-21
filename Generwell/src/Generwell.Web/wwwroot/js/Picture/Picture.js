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
        picturePage.uploadPicture();
        picturePage.validateImage();

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
            var albumId = $('#albumId').val();
            $('#processing-modal').modal("show");
            var url = '/Picture/AddPicture' + '?albumId=' + Base64.encode(albumId);
            window.location.href = url;

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

                //Make label and comment editable.
                $('#label').removeAttr('disabled');
                $('#comment').removeAttr('disabled');
                $('#editPicture').attr('value', 'Save');
                $('#editPicture').attr('id', 'savePicture');

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
            $('#savePicture').unbind().click(function () {
                debugger;
                picturePage.updatePicture();
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
              swal("Deleted!", "Picture deleted successfully", "success");
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
    },
    uploadPicture: function () {
        debugger;
        $("#fileupload").change(function () {
            debugger;
            $("#dvPreview").html("");
            var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
            if (regex.test($(this).val().toLowerCase())) {
                if ($.browser != undefined && $.browser.msie && parseFloat(jQuery.browser.version) <= 9.0) {
                    $("#dvPreview").show();
                    $("#dvPreview")[0].filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = $(this).val();
                }
                else {
                    if (typeof (FileReader) != "undefined") {
                        $("#dvPreview").show();
                        $("#dvPreview").append("<img />");
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            $("#dvPreview img").attr("src", e.target.result);
                        }
                        reader.readAsDataURL($(this)[0].files[0]);
                        $("#dvPreview img").addClass("img-responsive wh-300");
                        

                    } else {
                        swal("Warning!", "This browser does not support FileReader.");
                    }
                }
            } else {
                swal("Warning!", "Please upload a valid image file.");
            }
        });
    },
    validateImage: function () {
        
        $('form[name=pictureForm]').submit(function () {
            debugger;
            $('#processing-modal').modal("show");
            var imgTag = $('#dvPreview img').attr('src');
            var label = $.trim($('#label').val());
            var comment = $.trim($('#comment').val());

            if (imgTag == undefined) {
                $('#imageError').text("<b>Picture is required.</b>");
                $('#labelError').text("");
                $('#commentError').text("");
                $('#processing-modal').modal("hide");
                return false;
            } else if (label == "")
            {
                $('#labelError').text("Label name is required.");
                $('#imageError').text("");
                $('#commentError').text("");
                $('#processing-modal').modal("hide");
                return false;
            }
            else if (comment == "") {
                $('#commentError').text("Comment is required.");
                $('#imageError').text("");
                $('#labelError').text("");
                $('#processing-modal').modal("hide");
                return false;
            }
        });
    }
}
