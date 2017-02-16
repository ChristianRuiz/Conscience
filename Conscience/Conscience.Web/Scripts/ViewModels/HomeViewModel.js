function HomeViewModel(mapId) {
    var self = this;
    var hostsHub;

    self.Users = ko.observableArray();

    self.SendNotification = function (userId) {
        hostsHub.server.sendNotification(userId);
    };

    function getUser(id) {
        return self.Users().find(function (u) { return u.Id() == id });
    }

    function getOrAddUser(id, userName) {
        var user = getUser(id);

        if (user == null) {
            user = new User(id, userName, self);
            self.Users.push(user);
        }

        return user;
    }

    $(function () {
        hostsHub = $.connection.HostsHub;

        hostsHub.client.hostConnected = function (user) {
            getOrAddUser(user.Id, user.UserName);
        };

        hostsHub.client.locationUpdated = function (userId, userName, location) {
            var user = getOrAddUser(userId, userName);
            user.Location(location);
        };

        hostsHub.client.hostDisconnected = function (userId) {
            self.Users.remove(function (u) { return u.Id() == userId });
        };

        $.connection.hub.start().done(function () {
            hostsHub.server.subscribeWeb();
        });
    });

    var options = {
        controls: [
          new OpenLayers.Control.Navigation(),
          new OpenLayers.Control.PanZoomBar(),
          new OpenLayers.Control.Attribution()
        ]
    };

    var apiKey = "Aqh7oaz-q_8iKzjPjvzPaac4jn2HAU7iPF36ftyQ9u6-34rJktZsKTO_JNJsHUKB";

    var map = new OpenLayers.Map(mapId, options);
    //map.addLayer(new OpenLayers.Layer.OSM());
    map.addLayer(new OpenLayers.Layer.Bing({
        key: apiKey,
        type: "Aerial"
    }));
    var markers = new OpenLayers.Layer.Markers("Markers");
    var userMarkers = {};
    map.addLayer(markers);
    map.zoomToMaxExtent();

    var mapZoomed = false;

    self.Users.subscribe(function (changes) {

        changes.forEach(function (change) {
            if (change.status === 'added') {
                var user = change.value;

                if (user.Location().Longitude === '') {

                    user.Location.subscribe(function () {
                        var marker = userMarkers[user.Id()];

                        if (marker != null) {
                            markers.removeMarker(marker);
                        }

                        var position = new OpenLayers.LonLat(user.Location().Longitude, user.Location().Latitude).transform(
                                            new OpenLayers.Projection("EPSG:4326"), // transform from WGS 1984
                                            map.getProjectionObject() // to Spherical Mercator Projection
                                          );

                        marker = new OpenLayers.Marker(position);
                        markers.addMarker(marker);

                        userMarkers[user.Id()] = marker;

                        if (!mapZoomed) {
                            map.setCenter(position, 6);
                            mapZoomed = true;
                        }
                    });
                }
            } else if (change.status === 'deleted') {
                var user = change.value;

                var marker = userMarkers[user.Id()];

                if (marker != null) {
                    markers.removeMarker(marker);
                }
            }
        });

    }, null, "arrayChange");
}