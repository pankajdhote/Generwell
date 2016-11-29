//Add field

var mapPage = {

    initialize: function () {
        debugger;
        mapPage.attachEvents();
    },
    attachEvents: function () {
        debugger;

        $.ajax({
            type: "GET",
            url: '/Map/PlotMarker',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                debugger;
                var locations = result;
                var latitude;
                var longitude;
                //set latitude and longitude
                if (locations[0] == undefined) {
                    latitude = 56.1304;
                    longitude = 106.3468;
                } else {
                    latitude = locations[0].latitude;
                    longitude = locations[0].longitude;
                }

                //Display user current location if location not found
                if (locations[0] == undefined) {
                    if (navigator.geolocation) {
                        var im = 'https://www.robotwoods.com/dev/misc/bluecircle.png';

                        navigator.geolocation.getCurrentPosition(function (p) {
                            debugger;
                            var LatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
                            var mapOptions = {
                                center: LatLng,
                                zoom: 14,
                                mapTypeId: google.maps.MapTypeId.ROADMAP
                            };
                            var map = new google.maps.Map(document.getElementById("map"), mapOptions);
                            var marker = new google.maps.Marker({
                                position: LatLng,
                                map: map,
                                icon: im,
                                title: "Your location:</b><br />Latitude: " + p.coords.latitude + "<br />Longitude: " + p.coords.longitude
                            });

                            // Add circle overlay and bind to marker
                            var circle = new google.maps.Circle({
                                map: map,
                                fillColor: '#1b365d',
                                fillOpacity: .4,
                                scale: 5,
                                strokeWeight: 1,
                                strokeColor: 'white',
                                radius: 800
                            });
                            circle.bindTo('center', marker, 'position');

                            var markerCluster = new MarkerClusterer(map, markers,
                                  {
                                      imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                                  });
                            google.maps.event.addListener(marker, "click", function (e) {
                                var infoWindow = new google.maps.InfoWindow();
                                infoWindow.setContent(marker.title);
                                infoWindow.open(map, marker);
                            });
                        });
                    } else {
                        alert('Geo Location feature is not supported in this browser.');
                    }
                    $('#alert').modal("show");
                    $('#alertHeader').text('Asset Location');
                    $('#alertBody').text('No location information available for this asset. The map will only show your current location');
                }

                //Create google map if location found
                var map = new google.maps.Map(document.getElementById('map'), {
                    zoom: 13,
                    minZoom: 3,
                    center: new google.maps.LatLng(latitude, longitude),
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
                var markers = new Array();
                // Add the markers and infowindows to the map       
                var markers = [];
                for (var i = 0; i < locations.length; i++) {
                    var latLng = new google.maps.LatLng(locations[i].latitude, locations[i].longitude);
                    var marker = new google.maps.Marker({ 'position': latLng });

                    if (locations[i].latitude != null) {
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
