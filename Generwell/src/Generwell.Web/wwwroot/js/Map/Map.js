//Add field

var mapPage = {

    initialize: function () {
        debugger;
        mapPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        //on page unload get datatable rows and store in collection
        $(window).unload(function () {
            debugger;
            $.ajax({
                type: 'GET',
                dataType: 'html',
                url: '/Map/SetGooleMapObjects',
                async: false,
                data: { isMyWell: null, filterId: null, previousPage: "6" },
                success: function (response) {
                    debugger;
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#processing-modal').modal("hide");
                }
            });
        });

        $.ajax({
            type: "GET",
            url: '/Map/PlotMarker',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                debugger;
                var locations = result;

                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 3,
                    //maxZoom: 13,
                    minZoom: 3,
                    center: new google.maps.LatLng(locations[0].latitude, locations[0].longitude),
                    disableDefaultUI: false,
                    scaleControl: false,
                    zoomControl: false,
                    zoomControlOptions: {
                        style: google.maps.ZoomControlStyle.SMALL
                    },
                    mapTypeControl: true,
                    draggableCursor: 'move',
                    mapTypeId: google.maps.MapTypeId.satellite
                });

                var infowindow = new google.maps.InfoWindow({
                    maxWidth: 500
                });


                ////show geological location
                //if (navigator.geolocation) {
                //    navigator.geolocation.getCurrentPosition(showPosition);
                //} else {
                //    alert("Geolocation is not supported by this browser.");
                //}

                //function showPosition(position) {
                //    var lat = position.coords.latitude;
                //    var lng = position.coords.longitude;
                //    map.setCenter(new google.maps.LatLng(lat, lng));
                //}

                var markers = new Array();

                // Add the markers and infowindows to the map       
                var markers = [];
                for (var i = 0; i < locations.length; i++) {
                    var latLng = new google.maps.LatLng(locations[i].latitude, locations[i].longitude);
                    var marker = new google.maps.Marker({ 'position': latLng });

                    if (locations[i].latitude != 56.1304) {
                        if (locations[i].isFavorite == true) {
                            marker.setIcon('/images/favorite-location.png');
                        }
                        else {
                            marker.setIcon('/images/location.png');
                        }
                    }

                    markers.push(marker);

                    google.maps.event.addListener(marker, 'click', (function (marker, i) {
                        return function () {
                            infowindow.setContent('<img src="/images/car-Icon.png" /> &nbsp; <a href="/WellLineReport/Index?wellId=' + locations[i].id + '&wellName=' + locations[i].name + '&isFollow=' + locations[i].isFavorite + '">' + locations[i].name) + '</a>';
                            infowindow.open(map, marker);
                        }
                    })(marker, i));
                }

                //var markerCluster = new MarkerClusterer(map, markers, { imagePath: '~/images/m1.png' });
                var markerCluster = new MarkerClusterer(map, markers,
                                    {
                                        imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                                    });

                google.maps.event.addListener(marker, 'click', (function (marker, i) {
                    return function () {
                        infowindow.setContent(locations[i].name);
                        infowindow.open(map, marker);
                    }
                })(marker, i));


                function autoCenter() {
                    //  Create a new viewpoint bound
                    var bounds = new google.maps.LatLngBounds();
                    //  Go through each...
                    for (var i = 0; i < markers.length; i++) {
                        bounds.extend(markers[i].position);
                    }
                    //  Fit these bounds to the map
                    map.fitBounds(bounds);
                }
                autoCenter();
            }
        });
    }
}
