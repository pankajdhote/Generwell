//Add field

var TaskDetailsPage = {
    initialize: function (taskId, targetUrl) {
        debugger;
        TaskDetailsPage.attachEvents(taskId, targetUrl);
    },
    attachEvents: function (taskId, targetUrl) {
        debugger;
        $("#SaveTaskFieldDetailsId").click(function () {
            debugger;
            var IdArray = [];
            var ValueArray = [];
            var Content = []
            var count = 0;
            $('.clsedit').each(function () {
                var htmlType = $(this).prop('tagName')
                if (htmlType == 'SELECT') {
                    var txt = $(this).find(":selected").text();
                    IdArray.push(this.id);
                    ValueArray.push(txt);
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
                }
                else if (htmlType == 'INPUT') {
                    IdArray.push(this.id);
                    ValueArray.push(this.value);
                    Content.push("{ \"op\": \"replace\", \"path\": \"/Fields/" + IdArray[count] + "\", \"value\": " + "\"" + ValueArray[count] + "\"}");
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
                    this.find('.SaveTaskFieldDetailsId').attr('value', 'Confirmed');
                },
                error: function (xhr) {
                    alert(xhr.responsetext);
                }
            });
        });
    }
}








