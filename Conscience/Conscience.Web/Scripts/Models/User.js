function User(id, name, viewModel) {
    var self = this;

    self.Id = ko.observable(id);
    self.UserName = ko.observable(name);
    self.Location = ko.observable({ Latitude: '', Longitude: '', TimeStamp: '' });

    self.SendNotification = function() {
        viewModel.SendNotification(self.Id());
    };
}