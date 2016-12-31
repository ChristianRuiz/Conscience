function HomeViewModel() {
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
}