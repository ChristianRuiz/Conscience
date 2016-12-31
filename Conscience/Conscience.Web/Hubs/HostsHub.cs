using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Conscience.Domain;
using Microsoft.AspNet.SignalR.Hubs;

namespace Conscience.Web.Hubs
{
    [HubName("HostsHub")]
    public class HostsHub : Hub
    {
        private const string GroupHosts = "Hosts";
        private const string GroupWeb = "Web";

        private static Dictionary<string, User> Users = new Dictionary<string, User>();

        public override Task OnConnected()
        {
            RegisterCurrentUser();

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            RegisterCurrentUser();

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            if (Users.ContainsKey(Context.ConnectionId))
            {
                Clients.Group("Web").HostDisconnected(Users[Context.ConnectionId].Id);
                Users.Remove(Context.ConnectionId);
            }

            return base.OnDisconnected(stopCalled);
        }
        
        private void RegisterCurrentUser()
        {
            if (!Users.ContainsKey(Context.ConnectionId))
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "User " + (Users.Count + 1),
                    Device = new Device()
                };

                Users.Add(Context.ConnectionId, user);
            }
        }

        public void SubscribeHost()
        {
            var user = Users[Context.ConnectionId];
            Clients.Group(GroupWeb).HostConnected(user);
            Groups.Add(Context.ConnectionId, GroupHosts);
        }

        public void SubscribeWeb()
        {
            Groups.Add(Context.ConnectionId, GroupWeb);
        }

        public void LocationUpdates(List<Location> locations)
        {
            var user = Users[Context.ConnectionId];
            user.Device.Locations.AddRange(locations);

            //Hack: To avoid sending dates from javascript during the PoC
            user.Device.CurrentLocation.TimeStamp = DateTime.Now;

            Clients.Group(GroupWeb).LocationUpdated(user.Id, user.UserName, user.Device.CurrentLocation);
        }
    }
}