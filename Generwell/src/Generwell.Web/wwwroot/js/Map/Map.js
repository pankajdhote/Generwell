//Add field

var mapPage = {

    initialize: function () {
        debugger;
        mapPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        $('#processing-modal').modal("show");

        $.ajax({
            type: "GET",
            url: '/Map/PlotMarker',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (result) {
                debugger;
                //var directionsDisplay;
                //var directionsService;
                var currentUserLatLong;
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

                //if location not found then show alert popup
                if (locations[0] == undefined) {
                    $('#alert').modal("show");
                    $('#alertHeader').text('Asset Location');
                    $('#alertBody').text('No location information available for this asset. The map will only show your current location');
                }

                //Display user current location if location not found                
                if (navigator.geolocation) {
                    debugger;
                    var im = 'https://www.robotwoods.com/dev/misc/bluecircle.png';

                    navigator.geolocation.getCurrentPosition(function (p) {
                        debugger;
                        var LatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);

                        currentUserLatLong = LatLng;
                        var mapOptions = {
                            center: LatLng,
                            zoom: 14,
                            minZoom: 2,
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

                //if location found then dispaly google map
                if (locations[0] != undefined) {
                    //Create google map if location found
                    var map = new google.maps.Map(document.getElementById('map'), {
                        zoom: 13,
                        minZoom: 2,
                        center: new google.maps.LatLng(latitude, longitude),
                        mapTypeId: google.maps.MapTypeId.satellite
                    });
                    var infowindow = new google.maps.InfoWindow({
                        maxWidth: 500
                    });

                    var markers = new Array();
                    // Add the markers and infowindows to the map       
                    var markers = [];
                    for (var i = 0; i < locations.length; i++) {
                        debugger;
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
                        var location = locations[i];
                        markers.push(marker);
                        google.maps.event.addListener(marker, 'click', (function (marker, i) {
                            return function () {
                                debugger;
                                infowindow.setContent('<img id="direction" src="/images/car-Icon.png" >&nbsp; <a href="/WellLineReport/Index?wellId=' + locations[i].id + '&wellName=' + locations[i].name + '&isFollow=' + locations[i].isFavorite + '">' + locations[i].name) + '</a>';
                                infowindow.open(map, marker);
                                $('#direction').click(function () {
                                    debugger;
                                    mapPage.showDirection(location);
                                });
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
                $('#processing-modal').modal("hide");
            }
        });
    },

    showDirection: function (location) {
        //Show directions for wells from current position.
        //function calculateAndDisplayRoute(latitude, longitude) {
        debugger;
        location.latitude = "18.5176";
        location.longitude = "73.8417";
        //Display user current location if location not found                
        if (navigator.geolocation) {
            var im = 'https://www.robotwoods.com/dev/misc/bluecircle.png';

            navigator.geolocation.getCurrentPosition(function (p) {
                debugger;
                var LatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
                debugger;
                currentUserLatLong = LatLng;
                var mapOptions = {
                    center: LatLng,
                    zoom: 14,
                    minZoom: 2,
                    mapTypeId: google.maps.MapTypeId.ROADMAP
                };

                var map = new google.maps.Map(document.getElementById("map"), mapOptions);
                var marker = new google.maps.Marker({
                    position: LatLng,
                    map: map,
                    icon: im,
                    title: "Your location:</b><br />Latitude: " + p.coords.latitude + "<br />Longitude: " + p.coords.longitude
                });

                var infowindow = new google.maps.InfoWindow({
                    maxWidth: 500
                });

                debugger;
                //show direction
                var directionsDisplay = new google.maps.DirectionsRenderer;
                var directionsService = new google.maps.DirectionsService;

                directionsDisplay.setMap(map);
                //var selectedMode = document.getElementById('DestinationList').value;
                directionsService.route({
                    origin: { lat: p.coords.latitude, lng: p.coords.longitude },  // Haight.
                    destination: { lat: location.latitude, lng: location.longitude },  // Ocean Beach.
                    // Note that Javascript allows us to access the constant
                    // using square brackets and a string value as its
                    // "property."
                    travelMode: google.maps.TravelMode["DRIVING"]
                }, function (response, status) {
                    debugger;
                    if (status == 'OK') {
                        directionsDisplay.setDirections(response);
                    } else {
                        window.alert('Directions request failed due to ' + status);
                    }
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


                // Add the markers and infowindows to the map       
                var markers = [];
                // Add the markers and infowindows to the map       
                if (isFavorite == true) {
                    marker.setIcon('/images/favorite-location.png');
                }
                else {
                    marker.setIcon('/images/location.png');
                }
                markers.push(marker);

                google.maps.event.addListener(marker, 'click', (function (marker) {
                    return function () {
                        debugger;
                        infowindow.setContent('<img src="/images/car-Icon.png"/> &nbsp; <a href="/WellLineReport/Index?wellId=' + location.id + '&wellName=' + location.name + '&isFollow=' + location.isFavorite + '">' + location.name) + '</a>';
                        infowindow.open(map, marker);
                    }
                })(marker));
                var markerCluster = new MarkerClusterer(map, markers,
                     {
                         imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                     });
            });
        } else {
            alert('Geo Location feature is not supported in this browser.');
        }
    }
}
