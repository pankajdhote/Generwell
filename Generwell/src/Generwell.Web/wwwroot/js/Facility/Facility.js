//Add field

var facilityPage = {

    initialize: function (targetUrl) {
        debugger;
        facilityPage.attachEvents(targetUrl);
    },
    attachEvents: function (targetUrl) {
        debugger;
        facilityPage.createMyFilterCheckbox();
        facilityPage.createDatatable();
        facilityPage.filterByCheckbox();
        facilityPage.filterDatatableByDropdown();
        facilityPage.redirectEvent(targetUrl);

    },
    myFacilityFilter: function () {
        var oTable = $('#facilityListTableId').DataTable();
        if ($('.iCheck-helper').parent().attr("class").indexOf("checked") > -1) {
            oTable
              .columns(8)
              .search("^" + "True" + "$", true, false, false)
              .draw();
        } else {
            oTable
              .columns(7)
              .search("")
              .draw();
        }
    },
    filterByCheckbox: function () {
        //On checkbox click filter data tables rows
        var oTable = $('#facilityListTableId').DataTable();
        $('.iCheck-helper').on("click", function () {
            debugger;
            if ($(this).parent().attr("class").indexOf("checked") > -1) {
                oTable
                  .columns(7)
                  .search("^" + "True" + "$", true, false, false)
                  .draw();
            } else {
                oTable
                  .columns(7)
                  .search("")
                  .draw();
            }
        });
    },
    createMyFilterCheckbox: function () {
        //Added for checkbox style
        $(".i-checks").iCheck({
            checkboxClass: "icheckbox_square-green",
            radioClass: "iradio_square-green"
        });
    },
    createDatatable: function () {
        //create Generic datatable
        var dataTable = $('#facilityListTableId').DataTable({
            "columnDefs": [
                {
                    "targets": [0],
                    "visible": false,
                    "searchable": false
                },
                 {
                     "targets": [7],
                     "visible": false,
                     "searchable": true
                 },
            ],
        });
    },
    filterDatatableByDropdown: function () {
        //filter particular record on filter value
        $('#FilterList').unbind().bind("change", function () {
            debugger;
            $('#processing-modal').modal("show");
            var filterId = $('#FilterList option:selected').val();
            if (filterId.indexOf("Select") > -1) {
                filterId = null;
            }
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Well/FilterFacility',
                data: { id: filterId },
                cache: false,
                success: function (data) {
                    debugger;
                    if (data != undefined || data != "") {
                        $("#facilityTableDivId").html(data);
                        $('#processing-modal').modal("hide");
                        //On checkbox click filter data tables rows
                        facilityPage.myFacilityFilter();
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });
    },
    redirectWellLineReportPage: function (targetUrl, event) {
        debugger;
        //On click of datatable row redirect to well line report page.
        var oTable = $('#wellListTableId').DataTable();
        var data = oTable.row($(event).parent()).data();
        var url = targetUrl + '?wellId=' + Base64.encode(data[0]) + '&wellName=' + Base64.encode(data[2]) + '&isFollow=' + Base64.encode(data[8]) + '&latitude=' + Base64.encode(data[6]) + '&longitude=' + Base64.encode(data[7]);
        window.location.href = url;
    },
    redirectEvent: function (targetUrl) {
        //On click of datatable row redirect to well line report page.
        $('#wellListTableId tbody').on('click', 'tr td', function (event) {
            debugger;
            $('#myTask').prop('checked', false);
            $('#processing-modal').modal("show");
            if (event.currentTarget.children[0] != undefined) {
                wellPage.wellFollowUnfollow(event);
            } else {
                wellPage.redirectWellLineReportPage(targetUrl, this);
            }
        });
    },
    wellFollowUnfollow: function (event) {
        var wellId = parseInt(event.currentTarget.children[0].name);
        var followChecked = event.currentTarget.children[0].id;
        if (followChecked != undefined) {
            var filterId = $('#FilterList option:selected').val();

            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Well/SetFollowUnfollow',
                data: { isFollow: followChecked, wellId: wellId, filterId: filterId },
                cache: false,
                success: function (response) {
                    debugger;
                    if (response != undefined || response != "") {
                        $("#wellTableDivId").html(response);
                        //On checkbox click filter data tables rows
                        wellPage.myWellFilter();
                        $('#processing-modal').modal("hide");
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        }
    }
}


