//Add field

var mapPage = {
    initialize: function () {
        debugger;
        mapPage.attachEvents();
    },
    attachEvents: function () {
        debugger;
        mapPage.getResultForMap();
        var initialLatLng;
        var initialMap;

    },
    getResultForMap: function () {
        $('#processing-modal').modal("show");
        $.ajax({
            type: "GET",
            url: '/Map/PlotMarker',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            success: function (result) {
                debugger;
                mapPage.createMap(result);
            }
        });
    },
    createMap: function (result) {
        var locations = result;
        var latitude;
        var longitude;
        if (navigator.geolocation) {
            debugger;
            navigator.geolocation.getCurrentPosition(function (p) {
                debugger;
                //swal("Warning", p.coords.latitude + "&" + p.coords.longitude);
                mapPage.initializeMap(p);
                mapPage.addLocationMarkers(p, locations);
            });
        }
        else {
            swal("Warning", "Geo Location feature is not supported in this browser.");
        }
        $('#processing-modal').modal("hide");
    },
    initializeMap: function (p) {
        initialLatLng = new google.maps.LatLng(p.coords.latitude, p.coords.longitude);
        debugger;
        var mapOptions = {
            center: initialLatLng,
            zoom: 14,
            minZoom: 2,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        initialMap = new google.maps.Map(document.getElementById("map"), mapOptions);
    },
    addLocationMarkers: function (p, locations) {
        var markerCurrentLocation = new google.maps.Marker({
            position: initialLatLng,
            map: initialMap,
            icon: {
                path: google.maps.SymbolPath.CIRCLE,
                fillOpacity: 10,
                fillColor: '#73b0fc',
                strokeOpacity: 5,
                strokeColor: 'white',
                strokeWeight: 5,
                scale: 15
            },
            title: "Your location:</b><br />Latitude: " + p.coords.latitude + "<br />Longitude: " + p.coords.longitude
        });

        var infowindow = new google.maps.InfoWindow({
            maxWidth: 500
        });
        //create empty LatLngBounds object       
        var bounds = new google.maps.LatLngBounds();
        var markers = new Array();
        // Add the markers and infowindows to the map       
        var markers = [];
        markers.push(markerCurrentLocation);
        for (var i = 0; i < locations.length; i++) {
            debugger;
            var latLngLocations = new google.maps.LatLng(locations[i].latitude, locations[i].longitude);

            var lngVal = /^-?((1?[0-7]?|[0-9]?)[0-9]|180)\.[0-9]{1,6}$/;
            var previousPageValue = $('#previousPageValue').val();
            var latitudeCheck = $('#latitude').val();
            var longitudeCheck = $('#longitude').val();
            //Validate Location
            var isValidate = mapPage.validateLocation(latitudeCheck, longitudeCheck);
            if ((!lngVal.test(locations[i].latitude)) || (!lngVal.test(locations[i].longitude))) {
                if ((previousPageValue.toLowerCase() == "welllinereports" || previousPageValue.toLowerCase() == "welldetails") && !isValidate) {
                    swal("Asset Location", "No valid location information available for this asset. The map will only show your current location and followed wells.");
                    mapPage.setZoomLevel();
                }
                continue;
            }

            marker = new google.maps.Marker({ 'position': latLngLocations });
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
            if (locations[0] == undefined) {
                mapPage.showAlert();
            } else {
                bounds.extend(marker.position);
            }

            google.maps.event.addListener(marker, 'click', (function (marker, i) {
                return function () {
                    debugger;
                    infowindow.setContent('<b><a href="#" id="direction"><img src="/images/car-Icon.png" /></a> &nbsp; <a href="/WellLineReport/Index?wellId=' + Base64.encode(locations[i].id.toString()) + '&wellName=' + Base64.encode(locations[i].name.toString()) + '&isFollow=' + Base64.encode(locations[i].isFavorite.toString()) + '">' + locations[i].name) + '</a></b>';
                    infowindow.open(initialMap, marker);
                    $('#direction').click(function () {
                        debugger;
                        mapPage.showDirection(locations[i]);
                    });
                }
            })(marker, i));
            google.maps.event.addListener(markerCurrentLocation, 'click', (function (markerCurrentLocation) {
                return function () {
                    debugger;
                    infowindow.setContent('<b>Your Current Location</b>');
                    infowindow.open(initialMap, markerCurrentLocation);
                }
            })(markerCurrentLocation));
            // This is needed to set the zoom after fitbounds, 
            google.maps.event.addListener(initialMap, 'zoom_changed', function () {
                zoomChangeBoundsListener =
                    google.maps.event.addListener(initialMap, 'bounds_changed', function (event) {
                        if (this.getZoom() > 15 && this.initialZoom == true) {
                            // Change max/min zoom here
                            this.setZoom(20);
                            this.initialZoom = false;
                        }
                        google.maps.event.removeListener(zoomChangeBoundsListener);
                    });
            });
            initialMap.initialZoom = true;
        }
        var markerCluster = new MarkerClusterer(initialMap, markers, {
            imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
        });
        debugger;
        //now fit the map to the newly inclusive bounds
        initialMap.fitBounds(bounds);
        debugger;
        if (locations.length == 0) {
            mapPage.showAlert();
        }
        mapPage.showNullAlert();
        mapPage.showFollowedAlert(locations);

    },
    showDirection: function (location) {
        //Show directions for wells from current position.
        debugger;
        navigator.geolocation.getCurrentPosition(
       function (position) {
           //do succes handling
           mapPage.initializeMap(position);
           mapPage.getDirection(position, location);
       },
       function errorCallback(error) {
           //do error handling
           swal("Warning", "Geo Location feature is not supported in this browser.");
       },
       {
           maximumAge: Infinity,
           timeout: 5000
       });
    },
    getDirection: function (p, location) {
        var infowindow = new google.maps.InfoWindow({
            maxWidth: 500
        });
        var destinationImg;
        // Add the markers and infowindows to the map       
        if (location.isFavorite == true) {
            destinationImg = "/images/favorite-location.png";
        }
        else {
            destinationImg = "/images/location.png";
        }
        debugger;
        //show direction for well location from current location
        var directionsDisplay = new google.maps.DirectionsRenderer;
        var directionsService = new google.maps.DirectionsService;
        directionsDisplay.setMap(initialMap);
        directionsDisplay.setOptions({ suppressMarkers: true });
        directionsService.route({
            origin: { lat: p.coords.latitude, lng: p.coords.longitude },
            destination: { lat: location.latitude, lng: location.longitude },
            travelMode: google.maps.TravelMode["DRIVING"]
        }, function (response, status) {
            debugger;
            if (status == 'OK') {
                directionsDisplay.setDirections(response);
                var _route = response.routes[0].legs[0];
                pinA = new google.maps.Marker({
                    position: _route.start_location,
                    map: initialMap,
                    icon: {
                        path: google.maps.SymbolPath.CIRCLE,
                        fillOpacity: 10,
                        fillColor: '#73b0fc',
                        strokeOpacity: 5,
                        strokeColor: 'white',
                        strokeWeight: 5,
                        scale: 15
                    }
                }),
                pinB = new google.maps.Marker({
                    position: _route.end_location,
                    map: initialMap,
                    icon: destinationImg
                });
                google.maps.event.addListener(pinB, 'click', (function (pinB) {
                    return function () {
                        debugger;
                        infowindow.setContent('<b><a href="/WellLineReport/Index?wellId=' + Base64.encode(location.id.toString()) + '&wellName=' + Base64.encode(location.name.toString()) + '&isFollow=' + Base64.encode(location.isFavorite.toString()) + '">' + location.name) + '</a></b>';
                        infowindow.open(initialMap, pinB);
                    }
                })(pinB));
                google.maps.event.addListener(pinA, 'click', (function (pinA) {
                    return function () {
                        debugger;
                        infowindow.setContent('<b><a href="#" id="navigationStep">Navigation Steps</a></b> &nbsp;');
                        infowindow.open(initialMap, pinA);
                        $('#navigationStep').click(function () {
                            debugger;
                            mapPage.navigateStep(location);
                        });
                    }
                })(pinA));
                var markers = new Array();
                markers.push(pinA);
                markers.push(pinB);
                var markerCluster = new MarkerClusterer(initialMap, markers, {
                    imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                });

            } else {
                mapPage.showRouteAlert();
            }
        });

    },
    navigateStep: function (location) {
        //Show directions for wells from current position.
        debugger;
        $('#map').addClass('col-lg-8');
        $('#panel').addClass('col-lg-4 scrolable-div');
        $('#drivingDirection').removeClass("hide-div");
        $('#drivingDirection').addClass("show-div mr-100");
        $('#headingMap').addClass("ml-15");
        //Display user current location if location not found                
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(
     function (position) {
         //do succes handling
         mapPage.initializeMap(position);
         mapPage.getDrivingDirection(position, location);
     },
     function errorCallback(error) {
         //do error handling
         swal("Warning", "Geo Location feature is not supported in this browser.");
     },
     {
         maximumAge: Infinity,
         timeout: 5000
     });
        } else {
            swal("Warning", "Geo Location feature is not supported in this browser.");
        }
    },
    getDrivingDirection: function (p, location) {
        var infowindow = new google.maps.InfoWindow({
            maxWidth: 500
        });
        debugger;
        //Add the markers and infowindows to the map       
        var destinationImg;
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
        directionsDisplay.setMap(initialMap);
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
                    map: initialMap,
                    icon: {
                        path: google.maps.SymbolPath.CIRCLE,
                        fillOpacity: 10,
                        fillColor: '#73b0fc',
                        strokeOpacity: 5,
                        strokeColor: 'white',
                        strokeWeight: 5,
                        scale: 15
                    }
                }),
                pinB = new google.maps.Marker({
                    position: _route.end_location,
                    map: initialMap,
                    icon: destinationImg
                });
                google.maps.event.addListener(pinA, 'click', (function (pinA) {
                    return function () {
                        debugger;
                        infowindow.setContent('<b>Your Current Location</b>');
                        infowindow.open(initialMap, pinA);
                    }
                })(pinA));
                google.maps.event.addListener(pinB, 'click', (function (pinB) {
                    return function () {
                        debugger;
                        infowindow.setContent('<b><a href="/WellLineReport/Index?wellId=' + Base64.encode(location.id.toString()) + '&wellName=' + Base64.encode(location.name.toString()) + '&isFollow=' + Base64.encode(location.isFavorite.toString()) + '">' + location.name.toString()) + '</a></b>';
                        infowindow.open(initialMap, pinB);
                    }
                })(pinB));

                var markers = new Array();
                markers.push(pinA);
                markers.push(pinB);
                var markerCluster = new MarkerClusterer(initialMap, markers, {
                    imagePath: 'https://developers.google.com/maps/documentation/javascript/examples/markerclusterer/m'
                });
            } else {
                swal("Warning", 'Directions request failed due to ' + status);
            }
        });
    },
    showAlert: function () {
        //if location not found then show alert popup
        swal("Asset Location", "No location information available for this asset. The map will only show your current location and followed wells.");
        mapPage.setZoomLevel();
    },
    showRouteAlert: function () {
        //if location not found then show alert popup
        swal("Route Information", "Route is not available for your destination from your current location.");
        mapPage.setZoomLevel();
    },
    showNullAlert: function () {
        debugger;
        var latitudeCheck = $('#latitude').val();
        var longitudeCheck = $('#longitude').val();
        //Validate Location
        var isValidate = mapPage.validateLocation(latitudeCheck, longitudeCheck);
        if (latitudeCheck == "null" || longitudeCheck == "null") {
            mapPage.showAlert();
        } else if (isValidate && (latitudeCheck != "All" || longitudeCheck != "All") && (latitudeCheck != "null" || longitudeCheck != "null")) {
            var bounds = new google.maps.LatLngBounds();
            var latLngFocused = new google.maps.LatLng(latitudeCheck, longitudeCheck);
            var markerFocus = new google.maps.Marker({ 'position': latLngFocused });
            bounds.extend(markerFocus.position);
            initialMap.fitBounds(bounds);
        }
    },
    showFollowedAlert: function (locations) {
        var myWellCheck = $('#myWellCheck').val();
        if (myWellCheck == "true" && locations.length == 0) {
            swal("Asset Location", "You are not following any wells yet. No location information available for this asset. The map will only show you current location.");
            mapPage.setZoomLevel();
        }
    },
    setZoomLevel: function () {
        // This is needed to set the zoom after fitbounds, 
        google.maps.event.addListener(initialMap, 'zoom_changed', function () {
            zoomChangeBoundsListener =
                google.maps.event.addListener(initialMap, 'bounds_changed', function (event) {
                    if (this.getZoom() > 5 && this.initialZoom == true) {
                        // Change max/min zoom here
                        this.setZoom(1);
                        this.initialZoom = false;
                    }
                    google.maps.event.removeListener(zoomChangeBoundsListener);
                });
        });
        initialMap.initialZoom = true;
    },
    validateLocation: function (latitude, longitude) {
        debugger;
        var lngVal = /^-?((1?[0-7]?|[0-9]?)[0-9]|180)\.[0-9]{1,6}$/;
        if ((!lngVal.test(latitude)) || (!lngVal.test(longitude))) {
            return false;
        }
        return true;
    }

}
