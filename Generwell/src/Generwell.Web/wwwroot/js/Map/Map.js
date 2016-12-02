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
            async: true,
            success: function (result) {
                debugger;
                var currentUserLatLong;
                var locations = result;
                var latitude;
                var longitude;
                //set latitude and longitude
                if (locations[0] == undefined) {
                    latitude = 56.1304;
                    longitude = 106.3468;

                    //if location not found then show alert popup
                    $('#alert').modal("show");
                    $('#alertHeader').text('Asset Location');
                    $('#alertBody').text('No location information available for this asset. The map will only show your current location');

                } else {
                    latitude = locations[0].latitude;
                    longitude = locations[0].longitude;
                }

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
                        //create empty LatLngBounds object
                        var bounds = new google.maps.LatLngBounds();

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
                            //extend the bounds to include each marker's position
                            bounds.extend(marker.position);
                            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                                return function () {
                                    debugger;
                                    infowindow.setContent('<a href="#" id="direction"><img src="/images/car-Icon.png" /></a> &nbsp; <a href="/WellLineReport/Index?wellId=' + locations[i].id + '&wellName=' + locations[i].name + '&isFollow=' + locations[i].isFavorite + '">' + locations[i].name) + '</a>';
                                    infowindow.open(map, marker);
                                    $('#direction').click(function () {
                                        debugger;
                                        mapPage.showDirection(locations[i]);
                                    });
                                }
                            })(marker, i));
                        }

                        //now fit the map to the newly inclusive bounds
                        map.fitBounds(bounds);
                        var markerCluster = new MarkerClusterer(map, markers,
                                            {
                                                imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                                            });

                    });
                } else {
                    alert('Geo Location feature is not supported in this browser.');
                }

                $('#processing-modal').modal("hide");
            }
        });
    },

    showDirection: function (location) {
        //Show directions for wells from current position.
        //function calculateAndDisplayRoute(latitude, longitude) {
        debugger;
        //location.latitude = 18.5176;
        //location.longitude = 73.8417;
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

                debugger;
                //Add the markers and infowindows to the map       
                var destinationImg;
                // Add the markers and infowindows to the map       
                if (location.isFavorite == true) {
                    destinationImg = "/images/favorite-location.png";
                }
                else {
                    destinationImg = "/images/location.png";
                }
                debugger;
                //show direction
                var directionsDisplay = new google.maps.DirectionsRenderer;
                var directionsService = new google.maps.DirectionsService;

                directionsDisplay.setMap(map);
                directionsDisplay.setOptions({ suppressMarkers: true });
                directionsService.route({
                    origin: { lat: p.coords.latitude, lng: p.coords.longitude },  // Haight.
                    destination: { lat: location.latitude, lng: location.longitude },  // Ocean Beach.
                    travelMode: google.maps.TravelMode["DRIVING"]
                }, function (response, status) {
                    debugger;
                    if (status == 'OK') {
                        directionsDisplay.setDirections(response);
                        var _route = response.routes[0].legs[0];
                        pinA = new google.maps.Marker({
                            position: _route.start_location,
                            map: map,
                            icon: im
                        }),
                        pinB = new google.maps.Marker({
                            position: _route.end_location,
                            map: map,
                            icon: destinationImg
                        });
                        google.maps.event.addListener(pinB, 'click', (function (pinB) {
                            return function () {
                                debugger;
                                infowindow.setContent('<a href="/WellLineReport/Index?wellId=' + location.id + '&wellName=' + location.name + '&isFollow=' + location.isFavorite + '">' + location.name) + '</a>';
                                infowindow.open(map, pinB);                               
                            }
                        })(pinB));
                        google.maps.event.addListener(pinA, 'click', (function (pinA) {
                            return function () {
                                debugger;
                                infowindow.setContent('<a href="#" id="navigation">Start Navigation</a> &nbsp; <a href="#" id="navigationStep">Navigation Steps</a> &nbsp;');
                                infowindow.open(map, pinA);
                                $('#navigation').click(function () {
                                    debugger;
                                    mapPage.navigate(location);
                                });
                                $('#navigationStep').click(function () {
                                    debugger;
                                    mapPage.navigateStep(location);
                                });
                            }
                        })(pinA));
                    } else {
                        window.alert('Directions request failed due to ' + status);
                    }
                });
            });
        } else {
            alert('Geo Location feature is not supported in this browser.');
        }
    },

    navigate: function (location) {
        //Show directions for wells from current position.
        //function calculateAndDisplayRoute(latitude, longitude) {
        debugger;
        //location.latitude = 18.5176;
        //location.longitude = 73.8417;
        //Display user current location if location not found          
        if (navigator.geolocation) {
            var im = 'https://www.robotwoods.com/dev/misc/bluecircle.png';

            navigator.geolocation.getCurrentPosition(function (p) {
                debugger;
                var LatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
                debugger;
                var mapOptions = {
                    center: LatLng,
                    zoom: 14,
                    minZoom: 14,
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

                debugger;
                //Add the markers and infowindows to the map       
                var destinationImg;

                // Add the markers and infowindows to the map       
                if (location.isFavorite == true) {
                    destinationImg = "/images/favorite-location.png";
                }
                else {
                    destinationImg = "/images/location.png";
                }
                //var markers = [];
                debugger;
                //show direction
                var directionsDisplay = new google.maps.DirectionsRenderer;
                var directionsService = new google.maps.DirectionsService;

                directionsDisplay.setMap(map);
                directionsDisplay.setOptions({ suppressMarkers: true });
                directionsService.route({
                    origin: { lat: p.coords.latitude, lng: p.coords.longitude },  // Haight.
                    destination: { lat: location.latitude, lng: location.longitude },  // Ocean Beach.
                    travelMode: google.maps.TravelMode["DRIVING"]
                }, function (response, status) {
                    debugger;
                    if (status == 'OK') {
                        directionsDisplay.setDirections(response);
                        var _route = response.routes[0].legs[0];
                        pinA = new google.maps.Marker({
                            position: _route.start_location,
                            map: map,
                            icon: im
                        }),
                        pinB = new google.maps.Marker({
                            position: _route.end_location,
                            map: map,
                            icon: destinationImg
                        });
                        google.maps.event.addListener(pinB, 'click', (function (pinB) {
                            return function () {
                                debugger;
                                infowindow.setContent('<a href="/WellLineReport/Index?wellId=' + location.id + '&wellName=' + location.name + '&isFollow=' + location.isFavorite + '">' + location.name) + '</a>';
                                infowindow.open(map, pinB);
                                $('#direction').click(function () {
                                    debugger;
                                    mapPage.showDirection(locations[i]);
                                });
                            }
                        })(pinB));

                        if (pinA) {
                            // Marker already created - Move it
                            pinA.setPosition(newPoint);
                        }
                        else {
                            // Marker does not exist - Create it
                            pinA = new google.maps.Marker({
                                position: newPoint,
                                map: map
                            });
                        }
                        map.setCenter(newPoint);
                        // Call the autoUpdate() function every 5 seconds
                        setTimeout(mapPage.navigate(location), 5000);
                    } else {
                        window.alert('Directions request failed due to ' + status);
                    }
                });
            });

        } else {
            alert('Geo Location feature is not supported in this browser.');
        }
    },

    navigateStep: function (location) {
        //Show directions for wells from current position.
        //function calculateAndDisplayRoute(latitude, longitude) {
        debugger;

        $('#map').addClass('col-lg-8');
        $('#panel').addClass('col-lg-4 scrolable-div');
        $('#drivingDirection').removeClass("hide-div");
        $('#drivingDirection').addClass("show-div mr-100");
        $('#headingMap').addClass("ml-15");
        //location.latitude = 18.5176;
        //location.longitude = 73.8417;
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

                debugger;
                //Add the markers and infowindows to the map       
                var destinationImg;
                // Add the markers and infowindows to the map       
                if (location.isFavorite == true) {
                    destinationImg = "/images/favorite-location.png";
                }
                else {
                    destinationImg = "/images/location.png";
                }

                debugger;
                //show direction
                var directionsDisplay = new google.maps.DirectionsRenderer;
                var directionsService = new google.maps.DirectionsService;

                directionsDisplay.setMap(map);
                directionsDisplay.setPanel(document.getElementById('panel'));
                directionsDisplay.setOptions({ suppressMarkers: true });
                directionsService.route({
                    origin: { lat: p.coords.latitude, lng: p.coords.longitude },  // Haight.
                    destination: { lat: location.latitude, lng: location.longitude },  // Ocean Beach.
                    travelMode: google.maps.TravelMode["DRIVING"]
                }, function (response, status) {
                    debugger;
                    if (status == 'OK') {
                        directionsDisplay.setDirections(response);
                        var _route = response.routes[0].legs[0];

                        pinA = new google.maps.Marker({
                            position: _route.start_location,
                            map: map,
                            icon: im
                        }),
                        pinB = new google.maps.Marker({
                            position: _route.end_location,
                            map: map,
                            icon: destinationImg
                        });
                        google.maps.event.addListener(pinA, 'click', (function (pinA) {
                            return function () {
                                debugger;
                                infowindow.setContent('<a href="#" id="navigation">Start Navigation</a>');
                                infowindow.open(map, pinA);
                                $('#navigation').click(function () {
                                    debugger;
                                    mapPage.navigate(location);
                                });
                                $('#navigationStep').click(function () {
                                    debugger;
                                    mapPage.navigateStep(location);
                                });
                            }
                        })(pinA));
                        google.maps.event.addListener(pinB, 'click', (function (pinB) {
                            return function () {
                                debugger;
                                infowindow.setContent('<a href="/WellLineReport/Index?wellId=' + location.id + '&wellName=' + location.name + '&isFollow=' + location.isFavorite + '">' + location.name) + '</a>';
                                infowindow.open(map, pinB);
                            }
                        })(pinB));
                    } else {
                        window.alert('Directions request failed due to ' + status);
                    }
                });
            });
        } else {
            alert('Geo Location feature is not supported in this browser.');
        }
    }
  
}
