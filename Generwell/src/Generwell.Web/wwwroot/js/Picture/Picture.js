//Add field

var picturePage = {

    initialize: function () {
        debugger;
        picturePage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        picturePage.notification();
        picturePage.addPicture();
        picturePage.createCheckbox();
        picturePage.swapRadioButton();
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
                $('#deletePicture').removeClass('button');
                $('#deletePicture').attr('disabled', 'disabled');
                //Make label and comment editable.
                $('#label').removeAttr('readonly');
                $('#comment').removeAttr('readonly');

            } else {
                $('#edit').parent().removeClass('checked');
                $('#delete').parent().addClass('checked');
                $('#deletePicture').removeAttr('disabled', '');
                $('#deletePicture').addClass('button');
                //Disabled label and comment
                $('#label').attr('readonly', 'readonly');
                $('#comment').attr('readonly', 'readonly');
            }
        });
    },
    deletePicture: function () {
        $('#deletePicture').unbind().click(function () {
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
              $('#flagCheck').val("Deleted");
              var flagCheck = $('#flagCheck').val();
              picturePage.redirectToPreviousPage(flagCheck);
          });
        });

    },
    deletePictureCall: function () {
        debugger;
        var pictureId = $('#pictureId').val();
        var albumId = $('#albumId').val();
        $.ajax({
            type: "GET",
            url: '/Picture/DeletePicture',
            data: { pictureId: pictureId, albumId: albumId },
            datatype: "json",
            cache: false,
            success: function (response) {
                debugger;
            }, error: function (err) {
                $('#processing-modal').modal("hide");
            }
        });
    },
    redirectToPreviousPage: function (flagCheck) {
        debugger;
        var albumId = $('#albumId').val();
        var url = '/Picture/Index?id=' + Base64.encode(albumId) + '&flagCheck=' + Base64.encode(flagCheck != undefined ? flagCheck : "");
        window.location.href = url;
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
                $('#imageError').html("<b>Picture is required.</b>");
                $('#labelError').html("");
                $('#commentError').html("");
                $('#processing-modal').modal("hide");
                return false;
            } else if (label == "") {
                $('#labelError').html("<b>Label name is required.</b>");
                $('#imageError').html("");
                $('#commentError').html("");
                $('#processing-modal').modal("hide");
                return false;
            }
            else if (comment == "") {
                $('#commentError').html("<b>Comment is required.</b>");
                $('#imageError').html("");
                $('#labelError').html("");
                $('#processing-modal').modal("hide");
                return false;
            }
        });
    },
    notification: function () {
        debugger;
        var flagCheck = $('#flagCheck').val();
        if (flagCheck != undefined && flagCheck != "") {
            $('#notification').show();
            $('#notification').html("<button aria-hidden='true' data-dismiss='alert' class='close' type='button'>×</button>Picture " + flagCheck + " Successfully.");
            setTimeout(function () { $('#notification').hide(); }, 5000);
        }
    }
}
