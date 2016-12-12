//Add field

var TaskDetailsPage = {

    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;


        //$(function () {
        //    $("#dialog1").dialog({
        //        autoOpen: false
        //    });
        //});

        $(function () {
            $("#SaveTaskFieldDetailsId2").click(function () {
                debugger;
                $("#dialog1").dialog('open');
            });
        });
       
        $("#Complete_Yes").click(function () {

            $("#dialog1").dialog('close');
            debugger;
            var IdArray = [];
            var ValueArray = [];
            var Content = []
            var count = 0;
            $('.clsedit').each(function () {
                var htmlType = $(this).prop('tagName')
                //var fieldTypeId = document.getElementById("fieldTypeId").value;
                if (htmlType == 'SELECT') {
                    var txt = $(this).find(":selected").val();
                    IdArray.push(this.id);
                    ValueArray.push(txt);
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                }
                else if (htmlType == 'INPUT') {
                    IdArray.push(this.id);
                    ValueArray.push(this.value);

                    if (this.name == "date2") {

                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  1481020522  }");
                    }
                    else if (this.name == "checkbox") {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + ValueArray[count] + "   }");
                    }
                    else {
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                    }
                }

                count++;
            });

            $.ajax({
                type: "post",
                url: '/taskdetails/updatetaskfields',
                data: { Content: Content },
                datatype: "json",
                cache: false,
                processdata: true,
                success: function (data, status, xhr) {
                    //this.find('.SaveTaskFieldDetailsId').attr('value', 'Confirmed');
                    //alert(status);
                },
                error: function (xhr) {
                    //alert(xhr.responsetext);
                    //setTimeout(function () { $('#SaveMessage').css('display', 'inline'); }, 1000);
                }
            });
        });
        $(function () {
            $("#SaveTaskFieldDetailsId2").click(function () {
                $("#dialog1").dialog('open');
            });
        });

        $("#Complete_No").click(function () {
            $("#dialog1").dialog('close');
        });

        $("#taskDetailsListTableId1").change(function () {

            document.getElementById("SaveTaskFieldDetailsId2").value = "Save";
            document.getElementById("SaveTaskFieldDetailsId2").id = "SaveTaskFieldDetailsId";

        });

        $("#SaveTaskFieldDetailsId").click(function () {
            debugger;
                var IdArray = [];
                var ValueArray = [];
                var Content = []
                var count = 0;
                $('.clsedit').each(function () {
                    var htmlType = $(this).prop('tagName')
                    //var fieldTypeId = document.getElementById("fieldTypeId").value;
                    if (htmlType == 'SELECT') {
                        var txt = $(this).find(":selected").val();
                        IdArray.push(this.id);
                        ValueArray.push(txt);
                        Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");

                    }
                    else if (htmlType == 'INPUT') {
                        IdArray.push(this.id);
                        ValueArray.push(this.value);

                        if (this.name == "date2") {

                            Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\":  1481020522  }");
                        }
                        else if (this.name == "checkbox") {
                            Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + ValueArray[count] + "   }");
                        }
                        else {
                            Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                        }
                    }

                    count++;
                });

                $.ajax({
                    type: "post",
                    url: '/taskdetails/updatetaskfields',
                    data: { Content: Content },
                    datatype: "json",
                    cache: false,
                    processdata: true,

                    success: function (data, status, xhr) {
                        //this.find('.SaveTaskFieldDetailsId').attr('value', 'Confirmed');
                        //alert(status);
                    },
                    error: function (xhr) {
                        //alert(xhr.responsetext);
                        //setTimeout(function () { $('#SaveMessage').css('display', 'inline'); }, 1000);

                    }
                });
            });
       
    }
}








